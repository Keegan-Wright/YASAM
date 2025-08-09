using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Net.Http.Headers;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SukiUI;
using SukiUI.Dialogs;
using SukiUI.Models;
using YASAM.Data;
using YASAM.Services.Client;
using YASAM.ViewModels;
using YASAM.Views;
using YASAM.SteamInterface;

namespace YASAM;

public partial class App : Application
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

            SQLitePCL.Batteries.Init();
            services.AddDbContext<YasamDbContext>();
            
            
            AddWindows(services);
            AddViews(services);
            AddViewModels(services);
            AddServices(services);
            
            services.AddSingleton<ISukiDialogManager, SukiDialogManager>(x => new SukiDialogManager());
            
            var provider = services.BuildServiceProvider();

            Ioc.Default.ConfigureServices(provider);
            
            
            
            var db = Ioc.Default.GetRequiredService<YasamDbContext>();

            try
            {
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                _ = db.Database.EnsureCreated();
            }
            
            
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = Ioc.Default.GetService<MainWindow>();
            desktop.MainWindow.DataContext = Ioc.Default.GetService<MainWindowViewModel>();
            
            var PurpleTheme = new SukiColorTheme("Purple", Colors.SlateBlue, Colors.DarkBlue);
            SukiTheme.GetInstance().AddColorTheme(PurpleTheme);
            SukiTheme.GetInstance().ChangeColorTheme(PurpleTheme);
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
    }

    private static void AddServices(ServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        services.AddHttpClient<ISteamApiClient, SteamApiClient>(client =>
        {
            client.BaseAddress = new Uri("http://api.steampowered.com/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        });
        services.AddSingleton<ISteamWorksService, SteamWorksService>();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}