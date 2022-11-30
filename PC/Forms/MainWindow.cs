using System;
using System.Windows.Forms;
using PC.Classes;
using System.Threading;
using System.Net;

namespace PC.Forms
{
    public partial class MainWindow : Form
    {
        #region Private fields
        private Server server { get; set; }
        private Client client { get; set; }

        private Thread thread { get; set; }
        #endregion

        #region Public fields
        #endregion

        #region Methods
        public MainWindow()
        {
            InitializeComponent();
            server = new Server();
            client = new Client();
        }

        private void PrintMessageFromClient()
        {
            while (true)
            {
                this.Invoke(new Action(() => TxtChatServer.Text = server.Logs));
            }
        }

        private void BtnStartServer_Click(object sender, EventArgs e)
        {
            server.Logs += "Waiting connection...\r\n";
            server.StartServer(TxtServerIP.Text);
            thread = new Thread(new ThreadStart(PrintMessageFromClient));
            thread.Start();

            BtnStartServer.Enabled = false;
            BtnStopServer.Enabled = true;
        }

        private void BtnConnectClient_Click(object sender, EventArgs e)
        {
            IPAddress address;
            string ipAddress = "";
            Int32 port = -1;
            while (ipAddress == "" && port == -1)
            {
                try
                {
                    IPAddress.TryParse(TxtIpClient.Text, out address);
                    ipAddress = TxtIpClient.Text;
                    Int32.TryParse(TxtPortClient.Text, out port);
                    client.ConnectToServer(ipAddress, port);
                }
                catch (Exception)
                {
                    ipAddress = "";
                    port = -1;
                }
            }
            if (client.client != null && client.client.Connected == true)
            {
                TxtInfo.AppendText("Connected\r\n");
            }
            else
            {
                TxtInfo.AppendText("Connection error\r\n");
            }

            if(client.client != null && client.client.Connected == true)
            {
                BtnConnectClient.Enabled = false;
                BtnDisconnectClient.Enabled = true;
            }
        }

        private void BtnDisconnectClient_Click(object sender, EventArgs e)
        {
            client.DisconnectFromServer();
            TxtInfo.AppendText("Disconnected from server\r\n");
            BtnConnectClient.Enabled = true;
            BtnDisconnectClient.Enabled = false;
        }

        private void BtnStopServer_Click(object sender, EventArgs e)
        {
            server.StopServerWork();
            thread.Abort();
            BtnStartServer.Enabled = true;
            BtnStopServer.Enabled = false;
        }

        private void BtnSendClient_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog
            {
                //InitialDirectory = @"D:\",
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                //DefaultExt = "txt",
                Filter = "All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,
                Multiselect = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < FileDialog.FileNames.Length; i++)
                {
                    client.SendFile(FileDialog.FileNames[i]);
                }
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
            server.StopServerWork();
            if (client.client != null)
            {
                client.client.Close();
            }
        }
        #endregion
    }
}
