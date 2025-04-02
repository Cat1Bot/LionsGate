using System.Text.Json;
using System.Text.Json.Nodes;

namespace LionsGate
{
    class Program
    {
        private static readonly string latestReleaseUrl = "https://api.github.com/repos/Cat1Bot/LionsGate/releases/latest";
        private static readonly string currentVersion = "0.9.7";

        public static async Task Main(string[] args)
        {
            await CheckForUpdatesAsync();

            var leagueProxy = new LeagueProxy();

            await leagueProxy.Start();
            Console.WriteLine(" [INFO] Starting RCS process...");
            var process = leagueProxy.LaunchRCS(args);
            if (process is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" [ERROR] Failed to launch RCS process. Try running this app as administrator.");
                Console.ResetColor();
                leagueProxy.Stop();
                return;
            }

            Console.WriteLine(" [INFO] Started RCS process.");

            await process.WaitForExitAsync();
            Console.WriteLine(" [INFO] RCS process exited");
            leagueProxy.Stop();
        }


        public static async Task CheckForUpdatesAsync()
        {
            try
            {
                using var handler = new HttpClientHandler
                {
                    UseCookies = false,
                    UseProxy = false,
                    Proxy = null,
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    CheckCertificateRevocationList = false
                };

                using var client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("User-Agent", $"LionsGate/{currentVersion}");
                var response = await client.GetAsync(latestReleaseUrl);
                var content = await response.Content.ReadAsStringAsync();
                var release = JsonSerializer.Deserialize<JsonNode>(content);

                var latestVersion = release?["tag_name"]?.ToString();

                if (latestVersion is null)
                    return;

                var githubVersion = new Version(latestVersion.Replace("v", ""));
                var assemblyVersion = new Version(currentVersion.Replace("v", ""));

                if (assemblyVersion.CompareTo(githubVersion) != -1)
                    return;

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" [IMPORTANT] A new version ({latestVersion}) is available! Visit https://github.com/Cat1Bot/LionsGate/releases to get the latest version.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(" [WARN] Failed to check for updates: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}