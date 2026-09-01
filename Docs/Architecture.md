# ZMT Çelik Maliyet — Mimari

## Hedef ve çalışma sınırı

Uygulama .NET 8, WPF ve MVVM ile Windows 10/11 x64 için geliştirilmiştir. Çalışma zamanında Excel, Microsoft Office, COM/Interop veya legacy workbook kullanılmaz. `Legacy/ÇELİK FİYAT.xlsx` yalnızca geliştirme zamanı tersine mühendislik ve Golden Master kaynağıdır.

Hesaplama para ve miktar alanlarında `decimal`, kullanıcı arayüzünde `tr-TR` kültürü kullanır. Formül sürümü `LegacyExcel-v1` olarak her hesap snapshot'ına yazılır.

## Katmanlar

| Proje | Sorumluluk |
|---|---|
| `ZMT.SteelCost.Domain` | Bina, malzeme, fiyat, proje ve hesap sonucu modelleri; enum/stabil ID'ler; doğrulama ve Excel uyumlu matematik. UI, SQLite ve Excel bağımlılığı yoktur. |
| `ZMT.SteelCost.Application` | Hesap motoru, strongly typed legacy kuralları, çatı servisi, proje/fiyat listesi/rapor portları ve use-case'ler. |
| `ZMT.SteelCost.Infrastructure` | SQLite repository, seed ve günlük yedek, JSONL structured logging, PDF ve yeni XLSX export. |
| `ZMT.SteelCost.App` | WPF navigation shell, MVVM ekranları, DI composition root, Türkçe sunum ve kullanıcı dostu hata yakalama. |
| `ZMT.SteelCost.Tests` | Golden Master parity, matematik, eşik, kapsam/KDV/iskonto, özellik ve gerçek PDF/XLSX üretim testleri. |

Bağımlılık yönü `App → Infrastructure/Application → Domain` şeklindedir. View code-behind yalnızca `InitializeComponent` içerir; iş mantığı hesap motoru ve servislerdedir.

## Hesaplama akışı

1. `BuildingInputValidator` ve `PricingParametersValidator` negatif değerleri, kat adedini, çatı eğimini ve oranları doğrular.
2. `CalculationEngine`, proje içindeki immutable fiyat snapshot'ı ile `LegacyRuleContext` oluşturur.
3. `LegacyExcelV1Rules.g.cs`, 186 stabil malzeme kodu için C# ifadelerini çalıştırır. Runtime formula evaluator yoktur.
4. Otomatik miktar ve açıklamalı manuel override ayrıştırılır; hesap izi `CalculationTrace` içine eklenir.
5. On grup için hesaplanan toplam, ZMT kapsamı ve müşteri kapsamı ayrı tutulur.
6. Sıralama açıkça `ZMT kapsamı → iskonto → ara toplam → satış KDV → genel toplam` şeklindedir.

`tools/generate_legacy_rules.py` yalnızca geliştirme zamanı kod üreticisidir. Üretilen C# dosyası uygulamaya derlenir; EXE bu script'e veya workbook'a ihtiyaç duymaz.

## Kalıcılık ve sürümleme

Veritabanı `%AppData%/ZMT/SteelCost/steelcost.db` altında ilk çalıştırmada oluşturulur. Günlük çevrimiçi SQLite yedeği `%AppData%/ZMT/SteelCost/Backups/` altında tutulur.

İlk seed şunları içerir:

- 10 malzeme kategorisi ve 186 malzeme,
- `MaterialFormulaParameters`, lookup değerleri ve legacy fiyat parametreleri,
- aktif fiyat listesi, ilk fiyat listesi sürümü ve 186 materyal fiyatı,
- formül, iskonto, KDV ve şema ayarları.

Yeni proje aktif fiyat listesi sürümünü `PriceListVersionId` ve `PricingSnapshot` olarak alır. Kaydedilen proje tekrar açıldığında kendi snapshot'ını kullanır. “Güncel Fiyat Listesini Uygula” bilinçli bir kullanıcı eylemidir; otomatik fiyat kayması yapılmaz. Yeni fiyat listesi sürümü yaratıldığında parametreler ve 186 efektif alış fiyatı ayrı sürüm olarak saklanır.

Her kayıt `CalculationRuns` ve `CalculationLines` tablolarında input, pricing, result, formül sürümü ve satır trace snapshot'larıyla denetlenebilir.

## Raporlama ve operasyon

- PDF: PDFsharp/MigraDoc WPF, A4, Türkçe Windows fontu, header ve `Sayfa X / Y` footer.
- XLSX: ClosedXML; `Özet`, `Malzemeler`, `Yükleme Listesi` sayfaları; Office kurulumu gerekmez.
- Log: `%LocalAppData%/ZMT/SteelCost/Logs/` altında günlük JSONL.
- UI'ya stack trace yazılmaz; yakalanmayan hatalar loglanıp Türkçe genel mesajla gösterilir.

ClosedXML ve PDFsharp/MigraDoc MIT lisanslıdır. Dağıtım bağımlılıkları `THIRD-PARTY-NOTICES.md` içinde listelenmiştir.
