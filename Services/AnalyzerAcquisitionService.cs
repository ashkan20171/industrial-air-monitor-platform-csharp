using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public sealed class AcquisitionCycleResult
    {
        public DateTime Timestamp { get; set; }
        public List<AnalyzerReading> Readings { get; private set; } = new List<AnalyzerReading>();
        public int EnabledAnalyzerCount { get; set; }
        public int SuccessfulReadingCount { get { return Readings.Count(x => x.IsUsable); } }
        public string Summary { get; set; }
    }

    /// <summary>
    /// Owns the polling lifecycle. Disabled analyzers are never read.
    /// A cycle cannot overlap another cycle.
    /// </summary>
    public sealed class AnalyzerAcquisitionService : IDisposable
    {
        private readonly RealAnalyzerReader _reader = new RealAnalyzerReader();
        private readonly AuditLogger _audit = new AuditLogger();
        private bool _disposed;

        public async Task<AcquisitionCycleResult> ReadCycleAsync(IEnumerable<AnalyzerConfig> analyzers, CancellationToken token)
        {
            if (_disposed) throw new ObjectDisposedException("AnalyzerAcquisitionService");
            var result = new AcquisitionCycleResult { Timestamp = DateTime.Now };
            var active = (analyzers ?? Enumerable.Empty<AnalyzerConfig>()).Where(x => x != null && x.Enabled).ToList();
            result.EnabledAnalyzerCount = active.Count;

            foreach (var analyzer in active)
            {
                token.ThrowIfCancellationRequested();
                AnalyzerReading reading;
                try
                {
                    reading = await _reader.ReadAsync(analyzer, token).ConfigureAwait(false);
                }
                catch (OperationCanceledException) { throw; }
                catch (Exception ex)
                {
                    reading = new AnalyzerReading
                    {
                        AnalyzerId = analyzer.Id,
                        AnalyzerName = analyzer.Name,
                        GasType = analyzer.GasType,
                        Timestamp = DateTime.Now,
                        Value = double.NaN,
                        Quality = AnalyzerReadingQuality.CommunicationError,
                        Message = ex.Message
                    };
                }

                result.Readings.Add(reading);
                if (reading.Quality != AnalyzerReadingQuality.Good)
                    _audit.Write("ANALYZER_COMMUNICATION", analyzer.Name + " | " + reading.Quality + " | " + reading.Message);
            }

            result.Summary = result.EnabledAnalyzerCount == 0
                ? "No enabled analyzers."
                : result.SuccessfulReadingCount + "/" + result.EnabledAnalyzerCount + " analyzer readings valid.";
            return result;
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
