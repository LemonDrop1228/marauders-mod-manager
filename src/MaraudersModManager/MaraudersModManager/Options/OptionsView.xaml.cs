using System.Windows;
using MaraudersModManager.Controls.Base.View;
using MaraudersModManager.Enums;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MaraudersModManager.Settings;

[AddINotifyPropertyChangedInterface]
public partial class OptionsView : BaseView
{
    private readonly ISettingsManagerService _settingsManagerService;
    
    public AppSettings Settings => _settingsManagerService.Settings;

    public OptionsView(ISettingsManagerService settingsManagerService)
    {
        _settingsManagerService = settingsManagerService;
        this.DataContext = Settings;
        ViewMenuData = new ViewMenuData() { ViewIndex = 1, ViewLabel = "Settings", ViewIcon = PackIconKind.Settings, ViewType = ViewTypes.ToolViews };
        InitializeComponent();
    }

    public override ViewMenuData ViewMenuData { get; set; }

    private void SaveSettingsClicked(object sender, RoutedEventArgs e) => _settingsManagerService.Save();
}