using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public static class StorageService
    {
        private const string StorageFolderName = "AshkanAQMS";
        private const string StorageFileName = "analyzers.xml";

        private static readonly string StorageDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), StorageFolderName);

        private static readonly string StoragePath =
            Path.Combine(StorageDirectory, StorageFileName);

        public static List<AnalyzerConfig> LoadAnalyzers()
        {
            try
            {
                if (!File.Exists(StoragePath))
                {
                    return new List<AnalyzerConfig>();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<AnalyzerConfig>));

                using (FileStream stream = File.Open(StoragePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    object result = serializer.Deserialize(stream);
                    return result as List<AnalyzerConfig> ?? new List<AnalyzerConfig>();
                }
            }
            catch
            {
                return new List<AnalyzerConfig>();
            }
        }

        public static void SaveAnalyzers(List<AnalyzerConfig> analyzers)
        {
            if (analyzers == null)
            {
                analyzers = new List<AnalyzerConfig>();
            }

            try
            {
                EnsureStorageDirectory();

                XmlSerializer serializer = new XmlSerializer(typeof(List<AnalyzerConfig>));

                using (FileStream stream = File.Create(StoragePath))
                {
                    serializer.Serialize(stream, analyzers);
                }
            }
            catch
            {
                throw;
            }
        }

        public static string GetStoragePath()
        {
            return StoragePath;
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
