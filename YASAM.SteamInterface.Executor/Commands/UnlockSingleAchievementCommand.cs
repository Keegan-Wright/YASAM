using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using Steamworks.Data;
using YASAM.SteamInterface.Executor.Helpers;

public class UnlockSingleAchievementCommand : AsyncCommand<UnlockSingleAchievementCommand.Settings>
{
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
            SteamClient.Shutdown();
        };


        Environment.SetEnvironmentVariable("SteamAppId", settings.AppId.ToString());

        SteamClient.Init(settings.AppId,true);
        
        
        var achievement = SteamUserStats.Achievements.First(x => x.Identifier == settings.AchievementId);
        if (!achievement.State)
        {
            achievement.Trigger();
        }
        
        return 0;
    }
}

public class LockSingleAchievementCommand : AsyncCommand<LockSingleAchievementCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[AppId]")] public uint AppId { get; set; }
        [CommandArgument(1, "[AchievementId]")] public string AchievementId { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"[green]Locking achievement {settings.AchievementId} for app id: {settings.AppId}[/]");

        SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);


        AppDomain.CurrentDomain.ProcessExit += (_, __) =>
        {
            AnsiConsole.MarkupLine("[red] Exiting[/]");
            SteamClient.Shutdown();
        };


        Environment.SetEnvironmentVariable("SteamAppId", settings.AppId.ToString());

        SteamClient.Init(settings.AppId,true);
        
        
        var achievement = SteamUserStats.Achievements.First(x => x.Identifier == settings.AchievementId);
        if (achievement.State)
        {
            achievement.Trigger();
        }
        
        return 0;
    }
}