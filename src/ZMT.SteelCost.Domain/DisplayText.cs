namespace ZMT.SteelCost.Domain;

public static class DisplayText
{
    public static string ToTurkish(this RoofType value) => value switch
    {
        RoofType.Hip => "Kırma",
        RoofType.Gable => "Beşik",
        RoofType.Parapet => "Parapet",
        RoofType.MonoPitch => "Tek Eğim",
        _ => value.ToString()
    };

    public static string ToTurkish(this RoofSystem value) => value switch
    {
        RoofSystem.PurlinOmega => "Aşık Omega",
        RoofSystem.Panel => "Panel Sistem",
        _ => value.ToString()
    };

    public static string ToTurkish(this ResponsibilityType value) => value == ResponsibilityType.Zmt
        ? "ZMT'ye Ait"
        : "Müşteriye Ait";
}
