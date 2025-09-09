using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using SukiUI;
using SukiUI.Models;
using YASAM.SteamInterface;
using YASAM.ViewModels;
using YASAM.Views;

namespace YASAM;

public class App : GenericHostAvaloniaApplication<App>
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = Ioc.Default.GetRequiredService<MainWindow>();
            desktop.MainWindow.DataContext = Ioc.Default.GetRequiredService<MainWindowViewModel>();
            

            var purpleTheme = new SukiColorTheme("Purple", Colors.SlateBlue, Colors.DarkBlue);
            SukiTheme.GetInstance().AddColorTheme(purpleTheme);
            SukiTheme.GetInstance().ChangeColorTheme(purpleTheme);


            ((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).ShutdownRequested += delegate
            {
                // Perform any necessary cleanup here before the application shuts down.
                // For example, you might want to save user settings or close database connections.
                var dbContext = Ioc.Default.GetRequiredService<ISteamWorksService>();


                foreach (var game in dbContext.GetIdlingGames()) Process.GetProcessById(game.ProcessId).Kill();
            };
        }
        return base.StartAsync(cancellationToken);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();
                    
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }
    
    
    private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        var a = 1;
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var a = 1;
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove) BindingPlugins.DataValidators.Remove(plugin);
    }
}