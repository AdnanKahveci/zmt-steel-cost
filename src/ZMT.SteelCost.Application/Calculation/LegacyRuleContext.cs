using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Application.Calculation;

public sealed class LegacyRuleContext
{
    private static readonly IReadOnlyDictionary<WindowType, (string Code, decimal Perimeter)> WindowRules =
        new Dictionary<WindowType, (string, decimal)>
        {
            [WindowType.Pvc105X180] = ("1004-006", 5.7m),
            [WindowType.Pvc59X180] = ("1004-007", 4.8m),
            [WindowType.Pvc80X120] = ("1004-008", 4m),
            [WindowType.Pvc140X100] = ("1004-009", 4.8m),
            [WindowType.Pvc140X140] = ("1004-010", 5.6m),
            [WindowType.Pvc140X160] = ("1004-011", 6m),
            [WindowType.Pvc140X180] = ("1004-012", 6.4m),
            [WindowType.Pvc160X120] = ("1004-013", 5.6m),
            [WindowType.Pvc160X160] = ("1004-014", 6.4m),
            [WindowType.Pvc160X180] = ("1004-015", 6.8m),
            [WindowType.PvcSliding180X200] = ("1004-016", 7.6m),
            [WindowType.PvcTransom60X60] = ("1004-017", 2.4m)
        };

    private static readonly IReadOnlyDictionary<string, WindowType> WindowTypesByMaterialCode =
        WindowRules.ToDictionary(item => item.Value.Code, item => item.Key, StringComparer.Ordinal);

    private readonly BuildingInput _input;
    private readonly RoofCalculationResult _roof;
    private readonly IReadOnlyDictionary<string, LegacyMaterialDefinition> _definitions;
    private readonly IReadOnlyDictionary<string, decimal> _parameters;
    private readonly IReadOnlyDictionary<string, decimal> _materialPriceOverrides;
    private readonly Dictionary<string, decimal> _quantityCache = new(StringComparer.Ordinal);
    private readonly Dictionary<string, decimal> _purchaseCache = new(StringComparer.Ordinal);
    private readonly Dictionary<string, decimal> _salesCache = new(StringComparer.Ordinal);
    private readonly HashSet<string> _quantityStack = new(StringComparer.Ordinal);
    private readonly HashSet<string> _purchaseStack = new(StringComparer.Ordinal);
    private readonly HashSet<string> _salesStack = new(StringComparer.Ordinal);
    private readonly Stack<string> _evaluationStack = new();

    public LegacyRuleContext(
        BuildingInput input,
        PricingParameters pricing,
        IRoofCalculationService roofService,
        IReadOnlyDictionary<string, decimal>? materialPriceOverrides = null)
    {
        _input = input;
        Pricing = pricing;
        _roof = roofService.Calculate(input);
        _definitions = LegacyExcelV1Rules.Materials.ToDictionary(item => item.Code, StringComparer.Ordinal);
        _parameters = LegacyExcelV1Rules.FormulaParameters.ToDictionary(item => item.Id, item => item.Value, StringComparer.Ordinal);
        _materialPriceOverrides = materialPriceOverrides ?? new Dictionary<string, decimal>();
    }

    public PricingParameters Pricing { get; }

    public decimal Quantity(string materialCode) => Resolve(
        materialCode,
        _quantityCache,
        _quantityStack,
        LegacyExcelV1Rules.CalculateQuantity,
        "Miktar formüllerinde döngü bulundu");

    public decimal PurchaseUnitPriceExVat(string materialCode)
    {
        if (_materialPriceOverrides.TryGetValue(materialCode, out var overridden))
        {
            return overridden;
        }
        return Resolve(materialCode, _purchaseCache, _purchaseStack, LegacyExcelV1Rules.CalculatePurchaseUnitPriceExVat,
            "Alış fiyatı formüllerinde döngü bulundu");
    }

    public decimal PurchaseUnitPriceIncVat(string materialCode) =>
        PurchaseUnitPriceExVat(materialCode) * (1m + Pricing.PurchaseVatRate);

    public decimal SalesUnitPrice(string materialCode) => Resolve(
        materialCode,
        _salesCache,
        _salesStack,
        LegacyExcelV1Rules.CalculateSalesUnitPrice,
        "Satış fiyatı formüllerinde döngü bulundu");

    public decimal SalesLineTotal(string materialCode) => Quantity(materialCode) * SalesUnitPrice(materialCode);

    public decimal SpecificationNumber(string materialCode) => _definitions[materialCode].SpecificationNumber;

    public decimal Parameter(string id) => _parameters.TryGetValue(id, out var value) ? value : 0m;

    public decimal SumQuantities(params string[] materialCodes) => materialCodes.Sum(Quantity);

    public decimal SumFixtureCounts() => _input.Fixtures.Sum(item =>
        item.GroundFloorQuantity + (_input.FloorCount == 2 ? item.FirstFloorQuantity : 0));

    public decimal CountLayers(string stableCode) => _input.Surfaces.Sum(surface =>
        surface.Layers.Count(layer => string.Equals(LegacyCodes.Layer(layer), stableCode, StringComparison.Ordinal)));

    public decimal WindowTrimQuantity()
    {
        var roundedPerimeters = WindowRules.Values.Sum(rule => ExcelMath.RoundUp(rule.Perimeter * Quantity(rule.Code), 0));
        return roundedPerimeters / 2.5m;
    }

    public decimal Number(LegacyInputField field) => field switch
    {
        LegacyInputField.BuildingArea => _input.BuildingArea,
        LegacyInputField.EstimatedSteelKgPerM2 => _input.EstimatedSteelKgPerM2,
        LegacyInputField.CornerCount => _input.CornerCount,
        LegacyInputField.GroundFloorWidth => _input.GroundFloorWidth,
        LegacyInputField.GroundFloorLength => _input.GroundFloorLength,
        LegacyInputField.FloorCount => _input.FloorCount,
        LegacyInputField.IntermediateFloorArea => _input.IntermediateFloorArea,
        LegacyInputField.FloorHeight => _input.FloorHeight,
        LegacyInputField.RoofSlope => _input.RoofSlope,
        LegacyInputField.RoofFootprintArea => _input.RoofFootprintArea,
        LegacyInputField.EaveWidth => _input.EaveWidth,
        LegacyInputField.EaveLength => _input.EaveLength,
        LegacyInputField.GableLength => _input.GableLength,
        LegacyInputField.RidgeQuantity => Accessory("RIDGE"),
        LegacyInputField.ParapetCoverQuantity => Accessory("PARAPET_COVER"),
        LegacyInputField.MetalTileRidgeQuantity => Accessory("METAL_TILE_RIDGE"),
        LegacyInputField.NarrowRidgeQuantity => Accessory("NARROW_RIDGE"),
        LegacyInputField.WideRidgeQuantity => Accessory("WIDE_RIDGE"),
        LegacyInputField.MetalBoardQuantity => Accessory("METAL_BOARD"),
        LegacyInputField.WetAreaWallLength => _input.WetAreaWallLength,
        LegacyInputField.WetAreaCeilingArea => _input.WetAreaCeilingArea,
        LegacyInputField.ExteriorWallLength => _input.ExteriorWallLength,
        LegacyInputField.InteriorWallLength => _input.InteriorWallLength,
        LegacyInputField.RoofCoverArea => _roof.RoofCoverArea,
        LegacyInputField.CeilingArea => _input.CeilingArea,
        LegacyInputField.EaveArea => _roof.EaveArea,
        LegacyInputField.SteelDoorQuantity => Door(DoorType.Steel90X205),
        LegacyInputField.PvcDoorQuantity => Door(DoorType.Pvc90X200),
        LegacyInputField.DoublePvcDoorQuantity => Door(DoorType.DoublePvc160X200),
        LegacyInputField.MelamineDoorQuantity => Door(DoorType.Melamine90X201),
        LegacyInputField.AmericanDoorQuantity => Door(DoorType.AmericanWoodFrame90X201),
        LegacyInputField.Window105X180Quantity => Window(WindowType.Pvc105X180),
        LegacyInputField.Window59X180Quantity => Window(WindowType.Pvc59X180),
        LegacyInputField.Window80X120Quantity => Window(WindowType.Pvc80X120),
        LegacyInputField.Window140X100Quantity => Window(WindowType.Pvc140X100),
        LegacyInputField.Window140X140Quantity => Window(WindowType.Pvc140X140),
        LegacyInputField.Window140X160Quantity => Window(WindowType.Pvc140X160),
        LegacyInputField.Window140X180Quantity => Window(WindowType.Pvc140X180),
        LegacyInputField.Window160X120Quantity => Window(WindowType.Pvc160X120),
        LegacyInputField.Window160X160Quantity => Window(WindowType.Pvc160X160),
        LegacyInputField.Window160X180Quantity => Window(WindowType.Pvc160X180),
        LegacyInputField.SlidingWindow180X200Quantity => Window(WindowType.PvcSliding180X200),
        LegacyInputField.TransomWindow60X60Quantity => Window(WindowType.PvcTransom60X60),
        LegacyInputField.GroundFloorToiletQuantity => Fixture(FixtureType.Toilet, false),
        LegacyInputField.FirstFloorToiletQuantity => Fixture(FixtureType.Toilet, true),
        LegacyInputField.GroundFloorWashbasinQuantity => Fixture(FixtureType.Washbasin, false),
        LegacyInputField.FirstFloorWashbasinQuantity => Fixture(FixtureType.Washbasin, true),
        LegacyInputField.GroundFloorSquatToiletQuantity => Fixture(FixtureType.SquatToilet, false),
        LegacyInputField.FirstFloorSquatToiletQuantity => Fixture(FixtureType.SquatToilet, true),
        LegacyInputField.GroundFloorShowerTrayQuantity => Fixture(FixtureType.ShowerTray, false),
        LegacyInputField.FirstFloorShowerTrayQuantity => Fixture(FixtureType.ShowerTray, true),
        LegacyInputField.ExteriorWallThicknessMm => _input.ExteriorWallThicknessMm,
        LegacyInputField.InteriorWallThicknessMm => _input.InteriorWallThicknessMm,
        LegacyInputField.PurlinCount3000 => _input.PurlinCount3000,
        LegacyInputField.OmegaCount2500 => _input.OmegaCount2500,
        LegacyInputField.LegacyUnusedC61 => 0m,
        _ => throw new ArgumentOutOfRangeException(nameof(field), field, "Sayısal legacy alanı değil.")
    };

    public string Option(LegacyInputField field) => field switch
    {
        LegacyInputField.RoofCoverType => LegacyCodes.RoofCover(_input.RoofCoverType),
        LegacyInputField.RoofSystem => LegacyCodes.RoofSystem(_input.RoofSystem),
        LegacyInputField.WindowColor => LegacyCodes.WindowColor(CurrentWindowColor()),
        LegacyInputField.ExteriorLayer1 => Layer(SurfaceType.ExteriorWall, 0),
        LegacyInputField.ExteriorLayer2 => Layer(SurfaceType.ExteriorWall, 1),
        LegacyInputField.ExteriorLayer3 => Layer(SurfaceType.ExteriorWall, 2),
        LegacyInputField.InteriorLayer1 => Layer(SurfaceType.InteriorWall, 0),
        LegacyInputField.InteriorLayer2 => Layer(SurfaceType.InteriorWall, 1),
        LegacyInputField.InteriorLayer3 => Layer(SurfaceType.InteriorWall, 2),
        LegacyInputField.RoofLayer1 => Layer(SurfaceType.Roof, 0),
        LegacyInputField.RoofLayer2 => Layer(SurfaceType.Roof, 1),
        LegacyInputField.RoofLayer3 => Layer(SurfaceType.Roof, 2),
        LegacyInputField.CeilingLayer1 => Layer(SurfaceType.Ceiling, 0),
        LegacyInputField.CeilingLayer2 => Layer(SurfaceType.Ceiling, 1),
        LegacyInputField.CeilingLayer3 => Layer(SurfaceType.Ceiling, 2),
        _ => throw new ArgumentOutOfRangeException(nameof(field), field, "Metinsel legacy alanı değil.")
    };

    private decimal Resolve(
        string materialCode,
        IDictionary<string, decimal> cache,
        ISet<string> stack,
        Func<string, LegacyRuleContext, decimal> rule,
        string cycleMessage)
    {
        if (cache.TryGetValue(materialCode, out var value))
        {
            return value;
        }
        if (!stack.Add(materialCode))
        {
            throw new InvalidOperationException($"{cycleMessage}: {materialCode}");
        }
        try
        {
            _evaluationStack.Push(materialCode);
            value = rule(materialCode, this);
            cache[materialCode] = value;
            return value;
        }
        finally
        {
            _evaluationStack.Pop();
            stack.Remove(materialCode);
        }
    }

    private decimal Accessory(string key) => _input.SpecialAccessories.GetValueOrDefault(key);
    private decimal Door(DoorType type) => _input.Doors.Where(item => item.Type == type).Sum(item => item.Quantity);
    private decimal Window(WindowType type) => _input.Windows.Where(item => item.Type == type).Sum(item => item.Quantity);
    private WindowColor CurrentWindowColor()
    {
        if (_evaluationStack.TryPeek(out var materialCode) &&
            WindowTypesByMaterialCode.TryGetValue(materialCode, out var windowType))
        {
            return _input.Windows.FirstOrDefault(item => item.Type == windowType)?.Color ?? _input.WindowColor;
        }

        return _input.WindowColor;
    }
    private decimal Fixture(FixtureType type, bool firstFloor)
    {
        var item = _input.Fixtures.FirstOrDefault(value => value.Type == type);
        return item is null ? 0m : firstFloor
            ? _input.FloorCount == 2 ? item.FirstFloorQuantity : 0m
            : item.GroundFloorQuantity;
    }
    private string Layer(SurfaceType surfaceType, int index)
    {
        var layers = _input.Surfaces.FirstOrDefault(item => item.Surface == surfaceType)?.Layers;
        return layers is not null && index < layers.Count ? LegacyCodes.Layer(layers[index]) : string.Empty;
    }
}

public static class LegacyCodes
{
    public static string RoofSystem(RoofSystem value) => value switch
    {
        Domain.RoofSystem.PurlinOmega => "ASIK_OMEGA",
        Domain.RoofSystem.Panel => "PANEL_SISTEM",
        _ => string.Empty
    };

    public static string RoofCover(RoofCoverType value) => value switch
    {
        RoofCoverType.TrapezoidalSheet => "TRAPEZ_CATI",
        RoofCoverType.SandwichPanel => "SANDVIC_PANEL",
        RoofCoverType.MetalTile => "METAL_KIREMIT_CATI",
        _ => string.Empty
    };

    public static string WindowColor(WindowColor value) => value switch
    {
        Domain.WindowColor.White => "BEYAZ",
        Domain.WindowColor.Anthracite => "ANTRASIT",
        Domain.WindowColor.GoldenOak => "ALTINMESE",
        _ => string.Empty
    };

    public static string Layer(SurfaceLayerType value) => value switch
    {
        SurfaceLayerType.Drywall => "ALCIPAN",
        SurfaceLayerType.Bordex => "BORDEX",
        SurfaceLayerType.Osb11Mm => "11_MM_OSB_2",
        SurfaceLayerType.SidingFiberCement => "YALIBASKI_SIDING_FIBERCEMENT",
        SurfaceLayerType.WoodPatternJointedFiberCement => "AGACDESEN_FUGALI_FIBERCEMENT",
        SurfaceLayerType.StonePatternJointedFiberCement => "TASDESEN_FUGALI_FIBERCEMENT",
        SurfaceLayerType.WoodPatternBoard => "AHSAP_DESEN_LEVHA",
        SurfaceLayerType.FiberCementBoard => "FIBERCEMENT_LEVHA",
        SurfaceLayerType.MoistureBarrier => "NEM_BARIYERI",
        SurfaceLayerType.Membrane => "MEBRAN",
        SurfaceLayerType.SlateMembrane => "ARDUAZLI_MEBRAN",
        _ => string.Empty
    };
}

public static class LegacyMath
{
    public static decimal Power(decimal value, decimal exponent) => (decimal)Math.Pow((double)value, (double)exponent);
}
