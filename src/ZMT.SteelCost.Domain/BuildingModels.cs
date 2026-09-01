namespace ZMT.SteelCost.Domain;

public sealed class BuildingInput
{
    public decimal BuildingArea { get; set; } = 105m;
    public decimal EstimatedSteelKgPerM2 { get; set; } = 32m;
    public int CornerCount { get; set; } = 6;
    public decimal GroundFloorWidth { get; set; }
    public decimal GroundFloorLength { get; set; }
    public int FloorCount { get; set; } = 1;
    public decimal IntermediateFloorArea { get; set; }
    public decimal FloorHeight { get; set; } = 2.8m;
    public decimal ExteriorWallThicknessMm { get; set; } = 80m;
    public decimal InteriorWallThicknessMm { get; set; } = 80m;
    public RoofType RoofType { get; set; } = RoofType.Gable;
    public decimal RoofSlope { get; set; } = 0.30m;
    public decimal RoofFootprintArea { get; set; } = 105m;
    public RoofCoverType RoofCoverType { get; set; } = RoofCoverType.MetalTile;
    public RoofSystem RoofSystem { get; set; } = RoofSystem.Panel;
    public decimal EaveWidth { get; set; } = 0.4m;
    public decimal EaveLength { get; set; } = 20.56m;
    public decimal GableLength { get; set; } = 20.4m;
    public decimal WetAreaWallLength { get; set; } = 17.5m;
    public decimal WetAreaCeilingArea { get; set; } = 19.5m;
    public decimal ExteriorWallLength { get; set; } = 39.5m;
    public decimal InteriorWallLength { get; set; } = 33m;
    public decimal CeilingArea { get; set; } = 97m;
    public decimal PurlinCount3000 { get; set; }
    public decimal OmegaCount2500 { get; set; }
    public WindowColor WindowColor { get; set; } = WindowColor.Anthracite;

    public List<SurfaceConfiguration> Surfaces { get; set; } = [];
    public List<DoorSelection> Doors { get; set; } = [];
    public List<WindowSelection> Windows { get; set; } = [];
    public List<FixtureSelection> Fixtures { get; set; } = [];
    public Dictionary<string, decimal> SpecialAccessories { get; set; } = new(StringComparer.Ordinal);

    public static BuildingInput CreateLegacySample()
    {
        var input = new BuildingInput();
        input.Surfaces.AddRange(
        [
            new(SurfaceType.ExteriorWall, input.ExteriorWallLength,
                [SurfaceLayerType.Osb11Mm, SurfaceLayerType.MoistureBarrier, SurfaceLayerType.Bordex]),
            new(SurfaceType.InteriorWall, input.InteriorWallLength,
                [SurfaceLayerType.Osb11Mm, SurfaceLayerType.MoistureBarrier, SurfaceLayerType.Drywall]),
            new(SurfaceType.Roof, 0m,
                [SurfaceLayerType.Osb11Mm, SurfaceLayerType.MoistureBarrier]),
            new(SurfaceType.Ceiling, input.CeilingArea,
                [SurfaceLayerType.Drywall])
        ]);
        input.Doors.AddRange(
        [
            new(DoorType.Steel90X205, 1),
            new(DoorType.Melamine90X201, 6)
        ]);
        input.Windows.AddRange(
        [
            new(WindowType.Pvc80X120, 1, WindowColor.Anthracite),
            new(WindowType.Pvc140X100, 2, WindowColor.Anthracite),
            new(WindowType.Pvc160X180, 3, WindowColor.Anthracite),
            new(WindowType.PvcTransom60X60, 2, WindowColor.Anthracite)
        ]);
        input.Fixtures.AddRange(
        [
            new(FixtureType.Toilet, 2, 0),
            new(FixtureType.Washbasin, 2, 0),
            new(FixtureType.SquatToilet, 0, 0),
            new(FixtureType.ShowerTray, 2, 0)
        ]);
        input.SpecialAccessories["METAL_TILE_RIDGE"] = 4m;
        return input;
    }
}

public sealed record SurfaceLayer(int SortOrder, SurfaceLayerType Type);

public sealed class SurfaceConfiguration
{
    public SurfaceConfiguration()
    {
    }

    public SurfaceConfiguration(SurfaceType surface, decimal lengthOrArea, IEnumerable<SurfaceLayerType> layers)
    {
        Surface = surface;
        LengthOrArea = lengthOrArea;
        Layers = layers.ToList();
    }

    public SurfaceType Surface { get; set; }
    public decimal LengthOrArea { get; set; }
    public List<SurfaceLayerType> Layers { get; set; } = [];
}

public sealed class DoorSelection
{
    public DoorSelection()
    {
    }

    public DoorSelection(DoorType type, int quantity)
    {
        Type = type;
        Quantity = quantity;
    }

    public DoorType Type { get; set; }
    public int Quantity { get; set; }
}

public sealed class WindowSelection
{
    public WindowSelection()
    {
    }

    public WindowSelection(WindowType type, int quantity, WindowColor color)
    {
        Type = type;
        Quantity = quantity;
        Color = color;
    }

    public WindowType Type { get; set; }
    public int Quantity { get; set; }
    public WindowColor Color { get; set; }
}

public sealed class FixtureSelection
{
    public FixtureSelection()
    {
    }

    public FixtureSelection(FixtureType type, int groundFloorQuantity, int firstFloorQuantity)
    {
        Type = type;
        GroundFloorQuantity = groundFloorQuantity;
        FirstFloorQuantity = firstFloorQuantity;
    }

    public FixtureType Type { get; set; }
    public int GroundFloorQuantity { get; set; }
    public int FirstFloorQuantity { get; set; }
}

public sealed record RoofCalculationResult(
    decimal EaveArea,
    decimal SlopeFactor,
    decimal RoofCoverArea);
