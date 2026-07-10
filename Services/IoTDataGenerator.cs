using System;
using System.Collections.Generic;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public class IoTDataGenerator
    {
        private readonly Random _random = new Random();

        public AirQualityData GenerateNext(string location, AirQualityData previous)
        {
            // Walk-random generation to simulate realistic continuous sensor data
            double pm25 = Clamp(previous.PM25 + (_random.NextDouble() * 8 - 4), 2, 220);
            double pm10 = Clamp(pm25 * 1.5 + (_random.NextDouble() * 10 - 5), 5, 350);
            double co2 = Clamp(previous.CO2 + (_random.NextDouble() * 40 - 20), 380, 1500);
            double no2 = Clamp(previous.NO2 + (_random.NextDouble() * 6 - 3), 5, 80);
            double temp = Clamp(previous.Temperature + (_random.NextDouble() * 1.0 - 0.5), 15, 38);
            double hum = Clamp(previous.Humidity + (_random.NextDouble() * 4 - 2), 20, 85);

            return new AirQualityData
            {
                Timestamp = DateTime.Now,
                PM25 = Math.Round(pm25, 1),
                PM10 = Math.Round(pm10, 1),
                CO2 = Math.Round(co2, 0),
                NO2 = Math.Round(no2, 1),
                Temperature = Math.Round(temp, 1),
                Humidity = Math.Round(hum, 1),
                Location = location
            };
        }

        public AirQualityData GetInitial(string location)
        {
            return new AirQualityData
            {
                Timestamp = DateTime.Now,
                PM25 = 15.0,
                PM10 = 25.0,
                CO2 = 450.0,
                NO2 = 12.0,
                Temperature = 22.5,
                Humidity = 45.0,
                Location = location
            };
        }

        private double Clamp(double val, double min, double max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }
    }
}