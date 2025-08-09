using System.Text.Json.Serialization;

namespace YASAM.SteamInterface.Api;

public class ApiSchemaForGameResponse
{
    [JsonPropertyName("game")] public ApiGameFromSchema Game { get; set; }
}