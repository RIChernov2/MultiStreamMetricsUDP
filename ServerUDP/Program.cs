using ConfigurationUDP;
using ServerUDP;
using ServerUDP.Instances;

internal class Program
{
    // Server
    private static void Main(string[] args)
    {
        RestartApp();
    }

    private static void RestartApp()
    {
        bool createdNew;
        using Mutex mutex = new Mutex(true, "ServerUDP", out createdNew);

        if (!createdNew)
        {
            mutex.Dispose();

            Console.WriteLine("ServerUDP is already running! Multiple instances are not allowed!");
            Console.WriteLine("Press any key to finish");
            Console.ReadKey();
            return;
        }

        try
        {
            ConfigurationManager configManager = new ConfigurationManager();
            var config = configManager.LoadMulticastConfig();
            var random = configManager.LoadRandomConfig();

            string multicastAddress = config.Address;
            int serverPort = config.Port;

            RandomValueGeneretor generetor = new RandomValueGeneretor(random.MinValue, random.MaxValue);

            MulticastUDPServer server = new MulticastUDPServer(multicastAddress, serverPort, generetor);
            server.StartSending();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine("Restarting the application...");
            mutex.Dispose();
            RestartApp();
        }
    }
}
