using PropertyChanged;

namespace MaraudersModManager.Settings;

[AddINotifyPropertyChangedInterface]
public class AppSettings
{
    public AppSettings(SteamOptions steamOptions, MarauderOptions marauderOptions)
    {
        SteamOptions = steamOptions;
        MarauderOptions = marauderOptions;
    }

    public SteamOptions SteamOptions { get; set; }
    public MarauderOptions MarauderOptions { get; set; }
}