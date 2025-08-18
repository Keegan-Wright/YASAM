using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Models.Api;

public class ApiGameAchievement
{
    [JsonPropertyName("apiname")] public string? ApiName { get; set; }

    [JsonPropertyName("achieved")] public int? Achieved { get; set; }

    [JsonPropertyName("unlocktime")] public int? UnlockTime { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    // Mapped from Game schema API


    [JsonPropertyName("icongray")] public string? NotAchievedIcon { get; set; }

    [JsonPropertyName("icon")] public string? AchievedIcon { get; set; }

    [JsonPropertyName("hidden")] public int? Hidden { get; set; }
}