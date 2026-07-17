using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AshkanAQMS
{
    public partial class CurrentDataControl : UserControl
    {
        private const int MaxHistoryPoints = 50;

        private readonly List<SensorSnapshot> _history = new List<SensorSnapshot>();
        private readonly Random _random = new Random();

        private Timer _updateTimer;
        private bool _isMonitoring;

        public CurrentDataControl()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            SetupTimer();
            SetupGrid();
            ApplyColors();
            LoadInitialData();
        }

        private void SetupTimer()
        {
            _updateTimer = new Timer(components);
            _updateTimer.Interval = 2000;
            _updateTimer.Tick += OnTimerTick;
        }

        private void SetupGrid()
        {
            dgvLogs.AutoGenerateColumns = false;
            dgvLogs.Rows.Clear();
        }

        private void ApplyColors()
        {
            btnStartStop.BackColor = Color.FromArgb(39, 174, 96);
            btnStartStop.ForeColor = Color.White;

            btnExport.BackColor = Color.FromArgb(52, 152, 219);
            btnExport.ForeColor = Color.White;
        }

        private void LoadInitialData()
        {
            SensorSnapshot snapshot = GenerateSnapshot();
            AddSnapshot(snapshot);
            UpdateDisplay(snapshot);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            SensorSnapshot snapshot = GenerateSnapshot();
            AddSnapshot(snapshot);
            UpdateDisplay(snapshot);
        }

        private SensorSnapshot GenerateSnapshot()
        {
            double pm25 = NextDouble(8, 95);
            double pm10 = pm25 + NextDouble(5, 45);
            double co2 = NextDouble(420, 1200);
            double no2 = NextDouble(8, 110);
            double temp = NextDouble(18, 34);
            double hum = NextDouble(25, 80);

            int aqi = CalculateAqi(pm25, pm10);

            return new SensorSnapshot
            {
                Timestamp = DateTime.Now,
                AQI = aqi,
                PM25 = Math.Round(pm25, 1),
                PM10 = Math.Round(pm10, 1),
                CO2 = Math.Round(co2, 0),
                NO2 = Math.Round(no2, 1),
                Temperature = Math.Round(temp, 1),
                Humidity = Math.Round(hum, 1)
            };
        }

        private void AddSnapshot(SensorSnapshot snapshot)
        {
            _history.Add(snapshot);

            if (_history.Count > MaxHistoryPoints)
            {
                _history.RemoveAt(0);
            }

            dgvLogs.Rows.Insert(
                0,
                snapshot.Timestamp.ToString("HH:mm:ss"),
                snapshot.AQI.ToString(CultureInfo.InvariantCulture),
                snapshot.PM25.ToString("0.0", CultureInfo.InvariantCulture),
                snapshot.CO2.ToString("0", CultureInfo.InvariantCulture),
                snapshot.Temperature.ToString("0.0", CultureInfo.InvariantCulture),
                snapshot.Humidity.ToString("0.0", CultureInfo.InvariantCulture));

            while (dgvLogs.Rows.Count > MaxHistoryPoints)
            {
                dgvLogs.Rows.RemoveAt(dgvLogs.Rows.Count - 1);
            }

            lblChartTitle.Text = "AQI Trend (" + _history.Count.ToString(CultureInfo.InvariantCulture) + ")";
            pnlChartContainer.Invalidate();
        }

        private void UpdateDisplay(SensorSnapshot snapshot)
        {
            lblTimestamp.Text = "Last Update: " + snapshot.Timestamp.ToString("yyyy/MM/dd HH:mm:ss");
            lblAQIValue.Text = snapshot.AQI.ToString(CultureInfo.InvariantCulture);
            lblStatus.Text = GetAqiStatus(snapshot.AQI);

            lblPM25.Text = snapshot.PM25.ToString("0.0", CultureInfo.InvariantCulture) + " µg/m³";
            lblPM10.Text = snapshot.PM10.ToString("0.0", CultureInfo.InvariantCulture) + " µg/m³";
            lblCO2.Text = snapshot.CO2.ToString("0", CultureInfo.InvariantCulture) + " ppm";
            lblNO2.Text = snapshot.NO2.ToString("0.0", CultureInfo.InvariantCulture) + " ppb";
            lblTemp.Text = snapshot.Temperature.ToString("0.0", CultureInfo.InvariantCulture) + " °C";
            lblHum.Text = snapshot.Humidity.ToString("0.0", CultureInfo.InvariantCulture) + " %";

            Color aqiColor = GetAqiColor(snapshot.AQI);
            pnlAQIIndicator.BackColor = aqiColor;
            lblAQIValue.ForeColor = aqiColor;
            lblStatus.ForeColor = aqiColor;
        }

        private int CalculateAqi(double pm25, double pm10)
        {
            int pm25Index = (int)Math.Round(pm25 * 2.5);
            int pm10Index = (int)Math.Round(pm10 * 1.2);
            int aqi = Math.Max(pm25Index, pm10Index);

            if (aqi < 0)
            {
                return 0;
            }

            if (aqi > 300)
            {
                return 300;
            }

            return aqi;
        }

        private string GetAqiStatus(int aqi)
        {
            if (aqi <= 50)
            {
                return "Good";
            }

            if (aqi <= 100)
            {
                return "Moderate";
            }

            if (aqi <= 150)
            {
                return "Unhealthy for Sensitive";
            }

            if (aqi <= 200)
            {
                return "Unhealthy";
            }

            if (aqi <= 300)
            {
                return "Very Unhealthy";
            }

            return "Hazardous";
        }

        private Color GetAqiColor(int aqi)
        {
            if (aqi <= 50)
            {
                return Color.FromArgb(39, 174, 96);
            }

            if (aqi <= 100)
            {
                return Color.FromArgb(241, 196, 15);
            }

            if (aqi <= 150)
            {
                return Color.FromArgb(230, 126, 34);
            }

            if (aqi <= 200)
            {
                return Color.FromArgb(231, 76, 60);
            }

            if (aqi <= 300)
            {
                return Color.FromArgb(142, 68, 173);
            }

            return Color.FromArgb(127, 140, 141);
        }

        private double NextDouble(double min, double max)
        {
            return min + (_random.NextDouble() * (max - min));
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            _isMonitoring = !_isMonitoring;

            if (_isMonitoring)
            {
                _updateTimer.Start();
                btnStartStop.Text = "Stop Monitoring";
                btnStartStop.BackColor = Color.FromArgb(231, 76, 60);
            }
            else
            {
                _updateTimer.Stop();
                btnStartStop.Text = "Start Monitoring";
                btnStartStop.BackColor = Color.FromArgb(39, 174, 96);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_history.Count == 0)
            {
                MessageBox.Show(
                    "No data available to export.",
                    "Export CSV",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV files (*.csv)|*.csv";
                saveDialog.FileName = "aqi_log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    StringBuilder csv = new StringBuilder();
                    csv.AppendLine("Timestamp,AQI,PM2.5,PM10,CO2,NO2,Temperature,Humidity");

                    foreach (SensorSnapshot item in _history)
                    {
                        csv.Append(item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        csv.Append(",");
                        csv.Append(item.AQI.ToString(CultureInfo.InvariantCulture));
                        csv.Append(",");
                        csv.Append(item.PM25.ToString("0.0", CultureInfo.InvariantCulture));
                        csv.Append(",");
                        csv.Append(item.PM10.ToString("0.0", CultureInfo.InvariantCulture));
                        csv.Append(",");
                        csv.Append(item.CO2.ToString("0", CultureInfo.InvariantCulture));
                        csv.Append(",");
                        csv.Append(item.NO2.ToString("0.0", CultureInfo.InvariantCulture));
                        csv.Append(",");
                        csv.Append(item.Temperature.ToString("0.0", CultureInfo.InvariantCulture));
                        csv.Append(",");
                        csv.AppendLine(item.Humidity.ToString("0.0", CultureInfo.InvariantCulture));
                    }

                    File.WriteAllText(saveDialog.FileName, csv.ToString(), Encoding.UTF8);

                    MessageBox.Show(
                        "Data exported successfully.",
                        "Export CSV",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Export failed: " + ex.Message,
                        "Export CSV",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void pnlChartContainer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            Rectangle bounds = pnlChartContainer.ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            int leftPadding = 40;
            int rightPadding = 20;
            int topPadding = 20;
            int bottomPadding = 30;

            Rectangle plotArea = new Rectangle(
                leftPadding,
                topPadding,
                bounds.Width - leftPadding - rightPadding,
                bounds.Height - topPadding - bottomPadding);

            using (Pen gridPen = new Pen(Color.FromArgb(235, 238, 242), 1f))
            {
                for (int i = 0; i <= 5; i++)
                {
                    float y = plotArea.Top + (plotArea.Height * i / 5f);
                    g.DrawLine(gridPen, plotArea.Left, y, plotArea.Right, y);
                }

                for (int i = 0; i <= 5; i++)
                {
                    float x = plotArea.Left + (plotArea.Width * i / 5f);
                    g.DrawLine(gridPen, x, plotArea.Top, x, plotArea.Bottom);
                }
            }

            using (Pen axisPen = new Pen(Color.FromArgb(210, 214, 220), 1.2f))
            {
                g.DrawLine(axisPen, plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);
                g.DrawLine(axisPen, plotArea.Left, plotArea.Top, plotArea.Left, plotArea.Bottom);
            }

            using (Font axisFont = new Font("Segoe UI", 8f))
            using (Brush axisBrush = new SolidBrush(Color.Gray))
            {
                for (int i = 0; i <= 5; i++)
                {
                    int yValue = 300 - (i * 60);
                    float y = plotArea.Top + (plotArea.Height * i / 5f) - 8f;
                    g.DrawString(yValue.ToString(CultureInfo.InvariantCulture), axisFont, axisBrush, 5f, y);
                }
            }

            if (_history.Count < 2 || plotArea.Width <= 0 || plotArea.Height <= 0)
            {
                return;
            }

            PointF[] points = BuildChartPoints(plotArea);
            if (points.Length < 2)
            {
                return;
            }

            using (GraphicsPath areaPath = BuildAreaPath(points, plotArea))
            using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                new Point(plotArea.Left, plotArea.Top),
                new Point(plotArea.Left, plotArea.Bottom),
                Color.FromArgb(90, 52, 152, 219),
                Color.FromArgb(10, 52, 152, 219)))
            {
                g.FillPath(fillBrush, areaPath);
            }

            using (Pen curvePen = new Pen(Color.FromArgb(41, 128, 185), 3f))
            {
                curvePen.LineJoin = LineJoin.Round;
                curvePen.StartCap = LineCap.Round;
                curvePen.EndCap = LineCap.Round;
                g.DrawCurve(curvePen, points, 0.45f);
            }

            using (Brush pointBrush = new SolidBrush(Color.FromArgb(41, 128, 185)))
            using (Pen pointBorder = new Pen(Color.White, 2f))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    float x = points[i].X - 4f;
                    float y = points[i].Y - 4f;
                    g.FillEllipse(pointBrush, x, y, 8f, 8f);
                    g.DrawEllipse(pointBorder, x, y, 8f, 8f);
                }
            }
        }

        private PointF[] BuildChartPoints(Rectangle plotArea)
        {
            PointF[] points = new PointF[_history.Count];

            float xStep = _history.Count > 1
                ? (float)plotArea.Width / (float)(_history.Count - 1)
                : plotArea.Width;

            for (int i = 0; i < _history.Count; i++)
            {
                float normalized = _history[i].AQI / 300f;
                if (normalized < 0f)
                {
                    normalized = 0f;
                }

                if (normalized > 1f)
                {
                    normalized = 1f;
                }

                float x = plotArea.Left + (i * xStep);
                float y = plotArea.Bottom - (normalized * plotArea.Height);

                if (i == 0)
                {
                    x = plotArea.Left + 2f;
                }
                else if (i == _history.Count - 1)
                {
                    x = plotArea.Right - 2f;
                }

                points[i] = new PointF(x, y);
            }

            return points;
        }

        private GraphicsPath BuildAreaPath(PointF[] points, Rectangle plotArea)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddCurve(points, 0.45f);
            path.AddLine(points[points.Length - 1].X, plotArea.Bottom, points[0].X, plotArea.Bottom);
            path.CloseFigure();
            return path;
        }

        private sealed class SensorSnapshot
        {
            public DateTime Timestamp { get; set; }
            public int AQI { get; set; }
            public double PM25 { get; set; }
            public double PM10 { get; set; }
            public double CO2 { get; set; }
            public double NO2 { get; set; }
            public double Temperature { get; set; }
            public double Humidity { get; set; }
        }
    }
}
