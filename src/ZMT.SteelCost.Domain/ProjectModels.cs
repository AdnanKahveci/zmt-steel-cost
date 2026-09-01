namespace ZMT.SteelCost.Domain;

public sealed class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Company { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CrmNumber { get; set; } = string.Empty;
    public string OfferPreparedBy { get; set; } = string.Empty;
    public string ProjectCheckedBy { get; set; } = string.Empty;
    public string ProductionListPreparedBy { get; set; } = string.Empty;
    public ProjectStage Stage { get; set; } = ProjectStage.OfferDrawingReady;
    public DateTime DocumentDate { get; set; } = DateTime.Today;
    public BuildingInput Building { get; set; } = BuildingInput.CreateLegacySample();
    public Guid? PriceListVersionId { get; set; }
    public PricingParameters PricingSnapshot { get; set; } = new();
    public decimal? DiscountRateOverride { get; set; }
    public decimal? SalesVatRateOverride { get; set; }
    public string FormulaVersion { get; set; } = "LegacyExcel-v1";
    public List<ProjectCategoryScope> CategoryScopes { get; set; } = [];
    public List<MaterialOverride> MaterialOverrides { get; set; } = [];
    public Dictionary<string, decimal> MaterialPriceOverrides { get; set; } = new(StringComparer.Ordinal);
    public HashSet<string> InactiveMaterialCodes { get; set; } = new(StringComparer.Ordinal);
}

public sealed record ProjectCategoryScope(int CategoryId, ResponsibilityType Responsibility);

public sealed class MaterialOverride
{
    public required string MaterialCode { get; set; }
    public QuantityMode Mode { get; set; }
    public decimal CalculatedQuantity { get; set; }
    public decimal? OverrideQuantity { get; set; }
    public string OverrideReason { get; set; } = string.Empty;
    public decimal EffectiveQuantity => Mode == QuantityMode.Manual && OverrideQuantity.HasValue
        ? OverrideQuantity.Value
        : CalculatedQuantity;
}
