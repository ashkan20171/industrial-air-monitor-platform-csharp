using System;
using System.Collections.Generic;
using System.Linq;

namespace AshkanAQMS.Services
{
    public sealed class AnalyzerHealthSnapshot
    {
        public string AnalyzerId { get; set; }
        public string AnalyzerName { get; set; }
        public DateTime? LastAttemptUtc { get; set; }
        public DateTime? LastSuccessUtc { get; set; }
        public int ConsecutiveFailures { get; set; }
        public long LastResponseTimeMs { get; set; }
        public AnalyzerReadingQuality LastQuality { get; set; }
        public string LastMessage { get; set; }
    }

    /// <summary>
    /// Thread-safe in-process communication health registry. It never fabricates data and
    /// is intentionally independent from alarm/AQI calculations.
    /// </summary>
    public sealed class AnalyzerHealthRegistry
    {
        private readonly object _sync = new object();
        private readonly Dictionary<string, AnalyzerHealthSnapshot> _items =
            new Dictionary<string, AnalyzerHealthSnapshot>(StringComparer.OrdinalIgnoreCase);

        public void Record(AnalyzerReading reading)
        {
            if (reading == null || string.IsNullOrWhiteSpace(reading.AnalyzerId)) return;
            lock (_sync)
            {
                AnalyzerHealthSnapshot item;
                if (!_items.TryGetValue(reading.AnalyzerId, out item))
                {
                    item = new AnalyzerHealthSnapshot { AnalyzerId = reading.AnalyzerId };
                    _items[reading.AnalyzerId] = item;
                }
                item.AnalyzerName = reading.AnalyzerName;
                item.LastAttemptUtc = DateTime.UtcNow;
                item.LastResponseTimeMs = reading.ResponseTimeMs;
                item.LastQuality = reading.Quality;
                item.LastMessage = reading.Message;
                if (reading.IsUsable)
                {
                    item.LastSuccessUtc = DateTime.UtcNow;
                    item.ConsecutiveFailures = 0;
                }
                else if (reading.Quality != AnalyzerReadingQuality.Disabled)
                {
                    item.ConsecutiveFailures++;
                }
            }
        }

        public List<AnalyzerHealthSnapshot> GetSnapshot()
        {
            lock (_sync)
            {
                return _items.Values.Select(x => new AnalyzerHealthSnapshot
                {
                    AnalyzerId = x.AnalyzerId,
                    AnalyzerName = x.AnalyzerName,
                    LastAttemptUtc = x.LastAttemptUtc,
                    LastSuccessUtc = x.LastSuccessUtc,
                    ConsecutiveFailures = x.ConsecutiveFailures,
                    LastResponseTimeMs = x.LastResponseTimeMs,
                    LastQuality = x.LastQuality,
                    LastMessage = x.LastMessage
                }).ToList();
            }
        }
    }
}
