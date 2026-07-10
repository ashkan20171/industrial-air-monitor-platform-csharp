using System;

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

        // تنظیمات اتصال
        public string ConnectionType { get; set; } = "COM"; // COM or IP
        public string ComPort { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public string IpAddress { get; set; } = "192.168.1.100";
        public int IpPort { get; set; } = 502; // Modbus TCP default
    }
}
