using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Tests;

public sealed class ExcelMathTests
{
    [Theory]
    [InlineData("1.01", 0, "2")]
    [InlineData("-1.01", 0, "-2")]
    [InlineData("12.341", 2, "12.35")]
    [InlineData("-12.341", 2, "-12.35")]
    [InlineData("121", -1, "130")]
    [InlineData("-121", -1, "-130")]
    public void RoundUp_matches_Excel(string input, int digits, string expected)
    {
        var culture = System.Globalization.CultureInfo.InvariantCulture;
        Assert.Equal(decimal.Parse(expected, culture), ExcelMath.RoundUp(decimal.Parse(input, culture), digits));
    }
}
