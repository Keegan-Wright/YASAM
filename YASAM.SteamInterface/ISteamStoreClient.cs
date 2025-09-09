using YASAM.SteamInterface.Models.Api;

namespace YASAM.SteamInterface;

public interface ISteamStoreClient
{
    IAsyncEnumerable<SteamFreeGame> GetFreeGamesAsync(CancellationToken cancellationToken = default);
    Task OpenStorePage(ulong appId, string gameName, CancellationToken cancellationToken = default);
}