using MaraudersModManager.Extensions;
using MaraudersModManager.FileSystem;
using Microsoft.Extensions.Configuration;
using PropertyChanged;

namespace MaraudersModManager.Settings;

[AddINotifyPropertyChangedInterface]
public class SettingsManagerService : ISettingsManagerService
{
    private readonly IFileSystemService _fileSystemService;
    public AppSettings Settings { get; set; }

    private readonly IConfiguration _configuration;
    private IFileInfoEx File { get; set; }

    public SettingsManagerService(IConfiguration configuration, IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
        Settings = new AppSettings(configuration.GetSection("SteamOptions").Get<SteamOptions>(), configuration.GetSection("MarauderOptions").Get<MarauderOptions>());
        _configuration = configuration;
        this.Load();
    }
    
    public bool IsInitialized => Settings.SteamOptions.SteamInstallPath.HasContent() && Settings.SteamOptions.GameInstallPath.HasContent();
    
    public void Initialize(string steamPath, string gamePath)
    {
        Settings.SteamOptions.SteamInstallPath = steamPath;
        Settings.SteamOptions.GameInstallPath = gamePath;
    }
    
    public void Load() => File = new FileInfoEx(_fileSystemService.GetRootedFilePath("appsettings.json"), _fileSystemService);

    public ISettingsManagerService Save()
    {
        Update();
        File.Write();
        return this;
    }

    public ISettingsManagerService Update()
    {
        File.SetContent(Settings.ToJson());
        return this;
    }

    public ISettingsManagerService SetSteamInstallPath(string path)
    {
        Settings.SteamOptions.SteamInstallPath = path;
        return this;
    }
    
    public ISettingsManagerService SetGameInstallPath(string path)
    {
        Settings.SteamOptions.GameInstallPath = path;
        return this;
    }
}