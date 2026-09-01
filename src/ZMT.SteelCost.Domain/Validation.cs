namespace ZMT.SteelCost.Domain;

public sealed record ValidationError(string Property, string Message);

public static class BuildingInputValidator
{
    private static readonly decimal[] SupportedSlopes = [0.25m, 0.30m, 0.35m, 0.40m, 0.45m, 0.50m, 0.55m, 0.60m];

    public static IReadOnlyList<ValidationError> Validate(BuildingInput input)
    {
        var errors = new List<ValidationError>();
        NonNegative(errors, nameof(input.BuildingArea), input.BuildingArea, "Bina alanı");
        NonNegative(errors, nameof(input.EstimatedSteelKgPerM2), input.EstimatedSteelKgPerM2, "Tahmini çelik");
        NonNegative(errors, nameof(input.GroundFloorWidth), input.GroundFloorWidth, "Zemin kat en");
        NonNegative(errors, nameof(input.GroundFloorLength), input.GroundFloorLength, "Zemin kat boy");
        NonNegative(errors, nameof(input.IntermediateFloorArea), input.IntermediateFloorArea, "Ara kat alanı");
        NonNegative(errors, nameof(input.FloorHeight), input.FloorHeight, "Kat yüksekliği");
        NonNegative(errors, nameof(input.ExteriorWallThicknessMm), input.ExteriorWallThicknessMm, "Dış duvar kalınlığı");
        NonNegative(errors, nameof(input.InteriorWallThicknessMm), input.InteriorWallThicknessMm, "İç duvar kalınlığı");
        NonNegative(errors, nameof(input.RoofFootprintArea), input.RoofFootprintArea, "Çatı oturum alanı");
        NonNegative(errors, nameof(input.EaveWidth), input.EaveWidth, "Saçak genişliği");
        NonNegative(errors, nameof(input.EaveLength), input.EaveLength, "Saçak uzunluğu");
        NonNegative(errors, nameof(input.GableLength), input.GableLength, "Alın uzunluğu");
        NonNegative(errors, nameof(input.WetAreaWallLength), input.WetAreaWallLength, "Islak hacim duvar uzunluğu");
        NonNegative(errors, nameof(input.WetAreaCeilingArea), input.WetAreaCeilingArea, "Islak hacim tavan alanı");
        NonNegative(errors, nameof(input.ExteriorWallLength), input.ExteriorWallLength, "Dış duvar uzunluğu");
        NonNegative(errors, nameof(input.InteriorWallLength), input.InteriorWallLength, "İç duvar uzunluğu");
        NonNegative(errors, nameof(input.CeilingArea), input.CeilingArea, "Tavan alanı");
        NonNegative(errors, nameof(input.PurlinCount3000), input.PurlinCount3000, "Aşık adedi");
        NonNegative(errors, nameof(input.OmegaCount2500), input.OmegaCount2500, "Omega adedi");

        if (input.BuildingArea <= 0m)
        {
            errors.Add(new(nameof(input.BuildingArea), "Bina alanı sıfırdan büyük olmalıdır."));
        }
        if (input.FloorCount is < 1 or > 2)
        {
            errors.Add(new(nameof(input.FloorCount), "Legacy formül sürümünde kat adedi 1 veya 2 olmalıdır."));
        }
        if (!SupportedSlopes.Contains(input.RoofSlope))
        {
            errors.Add(new(nameof(input.RoofSlope), "Çatı eğimi tanımlı katsayılardan biri olmalıdır."));
        }
        if (input.Doors.Any(item => item.Quantity < 0) || input.Windows.Any(item => item.Quantity < 0) ||
            input.Fixtures.Any(item => item.GroundFloorQuantity < 0 || item.FirstFloorQuantity < 0))
        {
            errors.Add(new("Quantities", "Adet değerleri negatif olamaz."));
        }
        if (input.Surfaces.Any(item => item.LengthOrArea < 0m) || input.SpecialAccessories.Values.Any(value => value < 0m))
        {
            errors.Add(new("SurfaceOrAccessories", "Yüzey ve aksesuar değerleri negatif olamaz."));
        }

        return errors;
    }

    private static void NonNegative(ICollection<ValidationError> errors, string property, decimal value, string label)
    {
        if (value < 0m)
        {
            errors.Add(new(property, $"{label} negatif olamaz."));
        }
    }
}

public static class PricingParametersValidator
{
    public static IReadOnlyList<ValidationError> Validate(PricingParameters pricing)
    {
        var errors = new List<ValidationError>();
        CheckNonNegative(errors, nameof(pricing.ExchangeRate), pricing.ExchangeRate, "USD/TL");
        CheckNonNegative(errors, nameof(pricing.SteelPrice), pricing.SteelPrice, "Çelik fiyatı");
        CheckNonNegative(errors, nameof(pricing.SSeriesPrice), pricing.SSeriesPrice, "S Seri fiyatı");
        CheckNonNegative(errors, nameof(pricing.GalvanizedPrice), pricing.GalvanizedPrice, "Galvaniz fiyatı");
        CheckNonNegative(errors, nameof(pricing.PaintedSheetPrice), pricing.PaintedSheetPrice, "Boyalı sac fiyatı");
        CheckNonNegative(errors, nameof(pricing.SalesMarkupFactor), pricing.SalesMarkupFactor, "Satış katsayısı");
        CheckRate(errors, nameof(pricing.PurchaseVatRate), pricing.PurchaseVatRate, "Alış KDV oranı");
        CheckRate(errors, nameof(pricing.SalesVatRate), pricing.SalesVatRate, "Satış KDV oranı");
        CheckRate(errors, nameof(pricing.DiscountRate), pricing.DiscountRate, "İskonto oranı");
        return errors;
    }

    private static void CheckNonNegative(ICollection<ValidationError> errors, string property, decimal value, string label)
    {
        if (value < 0m)
        {
            errors.Add(new(property, $"{label} negatif olamaz."));
        }
    }

    private static void CheckRate(ICollection<ValidationError> errors, string property, decimal value, string label)
    {
        if (value is < 0m or > 1m)
        {
            errors.Add(new(property, $"{label} 0 ile 1 arasında olmalıdır."));
        }
    }
}
