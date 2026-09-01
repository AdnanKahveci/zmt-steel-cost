using System.Globalization;
using System.Text.RegularExpressions;

namespace ZMT.SteelCost.Application.Offers;

public static partial class OfferDocumentLocalizer
{
    private static readonly IReadOnlyDictionary<string, string> Texts = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["Hafif Çelik Panel ve Metal Aksam"] = "Light Gauge Steel Panels and Metal Components",
        ["Alçıpan ve Kaplama"] = "Drywall and Cladding",
        ["Çatı Sacı"] = "Roofing Sheet",
        ["Kapı ve Pencere"] = "Doors and Windows",
        ["Elektrik"] = "Electrical Works",
        ["Vida"] = "Fasteners",
        ["Depo ve Hırdavat"] = "Warehouse and Hardware",
        ["Sıhhi Tesisat"] = "Plumbing",
        ["Çatı Oluğu"] = "Rain Gutter",
        ["Boya ve Mastik"] = "Paint and Sealant",
        ["Karşılıklı görüşme ile belirlenecektir."] = "To be determined by mutual agreement.",
        ["Fiyatlar Türk Lirası (TRY) olarak düzenlenmiştir."] = "Prices are stated in Turkish Lira (TRY).",
        ["GENEL BİNA BİLGİLERİ"] = "GENERAL BUILDING INFORMATION",
        ["ÇATI SİSTEMİ"] = "ROOF SYSTEM",
        ["DUVAR VE KAPLAMA"] = "WALLS AND CLADDING",
        ["KAPI VE PENCERELER"] = "DOORS AND WINDOWS",
        ["TEKLİF KAPSAMI"] = "QUOTATION SCOPE",
        ["HAFİF ÇELİK BİNA TEKNİK ŞARTNAMESİ"] = "LIGHT GAUGE STEEL BUILDING TECHNICAL SPECIFICATION",
        ["Bina Alanı"] = "Building Area",
        ["Kat Adedi"] = "Number of Floors",
        ["Kat Yüksekliği"] = "Floor Height",
        ["Hesaplanan Çelik"] = "Calculated Steel",
        ["Formül Sürümü"] = "Formula Version",
        ["Çatı Tipi"] = "Roof Type",
        ["Çatı Sistemi"] = "Roof System",
        ["Kaplama Tipi"] = "Roof Covering",
        ["Çatı Eğimi"] = "Roof Slope",
        ["Çatı Oturum Alanı"] = "Roof Footprint Area",
        ["Dış Duvar"] = "Exterior Wall",
        ["İç Duvar"] = "Interior Wall",
        ["Çatı"] = "Roof",
        ["Tavan"] = "Ceiling",
        ["Kapı / Pencere"] = "Door / Window",
        ["Seçilmedi"] = "Not selected",
        ["ZMT'ye Ait"] = "By ZMT",
        ["Müşteriye Ait"] = "By Customer"
    };

    public static OfferDocument Localize(OfferDocument source, OfferLanguage language)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (language == OfferLanguage.Turkish)
        {
            return Clone(source, false);
        }
        return Clone(source, true);
    }

    private static OfferDocument Clone(OfferDocument source, bool english)
    {
        var document = new OfferDocument
        {
            Info = new OfferInfo
            {
                OfferDate = source.Info.OfferDate,
                PreparedBy = source.Info.PreparedBy,
                CompanyName = source.Info.CompanyName,
                AuthorizedPerson = source.Info.AuthorizedPerson,
                ContactInfo = source.Info.ContactInfo,
                ReferenceNumber = source.Info.ReferenceNumber,
                JobName = english ? TranslateTitle(source.Info.JobName) : source.Info.JobName,
                MainTitle = english ? TranslateTitle(source.Info.MainTitle) : source.Info.MainTitle,
                ProjectTitle = english ? TranslateTitle(source.Info.ProjectTitle) : source.Info.ProjectTitle
            },
            CompanySettings = new CompanySettings
            {
                CompanyName = source.CompanySettings.CompanyName,
                Address = source.CompanySettings.Address,
                Phone = source.CompanySettings.Phone,
                Email = source.CompanySettings.Email,
                Website = source.CompanySettings.Website,
                HeaderLogoPath = source.CompanySettings.HeaderLogoPath,
                FooterLogo1Path = source.CompanySettings.FooterLogo1Path,
                FooterLogo2Path = source.CompanySettings.FooterLogo2Path,
                WatermarkText = source.CompanySettings.WatermarkText,
                WatermarkOpacity = source.CompanySettings.WatermarkOpacity,
                WatermarkAngle = source.CompanySettings.WatermarkAngle,
                BrandRed = source.CompanySettings.BrandRed,
                BrandGray = source.CompanySettings.BrandGray,
                BrandBlack = source.CompanySettings.BrandBlack,
                DefaultOfferTitle = source.CompanySettings.DefaultOfferTitle
            },
            OfferTitle = english ? TranslateTitle(source.OfferTitle) : source.OfferTitle,
            OfferNotes = english ? TranslateNotes(source.OfferNotes) : source.OfferNotes,
            DeliveryText = english ? TranslateDelivery(source.DeliveryText) : source.DeliveryText,
            ScopeTotal = source.ScopeTotal,
            DiscountRate = source.DiscountRate,
            DiscountAmount = source.DiscountAmount,
            Subtotal = source.Subtotal,
            VatRate = source.VatRate,
            VatAmount = source.VatAmount,
            GrandTotal = source.GrandTotal,
            Currency = source.Currency,
            TechnicalSpecification = new TechnicalSpecification
            {
                Title = english ? Translate(source.TechnicalSpecification.Title) : source.TechnicalSpecification.Title,
                IncludeInPdf = source.TechnicalSpecification.IncludeInPdf,
                Sections = source.TechnicalSpecification.Sections.Select(section => new TechnicalSpecSection
                {
                    Title = english ? Translate(section.Title) : section.Title,
                    Content = english ? TranslateTechnicalContent(section.Content) : section.Content,
                    SortOrder = section.SortOrder,
                    IncludeInPdf = section.IncludeInPdf
                }).ToList()
            }
        };

        document.IncludedWorkGroups.AddRange(source.IncludedWorkGroups.Select(group => new OfferSectionGroup
        {
            SortOrder = group.SortOrder,
            Title = english ? Translate(group.Title) : group.Title,
            IsVisible = group.IsVisible,
            Items = group.Items.Select(item => new OfferBulletItem
            {
                SortOrder = item.SortOrder,
                Text = english ? TranslateIncludedItem(item.Text) : item.Text,
                IsIncludedInPdf = item.IsIncludedInPdf
            }).ToList()
        }));
        document.ExcludedWorks.AddRange(source.ExcludedWorks.Select(item => new OfferBulletItem
        {
            SortOrder = item.SortOrder,
            Text = english ? TranslateExcludedItem(item.Text) : item.Text,
            IsIncludedInPdf = item.IsIncludedInPdf
        }));
        document.OfferItems.AddRange(source.OfferItems.Select(item => new OfferItem
        {
            RowNo = item.RowNo,
            Description = english ? Translate(item.Description) : item.Description,
            Quantity = item.Quantity,
            Unit = english && item.Unit == "grup" ? "group" : item.Unit,
            UnitPrice = item.UnitPrice,
            Currency = item.Currency
        }));
        document.PaymentItems.AddRange(source.PaymentItems.Select(item => new OfferBulletItem
        {
            SortOrder = item.SortOrder,
            Text = english ? Translate(item.Text) : item.Text,
            IsIncludedInPdf = item.IsIncludedInPdf
        }));
        document.Images.AddRange(source.Images.Select(item => new OfferImage
        {
            FilePath = item.FilePath,
            Title = item.Title,
            Description = item.Description,
            ImageSection = item.ImageSection,
            PageNumber = item.PageNumber,
            SortOrder = item.SortOrder,
            IncludeInPdf = item.IncludeInPdf,
            HasBorder = item.HasBorder,
            FitWithoutCrop = item.FitWithoutCrop,
            LayoutMode = item.LayoutMode
        }));
        return document;
    }

    private static string Translate(string value) => Texts.GetValueOrDefault(value, value);

    private static string TranslateTitle(string value) => value
        .Replace("HAFİF ÇELİK BİNA FİYAT TEKLİFİ", "LIGHT GAUGE STEEL BUILDING QUOTATION", StringComparison.Ordinal)
        .Replace("Hafif Çelik Bina", "Light Gauge Steel Building", StringComparison.Ordinal);

    private static string TranslateIncludedItem(string value)
    {
        var match = IncludedItemPattern().Match(value);
        return match.Success
            ? $"The {match.Groups[1].Value} material and application items calculated for the project are included in ZMT's scope."
            : Translate(value);
    }

    private static string TranslateExcludedItem(string value)
    {
        const string suffix = ": Müşteri tarafından temin edilecek veya yaptırılacaktır.";
        return value.EndsWith(suffix, StringComparison.Ordinal)
            ? $"{Translate(value[..^suffix.Length])}: To be supplied or performed by the customer."
            : Translate(value);
    }

    private static string TranslateNotes(string notes) => string.Join(Environment.NewLine,
        notes.Split(["\r\n", "\n"], StringSplitOptions.None).Select(line =>
        {
            var validity = ValidityPattern().Match(line);
            if (validity.Success)
            {
                return $"Our quotation is valid for {validity.Groups[1].Value} days.";
            }
            if (line == "Teklifimize KDV dahil DEĞİLDİR.")
            {
                return "VAT is NOT included in our quotation.";
            }
            var vat = VatPattern().Match(line);
            if (vat.Success)
            {
                return $"VAT at {vat.Groups[1].Value}% is included in our quotation.";
            }
            return Translate(line);
        }));

    private static string TranslateDelivery(string value)
    {
        var match = DeliveryPattern().Match(value);
        return match.Success
            ? $"Shipment will commence after a {match.Groups[1].Value}-day manufacturing period following receipt of the advance payment. The on-site work schedule will be determined by mutual agreement."
            : value;
    }

    private static string TranslateTechnicalContent(string content) => string.Join(Environment.NewLine,
        content.Split(["\r\n", "\n"], StringSplitOptions.None).Select(line =>
        {
            var parts = line.Split('\t', 2);
            if (parts.Length != 2)
            {
                return TranslateTechnicalText(line);
            }
            return $"{Translate(parts[0])}\t{TranslateTechnicalText(Translate(parts[1]))}";
        }));

    private static string TranslateTechnicalText(string value) => value
        .Replace("Kırma", "Hip", StringComparison.Ordinal)
        .Replace("Beşik", "Gable", StringComparison.Ordinal)
        .Replace("Tek Eğim", "Mono-pitch", StringComparison.Ordinal)
        .Replace("Aşık Omega", "Purlin Omega", StringComparison.Ordinal)
        .Replace("Panel Sistem", "Panel System", StringComparison.Ordinal)
        .Replace("Trapez Çatı", "Trapezoidal Roofing", StringComparison.Ordinal)
        .Replace("Sandviç Panel", "Sandwich Panel", StringComparison.Ordinal)
        .Replace("Metal Kiremit Çatı", "Metal Tile Roofing", StringComparison.Ordinal)
        .Replace("PVC Sürgülü Pencere", "PVC Sliding Window", StringComparison.Ordinal)
        .Replace("PVC Vasistas", "PVC Transom Window", StringComparison.Ordinal)
        .Replace("PVC Pencere", "PVC Window", StringComparison.Ordinal)
        .Replace("Çelik Kapı", "Steel Door", StringComparison.Ordinal)
        .Replace("PVC Kapı", "PVC Door", StringComparison.Ordinal)
        .Replace("Duble", "Double", StringComparison.Ordinal)
        .Replace("Melamin Kapı", "Melamine Door", StringComparison.Ordinal)
        .Replace("Ahşap Kasalı Amerikan Kapı", "Wood-framed American Door", StringComparison.Ordinal)
        .Replace("Beyaz", "White", StringComparison.Ordinal)
        .Replace("Antrasit", "Anthracite", StringComparison.Ordinal)
        .Replace("Altınmeşe", "Golden Oak", StringComparison.Ordinal)
        .Replace("Alçıpan", "Drywall", StringComparison.Ordinal)
        .Replace("Nem Bariyeri", "Moisture Barrier", StringComparison.Ordinal)
        .Replace("Arduazlı Membran", "Slate Membrane", StringComparison.Ordinal)
        .Replace("Membran", "Membrane", StringComparison.Ordinal)
        .Replace("adet", "pcs", StringComparison.Ordinal);

    [GeneratedRegex(@"Proje hesabında yer alan (\d+) malzeme ve uygulama kalemi ZMT kapsamındadır\.", RegexOptions.CultureInvariant)]
    private static partial Regex IncludedItemPattern();

    [GeneratedRegex(@"Teklifimiz (\d+) gün süre ile geçerlidir\.", RegexOptions.CultureInvariant)]
    private static partial Regex ValidityPattern();

    [GeneratedRegex(@"Teklifimize KDV %(\d+(?:[.,]\d+)?) dahildir\.", RegexOptions.CultureInvariant)]
    private static partial Regex VatPattern();

    [GeneratedRegex(@"Avansın alındığı tarih itibarı ile (\d+) günlük imalat döneminden sonra", RegexOptions.CultureInvariant)]
    private static partial Regex DeliveryPattern();
}

public sealed class PdfLocalization
{
    private PdfLocalization(OfferLanguage language)
    {
        if (language == OfferLanguage.English)
        {
            IncludedWorksTitle = "WORKS INCLUDED IN OUR PRICE";
            ExcludedWorksTitle = "WORKS NOT INCLUDED IN OUR PRICE";
            QuotationTitle = "QUOTATION";
            PaymentTitle = "PAYMENT TERMS";
            DeliveryTitle = "DELIVERY PERIOD";
            DateLabel = "Date";
            PreparedByLabel = "Prepared By";
            CompanyLabel = "Company";
            AuthorizedPersonLabel = "Authorized Person";
            ReferenceLabel = "Reference";
            JobLabel = "Job";
            ContactLabel = "Contact";
            DescriptionLabel = "Description";
            QuantityLabel = "Quantity";
            UnitLabel = "Unit";
            UnitPriceLabel = "Unit Price";
            TotalLabel = "Total";
            ScopeTotalLabel = "Scope Total";
            DiscountLabel = "Discount";
            SubtotalLabel = "Subtotal";
            VatLabel = "VAT";
            GrandTotalLabel = "GRAND TOTAL";
            NotesLabel = "Notes";
            TechnicalSpecificationTitle = "TECHNICAL SPECIFICATION";
            TechnicalSpecificationImagesTitle = "TECHNICAL SPECIFICATION IMAGES";
            PageLabel = "Page";
        }
    }

    public string IncludedWorksTitle { get; } = "FİYATIMIZA DAHİL OLAN İŞLER";
    public string ExcludedWorksTitle { get; } = "FİYATIMIZA DAHİL OLMAYAN İŞLER";
    public string QuotationTitle { get; } = "TEKLİF";
    public string PaymentTitle { get; } = "ÖDEME ŞEKLİ";
    public string DeliveryTitle { get; } = "TESLİM SÜRESİ";
    public string DateLabel { get; } = "Tarih";
    public string PreparedByLabel { get; } = "Hazırlayan";
    public string CompanyLabel { get; } = "Firma";
    public string AuthorizedPersonLabel { get; } = "Yetkili";
    public string ReferenceLabel { get; } = "Referans";
    public string JobLabel { get; } = "İş";
    public string ContactLabel { get; } = "İletişim";
    public string DescriptionLabel { get; } = "Açıklama";
    public string QuantityLabel { get; } = "Miktar";
    public string UnitLabel { get; } = "Birim";
    public string UnitPriceLabel { get; } = "Birim Fiyat";
    public string TotalLabel { get; } = "Tutar";
    public string ScopeTotalLabel { get; } = "Kapsam Toplamı";
    public string DiscountLabel { get; } = "İskonto";
    public string SubtotalLabel { get; } = "Ara Toplam";
    public string VatLabel { get; } = "KDV";
    public string GrandTotalLabel { get; } = "GENEL TOPLAM";
    public string NotesLabel { get; } = "Notlar";
    public string TechnicalSpecificationTitle { get; } = "TEKNİK ŞARTNAME";
    public string TechnicalSpecificationImagesTitle { get; } = "TEKNİK ŞARTNAME GÖRSELLERİ";
    public string PageLabel { get; } = "Sayfa";

    public static PdfLocalization For(OfferLanguage language) => new(language);
}
