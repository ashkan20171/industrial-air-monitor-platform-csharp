using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public sealed class AnalyzerValidationResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();
        public bool IsValid { get { return Errors.Count == 0; } }
        public string ToMessage()
        {
            var lines = new List<string>();
            if (Errors.Count > 0) { lines.Add("Errors:"); lines.AddRange(Errors.Select(x => "• " + x)); }
            if (Warnings.Count > 0) { if (lines.Count > 0) lines.Add(""); lines.Add("Warnings:"); lines.AddRange(Warnings.Select(x => "• " + x)); }
            return string.Join(Environment.NewLine, lines);
        }
    }

    public static class AnalyzerConfigValidationService
    {
        private static readonly HashSet<string> SupportedGasTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { "PM2.5", "PM10", "CO2", "NO2", "O3", "SO2", "Temperature", "Humidity" };

        public static AnalyzerValidationResult Validate(AnalyzerConfig config, IEnumerable<AnalyzerConfig> existing, string ignoredId = null)
        {
            var result = new AnalyzerValidationResult();
            if (config == null) { result.Errors.Add("Analyzer configuration is missing."); return result; }
            if (string.IsNullOrWhiteSpace(config.Name)) result.Errors.Add("Analyzer name is required.");
            if (config.Name != null && config.Name.Trim().Length > 100) result.Errors.Add("Analyzer name must not exceed 100 characters.");
            if (string.IsNullOrWhiteSpace(config.GasType) || !SupportedGasTypes.Contains(config.GasType)) result.Errors.Add("A supported measurement type must be selected.");
            if (config.Channel < 1 || config.Channel > 255) result.Errors.Add("Channel must be between 1 and 255.");
            if (config.DecimalDigits < 0 || config.DecimalDigits > 6) result.Errors.Add("Decimal digits must be between 0 and 6.");
            if (config.Gain == 0) result.Warnings.Add("Gain is 0. Measurements will be forced to zero unless this is intentional.");
            if (config.RequestIntervalMs < 100) result.Errors.Add("Request interval must be at least 100 ms.");
            if (config.TimeoutMs < 100 || config.TimeoutMs > 120000) result.Errors.Add("Timeout must be between 100 and 120000 ms.");

            if (string.Equals(config.ConnectionType, "IP", StringComparison.OrdinalIgnoreCase))
            {
                IPAddress address;
                if (!IPAddress.TryParse(config.IpAddress, out address))
                {
                    if (config.Enabled) result.Errors.Add("A valid IP address is required.");
                    else result.Warnings.Add("IP address is incomplete while the analyzer is disabled.");
                }
                if (config.IpPort < 1 || config.IpPort > 65535)
                {
                    if (config.Enabled) result.Errors.Add("TCP port must be between 1 and 65535.");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(config.ComPort))
                {
                    if (config.Enabled) result.Errors.Add("A COM port is required.");
                    else result.Warnings.Add("COM port is not configured while the analyzer is disabled.");
                }
                else if (!SerialPort.GetPortNames().Contains(config.ComPort, StringComparer.OrdinalIgnoreCase)) result.Warnings.Add("The selected COM port is not currently present on this computer.");
                if (config.BaudRate < 300 || config.BaudRate > 4000000) result.Errors.Add("Baud rate is outside the supported range.");
                if (config.DataBits < 5 || config.DataBits > 8) result.Errors.Add("Data bits must be between 5 and 8.");
                if (!Enum.IsDefined(typeof(Parity), config.Parity)) result.Errors.Add("Invalid serial parity.");
                if (!Enum.IsDefined(typeof(StopBits), config.StopBits)) result.Errors.Add("Invalid serial stop bits.");
            }

            if (existing != null)
            {
                foreach (var other in existing)
                {
                    if (other == null || string.Equals(other.Id, ignoredId, StringComparison.OrdinalIgnoreCase)) continue;
                    if (string.Equals(other.Name, config.Name, StringComparison.OrdinalIgnoreCase)) result.Errors.Add("Another analyzer already uses this name.");
                    if (string.Equals(config.ConnectionType, "IP", StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(other.ConnectionType, "IP", StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(other.IpAddress, config.IpAddress, StringComparison.OrdinalIgnoreCase) && other.IpPort == config.IpPort)
                        result.Warnings.Add("Another analyzer uses the same IP endpoint.");
                }
            }
            if (config.EnableEngineeringRangeCheck && config.EngineeringMax <= config.EngineeringMin) result.Errors.Add("Engineering max must be greater than engineering min.");
            return result;
        }
    }
}
