using MaraudersModManager.Extensions;
using Newtonsoft.Json;
using PropertyChanged;

namespace MaraudersModManager.Settings;

[AddINotifyPropertyChangedInterface]
public class MarauderOptions
{
    public string ModInstallPath { get; set; }
    [JsonIgnore]
    public bool IsModInstallPathSet => ModInstallPath.HasContent();
    public bool FirstRun { get; set; }
}