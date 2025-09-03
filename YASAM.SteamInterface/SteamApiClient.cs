using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using YASAM.SteamInterface.Models.Api;

namespace YASAM.SteamInterface;

public class SteamApiClient : HttpClient, ISteamApiClient
{
    private readonly HttpClient _client;
    private readonly IMemoryCache _memoryCache;

    public SteamApiClient(HttpClient client, IMemoryCache memoryCache)
    {
        _client = client;
        _memoryCache = memoryCache;
    }


    public async IAsyncEnumerable<ApiGame> GetGamesAsync(ulong steamUserId, string steamApiKey)
    {
        var cacheItem = await _memoryCache.GetOrCreateAsync<ApiGetOwnedGames>($"OwnedGames-{steamUserId}", async cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            
            var apiResponse = await _client.GetFromJsonAsync<ApiGetOwnedGames?>(
                $"/IPlayerService/GetOwnedGames/v0001/?key={steamApiKey}&steamid={steamUserId}&include_played_free_games=true&include_appinfo=true");
            
            return apiResponse!;
        });
        
        
        foreach (var game in cacheItem?.Response?.Games!) yield return game;
    }

    public async IAsyncEnumerable<ApiGameAchievement> GetAchievementsAsync(ulong steamUserId, string apiKey, ulong appId)
    {
        var gameSchemaApiTask =
            _client.GetFromJsonAsync<ApiSchemaForGameResponse>(
                $"ISteamUserStats/GetSchemaForGame/v0002/?appid={appId}&key={apiKey}&l=en-gb");

        var achievementsApiTask = _client.GetFromJsonAsync<ApiAchievementsForGameResponse?>(
            $"/ISteamUserStats/GetPlayerAchievements/v0001/?appid={appId}&key={apiKey}&steamid={steamUserId}&l=en");

        await Task.WhenAll(gameSchemaApiTask, achievementsApiTask);

        foreach (var achievement in achievementsApiTask.Result?.PlayerStats?.Achievements!)
        {
            var matchingSchemaItem =
                (gameSchemaApiTask.Result?.Game?.AvailableGameStats?.Achievements!).FirstOrDefault(x =>
                    x.Name == achievement.ApiName);

            if (matchingSchemaItem != null)
            {
                achievement.Hidden = matchingSchemaItem.Hidden;
                achievement.AchievedIcon = matchingSchemaItem.Icon;
                achievement.NotAchievedIcon = matchingSchemaItem.Icongray;
            }


            yield return achievement;
        }
    }
}