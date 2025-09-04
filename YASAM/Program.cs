using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SQLitePCL;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using TickerQ.DependencyInjection;
using YASAM.Data;
using YASAM.Services.Client;
using YASAM.SteamInterface;
using YASAM.ViewModels;
using YASAM.Views;

namespace YASAM;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
        var builder = App.CreateBuilder(args, BuildAvaloniaApp);
        var app = builder.Build();
        _ = app.Run();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}