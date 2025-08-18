using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Models.Api;

public class ApiAvailableGameStatsFromSchema
{
    [JsonPropertyName("achievements")] public IEnumerable<ApiAchievementFromSchema>? Achievements { get; set; }
}