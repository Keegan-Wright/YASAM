using Microsoft.EntityFrameworkCore;
using YASAM.Data;
using YASAM.Data.Models;

namespace YASAM.Services.Client;

public interface IAutomationService
{
    IAsyncEnumerable<AutomaticIdlingConfiguration> GetGameIdlingAutomationsAsync();
}
public class AutomationService : IAutomationService
{
    private readonly IDbContextFactory<YasamDbContext> _dbContextFactory;

    public AutomationService(IDbContextFactory<YasamDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public async IAsyncEnumerable<AutomaticIdlingConfiguration> GetGameIdlingAutomationsAsync()
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();

        
        var query = db.AutomaticIdlingConfigurations
            .Include(x => x.CronTicker).AsAsyncEnumerable();
        
        await foreach (var automaticIdlingConfiguration in query)
        {
            yield return automaticIdlingConfiguration;
        }
    }
}