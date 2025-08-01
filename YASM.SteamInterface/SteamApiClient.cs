using System.Net.Http.Json;
using YASM.SteamInterface.Api;

namespace YASM.SteamInterface;

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
}