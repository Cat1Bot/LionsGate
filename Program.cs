namespace LionsGate
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var leagueProxy = new LeagueProxy();

            await leagueProxy.Start();

            var process = leagueProxy.LaunchRCS(args);
            if (process is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" [ERROR] Failed to launch RCS process. Try running this app is adminisrator.");
                Console.ResetColor();
                leagueProxy.Stop();
                return;
            }

            Console.WriteLine(" [INFO] Started RCS process.");

            await process.WaitForExitAsync();
            Console.WriteLine(" [INFO] RCS process exited");
            leagueProxy.Stop();
        }
    }
}