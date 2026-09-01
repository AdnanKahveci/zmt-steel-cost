using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Tests;

public sealed class RoofCalculationTests
{
    [Theory]
    [InlineData("0.25", "1.0308")]
    [InlineData("0.30", "1.0440")]
    [InlineData("0.35", "1.0595")]
    [InlineData("0.40", "1.0770")]
    [InlineData("0.45", "1.0966")]
    [InlineData("0.50", "1.1180")]
    [InlineData("0.55", "1.1413")]
    [InlineData("0.60", "1.1662")]
    public void All_legacy_slope_factors_are_supported(string slopeText, string factorText)
    {
        var service = new RoofCalculationService();
        var input = BuildingInput.CreateLegacySample();
        input.RoofSlope = decimal.Parse(slopeText, System.Globalization.CultureInfo.InvariantCulture);

        var result = service.Calculate(input);

        Assert.Equal(decimal.Parse(factorText, System.Globalization.CultureInfo.InvariantCulture), result.SlopeFactor);
        Assert.Equal(input.EaveWidth * (input.EaveLength + input.GableLength), result.EaveArea);
        Assert.Equal((input.RoofFootprintArea + result.EaveArea) * result.SlopeFactor, result.RoofCoverArea);
    }
}
