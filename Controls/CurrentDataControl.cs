using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AshkanAQMS
{
    public partial class CurrentDataControl : UserControl
    {
        private readonly Timer _monitorTimer;
        private readonly Random _random;
        private readonly List<SensorSnapshot> _history;
        private const int MaxHistoryItems = 50;

        private bool _isMonitoring = true;

        public CurrentDataControl()
        {
            InitializeComponent();

            _random = new Random();
            _history = new List<SensorSnapshot>();

            _monitorTimer = new Timer();
            _monitorTimer.Interval = 2000;
            _monitorTimer.Tick += MonitorTimer_Tick;

            Load += CurrentDataControl_Load;
        }

        private void CurrentDataControl_Load(object sender, EventArgs e)
        {
            btnStartStop.Text = "Stop";
            GenerateAndDisplaySnapshot();
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            _isMonitoring = true;

            if (!_monitorTimer.Enabled)
                _monitorTimer.Start();

            btnStartStop.Text = "Stop";
        }

        private void StopMonitoring()
        {
            _isMonitoring = false;

            if (_monitorTimer.Enabled)
                _monitorTimer.Stop();

            btnStartStop.Text = "Start";
        }

        private void ToggleMonitoring()
        {
            if (_isMonitoring)
                StopMonitoring();
            else
                StartMonitoring();
        }

        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            GenerateAndDisplaySnapshot();
        }

        private void GenerateAndDisplaySnapshot()
        {
            SensorSnapshot snapshot = GenerateSnapshot();

            _history.Insert(0, snapshot);

            if (_history.Count > MaxHistoryItems)
                _history.RemoveAt(_history.Count - 1);

            UpdateDashboard(snapshot);
            RefreshLogsGrid();
            pnlChartContainer.Invalidate();
        }

        private SensorSnapshot GenerateSnapshot()
        {
            double pm25 = Math.Round(5 + _random.NextDouble() * 75, 1);
            double pm10 = Math.Round(10 + _random.NextDouble() * 110, 1);
            double co2 = Math.Round(400 + _random.NextDouble() * 1400, 0);
            double no2 = Math.Round(5 + _random.NextDouble() * 195, 1);
            double temperature = Math.Round(15 + _random.NextDouble() * 20, 1);
            double humidity = Math.Round(20 + _random.NextDouble() * 60, 1);

            int aqi = CalculateAqi(pm25, pm10, co2, no2, out string dominantPollutant);
            string status = GetAqiStatus(aqi);

            return new SensorSnapshot
            {
                Timestamp = DateTime.Now,
                PM25 = pm25,
                PM10 = pm10,
                CO2 = co2,
                NO2 = no2,
                Temperature = temperature,
                Humidity = humidity,
                AQI = aqi,
                Status = status,
                DominantPollutant = dominantPollutant
            };
        }

        private int CalculateAqi(double pm25, double pm10, double co2, double no2, out string dominantPollutant)
        {
            double pm25Index = Math.Min(500, pm25 * 4.0);
            double pm10Index = Math.Min(500, pm10 * 2.0);
            double co2Index = Math.Min(500, Math.Max(0, (co2 - 400) / 4.0));
            double no2Index = Math.Min(500, no2 * 2.5);

            var scores = new Dictionary<string, double>
            {
                { "PM2.5", pm25Index },
                { "PM10", pm10Index },
                { "CO2", co2Index },
                { "NO2", no2Index }
            };

            var maxEntry = scores.OrderByDescending(x => x.Value).First();
            dominantPollutant = maxEntry.Key;

            return (int)Math.Round(maxEntry.Value);
        }

        private string GetAqiStatus(int aqi)
        {
            if (aqi <= 50) return "Good";
            if (aqi <= 100) return "Moderate";
            if (aqi <= 150) return "Unhealthy for Sensitive Groups";
            if (aqi <= 200) return "Unhealthy";
            if (aqi <= 300) return "Very Unhealthy";
            return "Hazardous";
        }

        private Color GetAqiColor(int aqi)
        {
            if (aqi <= 50) return Color.FromArgb(76, 175, 80);
            if (aqi <= 100) return Color.FromArgb(255, 193, 7);
            if (aqi <= 150) return Color.FromArgb(255, 152, 0);
            if (aqi <= 200) return Color.FromArgb(244, 67, 54);
            if (aqi <= 300) return Color.FromArgb(156, 39, 176);
            return Color.FromArgb(97, 97, 97);
        }

        private Color GetStatusTextColor(int aqi)
        {
            if (aqi <= 50) return Color.FromArgb(46, 125, 50);
            if (aqi <= 100) return Color.FromArgb(245, 124, 0);
            if (aqi <= 150) return Color.FromArgb(230, 81, 0);
            if (aqi <= 200) return Color.FromArgb(198, 40, 40);
            if (aqi <= 300) return Color.FromArgb(123, 31, 162);
            return Color.FromArgb(66, 66, 66);
        }

        private void UpdateDashboard(SensorSnapshot snapshot)
        {
            lblTimestamp.Text = $"Last update: {snapshot.Timestamp:yyyy-MM-dd HH:mm:ss}";
            lblAQIValue.Text = snapshot.AQI.ToString();

            lblStatus.Text = snapshot.Status;
            lblStatus.ForeColor = GetStatusTextColor(snapshot.AQI);

            lblPM25.Text = $"{snapshot.PM25:0.0} µg/m³";
            lblPM10.Text = $"{snapshot.PM10:0.0} µg/m³";
            lblCO2.Text = $"{snapshot.CO2:0} ppm";
            lblNO2.Text = $"{snapshot.NO2:0.0} ppb";
            lblTemp.Text = $"{snapshot.Temperature:0.0} °C";
            lblHum.Text = $"{snapshot.Humidity:0.0} %";
            lblDominantPollutant.Text = $"Dominant: {snapshot.DominantPollutant}";

            pnlAQIIndicator.BackColor = GetAqiColor(snapshot.AQI);
        }

        private void RefreshLogsGrid()
        {
            dgvLogs.SuspendLayout();
            try
            {
                dgvLogs.Rows.Clear();

                foreach (var item in _history.OrderByDescending(x => x.Timestamp))
                {
                    dgvLogs.Rows.Add(
                        item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                        item.AQI.ToString(),
                        item.PM25.ToString("0.0"),
                        item.CO2.ToString("0"),
                        item.Temperature.ToString("0.0"),
                        item.Humidity.ToString("0.0")
                    );
                }
            }
            finally
            {
                dgvLogs.ResumeLayout();
            }
        }

        private void ExportHistoryToCsv(string filePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Timestamp,AQI,Status,PM2.5,PM10,CO2,NO2,Temperature,Humidity,DominantPollutant");

            foreach (var item in _history.OrderBy(x => x.Timestamp))
            {
                sb.AppendLine(string.Join(",",
                    Csv(item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")),
                    Csv(item.AQI.ToString()),
                    Csv(item.Status),
                    Csv(item.PM25.ToString("0.0")),
                    Csv(item.PM10.ToString("0.0")),
                    Csv(item.CO2.ToString("0")),
                    Csv(item.NO2.ToString("0.0")),
                    Csv(item.Temperature.ToString("0.0")),
                    Csv(item.Humidity.ToString("0.0")),
                    Csv(item.DominantPollutant)
                ));
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private string Csv(string value)
        {
            if (value == null)
                return "\"\"";

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_history.Count == 0)
            {
                MessageBox.Show("No data available to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Export AQMS Data";
                dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dialog.FileName = $"aqms_report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportHistoryToCsv(dialog.FileName);
                        MessageBox.Show("CSV exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Export failed:\n{ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            ToggleMonitoring();
        }

        private void pnlChartContainer_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = pnlChartContainer.ClientRectangle;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(pnlChartContainer.BackColor);

            if (_history.Count == 0)
            {
                using (var brush = new SolidBrush(Color.Gray))
                using (var font = new Font("Segoe UI", 10, FontStyle.Italic))
                {
                    var text = "No chart data yet";
                    var size = g.MeasureString(text, font);
                    g.DrawString(
                        text,
                        font,
                        brush,
                        (rect.Width - size.Width) / 2,
                        (rect.Height - size.Height) / 2
                    );
                }
                return;
            }

            const int paddingLeft = 50;
            const int paddingRight = 20;
            const int paddingTop = 20;
            const int paddingBottom = 35;

            var plotRect = new Rectangle(
                paddingLeft,
                paddingTop,
                Math.Max(1, rect.Width - paddingLeft - paddingRight),
                Math.Max(1, rect.Height - paddingTop - paddingBottom)
            );

            using (var axisPen = new Pen(Color.FromArgb(180, 180, 180), 1))
            using (var gridPen = new Pen(Color.FromArgb(230, 230, 230), 1))
            using (var linePen = new Pen(Color.FromArgb(33, 150, 243), 2))
            using (var pointBrush = new SolidBrush(Color.FromArgb(33, 150, 243)))
            using (var labelBrush = new SolidBrush(Color.DimGray))
            using (var font = new Font("Segoe UI", 8f))
            {
                int[] yMarks = { 0, 100, 200, 300, 400, 500 };

                foreach (int mark in yMarks)
                {
                    float y = plotRect.Bottom - (mark / 500f) * plotRect.Height;

                    g.DrawLine(gridPen, plotRect.Left, y, plotRect.Right, y);
                    g.DrawLine(axisPen, plotRect.Left - 4, y, plotRect.Left, y);

                    string label = mark.ToString();
                    var size = g.MeasureString(label, font);
                    g.DrawString(label, font, labelBrush, plotRect.Left - size.Width - 6, y - size.Height / 2);
                }

                g.DrawLine(axisPen, plotRect.Left, plotRect.Top, plotRect.Left, plotRect.Bottom);
                g.DrawLine(axisPen, plotRect.Left, plotRect.Bottom, plotRect.Right, plotRect.Bottom);

                var ordered = _history.OrderBy(x => x.Timestamp).ToList();

                if (ordered.Count == 1)
                {
                    float singleX = plotRect.Left + plotRect.Width / 2f;
                    float singleY = plotRect.Bottom - (Math.Min(500, ordered[0].AQI) / 500f) * plotRect.Height;
                    g.FillEllipse(pointBrush, singleX - 4, singleY - 4, 8, 8);
                    return;
                }

                PointF? previousPoint = null;

                for (int i = 0; i < ordered.Count; i++)
                {
                    float x = plotRect.Left + (i * 1f / (ordered.Count - 1)) * plotRect.Width;
                    float normalizedAqi = Math.Min(500, Math.Max(0, ordered[i].AQI)) / 500f;
                    float y = plotRect.Bottom - normalizedAqi * plotRect.Height;

                    PointF currentPoint = new PointF(x, y);

                    if (previousPoint.HasValue)
                        g.DrawLine(linePen, previousPoint.Value, currentPoint);

                    g.FillEllipse(pointBrush, x - 4, y - 4, 8, 8);
                    previousPoint = currentPoint;
                }

                var latest = ordered.Last();
                string latestText = $"Latest AQI: {latest.AQI}";
                g.DrawString(latestText, new Font("Segoe UI", 9f, FontStyle.Bold), labelBrush, plotRect.Right - 120, plotRect.Top - 2);
            }
        }

        private class SensorSnapshot
        {
            public DateTime Timestamp { get; set; }
            public double PM25 { get; set; }
            public double PM10 { get; set; }
            public double CO2 { get; set; }
            public double NO2 { get; set; }
            public double Temperature { get; set; }
            public double Humidity { get; set; }
            public int AQI { get; set; }
            public string Status { get; set; }
            public string DominantPollutant { get; set; }
        }
    }
}
