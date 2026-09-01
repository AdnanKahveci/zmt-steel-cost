using Microsoft.Data.Sqlite;
using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Application.Logging;
using ZMT.SteelCost.Domain;
using ZMT.SteelCost.Infrastructure.Persistence;

namespace ZMT.SteelCost.Tests;

public sealed class PersistenceIntegrationTests
{
    [Fact]
    public async Task First_run_seeds_catalog_and_second_run_creates_daily_backup()
    {
        var directory = TemporaryDirectory();
        try
        {
            var database = new SqliteDatabase(new TestLogger(), Path.Combine(directory, "steelcost.db"));
            await database.InitializeAsync();

            Assert.Equal(10L, await ScalarAsync(database, "SELECT COUNT(*) FROM MaterialCategories"));
            Assert.Equal(186L, await ScalarAsync(database, "SELECT COUNT(*) FROM Materials"));
            Assert.Equal(1L, await ScalarAsync(database, "SELECT COUNT(*) FROM PriceListVersions"));
            Assert.Equal(186L, await ScalarAsync(database, "SELECT COUNT(*) FROM MaterialPrices"));

            await database.InitializeAsync();
            Assert.Single(Directory.GetFiles(Path.Combine(directory, "Backups"), "steelcost-*.db"));
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public async Task Price_list_versions_are_created_and_latest_version_is_loaded()
    {
        var directory = TemporaryDirectory();
        try
        {
            var logger = new TestLogger();
            var database = new SqliteDatabase(logger, Path.Combine(directory, "steelcost.db"));
            await database.InitializeAsync();
            var service = new SqlitePriceListService(database, new RoofCalculationService(), logger);
            var initial = await service.GetActiveVersionAsync();
            var pricing = initial.Parameters.Snapshot();
            pricing.ExchangeRate = 50m;

            var created = await service.CreateVersionAsync(pricing,
                new Dictionary<string, decimal> { ["1001-001"] = 123.45m });
            var latest = await service.GetActiveVersionAsync();

            Assert.Equal(initial.VersionNumber + 1, created.VersionNumber);
            Assert.Equal(created.Id, latest.Id);
            Assert.Equal(50m, latest.Parameters.ExchangeRate);
            Assert.Equal(123.45m, latest.Prices.Single(item => item.MaterialCode == "1001-001").PurchasePrice);
            Assert.Equal(186, latest.Prices.Count);
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public async Task Project_and_calculation_snapshots_can_be_saved_repeatedly()
    {
        var directory = TemporaryDirectory();
        try
        {
            var database = new SqliteDatabase(new TestLogger(), Path.Combine(directory, "steelcost.db"));
            await database.InitializeAsync();
            var repository = new SqliteProjectRepository(database);
            var project = TestProject.Create();
            project.Company = "Test Firma";
            project.CustomerName = "Test Müşteri";

            var first = TestProject.Engine.Calculate(project);
            await repository.SaveAsync(project, first);
            await repository.SaveAsync(project, first);
            var second = TestProject.Engine.Calculate(project);
            await repository.SaveAsync(project, second);

            Assert.Equal(1L, await ScalarAsync(database, "SELECT COUNT(*) FROM Projects"));
            Assert.Equal(2L, await ScalarAsync(database, "SELECT COUNT(*) FROM CalculationRuns"));
            Assert.Equal(372L, await ScalarAsync(database, "SELECT COUNT(*) FROM CalculationLines"));
            var loaded = await repository.GetAsync(project.Id);
            Assert.NotNull(loaded);
            Assert.Equal(project.PricingSnapshot.ExchangeRate, loaded.PricingSnapshot.ExchangeRate);
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            Directory.Delete(directory, recursive: true);
        }
    }

    private static async Task<long> ScalarAsync(SqliteDatabase database, string sql)
    {
        await using var connection = new SqliteConnection(database.ConnectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        return Convert.ToInt64(await command.ExecuteScalarAsync(), System.Globalization.CultureInfo.InvariantCulture);
    }

    private static string TemporaryDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), $"zmt-steelcost-db-{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        return path;
    }

    private sealed class TestLogger : IAppLogger
    {
        public void Information(string eventName, string message, object? data = null)
        {
        }

        public void Error(string eventName, Exception exception, object? data = null)
        {
        }
    }
}
