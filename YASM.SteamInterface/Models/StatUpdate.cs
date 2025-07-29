namespace YASM.SteamInterface;

internal record StatUpdate(string Name, object Value);

internal record Game(int AppId, string Name, bool Installed, string ImgUrl);