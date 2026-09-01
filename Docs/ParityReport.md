# Legacy Excel Parity Raporu

## Kaynak ve yöntem

- Kaynak: `ÇELİK FİYAT.xlsx`
- Korunan kopya: `Legacy/ÇELİK FİYAT.xlsx`
- Her iki dosyanın SHA-256 değeri: `CD68377711897BAD94067AFB9242493000BF01D765D3902DE0F988CF9BE1814C`
- Golden Master: `Legacy/LegacyBaseline.json`
- Formül sürümü: `LegacyExcel-v1`
- Satır sayısı: 186
- Grup sayısı: 10
- Para toleransı: en fazla `0,01 TL`
- Miktar toleransı: `0,000001`

Kaynak workbook değiştirilmemiştir. Baseline, XLSX içindeki cached değerlerden geliştirme zamanında çıkarılmış; testler runtime'da workbook veya Excel açmadan JSON ile C# motorunu karşılaştırmıştır.

## Golden Master sonuçları

| Grup | Beklenen satış toplamı (TL) |
|---|---:|
| 1001 Hafif Çelik Panel ve Metal Aksam | 342.767,98497840005 |
| 1002 Alçıpan ve Kaplama | 298.366,872 |
| 1003 Çatı Sacı | 78.615,26587834278 |
| 1004 Kapı ve Pencere | 165.634,9056 |
| 1005 Elektrik | 34.845,8157 |
| 1006 Vida | 15.185,940000000002 |
| 1007 Depo ve Hırdavat | 75.190,95065011199 |
| 1008 Sıhhi Tesisat | 52.706,235359999984 |
| 1009 Çatı Oluğu | 7.483,36744 |
| 1010 Boya ve Mastik | 42.678,408 |

| Toplam | Beklenen (TL) |
|---|---:|
| TEKLİF — tüm gruplar | 1.113.475,7456068546 |
| TEKLİF — %25 iskonto sonrası | 835.106,80920514092 |
| BİNA BİLGİLERİ — ZMT kapsamı | 852.456,19664685475 |
| BİNA BİLGİLERİ — %21 iskonto | 179.015,80129583951 |
| BİNA BİLGİLERİ — ara/genel toplam (KDV %0) | 673.440,39535101526 |

## Test sonucu

Release konfigürasyonunda `dotnet test ZMT.SteelCost.sln -c Release` sonucu:

- 47 test geçti,
- 0 test başarısız,
- 186/186 satır miktar, alış/satış birim fiyatı ve satır toplamı eşleşti,
- 10/10 grup toplamı eşleşti,
- iki legacy özet yolu (%25 tüm gruplar ve %21 ZMT kapsamı) eşleşti,
- 25/30/35/40/45/50/55/60 çatı eğimleri doğrulandı,
- 50/100/150/250 m² eşikleri, 1–2 kat ve Panel/Aşık Omega dalları test edildi,
- üç PDF türü ve üç sayfalı XLSX gerçek dosya üretimiyle doğrulandı.

Sonuç: legacy örnek için tanımlanan toleranslar içinde parity sağlanmıştır.
