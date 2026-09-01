using ZMT.SteelCost.App.Mvvm;
using ZMT.SteelCost.Domain;

namespace ZMT.SteelCost.App.ViewModels;

public sealed class ResultLineViewModel : ObservableObject
{
    private QuantityMode _mode;
    private decimal? _overrideQuantity;
    private string _overrideReason = string.Empty;

    public ResultLineViewModel(string categoryName, CalculationLine line)
    {
        CategoryName = categoryName;
        Line = line;
        _mode = line.QuantityMode;
        _overrideQuantity = line.QuantityMode == QuantityMode.Manual ? line.EffectiveQuantity : null;
        _overrideReason = line.OverrideReason ?? string.Empty;
    }

    public string CategoryName { get; }
    public CalculationLine Line { get; }
    public string MaterialCode => Line.MaterialCode;
    public string MaterialName => Line.MaterialName;
    public string? Specification => Line.Specification;
    public string Unit => Line.Unit;
    public decimal CalculatedQuantity => Line.CalculatedQuantity;
    public decimal EffectiveQuantity => Mode == QuantityMode.Manual && OverrideQuantity.HasValue ? OverrideQuantity.Value : Line.CalculatedQuantity;
    public decimal PurchaseUnitPriceExVat => Line.PurchaseUnitPriceExVat;
    public decimal PurchaseTotalExVat => EffectiveQuantity * PurchaseUnitPriceExVat;
    public decimal SalesUnitPrice => Line.SalesUnitPrice;
    public decimal SalesTotal => EffectiveQuantity * SalesUnitPrice;
    public decimal GrossMarginRate => Line.GrossMarginRate;
    public CalculationTrace Trace => Line.Trace;
    public string OverrideIndicator => Mode == QuantityMode.Manual ? "⚠" : string.Empty;

    public QuantityMode Mode
    {
        get => _mode;
        set
        {
            if (SetProperty(ref _mode, value))
            {
                OnPropertyChanged(nameof(EffectiveQuantity));
                OnPropertyChanged(nameof(PurchaseTotalExVat));
                OnPropertyChanged(nameof(SalesTotal));
                OnPropertyChanged(nameof(OverrideIndicator));
            }
        }
    }

    public decimal? OverrideQuantity
    {
        get => _overrideQuantity;
        set
        {
            if (SetProperty(ref _overrideQuantity, value))
            {
                OnPropertyChanged(nameof(EffectiveQuantity));
                OnPropertyChanged(nameof(PurchaseTotalExVat));
                OnPropertyChanged(nameof(SalesTotal));
            }
        }
    }

    public string OverrideReason
    {
        get => _overrideReason;
        set => SetProperty(ref _overrideReason, value);
    }
}

public sealed class CategoryScopeViewModel : ObservableObject
{
    private ResponsibilityType _responsibility;
    public CategoryScopeViewModel(int id, string name, ResponsibilityType responsibility)
    {
        CategoryId = id;
        CategoryName = name;
        _responsibility = responsibility;
    }
    public int CategoryId { get; }
    public string CategoryName { get; }
    public ResponsibilityType Responsibility { get => _responsibility; set => SetProperty(ref _responsibility, value); }
}

public sealed class MaterialPriceRowViewModel : ObservableObject
{
    private decimal _purchasePrice;
    private bool _isActive = true;

    public MaterialPriceRowViewModel(string code, int categoryId, string categoryName, string name, string? specification, string unit, decimal purchasePrice, string pricingRuleId, bool isActive)
    {
        Code = code;
        CategoryId = categoryId;
        CategoryName = categoryName;
        Name = name;
        Specification = specification;
        Unit = unit;
        _purchasePrice = purchasePrice;
        PricingRuleId = pricingRuleId;
        _isActive = isActive;
    }

    public string Code { get; }
    public int CategoryId { get; }
    public string CategoryName { get; }
    public string Name { get; }
    public string? Specification { get; }
    public string Unit { get; }
    public string PricingRuleId { get; }
    public decimal PurchasePrice { get => _purchasePrice; set => SetProperty(ref _purchasePrice, value); }
    public bool IsActive { get => _isActive; set => SetProperty(ref _isActive, value); }
}
