using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Api;

public class ApiAchievementsForGame
{
    [JsonPropertyName("steamID")] public string? SteamID { get; set; }

    [JsonPropertyName("gameName")] public string? GameName { get; set; }

    [JsonPropertyName("achievements")] public List<ApiGameAchievement>? Achievements { get; set; }

    [JsonPropertyName("success")] public bool Success { get; set; }
}