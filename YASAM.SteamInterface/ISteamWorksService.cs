using System.Collections.Frozen;
using YASAM.SteamInterface.Models.Internal;

namespace YASAM.SteamInterface;

public interface ISteamWorksService
{
    Task<bool> LockAchievements(ulong appId, IEnumerable<string> achievementIds);
    Task<bool> UnlockAchievements(ulong appId, IEnumerable<string> achievementIds);
    Task<bool> LockAllAchievements(ulong appId);
    Task<bool> UnlockAllAchievements(ulong appId);
    Task<bool> IdleGame(GameToInvoke gameToInvoke);
    bool StopIdleGame(GameToInvoke gameToInvoke);
    FrozenSet<IdlingGame> GetIdlingGames();
}