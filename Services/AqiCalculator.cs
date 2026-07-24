using System;
using System.Collections.Generic;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public class AqiCalculationResult
    {
        public int AQI { get; set; }
        public string Category { get; set; } = "Unknown";
        public string DominantPollutant { get; set; } = string.Empty;
        public bool IsValid { get; set; } = true;
        public string ValidationMessage { get; set; } = string.Empty;

        public Dictionary<string, int> SubIndices { get; set; } = new Dictionary<string, int>();
    }

    public static class AqiCalculator
    {
        public static AqiCalculationResult Calculate(AirQualityData data)
        {
            var result = new AqiCalculationResult();

            if (data == null)
            {
                result.IsValid = false;
                result.ValidationMessage = "Air quality data is null.";
                result.AQI = 0;
                result.Category = "Unknown";
                return result;
            }

            var validationErrors = new List<string>();

            if (!AqiStandard.IsValidConcentration(data.PM25))
                validationErrors.Add("PM2.5 is invalid");

            if (!AqiStandard.IsValidConcentration(data.PM10))
                validationErrors.Add("PM10 is invalid");

            if (!AqiStandard.IsValidConcentration(data.NO2))
                validationErrors.Add("NO2 is invalid");

            if (!AqiStandard.IsValidConcentration(data.CO2))
                validationErrors.Add("CO2 is invalid");

            if (data.Timestamp == default(DateTime))
                validationErrors.Add("Timestamp is invalid");

            if (validationErrors.Count > 0)
            {
                result.IsValid = false;
                result.ValidationMessage = string.Join("; ", validationErrors);
            }

            int pm25Index = AqiStandard.CalculateFromBreakpoints(data.PM25, AqiStandard.PM25Breakpoints);
            int pm10Index = AqiStandard.CalculateFromBreakpoints(data.PM10, AqiStandard.PM10Breakpoints);
            int no2Index = AqiStandard.CalculateFromBreakpoints(data.NO2, AqiStandard.NO2Breakpoints);

            // CO2 is not a formal AQI pollutant.
            // We keep it as a quality indicator only.
            int co2Index = CalculateInternalCO2Index(data.CO2);

            result.SubIndices["PM2.5"] = pm25Index;
            result.SubIndices["PM10"] = pm10Index;
            result.SubIndices["NO2"] = no2Index;
            result.SubIndices["CO2"] = co2Index;

            int maxIndex = pm25Index;
            string dominant = "PM2.5";

            foreach (var item in result.SubIndices)
            {
                if (item.Value > maxIndex)
                {
                    maxIndex = item.Value;
                    dominant = item.Key;
                }
            }

            result.AQI = AqiStandard.ClampAQI(maxIndex);
            result.Category = AqiStandard.GetCategory(result.AQI);
            result.DominantPollutant = dominant;

            return result;
        }

        private static int CalculateInternalCO2Index(double co2)
        {
            if (!AqiStandard.IsValidConcentration(co2))
                return 0;

            // Internal quality indicator, not official AQI.
            // 400–1000 ppm => 0–100
            // 1000–2000 ppm => 100–300
            // 2000+ => 500
            if (co2 <= 400) return 0;
            if (co2 <= 1000) return (int)Math.Round((co2 - 400) * 100.0 / 600.0);
            if (co2 <= 2000) return (int)Math.Round(100 + (co2 - 1000) * 200.0 / 1000.0);
            return 500;
        }

        public static int CalculatePM25Only(double pm25)
        {
            return AqiStandard.CalculateFromBreakpoints(pm25, AqiStandard.PM25Breakpoints);
        }

        public static string GetCategory(int aqi)
        {
            return AqiStandard.GetCategory(aqi);
        }
    }
}
