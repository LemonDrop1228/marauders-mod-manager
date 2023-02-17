using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MaraudersModManager;

public partial class App : Application
{
    private readonly IHost host;

    public App()
    {
        host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services)).Build();
    }

    // private static string Env { get; set; } = Environment.GetEnvironmentVariable(AppConstants.EnvironmentKey) ?? "local";
    
    private readonly IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddYamlFile("appsettings.yaml", false)
        .AddYamlFile($"appsettings.custom.yaml", true)
        .Build();


        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<MainWindow>();
        }
 
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = host.Services.GetService<MainWindow>();
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
