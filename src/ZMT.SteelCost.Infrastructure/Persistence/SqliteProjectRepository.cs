using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
using ZMT.SteelCost.Application.Projects;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Infrastructure.Persistence;

public sealed class SqliteProjectRepository(SqliteDatabase database) : IProjectRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false,
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<IReadOnlyList<Project>> GetRecentAsync(int count, CancellationToken cancellationToken = default)
    {
        var projects = new List<Project>();
        await using var connection = new SqliteConnection(database.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT ProjectJson FROM Projects ORDER BY UpdatedAt DESC LIMIT $count";
        command.Parameters.AddWithValue("$count", count);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var project = JsonSerializer.Deserialize<Project>(reader.GetString(0), JsonOptions);
            if (project is not null)
            {
                projects.Add(project);
            }
        }
        return projects;
    }

    public async Task<Project?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(database.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT ProjectJson FROM Projects WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id.ToString());
        var json = await command.ExecuteScalarAsync(cancellationToken) as string;
        return json is null ? null : JsonSerializer.Deserialize<Project>(json, JsonOptions);
    }

    public async Task SaveAsync(Project project, CalculationResult? result, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(database.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        var now = DateTime.UtcNow.ToString("O");
        await ExecuteAsync(connection, transaction,
            """
            INSERT INTO Projects(Id, Company, CustomerName, CrmNumber, Stage, DocumentDate, PriceListVersionId, FormulaVersion, ProjectJson, CreatedAt, UpdatedAt)
            VALUES($id, $company, $customer, $crm, $stage, $date, $priceList, $formula, $json, $now, $now)
            ON CONFLICT(Id) DO UPDATE SET Company=$company, CustomerName=$customer, CrmNumber=$crm, Stage=$stage,
                DocumentDate=$date, PriceListVersionId=$priceList, FormulaVersion=$formula, ProjectJson=$json, UpdatedAt=$now
            """, cancellationToken,
            ("$id", project.Id.ToString()), ("$company", project.Company), ("$customer", project.CustomerName),
            ("$crm", project.CrmNumber), ("$stage", (int)project.Stage), ("$date", project.DocumentDate.ToString("O")),
            ("$priceList", project.PriceListVersionId?.ToString()), ("$formula", project.FormulaVersion),
            ("$json", JsonSerializer.Serialize(project, JsonOptions)), ("$now", now));

        foreach (var table in new[] { "ProjectBuildingInputs", "ProjectSurfaceLayers", "ProjectDoors", "ProjectWindows", "ProjectFixtures", "ProjectCategoryScopes" })
        {
            await ExecuteAsync(connection, transaction, $"DELETE FROM {table} WHERE ProjectId=$id", cancellationToken, ("$id", project.Id.ToString()));
        }

        await ExecuteAsync(connection, transaction,
            "INSERT INTO ProjectBuildingInputs(ProjectId, InputJson) VALUES($id, $json)", cancellationToken,
            ("$id", project.Id.ToString()), ("$json", JsonSerializer.Serialize(project.Building, JsonOptions)));
        foreach (var surface in project.Building.Surfaces)
        {
            for (var index = 0; index < surface.Layers.Count; index++)
            {
                await ExecuteAsync(connection, transaction,
                    "INSERT INTO ProjectSurfaceLayers(ProjectId, SurfaceType, SortOrder, LayerType) VALUES($id, $surface, $sort, $layer)",
                    cancellationToken, ("$id", project.Id.ToString()), ("$surface", (int)surface.Surface), ("$sort", index + 1), ("$layer", (int)surface.Layers[index]));
            }
        }
        foreach (var door in project.Building.Doors)
        {
            await ExecuteAsync(connection, transaction,
                "INSERT INTO ProjectDoors(ProjectId, DoorType, Quantity) VALUES($id, $type, $quantity)", cancellationToken,
                ("$id", project.Id.ToString()), ("$type", (int)door.Type), ("$quantity", door.Quantity));
        }
        foreach (var window in project.Building.Windows)
        {
            await ExecuteAsync(connection, transaction,
                "INSERT INTO ProjectWindows(ProjectId, WindowType, Quantity, Color) VALUES($id, $type, $quantity, $color)", cancellationToken,
                ("$id", project.Id.ToString()), ("$type", (int)window.Type), ("$quantity", window.Quantity), ("$color", (int)window.Color));
        }
        foreach (var fixture in project.Building.Fixtures)
        {
            await ExecuteAsync(connection, transaction,
                "INSERT INTO ProjectFixtures(ProjectId, FixtureType, GroundFloorQuantity, FirstFloorQuantity) VALUES($id, $type, $ground, $first)", cancellationToken,
                ("$id", project.Id.ToString()), ("$type", (int)fixture.Type), ("$ground", fixture.GroundFloorQuantity), ("$first", fixture.FirstFloorQuantity));
        }
        foreach (var scope in project.CategoryScopes)
        {
            await ExecuteAsync(connection, transaction,
                "INSERT INTO ProjectCategoryScopes(ProjectId, CategoryId, Responsibility) VALUES($id, $category, $responsibility)", cancellationToken,
                ("$id", project.Id.ToString()), ("$category", scope.CategoryId), ("$responsibility", (int)scope.Responsibility));
        }

        if (result is not null)
        {
            var snapshot = SnapshotFactory.Create(project, result);
            await ExecuteAsync(connection, transaction,
                "DELETE FROM CalculationLines WHERE CalculationRunId=$id", cancellationToken,
                ("$id", result.RunId.ToString()));
            await ExecuteAsync(connection, transaction,
                "DELETE FROM CalculationRuns WHERE Id=$id", cancellationToken,
                ("$id", result.RunId.ToString()));
            await ExecuteAsync(connection, transaction,
                """
                INSERT INTO CalculationRuns(Id, ProjectId, FormulaVersion, PriceListVersionId, CalculatedAt,
                    InputSnapshotJson, PricingSnapshotJson, ResultSnapshotJson, FullCalculatedValue,
                    SupplierScopeValue, CustomerScopeValue, GrandTotal)
                VALUES($id, $project, $formula, $priceList, $calculated, $input, $pricing, $result, $full, $supplier, $customer, $grand)
                """, cancellationToken,
                ("$id", result.RunId.ToString()), ("$project", project.Id.ToString()), ("$formula", result.FormulaVersion),
                ("$priceList", project.PriceListVersionId?.ToString()), ("$calculated", result.CalculatedAt.ToString("O")),
                ("$input", snapshot.InputJson), ("$pricing", snapshot.PricingJson), ("$result", snapshot.ResultJson),
                ("$full", Decimal(result.FullCalculatedValue)), ("$supplier", Decimal(result.SupplierScopeValue)),
                ("$customer", Decimal(result.CustomerScopeValue)), ("$grand", Decimal(result.GrandTotal)));
            foreach (var line in result.Categories.SelectMany(item => item.Lines))
            {
                await ExecuteAsync(connection, transaction,
                    """
                    INSERT INTO CalculationLines(CalculationRunId, MaterialCode, CategoryId, CalculatedQuantity,
                        EffectiveQuantity, QuantityMode, OverrideReason, PurchaseUnitPrice, SalesUnitPrice, SalesTotal, TraceJson)
                    VALUES($run, $material, $category, $calculated, $effective, $mode, $reason, $purchase, $sales, $total, $trace)
                    """, cancellationToken,
                    ("$run", result.RunId.ToString()), ("$material", line.MaterialCode), ("$category", line.CategoryId),
                    ("$calculated", Decimal(line.CalculatedQuantity)), ("$effective", Decimal(line.EffectiveQuantity)),
                    ("$mode", (int)line.QuantityMode), ("$reason", line.OverrideReason),
                    ("$purchase", Decimal(line.PurchaseUnitPriceExVat)), ("$sales", Decimal(line.SalesUnitPrice)),
                    ("$total", Decimal(line.SalesTotal)), ("$trace", JsonSerializer.Serialize(line.Trace, JsonOptions)));
            }
        }

        await transaction.CommitAsync(cancellationToken);
    }

    private static string Decimal(decimal value) => value.ToString(CultureInfo.InvariantCulture);

    private static async Task ExecuteAsync(
        SqliteConnection connection,
        System.Data.Common.DbTransaction transaction,
        string sql,
        CancellationToken cancellationToken,
        params (string Name, object? Value)[] parameters)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = (SqliteTransaction)transaction;
        command.CommandText = sql;
        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
        }
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
