using System;
using System.IO;
using System.Net;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public static class ExecutiveReportService
    {
        public static bool ExportHtml(string fileName, AirQualityData data, AnalyticsSummary analytics, DataQualityResult quality, HealthSnapshot health, AppSettings settings)
        {
            if (string.IsNullOrWhiteSpace(fileName) || data == null) return false;
            string Safe(string s) { return WebUtility.HtmlEncode(s ?? string.Empty); }
            var html = "<!doctype html><html><head><meta charset='utf-8'><title>AQMS Executive Snapshot</title>" +
                       "<style>body{font-family:Segoe UI,Arial;background:#111820;color:#edf2f7;margin:40px}h1{margin-bottom:4px}.muted{color:#9aa8b6}.grid{display:grid;grid-template-columns:repeat(4,1fr);gap:14px}.card{background:#202a35;padding:18px;border-radius:10px}.value{font-size:28px;font-weight:700}.accent{color:#55d6be}table{width:100%;border-collapse:collapse;margin-top:20px}td,th{padding:10px;border-bottom:1px solid #364453;text-align:left}</style></head><body>" +
                       "<h1>Ashkan AQMS — Executive Snapshot</h1><div class='muted'>Station: " + Safe(settings == null ? "" : settings.StationName) + " • Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</div>" +
                       "<div class='grid' style='margin-top:24px'><div class='card'><div class='muted'>AQI</div><div class='value accent'>" + data.AQI + "</div><div>" + Safe(data.AQICategory) + "</div></div>" +
                       "<div class='card'><div class='muted'>PM2.5</div><div class='value'>" + data.PM25.ToString("0.0") + "</div><div>µg/m³</div></div>" +
                       "<div class='card'><div class='muted'>AI Risk</div><div class='value'>" + (analytics == null ? 0 : analytics.OverallRiskScore) + "%</div><div>" + Safe(analytics == null ? "-" : analytics.OverallTrend) + "</div></div>" +
                       "<div class='card'><div class='muted'>Data Quality</div><div class='value'>" + (quality == null ? 0 : quality.Score) + "%</div><div>" + Safe(quality == null ? "-" : quality.Status) + "</div></div></div>" +
                       "<table><tr><th>Parameter</th><th>Current</th><th>AI forecast</th></tr>" +
                       "<tr><td>PM2.5</td><td>" + data.PM25.ToString("0.0") + " µg/m³</td><td>" + (analytics == null ? 0 : analytics.Pm25Forecast).ToString("0.0") + "</td></tr>" +
                       "<tr><td>PM10</td><td>" + data.PM10.ToString("0.0") + " µg/m³</td><td>" + (analytics == null ? 0 : analytics.Pm10Forecast).ToString("0.0") + "</td></tr>" +
                       "<tr><td>CO₂</td><td>" + data.CO2.ToString("0") + " ppm</td><td>" + (analytics == null ? 0 : analytics.Co2Forecast).ToString("0") + "</td></tr>" +
                       "<tr><td>NO₂</td><td>" + data.NO2.ToString("0.0") + " ppb</td><td>" + (analytics == null ? 0 : analytics.No2Forecast).ToString("0.0") + "</td></tr></table>" +
                       "<h2>AI recommendation</h2><p>" + Safe(analytics == null ? "No recommendation available." : analytics.Recommendation) + "</p>" +
                       "<p class='muted'>System health: " + Safe(health == null ? "Unknown" : health.Status) + " • RAM " + (health == null ? 0 : health.WorkingSetMb).ToString("0") + " MB</p></body></html>";
            File.WriteAllText(fileName, html);
            return true;
        }
    }
}
