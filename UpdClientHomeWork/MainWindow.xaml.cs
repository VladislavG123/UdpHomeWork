using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UpdClientHomeWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            string ip = "127.0.0.1";
            int port = 12345;

            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            string text = "Hello UDP";
            socket.SendTo(Encoding.UTF8.GetBytes(text), remoteEndPoint);

            byte[] buffer = new byte[64 * 1024];
            socket.ReceiveFrom(buffer, ref remoteEndPoint);
        }
    }
}
