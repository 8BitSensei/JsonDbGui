using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using JsonDbGui.ViewModels;
using JsonDbGui.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace JsonDbGui
{
    public partial class App : Application
    {
        public IHost AppHost { get; }

        public static T GetService<T>()
        where T : class
        {
            if ((Current as App)!.AppHost.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.axaml.cs.");
            }

            return service;
        }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureServices((context, services) =>
                {
                    services.AddLocalJsonDb(options => 
                    {
                        options.DbPath = "C:\\jsonDb"; //TODO: Put this in appsettings and make it relative
                    });

                })
                .Build();

        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}