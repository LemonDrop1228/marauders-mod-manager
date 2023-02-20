using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using MaraudersModManager.Controls.Base.View;
using MaraudersModManager.Extensions;
using MaraudersModManager.FileSystem;
using MaraudersModManager.Settings;
using MaraudersModManager.Steam;
using MaraudersModManager.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MaraudersModManager;

public partial class App : Application
{
    private readonly IHost host;
    private readonly IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .Build();
    
    public App()
    {
        host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services)).Build();
    }
    
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton(configuration)
            .AddSingleton<ISettingsManagerService, SettingsManagerService>()
            .AddSingleton<IFileSystemService, FileSystemService>()
            .AddSingleton<ISteamService, SteamService>()
            .AddSingleton<IViewControllerService, ViewControllerService>()
            .AddSingleton<MainWindow>(); 
        
        Assembly.GetEntryAssembly().GetTypesAssignableFrom<IBaseView, BaseView>().ForEach((t)=>
        {
            serviceCollection.AddSingleton(typeof(IBaseView), t);
        });
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var mainWindow = host.Services.GetService<MainWindow>();
        
        var steamService = host.Services.GetService<ISteamService>();
        
        steamService.Initialize();

        mainWindow.Closed += (s, e) => {
            Debug.WriteLine("App shutting down");
            ShutItDown();
        };

        mainWindow.Show();
    }

    private void ShutItDown()
    { using (host)
        {
            host.StopAsync();
        }
        Current.Shutdown();
    }
    
    
}
