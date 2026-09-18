using System;
using System.Collections.Generic;
using System.Linq;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    /// <summary>Local, deterministic analytics for offline industrial deployments.</summary>
    public sealed class AdvancedAnalyticsService
    {
        public AnalyticsSummary Analyze(AirQualityData data, IEnumerable<AirQualityData> history)
        {
            var items = (history ?? Enumerable.Empty<AirQualityData>()).Where(x => x != null).ToList();
            var all = items.Concat(data == null ? Enumerable.Empty<AirQualityData>() : new[] { data }).ToList();
            if (data == null || all.Count == 0) return new AnalyticsSummary();

            var pm25 = Forecast(all.Select(x => x.PM25));
            var pm10 = Forecast(all.Select(x => x.PM10));
            var co2 = Forecast(all.Select(x => x.CO2));
            var no2 = Forecast(all.Select(x => x.NO2));
            var risk = 0;
            if (pm25.Trend > 0) risk += 25;
            if (pm10.Trend > 0) risk += 20;
            if (co2.Trend > 0) risk += 15;
            if (no2.Trend > 0) risk += 15;
            if (data.AQI > 100) risk += 25;
            risk = Math.Min(100, risk);

            return new AnalyticsSummary
            {
                Pm25Forecast = pm25.Forecast, Pm10Forecast = pm10.Forecast,
                Co2Forecast = co2.Forecast, No2Forecast = no2.Forecast,
                OverallRiskScore = risk,
                OverallTrend = risk >= 60 ? "Elevated" : risk >= 30 ? "Watch" : "Stable",
                Recommendation = risk >= 60 ? "Review ventilation, source conditions and active alarms." :
                                 risk >= 30 ? "Continue monitoring and verify the next measurement cycle." :
                                 "No immediate trend escalation detected. Continue normal monitoring."
            };
        }

        private static ForecastResult Forecast(IEnumerable<double> source)
        {
            var v = source.Where(x => !double.IsNaN(x) && !double.IsInfinity(x)).Take(60).Reverse().ToList();
            if (v.Count < 3) return new ForecastResult { Forecast = v.Count == 0 ? 0 : v.Last(), Trend = 0 };
            double n = v.Count, sx = n * (n - 1) / 2d, sy = v.Sum(), sxx = Enumerable.Range(0, v.Count).Sum(i => i * i), sxy = v.Select((x, i) => x * i).Sum();
            double d = n * sxx - sx * sx;
            double slope = Math.Abs(d) < 1e-9 ? 0 : (n * sxy - sx * sy) / d;
            double intercept = (sy - slope * sx) / n;
            return new ForecastResult { Forecast = Math.Max(0, intercept + slope * n), Trend = slope };
        }
    }

    public sealed class AnalyticsSummary
    {
        public double Pm25Forecast { get; set; }
        public double Pm10Forecast { get; set; }
        public double Co2Forecast { get; set; }
        public double No2Forecast { get; set; }
        public int OverallRiskScore { get; set; }
        public string OverallTrend { get; set; }
        public string Recommendation { get; set; }
    }
    internal sealed class ForecastResult { public double Forecast { get; set; } public double Trend { get; set; } }
}
