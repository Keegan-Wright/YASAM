using System.ComponentModel.DataAnnotations;

namespace YASAM.Data.Models;

public class TrackedSteamUser : BaseEntity
{
    public ulong SteamId { get; private set; }
    
    [MaxLength(200)]
    public string Name { get; private set; }
    
    [MaxLength(200)]
    public string ApiKey {get; private set;}

    public TrackedSteamUser(ulong steamId, string name, string apiKey)
    {
        SteamId = steamId;
        Name = name;
        ApiKey = apiKey;
    }
    
}
