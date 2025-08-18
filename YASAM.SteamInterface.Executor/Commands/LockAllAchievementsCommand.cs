using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using YASAM.SteamInterface.Executor.Helpers;

namespace YASAM.SteamInterface.Executor.Commands;

public class LockAllAchievementsCommand : AsyncCommand<LockAllAchievementsCommand.Settings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"[green]Locking all achievements for app id: {settings.AppId}[/]");

        await SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);
        SteamProcessHelpers.SetEnvionmentVariable(settings.AppId);

        SteamClient.Init(settings.AppId);

        foreach (var achievement in SteamUserStats.Achievements)
            if (achievement.State)
                achievement.Trigger();

        return 0;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<AppId>")] public uint AppId { get; set; }
    }
}