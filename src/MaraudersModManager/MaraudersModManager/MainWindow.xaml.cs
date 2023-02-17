using System;
using System.Windows;
using System.Windows.Input;
using MaraudersModManager.Commands;
using PropertyChanged;

namespace MaraudersModManager;

[AddINotifyPropertyChangedInterface]
public partial class MainWindow : Window
{
    public string MagicTitle => $"You're a Wizard {Environment.UserName}";
    public bool ShowApp { get; set; }
    
    public ICommand LogoFadeOutComplete => new RelayCommand(o =>
    {
        Logo.IsHitTestVisible = false;
        ShowApp = true;
    }, o => true);
    
    public MainWindow()
    {
        this.DataContext = this;
        InitializeComponent();
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
