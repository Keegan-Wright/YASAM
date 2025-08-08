namespace YASAM.SteamInterface.Internal;

public record GameToInvoke(ulong AppId, string GameName);

public record IdlingGame(int ProcessId, ulong AppId, string GameName) : GameToInvoke(AppId, GameName);