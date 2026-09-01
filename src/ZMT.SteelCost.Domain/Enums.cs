namespace ZMT.SteelCost.Domain;

public enum RoofType
{
    Hip,
    Gable,
    Parapet,
    MonoPitch
}

public enum RoofSystem
{
    PurlinOmega,
    Panel
}

public enum RoofCoverType
{
    TrapezoidalSheet,
    SandwichPanel,
    MetalTile
}

public enum ResponsibilityType
{
    Zmt,
    Customer
}

public enum SurfaceType
{
    ExteriorWall,
    InteriorWall,
    Roof,
    Ceiling
}

public enum SurfaceLayerType
{
    None,
    Drywall,
    Bordex,
    Osb11Mm,
    SidingFiberCement,
    WoodPatternJointedFiberCement,
    StonePatternJointedFiberCement,
    WoodPatternBoard,
    FiberCementBoard,
    MoistureBarrier,
    Membrane,
    SlateMembrane
}

public enum WindowColor
{
    White,
    Anthracite,
    GoldenOak
}

public enum DoorType
{
    Steel90X205,
    Pvc90X200,
    DoublePvc160X200,
    Melamine90X201,
    AmericanWoodFrame90X201
}

public enum WindowType
{
    Pvc105X180,
    Pvc59X180,
    Pvc80X120,
    Pvc140X100,
    Pvc140X140,
    Pvc140X160,
    Pvc140X180,
    Pvc160X120,
    Pvc160X160,
    Pvc160X180,
    PvcSliding180X200,
    PvcTransom60X60
}

public enum FixtureType
{
    Toilet,
    Washbasin,
    SquatToilet,
    ShowerTray
}

public enum QuantityMode
{
    Auto,
    Manual
}

public enum ProjectStage
{
    OfferDrawingReady,
    ManufacturingDrawingReady,
    OfferListReady,
    ProductionListReady,
    SentToProduction
}
