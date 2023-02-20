using System.Windows.Controls;
using MaraudersModManager.Enums;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MaraudersModManager.Controls.Base.View;

public interface IBaseView
{
    void CloseView();
    ViewMenuData ViewMenuData { get; set; }
}

[AddINotifyPropertyChangedInterface]
public abstract class BaseView : UserControl, IBaseView
{
    public virtual void CloseView()
    {

    }

    public abstract ViewMenuData ViewMenuData { get; set; }
}

public record ViewMenuData
{
    public int ViewIndex { get; set; }
    public string ViewLabel { get; set; }
    public PackIconKind ViewIcon { get; set; }
    public ViewTypes ViewType { get; set; }
}

