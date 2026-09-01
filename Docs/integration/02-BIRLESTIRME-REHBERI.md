# Birleştirme Rehberi

Bu doküman, ZmtOfferPdfGenerator'ı başka bir projeye taşımak için adım adım yönergeler içerir.

---

## Senaryo A — WPF Uygulamasına Tam Entegrasyon

**Ne zaman:** Hedef proje zaten WPF (.NET 8 Windows) ise ve teklif editör arayüzünü de istiyorsanız.

### Adımlar

1. **Dosyaları kopyalayın** — [05-DOSYA-LISTESI.md](./05-DOSYA-LISTESI.md) listesindeki tüm kaynak dosyaları hedef projeye taşıyın.

2. **Namespace değiştirin** — `ZmtOfferPdfGenerator` → hedef projenizin namespace'i.  
   Detay: [06-DUZENLEME-NOKTALARI.md](./06-DUZENLEME-NOKTALARI.md)

3. **csproj güncelleyin:**

```xml
<PackageReference Include="PDFsharp" Version="6.2.4" />
<UseWPF>true</UseWPF>

<ItemGroup>
  <None Include="Templates\*.json" CopyToOutputDirectory="PreserveNewest" />
  <None Include="Assets\logo.png" CopyToOutputDirectory="PreserveNewest" />
  <Resource Include="Assets\logo.png" />
</ItemGroup>
```

4. **App.xaml birleştirin:**
   - `App.xaml` içindeki `Application.Resources` stillerini hedef projenin `App.xaml`'ine ekleyin
   - `App.xaml.cs` → `OnStartup` içine font ve path resolver çağrılarını ekleyin

5. **MainWindow entegrasyonu** — İki seçenek:

   **Seçenek 5a:** `MainWindow` doğrudan ana pencere olur.

   **Seçenek 5b:** Teklif modülü alt pencere / sekme olur:

```xml
<!-- Hedef projenizde -->
<Window ...
        xmlns:views="clr-namespace:HedefProje.Views">
    <TabControl>
        <TabItem Header="Teklif">
            <views:MainWindowContent />  <!-- MainWindow içeriğini UserControl'e çevirin -->
        </TabItem>
    </TabControl>
</Window>
```

> **İpucu:** `MainWindow.xaml` içeriğini `OfferEditorView.xaml` adlı bir `UserControl`'e taşımak entegrasyonu kolaylaştırır.

6. **Derleyin ve test edin:**
   - PDF Önizle → English
   - Tüm adımlarda veri girişi
   - Teknik şartname PDF sayfası

---

## Senaryo B — Web API / Servis (UI yok)

**Ne zaman:** ASP.NET Core, Worker Service veya arka plan işi olarak PDF üretmek istiyorsanız.

### Taşınacak minimum dosyalar

```
Models/          (tümü)
Services/
  ├── PdfExportService.cs
  ├── OfferDocumentLocalizer.cs
  ├── OfferContentTranslations.cs
  ├── TechnicalSpecTranslations.cs
  ├── TechnicalSpecContent.cs
  ├── TechnicalSpecContentEnglish.cs
  ├── TechnicalSpecContentParser.cs
  ├── PdfLocalization.cs
  ├── WindowsFontResolver.cs
  ├── CurrencyFormatService.cs
  └── ValidationService.cs
Assets/logo.png
Templates/*.json  (isteğe bağlı)
```

**Taşınmayacaklar:** `Views/`, `ViewModels/`, `App.xaml`, `Converters/`, `FileDialogService.cs`

### csproj (WPF olmadan)

```xml
<TargetFramework>net8.0</TargetFramework>
<!-- UseWPF YOK -->
<RuntimeIdentifier>win-x64</RuntimeIdentifier>

<PackageReference Include="PDFsharp" Version="6.2.4" />
```

> **Dikkat:** PDFsharp font çözücüsü Windows font klasörüne ihtiyaç duyar. Linux sunucuda çalışmaz; PDF üretimini Windows servis / makinede yapın.

### API endpoint örneği

```csharp
[ApiController]
[Route("api/offers")]
public class OfferPdfController : ControllerBase
{
    private readonly IPdfExportService _pdf = new PdfExportService();

    [HttpPost("export")]
    public async Task<IActionResult> Export([FromBody] OfferDocument document, [FromQuery] string lang = "tr")
    {
        WindowsFontResolver.EnsureRegistered();

        OfferLanguage language = lang == "en" ? OfferLanguage.English : OfferLanguage.Turkish;
        OfferDocument localized = OfferDocumentLocalizer.Localize(document, language);

        string tempPath = Path.Combine(Path.GetTempPath(), $"offer_{Guid.NewGuid()}.pdf");
        await _pdf.ExportAsync(localized, new PdfExportOptions
        {
            OutputPath = tempPath,
            Language = language,
            OpenAfterExport = false,
            DocumentTitle = localized.Info.MainTitle
        });

        byte[] bytes = await System.IO.File.ReadAllBytesAsync(tempPath);
        System.IO.File.Delete(tempPath);
        return File(bytes, "application/pdf", "teklif.pdf");
    }
}
```

### AppPathResolver alternatifi

WPF olmadığı için `AppPathResolver` (pack URI) kullanılamaz. Logo yolunu doğrudan verin:

```csharp
document.CompanySettings.HeaderLogoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "logo.png");
```

---

## Senaryo C — Mevcut Projede Sadece PDF Motoru

**Ne zaman:** Kendi veri modeliniz var, yalnızca PDF çıktısı istiyorsunuz.

### Adımlar

1. Senaryo B dosyalarını kopyalayın.
2. Kendi modelinizden `OfferDocument`'a **mapper** yazın:

```csharp
public static class MyProjectToOfferMapper
{
    public static OfferDocument Map(MyQuotation source) => new()
    {
        Info = new OfferInfo
        {
            ReferenceNumber = source.RefNo,
            JobName = source.ProjectName,
            CompanyName = source.CustomerName,
            OfferDate = source.Date,
            MainTitle = source.Title,
            ProjectTitle = source.Title
        },
        OfferItems = new(source.Lines.Select(l => new OfferItem
        {
            RowNo = l.Index,
            Description = l.Description,
            Quantity = l.Qty,
            Unit = l.Unit,
            UnitPrice = l.Price,
            Currency = l.Currency
        })),
        // ... diğer alanlar
    };
}
```

3. PDF üretin — [04-PDF-EXPORT-API.md](./04-PDF-EXPORT-API.md)

---

## Senaryo D — Class Library (DLL) Olarak Ayırma

**Ne zaman:** Birden fazla uygulama aynı PDF motorunu kullanacaksa.

### Yapı

```
Solution/
├── ZmtOfferPdf.Core/          ← Models + Services (WPF yok)
├── ZmtOfferPdfGenerator/      ← WPF UI (Core'a referans)
└── HedefProje/                ← Core veya UI'ya referans
```

### Core csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <RootNamespace>ZmtOfferPdf.Core</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="PDFsharp" Version="6.2.4" />
  </ItemGroup>
</Project>
```

WPF UI projesi Core'a `<ProjectReference>` ile bağlanır.

---

## Birleştirme Sonrası Kontrol Listesi

- [ ] Proje derleniyor (`dotnet build`)
- [ ] `WindowsFontResolver.EnsureRegistered()` startup'ta çağrılıyor
- [ ] Logo dosyası output dizininde (`Assets/logo.png`)
- [ ] Türkçe PDF oluşturuluyor
- [ ] İngilizce PDF oluşturuluyor
- [ ] Teknik şartname sayfaları doğru
- [ ] Türkçe karakterler (İ, Ş, Ğ) PDF'de doğru görünüyor
- [ ] Görseller PDF'e ekleniyor
- [ ] Namespace çakışması yok

---

## Sık Yapılan Hatalar

| Hata | Çözüm |
|------|-------|
| `GlobalFontSettings.FontResolver` null | Export öncesi `EnsureRegistered()` |
| ObservableCollection JSON deserialize | API'de `List<>` kullanın veya custom converter |
| MainViewModel doğrudan new'leniyor, DI yok | DI container'a kaydedin veya factory kullanın |
| XAML StaticResource bulunamıyor | App.xaml stillerini birleştirmeyi unutmayın |
| PublishSingleFile'da Templates kayboluyor | `CopyToOutputDirectory` + `AppPathResolver` |
