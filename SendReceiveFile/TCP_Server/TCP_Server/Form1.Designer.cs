using System;
using System.Collections;
using System.Net;
using System.Threading;

namespace TCP_Server
{
    partial class ServerForm
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
            this.lbConnections = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbConnections
            // 
            this.lbConnections.FormattingEnabled = true;
            this.lbConnections.ItemHeight = 16;
            this.lbConnections.Location = new System.Drawing.Point(20, 50);
            this.lbConnections.Name = "lbConnections";
            this.lbConnections.Size = new System.Drawing.Size(360, 260);
            this.lbConnections.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(82, 17);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Server Status";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lbConnections);
            this.Name = "Form1";
            this.Text = "TCP Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            lblStatus.Text = "My IP address is " +
           IPHost.AddressList[0].ToString();
            alSockets = new ArrayList();
            Thread thdListener = new Thread(new
           ThreadStart(listenerThread));
            thdListener.Start();
        }


        #endregion

        private System.Windows.Forms.ListBox lbConnections;
        private System.Windows.Forms.Label lblStatus;
    }
}
