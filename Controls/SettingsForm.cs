using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS
{
    public partial class SettingsForm : Form
    {
        private AppSettings _settings;

        // کنترل‌های ورودی عددی
        private NumericUpDown numInterval;
        private NumericUpDown numZScore;
        private NumericUpDown numAlpha;
        private NumericUpDown numHistoryLimit;

        public SettingsForm()
        {
            InitializeComponent();
            SetupDynamicControls();
            LoadCurrentSettings();
            IndustrialTheme.ApplyDarkTheme(this);
        }

        private void SetupDynamicControls()
        {
            grpAi.ForeColor = IndustrialTheme.TextSecondary;
            grpAlerts.ForeColor = IndustrialTheme.TextSecondary;
            chkEnableSound.ForeColor = IndustrialTheme.TextPrimary;
            chkEnableAutoArchive.ForeColor = IndustrialTheme.TextPrimary;
            btnReset.ForeColor = IndustrialTheme.TextSecondary;

            numInterval = AddNumericRow(grpAi, "بازه به‌روزرسانی سنسورها (ثانیه):", 1, 60, 30);
            numZScore = AddNumericRow(grpAi, "آستانه ناهنجاری سنسور (Z-Score):", 1.0M, 5.0M, 65, 0.1M);
            numAlpha = AddNumericRow(grpAi, "ضریب هموارسازی پیش‌بینی (EMA Alpha):", 0.05M, 0.95M, 100, 0.05M);
            numHistoryLimit = AddNumericRow(grpAi, "حجم پنجره تحلیل تاریخچه:", 5, 100, 135);
        }

        private NumericUpDown AddNumericRow(GroupBox parent, string labelText, decimal min, decimal max, int top, decimal increment = 1M)
        {
            var lbl = new Label
            {
                Text = labelText,
                ForeColor = IndustrialTheme.TextPrimary,
                Location = new Point(15, top + 3),
                Width = 260,
                AutoSize = false
            };
            var num = new NumericUpDown
            {
                Location = new Point(290, top),
                Width = 115,
                Minimum = min,
                Maximum = max,
                Increment = increment,
                DecimalPlaces = increment < 1M ? 2 : 0,
                BackColor = IndustrialTheme.SurfaceCard,
                ForeColor = IndustrialTheme.TextPrimary
            };
            parent.Controls.Add(lbl);
            parent.Controls.Add(num);
            return num;
        }

        private void LoadCurrentSettings()
        {
            _settings = StorageService.LoadSettings();
            BindSettingsToInputs();
        }

        private void BindSettingsToInputs()
        {
            if (_settings == null) _settings = new AppSettings();

            numInterval.Value = Math.Max(numInterval.Minimum, Math.Min(numInterval.Maximum, _settings.PollingIntervalSeconds));
            numZScore.Value = Math.Max(numZScore.Minimum, Math.Min(numZScore.Maximum, (decimal)_settings.AnomalyZScoreThreshold));
            numAlpha.Value = Math.Max(numAlpha.Minimum, Math.Min(numAlpha.Maximum, (decimal)_settings.EmaAlpha));
            numHistoryLimit.Value = Math.Max(numHistoryLimit.Minimum, Math.Min(numHistoryLimit.Maximum, _settings.HistoryWindowSize));
            chkEnableSound.Checked = _settings.EnableSoundAlerts;
            chkEnableAutoArchive.Checked = _settings.EnableAutoArchive;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _settings.PollingIntervalSeconds = (int)numInterval.Value;
                _settings.AnomalyZScoreThreshold = (double)numZScore.Value;
                _settings.EmaAlpha = (double)numAlpha.Value;
                _settings.HistoryWindowSize = (int)numHistoryLimit.Value;
                _settings.EnableSoundAlerts = chkEnableSound.Checked;
                _settings.EnableAutoArchive = chkEnableAutoArchive.Checked;

                StorageService.SaveSettings(_settings);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ذخیره تنظیمات: " + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            _settings = new AppSettings();
            BindSettingsToInputs();
        }
    }
}
