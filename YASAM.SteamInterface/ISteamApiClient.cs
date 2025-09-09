using YASAM.SteamInterface.Models.Api;

namespace YASAM.SteamInterface;

public interface ISteamApiClient
{
    IAsyncEnumerable<ApiGame> GetGamesAsync(ulong steamUserId, string steamApiKey, CancellationToken cancellationToken = default);
    IAsyncEnumerable<ApiGameAchievement> GetAchievementsAsync(ulong steamUserId, string apiKey, ulong appId, CancellationToken cancellationToken = default);

}