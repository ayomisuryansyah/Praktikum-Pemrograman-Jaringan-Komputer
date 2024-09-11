using System;
using System.Windows.Forms;

namespace MovingObjectClient // Ganti dengan MovingObjectServer untuk server
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm()); // Ganti dengan Form1 untuk server
        }
    }
}
