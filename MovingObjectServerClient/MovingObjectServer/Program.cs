using System;
using System.Windows.Forms;
using MovingObjectServer; // Pastikan namespace sesuai dengan Form1.cs

namespace MovingObjectServer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // Pastikan Form1 ada di namespace ini
        }
    }
}
