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
        private readonly StorageService _storageService=new StorageService(); private AppSettings _currentSettings; private CurrentDataControl _currentDataControl; private FeatureCenterControl _featureCenter; private readonly Timer _clock=new Timer();
        public MainDashboard(){InitializeComponent();_currentSettings=_storageService.LoadSettings()??new AppSettings();_clock.Interval=1000;_clock.Tick+=(s,e)=>lblClock.Text=DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");_clock.Start();}
        private void MainDashboard_Load(object sender,EventArgs e){ApplyPresentationMode();lblStation.Text=(_currentSettings.StationName??"AQMS Station")+"  •  "+_currentSettings.DataSourceMode;btnOverview_Click(sender,e);}
        private void Show(Control c,string title,string subtitle){if(_currentDataControl!=null&&c!=_currentDataControl){_currentDataControl.StopMonitoring();_currentDataControl=null;}pnlContent.Controls.Clear();c.Dock=DockStyle.Fill;pnlContent.Controls.Add(c);lblTitle.Text=title;lblSubtitle.Text=subtitle;}
        private void btnOverview_Click(object s,EventArgs e){Show(new OperationsOverviewControl(),"Dashboard","Station overview, analyzer status and operational summary");}
        private void btnFleet_Click(object s,EventArgs e){Show(new FleetHealthControl(),"Fleet Health","Analyzer readiness, transport, endpoint and acquisition state");}
        private void btnMaintenance_Click(object s,EventArgs e){Show(new MaintenanceCenterControl(),"Maintenance & Reliability","Preventive maintenance, calibration readiness and engineering governance");}
        private void btnCurrentData_Click(object s,EventArgs e){_currentDataControl=new CurrentDataControl();Show(_currentDataControl,"Live Air Quality","Real hardware acquisition, AQI, data quality and alarms");}
        private void btnAnalyzers_Click(object s,EventArgs e){Show(new AnalyzerManagementControl(),"Analyzer Management","Configuration, endpoints, credentials, validation and enable/disable state");}
        private void btnDrivers_Click(object s,EventArgs e){Show(new DriverCatalogControl(),"Analyzer Driver Catalog","Migrated protocol inventory and hardware-validation status");}
        private void btnCalibration_Click(object s,EventArgs e){Show(new CalibrationCenterControl(),"Calibration Center","Zero/span workflow, interlocks and calibration history design");}
        private void btnIo_Click(object s,EventArgs e){Show(new IndustrialIoControl(),"DAQ / Weather / UPS","Industrial I/O and environmental auxiliary telemetry");}
        private void btnReports_Click(object s,EventArgs e){Show(new ReportsControl(),"Reports & Trends","Historical measurements, trends and export");}
        private void btnAlarms_Click(object s,EventArgs e){Show(new AlarmHistoryControl(),"Alarm & Audit History","Operational events, alarms and traceability");}
        private void btnRemote_Click(object s,EventArgs e){Show(new RemoteOperationsControl(),"Remote Operations","Analyzer remote control and safety supervision");}
        private void btnCommLab_Click(object s,EventArgs e){Show(new CommunicationLabControl(),"Communication Lab","Serial/TCP diagnostics and raw protocol capture");}
        private void btnDiagnostics_Click(object s,EventArgs e){Show(new DiagnosticsControl(),"System Diagnostics","Storage, deployment and configuration health");}
        private void btnFeatures_Click(object s,EventArgs e){_featureCenter=new FeatureCenterControl();_featureCenter.SettingsChanged+=(a,b)=>{_currentSettings=_storageService.LoadSettings()??new AppSettings();ApplyPresentationMode();};Show(_featureCenter,"Feature Center","AI, alarms, reporting, health and presentation capabilities");}
        private void btnSettings_Click(object s,EventArgs e){using(var f=new SettingsForm()){if(f.ShowDialog(this)==DialogResult.OK){_currentSettings=_storageService.LoadSettings()??new AppSettings();lblStation.Text=(_currentSettings.StationName??"AQMS Station")+"  •  "+_currentSettings.DataSourceMode;ApplyPresentationMode();}}}
        private void btnPlanner_Click(object s,EventArgs e){Show(new OperationsPlannerControl(),"Operations Planner","Work orders, preventive maintenance and calibration due planning");}
        private void btnTwin_Click(object s,EventArgs e){Show(new DigitalTwinControl(),"Station Digital Twin","Visual asset topology and synchronized operational state");}
        private void btnIncidents_Click(object s,EventArgs e){Show(new IncidentCenterControl(),"Incident & Shift Handover","Operational ownership, acknowledgement and continuity");}
        private void btnExplorer_Click(object s,EventArgs e){Show(new DataExplorerControl(),"Data Explorer","Analytics workbench for trends, quality and operational context");}
        private void btnIntegrations_Click(object s,EventArgs e){Show(new IntegrationHubControl(),"Integration Hub","Edge, industrial and enterprise interoperability surfaces");}
        private void btnQuality_Click(object s,EventArgs e){Show(new QualityAssuranceControl(),"Data Quality & QA","Configuration quality, measurement mapping and engineering-range governance");}
        private void btnCommissioning_Click(object s,EventArgs e){Show(new CommissioningControl(),"Commissioning & Readiness","Pre-deployment checklist and operator readiness workflow");}
        private void btnGovernance_Click(object s,EventArgs e){Show(new ConfigurationGovernanceControl(),"Configuration Governance","Backup inventory, recovery points and controlled configuration changes");}
        private void btnAbout_Click(object s,EventArgs e){Show(new AboutControl(),"About Ashkan AQMS","Industrial WinForms monitoring platform");}
        private void ApplyPresentationMode(){if(_currentSettings.PresentationMode){FormBorderStyle=FormBorderStyle.None;WindowState=FormWindowState.Maximized;}else{FormBorderStyle=FormBorderStyle.Sizable;if(WindowState==FormWindowState.Maximized)WindowState=FormWindowState.Normal;}}
        protected override void OnFormClosing(FormClosingEventArgs e){if(_currentDataControl!=null)_currentDataControl.StopMonitoring();_clock.Dispose();base.OnFormClosing(e);}
    }
}
