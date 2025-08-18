namespace YASAM.SteamInterface.Models.Internal;

public record IdlingGame(int ProcessId, ulong AppId, string GameName) : GameToInvoke(AppId, GameName);