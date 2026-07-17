using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization; // استفاده از این به جای JSON.NET برای سازگاری بیشتر در WinForms قدیمی
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public static class StorageService
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "analyzers_config.json");

        public static void SaveAnalyzers(List<AnalyzerConfig> analyzers)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(analyzers);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Save Error: " + ex.Message);
            }
        }

        public static List<AnalyzerConfig> LoadAnalyzers()
        {
            if (!File.Exists(ConfigPath)) return new List<AnalyzerConfig>();
            try
            {
                string json = File.ReadAllText(ConfigPath);
                var serializer = new JavaScriptSerializer();
                return serializer.Deserialize<List<AnalyzerConfig>>(json) ?? new List<AnalyzerConfig>();
            }
            catch
            {
                return new List<AnalyzerConfig>();
            }
        }
    }
}
