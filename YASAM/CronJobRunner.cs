using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TickerQ.Utilities;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Models;
using TickerQ.Utilities.Models.Ticker;
using YASAM.CronModels;
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
    public async Task AutomatedGameIdling(TickerFunctionContext<string> context, CancellationToken cancellationToken)
    {
        await using var serviceScope = _serviceProvider.CreateAsyncScope();

        var dbFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<YasamDbContext>>();
        var steamworksService = _serviceProvider.GetRequiredService<ISteamWorksService>();
        var tickerTimerManager = _serviceProvider.GetRequiredService<ITimeTickerManager<TimeTicker>>();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        
        var query = from automatedIdling in db.AutomaticIdlingConfigurations
            join cronTicker in db.CronTickers on automatedIdling.CronTickerId equals cronTicker.Id
            join cronTickerOccurence in db.CronTickerOccurrences on cronTicker.Id equals cronTickerOccurence.CronTickerId
            where cronTickerOccurence.Id == context.Id 
            select automatedIdling;

        await foreach (var game in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            if (!steamworksService.GetIdlingGames().Any(x => x.AppId == game.AppId))
            {
                await steamworksService.IdleGame(new GameToInvoke(game.AppId, game.GameName));
                await tickerTimerManager.AddAsync(new TimeTicker
                {
                    Request = TickerHelper.CreateTickerRequest<AutomaticStopIdlingGame>(new AutomaticStopIdlingGame(game.CronTickerId, game.AppId)),
                    ExecutionTime = DateTime.Now.AddMinutes(game.IdleTime),
                    Function = "AutomatedStopIdlingGame",
                    Description = $"Stop Idling Game {game.GameName}",
                    Retries = 3,
                    RetryIntervals = [20, 60, 100]
                }, cancellationToken);

            }
        }
    }

    [TickerFunction("AutomatedStopIdlingGame")]
    public async Task AutomatedStopIdlingGame(TickerFunctionContext<AutomaticStopIdlingGame> context, CancellationToken cancellationToken)
    {
        await using var serviceScope = _serviceProvider.CreateAsyncScope();
        var steamworksService = _serviceProvider.GetRequiredService<ISteamWorksService>();
        
        var dbFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<YasamDbContext>>();
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        
        var query = from automatedIdling in db.AutomaticIdlingConfigurations
            join cronTicker in db.CronTickers on automatedIdling.CronTickerId equals cronTicker.Id
            where cronTicker.Id == context.Request.CronTickerId && automatedIdling.AppId == context.Request.AppId
            select automatedIdling;

        var gameToStop = await query.FirstOrDefaultAsync(cancellationToken);
        steamworksService.StopIdlingGame(new GameToInvoke(gameToStop.AppId, gameToStop.GameName));
        
    }
}