using System;

namespace AshkanAQMS.Models
{
    public class AirQualityData
    {
        public DateTime Timestamp { get; set; }
        public double PM25 { get; set; }  // ug/m3
        public double PM10 { get; set; }  // ug/m3
        public double CO2 { get; set; }   // ppm
        public double NO2 { get; set; }   // ppb
        public double Temperature { get; set; } // C
        public double Humidity { get; set; }    // %
        public string Location { get; set; } = "Station A";

        public int CalculateAQI()
        {
            // Simple logic for overall AQI demonstration based on PM2.5 (US EPA simplified)
            double pm = PM25;
            if (pm <= 12.0) return (int)(pm * 50 / 12.0);
            if (pm <= 35.4) return (int)(50 + (pm - 12.0) * 50 / 23.4);
            if (pm <= 55.4) return (int)(100 + (pm - 35.4) * 50 / 20.0);
            if (pm <= 150.4) return (int)(150 + (pm - 55.4) * 50 / 95.0);
            return 250;
        }

        public string GetAQICategory(int aqi)
        {
            if (aqi <= 50) return "Good";
            if (aqi <= 100) return "Moderate";
            if (aqi <= 150) return "Unhealthy for Sensitive Groups";
            if (aqi <= 200) return "Unhealthy";
            return "Hazardous";
        }
    }
}