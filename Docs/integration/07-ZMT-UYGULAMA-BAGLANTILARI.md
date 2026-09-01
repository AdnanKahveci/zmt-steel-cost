# ZMT Çelik Maliyet Uygulama Bağlantıları

Entegrasyon, `02-BIRLESTIRME-REHBERI.md` içindeki **Senaryo C — Mevcut Projede Sadece PDF Motoru** akışına göre uygulanmıştır.

## Veri akışı

```text
Project + CalculationResult
    → SteelCostOfferDocumentMapper
    → OfferDocument (Türkçe kaynak)
    → Teklif Formu (düzenlenebilir çalışma kopyası)
    → OfferDocumentLocalizer (TR / EN)
    → OfferDocumentValidationService
    → OfferPdfExportService
    → Ayrı teklif PDF'i
```

## Otomatik eşlemeler

| ZMT Çelik Maliyet verisi | Teklif belgesi |
|---|---|
| Proje/firma/CRM/tarih/hazırlayan | `OfferInfo` |
| ZMT sorumluluğundaki kategoriler | A — Dahil işler |
| Müşteri sorumluluğundaki kategoriler | B — Hariç işler |
| ZMT kategori toplamları | C — Teklif kalemleri |
| İskonto, ara toplam, KDV, genel toplam | C — Ticari özet |
| Kullanıcının ödeme metni | D — Ödeme şekli |
| Kullanıcının teslim günü | E — Teslim süresi |
| Bina, çatı, kaplama, kapı/pencere ve kapsam | Teknik şartname |

## Kullanıcı arayüzü

Sol menüdeki `Teklif Formu` sayfasında:

- müşteri, yetkili, iletişim, referans ve başlık bilgileri,
- gruplu dahil işler ile hariç iş maddeleri,
- miktar, birim, birim fiyat ve para birimi bulunan teklif kalemleri,
- teklif özelinde iskonto ve KDV,
- ödeme koşulları ve teslim metni,
- teklif başlangıcı ile teknik şartname öncesi için ayrı görsel listeleri,
- görsel başlığı, açıklaması, sırası, sayfası, çerçevesi ve oran koruma ayarı,
- düzenlenebilir teknik şartname bölümleri,
- firma, logo, filigran, PDF dili ve PDF kapsamı

düzenlenebilir. `Hesaptan Yenile` son hesap sonucunu forma yeniden aktarır; bu işlem form metinlerini ve fiyat kalemlerini sıfırlar, seçilmiş görselleri aynı proje içinde korur. `Raporlar → Profesyonel Teklif Belgesi` kartındaki `Ayrıntılı Teklif Formunu Aç` düğmesi aynı sayfaya gider. Teklif PDF'i standart maliyet PDF'lerinden bağımsızdır.

Teklif kalemlerinde miktar veya birim fiyat değiştirildiğinde kapsam toplamı yeniden hesaplanır. İskonto sırasıyla kapsam toplamına, KDV ise iskonto sonrası ara toplama uygulanır. Bu değişiklikler maliyet hesabının snapshot'ını değiştirmez; yalnızca düzenlenen teklif belgesini etkiler.

## Kod konumları

- Model/API: `src/ZMT.SteelCost.Application/Offers/OfferModels.cs`
- Mapper: `src/ZMT.SteelCost.Application/Offers/SteelCostOfferDocumentMapper.cs`
- Yerelleştirme: `src/ZMT.SteelCost.Application/Offers/OfferDocumentLocalizer.cs`
- Doğrulama: `src/ZMT.SteelCost.Application/Offers/OfferDocumentValidationService.cs`
- PDF servisi: `src/ZMT.SteelCost.Infrastructure/Reports/OfferPdfExportService.cs`
- Ayrıntılı şablon: `src/ZMT.SteelCost.Application/Offers/DetailedOfferTemplates.cs`
- Editör ViewModel: `src/ZMT.SteelCost.App/ViewModels/OfferEditorViewModel.cs`
- Editör görünümü: `src/ZMT.SteelCost.App/Views/OfferEditorView.xaml`
- Rapor kısayolu: `src/ZMT.SteelCost.App/Views/ReportsView.xaml`
- Logo kaynağı: `src/ZMT.SteelCost.App/Assets/logo.png`
- DI: `src/ZMT.SteelCost.App/App.xaml.cs`

PDFsharp/MigraDoc WPF `6.2.4` kullanılır. Logo EXE içine WPF Resource olarak gömülür; ilk kullanımda PDF motorunun okuyabileceği yerel uygulama dizinine çıkarılır. Tek dosyalık publish için harici şablon veya Excel dosyası gerekmez.
