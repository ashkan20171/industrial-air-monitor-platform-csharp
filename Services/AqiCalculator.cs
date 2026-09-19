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
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; } = string.Empty;
        public Dictionary<string, int> SubIndices { get; set; } = new Dictionary<string, int>();
    }

    public static class AqiCalculator
    {
        public static AqiCalculationResult Calculate(AirQualityData data)
        {
            var result = new AqiCalculationResult { IsValid = false, AQI = 0, Category = "No Data" };
            if (data == null)
            {
                result.ValidationMessage = "Air quality data is null.";
                return result;
            }

            int maxIndex = 0;
            string dominant = string.Empty;
            bool anyFormalPollutant = false;

            if (AqiStandard.IsValidConcentration(data.PM25))
            {
                int index = AqiStandard.CalculateFromBreakpoints(data.PM25, AqiStandard.PM25Breakpoints);
                result.SubIndices["PM2.5"] = index;
                anyFormalPollutant = true;
                if (index >= maxIndex) { maxIndex = index; dominant = "PM2.5"; }
            }
            if (AqiStandard.IsValidConcentration(data.PM10))
            {
                int index = AqiStandard.CalculateFromBreakpoints(data.PM10, AqiStandard.PM10Breakpoints);
                result.SubIndices["PM10"] = index;
                anyFormalPollutant = true;
                if (index > maxIndex) { maxIndex = index; dominant = "PM10"; }
            }
            if (AqiStandard.IsValidConcentration(data.NO2))
            {
                int index = AqiStandard.CalculateFromBreakpoints(data.NO2, AqiStandard.NO2Breakpoints);
                result.SubIndices["NO2"] = index;
                anyFormalPollutant = true;
                if (index > maxIndex) { maxIndex = index; dominant = "NO2"; }
            }

            if (AqiStandard.IsValidConcentration(data.CO2))
                result.SubIndices["CO2"] = CalculateInternalCO2Index(data.CO2); // Informational only, not official AQI.

            if (!anyFormalPollutant)
            {
                result.ValidationMessage = "No formal AQI pollutant is available from enabled analyzers.";
                return result;
            }

            result.AQI = AqiStandard.ClampAQI(maxIndex);
            result.Category = AqiStandard.GetCategory(result.AQI);
            result.DominantPollutant = dominant;
            result.IsValid = data.Timestamp != default(DateTime);
            result.ValidationMessage = result.IsValid ? string.Empty : "Timestamp is invalid.";
            return result;
        }

        private static int CalculateInternalCO2Index(double co2)
        {
            if (!AqiStandard.IsValidConcentration(co2)) return 0;
            if (co2 <= 400) return 0;
            if (co2 <= 1000) return (int)Math.Round((co2 - 400) * 100.0 / 600.0);
            if (co2 <= 2000) return (int)Math.Round(100 + (co2 - 1000) * 200.0 / 1000.0);
            return 500;
        }

        public static int CalculatePM25Only(double pm25) { return AqiStandard.CalculateFromBreakpoints(pm25, AqiStandard.PM25Breakpoints); }
        public static string GetCategory(int aqi) { return AqiStandard.GetCategory(aqi); }
    }
}
