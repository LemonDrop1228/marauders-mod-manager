using System;
using System.Collections.Generic;
using PropertyChanged;

namespace MaraudersModManager.Settings;

[AddINotifyPropertyChangedInterface]
public class SteamOptions
{
    public string SteamInstallPath { get; set; }
    public string GameInstallPath { get; set; }
}