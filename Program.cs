using System;
using System.Threading;
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
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => ShowFatalError(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) => ShowFatalError(e.ExceptionObject as Exception);
            Application.Run(new MainDashboard());
        }

        private static void ShowFatalError(Exception ex)
        {
            var message = ex == null ? "An unexpected application error occurred." : ex.Message;
            try { MessageBox.Show(message, "Ashkan AQMS - Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
        }
    }
}
