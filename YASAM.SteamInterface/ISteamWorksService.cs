namespace YASAM.SteamInterface;

public interface ISteamWorksService
{
    IAsyncEnumerable<Achievement> GetAchivementsByAppId(ulong appId);
    Task<bool> LockAchivement(ulong appId, string achivementId);
    Task<bool> UnlockAchivement(ulong appId, string achivementId);
    Task<bool> LockAllAchievements(ulong appId);
    Task<bool> UnlockAllAchievements(ulong appId);
    Task<bool> UpdateStats(ulong appId, IEnumerable<StatUpdate> statUpdates);
    Task<bool> ResetAllStats(ulong appId);
    Task<bool> IdleGame(ulong appId);
    Task<bool> StopIdleGame(ulong appId);
}