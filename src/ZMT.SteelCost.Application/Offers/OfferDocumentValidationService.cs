namespace ZMT.SteelCost.Application.Offers;

public sealed class OfferDocumentValidationService
{
    public IReadOnlyList<string> Validate(OfferDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(document.Info.ReferenceNumber))
        {
            errors.Add("Teklif referans numarası boş olamaz.");
        }
        if (string.IsNullOrWhiteSpace(document.Info.JobName))
        {
            errors.Add("Teklif iş/proje adı boş olamaz.");
        }
        if (document.OfferItems.Count == 0)
        {
            errors.Add("Teklifte en az bir fiyat kalemi bulunmalıdır.");
        }
        if (document.OfferItems.Any(item => item.Quantity < 0m))
        {
            errors.Add("Teklif miktarı negatif olamaz.");
        }
        if (document.OfferItems.Any(item => item.UnitPrice < 0m))
        {
            errors.Add("Teklif birim fiyatı negatif olamaz.");
        }
        if (document.DiscountRate is < 0m or > 1m || document.VatRate is < 0m or > 1m)
        {
            errors.Add("İskonto ve KDV oranları 0 ile 1 arasında olmalıdır.");
        }
        return errors;
    }
}
