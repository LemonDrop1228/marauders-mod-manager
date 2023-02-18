using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using MaraudersModManager.Extensions;
using MaraudersModManager.FileSystem;
using MaraudersModManager.Settings;
using MaraudersModManager.Steam;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using static MaraudersModManager.Constants.AppConstants;

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
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<ISettingsManagerService, SettingsManagerService>()
            .AddSingleton<IFileSystemService, FileSystemService>()
            .AddSingleton<ISteamService, SteamService>()
            .AddSingleton<MainWindow>();    
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var mainWindow = host.Services.GetService<MainWindow>();
        
        var settingService = host.Services.GetService<ISettingsManagerService>();
        var steamService = host.Services.GetService<ISteamService>();


        if (!settingService.IsInitialized)
        {
            var steamPath = (Registry.GetValue(SteamRegistryKey, SteamInstallPathRegistryValue, string.Empty) as string).Replace("/", "\\\\");
        
            if (steamPath.HasContent() && Directory.Exists(steamPath))
            {
                steamService.Initialize(steamPath);
            
                if (Directory.Exists(Path.Combine(steamPath, LibraryConfigFileRoot)) &&
                    File.Exists(Path.Combine(steamPath, LibraryConfigFileRoot, LibraryConfigFileName)))
                {
                    string gameRootPath = steamService.GetGameRootPath();

                    if (gameRootPath.HasContent() && Directory.Exists(gameRootPath))
                    {
                        settingService.Initialize(steamPath, gameRootPath);
                        settingService.Update().Save();
#if DEBUG
                        Debug.WriteLine($"Steam Path: {steamPath}");
                        Debug.WriteLine($"Game Root Path: {gameRootPath}");
#endif
                    }
                }
            }
        }


        // host.Services.GetService<IViewService>().InitializeViews(
        //     host.Services.GetServices<IBaseView>()
        // );

        mainWindow.Closed += (s, e) => {
            Debug.WriteLine("App shutting down");
            ShutItDown();
        };

        mainWindow.Show();
    }

    private void ShutItDown()
    {
        using (host)
        {
            host.StopAsync();
        }
        Current.Shutdown();
    }
    
    
}
