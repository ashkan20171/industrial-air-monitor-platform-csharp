using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanAQMS.Controls;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS
{
    public partial class MainDashboard : Form
    {
        private readonly StorageService _storageService;
        private AppSettings _currentSettings;
        private CurrentDataControl _currentDataControl;
        private FeatureCenterControl _featureCenter;
        private RemoteOperationsControl _remoteOperations;

        public MainDashboard()
        {
            InitializeComponent();
            _storageService = new StorageService();
            _currentSettings = _storageService.LoadSettings() ?? new AppSettings();
            ConfigureWindow();
        }

        private void ConfigureWindow()
        {
            IndustrialTheme.ApplyDarkTheme(this);
            pnlSidebar.BackColor = Color.FromArgb(20, 27, 35);
            pnlTopBar.BackColor = Color.FromArgb(24, 32, 41);
            pnlContent.BackColor = IndustrialTheme.BackgroundDark;
            lblTitle.ForeColor = IndustrialTheme.TextPrimary;
            lblSubtitle.ForeColor = IndustrialTheme.TextSecondary;
            lblLogo.ForeColor = Color.White;
            foreach (Control c in pnlSidebar.Controls)
                if (c is Button b) { b.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 50, 64); b.FlatAppearance.MouseDownBackColor = Color.FromArgb(41,128,185); b.BackColor = Color.FromArgb(20,27,35); }
        }

        private void MainDashboard_Load(object sender, EventArgs e) { ApplyPresentationMode(); LoadCurrentDataView(); }
        private void btnCurrentData_Click(object sender, EventArgs e) { HighlightSidebarButton(btnCurrentData); LoadCurrentDataView(); }
        private void btnAnalyzers_Click(object sender, EventArgs e) { HighlightSidebarButton(btnAnalyzers); LoadControlIntoContent(new AnalyzerManagementControl()); SetHeader("Analyzer Management", "Configure analyzers, communication and diagnostics"); }
        private void btnReports_Click(object sender, EventArgs e) { HighlightSidebarButton(btnReports); LoadControlIntoContent(new ReportsControl()); SetHeader("Reports & Analytics", "Historical measurements, trends and export"); }
        private void btnFeatures_Click(object sender, EventArgs e) { HighlightSidebarButton(btnFeatures); SetHeader("Feature Center", "Enable or disable AI, alarms, reporting, health and presentation capabilities"); _featureCenter = new FeatureCenterControl(); _featureCenter.SettingsChanged += FeatureCenter_SettingsChanged; LoadControlIntoContent(_featureCenter); }
        private void FeatureCenter_SettingsChanged(object sender, EventArgs e) { _currentSettings = _storageService.LoadSettings() ?? new AppSettings(); ApplyPresentationMode(); }
        private void ApplyPresentationMode() { _currentSettings = _storageService.LoadSettings() ?? new AppSettings(); if (_currentSettings.PresentationMode) { FormBorderStyle = FormBorderStyle.None; WindowState = FormWindowState.Maximized; } else { FormBorderStyle = FormBorderStyle.Sizable; if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal; } }

        private void btnRemote_Click(object sender, EventArgs e) { HighlightSidebarButton(btnRemote); _remoteOperations = new RemoteOperationsControl(); LoadControlIntoContent(_remoteOperations); SetHeader("Remote Operations", "Analyzer remote control • calibration supervision • safety interlocks"); }

        private void btnAlarms_Click(object sender, EventArgs e) { HighlightSidebarButton(btnAlarms); LoadControlIntoContent(new AlarmHistoryControl()); SetHeader("Alarm & Audit History", "Operational events, alarm activity and audit trail"); }
        private void btnDiagnostics_Click(object sender, EventArgs e) { HighlightSidebarButton(btnDiagnostics); LoadControlIntoContent(new DiagnosticsControl()); SetHeader("System Diagnostics", "Deployment readiness, storage and configuration health"); }

        private void btnAbout_Click(object sender, EventArgs e) { HighlightSidebarButton(btnAbout); LoadControlIntoContent(new AboutControl()); SetHeader("About AshkanAQMS", "Product, technology and developer information"); }

        private void btnSettings_Click(object sender, EventArgs e) { HighlightSidebarButton(btnSettings); using(var f=new SettingsForm()){ if(f.ShowDialog(this)==DialogResult.OK){_currentSettings=_storageService.LoadSettings() ?? new AppSettings(); if(_currentDataControl!=null) LoadCurrentDataView(); }} }

        private void LoadCurrentDataView()
        {
            HighlightSidebarButton(btnCurrentData); SetHeader("AQMS Command Center", "Real-time air quality monitoring • anomaly detection • alarms");
            if(_currentDataControl!=null){ _currentDataControl.StopMonitoring(); _currentDataControl.Dispose(); }
            _currentDataControl=new CurrentDataControl(); LoadControlIntoContent(_currentDataControl);
        }
        private void SetHeader(string title,string subtitle){lblTitle.Text=title;lblSubtitle.Text=subtitle;}
        private void LoadControlIntoContent(Control control){pnlContent.Controls.Clear();control.Dock=DockStyle.Fill;pnlContent.Controls.Add(control);}
        private void HighlightSidebarButton(Button btn){foreach(Control ctrl in pnlSidebar.Controls)if(ctrl is Button b)b.BackColor=b==btn?Color.FromArgb(41,128,185):Color.FromArgb(20,27,35);}
        protected override void OnFormClosing(FormClosingEventArgs e){if(_currentDataControl!=null)_currentDataControl.StopMonitoring();base.OnFormClosing(e);}
    }
}
