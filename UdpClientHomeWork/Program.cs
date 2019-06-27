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

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));

            using (StreamReader reader = new StreamReader("screen.png"))
            {
                string content = reader.ReadToEnd();
                byte[] buffer = Encoding.Default.GetBytes(content);

                while (true)
                {
                    EndPoint endPoint = new IPEndPoint(0, 0);
                    byte[] receiveBuffer = new byte[64 * 1024];
                    int size = socket.ReceiveFrom(receiveBuffer, ref endPoint);

                    if (size == 0)
                    {
                        continue;
                    }
                    Console.WriteLine(Encoding.UTF8.GetString(receiveBuffer));
                    socket.SendTo(buffer, endPoint);
                }
            }

        }
    }
}
