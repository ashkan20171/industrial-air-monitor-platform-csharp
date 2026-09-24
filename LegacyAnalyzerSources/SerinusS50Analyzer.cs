
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SadraAQMS.Analyzers
{
    /// <summary>
    /// Serinus S50 (SO2) Analyzer – compatible with deviceAnalyzer base.
    /// Sends EC9800 read command: "DCONC,<id>\r" and parses responses.
    /// </summary>
    class SerinusS50Analyzer : deviceAnalyzer
    {
        public byte MultiDropID { get; set; } = 1;

        public SerinusS50Analyzer(DeviceType dvType, string devID)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            if (!byte.TryParse(devID, out var id)) id = 1;
            this.MultiDropID = id;
            ensurePolutionList();
        }

        public SerinusS50Analyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
        {
            this.deviceType = dvType;
            this.deviceName = dvType.ToString();
            this.DeviceName = devName;
            if (!byte.TryParse(devID, out var id)) id = 1;
            this.MultiDropID = id;
            int.TryParse(ignorableErrors, out IgnorableErrors);
            ensurePolutionList();
        }

        private void ensurePolutionList()
        {
            if (polutions == null) polutions = new List<polutionType>();
            if (polutions.Count == 0)
            {
                polutions.Add(new polutionType
                {
                    ID = 1,
                    unitID = 0,
                    gain = 1,
                    offset = 0,
                    concentration = 0,
                    mapperID = 0,
                    deviceT = (int)this.deviceType,
                    alarmValue = 0
                });
            }
        }

        public override void sendReadCommand()
        {
            try
            {
                if (flagGetCurrentData)
                {
                    messageSendOnSerial($"DCONC,{MultiDropID:000}\r");
                }
            }
            catch (Exception ex)
            {
                errorLog("[SerinusS50 sendReadCommand] " + ex.Message);
            }
        }

        public override void parseMessage()
        {
            try
            {
                string resp = Encoding.UTF8.GetString(messageReceived.ToArray());
                resp = string.Join("", resp.Split('\\0'));

                if (writeMode) { sw.WriteLine(resp); sw.Flush(); }

                foreach (var raw in resp.Replace("\\r", "\\n").Split('\\n'))
                {
                    var line = raw.Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    // Pattern: "<value> <hex>"
                    var m = Regex.Match(line, @"^(?<val>[+-]?\\d+(?:\\.\\d+)?)\\s+(?<hex>[0-9A-Fa-f]+)$");
                    if (m.Success)
                    {
                        if (double.TryParse(m.Groups["val"].Value,
                            System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture, out var v))
                        {
                            applyConcentration((float)v);
                            break;
                        }
                    }

                    // Pattern: "SO2=0.345 ppm"
                    var m2 = Regex.Match(line, @"SO2\\s*=\\s*(?<v>[+-]?\\d+(?:\\.\\d+)?)\\s*(?<u>ppm|mg/m3)?", RegexOptions.IgnoreCase);
                    if (m2.Success)
                    {
                        if (double.TryParse(m2.Groups["v"].Value,
                            System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture, out var v2))
                        {
                            applyConcentration((float)v2);
                            break;
                        }
                    }

                    OnSpecialMessageRecived("SerinusS50 Unparsed: " + line);
                }
            }
            catch (Exception ex)
            {
                errorLog("[SerinusS50 parseMessage] " + ex.Message);
            }
            finally
            {
                messageReceived.Clear();
            }
        }

        private void applyConcentration(float value)
        {
            if (polutions == null || polutions.Count == 0) return;
            var ch = polutions[0];
            ch.concentration = value * ch.gain + ch.offset;
            polutions[0] = ch;
        }
    }

    // Optional alias for older name
    class Serinus50Analyzer : SerinusS50Analyzer
    {
        public Serinus50Analyzer(DeviceType dvType, string devID) : base(dvType, devID) { }
        public Serinus50Analyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
            : base(dvType, devID, devName, ignorableErrors) { }
    }
}
