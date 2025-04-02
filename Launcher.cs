using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LionsGate;
public sealed class RiotClient
{
    public static Process? Launch(IEnumerable<string>? args = null)
    {
        var path = GetPath();
        if (path is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" [ERROR] Unable to find Riot Client installation path, cannot start.");
            Console.ResetColor();
            return null;
        }

        IEnumerable<string> allArgs = [$"--client-config-url=http://127.0.0.1:{LeagueProxy.ConfigPort}", "--launch-product=lion", "--launch-patchline=live", .. args ?? []];

        return Process.Start(path, allArgs);
    }

    private static string? GetPath()
    {
        string installPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                           "Riot Games/RiotClientInstalls.json");

        if (File.Exists(installPath))
        {
            try
            {
                var data = JsonSerializer.Deserialize<JsonNode>(File.ReadAllText(installPath));
                var rcPaths = new List<string?>
            {
                data?["rc_default"]?.ToString(),
                data?["rc_live"]?.ToString(),
                data?["rc_beta"]?.ToString()
            };

                var validPath = rcPaths.FirstOrDefault(File.Exists);
                if (validPath != null)
                    return validPath;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($" [WARN] An error occurred while processing the install path, using fallback path: {ex.Message}");
                Console.ResetColor();
            }
        }

        foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
        {
            var potentialPath = Path.Combine(drive.RootDirectory.FullName, "Riot Games", "Riot Client", "RiotClientServices.exe");
            if (File.Exists(potentialPath))
            {
                Console.WriteLine($" [DEBUG] Found RiotClient fallback path found at {drive}{potentialPath}");
                return potentialPath;
            }
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(" [ERROR] Failed to locate Riot Client installation path from both fallback method and RiotClientInstalls.");
        Console.ResetColor();
        return null;
    }
}