using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Models.Api;

public class ApiAchievementsForGame
{
    [JsonPropertyName("steamID")] public string? SteamId { get; set; }

    [JsonPropertyName("gameName")] public string? GameName { get; set; }

    [JsonPropertyName("achievements")] public IEnumerable<ApiGameAchievement>? Achievements { get; set; }

    [JsonPropertyName("success")] public bool Success { get; set; }
}