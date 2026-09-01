# Excel Analizi

> Bu belge `tools/extract_legacy_workbook.py` ile kaynak çalışma kitabından deterministik olarak üretilmiştir.

## Kaynak ve değişmezlik

- Kaynak: `ÇELİK FİYAT.xlsx`
- Legacy kopya: `Legacy/ÇELİK FİYAT.xlsx`
- SHA-256: `cd68377711897bad94067afb9242493000bf01d765d3902de0f988cf9be1814c`
- Çalışma zamanında Excel/Office/Interop kullanılmayacaktır.
- Cached değerler Golden Master olarak `Legacy/LegacyBaseline.json` içine alınmıştır.

## Sayfa envanteri

| Sayfa | Durum | Kullanılan aralık | Dolu hücre | Formül | Birleşik aralık | Doğrulama |
|---|---:|---:|---:|---:|---:|---:|
| BİNA BİLGİLERİ | visible | A1:M90 | 186 | 31 | 77 | 9 |
| TEKLİF | visible | A1:Q250 | 2230 | 1431 | 44 | 4 |
| İSİMLENDİRME | visible | A1:Z52 | 78 | 4 | 0 | 0 |
| FORMÜL | hidden | A1:J24 | 85 | 52 | 1 | 0 |

## Ana girdiler

`BİNA BİLGİLERİ` sayfasındaki sabit/cached girdiler aşağıdadır. Boş değerler legacy örnekte gerçekten boştur.

| Hücre | Etiket / anlam | Değer | Birim |
|---|---|---:|---|
| B2 | Bina Alanı | 105 | m² |
| B3 | Tahmini kg/m² | 32 | kg/m² |
| C3 | Tahmini kg/m² | 80 | — |
| E3 | Tahmini kg/m² | 80 | — |
| B4 | Bina Köşe Sayısı | 6 | adet |
| B7 | Kat Adeti | 1 | kat |
| B9 | Kat Yüksekliği | 2.8 | m |
| B10 | Çatı Tipi | BEŞİK | — |
| B11 | Çatı Eğimi | 0.3 | % |
| B12 | Toplam Çatı Oturum Alanı | 105 | m² |
| B13 | Çatı Kaplaması | METAL KİREMİT ÇATI  | — |
| B14 | Aşık & Omega Sistemi / Tavan Paneli Çatı Paneli | PANEL SİSTEM | AŞIK ADETİ L:3000 |
| B16 | Saçak Genişliği (mt) | 0.4 | m |
| B17 | Saçak Uzunluğu (mt) | 20.56 | m |
| B18 | Bina Alın Uzunluğu (mt) | 20.4 | m |
| B20 | ÖZEL BÜKÜM AKSESUAR | MİKTAR | — |
| B23 | Metal Kiremit Mahyası | 4 | — |
| B28 | KAPLAMA | MİKTAR | — |
| B29 | Duvar Islak Hacim  | 17.5 | m |
| B30 | Tavan Islak Hacim | 19.5 | m² |
| B32 | Dış Duvar Kaplaması | 39.5 | m |
| C32 | Dış Duvar Kaplaması | mt | — |
| D32 | Dış Duvar Kaplaması | 11 mm OSB 2 | — |
| E32 | Dış Duvar Kaplaması | NEM BARİYERİ | — |
| F32 | Dış Duvar Kaplaması | BORDEX | — |
| B33 | İç Duvar Kaplaması | 33 | m |
| C33 | İç Duvar Kaplaması | mt | — |
| D33 | İç Duvar Kaplaması | 11 mm OSB 2 | — |
| E33 | İç Duvar Kaplaması | NEM BARİYERİ | — |
| F33 | İç Duvar Kaplaması | ALÇIPAN | — |
| B34 | Çatı Kaplaması  | 126.724896 | m² |
| C34 | Çatı Kaplaması  | m² | — |
| D34 | Çatı Kaplaması  | 11 mm OSB 2 | — |
| E34 | Çatı Kaplaması  | NEM BARİYERİ | — |
| B35 | Tavan Kaplaması | 97 | m² |
| C35 | Tavan Kaplaması | m² | — |
| D35 | Tavan Kaplaması | ALÇIPAN | — |
| B36 | Saçak Altı Kaplama | 16.383999999999997 | m² |
| B38 | KAPI TÜRÜ | ADET | — |
| B39 | Çelik Kapı (90*205) | 1 | — |
| B42 | Melamin Kapı (90*201) | 6 | — |
| B45 | PENCERE TÜRÜ | ADET | RENK |
| C45 | PENCERE TÜRÜ | RENK | — |
| C46 | PVC Pencere (105*180) | ANTRASİT | — |
| B48 | PVC Pencere (80*120) | 1 | — |
| B49 | PVC Pencere (140*100) | 2 | — |
| B55 | PVC Pencere (160*180) | 3 | — |
| B57 | PVC Vasistas (60*60) | 2 | — |
| B59 | VİTRİFİYE  | ADET | — |
| B61 | Klozet | 2 | — |
| B62 | Lavabo  | 2 | — |
| B64 | Duş Teknesi | 2 | — |
| B87 | İSKONTO | 21 | — |

## Malzeme grupları

| Kod | Grup | Header | Malzeme satırları | Toplam hücresi | Satır sayısı |
|---:|---|---:|---:|---:|---:|
| 1001 | Hafif Çelik Panel ve Metal Aksam | TEKLİF!9 | 10–36 | TEKLİF!F37 | 21 |
| 1002 | Alçıpan ve Kaplama | TEKLİF!38 | 39–52 | TEKLİF!F53 | 14 |
| 1003 | Çatı Sacı | TEKLİF!54 | 55–73 | TEKLİF!F74 | 19 |
| 1004 | Kapı ve Pencere | TEKLİF!75 | 76–94 | TEKLİF!F95 | 18 |
| 1005 | Elektrik | TEKLİF!96 | 97–124 | TEKLİF!F125 | 28 |
| 1006 | Vida | TEKLİF!126 | 127–134 | TEKLİF!F135 | 8 |
| 1007 | Depo ve Hırdavat | TEKLİF!136 | 137–151 | TEKLİF!F152 | 15 |
| 1008 | Sıhhi Tesisat | TEKLİF!153 | 154–192 | TEKLİF!F193 | 39 |
| 1009 | Çatı Oluğu ve Boru | TEKLİF!194 | 195–207 | TEKLİF!F208 | 13 |
| 1010 | Boya ve Mastik | TEKLİF!209 | 210–220 | TEKLİF!F221 | 11 |

Toplam **186** malzeme satırı bulunmuştur; hiçbir satır atlanmamıştır.

## Dropdown / liste doğrulamaları

| Sayfa | Hedef | Tür | Kaynak / değerler |
|---|---|---|---|
| BİNA BİLGİLERİ | B14:B15 | list | İSİMLENDİRME!$A$45:$A$46 |
| BİNA BİLGİLERİ | C46 | list | İSİMLENDİRME!$A$10:$A$12 |
| BİNA BİLGİLERİ | B13 | list | TEKLİF!$B$55:$B$57 |
| BİNA BİLGİLERİ | B11 | list | İSİMLENDİRME!$P$19:$P$26 |
| BİNA BİLGİLERİ | E32:F35 D32:D35 | list | İSİMLENDİRME!$A$34:$A$44 |
| BİNA BİLGİLERİ | C3:F4 | list | İSİMLENDİRME!$G$45:$G$49 |
| BİNA BİLGİLERİ | B7 | list | İSİMLENDİRME!$I$28:$I$29 |
| BİNA BİLGİLERİ | B10 | list | İSİMLENDİRME!$L$32:$L$35 |
| BİNA BİLGİLERİ | E76:E85 | list | İSİMLENDİRME!$O$40:$O$41 |
| TEKLİF | C5:D5 | list | "MUSA DOĞANAY,SAİT ERASLAN,MUTLU ŞENGÖL,CEM KARAKOÇ,BARIŞ ŞENGÖL" |
| TEKLİF | C6:D6 | list | "CEM KARAKOÇ,BARIŞ ŞENGÖL,MUTLU ŞENGÖL,MUSA DOĞANAY" |
| TEKLİF | C7:D7 | list | "TEKLİF ÇİZİMİ HAZIR,İMALAT ÇİZİMİ HAZIR,TEKLİF LİSTESİ HAZIR,ÜRETİM LİSTESİ HAZIR,ÜRETİME VERİLDİ" |
| TEKLİF | B39:B48 | list | İSİMLENDİRME!$A$34:$A$41 |

## Fiyat ve katsayı kaynakları

| Parametre | Hücre | Cached değer | Uygulama modeli |
|---|---|---:|---|
| USD/TL | TEKLİF!G2 | 48.1 | `PricingParameters.ExchangeRate` |
| Çelik fiyatı | TEKLİF!H2 | 1.3 | `PricingParameters.SteelPrice` |
| S seri | TEKLİF!N2 | 1.35 | `PricingParameters.SSeriesPrice` |
| Galvaniz | TEKLİF!O2 | 0.9 | `PricingParameters.GalvanizedPrice` |
| Boyalı sac | TEKLİF!P2 | 1.05 | `PricingParameters.PaintedSheetPrice` |
| Satış katsayısı | TEKLİF formülleri | 1.73 | `PricingParameters.SalesMarkupFactor` |
| Alış KDV | TEKLİF formülleri | 20% | `PricingParameters.PurchaseVatRate` |
| Bina özeti iskonto | BİNA BİLGİLERİ!B87 | 21% | Legacy parity alanı |
| Teklif iskonto | TEKLİF!E223 | 25% | Legacy parity alanı |
| Satış KDV | BİNA BİLGİLERİ!B89 / TEKLİF!E225 | boş → 0% | Proje override alanı |

## Çatı katsayıları

| Eğim | Katsayı |
|---:|---:|
| 25.00% | 1.0308 |
| 30.0% | 1.044 |
| 35.00% | 1.0595 |
| 40.0% | 1.077 |
| 45.00% | 1.0966 |
| 50.0% | 1.118 |
| 55.00% | 1.1413 |
| 60.0% | 1.1662 |

## Formül bağımlılıkları

Toplam 1518 formül hücresi vardır. Hücre bazlı formül, bağımlılık, cached sonuç ve C# hedef kural eşlemesi `Docs/FormulaCatalog.md` ve makine okunur `Legacy/FormulaCatalog.json` dosyalarındadır.

Ana akış:

```text
BİNA BİLGİLERİ girdileri
  ├─ İSİMLENDİRME!P17:S17 → çatı eğim katsayısı ve kaplama alanı
  ├─ TEKLİF!D10:D220 → 186 malzeme miktarı
  ├─ TEKLİF!I/K/E → alış, KDV dahil alış ve satış birim fiyatı
  ├─ TEKLİF!F → satır satış toplamları
  ├─ TEKLİF!F37…F221 → 10 grup toplamı
  ├─ TEKLİF!F222:F226 → tüm gruplar/iskonto/KDV/genel toplam
  └─ BİNA BİLGİLERİ!F76:F90 → sorumluluk kapsamı/iskonto/KDV/genel toplam
```

## Legacy / Unused

`FORMÜL` sayfası hidden durumdadır. Aktif sayfalardaki formüllerin hiçbirinde `FORMÜL!` referansı yoktur; bu nedenle ana hesap motoruna dahil edilmemiştir. Sayfadaki 52 formül kaybolmaması için kataloglanmış, ancak `Legacy / Unused` olarak sınıflandırılmıştır.

## Bilinen legacy tutarsızlıkları

- BİNA BİLGİLERİ özeti yalnız ZMT kapsamını; TEKLİF sayfası bütün grupları toplar. Uygulamada ayrı değerlerdir.
- Bina özeti %21, TEKLİF %25 iskonto kullanır. Parity testleri ikisini ayrı doğrular.
- Satış KDV girişi boştur ve cached sonuç 0'dır; yeni UI bunu açık bir proje alanı yapar.
- Bazı legacy kâr hücrelerinde miktar/fiyat sıfırken `#DIV/0!` bulunur. Yeni motorda kâr oranı güvenli biçimde 0 döner; parasal parity etkilenmez.
- FORMÜL sayfası aktif modele bağlı değildir.

## Kullanılan hücrelerin tam dökümü

### BİNA BİLGİLERİ

| Hücre | Tür | Değer / formül | Cached değer |
|---|---|---|---:|
| A1 | Sabit | HAFİF ÇELİK FİYAT HESAPLAMA ROBOTU | — |
| A2 | Sabit | Bina Alanı | — |
| B2 | Sabit | 105 | — |
| C2 | Sabit | DIŞ DUVAR KALINLIĞI | — |
| E2 | Sabit | İÇ DUVAR KALINLIĞI | — |
| A3 | Sabit | Tahmini kg/m² | — |
| B3 | Sabit | 32 | — |
| C3 | Sabit | 80 | — |
| E3 | Sabit | 80 | — |
| A4 | Sabit | Bina Köşe Sayısı | — |
| B4 | Sabit | 6 | — |
| A5 | Sabit | Zemin Kat En | — |
| A6 | Sabit | Zemin Kat Boy | — |
| A7 | Sabit | Kat Adeti | — |
| B7 | Sabit | 1 | — |
| A8 | Sabit | Ara Kat Alanı | — |
| A9 | Sabit | Kat Yüksekliği | — |
| B9 | Sabit | 2.8 | — |
| A10 | Sabit | Çatı Tipi | — |
| B10 | Sabit | BEŞİK | — |
| A11 | Sabit | Çatı Eğimi | — |
| B11 | Sabit | 0.3 | — |
| A12 | Sabit | Toplam Çatı Oturum Alanı | — |
| B12 | Sabit | 105 | — |
| A13 | Sabit | Çatı Kaplaması | — |
| B13 | Sabit | METAL KİREMİT ÇATI  | — |
| A14 | Sabit | Aşık & Omega Sistemi / Tavan Paneli Çatı Paneli | — |
| B14 | Sabit | PANEL SİSTEM | — |
| C14 | Sabit | AŞIK ADETİ L:3000 | — |
| E14 | Sabit | OMEGA ADETİ   L : 2500 | — |
| A16 | Sabit | Saçak Genişliği (mt) | — |
| B16 | Sabit | 0.4 | — |
| A17 | Sabit | Saçak Uzunluğu (mt) | — |
| B17 | Formül | =10.28+10.28 | 20.56 |
| A18 | Sabit | Bina Alın Uzunluğu (mt) | — |
| B18 | Formül | =10.2+10.2 | 20.4 |
| A20 | Sabit | ÖZEL BÜKÜM AKSESUAR | — |
| B20 | Sabit | MİKTAR | — |
| F20 | Sabit | BİRİM | — |
| A21 | Sabit | Sırt Mahya | — |
| F21 | Sabit | L:2500 / Ad. | — |
| A22 | Sabit | Parapet Kaplama Showroom | — |
| F22 | Sabit | L:2500 / Ad. | — |
| A23 | Sabit | Metal Kiremit Mahyası | — |
| B23 | Sabit | 4 | — |
| F23 | Sabit | L:2500 / Ad. | — |
| A24 | Sabit | Özel Mahya (Dar) | — |
| F24 | Sabit | L:2500 / Ad. | — |
| A25 | Sabit | Özel Mahya (Geniş) | — |
| F25 | Sabit | L:2500 / Ad. | — |
| A26 | Sabit | Metal Tahta | — |
| F26 | Sabit | L:2500 / Ad. | — |
| A28 | Sabit | KAPLAMA | — |
| B28 | Sabit | MİKTAR | — |
| F28 | Sabit | BİRİM | — |
| A29 | Sabit | Duvar Islak Hacim  | — |
| B29 | Formül | =17.5 | 17.5 |
| F29 | Sabit | mt | — |
| A30 | Sabit | Tavan Islak Hacim | — |
| B30 | Formül | =7+1.5+11 | 19.5 |
| F30 | Sabit | m² | — |
| B31 | Sabit | DUVAR UZUNLUĞU | — |
| C31 | Sabit | BİRİM | — |
| D31 | Sabit | KAPLAMA-1 | — |
| E31 | Sabit | KAPLAMA-2 | — |
| F31 | Sabit | KAPLAMA-3 | — |
| A32 | Sabit | Dış Duvar Kaplaması | — |
| B32 | Sabit | 39.5 | — |
| C32 | Sabit | mt | — |
| D32 | Sabit | 11 mm OSB 2 | — |
| E32 | Sabit | NEM BARİYERİ | — |
| F32 | Sabit | BORDEX | — |
| A33 | Sabit | İç Duvar Kaplaması | — |
| B33 | Sabit | 33 | — |
| C33 | Sabit | mt | — |
| D33 | Sabit | 11 mm OSB 2 | — |
| E33 | Sabit | NEM BARİYERİ | — |
| F33 | Sabit | ALÇIPAN | — |
| A34 | Sabit | Çatı Kaplaması  | — |
| B34 | Formül | =İSİMLENDİRME!S17 | 126.724896 |
| C34 | Sabit | m² | — |
| D34 | Sabit | 11 mm OSB 2 | — |
| E34 | Sabit | NEM BARİYERİ | — |
| A35 | Sabit | Tavan Kaplaması | — |
| B35 | Sabit | 97 | — |
| C35 | Sabit | m² | — |
| D35 | Sabit | ALÇIPAN | — |
| A36 | Sabit | Saçak Altı Kaplama | — |
| B36 | Formül | =B16*(B17+B18) | 16.383999999999997 |
| C36 | Sabit | mt | — |
| A38 | Sabit | KAPI TÜRÜ | — |
| B38 | Sabit | ADET | — |
| A39 | Sabit | Çelik Kapı (90*205) | — |
| B39 | Sabit | 1 | — |
| A40 | Sabit | PVC Kapı (90*200) | — |
| A41 | Sabit | Duble PVC Kapı (160*200) | — |
| A42 | Sabit | Melamin Kapı (90*201) | — |
| B42 | Sabit | 6 | — |
| A43 | Sabit | Ahşap Kasalı Amerikan Kapı (90*201) | — |
| A45 | Sabit | PENCERE TÜRÜ | — |
| B45 | Sabit | ADET | — |
| C45 | Sabit | RENK | — |
| A46 | Sabit | PVC Pencere (105*180) | — |
| C46 | Sabit | ANTRASİT | — |
| A47 | Sabit | PVC Pencere (59*180) | — |
| A48 | Sabit | PVC Pencere (80*120) | — |
| B48 | Sabit | 1 | — |
| A49 | Sabit | PVC Pencere (140*100) | — |
| B49 | Sabit | 2 | — |
| A50 | Sabit | PVC Pencere (140*140) | — |
| A51 | Sabit | PVC Pencere (140*160) | — |
| A52 | Sabit | PVC Pencere (140*180) | — |
| A53 | Sabit | PVC Pencere (160*120) | — |
| A54 | Sabit | PVC Pencere (160*160) | — |
| A55 | Sabit | PVC Pencere (160*180) | — |
| B55 | Sabit | 3 | — |
| A56 | Sabit | PVC Pencere (Sürgülü) (180*200) | — |
| A57 | Sabit | PVC Vasistas (60*60) | — |
| B57 | Sabit | 2 | — |
| A59 | Sabit | VİTRİFİYE  | — |
| B59 | Sabit | ADET | — |
| B60 | Sabit | ZEMİN KAT | — |
| E60 | Sabit | 1.KAT | — |
| A61 | Sabit | Klozet | — |
| B61 | Sabit | 2 | — |
| A62 | Sabit | Lavabo  | — |
| B62 | Sabit | 2 | — |
| A63 | Sabit | Alaturka WC Taşı | — |
| A64 | Sabit | Duş Teknesi | — |
| B64 | Sabit | 2 | — |
| A74 | Sabit | ÖZET | — |
| A75 | Sabit | GRUP  | — |
| C75 | Sabit | TUTAR | — |
| E75 | Sabit | MÜŞTERİYE AİT  | — |
| F75 | Sabit | NİHAİ TOPLAM | — |
| A76 | Sabit | HAFİF ÇELİK PANEL VE METAL AKSAM GRUBU | — |
| C76 | Formül | =TEKLİF!F37 | 342767.98497840005 |
| E76 | Sabit | ZMT'YE AİT | — |
| F76 | Formül | =IF(E76="ZMT'YE AİT",C76,0) | 342767.98497840005 |
| A77 | Sabit | ALÇIPAN VE KAPLAMA GRUBU | — |
| C77 | Formül | =TEKLİF!F53 | 298366.872 |
| E77 | Sabit | ZMT'YE AİT | — |
| F77 | Formül | =IF(E77="ZMT'YE AİT",C77,0) | 298366.872 |
| A78 | Sabit | ÇATI SACI GRUBU | — |
| C78 | Formül | =TEKLİF!F74 | 78615.26587834278 |
| E78 | Sabit | ZMT'YE AİT | — |
| F78 | Formül | =IF(E78="ZMT'YE AİT",C78,0) | 78615.26587834278 |
| A79 | Sabit | KAPI VE PENCERE GRUBU | — |
| C79 | Formül | =TEKLİF!F95 | 165634.9056 |
| E79 | Sabit | MÜŞTERİYE AİT | — |
| F79 | Formül | =IF(E79="ZMT'YE AİT",C79,0) | 0 |
| A80 | Sabit | ELEKTRİK TESİSAT GRUBU | — |
| C80 | Formül | =TEKLİF!F125 | 34845.8157 |
| E80 | Sabit | ZMT'YE AİT | — |
| F80 | Formül | =IF(E80="ZMT'YE AİT",C80,0) | 34845.8157 |
| A81 | Sabit | VİDA GRUBU | — |
| C81 | Formül | =TEKLİF!F135 | 15185.940000000002 |
| E81 | Sabit | ZMT'YE AİT | — |
| F81 | Formül | =IF(E81="ZMT'YE AİT",C81,0) | 15185.940000000002 |
| A82 | Sabit | DEPO VE HIRDAVAT GRUBU | — |
| C82 | Formül | =TEKLİF!F152 | 75190.95065011199 |
| E82 | Sabit | ZMT'YE AİT | — |
| F82 | Formül | =IF(E82="ZMT'YE AİT",C82,0) | 75190.95065011199 |
| A83 | Sabit | SIHHİ TESİSAT GRUBU | — |
| C83 | Formül | =TEKLİF!F193 | 52706.235359999984 |
| E83 | Sabit | MÜŞTERİYE AİT | — |
| F83 | Formül | =IF(E83="ZMT'YE AİT",C83,0) | 0 |
| A84 | Sabit | ÇATI OLUĞU VE BORU GRUBU | — |
| C84 | Formül | =TEKLİF!F208 | 7483.36744 |
| E84 | Sabit | ZMT'YE AİT | — |
| F84 | Formül | =IF(E84="ZMT'YE AİT",C84,0) | 7483.36744 |
| A85 | Sabit | BOYA VE MASTİK GRUBU | — |
| C85 | Formül | =TEKLİF!F221 | 42678.408 |
| E85 | Sabit | MÜŞTERİYE AİT | — |
| F85 | Formül | =IF(E85="ZMT'YE AİT",C85,0) | 0 |
| A86 | Sabit | GRUP TOPLAMI | — |
| F86 | Formül | =SUM(F76:F85) | 852456.1966468547 |
| A87 | Sabit | İSKONTO | — |
| B87 | Sabit | 21 | — |
| F87 | Formül | =F86*B87/100 | 179015.8012958395 |
| A88 | Sabit | ARA TOPLAM | — |
| F88 | Formül | =F86-F87 | 673440.3953510153 |
| A89 | Sabit | KDV | — |
| F89 | Formül | =F88*B89/100 | 0 |
| A90 | Sabit | GENEL TOPLAM | — |
| C90 | Formül | =F89+F88 | 673440.3953510153 |

### TEKLİF

| Hücre | Tür | Değer / formül | Cached değer |
|---|---|---|---:|
| A1 | Sabit | ZMT PREFABRİK                                                                                                                                                    Hafif Çelik Bina Yükleme Listesi | — |
| C1 | Sabit | DÜZENLENME ⏎ TARİHİ | — |
| D1 | Sabit | 2026-06-22T00:00:00 | — |
| E1 | Sabit | ÇELİK  | — |
| F1 | Sabit | DIŞ:140'LIK           İÇ :  80'LİK  | — |
| G1 | Sabit | DOLAR KURU  | — |
| H1 | Sabit | TON FİYATI | — |
| I1 | Sabit | KDVSİZ  ⏎ FİYATLAR  | — |
| K1 | Sabit | KDV DAHİL  ⏎ FİYATLAR  | — |
| L1 | Sabit | İSKONTOLU FİYAT | — |
| M1 | Sabit | BRÜT KAR  | — |
| N1 | Sabit | S SERİ TON FİYATI | — |
| O1 | Sabit | GALVANİZ  ⏎ FİYATI | — |
| P1 | Sabit | BOYALI SAÇ   ⏎ FİYATI | — |
| A2 | Sabit | FİRMA: | — |
| E2 | Sabit | Alan (m²) | — |
| G2 | Sabit | 48.1 | — |
| H2 | Sabit | 1.3 | — |
| N2 | Sabit | 1.35 | — |
| O2 | Sabit | 0.9 | — |
| P2 | Sabit | 1.05 | — |
| A3 | Sabit | Müşteri Adı: | — |
| E3 | Sabit | H:Yükseklik | — |
| F3 | Sabit | 2800 | — |
| A4 | Sabit | Teklif Sayımı Yapan: | — |
| E4 | Sabit | Tarih | — |
| A5 | Sabit | Proje Kontrolü Yapan:  | — |
| E5 | Sabit | Tarih | — |
| A6 | Sabit | Üretim Listesi Yapan:  | — |
| E6 | Sabit | Tarih | — |
| A7 | Sabit | Proje Aşaması | — |
| E7 | Sabit | CRM NO | — |
| B8 | Sabit | Malzeme | — |
| C8 | Sabit | Malzeme Ölçüsü | — |
| D8 | Sabit | Adet | — |
| E8 | Sabit | Birim Fiyatı | — |
| F8 | Sabit | TOPLAM | — |
| I8 | Sabit | TOPLAM | — |
| J8 | Sabit | TOPLAM | — |
| K8 | Sabit | TOPLAM | — |
| L8 | Sabit | TOPLAM | — |
| M8 | Sabit | TOPLAM | — |
| A9 | Sabit |         HAFİF ÇELİK PANEL VE METAL AKSAM GRUBU | — |
| D9 | Sabit | 1001 | — |
| A10 | Sabit | 1 | — |
| B10 | Sabit | HAFİF ÇELİK PANEL KARKAS HALİNDE | — |
| C10 | Sabit | KG | — |
| D10 | Formül | ='BİNA BİLGİLERİ'!B2*'BİNA BİLGİLERİ'!B3 | 3360 |
| E10 | Formül | =H2*G2*1.6 | 100.048 |
| F10 | Formül | =D10*E10 | 336161.28 |
| I10 | Formül | =G2*O2 | 43.29 |
| J10 | Formül | =I10*D10 | 145454.4 |
| K10 | Formül | =I10*1.2 | 51.948 |
| L10 | Formül | =E10-E10*$E$223/100 | 75.036 |
| M10 | Formül | =(L10-K10)/K10 | 0.4444444444444445 |
| D11 | Sabit | Z1 | — |
| A12 | Sabit | 2 | — |
| B12 | Sabit | ÖZEL BÜKÜM  SAÇAK ALIN SACI (GALVANİZ) | — |
| C12 | Sabit | 250*2500 | — |
| D12 | Formül | =IF('BİNA BİLGİLERİ'!B14="AŞIK OMEGA",ROUNDUP('BİNA BİLGİLERİ'!B17/2.5,0),0) | 0 |
| E12 | Formül | =K12*1.73 | 647.9629884 |
| F12 | Formül | =E12*D12 | 0 |
| I12 | Formül | =7.21*O2*G2 | 312.1209 |
| J12 | Formül | =I12*D12 | 0 |
| K12 | Formül | =I12*1.2 | 374.54508 |
| L12 | Formül | =E12-E12*$E$223/100 | 485.97224129999995 |
| M12 | Formül | =(L12-K12)/K12 | 0.29749999999999993 |
| A13 | Sabit | 3 | — |
| B13 | Sabit | ÖZEL BÜKÜM ALIN SACI | — |
| C13 | Sabit | 120*2800 | — |
| D13 | Formül | =IF('BİNA BİLGİLERİ'!B14="AŞIK OMEGA",ROUNDUP('BİNA BİLGİLERİ'!B18/2.5,0),0) | 0 |
| E13 | Formül | =K13*1.73 | 220.18159799999998 |
| F13 | Formül | =E13*D13 | 0 |
| I13 | Formül | =2.1*P2*$G$2 | 106.0605 |
| J13 | Formül | =I13*D13 | 0 |
| K13 | Formül | =I13*1.2 | 127.2726 |
| L13 | Formül | =E13-E13*$E$223/100 | 165.13619849999998 |
| M13 | Formül | =(L13-K13)/K13 | 0.2974999999999999 |
| A14 | Sabit | 4 | — |
| B14 | Sabit | AŞIK KAPAMA U'SU (GALVANİZ) | — |
| C14 | Sabit | 80*2500 | — |
| D14 | Formül | =IF('BİNA BİLGİLERİ'!B14="AŞIK OMEGA",ROUNDUP('BİNA BİLGİLERİ'!B18/2.5,0),0) | 0 |
| E14 | Formül | =K14*1.73 | 197.71408800000003 |
| F14 | Formül | =E14*D14 | 0 |
| I14 | Formül | =2.2*O2*G2 | 95.23800000000001 |
| J14 | Formül | =I14*D14 | 0 |
| K14 | Formül | =I14*1.2 | 114.28560000000002 |
| L14 | Formül | =E14-E14*$E$223/100 | 148.28556600000002 |
| M14 | Formül | =(L14-K14)/K14 | 0.2975 |
| D15 | Sabit | Z1 | — |
| A16 | Sabit | 5 | — |
| B16 | Sabit | KÖŞEBENT | — |
| C16 | Sabit | 30*30*2500 | — |
| D16 | Formül | =IF('BİNA BİLGİLERİ'!B2<50,3,IF('BİNA BİLGİLERİ'!B2<150,5,IF('BİNA BİLGİLERİ'!B2<250,7,10))) | 5 |
| E16 | Formül | =K16*1.73 | 84.65757768 |
| F16 | Formül | =E16*D16 | 423.28788840000004 |
| I16 | Formül | =0.942*O2*G2 | 40.779180000000004 |
| J16 | Formül | =I16*D16 | 203.8959 |
| K16 | Formül | =I16*1.2 | 48.935016000000005 |
| L16 | Formül | =E16-E16*$E$223/100 | 63.49318326000001 |
| M16 | Formül | =(L16-K16)/K16 | 0.29750000000000004 |
| A17 | Sabit | 6 | — |
| B17 | Sabit | KÖŞEBENT | — |
| C17 | Sabit | 50*50*2500 | — |
| D17 | Formül | =IF('BİNA BİLGİLERİ'!B2<50,3,IF('BİNA BİLGİLERİ'!B2<150,5,IF('BİNA BİLGİLERİ'!B2<250,7,10))) | 5 |
| E17 | Formül | =K17*1.73 | 141.0959628 |
| F17 | Formül | =E17*D17 | 705.479814 |
| I17 | Formül | =1.57*O2*G2 | 67.9653 |
| J17 | Formül | =I17*D17 | 339.8265 |
| K17 | Formül | =I17*1.2 | 81.55836 |
| L17 | Formül | =E17-E17*$E$223/100 | 105.8219721 |
| M17 | Formül | =(L17-K17)/K17 | 0.29750000000000004 |
| A18 | Sabit | 7 | — |
| B18 | Sabit | ÖZEL AÇILI KÖŞEBENT 1,2 mm | — |
| C18 | Sabit | 100*100*2500 | — |
| E18 | Formül | =K18*1.73 | 359.48016 |
| F18 | Formül | =E18*D18 | 0 |
| I18 | Formül | =4*O2*G2 | 173.16 |
| J18 | Formül | =I18*D18 | 0 |
| K18 | Formül | =I18*1.2 | 207.792 |
| L18 | Formül | =E18-E18*$E$223/100 | 269.61012 |
| M18 | Formül | =(L18-K18)/K18 | 0.2975 |
| D19 | Sabit | Z1 | — |
| A20 | Sabit | 8 | — |
| B20 | Sabit | ANKALAJ AYAĞI | — |
| C20 | Sabit | 50x200 | — |
| D20 | Formül | =ROUNDUP('BİNA BİLGİLERİ'!B32/2,0) | 20 |
| E20 | Formül | =K20*4 | 168 |
| F20 | Formül | =E20*D20 | 3360 |
| I20 | Sabit | 35 | — |
| J20 | Formül | =I20*D20 | 700 |
| K20 | Formül | =I20*1.2 | 42 |
| L20 | Formül | =E20-E20*$E$223/100 | 126 |
| M20 | Formül | =(L20-K20)/K20 | 2 |
| A21 | Sabit | 9 | — |
| B21 | Sabit | BİRLEŞİM APARATI | — |
| C21 | Sabit | 70x550 | — |
| E21 | Formül | =K21*1.73 | 319.70399999999995 |
| F21 | Formül | =E21*D21 | 0 |
| I21 | Sabit | 154 | — |
| J21 | Formül | =I21*D21 | 0 |
| K21 | Formül | =I21*1.2 | 184.79999999999998 |
| L21 | Formül | =E21-E21*$E$223/100 | 239.77799999999996 |
| M21 | Formül | =(L21-K21)/K21 | 0.29749999999999993 |
| A22 | Sabit | 10 | — |
| B22 | Sabit | 100 LÜK  PİS SU KAPAMASI (GALVANİZ) | — |
| C22 | Sabit | 180*2800 | — |
| D22 | Formül | ='BİNA BİLGİLERİ'!C61 | 0 |
| E22 | Formül | =K22*1.73 | 665.038296 |
| F22 | Formül | =E22*D22 | 0 |
| I22 | Formül | =7.4*O2*G2 | 320.346 |
| J22 | Formül | =I22*D22 | 0 |
| K22 | Formül | =I22*1.2 | 384.41519999999997 |
| L22 | Formül | =E22-E22*$E$223/100 | 498.77872199999996 |
| M22 | Formül | =(L22-K22)/K22 | 0.2975 |
| D23 | Sabit | Z1 | — |
| A24 | Sabit | 11 | — |
| B24 | Sabit |  SIRT MAHYA | — |
| C24 | Sabit | L:2500 | — |
| D24 | Formül | ='BİNA BİLGİLERİ'!B21 | 0 |
| E24 | Formül | =K24*1.73 | 808.8303599999999 |
| F24 | Formül | =E24*D24 | 0 |
| I24 | Formül | =9*O2*$G$2 | 389.61 |
| J24 | Formül | =I24*D24 | 0 |
| K24 | Formül | =I24*1.2 | 467.532 |
| L24 | Formül | =E24-E24*$E$223/100 | 606.62277 |
| M24 | Formül | =(L24-K24)/K24 | 0.29749999999999993 |
| A25 | Sabit | 12 | — |
| B25 | Sabit |  PARAPET KAPLAMA SHOWROOM | — |
| C25 | Sabit | L:2800 | — |
| D25 | Formül | ='BİNA BİLGİLERİ'!B22 | 0 |
| E25 | Formül | =K25*1.73 | 471.81771000000003 |
| F25 | Formül | =E25*D25 | 0 |
| I25 | Formül | =4.5*P2*$G$2 | 227.27250000000004 |
| J25 | Formül | =I25*D25 | 0 |
| K25 | Formül | =I25*1.2 | 272.72700000000003 |
| L25 | Formül | =E25-E25*$E$223/100 | 353.8632825 |
| M25 | Formül | =(L25-K25)/K25 | 0.29749999999999993 |
| A26 | Sabit | 13 | — |
| B26 | Sabit | METAL KİREMİT MAHYASI | — |
| C26 | Sabit | L:2800 | — |
| D26 | Formül | ='BİNA BİLGİLERİ'!B23 | 4 |
| E26 | Formül | =K26*1.73 | 529.4843189999999 |
| F26 | Formül | =E26*D26 | 2117.9372759999997 |
| I26 | Formül | =5.05*P2*$G$2 | 255.05025 |
| J26 | Formül | =I26*D26 | 1020.201 |
| K26 | Formül | =I26*1.2 | 306.0603 |
| L26 | Formül | =E26-E26*$E$223/100 | 397.11323924999994 |
| M26 | Formül | =(L26-K26)/K26 | 0.2974999999999999 |
| A27 | Sabit | 14 | — |
| B27 | Sabit | ÖZEL MAHYA (DAR OLAN) | — |
| C27 | Sabit | 10x1500 | — |
| D27 | Formül | ='BİNA BİLGİLERİ'!B24 | 0 |
| E27 | Formül | =K27*1.73 | 92.58111954 |
| F27 | Formül | =E27*D27 | 0 |
| I27 | Formül | =0.883*P2*$G$2 | 44.595915000000005 |
| J27 | Formül | =I27*D27 | 0 |
| K27 | Formül | =I27*1.2 | 53.515098 |
| L27 | Formül | =E27-E27*$E$223/100 | 69.435839655 |
| M27 | Formül | =(L27-K27)/K27 | 0.2974999999999999 |
| A28 | Sabit | 15 | — |
| B28 | Sabit | ÖZEL MAHYA (GENİŞ OLAN) | — |
| C28 | Sabit | 20x2800 | — |
| D28 | Formül | ='BİNA BİLGİLERİ'!B25 | 0 |
| E28 | Formül | =K28*1.73 | 314.54514 |
| F28 | Formül | =E28*D28 | 0 |
| I28 | Formül | =3*P2*$G$2 | 151.51500000000001 |
| J28 | Formül | =I28*D28 | 0 |
| K28 | Formül | =I28*1.2 | 181.818 |
| L28 | Formül | =E28-E28*$E$223/100 | 235.90885500000002 |
| M28 | Formül | =(L28-K28)/K28 | 0.2975 |
| A29 | Sabit | 16 | — |
| B29 | Sabit | METAL TAHTA | — |
| C29 | Sabit | L:2500 | — |
| D29 | Formül | ='BİNA BİLGİLERİ'!B26 | 0 |
| E29 | Formül | =K29*1.73 | 134.80506 |
| F29 | Formül | =E29*D29 | 0 |
| I29 | Formül | =1.5*O2*$G$2 | 64.935 |
| J29 | Formül | =I29*D29 | 0 |
| K29 | Formül | =I29*1.2 | 77.922 |
| L29 | Formül | =E29-E29*$E$223/100 | 101.10379499999999 |
| M29 | Formül | =(L29-K29)/K29 | 0.29749999999999993 |
| D30 | Sabit | Z1 | — |
| A31 | Sabit | 17 | — |
| B31 | Sabit | 60'LIK DUVAR OMEGASI | — |
| C31 | Sabit | 60*2500 | — |
| D31 | Formül | =ROUNDUP(IF('BİNA BİLGİLERİ'!B7=2,((('BİNA BİLGİLERİ'!B5/0.62)+1)*'BİNA BİLGİLERİ'!B6)/2.5,0)+IF('BİNA BİLGİLERİ'!B14="AŞIK OMEGA",'BİNA BİLGİLERİ'!D15,0),0) | 0 |
| E31 | Formül | =K31*1.73 | 247.14261 |
| F31 | Formül | =E31*D31 | 0 |
| I31 | Formül | =2.75*O2*$G$2 | 119.04750000000001 |
| J31 | Formül | =I31*D31 | 0 |
| K31 | Formül | =I31*1.2 | 142.857 |
| L31 | Formül | =E31-E31*$E$223/100 | 185.3569575 |
| M31 | Formül | =(L31-K31)/K31 | 0.29749999999999993 |
| A32 | Sabit | 18 | — |
| B32 | Sabit | 60'LIK DUVAR OMEGASI | — |
| C32 | Sabit | 60*3760 | — |
| E32 | Formül | =K32*1.73 | 370.2645648 |
| F32 | Formül | =E32*D32 | 0 |
| I32 | Formül | =4.12*O2*$G$2 | 178.3548 |
| J32 | Formül | =I32*D32 | 0 |
| K32 | Formül | =I32*1.2 | 214.02576000000002 |
| L32 | Formül | =E32-E32*$E$223/100 | 277.6984236 |
| M32 | Formül | =(L32-K32)/K32 | 0.29749999999999993 |
| A33 | Sabit | 19 | — |
| B33 | Sabit | 60'LIK DUVAR OMEGASI | — |
| C33 | Sabit | 60*5000 | — |
| E33 | Formül | =K33*1.73 | 494.28522 |
| F33 | Formül | =E33*D33 | 0 |
| I33 | Formül | =5.5*O2*$G$2 | 238.09500000000003 |
| J33 | Formül | =I33*D33 | 0 |
| K33 | Formül | =I33*1.2 | 285.714 |
| L33 | Formül | =E33-E33*$E$223/100 | 370.713915 |
| M33 | Formül | =(L33-K33)/K33 | 0.29749999999999993 |
| D34 | Sabit | Z1 | — |
| A35 | Sabit | 20 | — |
| B35 | Sabit | ÇATI AŞIK OMEGASI | — |
| C35 | Sabit | L:4200 | — |
| E35 | Formül | =3.5*O2*1.2*$G$2*1.73 | 314.54513999999995 |
| F35 | Formül | =E35*D35 | 0 |
| I35 | Formül | =3.5*O2*$G$2 | 151.515 |
| J35 | Formül | =I35*D35 | 0 |
| K35 | Formül | =I35*1.2 | 181.81799999999998 |
| L35 | Formül | =E35-E35*$E$223/100 | 235.90885499999996 |
| M35 | Formül | =(L35-K35)/K35 | 0.2974999999999999 |
| A36 | Sabit | 21 | — |
| B36 | Sabit | ÇATI AŞIK OMEGASI | — |
| C36 | Sabit | L:3000 | — |
| D36 | Formül | =IF('BİNA BİLGİLERİ'!B14="AŞIK OMEGA",'BİNA BİLGİLERİ'!C15,0) | 0 |
| E36 | Formül | =2.5*O2*1.2*$G$2*1.73 | 224.67510000000001 |
| F36 | Formül | =E36*D36 | 0 |
| I36 | Formül | =2.5*O2*$G$2 | 108.22500000000001 |
| J36 | Formül | =I36*D36 | 0 |
| K36 | Formül | =I36*1.2 | 129.87 |
| L36 | Formül | =E36-E36*$E$223/100 | 168.506325 |
| M36 | Formül | =(L36-K36)/K36 | 0.2975 |
| D37 | Sabit | TOPLAM | — |
| F37 | Formül | =SUM(F10:F36) | 342767.98497840005 |
| I37 | Formül | =SUM(I10:I36) | 3276.1758449999998 |
| J37 | Formül | =SUM(J10:J36) | 147718.3234 |
| K37 | Formül | =SUM(K10:K36) | 3931.4110139999993 |
| L37 | Formül | =SUM(L10:L36) | 5180.144260665 |
| A38 | Sabit |                         ALÇIPAN VE KAPLAMA GRUBU | — |
| D38 | Sabit | 1002 | — |
| A39 | Sabit | 1 | — |
| B39 | Sabit | BEYAZ  ALÇIPAN | — |
| C39 | Sabit | 12x1200x2500 | — |
| D39 | Formül | =(ROUNDUP((IF('BİNA BİLGİLERİ'!D32="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!E32="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!F32="ALÇIPAN",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/3,0)+(ROUNDUP((IF('BİNA BİLGİLERİ'!D33="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!E33="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!F33="ALÇIPAN",1,0))*'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9/3,0)*2)+ROUNDUP((IF('BİNA BİLGİLERİ'!D34="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!E34="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!F34="ALÇIPAN",1,0))*'BİNA BİLGİLERİ'!B34/3,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D35="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!E35="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!F35="ALÇIPAN",1,0))*'BİNA BİLGİLERİ'!B35/3,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!E33="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!F33="ALÇIPAN",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/3,0))-D40 | 108 |
| E39 | Formül | =K39*1.73 | 371.604 |
| F39 | Formül | =E39*D39 | 40133.231999999996 |
| I39 | Sabit | 179 | — |
| J39 | Formül | =I39*D39 | 19332 |
| K39 | Formül | =I39*1.2 | 214.79999999999998 |
| L39 | Formül | =E39-E39*$E$223/100 | 278.703 |
| M39 | Formül | =(L39-K39)/K39 | 0.2975 |
| A40 | Sabit | 2 | — |
| B40 | Sabit | YEŞİL ALÇIPAN | — |
| C40 | Sabit | 12x1200x2500 | — |
| D40 | Formül | =ROUNDUP((IF('BİNA BİLGİLERİ'!D35="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!E35="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!F35="ALÇIPAN",1,0))*'BİNA BİLGİLERİ'!B30/3,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!E33="ALÇIPAN",1,0)+IF('BİNA BİLGİLERİ'!F33="ALÇIPAN",1,0))*('BİNA BİLGİLERİ'!B29*'BİNA BİLGİLERİ'!B9)/3,0) | 24 |
| E40 | Formül | =K40*1.73 | 544.95 |
| F40 | Formül | =E40*D40 | 13078.800000000001 |
| I40 | Formül | =315/1.2 | 262.5 |
| J40 | Formül | =I40*D40 | 6300 |
| K40 | Formül | =I40*1.2 | 315 |
| L40 | Formül | =E40-E40*$E$223/100 | 408.71250000000003 |
| M40 | Formül | =(L40-K40)/K40 | 0.2975000000000001 |
| A41 | Sabit | 3 | — |
| B41 | Sabit | BORDEX | — |
| C41 | Sabit | 12x1200x2500 | — |
| D41 | Formül | =ROUNDUP((IF('BİNA BİLGİLERİ'!D32="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!E32="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!F32="BORDEX",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/2.88,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!E33="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!F33="BORDEX",1,0))*'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9/2.88,0)*2+ROUNDUP((IF('BİNA BİLGİLERİ'!D34="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!E34="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!F34="BORDEX",1,0))*'BİNA BİLGİLERİ'!B34/2.88,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D35="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!E35="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!F35="BORDEX",1,0))*'BİNA BİLGİLERİ'!B35/2.88,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!E33="BORDEX",1,0)+IF('BİNA BİLGİLERİ'!F33="BORDEX",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/2.88,0)+IF('BİNA BİLGİLERİ'!D32="BORDEX",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="BORDEX",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="BORDEX",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),0))) | 57 |
| E41 | Formül | =K41*1.73 | 973.99 |
| F41 | Formül | =E41*D41 | 55517.43 |
| I41 | Formül | =563/1.2 | 469.1666666666667 |
| J41 | Formül | =I41*D41 | 26742.5 |
| K41 | Formül | =I41*1.2 | 563 |
| L41 | Formül | =E41-E41*$E$223/100 | 730.4925000000001 |
| M41 | Formül | =(L41-K41)/K41 | 0.2975000000000001 |
| A42 | Sabit | 4 | — |
| B42 | Sabit | 11 mm  OSB 2 | — |
| C42 | Sabit | 11*122*244 | — |
| D42 | Formül | =ROUNDUP((IF('BİNA BİLGİLERİ'!D32="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!E32="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!F32="11 mm OSB 2",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/2.97,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!E33="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!F33="11 mm OSB 2",1,0))*'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9/2.97,0)*2+ROUNDUP((IF('BİNA BİLGİLERİ'!D34="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!E34="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!F34="11 mm OSB 2",1,0))*'BİNA BİLGİLERİ'!B34/2.97,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D35="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!E35="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!F35="11 mm OSB 2",1,0))*'BİNA BİLGİLERİ'!B35/2.97,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!E33="11 mm OSB 2",1,0)+IF('BİNA BİLGİLERİ'!F33="11 mm OSB 2",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/2.97,0) | 183 |
| E42 | Formül | =K42*1.73 | 1036.27 |
| F42 | Formül | =E42*D42 | 189637.41 |
| I42 | Formül | =599/1.2 | 499.1666666666667 |
| J42 | Formül | =I42*D42 | 91347.5 |
| K42 | Formül | =I42*1.2 | 599 |
| L42 | Formül | =E42-E42*$E$223/100 | 777.2025 |
| M42 | Formül | =(L42-K42)/K42 | 0.2975 |
| A43 | Sabit | 5 | — |
| B43 | Sabit | YALIBASKI SİDİNG FİBERCEMENT | — |
| C43 | Sabit | 8*200*2500 | — |
| D43 | Formül | =ROUNDUP((IF('BİNA BİLGİLERİ'!D32="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E32="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F32="YALIBASKI SİDİNG FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/0.5,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E33="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F33="YALIBASKI SİDİNG FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9/0.5,0)*2+ROUNDUP((IF('BİNA BİLGİLERİ'!D34="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E34="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F34="YALIBASKI SİDİNG FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B34/0.5,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D35="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E35="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F35="YALIBASKI SİDİNG FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B35/0.5,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E33="YALIBASKI SİDİNG FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F33="YALIBASKI SİDİNG FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/0.5,0) | 0 |
| E43 | Formül | =K43*1.73 | 449.8 |
| F43 | Formül | =E43*D43 | 0 |
| I43 | Formül | =260/1.2 | 216.66666666666669 |
| J43 | Formül | =I43*D43 | 0 |
| K43 | Formül | =I43*1.2 | 260 |
| L43 | Formül | =E43-E43*$E$223/100 | 337.35 |
| M43 | Formül | =(L43-K43)/K43 | 0.2975000000000001 |
| A44 | Sabit | 6 | — |
| B44 | Sabit | AĞAÇDESEN FUGALI FİBERCEMENT | — |
| C44 | Sabit | 12*385*2500 | — |
| D44 | Formül | =ROUNDUP((IF('BİNA BİLGİLERİ'!D32="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E32="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F32="AĞAÇDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/0.96,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E33="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F33="AĞAÇDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9/0.96,0)*2+ROUNDUP((IF('BİNA BİLGİLERİ'!D34="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E34="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F34="AĞAÇDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B34/0.97,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D35="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E35="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F35="AĞAÇDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B35/0.97,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E33="AĞAÇDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F33="AĞAÇDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/0.97,0) | 0 |
| E44 | Formül | =K44*1.73 | 1086.44 |
| F44 | Formül | =E44*D44 | 0 |
| I44 | Formül | =628/1.2 | 523.3333333333334 |
| J44 | Formül | =I44*D44 | 0 |
| K44 | Formül | =I44*1.2 | 628 |
| L44 | Formül | =E44-E44*$E$223/100 | 814.83 |
| M44 | Formül | =(L44-K44)/K44 | 0.29750000000000004 |
| A45 | Sabit | 7 | — |
| B45 | Sabit | TAŞDESEN FUGALI FİBERCEMENT | — |
| C45 | Sabit | 12*385*2500 | — |
| D45 | Formül | =ROUNDUP((IF('BİNA BİLGİLERİ'!D32="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E32="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F32="TAŞDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/0.96,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E33="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F33="TAŞDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9/0.96,0)*2+ROUNDUP((IF('BİNA BİLGİLERİ'!D34="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E34="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F34="TAŞDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B34/0.97,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D35="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E35="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F35="TAŞDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B35/0.97,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!E33="TAŞDESEN FUGALI FİBERCEMENT",1,0)+IF('BİNA BİLGİLERİ'!F33="TAŞDESEN FUGALI FİBERCEMENT",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/0.97,0) | 0 |
| E45 | Formül | =K45*1.73 | 1131.42 |
| F45 | Formül | =E45*D45 | 0 |
| I45 | Formül | =654/1.2 | 545 |
| J45 | Formül | =I45*D45 | 0 |
| K45 | Formül | =I45*1.2 | 654 |
| L45 | Formül | =E45-E45*$E$223/100 | 848.565 |
| M45 | Formül | =(L45-K45)/K45 | 0.2975000000000001 |
| A46 | Sabit | 8 | — |
| B46 | Sabit | AHŞAP DESEN LEVHA | — |
| C46 | Sabit | 8x1250x3000 | — |
| D46 | Formül | =ROUNDUP((IF('BİNA BİLGİLERİ'!D32="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E32="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F32="AHŞAP DESEN LEVHA",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/3.75,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E33="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F33="AHŞAP DESEN LEVHA",1,0))*'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9/3.75,0)*2+ROUNDUP((IF('BİNA BİLGİLERİ'!D34="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E34="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F34="AHŞAP DESEN LEVHA",1,0))*'BİNA BİLGİLERİ'!B34/3.75,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D35="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E35="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F35="AHŞAP DESEN LEVHA",1,0))*'BİNA BİLGİLERİ'!B35/3.75,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E33="AHŞAP DESEN LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F33="AHŞAP DESEN LEVHA",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/3.75,0) | 0 |
| E46 | Formül | =K46*1.73 | 1672.91 |
| F46 | Formül | =E46*D46 | 0 |
| I46 | Formül | =967/1.2 | 805.8333333333334 |
| J46 | Formül | =I46*D46 | 0 |
| K46 | Formül | =I46*1.2 | 967 |
| L46 | Formül | =E46-E46*$E$223/100 | 1254.6825000000001 |
| M46 | Formül | =(L46-K46)/K46 | 0.2975000000000001 |
| A47 | Sabit | 9 | — |
| B47 | Sabit | FİBERCEMENT LEVHA (ARAKAT İÇİN) | — |
| C47 | Sabit | 16x1250x2500 | — |
| D47 | Formül | =IF('BİNA BİLGİLERİ'!B7="2",ROUNDUP('BİNA BİLGİLERİ'!B8/3.125,0)+2,0) | 0 |
| E47 | Formül | =K47*1.73 | 2159.04 |
| F47 | Formül | =E47*D47 | 0 |
| I47 | Formül | =1248/1.2 | 1040 |
| J47 | Formül | =I47*D47 | 0 |
| K47 | Formül | =I47*1.2 | 1248 |
| L47 | Formül | =E47-E47*$E$223/100 | 1619.28 |
| M47 | Formül | =(L47-K47)/K47 | 0.2975 |
| A48 | Sabit | 10 | — |
| B48 | Sabit | FİBERCEMENT LEVHA | — |
| C48 | Sabit | 8x1250x2500 | — |
| D48 | Formül | =ROUNDUP((IF('BİNA BİLGİLERİ'!D32="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E32="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F32="FİBERCEMENT LEVHA",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/3.125,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E33="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F33="FİBERCEMENT LEVHA",1,0))*'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9/3.125,0)*2+ROUNDUP((IF('BİNA BİLGİLERİ'!D34="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E34="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F34="FİBERCEMENT LEVHA",1,0))*'BİNA BİLGİLERİ'!B34/3.125,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D35="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E35="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F35="FİBERCEMENT LEVHA",1,0))*'BİNA BİLGİLERİ'!B35/3.125,0)+ROUNDUP((IF('BİNA BİLGİLERİ'!D33="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!E33="FİBERCEMENT LEVHA",1,0)+IF('BİNA BİLGİLERİ'!F33="FİBERCEMENT LEVHA",1,0))*'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9/3.125,0) | 0 |
| E48 | Formül | =K48*1.73 | 1079.52 |
| F48 | Formül | =E48*D48 | 0 |
| I48 | Formül | =624/1.2 | 520 |
| J48 | Formül | =I48*D48 | 0 |
| K48 | Formül | =I48*1.2 | 624 |
| L48 | Formül | =E48-E48*$E$223/100 | 809.64 |
| M48 | Formül | =(L48-K48)/K48 | 0.2975 |
| A49 | Sabit | 11 | — |
| B49 | Sabit | KÖŞE SÖVESİ FİBERCEMENT KAPLAMA | — |
| C49 | Sabit | 16x150x2800 | — |
| D49 | Formül | =IF('BİNA BİLGİLERİ'!D32="YALIBASKI SİDİNG FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!D32="AĞAÇDESEN FUGALI FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!D32="TAŞDESEN FUGALI FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!D32="AHŞAP DESEN LEVHA",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!D32="FİBERCEMENT LEVHA",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!E32="YALIBASKI SİDİNG FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!E32="AĞAÇDESEN FUGALI FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!E32="TAŞDESEN FUGALI FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!E32="AHŞAP DESEN LEVHA",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!E32="FİBERCEMENT LEVHA",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!F32="YALIBASKI SİDİNG FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!F32="AĞAÇDESEN FUGALI FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!F32="TAŞDESEN FUGALI FİBERCEMENT",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!F32="AHŞAP DESEN LEVHA",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,IF('BİNA BİLGİLERİ'!F32="FİBERCEMENT LEVHA",(('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B7+0.3)*'BİNA BİLGİLERİ'!B4)/2.8,0))))))))))))))) | 0 |
| E49 | Formül | =K49*1.73 | 363.3 |
| F49 | Formül | =E49*D49 | 0 |
| I49 | Formül | =210/1.2 | 175 |
| J49 | Formül | =I49*D49 | 0 |
| K49 | Formül | =I49*1.2 | 210 |
| L49 | Formül | =E49-E49*$E$223/100 | 272.475 |
| M49 | Formül | =(L49-K49)/K49 | 0.2975000000000001 |
| A50 | Sabit | 12 | — |
| B50 | Sabit | PENCERE KENAR SÖVESİ FİBERCEMENT KAPLAMA | — |
| C50 | Sabit | 16x120x2500 | — |
| D50 | Formül | =IF('BİNA BİLGİLERİ'!D32="YALIBASKI SİDİNG FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!D32="AĞAÇDESEN FUGALI FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!D32="TAŞDESEN FUGALI FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!D32="AHŞAP DESEN LEVHA",Q82,IF('BİNA BİLGİLERİ'!D32="FİBERCEMENT LEVHA",Q82,IF('BİNA BİLGİLERİ'!E32="YALIBASKI SİDİNG FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!E32="AĞAÇDESEN FUGALI FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!E32="TAŞDESEN FUGALI FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!E32="AHŞAP DESEN LEVHA",Q82,IF('BİNA BİLGİLERİ'!E32="FİBERCEMENT LEVHA",Q82,IF('BİNA BİLGİLERİ'!F32="YALIBASKI SİDİNG FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!F32="AĞAÇDESEN FUGALI FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!F32="TAŞDESEN FUGALI FİBERCEMENT",Q82,IF('BİNA BİLGİLERİ'!F32="AHŞAP DESEN LEVHA",Q82,IF('BİNA BİLGİLERİ'!F32="FİBERCEMENT LEVHA",Q82,0))))))))))))))) | 0 |
| E50 | Formül | =K50*1.73 | 363.3 |
| F50 | Formül | =E50*D50 | 0 |
| I50 | Formül | =210/1.2 | 175 |
| J50 | Formül | =I50*D50 | 0 |
| K50 | Formül | =I50*1.2 | 210 |
| L50 | Formül | =E50-E50*$E$223/100 | 272.475 |
| M50 | Formül | =(L50-K50)/K50 | 0.2975000000000001 |
| A51 | Sabit | 13 | — |
| B51 | Sabit | SAÇAK VE ALIN  FİBERCEMENT KAPLAMA | — |
| C51 | Sabit | 16x200x2500 | — |
| D51 | Formül | =IF('BİNA BİLGİLERİ'!D32="YALIBASKI SİDİNG FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!D32="AĞAÇDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!D32="TAŞDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!D32="AHŞAP DESEN LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!D32="FİBERCEMENT LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="YALIBASKI SİDİNG FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="AĞAÇDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="TAŞDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="AHŞAP DESEN LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="FİBERCEMENT LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="YALIBASKI SİDİNG FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="AĞAÇDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="TAŞDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="AHŞAP DESEN LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="FİBERCEMENT LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),0))))))))))))))) | 0 |
| E51 | Formül | =K51*1.73 | 467.1 |
| F51 | Formül | =E51*D51 | 0 |
| I51 | Formül | =270/1.2 | 225 |
| J51 | Formül | =I51*D51 | 0 |
| K51 | Formül | =I51*1.2 | 270 |
| L51 | Formül | =E51-E51*$E$223/100 | 350.32500000000005 |
| M51 | Formül | =(L51-K51)/K51 | 0.29750000000000015 |
| A52 | Sabit | 14 | — |
| B52 | Sabit | SAÇAK VE ALIN ALTI FİBERCEMENT KAPLAMA | — |
| C52 | Sabit | 8x400x2500 | — |
| D52 | Formül | =IF('BİNA BİLGİLERİ'!D32="YALIBASKI SİDİNG FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!D32="AĞAÇDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!D32="TAŞDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!D32="AHŞAP DESEN LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!D32="FİBERCEMENT LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="YALIBASKI SİDİNG FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="AĞAÇDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="TAŞDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="AHŞAP DESEN LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!E32="FİBERCEMENT LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="YALIBASKI SİDİNG FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="AĞAÇDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="TAŞDESEN FUGALI FİBERCEMENT",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="AHŞAP DESEN LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),IF('BİNA BİLGİLERİ'!F32="FİBERCEMENT LEVHA",ROUNDUP(('BİNA BİLGİLERİ'!B17+'BİNA BİLGİLERİ'!B18)/2.4,0),0))))))))))))))) | 0 |
| E52 | Formül | =K52*1.73 | 467.1 |
| F52 | Formül | =E52*D52 | 0 |
| I52 | Formül | =270/1.2 | 225 |
| J52 | Formül | =I52*D52 | 0 |
| K52 | Formül | =I52*1.2 | 270 |
| L52 | Formül | =E52-E52*$E$223/100 | 350.32500000000005 |
| M52 | Formül | =(L52-K52)/K52 | 0.29750000000000015 |
| D53 | Sabit | TOPLAM | — |
| F53 | Formül | =SUBTOTAL(9,F39:F52) | 298366.872 |
| I53 | Formül | =SUBTOTAL(9,I39:I52) | 5860.666666666667 |
| J53 | Formül | =SUBTOTAL(9,J39:J52) | 143722 |
| K53 | Formül | =SUBTOTAL(9,K39:K52) | 7032.8 |
| L53 | Formül | =SUBTOTAL(9,L39:L52) | 9125.058 |
| A54 | Sabit |                                  ÇATI SACI GRUBU | — |
| D54 | Sabit | 1003 | — |
| A55 | Sabit | 1 | — |
| B55 | Sabit | TRAPEZ ÇATI | — |
| C55 | Sabit | 1000 | — |
| D55 | Formül | =IF('BİNA BİLGİLERİ'!B13="TRAPEZ ÇATI",'BİNA BİLGİLERİ'!B34,0) | 0 |
| E55 | Formül | =C55*4.9*P2*1.2*$G$2*1.73/1000 | 513.757062 |
| F55 | Formül | =E55*D55 | 0 |
| I55 | Formül | =C55*4.9*P2*G2/1000 | 247.4745 |
| J55 | Formül | =I55*D55 | 0 |
| K55 | Formül | =I55*1.2 | 296.9694 |
| L55 | Formül | =E55-E55*$E$223/100 | 385.3177965 |
| M55 | Formül | =(L55-K55)/K55 | 0.29749999999999993 |
| A56 | Sabit | 2 | — |
| B56 | Sabit | SANDVİÇ PANEL | — |
| C56 | Sabit | 1 | — |
| D56 | Formül | =IF('BİNA BİLGİLERİ'!B13="SANDVİÇ PANEL",'BİNA BİLGİLERİ'!B34,0) | 0 |
| E56 | Sabit | 10 | — |
| F56 | Formül | =E56*D56 | 0 |
| I56 | Formül | =C56*4.9*P2*G2/1000 | 0.24747450000000004 |
| J56 | Formül | =I56*D56 | 0 |
| K56 | Formül | =I56*1.2 | 0.29696940000000005 |
| L56 | Formül | =E56-E56*$E$223/100 | 7.5 |
| M56 | Formül | =(L56-K56)/K56 | 24.25512729594362 |
| A57 | Sabit | 3 | — |
| B57 | Sabit | METAL KİREMİT ÇATI  | — |
| C57 | Sabit | 1000 | — |
| D57 | Formül | =IF('BİNA BİLGİLERİ'!B13="METAL KİREMİT ÇATI ",'BİNA BİLGİLERİ'!B34,0) | 126.724896 |
| E57 | Formül | =C57*4.9*$P$2*1.2*$G$2*1.73/1000*1.15*1.05 | 620.3616523649999 |
| F57 | Formül | =E57*D57 | 78615.26587834278 |
| I57 | Formül | =C57*4.9*P2*G2/1000*1.05 | 259.848225 |
| J57 | Formül | =I57*D57 | 32929.2392889096 |
| K57 | Formül | =I57*1.2 | 311.81787 |
| L57 | Formül | =E57-E57*$E$223/100 | 465.27123927375 |
| M57 | Formül | =(L57-K57)/K57 | 0.4921249999999998 |
| A58 | Sabit | 4 | — |
| B58 | Sabit | ANTRASİT GRİ BOYALI ÇATI SACI - 1100 KAPATIR  | — |
| E58 | Formül | =C58*4.9*P2*1.2*$G$2*1.73/1000*1.05 | 0 |
| F58 | Formül | =E58*D58 | 0 |
| I58 | Formül | =C58*4.9*P2*G2/1000*1.05 | 0 |
| J58 | Formül | =I58*D58 | 0 |
| K58 | Formül | =I58*1.2 | 0 |
| L58 | Formül | =E58-E58*$E$223/100 | 0 |
| M58 | Formül | =(L58-K58)/K58 | #DIV/0! |
| A59 | Sabit | 5 | — |
| B59 | Sabit | KIRMIZI BOYALI SAC MAHYA | — |
| C59 | Sabit | L:1100 | — |
| E59 | Formül | =K59*1.73 | 516.924 |
| F59 | Formül | =E59*D59 | 0 |
| I59 | Sabit | 249 | — |
| J59 | Formül | =I59*D59 | 0 |
| K59 | Formül | =I59*1.2 | 298.8 |
| L59 | Formül | =E59-E59*$E$223/100 | 387.693 |
| M59 | Formül | =(L59-K59)/K59 | 0.2974999999999999 |
| A60 | Sabit | 5 | — |
| B60 | Sabit | ANTRASİT GRİ BOYALI SAC MAHYA | — |
| C60 | Sabit | L:1100 | — |
| E60 | Formül | =K60*1.73 | 537.684 |
| F60 | Formül | =E60*D60 | 0 |
| I60 | Sabit | 259 | — |
| J60 | Formül | =I60*D60 | 0 |
| K60 | Formül | =I60*1.2 | 310.8 |
| L60 | Formül | =E60-E60*$E$223/100 | 403.263 |
| M60 | Formül | =(L60-K60)/K60 | 0.2974999999999999 |
| A61 | Sabit | 6 | — |
| B61 | Sabit | METAL KİREMİT ÇATI  | — |
| C61 | Sabit | 1000 | — |
| E61 | Formül | =C61*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 590.8206213 |
| F61 | Formül | =E61*D61 | 0 |
| I61 | Formül | =C61*4.9*$P$2*$G$2/1000*1.15 | 284.59567499999997 |
| J61 | Formül | =I61*D61 | 0 |
| K61 | Formül | =I61*1.2 | 341.51480999999995 |
| L61 | Formül | =E61-E61*$E$223/100 | 443.115465975 |
| M61 | Formül | =(L61-K61)/K61 | 0.2975000000000002 |
| A62 | Sabit | 7 | — |
| B62 | Sabit | KIRMIZI METAL KİREMİT | — |
| C62 | Sabit | 1522 | — |
| E62 | Formül | =C62*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 899.2289856185998 |
| F62 | Formül | =E62*D62 | 0 |
| I62 | Formül | =C62*4.9*$P$2*$G$2/1000*1.15 | 433.15461735 |
| J62 | Formül | =I62*D62 | 0 |
| K62 | Formül | =I62*1.2 | 519.78554082 |
| L62 | Formül | =E62-E62*$E$223/100 | 674.4217392139499 |
| M62 | Formül | =(L62-K62)/K62 | 0.29749999999999965 |
| A63 | Sabit | 8 | — |
| B63 | Sabit | KIRMIZI METAL KİREMİT | — |
| C63 | Sabit | 1872 | — |
| E63 | Formül | =C63*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 1106.0162030736 |
| F63 | Formül | =E63*D63 | 0 |
| I63 | Formül | =C63*4.9*$P$2*$G$2/1000*1.15 | 532.7631036000001 |
| J63 | Formül | =I63*D63 | 0 |
| K63 | Formül | =I63*1.2 | 639.3157243200002 |
| L63 | Formül | =E63-E63*$E$223/100 | 829.5121523052001 |
| M63 | Formül | =(L63-K63)/K63 | 0.2974999999999997 |
| A64 | Sabit | 9 | — |
| B64 | Sabit | KIRMIZI METAL KİREMİT | — |
| C64 | Sabit | 2222 | — |
| E64 | Formül | =C64*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 1312.8034205286 |
| F64 | Formül | =E64*D64 | 0 |
| I64 | Formül | =C64*4.9*$P$2*$G$2/1000*1.15 | 632.3715898500002 |
| J64 | Formül | =I64*D64 | 0 |
| K64 | Formül | =I64*1.2 | 758.8459078200002 |
| L64 | Formül | =E64-E64*$E$223/100 | 984.60256539645 |
| M64 | Formül | =(L64-K64)/K64 | 0.29749999999999965 |
| A65 | Sabit | 10 | — |
| B65 | Sabit | KIRMIZI METAL KİREMİT | — |
| C65 | Sabit | 2572 | — |
| E65 | Formül | =C65*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 1519.5906379836001 |
| F65 | Formül | =E65*D65 | 0 |
| I65 | Formül | =C65*4.9*$P$2*$G$2/1000*1.15 | 731.9800761 |
| J65 | Formül | =I65*D65 | 0 |
| K65 | Formül | =I65*1.2 | 878.37609132 |
| L65 | Formül | =E65-E65*$E$223/100 | 1139.6929784877002 |
| M65 | Formül | =(L65-K65)/K65 | 0.2975000000000002 |
| A66 | Sabit | 11 | — |
| B66 | Sabit | KIRMIZI METAL KİREMİT | — |
| C66 | Sabit | 2922 | — |
| E66 | Formül | =C66*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 1726.3778554386004 |
| F66 | Formül | =E66*D66 | 0 |
| I66 | Formül | =C66*4.9*$P$2*$G$2/1000*1.15 | 831.5885623500001 |
| J66 | Formül | =I66*D66 | 0 |
| K66 | Formül | =I66*1.2 | 997.90627482 |
| L66 | Formül | =E66-E66*$E$223/100 | 1294.7833915789504 |
| M66 | Formül | =(L66-K66)/K66 | 0.2975000000000004 |
| A67 | Sabit | 12 | — |
| B67 | Sabit | KIRMIZI METAL KİREMİT | — |
| C67 | Sabit | 3227 | — |
| E67 | Formül | =C67*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 1906.5781449351 |
| F67 | Formül | =E67*D67 | 0 |
| I67 | Formül | =C67*4.9*$P$2*$G$2/1000*1.15 | 918.3902432250001 |
| J67 | Formül | =I67*D67 | 0 |
| K67 | Formül | =I67*1.2 | 1102.06829187 |
| L67 | Formül | =E67-E67*$E$223/100 | 1429.933608701325 |
| M67 | Formül | =(L67-K67)/K67 | 0.2975000000000001 |
| A68 | Sabit | 13 | — |
| B68 | Sabit | KIRMIZI METAL KİREMİT | — |
| C68 | Sabit | 3622 | — |
| E68 | Formül | =C68*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 2139.9522903486004 |
| F68 | Formül | =E68*D68 | 0 |
| I68 | Formül | =C68*4.9*$P$2*$G$2/1000*1.15 | 1030.80553485 |
| J68 | Formül | =I68*D68 | 0 |
| K68 | Formül | =I68*1.2 | 1236.96664182 |
| L68 | Formül | =E68-E68*$E$223/100 | 1604.9642177614503 |
| M68 | Formül | =(L68-K68)/K68 | 0.2975000000000003 |
| A69 | Sabit | 14 | — |
| B69 | Sabit | KIRMIZI METAL KİREMİT | — |
| C69 | Sabit | 3970 | — |
| E69 | Formül | =C69*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 2345.557866561 |
| F69 | Formül | =E69*D69 | 0 |
| I69 | Formül | =C69*4.9*$P$2*$G$2/1000*1.15 | 1129.8448297500001 |
| J69 | Formül | =I69*D69 | 0 |
| K69 | Formül | =I69*1.2 | 1355.8137957000001 |
| L69 | Formül | =E69-E69*$E$223/100 | 1759.16839992075 |
| M69 | Formül | =(L69-K69)/K69 | 0.29749999999999993 |
| A70 | Sabit | 15 | — |
| B70 | Sabit | KIRMIZI METAL KİREMİT | — |
| C70 | Sabit | 4322 | — |
| E70 | Formül | =C70*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 2553.5267252586 |
| F70 | Formül | =E70*D70 | 0 |
| I70 | Formül | =C70*4.9*$P$2*$G$2/1000*1.15 | 1230.0225073499998 |
| J70 | Formül | =I70*D70 | 0 |
| K70 | Formül | =I70*1.2 | 1476.0270088199998 |
| L70 | Formül | =E70-E70*$E$223/100 | 1915.14504394395 |
| M70 | Formül | =(L70-K70)/K70 | 0.2975000000000002 |
| A71 | Sabit | 16 | — |
| B71 | Sabit | KIRMIZI METAL KİREMİT | — |
| C71 | Sabit | 4672 | — |
| E71 | Formül | =C71*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 2760.3139427136 |
| F71 | Formül | =E71*D71 | 0 |
| I71 | Formül | =C71*4.9*$P$2*$G$2/1000*1.15 | 1329.6309936 |
| J71 | Formül | =I71*D71 | 0 |
| K71 | Formül | =I71*1.2 | 1595.55719232 |
| L71 | Formül | =E71-E71*$E$223/100 | 2070.2354570352 |
| M71 | Formül | =(L71-K71)/K71 | 0.29749999999999993 |
| A72 | Sabit | 17 | — |
| B72 | Sabit | KIRMIZI METAL KİREMİT | — |
| C72 | Sabit | 5022 | — |
| E72 | Formül | =C72*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 2967.101160168601 |
| F72 | Formül | =E72*D72 | 0 |
| I72 | Formül | =C72*4.9*$P$2*$G$2/1000*1.15 | 1429.2394798500002 |
| J72 | Formül | =I72*D72 | 0 |
| K72 | Formül | =I72*1.2 | 1715.0873758200003 |
| L72 | Formül | =E72-E72*$E$223/100 | 2225.3258701264504 |
| M72 | Formül | =(L72-K72)/K72 | 0.2975 |
| A73 | Sabit | 18 | — |
| B73 | Sabit | KIRMIZI METAL KİREMİT | — |
| C73 | Sabit | 5372 | — |
| E73 | Formül | =C73*4.9*$P$2*1.2*$G$2*1.73/1000*1.15 | 3173.8883776236003 |
| F73 | Formül | =E73*D73 | 0 |
| I73 | Formül | =C73*4.9*$P$2*$G$2/1000*1.15 | 1528.8479661000003 |
| J73 | Formül | =I73*D73 | 0 |
| K73 | Formül | =I73*1.2 | 1834.6175593200003 |
| L73 | Formül | =E73-E73*$E$223/100 | 2380.4162832177 |
| M73 | Formül | =(L73-K73)/K73 | 0.2974999999999997 |
| D74 | Sabit | TOPLAM | — |
| F74 | Formül | =SUBTOTAL(9,F55:F73) | 78615.26587834278 |
| I74 | Formül | =SUBTOTAL(9,I55:I73) | 13058.805378475 |
| J74 | Formül | =SUBTOTAL(9,J55:J73) | 32929.2392889096 |
| K74 | Formül | =SUBTOTAL(9,K55:K73) | 15670.56645417 |
| L74 | Formül | =SUBTOTAL(9,L55:L73) | 20400.362209437826 |
| A75 | Sabit |                           KAPI VE PENCERE GRUBU | — |
| D75 | Sabit | 1004 | — |
| A76 | Sabit | 1 | — |
| B76 | Sabit | GENİŞ KASA ÇELİK KAPI  - Sol | — |
| C76 | Sabit | 90*205 | — |
| D76 | Formül | ='BİNA BİLGİLERİ'!B39 | 1 |
| E76 | Formül | =K76*1.73 | 13494 |
| F76 | Formül | =E76*D76 | 13494 |
| I76 | Sabit | 6500 | — |
| J76 | Formül | =I76*D76 | 6500 |
| K76 | Formül | =I76*1.2 | 7800 |
| L76 | Formül | =E76-E76*$E$223/100 | 10120.5 |
| M76 | Formül | =(L76-K76)/K76 | 0.2975 |
| A77 | Sabit | 2 | — |
| B77 | Sabit | L KASA PVC KAPI+PERVAZ+MATİK+KOL   Sağ - Sol | — |
| C77 | Sabit | 90*200 | — |
| D77 | Formül | ='BİNA BİLGİLERİ'!B40 | 0 |
| E77 | Formül | =K77*1.73 | 8978.7 |
| F77 | Formül | =E77*D77 | 0 |
| I77 | Sabit | 4325 | — |
| J77 | Formül | =I77*D77 | 0 |
| K77 | Formül | =I77*1.2 | 5190 |
| L77 | Formül | =E77-E77*$E$223/100 | 6734.025000000001 |
| M77 | Formül | =(L77-K77)/K77 | 0.2975000000000001 |
| A78 | Sabit | 3 | — |
| B78 | Sabit | L KASA DUBLE PVC KAPI +PERVAZ+MATİK+ KOL  | — |
| C78 | Sabit | 160*200 | — |
| D78 | Formül | ='BİNA BİLGİLERİ'!B41 | 0 |
| E78 | Formül | =K78*1.73 | 16161.66 |
| F78 | Formül | =E78*D78 | 0 |
| I78 | Formül | =4325*1.8 | 7785 |
| J78 | Formül | =I78*D78 | 0 |
| K78 | Formül | =I78*1.2 | 9342 |
| L78 | Formül | =E78-E78*$E$223/100 | 12121.244999999999 |
| M78 | Formül | =(L78-K78)/K78 | 0.2974999999999999 |
| A79 | Sabit | 4 | — |
| B79 | Sabit | MELAMİN AHŞAP KASALI KAPI   Sol - Sağ | — |
| C79 | Sabit | 90*201 | — |
| D79 | Formül | ='BİNA BİLGİLERİ'!B42 | 6 |
| E79 | Formül | =K79*1.73 | 11210.4 |
| F79 | Formül | =E79*D79 | 67262.4 |
| I79 | Sabit | 5400 | — |
| J79 | Formül | =I79*D79 | 32400 |
| K79 | Formül | =I79*1.2 | 6480 |
| L79 | Formül | =E79-E79*$E$223/100 | 8407.8 |
| M79 | Formül | =(L79-K79)/K79 | 0.2974999999999999 |
| A80 | Sabit | 5 | — |
| B80 | Sabit | AHŞAP KASALI AMERİKAN KAPI Sol - Sağ | — |
| C80 | Sabit | 90*201 | — |
| D80 | Formül | ='BİNA BİLGİLERİ'!B43 | 0 |
| E80 | Formül | =K80*1.73 | 9549.6 |
| F80 | Formül | =E80*D80 | 0 |
| I80 | Sabit | 4600 | — |
| J80 | Formül | =I80*D80 | 0 |
| K80 | Formül | =I80*1.2 | 5520 |
| L80 | Formül | =E80-E80*$E$223/100 | 7162.200000000001 |
| M80 | Formül | =(L80-K80)/K80 | 0.29750000000000015 |
| D81 | Sabit | Z1 | — |
| A82 | Sabit | 6 | — |
| B82 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C82 | Sabit | 105/180 | — |
| D82 | Formül | ='BİNA BİLGİLERİ'!B46 | 0 |
| E82 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K82*1.73,K82*1.73*1.4) | 12320.2296 |
| F82 | Formül | =E82*D82 | 0 |
| I82 | Sabit | 4239 | — |
| J82 | Formül | =I82*D82 | 0 |
| K82 | Formül | =I82*1.2 | 5086.8 |
| L82 | Formül | =E82-E82*$E$223/100 | 9240.1722 |
| M82 | Formül | =(L82-K82)/K82 | 0.8165000000000001 |
| N82 | Formül | =1.05+1.8+1.05+1.8 | 5.7 |
| O82 | Formül | =N82*D82 | 0 |
| P82 | Formül | =ROUNDUP(O82,0) | 0 |
| Q82 | Formül | =SUM(P82:P93)/2.5 | 16 |
| A83 | Sabit | 7 | — |
| B83 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+SABİT CAM | — |
| C83 | Sabit | 59/180 | — |
| D83 | Formül | ='BİNA BİLGİLERİ'!B47 | 0 |
| E83 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K83*1.73,K83*1.73*1.4) | 6051.1248 |
| F83 | Formül | =E83*D83 | 0 |
| I83 | Sabit | 2082 | — |
| J83 | Formül | =I83*D83 | 0 |
| K83 | Formül | =I83*1.2 | 2498.4 |
| L83 | Formül | =E83-E83*$E$223/100 | 4538.3436 |
| M83 | Formül | =(L83-K83)/K83 | 0.8165 |
| N83 | Formül | =0.6+1.8+1.8+0.6 | 4.8 |
| O83 | Formül | =N83*D83 | 0 |
| P83 | Formül | =ROUNDUP(O83,0) | 0 |
| A84 | Sabit | 8 | — |
| B84 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C84 | Sabit | 80/120 | — |
| D84 | Formül | ='BİNA BİLGİLERİ'!B48 | 1 |
| E84 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K84*1.73,K84*1.73*1.4) | 7277.6255999999985 |
| F84 | Formül | =E84*D84 | 7277.6255999999985 |
| I84 | Sabit | 2504 | — |
| J84 | Formül | =I84*D84 | 2504 |
| K84 | Formül | =I84*1.2 | 3004.7999999999997 |
| L84 | Formül | =E84-E84*$E$223/100 | 5458.219199999999 |
| M84 | Formül | =(L84-K84)/K84 | 0.8164999999999997 |
| N84 | Formül | =0.8+1.2+0.8+1.2 | 4 |
| O84 | Formül | =N84*D84 | 4 |
| P84 | Formül | =ROUNDUP(O84,0) | 4 |
| A85 | Sabit | 9 | — |
| B85 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C85 | Sabit | 140/100 | — |
| D85 | Formül | ='BİNA BİLGİLERİ'!B49 | 2 |
| E85 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K85*1.73,K85*1.73*1.4) | 9547.524 |
| F85 | Formül | =E85*D85 | 19095.048 |
| I85 | Sabit | 3285 | — |
| J85 | Formül | =I85*D85 | 6570 |
| K85 | Formül | =I85*1.2 | 3942 |
| L85 | Formül | =E85-E85*$E$223/100 | 7160.643 |
| M85 | Formül | =(L85-K85)/K85 | 0.8165 |
| N85 | Formül | =1.4+1.4+2 | 4.8 |
| O85 | Formül | =N85*D85 | 9.6 |
| P85 | Formül | =ROUNDUP(O85,0) | 10 |
| A86 | Sabit | 10 | — |
| B86 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C86 | Sabit | 140/140 | — |
| D86 | Formül | ='BİNA BİLGİLERİ'!B50 | 0 |
| E86 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K86*1.73,K86*1.73*1.4) | 12203.9736 |
| F86 | Formül | =E86*D86 | 0 |
| I86 | Sabit | 4199 | — |
| J86 | Formül | =I86*D86 | 0 |
| K86 | Formül | =I86*1.2 | 5038.8 |
| L86 | Formül | =E86-E86*$E$223/100 | 9152.9802 |
| M86 | Formül | =(L86-K86)/K86 | 0.8164999999999999 |
| N86 | Formül | =1.4*4 | 5.6 |
| O86 | Formül | =N86*D86 | 0 |
| P86 | Formül | =ROUNDUP(O86,0) | 0 |
| A87 | Sabit | 11 | — |
| B87 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C87 | Sabit | 140/160 | — |
| D87 | Formül | ='BİNA BİLGİLERİ'!B51 | 0 |
| E87 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K87*1.73,K87*1.73*1.4) | 13532.1984 |
| F87 | Formül | =E87*D87 | 0 |
| I87 | Sabit | 4656 | — |
| J87 | Formül | =I87*D87 | 0 |
| K87 | Formül | =I87*1.2 | 5587.2 |
| L87 | Formül | =E87-E87*$E$223/100 | 10149.148799999999 |
| M87 | Formül | =(L87-K87)/K87 | 0.8164999999999999 |
| N87 | Formül | =1.4+1.6+1.4+1.6 | 6 |
| O87 | Formül | =N87*D87 | 0 |
| P87 | Formül | =ROUNDUP(O87,0) | 0 |
| A88 | Sabit | 12 | — |
| B88 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C88 | Sabit | 140/180 | — |
| D88 | Formül | ='BİNA BİLGİLERİ'!B52 | 0 |
| E88 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K88*1.73,K88*1.73*1.4) | 14863.3296 |
| F88 | Formül | =E88*D88 | 0 |
| I88 | Sabit | 5114 | — |
| J88 | Formül | =I88*D88 | 0 |
| K88 | Formül | =I88*1.2 | 6136.8 |
| L88 | Formül | =E88-E88*$E$223/100 | 11147.4972 |
| M88 | Formül | =(L88-K88)/K88 | 0.8164999999999999 |
| N88 | Formül | =1.4+1.8+1.4+1.8 | 6.3999999999999995 |
| O88 | Formül | =N88*D88 | 0 |
| P88 | Formül | =ROUNDUP(O88,0) | 0 |
| A89 | Sabit | 13 | — |
| B89 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C89 | Sabit | 160/120 | — |
| D89 | Formül | ='BİNA BİLGİLERİ'!B53 | 0 |
| E89 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K89*1.73,K89*1.73*1.4) | 12424.859999999999 |
| F89 | Formül | =E89*D89 | 0 |
| I89 | Sabit | 4275 | — |
| J89 | Formül | =I89*D89 | 0 |
| K89 | Formül | =I89*1.2 | 5130 |
| L89 | Formül | =E89-E89*$E$223/100 | 9318.645 |
| M89 | Formül | =(L89-K89)/K89 | 0.8165000000000001 |
| N89 | Formül | =1.6+1.2+1.6+1.2 | 5.6000000000000005 |
| O89 | Formül | =N89*D89 | 0 |
| P89 | Formül | =ROUNDUP(O89,0) | 0 |
| A90 | Sabit | 14 | — |
| B90 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C90 | Sabit | 160/160 | — |
| D90 | Formül | ='BİNA BİLGİLERİ'!B54 | 0 |
| E90 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K90*1.73,K90*1.73*1.4) | 15555.052799999998 |
| F90 | Formül | =E90*D90 | 0 |
| I90 | Sabit | 5352 | — |
| J90 | Formül | =I90*D90 | 0 |
| K90 | Formül | =I90*1.2 | 6422.4 |
| L90 | Formül | =E90-E90*$E$223/100 | 11666.289599999998 |
| M90 | Formül | =(L90-K90)/K90 | 0.8164999999999998 |
| N90 | Formül | =1.6+1.6+1.6+1.6 | 6.4 |
| O90 | Formül | =N90*D90 | 0 |
| P90 | Formül | =ROUNDUP(O90,0) | 0 |
| A91 | Sabit | 15 | — |
| B91 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+ÇİFT AÇILIM | — |
| C91 | Sabit | 160/180 | — |
| D91 | Formül | ='BİNA BİLGİLERİ'!B55 | 3 |
| E91 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K91*1.73,K91*1.73*1.4) | 17118.696 |
| F91 | Formül | =E91*D91 | 51356.088 |
| I91 | Sabit | 5890 | — |
| J91 | Formül | =I91*D91 | 17670 |
| K91 | Formül | =I91*1.2 | 7068 |
| L91 | Formül | =E91-E91*$E$223/100 | 12839.022 |
| M91 | Formül | =(L91-K91)/K91 | 0.8165000000000001 |
| N91 | Formül | =1.6+1.8+1.6+1.8 | 6.8 |
| O91 | Formül | =N91*D91 | 20.4 |
| P91 | Formül | =ROUNDUP(O91,0) | 21 |
| A92 | Sabit | 16 | — |
| B92 | Sabit | L KASA PVC PENCERE+PERVAZ+MATİK+SÜRGÜLÜ | — |
| C92 | Sabit | 180/200 | — |
| D92 | Formül | ='BİNA BİLGİLERİ'!B56 | 0 |
| E92 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K92*1.73,K92*1.73*1.4) | 21766.029599999998 |
| F92 | Formül | =E92*D92 | 0 |
| I92 | Sabit | 7489 | — |
| J92 | Formül | =I92*D92 | 0 |
| K92 | Formül | =I92*1.2 | 8986.8 |
| L92 | Formül | =E92-E92*$E$223/100 | 16324.5222 |
| M92 | Formül | =(L92-K92)/K92 | 0.8165000000000001 |
| N92 | Formül | =1.8+1.8+4 | 7.6 |
| O92 | Formül | =N92*D92 | 0 |
| P92 | Formül | =ROUNDUP(O92,0) | 0 |
| A93 | Sabit | 17 | — |
| B93 | Sabit | L KASA PVC VASİSTAS+PERVAZ+MATİK+SİNEKLİK | — |
| C93 | Sabit | 60/60 | — |
| D93 | Formül | ='BİNA BİLGİLERİ'!B57 | 2 |
| E93 | Formül | =IF('BİNA BİLGİLERİ'!$C$46="BEYAZ",K93*1.73,K93*1.73*1.4) | 3574.872 |
| F93 | Formül | =E93*D93 | 7149.744 |
| I93 | Sabit | 1230 | — |
| J93 | Formül | =I93*D93 | 2460 |
| K93 | Formül | =I93*1.2 | 1476 |
| L93 | Formül | =E93-E93*$E$223/100 | 2681.1539999999995 |
| M93 | Formül | =(L93-K93)/K93 | 0.8164999999999997 |
| N93 | Formül | =0.6*4 | 2.4 |
| O93 | Formül | =N93*D93 | 4.8 |
| P93 | Formül | =ROUNDUP(O93,0) | 5 |
| A94 | Sabit | 18 | — |
| B94 | Sabit | PVC PENCERE KOLU | — |
| C94 | Sabit | Adet | — |
| E94 | Sabit | 0 | — |
| F94 | Formül | =E94*D94 | 0 |
| I94 | Formül | =Y94*X94 | 0 |
| J94 | Formül | =I94*D94 | 0 |
| K94 | Formül | =I94*Y94 | 0 |
| L94 | Formül | =E94-E94*$E$223/100 | 0 |
| M94 | Formül | =(L94-K94)/K94 | #DIV/0! |
| D95 | Sabit | TOPLAM | — |
| F95 | Formül | =SUM(F76:F94) | 165634.9056 |
| I95 | Formül | =SUM(I76:I94) | 78925 |
| J95 | Formül | =SUM(J76:J94) | 68104 |
| K95 | Formül | =SUM(K76:K94) | 94710 |
| L95 | Formül | =SUM(L76:L94) | 154222.40700000004 |
| A96 | Sabit |                           ELEKTRİK TESİSAT GRUBU | — |
| D96 | Sabit | 1005 | — |
| A97 | Sabit | 1 | — |
| B97 | Sabit | KARE TAVAN  ARMATÜR | — |
| C97 | Sabit | ADET | — |
| E97 | Formül | =K97*1.73 | 386.13599999999997 |
| F97 | Formül | =E97*D97 | 0 |
| I97 | Sabit | 186 | — |
| J97 | Formül | =I97*D97 | 0 |
| K97 | Formül | =I97*1.2 | 223.2 |
| L97 | Formül | =E97-E97*$E$223/100 | 289.602 |
| M97 | Formül | =(L97-K97)/K97 | 0.29749999999999993 |
| A98 | Sabit | 2 | — |
| B98 | Sabit | SENSÖRLÜ ARMATÜR | — |
| C98 | Sabit | ADET | — |
| E98 | Formül | =K98*1.73 | 155.7 |
| F98 | Formül | =E98*D98 | 0 |
| I98 | Sabit | 75 | — |
| J98 | Formül | =I98*D98 | 0 |
| K98 | Formül | =I98*1.2 | 90 |
| L98 | Formül | =E98-E98*$E$223/100 | 116.77499999999999 |
| M98 | Formül | =(L98-K98)/K98 | 0.29749999999999993 |
| A99 | Sabit | 3 | — |
| B99 | Sabit | YUVARLAK İÇ GLOP | — |
| C99 | Sabit | ADET | — |
| E99 | Formül | =K99*1.73 | 156.07368000000002 |
| F99 | Formül | =E99*D99 | 0 |
| I99 | Sabit | 75.18 | — |
| J99 | Formül | =I99*D99 | 0 |
| K99 | Formül | =I99*1.2 | 90.21600000000001 |
| L99 | Formül | =E99-E99*$E$223/100 | 117.05526000000002 |
| M99 | Formül | =(L99-K99)/K99 | 0.2975000000000001 |
| A100 | Sabit | 4 | — |
| B100 | Sabit | OVAL DIŞ  GLOP  | — |
| C100 | Sabit | ADET | — |
| E100 | Formül | =K100*1.73 | 186.84 |
| F100 | Formül | =E100*D100 | 0 |
| I100 | Sabit | 90 | — |
| J100 | Formül | =I100*D100 | 0 |
| K100 | Formül | =I100*1.2 | 108 |
| L100 | Formül | =E100-E100*$E$223/100 | 140.13 |
| M100 | Formül | =(L100-K100)/K100 | 0.29749999999999993 |
| A101 | Sabit | 5 | — |
| B101 | Sabit | 2*36 LED ARMATÜR | — |
| C101 | Sabit | ADET | — |
| E101 | Formül | =K101*1.73 | 550.14 |
| F101 | Formül | =E101*D101 | 0 |
| I101 | Sabit | 265 | — |
| J101 | Formül | =I101*D101 | 0 |
| K101 | Formül | =I101*1.2 | 318 |
| L101 | Formül | =E101-E101*$E$223/100 | 412.605 |
| M101 | Formül | =(L101-K101)/K101 | 0.29750000000000004 |
| A102 | Sabit | 6 | — |
| B102 | Sabit | LED AMPUL | — |
| C102 | Sabit | ADET | — |
| E102 | Formül | =K102*1.73 | 24.081599999999998 |
| F102 | Formül | =E102*D102 | 0 |
| I102 | Sabit | 11.6 | — |
| J102 | Formül | =I102*D102 | 0 |
| K102 | Formül | =I102*1.2 | 13.92 |
| L102 | Formül | =E102-E102*$E$223/100 | 18.0612 |
| M102 | Formül | =(L102-K102)/K102 | 0.2975 |
| A103 | Sabit | 7 | — |
| B103 | Sabit | NYM KABLO (Kofra)                | — |
| C103 | Sabit | 3 x 4 | — |
| D103 | Formül | =IF('BİNA BİLGİLERİ'!B7="2",15,0) | 0 |
| E103 | Formül | =K103*1.73 | 181.00644 |
| F103 | Formül | =E103*D103 | 0 |
| I103 | Sabit | 87.19 | — |
| J103 | Formül | =I103*D103 | 0 |
| K103 | Formül | =I103*1.2 | 104.628 |
| L103 | Formül | =E103-E103*$E$223/100 | 135.75483 |
| M103 | Formül | =(L103-K103)/K103 | 0.2975 |
| A104 | Sabit | 8 | — |
| B104 | Sabit | NYM KABLO  | — |
| C104 | Sabit | 3 x 2.5 | — |
| D104 | Formül | ='BİNA BİLGİLERİ'!B2*1.5 | 157.5 |
| E104 | Formül | =K104*1.73 | 124.95443999999999 |
| F104 | Formül | =E104*D104 | 19680.3243 |
| I104 | Sabit | 60.19 | — |
| J104 | Formül | =I104*D104 | 9479.925 |
| K104 | Formül | =I104*1.2 | 72.228 |
| L104 | Formül | =E104-E104*$E$223/100 | 93.71583 |
| M104 | Formül | =(L104-K104)/K104 | 0.29750000000000004 |
| A105 | Sabit | 9 | — |
| B105 | Sabit | NYM KABLO | — |
| C105 | Sabit | 2 x 1.5 | — |
| D105 | Formül | ='BİNA BİLGİLERİ'!B2 | 105 |
| E105 | Formül | =K105*1.73 | 59.47739999999999 |
| F105 | Formül | =E105*D105 | 6245.126999999999 |
| I105 | Sabit | 28.65 | — |
| J105 | Formül | =I105*D105 | 3008.25 |
| K105 | Formül | =I105*1.2 | 34.379999999999995 |
| L105 | Formül | =E105-E105*$E$223/100 | 44.60804999999999 |
| M105 | Formül | =(L105-K105)/K105 | 0.29749999999999993 |
| A106 | Sabit | 10 | — |
| B106 | Sabit | ANTEN KABLOSU | — |
| C106 | Sabit | METRE | — |
| D106 | Formül | =IF('BİNA BİLGİLERİ'!B7="2",60,30) | 30 |
| E106 | Formül | =K106*1.73 | 24.41376 |
| F106 | Formül | =E106*D106 | 732.4128 |
| I106 | Sabit | 11.76 | — |
| J106 | Formül | =I106*D106 | 352.8 |
| K106 | Formül | =I106*1.2 | 14.112 |
| L106 | Formül | =E106-E106*$E$223/100 | 18.310319999999997 |
| M106 | Formül | =(L106-K106)/K106 | 0.2974999999999998 |
| A107 | Sabit | 11 | — |
| B107 | Sabit | TEKLİ ANAHTAR | — |
| C107 | Sabit | SIVA ALTI | — |
| D107 | Formül | =IF('BİNA BİLGİLERİ'!B2<=100,3,3+ROUNDUP(('BİNA BİLGİLERİ'!B2-100)/20,0)) | 4 |
| E107 | Formül | =K107*1.73 | 105.87599999999999 |
| F107 | Formül | =E107*D107 | 423.50399999999996 |
| I107 | Sabit | 51 | — |
| J107 | Formül | =I107*D107 | 204 |
| K107 | Formül | =I107*1.2 | 61.199999999999996 |
| L107 | Formül | =E107-E107*$E$223/100 | 79.407 |
| M107 | Formül | =(L107-K107)/K107 | 0.29750000000000004 |
| A108 | Sabit | 12 | — |
| B108 | Sabit | ÇİFTLİ ANAHTAR | — |
| C108 | Sabit | SIVA ALTI | — |
| D108 | Formül | =IF('BİNA BİLGİLERİ'!B2<=100,3,3+ROUNDUP(('BİNA BİLGİLERİ'!B2-100)/20,0)) | 4 |
| E108 | Formül | =K108*1.73 | 130.78799999999998 |
| F108 | Formül | =E108*D108 | 523.1519999999999 |
| I108 | Sabit | 63 | — |
| J108 | Formül | =I108*D108 | 252 |
| K108 | Formül | =I108*1.2 | 75.6 |
| L108 | Formül | =E108-E108*$E$223/100 | 98.09099999999998 |
| M108 | Formül | =(L108-K108)/K108 | 0.2974999999999998 |
| A109 | Sabit | 13 | — |
| B109 | Sabit | VAVIEN ANAHTAR | — |
| C109 | Sabit | SIVA ALTI | — |
| E109 | Formül | =K109*1.73 | 352.92 |
| F109 | Formül | =E109*D109 | 0 |
| I109 | Sabit | 170 | — |
| J109 | Formül | =I109*D109 | 0 |
| K109 | Formül | =I109*1.2 | 204 |
| L109 | Formül | =E109-E109*$E$223/100 | 264.69 |
| M109 | Formül | =(L109-K109)/K109 | 0.2975 |
| A110 | Sabit | 14 | — |
| B110 | Sabit | TOPRAKLI PRİZ | — |
| C110 | Sabit | SIVA ALTI | — |
| D110 | Formül | =IF('BİNA BİLGİLERİ'!B2<=100,18,18+ROUNDUP(('BİNA BİLGİLERİ'!B2-100)/20,0)*2) | 20 |
| E110 | Formül | =K110*1.73 | 110.02799999999999 |
| F110 | Formül | =E110*D110 | 2200.56 |
| I110 | Sabit | 53 | — |
| J110 | Formül | =I110*D110 | 1060 |
| K110 | Formül | =I110*1.2 | 63.599999999999994 |
| L110 | Formül | =E110-E110*$E$223/100 | 82.52099999999999 |
| M110 | Formül | =(L110-K110)/K110 | 0.29749999999999993 |
| A111 | Sabit | 15 | — |
| B111 | Sabit | TV UYDU PRİZİ | — |
| C111 | Sabit | SIVA ALTI | — |
| D111 | Sabit | 1 | — |
| E111 | Formül | =K111*1.73 | 352.92 |
| F111 | Formül | =E111*D111 | 352.92 |
| I111 | Sabit | 170 | — |
| J111 | Formül | =I111*D111 | 170 |
| K111 | Formül | =I111*1.2 | 204 |
| L111 | Formül | =E111-E111*$E$223/100 | 264.69 |
| M111 | Formül | =(L111-K111)/K111 | 0.2975 |
| A112 | Sabit | 16 | — |
| B112 | Sabit | PRİZ,ANAHTAR ÇERÇEVESİ | — |
| C112 | Sabit | ADET | — |
| D112 | Formül | =SUM(D107:D111) | 29 |
| E112 | Formül | =K112*1.73 | 13.286399999999999 |
| F112 | Formül | =E112*D112 | 385.30559999999997 |
| I112 | Sabit | 6.4 | — |
| J112 | Formül | =I112*D112 | 185.60000000000002 |
| K112 | Formül | =I112*1.2 | 7.68 |
| L112 | Formül | =E112-E112*$E$223/100 | 9.964799999999999 |
| M112 | Formül | =(L112-K112)/K112 | 0.2974999999999999 |
| A113 | Sabit | 17 | — |
| B113 | Sabit | SİGORTA KUTUSU | — |
| C113 | Sabit | 9'lu | — |
| E113 | Formül | =K113*1.73 | 307.248 |
| F113 | Formül | =E113*D113 | 0 |
| I113 | Sabit | 148 | — |
| J113 | Formül | =I113*D113 | 0 |
| K113 | Formül | =I113*1.2 | 177.6 |
| L113 | Formül | =E113-E113*$E$223/100 | 230.43599999999998 |
| M113 | Formül | =(L113-K113)/K113 | 0.29749999999999993 |
| A114 | Sabit | 18 | — |
| B114 | Sabit | SİGORTA KUTUSU | — |
| C114 | Sabit | 12'li | — |
| D114 | Formül | ='BİNA BİLGİLERİ'!B7 | 1 |
| E114 | Formül | =K114*1.73 | 400.668 |
| F114 | Formül | =E114*D114 | 400.668 |
| I114 | Sabit | 193 | — |
| J114 | Formül | =I114*D114 | 193 |
| K114 | Formül | =I114*1.2 | 231.6 |
| L114 | Formül | =E114-E114*$E$223/100 | 300.501 |
| M114 | Formül | =(L114-K114)/K114 | 0.29749999999999993 |
| A115 | Sabit | 19 | — |
| B115 | Sabit | SİGORTA KUTUSU | — |
| C115 | Sabit | 24'lü | — |
| E115 | Formül | =K115*1.73 | 616.572 |
| F115 | Formül | =E115*D115 | 0 |
| I115 | Sabit | 297 | — |
| J115 | Formül | =I115*D115 | 0 |
| K115 | Formül | =I115*1.2 | 356.4 |
| L115 | Formül | =E115-E115*$E$223/100 | 462.429 |
| M115 | Formül | =(L115-K115)/K115 | 0.2975 |
| A116 | Sabit | 20 | — |
| B116 | Sabit | W OTOMAT | — |
| C116 | Sabit | 16  AMPER | — |
| D116 | Formül | =6*$D$114 | 6 |
| E116 | Formül | =K116*1.73 | 132.864 |
| F116 | Formül | =E116*D116 | 797.184 |
| I116 | Sabit | 64 | — |
| J116 | Formül | =I116*D116 | 384 |
| K116 | Formül | =I116*1.2 | 76.8 |
| L116 | Formül | =E116-E116*$E$223/100 | 99.648 |
| M116 | Formül | =(L116-K116)/K116 | 0.2975 |
| A117 | Sabit | 21 | — |
| B117 | Sabit | W OTOMAT  | — |
| C117 | Sabit | 25  AMPER | — |
| D117 | Formül | =3*$D$114 | 3 |
| E117 | Formül | =K117*1.73 | 132.864 |
| F117 | Formül | =E117*D117 | 398.592 |
| I117 | Sabit | 64 | — |
| J117 | Formül | =I117*D117 | 192 |
| K117 | Formül | =I117*1.2 | 76.8 |
| L117 | Formül | =E117-E117*$E$223/100 | 99.648 |
| M117 | Formül | =(L117-K117)/K117 | 0.2975 |
| A118 | Sabit | 22 | — |
| B118 | Sabit | KAÇAK AKIM ROLESİ MONOFAZE | — |
| C118 | Sabit | 40 AMPER | — |
| D118 | Formül | =1*$D$114 | 1 |
| E118 | Formül | =K118*1.73 | 1368.0839999999998 |
| F118 | Formül | =E118*D118 | 1368.0839999999998 |
| I118 | Sabit | 659 | — |
| J118 | Formül | =I118*D118 | 659 |
| K118 | Formül | =I118*1.2 | 790.8 |
| L118 | Formül | =E118-E118*$E$223/100 | 1026.0629999999999 |
| M118 | Formül | =(L118-K118)/K118 | 0.29749999999999993 |
| A119 | Sabit | 23 | — |
| B119 | Sabit | KAÇAK AKIM ROLESİ TRİFAZE | — |
| C119 | Sabit | 63 AMPER | — |
| E119 | Formül | =K119*1.73 | 2595 |
| F119 | Formül | =E119*D119 | 0 |
| I119 | Sabit | 1250 | — |
| J119 | Formül | =I119*D119 | 0 |
| K119 | Formül | =I119*1.2 | 1500 |
| L119 | Formül | =E119-E119*$E$223/100 | 1946.25 |
| M119 | Formül | =(L119-K119)/K119 | 0.2975 |
| A120 | Sabit | 24 | — |
| B120 | Sabit | PVC BUAT KAPAĞI | — |
| C120 | Sabit | ADET | — |
| E120 | Formül | =K120*1.73 | 14.532 |
| F120 | Formül | =E120*D120 | 0 |
| I120 | Sabit | 7 | — |
| J120 | Formül | =I120*D120 | 0 |
| K120 | Formül | =I120*1.2 | 8.4 |
| L120 | Formül | =E120-E120*$E$223/100 | 10.899000000000001 |
| M120 | Formül | =(L120-K120)/K120 | 0.29750000000000004 |
| A121 | Sabit | 25 | — |
| B121 | Sabit |  BUAT KLEMENSİ | — |
| C121 | Sabit | ADET | — |
| D121 | Formül | ='BİNA BİLGİLERİ'!B2 | 105 |
| E121 | Formül | =K121*1.73 | 7.6812000000000005 |
| F121 | Formül | =E121*D121 | 806.5260000000001 |
| I121 | Sabit | 3.7 | — |
| J121 | Formül | =I121*D121 | 388.5 |
| K121 | Formül | =I121*1.2 | 4.44 |
| L121 | Formül | =E121-E121*$E$223/100 | 5.7609 |
| M121 | Formül | =(L121-K121)/K121 | 0.2975 |
| A122 | Sabit | 26 | — |
| B122 | Sabit | MONEFAZE BARA | — |
| C122 | Sabit | ADET | — |
| D122 | Sabit | 1 | — |
| E122 | Formül | =K122*1.73 | 431.808 |
| F122 | Formül | =E122*D122 | 431.808 |
| I122 | Sabit | 208 | — |
| J122 | Formül | =I122*D122 | 208 |
| K122 | Formül | =I122*1.2 | 249.6 |
| L122 | Formül | =E122-E122*$E$223/100 | 323.856 |
| M122 | Formül | =(L122-K122)/K122 | 0.2975 |
| A123 | Sabit | 27 | — |
| B123 | Sabit | TRİFAZE  BARA | — |
| C123 | Sabit | ADET | — |
| E123 | Formül | =K123*1.73 | 1262.208 |
| F123 | Formül | =E123*D123 | 0 |
| I123 | Sabit | 608 | — |
| J123 | Formül | =I123*D123 | 0 |
| K123 | Formül | =I123*1.2 | 729.6 |
| L123 | Formül | =E123-E123*$E$223/100 | 946.6560000000001 |
| M123 | Formül | =(L123-K123)/K123 | 0.29750000000000004 |
| A124 | Sabit | 28 | — |
| B124 | Sabit | İZOLE BANT | — |
| C124 | Sabit | ADET | — |
| D124 | Formül | =ROUNDUP('BİNA BİLGİLERİ'!B2/30,0) | 4 |
| E124 | Formül | =K124*1.73 | 24.912 |
| F124 | Formül | =E124*D124 | 99.648 |
| I124 | Sabit | 12 | — |
| J124 | Formül | =I124*D124 | 48 |
| K124 | Formül | =I124*1.2 | 14.399999999999999 |
| L124 | Formül | =E124-E124*$E$223/100 | 18.683999999999997 |
| M124 | Formül | =(L124-K124)/K124 | 0.29749999999999993 |
| D125 | Sabit | TOPLAM | — |
| F125 | Formül | =SUBTOTAL(9,F97:F124) | 34845.8157 |
| I125 | Formül | =SUBTOTAL(9,I97:I124) | 4917.67 |
| J125 | Formül | =SUBTOTAL(9,J97:J124) | 16785.074999999997 |
| K125 | Formül | =SUBTOTAL(9,K97:K124) | 5901.204 |
| L125 | Formül | =SUBTOTAL(9,L97:L124) | 7656.8121900000015 |
| A126 | Sabit |                                      VİDA GRUBU | — |
| D126 | Sabit | 1006 | — |
| A127 | Sabit | 1 | — |
| B127 | Sabit | ALÇIPAN VİDASI SİVRİ UÇLU ( 3.5 x 35 ) | — |
| C127 | Sabit | kutu 1000 adet | — |
| D127 | Formül | =((D39+D40)*30)-D128 | 2910 |
| E127 | Formül | =K127*1.73 | 0.70584 |
| F127 | Formül | =E127*D127 | 2053.9944 |
| I127 | Sabit | 0.34 | — |
| J127 | Formül | =I127*D127 | 989.4000000000001 |
| K127 | Formül | =I127*1.2 | 0.40800000000000003 |
| L127 | Formül | =E127-E127*$E$223/100 | 0.52938 |
| M127 | Formül | =(L127-K127)/K127 | 0.2974999999999998 |
| A128 | Sabit | 2 | — |
| B128 | Sabit | ALÇIPAN VİDASI MATKAP UÇLU ( 3.5 x 35 ) | — |
| C128 | Sabit | kutu 1000 adet | — |
| D128 | Formül | =('BİNA BİLGİLERİ'!B2/3)*30 | 1050 |
| E128 | Formül | =K128*1.73 | 0.87192 |
| F128 | Formül | =E128*D128 | 915.5160000000001 |
| I128 | Sabit | 0.42 | — |
| J128 | Formül | =I128*D128 | 441 |
| K128 | Formül | =I128*1.2 | 0.504 |
| L128 | Formül | =E128-E128*$E$223/100 | 0.65394 |
| M128 | Formül | =(L128-K128)/K128 | 0.29749999999999993 |
| A129 | Sabit | 3 | — |
| B129 | Sabit | SAÇAK VE DERE KANCASI ( 3.9 x 32 ) | — |
| C129 | Sabit | kutu 1250 adet | — |
| D129 | Formül | ='BİNA BİLGİLERİ'!B2*10 | 1050 |
| E129 | Formül | =K129*1.73 | 0.87192 |
| F129 | Formül | =E129*D129 | 915.5160000000001 |
| I129 | Sabit | 0.42 | — |
| J129 | Formül | =I129*D129 | 441 |
| K129 | Formül | =I129*1.2 | 0.504 |
| L129 | Formül | =E129-E129*$E$223/100 | 0.65394 |
| M129 | Formül | =(L129-K129)/K129 | 0.29749999999999993 |
| A130 | Sabit | 4 | — |
| B130 | Sabit | 16 MM BETOPAN VE KAPI KOLLARI ( 3.9x32 ) | — |
| C130 | Sabit | kutu 1250 adet | — |
| D130 | Formül | =(D42+D41+D47)*30 | 7200 |
| E130 | Formül | =K130*1.73 | 0.87192 |
| F130 | Formül | =E130*D130 | 6277.8240000000005 |
| I130 | Sabit | 0.42 | — |
| J130 | Formül | =I130*D130 | 3024 |
| K130 | Formül | =I130*1.2 | 0.504 |
| L130 | Formül | =E130-E130*$E$223/100 | 0.65394 |
| M130 | Formül | =(L130-K130)/K130 | 0.29749999999999993 |
| A131 | Sabit | 5 | — |
| B131 | Sabit | PVC  PENCERELER ( 4.8 x 70 ) | — |
| C131 | Sabit | kutu 250 adet | — |
| D131 | Formül | =SUM(D82:D93)*10 | 80 |
| E131 | Formül | =K131*1.73 | 2.4081599999999996 |
| F131 | Formül | =E131*D131 | 192.65279999999996 |
| I131 | Sabit | 1.16 | — |
| J131 | Formül | =I131*D131 | 92.8 |
| K131 | Formül | =I131*1.2 | 1.392 |
| L131 | Formül | =E131-E131*$E$223/100 | 1.8061199999999997 |
| M131 | Formül | =(L131-K131)/K131 | 0.2974999999999999 |
| A132 | Sabit | 6 | — |
| B132 | Sabit | ÇATI AŞIKLARI ( 5.5 x 25 ) | — |
| C132 | Sabit | kutu 500 adet | — |
| D132 | Formül | ='BİNA BİLGİLERİ'!B2*10 | 1050 |
| E132 | Formül | =K132*1.73 | 2.076 |
| F132 | Formül | =E132*D132 | 2179.8 |
| I132 | Sabit | 1 | — |
| J132 | Formül | =I132*D132 | 1050 |
| K132 | Formül | =I132*1.2 | 1.2 |
| L132 | Formül | =E132-E132*$E$223/100 | 1.557 |
| M132 | Formül | =(L132-K132)/K132 | 0.2975 |
| A133 | Sabit | 7 | — |
| B133 | Sabit | TRAPEZ SAC - METAL KİREMİT ( 5.5 x 60) | — |
| C133 | Sabit | kutu 200 adet | — |
| D133 | Formül | ='BİNA BİLGİLERİ'!B2*8 | 840 |
| E133 | Formül | =K133*1.73 | 3.1555199999999997 |
| F133 | Formül | =E133*D133 | 2650.6367999999998 |
| I133 | Sabit | 1.52 | — |
| J133 | Formül | =I133*D133 | 1276.8 |
| K133 | Formül | =I133*1.2 | 1.8239999999999998 |
| L133 | Formül | =E133-E133*$E$223/100 | 2.36664 |
| M133 | Formül | =(L133-K133)/K133 | 0.29750000000000004 |
| A134 | Sabit | 8 | — |
| B134 | Sabit | SHINGLE ÇİVİSİ | — |
| C134 | Sabit | Adet | — |
| E134 | Formül | =K134*1.73 | 2.076 |
| F134 | Formül | =E134*D134 | 0 |
| I134 | Sabit | 1 | — |
| J134 | Formül | =I134*D134 | 0 |
| K134 | Formül | =I134*1.2 | 1.2 |
| L134 | Formül | =E134-E134*$E$223/100 | 1.557 |
| M134 | Formül | =(L134-K134)/K134 | 0.2975 |
| D135 | Sabit | TOPLAM | — |
| F135 | Formül | =SUM(F127:F134) | 15185.940000000002 |
| I135 | Formül | =SUM(I127:I134) | 6.279999999999999 |
| J135 | Formül | =SUM(J127:J134) | 7315 |
| K135 | Formül | =SUM(K127:K134) | 7.536 |
| L135 | Formül | =SUM(L127:L134) | 9.77796 |
| A136 | Sabit |                           DEPO VE HIRDAVAT GRUBU | — |
| D136 | Sabit | 1007 | — |
| A137 | Sabit | 1 | — |
| B137 | Sabit | ÇELİK DÜBEL M10 12 mm | — |
| C137 | Sabit | ADET | — |
| D137 | Formül | =D20 | 20 |
| E137 | Formül | =K137*1.73 | 16.857119999999995 |
| F137 | Formül | =E137*D137 | 337.1423999999999 |
| I137 | Sabit | 8.12 | — |
| J137 | Formül | =I137*D137 | 162.39999999999998 |
| K137 | Formül | =I137*1.2 | 9.743999999999998 |
| L137 | Formül | =E137-E137*$E$223/100 | 12.642839999999996 |
| M137 | Formül | =(L137-K137)/K137 | 0.2974999999999999 |
| A138 | Sabit | 2 | — |
| B138 | Sabit | İÇ KAPI KOL + KİLİT | — |
| C138 | Sabit | ADET | — |
| D138 | Formül | =D79+D80 | 6 |
| E138 | Formül | =K138*1.73 | 0 |
| F138 | Formül | =E138*D138 | 0 |
| I138 | Formül | =Y138*X138 | 0 |
| J138 | Formül | =I138*D138 | 0 |
| K138 | Formül | =I138*1.2 | 0 |
| L138 | Formül | =E138-E138*$E$223/100 | 0 |
| M138 | Formül | =(L138-K138)/K138 | #DIV/0! |
| A139 | Sabit | 3 | — |
| B139 | Sabit | İÇ KAPI MENTEŞE | — |
| C139 | Sabit | ADET | — |
| E139 | Formül | =K139*1.73 | 53.976 |
| F139 | Formül | =E139*D139 | 0 |
| I139 | Sabit | 26 | — |
| J139 | Formül | =I139*D139 | 0 |
| K139 | Formül | =I139*1.2 | 31.2 |
| L139 | Formül | =E139-E139*$E$223/100 | 40.482 |
| M139 | Formül | =(L139-K139)/K139 | 0.2975 |
| A140 | Sabit | 4 | — |
| B140 | Sabit | KESME TAŞI 180'LİK | — |
| C140 | Sabit | ADET | — |
| D140 | Formül | =ROUNDUP('BİNA BİLGİLERİ'!B2/30,0) | 4 |
| E140 | Formül | =K140*1.73 | 91.344 |
| F140 | Formül | =E140*D140 | 365.376 |
| I140 | Sabit | 44 | — |
| J140 | Formül | =I140*D140 | 176 |
| K140 | Formül | =I140*1.2 | 52.8 |
| L140 | Formül | =E140-E140*$E$223/100 | 68.508 |
| M140 | Formül | =(L140-K140)/K140 | 0.2975 |
| A141 | Sabit | 5 | — |
| B141 | Sabit | YILDIZ UÇ KISA | — |
| C141 | Sabit | ADET | — |
| D141 | Formül | =ROUNDUP('BİNA BİLGİLERİ'!B2/30,0) | 4 |
| E141 | Formül | =K141*1.73 | 72.66 |
| F141 | Formül | =E141*D141 | 290.64 |
| I141 | Sabit | 35 | — |
| J141 | Formül | =I141*D141 | 140 |
| K141 | Formül | =I141*1.2 | 42 |
| L141 | Formül | =E141-E141*$E$223/100 | 54.495 |
| M141 | Formül | =(L141-K141)/K141 | 0.29749999999999993 |
| A142 | Sabit | 6 | — |
| B142 | Sabit | ALYAN UÇ | — |
| C142 | Sabit | ADET | — |
| D142 | Formül | =ROUNDUP('BİNA BİLGİLERİ'!B2/33,0) | 4 |
| E142 | Formül | =K142*1.73 | 149.47199999999998 |
| F142 | Formül | =E142*D142 | 597.8879999999999 |
| I142 | Sabit | 72 | — |
| J142 | Formül | =I142*D142 | 288 |
| K142 | Formül | =I142*1.2 | 86.39999999999999 |
| L142 | Formül | =E142-E142*$E$223/100 | 112.10399999999998 |
| M142 | Formül | =(L142-K142)/K142 | 0.29749999999999993 |
| A143 | Sabit | 7 | — |
| B143 | Sabit | POLÜRETAN KÖPÜK | — |
| C143 | Sabit | ADET | — |
| D143 | Formül | =IF('BİNA BİLGİLERİ'!B2<=100,2,2+ROUNDUP(('BİNA BİLGİLERİ'!B2-100)/100,0)) | 3 |
| E143 | Formül | =K143*1.73 | 207.6 |
| F143 | Formül | =E143*D143 | 622.8 |
| I143 | Sabit | 100 | — |
| J143 | Formül | =I143*D143 | 300 |
| K143 | Formül | =I143*1.2 | 120 |
| L143 | Formül | =E143-E143*$E$223/100 | 155.7 |
| M143 | Formül | =(L143-K143)/K143 | 0.29749999999999993 |
| A144 | Sabit | 8 | — |
| B144 | Sabit | POLÜRETAN MASTİK  2  Antrasit gri - 2  Beyaz | — |
| C144 | Sabit | ADET | — |
| D144 | Formül | =IF('BİNA BİLGİLERİ'!B2<=100,2,2+ROUNDUP(('BİNA BİLGİLERİ'!B2-100)/100,0)) | 3 |
| E144 | Formül | =K144*1.73 | 301.02 |
| F144 | Formül | =E144*D144 | 903.06 |
| I144 | Sabit | 145 | — |
| J144 | Formül | =I144*D144 | 435 |
| K144 | Formül | =I144*1.2 | 174 |
| L144 | Formül | =E144-E144*$E$223/100 | 225.765 |
| M144 | Formül | =(L144-K144)/K144 | 0.29749999999999993 |
| A145 | Sabit | 9 | — |
| B145 | Sabit |  NEM BARİYERİ | — |
| C145 | Sabit | m² | — |
| D145 | Formül | =IF('BİNA BİLGİLERİ'!D33="NEM BARİYERİ",'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9*2,IF('BİNA BİLGİLERİ'!E33="NEM BARİYERİ",'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9*2,IF('BİNA BİLGİLERİ'!F33="NEM BARİYERİ",'BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9*2,0)))+IF('BİNA BİLGİLERİ'!D32="NEM BARİYERİ",'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9,IF('BİNA BİLGİLERİ'!E32="NEM BARİYERİ",'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9,IF('BİNA BİLGİLERİ'!F32="NEM BARİYERİ",'BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9,0)))+IF('BİNA BİLGİLERİ'!D34="NEM BARİYERİ",'BİNA BİLGİLERİ'!B34,IF('BİNA BİLGİLERİ'!E34="NEM BARİYERİ",'BİNA BİLGİLERİ'!B34,IF('BİNA BİLGİLERİ'!F34="NEM BARİYERİ",'BİNA BİLGİLERİ'!B34,0)))+IF('BİNA BİLGİLERİ'!D35="NEM BARİYERİ",'BİNA BİLGİLERİ'!B35,IF('BİNA BİLGİLERİ'!E35="NEM BARİYERİ",'BİNA BİLGİLERİ'!B35,IF('BİNA BİLGİLERİ'!F35="NEM BARİYERİ",'BİNA BİLGİLERİ'!B35,0))) | 422.124896 |
| E145 | Formül | =K145*1.73 | 45.672 |
| F145 | Formül | =E145*D145 | 19279.288250111997 |
| I145 | Sabit | 22 | — |
| J145 | Formül | =I145*D145 | 9286.747712 |
| K145 | Formül | =I145*1.2 | 26.4 |
| L145 | Formül | =E145-E145*$E$223/100 | 34.254 |
| M145 | Formül | =(L145-K145)/K145 | 0.2975 |
| A146 | Sabit | 10 | — |
| B146 | Sabit | KIRMIZI SHINGLE ( 1 PAKETİ 2,61 m² )  | — |
| C146 | Sabit | ADET | — |
| E146 | Formül | =K146*1.73 | 0 |
| F146 | Formül | =E146*D146 | 0 |
| I146 | Formül | =Y146*X146 | 0 |
| J146 | Formül | =I146*D146 | 0 |
| K146 | Formül | =I146*1.2 | 0 |
| L146 | Formül | =E146-E146*$E$223/100 | 0 |
| M146 | Formül | =(L146-K146)/K146 | #DIV/0! |
| A147 | Sabit | 11 | — |
| B147 | Sabit | MEBRAN   | — |
| C147 | Sabit | TOP 10 m2 | — |
| D147 | Formül | =IF('BİNA BİLGİLERİ'!D32="MEBRAN",('BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9)/10,IF('BİNA BİLGİLERİ'!E32="MEBRAN",('BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9)/10,IF('BİNA BİLGİLERİ'!F32="MEBRAN",('BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9)/10,0)))+IF('BİNA BİLGİLERİ'!D33="MEBRAN",(('BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9)/10)*2,IF('BİNA BİLGİLERİ'!E33="MEBRAN",(('BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9)/10)*2,IF('BİNA BİLGİLERİ'!F33="MEBRAN",(('BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9)/10)*2,0)))+IF('BİNA BİLGİLERİ'!D34="MEBRAN",'BİNA BİLGİLERİ'!B34/10,IF('BİNA BİLGİLERİ'!E34="MEBRAN",'BİNA BİLGİLERİ'!B34/10,IF('BİNA BİLGİLERİ'!F34="MEBRAN",'BİNA BİLGİLERİ'!B34/10,0)))+IF('BİNA BİLGİLERİ'!D35="MEBRAN",'BİNA BİLGİLERİ'!B35/10,IF('BİNA BİLGİLERİ'!E35="MEBRAN",'BİNA BİLGİLERİ'!B35/10,IF('BİNA BİLGİLERİ'!F35="MEBRAN",'BİNA BİLGİLERİ'!B35/10,0))) | 0 |
| E147 | Formül | =K147*1.73 | 0 |
| F147 | Formül | =E147*D147 | 0 |
| I147 | Formül | =Y147*X147 | 0 |
| J147 | Formül | =I147*D147 | 0 |
| K147 | Formül | =I147*1.2 | 0 |
| L147 | Formül | =E147-E147*$E$223/100 | 0 |
| M147 | Formül | =(L147-K147)/K147 | #DIV/0! |
| A148 | Sabit | 12 | — |
| B148 | Sabit | ARDUAZLI MEBRAN | — |
| C148 | Sabit | TOP 10 m2 | — |
| D148 | Formül | =IF('BİNA BİLGİLERİ'!D32="ARDUAZLI MEBRAN",('BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9)/10,IF('BİNA BİLGİLERİ'!E32="ARDUAZLI MEBRAN",('BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9)/10,IF('BİNA BİLGİLERİ'!F32="ARDUAZLI MEBRAN",('BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9)/10,0)))+IF('BİNA BİLGİLERİ'!D33="ARDUAZLI MEBRAN",(('BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9)/10)*2,IF('BİNA BİLGİLERİ'!E33="ARDUAZLI MEBRAN",(('BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9)/10)*2,IF('BİNA BİLGİLERİ'!F33="ARDUAZLI MEBRAN",(('BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9)/10)*2,0)))+IF('BİNA BİLGİLERİ'!D34="ARDUAZLI MEBRAN",'BİNA BİLGİLERİ'!B34/10,IF('BİNA BİLGİLERİ'!E34="ARDUAZLI MEBRAN",'BİNA BİLGİLERİ'!B34/10,IF('BİNA BİLGİLERİ'!F34="ARDUAZLI MEBRAN",'BİNA BİLGİLERİ'!B34/10,0)))+IF('BİNA BİLGİLERİ'!D35="ARDUAZLI MEBRAN",'BİNA BİLGİLERİ'!B35/10,IF('BİNA BİLGİLERİ'!E35="ARDUAZLI MEBRAN",'BİNA BİLGİLERİ'!B35/10,IF('BİNA BİLGİLERİ'!F35="ARDUAZLI MEBRAN",'BİNA BİLGİLERİ'!B35/10,0))) | 0 |
| E148 | Formül | =K148*1.73 | 0 |
| F148 | Formül | =E148*D148 | 0 |
| I148 | Formül | =Y148*X148 | 0 |
| J148 | Formül | =I148*D148 | 0 |
| K148 | Formül | =I148*1.2 | 0 |
| L148 | Formül | =E148-E148*$E$223/100 | 0 |
| M148 | Formül | =(L148-K148)/K148 | #DIV/0! |
| A149 | Sabit | 13 | — |
| B149 | Sabit | KUBBE ÜSTÜ KIRMIZI ARDUAZ | — |
| C149 | Sabit | 20*3000 | — |
| E149 | Formül | =K149*1.73 | 0 |
| F149 | Formül | =E149*D149 | 0 |
| I149 | Formül | =Y149*X149 | 0 |
| J149 | Formül | =I149*D149 | 0 |
| K149 | Formül | =I149*1.2 | 0 |
| L149 | Formül | =E149-E149*$E$223/100 | 0 |
| M149 | Formül | =(L149-K149)/K149 | #DIV/0! |
| A150 | Sabit | 14 | — |
| B150 | Sabit |  KNAUF ARA BÖLME 50 MM | — |
| C150 | Sabit | Paket:14,4 m² | — |
| D150 | Formül | =ROUNDUP(('BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9)*IF('BİNA BİLGİLERİ'!C3=80,2,IF('BİNA BİLGİLERİ'!C3=90,2,IF('BİNA BİLGİLERİ'!C3=140,3,IF('BİNA BİLGİLERİ'!C3=200,4,IF('BİNA BİLGİLERİ'!C3=300,6,0)))))/14.4,0)+ROUNDUP(('BİNA BİLGİLERİ'!B33*'BİNA BİLGİLERİ'!B9)*IF('BİNA BİLGİLERİ'!E3=80,2,IF('BİNA BİLGİLERİ'!E3=90,2,IF('BİNA BİLGİLERİ'!E3=140,3,IF('BİNA BİLGİLERİ'!E3=200,4,IF('BİNA BİLGİLERİ'!E3=300,6,0)))))/14.4,0) | 29 |
| E150 | Formül | =K150*1.73 | 1376.388 |
| F150 | Formül | =E150*D150 | 39915.252 |
| I150 | Sabit | 663 | — |
| J150 | Formül | =I150*D150 | 19227 |
| K150 | Formül | =I150*1.2 | 795.6 |
| L150 | Formül | =E150-E150*$E$223/100 | 1032.291 |
| M150 | Formül | =(L150-K150)/K150 | 0.2974999999999999 |
| A151 | Sabit | 15 | — |
| B151 | Sabit |  KNAUF ÇATI ŞİLTESİ 100 MM | — |
| C151 | Sabit | Paket:9,6 m² | — |
| D151 | Formül | =ROUNDUP('BİNA BİLGİLERİ'!B12/9.6,0) | 11 |
| E151 | Formül | =K151*1.73 | 1170.8639999999998 |
| F151 | Formül | =E151*D151 | 12879.503999999997 |
| I151 | Sabit | 564 | — |
| J151 | Formül | =I151*D151 | 6204 |
| K151 | Formül | =I151*1.2 | 676.8 |
| L151 | Formül | =E151-E151*$E$223/100 | 878.1479999999999 |
| M151 | Formül | =(L151-K151)/K151 | 0.29749999999999993 |
| D152 | Sabit | TOPLAM | — |
| F152 | Formül | =SUBTOTAL(9,F137:F151) | 75190.95065011199 |
| I152 | Formül | =SUBTOTAL(9,I137:I151) | 1679.12 |
| J152 | Formül | =SUBTOTAL(9,J137:J151) | 36219.147712 |
| K152 | Formül | =SUBTOTAL(9,K137:K151) | 2014.944 |
| L152 | Formül | =SUBTOTAL(9,L137:L151) | 2614.38984 |
| A153 | Sabit |                               SIHHİ TESİSAT GRUBU | — |
| D153 | Sabit | 1008 | — |
| A154 | Sabit | 1 | — |
| B154 | Sabit | PPR TEMİZ SU BORUSU 20mm | — |
| C154 | Sabit | ADET | — |
| D154 | Formül | =H154*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 16 |
| E154 | Formül | =K154*1.73 | 197.22 |
| F154 | Formül | =E154*D154 | 3155.52 |
| H154 | Sabit | 8 | — |
| I154 | Sabit | 95 | — |
| J154 | Formül | =I154*D154 | 1520 |
| K154 | Formül | =I154*1.2 | 114 |
| L154 | Formül | =E154-E154*$E$223/100 | 147.915 |
| M154 | Formül | =(L154-K154)/K154 | 0.29749999999999993 |
| A155 | Sabit | 2 | — |
| B155 | Sabit |  DİRSEK   20mm | — |
| C155 | Sabit | ADET | — |
| D155 | Formül | =H155*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 32 |
| E155 | Formül | =K155*1.73 | 6.228 |
| F155 | Formül | =E155*D155 | 199.296 |
| H155 | Sabit | 16 | — |
| I155 | Sabit | 3 | — |
| J155 | Formül | =I155*D155 | 96 |
| K155 | Formül | =I155*1.2 | 3.5999999999999996 |
| L155 | Formül | =E155-E155*$E$223/100 | 4.670999999999999 |
| M155 | Formül | =(L155-K155)/K155 | 0.29749999999999993 |
| A156 | Sabit | 3 | — |
| B156 | Sabit | KULAKLI DİRSEK   20mm | — |
| C156 | Sabit | ADET | — |
| D156 | Formül | =H156*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 4 |
| E156 | Formül | =K156*1.73 | 65.18639999999999 |
| F156 | Formül | =E156*D156 | 260.74559999999997 |
| H156 | Sabit | 2 | — |
| I156 | Sabit | 31.4 | — |
| J156 | Formül | =I156*D156 | 125.6 |
| K156 | Formül | =I156*1.2 | 37.68 |
| L156 | Formül | =E156-E156*$E$223/100 | 48.889799999999994 |
| M156 | Formül | =(L156-K156)/K156 | 0.2974999999999999 |
| A157 | Sabit | 4 | — |
| B157 | Sabit | DÜZ T   20mm | — |
| C157 | Sabit | ADET | — |
| D157 | Formül | =H157*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 14 |
| E157 | Formül | =K157*1.73 | 9.48732 |
| F157 | Formül | =E157*D157 | 132.82248 |
| H157 | Sabit | 7 | — |
| I157 | Sabit | 4.57 | — |
| J157 | Formül | =I157*D157 | 63.980000000000004 |
| K157 | Formül | =I157*1.2 | 5.484 |
| L157 | Formül | =E157-E157*$E$223/100 | 7.11549 |
| M157 | Formül | =(L157-K157)/K157 | 0.29750000000000004 |
| A158 | Sabit | 5 | — |
| B158 | Sabit | İÇ DİŞLİ T   20mm | — |
| C158 | Sabit | ADET | — |
| D158 | Formül | =H158*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 10 |
| E158 | Formül | =K158*1.73 | 64.356 |
| F158 | Formül | =E158*D158 | 643.56 |
| H158 | Sabit | 5 | — |
| I158 | Sabit | 31 | — |
| J158 | Formül | =I158*D158 | 310 |
| K158 | Formül | =I158*1.2 | 37.199999999999996 |
| L158 | Formül | =E158-E158*$E$223/100 | 48.266999999999996 |
| M158 | Formül | =(L158-K158)/K158 | 0.29750000000000004 |
| A159 | Sabit | 6 | — |
| B159 | Sabit | MANŞON   20mm | — |
| C159 | Sabit | ADET | — |
| D159 | Formül | =H159*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 8 |
| E159 | Formül | =K159*1.73 | 4.48416 |
| F159 | Formül | =E159*D159 | 35.87328 |
| H159 | Sabit | 4 | — |
| I159 | Sabit | 2.16 | — |
| J159 | Formül | =I159*D159 | 17.28 |
| K159 | Formül | =I159*1.2 | 2.592 |
| L159 | Formül | =E159-E159*$E$223/100 | 3.3631200000000003 |
| M159 | Formül | =(L159-K159)/K159 | 0.2975000000000001 |
| A160 | Sabit | 7 | — |
| B160 | Sabit | KAVİS 20mm | — |
| C160 | Sabit | ADET | — |
| D160 | Formül | =H160*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 10 |
| E160 | Formül | =K160*1.73 | 24.912 |
| F160 | Formül | =E160*D160 | 249.12 |
| H160 | Sabit | 5 | — |
| I160 | Sabit | 12 | — |
| J160 | Formül | =I160*D160 | 120 |
| K160 | Formül | =I160*1.2 | 14.399999999999999 |
| L160 | Formül | =E160-E160*$E$223/100 | 18.683999999999997 |
| M160 | Formül | =(L160-K160)/K160 | 0.29749999999999993 |
| A161 | Sabit | 8 | — |
| B161 | Sabit | KÖR TAPA | — |
| C161 | Sabit | ADET | — |
| D161 | Formül | =H161*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 30 |
| E161 | Formül | =K161*1.73 | 3.8821200000000005 |
| F161 | Formül | =E161*D161 | 116.46360000000001 |
| H161 | Sabit | 15 | — |
| I161 | Sabit | 1.87 | — |
| J161 | Formül | =I161*D161 | 56.1 |
| K161 | Formül | =I161*1.2 | 2.244 |
| L161 | Formül | =E161-E161*$E$223/100 | 2.9115900000000003 |
| M161 | Formül | =(L161-K161)/K161 | 0.29750000000000004 |
| A162 | Sabit | 9 | — |
| B162 | Sabit | 100x2000 PVC BORU | — |
| C162 | Sabit | ADET | — |
| D162 | Formül | =H162*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 2 |
| E162 | Formül | =K162*1.73 | 423.50399999999996 |
| F162 | Formül | =E162*D162 | 847.0079999999999 |
| H162 | Sabit | 1 | — |
| I162 | Sabit | 204 | — |
| J162 | Formül | =I162*D162 | 408 |
| K162 | Formül | =I162*1.2 | 244.79999999999998 |
| L162 | Formül | =E162-E162*$E$223/100 | 317.628 |
| M162 | Formül | =(L162-K162)/K162 | 0.29750000000000004 |
| A163 | Sabit | 10 | — |
| B163 | Sabit | 100x1000 PVC BORU | — |
| C163 | Sabit | ADET | — |
| D163 | Formül | =H163*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 4 |
| E163 | Formül | =K163*1.73 | 218.60279999999997 |
| F163 | Formül | =E163*D163 | 874.4111999999999 |
| H163 | Sabit | 2 | — |
| I163 | Sabit | 105.3 | — |
| J163 | Formül | =I163*D163 | 421.2 |
| K163 | Formül | =I163*1.2 | 126.35999999999999 |
| L163 | Formül | =E163-E163*$E$223/100 | 163.95209999999997 |
| M163 | Formül | =(L163-K163)/K163 | 0.29749999999999993 |
| A164 | Sabit | 11 | — |
| B164 | Sabit | 100x87 KAPALI DİRSEK | — |
| C164 | Sabit | ADET | — |
| D164 | Formül | =H164*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 2 |
| E164 | Formül | =K164*1.73 | 110.02799999999999 |
| F164 | Formül | =E164*D164 | 220.05599999999998 |
| H164 | Sabit | 1 | — |
| I164 | Sabit | 53 | — |
| J164 | Formül | =I164*D164 | 106 |
| K164 | Formül | =I164*1.2 | 63.599999999999994 |
| L164 | Formül | =E164-E164*$E$223/100 | 82.52099999999999 |
| M164 | Formül | =(L164-K164)/K164 | 0.29749999999999993 |
| A165 | Sabit | 12 | — |
| B165 | Sabit | 100x50 TEK ÇATAL | — |
| C165 | Sabit | ADET | — |
| D165 | Formül | =H165*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 2 |
| E165 | Formül | =K165*1.73 | 122.484 |
| F165 | Formül | =E165*D165 | 244.968 |
| H165 | Sabit | 1 | — |
| I165 | Sabit | 59 | — |
| J165 | Formül | =I165*D165 | 118 |
| K165 | Formül | =I165*1.2 | 70.8 |
| L165 | Formül | =E165-E165*$E$223/100 | 91.863 |
| M165 | Formül | =(L165-K165)/K165 | 0.29750000000000004 |
| A166 | Sabit | 13 | — |
| B166 | Sabit | 100x100   T | — |
| C166 | Sabit | ADET | — |
| D166 | Formül | =H166*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 2 |
| E166 | Formül | =K166*1.73 | 184.76399999999998 |
| F166 | Formül | =E166*D166 | 369.52799999999996 |
| H166 | Sabit | 1 | — |
| I166 | Sabit | 89 | — |
| J166 | Formül | =I166*D166 | 178 |
| K166 | Formül | =I166*1.2 | 106.8 |
| L166 | Formül | =E166-E166*$E$223/100 | 138.57299999999998 |
| M166 | Formül | =(L166-K166)/K166 | 0.2974999999999998 |
| A167 | Sabit | 14 | — |
| B167 | Sabit | 50x500 PVC BORU | — |
| C167 | Sabit | ADET | — |
| D167 | Formül | =H167*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 10 |
| E167 | Formül | =K167*1.73 | 49.907039999999995 |
| F167 | Formül | =E167*D167 | 499.07039999999995 |
| H167 | Sabit | 5 | — |
| I167 | Sabit | 24.04 | — |
| J167 | Formül | =I167*D167 | 240.39999999999998 |
| K167 | Formül | =I167*1.2 | 28.848 |
| L167 | Formül | =E167-E167*$E$223/100 | 37.430279999999996 |
| M167 | Formül | =(L167-K167)/K167 | 0.29749999999999993 |
| A168 | Sabit | 15 | — |
| B168 | Sabit | 50x1000 PVC BORU | — |
| C168 | Sabit | ADET | — |
| D168 | Formül | =H168*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 10 |
| E168 | Formül | =K168*1.73 | 83.57976 |
| F168 | Formül | =E168*D168 | 835.7975999999999 |
| H168 | Sabit | 5 | — |
| I168 | Sabit | 40.26 | — |
| J168 | Formül | =I168*D168 | 402.59999999999997 |
| K168 | Formül | =I168*1.2 | 48.312 |
| L168 | Formül | =E168-E168*$E$223/100 | 62.684819999999995 |
| M168 | Formül | =(L168-K168)/K168 | 0.29749999999999993 |
| A169 | Sabit | 16 | — |
| B169 | Sabit | 50x50 TEK ÇATAL | — |
| C169 | Sabit | ADET | — |
| D169 | Formül | =H169*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 6 |
| E169 | Formül | =K169*1.73 | 51.381 |
| F169 | Formül | =E169*D169 | 308.286 |
| H169 | Sabit | 3 | — |
| I169 | Sabit | 24.75 | — |
| J169 | Formül | =I169*D169 | 148.5 |
| K169 | Formül | =I169*1.2 | 29.7 |
| L169 | Formül | =E169-E169*$E$223/100 | 38.53575 |
| M169 | Formül | =(L169-K169)/K169 | 0.29750000000000004 |
| A170 | Sabit | 17 | — |
| B170 | Sabit | 50x45 DİRSEK  | — |
| C170 | Sabit | ADET | — |
| D170 | Formül | =H170*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 10 |
| E170 | Formül | =K170*1.73 | 25.472519999999996 |
| F170 | Formül | =E170*D170 | 254.72519999999997 |
| H170 | Sabit | 5 | — |
| I170 | Sabit | 12.27 | — |
| J170 | Formül | =I170*D170 | 122.69999999999999 |
| K170 | Formül | =I170*1.2 | 14.723999999999998 |
| L170 | Formül | =E170-E170*$E$223/100 | 19.104389999999995 |
| M170 | Formül | =(L170-K170)/K170 | 0.2974999999999998 |
| A171 | Sabit | 18 | — |
| B171 | Sabit | 50x87 DİRSEK | — |
| C171 | Sabit | ADET | — |
| D171 | Formül | =H171*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 10 |
| E171 | Formül | =K171*1.73 | 29.8944 |
| F171 | Formül | =E171*D171 | 298.944 |
| H171 | Sabit | 5 | — |
| I171 | Sabit | 14.4 | — |
| J171 | Formül | =I171*D171 | 144 |
| K171 | Formül | =I171*1.2 | 17.28 |
| L171 | Formül | =E171-E171*$E$223/100 | 22.4208 |
| M171 | Formül | =(L171-K171)/K171 | 0.29749999999999993 |
| A172 | Sabit | 19 | — |
| B172 | Sabit | YER SÜZGECİ 80x80 | — |
| C172 | Sabit | ADET | — |
| D172 | Formül | =H172*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 2 |
| E172 | Formül | =K172*1.73 | 93.42 |
| F172 | Formül | =E172*D172 | 186.84 |
| H172 | Sabit | 1 | — |
| I172 | Sabit | 45 | — |
| J172 | Formül | =I172*D172 | 90 |
| K172 | Formül | =I172*1.2 | 54 |
| L172 | Formül | =E172-E172*$E$223/100 | 70.065 |
| M172 | Formül | =(L172-K172)/K172 | 0.29749999999999993 |
| A173 | Sabit | 20 | — |
| B173 | Sabit | TAKOZ CONTA | — |
| C173 | Sabit | ADET | — |
| D173 | Formül | =H173*('BİNA BİLGİLERİ'!$B$62+'BİNA BİLGİLERİ'!$E$62) | 10 |
| E173 | Formül | =K173*1.73 | 16.608 |
| F173 | Formül | =E173*D173 | 166.08 |
| H173 | Sabit | 5 | — |
| I173 | Sabit | 8 | — |
| J173 | Formül | =I173*D173 | 80 |
| K173 | Formül | =I173*1.2 | 9.6 |
| L173 | Formül | =E173-E173*$E$223/100 | 12.456 |
| M173 | Formül | =(L173-K173)/K173 | 0.2975 |
| A174 | Sabit | 21 | — |
| B174 | Sabit | ALATURKA WC TAŞ'I + S'İ | — |
| C174 | Sabit | ADET | — |
| D174 | Formül | ='BİNA BİLGİLERİ'!B63+'BİNA BİLGİLERİ'!E63 | 0 |
| E174 | Formül | =K174*1.73 | 3030.96 |
| F174 | Formül | =E174*D174 | 0 |
| I174 | Sabit | 1460 | — |
| J174 | Formül | =I174*D174 | 0 |
| K174 | Formül | =I174*1.2 | 1752 |
| L174 | Formül | =E174-E174*$E$223/100 | 2273.2200000000003 |
| M174 | Formül | =(L174-K174)/K174 | 0.29750000000000015 |
| A175 | Sabit | 22 | — |
| B175 | Sabit | ASMA REZERVUAR | — |
| C175 | Sabit | ADET | — |
| D175 | Formül | =D174 | 0 |
| E175 | Formül | =K175*1.73 | 1125.192 |
| F175 | Formül | =E175*D175 | 0 |
| I175 | Sabit | 542 | — |
| J175 | Formül | =I175*D175 | 0 |
| K175 | Formül | =I175*1.2 | 650.4 |
| L175 | Formül | =E175-E175*$E$223/100 | 843.894 |
| M175 | Formül | =(L175-K175)/K175 | 0.29750000000000004 |
| A176 | Sabit | 23 | — |
| B176 | Sabit | UZUN MUSLUK | — |
| C176 | Sabit | ADET | — |
| D176 | Formül | =D174 | 0 |
| E176 | Formül | =K176*1.73 | 415.2 |
| F176 | Formül | =E176*D176 | 0 |
| I176 | Sabit | 200 | — |
| J176 | Formül | =I176*D176 | 0 |
| K176 | Formül | =I176*1.2 | 240 |
| L176 | Formül | =E176-E176*$E$223/100 | 311.4 |
| M176 | Formül | =(L176-K176)/K176 | 0.29749999999999993 |
| A177 | Sabit | 24 | — |
| B177 | Sabit | TAHARET MUSLUĞU | — |
| C177 | Sabit | ADET | — |
| D177 | Formül | =SUM('BİNA BİLGİLERİ'!B61:F64)*2 | 12 |
| E177 | Formül | =K177*1.73 | 166.07999999999998 |
| F177 | Formül | =E177*D177 | 1992.9599999999998 |
| I177 | Sabit | 80 | — |
| J177 | Formül | =I177*D177 | 960 |
| K177 | Formül | =I177*1.2 | 96 |
| L177 | Formül | =E177-E177*$E$223/100 | 124.55999999999997 |
| M177 | Formül | =(L177-K177)/K177 | 0.2974999999999997 |
| A178 | Sabit | 25 | — |
| B178 | Sabit | MUSLUK AYNASI | — |
| C178 | Sabit | ADET | — |
| D178 | Formül | =D177 | 12 |
| E178 | Formül | =K178*1.73 | 12.975 |
| F178 | Formül | =E178*D178 | 155.7 |
| I178 | Sabit | 6.25 | — |
| J178 | Formül | =I178*D178 | 75 |
| K178 | Formül | =I178*1.2 | 7.5 |
| L178 | Formül | =E178-E178*$E$223/100 | 9.73125 |
| M178 | Formül | =(L178-K178)/K178 | 0.29749999999999993 |
| A179 | Sabit | 26 | — |
| B179 | Sabit | DUŞ TEKNESİ  80x80 | — |
| C179 | Sabit | ADET | — |
| D179 | Formül | ='BİNA BİLGİLERİ'!B64+'BİNA BİLGİLERİ'!E64 | 2 |
| E179 | Formül | =K179*1.73 | 3736.8 |
| F179 | Formül | =E179*D179 | 7473.6 |
| I179 | Sabit | 1800 | — |
| J179 | Formül | =I179*D179 | 3600 |
| K179 | Formül | =I179*1.2 | 2160 |
| L179 | Formül | =E179-E179*$E$223/100 | 2802.6000000000004 |
| M179 | Formül | =(L179-K179)/K179 | 0.29750000000000015 |
| A180 | Sabit | 27 | — |
| B180 | Sabit | DUŞ BATARYASI (MİX) | — |
| C180 | Sabit | ADET | — |
| D180 | Formül | =D179 | 2 |
| E180 | Formül | =K180*1.73 | 2076 |
| F180 | Formül | =E180*D180 | 4152 |
| I180 | Sabit | 1000 | — |
| J180 | Formül | =I180*D180 | 2000 |
| K180 | Formül | =I180*1.2 | 1200 |
| L180 | Formül | =E180-E180*$E$223/100 | 1557 |
| M180 | Formül | =(L180-K180)/K180 | 0.2975 |
| A181 | Sabit | 28 | — |
| B181 | Sabit | DUŞ TELEFONU | — |
| C181 | Sabit | ADET | — |
| D181 | Formül | =D179 | 2 |
| E181 | Formül | =K181*1.73 | 259.5 |
| F181 | Formül | =E181*D181 | 519 |
| I181 | Sabit | 125 | — |
| J181 | Formül | =I181*D181 | 250 |
| K181 | Formül | =I181*1.2 | 150 |
| L181 | Formül | =E181-E181*$E$223/100 | 194.625 |
| M181 | Formül | =(L181-K181)/K181 | 0.2975 |
| A182 | Sabit | 29 | — |
| B182 | Sabit | DUŞ TRONBLEMİ | — |
| C182 | Sabit | ADET | — |
| D182 | Formül | =D179 | 2 |
| E182 | Formül | =K182*1.73 | 166.07999999999998 |
| F182 | Formül | =E182*D182 | 332.15999999999997 |
| I182 | Sabit | 80 | — |
| J182 | Formül | =I182*D182 | 160 |
| K182 | Formül | =I182*1.2 | 96 |
| L182 | Formül | =E182-E182*$E$223/100 | 124.55999999999997 |
| M182 | Formül | =(L182-K182)/K182 | 0.2974999999999997 |
| A183 | Sabit | 30 | — |
| B183 | Sabit | ALTTAN ÇIKIŞLI KLOZET + REZERVUAR | — |
| C183 | Sabit | ADET | — |
| D183 | Formül | ='BİNA BİLGİLERİ'!B61+'BİNA BİLGİLERİ'!E61 | 2 |
| E183 | Formül | =K183*1.73 | 6228 |
| F183 | Formül | =E183*D183 | 12456 |
| I183 | Sabit | 3000 | — |
| J183 | Formül | =I183*D183 | 6000 |
| K183 | Formül | =I183*1.2 | 3600 |
| L183 | Formül | =E183-E183*$E$223/100 | 4671 |
| M183 | Formül | =(L183-K183)/K183 | 0.2975 |
| A184 | Sabit | 31 | — |
| B184 | Sabit | KLOZET İÇ TAKIM | — |
| C184 | Sabit | ADET | — |
| D184 | Formül | =D183 | 2 |
| E184 | Formül | =K184*1.73 | 332.15999999999997 |
| F184 | Formül | =E184*D184 | 664.3199999999999 |
| I184 | Sabit | 160 | — |
| J184 | Formül | =I184*D184 | 320 |
| K184 | Formül | =I184*1.2 | 192 |
| L184 | Formül | =E184-E184*$E$223/100 | 249.11999999999995 |
| M184 | Formül | =(L184-K184)/K184 | 0.2974999999999997 |
| A185 | Sabit | 32 | — |
| B185 | Sabit | KLOZET KAPAĞI | — |
| C185 | Sabit | ADET | — |
| D185 | Formül | =D183 | 2 |
| E185 | Formül | =K185*1.73 | 280.26 |
| F185 | Formül | =E185*D185 | 560.52 |
| I185 | Sabit | 135 | — |
| J185 | Formül | =I185*D185 | 270 |
| K185 | Formül | =I185*1.2 | 162 |
| L185 | Formül | =E185-E185*$E$223/100 | 210.195 |
| M185 | Formül | =(L185-K185)/K185 | 0.29749999999999993 |
| A186 | Sabit | 33 | — |
| B186 | Sabit | KLOZET MONTAJ VİDASI | — |
| C186 | Sabit | ADET | — |
| D186 | Formül | =D183*2 | 4 |
| E186 | Formül | =K186*1.73 | 20.759999999999998 |
| F186 | Formül | =E186*D186 | 83.03999999999999 |
| I186 | Sabit | 10 | — |
| J186 | Formül | =I186*D186 | 40 |
| K186 | Formül | =I186*1.2 | 12 |
| L186 | Formül | =E186-E186*$E$223/100 | 15.569999999999997 |
| M186 | Formül | =(L186-K186)/K186 | 0.2974999999999997 |
| A187 | Sabit | 34 | — |
| B187 | Sabit | ÇELİK SPİRAL HORTUM | — |
| C187 | Sabit | ADET | — |
| D187 | Formül | =D183*2 | 4 |
| E187 | Formül | =K187*1.73 | 129.75 |
| F187 | Formül | =E187*D187 | 519 |
| I187 | Sabit | 62.5 | — |
| J187 | Formül | =I187*D187 | 250 |
| K187 | Formül | =I187*1.2 | 75 |
| L187 | Formül | =E187-E187*$E$223/100 | 97.3125 |
| M187 | Formül | =(L187-K187)/K187 | 0.2975 |
| A188 | Sabit | 35 | — |
| B188 | Sabit | AYAKLI LAVABO (40X50) | — |
| C188 | Sabit | ADET | — |
| D188 | Formül | ='BİNA BİLGİLERİ'!B62+'BİNA BİLGİLERİ'!E62 | 2 |
| E188 | Formül | =K188*1.73 | 4671 |
| F188 | Formül | =E188*D188 | 9342 |
| I188 | Sabit | 2250 | — |
| J188 | Formül | =I188*D188 | 4500 |
| K188 | Formül | =I188*1.2 | 2700 |
| L188 | Formül | =E188-E188*$E$223/100 | 3503.25 |
| M188 | Formül | =(L188-K188)/K188 | 0.2975 |
| A189 | Sabit | 36 | — |
| B189 | Sabit | KARTAL BATARYA (MİX) | — |
| C189 | Sabit | ADET | — |
| D189 | Formül | =D188 | 2 |
| E189 | Formül | =K189*1.73 | 2076 |
| F189 | Formül | =E189*D189 | 4152 |
| I189 | Sabit | 1000 | — |
| J189 | Formül | =I189*D189 | 2000 |
| K189 | Formül | =I189*1.2 | 1200 |
| L189 | Formül | =E189-E189*$E$223/100 | 1557 |
| M189 | Formül | =(L189-K189)/K189 | 0.2975 |
| A190 | Sabit | 37 | — |
| B190 | Sabit | LAVABO SİFONU | — |
| C190 | Sabit | ADET | — |
| D190 | Formül | =D188 | 2 |
| E190 | Formül | =K190*1.73 | 62.28 |
| F190 | Formül | =E190*D190 | 124.56 |
| I190 | Sabit | 30 | — |
| J190 | Formül | =I190*D190 | 60 |
| K190 | Formül | =I190*1.2 | 36 |
| L190 | Formül | =E190-E190*$E$223/100 | 46.71 |
| M190 | Formül | =(L190-K190)/K190 | 0.29750000000000004 |
| A191 | Sabit | 38 | — |
| B191 | Sabit | LAVABO CİVATASI | — |
| C191 | Sabit | ADET | — |
| D191 | Formül | =D188*2 | 4 |
| E191 | Formül | =K191*1.73 | 43.596 |
| F191 | Formül | =E191*D191 | 174.384 |
| I191 | Sabit | 21 | — |
| J191 | Formül | =I191*D191 | 84 |
| K191 | Formül | =I191*1.2 | 25.2 |
| L191 | Formül | =E191-E191*$E$223/100 | 32.696999999999996 |
| M191 | Formül | =(L191-K191)/K191 | 0.2974999999999999 |
| A192 | Sabit | 39 | — |
| B192 | Sabit | TEFLON BAND | — |
| C192 | Sabit | ADET | — |
| D192 | Formül | =SUM('BİNA BİLGİLERİ'!B61:F64) | 6 |
| E192 | Formül | =K192*1.73 | 17.645999999999997 |
| F192 | Formül | =E192*D192 | 105.87599999999998 |
| I192 | Sabit | 8.5 | — |
| J192 | Formül | =I192*D192 | 51 |
| K192 | Formül | =I192*1.2 | 10.2 |
| L192 | Formül | =E192-E192*$E$223/100 | 13.234499999999997 |
| M192 | Formül | =(L192-K192)/K192 | 0.2974999999999998 |
| D193 | Sabit | TOPLAM | — |
| F193 | Formül | =SUBTOTAL(9,F154:F192) | 52706.235359999984 |
| I193 | Formül | =SUBTOTAL(9,I154:I192) | 12830.27 |
| J193 | Formül | =SUBTOTAL(9,J154:J192) | 25388.36 |
| K193 | Formül | =SUBTOTAL(9,K154:K192) | 15396.324 |
| L193 | Formül | =SUBTOTAL(9,L154:L192) | 19976.730389999997 |
| A194 | Sabit |                       ÇATI OLUĞU VE BORU GRUBU | — |
| D194 | Sabit | 1009 | — |
| A195 | Sabit | 1 | — |
| B195 | Sabit | METAL ÇATI OLUĞU  150 mm | — |
| C195 | Sabit | 5000 | — |
| D195 | Formül | =('BİNA BİLGİLERİ'!B17/5) | 4.112 |
| E195 | Formül | =C195*127/1000 | 635 |
| F195 | Formül | =E195*D195 | 2611.12 |
| I195 | Formül | =C195*61/1000 | 305 |
| J195 | Formül | =I195*D195 | 1254.16 |
| K195 | Formül | =I195*1.2 | 366 |
| L195 | Formül | =E195-E195*$E$223/100 | 476.25 |
| M195 | Formül | =(L195-K195)/K195 | 0.3012295081967213 |
| A196 | Sabit | 2 | — |
| B196 | Sabit | METAL ÇATI OLUĞU  150 mm | — |
| E196 | Formül | =C196*127/1000 | 0 |
| F196 | Formül | =E196*D196 | 0 |
| I196 | Formül | =C196*61/1000 | 0 |
| J196 | Formül | =I196*D196 | 0 |
| K196 | Formül | =I196*1.2 | 0 |
| L196 | Formül | =E196-E196*$E$223/100 | 0 |
| M196 | Formül | =(L196-K196)/K196 | #DIV/0! |
| A197 | Sabit | 3 | — |
| B197 | Sabit | METAL ÇATI OLUĞU  150 mm | — |
| E197 | Formül | =C197*127/1000 | 0 |
| F197 | Formül | =E197*D197 | 0 |
| I197 | Formül | =C197*61/1000 | 0 |
| J197 | Formül | =I197*D197 | 0 |
| K197 | Formül | =I197*1.2 | 0 |
| L197 | Formül | =E197-E197*$E$223/100 | 0 |
| M197 | Formül | =(L197-K197)/K197 | #DIV/0! |
| A198 | Sabit | 4 | — |
| B198 | Sabit | METAL ÇATI OLUĞU  150 mm | — |
| E198 | Formül | =C198*127/1000 | 0 |
| F198 | Formül | =E198*D198 | 0 |
| I198 | Formül | =C198*61/1000 | 0 |
| J198 | Formül | =I198*D198 | 0 |
| K198 | Formül | =I198*1.2 | 0 |
| L198 | Formül | =E198-E198*$E$223/100 | 0 |
| M198 | Formül | =(L198-K198)/K198 | #DIV/0! |
| A199 | Sabit | 5 | — |
| B199 | Sabit | METAL OLUK KAPAĞI   Sağ 3 - Sol 3 | — |
| C199 | Sabit | ADET | — |
| D199 | Formül | ='BİNA BİLGİLERİ'!B4 | 6 |
| E199 | Formül | =K199*1.73 | 14.532 |
| F199 | Formül | =E199*D199 | 87.19200000000001 |
| I199 | Sabit | 7 | — |
| J199 | Formül | =I199*D199 | 42 |
| K199 | Formül | =I199*1.2 | 8.4 |
| L199 | Formül | =E199-E199*$E$223/100 | 10.899000000000001 |
| M199 | Formül | =(L199-K199)/K199 | 0.29750000000000004 |
| A200 | Sabit | 6 | — |
| B200 | Sabit | METAL OLUK İNİŞİ | — |
| C200 | Sabit | ADET | — |
| D200 | Formül | =D199 | 6 |
| E200 | Formül | =K200*1.73 | 16.608 |
| F200 | Formül | =E200*D200 | 99.648 |
| I200 | Sabit | 8 | — |
| J200 | Formül | =I200*D200 | 48 |
| K200 | Formül | =I200*1.2 | 9.6 |
| L200 | Formül | =E200-E200*$E$223/100 | 12.456 |
| M200 | Formül | =(L200-K200)/K200 | 0.2975 |
| A201 | Sabit | 7 | — |
| B201 | Sabit | 70 mm PVC BORU | — |
| C201 | Sabit | L:3000 | — |
| D201 | Formül | =D199*'BİNA BİLGİLERİ'!B7 | 6 |
| E201 | Formül | =K201*1.73 | 386.13599999999997 |
| F201 | Formül | =E201*D201 | 2316.816 |
| I201 | Sabit | 186 | — |
| J201 | Formül | =I201*D201 | 1116 |
| K201 | Formül | =I201*1.2 | 223.2 |
| L201 | Formül | =E201-E201*$E$223/100 | 289.602 |
| M201 | Formül | =(L201-K201)/K201 | 0.29749999999999993 |
| A202 | Sabit | 8 | — |
| B202 | Sabit | 70 mm PVC BORU | — |
| C202 | Sabit | L:2000 | — |
| E202 | Formül | =K202*1.73 | 174.384 |
| F202 | Formül | =E202*D202 | 0 |
| I202 | Sabit | 84 | — |
| J202 | Formül | =I202*D202 | 0 |
| K202 | Formül | =I202*1.2 | 100.8 |
| L202 | Formül | =E202-E202*$E$223/100 | 130.78799999999998 |
| M202 | Formül | =(L202-K202)/K202 | 0.2974999999999999 |
| A203 | Sabit | 9 | — |
| B203 | Sabit | 70 mm PVC BORU | — |
| C203 | Sabit | L:500 | — |
| D203 | Formül | =D199 | 6 |
| E203 | Formül | =K203*1.73 | 80.964 |
| F203 | Formül | =E203*D203 | 485.784 |
| I203 | Sabit | 39 | — |
| J203 | Formül | =I203*D203 | 234 |
| K203 | Formül | =I203*1.2 | 46.8 |
| L203 | Formül | =E203-E203*$E$223/100 | 60.723 |
| M203 | Formül | =(L203-K203)/K203 | 0.29750000000000004 |
| A204 | Sabit | 10 | — |
| B204 | Sabit | 70 mm AÇIK DİRSEK | — |
| C204 | Sabit | ADET | — |
| D204 | Formül | =D199*2 | 12 |
| E204 | Formül | =K204*1.73 | 80.964 |
| F204 | Formül | =E204*D204 | 971.568 |
| I204 | Sabit | 39 | — |
| J204 | Formül | =I204*D204 | 468 |
| K204 | Formül | =I204*1.2 | 46.8 |
| L204 | Formül | =E204-E204*$E$223/100 | 60.723 |
| M204 | Formül | =(L204-K204)/K204 | 0.29750000000000004 |
| A205 | Sabit | 11 | — |
| B205 | Sabit | 70 mm KAPALI DİRSEK | — |
| C205 | Sabit | ADET | — |
| D205 | Formül | =D199 | 6 |
| E205 | Formül | =K205*1.73 | 39.444 |
| F205 | Formül | =E205*D205 | 236.66400000000002 |
| I205 | Sabit | 19 | — |
| J205 | Formül | =I205*D205 | 114 |
| K205 | Formül | =I205*1.2 | 22.8 |
| L205 | Formül | =E205-E205*$E$223/100 | 29.583000000000002 |
| M205 | Formül | =(L205-K205)/K205 | 0.29750000000000004 |
| A206 | Sabit | 12 | — |
| B206 | Sabit | 70'LİK BORU KELEPÇESİ | — |
| C206 | Sabit | ADET | — |
| D206 | Formül | =D199*3 | 18 |
| E206 | Formül | =K206*1.73 | 13.763879999999999 |
| F206 | Formül | =E206*D206 | 247.74983999999998 |
| I206 | Sabit | 6.63 | — |
| J206 | Formül | =I206*D206 | 119.34 |
| K206 | Formül | =I206*1.2 | 7.9559999999999995 |
| L206 | Formül | =E206-E206*$E$223/100 | 10.322909999999998 |
| M206 | Formül | =(L206-K206)/K206 | 0.2974999999999999 |
| A207 | Sabit | 13 | — |
| B207 | Sabit | METAL OLUK KANCASI | — |
| C207 | Sabit | ADET | — |
| D207 | Formül | ='BİNA BİLGİLERİ'!B17/0.5 | 41.12 |
| E207 | Formül | =K207*1.73 | 10.379999999999999 |
| F207 | Formül | =E207*D207 | 426.82559999999995 |
| I207 | Sabit | 5 | — |
| J207 | Formül | =I207*D207 | 205.6 |
| K207 | Formül | =I207*1.2 | 6 |
| L207 | Formül | =E207-E207*$E$223/100 | 7.784999999999998 |
| M207 | Formül | =(L207-K207)/K207 | 0.2974999999999997 |
| D208 | Sabit | TOPLAM | — |
| F208 | Formül | =SUBTOTAL(9,F195:F207) | 7483.36744 |
| I208 | Formül | =SUBTOTAL(9,I195:I207) | 698.63 |
| J208 | Formül | =SUBTOTAL(9,J195:J207) | 3601.1 |
| K208 | Formül | =SUBTOTAL(9,K195:K207) | 838.3559999999999 |
| L208 | Formül | =SUBTOTAL(9,L195:L207) | 1089.1319100000003 |
| A209 | Sabit |                            BOYA VE MASTİK GRUBU | — |
| D209 | Sabit | 1010 | — |
| A210 | Sabit | 1 | — |
| B210 | Sabit | FAVORİ DIŞ CEPHE SİLİKONLU | — |
| C210 | Sabit | 20 kg | — |
| E210 | Formül | =K210*1.73 | 6487.5 |
| F210 | Formül | =E210*D210 | 0 |
| I210 | Sabit | 3125 | — |
| J210 | Formül | =I210*D210 | 0 |
| K210 | Formül | =I210*1.2 | 3750 |
| L210 | Formül | =E210-E210*$E$223/100 | 4865.625 |
| M210 | Formül | =(L210-K210)/K210 | 0.2975 |
| A211 | Sabit | 2 | — |
| B211 | Sabit | FAVORİ DIŞ CEPHE SİLİKONLU | — |
| C211 | Sabit | 10 kg | — |
| D211 | Formül | =ROUNDUP(('BİNA BİLGİLERİ'!B32*'BİNA BİLGİLERİ'!B9*0.25)/10,0) | 3 |
| E211 | Formül | =K211*1.73 | 3633 |
| F211 | Formül | =E211*D211 | 10899 |
| I211 | Sabit | 1750 | — |
| J211 | Formül | =I211*D211 | 5250 |
| K211 | Formül | =I211*1.2 | 2100 |
| L211 | Formül | =E211-E211*$E$223/100 | 2724.75 |
| M211 | Formül | =(L211-K211)/K211 | 0.2975 |
| A212 | Sabit | 3 | — |
| B212 | Sabit | FAVORİ DIŞ CEPHE SİLİKONLU (KÖŞE SÖVESİ İÇİN) | — |
| C212 | Sabit | 2.5 kg | — |
| D212 | Sabit | 1 | — |
| E212 | Formül | =K212*1.73 | 1349.4 |
| F212 | Formül | =E212*D212 | 1349.4 |
| I212 | Sabit | 650 | — |
| J212 | Formül | =I212*D212 | 650 |
| K212 | Formül | =I212*1.2 | 780 |
| L212 | Formül | =E212-E212*$E$223/100 | 1012.0500000000001 |
| M212 | Formül | =(L212-K212)/K212 | 0.2975000000000001 |
| A213 | Sabit | 4 | — |
| B213 | Sabit | FAVORİ  İÇ ÇEPHE PLASTİK | — |
| C213 | Sabit | 20 kg | — |
| E213 | Formül | =K213*1.73 | 4152 |
| F213 | Formül | =E213*D213 | 0 |
| I213 | Sabit | 2000 | — |
| J213 | Formül | =I213*D213 | 0 |
| K213 | Formül | =I213*1.2 | 2400 |
| L213 | Formül | =E213-E213*$E$223/100 | 3114 |
| M213 | Formül | =(L213-K213)/K213 | 0.2975 |
| A214 | Sabit | 5 | — |
| B214 | Sabit | FAVORİ  İÇ ÇEPHE PLASTİK | — |
| C214 | Sabit | 10 kg | — |
| D214 | Formül | =ROUNDUP(((('BİNA BİLGİLERİ'!B9*'BİNA BİLGİLERİ'!B32)+(('BİNA BİLGİLERİ'!B33+'BİNA BİLGİLERİ'!B33)*'BİNA BİLGİLERİ'!B9))*0.25)/10,0) | 8 |
| E214 | Formül | =K214*1.73 | 2767.308 |
| F214 | Formül | =E214*D214 | 22138.464 |
| I214 | Sabit | 1333 | — |
| J214 | Formül | =I214*D214 | 10664 |
| K214 | Formül | =I214*1.2 | 1599.6 |
| L214 | Formül | =E214-E214*$E$223/100 | 2075.4809999999998 |
| M214 | Formül | =(L214-K214)/K214 | 0.29749999999999993 |
| A215 | Sabit | 6 | — |
| B215 | Sabit | FAVORİ EXTRA TAVAN BOYASI | — |
| C215 | Sabit | 17.5 kg | — |
| E215 | Formül | =K215*1.73 | 2076 |
| F215 | Formül | =E215*D215 | 0 |
| I215 | Sabit | 1000 | — |
| J215 | Formül | =I215*D215 | 0 |
| K215 | Formül | =I215*1.2 | 1200 |
| L215 | Formül | =E215-E215*$E$223/100 | 1557 |
| M215 | Formül | =(L215-K215)/K215 | 0.2975 |
| A216 | Sabit | 7 | — |
| B216 | Sabit | FAVORİ EXTRA TAVAN BOYASI | — |
| C216 | Sabit | 10  kg | — |
| D216 | Formül | =ROUNDUP(('BİNA BİLGİLERİ'!B35*0.29)/10,0) | 3 |
| E216 | Formül | =K216*1.73 | 1262.208 |
| F216 | Formül | =E216*D216 | 3786.6240000000003 |
| I216 | Sabit | 608 | — |
| J216 | Formül | =I216*D216 | 1824 |
| K216 | Formül | =I216*1.2 | 729.6 |
| L216 | Formül | =E216-E216*$E$223/100 | 946.6560000000001 |
| M216 | Formül | =(L216-K216)/K216 | 0.29750000000000004 |
| A217 | Sabit | 8 | — |
| B217 | Sabit | METAL AKSAM RAPİD BOYA ( BEYAZ ) | — |
| C217 | Sabit | 2.5 kg | — |
| D217 | Sabit | 0 | — |
| E217 | Formül | =K217*1.73 | 1384.692 |
| F217 | Formül | =E217*D217 | 0 |
| I217 | Sabit | 667 | — |
| J217 | Formül | =I217*D217 | 0 |
| K217 | Formül | =I217*1.2 | 800.4 |
| L217 | Formül | =E217-E217*$E$223/100 | 1038.519 |
| M217 | Formül | =(L217-K217)/K217 | 0.29750000000000004 |
| A218 | Sabit | 9 | — |
| B218 | Sabit | SELÜLOZİK TİNER | — |
| C218 | Sabit | 1 kg | — |
| D218 | Sabit | 0 | — |
| E218 | Formül | =K218*1.73 | 280.26 |
| F218 | Formül | =E218*D218 | 0 |
| I218 | Sabit | 135 | — |
| J218 | Formül | =I218*D218 | 0 |
| K218 | Formül | =I218*1.2 | 162 |
| L218 | Formül | =E218-E218*$E$223/100 | 210.195 |
| M218 | Formül | =(L218-K218)/K218 | 0.29749999999999993 |
| A219 | Sabit | 10 | — |
| B219 | Sabit | SİLİKONİZE MASTİK BEYAZ | — |
| C219 | Sabit | 1 kutu / 25 Adet | — |
| D219 | Formül | =IF((COUNTIF('BİNA BİLGİLERİ'!D32:F35,"YALIBASKI SİDİNG FİBERCEMENT")+COUNTIF('BİNA BİLGİLERİ'!D32:F35,"AĞAÇDESEN FUGALI FİBERCEMENT")+COUNTIF('BİNA BİLGİLERİ'!D32:F35,"TAŞDESEN FUGALI FİBERCEMENT")+COUNTIF('BİNA BİLGİLERİ'!D32:F35,"AHŞAP DESEN LEVHA")+COUNTIF('BİNA BİLGİLERİ'!D32:F35,"FİBERCEMENT LEVHA"))>0,'BİNA BİLGİLERİ'!B2,50) | 50 |
| E219 | Formül | =K219*1.73 | 66.432 |
| F219 | Formül | =E219*D219 | 3321.6 |
| I219 | Sabit | 32 | — |
| J219 | Formül | =I219*D219 | 1600 |
| K219 | Formül | =I219*1.2 | 38.4 |
| L219 | Formül | =E219-E219*$E$223/100 | 49.824 |
| M219 | Formül | =(L219-K219)/K219 | 0.2975 |
| A220 | Sabit | 11 | — |
| B220 | Sabit | SİLİKONİZE MASTİK RENKLİ | — |
| C220 | Sabit | 1 kutu / 25 Adet | — |
| D220 | Sabit | 15 | — |
| E220 | Formül | =K220*1.73 | 78.888 |
| F220 | Formül | =E220*D220 | 1183.3200000000002 |
| I220 | Sabit | 38 | — |
| J220 | Formül | =I220*D220 | 570 |
| K220 | Formül | =I220*1.2 | 45.6 |
| L220 | Formül | =E220-E220*$E$223/100 | 59.166000000000004 |
| M220 | Formül | =(L220-K220)/K220 | 0.29750000000000004 |
| D221 | Sabit | TOPLAM | — |
| F221 | Formül | =SUBTOTAL(9,F210:F220) | 42678.408 |
| I221 | Formül | =SUBTOTAL(9,I210:I220) | 11338 |
| J221 | Formül | =SUBTOTAL(9,J210:J220) | 20558 |
| K221 | Formül | =SUBTOTAL(9,K210:K220) | 13605.6 |
| L221 | Formül | =SUBTOTAL(9,L210:L220) | 17653.266 |
| M221 | Formül | =SUBTOTAL(9,M210:M220) | 3.272499999999999 |
| D222 | Sabit | GRUP TOPLAMI | — |
| F222 | Formül | =+F37+F95+F74+F125+F135+F152+F193+F208+F221+F53 | 1113475.7456068546 |
| I222 | Formül | =+I37+I95+I74+I125+I135+I152+I193+I208+I221+I53 | 132590.61789014167 |
| J222 | Formül | =+J37+J95+J74+J125+J135+J152+J193+J208+J221+J53 | 502340.24540090957 |
| K222 | Formül | =+K37+K95+K74+K125+K135+K152+K193+K208+K221+K53 | 159108.74146817 |
| L222 | Formül | =+L37+L95+L74+L125+L135+L152+L193+L208+L221+L53 | 237928.07976010282 |
| M222 | Formül | =+M37+M95+M74+M125+M135+M152+M193+M208+M221+M53 | 3.272499999999999 |
| D223 | Sabit | İSKONTO | — |
| E223 | Sabit | 25 | — |
| F223 | Formül | =F222*E223/100 | 278368.93640171364 |
| G223 | Sabit | KAR % | — |
| H223 | Formül | =(F226-H225)/H225 | 0.38536051486155537 |
| I223 | Formül | =I222*Y223/100 | 0 |
| J223 | Formül | =J222*Y223/100 | 0 |
| K223 | Formül | =K222*I223/100 | 0 |
| L223 | Formül | =L222*J223/100 | 0 |
| M223 | Formül | =M222*K223/100 | 0 |
| D224 | Sabit | ARA TOPLAM | — |
| F224 | Formül | =F222-F223 | 835106.8092051409 |
| G224 | Sabit | KDV HARİÇ  | — |
| H224 | Formül | =J221+J208+J193+J152+J135+J125+J95+J74+J53+J37 | 502340.24540090957 |
| I224 | Formül | =I222-I223 | 132590.61789014167 |
| J224 | Formül | =J222-J223 | 502340.24540090957 |
| K224 | Formül | =K222-K223 | 159108.74146817 |
| L224 | Formül | =L222-L223 | 237928.07976010282 |
| M224 | Formül | =M222-M223 | 3.272499999999999 |
| D225 | Sabit | KDV | — |
| F225 | Formül | =F224*E225/100 | 0 |
| G225 | Sabit | KDV DAHİL  | — |
| H225 | Formül | =H224*1.2 | 602808.2944810914 |
| I225 | Formül | =I224*Y225/100 | 0 |
| J225 | Formül | =J224*Y225/100 | 0 |
| K225 | Formül | =K224*I225/100 | 0 |
| L225 | Formül | =L224*J225/100 | 0 |
| M225 | Formül | =M224*K225/100 | 0 |
| D226 | Sabit | GENEL TOPLAM | — |
| F226 | Formül | =F224+F225 | 835106.8092051409 |
| G226 | Sabit | BRÜT KAR  | — |
| H226 | Formül | =F226-H225 | 232298.51472404948 |
| I226 | Formül | =I224+I225 | 132590.61789014167 |
| J226 | Formül | =J224+J225 | 502340.24540090957 |
| K226 | Formül | =K224+K225 | 159108.74146817 |
| L226 | Formül | =L224+L225 | 237928.07976010282 |
| M226 | Formül | =M224+M225 | 3.272499999999999 |
| A227 | Sabit | Fiyatlarımıza %20 KDV Dahil Değildir | — |
| D227 | Sabit | 1011 | — |

### İSİMLENDİRME

| Hücre | Tür | Değer / formül | Cached değer |
|---|---|---|---:|
| A1 | Sabit | VAR | — |
| A2 | Sabit | YOK | — |
| A10 | Sabit | BEYAZ | — |
| A11 | Sabit | ANTRASİT | — |
| A12 | Sabit | ALTINMEŞE | — |
| P17 | Formül | ='BİNA BİLGİLERİ'!B11 | 0.3 |
| Q17 | Formül | =VLOOKUP(P17,P19:Q26,2,1) | 1.044 |
| R17 | Formül | ='BİNA BİLGİLERİ'!B12+'BİNA BİLGİLERİ'!B36 | 121.384 |
| S17 | Formül | =Q17*R17 | 126.724896 |
| P19 | Sabit | 0.25 | — |
| Q19 | Sabit | 1.0308 | — |
| P20 | Sabit | 0.3 | — |
| Q20 | Sabit | 1.044 | — |
| P21 | Sabit | 0.35 | — |
| Q21 | Sabit | 1.0595 | — |
| P22 | Sabit | 0.4 | — |
| Q22 | Sabit | 1.077 | — |
| P23 | Sabit | 0.45 | — |
| Q23 | Sabit | 1.0966 | — |
| P24 | Sabit | 0.5 | — |
| Q24 | Sabit | 1.118 | — |
| P25 | Sabit | 0.55 | — |
| Q25 | Sabit | 1.1413 | — |
| P26 | Sabit | 0.6 | — |
| Q26 | Sabit | 1.1662 | — |
| I28 | Sabit | 1 | — |
| I29 | Sabit | 2 | — |
| L32 | Sabit | KIRMA | — |
| L33 | Sabit | BEŞİK | — |
| A34 | Sabit | ALÇIPAN | — |
| L34 | Sabit | PARAPET | — |
| Z34 | Sabit | 8 | — |
| A35 | Sabit | BORDEX | — |
| L35 | Sabit | TEK EĞİM | — |
| Z35 | Sabit | 16 | — |
| A36 | Sabit | 11 mm OSB 2 | — |
| Z36 | Sabit | 12 | — |
| A37 | Sabit | YALIBASKI SİDİNG FİBERCEMENT | — |
| Z37 | Sabit | 7 | — |
| A38 | Sabit | AĞAÇDESEN FUGALI FİBERCEMENT | — |
| Z38 | Sabit | 5 | — |
| A39 | Sabit | TAŞDESEN FUGALI FİBERCEMENT | — |
| G39 | Sabit |  NEM BARİYERİ | — |
| Z39 | Sabit | 4 | — |
| A40 | Sabit | AHŞAP DESEN LEVHA | — |
| G40 | Sabit | MEBRAN   | — |
| O40 | Sabit | ZMT'YE AİT | — |
| Z40 | Sabit | 5 | — |
| A41 | Sabit | FİBERCEMENT LEVHA | — |
| G41 | Sabit | ARDUAZLI MEBRAN | — |
| O41 | Sabit | MÜŞTERİYE AİT | — |
| Z41 | Sabit | 15 | — |
| A42 | Sabit | NEM BARİYERİ | — |
| Z42 | Sabit | 1 | — |
| A43 | Sabit | MEBRAN | — |
| Z43 | Sabit | 2 | — |
| A44 | Sabit | ARDUAZLI MEBRAN | — |
| Z44 | Sabit | 1 | — |
| A45 | Sabit | AŞIK OMEGA  | — |
| G45 | Sabit | 80 | — |
| H45 | Sabit | 2 | — |
| Z45 | Sabit | 1 | — |
| A46 | Sabit | PANEL SİSTEM | — |
| G46 | Sabit | 90 | — |
| H46 | Sabit | 2 | — |
| Z46 | Sabit | 5 | — |
| G47 | Sabit | 140 | — |
| H47 | Sabit | 3 | — |
| Z47 | Sabit | 5 | — |
| G48 | Sabit | 200 | — |
| H48 | Sabit | 4 | — |
| Z48 | Sabit | 3 | — |
| G49 | Sabit | 300 | — |
| H49 | Sabit | 6 | — |
| Z49 | Sabit | 5 | — |
| Z50 | Sabit | 5 | — |
| Z51 | Sabit | 1 | — |
| Z52 | Sabit | 5 | — |

### FORMÜL

| Hücre | Tür | Değer / formül | Cached değer |
|---|---|---|---:|
| I9 | Sabit | DIŞ DUVAR M² | — |
| C10 | Sabit | TOPLAM M² | — |
| D10 | Sabit | ADET | — |
| E10 | Sabit | FİRELİ ADET SAYISI %3 | — |
| F10 | Sabit | YÜKLENECEK ADET | — |
| I10 | Sabit | BAŞ MAKAS M² | — |
| B11 | Sabit | FİBERCEMENT YALI BASKI  | — |
| C11 | Formül | =J9+J10+2*J11 | 0 |
| D11 | Formül | =C11/0.45 | 0 |
| E11 | Formül | =D11*1.03 | 0 |
| F11 | Formül | =ROUNDUP(E11,0) | 0 |
| I11 | Sabit | DİREK SAYI*DİREK M² | — |
| B12 | Sabit | KNAUF PLUS(5CM)DIŞ | — |
| C12 | Formül | =J9*2 | 0 |
| D12 | Formül | =C12/14.4 | 0 |
| E12 | Formül | =D12*1.03 | 0 |
| F12 | Formül | =ROUNDUP(E12,0) | 0 |
| I12 | Sabit | DUVAR USTU ŞASE M² | — |
| B13 | Sabit | KNAUF PLUS (5CM) İÇ | — |
| C13 | Formül | =J13+J15 | 0 |
| D13 | Formül | =C13/14.4 | 0 |
| E13 | Formül | =D13*1.03 | 0 |
| F13 | Formül | =ROUNDUP(E13,0) | 0 |
| I13 | Sabit | İÇ DUVAR M² | — |
| B14 | Sabit | ALÇIPAN | — |
| C14 | Formül | =2*J9+4*J13+J14+J15+J20 | 0 |
| D14 | Formül | =C14/3 | 0 |
| E14 | Formül | =D14*1.03 | 0 |
| F14 | Formül | =ROUNDUP(E14,0)-F19 | 0 |
| I14 | Sabit | TAVAN M² | — |
| B15 | Sabit | FİBERCEMENT 8mm  | — |
| C15 | Formül | =J9+J10+J11+J12+J21 | 0 |
| D15 | Formül | =C15/10 | 0 |
| E15 | Formül | =D15*1.03 | 0 |
| F15 | Formül | =ROUNDUP(E15,0) | 0 |
| I15 | Sabit | ŞASE M² | — |
| B16 | Sabit | Knauf Çatı Şiltesi | — |
| C16 | Formül | =J24 | 0 |
| D16 | Formül | =C16/9.6 | 0 |
| E16 | Formül | =D16*1.03 | 0 |
| F16 | Formül | =ROUNDUP(E16,0) | 0 |
| I16 | Sabit | ÇATI M² / OSB İÇİN  | — |
| B17 | Sabit | OSB Dış Duvar + Çatı  | — |
| C17 | Formül | =J9+J10+J11+J12+J16 | 0 |
| D17 | Formül | =C17/2.97 | 0 |
| E17 | Formül | =D17*1.03 | 0 |
| F17 | Formül | =ROUNDUP(E17,0) | 0 |
| I17 | Sabit | BACA M² | — |
| B18 | Sabit | OSB ÇATI SADECE  | — |
| C18 | Formül | =J16 | 0 |
| D18 | Formül | =C18/2.97 | 0 |
| E18 | Formül | =D18*1.03 | 0 |
| F18 | Formül | =ROUNDUP(E18,0) | 0 |
| I18 | Sabit | YEŞİL ALÇIPAN M² | — |
| B19 | Sabit | YEŞİL ALÇIPAN | — |
| C19 | Formül | =(J18) | 0 |
| D19 | Formül | =C19/3 | 0 |
| E19 | Formül | =D19*1.03 | 0 |
| F19 | Formül | =ROUNDUP(E19,0) | 0 |
| I19 | Sabit | MERDİVEN | — |
| B20 | Sabit | FİBERCEMENT LAMBA ZIVANA | — |
| C20 | Formül | =J9+J10+2*J11+J12 | 0 |
| D20 | Formül | =C20/1.025 | 0 |
| E20 | Formül | =D20*1.07 | 0 |
| F20 | Formül | =ROUNDUP(E20,0) | 0 |
| I20 | Sabit | MERDİVEN ALTI | — |
| B21 | Sabit | BETOPAN  16mm | — |
| C21 | Formül | =(J15+J19) | 0 |
| D21 | Formül | =C21/3.125 | 0 |
| E21 | Formül | =D21*1.03 | 0 |
| F21 | Formül | =ROUNDUP(E21,0) | 0 |
| I21 | Sabit | SAÇAK KAPAMA MT | — |
| B22 | Sabit | SAÇAK ALIN UZUNLUK  | — |
| C22 | Formül | =(J22) | 0 |
| D22 | Formül | =C22/2.5 | 0 |
| E22 | Formül | =D22*1.01 | 0 |
| F22 | Formül | =ROUNDUP(E22,0) | 0 |
| I22 | Sabit | SAÇAK ALIN  UZUNLUK  | — |
| B23 | Sabit | BORDEX  ( Plaka ) | — |
| C23 | Formül | =J9+J10+J11+J12 | 0 |
| D23 | Formül | =C23/2.88 | 0 |
| E23 | Formül | =D23*1.03 | 0 |
| F23 | Formül | =ROUNDUP(E23,0) | 0 |
| I23 | Sabit | BACA  | — |
| I24 | Sabit | BİNA M² | — |

