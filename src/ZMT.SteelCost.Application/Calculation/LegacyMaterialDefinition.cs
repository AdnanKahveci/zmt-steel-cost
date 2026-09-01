namespace ZMT.SteelCost.Application.Calculation;

public sealed record LegacyMaterialDefinition(
    string Code,
    int CategoryId,
    string CategoryName,
    int SortOrder,
    int LegacyRow,
    string Name,
    string? Specification,
    string Unit,
    decimal SpecificationNumber,
    string QuantityRuleId,
    string PricingRuleId);
