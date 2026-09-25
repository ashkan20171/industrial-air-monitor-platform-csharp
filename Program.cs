using System;
using System.Windows.Forms;

namespace AshkanAQMS
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainDashboard());
        }
    }
}
