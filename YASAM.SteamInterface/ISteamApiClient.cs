using YASAM.SteamInterface.Models.Api;

namespace YASAM.SteamInterface;

public interface ISteamApiClient
{
    IAsyncEnumerable<ApiGame> GetGamesAsync(ulong steamUserId, string steamApiKey);
    IAsyncEnumerable<ApiGameAchievement> GetAchievementsAsync(ulong steamUserId, string apiKey, ulong appId);

}