using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using YASAM.SteamInterface.Executor.Helpers;

public class LockAllAchievementsCommand : AsyncCommand<LockAllAchievementsCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[AppId]")] public uint AppId { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"[green]Locking all achievements for app id: {settings.AppId}[/]");

        SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);


        AppDomain.CurrentDomain.ProcessExit += (_, __) =>
        {
            AnsiConsole.MarkupLine("[red] Exiting[/]");
            SteamClient.Shutdown();
        };


        Environment.SetEnvironmentVariable("SteamAppId", settings.AppId.ToString());

        SteamClient.Init(settings.AppId,true);

        foreach (var achievement in SteamUserStats.Achievements)
        {
            if (achievement.State)
            {
                achievement.Trigger();
            }
        }
        
        return 0;
    }
}