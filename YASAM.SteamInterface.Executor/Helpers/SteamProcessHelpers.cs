using System.Reflection;
using System.Text;

namespace YASAM.SteamInterface.Executor.Helpers;

public static class SteamProcessHelpers
{
    internal static async Task SetupSteamAppIdTextFile(ulong appId)
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var fileName = "steam_appid.txt";

        var combinedPath = Path.Combine(appPath!, fileName);
        if (File.Exists(combinedPath)) File.Delete(combinedPath);

        using var filestream = new FileStream(combinedPath, FileMode.CreateNew);
        using var writer = new StreamWriter(filestream, Encoding.ASCII);

        await writer.WriteAsync(appId.ToString());
        await writer.FlushAsync();
        writer.Close();
    }

    internal static void SetEnvionmentVariable(ulong appId)
    {
        Environment.SetEnvironmentVariable("SteamAppId", appId.ToString());
    }
}