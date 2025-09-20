using YASAM.Data.Models;

namespace YASAM.Services.Client;

public interface IAutomationService
{
    IAsyncEnumerable<AutomaticIdlingConfiguration> GetGameIdlingAutomationsAsync();
}