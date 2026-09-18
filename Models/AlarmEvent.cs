using System;

namespace AshkanAQMS.Models
{
    public enum AlarmSeverity { Info, Warning, Critical }

    [Serializable]
    public sealed class AlarmEvent : EventArgs
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Parameter { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public AlarmSeverity Severity { get; set; }
        public string Message { get; set; }
        public bool IsAcknowledged { get; set; }
    }
}
