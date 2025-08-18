using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using YASAM.SteamInterface.Executor.Helpers;

namespace YASAM.SteamInterface.Executor.Commands;

public class UnlockAchievementsCommand : AsyncCommand<UnlockAchievementsCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<AppId>")] public uint AppId { get; set; }

        [CommandArgument(1, "<AchievementIds>")]
        public string[] AchievementIds { get; set; } = [];
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        foreach (var achievementId in settings.AchievementIds)
        {
            AnsiConsole.MarkupLine($"[green]Unlocking achievement {achievementId} for app id: {settings.AppId}[/]");

            await SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);
            SteamProcessHelpers.SetEnvionmentVariable(settings.AppId);

            SteamClient.Init(settings.AppId);
        
        
            var achievement = SteamUserStats.Achievements.First(x => x.Identifier == achievementId);
            if (!achievement.State)
            {
                achievement.Trigger();
            }
        }
        return 0;
    }
}