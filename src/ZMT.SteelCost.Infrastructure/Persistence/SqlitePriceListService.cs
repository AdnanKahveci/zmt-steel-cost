using System.Globalization;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Application.Logging;
using ZMT.SteelCost.Application.Pricing;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Infrastructure.Persistence;

public sealed class SqlitePriceListService(
    SqliteDatabase database,
    IRoofCalculationService roofService,
    IAppLogger logger) : IPriceListService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<PriceListVersion> GetActiveVersionAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(database.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT plv.Id, plv.PriceListId, plv.VersionNumber, plv.ParametersJson, plv.CreatedAt
            FROM PriceListVersions plv
            INNER JOIN PriceLists pl ON pl.Id = plv.PriceListId
            WHERE pl.IsActive = 1
            ORDER BY plv.VersionNumber DESC
            LIMIT 1
            """;

        Guid id;
        Guid priceListId;
        int versionNumber;
        PricingParameters parameters;
        DateTime createdAt;
        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            if (!await reader.ReadAsync(cancellationToken))
            {
                throw new InvalidOperationException("Aktif fiyat listesi sürümü bulunamadı.");
            }
            id = Guid.Parse(reader.GetString(0));
            priceListId = Guid.Parse(reader.GetString(1));
            versionNumber = reader.GetInt32(2);
            parameters = JsonSerializer.Deserialize<PricingParameters>(reader.GetString(3), JsonOptions)
                ?? throw new InvalidDataException("Fiyat parametreleri okunamadı.");
            createdAt = DateTime.Parse(reader.GetString(4), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        var prices = await ReadPricesAsync(connection, id, cancellationToken);
        return new PriceListVersion
        {
            Id = id,
            PriceListId = priceListId,
            VersionNumber = versionNumber,
            Parameters = parameters,
            CreatedAt = createdAt,
            Prices = prices
        };
    }

    public async Task<PriceListVersion> CreateVersionAsync(
        PricingParameters parameters,
        IReadOnlyDictionary<string, decimal> materialPriceOverrides,
        CancellationToken cancellationToken = default)
    {
        Validate(parameters, materialPriceOverrides);
        var snapshot = parameters.Snapshot();
        snapshot.LastUpdatedAt = DateTime.UtcNow;
        var versionId = Guid.NewGuid();

        await using var connection = new SqliteConnection(database.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        var priceListId = await ScalarStringAsync(connection, transaction,
            "SELECT Id FROM PriceLists WHERE IsActive = 1 ORDER BY CreatedAt LIMIT 1", cancellationToken)
            ?? throw new InvalidOperationException("Aktif fiyat listesi bulunamadı.");
        var nextVersion = Convert.ToInt32(await ScalarAsync(connection, transaction,
            "SELECT COALESCE(MAX(VersionNumber), 0) + 1 FROM PriceListVersions WHERE PriceListId = $list",
            cancellationToken, ("$list", priceListId)), CultureInfo.InvariantCulture);

        await ExecuteAsync(connection, transaction,
            "INSERT INTO PriceListVersions(Id, PriceListId, VersionNumber, ParametersJson, CreatedAt) VALUES($id, $list, $version, $parameters, $created)",
            cancellationToken, ("$id", versionId.ToString()), ("$list", priceListId), ("$version", nextVersion),
            ("$parameters", JsonSerializer.Serialize(snapshot, JsonOptions)), ("$created", snapshot.LastUpdatedAt.ToString("O")));

        var context = new LegacyRuleContext(BuildingInput.CreateLegacySample(), snapshot, roofService, materialPriceOverrides);
        var prices = new List<MaterialPrice>(LegacyExcelV1Rules.Materials.Count);
        foreach (var material in LegacyExcelV1Rules.Materials)
        {
            var price = context.PurchaseUnitPriceExVat(material.Code);
            prices.Add(new(material.Code, price));
            await ExecuteAsync(connection, transaction,
                "INSERT INTO MaterialPrices(PriceListVersionId, MaterialCode, PurchasePrice) VALUES($version, $material, $price)",
                cancellationToken, ("$version", versionId.ToString()), ("$material", material.Code),
                ("$price", price.ToString(CultureInfo.InvariantCulture)));
        }

        await transaction.CommitAsync(cancellationToken);
        logger.Information("PriceListVersionCreated", "Yeni fiyat listesi sürümü oluşturuldu.",
            new { VersionId = versionId, VersionNumber = nextVersion });
        return new PriceListVersion
        {
            Id = versionId,
            PriceListId = Guid.Parse(priceListId),
            VersionNumber = nextVersion,
            CreatedAt = snapshot.LastUpdatedAt,
            Parameters = snapshot,
            Prices = prices
        };
    }

    private static async Task<List<MaterialPrice>> ReadPricesAsync(
        SqliteConnection connection,
        Guid versionId,
        CancellationToken cancellationToken)
    {
        var prices = new List<MaterialPrice>();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT MaterialCode, PurchasePrice FROM MaterialPrices WHERE PriceListVersionId = $version ORDER BY MaterialCode";
        command.Parameters.AddWithValue("$version", versionId.ToString());
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            prices.Add(new(reader.GetString(0), decimal.Parse(reader.GetString(1), CultureInfo.InvariantCulture)));
        }
        return prices;
    }

    private static void Validate(PricingParameters parameters, IReadOnlyDictionary<string, decimal> materialPriceOverrides)
    {
        var prices = new[]
        {
            parameters.ExchangeRate, parameters.SteelPrice, parameters.SSeriesPrice,
            parameters.GalvanizedPrice, parameters.PaintedSheetPrice, parameters.SalesMarkupFactor
        };
        if (prices.Any(value => value < 0m) || materialPriceOverrides.Values.Any(value => value < 0m))
        {
            throw new ArgumentOutOfRangeException(nameof(parameters), "Fiyat ve katsayı değerleri negatif olamaz.");
        }
        if (parameters.PurchaseVatRate is < 0m or > 1m || parameters.SalesVatRate is < 0m or > 1m ||
            parameters.DiscountRate is < 0m or > 1m)
        {
            throw new ArgumentOutOfRangeException(nameof(parameters), "KDV ve iskonto oranları 0 ile 1 arasında olmalıdır.");
        }
    }

    private static async Task<object?> ScalarAsync(
        SqliteConnection connection,
        System.Data.Common.DbTransaction transaction,
        string sql,
        CancellationToken cancellationToken,
        params (string Name, object Value)[] parameters)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = (SqliteTransaction)transaction;
        command.CommandText = sql;
        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue(parameter.Name, parameter.Value);
        }
        return await command.ExecuteScalarAsync(cancellationToken);
    }

    private static async Task<string?> ScalarStringAsync(
        SqliteConnection connection,
        System.Data.Common.DbTransaction transaction,
        string sql,
        CancellationToken cancellationToken)
    {
        var value = await ScalarAsync(connection, transaction, sql, cancellationToken);
        return value is null or DBNull ? null : Convert.ToString(value, CultureInfo.InvariantCulture);
    }

    private static async Task ExecuteAsync(
        SqliteConnection connection,
        System.Data.Common.DbTransaction transaction,
        string sql,
        CancellationToken cancellationToken,
        params (string Name, object Value)[] parameters)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = (SqliteTransaction)transaction;
        command.CommandText = sql;
        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue(parameter.Name, parameter.Value);
        }
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
