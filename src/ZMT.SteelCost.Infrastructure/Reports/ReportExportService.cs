using System.Globalization;
using ClosedXML.Excel;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using ZMT.SteelCost.Application.Reports;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Infrastructure.Reports;

public sealed class ReportExportService : IReportExportService
{
    private const string ReportTitleStyle = "ReportTitle";
    private static readonly CultureInfo TurkishCulture = CultureInfo.GetCultureInfo("tr-TR");

    public Task ExportPdfAsync(
        Project project,
        CalculationResult result,
        ReportKind kind,
        string path,
        CancellationToken cancellationToken = default) =>
        Task.Run(() => ExportPdf(project, result, kind, path, cancellationToken), cancellationToken);

    public Task ExportExcelAsync(
        Project project,
        CalculationResult result,
        string path,
        CancellationToken cancellationToken = default) =>
        Task.Run(() => ExportExcel(project, result, path, cancellationToken), cancellationToken);

    private static void ExportPdf(
        Project project,
        CalculationResult result,
        ReportKind kind,
        string path,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureDirectory(path);
        var document = CreateDocument(project, result, kind, cancellationToken);
        var renderer = new PdfDocumentRenderer { Document = document };
        renderer.RenderDocument();
        renderer.PdfDocument.Save(path);
    }

    private static Document CreateDocument(
        Project project,
        CalculationResult result,
        ReportKind kind,
        CancellationToken cancellationToken)
    {
        var document = new Document
        {
            Info =
            {
                Title = ReportTitle(kind),
                Subject = $"ZMT Çelik Maliyet · {project.CrmNumber}",
                Author = "ZMT Prefabrik"
            }
        };
        ConfigureStyles(document);
        var section = document.AddSection();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.TopMargin = Unit.FromCentimeter(2.7);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(2.0);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(1.4);
        section.PageSetup.RightMargin = Unit.FromCentimeter(1.4);
        AddHeaderFooter(section, project);

        var title = section.AddParagraph(ReportTitle(kind));
        title.Style = ReportTitleStyle;
        var metadata = section.AddParagraph();
        metadata.Format.SpaceAfter = Unit.FromCentimeter(0.45);
        metadata.AddFormattedText($"Firma: {ValueOrDash(project.Company)}", TextFormat.Bold);
        metadata.AddText($"   ·   Müşteri: {ValueOrDash(project.CustomerName)}   ·   CRM: {ValueOrDash(project.CrmNumber)}");
        metadata.AddLineBreak();
        metadata.AddText($"Proje: {project.Stage.ToString()}   ·   Tarih: {project.DocumentDate:dd.MM.yyyy}   ·   Formül: {result.FormulaVersion}");

        switch (kind)
        {
            case ReportKind.InternalCost:
                AddInternalCostReport(section, result, cancellationToken);
                break;
            case ReportKind.CustomerOffer:
                AddCustomerOffer(section, result, cancellationToken);
                break;
            case ReportKind.LoadingList:
                AddLoadingList(section, result, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }

        return document;
    }

    private static void ConfigureStyles(Document document)
    {
        var normal = document.Styles[StyleNames.Normal]
            ?? throw new InvalidOperationException("MigraDoc normal stili bulunamadı.");
        normal.Font.Name = "Segoe UI";
        normal.Font.Size = Unit.FromPoint(8.5);
        normal.Font.Color = Colors.DarkSlateGray;

        var title = document.Styles.AddStyle(ReportTitleStyle, StyleNames.Normal);
        title.Font.Name = "Segoe UI";
        title.Font.Size = Unit.FromPoint(19);
        title.Font.Bold = true;
        title.Font.Color = Colors.Teal;
        title.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.25);
    }

    private static void AddHeaderFooter(Section section, Project project)
    {
        var header = section.Headers.Primary.AddParagraph();
        header.Format.Alignment = ParagraphAlignment.Left;
        header.Format.Borders.Bottom.Width = Unit.FromPoint(0.7);
        header.Format.Borders.Bottom.Color = Colors.Teal;
        header.Format.SpaceAfter = Unit.FromPoint(4);
        header.AddFormattedText("ZMT PREFABRİK", TextFormat.Bold);
        header.AddText($"  |  ZMT Çelik Maliyet  |  {ValueOrDash(project.CrmNumber)}");

        var footer = section.Footers.Primary.AddParagraph();
        footer.Format.Alignment = ParagraphAlignment.Center;
        footer.Format.Font.Size = Unit.FromPoint(8);
        footer.Format.Font.Color = Colors.Gray;
        footer.AddText("ZMT Çelik Maliyet  ·  Sayfa ");
        footer.AddPageField();
        footer.AddText(" / ");
        footer.AddNumPagesField();
    }

    private static void AddInternalCostReport(Section section, CalculationResult result, CancellationToken cancellationToken)
    {
        AddKpiLine(section, result);
        var table = CreateTable(section, [1.4, 4.0, 1.2, 1.0, 1.7, 1.9, 1.9, 1.7, 1.4],
            ["Kod", "Malzeme", "Miktar", "Birim", "Alış", "Maliyet", "Satış", "Kâr", "Kâr %"]);
        foreach (var category in result.Categories)
        {
            AddGroupRow(table, $"{category.CategoryId} · {category.CategoryName}", 9);
            foreach (var line in category.Lines.Where(item => item.EffectiveQuantity != 0m))
            {
                cancellationToken.ThrowIfCancellationRequested();
                AddRow(table,
                    line.MaterialCode,
                    line.MaterialName,
                    Number(line.EffectiveQuantity),
                    line.Unit,
                    Money(line.PurchaseUnitPriceExVat),
                    Money(line.PurchaseTotalExVat),
                    Money(line.SalesTotal),
                    Money(line.GrossProfit),
                    line.GrossMarginRate.ToString("P1", TurkishCulture));
            }
        }
    }

    private static void AddCustomerOffer(Section section, CalculationResult result, CancellationToken cancellationToken)
    {
        var intro = section.AddParagraph("Teklif kapsamı yalnızca ZMT sorumluluğundaki grupları içerir. İç alış fiyatları ve kâr bilgileri bu belgede gösterilmez.");
        intro.Format.SpaceAfter = Unit.FromCentimeter(0.3);
        var table = CreateTable(section, [2.0, 7.8, 3.3, 3.3], ["Grup", "Açıklama", "Kapsam", "Tutar"]);
        foreach (var category in result.Categories.Where(item => item.Responsibility == ResponsibilityType.Zmt))
        {
            cancellationToken.ThrowIfCancellationRequested();
            AddRow(table, category.CategoryId.ToString(CultureInfo.InvariantCulture), category.CategoryName, "ZMT'ye Ait", Money(category.IncludedTotal));
        }
        AddSummaryBlock(section, result);
    }

    private static void AddLoadingList(Section section, CalculationResult result, CancellationToken cancellationToken)
    {
        var table = CreateTable(section, [1.9, 7.8, 2.8, 1.7, 2.2], ["Kod", "Malzeme", "Ölçü", "Birim", "Miktar"]);
        foreach (var category in result.Categories)
        {
            var nonZero = category.Lines.Where(item => item.EffectiveQuantity != 0m).ToArray();
            if (nonZero.Length == 0)
            {
                continue;
            }
            AddGroupRow(table, $"{category.CategoryId} · {category.CategoryName}", 5);
            foreach (var line in nonZero)
            {
                cancellationToken.ThrowIfCancellationRequested();
                AddRow(table, line.MaterialCode, line.MaterialName, line.Specification ?? "—", line.Unit, Number(line.EffectiveQuantity));
            }
        }
    }

    private static void AddKpiLine(Section section, CalculationResult result)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0.5);
        table.Borders.Color = Colors.LightGray;
        for (var index = 0; index < 4; index++)
        {
            table.AddColumn(Unit.FromCentimeter(4.35));
        }
        var labels = table.AddRow();
        labels.Shading.Color = Colors.LightGray;
        SetCells(labels, "Tüm Gruplar", "ZMT Kapsamı", "Toplam Maliyet", "Nihai Teklif");
        var values = table.AddRow();
        values.Format.Font.Bold = true;
        SetCells(values, Money(result.FullCalculatedValue), Money(result.SupplierScopeValue), Money(result.TotalPurchaseCost), Money(result.GrandTotal));
        table.Format.SpaceAfter = Unit.FromCentimeter(0.35);
    }

    private static void AddSummaryBlock(Section section, CalculationResult result)
    {
        var table = section.AddTable();
        table.AddColumn(Unit.FromCentimeter(11.8));
        table.AddColumn(Unit.FromCentimeter(5.2));
        table.Borders.Width = Unit.FromPoint(0.5);
        table.Borders.Color = Colors.LightGray;
        SummaryRow(table, "Grup Toplamı", result.SupplierScopeValue);
        SummaryRow(table, $"İskonto %{result.DiscountRate * 100m:0.##}", -result.DiscountAmount);
        SummaryRow(table, "Ara Toplam", result.SubtotalAfterDiscount);
        SummaryRow(table, $"KDV %{result.VatRate * 100m:0.##}", result.VatAmount);
        var total = SummaryRow(table, "GENEL TOPLAM", result.GrandTotal);
        total.Shading.Color = Colors.Teal;
        total.Format.Font.Color = Colors.White;
        total.Format.Font.Bold = true;
        total.Format.Font.Size = Unit.FromPoint(11);
    }

    private static Table CreateTable(Section section, double[] widths, string[] headers)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0.35);
        table.Borders.Color = Colors.LightGray;
        table.Rows.LeftIndent = 0;
        foreach (var width in widths)
        {
            table.AddColumn(Unit.FromCentimeter(width));
        }
        var header = table.AddRow();
        header.HeadingFormat = true;
        header.Shading.Color = Colors.Teal;
        header.Format.Font.Color = Colors.White;
        header.Format.Font.Bold = true;
        SetCells(header, headers);
        return table;
    }

    private static void AddGroupRow(Table table, string text, int cellCount)
    {
        var row = table.AddRow();
        row.Shading.Color = Colors.LightGray;
        row.Format.Font.Bold = true;
        row.Cells[0].AddParagraph(text);
        row.Cells[0].MergeRight = cellCount - 1;
    }

    private static void AddRow(Table table, params string[] values)
    {
        var row = table.AddRow();
        SetCells(row, values);
        for (var index = 0; index < row.Cells.Count; index++)
        {
            var cell = row.Cells[index];
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Format.LeftIndent = Unit.FromPoint(2);
            cell.Format.RightIndent = Unit.FromPoint(2);
        }
    }

    private static void SetCells(Row row, params string[] values)
    {
        for (var index = 0; index < values.Length; index++)
        {
            row.Cells[index].AddParagraph(values[index]);
        }
    }

    private static Row SummaryRow(Table table, string label, decimal value)
    {
        var row = table.AddRow();
        row.Cells[0].AddParagraph(label);
        row.Cells[1].AddParagraph(Money(value));
        row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
        return row;
    }

    private static void ExportExcel(Project project, CalculationResult result, string path, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureDirectory(path);
        using var workbook = new XLWorkbook();
        AddSummaryWorksheet(workbook, project, result);
        AddMaterialWorksheet(workbook, result, cancellationToken);
        AddLoadingWorksheet(workbook, result, cancellationToken);
        workbook.Properties.Title = $"ZMT Çelik Maliyet · {project.CrmNumber}";
        workbook.Properties.Author = "ZMT Prefabrik";
        workbook.SaveAs(path);
    }

    private static void AddSummaryWorksheet(XLWorkbook workbook, Project project, CalculationResult result)
    {
        var sheet = workbook.Worksheets.Add("Özet");
        sheet.Cell("A1").Value = "ZMT PREFABRİK";
        sheet.Cell("A2").Value = "ÇELİK MALİYET HESAP ÖZETİ";
        sheet.Range("A1:D1").Merge();
        sheet.Range("A2:D2").Merge();
        sheet.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.FromHtml("#0B172A");
        sheet.Range("A1:D1").Style.Font.FontColor = XLColor.White;
        sheet.Range("A1:D2").Style.Font.Bold = true;
        sheet.Range("A1:D2").Style.Font.FontSize = 15;
        sheet.Cell("A4").Value = "Firma"; sheet.Cell("B4").Value = project.Company;
        sheet.Cell("A5").Value = "Müşteri"; sheet.Cell("B5").Value = project.CustomerName;
        sheet.Cell("A6").Value = "CRM No"; sheet.Cell("B6").Value = project.CrmNumber;
        sheet.Cell("C4").Value = "Tarih"; sheet.Cell("D4").Value = project.DocumentDate;
        sheet.Cell("C5").Value = "FormulaVersion"; sheet.Cell("D5").Value = result.FormulaVersion;
        sheet.Cell("C6").Value = "USD/TL"; sheet.Cell("D6").Value = project.PricingSnapshot.ExchangeRate;

        var row = 9;
        SetExcelHeader(sheet.Range(row, 1, row, 4), ["Grup", "Hesaplanan Tutar", "Sorumluluk", "Teklife Dahil"]);
        row++;
        foreach (var category in result.Categories)
        {
            sheet.Cell(row, 1).Value = category.CategoryName;
            sheet.Cell(row, 2).Value = category.CalculatedTotal;
            sheet.Cell(row, 3).Value = category.Responsibility == ResponsibilityType.Zmt ? "ZMT'ye Ait" : "Müşteriye Ait";
            sheet.Cell(row, 4).Value = category.IncludedTotal;
            row++;
        }
        row++;
        WriteSummaryExcelRow(sheet, row++, "Grup Toplamı", result.SupplierScopeValue);
        WriteSummaryExcelRow(sheet, row++, $"İskonto %{result.DiscountRate * 100m:0.##}", result.DiscountAmount);
        WriteSummaryExcelRow(sheet, row++, "Ara Toplam", result.SubtotalAfterDiscount);
        WriteSummaryExcelRow(sheet, row++, $"KDV %{result.VatRate * 100m:0.##}", result.VatAmount);
        WriteSummaryExcelRow(sheet, row, "GENEL TOPLAM", result.GrandTotal);
        sheet.Range(row, 1, row, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#0EA5A8");
        sheet.Range(row, 1, row, 2).Style.Font.FontColor = XLColor.White;
        sheet.Range(row, 1, row, 2).Style.Font.Bold = true;
        sheet.Column(1).Width = 38;
        sheet.Columns(2, 4).Width = 19;
        sheet.Range(10, 2, row, 4).Style.NumberFormat.Format = "#,##0.00 [$₺-tr-TR]";
        ConfigureWorksheet(sheet, 1, row, 4);
    }

    private static void AddMaterialWorksheet(XLWorkbook workbook, CalculationResult result, CancellationToken cancellationToken)
    {
        var sheet = workbook.Worksheets.Add("Malzemeler");
        var headers = new[] { "Grup", "Kod", "Malzeme", "Ölçü", "Birim", "Hesaplanan", "Efektif", "Mod", "Birim Alış", "Alış Toplam", "Birim Satış", "Satış Toplam", "Kâr", "Kâr %" };
        SetExcelHeader(sheet.Range(1, 1, 1, headers.Length), headers);
        var row = 2;
        foreach (var category in result.Categories)
        {
            foreach (var line in category.Lines)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var values = new object?[]
                {
                    category.CategoryName, line.MaterialCode, line.MaterialName, line.Specification, line.Unit,
                    line.CalculatedQuantity, line.EffectiveQuantity, line.QuantityMode.ToString(), line.PurchaseUnitPriceExVat,
                    line.PurchaseTotalExVat, line.SalesUnitPrice, line.SalesTotal, line.GrossProfit, line.GrossMarginRate
                };
                for (var column = 0; column < values.Length; column++)
                {
                    sheet.Cell(row, column + 1).Value = XLCellValue.FromObject(values[column]);
                }
                row++;
            }
        }
        sheet.Columns(1, headers.Length).AdjustToContents(8, 45);
        sheet.Column(3).Width = 42;
        sheet.Range(2, 6, row - 1, 7).Style.NumberFormat.Format = "0.###";
        sheet.Range(2, 9, row - 1, 13).Style.NumberFormat.Format = "#,##0.00 [$₺-tr-TR]";
        sheet.Range(2, 14, row - 1, 14).Style.NumberFormat.Format = "0.0%";
        ConfigureWorksheet(sheet, 1, row - 1, headers.Length);
    }

    private static void AddLoadingWorksheet(XLWorkbook workbook, CalculationResult result, CancellationToken cancellationToken)
    {
        var sheet = workbook.Worksheets.Add("Yükleme Listesi");
        SetExcelHeader(sheet.Range(1, 1, 1, 6), ["Grup", "Kod", "Malzeme", "Ölçü", "Birim", "Miktar"]);
        var row = 2;
        foreach (var category in result.Categories)
        {
            foreach (var line in category.Lines.Where(item => item.EffectiveQuantity != 0m))
            {
                cancellationToken.ThrowIfCancellationRequested();
                sheet.Cell(row, 1).Value = category.CategoryName;
                sheet.Cell(row, 2).Value = line.MaterialCode;
                sheet.Cell(row, 3).Value = line.MaterialName;
                sheet.Cell(row, 4).Value = line.Specification ?? string.Empty;
                sheet.Cell(row, 5).Value = line.Unit;
                sheet.Cell(row, 6).Value = line.EffectiveQuantity;
                row++;
            }
        }
        sheet.Columns(1, 6).AdjustToContents(8, 45);
        sheet.Column(3).Width = 46;
        sheet.Column(6).Style.NumberFormat.Format = "0.###";
        ConfigureWorksheet(sheet, 1, row - 1, 6);
    }

    private static void SetExcelHeader(IXLRange range, IReadOnlyList<string> values)
    {
        for (var index = 0; index < values.Count; index++)
        {
            range.FirstCell().CellRight(index).Value = values[index];
        }
        range.Style.Fill.BackgroundColor = XLColor.FromHtml("#0B172A");
        range.Style.Font.FontColor = XLColor.White;
        range.Style.Font.Bold = true;
        range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
    }

    private static void WriteSummaryExcelRow(IXLWorksheet sheet, int row, string label, decimal value)
    {
        sheet.Cell(row, 1).Value = label;
        sheet.Cell(row, 2).Value = value;
        sheet.Cell(row, 1).Style.Font.Bold = true;
    }

    private static void ConfigureWorksheet(IXLWorksheet sheet, int firstRow, int lastRow, int lastColumn)
    {
        sheet.SheetView.FreezeRows(firstRow);
        sheet.Range(firstRow, 1, lastRow, lastColumn).SetAutoFilter();
        sheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        sheet.PageSetup.PaperSize = XLPaperSize.A4Paper;
        sheet.PageSetup.FitToPages(1, 0);
        sheet.PageSetup.Header.Center.AddText("ZMT PREFABRİK");
        sheet.PageSetup.Footer.Center.AddText("Sayfa &P / &N");
    }

    private static string ReportTitle(ReportKind kind) => kind switch
    {
        ReportKind.InternalCost => "İÇ MALİYET RAPORU",
        ReportKind.CustomerOffer => "MÜŞTERİ TEKLİFİ",
        ReportKind.LoadingList => "HAFİF ÇELİK BİNA YÜKLEME LİSTESİ",
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };

    private static string Money(decimal value) => value.ToString("N2", TurkishCulture) + " ₺";
    private static string Number(decimal value) => value.ToString("0.###", TurkishCulture);
    private static string ValueOrDash(string? value) => string.IsNullOrWhiteSpace(value) ? "—" : value;
    private static void EnsureDirectory(string path)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(path));
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
