using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Models.Api;

public class ApiGameFromSchema
{
    [JsonPropertyName("gameName")] public string? GameName { get; set; }

    [JsonPropertyName("gameVersion")] public string? GameVersion { get; set; }

    [JsonPropertyName("availableGameStats")]
    public ApiAvailableGameStatsFromSchema? AvailableGameStats { get; set; }
}