using System;
using System.Collections.Generic;
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
            if (data == null)
                return new DataQualityResult { Score = 0, Status = "Invalid", Message = "No measurement available." };

            var available = new List<string>();
            var issues = new List<string>();
            Action<double, string, double> check = (value, name, max) =>
            {
                if (double.IsNaN(value) || double.IsInfinity(value)) return; // Not configured/not supplied is not a corrupt measurement.
                available.Add(name);
                if (value < 0 || (max > 0 && value > max)) issues.Add(name + " out of range");
            };

            check(data.PM25, "PM2.5", 1000);
            check(data.PM10, "PM10", 2000);
            check(data.CO2, "CO2", 10000);
            check(data.NO2, "NO2", 5000);
            check(data.Temperature, "Temperature", 70);
            check(data.Humidity, "Humidity", 100);

            if (available.Count == 0)
                return new DataQualityResult { Score = 0, Status = "No Data", Message = "No analyzer measurement is currently available." };

            int score = issues.Count == 0 ? 100 : Math.Max(0, 100 - issues.Count * 20);
            string status = score >= 95 ? "Excellent" : score >= 80 ? "Good" : score >= 60 ? "Degraded" : "Poor";
            string message = issues.Count == 0
                ? available.Count + " live parameter(s) passed basic quality checks."
                : string.Join("; ", issues) + ".";

            return new DataQualityResult { Score = score, Status = status, Message = message };
        }
    }
}
