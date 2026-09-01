using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.Application.Reports;

public enum ReportKind
{
    InternalCost,
    CustomerOffer,
    LoadingList
}

public interface IReportExportService
{
    Task ExportPdfAsync(Project project, CalculationResult result, ReportKind kind, string path, CancellationToken cancellationToken = default);
    Task ExportExcelAsync(Project project, CalculationResult result, string path, CancellationToken cancellationToken = default);
}
