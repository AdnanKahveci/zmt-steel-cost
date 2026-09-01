# Mimari Genel Bakış

## Katman Diyagramı

```
┌─────────────────────────────────────────────────────────┐
│  Views (XAML)                                           │
│  MainWindow, OfferInfoView, WorksScopeView, ...         │
└───────────────────────────┬─────────────────────────────┘
                            │ DataBinding
┌───────────────────────────▼─────────────────────────────┐
│  ViewModels (MVVM)                                      │
│  MainViewModel → alt ViewModel'ler                        │
└───────────────────────────┬─────────────────────────────┘
                            │ BuildDocument()
┌───────────────────────────▼─────────────────────────────┐
│  Models                                                 │
│  OfferDocument, OfferInfo, OfferItem, ...               │
└───────────────────────────┬─────────────────────────────┘
                            │
        ┌───────────────────┼───────────────────┐
        ▼                   ▼                   ▼
 OfferDocumentLocalizer  ValidationService   PdfExportService
 (çeviri)                (doğrulama)         (PDF üretimi)
        │                                       │
 OfferContentTranslations              PdfLocalization
 TechnicalSpecTranslations             WindowsFontResolver
 TechnicalSpecContent / English        CurrencyFormatService
```

## Ana Veri Modeli: `OfferDocument`

Tüm teklif verisi tek bir nesnede toplanır:

```csharp
public sealed class OfferDocument
{
    public OfferInfo Info { get; set; }                          // Kapak bilgileri
    public CompanySettings CompanySettings { get; set; }         // Firma / logo / filigran
    public ObservableCollection<OfferSectionGroup> IncludedWorkGroups { get; set; }
    public ObservableCollection<OfferBulletItem> ExcludedWorks { get; set; }
    public ObservableCollection<OfferItem> OfferItems { get; set; }
    public string OfferTitle { get; set; }                       // C bölümü başlığı
    public string OfferNotes { get; set; }                       // Notlar (KDV vb.)
    public ObservableCollection<OfferBulletItem> PaymentItems { get; set; }
    public string DeliveryText { get; set; }                     // E bölümü
    public ObservableCollection<OfferImage> Images { get; set; }
    public TechnicalSpecification TechnicalSpecification { get; set; }
}
```

## ViewModel → Document Eşlemesi

`MainViewModel.BuildDocument()` metodu tüm alt ViewModel'leri birleştirir:

| ViewModel | OfferDocument alanı |
|-----------|---------------------|
| `OfferInfoViewModel` | `Info` |
| `CompanySettingsViewModel` | `CompanySettings` |
| `WorksScopeViewModel` | `IncludedWorkGroups`, `ExcludedWorks` |
| `OfferItemsViewModel` | `OfferItems`, `OfferTitle`, `OfferNotes` |
| `PaymentDeliveryViewModel` | `PaymentItems`, `DeliveryText` |
| `ImagesViewModel` | `Images` |
| `TechnicalSpecViewModel` | `TechnicalSpecification` |

## PDF Sayfa Sırası

`PdfExportService.ExportAsync()` şu sırayla sayfa üretir:

1. **Teklif görselleri** (varsa, `ImageSections.Offer`)
2. **Sayfa A** — Kapak bilgileri + Dahil işler (A bölümü)
3. **Sayfa B** — Hariç işler (B bölümü)
4. **Sayfa C+D+E** — Teklif tablosu + Ödeme + Teslim
5. **Teknik şartname görselleri** (varsa)
6. **Teknik şartname metin sayfaları** (varsa)

## PDF Bölüm Kodları

| Kod | Türkçe | İngilizce (PdfLocalization) |
|-----|--------|------------------------------|
| A | FİYATIMIZA DAHİL OLAN İŞLER | WORKS INCLUDED IN OUR PRICE |
| B | FİYATIMIZA DAHİL OLMAYAN İŞLER | WORKS NOT INCLUDED IN OUR PRICE |
| C | TEKLİF | QUOTATION |
| D | ÖDEME ŞEKLİ | PAYMENT TERMS |
| E | TESLİM SÜRESİ | DELIVERY PERIOD |

## Teknik Şartname İçerik Formatı

Teknik şartname metinleri özel bir satır formatı kullanır (`TechnicalSpecContentParser`):

| İşaret / format | Anlam |
|-----------------|-------|
| `@table` / `@endtable` | Tablo bloğu |
| `@disclaimer` | Yasal uyarı satırı |
| `Etiket\tDeğer` | Etiket-değer satırı |
| `**Başlık**` | Alt başlık |
| `NOT ...` | Not satırı |
| Düz metin | Paragraf |

Varsayılan içerik kaynakları:
- Türkçe: `Services/TechnicalSpecContent.cs`
- İngilizce: `Services/TechnicalSpecContentEnglish.cs`

## Bağımlılıklar

### NuGet

```xml
<PackageReference Include="PDFsharp" Version="6.2.4" />
```

### Proje ayarları (WPF için)

```xml
<TargetFramework>net8.0-windows</TargetFramework>
<UseWPF>true</UseWPF>
```

### Statik dosyalar

```
Assets/logo.png
Templates/DefaultCompanySettings.json
Templates/DefaultIncludedWorks.json
Templates/DefaultExcludedWorks.json
Templates/DefaultTechnicalSpecification.json
```

## Uygulama Başlangıç Noktası

`App.xaml.cs` içinde iki kritik çağrı vardır:

```csharp
WindowsFontResolver.EnsureRegistered();      // PDF font — zorunlu
AppPathResolver.EnsureBundledContentAvailable(); // Logo / şablon — WPF için
```

WPF olmayan projelerde `AppPathResolver` yerine dosya yollarını doğrudan verebilirsiniz.

## MVVM Altyapısı

| Dosya | Rol |
|-------|-----|
| `ViewModels/ViewModelBase.cs` | `INotifyPropertyChanged` tabanı |
| `ViewModels/RelayCommand.cs` | `ICommand` implementasyonu |
| `Converters/FilePathToImageSourceConverter.cs` | Görsel önizleme |

Başka bir MVVM framework'ü (CommunityToolkit, Prism vb.) kullanıyorsanız bu iki dosyayı değiştirebilir veya kaldırabilirsiniz; PDF çekirdeği bunlara bağımlı değildir.
