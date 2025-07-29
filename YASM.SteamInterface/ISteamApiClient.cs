using YASM.SteamInterface.Api;

namespace YASM.SteamInterface;

internal interface ISteamApiClient
{
    IAsyncEnumerable<ApiGame> GetGames(string steamUserId);

}