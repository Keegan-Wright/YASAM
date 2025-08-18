namespace YASAM.SteamInterface.Models;

public record Game(ulong AppId, string Name, bool Installed, string ImgUrl, int PlaytimeTotal);