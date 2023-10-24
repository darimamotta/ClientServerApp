using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientServerApp
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            int port = 8005;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);
                Console.WriteLine("Server was started. Please wait for connection...");
                Socket handler = listenSocket.Accept();
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[256]; // буфер для получаемых данных
                StringBuilder builder = new StringBuilder();
                do
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (handler.Available > 0);
                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                // отправка ответа
                string message = "Your message was delivered";
                data = Encoding.Unicode.GetBytes(message);
                handler.Send(data);
                // закрытие сокета
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}