using System;
using System.IO;
using System.Text;

namespace AshkanAQMS.Services
{
    public sealed class AuditLogger
    {
        private readonly string _path;
        public AuditLogger()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AshkanAQMS");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            _path = Path.Combine(folder, "audit.log");
        }
        public void Write(string action, string details)
        {
            try
            {
                var line = string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] [{1}] {2}{3}", DateTime.Now, action ?? "INFO", details ?? string.Empty, Environment.NewLine);
                File.AppendAllText(_path, line, Encoding.UTF8);
            }
            catch { }
        }
        public string GetPath() { return _path; }
    }
}
