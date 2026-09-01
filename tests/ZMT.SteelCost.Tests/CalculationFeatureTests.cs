using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Tests;

public sealed class SteelCalculationTests
{
    [Fact]
    public void Steel_weight_is_area_times_estimated_kg_per_square_meter()
    {
        var project = TestProject.Create();
        project.Building.BuildingArea = 120m;
        project.Building.EstimatedSteelKgPerM2 = 35m;

        var result = TestProject.Engine.Calculate(project);

        Assert.Equal(4_200m, result.SteelWeight);
        Assert.Equal(4_200m, TestProject.Line(result, "1001-001").CalculatedQuantity);
    }
}

public sealed class CladdingCalculationTests
{
    [Fact]
    public void Drywall_layer_selection_changes_drywall_quantity_by_stable_enum()
    {
        var project = TestProject.Create();
        var withDrywall = TestProject.Line(TestProject.Engine.Calculate(project), "1002-001").CalculatedQuantity;
        project.Building.Surfaces.Single(item => item.Surface == SurfaceType.Roof).Layers.Add(SurfaceLayerType.Drywall);

        var withAdditionalDrywall = TestProject.Line(TestProject.Engine.Calculate(project), "1002-001").CalculatedQuantity;

        Assert.True(withAdditionalDrywall > withDrywall);
    }
}

public sealed class DoorWindowTests
{
    [Fact]
    public void Door_and_window_quantities_follow_typed_selections()
    {
        var project = TestProject.Create();
        project.Building.Doors.First(item => item.Type == DoorType.Steel90X205).Quantity = 3;
        project.Building.Windows.First(item => item.Type == WindowType.Pvc80X120).Quantity = 4;

        var result = TestProject.Engine.Calculate(project);

        Assert.Equal(3m, TestProject.Line(result, "1004-001").CalculatedQuantity);
        Assert.Equal(4m, TestProject.Line(result, "1004-008").CalculatedQuantity);
    }

    [Fact]
    public void Window_color_changes_the_matching_window_price_only()
    {
        var project = TestProject.Create();
        var window80X120 = project.Building.Windows.First(item => item.Type == WindowType.Pvc80X120);
        var window140X100 = project.Building.Windows.First(item => item.Type == WindowType.Pvc140X100);
        window80X120.Color = WindowColor.White;
        window140X100.Color = WindowColor.Anthracite;

        var whiteResult = TestProject.Engine.Calculate(project);
        var whitePrice = TestProject.Line(whiteResult, "1004-008").SalesUnitPrice;
        var otherWindowPrice = TestProject.Line(whiteResult, "1004-009").SalesUnitPrice;

        window80X120.Color = WindowColor.Anthracite;
        var anthraciteResult = TestProject.Engine.Calculate(project);
        var anthracitePrice = TestProject.Line(anthraciteResult, "1004-008").SalesUnitPrice;

        Assert.Equal(whitePrice * 1.4m, anthracitePrice);
        Assert.Equal(otherWindowPrice, TestProject.Line(anthraciteResult, "1004-009").SalesUnitPrice);

        window80X120.Color = WindowColor.GoldenOak;
        var goldenOakResult = TestProject.Engine.Calculate(project);
        Assert.Equal(anthracitePrice, TestProject.Line(goldenOakResult, "1004-008").SalesUnitPrice);
    }
}

public sealed class ElectricalTests
{
    [Theory]
    [InlineData("99.99", "3")]
    [InlineData("100", "3")]
    [InlineData("100.01", "4")]
    public void Switch_threshold_at_100_square_meters_matches_excel(string area, string expected)
    {
        var project = TestProject.Create();
        project.Building.BuildingArea = decimal.Parse(area, System.Globalization.CultureInfo.InvariantCulture);

        var quantity = TestProject.Line(TestProject.Engine.Calculate(project), "1005-011").CalculatedQuantity;

        Assert.Equal(decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture), quantity);
    }
}

public sealed class PlumbingTests
{
    [Fact]
    public void First_floor_fixture_counts_are_ignored_for_one_floor_projects()
    {
        var project = TestProject.Create();
        var toilet = project.Building.Fixtures.First(item => item.Type == FixtureType.Toilet);
        toilet.GroundFloorQuantity = 1;
        toilet.FirstFloorQuantity = 7;
        project.Building.FloorCount = 1;
        var oneFloor = TestProject.Line(TestProject.Engine.Calculate(project), "1008-030").CalculatedQuantity;

        project.Building.FloorCount = 2;
        var twoFloor = TestProject.Line(TestProject.Engine.Calculate(project), "1008-030").CalculatedQuantity;

        Assert.Equal(1m, oneFloor);
        Assert.Equal(8m, twoFloor);
    }
}

public sealed class PricingTests
{
    [Fact]
    public void Project_pricing_snapshot_controls_steel_purchase_price()
    {
        var project = TestProject.Create();
        var original = TestProject.Line(TestProject.Engine.Calculate(project), "1001-001").PurchaseUnitPriceExVat;
        project.PricingSnapshot.GalvanizedPrice *= 2m;

        var changed = TestProject.Line(TestProject.Engine.Calculate(project), "1001-001").PurchaseUnitPriceExVat;

        Assert.Equal(original * 2m, changed);
    }

    [Fact]
    public void Sales_markup_factor_controls_legacy_markup_prices()
    {
        var project = TestProject.Create();
        project.PricingSnapshot.SalesMarkupFactor = 2m;

        var line = TestProject.Line(TestProject.Engine.Calculate(project), "1005-001");

        Assert.Equal(line.PurchaseUnitPriceIncVat * 2m, line.SalesUnitPrice);
    }

    [Fact]
    public void Negative_price_is_rejected_before_calculation()
    {
        var project = TestProject.Create();
        project.PricingSnapshot.ExchangeRate = -1m;

        Assert.Throws<CalculationValidationException>(() => TestProject.Engine.Calculate(project));
    }
}

public sealed class MaterialManagementTests
{
    [Fact]
    public void Inactive_material_remains_auditable_but_is_excluded_from_totals()
    {
        var project = TestProject.Create();
        project.InactiveMaterialCodes.Add("1001-001");

        var line = TestProject.Line(TestProject.Engine.Calculate(project), "1001-001");

        Assert.True(line.CalculatedQuantity > 0m);
        Assert.Equal(0m, line.EffectiveQuantity);
        Assert.Equal(QuantityMode.Manual, line.QuantityMode);
        Assert.Equal("Malzeme proje kapsamında pasif.", line.OverrideReason);
    }
}

public sealed class LegacyBoundaryTests
{
    [Theory]
    [InlineData("49.99", "3")]
    [InlineData("50", "5")]
    [InlineData("100", "5")]
    [InlineData("149.99", "5")]
    [InlineData("150", "7")]
    [InlineData("249.99", "7")]
    [InlineData("250", "10")]
    public void Area_thresholds_match_excel_if_branches(string area, string expected)
    {
        var project = TestProject.Create();
        project.Building.BuildingArea = decimal.Parse(area, System.Globalization.CultureInfo.InvariantCulture);

        var quantity = TestProject.Line(TestProject.Engine.Calculate(project), "1001-005").CalculatedQuantity;

        Assert.Equal(decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture), quantity);
    }

    [Theory]
    [InlineData(1, "0", "30")]
    [InlineData(2, "15", "60")]
    public void Floor_count_branches_match_excel(int floorCount, string upperFloorCable, string cable)
    {
        var project = TestProject.Create();
        project.Building.FloorCount = floorCount;
        var result = TestProject.Engine.Calculate(project);

        Assert.Equal(decimal.Parse(upperFloorCable), TestProject.Line(result, "1005-007").CalculatedQuantity);
        Assert.Equal(decimal.Parse(cable), TestProject.Line(result, "1005-010").CalculatedQuantity);
    }

    [Fact]
    public void Purlin_omega_and_panel_roof_systems_take_different_branches()
    {
        var project = TestProject.Create();
        project.Building.RoofSystem = RoofSystem.Panel;
        var panel = TestProject.Line(TestProject.Engine.Calculate(project), "1001-002").CalculatedQuantity;
        project.Building.RoofSystem = RoofSystem.PurlinOmega;
        var purlin = TestProject.Line(TestProject.Engine.Calculate(project), "1001-002").CalculatedQuantity;

        Assert.Equal(0m, panel);
        Assert.True(purlin > 0m);
    }
}

internal static class TestProject
{
    public static CalculationEngine Engine { get; } = new(new RoofCalculationService());

    public static Project Create() => new()
    {
        Building = BuildingInput.CreateLegacySample(),
        PricingSnapshot = new PricingParameters()
    };

    public static CalculationLine Line(CalculationResult result, string code) =>
        result.Categories.SelectMany(category => category.Lines).Single(line => line.MaterialCode == code);
}
