using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Api;

public class ApiAchievementsForGameResponse
{
    [JsonPropertyName("playerstats")] public ApiAchievementsForGame? PlayerStats { get; set; }
}