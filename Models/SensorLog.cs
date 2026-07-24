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
        public double CO2 { get; set; }
        public double AQI { get; set; }
        public string Status { get; set; }

        public string ToCsvLine()
        {
            return string.Join(",",
                EscapeCsv(Timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)),
                EscapeCsv(AnalyzerId ?? string.Empty),
                EscapeCsv(PM25.ToString(CultureInfo.InvariantCulture)),
                EscapeCsv(CO2.ToString(CultureInfo.InvariantCulture)),
                EscapeCsv(AQI.ToString(CultureInfo.InvariantCulture)),
                EscapeCsv(Status ?? string.Empty)
            );
        }

        public static SensorLog FromCsvLine(string csvLine)
        {
            if (string.IsNullOrWhiteSpace(csvLine))
                return null;

            var parts = ParseCsvLine(csvLine);

            if (parts.Count < 6)
                return null;

            return new SensorLog
            {
                Timestamp = DateTime.Parse(parts[0], CultureInfo.InvariantCulture),
                AnalyzerId = parts[1],
                PM25 = double.Parse(parts[2], CultureInfo.InvariantCulture),
                CO2 = double.Parse(parts[3], CultureInfo.InvariantCulture),
                AQI = double.Parse(parts[4], CultureInfo.InvariantCulture),
                Status = parts[5]
            };
        }

        private static string EscapeCsv(string value)
        {
            if (value == null)
                return "\"\"";

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        private static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            if (line == null)
                return result;

            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(ch);
                }
            }

            result.Add(sb.ToString());
            return result;
        }
    }
}
