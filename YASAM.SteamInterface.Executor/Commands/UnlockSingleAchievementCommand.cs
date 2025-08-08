using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using YASAM.SteamInterface.Executor.Helpers;

public class UnlockSingleAchievementCommand : AsyncCommand<UnlockSingleAchievementCommand.Settings>
{
    static bool statsReceived = false;
    static Callback<UserStatsReceived_t> statsReceivedCallback;
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[AppId]")] public uint AppId { get; set; }
        [CommandArgument(1, "[AchievementId]")] public string AchievementId { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"[green]Unlocking achievement {settings.AchievementId} for app id: {settings.AppId}[/]");

        SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);


        AppDomain.CurrentDomain.ProcessExit += (_, __) =>
        {
            AnsiConsole.MarkupLine("[red] Exiting[/]");
            SteamAPI.Shutdown();
        };


        Environment.SetEnvironmentVariable("SteamAppId", settings.AppId.ToString());

        SteamAPI.InitEx(out var msg);
        Console.WriteLine(msg);

        if (!string.IsNullOrWhiteSpace(msg))
        {
            AnsiConsole.MarkupLine($"[red] Error occured: {msg}[/]");
            throw new Exception(msg);
        }

        // Get the Steam user ID and create a callback for receiving user stats
        CSteamID steamId = SteamUser.GetSteamID();
        statsReceivedCallback = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        // Request user stats from Steam
        SteamAPICall_t apiCall = SteamUserStats.RequestUserStats(steamId);

        // Check if the API call is valid
        if (apiCall == SteamAPICall_t.Invalid)
        {
            Console.WriteLine("{\"error\":\"Failed to requests stats from Steam\"}");
            return 1;
        }

        // Wait for stats to be received
        DateTime startTime = DateTime.Now;
        while (!statsReceived)
        {
            SteamAPI.RunCallbacks();
            if ((DateTime.Now - startTime).TotalSeconds > 120)
            {
                Console.WriteLine("{\"error\":\"Callback timed out\"}");
                return  1;
            }
            Thread.Sleep(100);
        }
        

        var b = SteamUserStats.GetAchievement(settings.AchievementId.ToString(), out bool isUnlocked);
        
        if (!isUnlocked)
        {
            AnsiConsole.WriteLine("Achievement is:" + isUnlocked);
            if (!isUnlocked)
            {
                SteamUserStats.SetAchievement(settings.AchievementId.ToString());
                SteamUserStats.StoreStats();
            }
        }

        return 0;
    }
// Callback method for when user stats are received
    static void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if (pCallback.m_nGameID == SteamUtils.GetAppID().m_AppId)
        {
            if (pCallback.m_eResult == EResult.k_EResultOK)
            {
                statsReceived = true;
            }
            else
            {
                Console.WriteLine(
                    "{\"error\":\"Failed to receive stats from Steam. Error code: "
                    + pCallback.m_eResult
                    + "\"}"
                );
            }
        }
    }
}