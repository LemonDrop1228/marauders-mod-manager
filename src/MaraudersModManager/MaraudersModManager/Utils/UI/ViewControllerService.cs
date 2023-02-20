using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MaraudersModManager.Controls.Base.View;
using MaraudersModManager.Enums;
using MaraudersModManager.Extensions;
using PropertyChanged;

namespace MaraudersModManager.UI;

public interface IViewControllerService
{
    IBaseView CurrentView { get; }
    void InitializeViews(IEnumerable<IBaseView> views);
    void SetView(IBaseView view);
    void CloseViews();
    void StartAtHome();
    IEnumerable<IBaseView> GetToolViews();
    IEnumerable<IBaseView> GetDialogViews();
}

[AddINotifyPropertyChangedInterface]
public class ViewControllerService : IViewControllerService
{
    public ViewControllerService(IEnumerable<IBaseView> views)
    {
        InitializeViews(views);
    }

    public IBaseView CurrentView { get; private set; }
    ObservableCollection<IBaseView> ViewCollection { get; set; }

    public void InitializeViews(IEnumerable<IBaseView> views)
    {
        ViewCollection = new ObservableCollection<IBaseView>();
        ViewCollection.AddRange(views.OrderBy(v => v.ViewMenuData.ViewIndex).ToArray());
    }

    public void SetView(IBaseView view) => CurrentView = view;

    public void CloseViews() => ViewCollection.ForEach(v => v.CloseView());

    public IEnumerable<IBaseView> GetToolViews() => ViewCollection.Where(v => v.ViewMenuData.ViewType == ViewTypes.ToolViews);

    public IEnumerable<IBaseView> GetDialogViews() => ViewCollection.Where(v => v.ViewMenuData.ViewType == ViewTypes.DialogViews);
    
    public void StartAtHome() => CurrentView = ViewCollection.FirstOrDefault();
}