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
        var gameSchemaApiTask =
             _client.GetFromJsonAsync<ApiSchemaForGameResponse>(
                $"ISteamUserStats/GetSchemaForGame/v0002/?appid={appId}&key={apiKey}&l=en-gb");
        
        var achievementsApiTask =  _client.GetFromJsonAsync<ApiAchievementsForGameResponse?>($"/ISteamUserStats/GetPlayerAchievements/v0001/?appid={appId}&key={apiKey}&steamid={steamUserId}&l=en");

        await Task.WhenAll(gameSchemaApiTask, achievementsApiTask);
        
        foreach (var achievement in achievementsApiTask.Result.PlayerStats.Achievements)
        {
            var matchingSchemaItem = gameSchemaApiTask.Result.Game.AvailableGameStats.Achievements.FirstOrDefault(x => x.Name == achievement.ApiName);
            
            achievement.Hidden = matchingSchemaItem.Hidden;
            achievement.AchievedIcon = matchingSchemaItem.Icon;
            achievement.NotAchievedIcon = matchingSchemaItem.Icongray;
            
            yield return achievement;
        }
    }
}