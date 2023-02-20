using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using MaraudersModManager.Extensions;
using MaraudersModManager.FileSystem;
using MaraudersModManager.Settings;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using static MaraudersModManager.Constants.AppConstants;

namespace MaraudersModManager.Steam;

public class SteamService : ISteamService
{
    private readonly IFileSystemService _fileSystemService;
    private readonly ISettingsManagerService _settingService;
    private string SteamInstallationPath { get; set; }

    public SteamService(IFileSystemService fileSystemService, ISettingsManagerService settingService)
    {
        _fileSystemService = fileSystemService;
        _settingService = settingService;
    }
    
    public void Initialize()
    {
        if (!_settingService.IsInitialized)
        {
            var steamPath = (Registry.GetValue(SteamRegistryKey, SteamInstallPathRegistryValue, string.Empty) as string).Replace("/", "\\");
        
            if (steamPath.HasContent() && Directory.Exists(steamPath))
            {
                SteamInstallationPath = steamPath;
            
                if (Directory.Exists(Path.Combine(steamPath, LibraryConfigFileRoot)) &&
                    File.Exists(Path.Combine(steamPath, LibraryConfigFileRoot, LibraryConfigFileName)))
                {
                    string gameRootPath = GetGameRootPath();

                    if (gameRootPath.HasContent() && Directory.Exists(gameRootPath))
                    {
                        _settingService.Initialize(steamPath, gameRootPath);
                        _settingService.Save();
#if DEBUG
                        Debug.WriteLine($"Steam Path: {steamPath}");
                        Debug.WriteLine($"Game Root Path: {gameRootPath}");
#endif
                    }
                }
            }
        }
    }

    public string GetGameRootPath()
    {
        var libraryConfigPath = Path.Combine(SteamInstallationPath, LibraryConfigFileRoot, LibraryConfigFileName);
        var libraryConfigJson = $"{{{VdfConvert.Deserialize(File.ReadAllText(libraryConfigPath)).ToJson()}}}";
        return SearchLibraries(libraryConfigJson);
    }
    
    private string SearchLibraries(string libraryConfigJson)
    {
        var tree = (JContainer)JToken.Parse(libraryConfigJson);
        var libraryFolders = tree.DescendantsAndSelf().OfType<JProperty>().Where(p => p.Name == "path").Values().Select(t => t.ToString()).ToList();
        foreach (string directory in libraryFolders)
        {
            var steamAppsRoot = Path.Combine(directory, SteamAppsRoot, SteamAppsCommonRoot);
            var results = Directory.EnumerateDirectories(steamAppsRoot,"Hogwarts Legacy", SearchOption.TopDirectoryOnly).ToList();
            if(results != null && results.Any())
                return results.First();
        }
        
        return string.Empty;
    }
}