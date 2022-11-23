using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PC.Classes
{
    public class Server
    {
        #region Private fields
        private const int bufferSize = 1024;

        private Thread threadWaitForConnection;

        private NetworkStream streamMsg;

        private string fileName;
        #endregion

        #region Public fields
        public TcpListener server = null;

        public TcpClient client = null;

        public string Data;

        public TextBox TxtMsgServer;
        #endregion

        #region Methods
        public void StartServer()
        {
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();
            threadWaitForConnection = new Thread(WaitForConnection);
            threadWaitForConnection.Start();
        }

        public void WaitForConnection()
        {
            try
            {
                client = server.AcceptTcpClient();
                server.Stop();
                Data = $"User {client.Client.RemoteEndPoint} successfully connected\r\n";
                TxtMsgServer.AppendText(Data);
                RecieveMessage();

            }
            catch (SocketException ex) when (ex.ErrorCode == 10004)
            {
                Data = "Server stopped\n\r";
                TxtMsgServer.AppendText(Data);
            }
            catch (Exception ex)
            {
                string message = $"{ex.Message}";
                string caption = $"Exception";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
            }
        }

        public void RecieveMessage()
        {
            try
            {
                streamMsg = client.GetStream();
                byte[] bytes = new Byte[bufferSize];
                int i;
                while ((i = streamMsg.Read(bytes, 0, bytes.Length)) != 0)
                {
                    Data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                    SendMsg("<Sent>");

                    if (Data.IndexOf("<Disconnect>") > -1)
                    {
                        Data = $"User {client.Client.RemoteEndPoint} successfully disconnected\r\n";
                        TxtMsgServer.AppendText(Data);

                        streamMsg.Close();
                        client.Close();
                        client = null;
                        server.Start();
                        WaitForConnection();
                        Data = "";
                        break;
                    }
                    else
                    {
                        TxtMsgServer.AppendText(Data);

                        i = streamMsg.Read(bytes, 0, bytes.Length);
                        fileName = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                        SendMsg("<Sent>");
                        RecieveFile(fileName);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"{ex.Message}";
                string caption = "Exception";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
            }
        }

        public void RecieveFile(string fileName)
        {
            byte[] RecData = new Byte[bufferSize];
            int RecBytes;
            string path = Environment.CurrentDirectory + $"\\{fileName}";
            using (FileStream Fs = new FileStream(path, FileMode.Create, FileAccess.Write))
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
            Data = "File sent\r\n";
            TxtMsgServer.AppendText(Data);

            SendMsg("<Sent>");
            RecieveMessage();
        }
        public void SendMsg(string Msg)
        {
            byte[] messageSent = Encoding.UTF8.GetBytes(Msg);
            streamMsg.Write(messageSent, 0, messageSent.Length);
        }

        public void StopServerWork()
        {
            if (client != null)
            {
                client.Close();
                streamMsg.Close();
            }
            if (server != null)
            {
                server.Stop();
            }
        }
        #endregion
    }
}
