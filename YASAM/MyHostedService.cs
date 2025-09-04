using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Models;

namespace YASAM;

public class MyHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    [TickerFunction("ExampleTicker")]
    public async Task ExampleTicker(TickerFunctionContext<string> tickerContext, CancellationToken cancellationToken)
    {
        Console.WriteLine(tickerContext.Request); // Output Hello
    }
}