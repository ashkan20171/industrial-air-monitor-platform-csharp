using System;

namespace AshkanAQMS.Models
{
    public class SensorLog
    {
        public DateTime Timestamp { get; set; }
        public string AnalyzerId { get; set; }
        public double PM25 { get; set; }
        public double CO2 { get; set; }
        public double AQI { get; set; }
        public string Status { get; set; }

        // تبدیل به فرمت CSV برای ذخیره در فایل
        public string ToCsvLine()
        {
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss},{AnalyzerId},{PM25},{CO2},{AQI},{Status}";
        }

        // تبدیل از CSV به مدل
        public static SensorLog FromCsvLine(string csvLine)
        {
            var parts = csvLine.Split(',');
            return new SensorLog
            {
                Timestamp = DateTime.Parse(parts[0]),
                AnalyzerId = parts[1],
                PM25 = double.Parse(parts[2]),
                CO2 = double.Parse(parts[3]),
                AQI = double.Parse(parts[4]),
                Status = parts[5]
            };
        }
    }
}
