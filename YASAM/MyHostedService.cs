using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TickerQ.Utilities.Base;

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
    
    [TickerFunction("ExampleMethod", "* * * * *")]
    public void ExampleMethod()
    {
        var a = 1;
    }

    [TickerFunction("DeactivateStaleUsers", "0 0 * * 0")]
    public void DeactivateStaleUsersAsync()
    {
        var a = 1;
        // Deactivate accounts that haven't been used for a set period.
    }

    [TickerFunction("CleanUpUserSessions", "0 */2 * * *")]
    public void CleanUpUserSessions()
    {
        var a = 1;
        // Remove expired or inactive user sessions.
    }
    
}