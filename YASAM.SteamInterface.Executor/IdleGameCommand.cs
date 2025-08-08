using System.Reflection;
using System.Text;
using Spectre.Console;
using Spectre.Console.Cli;
using Steamworks;

public class IdleGameCommand : AsyncCommand<IdleGameCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[AppId]")] public uint AppId { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"[grey]IdleGameCommand for app id: {settings.AppId}[/]");

        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var fileName = "steam_appid.txt";

        var combinedPath = Path.Combine(appPath, fileName);
        if (File.Exists(combinedPath)) File.Delete(combinedPath);

        using var filestream = new FileStream(combinedPath, FileMode.CreateNew);
        using var writer = new StreamWriter(filestream, Encoding.ASCII);
        writer.Write(settings.AppId);
        writer.Flush();
        writer.Close();


        AppDomain.CurrentDomain.ProcessExit += (_, __) =>
        {
            AnsiConsole.MarkupLine("[red] Exiting[/]");
            SteamAPI.Shutdown();
        };


        Environment.SetEnvironmentVariable("SteamAppId", settings.AppId.ToString());

        SteamAPI.InitEx(out var msg);
        Console.WriteLine(msg);

        if (!string.IsNullOrWhiteSpace(msg))
        {
            AnsiConsole.MarkupLine($"[red] Error occured: {msg}[/]");
            throw new Exception(msg);
        }

        AnsiConsole.MarkupLine("[green] Successfully idling game[/]");
        
        while (true) Thread.Sleep(1000);
    }
}