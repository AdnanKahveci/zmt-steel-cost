using Microsoft.Win32;

namespace ZMT.SteelCost.App.Services;

public interface IFileDialogService
{
    string? SavePdf(string suggestedName);
    string? SaveExcel(string suggestedName);
    IReadOnlyList<string> SelectImages();
}

public sealed class FileDialogService : IFileDialogService
{
    public string? SavePdf(string suggestedName) => Save(suggestedName, "PDF Belgesi (*.pdf)|*.pdf", ".pdf");
    public string? SaveExcel(string suggestedName) => Save(suggestedName, "Excel Çalışma Kitabı (*.xlsx)|*.xlsx", ".xlsx");
    public IReadOnlyList<string> SelectImages()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Teklif görsellerini seçin",
            Filter = "Görsel Dosyaları|*.jpg;*.jpeg;*.png;*.bmp;*.webp|Tüm Dosyalar|*.*",
            Multiselect = true,
            CheckFileExists = true
        };
        return dialog.ShowDialog() == true ? dialog.FileNames : [];
    }

    private static string? Save(string fileName, string filter, string extension)
    {
        var dialog = new SaveFileDialog
        {
            FileName = fileName,
            Filter = filter,
            DefaultExt = extension,
            AddExtension = true,
            OverwritePrompt = true
        };
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
}
