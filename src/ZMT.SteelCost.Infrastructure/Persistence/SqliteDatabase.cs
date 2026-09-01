using Microsoft.Data.Sqlite;
using System.Text.Json;
using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Application.Logging;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Infrastructure.Persistence;

public sealed class SqliteDatabase
{
    private readonly IAppLogger _logger;
    private readonly string _databasePath;

    public SqliteDatabase(IAppLogger logger) : this(logger, AppPaths.DatabasePath)
    {
    }

    public SqliteDatabase(IAppLogger logger, string databasePath)
    {
        _logger = logger;
        _databasePath = databasePath;
    }

    private const string Schema = """
        PRAGMA foreign_keys = ON;

        CREATE TABLE IF NOT EXISTS Projects (
            Id TEXT PRIMARY KEY,
            Company TEXT NOT NULL,
            CustomerName TEXT NOT NULL,
            CrmNumber TEXT NOT NULL,
            Stage INTEGER NOT NULL,
            DocumentDate TEXT NOT NULL,
            PriceListVersionId TEXT NULL,
            FormulaVersion TEXT NOT NULL,
            ProjectJson TEXT NOT NULL,
            CreatedAt TEXT NOT NULL,
            UpdatedAt TEXT NOT NULL
        );
        CREATE TABLE IF NOT EXISTS ProjectBuildingInputs (
            ProjectId TEXT PRIMARY KEY,
            InputJson TEXT NOT NULL,
            FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
        );
        CREATE TABLE IF NOT EXISTS ProjectSurfaceLayers (
            ProjectId TEXT NOT NULL,
            SurfaceType INTEGER NOT NULL,
            SortOrder INTEGER NOT NULL,
            LayerType INTEGER NOT NULL,
            PRIMARY KEY(ProjectId, SurfaceType, SortOrder),
            FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
        );
        CREATE TABLE IF NOT EXISTS ProjectDoors (
            ProjectId TEXT NOT NULL,
            DoorType INTEGER NOT NULL,
            Quantity INTEGER NOT NULL CHECK(Quantity >= 0),
            PRIMARY KEY(ProjectId, DoorType),
            FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
        );
        CREATE TABLE IF NOT EXISTS ProjectWindows (
            ProjectId TEXT NOT NULL,
            WindowType INTEGER NOT NULL,
            Quantity INTEGER NOT NULL CHECK(Quantity >= 0),
            Color INTEGER NOT NULL,
            PRIMARY KEY(ProjectId, WindowType, Color),
            FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
        );
        CREATE TABLE IF NOT EXISTS ProjectFixtures (
            ProjectId TEXT NOT NULL,
            FixtureType INTEGER NOT NULL,
            GroundFloorQuantity INTEGER NOT NULL CHECK(GroundFloorQuantity >= 0),
            FirstFloorQuantity INTEGER NOT NULL CHECK(FirstFloorQuantity >= 0),
            PRIMARY KEY(ProjectId, FixtureType),
            FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
        );
        CREATE TABLE IF NOT EXISTS MaterialCategories (
            Id INTEGER PRIMARY KEY,
            Code TEXT NOT NULL UNIQUE,
            Name TEXT NOT NULL,
            SortOrder INTEGER NOT NULL
        );
        CREATE TABLE IF NOT EXISTS Materials (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Code TEXT NOT NULL UNIQUE,
            CategoryId INTEGER NOT NULL,
            Name TEXT NOT NULL,
            Specification TEXT NULL,
            Unit TEXT NOT NULL,
            BasePurchasePrice TEXT NOT NULL,
            QuantityRuleId TEXT NOT NULL,
            PricingRuleId TEXT NOT NULL,
            IsActive INTEGER NOT NULL,
            AllowManualQuantityOverride INTEGER NOT NULL,
            AllowManualPriceOverride INTEGER NOT NULL,
            FOREIGN KEY(CategoryId) REFERENCES MaterialCategories(Id)
        );
        CREATE TABLE IF NOT EXISTS MaterialFormulaParameters (
            Id TEXT PRIMARY KEY,
            MaterialCode TEXT NOT NULL,
            Name TEXT NOT NULL,
            Value TEXT NOT NULL,
            FormulaVersion TEXT NOT NULL,
            FOREIGN KEY(MaterialCode) REFERENCES Materials(Code)
        );
        CREATE TABLE IF NOT EXISTS PriceLists (
            Id TEXT PRIMARY KEY,
            Name TEXT NOT NULL,
            IsActive INTEGER NOT NULL,
            CreatedAt TEXT NOT NULL
        );
        CREATE TABLE IF NOT EXISTS PriceListVersions (
            Id TEXT PRIMARY KEY,
            PriceListId TEXT NOT NULL,
            VersionNumber INTEGER NOT NULL,
            ParametersJson TEXT NOT NULL,
            CreatedAt TEXT NOT NULL,
            UNIQUE(PriceListId, VersionNumber),
            FOREIGN KEY(PriceListId) REFERENCES PriceLists(Id)
        );
        CREATE TABLE IF NOT EXISTS MaterialPrices (
            PriceListVersionId TEXT NOT NULL,
            MaterialCode TEXT NOT NULL,
            PurchasePrice TEXT NOT NULL,
            PRIMARY KEY(PriceListVersionId, MaterialCode),
            FOREIGN KEY(PriceListVersionId) REFERENCES PriceListVersions(Id),
            FOREIGN KEY(MaterialCode) REFERENCES Materials(Code)
        );
        CREATE TABLE IF NOT EXISTS CalculationRuns (
            Id TEXT PRIMARY KEY,
            ProjectId TEXT NOT NULL,
            FormulaVersion TEXT NOT NULL,
            PriceListVersionId TEXT NULL,
            CalculatedAt TEXT NOT NULL,
            InputSnapshotJson TEXT NOT NULL,
            PricingSnapshotJson TEXT NOT NULL,
            ResultSnapshotJson TEXT NOT NULL,
            FullCalculatedValue TEXT NOT NULL,
            SupplierScopeValue TEXT NOT NULL,
            CustomerScopeValue TEXT NOT NULL,
            GrandTotal TEXT NOT NULL,
            FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
        );
        CREATE TABLE IF NOT EXISTS CalculationLines (
            CalculationRunId TEXT NOT NULL,
            MaterialCode TEXT NOT NULL,
            CategoryId INTEGER NOT NULL,
            CalculatedQuantity TEXT NOT NULL,
            EffectiveQuantity TEXT NOT NULL,
            QuantityMode INTEGER NOT NULL,
            OverrideReason TEXT NULL,
            PurchaseUnitPrice TEXT NOT NULL,
            SalesUnitPrice TEXT NOT NULL,
            SalesTotal TEXT NOT NULL,
            TraceJson TEXT NOT NULL,
            PRIMARY KEY(CalculationRunId, MaterialCode),
            FOREIGN KEY(CalculationRunId) REFERENCES CalculationRuns(Id) ON DELETE CASCADE
        );
        CREATE TABLE IF NOT EXISTS ProjectCategoryScopes (
            ProjectId TEXT NOT NULL,
            CategoryId INTEGER NOT NULL,
            Responsibility INTEGER NOT NULL,
            PRIMARY KEY(ProjectId, CategoryId),
            FOREIGN KEY(ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
        );
        CREATE TABLE IF NOT EXISTS AppSettings (
            Key TEXT PRIMARY KEY,
            Value TEXT NOT NULL,
            UpdatedAt TEXT NOT NULL
        );
        CREATE TABLE IF NOT EXISTS LookupValues (
            LookupType TEXT NOT NULL,
            Code TEXT NOT NULL,
            DisplayName TEXT NOT NULL,
            SortOrder INTEGER NOT NULL,
            IsActive INTEGER NOT NULL,
            PRIMARY KEY(LookupType, Code)
        );
        """;

    public string ConnectionString => new SqliteConnectionStringBuilder
    {
        DataSource = _databasePath,
        Mode = SqliteOpenMode.ReadWriteCreate,
        Cache = SqliteCacheMode.Shared
    }.ToString();

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_databasePath)!);
        var existingDatabase = File.Exists(_databasePath);
        await using var connection = new SqliteConnection(ConnectionString);
        await connection.OpenAsync(cancellationToken);
        if (existingDatabase)
        {
            CreateDailyBackup(connection);
        }
        await using (var command = connection.CreateCommand())
        {
            command.CommandText = Schema;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        await SeedAsync(connection, cancellationToken);
        _logger.Information("DatabaseInitialized", "SQLite veritabanı hazırlandı.", new { DatabasePath = _databasePath });
    }

    private void CreateDailyBackup(SqliteConnection source)
    {
        var backupDirectory = Path.Combine(Path.GetDirectoryName(_databasePath)!, "Backups");
        Directory.CreateDirectory(backupDirectory);
        var backupPath = Path.Combine(backupDirectory, $"steelcost-{DateTime.Today:yyyyMMdd}.db");
        if (File.Exists(backupPath))
        {
            return;
        }

        using var destination = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = backupPath,
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString());
        destination.Open();
        source.BackupDatabase(destination);
        _logger.Information("DatabaseBackupCreated", "Günlük SQLite yedeği oluşturuldu.", new { BackupPath = backupPath });
    }

    private static async Task SeedAsync(SqliteConnection connection, CancellationToken cancellationToken)
    {
        const string priceListId = "8f01c455-2d91-4eea-a24e-63e6f4e84c01";
        const string priceListVersionId = "bf741a70-44bb-47d1-bf19-a83c5a42d101";
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        var categories = LegacyExcelV1Rules.Materials
            .GroupBy(item => new { item.CategoryId, item.CategoryName })
            .OrderBy(item => item.Key.CategoryId);
        foreach (var category in categories)
        {
            await ExecuteAsync(connection, transaction,
                "INSERT OR IGNORE INTO MaterialCategories(Id, Code, Name, SortOrder) VALUES($id, $code, $name, $sort)",
                cancellationToken,
                ("$id", category.Key.CategoryId), ("$code", category.Key.CategoryId.ToString()),
                ("$name", category.Key.CategoryName), ("$sort", category.Key.CategoryId - 1000));
        }

        var pricing = new PricingParameters();
        var context = new LegacyRuleContext(BuildingInput.CreateLegacySample(), pricing, new RoofCalculationService());
        foreach (var material in LegacyExcelV1Rules.Materials)
        {
            await ExecuteAsync(connection, transaction,
                """
                INSERT OR IGNORE INTO Materials(
                    Code, CategoryId, Name, Specification, Unit, BasePurchasePrice,
                    QuantityRuleId, PricingRuleId, IsActive, AllowManualQuantityOverride, AllowManualPriceOverride)
                VALUES($code, $category, $name, $specification, $unit, $price, $quantityRule, $pricingRule, 1, 1, 1)
                """, cancellationToken,
                ("$code", material.Code), ("$category", material.CategoryId), ("$name", material.Name),
                ("$specification", material.Specification), ("$unit", material.Unit),
                ("$price", context.PurchaseUnitPriceExVat(material.Code).ToString(System.Globalization.CultureInfo.InvariantCulture)),
                ("$quantityRule", material.QuantityRuleId), ("$pricingRule", material.PricingRuleId));
        }

        var seededAt = DateTime.UtcNow.ToString("O");
        await ExecuteAsync(connection, transaction,
            "INSERT OR IGNORE INTO PriceLists(Id, Name, IsActive, CreatedAt) VALUES($id, $name, 1, $created)",
            cancellationToken, ("$id", priceListId), ("$name", "Legacy Excel Varsayılan"), ("$created", seededAt));
        await ExecuteAsync(connection, transaction,
            "INSERT OR IGNORE INTO PriceListVersions(Id, PriceListId, VersionNumber, ParametersJson, CreatedAt) VALUES($id, $list, 1, $parameters, $created)",
            cancellationToken, ("$id", priceListVersionId), ("$list", priceListId),
            ("$parameters", JsonSerializer.Serialize(pricing)), ("$created", seededAt));
        foreach (var material in LegacyExcelV1Rules.Materials)
        {
            await ExecuteAsync(connection, transaction,
                "INSERT OR IGNORE INTO MaterialPrices(PriceListVersionId, MaterialCode, PurchasePrice) VALUES($version, $material, $price)",
                cancellationToken, ("$version", priceListVersionId), ("$material", material.Code),
                ("$price", context.PurchaseUnitPriceExVat(material.Code).ToString(System.Globalization.CultureInfo.InvariantCulture)));
        }

        foreach (var parameter in LegacyExcelV1Rules.FormulaParameters)
        {
            await ExecuteAsync(connection, transaction,
                "INSERT OR IGNORE INTO MaterialFormulaParameters(Id, MaterialCode, Name, Value, FormulaVersion) VALUES($id, $material, $name, $value, $version)",
                cancellationToken,
                ("$id", parameter.Id), ("$material", parameter.MaterialCode), ("$name", parameter.Name),
                ("$value", parameter.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)), ("$version", parameter.FormulaVersion));
        }

        var lookups = new (string Type, string Code, string Name, int Sort)[]
        {
            ("RoofType", "HIP", "Kırma", 1), ("RoofType", "GABLE", "Beşik", 2),
            ("RoofType", "PARAPET", "Parapet", 3), ("RoofType", "MONO_PITCH", "Tek Eğim", 4),
            ("RoofSystem", "PURLIN_OMEGA", "Aşık Omega", 1), ("RoofSystem", "PANEL", "Panel Sistem", 2),
            ("WindowColor", "WHITE", "Beyaz", 1), ("WindowColor", "ANTHRACITE", "Antrasit", 2),
            ("WindowColor", "GOLDEN_OAK", "Altınmeşe", 3),
            ("ProjectStage", "OFFER_DRAWING_READY", "Teklif Çizimi Hazır", 1),
            ("ProjectStage", "MANUFACTURING_DRAWING_READY", "İmalat Çizimi Hazır", 2),
            ("ProjectStage", "OFFER_LIST_READY", "Teklif Listesi Hazır", 3),
            ("ProjectStage", "PRODUCTION_LIST_READY", "Üretim Listesi Hazır", 4),
            ("ProjectStage", "SENT_TO_PRODUCTION", "Üretime Verildi", 5)
        };
        foreach (var lookup in lookups)
        {
            await ExecuteAsync(connection, transaction,
                "INSERT OR IGNORE INTO LookupValues(LookupType, Code, DisplayName, SortOrder, IsActive) VALUES($type, $code, $name, $sort, 1)",
                cancellationToken, ("$type", lookup.Type), ("$code", lookup.Code), ("$name", lookup.Name), ("$sort", lookup.Sort));
        }

        foreach (var setting in new[]
                 {
                     ("FormulaVersion", LegacyExcelV1Rules.FormulaVersion),
                     ("DefaultDiscountRate", "0.21"),
                     ("DefaultSalesVatRate", "0"),
                     ("DefaultPriceListVersionId", priceListVersionId),
                     ("DatabaseSchemaVersion", "1")
                 })
        {
            await ExecuteAsync(connection, transaction,
                "INSERT OR IGNORE INTO AppSettings(Key, Value, UpdatedAt) VALUES($key, $value, $updated)",
                cancellationToken, ("$key", setting.Item1), ("$value", setting.Item2), ("$updated", DateTime.UtcNow.ToString("O")));
        }
        await transaction.CommitAsync(cancellationToken);
    }

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
