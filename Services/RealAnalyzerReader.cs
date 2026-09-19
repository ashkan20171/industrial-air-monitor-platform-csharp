using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public enum AnalyzerReadingQuality
    {
        Good,
        Disabled,
        Offline,
        CommunicationError,
        InvalidData,
        Timeout
    }

    public sealed class AnalyzerReading
    {
        public string AnalyzerId { get; set; }
        public string AnalyzerName { get; set; }
        public string GasType { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public string RawResponse { get; set; }
        public AnalyzerReadingQuality Quality { get; set; }
        public string Message { get; set; }
        public long ResponseTimeMs { get; set; }

        public bool IsUsable
        {
            get { return Quality == AnalyzerReadingQuality.Good && !double.IsNaN(Value) && !double.IsInfinity(Value); }
        }
    }

    /// <summary>
    /// Generic real-hardware reader for simple ASCII/number based analyzers.
    /// Vendor-specific binary protocols should use a dedicated driver instead of pretending that a value was received.
    /// </summary>
    public sealed class RealAnalyzerReader
    {
        public Task<AnalyzerReading> ReadAsync(AnalyzerConfig config, CancellationToken token)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (!config.Enabled)
            {
                return Task.FromResult(Failure(config, AnalyzerReadingQuality.Disabled, "Analyzer is disabled."));
            }

            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                var watch = Stopwatch.StartNew();
                try
                {
                    AnalyzerReading result;
                    if (string.Equals(config.ConnectionType, "IP", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(config.ConnectionType, "TCP", StringComparison.OrdinalIgnoreCase))
                    {
                        result = ReadTcp(config, token);
                    }
                    else
                    {
                        result = ReadSerial(config, token);
                    }
                    result.ResponseTimeMs = watch.ElapsedMilliseconds;
                    return result;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    return Failure(config, AnalyzerReadingQuality.CommunicationError, ex.Message, watch.ElapsedMilliseconds);
                }
            }, token);
        }

        private AnalyzerReading ReadSerial(AnalyzerConfig config, CancellationToken token)
        {
            var ports = SerialPort.GetPortNames();
            if (!ports.Any(p => string.Equals(p, config.ComPort, StringComparison.OrdinalIgnoreCase)))
            {
                return Failure(config, AnalyzerReadingQuality.Offline,
                    "Serial port '" + config.ComPort + "' is not available. Available ports: " +
                    (ports.Length == 0 ? "none" : string.Join(", ", ports.OrderBy(x => x))));
            }

            using (var port = new SerialPort(config.ComPort, config.BaudRate, config.Parity, config.DataBits, config.StopBits))
            {
                port.ReadTimeout = Math.Max(100, config.TimeoutMs);
                port.WriteTimeout = Math.Max(100, config.TimeoutMs);
                port.Open();
                token.ThrowIfCancellationRequested();

                if (!string.IsNullOrWhiteSpace(config.RequestCommand))
                {
                    port.Write(Unescape(config.RequestCommand));
                    if (config.ReadAfterWriteDelayMs > 0)
                        Thread.Sleep(Math.Min(config.ReadAfterWriteDelayMs, config.TimeoutMs));
                }

                string response = ReadUntilDelimiterOrTimeout(port, config, token);
                return ParseResponse(config, response);
            }
        }

        private AnalyzerReading ReadTcp(AnalyzerConfig config, CancellationToken token)
        {
            using (var client = new TcpClient())
            {
                var connectTask = client.ConnectAsync(config.IpAddress, config.IpPort);
                if (!connectTask.Wait(Math.Max(100, config.TimeoutMs)))
                    return Failure(config, AnalyzerReadingQuality.Timeout, "TCP connection timed out.");

                token.ThrowIfCancellationRequested();
                using (var stream = client.GetStream())
                {
                    stream.ReadTimeout = Math.Max(100, config.TimeoutMs);
                    stream.WriteTimeout = Math.Max(100, config.TimeoutMs);

                    if (!string.IsNullOrWhiteSpace(config.RequestCommand))
                    {
                        byte[] command = Encoding.GetEncoding(config.EncodingName ?? "ASCII").GetBytes(Unescape(config.RequestCommand));
                        stream.Write(command, 0, command.Length);
                        stream.Flush();
                        if (config.ReadAfterWriteDelayMs > 0)
                            Thread.Sleep(Math.Min(config.ReadAfterWriteDelayMs, config.TimeoutMs));
                    }

                    string response = ReadNetworkResponse(stream, config, token);
                    return ParseResponse(config, response);
                }
            }
        }

        private static string ReadUntilDelimiterOrTimeout(SerialPort port, AnalyzerConfig config, CancellationToken token)
        {
            var watch = Stopwatch.StartNew();
            var builder = new StringBuilder();
            string delimiter = Unescape(config.ResponseDelimiter);
            while (watch.ElapsedMilliseconds < Math.Max(100, config.TimeoutMs))
            {
                token.ThrowIfCancellationRequested();
                if (port.BytesToRead > 0)
                    builder.Append(port.ReadExisting());
                string current = builder.ToString();
                if (!string.IsNullOrEmpty(delimiter) && current.IndexOf(delimiter, StringComparison.Ordinal) >= 0)
                    break;
                if (string.IsNullOrWhiteSpace(delimiter) && ContainsParsableNumber(current, config))
                    break;
                Thread.Sleep(20);
            }
            return builder.ToString();
        }

        private static string ReadNetworkResponse(NetworkStream stream, AnalyzerConfig config, CancellationToken token)
        {
            var watch = Stopwatch.StartNew();
            var builder = new StringBuilder();
            byte[] buffer = new byte[2048];
            string delimiter = Unescape(config.ResponseDelimiter);
            while (watch.ElapsedMilliseconds < Math.Max(100, config.TimeoutMs))
            {
                token.ThrowIfCancellationRequested();
                if (stream.DataAvailable)
                {
                    int count = stream.Read(buffer, 0, buffer.Length);
                    if (count > 0) builder.Append(Encoding.GetEncoding(config.EncodingName ?? "ASCII").GetString(buffer, 0, count));
                }
                string current = builder.ToString();
                if (!string.IsNullOrEmpty(delimiter) && current.IndexOf(delimiter, StringComparison.Ordinal) >= 0)
                    break;
                if (string.IsNullOrWhiteSpace(delimiter) && ContainsParsableNumber(current, config))
                    break;
                Thread.Sleep(20);
            }
            return builder.ToString();
        }

        private static bool ContainsParsableNumber(string response, AnalyzerConfig config)
        {
            double value;
            return TryParseValue(response, config, out value);
        }

        private static AnalyzerReading ParseResponse(AnalyzerConfig config, string response)
        {
            response = response ?? string.Empty;
            double value;
            if (!TryParseValue(response, config, out value))
            {
                return Failure(config, string.IsNullOrWhiteSpace(response) ? AnalyzerReadingQuality.Timeout : AnalyzerReadingQuality.InvalidData,
                    string.IsNullOrWhiteSpace(response) ? "No response received from analyzer." : "Analyzer response did not contain a valid numeric value.");
            }

            value = value * config.Gain + config.Offset;
            return new AnalyzerReading
            {
                AnalyzerId = config.Id,
                AnalyzerName = config.Name,
                GasType = config.GasType,
                Timestamp = DateTime.Now,
                Value = value,
                RawResponse = response.Trim(),
                Quality = AnalyzerReadingQuality.Good,
                Message = "Valid sample received."
            };
        }

        private static bool TryParseValue(string response, AnalyzerConfig config, out double value)
        {
            value = 0;
            string text = response == null ? string.Empty : response.Trim();
            if (string.IsNullOrWhiteSpace(text)) return false;

            if (!string.IsNullOrWhiteSpace(config.ResponseRegex))
            {
                var match = Regex.Match(text, config.ResponseRegex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                if (match.Success)
                {
                    string candidate = match.Groups["value"].Success ? match.Groups["value"].Value : match.Groups.Count > 1 ? match.Groups[1].Value : match.Value;
                    if (double.TryParse(candidate.Trim(), NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value)) return true;
                }
            }

            string key = config.ResponseKey;
            if (!string.IsNullOrWhiteSpace(key))
            {
                var match = Regex.Match(text, "(?:^|[;,\\r\\n\\t])\\s*" + Regex.Escape(key) + "\\s*[:=]\\s*([-+]?\\d+(?:[\\.,]\\d+)?)", RegexOptions.IgnoreCase);
                if (match.Success && TryParseFlexible(match.Groups[1].Value, out value)) return true;
            }

            if (!string.IsNullOrWhiteSpace(config.ResponseDelimiter))
            {
                string[] fields = text.Split(new[] { Unescape(config.ResponseDelimiter) }, StringSplitOptions.None);
                if (config.ResponseFieldIndex >= 0 && config.ResponseFieldIndex < fields.Length && TryParseFlexible(fields[config.ResponseFieldIndex], out value)) return true;
            }

            var numbers = Regex.Matches(text, @"[-+]?(?:\d+(?:[\.,]\d*)?|[\.,]\d+)(?:[eE][-+]?\d+)?");
            // If a structured response was supplied without a configured key/field,
            // prefer the last numeric token. This avoids interpreting the 25 in
            // "PM25=12.3" as the measurement.
            for (int i = numbers.Count - 1; i >= 0; i--)
            {
                if (TryParseFlexible(numbers[i].Value, out value)) return true;
            }
            return false;
        }

        private static bool TryParseFlexible(string value, out double result)
        {
            value = (value ?? string.Empty).Trim();
            if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result)) return true;
            value = value.Replace(',', '.');
            return double.TryParse(value, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
        }

        private static string Unescape(string value)
        {
            if (value == null) return string.Empty;
            return value.Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
        }

        private static AnalyzerReading Failure(AnalyzerConfig config, AnalyzerReadingQuality quality, string message, long responseTimeMs = 0)
        {
            return new AnalyzerReading
            {
                AnalyzerId = config == null ? string.Empty : config.Id,
                AnalyzerName = config == null ? string.Empty : config.Name,
                GasType = config == null ? string.Empty : config.GasType,
                Timestamp = DateTime.Now,
                Value = double.NaN,
                RawResponse = string.Empty,
                Quality = quality,
                Message = message,
                ResponseTimeMs = responseTimeMs
            };
        }
    }
}
