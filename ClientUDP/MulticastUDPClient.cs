namespace ClientUDP
{
    using System;
    using System.Net.Sockets;
    using System.Net;
    using System.Text.Json;
    using ConfigurationUDP.Instances;
    using ClientUDP.Instances;

    public class MulticastUDPClient
    {
        private static MulticastUDPClient? s_instance;
        private readonly UdpClient _udpClient;
        private IPEndPoint _endPoint;
        private ClientMetricTracker _tracker;
        private object _lock = new object();

        public MulticastUDPClient(string multicastAddress, int port)
        {
            _udpClient = new UdpClient(port);
            _udpClient.JoinMulticastGroup(IPAddress.Parse(multicastAddress));
            _endPoint = new IPEndPoint(IPAddress.Any, port);
            _tracker = new ClientMetricTracker();
        }

        public void StartReceiving()
        {
            Console.WriteLine("Client connected to the multicast group and waiting for data...");
            Console.WriteLine("Press Enter to print data");

            Task.Run(() => HandleUserInput());

            while (true)
            {
                byte[] receivedData = _udpClient.Receive(ref _endPoint);

                try
                {
                    DataPackage dataPackage = JsonSerializer.Deserialize<DataPackage>(receivedData);
                    //lock (_lock)
                    //{
                    //    Console.WriteLine($"Client received number: {dataPackage.Value:F4} with number {dataPackage.Number}");
                    //}
                    Task.Run(() => _tracker.AddValues(dataPackage));
                }
                catch (Exception e)
                {
                    lock (_lock)
                    {
                        Console.WriteLine($"An exception was thrown: {e.Message}");
                    }
                }

            }
        }

        private void HandleUserInput()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {

                    string message;
                    ConsoleKey key = Console.ReadKey(intercept: true).Key;
                    if (key == ConsoleKey.Enter)
                    {
                        message = _tracker.GetStatisticsMessage();
                    }
                    else
                    {
                        message = "____ Press Enter to print data";
                    }


                    lock (_lock)
                    {
                        Console.WriteLine(message);
                    }
                }
            }
        }
    }
}
