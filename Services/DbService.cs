using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    internal class DbService
    {
        private static readonly object SyncRoot = new object();

        private const string StorageFolderName = "AshkanAQMS";
        private const string StorageFileName = "sensor_logs.csv";

        private static readonly string StorageDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), StorageFolderName);

        private static readonly string StoragePath =
            Path.Combine(StorageDirectory, StorageFileName);

        private const string CsvHeader = "Timestamp,AnalyzerId,PM25,CO2,AQI,Status";

        public void SaveLog(SensorLog log)
        {
            if (log == null)
                return;

            lock (SyncRoot)
            {
                EnsureStorageFile();

                File.AppendAllText(
                    StoragePath,
                    log.ToCsvLine() + Environment.NewLine,
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
        }

        public List<SensorLog> GetAllLogs()
        {
            lock (SyncRoot)
            {
                EnsureStorageFile();

                var result = new List<SensorLog>();

                string[] lines;
                try
                {
                    lines = File.ReadAllLines(StoragePath, Encoding.UTF8);
                }
                catch
                {
                    return result;
                }

                foreach (var rawLine in lines)
                {
                    if (string.IsNullOrWhiteSpace(rawLine))
                        continue;

                    var line = rawLine.Trim();

                    if (line.StartsWith("Timestamp,", StringComparison.OrdinalIgnoreCase))
                        continue;

                    try
                    {
                        result.Add(SensorLog.FromCsvLine(line));
                    }
                    catch
                    {
                        // Skip malformed lines but keep the rest of the file usable.
                    }
                }

                return result;
            }
        }

        public List<SensorLog> GetLogs(DateTime from, DateTime to)
        {
            if (from > to)
            {
                var temp = from;
                from = to;
                to = temp;
            }

            return GetAllLogs()
                .Where(x => x.Timestamp >= from && x.Timestamp <= to)
                .OrderBy(x => x.Timestamp)
                .ToList();
        }

        public void ClearAllLogs()
        {
            lock (SyncRoot)
            {
                EnsureStorageDirectory();

                File.WriteAllText(
                    StoragePath,
                    CsvHeader + Environment.NewLine,
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
        }

        public string GetStoragePath()
        {
            EnsureStorageFile();
            return StoragePath;
        }

        private static void EnsureStorageFile()
        {
            EnsureStorageDirectory();

            if (!File.Exists(StoragePath))
            {
                File.WriteAllText(
                    StoragePath,
                    CsvHeader + Environment.NewLine,
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
        }

        private static void EnsureStorageDirectory()
        {
            if (!Directory.Exists(StorageDirectory))
            {
                Directory.CreateDirectory(StorageDirectory);
            }
        }
    }
}
