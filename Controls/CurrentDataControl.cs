using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS
{
    public partial class CurrentDataControl : UserControl
    {
        private readonly Timer _monitorTimer;
        private readonly IoTDataGenerator _dataGenerator;
        private readonly List<SensorSnapshot> _history;

        private const int MaxHistoryItems = 50;
        private const string DefaultLocation = "Station A";

        private bool _isMonitoring = true;
        private AirQualityData? _lastData;

        private int _lastAqi = 0;
        private string _lastAqiStatus = "Unknown";
        private string _lastDominantPollutant = "Unknown";

        public CurrentDataControl()
        {
            InitializeComponent();

            _dataGenerator = new IoTDataGenerator();
            _history = new List<SensorSnapshot>();

            _monitorTimer = new Timer
            {
                Interval = 2000
            };
            _monitorTimer.Tick += MonitorTimer_Tick;

            ConfigureGrid();
            Load += CurrentDataControl_Load;
        }


        private void CurrentDataControl_Load(object sender, EventArgs e)
        {
            InitializeDashboard();
            StartMonitoring();
        }

        private void InitializeDashboard()
        {
            if (_lastData == null)
            {
                _lastData = _dataGenerator.GetInitial(DefaultLocation);
            }

            RefreshDataFromGenerator();
        }

        private void StartMonitoring()
        {
            _isMonitoring = true;
            _monitorTimer.Start();
            btnStartStop.Text = "Stop";
            lblTimestamp.Text = $"Last update: {DateTime.Now:HH:mm:ss}";
        }

        private void StopMonitoring()
        {
            _isMonitoring = false;
            _monitorTimer.Stop();
            btnStartStop.Text = "Start";
        }

        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            if (_isMonitoring)
            {
                RefreshDataFromGenerator();
            }
        }

        private void RefreshDataFromGenerator()
        {
            AirQualityData airData = _dataGenerator.GenerateNext(DefaultLocation, _lastData);
            _lastData = airData;

            AqiCalculationResult calculation = AqiCalculator.Calculate(airData);

            _lastAqi = calculation.AQI;
            _lastAqiStatus = calculation.Category;
            _lastDominantPollutant = calculation.DominantPollutant;

            var snapshot = new SensorSnapshot
            {
                Timestamp = airData.Timestamp == default ? DateTime.Now : airData.Timestamp,
                PM25 = airData.PM25,
                PM10 = airData.PM10,
                CO2 = airData.CO2,
                NO2 = airData.NO2,
                Temperature = airData.Temperature,
                Humidity = airData.Humidity,
                AQI = calculation.AQI,
                AQIStatus = calculation.Category,
                DominantPollutant = calculation.DominantPollutant
            };

            _history.Add(snapshot);
            if (_history.Count > MaxHistoryItems)
                _history.RemoveAt(0);

            UpdateDashboard(snapshot);
            BindGrid();
            pnlChartContainer.Invalidate();
        }

        private void UpdateDashboard(SensorSnapshot snapshot)
        {
            lblAQIValue.Text = snapshot.AQI.ToString();
            lblStatus.Text = snapshot.AQIStatus;
            lblDominantPollutant.Text = $"Dominant: {snapshot.DominantPollutant}";

            lblPM25.Text = snapshot.PM25.ToString("0.0");
            lblPM10.Text = snapshot.PM10.ToString("0.0");
            lblCO2.Text = snapshot.CO2.ToString("0");
            lblNO2.Text = snapshot.NO2.ToString("0.0");
            lblTemp.Text = snapshot.Temperature.ToString("0.0");
            lblHum.Text = snapshot.Humidity.ToString("0.0");

            lblTimestamp.Text = $"Last update: {snapshot.Timestamp:HH:mm:ss}";

            UpdateAqiStatusAppearance(snapshot.AQI);
        }

        private void UpdateAqiStatusAppearance(int aqi)
        {
            Color statusColor;

            if (aqi <= 50)
                statusColor = Color.FromArgb(46, 204, 113);
            else if (aqi <= 100)
                statusColor = Color.FromArgb(241, 196, 15);
            else if (aqi <= 150)
                statusColor = Color.FromArgb(230, 126, 34);
            else if (aqi <= 200)
                statusColor = Color.FromArgb(231, 76, 60);
            else if (aqi <= 300)
                statusColor = Color.FromArgb(155, 89, 182);
            else
                statusColor = Color.FromArgb(127, 140, 141);

            pnlAQIIndicator.BackColor = statusColor;
            lblAQIValue.ForeColor = statusColor;
            lblStatus.ForeColor = statusColor;
        }

        private void ConfigureGrid()
        {
            dgvLogs.AutoGenerateColumns = false;
            dgvLogs.Columns.Clear();

            colTime.DataPropertyName = nameof(SensorSnapshot.Timestamp);
            colAQI.DataPropertyName = nameof(SensorSnapshot.AQI);
            colPM25.DataPropertyName = nameof(SensorSnapshot.PM25);
            colCO2.DataPropertyName = nameof(SensorSnapshot.CO2);
            colTemp.DataPropertyName = nameof(SensorSnapshot.Temperature);
            colHum.DataPropertyName = nameof(SensorSnapshot.Humidity);

            colTime.DefaultCellStyle.Format = "HH:mm:ss";
            colPM25.DefaultCellStyle.Format = "0.0";
            colCO2.DefaultCellStyle.Format = "0";
            colTemp.DefaultCellStyle.Format = "0.0";
            colHum.DefaultCellStyle.Format = "0.0";

            dgvLogs.Columns.AddRange(new DataGridViewColumn[]
            {
                colTime,
                colAQI,
                colPM25,
                colCO2,
                colTemp,
                colHum
            });
        }

        private void BindGrid()
        {
            dgvLogs.DataSource = null;
            dgvLogs.DataSource = _history.ToList();
        }

        private void pnlChartContainer_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(pnlChartContainer.BackColor);

            if (_history.Count < 2)
                return;

            Rectangle rect = pnlChartContainer.ClientRectangle;
            rect.Inflate(-20, -20);

            if (rect.Width <= 0 || rect.Height <= 0)
                return;

            int maxAqi = Math.Max(100, _history.Max(x => x.AQI));
            int minAqi = 0;

            float stepX = (float)rect.Width / (_history.Count - 1);
            float scaleY = (float)rect.Height / Math.Max(1, maxAqi - minAqi);

            var points = _history
                .Select((item, index) =>
                {
                    float x = rect.Left + index * stepX;
                    float y = rect.Bottom - ((item.AQI - minAqi) * scaleY);
                    return new PointF(x, y);
                })
                .ToArray();

            using var axisPen = new Pen(Color.FromArgb(90, 90, 90), 1);
            using var linePen = new Pen(Color.FromArgb(0, 122, 204), 2.5f);
            using var pointBrush = new SolidBrush(Color.FromArgb(0, 122, 204));

            g.DrawRectangle(axisPen, rect);

            if (points.Length >= 2)
                g.DrawLines(linePen, points);

            foreach (var p in points)
            {
                g.FillEllipse(pointBrush, p.X - 3, p.Y - 3, 6, 6);
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_isMonitoring)
                StopMonitoring();
            else
                StartMonitoring();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_history.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "CSV File (*.csv)|*.csv",
                Title = "Export AQMS Data",
                FileName = $"AQMS_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            var lines = new List<string>
            {
                "Timestamp,AQI,PM2.5,CO2,Temperature,Humidity,DominantPollutant,Status"
            };

            lines.AddRange(_history.Select(x =>
                $"{x.Timestamp:yyyy-MM-dd HH:mm:ss},{x.AQI},{x.PM25:0.0},{x.CO2:0},{x.Temperature:0.0},{x.Humidity:0.0},{x.DominantPollutant},{x.AQIStatus}"
            ));

            File.WriteAllLines(sfd.FileName, lines);
            MessageBox.Show("Export completed successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
// فراخوانی سرویس لاگر در زمان دریافت داده جدید
var newLog = new SensorLog
{
    Timestamp = DateTime.Now,
    AnalyzerId = "Analyzer_01", // می‌توانید شناسه‌ یا پورت واقعی را قرار دهید
    PM25 = snapshot.PM25,
    CO2 = snapshot.CO2,
    AQI = calculatedAqi, // با نام فیلد AQI که قبلا اصلاح کردیم
    Status = currentStatus
};
DbService.SaveLog(newLog);

    public class SensorSnapshot
    {
        public DateTime Timestamp { get; set; }
        public double PM25 { get; set; }
        public double PM10 { get; set; }
        public double CO2 { get; set; }
        public double NO2 { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int AQI { get; set; }
        public string AQIStatus { get; set; } = "Unknown";
        public string DominantPollutant { get; set; } = "Unknown";
    }
}
