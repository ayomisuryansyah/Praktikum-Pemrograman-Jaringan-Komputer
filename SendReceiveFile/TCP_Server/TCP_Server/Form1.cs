using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TCP_Server
{
    public partial class ServerForm : System.Windows.Forms.Form
    {
        // List untuk menyimpan semua socket koneksi
        private ArrayList alSockets;

        public ServerForm()
        {
            InitializeComponent();
        }

        // Event yang dijalankan ketika form dimuat
        private void ServerForm_Load(object sender, EventArgs e)
        {
            // Mendapatkan IP Address lokal dan menampilkannya
            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
            lblStatus.Text = "Server IP Address: " + IPHost.AddressList[0].ToString();

            // Inisialisasi list untuk menyimpan socket koneksi
            alSockets = new ArrayList();

            // Memulai thread untuk mendengarkan koneksi client
            Thread thdListener = new Thread(new ThreadStart(listenerThread));
            thdListener.Start();
        }

        // Method to safely update the ListBox
        private void UpdateListBox(string message)
        {
            if (lbConnections.InvokeRequired)
            {
                // If this is not the UI thread, invoke the update on the UI thread
                lbConnections.Invoke(new MethodInvoker(() => UpdateListBox(message)));
            }
            else
            {
                // Update the ListBox on the UI thread
                lbConnections.Items.Add(message);
            }
        }

        // Example of using UpdateListBox in your threads
        private void listenerThread()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 8080);
            tcpListener.Start();
            while (true)
            {
                Socket handlerSocket = tcpListener.AcceptSocket();
                if (handlerSocket.Connected)
                {
                    UpdateListBox(handlerSocket.RemoteEndPoint.ToString() + " connected.");
                    lock (this)
                    {
                        alSockets.Add(handlerSocket);
                    }
                    Thread thdHandler = new Thread(handlerThread);
                    thdHandler.Start();
                }
            }
        }

        public void handlerThread()
        {
            Socket handlerSocket = (Socket)alSockets[alSockets.Count - 1];
            NetworkStream networkStream = new NetworkStream(handlerSocket);
            int thisRead = 0;
            int blockSize = 1024;
            Byte[] dataByte = new Byte[blockSize];

            try
            {
                using (Stream fileStream = File.OpenWrite("C:/Users/GAMING/Documents/SEMESTER 5/test.txt"))
                {
                    while (true)
                    {
                        thisRead = networkStream.Read(dataByte, 0, blockSize);
                        if (thisRead == 0) break;  // Exit when done reading
                        fileStream.Write(dataByte, 0, thisRead);
                    }
                }

                UpdateListBox("File Written");
            }
            catch (Exception ex)
            {
                UpdateListBox("Error: " + ex.Message);
            }
            finally
            {
                handlerSocket.Close();
                handlerSocket = null;
            }
        }

    }
}
