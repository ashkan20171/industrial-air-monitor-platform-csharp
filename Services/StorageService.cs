using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public class StorageService
    {
        private readonly string _folderPath;
        private readonly string _analyzersFilePath;
        private readonly string _settingsFilePath;
        private readonly string _logFilePath;

        public StorageService()
        {
            _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AshkanAQMS");
            EnsureStorageDirectory();
            _analyzersFilePath = Path.Combine(_folderPath, "analyzers.xml");
            _settingsFilePath = Path.Combine(_folderPath, "settings.xml");
            _logFilePath = Path.Combine(_folderPath, "archive_log.txt");
        }

        private void EnsureStorageDirectory()
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        public AppSettings LoadSettings()
        {
            if (!File.Exists(_settingsFilePath))
            {
                var defaultSettings = new AppSettings();
                SaveSettings(defaultSettings);
                return defaultSettings;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                using (StreamReader reader = new StreamReader(_settingsFilePath))
                {
                    return (AppSettings)serializer.Deserialize(reader);
                }
            }
            catch
            {
                return new AppSettings();
            }
        }

        public void SaveSettings(AppSettings settings)
        {
            try
            {
                EnsureStorageDirectory();
                XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                using (StreamWriter writer = new StreamWriter(_settingsFilePath))
                {
                    serializer.Serialize(writer, settings);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error saving settings: " + ex.Message);
            }
        }

        public List<AnalyzerConfig> LoadAnalyzers()
        {
            if (!File.Exists(_analyzersFilePath))
                return new List<AnalyzerConfig>();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AnalyzerConfig>));
                using (FileStream fs = new FileStream(_analyzersFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return (List<AnalyzerConfig>)serializer.Deserialize(fs);
                }
            }
            catch
            {
                return new List<AnalyzerConfig>();
            }
        }

        public void SaveAnalyzers(List<AnalyzerConfig> analyzers)
        {
            EnsureStorageDirectory();
            if (analyzers == null) analyzers = new List<AnalyzerConfig>();

            string tempPath = _analyzersFilePath + ".tmp";
            string backupPath = _analyzersFilePath + ".bak";
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AnalyzerConfig>));
                using (var writer = new StreamWriter(tempPath, false, Encoding.UTF8)) serializer.Serialize(writer, analyzers);
                if (File.Exists(_analyzersFilePath)) File.Copy(_analyzersFilePath, backupPath, true);
                if (File.Exists(_analyzersFilePath)) File.Replace(tempPath, _analyzersFilePath, backupPath, true);
                else File.Move(tempPath, _analyzersFilePath);
            }
            catch (Exception ex)
            {
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { }
                System.Diagnostics.Debug.WriteLine("Error saving analyzers: " + ex.Message);
                throw;
            }
        }

        public string GetAnalyzerBackupFolder()
        {
            string folder = Path.Combine(_folderPath, "backups");
            Directory.CreateDirectory(folder);
            return folder;
        }

        public void AppendArchiveLog(string message)
        {
            try
            {
                EnsureStorageDirectory();
                string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                File.AppendAllText(_logFilePath, logLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error writing archive log: " + ex.Message);
            }
        }

        public bool ExportToCsv(List<string> headers, List<List<string>> rows, string filePath)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Join(",", headers));

                foreach (var row in rows)
                {
                    List<string> escapedCells = new List<string>();
                    foreach (var cell in row)
                    {
                        string val = cell ?? "";
                        if (val.Contains(",") || val.Contains("\"") || val.Contains("\n"))
                        {
                            val = "\"" + val.Replace("\"", "\"\"") + "\"";
                        }
                        escapedCells.Add(val);
                    }
                    sb.AppendLine(string.Join(",", escapedCells));
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
