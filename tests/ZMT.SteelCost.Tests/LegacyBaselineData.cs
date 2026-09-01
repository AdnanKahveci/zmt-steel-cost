using System.Globalization;
using System.Text.Json;

namespace ZMT.SteelCost.Tests;

internal sealed class LegacyBaselineData : IDisposable
{
    private readonly JsonDocument _document;

    private LegacyBaselineData(JsonDocument document) => _document = document;

    public JsonElement Root => _document.RootElement;
    public JsonElement Materials => Root.GetProperty("materials");
    public JsonElement CategoryTotals => Root.GetProperty("categoryTotals");
    public JsonElement Totals => Root.GetProperty("totals");

    public static LegacyBaselineData Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "TestData", "LegacyBaseline.json");
        return new(JsonDocument.Parse(File.ReadAllText(path)));
    }

    public static decimal Decimal(JsonElement parent, string property)
    {
        var value = parent.GetProperty(property);
        return value.ValueKind switch
        {
            JsonValueKind.Number => value.GetDecimal(),
            JsonValueKind.String => decimal.Parse(value.GetString()!, NumberStyles.Float, CultureInfo.InvariantCulture),
            JsonValueKind.Null => 0m,
            _ => throw new InvalidDataException($"{property} sayısal değil: {value.ValueKind}")
        };
    }

    public void Dispose() => _document.Dispose();
}
