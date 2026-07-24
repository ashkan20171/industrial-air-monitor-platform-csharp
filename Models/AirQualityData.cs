using System;
using AshkanAQMS.Services;

namespace AshkanAQMS.Models
{
    public class AirQualityData
    {
        public DateTime Timestamp { get; set; }

        public double PM25 { get; set; }   // ug/m3
        public double PM10 { get; set; }   // ug/m3
        public double CO2 { get; set; }    // ppm
        public double NO2 { get; set; }    // ppb
        public double Temperature { get; set; } // C
        public double Humidity { get; set; }    // %

        public string Location { get; set; } = "Station A";

        public bool IsValid { get; set; } = true;
        public string ValidationMessage { get; set; } = string.Empty;

        public int AQI { get; set; }
        public string AQICategory { get; set; } = "Unknown";
        public string DominantPollutant { get; set; } = string.Empty;

        public AirQualityData()
        {
            Timestamp = DateTime.Now;
        }

        public int CalculateAQI()
        {
            var result = AqiCalculator.Calculate(this);
            AQI = result.AQI;
            AQICategory = result.Category;
            DominantPollutant = result.DominantPollutant;
            IsValid = result.IsValid;
            ValidationMessage = result.ValidationMessage;

            return AQI;
        }

        public string GetAQICategory(int aqi)
        {
            return AqiStandard.GetCategory(aqi);
        }
    }
}
