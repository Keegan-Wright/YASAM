using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Models.Api;

internal record ApiOwnedGamesResponse
{
    [JsonPropertyName("game_count")]
    public int? GameCount { get; set; }

    [JsonPropertyName("games")]
    public IEnumerable<ApiGame>? Games { get; set; }
}