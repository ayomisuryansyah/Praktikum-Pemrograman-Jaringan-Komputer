using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace MovingObjectClient
{
    public partial class ClientForm : Form
    {
        private Socket clientSocket;
        private Rectangle rect;
        private const int port = 11111;

        public ClientForm()
        {
            InitializeComponent();
            rect = new Rectangle(20, 20, 30, 30); // Inisialisasi dengan ukuran default
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                IPAddress ipAddr = IPAddress.Loopback; // Ganti dengan alamat IP server jika diperlukan
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddr, port);

                clientSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(remoteEndPoint);

                ReceiveDataContinuously();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ReceiveDataContinuously()
        {
            Task.Run(() =>
            {
                try
                {
                    while (clientSocket.Connected)
                    {
                        byte[] buffer = new byte[1024];
                        int receivedBytes = clientSocket.Receive(buffer);

                        if (receivedBytes > 0)
                        {
                            string data = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                            string[] parts = data.Split(',');

                            if (parts.Length == 4)
                            {
                                int x = int.Parse(parts[0]);
                                int y = int.Parse(parts[1]);
                                int width = int.Parse(parts[2]);
                                int height = int.Parse(parts[3]);

                                rect = new Rectangle(x, y, width, height);
                                this.Invoke(new Action(() => this.Invalidate())); // Meminta form untuk menggambar ulang
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Graphics g = e.Graphics)
            {
                using (Pen red = new Pen(Color.Red))
                using (SolidBrush fillBlue = new SolidBrush(Color.Blue))
                {
                    g.DrawRectangle(red, rect);
                    g.FillRectangle(fillBlue, rect);
                }
            }
        }
    }
}
