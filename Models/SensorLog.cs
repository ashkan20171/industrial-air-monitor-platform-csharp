using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AshkanAQMS.Models
{
    public class SensorLog
    {
        public DateTime Timestamp { get; set; }
        public string AnalyzerId { get; set; }
        public double PM25 { get; set; }
        public double PM10 { get; set; }
        public double CO2 { get; set; }
        public double NO2 { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double AQI { get; set; }
        public string Status { get; set; }

        public string ToCsvLine()
        {
            return string.Join(",", Escape(Timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)), Escape(AnalyzerId),
                Escape(PM25.ToString("0.0", CultureInfo.InvariantCulture)), Escape(PM10.ToString("0.0", CultureInfo.InvariantCulture)),
                Escape(CO2.ToString("0.0", CultureInfo.InvariantCulture)), Escape(NO2.ToString("0.0", CultureInfo.InvariantCulture)),
                Escape(Temperature.ToString("0.0", CultureInfo.InvariantCulture)), Escape(Humidity.ToString("0.0", CultureInfo.InvariantCulture)),
                Escape(AQI.ToString("0.0", CultureInfo.InvariantCulture)), Escape(Status));
        }

        public static SensorLog FromCsvLine(string line)
        {
            var p = ParseCsvLine(line);
            if (p.Count < 6) return null;
            DateTime ts; double pm25, pm10 = 0, co2 = 0, no2 = 0, temp = 0, hum = 0, aqi = 0;
            if (!DateTime.TryParse(p[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out ts) ||
                !double.TryParse(p[2], NumberStyles.Float, CultureInfo.InvariantCulture, out pm25)) return null;
            if (p.Count >= 10)
            {
                double.TryParse(p[3], NumberStyles.Float, CultureInfo.InvariantCulture, out pm10);
                double.TryParse(p[4], NumberStyles.Float, CultureInfo.InvariantCulture, out co2);
                double.TryParse(p[5], NumberStyles.Float, CultureInfo.InvariantCulture, out no2);
                double.TryParse(p[6], NumberStyles.Float, CultureInfo.InvariantCulture, out temp);
                double.TryParse(p[7], NumberStyles.Float, CultureInfo.InvariantCulture, out hum);
                double.TryParse(p[8], NumberStyles.Float, CultureInfo.InvariantCulture, out aqi);
                return new SensorLog { Timestamp=ts, AnalyzerId=p[1], PM25=pm25, PM10=pm10, CO2=co2, NO2=no2, Temperature=temp, Humidity=hum, AQI=aqi, Status=p[9] };
            }
            // Legacy six-column format: Timestamp,AnalyzerId,PM25,CO2,AQI,Status
            double.TryParse(p[3], NumberStyles.Float, CultureInfo.InvariantCulture, out co2);
            double.TryParse(p[4], NumberStyles.Float, CultureInfo.InvariantCulture, out aqi);
            return new SensorLog { Timestamp=ts, AnalyzerId=p[1], PM25=pm25, CO2=co2, AQI=aqi, Status=p[5] };
        }

        private static string Escape(string value)
        {
            value = value ?? string.Empty;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
        private static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>(); if (line == null) return result; var sb = new StringBuilder(); bool q=false;
            for (int i=0;i<line.Length;i++) { char c=line[i]; if(c=='\"'){ if(q && i+1<line.Length && line[i+1]=='\"'){sb.Append('\"');i++;} else q=!q;} else if(c==','&&!q){result.Add(sb.ToString());sb.Clear();} else sb.Append(c); }
            result.Add(sb.ToString()); return result;
        }
    }
}
