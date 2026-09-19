using System;

namespace AshkanAQMS.Models
{
    [Serializable]
    public class AppSettings
    {
        public int PollingIntervalSeconds { get; set; } = 3;
        public string DataSourceMode { get; set; } = "RealHardware"; // RealHardware or Simulation
        public bool AllowSimulationMode { get; set; } = false;
        public double AnomalyZScoreThreshold { get; set; } = 2.5;
        public double EmaAlpha { get; set; } = 0.3;
        public int HistoryWindowSize { get; set; } = 50;
        public bool EnableSoundAlerts { get; set; } = true;
        public bool EnableAutoArchive { get; set; } = true;
        public bool EnableAiInsights { get; set; } = true;
        public bool EnableAuditLogging { get; set; } = true;
        public bool EnableHealthMonitoring { get; set; } = true;
        public bool EnableAnomalyAlarms { get; set; } = true;
        public int AlarmEvaluationEveryNMeasurements { get; set; } = 1;
        public bool EnableExecutiveReports { get; set; } = true;
        public bool EnableAdvancedAnalytics { get; set; } = true;
        public bool PresentationMode { get; set; } = false;
        public int RetentionLimit { get; set; } = 5000;
        public string StationName { get; set; } = "Station A";
        public string Language { get; set; } = "English";
        public double Pm25WarningThreshold { get; set; } = 35.5;
        public double Pm25CriticalThreshold { get; set; } = 150.5;
        public double Pm10WarningThreshold { get; set; } = 155;
        public double Pm10CriticalThreshold { get; set; } = 355;
        public double No2WarningThreshold { get; set; } = 100;
        public double No2CriticalThreshold { get; set; } = 200;
        public double Co2WarningThreshold { get; set; } = 1000;
        public double Co2CriticalThreshold { get; set; } = 2000;
    }
}
