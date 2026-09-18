using System;
using System.Diagnostics;

namespace AshkanAQMS.Services
{
    public sealed class HealthSnapshot
    {
        public double WorkingSetMb { get; set; }
        public double CpuTimeSeconds { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }

    public sealed class HealthMonitor
    {
        private readonly Process _process = Process.GetCurrentProcess();
        private TimeSpan _lastCpu;
        private DateTime _lastSample;
        public HealthMonitor() { _lastCpu = _process.TotalProcessorTime; _lastSample = DateTime.Now; }
        public HealthSnapshot Capture()
        {
            _process.Refresh();
            var now = DateTime.Now; var cpu = _process.TotalProcessorTime;
            double elapsed = Math.Max(0.001, (now - _lastSample).TotalSeconds);
            double cpuPct = Math.Max(0, (cpu - _lastCpu).TotalSeconds / elapsed / Environment.ProcessorCount * 100.0);
            _lastCpu = cpu; _lastSample = now;
            return new HealthSnapshot { Timestamp = now, WorkingSetMb = _process.WorkingSet64 / 1024d / 1024d, CpuTimeSeconds = Math.Round(cpuPct, 1), Status = cpuPct < 80 ? "Healthy" : "High CPU" };
        }
    }
}
