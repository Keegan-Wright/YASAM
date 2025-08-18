using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Models.Api;

public class ApiSchemaForGameResponse
{
    [JsonPropertyName("game")] public ApiGameFromSchema? Game { get; set; }
}