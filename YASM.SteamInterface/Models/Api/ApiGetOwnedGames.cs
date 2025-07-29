using System.Text.Json.Serialization;

namespace YASM.SteamInterface.Api;

internal record ApiGetOwnedGames
{
    [JsonPropertyName("response")]
    public ApiOwnedGamesResponse? Response { get; set; }
}