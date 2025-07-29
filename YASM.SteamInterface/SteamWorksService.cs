using Steamworks;

namespace YASM.SteamInterface;

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

    public async IAsyncEnumerable<Game> GetGames()
    {
        var steamUserId = SteamUser.GetSteamID();
        var games = _steamApiClient.GetGames(steamUserId.ToString());

        await foreach (var game in games)
        {
            var installed = SteamApps.BIsAppInstalled(new AppId_t(Convert.ToUInt32(game.AppId)));
            yield return new Game(game.AppId, game.Name, installed, $"https://cdn.cloudflare.steamstatic.com/steam/apps/{game.AppId}/header.jpg", game.PlaytimeForever);
        }
    }
}