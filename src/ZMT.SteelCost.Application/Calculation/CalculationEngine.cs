using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Application.Calculation;

public interface ICalculationEngine
{
    CalculationResult Calculate(Project project);
}

public interface ICalculationRule
{
    decimal Calculate(LegacyRuleContext context);
}

public sealed class CalculationEngine(IRoofCalculationService roofService) : ICalculationEngine
{
    private static readonly IReadOnlyDictionary<int, ResponsibilityType> LegacyScopes =
        new Dictionary<int, ResponsibilityType>
        {
            [1001] = ResponsibilityType.Zmt,
            [1002] = ResponsibilityType.Zmt,
            [1003] = ResponsibilityType.Zmt,
            [1004] = ResponsibilityType.Customer,
            [1005] = ResponsibilityType.Zmt,
            [1006] = ResponsibilityType.Zmt,
            [1007] = ResponsibilityType.Zmt,
            [1008] = ResponsibilityType.Customer,
            [1009] = ResponsibilityType.Zmt,
            [1010] = ResponsibilityType.Customer
        };

    public CalculationResult Calculate(Project project)
    {
        var validationErrors = BuildingInputValidator.Validate(project.Building)
            .Concat(PricingParametersValidator.Validate(project.PricingSnapshot))
            .ToList();
        if (project.MaterialPriceOverrides.Values.Any(value => value < 0m))
        {
            validationErrors.Add(new("MaterialPriceOverrides", "Malzeme alış fiyatı negatif olamaz."));
        }
        var knownCodes = LegacyExcelV1Rules.Materials.Select(item => item.Code).ToHashSet(StringComparer.Ordinal);
        foreach (var code in project.MaterialPriceOverrides.Keys.Concat(project.InactiveMaterialCodes))
        {
            if (!knownCodes.Contains(code))
            {
                validationErrors.Add(new("MaterialCatalog", $"Bilinmeyen malzeme kodu: {code}"));
            }
        }
        foreach (var materialOverride in project.MaterialOverrides)
        {
            if (!knownCodes.Contains(materialOverride.MaterialCode))
            {
                validationErrors.Add(new("MaterialOverrides", $"Bilinmeyen malzeme kodu: {materialOverride.MaterialCode}"));
            }
            if (materialOverride.Mode == QuantityMode.Manual &&
                (materialOverride.OverrideQuantity is null or < 0m || string.IsNullOrWhiteSpace(materialOverride.OverrideReason)))
            {
                validationErrors.Add(new("MaterialOverrides", $"{materialOverride.MaterialCode} manuel miktarı için değer ve açıklama zorunludur."));
            }
        }
        if (validationErrors.Count != 0)
        {
            throw new CalculationValidationException(validationErrors);
        }

        var pricing = project.PricingSnapshot;
        var discountRate = project.DiscountRateOverride ?? pricing.DiscountRate;
        var vatRate = project.SalesVatRateOverride ?? pricing.SalesVatRate;
        ValidateRate(discountRate, "İskonto");
        ValidateRate(vatRate, "KDV");

        var context = new LegacyRuleContext(project.Building, pricing, roofService, project.MaterialPriceOverrides);
        var overrides = project.MaterialOverrides.ToDictionary(item => item.MaterialCode, StringComparer.Ordinal);
        var lines = new List<CalculationLine>(LegacyExcelV1Rules.Materials.Count);

        foreach (var definition in LegacyExcelV1Rules.Materials)
        {
            var calculatedQuantity = context.Quantity(definition.Code);
            overrides.TryGetValue(definition.Code, out var materialOverride);
            var inactive = project.InactiveMaterialCodes.Contains(definition.Code);
            var mode = inactive ? QuantityMode.Manual : materialOverride?.Mode ?? QuantityMode.Auto;
            var effectiveQuantity = inactive
                ? 0m
                : mode == QuantityMode.Manual && materialOverride?.OverrideQuantity is not null
                ? materialOverride.OverrideQuantity.Value
                : calculatedQuantity;
            if (effectiveQuantity < 0m)
            {
                throw new InvalidOperationException($"{definition.Code} için negatif miktar kullanılamaz.");
            }

            var purchaseExVat = context.PurchaseUnitPriceExVat(definition.Code);
            var purchaseIncVat = context.PurchaseUnitPriceIncVat(definition.Code);
            var salesUnit = context.SalesUnitPrice(definition.Code);
            var purchaseTotal = effectiveQuantity * purchaseExVat;
            var salesTotal = effectiveQuantity * salesUnit;
            var discountedUnit = salesUnit * (1m - discountRate);
            var grossProfit = salesTotal - (effectiveQuantity * purchaseIncVat);
            var margin = DecimalMath.SafeDivide(discountedUnit - purchaseIncVat, purchaseIncVat);

            lines.Add(new(
                definition.Code,
                definition.CategoryId,
                definition.Name,
                definition.Specification,
                definition.Unit,
                calculatedQuantity,
                effectiveQuantity,
                mode,
                inactive ? "Malzeme proje kapsamında pasif." : materialOverride?.OverrideReason,
                purchaseExVat,
                purchaseIncVat,
                purchaseTotal,
                salesUnit,
                discountedUnit,
                salesTotal,
                grossProfit,
                margin,
                BuildTrace(definition, project.Building, calculatedQuantity)));
        }

        var requestedScopes = project.CategoryScopes.ToDictionary(item => item.CategoryId, item => item.Responsibility);
        var categories = lines
            .GroupBy(line => line.CategoryId)
            .OrderBy(group => group.Key)
            .Select(group =>
            {
                var definition = LegacyExcelV1Rules.Materials.First(item => item.CategoryId == group.Key);
                var responsibility = requestedScopes.TryGetValue(group.Key, out var configured)
                    ? configured
                    : LegacyScopes[group.Key];
                var total = group.Sum(item => item.SalesTotal);
                return new CategoryCalculation(
                    group.Key,
                    definition.CategoryName,
                    responsibility,
                    total,
                    responsibility == ResponsibilityType.Zmt ? total : 0m,
                    group.ToArray());
            })
            .ToArray();

        var fullValue = categories.Sum(item => item.CalculatedTotal);
        var supplierValue = categories.Sum(item => item.IncludedTotal);
        var customerValue = fullValue - supplierValue;
        var discountAmount = supplierValue * discountRate;
        var subtotal = supplierValue - discountAmount;
        var vatAmount = subtotal * vatRate;
        var grandTotal = subtotal + vatAmount;
        var totalPurchase = categories
            .Where(item => item.Responsibility == ResponsibilityType.Zmt)
            .SelectMany(item => item.Lines)
            .Sum(item => item.EffectiveQuantity * item.PurchaseUnitPriceIncVat);
        var grossProfitTotal = grandTotal - totalPurchase;
        var grossMarginRate = DecimalMath.SafeDivide(grossProfitTotal, totalPurchase);

        return new(
            Guid.NewGuid(),
            DateTime.UtcNow,
            LegacyExcelV1Rules.FormulaVersion,
            project.Building.BuildingArea * project.Building.EstimatedSteelKgPerM2,
            fullValue,
            supplierValue,
            customerValue,
            discountRate,
            discountAmount,
            subtotal,
            vatRate,
            vatAmount,
            grandTotal,
            totalPurchase,
            grossProfitTotal,
            grossMarginRate,
            categories);
    }

    private static CalculationTrace BuildTrace(
        LegacyMaterialDefinition definition,
        BuildingInput input,
        decimal calculatedQuantity)
    {
        if (definition.Code == "1001-001")
        {
            return new(definition.QuantityRuleId,
            [
                new("Bina alanı", $"{input.BuildingArea:0.##} m²", input.BuildingArea, "m²"),
                new("Tahmini çelik", $"{input.EstimatedSteelKgPerM2:0.##} kg/m²", input.EstimatedSteelKgPerM2, "kg/m²"),
                new("Sonuç", $"{input.BuildingArea:0.##} × {input.EstimatedSteelKgPerM2:0.##}", calculatedQuantity, "kg")
            ]);
        }

        return new(definition.QuantityRuleId,
        [
            new("Strongly typed kural", definition.QuantityRuleId, calculatedQuantity, definition.Unit)
        ]);
    }

    private static void ValidateRate(decimal value, string label)
    {
        if (value is < 0m or > 1m)
        {
            throw new ArgumentOutOfRangeException(label, $"{label} 0 ile 1 arasında olmalıdır.");
        }
    }
}

public sealed class CalculationValidationException(IReadOnlyList<ValidationError> errors)
    : Exception(string.Join(Environment.NewLine, errors.Select(item => item.Message)))
{
    public IReadOnlyList<ValidationError> Errors { get; } = errors;
}
