using System.Globalization;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Application.Offers;

public sealed class SteelCostOfferDocumentMapper : IOfferDocumentMapper
{
    public OfferDocument Map(Project project, CalculationResult result, OfferGenerationOptions options)
    {
        ArgumentNullException.ThrowIfNull(project);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(options);

        var area = project.Building.BuildingArea;
        var projectTitle = $"{area:0.##} m² HAFİF ÇELİK BİNA FİYAT TEKLİFİ";
        var document = new OfferDocument
        {
            Info = new OfferInfo
            {
                OfferDate = project.DocumentDate,
                PreparedBy = project.OfferPreparedBy,
                CompanyName = project.Company,
                AuthorizedPerson = project.CustomerName,
                ContactInfo = string.Empty,
                ReferenceNumber = string.IsNullOrWhiteSpace(project.CrmNumber)
                    ? $"ZMT-{project.Id.ToString("N")[..8].ToUpperInvariant()}"
                    : project.CrmNumber,
                JobName = $"{area:0.##} m² Hafif Çelik Bina",
                MainTitle = projectTitle,
                ProjectTitle = projectTitle
            },
            OfferTitle = projectTitle,
            OfferNotes = BuildOfferNotes(result, options),
            DeliveryText = $"Avansın alındığı tarih itibarı ile {options.DeliveryDays} günlük imalat döneminden sonra sevkiyat başlayacaktır. Sahadaki iş programı karşılıklı görüşme ile belirlenecektir.",
            ScopeTotal = result.SupplierScopeValue,
            DiscountRate = result.DiscountRate,
            DiscountAmount = result.DiscountAmount,
            Subtotal = result.SubtotalAfterDiscount,
            VatRate = result.VatRate,
            VatAmount = result.VatAmount,
            GrandTotal = result.GrandTotal,
            Currency = "TRY",
            TechnicalSpecification = BuildTechnicalSpecification(project, result, options.IncludeTechnicalSpecification)
        };

        document.PaymentItems.Add(new OfferBulletItem { SortOrder = 1, Text = options.PaymentTerms.Trim() });
        MapScope(result, document);
        return document;
    }

    private static void MapScope(CalculationResult result, OfferDocument document)
    {
        var included = result.Categories
            .Where(item => item.Responsibility == ResponsibilityType.Zmt)
            .OrderBy(item => item.CategoryId)
            .ToArray();

        var rowNo = 1;
        foreach (var category in included)
        {
            var nonZeroLineCount = category.Lines.Count(item => item.EffectiveQuantity != 0m);
            document.IncludedWorkGroups.Add(new OfferSectionGroup
            {
                SortOrder = category.CategoryId,
                Title = category.CategoryName,
                Items =
                [
                    new OfferBulletItem
                    {
                        SortOrder = 1,
                        Text = $"Proje hesabında yer alan {nonZeroLineCount} malzeme ve uygulama kalemi ZMT kapsamındadır."
                    }
                ]
            });

            if (category.IncludedTotal != 0m)
            {
                document.OfferItems.Add(new OfferItem
                {
                    RowNo = rowNo++,
                    Description = category.CategoryName,
                    Quantity = 1m,
                    Unit = "grup",
                    UnitPrice = category.IncludedTotal,
                    Currency = document.Currency
                });
            }
        }

        foreach (var category in result.Categories
                     .Where(item => item.Responsibility == ResponsibilityType.Customer)
                     .OrderBy(item => item.CategoryId))
        {
            document.ExcludedWorks.Add(new OfferBulletItem
            {
                SortOrder = category.CategoryId,
                Text = $"{category.CategoryName}: Müşteri tarafından temin edilecek veya yaptırılacaktır."
            });
        }
    }

    private static string BuildOfferNotes(CalculationResult result, OfferGenerationOptions options)
    {
        var lines = new List<string>
        {
            $"Teklifimiz {options.ValidityDays} gün süre ile geçerlidir.",
            result.VatRate == 0m
                ? "Teklifimize KDV dahil DEĞİLDİR."
                : $"Teklifimize KDV %{result.VatRate * 100m:0.##} dahildir.",
            "Fiyatlar Türk Lirası (TRY) olarak düzenlenmiştir."
        };
        if (!string.IsNullOrWhiteSpace(options.AdditionalNotes))
        {
            lines.Add(options.AdditionalNotes.Trim());
        }
        return string.Join(Environment.NewLine, lines);
    }

    private static TechnicalSpecification BuildTechnicalSpecification(
        Project project,
        CalculationResult result,
        bool include)
    {
        var input = project.Building;
        var specification = new TechnicalSpecification { IncludeInPdf = include };
        specification.Sections.Add(new TechnicalSpecSection
        {
            SortOrder = 1,
            Title = "GENEL BİNA BİLGİLERİ",
            Content = Lines(
                ("Bina Alanı", $"{input.BuildingArea:0.##} m²"),
                ("Kat Adedi", input.FloorCount.ToString(CultureInfo.InvariantCulture)),
                ("Kat Yüksekliği", $"{input.FloorHeight:0.##} m"),
                ("Hesaplanan Çelik", $"{result.SteelWeight:0.##} kg"),
                ("Formül Sürümü", result.FormulaVersion))
        });
        specification.Sections.Add(new TechnicalSpecSection
        {
            SortOrder = 2,
            Title = "ÇATI SİSTEMİ",
            Content = Lines(
                ("Çatı Tipi", RoofTypeName(input.RoofType)),
                ("Çatı Sistemi", RoofSystemName(input.RoofSystem)),
                ("Kaplama Tipi", RoofCoverName(input.RoofCoverType)),
                ("Çatı Eğimi", $"%{input.RoofSlope * 100m:0.##}"),
                ("Çatı Oturum Alanı", $"{input.RoofFootprintArea:0.##} m²"))
        });
        specification.Sections.Add(new TechnicalSpecSection
        {
            SortOrder = 3,
            Title = "DUVAR VE KAPLAMA",
            Content = string.Join(Environment.NewLine, input.Surfaces.OrderBy(item => item.Surface).Select(item =>
                $"{SurfaceName(item.Surface)}\t{string.Join(" + ", item.Layers.Where(layer => layer != SurfaceLayerType.None).Select(LayerName))}"))
        });
        specification.Sections.Add(new TechnicalSpecSection
        {
            SortOrder = 4,
            Title = "KAPI VE PENCERELER",
            Content = BuildDoorWindowContent(input)
        });
        specification.Sections.Add(new TechnicalSpecSection
        {
            SortOrder = 5,
            Title = "TEKLİF KAPSAMI",
            Content = string.Join(Environment.NewLine, result.Categories.OrderBy(item => item.CategoryId).Select(item =>
                $"{item.CategoryName}\t{(item.Responsibility == ResponsibilityType.Zmt ? "ZMT'ye Ait" : "Müşteriye Ait")}"))
        });
        specification.Sections.AddRange(DetailedOfferTemplates.CreateTechnicalSpecificationSections());
        return specification;
    }

    private static string BuildDoorWindowContent(BuildingInput input)
    {
        var lines = input.Doors.Where(item => item.Quantity > 0)
            .Select(item => $"{DoorName(item.Type)}\t{item.Quantity} adet")
            .Concat(input.Windows.Where(item => item.Quantity > 0)
                .Select(item => $"{WindowName(item.Type)} ({WindowColorName(item.Color)})\t{item.Quantity} adet"))
            .ToArray();
        return lines.Length == 0 ? "Kapı / Pencere\tSeçilmedi" : string.Join(Environment.NewLine, lines);
    }

    private static string Lines(params (string Label, string Value)[] values) =>
        string.Join(Environment.NewLine, values.Select(item => $"{item.Label}\t{item.Value}"));

    private static string RoofTypeName(RoofType value) => value switch
    {
        RoofType.Hip => "Kırma",
        RoofType.Gable => "Beşik",
        RoofType.Parapet => "Parapet",
        RoofType.MonoPitch => "Tek Eğim",
        _ => value.ToString()
    };

    private static string RoofSystemName(RoofSystem value) => value switch
    {
        RoofSystem.PurlinOmega => "Aşık Omega",
        RoofSystem.Panel => "Panel Sistem",
        _ => value.ToString()
    };

    private static string RoofCoverName(RoofCoverType value) => value switch
    {
        RoofCoverType.TrapezoidalSheet => "Trapez Çatı",
        RoofCoverType.SandwichPanel => "Sandviç Panel",
        RoofCoverType.MetalTile => "Metal Kiremit Çatı",
        _ => value.ToString()
    };

    private static string SurfaceName(SurfaceType value) => value switch
    {
        SurfaceType.ExteriorWall => "Dış Duvar",
        SurfaceType.InteriorWall => "İç Duvar",
        SurfaceType.Roof => "Çatı",
        SurfaceType.Ceiling => "Tavan",
        _ => value.ToString()
    };

    private static string LayerName(SurfaceLayerType value) => value switch
    {
        SurfaceLayerType.Drywall => "Alçıpan",
        SurfaceLayerType.Bordex => "Bordex",
        SurfaceLayerType.Osb11Mm => "11 mm OSB 2",
        SurfaceLayerType.SidingFiberCement => "Yalıbaskı Siding Fibercement",
        SurfaceLayerType.WoodPatternJointedFiberCement => "Ağaç Desen Fugalı Fibercement",
        SurfaceLayerType.StonePatternJointedFiberCement => "Taş Desen Fugalı Fibercement",
        SurfaceLayerType.WoodPatternBoard => "Ahşap Desen Levha",
        SurfaceLayerType.FiberCementBoard => "Fibercement Levha",
        SurfaceLayerType.MoistureBarrier => "Nem Bariyeri",
        SurfaceLayerType.Membrane => "Membran",
        SurfaceLayerType.SlateMembrane => "Arduazlı Membran",
        _ => value.ToString()
    };

    private static string DoorName(DoorType value) => value switch
    {
        DoorType.Steel90X205 => "Çelik Kapı 90×205",
        DoorType.Pvc90X200 => "PVC Kapı 90×200",
        DoorType.DoublePvc160X200 => "Duble PVC Kapı 160×200",
        DoorType.Melamine90X201 => "Melamin Kapı 90×201",
        DoorType.AmericanWoodFrame90X201 => "Ahşap Kasalı Amerikan Kapı 90×201",
        _ => value.ToString()
    };

    private static string WindowName(WindowType value) => value switch
    {
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
        WindowType.PvcSliding180X200 => "PVC Sürgülü Pencere 180×200",
        WindowType.PvcTransom60X60 => "PVC Vasistas 60×60",
        _ => value.ToString()
    };

    private static string WindowColorName(WindowColor value) => value switch
    {
        WindowColor.White => "Beyaz",
        WindowColor.Anthracite => "Antrasit",
        WindowColor.GoldenOak => "Altınmeşe",
        _ => value.ToString()
    };
}
