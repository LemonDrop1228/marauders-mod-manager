using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaraudersModManager.Commands;
using MaraudersModManager.Controls.Base.View;
using MaraudersModManager.Settings;
using MaraudersModManager.UI;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Hosting;
using PropertyChanged;

namespace MaraudersModManager;

[AddINotifyPropertyChangedInterface]
public partial class MainWindow : Window
{
    private readonly ISettingsManagerService _settingsManagerService;
    private readonly IViewControllerService _controller;

    public IBaseView ActiveView { get => _controller.CurrentView;}
    public IEnumerable<ViewBucket> DefaultViews { get => _controller.GetToolViews().Select(v => new ViewBucket(v));}
    public string MagicTitle => $"You're a Wizard {Environment.UserName}";
    public bool ShowApp { get; set; }
    
    public ICommand LogoFadeOutComplete => new RelayCommand(o =>
    {
        Logo.IsHitTestVisible = false;
        HostGrid.IsHitTestVisible = true;
        ShowApp = true;
    }, o => true);
    
    public ICommand ChangeView => new RelayCommand(o =>
    {
        _controller.SetView((o as ViewBucket).ViewRef);
        HostElement.GetBindingExpression(Page.ContentProperty).UpdateTarget();
    }, o => true);
    
    public MainWindow(ISettingsManagerService settingsManagerService, IViewControllerService controller)
    {
        this.DataContext = this;
        _settingsManagerService = settingsManagerService;
        _controller = controller;
        InitializeComponent();
        _controller.StartAtHome();

#if DEBUG
        this.Topmost = true;
#endif
    }

    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if(e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            DragMove();
    }

    private void MinimizeClicked(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

    private void MaximizeClicked(object sender, RoutedEventArgs e) =>
        this.WindowState = this.WindowState switch
        {
            WindowState.Normal => WindowState.Maximized,
            WindowState.Maximized => WindowState.Normal
        };

    private void CloseWindowClicked(object sender, RoutedEventArgs e) => this.Close();
}
