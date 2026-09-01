using System.Diagnostics;
using System.Globalization;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using ZMT.SteelCost.Application.Offers;

namespace ZMT.SteelCost.Infrastructure.Reports;

public sealed class OfferPdfExportService : IOfferPdfExportService
{
    private const string OfferTitleStyle = "OfferTitle";
    private const string SectionTitleStyle = "OfferSectionTitle";

    public Task ExportAsync(
        OfferDocument document,
        PdfExportOptions options,
        CancellationToken cancellationToken = default) =>
        Task.Run(() => Export(document, options, cancellationToken), cancellationToken);

    private static void Export(
        OfferDocument offer,
        PdfExportOptions options,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(offer);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.OutputPath))
        {
            throw new InvalidOperationException("PDF çıktı yolu seçilmedi.");
        }

        cancellationToken.ThrowIfCancellationRequested();
        EnsureDirectory(options.OutputPath);
        var localization = PdfLocalization.For(options.Language);
        var document = CreateDocument(offer, options, localization, cancellationToken);
        var renderer = new PdfDocumentRenderer { Document = document };
        renderer.RenderDocument();
        AddWatermarks(renderer.PdfDocument, offer.CompanySettings);
        renderer.PdfDocument.Save(options.OutputPath);

        if (options.OpenAfterExport)
        {
            Process.Start(new ProcessStartInfo(options.OutputPath) { UseShellExecute = true });
        }
    }

    private static Document CreateDocument(
        OfferDocument offer,
        PdfExportOptions options,
        PdfLocalization localization,
        CancellationToken cancellationToken)
    {
        var document = new Document
        {
            Info =
            {
                Title = string.IsNullOrWhiteSpace(options.DocumentTitle) ? offer.OfferTitle : options.DocumentTitle,
                Subject = $"ZMT Teklif · {offer.Info.ReferenceNumber}",
                Author = offer.CompanySettings.CompanyName
            }
        };
        ConfigureStyles(document);

        if (options.IncludeImages)
        {
            AddImagePages(document, offer, ImageSections.Offer, localization, cancellationToken);
        }
        AddIncludedWorksPage(document, offer, localization);
        AddExcludedWorksPage(document, offer, localization);
        AddQuotationPage(document, offer, localization, cancellationToken);

        if (options.IncludeImages)
        {
            AddImagePages(document, offer, ImageSections.TechnicalSpec, localization, cancellationToken);
        }
        if (options.IncludeTechnicalSpecification && offer.TechnicalSpecification.IncludeInPdf)
        {
            AddTechnicalSpecification(document, offer, localization, cancellationToken);
        }
        return document;
    }

    private static void ConfigureStyles(Document document)
    {
        var normal = document.Styles[StyleNames.Normal]
            ?? throw new InvalidOperationException("MigraDoc normal stili bulunamadı.");
        normal.Font.Name = "Segoe UI";
        normal.Font.Size = Unit.FromPoint(9);
        normal.Font.Color = Colors.DarkSlateGray;

        var title = document.Styles.AddStyle(OfferTitleStyle, StyleNames.Normal);
        title.Font.Name = "Segoe UI";
        title.Font.Size = Unit.FromPoint(18);
        title.Font.Bold = true;
        title.Font.Color = Colors.Firebrick;
        title.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.35);

        var sectionTitle = document.Styles.AddStyle(SectionTitleStyle, StyleNames.Normal);
        sectionTitle.Font.Name = "Segoe UI";
        sectionTitle.Font.Size = Unit.FromPoint(13);
        sectionTitle.Font.Bold = true;
        sectionTitle.Font.Color = Colors.Firebrick;
        sectionTitle.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.25);
        sectionTitle.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.15);
    }

    private static Section AddPage(Document document, OfferDocument offer, PdfLocalization localization)
    {
        var section = document.AddSection();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.TopMargin = Unit.FromCentimeter(2.6);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(1.8);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(1.6);
        section.PageSetup.RightMargin = Unit.FromCentimeter(1.6);
        AddHeaderFooter(section, offer, localization);
        return section;
    }

    private static void AddHeaderFooter(Section section, OfferDocument offer, PdfLocalization localization)
    {
        var headerTable = section.Headers.Primary.AddTable();
        headerTable.AddColumn(Unit.FromCentimeter(5.5));
        headerTable.AddColumn(Unit.FromCentimeter(11.2));
        var headerRow = headerTable.AddRow();
        headerRow.BottomPadding = Unit.FromPoint(4);
        headerRow.Borders.Bottom.Width = Unit.FromPoint(0.8);
        headerRow.Borders.Bottom.Color = Colors.Firebrick;
        if (!string.IsNullOrWhiteSpace(offer.CompanySettings.HeaderLogoPath)
            && File.Exists(offer.CompanySettings.HeaderLogoPath))
        {
            var logo = headerRow.Cells[0].AddImage(offer.CompanySettings.HeaderLogoPath);
            logo.LockAspectRatio = true;
            logo.Height = Unit.FromCentimeter(1.25);
        }
        else
        {
            var brand = headerRow.Cells[0].AddParagraph("ZMT");
            brand.Format.Font.Size = Unit.FromPoint(22);
            brand.Format.Font.Bold = true;
            brand.Format.Font.Color = Colors.Firebrick;
        }

        var company = headerRow.Cells[1].AddParagraph();
        company.Format.Alignment = ParagraphAlignment.Right;
        company.Format.Font.Size = Unit.FromPoint(7.5);
        company.AddFormattedText(offer.CompanySettings.CompanyName, TextFormat.Bold);
        company.AddLineBreak();
        company.AddText(offer.CompanySettings.Address);
        company.AddLineBreak();
        company.AddText($"{offer.CompanySettings.Phone}  ·  {offer.CompanySettings.Email}");
        company.AddLineBreak();
        company.AddText($"{offer.Info.ReferenceNumber}  ·  {offer.Info.OfferDate:dd.MM.yyyy}");

        var footer = section.Footers.Primary.AddParagraph();
        footer.Format.Alignment = ParagraphAlignment.Center;
        footer.Format.Font.Size = Unit.FromPoint(8);
        footer.Format.Font.Color = Colors.Gray;
        footer.AddText($"{offer.CompanySettings.Website}  ·  {localization.PageLabel} ");
        footer.AddPageField();
        footer.AddText(" / ");
        footer.AddNumPagesField();
    }

    private static void AddPageHeading(Section section, string code, string title)
    {
        var heading = section.AddParagraph();
        heading.Style = OfferTitleStyle;
        heading.AddFormattedText($"{code}  ", TextFormat.Bold);
        heading.AddText(title);
    }

    private static void AddIncludedWorksPage(Document document, OfferDocument offer, PdfLocalization localization)
    {
        var section = AddPage(document, offer, localization);
        var mainTitle = section.AddParagraph(offer.Info.MainTitle);
        mainTitle.Style = OfferTitleStyle;
        AddOfferInfo(section, offer, localization);
        AddPageHeading(section, "A", localization.IncludedWorksTitle);

        foreach (var group in offer.IncludedWorkGroups.Where(item => item.IsVisible).OrderBy(item => item.SortOrder))
        {
            var title = section.AddParagraph(group.Title);
            title.Style = SectionTitleStyle;
            foreach (var item in group.Items.Where(value => value.IsIncludedInPdf).OrderBy(value => value.SortOrder))
            {
                AddBullet(section, item.Text);
            }
        }
    }

    private static void AddExcludedWorksPage(Document document, OfferDocument offer, PdfLocalization localization)
    {
        var section = AddPage(document, offer, localization);
        AddPageHeading(section, "B", localization.ExcludedWorksTitle);
        if (offer.ExcludedWorks.Count == 0)
        {
            AddBullet(section, optionsText(localization));
            return;
        }
        foreach (var item in offer.ExcludedWorks.Where(value => value.IsIncludedInPdf).OrderBy(value => value.SortOrder))
        {
            AddBullet(section, item.Text);
        }

        static string optionsText(PdfLocalization loc) =>
            loc.ExcludedWorksTitle.StartsWith("WORKS", StringComparison.Ordinal) ? "No excluded work item." : "Hariç iş kalemi bulunmamaktadır.";
    }

    private static void AddQuotationPage(
        Document document,
        OfferDocument offer,
        PdfLocalization localization,
        CancellationToken cancellationToken)
    {
        var section = AddPage(document, offer, localization);
        AddPageHeading(section, "C", localization.QuotationTitle);
        var offerTitle = section.AddParagraph(offer.OfferTitle);
        offerTitle.Format.Font.Bold = true;
        offerTitle.Format.SpaceAfter = Unit.FromCentimeter(0.25);

        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0.4);
        table.Borders.Color = Colors.LightGray;
        foreach (var width in new[] { 0.8, 7.0, 1.5, 1.4, 3.0, 3.2 })
        {
            table.AddColumn(Unit.FromCentimeter(width));
        }
        var header = table.AddRow();
        header.HeadingFormat = true;
        header.Shading.Color = Colors.Firebrick;
        header.Format.Font.Color = Colors.White;
        header.Format.Font.Bold = true;
        SetCells(header, "#", localization.DescriptionLabel, localization.QuantityLabel, localization.UnitLabel,
            localization.UnitPriceLabel, localization.TotalLabel);

        foreach (var item in offer.OfferItems.OrderBy(value => value.RowNo))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var row = table.AddRow();
            SetCells(row,
                item.RowNo.ToString(CultureInfo.InvariantCulture),
                item.Description,
                item.Quantity.ToString("0.###", CultureInfo.GetCultureInfo("tr-TR")),
                item.Unit,
                Money(item.UnitPrice, item.Currency, localization),
                Money(item.Total, item.Currency, localization));
            row.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
        }
        table.Format.SpaceAfter = Unit.FromCentimeter(0.3);
        AddCommercialSummary(section, offer, localization);

        var notesTitle = section.AddParagraph(localization.NotesLabel);
        notesTitle.Style = SectionTitleStyle;
        foreach (var line in offer.OfferNotes.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries))
        {
            AddBullet(section, line);
        }

        AddPageHeading(section, "D", localization.PaymentTitle);
        foreach (var item in offer.PaymentItems.Where(value => value.IsIncludedInPdf).OrderBy(value => value.SortOrder))
        {
            AddBullet(section, item.Text);
        }

        AddPageHeading(section, "E", localization.DeliveryTitle);
        var delivery = section.AddParagraph(offer.DeliveryText);
        delivery.Format.SpaceAfter = Unit.FromCentimeter(0.2);
    }

    private static void AddOfferInfo(Section section, OfferDocument offer, PdfLocalization localization)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0.35);
        table.Borders.Color = Colors.LightGray;
        foreach (var width in new[] { 2.6, 5.7, 2.6, 5.7 })
        {
            table.AddColumn(Unit.FromCentimeter(width));
        }
        AddInfoRow(table, localization.DateLabel, offer.Info.OfferDate.ToString("dd.MM.yyyy"),
            localization.ReferenceLabel, offer.Info.ReferenceNumber);
        AddInfoRow(table, localization.CompanyLabel, ValueOrDash(offer.Info.CompanyName),
            localization.AuthorizedPersonLabel, ValueOrDash(offer.Info.AuthorizedPerson));
        AddInfoRow(table, localization.JobLabel, offer.Info.JobName,
            localization.PreparedByLabel, ValueOrDash(offer.Info.PreparedBy));
        if (!string.IsNullOrWhiteSpace(offer.Info.ContactInfo))
        {
            AddInfoRow(table, localization.ContactLabel, offer.Info.ContactInfo, string.Empty, string.Empty);
        }
        table.Format.SpaceAfter = Unit.FromCentimeter(0.45);
    }

    private static void AddInfoRow(Table table, string label1, string value1, string label2, string value2)
    {
        var row = table.AddRow();
        row.Cells[0].Shading.Color = Colors.LightGray;
        row.Cells[2].Shading.Color = Colors.LightGray;
        row.Cells[0].Format.Font.Bold = true;
        row.Cells[2].Format.Font.Bold = true;
        SetCells(row, label1, value1, label2, value2);
    }

    private static void AddCommercialSummary(Section section, OfferDocument offer, PdfLocalization localization)
    {
        var table = section.AddTable();
        table.AddColumn(Unit.FromCentimeter(11.8));
        table.AddColumn(Unit.FromCentimeter(5.1));
        table.Borders.Width = Unit.FromPoint(0.4);
        table.Borders.Color = Colors.LightGray;
        SummaryRow(table, localization.ScopeTotalLabel, offer.ScopeTotal, offer.Currency, localization);
        SummaryRow(table, $"{localization.DiscountLabel} %{offer.DiscountRate * 100m:0.##}", -offer.DiscountAmount, offer.Currency, localization);
        SummaryRow(table, localization.SubtotalLabel, offer.Subtotal, offer.Currency, localization);
        SummaryRow(table, $"{localization.VatLabel} %{offer.VatRate * 100m:0.##}", offer.VatAmount, offer.Currency, localization);
        var total = SummaryRow(table, localization.GrandTotalLabel, offer.GrandTotal, offer.Currency, localization);
        total.Shading.Color = Colors.Firebrick;
        total.Format.Font.Color = Colors.White;
        total.Format.Font.Bold = true;
        total.Format.Font.Size = Unit.FromPoint(11);
        table.Format.SpaceAfter = Unit.FromCentimeter(0.25);
    }

    private static Row SummaryRow(Table table, string label, decimal value, string currency, PdfLocalization localization)
    {
        var row = table.AddRow();
        row.Cells[0].AddParagraph(label);
        row.Cells[1].AddParagraph(Money(value, currency, localization));
        row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
        return row;
    }

    private static void AddTechnicalSpecification(
        Document document,
        OfferDocument offer,
        PdfLocalization localization,
        CancellationToken cancellationToken)
    {
        var section = AddPage(document, offer, localization);
        AddPageHeading(section, "", localization.TechnicalSpecificationTitle);
        var title = section.AddParagraph(offer.TechnicalSpecification.Title);
        title.Format.Font.Bold = true;
        title.Format.Font.Size = Unit.FromPoint(12);
        title.Format.SpaceAfter = Unit.FromCentimeter(0.2);

        foreach (var specificationSection in offer.TechnicalSpecification.Sections
                     .Where(item => item.IncludeInPdf)
                     .OrderBy(item => item.SortOrder))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var heading = section.AddParagraph(specificationSection.Title);
            heading.Style = SectionTitleStyle;
            foreach (var line in specificationSection.Content.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split('\t', 2);
                var paragraph = section.AddParagraph();
                paragraph.Format.SpaceAfter = Unit.FromPoint(3);
                if (parts.Length == 2)
                {
                    paragraph.AddFormattedText(parts[0] + ": ", TextFormat.Bold);
                    paragraph.AddText(parts[1]);
                }
                else
                {
                    paragraph.AddText(line);
                }
            }
        }
    }

    private static void AddImagePages(
        Document document,
        OfferDocument offer,
        string imageSection,
        PdfLocalization localization,
        CancellationToken cancellationToken)
    {
        var images = offer.Images
            .Where(item => item.IncludeInPdf && item.ImageSection == imageSection && File.Exists(item.FilePath))
            .OrderBy(item => item.PageNumber)
            .ThenBy(item => item.SortOrder)
            .ToArray();

        foreach (var pageGroup in images.GroupBy(item => item.PageNumber <= 0 ? 1 : item.PageNumber).OrderBy(item => item.Key))
        {
            var ordered = pageGroup.ToArray();
            for (var offset = 0; offset < ordered.Length; offset += 2)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var section = AddPage(document, offer, localization);
                var pageTitle = section.AddParagraph(
                    imageSection == ImageSections.TechnicalSpec
                        ? localization.TechnicalSpecificationImagesTitle
                        : offer.Info.ProjectTitle);
                pageTitle.Style = OfferTitleStyle;
                var pageImages = ordered.Skip(offset).Take(2).ToArray();

                foreach (var offerImage in pageImages)
                {
                    var frame = section.AddTable();
                    frame.AddColumn(Unit.FromCentimeter(16.7));
                    var row = frame.AddRow();
                    row.Cells[0].Borders.Width = offerImage.HasBorder ? Unit.FromPoint(0.8) : Unit.FromPoint(0);
                    row.Cells[0].Borders.Color = Colors.Gray;
                    row.TopPadding = Unit.FromPoint(5);
                    row.BottomPadding = Unit.FromPoint(5);

                    if (!string.IsNullOrWhiteSpace(offerImage.Title))
                    {
                        var title = row.Cells[0].AddParagraph(offerImage.Title);
                        title.Format.Font.Bold = true;
                        title.Format.Font.Size = Unit.FromPoint(10);
                        title.Format.Font.Color = Colors.Firebrick;
                        title.Format.SpaceAfter = Unit.FromPoint(4);
                    }
                    var imageParagraph = row.Cells[0].AddParagraph();
                    imageParagraph.Format.Alignment = ParagraphAlignment.Center;
                    var image = imageParagraph.AddImage(offerImage.FilePath);
                    image.LockAspectRatio = offerImage.FitWithoutCrop;
                    image.Height = Unit.FromCentimeter(pageImages.Length == 2 ? 8.1 : 16.5);
                    image.Width = Unit.FromCentimeter(15.6);

                    if (!string.IsNullOrWhiteSpace(offerImage.Description))
                    {
                        var description = row.Cells[0].AddParagraph(offerImage.Description);
                        description.Format.Font.Size = Unit.FromPoint(8);
                        description.Format.Font.Color = Colors.DimGray;
                        description.Format.SpaceBefore = Unit.FromPoint(4);
                    }
                    frame.Format.SpaceAfter = Unit.FromCentimeter(0.35);
                }
            }
        }
    }

    private static void AddBullet(Section section, string text)
    {
        var paragraph = section.AddParagraph();
        paragraph.Format.LeftIndent = Unit.FromCentimeter(0.25);
        paragraph.Format.FirstLineIndent = Unit.FromCentimeter(-0.2);
        paragraph.Format.SpaceAfter = Unit.FromPoint(4);
        paragraph.AddText("•  " + text);
    }

    private static void SetCells(Row row, params string[] values)
    {
        for (var index = 0; index < values.Length; index++)
        {
            row.Cells[index].AddParagraph(values[index]);
            row.Cells[index].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[index].Format.LeftIndent = Unit.FromPoint(2);
            row.Cells[index].Format.RightIndent = Unit.FromPoint(2);
        }
    }

    private static string Money(decimal value, string currency, PdfLocalization localization)
    {
        var english = localization.GrandTotalLabel == "GRAND TOTAL";
        var culture = CultureInfo.GetCultureInfo(english ? "en-US" : "tr-TR");
        var symbol = currency.ToUpperInvariant() switch
        {
            "USD" => "$",
            "EUR" => "€",
            _ => english ? "TRY" : "₺"
        };
        return english || currency is "USD" or "EUR"
            ? $"{symbol} {value.ToString("N2", culture)}"
            : $"{value.ToString("N2", culture)} {symbol}";
    }

    private static string ValueOrDash(string? value) => string.IsNullOrWhiteSpace(value) ? "—" : value;

    private static void AddWatermarks(PdfSharp.Pdf.PdfDocument pdf, CompanySettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.WatermarkText) || settings.WatermarkOpacity <= 0)
        {
            return;
        }

        var alpha = (int)Math.Round(Math.Clamp(settings.WatermarkOpacity, 0d, 1d) * 255d);
        var brush = new XSolidBrush(XColor.FromArgb(alpha, 165, 165, 165));
        var font = new XFont("Arial", 58, XFontStyleEx.Bold);
        foreach (var page in pdf.Pages)
        {
            using var graphics = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
            var state = graphics.Save();
            graphics.TranslateTransform(page.Width.Point / 2d, page.Height.Point / 2d);
            graphics.RotateTransform(settings.WatermarkAngle);
            graphics.DrawString(
                settings.WatermarkText,
                font,
                brush,
                new XRect(-250, -55, 500, 110),
                XStringFormats.Center);
            graphics.Restore(state);
        }
    }

    private static void EnsureDirectory(string path)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(path));
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
