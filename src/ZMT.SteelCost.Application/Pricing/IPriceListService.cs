using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Application.Pricing;

public interface IPriceListService
{
    Task<PriceListVersion> GetActiveVersionAsync(CancellationToken cancellationToken = default);

    Task<PriceListVersion> CreateVersionAsync(
        PricingParameters parameters,
        IReadOnlyDictionary<string, decimal> materialPriceOverrides,
        CancellationToken cancellationToken = default);
}
