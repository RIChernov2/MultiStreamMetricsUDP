namespace ServerUDP
{
    using System;
    using System.Net.Sockets;
    using System.Net;
    using System.Text.Json;
    using ConfigurationUDP.Instances;
    using ServerUDP.Instances.Interfaces;

    public class MulticastUDPServer
    {
        private static MulticastUDPServer? s_instance;
        private readonly UdpClient _udpServer;
        private readonly IPEndPoint _endPoint;
        private readonly IValueGeneretor _generator;
        private static int s_packetNumber = 0;

        public MulticastUDPServer(string multicastAddress, int port, IValueGeneretor generetor)
        {
            _udpServer = new UdpClient();
            _udpServer.JoinMulticastGroup(IPAddress.Parse(multicastAddress));
            _endPoint = new IPEndPoint(IPAddress.Parse(multicastAddress), port);
            _generator = generetor;
        }

        public void StartSending()
        {
            Console.WriteLine($"Server started, sending to multicast{_endPoint.Address}:{_endPoint.Port}");
            Random rand = new Random();

            while (true)
            {
                double value = _generator.Next();
                DataPackage dataPackage = new DataPackage(s_packetNumber, value);
                byte[] data = JsonSerializer.SerializeToUtf8Bytes(dataPackage);

                _udpServer.Send(data, data.Length, _endPoint);
                Console.WriteLine($"Server sent number: {value:F4} with number {s_packetNumber}");

                s_packetNumber++;

                if (s_packetNumber % 100_000 == 0) throw new Exception("Тест");
            }
        }
    }
}
