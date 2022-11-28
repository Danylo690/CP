using PC.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Phone
{
    public partial class MainPage : ContentPage
    {
        #region Private fields
        private Server server { get; set; }
        private Client client { get; set; }
        private Thread thread { get; set; }
        #endregion

        #region Public fields
        #endregion

        #region Methods
        public MainPage()
        {
            client = new Client();
            server = new Server();
            InitializeComponent();
        }

        public void StartConnect(object sender, EventArgs e)
        {
            client.ConnectToServer(txtIpClient.Text, int.Parse(txtPortClient.Text));
        }

        public void StartDisconnect(object sender, EventArgs e)
        {
            client.DisconnectFromServer();
        }

        public async void PickAndShow(object sender, EventArgs e)
        {
            try
            {
                var pickedFile = await FilePicker.PickMultipleAsync();
                foreach (var file in pickedFile)
                {
                    if (file != null)
                    {
                        client.SendFile(file.FullPath);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
