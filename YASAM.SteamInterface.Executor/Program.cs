using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddCommand<IdleGameCommand>("idle");
            config.AddCommand<UnlockAchievementsCommand>("unlockAchievements");
            config.AddCommand<LockAchievementsCommand>("lockAchievements");
            config.AddCommand<UnlockAllAchievementsCommand>("unlockAllAchievements");
            config.AddCommand<LockAllAchievementsCommand>("lockAllAchievements");
        });

        AppDomain.CurrentDomain.ProcessExit += (_, __) =>
        {
            AnsiConsole.MarkupLine("[red] Exiting[/]");
            SteamClient.Shutdown();
        };

        await app.RunAsync(args);
    }
    
}