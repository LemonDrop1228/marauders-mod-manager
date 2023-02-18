using System.Collections.Generic;
using MaraudersModManager.Extensions;
using MaraudersModManager.FileSystem;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SharpYaml.Model;

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
}

public class SettingsManagerService : ISettingsManagerService
{
    private readonly IFileSystemService _fileSystemService;
    private SteamOptions Options { get; set; }
    private readonly IConfiguration _configuration;
    private IFileInfoEx File { get; set; }

    public SettingsManagerService(IConfiguration configuration, IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
        Options = configuration.GetSection("SteamOptions").Get<SteamOptions>();
        _configuration = configuration;
        this.Load();
    }
    
    public bool IsInitialized => Options.SteamInstallPath.HasContent() && Options.GameInstallPath.HasContent();
    
    public void Initialize(string steamPath, string gamePath)
    {
        Options.SteamInstallPath = steamPath;
        Options.GameInstallPath = gamePath;
    }
    
    public void Load() => File = new FileInfoEx(_fileSystemService.GetRootedFilePath("appsettings.json"), _fileSystemService);

    public ISettingsManagerService Save()
    {
        File.Write();
        return this;
    }

    public ISettingsManagerService Update()
    {
        File.SetContent( new { SteamOptions = Options }.ToJson());
        return this;
    }

    public ISettingsManagerService SetSteamInstallPath(string path)
    {
        Options.SteamInstallPath = path;
        return this;
    }
    
    public ISettingsManagerService SetGameInstallPath(string path)
    {
        Options.GameInstallPath = path;
        return this;
    }
}