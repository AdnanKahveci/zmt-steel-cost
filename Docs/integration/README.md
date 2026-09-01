# ZmtOfferPdfGenerator — Başka Projeye Birleştirme Rehberi

Bu klasör, **ZmtOfferPdfGenerator** uygulamasını başka bir .NET projesine taşımak veya entegre etmek için hazırlanmış yönlendirme dokümantasyonunu içerir.

## Proje Özeti

| Özellik | Değer |
|---------|-------|
| Platform | .NET 8 WPF (Windows) |
| PDF kütüphanesi | PDFsharp 6.2.4 |
| Ana çıktı | Çok sayfalı teklif PDF'i (TR/EN) |
| Veri modeli | `OfferDocument` |
| UI deseni | MVVM |

## Dokümantasyon Sırası

Aşağıdaki dosyaları **sırayla** okuyun:

| # | Dosya | İçerik |
|---|-------|--------|
| 1 | [01-MIMARI.md](./01-MIMARI.md) | Katmanlar, veri akışı, PDF sayfa yapısı |
| 2 | [02-BIRLESTIRME-REHBERI.md](./02-BIRLESTIRME-REHBERI.md) | Adım adım birleştirme senaryoları |
| 3 | [03-CEVIRI-SISTEMI.md](./03-CEVIRI-SISTEMI.md) | Türkçe → İngilizce çeviri katmanları |
| 4 | [04-PDF-EXPORT-API.md](./04-PDF-EXPORT-API.md) | Programatik PDF üretimi |
| 5 | [05-DOSYA-LISTESI.md](./05-DOSYA-LISTESI.md) | Kopyalanacak dosya kontrol listesi |
| 6 | [06-DUZENLEME-NOKTALARI.md](./06-DUZENLEME-NOKTALARI.md) | Namespace, marka, şablon özelleştirme |
| 7 | [07-ZMT-UYGULAMA-BAGLANTILARI.md](./07-ZMT-UYGULAMA-BAGLANTILARI.md) | Bu projede gerçekleştirilen veri ve UI bağlantıları |

## Hızlı Karar: Hangi Senaryo?

```
Başka projeniz ne tür?
│
├─ WPF masaüstü uygulaması
│   └─→ Senaryo A: View/ViewModel'leri doğrudan taşı (02-BIRLESTIRME-REHBERI.md § A)
│
├─ ASP.NET / Web API / servis
│   └─→ Senaryo B: Yalnızca Models + Services katmanını taşı (§ B)
│
├─ Mevcut uygulamada sadece PDF üretmek istiyorum
│   └─→ Senaryo C: Minimal çekirdek (§ C) + 04-PDF-EXPORT-API.md
│
└─ Class library (DLL) olarak paketlemek istiyorum
    └─→ Senaryo D: Ayrı proje referansı (§ D)
```

## Minimum Çalışma Gereksinimleri

- **Windows** (PDFsharp font çözücüsü Windows font klasörünü kullanır)
- **.NET 8+** (`net8.0-windows` WPF için)
- **NuGet:** `PDFsharp` 6.2.4
- **Başlangıçta çağrılması gereken:** `WindowsFontResolver.EnsureRegistered()`

## Tek Satırlık Entegrasyon Örneği

Başka bir projeden PDF üretmek için minimum kod:

```csharp
using ZmtOfferPdfGenerator.Models;
using ZmtOfferPdfGenerator.Services;

WindowsFontResolver.EnsureRegistered();

OfferDocument document = /* verinizi doldurun */;
OfferDocument english = OfferDocumentLocalizer.Localize(document, OfferLanguage.English);

IPdfExportService pdf = new PdfExportService();
await pdf.ExportAsync(english, new PdfExportOptions
{
    OutputPath = @"C:\Temp\Teklif.pdf",
    Language = OfferLanguage.English,
    DocumentTitle = english.Info.MainTitle
});
```

Detaylar için [04-PDF-EXPORT-API.md](./04-PDF-EXPORT-API.md) dosyasına bakın.

## Taşınmaması Gerekenler

Aşağıdaki klasörleri **kopyalamayın**:

- `bin/`
- `obj/`
- `.vs/`

## Sorun Giderme

| Sorun | Olası neden | Çözüm |
|-------|-------------|-------|
| PDF boş / font hatası | Font resolver kayıtlı değil | `WindowsFontResolver.EnsureRegistered()` |
| Logo görünmüyor | Asset yolu yanlış | `AppPathResolver` + `Assets/logo.png` |
| Türkçe karakter bozuk | Arial font bulunamadı | Windows Fonts klasöründe `arial.ttf` olmalı |
| Çeviri eksik | Sözlükte eşleşme yok | `03-CEVIRI-SISTEMI.md` |
| Tek exe'de Templates yok | Single-file publish | `AppPathResolver.EnsureBundledContentAvailable()` |
