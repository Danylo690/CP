using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PC.Classes
{
    internal class Client
    {
        #region Private fields
        private const int bufferSize = 1024;

        private NetworkStream clientStream { get; set; }

        private Thread threadServerCheck;

        private bool IsMsgSent;
        #endregion

        #region Public fields
        public TcpClient client;
        #endregion

        #region Methods
        public void ConnectToServer(string ipAddress, Int32 port)
        {
            try
            {
                client = new TcpClient(ipAddress, port);
                clientStream = client.GetStream();
                threadServerCheck = new Thread(MsgRecieved);
                threadServerCheck.Start();
            }
            catch (SocketException)
            {
                //string message = "One user is already connected.";
                //string caption = "Connection error";
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //MessageBox.Show(message, caption, buttons);
            }
        }

        public void DisconnectFromServer()
        {
            SendMsg("<Disconnect>");
            StopClientWork();
        }

        public void SendFile(string path)
        {
            byte[] SendingBuffer = null;
            FileStream Fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            SendMsg($"Sending file {Path.GetFileName(Fs.Name)}\r\n");

            SendMsg(Path.GetFileName(Fs.Name));

            int NoOfPackets = Convert.ToInt32
                (Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(bufferSize)));
            int TotalLength = (int)Fs.Length, CurrentPacketLength;

            SendMsg(Path.GetFileName($"{NoOfPackets}"));

            for (int i = 0; i < NoOfPackets; i++)
            {
                if (TotalLength > bufferSize)
                {
                    CurrentPacketLength = bufferSize;
                    TotalLength = TotalLength - CurrentPacketLength;
                }
                else
                {
                    CurrentPacketLength = TotalLength;
                }
                SendingBuffer = new byte[CurrentPacketLength];
                Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                IsMsgSent = false;
                clientStream.Write(SendingBuffer, 0, SendingBuffer.Length);

                WaitUntilSentMsg();
            }
            SendMsg("<End file sent>");
            Fs.Close();
        }

        public void SendMsg(string data)
        {
            byte[] messageSent = Encoding.UTF8.GetBytes(data);
            IsMsgSent = false;
            clientStream.Write(messageSent, 0, messageSent.Length);
            WaitUntilSentMsg();
        }

        public void MsgRecieved()
        {
            byte[] Msg = new byte[bufferSize];
            while (true)
            {
                try
                {
                    int i = clientStream.Read(Msg, 0, Msg.Length);
                    string Data = System.Text.Encoding.UTF8.GetString(Msg, 0, i);
                    if (Data.IndexOf("<Sent>") > -1)
                    {
                        IsMsgSent = true;
                    }
                    if (Data.IndexOf("<Server stopped>") > -1)
                    {
                        StopClientWork();
                    }
                }
                catch (Exception)
                {

                }

            }
        }

        public void WaitUntilSentMsg()
        {
            while (true)
            {
                if (IsMsgSent)
                {
                    break;
                }
            }
        }

        public void StopClientWork()
        {
            if (client != null)
            {
                clientStream.Close();
                client.Close();
                client = null;
            }
        }
        #endregion
    }
}