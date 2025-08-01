using YASM.SteamInterface.Api;

namespace YASM.SteamInterface;

public interface ISteamApiClient
{
    IAsyncEnumerable<ApiGame> GetGames(ulong steamUserId, string steamApiKey);

}