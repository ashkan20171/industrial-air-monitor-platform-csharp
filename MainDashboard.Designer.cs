using System.Drawing;
using System.Windows.Forms;
namespace AshkanAQMS
{
 partial class MainDashboard
 {
  private System.ComponentModel.IContainer components=null;
  protected override void Dispose(bool disposing){if(disposing&&components!=null)components.Dispose();base.Dispose(disposing);}
  private void InitializeComponent()
  {
   this.menu=new MenuStrip(); this.toolBar=new ToolStrip(); this.pnlBrand=new Panel(); this.lblBrand=new Label(); this.lblStation=new Label(); this.lblClock=new Label(); this.lblSystem=new Label(); this.pnlSubHeader=new Panel(); this.lblTitle=new Label(); this.lblSubtitle=new Label(); this.pnlContent=new Panel(); this.statusBar=new StatusStrip();
   // Classic SadraAQMS-inspired top menu, reorganized for the enterprise edition.
   var file=new ToolStripMenuItem("File"); file.DropDownItems.Add("Dashboard",null,btnOverview_Click); file.DropDownItems.Add("Live Data",null,btnCurrentData_Click); file.DropDownItems.Add(new ToolStripSeparator()); file.DropDownItems.Add("Settings",null,btnSettings_Click); file.DropDownItems.Add("About",null,btnAbout_Click);
   var analyzer=new ToolStripMenuItem("Analyzers"); analyzer.DropDownItems.Add("Analyzer Parameters",null,btnAnalyzers_Click); analyzer.DropDownItems.Add("Fleet Health",null,btnFleet_Click); analyzer.DropDownItems.Add("Driver Catalog",null,btnDrivers_Click); analyzer.DropDownItems.Add("Calibration",null,btnCalibration_Click); analyzer.DropDownItems.Add("Maintenance",null,btnMaintenance_Click);
   var view=new ToolStripMenuItem("View"); view.DropDownItems.Add("Current Data",null,btnCurrentData_Click); view.DropDownItems.Add("Historical / Reports",null,btnReports_Click); view.DropDownItems.Add("Alarms & Audit",null,btnAlarms_Click); view.DropDownItems.Add("Data Explorer",null,btnExplorer_Click); view.DropDownItems.Add("Station Digital Twin",null,btnTwin_Click);
   var tools=new ToolStripMenuItem("Tools"); tools.DropDownItems.Add("Remote",null,btnRemote_Click); tools.DropDownItems.Add("DAQ / Weather / UPS",null,btnIo_Click); tools.DropDownItems.Add("Communication Lab",null,btnCommLab_Click); tools.DropDownItems.Add("Diagnostics",null,btnDiagnostics_Click); tools.DropDownItems.Add("Integration Hub",null,btnIntegrations_Click);
   var operations=new ToolStripMenuItem("Operations"); operations.DropDownItems.Add("Operations Planner",null,btnPlanner_Click); operations.DropDownItems.Add("Incident & Shift Handover",null,btnIncidents_Click); operations.DropDownItems.Add("Commissioning & Readiness",null,btnCommissioning_Click); operations.DropDownItems.Add("Data Quality & QA",null,btnQuality_Click); operations.DropDownItems.Add("Configuration Governance",null,btnGovernance_Click); operations.DropDownItems.Add("Feature Center",null,btnFeatures_Click);
   var help=new ToolStripMenuItem("Help"); help.DropDownItems.Add("About Ashkan AQMS",null,btnAbout_Click);
   this.menu.Items.AddRange(new ToolStripItem[]{file,analyzer,view,tools,operations,help}); this.menu.Dock=DockStyle.Top; this.menu.BackColor=IndustrialTheme.Navy900; this.menu.ForeColor=Color.White; this.menu.Font=new Font("Segoe UI Semibold",9.5f,FontStyle.Bold); this.menu.Padding=new Padding(12,4,6,4); this.menu.RenderMode=ToolStripRenderMode.System;
   foreach(ToolStripMenuItem item in this.menu.Items){item.ForeColor=Color.White;item.Padding=new Padding(10,4,10,4);}

   this.pnlBrand.Dock=DockStyle.Top; this.pnlBrand.Height=68; this.pnlBrand.BackColor=IndustrialTheme.Navy800; this.pnlBrand.Controls.Add(this.lblSystem); this.pnlBrand.Controls.Add(this.lblClock); this.pnlBrand.Controls.Add(this.lblStation); this.pnlBrand.Controls.Add(this.lblBrand);
   this.lblBrand.Text="ASHKAN AQMS  •  INDUSTRIAL AIR QUALITY MONITORING"; this.lblBrand.Font=new Font("Segoe UI Semibold",18,FontStyle.Bold); this.lblBrand.ForeColor=Color.White; this.lblBrand.AutoSize=true; this.lblBrand.Location=new Point(20,10);
   this.lblStation.Text="Station"; this.lblStation.Font=new Font("Segoe UI",9f); this.lblStation.ForeColor=Color.FromArgb(176,207,222); this.lblStation.AutoSize=true; this.lblStation.Location=new Point(23,42);
   this.lblSystem.Text="● SYSTEM ONLINE"; this.lblSystem.Font=new Font("Segoe UI Semibold",9,FontStyle.Bold); this.lblSystem.ForeColor=IndustrialTheme.Green; this.lblSystem.AutoSize=true; this.lblSystem.Anchor=AnchorStyles.Top|AnchorStyles.Right; this.lblSystem.Location=new Point(1240,13);
   this.lblClock.AutoSize=true; this.lblClock.Anchor=AnchorStyles.Top|AnchorStyles.Right; this.lblClock.Font=new Font("Consolas",10,FontStyle.Bold); this.lblClock.ForeColor=Color.White; this.lblClock.Location=new Point(1215,40);

   // Navigation lives in the permanent MenuStrip. The former quick-action toolbar was intentionally removed
   // to keep the dashboard focused on operational information and avoid duplicate navigation.
   this.toolBar.Visible=false; this.toolBar.Enabled=false; this.toolBar.AutoSize=false; this.toolBar.Height=0;


   this.pnlSubHeader.Dock=DockStyle.Top; this.pnlSubHeader.Height=70; this.pnlSubHeader.BackColor=Color.FromArgb(247,249,251); this.pnlSubHeader.Padding=new Padding(24,9,24,7); this.pnlSubHeader.Controls.Add(this.lblSubtitle); this.pnlSubHeader.Controls.Add(this.lblTitle);
   this.lblTitle.Dock=DockStyle.Top; this.lblTitle.Height=34; this.lblTitle.Font=new Font("Segoe UI Semibold",17,FontStyle.Bold); this.lblTitle.ForeColor=IndustrialTheme.Ink; this.lblTitle.Text="Dashboard";
   this.lblSubtitle.Dock=DockStyle.Bottom; this.lblSubtitle.Height=21; this.lblSubtitle.Font=new Font("Segoe UI",9.2f); this.lblSubtitle.ForeColor=IndustrialTheme.Muted; this.lblSubtitle.Text="Industrial monitoring, analyzer control and station operations";
   this.pnlContent.Dock=DockStyle.Fill; this.pnlContent.BackColor=IndustrialTheme.Canvas;

   var stServer=new ToolStripStatusLabel("● SERVER") { ForeColor=IndustrialTheme.Green, Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold) };
   var stDb=new ToolStripStatusLabel("● DATABASE") { ForeColor=IndustrialTheme.Green, Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold) };
   var stDaq=new ToolStripStatusLabel("● DAQ") { ForeColor=IndustrialTheme.Cyan, Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold) };
   var stWatch=new ToolStripStatusLabel("● WATCHDOG") { ForeColor=IndustrialTheme.Green, Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold) };
   var spring=new ToolStripStatusLabel(){Spring=true}; var edition=new ToolStripStatusLabel("AshkanAQMS Enterprise • v10.3 Menu-Only Navigation") { ForeColor=IndustrialTheme.Muted };
   this.statusBar.Items.AddRange(new ToolStripItem[]{stServer,new ToolStripStatusLabel("  |  "),stDb,new ToolStripStatusLabel("  |  "),stDaq,new ToolStripStatusLabel("  |  "),stWatch,spring,edition}); this.statusBar.BackColor=Color.White; this.statusBar.SizingGrip=false;

   this.ClientSize=new Size(1500,920); this.MinimumSize=new Size(1200,760); this.Controls.Add(this.pnlContent); this.Controls.Add(this.pnlSubHeader); this.Controls.Add(this.pnlBrand); this.Controls.Add(this.menu); this.Controls.Add(this.statusBar); this.MainMenuStrip=this.menu; this.StartPosition=FormStartPosition.CenterScreen; this.Text="Ashkan AQMS Enterprise"; this.Font=new Font("Segoe UI",9); this.Load+=MainDashboard_Load;
  }
  private ToolStripButton ToolButton(string text,System.EventHandler handler){var b=new ToolStripButton(text){AutoSize=false,Width=118,Height=56,DisplayStyle=ToolStripItemDisplayStyle.Text,TextAlign=ContentAlignment.MiddleCenter,Font=new Font("Segoe UI Semibold",8.7f,FontStyle.Bold),ForeColor=IndustrialTheme.Ink,Margin=new Padding(2,0,2,0)};b.Click+=handler;return b;}
  private Panel pnlBrand,pnlSubHeader,pnlContent; private Label lblBrand,lblStation,lblClock,lblSystem,lblTitle,lblSubtitle; private MenuStrip menu; private ToolStrip toolBar; private StatusStrip statusBar;
 }
}
