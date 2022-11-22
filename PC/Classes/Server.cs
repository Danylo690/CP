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
        #endregion

        #region Public fields
        public TcpListener server = null;

        public TcpClient client = null;

        public string Data;
        #endregion

        #region Methods
        public Server()
        {
            threadRecieveMsg = new Thread(RecieveMessage);
            //threadRecieveFile = new Thread(RecieveFile);
        }

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
                //RecieveMessage();
                threadRecieveMsg.Start();
                Data = $"User {client.Client.RemoteEndPoint} successfully connected\r\n";
            }
            catch (ObjectDisposedException)
            {

            }
        }

        public async void RecieveMessage()
        {
            streamMsg = client.GetStream();
            byte[] bytes = new Byte[1024];
            Data = null;
            int i;
            while ((i = streamMsg.Read(bytes, 0, bytes.Length)) != 0)
            {
                Data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                if (Data.IndexOf("D") == 0)
                {
                    Data = $"User {client.Client.RemoteEndPoint} successfully disconnected\r\n";
                    client.Close();
                    server.Start();
                    WaitForConnection();
                    break;
                }
                else
                {
                    //streamMsg.Close();
                    RecieveFile();
                }
            }
        }

        public async void RecieveFile()
        {
            //threadRecieveMsg.Abort();
            //NetworkStream stream = client.GetStream();
            byte[] RecData = new byte[bufferSize];
            int RecBytes;
            string path = Environment.CurrentDirectory + "\\test.docx";

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
            //streamMsg = client.GetStream();
        }
        #endregion
    }
}
