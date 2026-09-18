using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    public class FeatureCenterControl : UserControl
    {
        private readonly StorageService _storage = new StorageService();
        private AppSettings _settings;
        private readonly Dictionary<string, CheckBox> _checks = new Dictionary<string, CheckBox>();
        private Label _status;
        private FlowLayoutPanel _flow;

        public event EventHandler SettingsChanged;

        public FeatureCenterControl()
        {
            _settings = _storage.LoadSettings() ?? new AppSettings();
            BuildUi();
        }

        private void BuildUi()
        {
            Dock = DockStyle.Fill;
            BackColor = IndustrialTheme.BackgroundDark;
            Padding = new Padding(24);

            var title = new Label { Dock = DockStyle.Top, Height = 38, Text = "Feature Center", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = IndustrialTheme.TextPrimary };
            var subtitle = new Label { Dock = DockStyle.Top, Height = 42, Text = "Enable, disable and configure the platform capabilities directly from the dashboard.", Font = new Font("Segoe UI", 10F), ForeColor = IndustrialTheme.TextSecondary };
            Controls.Add(subtitle); Controls.Add(title);

            _status = new Label { Dock = DockStyle.Bottom, Height = 34, TextAlign = ContentAlignment.MiddleLeft, ForeColor = IndustrialTheme.TextSecondary, Text = "Changes are saved automatically." };
            Controls.Add(_status);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
            var reset = CreateButton("Reset defaults", Color.FromArgb(52, 73, 94));
            var enableAll = CreateButton("Enable all", Color.FromArgb(39, 174, 96));
            var disableOptional = CreateButton("Safe demo mode", Color.FromArgb(127, 140, 141));
            reset.Click += (s, e) => { _settings = new AppSettings(); SaveAndRefresh("Default feature configuration restored."); };
            enableAll.Click += (s, e) => { SetAll(true); SaveAndRefresh("All available capabilities enabled."); };
            disableOptional.Click += (s, e) => { SetAll(false); _settings.EnableAutoArchive = true; _settings.EnableAuditLogging = true; SaveAndRefresh("Demo mode enabled: monitoring remains active with non-essential features disabled."); };
            var portfolio = CreateButton("Portfolio mode", Color.FromArgb(52, 152, 219));
            portfolio.Click += (s, e) => { _settings.PresentationMode = true; _settings.EnableAiInsights = true; _settings.EnableAdvancedAnalytics = true; _settings.EnableAnomalyAlarms = true; _settings.EnableHealthMonitoring = true; _settings.EnableExecutiveReports = true; SaveAndRefresh("Portfolio mode enabled: key capabilities are active for demonstration."); };
            buttons.Controls.Add(enableAll); buttons.Controls.Add(disableOptional); buttons.Controls.Add(portfolio); buttons.Controls.Add(reset); Controls.Add(buttons);

            _flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(0, 12, 0, 12), WrapContents = true };
            Controls.Add(_flow);
            BuildFeature("AI Insights", "EMA, trend, forecast and anomaly analytics", "EnableAiInsights", () => _settings.EnableAiInsights);
            BuildFeature("Advanced Analytics", "Multi-pollutant risk, forecasts and recommendations", "EnableAdvancedAnalytics", () => _settings.EnableAdvancedAnalytics);
            BuildFeature("Anomaly Alarms", "Raise operational alarms when AI detects unusual patterns", "EnableAnomalyAlarms", () => _settings.EnableAnomalyAlarms);
            BuildFeature("Sound Alerts", "Audible critical/warning notifications", "EnableSoundAlerts", () => _settings.EnableSoundAlerts);
            BuildFeature("Auto Archive", "Persist measurements to the local historian", "EnableAutoArchive", () => _settings.EnableAutoArchive);
            BuildFeature("Audit Logging", "Operational and alarm audit trail", "EnableAuditLogging", () => _settings.EnableAuditLogging);
            BuildFeature("System Health", "RAM, CPU and monitoring uptime telemetry", "EnableHealthMonitoring", () => _settings.EnableHealthMonitoring);
            BuildFeature("Executive Reports", "One-click HTML management snapshot", "EnableExecutiveReports", () => _settings.EnableExecutiveReports);
            BuildFeature("Presentation Mode", "Clean portfolio/demo presentation mode", "PresentationMode", () => _settings.PresentationMode);
        }

        private void BuildFeature(string title, string description, string key, Func<bool> getter)
        {
            var card = new Panel { Width = 330, Height = 118, Margin = new Padding(0, 0, 14, 14), BackColor = IndustrialTheme.SurfaceCard, Padding = new Padding(14) };
            var name = new Label { AutoSize = true, Location = new Point(14, 12), Text = title, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = IndustrialTheme.TextPrimary };
            var desc = new Label { AutoSize = false, Location = new Point(14, 40), Size = new Size(295, 42), Text = description, Font = new Font("Segoe UI", 8.5F), ForeColor = IndustrialTheme.TextSecondary };
            var check = new CheckBox { AutoSize = true, Location = new Point(14, 87), Text = "Enabled", ForeColor = Color.White, Checked = getter(), Tag = key };
            check.CheckedChanged += Feature_CheckedChanged;
            card.Controls.Add(name); card.Controls.Add(desc); card.Controls.Add(check); _flow.Controls.Add(card); _checks[key] = check;
        }

        private void Feature_CheckedChanged(object sender, EventArgs e)
        {
            var check = sender as CheckBox; if (check == null) return;
            ApplyValue((string)check.Tag, check.Checked);
            _storage.SaveSettings(_settings);
            _status.Text = string.Format("Saved • {0}: {1}", check.Text, check.Checked ? "Enabled" : "Disabled");
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ApplyValue(string key, bool value)
        {
            switch (key)
            {
                case "EnableAiInsights": _settings.EnableAiInsights = value; break;
                case "EnableAdvancedAnalytics": _settings.EnableAdvancedAnalytics = value; break;
                case "EnableAnomalyAlarms": _settings.EnableAnomalyAlarms = value; break;
                case "EnableSoundAlerts": _settings.EnableSoundAlerts = value; break;
                case "EnableAutoArchive": _settings.EnableAutoArchive = value; break;
                case "EnableAuditLogging": _settings.EnableAuditLogging = value; break;
                case "EnableHealthMonitoring": _settings.EnableHealthMonitoring = value; break;
                case "EnableExecutiveReports": _settings.EnableExecutiveReports = value; break;
                case "PresentationMode": _settings.PresentationMode = value; break;
            }
        }

        private void SetAll(bool value)
        {
            foreach (var pair in _checks) pair.Value.Checked = value;
        }

        private void SaveAndRefresh(string message)
        {
            _storage.SaveSettings(_settings);
            foreach (var pair in _checks) pair.Value.Checked = GetValue(pair.Key);
            _status.Text = message;
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool GetValue(string key)
        {
            switch (key)
            {
                case "EnableAiInsights": return _settings.EnableAiInsights;
                case "EnableAdvancedAnalytics": return _settings.EnableAdvancedAnalytics;
                case "EnableAnomalyAlarms": return _settings.EnableAnomalyAlarms;
                case "EnableSoundAlerts": return _settings.EnableSoundAlerts;
                case "EnableAutoArchive": return _settings.EnableAutoArchive;
                case "EnableAuditLogging": return _settings.EnableAuditLogging;
                case "EnableHealthMonitoring": return _settings.EnableHealthMonitoring;
                case "EnableExecutiveReports": return _settings.EnableExecutiveReports;
                case "PresentationMode": return _settings.PresentationMode;
                default: return false;
            }
        }

        private Button CreateButton(string text, Color color)
        {
            return new Button { Text = text, Width = 140, Height = 34, Margin = new Padding(0, 0, 8, 0), FlatStyle = FlatStyle.Flat, BackColor = color, ForeColor = Color.White };
        }
    }
}
