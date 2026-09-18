using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    public partial class CurrentDataControl : UserControl
    {
        private readonly List<SensorSnapshot> _history = new List<SensorSnapshot>();
        private readonly StorageService _storageService = new StorageService();
        private readonly IoTDataGenerator _generator = new IoTDataGenerator();
        private readonly AlarmEngine _alarmEngine = new AlarmEngine();
        private readonly AiService _aiService = new AiService();
        private Timer _monitorTimer;
        private bool _isRunning;
        private AirQualityData _lastData;
        private AppSettings _settings;
        private int _alarmCooldown;
        private readonly AuditLogger _auditLogger = new AuditLogger();
        private readonly HealthMonitor _healthMonitor = new HealthMonitor();
        private readonly AdvancedAnalyticsService _advancedAnalytics = new AdvancedAnalyticsService();
        private readonly List<AirQualityData> _dataHistory = new List<AirQualityData>();
        private Label _analyticsLabel;
        private Label _qualityLabel;
        private DateTime _monitoringStarted = DateTime.Now;
        private Label _aiInsightLabel;
        private Label _healthLabel;
        private const int MaxHistoryItems = 240;

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
            public string Category { get; set; }
            public string DominantPollutant { get; set; }
        }

        public CurrentDataControl()
        {
            InitializeComponent();
            _settings = _storageService.LoadSettings() ?? new AppSettings();
            ApplyProfessionalTheme();
            CreateInsightPanel();
            CreateExecutiveTools();
            _alarmEngine.AlarmRaised += AlarmEngine_AlarmRaised;
            Disposed += (s, e) => { if (_monitorTimer != null) { _monitorTimer.Stop(); _monitorTimer.Dispose(); } };
            InitializeTimer();
            GenerateAndDisplaySnapshot();
        }

        private void ApplyProfessionalTheme()
        {
            BackColor = IndustrialTheme.BackgroundDark;
            if (pnlHeader != null) pnlHeader.BackColor = IndustrialTheme.SurfaceCard;
            if (pnlAQICard != null) pnlAQICard.BackColor = IndustrialTheme.SurfaceCard;
            if (pnlDetailsCard != null) pnlDetailsCard.BackColor = IndustrialTheme.SurfaceCard;
            if (pnlChartCard != null) pnlChartCard.BackColor = IndustrialTheme.SurfaceCard;
            if (pnlGridCard != null) pnlGridCard.BackColor = IndustrialTheme.SurfaceCard;
            foreach (var l in new[] { lblAppName, lblAQITitle, lblPM25Header, lblPM10Header, lblCO2Header, lblNO2Header, lblTempHeader, lblHumHeader, lblChartTitle, lblLogTitle })
                if (l != null) l.ForeColor = IndustrialTheme.TextPrimary;
            if (lblTimestamp != null) lblTimestamp.ForeColor = IndustrialTheme.TextSecondary;
            if (dgvLogs != null)
            {
                dgvLogs.BackgroundColor = IndustrialTheme.SurfaceCard;
                dgvLogs.BorderStyle = BorderStyle.None;
                dgvLogs.EnableHeadersVisualStyles = false;
                dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 55, 70);
                dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvLogs.DefaultCellStyle.BackColor = IndustrialTheme.SurfaceCard;
                dgvLogs.DefaultCellStyle.ForeColor = IndustrialTheme.TextPrimary;
                dgvLogs.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 73, 94);
                dgvLogs.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvLogs.GridColor = IndustrialTheme.BorderColor;
            }
        }

        private void CreateInsightPanel()
        {
            _aiInsightLabel = new Label { AutoSize = false, Dock = DockStyle.Bottom, Height = 42, Padding = new Padding(12, 5, 12, 5), Font = new Font("Segoe UI", 9F, FontStyle.Regular), ForeColor = IndustrialTheme.TextSecondary, BackColor = IndustrialTheme.SurfaceCard, Text = "AI Insights • Initializing local analytics..." };
            if (pnlChartCard != null) pnlChartCard.Controls.Add(_aiInsightLabel);
            _aiInsightLabel.Visible = _settings.EnableAiInsights;
            _healthLabel = new Label { AutoSize = false, Dock = DockStyle.Bottom, Height = 22, Padding = new Padding(12, 2, 12, 2), Font = new Font("Segoe UI", 8F), ForeColor = IndustrialTheme.TextSecondary, BackColor = IndustrialTheme.SurfaceCard, TextAlign = ContentAlignment.MiddleRight };
            if (pnlHeader != null) pnlHeader.Controls.Add(_healthLabel);
            _healthLabel.Visible = _settings.EnableHealthMonitoring;
        }

        private void CreateExecutiveTools()
        {
            _analyticsLabel = new Label { AutoSize = false, Dock = DockStyle.Bottom, Height = 28, Padding = new Padding(12, 4, 12, 4), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = IndustrialTheme.TextPrimary, BackColor = IndustrialTheme.SurfaceCard, Text = "AI Risk: --  •  Forecast: --" };
            if (pnlChartCard != null) pnlChartCard.Controls.Add(_analyticsLabel);
            _analyticsLabel.Visible = _settings.EnableAdvancedAnalytics;
            _qualityLabel = new Label { AutoSize = false, Dock = DockStyle.Bottom, Height = 24, Padding = new Padding(12, 3, 12, 3), Font = new Font("Segoe UI", 8F), ForeColor = IndustrialTheme.TextSecondary, BackColor = IndustrialTheme.SurfaceCard, Text = "Data Quality: --" };
            if (pnlGridCard != null) pnlGridCard.Controls.Add(_qualityLabel);
            _qualityLabel.Visible = true;
            var assistantButton = new Button { Text = "AI Assistant", FlatStyle = FlatStyle.Flat, Width = 105, Height = 28, Dock = DockStyle.Right, BackColor = Color.FromArgb(70, 82, 110), ForeColor = Color.White, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Margin = new Padding(4) };
            assistantButton.FlatAppearance.BorderSize = 0;
            assistantButton.Enabled = _settings.EnableAiInsights || _settings.EnableAdvancedAnalytics;
            assistantButton.Click += ShowAiAssistant;
            if (pnlHeader != null) pnlHeader.Controls.Add(assistantButton);
            var reportButton = new Button { Text = "Executive Snapshot", FlatStyle = FlatStyle.Flat, Width = 145, Height = 28, Dock = DockStyle.Right, BackColor = Color.FromArgb(46, 125, 110), ForeColor = Color.White, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Margin = new Padding(4) };
            reportButton.FlatAppearance.BorderSize = 0;
            reportButton.Enabled = _settings.EnableExecutiveReports;
            reportButton.Click += ExportExecutiveSnapshot;
            if (pnlHeader != null) pnlHeader.Controls.Add(reportButton);
        }

        private void ShowAiAssistant(object sender, EventArgs e)
        {
            if (_lastData == null) return;
            var analytics = _advancedAnalytics.Analyze(_lastData, _dataHistory.Skip(1));
            var quality = DataQualityService.Assess(_lastData);
            string text = string.Format("AQMS Local AI Assistant\r\n\r\nStation: {0}\r\nCurrent AQI: {1} ({2})\r\nDominant pollutant: {3}\r\n\r\nForecasts\r\nPM2.5: {4:0.0} µg/m³\r\nPM10:  {5:0.0} µg/m³\r\nCO₂:   {6:0} ppm\r\nNO₂:   {7:0.0} ppb\r\n\r\nRisk assessment: {8}% ({9})\r\nData quality: {10}% ({11})\r\n\r\nRecommendation\r\n{12}\r\n\r\nNote: analytics are local, deterministic decision-support and require validation before regulatory use.", _settings.StationName, _lastData.AQI, _lastData.AQICategory, _lastData.DominantPollutant, analytics.Pm25Forecast, analytics.Pm10Forecast, analytics.Co2Forecast, analytics.No2Forecast, analytics.OverallRiskScore, analytics.OverallTrend, quality.Score, quality.Status, analytics.Recommendation);
            using (var f = new Form { Text = "AI Environmental Assistant", Width = 620, Height = 520, StartPosition = FormStartPosition.CenterParent, BackColor = IndustrialTheme.BackgroundDark, ForeColor = IndustrialTheme.TextPrimary, MinimizeBox = false, MaximizeBox = false })
            {
                var box = new TextBox { Multiline = true, ReadOnly = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical, BorderStyle = BorderStyle.None, BackColor = IndustrialTheme.SurfaceCard, ForeColor = IndustrialTheme.TextPrimary, Font = new Font("Segoe UI", 10F), Padding = new Padding(16), Text = text };
                f.Controls.Add(box); f.ShowDialog(this);
            }
        }

        private void ExportExecutiveSnapshot(object sender, EventArgs e)
        {
            if (_lastData == null) return;
            using (var sfd = new SaveFileDialog { Filter = "HTML Executive Report (*.html)|*.html", FileName = "AQMS_Executive_Snapshot_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".html" })
            {
                if (sfd.ShowDialog(this) != DialogResult.OK) return;
                var analytics = _advancedAnalytics.Analyze(_lastData, _dataHistory);
                var quality = DataQualityService.Assess(_lastData);
                var health = _healthMonitor.Capture();
                if (ExecutiveReportService.ExportHtml(sfd.FileName, _lastData, analytics, quality, health, _settings))
                    MessageBox.Show("Executive snapshot created successfully.", "AQMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitializeTimer()
        {
            _monitorTimer = new Timer { Interval = Math.Max(1000, _settings.PollingIntervalSeconds * 1000) };
            _monitorTimer.Tick += MonitorTimer_Tick;
            _isRunning = true;
            _monitorTimer.Start();
            if (btnStartStop != null) { btnStartStop.Text = "Pause Monitoring"; btnStartStop.BackColor = Color.FromArgb(192,57,43); }
        }

        private void MonitorTimer_Tick(object sender, EventArgs e) => GenerateAndDisplaySnapshot();

        private void GenerateAndDisplaySnapshot()
        {
            var previous = _lastData ?? _generator.GetInitial(_settings.StationName);
            var data = _generator.GenerateNext(_settings.StationName, previous);
            data.CalculateAQI();
            _lastData = data;

            _aiService.UpdateParameters(_settings.EmaAlpha, _settings.AnomalyZScoreThreshold);
            var recent = _history.Select(x => x.PM25).Take(Math.Max(3, _settings.HistoryWindowSize)).ToList();
            var ai = _settings.EnableAiInsights ? _aiService.Analyze(data.PM25, recent, data.IsValid) : new AiAnalysisResult { Ema = data.PM25, Forecast = data.PM25, Confidence = 0, Trend = "Disabled", Insight = "Local AI insights are disabled." };
            var quality = DataQualityService.Assess(data);
            var anomaly = ai.IsAnomaly;
            _dataHistory.Insert(0, data);
            if (_dataHistory.Count > MaxHistoryItems) _dataHistory.RemoveAt(_dataHistory.Count - 1);
            var advanced = _settings.EnableAdvancedAnalytics ? _advancedAnalytics.Analyze(data, _dataHistory.Skip(1)) : new AnalyticsSummary { OverallTrend = "Disabled", Recommendation = "Advanced analytics disabled." };

            var snapshot = new SensorSnapshot { Timestamp=data.Timestamp, PM25=data.PM25, PM10=data.PM10, CO2=data.CO2, NO2=data.NO2, Temperature=data.Temperature, Humidity=data.Humidity, AQI=data.AQI, Category=data.AQICategory, DominantPollutant=data.DominantPollutant };
            _history.Insert(0, snapshot);
            if (_history.Count > MaxHistoryItems) _history.RemoveAt(_history.Count - 1);

            UpdateDashboard(data, ai.Ema, anomaly, ai);
            if (_aiInsightLabel != null) { _aiInsightLabel.Visible = _settings.EnableAiInsights; _aiInsightLabel.Text = string.Format("AI • Trend: {0} • Forecast: {1:0.0} µg/m³ • Confidence: {2:0}% • Data quality: {3}%  |  {4}", ai.Trend, ai.Forecast, ai.Confidence, quality.Score, ai.Insight); }
            if (_healthLabel != null && _settings.EnableHealthMonitoring) { var health = _healthMonitor.Capture(); var uptime = DateTime.Now - _monitoringStarted; _healthLabel.Text = string.Format("System {0}  •  RAM {1:0} MB  •  CPU {2:0.0}%  •  Uptime {3:00}:{4:00}:{5:00}", health.Status, health.WorkingSetMb, health.CpuTimeSeconds, (int)uptime.TotalHours, uptime.Minutes, uptime.Seconds); }
            if (_analyticsLabel != null && _settings.EnableAdvancedAnalytics) _analyticsLabel.Text = string.Format("AI Risk: {0}% ({1})  •  Forecast PM2.5: {2:0.0} µg/m³  •  Recommendation: {3}", advanced.OverallRiskScore, advanced.OverallTrend, advanced.Pm25Forecast, advanced.Recommendation);
            if (_qualityLabel != null) _qualityLabel.Text = string.Format("Data Quality: {0}% • {1} • {2}", quality.Score, quality.Status, quality.Message);
            AddSnapshotToGrid(snapshot);

            try
            {
                if (_settings.EnableAutoArchive)
                    new DbService().SaveLog(new SensorLog { Timestamp=data.Timestamp, AnalyzerId="SIM-001", PM25=data.PM25, PM10=data.PM10, CO2=data.CO2, NO2=data.NO2, Temperature=data.Temperature, Humidity=data.Humidity, AQI=data.AQI, Status=data.AQICategory });
            }
            catch { /* monitoring must continue if archival storage is unavailable */ }

            _alarmCooldown++;
            int every = Math.Max(1, _settings.AlarmEvaluationEveryNMeasurements);
            if (_alarmCooldown >= every) { _alarmEngine.Evaluate(data, _settings); if (_settings.EnableAnomalyAlarms && anomaly) _alarmEngine.RaiseAnomaly("PM2.5", data.PM25, "AI anomaly detected in recent PM2.5 pattern"); _alarmCooldown = 0; }
            if (pnlChartContainer != null) pnlChartContainer.Invalidate();
        }

        private void UpdateDashboard(AirQualityData data, double ema, bool anomaly, AiAnalysisResult ai)
        {
            if (lblTimestamp != null) lblTimestamp.Text = $"Last update  •  {data.Timestamp:yyyy-MM-dd HH:mm:ss}  •  {_settings.StationName}";
            if (lblAQIValue != null) lblAQIValue.Text = data.AQI.ToString(CultureInfo.InvariantCulture);
            if (lblPM25Value != null) lblPM25Value.Text = $"{data.PM25:0.0} µg/m³";
            if (lblPM10Value != null) lblPM10Value.Text = $"{data.PM10:0.0} µg/m³";
            if (lblCO2Value != null) lblCO2Value.Text = $"{data.CO2:0} ppm";
            if (lblNO2Value != null) lblNO2Value.Text = $"{data.NO2:0.0} ppb";
            if (lblTempValue != null) lblTempValue.Text = $"{data.Temperature:0.0} °C";
            if (lblHumValue != null) lblHumValue.Text = $"{data.Humidity:0.0} %";
            var c = GetAqiColor(data.AQI);
            if (lblStatus != null) lblStatus.Text = $"{data.AQICategory}   •   AI {ai.Trend}   •   forecast {ai.Forecast:0.0}" + (anomaly ? "   •   ANOMALY" : "");
            if (lblStatus != null) lblStatus.ForeColor = anomaly ? Color.OrangeRed : c;
            if (lblAQIValue != null) lblAQIValue.ForeColor = c;
            if (pnlAQIIndicator != null) pnlAQIIndicator.BackColor = c;
            if (lblDominantPollutant != null) lblDominantPollutant.Text = "Dominant pollutant: " + (string.IsNullOrWhiteSpace(data.DominantPollutant) ? "—" : data.DominantPollutant);
        }

        private void AlarmEngine_AlarmRaised(object sender, AlarmEvent alarm)
        {
            if (!IsHandleCreated) return;
            BeginInvoke(new Action(() =>
            {
                if (lblStatus != null && alarm.Severity == AlarmSeverity.Critical) lblStatus.Text = "CRITICAL  •  " + alarm.Parameter + "  •  " + alarm.Value.ToString("0.0", CultureInfo.InvariantCulture) + " " + alarm.Unit;
                if (_settings.EnableSoundAlerts) System.Media.SystemSounds.Exclamation.Play();
                if (_settings.EnableAuditLogging) _auditLogger.Write("ALARM", alarm.Severity + " | " + alarm.Parameter + " | " + alarm.Value.ToString(CultureInfo.InvariantCulture) + " | " + alarm.Message);
            }));
        }

        private void AddSnapshotToGrid(SensorSnapshot item)
        {
            if (dgvLogs == null) return;
            dgvLogs.SuspendLayout();
            try
            {
                dgvLogs.Rows.Insert(0, item.Timestamp.ToString("HH:mm:ss"), item.AQI, item.PM25.ToString("0.0"), item.CO2.ToString("0"), item.Temperature.ToString("0.0"), item.Humidity.ToString("0.0"));
                while (dgvLogs.Rows.Count > 30) dgvLogs.Rows.RemoveAt(dgvLogs.Rows.Count - 1);
            }
            finally { dgvLogs.ResumeLayout(); }
        }

        private void pnlChartContainer_Paint(object sender, PaintEventArgs e)
        {
            var g=e.Graphics; g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias; var r=pnlChartContainer.ClientRectangle;
            g.Clear(IndustrialTheme.SurfaceCard); if(_history.Count<2) return;
            var d=_history.Take(40).Reverse().ToList(); float left=40, right=15, top=18, bottom=25; float w=r.Width-left-right, h=r.Height-top-bottom; double max=Math.Max(100,d.Max(x=>x.PM25)*1.2);
            using(var grid=new Pen(IndustrialTheme.BorderColor)) for(int i=0;i<=4;i++){float y=top+h*i/4f;g.DrawLine(grid,left,y,r.Width-right,y);}
            var pts=new PointF[d.Count]; for(int i=0;i<d.Count;i++) pts[i]=new PointF(left+w*i/(d.Count-1), (float)(top+h-(d[i].PM25/max)*h));
            using(var pen=new Pen(Color.FromArgb(52,152,219),2.5f)) g.DrawLines(pen,pts);
            using(var font=new Font("Segoe UI",8)) using(var brush=new SolidBrush(IndustrialTheme.TextSecondary)) g.DrawString("PM2.5 trend",font,brush,8,5);
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_isRunning) { _monitorTimer.Stop(); _isRunning=false; btnStartStop.Text="Resume Monitoring"; btnStartStop.BackColor=Color.FromArgb(39,174,96); }
            else { _monitorTimer.Interval=Math.Max(1000,_settings.PollingIntervalSeconds*1000); _monitorTimer.Start(); _isRunning=true; btnStartStop.Text="Pause Monitoring"; btnStartStop.BackColor=Color.FromArgb(192,57,43); }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if(_history.Count==0) return;
            using(var sfd=new SaveFileDialog{Filter="CSV Files (*.csv)|*.csv",FileName=$"AQMS_Measurements_{DateTime.Now:yyyyMMdd_HHmmss}.csv"})
            if(sfd.ShowDialog(this)==DialogResult.OK)
            {
                var headers=new List<string>{"Timestamp","AQI","PM2.5","PM10","CO2","NO2","Temperature","Humidity","Category"};
                var rows=_history.Select(x=>new List<string>{x.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),x.AQI.ToString(),x.PM25.ToString("0.0"),x.PM10.ToString("0.0"),x.CO2.ToString("0"),x.NO2.ToString("0.0"),x.Temperature.ToString("0.0"),x.Humidity.ToString("0.0"),x.Category}).ToList();
                if(_storageService.ExportToCsv(headers,rows,sfd.FileName)) MessageBox.Show("Measurement export completed.","AQMS",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        public void StopMonitoring() { if(_monitorTimer!=null) _monitorTimer.Stop(); _isRunning=false; _auditLogger.Write("MONITORING", "Stopped"); }
        private static Color GetAqiColor(int aqi) { if(aqi<=50)return IndustrialTheme.StatusGood; if(aqi<=100)return IndustrialTheme.StatusModerate; if(aqi<=150)return IndustrialTheme.StatusUnhealthySensitive; if(aqi<=200)return IndustrialTheme.StatusUnhealthy; return IndustrialTheme.StatusHazardous; }
    }
}
