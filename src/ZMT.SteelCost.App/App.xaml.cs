using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using ZMT.SteelCost.App.Services;
using ZMT.SteelCost.App.ViewModels;
using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Application.Logging;
using ZMT.SteelCost.Application.Offers;
using ZMT.SteelCost.Application.Pricing;
using ZMT.SteelCost.Application.Projects;
using ZMT.SteelCost.Application.Reports;
using ZMT.SteelCost.Infrastructure.Logging;
using ZMT.SteelCost.Infrastructure.Persistence;
using ZMT.SteelCost.Infrastructure.Reports;

namespace ZMT.SteelCost.App;

public partial class App : System.Windows.Application
{
    private ServiceProvider? _services;
    private int _fatalErrorShown;
    private bool _isPageSmokeTest;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _isPageSmokeTest = e.Args.Contains("--smoke-test-pages", StringComparer.OrdinalIgnoreCase);
        ConfigureCulture();
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

        _services = ConfigureServices();
        try
        {
            await _services.GetRequiredService<SqliteDatabase>().InitializeAsync();
            var viewModel = _services.GetRequiredService<MainViewModel>();
            await viewModel.InitializeAsync();

            var window = _services.GetRequiredService<MainWindow>();
            MainWindow = window;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            window.Show();
            if (_isPageSmokeTest)
            {
                await RunPageSmokeTestAsync(viewModel, window);
                if (Volatile.Read(ref _fatalErrorShown) == 0)
                {
                    _services.GetRequiredService<IAppLogger>().Information(
                        "PageSmokeTestCompleted", "Tüm WPF sayfaları başarıyla oluşturuldu.");
                    Shutdown(0);
                }
            }
        }
        catch (Exception exception)
        {
            LogFatal("ApplicationStartupFailed", exception);
            if (!_isPageSmokeTest)
            {
                ShowFriendlyError("Uygulama başlatılamadı. Ayrıntılar günlük dosyasına kaydedildi.");
            }
            Shutdown(1);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
        DispatcherUnhandledException -= OnDispatcherUnhandledException;
        _services?.Dispose();
        base.OnExit(e);
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IAppLogger, JsonFileLogger>();
        services.AddSingleton<SqliteDatabase>();
        services.AddSingleton<IProjectRepository, SqliteProjectRepository>();
        services.AddSingleton<IRoofCalculationService, RoofCalculationService>();
        services.AddSingleton<ICalculationEngine, CalculationEngine>();
        services.AddSingleton<IProjectService, ProjectService>();
        services.AddSingleton<IPriceListService, SqlitePriceListService>();
        services.AddSingleton<IReportExportService, ReportExportService>();
        services.AddSingleton<IOfferDocumentMapper, SteelCostOfferDocumentMapper>();
        services.AddSingleton<OfferDocumentValidationService>();
        services.AddSingleton<IOfferPdfExportService, OfferPdfExportService>();
        services.AddSingleton<IFileDialogService, FileDialogService>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();
        return services.BuildServiceProvider(validateScopes: true);
    }

    private static void ConfigureCulture()
    {
        var culture = CultureInfo.GetCultureInfo("tr-TR");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        if (Interlocked.Exchange(ref _fatalErrorShown, 1) != 0)
        {
            return;
        }
        LogFatal("DispatcherUnhandledException", e.Exception);
        if (!_isPageSmokeTest)
        {
            ShowFriendlyError("Beklenmeyen bir hata oluştu. İşlem durduruldu ve ayrıntılar günlük dosyasına kaydedildi.");
        }
        Shutdown(1);
    }

    private void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            LogFatal("UnhandledException", exception);
        }
    }

    private void LogFatal(string eventName, Exception exception)
    {
        try
        {
            _services?.GetService<IAppLogger>()?.Error(eventName, exception);
        }
        catch
        {
            // Hata işleyicisi uygulamanın kapanmasını engellememelidir.
        }
    }

    private static void ShowFriendlyError(string message) =>
        MessageBox.Show(message, "ZMT Çelik Maliyet", MessageBoxButton.OK, MessageBoxImage.Error);

    private async Task RunPageSmokeTestAsync(MainViewModel viewModel, MainWindow window)
    {
        string[] pages =
        [
            "Dashboard", "Projects", "ProjectInfo", "Building", "Cladding", "DoorWindow",
            "Fixtures", "Results", "Materials", "Reports", "OfferEditor", "Settings"
        ];
        foreach (var page in pages)
        {
            viewModel.NavigateCommand.Execute(page);
            await Dispatcher.InvokeAsync(window.UpdateLayout, DispatcherPriority.Loaded);
            await Dispatcher.InvokeAsync(window.UpdateLayout, DispatcherPriority.ApplicationIdle);
            if (Volatile.Read(ref _fatalErrorShown) != 0)
            {
                return;
            }
        }
    }
}
