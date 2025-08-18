using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;
using YASAM.SteamInterface.Executor.Helpers;

namespace YASAM.SteamInterface.Executor.Commands;

public class IdleGameCommand : AsyncCommand<IdleGameCommand.Settings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"[grey]IdleGameCommand for app id: {settings.AppId}[/]");

        await SteamProcessHelpers.SetupSteamAppIdTextFile(settings.AppId);
        SteamProcessHelpers.SetEnvionmentVariable(settings.AppId);
        SteamClient.Init(settings.AppId);

        AnsiConsole.MarkupLine("[green] Successfully idling game[/]");

        while (true) await Task.Delay(1000);
        // ReSharper disable once FunctionNeverReturns
        // This is intentional and used in a fire and forget manner
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<AppId>")] public uint AppId { get; set; }
    }
}