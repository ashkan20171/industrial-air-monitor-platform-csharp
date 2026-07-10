using AshkanAQMS.Models;
using AshkanAQMS.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq; // Added for Max()
using System.Windows.Forms;

namespace AshkanAQMS
{
    public partial class CurrentDataControl : UserControl
    {
        private readonly IoTDataGenerator _sensorService;
        private readonly List<AirQualityData> _history;
        private AirQualityData _currentData;
        private Timer _updateTimer;
        private bool _isMonitoring = false;

        public CurrentDataControl()
        {
            InitializeComponent();
            _sensorService = new IoTDataGenerator();
            _history = new List<AirQualityData>();
            _currentData = _sensorService.GetInitial("Tehran Central");

            SetupTimer();
            ApplyColors();
            UpdateUI();
        }

        private void SetupTimer()
        {
            _updateTimer = new Timer();
            _updateTimer.Interval = 2000;
            _updateTimer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _currentData = _sensorService.GenerateNext("Tehran Central", _currentData);
            _history.Add(_currentData);

            if (_history.Count > 50) _history.RemoveAt(0);

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_currentData == null) return;

            int aqi = _currentData.CalculateAQI();
            lblAQIValue.Text = aqi.ToString();
            lblStatus.Text = _currentData.GetAQICategory(aqi);
            pnlAQIIndicator.BackColor = GetAQIColor(aqi);

            lblPM25.Text = string.Format("{0:F1} µg/m³", _currentData.PM25);
            lblPM10.Text = string.Format("{0:F1} µg/m³", _currentData.PM10);
            lblCO2.Text = string.Format("{0:F0} ppm", _currentData.CO2);
            lblNO2.Text = string.Format("{0:F0} ppb", _currentData.NO2);
            lblTemp.Text = string.Format("{0:F1} °C", _currentData.Temperature);
            lblHum.Text = string.Format("{0:F0} %", _currentData.Humidity);
            lblTimestamp.Text = "Last Update: " + _currentData.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");

            dgvLogs.Rows.Add(_currentData.Timestamp.ToString("HH:mm:ss"), aqi, _currentData.PM25.ToString("F1"), _currentData.CO2.ToString("F0"), _currentData.Temperature.ToString("F1"), _currentData.Humidity.ToString("F0"));

            while (dgvLogs.Rows.Count > 20) dgvLogs.Rows.RemoveAt(0);
            pnlChartContainer.Invalidate();
        }

        private Color GetAQIColor(int aqi)
        {
            if (aqi <= 50) return Color.FromArgb(39, 174, 96);
            if (aqi <= 100) return Color.FromArgb(244, 237, 35);
            if (aqi <= 150) return Color.FromArgb(245, 176, 65);
            if (aqi <= 200) return Color.FromArgb(231, 76, 60);
            return Color.FromArgb(155, 89, 182);
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
            if (_history.Count == 0) { MessageBox.Show("No data."); return; }
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV (*.csv)|*.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine("Time,AQI,PM2.5,CO2,Temp,Humidity");
                    foreach (var d in _history) sb.AppendLine($"{d.Timestamp:HH:mm:ss},{d.CalculateAQI()},{d.PM25:F1},{d.CO2:F0},{d.Temperature:F1},{d.Humidity:F0}");
                    File.WriteAllText(sfd.FileName, sb.ToString());
                }
            }
        }

        private void pnlChartContainer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (_history.Count < 2) return;
            double maxAQI = _history.Max(d => d.CalculateAQI());
            DrawLine(g, pnlChartContainer.ClientRectangle, _history, d => d.CalculateAQI(), Color.DodgerBlue, maxAQI);
        }

        private void DrawLine(Graphics g, Rectangle rect, List<AirQualityData> data, Func<AirQualityData, double> selector, Color color, double max)
        {
            float scaleY = (float)rect.Height / (float)(max + 10);
            float scaleX = (float)rect.Width / (data.Count - 1);
            var points = data.Select((d, i) => new PointF(rect.Left + i * scaleX, rect.Top + rect.Height - (float)(selector(d) * scaleY))).ToArray();
            if (points.Length > 1) g.DrawLines(new Pen(color, 2), points);
        }

        private void ApplyColors()
        {
            pnlHeader.BackColor = Color.White;
            pnlAQICard.BackColor = Color.White;
            pnlDetailsCard.BackColor = Color.White;
            pnlChartCard.BackColor = Color.White;
            pnlGridCard.BackColor = Color.White;
        }
    }
}
