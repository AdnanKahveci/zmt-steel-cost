using ZMT.SteelCost.App.Mvvm;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.App.ViewModels;

public abstract class PageViewModel(MainViewModel owner, string title, string subtitle) : ObservableObject
{
    public MainViewModel Owner { get; } = owner;
    public string Title { get; } = title;
    public string Subtitle { get; } = subtitle;
}

public sealed class DashboardPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Dashboard", "Projeler ve maliyet görünümü") { }

public sealed class ProjectsPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Projeler", "Kayıtlı teklifleri açın ve yönetin") { }

public sealed class ProjectInfoPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Proje Bilgileri", "Müşteri, ekip ve teklif durumu")
{
    public Array ProjectStages { get; } = Enum.GetValues<ProjectStage>();
}

public sealed class BuildingPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Bina Bilgileri", "Geometri, profil ve çatı parametreleri")
{
    public int[] FloorCounts { get; } = [1, 2];
    public Array RoofTypes { get; } = Enum.GetValues<RoofType>();
    public Array RoofSystems { get; } = Enum.GetValues<RoofSystem>();
    public Array RoofCoverTypes { get; } = Enum.GetValues<RoofCoverType>();
    public decimal[] RoofSlopes { get; } = [0.25m, 0.30m, 0.35m, 0.40m, 0.45m, 0.50m, 0.55m, 0.60m];
    public decimal[] WallThicknesses { get; } = [80m, 90m, 140m, 200m, 300m];
}

public sealed class CladdingPageViewModel : PageViewModel
{
    public CladdingPageViewModel(MainViewModel owner) : base(owner, "Kaplama", "Yüzey katmanlarını stabil malzeme kodlarıyla tanımlayın")
    {
    }

    public Array LayerOptions { get; } = Enum.GetValues<SurfaceLayerType>();
    public decimal ExteriorLength { get => Owner.ActiveProject.Building.ExteriorWallLength; set => Owner.ActiveProject.Building.ExteriorWallLength = value; }
    public decimal InteriorLength { get => Owner.ActiveProject.Building.InteriorWallLength; set => Owner.ActiveProject.Building.InteriorWallLength = value; }
    public decimal CeilingArea { get => Owner.ActiveProject.Building.CeilingArea; set => Owner.ActiveProject.Building.CeilingArea = value; }
    public decimal RoofArea => Owner.RoofPreview.RoofCoverArea;
    public decimal WetWallLength { get => Owner.ActiveProject.Building.WetAreaWallLength; set => Owner.ActiveProject.Building.WetAreaWallLength = value; }
    public decimal WetCeilingArea { get => Owner.ActiveProject.Building.WetAreaCeilingArea; set => Owner.ActiveProject.Building.WetAreaCeilingArea = value; }

    public SurfaceLayerType ExteriorLayer1 { get => Layer(SurfaceType.ExteriorWall, 0); set => SetLayer(SurfaceType.ExteriorWall, 0, value); }
    public SurfaceLayerType ExteriorLayer2 { get => Layer(SurfaceType.ExteriorWall, 1); set => SetLayer(SurfaceType.ExteriorWall, 1, value); }
    public SurfaceLayerType ExteriorLayer3 { get => Layer(SurfaceType.ExteriorWall, 2); set => SetLayer(SurfaceType.ExteriorWall, 2, value); }
    public SurfaceLayerType InteriorLayer1 { get => Layer(SurfaceType.InteriorWall, 0); set => SetLayer(SurfaceType.InteriorWall, 0, value); }
    public SurfaceLayerType InteriorLayer2 { get => Layer(SurfaceType.InteriorWall, 1); set => SetLayer(SurfaceType.InteriorWall, 1, value); }
    public SurfaceLayerType InteriorLayer3 { get => Layer(SurfaceType.InteriorWall, 2); set => SetLayer(SurfaceType.InteriorWall, 2, value); }
    public SurfaceLayerType RoofLayer1 { get => Layer(SurfaceType.Roof, 0); set => SetLayer(SurfaceType.Roof, 0, value); }
    public SurfaceLayerType RoofLayer2 { get => Layer(SurfaceType.Roof, 1); set => SetLayer(SurfaceType.Roof, 1, value); }
    public SurfaceLayerType RoofLayer3 { get => Layer(SurfaceType.Roof, 2); set => SetLayer(SurfaceType.Roof, 2, value); }
    public SurfaceLayerType CeilingLayer1 { get => Layer(SurfaceType.Ceiling, 0); set => SetLayer(SurfaceType.Ceiling, 0, value); }
    public SurfaceLayerType CeilingLayer2 { get => Layer(SurfaceType.Ceiling, 1); set => SetLayer(SurfaceType.Ceiling, 1, value); }
    public SurfaceLayerType CeilingLayer3 { get => Layer(SurfaceType.Ceiling, 2); set => SetLayer(SurfaceType.Ceiling, 2, value); }

    private SurfaceConfiguration Surface(SurfaceType type) =>
        Owner.ActiveProject.Building.Surfaces.First(item => item.Surface == type);

    private SurfaceLayerType Layer(SurfaceType type, int index)
    {
        var layers = Surface(type).Layers;
        return layers.Count > index ? layers[index] : SurfaceLayerType.None;
    }

    private void SetLayer(SurfaceType type, int index, SurfaceLayerType value)
    {
        var layers = Surface(type).Layers;
        while (layers.Count <= index)
        {
            layers.Add(SurfaceLayerType.None);
        }
        layers[index] = value;
    }
}

public sealed class DoorWindowPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Kapı ve Pencere", "Tip, ölçü, adet ve renk seçimleri")
{
    public Array WindowColors { get; } = Enum.GetValues<WindowColor>();
}

public sealed class FixturesPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Vitrifiye", "Zemin ve üst kat adetleri") { }

public sealed class ResultsPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Hesaplama Sonucu", "186 satır, 10 grup ve kapsam özeti") { }

public sealed class MaterialsPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Malzeme ve Fiyatlar", "Aktif fiyat listesi ve malzeme kataloğu") { }

public sealed class ReportsPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Raporlar", "PDF ve Excel çıktıları") { }

public sealed class SettingsPageViewModel(MainViewModel owner)
    : PageViewModel(owner, "Ayarlar", "Döviz, metal ve ticari parametreler") { }
