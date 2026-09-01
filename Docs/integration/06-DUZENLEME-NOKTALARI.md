# Düzenleme Noktaları

Birleştirme sonrası özelleştirmeniz gereken yerler.

---

## 1. Namespace Değişikliği

Mevcut namespace: `ZmtOfferPdfGenerator`

**Değiştirilecek yerler:**
- Tüm `.cs` dosyaları → `namespace` satırı
- Tüm `.xaml` dosyaları → `x:Class`, `clr-namespace:` referansları
- `AssemblyName` / `RootNamespace` → `.csproj`

**PowerShell ile toplu değiştirme (dikkatli kullanın):**

```powershell
$old = "ZmtOfferPdfGenerator"
$new = "HedefProje.Teklif"
Get-ChildItem -Recurse -Include *.cs,*.xaml | ForEach-Object {
    (Get-Content $_.FullName -Raw) -replace $old, $new | Set-Content $_.FullName
}
```

---

## 2. Marka / Firma Varsayılanları

| Dosya | Alan | Mevcut değer |
|-------|------|--------------|
| `Models/CompanySettings.cs` | Tüm varsayılanlar | ZMT ÇELİK & PREFABRİK A.Ş |
| `Templates/DefaultCompanySettings.json` | JSON varsayılanlar | Aynı |
| `ViewModels/OfferInfoViewModel.cs` | `DefaultInfo` | LİBYA PROJESİ örneği |
| `Services/PdfLocalization.cs` | `DisclaimerPrefix` | ZMT ÇELİK VE PREFABRİK |
| `Services/TechnicalSpecContent.cs` | Disclaimer satırı | ZMT ÇELİK VE PREFABRİK... |
| `App.xaml` | Renk paleti | `#C8202F` kırmızı |

---

## 3. Varsayılan Teklif Metinleri

Şablon metinleri birden fazla yerde tanımlı — **hepsini senkron tutun:**

| Kaynak | İçerik |
|--------|--------|
| `ViewModels/WorksScopeViewModel.cs` | Dahil / hariç işler |
| `ViewModels/OfferItemsViewModel.cs` | Başlık, notlar, kalemler |
| `ViewModels/PaymentDeliveryViewModel.cs` | Ödeme, teslim |
| `Templates/DefaultIncludedWorks.json` | JSON yedek |
| `Templates/DefaultExcludedWorks.json` | JSON yedek |
| `Services/OfferContentTranslations.cs` | EN çevirileri |

**Yeni madde eklerken:** Türkçe metni hem ViewModel/JSON'a hem `OfferContentTranslations`'a ekleyin.

---

## 4. Teknik Şartname

| Dosya | Rol |
|-------|-----|
| `Services/TechnicalSpecContent.cs` | TR varsayılan (kaynak) |
| `Services/TechnicalSpecContentEnglish.cs` | EN varsayılan (kaynak) |
| `TechnicalSpecTranslations.cs` | Otomatik — elle düzenlemeyin |

Bölüm eklemek/değiştirmek için TR ve EN dosyalarını **aynı `SortOrder`** ile güncelleyin.

---

## 5. PDF Görsel Düzeni

`Services/PdfExportService.cs` içinde sabitler:

```csharp
private const double PageWidth = 595;    // A4
private const double PageHeight = 842;
private const double Left = 58;
private const double ContentTop = 132;
private const double FooterY = 800;
```

Logo boyutu, kenar boşlukları, tablo genişlikleri burada ayarlanır.

---

## 6. Renkler

PDF renkleri `PdfExportService` constructor alanında:

```csharp
private readonly XColor brandRed = XColor.FromArgb(200, 32, 47);
```

UI renkleri `App.xaml` → `BrandRedColor`, `SidebarColor` vb.

`CompanySettings.BrandRed` PDF'de henüz dinamik kullanılmıyor; sabit değerler geçerli.

---

## 7. Dil Seçimi Konumu

Şu an dil seçimi yalnızca `PdfPreviewViewModel.SelectedLanguage` içinde.

Başka projede üst menüye taşımak için:

1. `SelectedLanguage`'i `MainViewModel`'e veya paylaşılan servise taşıyın
2. `BuildDocument()` ve `PdfExportOptions.Language` bu değeri kullansın

---

## 8. DI (Dependency Injection) Entegrasyonu

Mevcut kod `new` ile servis oluşturuyor:

```csharp
// MainViewModel.cs
private readonly IPdfExportService pdfExportService = new PdfExportService();
```

Hedef proje DI kullanıyorsa:

```csharp
// Startup / App.xaml.cs
services.AddSingleton<IPdfExportService, PdfExportService>();
services.AddSingleton<ValidationService>();
services.AddTransient<MainViewModel>();
```

---

## 9. Veritabanı / API Bağlantısı

Mevcut uygulama veriyi bellekte tutar; kalıcılık yok.

Entegrasyon noktası: `MainViewModel.BuildDocument()` — buradan DB/API'den gelen veriyi `OfferDocument`'a map edin.

Önerilen yapı:

```
DB/API → MyQuotationDto → Mapper → OfferDocument → Localize → PdfExport
```

---

## 10. Publish Ayarları

Mevcut single-file publish ayarları `.csproj`'da:

```xml
<SelfContained>true</SelfContained>
<PublishSingleFile>true</PublishSingleFile>
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
```

Hedef proje farklı RID kullanıyorsa (`win-arm64` vb.) font resolver'ın Arial bulabildiğini doğrulayın.

---

## Özelleştirme Öncelik Sırası

1. Namespace + assembly adı
2. `CompanySettings` / logo / filigran
3. Varsayılan teklif metinleri + çeviri sözlüğü
4. Teknik şartname içeriği
5. PDF layout sabitleri
6. UI renkleri / stiller
7. DI / veri kaynağı entegrasyonu
