using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SadraAQMS.Analyzers
{
    /// <summary>
    /// Serinus S50 (SO2)
    /// - اگر portCOM به شکل TCP:host:port یا host:port باشد از TCP استفاده می‌کند، وگرنه سریال
    /// - فرمان خواندن: "DCONC,<id>\r"
    /// نمونه تنظیم پورت: TCP:192.168.1.50:4000  یا  192.168.1.50:4000
    /// </summary>
    class SerinusS50Analyzer : deviceAnalyzer
    {
        public byte MultiDropID { get; set; } = 1;

        // --- TCP state ---
        private System.Net.Sockets.TcpClient _tcp;
        private System.Net.Sockets.NetworkStream _ns;
        private System.Threading.Thread _rxThread;
        private volatile bool _runRx;

        // --- CTORها: مطابق امضای deviceAnalyzer(DeviceType, string) ---
        public SerinusS50Analyzer(DeviceType dvType, string devID)
            : base(dvType, devID)
        {
            // deviceAnalyzer خودش dvType/devID را نگه می‌دارد
            byte idParsed; if (!byte.TryParse(devID, out idParsed)) idParsed = 1;
            MultiDropID = idParsed;
            EnsurePolutionList();
        }

        public SerinusS50Analyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
            : base(dvType, devID) // توجه: deviceAnalyzer سازندهٔ ۴پارامتری ندارد
        {
            this.DeviceName = devName;
            int ie; if (int.TryParse(ignorableErrors, out ie)) this.IgnorableErrors = ie;

            byte idParsed; if (!byte.TryParse(devID, out idParsed)) idParsed = 1;
            MultiDropID = idParsed;
            EnsurePolutionList();
        }

        private void EnsurePolutionList()
        {
            if (polutions == null) polutions = new List<polutionType>();
            if (polutions.Count == 0)
            {
                // امضای polutionType طبق کدِ تو:
                // polutionType(byte polID, int deviceTypeId, int alarmVal, byte untID, float _gin=1, float _offst=0, byte _mapperID=0)
                polutions.Add(new polutionType(
                    polID: (byte)2,                  // شناسه آلاینده (فرضاً SO2)
                    deviceTypeId: (int)this.deviceType,
                    alarmVal: 0,
                    untID: (byte)0,
                    _gin: 1f,
                    _offst: 0f,
                    _mapperID: (byte)0
                ));
            }
        }

        // --- اتصال: قبل از کارهای مشترک base، اگر TCP است بازش می‌کنیم
        internal override void initialize_Connect()
        {
            if (IsTcpPortSpec()) TryOpenTcp();
            base.initialize_Connect(); // تایمر و لاگ و...
        }

        private bool IsTcpPortSpec()
        {
            if (string.IsNullOrWhiteSpace(portCOM)) return false;
            var s = portCOM.Trim();
            if (s.StartsWith("TCP", StringComparison.OrdinalIgnoreCase)) return true;
            // host:port (و نه COMx:)
            return s.Contains(":") && !s.StartsWith("COM", StringComparison.OrdinalIgnoreCase);
        }

        private void TryOpenTcp()
        {
            try
            {
                string spec = Regex.Replace(portCOM, "^TCP:", "", RegexOptions.IgnoreCase).Trim();
                var parts = spec.Split(':');
                string host = parts[0];
                int port = (parts.Length > 1 && int.TryParse(parts[1], out var p)) ? p : 4000;

                _tcp = new System.Net.Sockets.TcpClient
                {
                    ReceiveTimeout = 5000,
                    SendTimeout = 5000,
                    NoDelay = true
                };
                _tcp.Connect(host, port);
                _ns = _tcp.GetStream();

                _runRx = true;
                _rxThread = new System.Threading.Thread(RxLoop) { IsBackground = true, Name = "S50-TCP-RX" };
                _rxThread.Start();

                isConnected = true;
                statusLog("[S50] TCP connected to " + host + ":" + port);
            }
            catch (Exception ex)
            {
                isConnected = false;
                errorLog("[S50 TCP connect] " + ex.Message);
            }
        }

        private void EnsureTcpConnected()
        {
            if (_tcp != null && _tcp.Connected && _ns != null && _ns.CanWrite) return;
            if (IsTcpPortSpec()) TryOpenTcp();
        }

        private void RxLoop()
        {
            var buf = new byte[4096];
            try
            {
                while (_runRx && _tcp != null && _tcp.Connected && _ns != null && _ns.CanRead)
                {
                    if (!_ns.DataAvailable) { Thread.Sleep(20); continue; }
                    int n = _ns.Read(buf, 0, buf.Length);
                    if (n > 0)
                    {
                        for (int i = 0; i < n; i++) messageReceived.Add(buf[i]);
                        try { parseMessage(); } catch { /* نذاریم RX بخوابه */ }
                    }
                }
            }
            catch (Exception ex) { errorLog("[S50 TCP Rx] " + ex.Message); }
        }

        // --- فرمان خواندن فقط وقتی flagGetCurrentData روشن است
        public override void sendReadCommand()
        {
            try
            {
                if (!flagGetCurrentData) return;

                string cmd = "DCONC," + MultiDropID.ToString("000") + "\r";
                EnsureTcpConnected();

                if (_ns != null && _tcp != null && _tcp.Connected && _ns.CanWrite)
                {
                    var bytes = Encoding.ASCII.GetBytes(cmd);
                    _ns.Write(bytes, 0, bytes.Length);
                    _ns.Flush();
                }
                else
                {
                    // بازگشت به سریال
                    messageSendOnSerial(cmd);
                }
            }
            catch (Exception ex) { errorLog("[S50 sendReadCommand] " + ex.Message); }
        }

        // --- پارس پیام‌ها: رشته دریافتی را خط‌به‌خط چک می‌کنیم
        public override void parseMessage()
        {
            try
            {
                string resp = Encoding.UTF8.GetString(messageReceived.ToArray());
                resp = string.Join("", resp.Split('\0')); // حذف NUL

                if (writeMode) { sw.WriteLine(resp); sw.Flush(); }

                var lines = resp.Replace("\r", "\n").Split('\n');
                foreach (var raw in lines)
                {
                    var line = (raw ?? "").Trim();
                    if (line.Length == 0) continue;

                    // الگو 1: "<value> <hexStatus>"  مثل: "0.345 A01F"
                    var m = Regex.Match(line, @"^(?<val>[+-]?\d+(?:\.\d+)?)\s+(?<hex>[0-9A-Fa-f]+)$");
                    if (m.Success)
                    {
                        if (double.TryParse(m.Groups["val"].Value,
                            System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture, out var v))
                        {
                            ApplyConcentration((float)v);
                            continue;
                        }
                    }

                    // الگو 2: "SO2=0.345 ppm"
                    var m2 = Regex.Match(line, @"SO2\s*=\s*(?<v>[+-]?\d+(?:\.\d+)?)\s*(?<u>ppm|mg/m3)?", RegexOptions.IgnoreCase);
                    if (m2.Success)
                    {
                        if (double.TryParse(m2.Groups["v"].Value,
                            System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture, out var v2))
                        {
                            ApplyConcentration((float)v2);
                            continue;
                        }
                    }

                    OnSpecialMessageRecived("SerinusS50 Unparsed: " + line);
                }

                workingStatus = true;
                timeLastMessageRecieved = DateTime.Now;
            }
            catch (Exception ex) { errorLog("[S50 parseMessage] " + ex.Message); }
            finally { messageReceived.Clear(); }
        }

        private void ApplyConcentration(float value)
        {
            if (polutions == null || polutions.Count == 0) return;
            var ch = polutions[0];
            ch.concentration = value * ch.gain + ch.offset;
            polutions[0] = ch;
        }

        ~SerinusS50Analyzer()
        {
            try { _runRx = false; if (_ns != null) _ns.Close(); if (_tcp != null) _tcp.Close(); } catch { }
        }
    }

    /// <summary>
    /// Alias برای سازگاری با نام قبلی در پروژه.
    /// </summary>
    public class Serinus50Analyzer : deviceAnalyzer
    {
        public Serinus50Analyzer(DeviceType dvType, string devID)
            : base(dvType, devID) { }

        public Serinus50Analyzer(DeviceType dvType, string devID, string devName, string ignorableErrors)
            : base(dvType, devID) // چون deviceAnalyzer سازندهٔ ۴پارامتری ندارد
        {
            this.DeviceName = devName;
            int ie; if (int.TryParse(ignorableErrors, out ie)) this.IgnorableErrors = ie;
        }
    }
}

