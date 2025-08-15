using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using YASAM.SteamInterface.Executor.Helpers;

public class UnlockAllAchievementsCommand : AsyncCommand<UnlockAllAchievementsCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<AppId>")] public uint AppId { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"[green]unlocking all achievements for app id: {settings.AppId}[/]");

        SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);
        SteamProcessHelpers.SetEnvionmentVariable(settings.AppId);

        SteamClient.Init(settings.AppId,true);

        foreach (var achievement in SteamUserStats.Achievements)
        {
            if (!achievement.State)
            {
                achievement.Trigger();
            }
        }
        
        return 0;
    }
}