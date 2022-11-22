using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace PC.Classes
{
    internal class Client
    {
        #region Private fields
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
            NetworkStream stream = client.GetStream();
            byte[] messageSent = Encoding.ASCII.GetBytes("D");
            stream.Write(messageSent, 0, messageSent.Length);
            stream.Close();
            client.Close();
            client = null;
        }
        #endregion
    }
}
