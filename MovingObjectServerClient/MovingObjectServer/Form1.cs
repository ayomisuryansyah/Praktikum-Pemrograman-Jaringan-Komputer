using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MovingObjectServer
{
    public partial class Form1 : Form
    {
        private Pen red = new Pen(System.Drawing.Color.Red);
        private Rectangle rect = new Rectangle(20, 20, 30, 30);
        private int slide = 10;
        private Socket serverSocket;

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 50;
            timer1.Enabled = true;
            StartServer();
        }

        private void StartServer()
        {
            IPAddress ipAddr = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            serverSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEndPoint);
            serverSocket.Listen(10);

            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = serverSocket.EndAccept(ar);
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

                Task.Run(() => SendDataContinuously(clientSocket));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SendDataContinuously(Socket clientSocket)
        {
            try
            {
                while (clientSocket.Connected)
                {
                    // Buat data posisi kotak biru sebagai string
                    string data = $"{rect.X},{rect.Y},{rect.Width},{rect.Height}";
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    clientSocket.BeginSend(dataBytes, 0, dataBytes.Length, SocketFlags.None, new AsyncCallback(SendCallback), clientSocket);

                    // Sleep to control the frame rate
                    System.Threading.Thread.Sleep(50); // Adjust as needed
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                clientSocket.EndSend(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            back();
            rect.X += slide;
            this.Invoke(new Action(() => this.Invalidate()));
        }

        private void back()
        {
            if (rect.X >= this.Width - rect.Width * 2)
                slide = -10;
            else if (rect.X <= rect.Width / 2)
                slide = 10;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(red, rect);
            g.FillRectangle(new SolidBrush(System.Drawing.Color.Blue), rect);
        }
    }
}
