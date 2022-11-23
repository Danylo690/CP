namespace PC.Forms
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerLabel = new System.Windows.Forms.Label();
            this.TxtIpClient = new System.Windows.Forms.TextBox();
            this.BtnConnectClient = new System.Windows.Forms.Button();
            this.TxtInfo = new System.Windows.Forms.TextBox();
            this.MsgLabel = new System.Windows.Forms.Label();
            this.TxtMsg = new System.Windows.Forms.TextBox();
            this.BtnSendClient = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtServerIP = new System.Windows.Forms.TextBox();
            this.TxtChatServer = new System.Windows.Forms.TextBox();
            this.BtnStartServer = new System.Windows.Forms.Button();
            this.BtnSendServer = new System.Windows.Forms.Button();
            this.BtnDisconnectClient = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtMsgServer = new System.Windows.Forms.TextBox();
            this.BtnStopServer = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtPortClient = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(16, 15);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(75, 16);
            this.ServerLabel.TabIndex = 0;
            this.ServerLabel.Text = "IP address:";
            // 
            // TxtIpClient
            // 
            this.TxtIpClient.Location = new System.Drawing.Point(97, 12);
            this.TxtIpClient.Name = "TxtIpClient";
            this.TxtIpClient.Size = new System.Drawing.Size(313, 22);
            this.TxtIpClient.TabIndex = 1;
            this.TxtIpClient.Text = "192.168.1.107";
            // 
            // BtnConnectClient
            // 
            this.BtnConnectClient.Location = new System.Drawing.Point(239, 419);
            this.BtnConnectClient.Name = "BtnConnectClient";
            this.BtnConnectClient.Size = new System.Drawing.Size(75, 23);
            this.BtnConnectClient.TabIndex = 2;
            this.BtnConnectClient.Text = "Connect";
            this.BtnConnectClient.UseVisualStyleBackColor = true;
            this.BtnConnectClient.Click += new System.EventHandler(this.BtnConnectClient_Click);
            // 
            // TxtInfo
            // 
            this.TxtInfo.Location = new System.Drawing.Point(97, 74);
            this.TxtInfo.Multiline = true;
            this.TxtInfo.Name = "TxtInfo";
            this.TxtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtInfo.Size = new System.Drawing.Size(313, 308);
            this.TxtInfo.TabIndex = 1;
            // 
            // MsgLabel
            // 
            this.MsgLabel.AutoSize = true;
            this.MsgLabel.Location = new System.Drawing.Point(24, 394);
            this.MsgLabel.Name = "MsgLabel";
            this.MsgLabel.Size = new System.Drawing.Size(67, 16);
            this.MsgLabel.TabIndex = 0;
            this.MsgLabel.Text = "Message:";
            // 
            // TxtMsg
            // 
            this.TxtMsg.Location = new System.Drawing.Point(97, 391);
            this.TxtMsg.Name = "TxtMsg";
            this.TxtMsg.Size = new System.Drawing.Size(313, 22);
            this.TxtMsg.TabIndex = 1;
            // 
            // BtnSendClient
            // 
            this.BtnSendClient.Location = new System.Drawing.Point(158, 419);
            this.BtnSendClient.Name = "BtnSendClient";
            this.BtnSendClient.Size = new System.Drawing.Size(75, 23);
            this.BtnSendClient.TabIndex = 2;
            this.BtnSendClient.Text = "Send";
            this.BtnSendClient.UseVisualStyleBackColor = true;
            this.BtnSendClient.Click += new System.EventHandler(this.BtnSendClient_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(534, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // TxtServerIP
            // 
            this.TxtServerIP.Location = new System.Drawing.Point(590, 46);
            this.TxtServerIP.Name = "TxtServerIP";
            this.TxtServerIP.Size = new System.Drawing.Size(313, 22);
            this.TxtServerIP.TabIndex = 1;
            this.TxtServerIP.Text = "192.168.1.107: 13000";
            // 
            // TxtChatServer
            // 
            this.TxtChatServer.Location = new System.Drawing.Point(590, 74);
            this.TxtChatServer.Multiline = true;
            this.TxtChatServer.Name = "TxtChatServer";
            this.TxtChatServer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtChatServer.Size = new System.Drawing.Size(313, 308);
            this.TxtChatServer.TabIndex = 1;
            // 
            // BtnStartServer
            // 
            this.BtnStartServer.Location = new System.Drawing.Point(747, 419);
            this.BtnStartServer.Name = "BtnStartServer";
            this.BtnStartServer.Size = new System.Drawing.Size(75, 23);
            this.BtnStartServer.TabIndex = 2;
            this.BtnStartServer.Text = "Start";
            this.BtnStartServer.UseVisualStyleBackColor = true;
            this.BtnStartServer.Click += new System.EventHandler(this.BtnStartServer_Click);
            // 
            // BtnSendServer
            // 
            this.BtnSendServer.Location = new System.Drawing.Point(666, 419);
            this.BtnSendServer.Name = "BtnSendServer";
            this.BtnSendServer.Size = new System.Drawing.Size(75, 23);
            this.BtnSendServer.TabIndex = 2;
            this.BtnSendServer.Text = "Send";
            this.BtnSendServer.UseVisualStyleBackColor = true;
            // 
            // BtnDisconnectClient
            // 
            this.BtnDisconnectClient.Enabled = false;
            this.BtnDisconnectClient.Location = new System.Drawing.Point(320, 419);
            this.BtnDisconnectClient.Name = "BtnDisconnectClient";
            this.BtnDisconnectClient.Size = new System.Drawing.Size(90, 23);
            this.BtnDisconnectClient.TabIndex = 2;
            this.BtnDisconnectClient.Text = "Disconnect";
            this.BtnDisconnectClient.UseVisualStyleBackColor = true;
            this.BtnDisconnectClient.Click += new System.EventHandler(this.BtnDisconnectClient_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 394);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Message:";
            // 
            // TxtMsgServer
            // 
            this.TxtMsgServer.Location = new System.Drawing.Point(590, 391);
            this.TxtMsgServer.Name = "TxtMsgServer";
            this.TxtMsgServer.Size = new System.Drawing.Size(313, 22);
            this.TxtMsgServer.TabIndex = 1;
            // 
            // BtnStopServer
            // 
            this.BtnStopServer.Enabled = false;
            this.BtnStopServer.Location = new System.Drawing.Point(828, 419);
            this.BtnStopServer.Name = "BtnStopServer";
            this.BtnStopServer.Size = new System.Drawing.Size(75, 23);
            this.BtnStopServer.TabIndex = 3;
            this.BtnStopServer.Text = "Stop";
            this.BtnStopServer.UseVisualStyleBackColor = true;
            this.BtnStopServer.Click += new System.EventHandler(this.BtnStopServer_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Port:";
            // 
            // TxtPortClient
            // 
            this.TxtPortClient.Location = new System.Drawing.Point(97, 43);
            this.TxtPortClient.Name = "TxtPortClient";
            this.TxtPortClient.Size = new System.Drawing.Size(313, 22);
            this.TxtPortClient.TabIndex = 1;
            this.TxtPortClient.Text = "13000";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 517);
            this.Controls.Add(this.BtnStopServer);
            this.Controls.Add(this.BtnSendServer);
            this.Controls.Add(this.BtnSendClient);
            this.Controls.Add(this.BtnStartServer);
            this.Controls.Add(this.BtnDisconnectClient);
            this.Controls.Add(this.BtnConnectClient);
            this.Controls.Add(this.TxtChatServer);
            this.Controls.Add(this.TxtMsgServer);
            this.Controls.Add(this.TxtInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtMsg);
            this.Controls.Add(this.TxtServerIP);
            this.Controls.Add(this.MsgLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtPortClient);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtIpClient);
            this.Controls.Add(this.ServerLabel);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.TextBox TxtIpClient;
        private System.Windows.Forms.Button BtnConnectClient;
        private System.Windows.Forms.TextBox TxtInfo;
        private System.Windows.Forms.Label MsgLabel;
        private System.Windows.Forms.TextBox TxtMsg;
        private System.Windows.Forms.Button BtnSendClient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtServerIP;
        private System.Windows.Forms.TextBox TxtChatServer;
        private System.Windows.Forms.Button BtnStartServer;
        private System.Windows.Forms.Button BtnSendServer;
        private System.Windows.Forms.Button BtnDisconnectClient;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtMsgServer;
        private System.Windows.Forms.Button BtnStopServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtPortClient;
    }
}