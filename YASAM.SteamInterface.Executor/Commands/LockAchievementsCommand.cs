using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using YASAM.SteamInterface.Executor.Helpers;

public class LockAchievementsCommand : AsyncCommand<LockAchievementsCommand.Settings>
{
    public class Settings : CommandSettings
    {

        [CommandArgument(0, "<AppId>")] 
        public uint AppId { get; set; }
        
        [CommandArgument(1, "<AchievementIds>")]
        public string[] AchievementIds { get; set; } = [];
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        foreach (var achievementId in settings.AchievementIds)
        {
            AnsiConsole.MarkupLine($"[green]Locking achievement {achievementId} for app id: {settings.AppId}[/]");

             SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);
             SteamProcessHelpers.SetEnvionmentVariable(settings.AppId);
            
             SteamClient.Init(settings.AppId,true);
            
        
            var achievement = SteamUserStats.Achievements.First(x => x.Identifier == achievementId);
            if (achievement.State)
            {
                achievement.Trigger();
            }
        }

        
        return 0;
    }
}