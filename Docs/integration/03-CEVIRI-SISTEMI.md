# Çeviri Sistemi

PDF İngilizce seçildiğinde metinler üç ayrı katmandan geçer. Birleştirme yaparken hangi katmanın ne yaptığını bilmek önemlidir.

---

## Çeviri Akışı

```
OfferDocument (Türkçe kaynak)
        │
        ▼
OfferDocumentLocalizer.Localize(document, OfferLanguage.English)
        │
        ├── PdfLocalization          → PDF sabit etiketleri (Tarih, Miktar, vb.)
        ├── OfferContentTranslations → Teklif metin sözlüğü
        ├── TechnicalSpecTranslations → Teknik şartname satır/etiket eşlemesi
        └── DeliveryText regex       → Teslim süresi kalıbı
        │
        ▼
OfferDocument (İngilizce kopya) → PdfExportService
```

**Önemli:** Çeviri yalnızca PDF oluşturma anında uygulanır. UI (formlar) her zaman Türkçe kalır.

---

## Katman 1: `PdfLocalization`

**Dosya:** `Services/PdfLocalization.cs`

PDF şablonundaki **sabit etiketler** — kullanıcı tarafından düzenlenemez.

| Türkçe | İngilizce |
|--------|-----------|
| FİYATIMIZA DAHİL OLAN İŞLER | WORKS INCLUDED IN OUR PRICE |
| GENEL TOPLAM | GRAND TOTAL |
| Tarih | Date |
| ... | ... |

**Kullanım:**

```csharp
PdfLocalization loc = PdfLocalization.For(OfferLanguage.English);
string label = loc.DateLabel; // "Date"
```

**Özelleştirme:** Yeni bir dil eklemek için `PdfLocalization` sınıfına üçüncü bir static instance ekleyin ve `For()` switch'ini genişletin.

---

## Katman 2: `OfferContentTranslations`

**Dosya:** `Services/OfferContentTranslations.cs`

Sabit teklif cümleleri için **Türkçe → İngilizce sözlük** (~45 giriş).

```csharp
// Dahil işler, hariç işler, notlar, başlıklar
["BİNA MALZEMELERİ;"] = "BUILDING MATERIALS;",
["Teklifimiz 1 gün süre ile geçerlidir."] = "Our quotation is valid for 1 day.",
```

**Eşleşme kuralı:** Tam metin, `StringComparer.Ordinal` — birebir aynı olmalı (boşluk, nokta dahil).

**Yeni metin eklemek:**

```csharp
["Yeni Türkçe cümle"] = "New English sentence",
```

**Çevrilmeyen alanlar (kasıtlı):**
- `PreparedBy`, `CompanyName`, `JobName`, `ReferenceNumber`
- `ContactInfo`, `AuthorizedPerson`
- Firma adresi, telefon, filigran

---

## Katman 3: `TechnicalSpecTranslations`

**Dosya:** `Services/TechnicalSpecTranslations.cs`

Teknik şartname için otomatik oluşturulan eşleme tabloları:

| Tablo | Kaynak |
|-------|--------|
| `TitleByTurkish` | Bölüm başlıkları (KONSTRÜKSİYON → CONSTRUCTION) |
| `LineByTurkish` | Tam satır eşleşmeleri |
| `LabelByTurkish` | Tab-separated etiketler |
| `ValueByTurkish` | Tab-separated değerler |

Statik constructor, `TechnicalSpecContent` ve `TechnicalSpecContentEnglish` dosyalarını satır satır eşleştirerek tabloları doldurur.

**Özel değerler:** Kullanıcı `140 mm` veya `MANKA PANEL` gibi varsayılandan farklı değer girerse:
- **Etiket** çevrilir (`Dış Duvar Konstrüksiyonu` → `Exterior Wall Construction`)
- **Değer** olduğu gibi kalır (sözlükte yoksa)

**Varsayılan şablon tam eşleşirse:** Tüm bölüm içeriği İngilizce varsayılan metinle değiştirilir (en hızlı yol).

---

## Katman 4: Teslim Metni (Regex)

**Dosya:** `Services/OfferDocumentLocalizer.cs`

Standart teslim cümlesi regex ile çevrilir:

```
Avansın alındığı tarih itibarı ile {X} günlük imalat döneminden sonra...
→
Shipment will commence after a {X}-day manufacturing period...
```

`{X}` rakam veya placeholder olabilir (`15`, `X`).

---

## Birleştirmede Çeviriyi Genişletme

### Yeni şablon metni eklemek

1. `OfferContentTranslations.cs` → sözlüğe ekleyin
2. Veya `TechnicalSpecContent.cs` + `TechnicalSpecContentEnglish.cs` → yeni bölüm/satır (TechnicalSpecTranslations otomatik güncellenir)

### Otomatik çeviri (Google / DeepL) eklemek

Mevcut yapı buna uygun değil; şu değişiklikler gerekir:

```csharp
public interface ITranslationService
{
    Task<string> TranslateAsync(string text, string from, string to);
}

// OfferDocumentLocalizer.TranslateText içinde:
if (!OfferContentTranslations.TryGetEnglish(text, out english))
    english = await translationService.TranslateAsync(text, "tr", "en");
```

Önerilen hibrit yaklaşım:
1. Önce sözlük
2. Sonra API (cache ile)
3. Firma/proje adları hariç tutulmalı

### UI çevirisi (formlar)

Şu an yok. WPF `.resx` dosyaları ile eklenebilir; PDF çekirdeğinden bağımsızdır.

---

## Çeviri Kapsam Tablosu

| İçerik | Tam eşleşme | Kısmi | Çevrilmez |
|--------|-------------|-------|-----------|
| PDF etiketleri | ✅ | — | — |
| Varsayılan dahil/hariç işler | ✅ | — | — |
| Varsayılan teknik şartname | ✅ | — | — |
| Düzenlenmiş şartname etiketleri | ✅ | — | — |
| Düzenlenmiş şartname değerleri | — | — | ✅ (sayı/marka) |
| Kullanıcı yazdığı yeni cümle | — | — | ✅ |
| Proje/firma bilgileri | — | — | ✅ (kasıtlı) |
| Uygulama arayüzü | — | — | ✅ |

---

## Test Kontrol Listesi

- [ ] Varsayılan şablon + English → tüm A/B/C/D/E bölümleri İngilizce
- [ ] Teknik şartname başlıkları İngilizce
- [ ] Teknik şartname etiketleri İngilizce, sayılar aynı
- [ ] Teslim metni İngilizce
- [ ] Firma adı / proje adı değişmedi
- [ ] Özel eklenen madde → beklenen davranış (Türkçe kalır veya sözlükte varsa çevrilir)
