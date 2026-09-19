using System;
using System.Collections.Generic;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public sealed class AlarmEngine
    {
        private readonly Dictionary<string, AlarmSeverity> _activeStates = new Dictionary<string, AlarmSeverity>(StringComparer.OrdinalIgnoreCase);
        public event EventHandler<AlarmEvent> AlarmRaised;
        public event EventHandler<AlarmEvent> AlarmCleared;

        public IReadOnlyList<AlarmEvent> Evaluate(AirQualityData data, AppSettings settings)
        {
            var alarms = new List<AlarmEvent>();
            if (data == null || settings == null) return alarms;
            EvaluateThreshold(alarms, "PM2.5", data.PM25, settings.Pm25WarningThreshold, settings.Pm25CriticalThreshold, "µg/m³");
            EvaluateThreshold(alarms, "PM10", data.PM10, settings.Pm10WarningThreshold, settings.Pm10CriticalThreshold, "µg/m³");
            EvaluateThreshold(alarms, "NO₂", data.NO2, settings.No2WarningThreshold, settings.No2CriticalThreshold, "ppb");
            EvaluateThreshold(alarms, "CO₂", data.CO2, settings.Co2WarningThreshold, settings.Co2CriticalThreshold, "ppm");
            return alarms;
        }

        private void EvaluateThreshold(List<AlarmEvent> target, string parameter, double value, double warning, double critical, string unit)
        {
            AlarmSeverity current = AlarmSeverity.Info;
            string message = string.Empty;
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                AlarmSeverity previousUnavailable;
                if (_activeStates.TryGetValue(parameter, out previousUnavailable) && previousUnavailable != AlarmSeverity.Info)
                {
                    var clearedUnavailable = Create(parameter, value, AlarmSeverity.Info, "Measurement unavailable; threshold alarm cleared", unit);
                    clearedUnavailable.IsAcknowledged = true;
                    _activeStates.Remove(parameter);
                    if (AlarmCleared != null) AlarmCleared(this, clearedUnavailable);
                }
                return;
            }
            else if (value >= critical) { current = AlarmSeverity.Critical; message = "Critical threshold exceeded"; }
            else if (value >= warning) { current = AlarmSeverity.Warning; message = "Warning threshold exceeded"; }

            AlarmSeverity previous;
            _activeStates.TryGetValue(parameter, out previous);
            if (current != AlarmSeverity.Info)
            {
                if (previous != current)
                {
                    var alarm = Create(parameter, value, current, message, unit);
                    _activeStates[parameter] = current;
                    target.Add(alarm);
                    if (AlarmRaised != null) AlarmRaised(this, alarm);
                }
                else _activeStates[parameter] = current;
            }
            else if (previous != AlarmSeverity.Info)
            {
                var cleared = Create(parameter, value, AlarmSeverity.Info, "Alarm condition cleared", unit);
                cleared.IsAcknowledged = true;
                _activeStates.Remove(parameter);
                if (AlarmCleared != null) AlarmCleared(this, cleared);
            }
        }


        public AlarmEvent RaiseAnomaly(string parameter, double value, string message)
        {
            var alarm = Create(parameter, value, AlarmSeverity.Warning, message ?? "AI anomaly detected", "µg/m³");
            if (AlarmRaised != null) AlarmRaised(this, alarm);
            return alarm;
        }
        private static AlarmEvent Create(string parameter, double value, AlarmSeverity severity, string message, string unit)
        {
            return new AlarmEvent { Id = Guid.NewGuid(), Timestamp = DateTime.Now, Parameter = parameter, Value = value, Severity = severity, Message = message, Unit = unit, IsAcknowledged = false };
        }
    }
}
