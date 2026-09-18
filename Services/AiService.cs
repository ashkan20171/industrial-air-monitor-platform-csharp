using System;
using System.Collections.Generic;
using System.Linq;

namespace AshkanAQMS.Services
{
    public sealed class AiAnalysisResult
    {
        public double Ema { get; set; }
        public double Forecast { get; set; }
        public double TrendPerSample { get; set; }
        public double Confidence { get; set; }
        public double ZScore { get; set; }
        public bool IsAnomaly { get; set; }
        public string Trend { get; set; }
        public string Insight { get; set; }
        public int DataQualityScore { get; set; }
    }

    /// <summary>Lightweight local analytics engine. It requires no cloud service and works on .NET Framework 4.8.</summary>
    public sealed class AiService
    {
        private double _emaAlpha;
        private double _anomalyThreshold;
        private double? _previousEma;

        public AiService(double emaAlpha = 0.3, double anomalyThreshold = 2.5)
        {
            UpdateParameters(emaAlpha, anomalyThreshold);
        }

        public void UpdateParameters(double alpha, double threshold)
        {
            _emaAlpha = Math.Max(0.01, Math.Min(1.0, alpha));
            _anomalyThreshold = Math.Max(0.1, threshold);
        }

        public double CalculateEma(double currentValue)
        {
            if (!_previousEma.HasValue) _previousEma = currentValue;
            else _previousEma = (_emaAlpha * currentValue) + ((1 - _emaAlpha) * _previousEma.Value);
            return Math.Round(_previousEma.Value, 2);
        }

        public AiAnalysisResult Analyze(double currentValue, IEnumerable<double> historyValues, bool validData = true)
        {
            var values = (historyValues ?? Enumerable.Empty<double>()).Where(IsFinite).ToList();
            var ema = CalculateEma(currentValue);
            var result = new AiAnalysisResult { Ema = ema, DataQualityScore = validData ? 100 : 0 };

            if (values.Count < 3)
            {
                result.Forecast = currentValue;
                result.Confidence = 35;
                result.Trend = "Learning";
                result.Insight = "Collecting more measurements to improve the local forecast.";
                return result;
            }

            var ordered = values.AsEnumerable().Reverse().ToList();
            double mean = ordered.Average();
            double variance = ordered.Sum(v => Math.Pow(v - mean, 2)) / ordered.Count;
            double std = Math.Sqrt(variance);
            result.ZScore = std > 1e-9 ? Math.Abs(currentValue - mean) / std : 0;
            result.IsAnomaly = std > 1e-9 && result.ZScore >= _anomalyThreshold;

            int n = ordered.Count;
            double sx = n * (n - 1) / 2.0;
            double sy = ordered.Sum();
            double sxx = Enumerable.Range(0, n).Sum(i => i * i);
            double sxy = ordered.Select((v, i) => i * v).Sum();
            double denominator = n * sxx - sx * sx;
            double slope = Math.Abs(denominator) < 1e-9 ? 0 : (n * sxy - sx * sy) / denominator;
            double intercept = (sy - slope * sx) / n;
            double forecast = intercept + slope * n;

            result.TrendPerSample = slope;
            result.Forecast = Math.Max(0, Math.Round(forecast, 2));
            double signal = Math.Abs(slope) / Math.Max(std, 1.0);
            result.Confidence = Math.Max(40, Math.Min(98, 65 + (Math.Min(n, 50) * 0.35) + signal * 12));
            result.Trend = slope > 0.15 ? "Rising" : slope < -0.15 ? "Falling" : "Stable";
            result.DataQualityScore = Math.Max(0, Math.Min(100, 100 - (result.IsAnomaly ? 25 : 0)));

            if (result.IsAnomaly)
                result.Insight = "An unusual PM2.5 pattern was detected. Verify the sensor and local conditions.";
            else if (result.Trend == "Rising")
                result.Insight = "PM2.5 is trending upward. Monitor the next measurements and alarm thresholds.";
            else if (result.Trend == "Falling")
                result.Insight = "PM2.5 is trending downward based on recent measurements.";
            else
                result.Insight = "PM2.5 is relatively stable over the recent observation window.";

            return result;
        }

        public bool IsAnomaly(double currentValue, IEnumerable<double> historyValues)
        {
            return Analyze(currentValue, historyValues).IsAnomaly;
        }

        public void ResetState() { _previousEma = null; }
        private static bool IsFinite(double value) { return !double.IsNaN(value) && !double.IsInfinity(value); }
    }
}
