using System.Reflection;
using System.Text;
using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using YASAM.SteamInterface.Executor.Helpers;

public class IdleGameCommand : AsyncCommand<IdleGameCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<AppId>")] public uint AppId { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"[grey]IdleGameCommand for app id: {settings.AppId}[/]");

        SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);
        SteamProcessHelpers.SetEnvionmentVariable(settings.AppId);
        SteamClient.Init(settings.AppId, true);

        AnsiConsole.MarkupLine("[green] Successfully idling game[/]");
        
        while (true) Thread.Sleep(1000);
    }
    
}