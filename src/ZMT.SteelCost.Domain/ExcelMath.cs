namespace ZMT.SteelCost.Domain;

public static class ExcelMath
{
    public static decimal RoundUp(decimal value, int digits)
    {
        if (value == 0m)
        {
            return 0m;
        }

        var factor = Pow10(Math.Abs(digits));
        if (digits >= 0)
        {
            var scaled = value * factor;
            return (value > 0m ? decimal.Ceiling(scaled) : decimal.Floor(scaled)) / factor;
        }

        var reduced = value / factor;
        return (value > 0m ? decimal.Ceiling(reduced) : decimal.Floor(reduced)) * factor;
    }

    private static decimal Pow10(int exponent)
    {
        var result = 1m;
        for (var index = 0; index < exponent; index++)
        {
            result *= 10m;
        }

        return result;
    }
}

public static class DecimalMath
{
    public static decimal SafeDivide(decimal numerator, decimal denominator) =>
        denominator == 0m ? 0m : numerator / denominator;
}
