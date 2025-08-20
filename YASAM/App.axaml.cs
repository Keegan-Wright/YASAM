using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SQLitePCL;
using SukiUI;
using SukiUI.Dialogs;
using SukiUI.Models;
using SukiUI.Toasts;
using YASAM.Data;
using YASAM.Services.Client;
using YASAM.SteamInterface;
using YASAM.ViewModels;
using YASAM.Views;

namespace YASAM;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();
            services.AddSingleton(desktop);

            Batteries.Init();
            services.AddDbContext<YasamDbContext>();


            AddWindows(services);
            AddViews(services);
            AddViewModels(services);
            AddServices(services);

            services.AddSingleton<ISukiDialogManager, SukiDialogManager>(_ => new SukiDialogManager());
            services.AddSingleton<ISukiToastManager, SukiToastManager>(_ => new SukiToastManager());

            var provider = services.BuildServiceProvider();

            Ioc.Default.ConfigureServices(provider);


            var db = Ioc.Default.GetRequiredService<YasamDbContext>();

            try
            {
                db.Database.Migrate();
            }
            catch
            {
                db.Database.EnsureCreated();
            }


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

        base.OnFrameworkInitializationCompleted();
    }

    private static void AddViewModels(ServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<YourGamesViewModel>();
        services.AddSingleton<IdlingGamesViewModel>();
        services.AddSingleton<LandingViewModel>();
        services.AddSingleton<SelectedUserViewModel>();
        services.AddSingleton<GameAchievementsViewModel>();
        services.AddSingleton<FreeGamesViewModel>();
    }

    private static void AddWindows(ServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
    }

    private static void AddViews(ServiceCollection services)
    {
        services.AddSingleton<YourGamesView>();
        services.AddSingleton<IdlingGamesView>();
        services.AddSingleton<LandingView>();
        services.AddSingleton<GameAchievementsView>();
        services.AddSingleton<FreeGamesView>();
    }

    private static void AddServices(ServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        services.AddHttpClient<ISteamApiClient, SteamApiClient>(client =>
        {
            client.BaseAddress = new Uri("http://api.steampowered.com/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        
        services.AddHttpClient<ISteamStoreClient, SteamStoreClient>(client =>
        {
            client.BaseAddress = new Uri("https://store.steampowered.com/");
        });
        services.AddSingleton<ISteamWorksService, SteamWorksService>();
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