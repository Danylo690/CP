using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PC.Classes;
using System.Threading;
using System.Runtime.InteropServices;

namespace PC.Forms
{
    public partial class MainWindow : Form
    {
        private Server server { get; set; }
        private Client client { get; set; }
        private Thread thread;
        public MainWindow()
        {
            InitializeComponent();
            server = new Server();
            client = new Client();
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            TxtChatServer.AppendText("Waiting connection...\r\n");
            server.StartServerAsync();
            thread = new Thread(new ThreadStart(PrintMessageFromClient));
            thread.Start();
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            client.ConnectToServerAsync();
         //   NetworkStream ns = new NetworkStream(mySocket, true);
            TxtInfo.AppendText("Connected\r\n");
            if(client.ClientSocket.Connected == true)
            {
                BtnConnect.Enabled = false;
                DisconnectBtn.Enabled = true;
            }
        }

        private void DisconnectBtn_Click(object sender, EventArgs e)
        {
            client.DisconnectFromServerAsync();
            TxtInfo.AppendText("Disconnected from server\r\n");
            BtnConnect.Enabled = true;
            DisconnectBtn.Enabled = false;
            if (client.ClientSocket.Connected)
                TxtInfo.AppendText("We're still connnected\r\n");
            else
                TxtInfo.AppendText("We're disconnected\r\n");
            //client.Shutdown(SocketShutdown.Both);
            //client.Close();
        }
        private void PrintMessageFromClient()
        {
            while (true)
            {
                if (server.ClientSocket != null && server.Data != "" && server.Data != null)
                {
                    this.Invoke(new Action(() => TxtChatServer.AppendText(server.Data)));
                    server.Data = "";
                }
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
        }
    }
}
