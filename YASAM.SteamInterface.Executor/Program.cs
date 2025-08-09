using Spectre.Console.Cli;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddCommand<IdleGameCommand>("idle");
            config.AddCommand<UnlockSingleAchievementCommand>("unlockAchievement");
            config.AddCommand<LockSingleAchievementCommand>("lockAchievement");
            config.AddCommand<UnlockAllAchievementsCommand>("unlockAllAchievements");
            config.AddCommand<LockAllAchievementsCommand>("lockAllAchievements");
        });


        await app.RunAsync(args);
    }
    
}

public enum SteamUtilityCommandType
{
    Idle = 0,
    UnlockSingleAchievement = 1,
    LockSingleAchievement = 2,
    LockAllAchievements = 3,
    UnlockAllAchievements = 4
}