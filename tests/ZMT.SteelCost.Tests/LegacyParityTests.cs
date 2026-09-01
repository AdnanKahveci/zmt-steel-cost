using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Tests;

public sealed class LegacyParityTests
{
    private readonly CalculationEngine _engine = new(new RoofCalculationService());

    [Fact]
    public void All_186_material_rows_match_golden_master()
    {
        using var baseline = LegacyBaselineData.Load();
        var result = CalculateAllGroups(0.25m);
        var actual = result.Categories.SelectMany(item => item.Lines).ToDictionary(item => item.MaterialCode);

        Assert.Equal(186, actual.Count);
        Assert.Equal(186, baseline.Materials.GetArrayLength());

        foreach (var expected in baseline.Materials.EnumerateArray())
        {
            var code = expected.GetProperty("code").GetString()!;
            var line = actual[code];
            Close(LegacyBaselineData.Decimal(expected, "expectedQuantity"), line.CalculatedQuantity, 0.000001m, $"{code} miktar");
            Close(LegacyBaselineData.Decimal(expected, "expectedSalesUnitPrice"), line.SalesUnitPrice, 0.01m, $"{code} satış birim");
            Close(LegacyBaselineData.Decimal(expected, "expectedSalesLineTotal"), line.SalesTotal, 0.01m, $"{code} satış toplam");
            Close(LegacyBaselineData.Decimal(expected, "expectedPurchaseUnitPriceExVat"), line.PurchaseUnitPriceExVat, 0.01m, $"{code} alış birim");
            Close(LegacyBaselineData.Decimal(expected, "expectedPurchaseLineTotalExVat"), line.PurchaseTotalExVat, 0.01m, $"{code} alış toplam");
            Close(LegacyBaselineData.Decimal(expected, "expectedPurchaseUnitPriceIncVat"), line.PurchaseUnitPriceIncVat, 0.01m, $"{code} KDV dahil alış");
        }
    }

    [Fact]
    public void Ten_category_totals_match_golden_master()
    {
        using var baseline = LegacyBaselineData.Load();
        var result = CalculateAllGroups(0.25m);
        var actual = result.Categories.ToDictionary(item => item.CategoryId);

        Assert.Equal(10, actual.Count);
        foreach (var expected in baseline.CategoryTotals.EnumerateArray())
        {
            var categoryId = expected.GetProperty("categoryId").GetInt32();
            Close(LegacyBaselineData.Decimal(expected, "expectedSalesTotal"), actual[categoryId].CalculatedTotal, 0.01m, $"{categoryId} grup toplamı");
        }
    }

    [Fact]
    public void Offer_page_full_total_and_25_percent_discount_match()
    {
        using var baseline = LegacyBaselineData.Load();
        var result = CalculateAllGroups(0.25m);

        Close(LegacyBaselineData.Decimal(baseline.Totals, "fullCalculatedValue"), result.FullCalculatedValue, 0.01m, "TEKLİF tüm gruplar");
        Close(LegacyBaselineData.Decimal(baseline.Totals, "offerAfterDiscount"), result.GrandTotal, 0.01m, "TEKLİF %25 sonrası");
    }

    [Fact]
    public void Building_summary_scope_and_21_percent_discount_match()
    {
        using var baseline = LegacyBaselineData.Load();
        var project = CreateProject();
        project.DiscountRateOverride = 0.21m;
        var result = _engine.Calculate(project);

        Close(LegacyBaselineData.Decimal(baseline.Totals, "supplierScopeValue"), result.SupplierScopeValue, 0.01m, "ZMT kapsamı");
        Close(LegacyBaselineData.Decimal(baseline.Totals, "buildingSummaryDiscountAmount"), result.DiscountAmount, 0.01m, "Bina özeti iskonto");
        Close(LegacyBaselineData.Decimal(baseline.Totals, "buildingSummaryGrandTotal"), result.GrandTotal, 0.01m, "Bina özeti genel toplam");
    }

    private CalculationResult CalculateAllGroups(decimal discountRate)
    {
        var project = CreateProject();
        project.DiscountRateOverride = discountRate;
        foreach (var category in LegacyExcelV1Rules.Materials.Select(item => item.CategoryId).Distinct())
        {
            project.CategoryScopes.Add(new(category, ResponsibilityType.Zmt));
        }
        return _engine.Calculate(project);
    }

    private static Project CreateProject() => new()
    {
        Building = BuildingInput.CreateLegacySample(),
        PricingSnapshot = new PricingParameters
        {
            ExchangeRate = 48.1m,
            SteelPrice = 1.3m,
            SSeriesPrice = 1.35m,
            GalvanizedPrice = 0.90m,
            PaintedSheetPrice = 1.05m,
            SalesMarkupFactor = 1.73m,
            PurchaseVatRate = 0.20m,
            SalesVatRate = 0m,
            DiscountRate = 0.21m
        }
    };

    private static void Close(decimal expected, decimal actual, decimal tolerance, string label) =>
        Assert.True(Math.Abs(expected - actual) <= tolerance,
            $"{label}: beklenen {expected}, gerçek {actual}, fark {Math.Abs(expected - actual)}");
}
