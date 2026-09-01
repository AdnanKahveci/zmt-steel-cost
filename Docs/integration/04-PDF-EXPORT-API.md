# PDF Export API

Programatik PDF üretimi için referans dokümantasyonu.

---

## Temel Arayüz

```csharp
public interface IPdfExportService
{
    Task ExportAsync(
        OfferDocument document,
        PdfExportOptions options,
        CancellationToken cancellationToken = default);
}
```

**Implementasyon:** `Services/PdfExportService.cs`

---

## PdfExportOptions

```csharp
public sealed class PdfExportOptions
{
    public string OutputPath { get; set; }              // Zorunlu — .pdf tam yolu
    public bool OpenAfterExport { get; set; } = true;   // WPF/masaüstü: PDF'i aç
    public bool IncludeImages { get; set; } = true;
    public bool IncludeTechnicalSpecification { get; set; } = true;
    public string DocumentTitle { get; set; }           // PDF metadata başlığı
    public OfferLanguage Language { get; set; }         // Turkish | English
}
```

---

## OfferLanguage

```csharp
public enum OfferLanguage
{
    Turkish,
    English
}
```

---

## Tam Kullanım Örneği

```csharp
using ZmtOfferPdfGenerator.Models;
using ZmtOfferPdfGenerator.Services;

// 1. Font resolver — her PDF oturumunda bir kez
WindowsFontResolver.EnsureRegistered();

// 2. Document oluştur
OfferDocument document = new()
{
    Info = new OfferInfo
    {
        OfferDate = DateTime.Today,
        PreparedBy = "Fatih YALÇINKAYA",
        CompanyName = "LİBYA PROJESİ",
        ReferenceNumber = "T-26-2845",
        JobName = "LİBYA KONUT PROJESİ",
        MainTitle = "170 m² HAFİF ÇELİK KONUT FİYAT TEKLİFİ",
        ProjectTitle = "170 m² HAFİF ÇELİK KONUT FİYAT TEKLİFİ"
    },
    OfferTitle = "170 m² HAFİF ÇELİK BİNA FİYAT TEKLİFİ",
    OfferNotes = "Teklifimiz 1 gün süre ile geçerlidir.\nTeklifimize KDV dahil DEĞİLDİR.",
    DeliveryText = "Avansın alındığı tarih itibarı ile 15 günlük imalat döneminden sonra sevkiyat başlayacaktır. Sahadaki iş programı karşılıklı görüşme ile belirlenecektir.",
    OfferItems = new ObservableCollection<OfferItem>
    {
        new()
        {
            RowNo = 1,
            Description = "HAFİF ÇELİK KONUT H:2800 mm",
            Quantity = 170,
            Unit = "m²",
            UnitPrice = 708.75m,
            Currency = "USD"
        }
    },
    PaymentItems = new ObservableCollection<OfferBulletItem>
    {
        new() { Text = "Karşılıklı görüşme ile belirlenecektir.", SortOrder = 1 }
    }
    // IncludedWorkGroups, ExcludedWorks, TechnicalSpecification, Images, CompanySettings ...
};

// 3. İngilizceye çevir (isteğe bağlı)
OfferLanguage language = OfferLanguage.English;
OfferDocument exportDoc = OfferDocumentLocalizer.Localize(document, language);

// 4. Doğrula (isteğe bağlı)
ValidationService validator = new();
IReadOnlyList<string> errors = validator.Validate(exportDoc);
if (errors.Count > 0)
    throw new InvalidOperationException(string.Join("\n", errors));

// 5. PDF üret
IPdfExportService pdfService = new PdfExportService();
await pdfService.ExportAsync(exportDoc, new PdfExportOptions
{
    OutputPath = @"C:\Temp\Teklif_EN.pdf",
    Language = language,
    DocumentTitle = exportDoc.Info.MainTitle,
    OpenAfterExport = false,
    IncludeImages = true,
    IncludeTechnicalSpecification = true
});
```

---

## Görseller

```csharp
document.Images.Add(new OfferImage
{
    FilePath = @"C:\Images\proje1.jpg",   // Mutlak yol — dosya mevcut olmalı
    Title = "Proje Görseli",               // İsteğe bağlı
    ImageSection = ImageSections.Offer,    // Offer | TechnicalSpec
    PageNumber = 1,
    SortOrder = 1,
    IncludeInPdf = true,
    HasBorder = false,
    FitWithoutCrop = true
});
```

**ImageSections sabitleri** (`Models/OfferImage.cs`):
- `ImageSections.Offer` — Teklif başı görselleri
- `ImageSections.TechnicalSpec` — Teknik şartname önü görselleri

---

## Teknik Şartname

```csharp
// Varsayılan şablonu yükle
TechnicalSpecification spec = new()
{
    Title = "HAFİF ÇELİK BİNA TEKNİK ŞARTNAMESİ",
    IncludeInPdf = true,
    Sections = new ObservableCollection<TechnicalSpecSection>(
        TechnicalSpecContent.DefaultSections.Select(s => new TechnicalSpecSection
        {
            Title = s.Title,
            Content = s.Content.Trim(),
            SortOrder = s.SortOrder,
            IncludeInPdf = true
        }))
};
document.TechnicalSpecification = spec;
```

UI olmadan düzenlemek için `TechnicalSpecContentParser.Parse(content)` ve `Serialize(lines)` kullanın.

---

## CompanySettings (Logo / Filigran)

```csharp
document.CompanySettings = new CompanySettings
{
    CompanyName = "ZMT ÇELİK & PREFABRİK A.Ş",
    Address = "Kirazpınar, 2701. Sk. No:25, 41400 Gebze/Kocaeli",
    Phone = "0262 320 01 12",
    Email = "info@zmtprefabrik.com",
    Website = "https://zmtprefabrik.com/",
    HeaderLogoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "logo.png"),
    WatermarkText = "ZMT ÇELİK",
    WatermarkOpacity = 0.08,
    WatermarkAngle = -28,
    BrandRed = "#B51F29"
};
```

---

## ValidationService

Minimum doğrulama kuralları:

| Alan | Kural |
|------|-------|
| `Info.ReferenceNumber` | Boş olamaz |
| `Info.JobName` | Boş olamaz |
| `OfferItems[].Quantity` | Negatif olamaz |
| `OfferItems[].UnitPrice` | Negatif olamaz |

---

## Para Birimi Formatı

`CurrencyFormatService` PDF tablosunda fiyatları biçimlendirir. Desteklenen: `USD`, `EUR`, `TRY`.

---

## Hata Durumları

| Exception | Neden |
|-----------|-------|
| `InvalidOperationException: PDF çıktı yolu seçilmedi` | `OutputPath` boş |
| Font resolver hatası | `EnsureRegistered()` çağrılmadı |
| Görsel eklenmedi | `FilePath` yok veya `IncludeInPdf = false` |

---

## Publish / Dağıtım

Tek exe dağıtım için mevcut csproj ayarları:

```xml
<SelfContained>true</SelfContained>
<PublishSingleFile>true</PublishSingleFile>
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
```

Publish komutu:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true
```

WPF başlangıcında `AppPathResolver.EnsureBundledContentAvailable()` logo ve şablonları exe yanına çıkarır.
