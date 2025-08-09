using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Api;

public class ApiAvailableGameStatsFromSchema
{
    [JsonPropertyName("achievements")] public List<ApiAchievementFromSchema> Achievements { get; set; }
}