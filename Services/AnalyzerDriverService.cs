using System;
using System.Threading;
using System.Threading.Tasks;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    /// <summary>Central driver dispatch boundary. Vendor drivers can be added without changing acquisition lifecycle.</summary>
    public sealed class AnalyzerDriverService
    {
        private readonly RealAnalyzerReader _generic = new RealAnalyzerReader();
        public Task<AnalyzerReading> ReadAsync(AnalyzerConfig config, CancellationToken token)
        {
            if (config == null) throw new ArgumentNullException("config");
            // The migrated vendor catalog is intentionally conservative: until a vendor protocol is
            // validated against physical hardware, it uses configured request/parse settings rather
            // than silently fabricating or guessing a binary response.
            return _generic.ReadAsync(config, token);
        }
    }
}
