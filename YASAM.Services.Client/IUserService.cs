using YASAM.Data.Models;

namespace YASAM.Services.Client;


public interface IUserService
{
    Task<TrackedSteamUser> GetSteamUserAsync(Guid id);
    IAsyncEnumerable<TrackedSteamUser> GetTrackedUsersAsync();
    Task<TrackedSteamUser> AddTrackedUserAsync(string name, ulong steamUserId, string apiKey);
    
}