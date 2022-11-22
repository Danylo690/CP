using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace PC.Classes
{
    public class Server
    {
        #region Private fields
        private const int bufferSize = 1024;

        private Thread threadRecieveMsg;

        private NetworkStream streamMsg;

        private string fileName;
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
                threadRecieveMsg = new Thread(RecieveMessage);
                threadRecieveMsg.Start();
                Data = $"User {client.Client.RemoteEndPoint} successfully connected\r\n";
            }
            catch (ObjectDisposedException)
            {

            }
        }

        public void RecieveMessage()
        {
            streamMsg = client.GetStream();
            byte[] bytes = new Byte[bufferSize];
            Data = null;
            int i;
            while ((i = streamMsg.Read(bytes, 0, bytes.Length)) != 0)
            {
                Data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                if (Data.IndexOf("<Disconnect>") > -1)
                {
                    Data = $"User {client.Client.RemoteEndPoint} successfully disconnected\r\n";
                    client.Close();
                    server.Start();
                    WaitForConnection();
                    break;
                }
                else
                {
                    i = streamMsg.Read(bytes, 0, bytes.Length);
                    fileName = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                    RecieveFile(fileName);
                }
            }
            threadRecieveMsg.Abort();
        }

        public void RecieveFile(string fileName)
        {
            byte[] RecData = new Byte[bufferSize];
            int RecBytes;
            string path = Environment.CurrentDirectory + $"\\{fileName}";
            using (FileStream Fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                while ((RecBytes = streamMsg.Read(RecData, 0, RecData.Length)) > 0)
                {
                    Fs.Write(RecData, 0, RecBytes);
                    if (RecBytes < 1024)
                    {
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
