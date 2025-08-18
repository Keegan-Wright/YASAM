using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Models.Api;

public class ApiAchievementFromSchema
{
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("defaultvalue")] public int? Defaultvalue { get; set; }

    [JsonPropertyName("displayName")] public string? DisplayName { get; set; }

    [JsonPropertyName("hidden")] public int? Hidden { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("icon")] public string? Icon { get; set; }

    [JsonPropertyName("icongray")] public string? Icongray { get; set; }
}