using System;
using System.Net;
using System.Net.Sockets;

namespace PC.Classes
{
    public class Server
    {
        #region Private fields

        #endregion

        #region Public fields
        public TcpListener server = null;

        public TcpClient client = null;

        public string Data;
        #endregion

        #region Methods
        public void StartServer()
        {
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();
            WaitForConnection();
        }

        public async void WaitForConnection()
        {
            try
            {
                client = await server.AcceptTcpClientAsync();
                server.Stop();
                RecieveMessage();
                Data = $"User {client.Client.RemoteEndPoint} successfully connected\r\n";
            }
            catch (ObjectDisposedException)
            {

            }
        }

        public async void RecieveMessage()
        {
            NetworkStream stream = client.GetStream();
            byte[] bytes = new Byte[1024];
            Data = null;
            int i;
            while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
            {
                Data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                if (Data.IndexOf("D") > -1)
                {
                    Data = $"User {client.Client.RemoteEndPoint} successfully disconnected\r\n";
                    client.Close();
                    server.Start();
                    WaitForConnection();
                    break;
                }
            }
        }
        #endregion
    }
}
