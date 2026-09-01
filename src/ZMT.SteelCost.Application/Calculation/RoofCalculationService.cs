using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Application.Calculation;

public interface IRoofCalculationService
{
    RoofCalculationResult Calculate(BuildingInput input);
    IReadOnlyDictionary<decimal, decimal> SlopeFactors { get; }
}

public sealed class RoofCalculationService : IRoofCalculationService
{
    private static readonly IReadOnlyDictionary<decimal, decimal> Factors =
        new Dictionary<decimal, decimal>
        {
            [0.25m] = 1.0308m,
            [0.30m] = 1.0440m,
            [0.35m] = 1.0595m,
            [0.40m] = 1.0770m,
            [0.45m] = 1.0966m,
            [0.50m] = 1.1180m,
            [0.55m] = 1.1413m,
            [0.60m] = 1.1662m
        };

    public IReadOnlyDictionary<decimal, decimal> SlopeFactors => Factors;

    public RoofCalculationResult Calculate(BuildingInput input)
    {
        if (!Factors.TryGetValue(input.RoofSlope, out var factor))
        {
            throw new ArgumentOutOfRangeException(nameof(input.RoofSlope), "Tanımsız çatı eğimi.");
        }

        var eaveArea = input.EaveWidth * (input.EaveLength + input.GableLength);
        var coverArea = (input.RoofFootprintArea + eaveArea) * factor;
        return new(eaveArea, factor, coverArea);
    }
}
