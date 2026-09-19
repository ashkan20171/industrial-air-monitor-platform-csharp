using System;
using System.IO.Ports;
using System.Xml.Serialization;

namespace AshkanAQMS.Models
{
    public class AnalyzerConfig
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "New Analyzer";
        public string Model { get; set; } = "AQM-100";
        public string GasType { get; set; } = "PM2.5"; // PM2.5, PM10, CO2, NO2, O3, SO2
        public string Unit { get; set; } = "µg/m³"; // µg/m³, ppm, %
        public int Channel { get; set; } = 1;
        public int DecimalDigits { get; set; } = 1; // فرم تنظیم ممیز/اعشار
        public bool Enabled { get; set; } = true;
        public string DeviceId { get; set; } = "1";
        public int RequestIntervalMs { get; set; } = 1000;
        public int TimeoutMs { get; set; } = 2000;
        public int DataBits { get; set; } = 8;
        public Parity Parity { get; set; } = Parity.None;
        public StopBits StopBits { get; set; } = StopBits.One;
        public double Gain { get; set; } = 1.0;
        public double Offset { get; set; } = 0.0;
        public string Username { get; set; } = string.Empty;
        [XmlElement("ProtectedPassword")]
        public string ProtectedPassword { get; set; } = string.Empty;
        [XmlIgnore]
        public string Password { get; set; } = string.Empty;
        public string IgnorableErrors { get; set; } = string.Empty;

        // تنظیمات اتصال
        public string ConnectionType { get; set; } = "COM"; // COM or IP
        public string ComPort { get; set; } = string.Empty;
        public int BaudRate { get; set; } = 9600;
        public string IpAddress { get; set; } = "192.168.1.100";
        public int IpPort { get; set; } = 502; // Modbus TCP default

        // Generic ASCII/text protocol settings. These allow real analyzers that
        // expose a request/response value without inventing measurements.
        public string RequestCommand { get; set; } = string.Empty;
        public string ResponseDelimiter { get; set; } = "\r\n";
        public int ResponseFieldIndex { get; set; } = 0;
        public string ResponseKey { get; set; } = string.Empty;
        public string ResponseRegex { get; set; } = string.Empty;
        public int ReadAfterWriteDelayMs { get; set; } = 50;
        public string EncodingName { get; set; } = "ASCII";
    }
}
