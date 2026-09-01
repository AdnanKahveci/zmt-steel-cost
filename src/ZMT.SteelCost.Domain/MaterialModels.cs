namespace ZMT.SteelCost.Domain;

public sealed class Material
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public int CategoryId { get; set; }
    public required string Name { get; set; }
    public string? Specification { get; set; }
    public required string Unit { get; set; }
    public decimal BasePurchasePrice { get; set; }
    public required string QuantityRuleId { get; set; }
    public required string PricingRuleId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool AllowManualQuantityOverride { get; set; } = true;
    public bool AllowManualPriceOverride { get; set; } = true;
}

public sealed record MaterialCategory(int Id, string Code, string Name, int SortOrder);

public sealed record MaterialFormulaParameter(
    string Id,
    string MaterialCode,
    string Name,
    decimal Value,
    string FormulaVersion);

public sealed class PricingParameters
{
    public decimal ExchangeRate { get; set; } = 48.1m;
    public decimal SteelPrice { get; set; } = 1.3m;
    public decimal SSeriesPrice { get; set; } = 1.35m;
    public decimal GalvanizedPrice { get; set; } = 0.90m;
    public decimal PaintedSheetPrice { get; set; } = 1.05m;
    public decimal SalesMarkupFactor { get; set; } = 1.73m;
    public decimal PurchaseVatRate { get; set; } = 0.20m;
    public decimal SalesVatRate { get; set; }
    public decimal DiscountRate { get; set; } = 0.21m;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    public PricingParameters Snapshot() => new()
    {
        ExchangeRate = ExchangeRate,
        SteelPrice = SteelPrice,
        SSeriesPrice = SSeriesPrice,
        GalvanizedPrice = GalvanizedPrice,
        PaintedSheetPrice = PaintedSheetPrice,
        SalesMarkupFactor = SalesMarkupFactor,
        PurchaseVatRate = PurchaseVatRate,
        SalesVatRate = SalesVatRate,
        DiscountRate = DiscountRate,
        LastUpdatedAt = LastUpdatedAt
    };
}

public sealed class PriceList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public bool IsActive { get; set; }
    public List<PriceListVersion> Versions { get; set; } = [];
}

public sealed class PriceListVersion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PriceListId { get; set; }
    public int VersionNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public PricingParameters Parameters { get; set; } = new();
    public List<MaterialPrice> Prices { get; set; } = [];
}

public sealed record MaterialPrice(string MaterialCode, decimal PurchasePrice);
