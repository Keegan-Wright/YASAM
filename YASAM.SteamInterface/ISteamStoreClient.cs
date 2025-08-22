using YASAM.SteamInterface.Models.Api;

namespace YASAM.SteamInterface;

public interface ISteamStoreClient
{
    IAsyncEnumerable<SteamFreeGame> GetFreeGamesAsync();
    void OpenStorePage(ulong appId, string gameName);
}