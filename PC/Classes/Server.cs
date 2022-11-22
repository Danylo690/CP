using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PC.Classes
{
    public class Server
    {
        public Socket Listener;
        public IPEndPoint IpPoint;
        public Socket ClientSocket;
        public String Data;
        //private Thread thread;
        public Server()
        {
            IpPoint = new IPEndPoint(IPAddress.Any, 8888);
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //thread = new Thread(RecieveMessage);
        }

        public async void StartServerAsync()
        {
            Listener.Bind(IpPoint);
            Listener.Listen(10);
            await WaitForConnectionAsync();
        }

        public async Task WaitForConnectionAsync()
        {
            ClientSocket = await Listener.AcceptAsync();
            RecieveMessage();
            Data = $"User {ClientSocket.RemoteEndPoint} successfully connected\r\n";
        }

        public async void RecieveMessage()
        {
            byte[] bytes = new Byte[1024];
            ArraySegment<byte> buffer = new ArraySegment<byte>(bytes);
            Data = null;
            if (ClientSocket != null)
            {
                while (true)
                {
                    //ArraySegment<byte> buffer = new(readBuffer, 0, 1024);

                    int numByte = await ClientSocket.ReceiveAsync(buffer, SocketFlags.None);
                    Data = Encoding.ASCII.GetString(buffer.Array, 0, numByte);
                    if (Data.IndexOf("D") > -1)
                    {
                        ClientSocket.Shutdown(SocketShutdown.Both);
                        ClientSocket.Close();
                        await WaitForConnectionAsync();
                        break;
                    }
                }
            }
        }
    }
}
