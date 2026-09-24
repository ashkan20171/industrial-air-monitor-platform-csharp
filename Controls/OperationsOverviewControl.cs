using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    /// <summary>
    /// Read-only station command dashboard. Navigation deliberately stays in the
    /// permanent menu so this surface is reserved for operational context.
    /// </summary>
    public sealed class OperationsOverviewControl : UserControl
    {
        private static readonly DateTime ProcessStarted = DateTime.Now;
        private readonly StorageService storage = new StorageService();
        private readonly DbService db = new DbService();
        private readonly TableLayoutPanel kpis = new TableLayoutPanel();
        private readonly TableLayoutPanel body = new TableLayoutPanel();
        private readonly DataGridView grid = new DataGridView();
        private readonly Label status = new Label();
        private readonly Label aqiValue = new Label();
        private readonly Label aqiCategory = new Label();
        private readonly Label alarmSummary = new Label();
        private readonly Label weatherSummary = new Label();
        private readonly Label stationSummary = new Label();
        private readonly TrendPanel trend = new TrendPanel();
        private readonly Timer timer = new Timer();

        public OperationsOverviewControl()
        {
            Dock = DockStyle.Fill;
            BackColor = IndustrialTheme.Canvas;
            Padding = new Padding(24);

            var title = new Label { Text = "Station Dashboard", Dock = DockStyle.Top, Height = 42, Font = new Font("Segoe UI Semibold", 22, FontStyle.Bold), ForeColor = IndustrialTheme.Ink };
            status.Dock = DockStyle.Top; status.Height = 30; status.Font = new Font("Segoe UI", 9.5f); status.ForeColor = IndustrialTheme.Muted;

            kpis.Dock = DockStyle.Top; kpis.Height = 142; kpis.ColumnCount = 5; kpis.RowCount = 1; kpis.Padding = new Padding(0, 10, 0, 10);
            for (int i = 0; i < 5; i++) kpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            body.Dock = DockStyle.Top; body.Height = 265; body.ColumnCount = 3; body.RowCount = 1; body.Padding = new Padding(0, 8, 0, 12);
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            body.Controls.Add(BuildAqiPanel(), 0, 0);
            body.Controls.Add(BuildTrendPanel(), 1, 0);
            body.Controls.Add(BuildStatusPanel(), 2, 0);

            var section = new Label { Text = "Analyzer Fleet Snapshot", Dock = DockStyle.Top, Height = 34, Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold), ForeColor = IndustrialTheme.Ink };
            grid.Dock = DockStyle.Fill; grid.ReadOnly = true; grid.AutoGenerateColumns = true; grid.MultiSelect = false; grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.RowHeadersVisible = false; grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; IndustrialTheme.PolishGrid(grid); grid.CellFormatting += Grid_CellFormatting;

            Controls.Add(grid); Controls.Add(section); Controls.Add(body); Controls.Add(kpis); Controls.Add(status); Controls.Add(title);
            timer.Interval = 5000; timer.Tick += (s, e) => RefreshView(); timer.Start(); RefreshView();
        }

        private Control BuildAqiPanel()
        {
            var p = CardPanel();
            p.Controls.Add(aqiCategory);
            p.Controls.Add(aqiValue);
            p.Controls.Add(Heading("Current AQI"));
            aqiValue.Dock = DockStyle.Fill; aqiValue.TextAlign = ContentAlignment.MiddleCenter; aqiValue.Font = new Font("Segoe UI Semibold", 44, FontStyle.Bold); aqiValue.ForeColor = IndustrialTheme.Blue;
            aqiCategory.Dock = DockStyle.Bottom; aqiCategory.Height = 38; aqiCategory.TextAlign = ContentAlignment.MiddleCenter; aqiCategory.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold); aqiCategory.ForeColor = IndustrialTheme.Muted;
            return p;
        }

        private Control BuildTrendPanel()
        {
            var p = CardPanel();
            trend.Dock = DockStyle.Fill; trend.BackColor = Color.White;
            p.Controls.Add(trend); p.Controls.Add(Heading("24-hour particulate trend  •  PM2.5 / PM10"));
            return p;
        }

        private Control BuildStatusPanel()
        {
            var p = CardPanel();
            var table = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, Padding = new Padding(12, 2, 12, 8) };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 34)); table.RowStyles.Add(new RowStyle(SizeType.Percent, 33)); table.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            ConfigureSummary(alarmSummary); ConfigureSummary(weatherSummary); ConfigureSummary(stationSummary);
            table.Controls.Add(alarmSummary, 0, 0); table.Controls.Add(weatherSummary, 0, 1); table.Controls.Add(stationSummary, 0, 2);
            p.Controls.Add(table); p.Controls.Add(Heading("Operational Summary")); return p;
        }

        private static void ConfigureSummary(Label l) { l.Dock = DockStyle.Fill; l.Font = new Font("Segoe UI", 9.5f); l.ForeColor = IndustrialTheme.Ink; l.TextAlign = ContentAlignment.MiddleLeft; l.AutoEllipsis = true; }
        private static Label Heading(string text) { return new Label { Text = text, Dock = DockStyle.Top, Height = 32, Padding = new Padding(12, 8, 0, 0), Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold), ForeColor = IndustrialTheme.Ink }; }
        private static Panel CardPanel() { return new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 14, 0), Padding = new Padding(1) }; }

        private void RefreshView()
        {
            var analyzers = storage.LoadAnalyzers() ?? new List<AnalyzerConfig>();
            var enabled = analyzers.Count(x => x != null && x.Enabled);
            var settings = storage.LoadSettings() ?? new AppSettings();
            var logs = db.GetLogs(DateTime.Now.AddHours(-24), DateTime.Now);
            var latest = logs.LastOrDefault();

            kpis.Controls.Clear();
            AddCard("Configured", analyzers.Count.ToString(), "Analyzer inventory", IndustrialTheme.Blue);
            AddCard("Enabled", enabled.ToString(), "Eligible for acquisition", IndustrialTheme.Green);
            AddCard("Disabled", (analyzers.Count - enabled).ToString(), "Polling blocked", IndustrialTheme.Amber);
            AddCard("Data source", settings.DataSourceMode ?? "—", "Acquisition mode", IndustrialTheme.Cyan);
            AddCard("Station uptime", FormatUptime(DateTime.Now - ProcessStarted), "Current application session", IndustrialTheme.Blue);

            status.Text = "Operational command view  •  Navigation is available from the permanent menu  •  Updated " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (latest != null)
            {
                aqiValue.Text = latest.AQI > 0 ? latest.AQI.ToString() : "--";
                aqiCategory.Text = string.IsNullOrWhiteSpace(latest.Status) ? "Latest stored measurement" : latest.Status;
                aqiValue.ForeColor = GetAqiColor(latest.AQI);
                weatherSummary.Text = "WEATHER\r\n" + Format(latest.Temperature, "0.0", "°C") + "   •   " + Format(latest.Humidity, "0", "% RH");
                alarmSummary.Text = "ALARM SUMMARY\r\n" + BuildAlarmSummary(latest, settings);
            }
            else
            {
                aqiValue.Text = "--"; aqiCategory.Text = "No stored live measurement"; aqiValue.ForeColor = IndustrialTheme.Muted;
                weatherSummary.Text = "WEATHER\r\nNo stored temperature / humidity data";
                alarmSummary.Text = "ALARM SUMMARY\r\nNo stored measurement available for threshold evaluation";
            }

            stationSummary.Text = "STATION\r\n" + enabled + "/" + analyzers.Count + " analyzers enabled   •   " + (settings.StationName ?? "AQMS Station");
            trend.SetData(logs);

            grid.DataSource = analyzers.Where(x => x != null).Select(x => new
            {
                x.Name,
                x.Manufacturer,
                x.Model,
                Driver = x.DriverId,
                Measurement = x.GasType,
                x.Unit,
                State = x.Enabled ? "Enabled" : "Disabled",
                Connection = x.ConnectionType,
                Endpoint = x.ConnectionType == "COM" ? x.ComPort : ((x.IpAddress ?? "") + (x.IpPort > 0 ? ":" + x.IpPort : ""))
            }).ToList();
        }

        private static string BuildAlarmSummary(SensorLog l, AppSettings s)
        {
            int critical = 0, warning = 0;
            Count(l.PM25, s.Pm25WarningThreshold, s.Pm25CriticalThreshold, ref warning, ref critical);
            Count(l.PM10, s.Pm10WarningThreshold, s.Pm10CriticalThreshold, ref warning, ref critical);
            Count(l.NO2, s.No2WarningThreshold, s.No2CriticalThreshold, ref warning, ref critical);
            Count(l.CO2, s.Co2WarningThreshold, s.Co2CriticalThreshold, ref warning, ref critical);
            if (critical > 0) return critical + " critical threshold condition(s) • " + warning + " warning";
            if (warning > 0) return warning + " warning threshold condition(s) • 0 critical";
            return "No threshold exceedance in latest stored measurement";
        }
        private static void Count(double value, double warningLimit, double criticalLimit, ref int warning, ref int critical)
        {
            if (double.IsNaN(value) || double.IsInfinity(value)) return;
            if (value >= criticalLimit) critical++; else if (value >= warningLimit) warning++;
        }
        private static string Format(double v, string f, string unit) { return double.IsNaN(v) || double.IsInfinity(v) ? "-- " + unit : v.ToString(f) + " " + unit; }
        private static string FormatUptime(TimeSpan t) { return t.TotalDays >= 1 ? ((int)t.TotalDays) + "d " + t.Hours + "h" : ((int)t.TotalHours) + "h " + t.Minutes + "m"; }
        private static Color GetAqiColor(double aqi) { if (aqi <= 50) return IndustrialTheme.Green; if (aqi <= 100) return IndustrialTheme.Amber; return Color.FromArgb(205, 67, 67); }

        private void AddCard(string heading, string value, string sub, Color accent)
        {
            var p = new AccentPanel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 14, 0), Padding = new Padding(16, 12, 12, 10), Accent = accent };
            var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, BackColor = Color.White, Margin = Padding.Empty, Padding = Padding.Empty };
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            table.Controls.Add(new Label { Text = heading, Dock = DockStyle.Fill, ForeColor = IndustrialTheme.Ink, Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            table.Controls.Add(new Label { Text = value, Dock = DockStyle.Fill, ForeColor = accent, Font = new Font("Segoe UI Semibold", value != null && value.Length > 11 ? 16f : 21f, FontStyle.Bold), AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            table.Controls.Add(new Label { Text = sub, Dock = DockStyle.Fill, ForeColor = IndustrialTheme.Muted, Font = new Font("Segoe UI", 9), AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft }, 0, 2);
            p.Controls.Add(table);
            kpis.Controls.Add(p);
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (grid.Columns[e.ColumnIndex].Name != "State" || e.Value == null) return;
            var state = Convert.ToString(e.Value);
            e.CellStyle.Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold);
            if (string.Equals(state, "Enabled", StringComparison.OrdinalIgnoreCase)) e.CellStyle.ForeColor = IndustrialTheme.Green;
            else e.CellStyle.ForeColor = IndustrialTheme.Amber;
        }
        protected override void Dispose(bool disposing) { if (disposing) timer.Dispose(); base.Dispose(disposing); }

        private sealed class TrendPanel : Panel
        {
            private List<SensorLog> data = new List<SensorLog>();
            public TrendPanel() { DoubleBuffered = true; }
            public void SetData(IEnumerable<SensorLog> logs) { data = (logs ?? Enumerable.Empty<SensorLog>()).OrderBy(x => x.Timestamp).ToList(); Invalidate(); }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e); var g = e.Graphics; g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var r = new Rectangle(42, 18, Math.Max(10, Width - 58), Math.Max(10, Height - 48));
                using (var pen = new Pen(Color.FromArgb(226, 232, 240))) for (int i = 0; i < 4; i++) { int y = r.Top + i * r.Height / 3; g.DrawLine(pen, r.Left, y, r.Right, y); }
                if (data.Count < 2) { using (var b = new SolidBrush(IndustrialTheme.Muted)) g.DrawString("No 24-hour stored trend data", new Font("Segoe UI", 9), b, r.Left + 12, r.Top + 28); return; }
                double max = data.SelectMany(x => new[] { Safe(x.PM25), Safe(x.PM10) }).Max(); if (max <= 0) max = 1;
                DrawSeries(g, r, data.Select(x => x.PM25).ToList(), max, IndustrialTheme.Blue);
                DrawSeries(g, r, data.Select(x => x.PM10).ToList(), max, IndustrialTheme.Cyan);
                using (var b = new SolidBrush(IndustrialTheme.Muted)) { g.DrawString("PM2.5", new Font("Segoe UI", 8), b, r.Left, r.Bottom + 7); g.DrawString("PM10", new Font("Segoe UI", 8), b, r.Left + 55, r.Bottom + 7); }
            }
            private static void DrawSeries(Graphics g, Rectangle r, IList<double> values, double max, Color c)
            {
                var pts = new List<PointF>(); for (int i = 0; i < values.Count; i++) { var v = values[i]; if (double.IsNaN(v) || double.IsInfinity(v)) continue; float x = r.Left + (values.Count == 1 ? 0 : (float)i / (values.Count - 1) * r.Width); float y = r.Bottom - (float)(Math.Max(0, v) / max * r.Height); pts.Add(new PointF(x, y)); }
                if (pts.Count > 1) using (var p = new Pen(c, 2f)) g.DrawLines(p, pts.ToArray());
            }
            private static double Safe(double v) { return double.IsNaN(v) || double.IsInfinity(v) ? 0 : Math.Max(0, v); }
        }
    }
}
