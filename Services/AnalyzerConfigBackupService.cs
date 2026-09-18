using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public static class AnalyzerConfigBackupService
    {
        public static string CreateBackup(IEnumerable<AnalyzerConfig> analyzers, string folder)
        {
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, "analyzers_backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml");
            var serializer = new XmlSerializer(typeof(List<AnalyzerConfig>));
            using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None)) serializer.Serialize(fs, new List<AnalyzerConfig>(analyzers ?? new List<AnalyzerConfig>()));
            return path;
        }
    }
}
