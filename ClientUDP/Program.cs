using ConfigurationUDP;
using ClientUDP;

internal class Program
{
    // Client
    private static void Main(string[] args)
    {
        bool createdNew;
        using Mutex mutex = new Mutex(true, "ClientUDP", out createdNew);

        if (!createdNew)
        {
            mutex.Dispose();

            Console.WriteLine("ClientUDP is already running! Multiple instances are not allowed!");
            Console.WriteLine("Press any key to finish");
            Console.ReadKey();
            return;
        }

        ConfigurationManager configManager = new ConfigurationManager();
        var config = configManager.LoadMulticastConfig();

        string multicastAddress = config.Address;
        int serverPort = config.Port;

        MulticastUDPClient client =  new MulticastUDPClient(multicastAddress, serverPort);
        client.StartReceiving();
    }
}

//private static void Main(string[] args)
//{
//    int clientPort = 12346;

//    UdpClient udpClient = new UdpClient(clientPort);

//    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, clientPort);

//    Console.WriteLine("Клиент ожидает данные на порту " + clientPort);

//    while (true)
//    {
//        byte[] receivedData = udpClient.Receive(ref endPoint);
//        int number = BitConverter.ToInt32(receivedData, 0);
//        Console.WriteLine("Клиент получил число: " + number);

//        // Проверка нажатия клавиши Enter
//        if (Console.KeyAvailable)
//        {
//            ConsoleKey key = Console.ReadKey(intercept: true).Key;
//            if (key == ConsoleKey.Enter)
//            {
//                PrintData();
//            }
//            else
//            {
//                Console.WriteLine("Press Enter to print data");
//            }
//        }
//    }

//}