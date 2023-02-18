using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using MaraudersModManager.FileSystem;
using Newtonsoft.Json.Linq;
using static MaraudersModManager.Constants.AppConstants;

namespace MaraudersModManager.Steam;

public interface ISteamService
{
    string GetGameRootPath();
    void Initialize(string path);
}

public class SteamService : ISteamService
{
    private readonly IFileSystemService _fileSystemService;
    private string SteamInstallationPath { get; set; }

    public SteamService(IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
    }
    
    public void Initialize(string path)
    {
        SteamInstallationPath = path;
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