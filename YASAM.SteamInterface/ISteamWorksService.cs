using System.Collections.Frozen;
using YASAM.SteamInterface.Models.Internal;

namespace YASAM.SteamInterface;

public interface ISteamWorksService
{
    Task<bool> LockAchievements(ulong appId, IEnumerable<string> achievementIds, CancellationToken cancellationToken = default);
    Task<bool> UnlockAchievements(ulong appId, IEnumerable<string> achievementIds, CancellationToken cancellationToken = default);
    Task<bool> LockAllAchievements(ulong appId, CancellationToken cancellationToken = default);
    Task<bool> UnlockAllAchievements(ulong appId, CancellationToken cancellationToken = default);
    Task<bool> IdleGame(GameToInvoke gameToInvoke);
    bool StopIdlingGame(GameToInvoke gameToInvoke);
    FrozenSet<IdlingGame> GetIdlingGames();
}