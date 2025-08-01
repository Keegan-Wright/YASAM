using Steamworks;

namespace YASAM.SteamInterface;

public class SteamWorksService : ISteamWorksService
{
    private readonly CSteamID _steamId;
    private readonly ISteamApiClient _steamApiClient;
    
    public SteamWorksService(ISteamApiClient steamApiClient)
    {
        if (!SteamAPI.Init())
        {
            throw new Exception("Please run the steam client");
        }
        _steamApiClient  = steamApiClient;
        _steamId = SteamUser.GetSteamID();
    }

    public IAsyncEnumerable<Achievement> GetAchivementsByAppId(string appId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> LockAchivement(string appId, string achivementId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UnlockAchivement(string appId, string achivementId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> LockAllAchievements(string appId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UnlockAllAchievements(string appId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateStats(string appId, IEnumerable<StatUpdate> statUpdates)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResetAllStats(string appId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IdleGame(string appId)
    {
        throw new NotImplementedException();
    }
}