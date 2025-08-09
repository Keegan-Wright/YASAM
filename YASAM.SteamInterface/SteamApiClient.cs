using System.Net.Http.Json;
using YASAM.SteamInterface.Api;

namespace YASAM.SteamInterface;

public class SteamApiClient : HttpClient, ISteamApiClient
{
    private readonly HttpClient _client;
    
    public SteamApiClient(HttpClient client)
    {
        _client = client;
    }


    public async IAsyncEnumerable<ApiGame> GetGames(ulong steamUserId, string steamApiKey)
    {
        
        var apiResponse =  await _client.GetFromJsonAsync<ApiGetOwnedGames?>($"/IPlayerService/GetOwnedGames/v0001/?key={steamApiKey}&steamid={steamUserId}&include_played_free_games=true&include_appinfo=true");

        foreach (var game in apiResponse.Response.Games)
        {
            yield return game;
        }

    }

    public async IAsyncEnumerable<ApiGameAchievement> GetAchievements(ulong steamUserId, string apiKey, ulong appId)
    {
        var apiResponse =  await _client.GetFromJsonAsync<ApiAchievementsForGameResponse?>($"/ISteamUserStats/GetPlayerAchievements/v0001/?appid={appId}&key={apiKey}&steamid={steamUserId}&l=en-gb");

        foreach (var achievement in apiResponse.PlayerStats.Achievements)
        {
            yield return achievement;
        }
    }
}