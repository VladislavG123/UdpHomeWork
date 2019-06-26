using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpServerHomeWork
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\гороховв.CORP\source\repos\UdpClientHomeWork\UdpClientHomeWork\bin\Debug\screen.png";
            ScreenCapturer.CaptureAndSave(path, CaptureMode.Screen);

            Socket serverUdp = new Socket(
                            AddressFamily.InterNetwork,
                            SocketType.Dgram,
                            ProtocolType.Udp);
            string ip = "0.0.0.0";
            int port = 12345;

            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip),
            port);

            serverUdp.Bind(serverEndPoint);
            Console.WriteLine("Сервер запущен");

            string content;
            using (var reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
            }

            byte[] buffer = Encoding.Default.GetBytes(content); /*new byte[64 * 1024];*/

            EndPoint clientEndPoint = new IPEndPoint(0, 0);
            Console.WriteLine("Ожидание клиента");
            while (true)
            {
                int receiveSize = serverUdp.ReceiveFrom(buffer, ref clientEndPoint);
                Console.WriteLine("Получено сообщение от клиента " + receiveSize);
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, receiveSize));

            }


        }
    }
}
