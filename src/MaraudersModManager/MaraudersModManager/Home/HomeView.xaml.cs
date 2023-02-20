using MaraudersModManager.Controls.Base.View;
using MaraudersModManager.Enums;
using MaterialDesignThemes.Wpf;

namespace MaraudersModManager.Home;

public partial class HomeView : BaseView
{
    public HomeView()
    {
        ViewMenuData = new ViewMenuData() { ViewIndex = 0, ViewLabel = "Home", ViewIcon = PackIconKind.Castle, ViewType = ViewTypes.ToolViews };
        InitializeComponent();
    }

    public override ViewMenuData ViewMenuData { get; set; }
}