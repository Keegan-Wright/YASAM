using System.Collections.Frozen;
using System.Diagnostics;
using YASAM.SteamInterface.Internal;

namespace YASAM.SteamInterface;

public class SteamWorksService : ISteamWorksService
{
    private readonly ISteamApiClient _steamApiClient;
    
    private readonly Dictionary<ulong, IdlingGame> _idlingGames = new();
    
    public SteamWorksService(ISteamApiClient steamApiClient)
    {
        _steamApiClient  = steamApiClient;
    }

    public IAsyncEnumerable<Achievement> GetAchivementsByAppId(ulong appId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> LockAchivement(ulong appId, string achivementId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UnlockAchivement(ulong appId, string achivementId)
    {
 
        var process = InvokeSteamCommand(appId,SteamUtilityCommandType.UnlockSingleAchievement);
        process.WaitForExit();
        process.Dispose();
        return Task.FromResult(process.ExitCode == 0);

    }

    public Task<bool> LockAllAchievements(ulong appId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UnlockAllAchievements(ulong appId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateStats(ulong appId, IEnumerable<StatUpdate> statUpdates)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResetAllStats(ulong appId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IdleGame(GameToInvoke gameToInvoke)
    {
        if (_idlingGames.ContainsKey(gameToInvoke.AppId))
        {
            return true;
        }

        var process = InvokeSteamCommand(gameToInvoke.AppId,SteamUtilityCommandType.Idle);

        _idlingGames.Add(gameToInvoke.AppId, new IdlingGame(process.Id, gameToInvoke.AppId, gameToInvoke.GameName));
        

        return true;
    }

    private Process InvokeSteamCommand(ulong appId, SteamUtilityCommandType commandType)
    {
        Process proc = new Process ();
        proc.StartInfo.FileName = "/bin/bash";
        proc.StartInfo.UseShellExecute = false;
        var baseCommandPath = "./YASAM.SteamInterface.Executor";
        switch (commandType)
        {
            case SteamUtilityCommandType.Idle:
            {
                proc.StartInfo.Arguments = $"-c \" {baseCommandPath} idle {appId} \"";
                break;
            }
            case SteamUtilityCommandType.UnlockSingleAchievement:
            {
                proc.StartInfo.Arguments = $"-c \" {baseCommandPath} unlockAchievement {appId} 115 \"";
                break;
            }
        }
        proc.Start();
        return proc;
    }

    public bool StopIdleGame(GameToInvoke gameToInvoke)
    {
        var process = Process.GetProcessById(_idlingGames[gameToInvoke.AppId].ProcessId);
        process.Kill();
        process.Dispose();
        _idlingGames.Remove(gameToInvoke.AppId);
        return true;
    }

    public FrozenSet<IdlingGame> GetIdlingGames()
    {
        var games = _idlingGames.Select(x => x.Value).ToList();
        return games.ToFrozenSet();
    }
}