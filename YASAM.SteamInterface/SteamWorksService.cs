using System.Reflection;
using System.Text;
using System.Text.Unicode;
using Steamworks;

namespace YASAM.SteamInterface;

public class SteamWorksService : ISteamWorksService
{
    private protected CSteamID _steamUserId;
    private readonly ISteamApiClient _steamApiClient;
    
    private readonly Dictionary<ulong, CancellationTokenSource> _idlingTasks = new();
    
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
        throw new NotImplementedException();
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

    public async Task<bool> IdleGame(ulong appId)
    {
        if (_idlingTasks.ContainsKey(appId))
        {
            await StopIdleGame(appId);
            return true;
        }

        Console.WriteLine("Running idle game" + appId);
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        _idlingTasks.Add(appId, cts);
        await InitialiseSteamWithAppId(appId, cts.Token);
        return true;
    }
    public async Task<bool> StopIdleGame(ulong appId)
    {
        await _idlingTasks[appId].CancelAsync();
        return true;
    }
    private async Task InitialiseSteamWithAppId(ulong appId, CancellationToken token)
    {
        var runTask = Task.Run(async () =>
        {
            var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var fileName = "steam_appid.txt";
        
            var combinedPath = Path.Combine(appPath, fileName);
            if (File.Exists(combinedPath))
            {
            
                File.Delete(combinedPath);
            }

            using var filestream = new FileStream(combinedPath, FileMode.CreateNew); 
            using var writer = new StreamWriter(filestream, Encoding.ASCII);
            writer.Write(appId);
            writer.Flush();
            writer.Close();

        
            AppDomain.CurrentDomain.ProcessExit += (_, __) => SteamAPI.Shutdown();
        
            try
            {
                Environment.SetEnvironmentVariable("SteamAppId", appId.ToString());
        
                SteamAPI.InitEx(out string msg);
                Console.WriteLine(msg);

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    throw new Exception(msg);
                }
            
                Console.WriteLine($"SteamAppId: {appId}");
                _steamUserId = SteamUser.GetSteamID();
                Console.WriteLine($"SteamUserId: {_steamUserId}");

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    
                    Thread.Sleep(1000);
                }
                
            }
            finally
            {
                SteamAPI.Shutdown();
            }
        }, token);
        await runTask;

        var a = 1;
    }
}