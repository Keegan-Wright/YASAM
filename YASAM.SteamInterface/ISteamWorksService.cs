namespace YASAM.SteamInterface;

public interface ISteamWorksService
{
    IAsyncEnumerable<Achievement> GetAchivementsByAppId(string appId);
    Task<bool> LockAchivement(string appId, string achivementId);
    Task<bool> UnlockAchivement(string appId, string achivementId);
    Task<bool> LockAllAchievements(string appId);
    Task<bool> UnlockAllAchievements(string appId);
    Task<bool> UpdateStats(string appId, IEnumerable<StatUpdate> statUpdates);
    Task<bool> ResetAllStats(string appId);
    Task<bool> IdleGame(string appId);
}