using System.Collections.Frozen;
using YASAM.SteamInterface.Internal;

namespace YASAM.SteamInterface;

public interface ISteamWorksService
{
    Task<bool> LockAchivement(ulong appId, string achievementId);
    Task<bool> UnlockAchivement(ulong appId, string achievementId);
    Task<bool> LockAllAchievements(ulong appId);
    Task<bool> UnlockAllAchievements(ulong appId);
    Task<bool> IdleGame(GameToInvoke gameToInvoke);
    bool StopIdleGame(GameToInvoke gameToInvoke);
    FrozenSet<IdlingGame> GetIdlingGames();
}