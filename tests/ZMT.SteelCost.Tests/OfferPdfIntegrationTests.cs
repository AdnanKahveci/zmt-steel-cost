using System.Text;
using PdfSharp.Pdf.IO;
using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Application.Offers;
using ZMT.SteelCost.Domain;
using ZMT.SteelCost.Infrastructure.Reports;

namespace ZMT.SteelCost.Tests;

public sealed class OfferPdfIntegrationTests
{
    private readonly Project _project;
    private readonly CalculationResult _result;
    private readonly SteelCostOfferDocumentMapper _mapper = new();

    public OfferPdfIntegrationTests()
    {
        _project = new Project
        {
            Company = "ZMT Test Firma",
            CustomerName = "Türkçe Karakter Müşteri",
            CrmNumber = "CRM-TEKLIF-001",
            OfferPreparedBy = "Teklif Hazırlayan",
            Building = BuildingInput.CreateLegacySample(),
            PricingSnapshot = new PricingParameters()
        };
        _result = new CalculationEngine(new RoofCalculationService()).Calculate(_project);
    }

    [Fact]
    public void Mapper_connects_project_scope_totals_and_technical_data_to_offer_document()
    {
        var document = Map();

        Assert.Equal(_project.CrmNumber, document.Info.ReferenceNumber);
        Assert.Equal(_result.SupplierScopeValue, document.ScopeTotal);
        Assert.Equal(_result.GrandTotal, document.GrandTotal);
        Assert.Equal(_result.SupplierScopeValue, document.OfferItems.Sum(item => item.Total));
        Assert.Equal(_result.Categories.Count(item => item.Responsibility == ResponsibilityType.Zmt), document.IncludedWorkGroups.Count);
        Assert.Equal(_result.Categories.Count(item => item.Responsibility == ResponsibilityType.Customer), document.ExcludedWorks.Count);
        Assert.True(document.TechnicalSpecification.IncludeInPdf);
        Assert.True(document.TechnicalSpecification.Sections.Count >= 15);
        Assert.Empty(new OfferDocumentValidationService().Validate(document));
    }

    [Fact]
    public void English_localization_translates_template_but_preserves_customer_data()
    {
        var turkish = Map();

        var english = OfferDocumentLocalizer.Localize(turkish, OfferLanguage.English);

        Assert.Equal(turkish.Info.CompanyName, english.Info.CompanyName);
        Assert.Equal(turkish.Info.AuthorizedPerson, english.Info.AuthorizedPerson);
        Assert.Contains("QUOTATION", english.Info.MainTitle);
        Assert.Contains("Shipment will commence", english.DeliveryText);
        Assert.Contains("Our quotation is valid", english.OfferNotes);
        Assert.Contains("Light", english.IncludedWorkGroups[0].Title, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Shipment will commence", turkish.DeliveryText);
    }

    [Theory]
    [InlineData(OfferLanguage.Turkish)]
    [InlineData(OfferLanguage.English)]
    public async Task Separate_offer_pdf_contains_all_document_sections(OfferLanguage language)
    {
        var source = Map();
        var document = OfferDocumentLocalizer.Localize(source, language);
        var path = Path.Combine(Path.GetTempPath(), $"zmt-offer-{Guid.NewGuid():N}.pdf");
        try
        {
            await new OfferPdfExportService().ExportAsync(document, new PdfExportOptions
            {
                OutputPath = path,
                Language = language,
                DocumentTitle = document.Info.MainTitle,
                IncludeTechnicalSpecification = true,
                OpenAfterExport = false
            });

            var bytes = await File.ReadAllBytesAsync(path);
            Assert.True(bytes.Length > 5_000);
            Assert.Equal("%PDF-", Encoding.ASCII.GetString(bytes, 0, 5));
            using var pdf = PdfReader.Open(path, PdfDocumentOpenMode.Import);
            Assert.True(pdf.PageCount >= 4);
            Assert.Equal(document.Info.MainTitle, pdf.Info.Title);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task Detailed_offer_pdf_renders_logo_and_both_image_sections()
    {
        var document = Map();
        var logoPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..",
            "src", "ZMT.SteelCost.App", "Assets", "logo.png"));
        Assert.True(File.Exists(logoPath), logoPath);
        document.CompanySettings.HeaderLogoPath = logoPath;
        document.Images.Add(new OfferImage
        {
            FilePath = logoPath,
            Title = "Teklif Görseli",
            Description = "Müşteriye sunulan proje görseli",
            ImageSection = ImageSections.Offer,
            PageNumber = 1,
            SortOrder = 1,
            HasBorder = true
        });
        document.Images.Add(new OfferImage
        {
            FilePath = logoPath,
            Title = "Teknik Görsel",
            Description = "Teknik şartname eki",
            ImageSection = ImageSections.TechnicalSpec,
            PageNumber = 1,
            SortOrder = 1,
            HasBorder = true
        });

        var path = Path.Combine(Path.GetTempPath(), $"zmt-detailed-offer-{Guid.NewGuid():N}.pdf");
        try
        {
            await new OfferPdfExportService().ExportAsync(document, new PdfExportOptions
            {
                OutputPath = path,
                Language = OfferLanguage.Turkish,
                DocumentTitle = document.Info.MainTitle,
                IncludeImages = true,
                IncludeTechnicalSpecification = true,
                OpenAfterExport = false
            });

            using var pdf = PdfReader.Open(path, PdfDocumentOpenMode.Import);
            Assert.True(pdf.PageCount >= 6);
            Assert.True(new FileInfo(path).Length > 20_000);
        }
        finally
        {
            File.Delete(path);
        }
    }

    private OfferDocument Map() => _mapper.Map(_project, _result, new OfferGenerationOptions
    {
        ValidityDays = 7,
        DeliveryDays = 15,
        PaymentTerms = "Karşılıklı görüşme ile belirlenecektir.",
        IncludeTechnicalSpecification = true
    });
}
