using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace PC.Classes
{
    internal class Client
    {
        #region Private fields
        private const int bufferSize = 1024;

        private NetworkStream clientStream { get; set; }
        #endregion

        #region Public fields
        public TcpClient client;
        #endregion

        #region Methods
        public void ConnectToServer()
        {
            try
            {
                Int32 port = 13000;
                client = new TcpClient("127.0.0.1", port);
                clientStream = client.GetStream();
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
            
            byte[] messageSent = Encoding.ASCII.GetBytes("D");
            clientStream.Write(messageSent, 0, messageSent.Length);
            clientStream.Close();
            client.Close();
            client = null;
        }

        public void SendFile()
        {
            byte[] SendingBuffer = null;
            string path = Environment.CurrentDirectory + "\\test.docx";
            FileStream Fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            byte[] messageSent = Encoding.ASCII.GetBytes($"Sending file {Fs.Name}");
            clientStream.Write(messageSent, 0, messageSent.Length);

            int NoOfPackets = Convert.ToInt32
                (Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(bufferSize)));
            int TotalLength = (int)Fs.Length, CurrentPacketLength;
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
                clientStream.Write(SendingBuffer, 0, SendingBuffer.Length);
            }
            Fs.Close();
        }
        #endregion
    }
}
