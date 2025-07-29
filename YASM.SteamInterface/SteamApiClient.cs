using System.Net.Http.Json;
using YASM.SteamInterface.Api;

namespace YASM.SteamInterface;

internal class SteamApiClient : HttpClient, ISteamApiClient
{
    private readonly HttpClient _client;
    private readonly string _SteamApiKey;
    
    public SteamApiClient(HttpClient client)
    {
        _client = client;
    }


    public async IAsyncEnumerable<ApiGame> GetGames(string steamUserId)
    {
        var apiResponse =  await _client.GetFromJsonAsync<ApiGetOwnedGames?>($"/IPlayerService/GetOwnedGames/v0001/?key={_SteamApiKey}&steamid={steamUserId}&include_played_free_games=true&include_appinfo=true");

        foreach (var game in apiResponse.Response.Games)
        {
            yield return game;
        }

    }
}