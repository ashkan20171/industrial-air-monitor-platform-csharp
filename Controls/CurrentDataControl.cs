using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS
{
    public partial class CurrentDataControl : UserControl
    {
        private Timer _monitorTimer;
        private Random _random;
        private readonly List<AirQualityData> _history = new List<AirQualityData>();
        private readonly AiService _aiService = new AiService();
        private bool _isMonitoring = true;
        private const int MaxHistoryCount = 50;

        // متغیرهای ذخیره وضعیت AI
        private double _lastPredictedAqi = 0;
        private string _lastAiRecommendation = "سیستم در حال جمع‌آوری داده...";
        private string _lastAnomalyMessage = string.Empty;
        private bool _isAnomalyDetected = false;

        public CurrentDataControl()
        {
            InitializeComponent();
            _random = new Random();
            InitializeMonitoringTimer();
        }

        private void InitializeMonitoringTimer()
        {
            _monitorTimer = new Timer();
            _monitorTimer.Interval = 2000;
            _monitorTimer.Tick += MonitorTimer_Tick;
        }

        private void CurrentDataControl_Load(object sender, EventArgs e)
        {
            if (btnStartStop != null)
                btnStartStop.Text = "Stop";

            // تولید داده اولیه و شروع
            GenerateAndDisplaySnapshot();
            StartMonitoring();
        }

        public void StartMonitoring()
        {
            _isMonitoring = true;
            _monitorTimer?.Start();
            if (btnStartStop != null)
                btnStartStop.Text = "Stop";
        }

        public void StopMonitoring()
        {
            _isMonitoring = false;
            _monitorTimer?.Stop();
            if (btnStartStop != null)
                btnStartStop.Text = "Start";
        }

        public void ToggleMonitoring()
        {
            if (_isMonitoring)
                StopMonitoring();
            else
                StartMonitoring();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            ToggleMonitoring();
        }

        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            GenerateAndDisplaySnapshot();
        }

        private void GenerateAndDisplaySnapshot()
        {
            var data = GenerateData();

            // محاسبه AQI با ساختار استاندارد جدید
            data.CalculateAQI();

            // ذخیره در سابقه داده‌ها (جدیدترین در ابتدای لیست)
            _history.Insert(0, data);
            if (_history.Count > MaxHistoryCount)
            {
                _history.RemoveAt(_history.Count - 1);
            }

            // پردازش و تحلیل با AiService (سازگار با .NET 4.8 بدون نیاز به TakeLast)
            RunAiAnalysis(data);

            // به‌روزرسانی UI
            UpdateDashboard(data);
            RefreshLogsGrid();
            pnlChartContainer?.Invalidate();
        }

        private AirQualityData GenerateData()
        {
            return new AirQualityData
            {
                Timestamp = DateTime.Now,
                PM25 = Math.Round(5.0 + (_random.NextDouble() * 75.0), 1),
                PM10 = Math.Round(10.0 + (_random.NextDouble() * 110.0), 1),
                CO2 = Math.Round(400.0 + (_random.NextDouble() * 1400.0), 0),
                NO2 = Math.Round(5.0 + (_random.NextDouble() * 195.0), 1),
                Temperature = Math.Round(15.0 + (_random.NextDouble() * 20.0), 1),
                Humidity = Math.Round(20.0 + (_random.NextDouble() * 60.0), 1),
                Location = "Station A"
            };
        }

        private void RunAiAnalysis(AirQualityData currentData)
        {
            // ترتیب زمانی صعودی برای تحلیل سری زمانی
            var chronologicalList = _history.AsEnumerable().Reverse().ToList();

            if (chronologicalList.Count >= 5)
            {
                // ۱. تشخیص آنومالی سنسور روی شاخص PM2.5
                var pm25History = chronologicalList.Select(x => x.PM25);
                _isAnomalyDetected = _aiService.IsAnomalyDetected(pm25History, currentData.PM25, out _lastAnomalyMessage);

                // ۲. پیش‌بینی روند AQI بعدی
                var aqiHistory = chronologicalList.Select(x => (double)x.AQI);
                _lastPredictedAqi = _aiService.PredictNextValue(aqiHistory, alpha: 0.35);

                // ۳. صدور توصیه‌های هوشمند برای اپراتور
                _lastAiRecommendation = _aiService.GenerateActionRecommendation(currentData, _lastPredictedAqi);
            }
            else
            {
                _lastPredictedAqi = currentData.AQI;
                _lastAiRecommendation = "سیستم در حال ثبت و تثبیت الگوهای داده است...";
                _isAnomalyDetected = false;
                _lastAnomalyMessage = string.Empty;
            }
        }

        private void UpdateDashboard(AirQualityData data)
        {
            if (lblTimestamp != null)
                lblTimestamp.Text = "Last Update: " + data.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");

            if (lblAQIValue != null)
                lblAQIValue.Text = data.AQI.ToString();

            if (lblStatus != null)
            {
                lblStatus.Text = data.AQICategory;
                lblStatus.ForeColor = GetStatusTextColor(data.AQI);
            }

            if (lblPM25Value != null)
                lblPM25Value.Text = data.PM25.ToString("F1") + " µg/m³";

            if (lblPM10Value != null)
                lblPM10Value.Text = data.PM10.ToString("F1") + " µg/m³";

            if (lblCO2Value != null)
                lblCO2Value.Text = data.CO2.ToString("F0") + " ppm";

            if (lblNO2Value != null)
                lblNO2Value.Text = data.NO2.ToString("F1") + " ppb";

            if (lblTempValue != null)
                lblTempValue.Text = data.Temperature.ToString("F1") + " °C";

            if (lblHumValue != null)
                lblHumValue.Text = data.Humidity.ToString("F1") + " %";

            if (lblDominantPollutant != null)
            {
                string infoText = "Dominant: " + data.DominantPollutant;
                if (_lastPredictedAqi > 0)
                {
                    infoText += string.Format(" | Predicted AQI: {0:F0}", _lastPredictedAqi);
                }
                lblDominantPollutant.Text = infoText;
            }

            if (pnlAQIIndicator != null)
            {
                if (_isAnomalyDetected)
                {
                    pnlAQIIndicator.BackColor = Color.Crimson; // هشدار خطای سنسور
                }
                else
                {
                    pnlAQIIndicator.BackColor = GetAqiColor(data.AQI);
                }
            }
        }

        private Color GetAqiColor(int aqi)
        {
            if (aqi <= 50) return Color.FromArgb(76, 175, 80);     // Good (Green)
            if (aqi <= 100) return Color.FromArgb(255, 235, 59);   // Moderate (Yellow)
            if (aqi <= 150) return Color.FromArgb(255, 152, 0);    // Sensitive (Orange)
            if (aqi <= 200) return Color.FromArgb(244, 67, 54);    // Unhealthy (Red)
            if (aqi <= 300) return Color.FromArgb(156, 39, 176);   // Very Unhealthy (Purple)
            return Color.FromArgb(136, 14, 79);                    // Hazardous (Maroon)
        }

        private Color GetStatusTextColor(int aqi)
        {
            if (aqi <= 50) return Color.FromArgb(46, 125, 50);
            if (aqi <= 100) return Color.FromArgb(245, 127, 23);
            if (aqi <= 150) return Color.FromArgb(230, 81, 0);
            if (aqi <= 200) return Color.FromArgb(198, 40, 40);
            if (aqi <= 300) return Color.FromArgb(106, 27, 154);
            return Color.FromArgb(74, 20, 140);
        }

        private void RefreshLogsGrid()
        {
            if (dgvLogs == null) return;

            dgvLogs.Rows.Clear();
            foreach (var item in _history)
            {
                dgvLogs.Rows.Add(
                    item.Timestamp.ToString("HH:mm:ss"),
                    item.AQI,
                    item.PM25.ToString("F1"),
                    item.CO2.ToString("F0"),
                    item.Temperature.ToString("F1"),
                    item.Humidity.ToString("F1")
                );
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = string.Format("aqms_report_{0:yyyyMMdd_HHmmss}.csv", DateTime.Now);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportHistoryToCsv(sfd.FileName);
                }
            }
        }

        public void ExportHistoryToCsv(string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    sw.WriteLine("Timestamp,AQI,Category,PM2.5,PM10,CO2,NO2,Temperature,Humidity,DominantPollutant");
                    foreach (var item in _history)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                            Csv(item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")),
                            item.AQI,
                            Csv(item.AQICategory),
                            item.PM25,
                            item.PM10,
                            item.CO2,
                            item.NO2,
                            item.Temperature,
                            item.Humidity,
                            Csv(item.DominantPollutant)));
                    }
                }
                MessageBox.Show("گزارش داده‌ها با موفقیت ذخیره شد.", "صادرات فایل", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ذخیره فایل: " + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string Csv(string text)
        {
            if (string.IsNullOrEmpty(text)) return "\"\"";
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

        private void pnlChartContainer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int width = pnlChartContainer.Width;
            int height = pnlChartContainer.Height;

            int marginLeft = 50;
            int marginRight = 20;
            int marginTop = 20;
            int marginBottom = 35;

            int plotWidth = width - marginLeft - marginRight;
            int plotHeight = height - marginTop - marginBottom;

            if (plotWidth <= 10 || plotHeight <= 10)
                return;

            // پس‌زمینه نمودار
            using (SolidBrush bgBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(bgBrush, marginLeft, marginTop, plotWidth, plotHeight);
            }

            // خطوط گرید و برچسب‌های محور عمودی (AQI: 0 تا 500)
            using (Pen gridPen = new Pen(Color.FromArgb(230, 230, 230), 1))
            using (Font font = new Font("Segoe UI", 8))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(120, 120, 120)))
            {
                for (int aqiVal = 0; aqiVal <= 500; aqiVal += 100)
                {
                    float y = marginTop + plotHeight - (aqiVal / 500f * plotHeight);
                    g.DrawLine(gridPen, marginLeft, y, marginLeft + plotWidth, y);
                    g.DrawString(aqiVal.ToString(), font, textBrush, 10, y - 6);
                }
            }

            if (_history.Count == 0) return;

            // رسم داده‌های AQI
            var chronological = _history.AsEnumerable().Reverse().ToList();
            PointF[] points = new PointF[chronological.Count];

            float stepX = (chronological.Count > 1) ? (float)plotWidth / (chronological.Count - 1) : 0;

            for (int i = 0; i < chronological.Count; i++)
            {
                float x = marginLeft + (i * stepX);
                float normalizedAqi = Math.Min(500, Math.Max(0, chronological[i].AQI));
                float y = marginTop + plotHeight - (normalizedAqi / 500f * plotHeight);
                points[i] = new PointF(x, y);
            }

            if (points.Length > 1)
            {
                using (Pen linePen = new Pen(Color.FromArgb(33, 150, 243), 2.5f))
                {
                    g.DrawLines(linePen, points);
                }
            }

            // رسم نقاط داده و نقطه پیش‌بینی
            using (SolidBrush dotBrush = new SolidBrush(Color.FromArgb(30, 136, 229)))
            {
                foreach (var pt in points)
                {
                    g.FillEllipse(dotBrush, pt.X - 3.5f, pt.Y - 3.5f, 7, 7);
                }
            }

            // نمایش هشدار یا توصیه هوش مصنوعی در زیر نمودار در صورت بروز ناهنجاری
            if (_isAnomalyDetected)
            {
                using (Font alertFont = new Font("Segoe UI", 8.5f, FontStyle.Bold))
                using (SolidBrush alertBrush = new SolidBrush(Color.Crimson))
                {
                    g.DrawString("⚠ " + _lastAnomalyMessage, alertFont, alertBrush, marginLeft + 5, marginTop + 5);
                }
            }
        }
    }
}
