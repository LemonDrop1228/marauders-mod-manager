namespace MaraudersModManager.Settings;

public interface ISettingsManagerService
{
    ISettingsManagerService Save();
    ISettingsManagerService Update();
    ISettingsManagerService SetSteamInstallPath(string path);
    ISettingsManagerService SetGameInstallPath(string path);
    void Load();
    void Initialize(string steamPath, string gamePath);
    bool IsInitialized { get; }
    AppSettings Settings { get; set; }
}