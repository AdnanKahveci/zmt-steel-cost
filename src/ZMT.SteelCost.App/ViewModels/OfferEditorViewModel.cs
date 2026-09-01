using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ZMT.SteelCost.App.Mvvm;
using ZMT.SteelCost.App.Services;
using ZMT.SteelCost.Application.Logging;
using ZMT.SteelCost.Application.Offers;

namespace ZMT.SteelCost.App.ViewModels;

public sealed class OfferEditorPageViewModel : PageViewModel
{
    private static readonly HashSet<string> SupportedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".bmp", ".webp"
    };

    private readonly IOfferDocumentMapper _mapper;
    private readonly IOfferPdfExportService _pdf;
    private readonly OfferDocumentValidationService _validator;
    private readonly IFileDialogService _dialogs;
    private readonly IAppLogger _logger;
    private Guid? _loadedProjectId;
    private Guid? _loadedRunId;
    private OfferInfo _info = new();
    private CompanySettings _company = new();
    private string _offerTitle = string.Empty;
    private string _offerNotes = string.Empty;
    private string _deliveryText = string.Empty;
    private string _currency = "TRY";
    private decimal _discountPercent;
    private decimal _vatPercent;
    private OfferLanguage _selectedLanguage = OfferLanguage.Turkish;
    private bool _includeImages = true;
    private bool _includeTechnicalSpecification = true;
    private bool _openAfterExport = true;
    private string _technicalSpecificationTitle = "HAFİF ÇELİK BİNA TEKNİK ŞARTNAMESİ";
    private string _statusMessage = "Hesap sonucundan teklif verilerini yükleyin veya mevcut alanları düzenleyin.";
    private string? _lastGeneratedPdfPath;
    private OfferScopeGroupEditor? _selectedIncludedGroup;
    private OfferBulletEditor? _selectedExcludedItem;
    private OfferItemEditor? _selectedOfferItem;
    private OfferBulletEditor? _selectedPaymentItem;
    private OfferImageEditor? _selectedOfferImage;
    private OfferImageEditor? _selectedTechnicalImage;
    private TechnicalSpecSectionEditor? _selectedTechnicalSection;

    public OfferEditorPageViewModel(
        MainViewModel owner,
        IOfferDocumentMapper mapper,
        IOfferPdfExportService pdf,
        OfferDocumentValidationService validator,
        IFileDialogService dialogs,
        IAppLogger logger)
        : base(owner, "Teklif Formu", "Ayrıntılı teklif, görseller ve teknik şartname")
    {
        _mapper = mapper;
        _pdf = pdf;
        _validator = validator;
        _dialogs = dialogs;
        _logger = logger;

        OfferItems.CollectionChanged += OfferItemsChanged;

        RefreshFromCalculationCommand = new RelayCommand(() => EnsureLoaded(true));
        AddIncludedGroupCommand = new RelayCommand(AddIncludedGroup);
        RemoveIncludedGroupCommand = new RelayCommand<OfferScopeGroupEditor>(RemoveIncludedGroup);
        AddIncludedItemCommand = new RelayCommand<OfferScopeGroupEditor>(AddIncludedItem);
        RemoveIncludedItemCommand = new RelayCommand<OfferBulletEditor>(RemoveIncludedItem);
        AddExcludedItemCommand = new RelayCommand(AddExcludedItem);
        RemoveExcludedItemCommand = new RelayCommand(RemoveExcludedItem);
        AddOfferItemCommand = new RelayCommand(AddOfferItem);
        RemoveOfferItemCommand = new RelayCommand(RemoveOfferItem);
        MoveOfferItemUpCommand = new RelayCommand(() => MoveOfferItem(-1));
        MoveOfferItemDownCommand = new RelayCommand(() => MoveOfferItem(1));
        AddPaymentItemCommand = new RelayCommand(AddPaymentItem);
        RemovePaymentItemCommand = new RelayCommand(RemovePaymentItem);
        AddOfferImagesCommand = new RelayCommand(() => AddImages(ImageSections.Offer));
        AddTechnicalImagesCommand = new RelayCommand(() => AddImages(ImageSections.TechnicalSpec));
        RemoveImageCommand = new RelayCommand<OfferImageEditor>(RemoveImage);
        MoveOfferImageUpCommand = new RelayCommand(() => MoveImage(ImageSections.Offer, -1));
        MoveOfferImageDownCommand = new RelayCommand(() => MoveImage(ImageSections.Offer, 1));
        MoveTechnicalImageUpCommand = new RelayCommand(() => MoveImage(ImageSections.TechnicalSpec, -1));
        MoveTechnicalImageDownCommand = new RelayCommand(() => MoveImage(ImageSections.TechnicalSpec, 1));
        SelectLogoCommand = new RelayCommand(SelectLogo);
        AddTechnicalSectionCommand = new RelayCommand(AddTechnicalSection);
        RemoveTechnicalSectionCommand = new RelayCommand(RemoveTechnicalSection);
        PreviewPdfCommand = new AsyncRelayCommand(PreviewPdfAsync);
        ExportPdfCommand = new AsyncRelayCommand(ExportPdfAsync);
        OpenLastPdfCommand = new RelayCommand(OpenLastPdf);
    }

    public OfferInfo Info { get => _info; private set => SetProperty(ref _info, value); }
    public CompanySettings Company { get => _company; private set => SetProperty(ref _company, value); }
    public ObservableCollection<OfferScopeGroupEditor> IncludedGroups { get; } = [];
    public ObservableCollection<OfferBulletEditor> ExcludedWorks { get; } = [];
    public ObservableCollection<OfferItemEditor> OfferItems { get; } = [];
    public ObservableCollection<OfferBulletEditor> PaymentItems { get; } = [];
    public ObservableCollection<OfferImageEditor> OfferImages { get; } = [];
    public ObservableCollection<OfferImageEditor> TechnicalImages { get; } = [];
    public ObservableCollection<TechnicalSpecSectionEditor> TechnicalSections { get; } = [];
    public IReadOnlyList<string> Currencies { get; } = ["TRY", "USD", "EUR"];
    public Array Languages { get; } = Enum.GetValues<OfferLanguage>();

    public string OfferTitle { get => _offerTitle; set => SetProperty(ref _offerTitle, value); }
    public string OfferNotes { get => _offerNotes; set => SetProperty(ref _offerNotes, value); }
    public string DeliveryText { get => _deliveryText; set => SetProperty(ref _deliveryText, value); }
    public string Currency { get => _currency; set { if (SetProperty(ref _currency, value)) RefreshTotals(); } }
    public decimal DiscountPercent { get => _discountPercent; set { if (SetProperty(ref _discountPercent, value)) RefreshTotals(); } }
    public decimal VatPercent { get => _vatPercent; set { if (SetProperty(ref _vatPercent, value)) RefreshTotals(); } }
    public OfferLanguage SelectedLanguage { get => _selectedLanguage; set => SetProperty(ref _selectedLanguage, value); }
    public bool IncludeImages { get => _includeImages; set => SetProperty(ref _includeImages, value); }
    public bool IncludeTechnicalSpecification { get => _includeTechnicalSpecification; set => SetProperty(ref _includeTechnicalSpecification, value); }
    public bool OpenAfterExport { get => _openAfterExport; set => SetProperty(ref _openAfterExport, value); }
    public string TechnicalSpecificationTitle { get => _technicalSpecificationTitle; set => SetProperty(ref _technicalSpecificationTitle, value); }
    public string StatusMessage { get => _statusMessage; private set => SetProperty(ref _statusMessage, value); }
    public string? LastGeneratedPdfPath { get => _lastGeneratedPdfPath; private set => SetProperty(ref _lastGeneratedPdfPath, value); }
    public decimal ScopeTotal => OfferItems.Sum(item => item.Total);
    public decimal DiscountAmount => Math.Round(ScopeTotal * DiscountPercent / 100m, 2, MidpointRounding.AwayFromZero);
    public decimal Subtotal => ScopeTotal - DiscountAmount;
    public decimal VatAmount => Math.Round(Subtotal * VatPercent / 100m, 2, MidpointRounding.AwayFromZero);
    public decimal GrandTotal => Subtotal + VatAmount;
    public string ImagesSummary => $"{OfferImages.Count} teklif · {TechnicalImages.Count} şartname görseli";

    public OfferScopeGroupEditor? SelectedIncludedGroup { get => _selectedIncludedGroup; set => SetProperty(ref _selectedIncludedGroup, value); }
    public OfferBulletEditor? SelectedExcludedItem { get => _selectedExcludedItem; set => SetProperty(ref _selectedExcludedItem, value); }
    public OfferItemEditor? SelectedOfferItem { get => _selectedOfferItem; set => SetProperty(ref _selectedOfferItem, value); }
    public OfferBulletEditor? SelectedPaymentItem { get => _selectedPaymentItem; set => SetProperty(ref _selectedPaymentItem, value); }
    public OfferImageEditor? SelectedOfferImage { get => _selectedOfferImage; set => SetProperty(ref _selectedOfferImage, value); }
    public OfferImageEditor? SelectedTechnicalImage { get => _selectedTechnicalImage; set => SetProperty(ref _selectedTechnicalImage, value); }
    public TechnicalSpecSectionEditor? SelectedTechnicalSection { get => _selectedTechnicalSection; set => SetProperty(ref _selectedTechnicalSection, value); }

    public ICommand RefreshFromCalculationCommand { get; }
    public ICommand AddIncludedGroupCommand { get; }
    public ICommand RemoveIncludedGroupCommand { get; }
    public ICommand AddIncludedItemCommand { get; }
    public ICommand RemoveIncludedItemCommand { get; }
    public ICommand AddExcludedItemCommand { get; }
    public ICommand RemoveExcludedItemCommand { get; }
    public ICommand AddOfferItemCommand { get; }
    public ICommand RemoveOfferItemCommand { get; }
    public ICommand MoveOfferItemUpCommand { get; }
    public ICommand MoveOfferItemDownCommand { get; }
    public ICommand AddPaymentItemCommand { get; }
    public ICommand RemovePaymentItemCommand { get; }
    public ICommand AddOfferImagesCommand { get; }
    public ICommand AddTechnicalImagesCommand { get; }
    public ICommand RemoveImageCommand { get; }
    public ICommand MoveOfferImageUpCommand { get; }
    public ICommand MoveOfferImageDownCommand { get; }
    public ICommand MoveTechnicalImageUpCommand { get; }
    public ICommand MoveTechnicalImageDownCommand { get; }
    public ICommand SelectLogoCommand { get; }
    public ICommand AddTechnicalSectionCommand { get; }
    public ICommand RemoveTechnicalSectionCommand { get; }
    public ICommand PreviewPdfCommand { get; }
    public ICommand ExportPdfCommand { get; }
    public ICommand OpenLastPdfCommand { get; }

    public void Invalidate() => _loadedRunId = null;

    public void EnsureLoaded(bool force = false)
    {
        if (Owner.Result is null)
        {
            StatusMessage = "Teklif formu için önce projeyi hesaplayın.";
            return;
        }
        if (!force && _loadedProjectId == Owner.ActiveProject.Id && _loadedRunId == Owner.Result.RunId)
        {
            return;
        }
        if (_loadedProjectId.HasValue && _loadedProjectId != Owner.ActiveProject.Id)
        {
            OfferImages.Clear();
            TechnicalImages.Clear();
            SelectedOfferImage = null;
            SelectedTechnicalImage = null;
            OnPropertyChanged(nameof(ImagesSummary));
        }

        var mapped = _mapper.Map(Owner.ActiveProject, Owner.Result, new OfferGenerationOptions
        {
            ValidityDays = Math.Max(1, Owner.OfferValidityDays),
            DeliveryDays = Math.Max(1, Owner.OfferDeliveryDays),
            PaymentTerms = string.IsNullOrWhiteSpace(Owner.OfferPaymentTerms)
                ? "Karşılıklı görüşme ile belirlenecektir."
                : Owner.OfferPaymentTerms,
            AdditionalNotes = Owner.OfferAdditionalNotes,
            IncludeTechnicalSpecification = Owner.IncludeOfferTechnicalSpecification
        });

        Info = mapped.Info;
        Company = mapped.CompanySettings;
        Company.HeaderLogoPath = EnsureBundledLogoFile();
        OfferTitle = mapped.OfferTitle;
        OfferNotes = mapped.OfferNotes;
        DeliveryText = mapped.DeliveryText;
        Currency = mapped.Currency;
        DiscountPercent = mapped.DiscountRate * 100m;
        VatPercent = mapped.VatRate * 100m;
        SelectedLanguage = Owner.SelectedOfferLanguage;
        IncludeTechnicalSpecification = mapped.TechnicalSpecification.IncludeInPdf;
        TechnicalSpecificationTitle = mapped.TechnicalSpecification.Title;

        IncludedGroups.Clear();
        foreach (var group in mapped.IncludedWorkGroups.OrderBy(item => item.SortOrder))
        {
            IncludedGroups.Add(OfferScopeGroupEditor.FromModel(group));
        }
        ExcludedWorks.ReplaceWith(mapped.ExcludedWorks.OrderBy(item => item.SortOrder).Select(OfferBulletEditor.FromModel));
        OfferItems.ReplaceWith(mapped.OfferItems.OrderBy(item => item.RowNo).Select(OfferItemEditor.FromModel));
        PaymentItems.ReplaceWith(mapped.PaymentItems.OrderBy(item => item.SortOrder).Select(OfferBulletEditor.FromModel));
        TechnicalSections.ReplaceWith(mapped.TechnicalSpecification.Sections.OrderBy(item => item.SortOrder).Select(TechnicalSpecSectionEditor.FromModel));

        _loadedProjectId = Owner.ActiveProject.Id;
        _loadedRunId = Owner.Result.RunId;
        SelectedIncludedGroup = IncludedGroups.FirstOrDefault();
        SelectedOfferItem = OfferItems.FirstOrDefault();
        SelectedPaymentItem = PaymentItems.FirstOrDefault();
        SelectedTechnicalSection = TechnicalSections.FirstOrDefault();
        RefreshTotals();
        StatusMessage = force
            ? "Teklif formu son hesap sonucundan yeniden oluşturuldu. Önceki form düzenlemeleri sıfırlandı."
            : "Teklif formu son hesap sonucundan oluşturuldu; tüm alanlar PDF öncesinde düzenlenebilir.";
    }

    private void AddIncludedGroup()
    {
        var group = new OfferScopeGroupEditor { SortOrder = IncludedGroups.Count + 1, Title = "Yeni kapsam grubu" };
        group.Items.Add(new OfferBulletEditor { SortOrder = 1, Text = "Yeni dahil iş açıklaması" });
        IncludedGroups.Add(group);
        SelectedIncludedGroup = group;
    }

    private void RemoveIncludedGroup(OfferScopeGroupEditor? selectedGroup)
    {
        selectedGroup ??= SelectedIncludedGroup;
        if (selectedGroup is null) return;
        var index = IncludedGroups.IndexOf(selectedGroup);
        IncludedGroups.Remove(selectedGroup);
        Reindex(IncludedGroups, (item, order) => item.SortOrder = order);
        SelectedIncludedGroup = IncludedGroups.Count == 0 ? null : IncludedGroups[Math.Min(index, IncludedGroups.Count - 1)];
    }

    private static void AddIncludedItem(OfferScopeGroupEditor? group)
    {
        if (group is null) return;
        group.Items.Add(new OfferBulletEditor { SortOrder = group.Items.Count + 1, Text = "Yeni dahil iş açıklaması" });
    }

    private void RemoveIncludedItem(OfferBulletEditor? item)
    {
        if (item is null) return;
        var group = IncludedGroups.FirstOrDefault(value => value.Items.Contains(item));
        if (group is null) return;
        group.Items.Remove(item);
        Reindex(group.Items, (value, order) => value.SortOrder = order);
    }

    private void AddExcludedItem()
    {
        var item = new OfferBulletEditor { SortOrder = ExcludedWorks.Count + 1, Text = "Yeni hariç iş açıklaması" };
        ExcludedWorks.Add(item);
        SelectedExcludedItem = item;
    }

    private void RemoveExcludedItem()
    {
        if (SelectedExcludedItem is null) return;
        var index = ExcludedWorks.IndexOf(SelectedExcludedItem);
        ExcludedWorks.Remove(SelectedExcludedItem);
        Reindex(ExcludedWorks, (item, order) => item.SortOrder = order);
        SelectedExcludedItem = ExcludedWorks.Count == 0 ? null : ExcludedWorks[Math.Min(index, ExcludedWorks.Count - 1)];
    }

    private void AddOfferItem()
    {
        var item = new OfferItemEditor
        {
            RowNo = OfferItems.Count + 1,
            Description = "Yeni teklif kalemi",
            Quantity = 1m,
            Unit = "adet",
            Currency = Currency
        };
        OfferItems.Add(item);
        SelectedOfferItem = item;
    }

    private void RemoveOfferItem()
    {
        if (SelectedOfferItem is null) return;
        var index = OfferItems.IndexOf(SelectedOfferItem);
        OfferItems.Remove(SelectedOfferItem);
        Reindex(OfferItems, (item, order) => item.RowNo = order);
        SelectedOfferItem = OfferItems.Count == 0 ? null : OfferItems[Math.Min(index, OfferItems.Count - 1)];
    }

    private void MoveOfferItem(int direction)
    {
        if (SelectedOfferItem is null) return;
        var index = OfferItems.IndexOf(SelectedOfferItem);
        var target = index + direction;
        if (index < 0 || target < 0 || target >= OfferItems.Count) return;
        OfferItems.Move(index, target);
        Reindex(OfferItems, (item, order) => item.RowNo = order);
    }

    private void AddPaymentItem()
    {
        var item = new OfferBulletEditor { SortOrder = PaymentItems.Count + 1, Text = "Yeni ödeme koşulu" };
        PaymentItems.Add(item);
        SelectedPaymentItem = item;
    }

    private void RemovePaymentItem()
    {
        if (SelectedPaymentItem is null) return;
        var index = PaymentItems.IndexOf(SelectedPaymentItem);
        PaymentItems.Remove(SelectedPaymentItem);
        Reindex(PaymentItems, (item, order) => item.SortOrder = order);
        SelectedPaymentItem = PaymentItems.Count == 0 ? null : PaymentItems[Math.Min(index, PaymentItems.Count - 1)];
    }

    private void AddImages(string section)
    {
        var paths = _dialogs.SelectImages();
        var target = section == ImageSections.TechnicalSpec ? TechnicalImages : OfferImages;
        foreach (var path in paths.Where(path => File.Exists(path) && SupportedImageExtensions.Contains(Path.GetExtension(path))))
        {
            var item = new OfferImageEditor
            {
                FilePath = path,
                Title = Path.GetFileNameWithoutExtension(path),
                ImageSection = section,
                SortOrder = target.Count + 1,
                PageNumber = target.Count / 2 + 1,
                IncludeInPdf = true,
                FitWithoutCrop = true,
                HasBorder = true
            };
            target.Add(item);
            if (section == ImageSections.TechnicalSpec) SelectedTechnicalImage = item;
            else SelectedOfferImage = item;
        }
        ReindexImages(target);
        OnPropertyChanged(nameof(ImagesSummary));
        StatusMessage = paths.Count == 0 ? StatusMessage : $"{paths.Count} görsel seçildi; desteklenen dosyalar teklif formuna eklendi.";
    }

    private void RemoveImage(OfferImageEditor? image)
    {
        if (image is null) return;
        var target = image.ImageSection == ImageSections.TechnicalSpec ? TechnicalImages : OfferImages;
        target.Remove(image);
        ReindexImages(target);
        if (image.ImageSection == ImageSections.TechnicalSpec) SelectedTechnicalImage = target.FirstOrDefault();
        else SelectedOfferImage = target.FirstOrDefault();
        OnPropertyChanged(nameof(ImagesSummary));
    }

    private void MoveImage(string section, int direction)
    {
        var selected = section == ImageSections.TechnicalSpec ? SelectedTechnicalImage : SelectedOfferImage;
        if (selected is null) return;
        var target = section == ImageSections.TechnicalSpec ? TechnicalImages : OfferImages;
        var index = target.IndexOf(selected);
        var destination = index + direction;
        if (index < 0 || destination < 0 || destination >= target.Count) return;
        target.Move(index, destination);
        ReindexImages(target);
    }

    private void SelectLogo()
    {
        var path = _dialogs.SelectImages().FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(path))
        {
            Company.HeaderLogoPath = path;
            OnPropertyChanged(nameof(Company));
            StatusMessage = "Teklif üst logosu değiştirildi.";
        }
    }

    private void AddTechnicalSection()
    {
        var section = new TechnicalSpecSectionEditor
        {
            SortOrder = TechnicalSections.Count + 1,
            Title = "YENİ TEKNİK ŞARTNAME BÖLÜMÜ",
            Content = "Başlık\tDeğer"
        };
        TechnicalSections.Add(section);
        SelectedTechnicalSection = section;
    }

    private void RemoveTechnicalSection()
    {
        if (SelectedTechnicalSection is null) return;
        var index = TechnicalSections.IndexOf(SelectedTechnicalSection);
        TechnicalSections.Remove(SelectedTechnicalSection);
        Reindex(TechnicalSections, (item, order) => item.SortOrder = order);
        SelectedTechnicalSection = TechnicalSections.Count == 0 ? null : TechnicalSections[Math.Min(index, TechnicalSections.Count - 1)];
    }

    private async Task ExportPdfAsync()
    {
        EnsureLoaded();
        var raw = BuildDocument();
        var errors = _validator.Validate(raw);
        if (errors.Count != 0)
        {
            StatusMessage = string.Join(" ", errors);
            Owner.SetStatusMessage(StatusMessage);
            return;
        }
        var code = SelectedLanguage == OfferLanguage.English ? "EN" : "TR";
        var path = _dialogs.SavePdf($"ZMT-{Info.ReferenceNumber}-Teklif-{code}");
        if (path is null) return;
        await ExportToPathAsync(path, OpenAfterExport);
    }

    private async Task PreviewPdfAsync()
    {
        EnsureLoaded();
        var path = Path.Combine(Path.GetTempPath(), $"ZMT-Teklif-Onizleme-{Guid.NewGuid():N}.pdf");
        await ExportToPathAsync(path, true);
    }

    private async Task ExportToPathAsync(string path, bool openAfterExport)
    {
        try
        {
            var raw = BuildDocument();
            var errors = _validator.Validate(raw);
            if (errors.Count != 0)
            {
                StatusMessage = string.Join(" ", errors);
                Owner.SetStatusMessage(StatusMessage);
                return;
            }
            var localized = OfferDocumentLocalizer.Localize(raw, SelectedLanguage);
            await _pdf.ExportAsync(localized, new PdfExportOptions
            {
                OutputPath = path,
                Language = SelectedLanguage,
                DocumentTitle = localized.Info.MainTitle,
                IncludeImages = IncludeImages,
                IncludeTechnicalSpecification = IncludeTechnicalSpecification,
                OpenAfterExport = openAfterExport
            });
            LastGeneratedPdfPath = path;
            StatusMessage = $"Teklif PDF'i oluşturuldu: {Path.GetFileName(path)}";
            Owner.SetStatusMessage(StatusMessage);
            _logger.Information("DetailedOfferPdfExported", "Ayrıntılı teklif PDF'i oluşturuldu.", new
            {
                Owner.ActiveProject.Id,
                File = Path.GetFileName(path),
                Language = SelectedLanguage.ToString(),
                OfferImages = OfferImages.Count,
                TechnicalImages = TechnicalImages.Count
            });
        }
        catch (Exception exception)
        {
            StatusMessage = "Teklif PDF'i oluşturulamadı. Ayrıntılar günlük dosyasına yazıldı.";
            Owner.SetStatusMessage(StatusMessage);
            _logger.Error("DetailedOfferPdfExportFailed", exception, new { Owner.ActiveProject.Id });
        }
    }

    private OfferDocument BuildDocument()
    {
        var document = new OfferDocument
        {
            Info = Info,
            CompanySettings = Company,
            OfferTitle = OfferTitle,
            OfferNotes = OfferNotes,
            DeliveryText = DeliveryText,
            ScopeTotal = ScopeTotal,
            DiscountRate = DiscountPercent / 100m,
            DiscountAmount = DiscountAmount,
            Subtotal = Subtotal,
            VatRate = VatPercent / 100m,
            VatAmount = VatAmount,
            GrandTotal = GrandTotal,
            Currency = Currency,
            IncludedWorkGroups = IncludedGroups.Select(item => item.ToModel()).ToList(),
            ExcludedWorks = ExcludedWorks.Select(item => item.ToModel()).ToList(),
            OfferItems = OfferItems.Select(item => item.ToModel()).ToList(),
            PaymentItems = PaymentItems.Select(item => item.ToModel()).ToList(),
            Images = OfferImages.Concat(TechnicalImages).Select(item => item.ToModel()).ToList(),
            TechnicalSpecification = new TechnicalSpecification
            {
                Title = TechnicalSpecificationTitle,
                IncludeInPdf = IncludeTechnicalSpecification,
                Sections = TechnicalSections.Select(item => item.ToModel()).ToList()
            }
        };
        return document;
    }

    private void OpenLastPdf()
    {
        if (!string.IsNullOrWhiteSpace(LastGeneratedPdfPath) && File.Exists(LastGeneratedPdfPath))
        {
            Process.Start(new ProcessStartInfo(LastGeneratedPdfPath) { UseShellExecute = true });
        }
        else
        {
            StatusMessage = "Önce bir teklif PDF'i oluşturun veya önizleyin.";
        }
    }

    private void OfferItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
        {
            foreach (OfferItemEditor item in e.NewItems) item.PropertyChanged += OfferItemPropertyChanged;
        }
        if (e.OldItems is not null)
        {
            foreach (OfferItemEditor item in e.OldItems) item.PropertyChanged -= OfferItemPropertyChanged;
        }
        RefreshTotals();
    }

    private void OfferItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(OfferItemEditor.Quantity) or nameof(OfferItemEditor.UnitPrice) or nameof(OfferItemEditor.Total))
        {
            RefreshTotals();
        }
    }

    private void RefreshTotals()
    {
        OnPropertyChanged(nameof(ScopeTotal));
        OnPropertyChanged(nameof(DiscountAmount));
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(VatAmount));
        OnPropertyChanged(nameof(GrandTotal));
    }

    private static void ReindexImages(ObservableCollection<OfferImageEditor> images)
    {
        for (var index = 0; index < images.Count; index++)
        {
            images[index].SortOrder = index + 1;
            if (images[index].PageNumber <= 0) images[index].PageNumber = index / 2 + 1;
        }
    }

    private static void Reindex<T>(IEnumerable<T> values, Action<T, int> assign)
    {
        var order = 1;
        foreach (var value in values) assign(value, order++);
    }

    private static string? EnsureBundledLogoFile()
    {
        try
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZMT", "SteelCost", "Assets");
            Directory.CreateDirectory(directory);
            var target = Path.Combine(directory, "logo.png");
            if (File.Exists(target)) return target;
            var resource = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Assets/logo.png"));
            if (resource is null) return null;
            using (resource.Stream)
            using (var output = File.Create(target))
            {
                resource.Stream.CopyTo(output);
            }
            return target;
        }
        catch
        {
            return null;
        }
    }
}

public sealed class OfferScopeGroupEditor : ObservableObject
{
    private int _sortOrder;
    private string _title = string.Empty;
    private bool _isVisible = true;
    public int SortOrder { get => _sortOrder; set => SetProperty(ref _sortOrder, value); }
    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public bool IsVisible { get => _isVisible; set => SetProperty(ref _isVisible, value); }
    public ObservableCollection<OfferBulletEditor> Items { get; } = [];

    public static OfferScopeGroupEditor FromModel(OfferSectionGroup source)
    {
        var result = new OfferScopeGroupEditor { SortOrder = source.SortOrder, Title = source.Title, IsVisible = source.IsVisible };
        result.Items.ReplaceWith(source.Items.OrderBy(item => item.SortOrder).Select(OfferBulletEditor.FromModel));
        return result;
    }

    public OfferSectionGroup ToModel() => new()
    {
        SortOrder = SortOrder,
        Title = Title,
        IsVisible = IsVisible,
        Items = Items.Select(item => item.ToModel()).ToList()
    };
}

public sealed class OfferBulletEditor : ObservableObject
{
    private int _sortOrder;
    private string _text = string.Empty;
    private bool _isIncludedInPdf = true;
    public int SortOrder { get => _sortOrder; set => SetProperty(ref _sortOrder, value); }
    public string Text { get => _text; set => SetProperty(ref _text, value); }
    public bool IsIncludedInPdf { get => _isIncludedInPdf; set => SetProperty(ref _isIncludedInPdf, value); }
    public static OfferBulletEditor FromModel(OfferBulletItem source) => new() { SortOrder = source.SortOrder, Text = source.Text, IsIncludedInPdf = source.IsIncludedInPdf };
    public OfferBulletItem ToModel() => new() { SortOrder = SortOrder, Text = Text, IsIncludedInPdf = IsIncludedInPdf };
}

public sealed class OfferItemEditor : ObservableObject
{
    private int _rowNo;
    private string _description = string.Empty;
    private decimal _quantity;
    private string _unit = string.Empty;
    private decimal _unitPrice;
    private string _currency = "TRY";
    public int RowNo { get => _rowNo; set => SetProperty(ref _rowNo, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }
    public decimal Quantity { get => _quantity; set { if (SetProperty(ref _quantity, value)) OnPropertyChanged(nameof(Total)); } }
    public string Unit { get => _unit; set => SetProperty(ref _unit, value); }
    public decimal UnitPrice { get => _unitPrice; set { if (SetProperty(ref _unitPrice, value)) OnPropertyChanged(nameof(Total)); } }
    public string Currency { get => _currency; set => SetProperty(ref _currency, value); }
    public decimal Total => Quantity * UnitPrice;
    public static OfferItemEditor FromModel(OfferItem source) => new() { RowNo = source.RowNo, Description = source.Description, Quantity = source.Quantity, Unit = source.Unit, UnitPrice = source.UnitPrice, Currency = source.Currency };
    public OfferItem ToModel() => new() { RowNo = RowNo, Description = Description, Quantity = Quantity, Unit = Unit, UnitPrice = UnitPrice, Currency = Currency };
}

public sealed class OfferImageEditor : ObservableObject
{
    private string _filePath = string.Empty;
    private string _title = string.Empty;
    private string _description = string.Empty;
    private string _imageSection = ImageSections.Offer;
    private int _pageNumber = 1;
    private int _sortOrder;
    private bool _includeInPdf = true;
    private bool _hasBorder = true;
    private bool _fitWithoutCrop = true;
    public string FilePath { get => _filePath; set => SetProperty(ref _filePath, value); }
    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }
    public string ImageSection { get => _imageSection; set => SetProperty(ref _imageSection, value); }
    public int PageNumber { get => _pageNumber; set => SetProperty(ref _pageNumber, value); }
    public int SortOrder { get => _sortOrder; set => SetProperty(ref _sortOrder, value); }
    public bool IncludeInPdf { get => _includeInPdf; set => SetProperty(ref _includeInPdf, value); }
    public bool HasBorder { get => _hasBorder; set => SetProperty(ref _hasBorder, value); }
    public bool FitWithoutCrop { get => _fitWithoutCrop; set => SetProperty(ref _fitWithoutCrop, value); }
    public OfferImage ToModel() => new()
    {
        FilePath = FilePath,
        Title = Title,
        Description = Description,
        ImageSection = ImageSection,
        PageNumber = PageNumber,
        SortOrder = SortOrder,
        IncludeInPdf = IncludeInPdf,
        HasBorder = HasBorder,
        FitWithoutCrop = FitWithoutCrop
    };
}

public sealed class TechnicalSpecSectionEditor : ObservableObject
{
    private int _sortOrder;
    private string _title = string.Empty;
    private string _content = string.Empty;
    private bool _includeInPdf = true;
    public int SortOrder { get => _sortOrder; set => SetProperty(ref _sortOrder, value); }
    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string Content { get => _content; set => SetProperty(ref _content, value); }
    public bool IncludeInPdf { get => _includeInPdf; set => SetProperty(ref _includeInPdf, value); }
    public static TechnicalSpecSectionEditor FromModel(TechnicalSpecSection source) => new() { SortOrder = source.SortOrder, Title = source.Title, Content = source.Content, IncludeInPdf = source.IncludeInPdf };
    public TechnicalSpecSection ToModel() => new() { SortOrder = SortOrder, Title = Title, Content = Content, IncludeInPdf = IncludeInPdf };
}

internal static class ObservableCollectionExtensions
{
    public static void ReplaceWith<T>(this ObservableCollection<T> target, IEnumerable<T> values)
    {
        target.Clear();
        foreach (var value in values) target.Add(value);
    }
}
