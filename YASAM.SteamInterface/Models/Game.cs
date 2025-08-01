namespace YASAM.SteamInterface;

public record Game(uint AppId, string Name, bool Installed, string ImgUrl, int PlaytimeTotal);