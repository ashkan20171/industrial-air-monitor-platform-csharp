using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public class DbService
    {
        private readonly string _dataDirectory;
        private readonly string _logFilePath;
        private readonly object _syncRoot = new object();

        public DbService()
        {
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _logFilePath = Path.Combine(_dataDirectory, "sensor_logs.csv");
            EnsureStorage();
        }

        public string LogFilePath => _logFilePath;

        public void SaveLog(SensorLog log)
        {
            if (log == null)
                return;

            EnsureStorage();

            lock (_syncRoot)
            {
                bool writeHeader = !File.Exists(_logFilePath) || new FileInfo(_logFilePath).Length == 0;

                using (var writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    if (writeHeader)
                    {
                        writer.WriteLine("Timestamp,AnalyzerId,PM25,CO2,AQI,Status");
                    }

                    writer.WriteLine(log.ToCsvLine());
                }
            }
        }

        public void SaveLogs(IEnumerable<SensorLog> logs)
        {
            if (logs == null)
                return;

            foreach (var log in logs)
            {
                SaveLog(log);
            }
        }

        public List<SensorLog> GetAllLogs()
        {
            EnsureStorage();

            lock (_syncRoot)
            {
                if (!File.Exists(_logFilePath))
                    return new List<SensorLog>();

                var lines = File.ReadAllLines(_logFilePath, Encoding.UTF8);

                if (lines.Length == 0)
                    return new List<SensorLog>();

                var result = new List<SensorLog>();

                foreach (var line in lines.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try
                    {
                        var log = SensorLog.FromCsvLine(line);
                        if (log != null)
                            result.Add(log);
                    }
                    catch
                    {
                        // Skip invalid rows
                    }
                }

                return result.OrderBy(x => x.Timestamp).ToList();
            }
        }

        public List<SensorLog> GetLogs(DateTime from, DateTime to)
        {
            var allLogs = GetAllLogs();

            return allLogs
                .Where(x => x.Timestamp >= from && x.Timestamp <= to)
                .OrderByDescending(x => x.Timestamp)
                .ToList();
        }

        public void ClearAllLogs()
        {
            EnsureStorage();

            lock (_syncRoot)
            {
                using (var writer = new StreamWriter(_logFilePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("Timestamp,AnalyzerId,PM25,CO2,AQI,Status");
                }
            }
        }

        private void EnsureStorage()
        {
            if (!Directory.Exists(_dataDirectory))
                Directory.CreateDirectory(_dataDirectory);

            if (!File.Exists(_logFilePath))
            {
                using (var writer = new StreamWriter(_logFilePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("Timestamp,AnalyzerId,PM25,CO2,AQI,Status");
                }
            }
        }
    }
}
