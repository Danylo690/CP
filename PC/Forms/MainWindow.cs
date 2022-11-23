using System;
using System.Windows.Forms;
using PC.Classes;
using System.Threading;

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
                if (server.Data != "" && server.Data != null)
                {
                    this.Invoke(new Action(() => TxtChatServer.AppendText(server.Data)));
                    server.Data = "";
                }
            }
        }

        private void BtnStartServer_Click(object sender, EventArgs e)
        {
            TxtChatServer.AppendText("Waiting connection...\r\n");
            server.StartServer();
            thread = new Thread(new ThreadStart(PrintMessageFromClient));
            thread.Start();

            BtnStartServer.Enabled = false;
            BtnStopServer.Enabled = true;
        }

        private void BtnConnectClient_Click(object sender, EventArgs e)
        {
            client.ConnectToServer();
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

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                client.SendFile(FileDialog.FileName);
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
