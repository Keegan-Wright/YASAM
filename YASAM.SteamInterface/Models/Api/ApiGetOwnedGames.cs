using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Api;

internal record ApiGetOwnedGames
{
    [JsonPropertyName("response")]
    public ApiOwnedGamesResponse? Response { get; set; }
}