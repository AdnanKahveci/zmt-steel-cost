using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using ZMT.SteelCost.Domain;
using ZMT.SteelCost.Application.Offers;

namespace ZMT.SteelCost.App.Converters;

public sealed class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
        nameof(Data), typeof(object), typeof(BindingProxy), new PropertyMetadata(null));

    public object? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    protected override Freezable CreateInstanceCore() => new BindingProxy();
}

public sealed class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is true ? Visibility.Visible : Visibility.Collapsed;
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is Visibility.Visible;
}

public sealed class InverseBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is true ? Visibility.Collapsed : Visibility.Visible;
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is Visibility.Collapsed;
}

public sealed class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is null ? Visibility.Collapsed : Visibility.Visible;
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

public sealed class FloorCountToReadOnlyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is not int floorCount || floorCount != 2;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}

public sealed class ZeroQuantityVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var hideZero = values.Length > 1 && values[1] is true;
        var quantity = values.Length > 0 && values[0] is int number ? number : 0;
        return hideZero && quantity == 0 ? Visibility.Collapsed : Visibility.Visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        targetTypes.Select(_ => Binding.DoNothing).ToArray();
}

public sealed class EnumToTurkishConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
    {
        RoofType.Hip => "Kırma",
        RoofType.Gable => "Beşik",
        RoofType.Parapet => "Parapet",
        RoofType.MonoPitch => "Tek Eğim",
        RoofSystem.PurlinOmega => "Aşık Omega",
        RoofSystem.Panel => "Panel Sistem",
        RoofCoverType.TrapezoidalSheet => "Trapez Çatı",
        RoofCoverType.SandwichPanel => "Sandviç Panel",
        RoofCoverType.MetalTile => "Metal Kiremit Çatı",
        WindowColor.White => "Beyaz",
        WindowColor.Anthracite => "Antrasit",
        WindowColor.GoldenOak => "Altınmeşe",
        ResponsibilityType.Zmt => "ZMT'ye Ait",
        ResponsibilityType.Customer => "Müşteriye Ait",
        DoorType.Steel90X205 => "Çelik Kapı 90×205",
        DoorType.Pvc90X200 => "PVC Kapı 90×200",
        DoorType.DoublePvc160X200 => "Duble PVC Kapı 160×200",
        DoorType.Melamine90X201 => "Melamin Kapı 90×201",
        DoorType.AmericanWoodFrame90X201 => "Ahşap Kasalı Amerikan Kapı 90×201",
        WindowType.Pvc105X180 => "PVC Pencere 105×180",
        WindowType.Pvc59X180 => "PVC Pencere 59×180",
        WindowType.Pvc80X120 => "PVC Pencere 80×120",
        WindowType.Pvc140X100 => "PVC Pencere 140×100",
        WindowType.Pvc140X140 => "PVC Pencere 140×140",
        WindowType.Pvc140X160 => "PVC Pencere 140×160",
        WindowType.Pvc140X180 => "PVC Pencere 140×180",
        WindowType.Pvc160X120 => "PVC Pencere 160×120",
        WindowType.Pvc160X160 => "PVC Pencere 160×160",
        WindowType.Pvc160X180 => "PVC Pencere 160×180",
        WindowType.PvcSliding180X200 => "PVC Sürgülü 180×200",
        WindowType.PvcTransom60X60 => "PVC Vasistas 60×60",
        FixtureType.Toilet => "Klozet",
        FixtureType.Washbasin => "Lavabo",
        FixtureType.SquatToilet => "Alaturka WC",
        FixtureType.ShowerTray => "Duş Teknesi",
        SurfaceLayerType.None => "—",
        SurfaceLayerType.Drywall => "Alçıpan",
        SurfaceLayerType.Bordex => "Bordex",
        SurfaceLayerType.Osb11Mm => "11 mm OSB 2",
        SurfaceLayerType.SidingFiberCement => "Yalıbaskı Siding Fibercement",
        SurfaceLayerType.WoodPatternJointedFiberCement => "Ağaçdesen Fugalı Fibercement",
        SurfaceLayerType.StonePatternJointedFiberCement => "Taşdesen Fugalı Fibercement",
        SurfaceLayerType.WoodPatternBoard => "Ahşap Desen Levha",
        SurfaceLayerType.FiberCementBoard => "Fibercement Levha",
        SurfaceLayerType.MoistureBarrier => "Nem Bariyeri",
        SurfaceLayerType.Membrane => "Membran",
        SurfaceLayerType.SlateMembrane => "Arduazlı Membran",
        ProjectStage.OfferDrawingReady => "Teklif Çizimi Hazır",
        ProjectStage.ManufacturingDrawingReady => "İmalat Çizimi Hazır",
        ProjectStage.OfferListReady => "Teklif Listesi Hazır",
        ProjectStage.ProductionListReady => "Üretim Listesi Hazır",
        ProjectStage.SentToProduction => "Üretime Verildi",
        QuantityMode.Auto => "Auto",
        QuantityMode.Manual => "Manual",
        OfferLanguage.Turkish => "Türkçe",
        OfferLanguage.English => "İngilizce",
        _ => value?.ToString() ?? string.Empty
    };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}

public sealed class FilePathToImageSourceConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string path || string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            return null;
        }
        try
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = new Uri(path, UriKind.Absolute);
            if (parameter is string widthText && int.TryParse(widthText, out var width) && width > 0)
            {
                image.DecodePixelWidth = width;
            }
            image.EndInit();
            image.Freeze();
            return image;
        }
        catch
        {
            return null;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}
