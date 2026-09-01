using System.IO.Compression;
using System.Text;
using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Application.Reports;
using ZMT.SteelCost.Domain;
using ZMT.SteelCost.Infrastructure.Reports;

namespace ZMT.SteelCost.Tests;

public sealed class ReportExportTests
{
    private readonly ReportExportService _exporter = new();
    private readonly Project _project;
    private readonly CalculationResult _result;

    public ReportExportTests()
    {
        _project = new Project
        {
            Company = "ZMT Test Firma",
            CustomerName = "Türkçe Karakter Müşteri",
            CrmNumber = "CRM-TEST-001",
            Building = BuildingInput.CreateLegacySample(),
            PricingSnapshot = new PricingParameters()
        };
        _result = new CalculationEngine(new RoofCalculationService()).Calculate(_project);
    }

    [Theory]
    [InlineData(ReportKind.InternalCost)]
    [InlineData(ReportKind.CustomerOffer)]
    [InlineData(ReportKind.LoadingList)]
    public async Task Pdf_report_is_a_valid_non_empty_pdf(ReportKind kind)
    {
        var path = TemporaryPath("pdf");
        try
        {
            await _exporter.ExportPdfAsync(_project, _result, kind, path);

            var bytes = await File.ReadAllBytesAsync(path);
            Assert.True(bytes.Length > 1_000);
            Assert.Equal("%PDF-", Encoding.ASCII.GetString(bytes, 0, 5));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task Excel_report_contains_all_required_worksheets()
    {
        var path = TemporaryPath("xlsx");
        try
        {
            await _exporter.ExportExcelAsync(_project, _result, path);

            Assert.True(new FileInfo(path).Length > 1_000);
            using var archive = ZipFile.OpenRead(path);
            var workbookEntry = Assert.Single(archive.Entries, entry => entry.FullName == "xl/workbook.xml");
            using var reader = new StreamReader(workbookEntry.Open(), Encoding.UTF8);
            var workbookXml = await reader.ReadToEndAsync();
            Assert.Contains("Özet", workbookXml);
            Assert.Contains("Malzemeler", workbookXml);
            Assert.Contains("Yükleme Listesi", workbookXml);
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static string TemporaryPath(string extension) =>
        Path.Combine(Path.GetTempPath(), $"zmt-steelcost-{Guid.NewGuid():N}.{extension}");
}
