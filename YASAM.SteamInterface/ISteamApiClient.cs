using YASAM.SteamInterface.Api;

namespace YASAM.SteamInterface;

public interface ISteamApiClient
{
    IAsyncEnumerable<ApiGame> GetGames(ulong steamUserId, string steamApiKey);

}