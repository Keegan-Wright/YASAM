using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using TickerQ.DependencyInjection;
using TickerQ.DependencyInjection.Hosting;
using TickerQ.EntityFrameworkCore.DependencyInjection;
using TickerQ.Utilities;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Models.Ticker;
using YASAM.Data;
using YASAM.Services.Client;
using YASAM.SteamInterface;
using YASAM.ViewModels;
using YASAM.Views;

namespace YASAM;

public abstract class GenericHostAvaloniaApplication<TAvaloniaApplication> : Application, IHostedService
    where TAvaloniaApplication : GenericHostAvaloniaApplication<TAvaloniaApplication>, new()
{
    public IServiceProvider Services { get; private set; } = default!;

    public static AvaloniaApplicationBuilder CreateBuilder(string[]? args = null,
        Func<AppBuilder>? builderFactory = null)
    {
        args ??= [.. Environment.GetCommandLineArgs().Skip(1)];

        var builder = new AvaloniaApplicationBuilder(args, builderFactory);

        builder.Services.AddSingleton(x => (TAvaloniaApplication)Current!);
        
        Batteries.Init();
        
        builder.Services.AddDbContextFactory<YasamDbContext>();
        
        AddWindows(builder.Services);
        AddViews(builder.Services);
        AddViewModels(builder.Services);
        AddServices(builder.Services);
        
        builder.Services.AddSingleton<ISukiDialogManager, SukiDialogManager>(_ => new SukiDialogManager());
        builder.Services.AddSingleton<ISukiToastManager, SukiToastManager>(_ => new SukiToastManager());

        builder.Services.AddTickerQ(opt =>
        {
            opt.UpdateMissedJobCheckDelay(TimeSpan.FromMinutes(5));

            opt.AddOperationalStore<YasamDbContext>(efOpt =>
            {
                efOpt.CancelMissedTickersOnAppStart();          
                efOpt.IgnoreSeedMemoryCronTickers();     
            });
        });
        
        
        builder.Services.AddHostedService<CronJobRunner>();
        
        return builder;
    }

    public int Run()
    {
        Dispatcher.UIThread.VerifyAccess();

        var appLifeTime = (ClassicDesktopStyleApplicationLifetime)ApplicationLifetime!;

        var hostLifeTime = Services.GetRequiredService<IHostApplicationLifetime>();

        appLifeTime.Startup += async delegate
        {
            await HostMain().ConfigureAwait(false);
            Dispatcher.UIThread.Invoke(delegate { appLifeTime.Shutdown(); });
        };

        appLifeTime.ShutdownRequested += (_, request) =>
        {
            request.Cancel = true;
            hostLifeTime.StopApplication();
        };

        return Environment.ExitCode = appLifeTime.Start();
    }

    public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    async Task HostMain()
    {
        var hostLifeTime = Services.GetRequiredService<IHostApplicationLifetime>();

        var host = Services.GetRequiredService<IHost>();
        Ioc.Default.ConfigureServices(host.Services);
        
        try
        {
            await host.StartAsync(hostLifeTime.ApplicationStopping);

            await host.WaitForShutdownAsync(hostLifeTime.ApplicationStopping).ConfigureAwait(false);
        }
        finally
        {
            if (host is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                host.Dispose();
            }
        }
    }
    
    public sealed class AvaloniaApplicationBuilder : IHostApplicationBuilder
    {
        public IServiceCollection Services => _hostBuilder.Services;
        public ILoggingBuilder Logging => _hostBuilder.Logging;
        public IConfigurationManager Configuration => _hostBuilder.Configuration;

        IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostBuilder).Properties;

        IHostEnvironment IHostApplicationBuilder.Environment => _hostBuilder.Environment;

        IMetricsBuilder IHostApplicationBuilder.Metrics => _hostBuilder.Metrics;

        public TAvaloniaApplication Build(ServiceProviderOptions? serviceProviderOptions = null, Action<IClassicDesktopStyleApplicationLifetime>? lifetimeBuilder = null)
        {
            serviceProviderOptions ??= new ServiceProviderOptions()
            {
                ValidateOnBuild = true
            };

            _hostBuilder.ConfigureContainer(new DefaultServiceProviderFactory(serviceProviderOptions));


            _appBuilder.SetupWithClassicDesktopLifetime(_args, lifetimeBuilder);


            
            _hostBuilder.Services.AddHostedService<TAvaloniaApplication>(_ => (TAvaloniaApplication)Current!);
            
            IHost host = _hostBuilder.Build();
            
        
                var db = host.Services.GetRequiredService<IDbContextFactory<YasamDbContext>>().CreateDbContext();

                try
                {
                    db.Database.Migrate();
                }
                catch
                {
                    db.Database.EnsureCreated();
                }
         
            
            host.UseTickerQ();
            
            
            // var _cronTickerManager = host.Services.GetRequiredService<ICronTickerManager<CronTicker>>();
            // var a = _cronTickerManager.AddAsync(new CronTicker
            // {
            //     Expression = "* * * * *",
            //     Function = nameof(CronJobRunner.AutomatedGameIdling),
            //     Description = $"Short Description 2",
            //     Retries = 3,
            //     RetryIntervals = [20, 60, 100] // set in seconds
            // }).Result;

            var app = host.Services.GetRequiredService<TAvaloniaApplication>();

            app.Services = host.Services;

            return app;
        }

        void IHostApplicationBuilder.ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure) => throw new NotImplementedException();

        readonly AppBuilder _appBuilder;
        readonly HostApplicationBuilder _hostBuilder;
        readonly string[] _args;
        internal AvaloniaApplicationBuilder(string[] args,
           Func<AppBuilder>? builderFactory = null)
        {
            _args = args;

            _hostBuilder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
            
            _appBuilder = (builderFactory ?? AppBuilder.Configure<TAvaloniaApplication>).Invoke();
        }


    }
    
    private static void AddViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<YourGamesViewModel>();
        services.AddSingleton<IdlingGamesViewModel>();
        services.AddSingleton<LandingViewModel>();
        services.AddSingleton<SelectedUserViewModel>();
        services.AddSingleton<GameAchievementsViewModel>();
        services.AddSingleton<FreeGamesViewModel>();
        services.AddSingleton<AutomationsViewModel>();
    }

    private static void AddWindows(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
    }

    private static void AddViews(IServiceCollection services)
    {
        services.AddSingleton<YourGamesView>();
        services.AddSingleton<IdlingGamesView>();
        services.AddSingleton<LandingView>();
        services.AddSingleton<GameAchievementsView>();
        services.AddSingleton<FreeGamesView>();
        services.AddSingleton<AutomationsView>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IAutomationService, AutomationService>();
        
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
    
}