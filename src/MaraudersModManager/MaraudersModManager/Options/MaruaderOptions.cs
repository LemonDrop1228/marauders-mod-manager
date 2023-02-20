using MaraudersModManager.Extensions;
using Newtonsoft.Json;

namespace MaraudersModManager.Settings;

public class MaruaderOptions
{
    public string ModInstallPath { get; set; }
    [JsonIgnore]
    public bool IsModInstallPathSet => ModInstallPath.HasContent();
    public bool FirstRun { get; set; }
}