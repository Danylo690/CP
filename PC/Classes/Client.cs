using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PC.Classes
{
    internal class Client
    {
        public Socket ClientSocket;
        public Client()
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public async void ConnectToServerAsync()
        {
            await ClientSocket.ConnectAsync("127.0.0.1", 8888);
        }
        public void DisconnectFromServerAsync() 
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("D");
            int byteSent = ClientSocket.Send(messageSent);
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Disconnect(true);
        }   
    }
}
