namespace MaraudersModManager.Settings;

public class AppSettings
{
    public AppSettings(SteamOptions steamOptions, MaruaderOptions maruaderOptions)
    {
        SteamOptions = steamOptions;
        MaruaderOptions = maruaderOptions;
    }

    public SteamOptions SteamOptions { get; set; }
    public MaruaderOptions MaruaderOptions { get; set; }
}