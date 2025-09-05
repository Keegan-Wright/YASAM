using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Models;
using YASAM.Data;
using YASAM.SteamInterface;
using YASAM.SteamInterface.Models.Internal;

namespace YASAM;

public class CronJobRunner : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    public CronJobRunner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    [TickerFunction("AutomatedGameIdling")]
    public async Task AutomatedGameIdling(CancellationToken cancellationToken)
    {
        await using var serviceScope = _serviceProvider.CreateAsyncScope();
        
        var dbFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<YasamDbContext>>();
        
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        
        // Read cron settings from db, idle all games requested
        
        // var steamworksService = _serviceProvider.GetRequiredService<ISteamWorksService>();
        // var gamesToIdle = new List<int>();
        //
        // await foreach (var idleRequest in gamesToIdle)
        // {
        //     steamworksService.IdleGame(new GameToInvoke()));
        // }
        
    }
    
    
}