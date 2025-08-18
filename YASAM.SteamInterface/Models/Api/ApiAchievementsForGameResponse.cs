using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Models.Api;

public class ApiAchievementsForGameResponse
{
    [JsonPropertyName("playerstats")] public ApiAchievementsForGame? PlayerStats { get; set; }
}