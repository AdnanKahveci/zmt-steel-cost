using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Tests;

public sealed class ScopeDiscountVatTests
{
    [Fact]
    public void Customer_categories_are_calculated_but_excluded_from_supplier_total()
    {
        var engine = new CalculationEngine(new RoofCalculationService());
        var project = new Project { Building = BuildingInput.CreateLegacySample(), PricingSnapshot = new() };
        var result = engine.Calculate(project);

        Assert.True(result.CustomerScopeValue > 0m);
        Assert.Equal(result.FullCalculatedValue, result.SupplierScopeValue + result.CustomerScopeValue);
        Assert.All(result.Categories.Where(item => item.Responsibility == ResponsibilityType.Customer), item => Assert.Equal(0m, item.IncludedTotal));
    }

    [Fact]
    public void Discount_then_vat_order_is_explicit()
    {
        var engine = new CalculationEngine(new RoofCalculationService());
        var project = new Project
        {
            Building = BuildingInput.CreateLegacySample(),
            PricingSnapshot = new PricingParameters { DiscountRate = 0.10m, SalesVatRate = 0.20m }
        };
        var result = engine.Calculate(project);

        Assert.Equal(result.SupplierScopeValue * 0.10m, result.DiscountAmount);
        Assert.Equal(result.SupplierScopeValue - result.DiscountAmount, result.SubtotalAfterDiscount);
        Assert.Equal(result.SubtotalAfterDiscount * 0.20m, result.VatAmount);
        Assert.Equal(result.SubtotalAfterDiscount + result.VatAmount, result.GrandTotal);
    }
}
