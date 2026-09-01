using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Application.Offers;

public enum OfferLanguage
{
    Turkish,
    English
}

public sealed class OfferDocument
{
    public OfferInfo Info { get; init; } = new();
    public CompanySettings CompanySettings { get; init; } = new();
    public List<OfferSectionGroup> IncludedWorkGroups { get; init; } = [];
    public List<OfferBulletItem> ExcludedWorks { get; init; } = [];
    public List<OfferItem> OfferItems { get; init; } = [];
    public string OfferTitle { get; set; } = string.Empty;
    public string OfferNotes { get; set; } = string.Empty;
    public List<OfferBulletItem> PaymentItems { get; init; } = [];
    public string DeliveryText { get; set; } = string.Empty;
    public List<OfferImage> Images { get; init; } = [];
    public TechnicalSpecification TechnicalSpecification { get; init; } = new();
    public decimal ScopeTotal { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal VatRate { get; set; }
    public decimal VatAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public string Currency { get; set; } = "TRY";
}

public sealed class OfferInfo
{
    public DateTime OfferDate { get; set; } = DateTime.Today;
    public string PreparedBy { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string AuthorizedPerson { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string JobName { get; set; } = string.Empty;
    public string MainTitle { get; set; } = string.Empty;
    public string ProjectTitle { get; set; } = string.Empty;
}

public sealed class CompanySettings
{
    public string CompanyName { get; set; } = "ZMT ÇELİK & PREFABRİK A.Ş";
    public string Address { get; set; } = "Kirazpınar, 2701. Sk. No:25, 41400 Gebze/Kocaeli";
    public string Phone { get; set; } = "0262 320 01 12";
    public string Email { get; set; } = "info@zmtprefabrik.com";
    public string Website { get; set; } = "https://zmtprefabrik.com/";
    public string? HeaderLogoPath { get; set; }
    public string? FooterLogo1Path { get; set; }
    public string? FooterLogo2Path { get; set; }
    public string WatermarkText { get; set; } = "ZMT ÇELİK";
    public double WatermarkOpacity { get; set; } = 0.08;
    public double WatermarkAngle { get; set; } = -28;
    public string BrandRed { get; set; } = "#B51F29";
    public string BrandGray { get; set; } = "#EFEFEF";
    public string BrandBlack { get; set; } = "#222222";
    public string DefaultOfferTitle { get; set; } = "HAFİF ÇELİK FİYAT TEKLİFİ";
}

public sealed class OfferSectionGroup
{
    public int SortOrder { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public List<OfferBulletItem> Items { get; init; } = [];
}

public sealed class OfferBulletItem
{
    public int SortOrder { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsIncludedInPdf { get; set; } = true;
}

public sealed class OfferItem
{
    public int RowNo { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public string Currency { get; set; } = "TRY";
    public decimal Total => Quantity * UnitPrice;
}

public static class ImageSections
{
    public const string Offer = "Offer";
    public const string TechnicalSpec = "TechnicalSpec";
}

public sealed class OfferImage
{
    public string FilePath { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageSection { get; set; } = ImageSections.Offer;
    public int PageNumber { get; set; }
    public int SortOrder { get; set; }
    public bool IncludeInPdf { get; set; } = true;
    public bool HasBorder { get; set; }
    public bool FitWithoutCrop { get; set; } = true;
    public string LayoutMode { get; set; } = "2 görsel / sayfa";
}

public sealed class TechnicalSpecification
{
    public string Title { get; set; } = "HAFİF ÇELİK BİNA TEKNİK ŞARTNAMESİ";
    public bool IncludeInPdf { get; set; } = true;
    public List<TechnicalSpecSection> Sections { get; init; } = [];
}

public sealed class TechnicalSpecSection
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IncludeInPdf { get; set; } = true;
}

public sealed class PdfExportOptions
{
    public string OutputPath { get; set; } = string.Empty;
    public bool OpenAfterExport { get; set; } = true;
    public bool IncludeImages { get; set; } = true;
    public bool IncludeTechnicalSpecification { get; set; } = true;
    public string DocumentTitle { get; set; } = string.Empty;
    public OfferLanguage Language { get; set; }
}

public sealed class OfferGenerationOptions
{
    public int ValidityDays { get; set; } = 7;
    public int DeliveryDays { get; set; } = 15;
    public string PaymentTerms { get; set; } = "Karşılıklı görüşme ile belirlenecektir.";
    public string AdditionalNotes { get; set; } = string.Empty;
    public bool IncludeTechnicalSpecification { get; set; } = true;
}

public interface IOfferDocumentMapper
{
    OfferDocument Map(Project project, CalculationResult result, OfferGenerationOptions options);
}

public interface IOfferPdfExportService
{
    Task ExportAsync(OfferDocument document, PdfExportOptions options, CancellationToken cancellationToken = default);
}
