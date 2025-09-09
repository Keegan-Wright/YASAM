using System.Collections.Frozen;
using System.Diagnostics;
using YASAM.SteamInterface.Executor;
using YASAM.SteamInterface.Models.Internal;

namespace YASAM.SteamInterface;

public class SteamWorksService : ISteamWorksService
{
    private readonly Dictionary<ulong, IdlingGame> _idlingGames = new();
    public async Task<bool> LockAchievements(ulong appId, IEnumerable<string> achievementIds, CancellationToken cancellationToken = default)
    {
        var enumerable = achievementIds.ToFrozenSet();
        
        if (!enumerable.Any())
            return true;

        var args = $"{string.Join(" ", enumerable)}";
        var process = InvokeSteamCommand(appId, SteamUtilityCommandType.LockAchievements, args);
        await process.WaitForExitAsync(cancellationToken);
        var result = process.ExitCode == 0;
        process.Dispose();
        return result;
    }

    public async Task<bool> UnlockAchievements(ulong appId, IEnumerable<string> achievementIds, CancellationToken cancellationToken = default)
    {
        var enumerable = achievementIds.ToFrozenSet();
        if (!enumerable.Any())
            return true;

        var args = $"{string.Join(" ", enumerable)}";
        var process = InvokeSteamCommand(appId, SteamUtilityCommandType.UnlockAchievements, args);
        await process.WaitForExitAsync(cancellationToken);
        var result = process.ExitCode == 0;
        process.Dispose();
        return result;
    }

    public async Task<bool> LockAllAchievements(ulong appId, CancellationToken cancellationToken = default)
    {
        var process = InvokeSteamCommand(appId, SteamUtilityCommandType.LockAllAchievements);
        await process.WaitForExitAsync(cancellationToken);
        var result = process.ExitCode == 0;
        process.Dispose();
        return result;
    }

    public async Task<bool> UnlockAllAchievements(ulong appId, CancellationToken cancellationToken = default)
    {
        var process = InvokeSteamCommand(appId, SteamUtilityCommandType.UnlockAllAchievements);
        await process.WaitForExitAsync(cancellationToken);;
        var result = process.ExitCode == 0;
        process.Dispose();
        return result;
    }

    public Task<bool> IdleGame(GameToInvoke gameToInvoke)
    {
        if (_idlingGames.ContainsKey(gameToInvoke.AppId)) return Task.FromResult(true);

        var process = InvokeSteamCommand(gameToInvoke.AppId, SteamUtilityCommandType.Idle);

        _idlingGames.Add(gameToInvoke.AppId, new IdlingGame(process.Id, gameToInvoke.AppId, gameToInvoke.GameName));


        return Task.FromResult(true);
    }

    public bool StopIdlingGame(GameToInvoke gameToInvoke)
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

    private Process InvokeSteamCommand(ulong appId, SteamUtilityCommandType commandType, string arguments = null!)
    {
        var proc = new Process();
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.UseShellExecute = false;

        var executableName = "YASAM.SteamInterface.Executor";

        if (OperatingSystem.IsWindows()) executableName += ".exe";


        proc.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, executableName);

        switch (commandType)
        {
            case SteamUtilityCommandType.Idle:
            {
                proc.StartInfo.Arguments = $"idle {appId}";
                break;
            }
            case SteamUtilityCommandType.UnlockAchievements:
            {
                proc.StartInfo.Arguments = $"unlockAchievements {appId} {arguments}";
                break;
            }
            case SteamUtilityCommandType.LockAchievements:
                proc.StartInfo.Arguments = $"unlockAchievements {appId} {arguments}";
                break;
            case SteamUtilityCommandType.LockAllAchievements:
                proc.StartInfo.Arguments = $"lockAllAchievement {appId}";
                break;

            case SteamUtilityCommandType.UnlockAllAchievements:
                proc.StartInfo.Arguments = $"unlockAllAchievements {appId}";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
        }

        proc.Start();
        return proc;
    }
}