using System.Text.Json.Serialization;

namespace YASM.SteamInterface.Api;

internal record ApiOwnedGamesResponse
{
    [JsonPropertyName("game_count")]
    public int GameCount { get; set; }

    [JsonPropertyName("games")]
    public List<ApiGame>? Games { get; set; }
}