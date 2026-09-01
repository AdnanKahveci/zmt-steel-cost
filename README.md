# ZMT Çelik Maliyet

Excel'deki hafif çelik bina maliyet ve malzeme kurallarını runtime Excel bağımlılığı olmadan çalıştıran .NET 8 WPF masaüstü uygulaması.

## Hazır EXE

Release çıktısı `publish/ZMT.SteelCost.App.exe` dosyasıdır. Windows 10/11 x64 üzerinde Visual Studio, .NET Runtime, Excel veya Microsoft Office kurulumu gerektirmez.

İlk çalıştırmada SQLite veritabanı ve seed verileri otomatik oluşturulur:

- Veritabanı: `%AppData%/ZMT/SteelCost/steelcost.db`
- Günlük yedek: `%AppData%/ZMT/SteelCost/Backups/`
- Loglar: `%LocalAppData%/ZMT/SteelCost/Logs/`

## Geliştirme

Gereksinim: .NET SDK 8 veya üzeri; hedef framework `net8.0-windows` olarak sabittir.

```powershell
dotnet restore ZMT.SteelCost.sln
dotnet build ZMT.SteelCost.sln -c Release
dotnet test ZMT.SteelCost.sln -c Release
```

Tek satır self-contained publish komutu:

```powershell
dotnet publish src/ZMT.SteelCost.App/ZMT.SteelCost.App.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish
```

## Kullanım akışı

1. Yeni Hesaplama'dan proje ve bina bilgilerini girin.
2. Kaplama, kapı/pencere ve vitrifiye seçimlerini tamamlayın.
3. Hesapla ile 186 satırlık sonucu ve hesap izlerini görüntüleyin.
4. Gerekli satırlarda açıklama zorunlu manuel miktar override kullanın.
5. Kategori sorumluluklarını, iskonto ve KDV'yi Ayarlar'dan belirleyin.
6. Projeyi kaydedin; fiyat ve formül snapshot'ı SQLite'a yazılır.
7. Raporlar'dan Internal Cost, Customer Offer veya Loading List PDF'i; ayrıca yeni XLSX çıktısı üretin.
8. Raporlar'daki `Ayrıntılı Teklif Formunu Aç` düğmesinden veya sol menüdeki `Teklif Formu` sayfasından müşteri/kapsam/fiyat/ödeme/teslim/firma bilgilerini düzenleyin; teklif ve teknik şartname görsellerini ekleyin.
9. Türkçe/English dilini, teknik şartname ve görsel kapsamını seçerek logolu, A/B/C/D/E bölümlü ayrı teklif PDF'ini oluşturun.

Legacy analizleri ve doğrulama kanıtları [Docs](Docs/) klasöründedir. Kaynak workbook yalnızca [Legacy](Legacy/) altında referans olarak korunur ve uygulama tarafından runtime'da okunmaz.
