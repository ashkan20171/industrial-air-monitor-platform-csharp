using System;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public sealed class DataQualityResult
    {
        public int Score { get; set; }
        public bool IsAcceptable { get { return Score >= 80; } }
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public static class DataQualityService
    {
        public static DataQualityResult Assess(AirQualityData data)
        {
            if (data == null) return new DataQualityResult { Score = 0, Status = "Invalid", Message = "No measurement available." };
            int score = 100;
            string issue = string.Empty;
            Action<double,string> check = (v,n) => { if (double.IsNaN(v) || double.IsInfinity(v) || v < 0) { score -= 20; issue += n + " invalid; "; } };
            check(data.PM25, "PM2.5"); check(data.PM10, "PM10"); check(data.CO2, "CO2"); check(data.NO2, "NO2");
            if (data.Humidity > 100) { score -= 15; issue += "Humidity out of range; "; }
            if (data.Temperature < -50 || data.Temperature > 70) { score -= 15; issue += "Temperature out of range; "; }
            score = Math.Max(0, score);
            return new DataQualityResult { Score = score, Status = score >= 95 ? "Excellent" : score >= 80 ? "Good" : score >= 60 ? "Degraded" : "Poor", Message = string.IsNullOrEmpty(issue) ? "Measurement passed basic quality checks." : issue.Trim() };
        }
    }
}
