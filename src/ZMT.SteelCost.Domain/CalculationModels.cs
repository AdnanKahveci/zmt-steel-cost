namespace ZMT.SteelCost.Domain;

public sealed record CalculationTraceStep(string Label, string Expression, decimal Result, string? Unit = null);

public sealed record CalculationTrace(string RuleId, IReadOnlyList<CalculationTraceStep> Steps);

public sealed record CalculationLine(
    string MaterialCode,
    int CategoryId,
    string MaterialName,
    string? Specification,
    string Unit,
    decimal CalculatedQuantity,
    decimal EffectiveQuantity,
    QuantityMode QuantityMode,
    string? OverrideReason,
    decimal PurchaseUnitPriceExVat,
    decimal PurchaseUnitPriceIncVat,
    decimal PurchaseTotalExVat,
    decimal SalesUnitPrice,
    decimal DiscountedSalesUnitPrice,
    decimal SalesTotal,
    decimal GrossProfit,
    decimal GrossMarginRate,
    CalculationTrace Trace);

public sealed record CategoryCalculation(
    int CategoryId,
    string CategoryName,
    ResponsibilityType Responsibility,
    decimal CalculatedTotal,
    decimal IncludedTotal,
    IReadOnlyList<CalculationLine> Lines);

public sealed record CalculationResult(
    Guid RunId,
    DateTime CalculatedAt,
    string FormulaVersion,
    decimal SteelWeight,
    decimal FullCalculatedValue,
    decimal SupplierScopeValue,
    decimal CustomerScopeValue,
    decimal DiscountRate,
    decimal DiscountAmount,
    decimal SubtotalAfterDiscount,
    decimal VatRate,
    decimal VatAmount,
    decimal GrandTotal,
    decimal TotalPurchaseCost,
    decimal GrossProfit,
    decimal GrossMarginRate,
    IReadOnlyList<CategoryCalculation> Categories);

public sealed record CalculationSnapshot(
    Guid ProjectId,
    string FormulaVersion,
    Guid? PriceListVersionId,
    DateTime CreatedAt,
    string InputJson,
    string PricingJson,
    string ResultJson);
