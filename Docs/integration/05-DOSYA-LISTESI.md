# Kopyalanacak Dosya Listesi

Birleştirme senaryonuza göre işaretli dosyaları kopyalayın.

**Lejant:**  
✅ = Her senaryoda gerekli  
🖥️ = WPF UI (Senaryo A)  
📄 = PDF çekirdeği (Senaryo B/C)  
📁 = Statik dosya  

---

## Models/ (✅ tümü)

| Dosya | Açıklama |
|-------|----------|
| `OfferDocument.cs` | Ana veri modeli |
| `OfferInfo.cs` | Kapak / proje bilgileri |
| `OfferItem.cs` | Teklif kalemi (miktar, fiyat) |
| `OfferSection.cs` | `OfferSectionGroup`, `OfferBulletItem` |
| `OfferImage.cs` | Görsel + `ImageSections` sabitleri |
| `TechnicalSpecification.cs` | `TechnicalSpecSection` dahil |
| `TechnicalSpecLine.cs` | `TechnicalSpecLineTypes` sabitleri |
| `CompanySettings.cs` | Firma / logo / filigran |
| `OfferLanguage.cs` | TR/EN enum |
| `PdfExportOptions.cs` | Export seçenekleri |

---

## Services/ (📄 PDF çekirdeği)

| Dosya | Açıklama |
|-------|----------|
| `PdfExportService.cs` | PDF üretimi + `IPdfExportService` |
| `OfferDocumentLocalizer.cs` | TR→EN document çevirisi |
| `OfferContentTranslations.cs` | Teklif metin sözlüğü |
| `TechnicalSpecTranslations.cs` | Şartname satır eşlemesi |
| `TechnicalSpecContent.cs` | TR varsayılan şartname |
| `TechnicalSpecContentEnglish.cs` | EN varsayılan şartname |
| `TechnicalSpecContentParser.cs` | Şartname parse/serialize |
| `PdfLocalization.cs` | PDF sabit etiketler |
| `WindowsFontResolver.cs` | PDFsharp font — zorunlu |
| `CurrencyFormatService.cs` | Fiyat biçimlendirme |
| `ValidationService.cs` | Minimum doğrulama |
| `ImageService.cs` | Görsel tür kontrolü |
| `SettingsService.cs` | JSON şablon okuma |
| `AppPathResolver.cs` | 🖥️ Tek exe asset yolu |

---

## Services/ (🖥️ yalnızca WPF UI)

| Dosya | Açıklama |
|-------|----------|
| `FileDialogService.cs` | Dosya kaydet / görsel seç |

---

## ViewModels/ (🖥️ tümü)

| Dosya | Bağlı View |
|-------|------------|
| `MainViewModel.cs` | Ana pencere + komutlar |
| `OfferInfoViewModel.cs` | Teklif bilgileri |
| `WorksScopeViewModel.cs` | Dahil / hariç işler |
| `OfferItemsViewModel.cs` | Teklif kalemleri |
| `PaymentDeliveryViewModel.cs` | Ödeme / teslim |
| `ImagesViewModel.cs` | Görseller |
| `TechnicalSpecViewModel.cs` | Teknik şartname |
| `CompanySettingsViewModel.cs` | Firma ayarları |
| `PdfPreviewViewModel.cs` | PDF önizleme / dil seçimi |
| `IncludedWorksViewModel.cs` | (eski — WorksScope kullanılıyor) |
| `ExcludedWorksViewModel.cs` | (eski — WorksScope kullanılıyor) |
| `ViewModelBase.cs` | MVVM tabanı |
| `RelayCommand.cs` | ICommand |

> `IncludedWorksViewModel` ve `ExcludedWorksViewModel` artık `WorksScopeViewModel` ile birleştirilmiş durumda; yalnızca referans varsa taşıyın.

---

## Views/ (🖥️ tümü)

| Dosya | |
|-------|---|
| `MainWindow.xaml` + `.cs` | Ana pencere |
| `OfferInfoView.xaml` + `.cs` | |
| `WorksScopeView.xaml` + `.cs` | |
| `OfferItemsView.xaml` + `.cs` | |
| `PaymentDeliveryView.xaml` + `.cs` | |
| `ImagesView.xaml` + `.cs` | |
| `TechnicalSpecView.xaml` + `.cs` | |
| `CompanySettingsView.xaml` + `.cs` | |
| `PdfPreviewView.xaml` + `.cs` | |
| `IncludedWorksView.xaml` + `.cs` | (opsiyonel / eski) |
| `ExcludedWorksView.xaml` + `.cs` | (opsiyonel / eski) |

---

## Converters/ (🖥️)

| Dosya | |
|-------|---|
| `FilePathToImageSourceConverter.cs` | Görsel önizleme |

---

## App (🖥️)

| Dosya | |
|-------|---|
| `App.xaml` | Global stiller |
| `App.xaml.cs` | Startup (font, path resolver) |

---

## Statik Dosyalar (📁)

```
Assets/
  logo.png                          ✅

Templates/
  DefaultCompanySettings.json       ✅
  DefaultIncludedWorks.json         🖥️ (SettingsService kullanıyorsa)
  DefaultExcludedWorks.json         🖥️
  DefaultTechnicalSpecification.json 🖥️
```

---

## Proje Dosyası Parçaları

Hedef `.csproj`'a eklenecekler:

```xml
<PropertyGroup>
  <TargetFramework>net8.0-windows</TargetFramework>  <!-- WPF için -->
  <UseWPF>true</UseWPF>
  <Nullable>enable</Nullable>
  <ImplicitUsings>enable</ImplicitUsings>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="PDFsharp" Version="6.2.4" />
</ItemGroup>

<ItemGroup>
  <None Include="Templates\*.json" CopyToOutputDirectory="PreserveNewest" />
  <None Include="Assets\logo.png" CopyToOutputDirectory="PreserveNewest" />
  <Resource Include="Assets\logo.png" />
</ItemGroup>
```

---

## Kopyalanmayacaklar

```
bin/
obj/
.vs/
*.user
docs/          (bu rehber — isteğe bağlı taşınır)
```

---

## Dosya Sayısı Özeti

| Katman | Dosya sayısı (yaklaşık) |
|--------|-------------------------|
| Models | 10 |
| Services (çekirdek) | 14 |
| ViewModels | 13 |
| Views | 20 (xaml+cs) |
| Converters | 1 |
| App | 2 |
| Assets + Templates | 5 |
| **Toplam (tam UI)** | **~65 kaynak dosya** |
| **Minimum (PDF only)** | **~25 dosya** |
