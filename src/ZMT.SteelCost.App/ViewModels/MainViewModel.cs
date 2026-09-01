using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using ZMT.SteelCost.App.Mvvm;
using ZMT.SteelCost.App.Services;
using ZMT.SteelCost.Application.Calculation;
using ZMT.SteelCost.Application.Logging;
using ZMT.SteelCost.Application.Offers;
using ZMT.SteelCost.Application.Pricing;
using ZMT.SteelCost.Application.Projects;
using ZMT.SteelCost.Application.Reports;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.App.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly IPriceListService _priceLists;
    private readonly IReportExportService _reports;
    private readonly IOfferDocumentMapper _offerMapper;
    private readonly IOfferPdfExportService _offerPdf;
    private readonly OfferDocumentValidationService _offerValidator;
    private readonly IFileDialogService _dialogs;
    private readonly IAppLogger _logger;
    private readonly IRoofCalculationService _roofService;
    private readonly OfferEditorPageViewModel _offerEditorPage;
    private Project _activeProject;
    private CalculationResult? _result;
    private PageViewModel _currentPage;
    private ResultLineViewModel? _selectedResultLine;
    private string _statusMessage = "Hazır";
    private bool _showInternalPrices = true;
    private bool _hideZeroDoorWindows;
    private ReportKind _selectedReportKind = ReportKind.CustomerOffer;
    private OfferLanguage _selectedOfferLanguage = OfferLanguage.Turkish;
    private bool _includeOfferTechnicalSpecification = true;
    private int _offerValidityDays = 7;
    private int _offerDeliveryDays = 15;
    private string _offerPaymentTerms = "Karşılıklı görüşme ile belirlenecektir.";
    private string _offerAdditionalNotes = string.Empty;

    public MainViewModel(
        IProjectService projectService,
        IPriceListService priceLists,
        IReportExportService reports,
        IOfferDocumentMapper offerMapper,
        IOfferPdfExportService offerPdf,
        OfferDocumentValidationService offerValidator,
        IFileDialogService dialogs,
        IAppLogger logger,
        IRoofCalculationService roofService)
    {
        _projectService = projectService;
        _priceLists = priceLists;
        _reports = reports;
        _offerMapper = offerMapper;
        _offerPdf = offerPdf;
        _offerValidator = offerValidator;
        _dialogs = dialogs;
        _logger = logger;
        _roofService = roofService;
        _offerEditorPage = new OfferEditorPageViewModel(this, offerMapper, offerPdf, offerValidator, dialogs, logger);
        _activeProject = projectService.CreateNew();
        EnsureMasterSelections(_activeProject.Building);
        _currentPage = new DashboardPageViewModel(this);

        NavigateCommand = new RelayCommand<string>(Navigate);
        NewProjectCommand = new AsyncRelayCommand(() => NewProjectAsync(true));
        CalculateCommand = new RelayCommand(Calculate);
        CalculateAndShowResultsCommand = new RelayCommand(() =>
        {
            Calculate();
            if (Result is not null)
            {
                Navigate("Results");
            }
        });
        ApplyCommercialRatesCommand = new RelayCommand(ApplyCommercialRates);
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        OpenProjectCommand = new AsyncRelayCommand<Project>(OpenProjectAsync);
        ApplyOverrideCommand = new RelayCommand<ResultLineViewModel>(ApplyOverride);
        ApplyMaterialPriceCommand = new RelayCommand<MaterialPriceRowViewModel>(ApplyMaterialPrice);
        ResetMaterialPricesCommand = new RelayCommand(ResetMaterialPrices);
        UseLatestPriceListCommand = new AsyncRelayCommand(UseLatestPriceListAsync);
        PublishPriceListVersionCommand = new AsyncRelayCommand(PublishPriceListVersionAsync);
        ExportPdfCommand = new AsyncRelayCommand(ExportPdfAsync, () => Result is not null);
        ExportOfferPdfCommand = new AsyncRelayCommand(ExportOfferPdfAsync, () => Result is not null);
        ExportExcelCommand = new AsyncRelayCommand(ExportExcelAsync, () => Result is not null);
        RefreshRecentCommand = new AsyncRelayCommand(LoadRecentAsync);

        BuildScopeRows();
        Calculate();
        CurrentPage = new DashboardPageViewModel(this);
    }

    public Project ActiveProject
    {
        get => _activeProject;
        private set
        {
            if (SetProperty(ref _activeProject, value))
            {
                OnPropertyChanged(nameof(RoofPreview));
                OnPropertyChanged(nameof(HasActiveProject));
            }
        }
    }

    public CalculationResult? Result
    {
        get => _result;
        private set
        {
            if (SetProperty(ref _result, value))
            {
                OnPropertyChanged(nameof(HasResult));
            }
        }
    }

    public PageViewModel CurrentPage { get => _currentPage; private set => SetProperty(ref _currentPage, value); }
    public ResultLineViewModel? SelectedResultLine { get => _selectedResultLine; set => SetProperty(ref _selectedResultLine, value); }
    public string StatusMessage { get => _statusMessage; private set => SetProperty(ref _statusMessage, value); }
    public bool HasActiveProject => ActiveProject is not null;
    public bool HasResult => Result is not null;
    public string PriceListLabel => ActiveProject.PriceListVersionId is Guid id
        ? $"Legacy · {id.ToString("N")[..8]}"
        : "Varsayılan";
    public bool ShowInternalPrices { get => _showInternalPrices; set => SetProperty(ref _showInternalPrices, value); }
    public bool HideZeroDoorWindows { get => _hideZeroDoorWindows; set => SetProperty(ref _hideZeroDoorWindows, value); }
    public ReportKind SelectedReportKind { get => _selectedReportKind; set => SetProperty(ref _selectedReportKind, value); }
    public OfferLanguage SelectedOfferLanguage { get => _selectedOfferLanguage; set => SetProperty(ref _selectedOfferLanguage, value); }
    public bool IncludeOfferTechnicalSpecification { get => _includeOfferTechnicalSpecification; set => SetProperty(ref _includeOfferTechnicalSpecification, value); }
    public int OfferValidityDays { get => _offerValidityDays; set => SetProperty(ref _offerValidityDays, value); }
    public int OfferDeliveryDays { get => _offerDeliveryDays; set => SetProperty(ref _offerDeliveryDays, value); }
    public string OfferPaymentTerms { get => _offerPaymentTerms; set => SetProperty(ref _offerPaymentTerms, value); }
    public string OfferAdditionalNotes { get => _offerAdditionalNotes; set => SetProperty(ref _offerAdditionalNotes, value); }
    public decimal DiscountPercent
    {
        get => ActiveProject.DiscountRateOverride.HasValue
            ? ActiveProject.DiscountRateOverride.Value * 100m
            : ActiveProject.PricingSnapshot.DiscountRate * 100m;
        set
        {
            ActiveProject.DiscountRateOverride = value / 100m;
            OnPropertyChanged();
        }
    }
    public decimal VatPercent
    {
        get => ActiveProject.SalesVatRateOverride.HasValue
            ? ActiveProject.SalesVatRateOverride.Value * 100m
            : ActiveProject.PricingSnapshot.SalesVatRate * 100m;
        set
        {
            ActiveProject.SalesVatRateOverride = value / 100m;
            OnPropertyChanged();
        }
    }
    public decimal SalesMarkupPercent
    {
        get => (ActiveProject.PricingSnapshot.SalesMarkupFactor - 1m) * 100m;
        set
        {
            ActiveProject.PricingSnapshot.SalesMarkupFactor = 1m + (value / 100m);
            OnPropertyChanged();
        }
    }
    public Array ReportKinds { get; } = Enum.GetValues<ReportKind>();
    public Array OfferLanguages { get; } = Enum.GetValues<OfferLanguage>();
    public Array QuantityModes { get; } = Enum.GetValues<QuantityMode>();
    public Array ResponsibilityTypes { get; } = Enum.GetValues<ResponsibilityType>();
    public RoofCalculationResult RoofPreview => _roofService.Calculate(ActiveProject.Building);
    public ObservableCollection<Project> RecentProjects { get; } = [];
    public ObservableCollection<ResultLineViewModel> ResultLines { get; } = [];
    public ObservableCollection<CategoryScopeViewModel> ScopeRows { get; } = [];
    public ObservableCollection<MaterialPriceRowViewModel> MaterialRows { get; } = [];
    public IReadOnlyList<LegacyMaterialDefinition> Materials => LegacyExcelV1Rules.Materials;

    public ICommand NavigateCommand { get; }
    public ICommand NewProjectCommand { get; }
    public ICommand CalculateCommand { get; }
    public ICommand CalculateAndShowResultsCommand { get; }
    public ICommand ApplyCommercialRatesCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand OpenProjectCommand { get; }
    public ICommand ApplyOverrideCommand { get; }
    public ICommand ApplyMaterialPriceCommand { get; }
    public ICommand ResetMaterialPricesCommand { get; }
    public ICommand UseLatestPriceListCommand { get; }
    public ICommand PublishPriceListVersionCommand { get; }
    public ICommand ExportPdfCommand { get; }
    public ICommand ExportOfferPdfCommand { get; }
    public ICommand ExportExcelCommand { get; }
    public ICommand RefreshRecentCommand { get; }

    public async Task InitializeAsync()
    {
        try
        {
            await ApplyLatestPriceListAsync();
            Calculate();
        }
        catch (Exception exception)
        {
            StatusMessage = "Aktif fiyat listesi okunamadı; varsayılan snapshot kullanılıyor.";
            _logger.Error("PriceListLoadFailed", exception, new { ActiveProject.Id });
        }
        await LoadRecentAsync();
    }

    private void Navigate(string? target)
    {
        CurrentPage = target switch
        {
            "Dashboard" => new DashboardPageViewModel(this),
            "Projects" => new ProjectsPageViewModel(this),
            "NewCalculation" or "ProjectInfo" => new ProjectInfoPageViewModel(this),
            "Building" => new BuildingPageViewModel(this),
            "Cladding" => new CladdingPageViewModel(this),
            "DoorWindow" => new DoorWindowPageViewModel(this),
            "Fixtures" => new FixturesPageViewModel(this),
            "Results" => new ResultsPageViewModel(this),
            "Materials" => new MaterialsPageViewModel(this),
            "Reports" => new ReportsPageViewModel(this),
            "OfferEditor" => OpenOfferEditor(),
            "Settings" => new SettingsPageViewModel(this),
            _ => CurrentPage
        };
    }

    private PageViewModel OpenOfferEditor()
    {
        _offerEditorPage.EnsureLoaded();
        return _offerEditorPage;
    }

    public void SetStatusMessage(string message) => StatusMessage = message;

    private async Task NewProjectAsync(bool navigate)
    {
        ActiveProject = _projectService.CreateNew();
        EnsureMasterSelections(ActiveProject.Building);
        await ApplyLatestPriceListAsync();
        Result = null;
        ResultLines.Clear();
        BuildScopeRows();
        StatusMessage = "Yeni proje oluşturuldu.";
        if (navigate)
        {
            Navigate("ProjectInfo");
        }
    }

    private void Calculate()
    {
        try
        {
            SyncScopes();
            Result = _projectService.Calculate(ActiveProject);
            ResultLines.Clear();
            foreach (var category in Result.Categories)
            {
                foreach (var line in category.Lines)
                {
                    ResultLines.Add(new(category.CategoryName, line));
                }
            }
            MaterialRows.Clear();
            var definitions = LegacyExcelV1Rules.Materials.ToDictionary(item => item.Code, StringComparer.Ordinal);
            foreach (var category in Result.Categories)
            {
                foreach (var line in category.Lines)
                {
                    var definition = definitions[line.MaterialCode];
                    MaterialRows.Add(new(line.MaterialCode, line.CategoryId, category.CategoryName, line.MaterialName,
                        line.Specification, line.Unit, line.PurchaseUnitPriceExVat, definition.PricingRuleId,
                        !ActiveProject.InactiveMaterialCodes.Contains(line.MaterialCode)));
                }
            }
            StatusMessage = $"Hesap tamamlandı · {DateTime.Now:HH:mm}";
            _logger.Information("CalculationCompleted", "Proje hesaplandı.", new { ActiveProject.Id, Result.GrandTotal });
        }
        catch (Exception exception)
        {
            StatusMessage = exception.Message;
            _logger.Error("CalculationFailed", exception, new { ActiveProject.Id });
        }
    }

    private void ApplyCommercialRates()
    {
        if (SalesMarkupPercent < 0m)
        {
            StatusMessage = "Kâr ekleme oranı negatif olamaz.";
            return;
        }
        if (DiscountPercent is < 0m or > 100m || VatPercent is < 0m or > 100m)
        {
            StatusMessage = "İskonto ve KDV oranları 0 ile 100 arasında olmalıdır.";
            return;
        }

        Calculate();
        if (Result is not null)
        {
            Navigate("Results");
            StatusMessage = "Kâr, iskonto ve KDV oranları teklife uygulandı.";
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ActiveProject.Company) || string.IsNullOrWhiteSpace(ActiveProject.CustomerName))
            {
                StatusMessage = "Firma ve müşteri adı zorunludur.";
                return;
            }
            if (Result is null)
            {
                Calculate();
            }
            if (Result is null)
            {
                return;
            }
            await _projectService.SaveAsync(ActiveProject, Result);
            await LoadRecentAsync();
            StatusMessage = "Proje ve hesap snapshot'ı kaydedildi.";
        }
        catch (Exception exception)
        {
            StatusMessage = "Proje kaydedilemedi.";
            _logger.Error("ProjectSaveFailed", exception, new { ActiveProject.Id });
        }
    }

    private async Task OpenProjectAsync(Project? project)
    {
        if (project is null)
        {
            return;
        }
        var loaded = await _projectService.OpenAsync(project.Id);
        if (loaded is null)
        {
            StatusMessage = "Proje bulunamadı.";
            return;
        }
        ActiveProject = loaded;
        OnPropertyChanged(nameof(PriceListLabel));
        EnsureMasterSelections(ActiveProject.Building);
        BuildScopeRows();
        Calculate();
        Navigate("ProjectInfo");
        StatusMessage = "Proje açıldı.";
    }

    private async Task LoadRecentAsync()
    {
        RecentProjects.Clear();
        foreach (var project in await _projectService.GetRecentAsync(10))
        {
            RecentProjects.Add(project);
        }
    }

    private void ApplyOverride(ResultLineViewModel? row)
    {
        if (row is null)
        {
            return;
        }
        ActiveProject.MaterialOverrides.RemoveAll(item => item.MaterialCode == row.MaterialCode);
        if (row.Mode == QuantityMode.Manual)
        {
            if (row.OverrideQuantity is null or < 0m || string.IsNullOrWhiteSpace(row.OverrideReason))
            {
                StatusMessage = "Manual override için negatif olmayan miktar ve açıklama zorunludur.";
                return;
            }
            ActiveProject.MaterialOverrides.Add(new()
            {
                MaterialCode = row.MaterialCode,
                Mode = QuantityMode.Manual,
                CalculatedQuantity = row.CalculatedQuantity,
                OverrideQuantity = row.OverrideQuantity,
                OverrideReason = row.OverrideReason
            });
        }
        Calculate();
        Navigate("Results");
    }

    private void ApplyMaterialPrice(MaterialPriceRowViewModel? row)
    {
        if (row is null || row.PurchasePrice < 0m)
        {
            StatusMessage = "Alış fiyatı negatif olamaz.";
            return;
        }
        ActiveProject.MaterialPriceOverrides[row.Code] = row.PurchasePrice;
        if (row.IsActive)
        {
            ActiveProject.InactiveMaterialCodes.Remove(row.Code);
        }
        else
        {
            ActiveProject.InactiveMaterialCodes.Add(row.Code);
        }
        Calculate();
        Navigate("Materials");
        StatusMessage = $"{row.Code} fiyat ve aktiflik durumu proje snapshot'ına uygulandı.";
    }

    private void ResetMaterialPrices()
    {
        ActiveProject.MaterialPriceOverrides.Clear();
        Calculate();
        Navigate("Materials");
        StatusMessage = "Güncel hesaplanan fiyat kuralları yeniden uygulandı.";
    }

    private async Task UseLatestPriceListAsync()
    {
        try
        {
            await ApplyLatestPriceListAsync();
            Calculate();
            Navigate("Materials");
            StatusMessage = "Aktif fiyat listesi snapshot'ı projeye uygulandı.";
        }
        catch (Exception exception)
        {
            StatusMessage = "Güncel fiyat listesi uygulanamadı.";
            _logger.Error("PriceListRefreshFailed", exception, new { ActiveProject.Id });
        }
    }

    private async Task PublishPriceListVersionAsync()
    {
        try
        {
            var version = await _priceLists.CreateVersionAsync(
                ActiveProject.PricingSnapshot,
                ActiveProject.MaterialPriceOverrides);
            ApplyPriceVersion(version);
            Calculate();
            StatusMessage = $"Fiyat listesi v{version.VersionNumber} oluşturuldu ve projeye bağlandı.";
        }
        catch (Exception exception)
        {
            StatusMessage = "Fiyat listesi sürümü oluşturulamadı.";
            _logger.Error("PriceListVersionCreateFailed", exception, new { ActiveProject.Id });
        }
    }

    private async Task ApplyLatestPriceListAsync()
    {
        var version = await _priceLists.GetActiveVersionAsync();
        ApplyPriceVersion(version);
    }

    private void ApplyPriceVersion(PriceListVersion version)
    {
        ActiveProject.PriceListVersionId = version.Id;
        ActiveProject.PricingSnapshot = version.Parameters.Snapshot();
        ActiveProject.MaterialPriceOverrides.Clear();

        var baseline = new LegacyRuleContext(ActiveProject.Building, ActiveProject.PricingSnapshot, _roofService);
        foreach (var price in version.Prices)
        {
            if (Math.Abs(price.PurchasePrice - baseline.PurchaseUnitPriceExVat(price.MaterialCode)) > 0.000001m)
            {
                ActiveProject.MaterialPriceOverrides[price.MaterialCode] = price.PurchasePrice;
            }
        }
        OnPropertyChanged(nameof(ActiveProject));
        OnPropertyChanged(nameof(PriceListLabel));
        OnPropertyChanged(nameof(SalesMarkupPercent));
        OnPropertyChanged(nameof(DiscountPercent));
        OnPropertyChanged(nameof(VatPercent));
    }

    private async Task ExportPdfAsync()
    {
        try
        {
            if (Result is null)
            {
                return;
            }
            var path = _dialogs.SavePdf($"ZMT-{ActiveProject.CrmNumber}-{SelectedReportKind}");
            if (path is null)
            {
                return;
            }
            await _reports.ExportPdfAsync(ActiveProject, Result, SelectedReportKind, path);
            StatusMessage = $"PDF oluşturuldu: {Path.GetFileName(path)}";
        }
        catch (Exception exception)
        {
            StatusMessage = "PDF oluşturulamadı. Ayrıntılar günlüğe yazıldı.";
            _logger.Error("PdfExportFailed", exception, new { ActiveProject.Id, SelectedReportKind });
        }
    }

    private async Task ExportOfferPdfAsync()
    {
        try
        {
            if (Result is null)
            {
                return;
            }
            if (OfferValidityDays <= 0 || OfferDeliveryDays <= 0)
            {
                StatusMessage = "Teklif geçerlilik ve teslim süreleri sıfırdan büyük olmalıdır.";
                return;
            }
            if (string.IsNullOrWhiteSpace(OfferPaymentTerms))
            {
                StatusMessage = "Teklif için ödeme şekli boş olamaz.";
                return;
            }

            var source = _offerMapper.Map(ActiveProject, Result, new OfferGenerationOptions
            {
                ValidityDays = OfferValidityDays,
                DeliveryDays = OfferDeliveryDays,
                PaymentTerms = OfferPaymentTerms,
                AdditionalNotes = OfferAdditionalNotes,
                IncludeTechnicalSpecification = IncludeOfferTechnicalSpecification
            });
            var document = OfferDocumentLocalizer.Localize(source, SelectedOfferLanguage);
            var validationErrors = _offerValidator.Validate(document);
            if (validationErrors.Count != 0)
            {
                StatusMessage = string.Join(" ", validationErrors);
                return;
            }

            var languageCode = SelectedOfferLanguage == OfferLanguage.English ? "EN" : "TR";
            var path = _dialogs.SavePdf($"ZMT-{ActiveProject.CrmNumber}-Teklif-{languageCode}");
            if (path is null)
            {
                return;
            }
            await _offerPdf.ExportAsync(document, new PdfExportOptions
            {
                OutputPath = path,
                Language = SelectedOfferLanguage,
                DocumentTitle = document.Info.MainTitle,
                IncludeImages = true,
                IncludeTechnicalSpecification = IncludeOfferTechnicalSpecification,
                OpenAfterExport = true
            });
            StatusMessage = $"Ayrı teklif PDF'i oluşturuldu: {Path.GetFileName(path)}";
            _logger.Information("OfferPdfExported", "Ayrı teklif PDF'i oluşturuldu.", new
            {
                ActiveProject.Id,
                ActiveProject.CrmNumber,
                Language = SelectedOfferLanguage.ToString(),
                IncludeOfferTechnicalSpecification
            });
        }
        catch (Exception exception)
        {
            StatusMessage = "Teklif PDF'i oluşturulamadı. Ayrıntılar günlüğe yazıldı.";
            _logger.Error("OfferPdfExportFailed", exception, new { ActiveProject.Id, SelectedOfferLanguage });
        }
    }

    private async Task ExportExcelAsync()
    {
        try
        {
            if (Result is null)
            {
                return;
            }
            var path = _dialogs.SaveExcel($"ZMT-{ActiveProject.CrmNumber}-Hesap");
            if (path is null)
            {
                return;
            }
            await _reports.ExportExcelAsync(ActiveProject, Result, path);
            StatusMessage = $"Excel oluşturuldu: {Path.GetFileName(path)}";
        }
        catch (Exception exception)
        {
            StatusMessage = "Excel oluşturulamadı. Ayrıntılar günlüğe yazıldı.";
            _logger.Error("ExcelExportFailed", exception, new { ActiveProject.Id });
        }
    }

    private void BuildScopeRows()
    {
        ScopeRows.Clear();
        var configured = ActiveProject.CategoryScopes.ToDictionary(item => item.CategoryId, item => item.Responsibility);
        foreach (var category in LegacyExcelV1Rules.Materials.GroupBy(item => new { item.CategoryId, item.CategoryName }).OrderBy(item => item.Key.CategoryId))
        {
            var responsibility = configured.TryGetValue(category.Key.CategoryId, out var value)
                ? value
                : category.Key.CategoryId is 1004 or 1008 or 1010 ? ResponsibilityType.Customer : ResponsibilityType.Zmt;
            ScopeRows.Add(new(category.Key.CategoryId, category.Key.CategoryName, responsibility));
        }
    }

    private void SyncScopes()
    {
        ActiveProject.CategoryScopes = ScopeRows.Select(item => new ProjectCategoryScope(item.CategoryId, item.Responsibility)).ToList();
    }

    private static void EnsureMasterSelections(BuildingInput input)
    {
        foreach (var type in Enum.GetValues<DoorType>())
        {
            if (input.Doors.All(item => item.Type != type))
            {
                input.Doors.Add(new(type, 0));
            }
        }
        foreach (var type in Enum.GetValues<WindowType>())
        {
            if (input.Windows.All(item => item.Type != type))
            {
                input.Windows.Add(new(type, 0, input.WindowColor));
            }
        }
        foreach (var type in Enum.GetValues<FixtureType>())
        {
            if (input.Fixtures.All(item => item.Type != type))
            {
                input.Fixtures.Add(new(type, 0, 0));
            }
        }
        foreach (var type in Enum.GetValues<SurfaceType>())
        {
            if (input.Surfaces.All(item => item.Surface != type))
            {
                input.Surfaces.Add(new(type, 0m, []));
            }
        }
    }
}
