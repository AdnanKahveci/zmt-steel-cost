namespace ZMT.SteelCost.Application.Offers;

/// <summary>
/// ZMT teklif oluşturucu kaynak projesindeki teknik şartname şablonunun,
/// hesaplanan proje verilerine eklenebilen düzenlenebilir başlangıç metinleri.
/// </summary>
public static class DetailedOfferTemplates
{
    public static IReadOnlyList<TechnicalSpecSection> CreateTechnicalSpecificationSections() =>
    [
        Section(10, "KONSTRÜKSİYON STANDARDI", """
            Taşıyıcı sistem	Projesine göre statik hesabı yapılmış galvaniz kaplamalı hafif çelik profiller
            Malzeme standardı	DIN EN 10326 veya proje standardına uygun
            DX51 kalite	Akma sınırı 140 N/mm² · çekme dayanımı 270 N/mm²
            S250 kalite	Akma sınırı 250 N/mm² · çekme dayanımı 330 N/mm²
            S350 kalite	Akma sınırı 350 N/mm² · çekme dayanımı 420 N/mm²
            Bağlantılar	Projede belirtilen vida, ankraj ve birleşim elemanlarıyla yapılır.
            """),
        Section(11, "HESAP KRİTERLERİ", """
            Rüzgâr yükü	Proje yeri ve yürürlükteki standarda göre doğrulanacaktır.
            Kar yükü	Proje yeri ve yürürlükteki standarda göre doğrulanacaktır.
            Deprem kriteri	Zemin sınıfı, bina önem katsayısı ve kullanım amacı statik projede esas alınır.
            Proje notu	Nihai değerler onaylı mimari, statik ve zemin etüdü verileriyle kesinleştirilir.
            """),
        Section(12, "DIŞ VE İÇ DUVAR UYGULAMASI", """
            Dış duvar konstrüksiyonu	Projedeki duvar kalınlığına uygun galvaniz sigma/C profiller
            Dış cephe kaplaması	Hesap formunda seçilen OSB, fibercement, siding veya eşdeğer katmanlar
            Dış duvar yalıtımı	Projede belirlenen kalınlık ve yoğunlukta mineral yün
            İç duvar konstrüksiyonu	Projedeki duvar kalınlığına uygun galvaniz profiller
            İç cephe kaplaması	Hesap formunda seçilen alçıpan, bordex veya eşdeğer katmanlar
            Islak hacimler	Suya ve neme dayanıklı levha/yalıtım katmanları uygulanır.
            """),
        Section(13, "TAVAN VE ARA KAT", """
            Normal alan tavanı	12,5 mm beyaz alçıpan veya projede seçilen kaplama
            Islak alan tavanı	Neme dayanıklı levha veya projede seçilen kaplama
            Tavan yalıtımı	Projede belirtilen kalınlıkta camyünü/taşyünü
            Ara kat taşıyıcısı	Statik projede boyutlandırılan galvaniz C profiller
            Ara kat üst kaplaması	Projede belirtilen levha ve son kat döşeme sistemi
            """),
        Section(14, "ÇATI SİSTEMİ", """
            Çatı konstrüksiyonu	Hesap formunda seçilen makas/panel ve aşık sistemi
            Çatı kaplaması	Hesap formunda seçilen trapez sac, sandviç panel veya metal kiremit
            Su tahliyesi	Projeye göre metal dere, oluk ve iniş elemanları
            Yalıtım ve membran	Seçilen çatı katmanlarına ve proje detayına göre uygulanır.
            """),
        Section(15, "KAPILAR, PENCERELER VE CAMLAR", """
            Dış kapılar	Hesap formunda seçilen çelik/PVC kapı tipleri ve adetleri
            İç kapılar	Hesap formunda seçilen melamin/ahşap kasalı kapı tipleri ve adetleri
            Pencereler	Seçilen ölçü, açılım tipi, renk ve adetlere göre PVC doğrama
            Normal pencere camı	Proje şartına uygun çift cam
            Islak hacim/vasistas camı	Proje şartına uygun buzlu veya düz cam
            Aksesuarlar	Standart kol, kilit, menteşe ve bağlantı elemanları
            """),
        Section(16, "BOYA VE YÜZEY İŞLERİ", """
            Dış cephe	Kaplama tipine uygun dış cephe boya sistemi
            İç cephe	Alçıpan derz, astar ve son kat boya sistemi
            Tavan	Uygun astar ve tavan boyası
            Metal birleşimler	Gerekli noktalarda mastik ve sızdırmazlık uygulaması
            """),
        Section(17, "ELEKTRİK TESİSATI", """
            Kablolar	Proje ve standartlara uygun kesitte, belgeli ürünler
            Priz ve aydınlatma hatları	Mimari yerleşime göre çekilir.
            Armatürler	Teklif kapsamında seçilen armatür tip ve adetleri
            Sigorta kutuları	Teklif kapsamına göre dahildir.
            Ana hat bağlantısı	Aksi yazılı belirtilmedikçe işveren sorumluluğundadır.
            Zayıf akım, data, UPS ve yangın algılama	Teklif kalemlerinde ayrıca belirtilmedikçe hariçtir.
            """),
        Section(18, "SIHHİ TESİSAT VE VİTRİFİYE", """
            Temiz su boruları	Projeye uygun PPRC boru veya muadili
            Pis su boruları	Projeye uygun PVC boru veya muadili
            Vitrifiye ürünleri	Hesap formunda seçilen zemin ve üst kat adetlerine göre
            Batarya ve aksesuarlar	Teklif kalemlerinde belirtilen marka veya muadili
            Bina dışı ana hatlar	Aksi yazılı belirtilmedikçe işveren sorumluluğundadır.
            Endüstriyel mutfak ve çamaşırhane	Teklif kalemlerinde ayrıca belirtilmedikçe hariçtir.
            """),
        Section(19, "GENEL UYGULAMA NOTU", """
            Uygulama	Onaylı proje, üretici talimatları ve yürürlükteki standartlara göre yapılır.
            Değişiklik hakkı	ZMT, eşdeğer veya daha yüksek teknik özellikte malzeme kullanma hakkını saklı tutar.
            Kesin kapsam	Bu şartname, dahil/hariç işler listesi ve fiyat tablosu birlikte değerlendirilir.
            """)
    ];

    private static TechnicalSpecSection Section(int order, string title, string content) => new()
    {
        SortOrder = order,
        Title = title,
        Content = content.Trim(),
        IncludeInPdf = true
    };
}
