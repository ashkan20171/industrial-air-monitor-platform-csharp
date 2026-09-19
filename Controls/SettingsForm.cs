using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS
{
    public partial class SettingsForm : Form
    {
        private readonly StorageService _storageService;
        private ComboBox _dataSourceCombo;
        private CheckBox _allowSimulation;
        public AppSettings CurrentSettings { get; private set; }

        public SettingsForm()
        {
            InitializeComponent();
            _storageService = new StorageService();

            // بارگذاری تنظیمات یا ایجاد پیش‌فرض
            CurrentSettings = _storageService.LoadSettings() ?? new AppSettings();

            // اعمال تم صنعتی تیره
            IndustrialTheme.ApplyDarkTheme(this);
            BuildDataSourceSettingsUi();

            // بارگذاری مقادیر در کنترل‌های فرم
            LoadSettingsToUi();
        }


        private void BuildDataSourceSettingsUi()
        {
            ClientSize = new Size(406, 410);
            _dataSourceCombo = new ComboBox { Left = 240, Top = 278, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            _dataSourceCombo.Items.AddRange(new object[] { "RealHardware", "Simulation" });
            Controls.Add(new Label { Left = 38, Top = 282, Width = 185, Height = 22, Text = "Data source:", ForeColor = Color.White });
            Controls.Add(_dataSourceCombo);
            _allowSimulation = new CheckBox { Left = 38, Top = 314, Width = 350, Height = 24, Text = "Allow Simulation mode (test only)", ForeColor = Color.White };
            Controls.Add(_allowSimulation);
            btnReset.Location = new Point(18, 360);
            btnCancel.Location = new Point(192, 360);
            btnSave.Location = new Point(293, 360);
        }

        private void LoadSettingsToUi()
        {
            if (CurrentSettings == null) return;

            // تنظیمات عمومی و هشدارها
            chkEnableSound.Checked = CurrentSettings.EnableSoundAlerts;
            chkEnableAutoArchive.Checked = CurrentSettings.EnableAutoArchive;

            // تنظیمات داده و موتور هوش مصنوعی
            numPollingInterval.Value = Math.Max(numPollingInterval.Minimum, Math.Min(numPollingInterval.Maximum, CurrentSettings.PollingIntervalSeconds));
            numHistoryWindowSize.Value = Math.Max(numHistoryWindowSize.Minimum, Math.Min(numHistoryWindowSize.Maximum, CurrentSettings.HistoryWindowSize));
            numZScoreThreshold.Value = Math.Max(numZScoreThreshold.Minimum, Math.Min(numZScoreThreshold.Maximum, (decimal)CurrentSettings.AnomalyZScoreThreshold));
            numEmaAlpha.Value = Math.Max(numEmaAlpha.Minimum, Math.Min(numEmaAlpha.Maximum, (decimal)CurrentSettings.EmaAlpha));
            _dataSourceCombo.SelectedItem = string.Equals(CurrentSettings.DataSourceMode, "Simulation", StringComparison.OrdinalIgnoreCase) ? "Simulation" : "RealHardware";
            _allowSimulation.Checked = CurrentSettings.AllowSimulationMode;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // خواندن مقادیر از UI
                CurrentSettings.EnableSoundAlerts = chkEnableSound.Checked;
                CurrentSettings.EnableAutoArchive = chkEnableAutoArchive.Checked;

                CurrentSettings.PollingIntervalSeconds = (int)numPollingInterval.Value;
                CurrentSettings.HistoryWindowSize = (int)numHistoryWindowSize.Value;
                CurrentSettings.AnomalyZScoreThreshold = (double)numZScoreThreshold.Value;
                CurrentSettings.EmaAlpha = (double)numEmaAlpha.Value;
                CurrentSettings.AllowSimulationMode = _allowSimulation.Checked;
                CurrentSettings.DataSourceMode = _dataSourceCombo.SelectedItem == null ? "RealHardware" : _dataSourceCombo.SelectedItem.ToString();
                if (string.Equals(CurrentSettings.DataSourceMode, "Simulation", StringComparison.OrdinalIgnoreCase) && !CurrentSettings.AllowSimulationMode)
                    throw new InvalidOperationException("Simulation mode is selected but the explicit test-mode permission is disabled.");

                // ذخیره‌سازی روی دیسک
                _storageService.SaveSettings(CurrentSettings);

                MessageBox.Show("تنظیمات با موفقیت ذخیره شد.", "ذخیره تنظیمات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره تنظیمات:\n{ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید تمام تنظیمات به حالت پیش‌فرض بازگردانده شوند؟",
                "بازنشانی تنظیمات",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                CurrentSettings = new AppSettings();
                LoadSettingsToUi();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
