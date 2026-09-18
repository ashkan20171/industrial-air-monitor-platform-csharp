using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    /// <summary>Self-diagnostics and deployment-readiness view for operators and technical reviewers.</summary>
    public class DiagnosticsControl : UserControl
    {
        private readonly StorageService _storage = new StorageService();
        private readonly AuditLogger _audit = new AuditLogger();
        private readonly TableLayoutPanel _table = new TableLayoutPanel();
        private readonly Label _overall = new Label();

        public DiagnosticsControl()
        {
            Dock = DockStyle.Fill; BackColor = IndustrialTheme.BackgroundDark; Padding = new Padding(24); BuildUi(); RunDiagnostics();
        }

        private void BuildUi()
        {
            var title = new Label { Dock = DockStyle.Top, Height = 38, Text = "System Diagnostics", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = IndustrialTheme.TextPrimary };
            var subtitle = new Label { Dock = DockStyle.Top, Height = 34, Text = "Deployment readiness, storage integrity and feature configuration overview.", Font = new Font("Segoe UI", 9.5F), ForeColor = IndustrialTheme.TextSecondary };
            Controls.Add(subtitle); Controls.Add(title);
            var bar = new Panel { Dock = DockStyle.Top, Height = 50 };
            var refresh = new Button { Text = "Run Diagnostics", Location = new Point(0, 8), Size = new Size(130, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(52,152,219), ForeColor = Color.White };
            refresh.Click += (s,e) => RunDiagnostics();
            var export = new Button { Text = "Export Snapshot", Location = new Point(140, 8), Size = new Size(130, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(39,174,96), ForeColor = Color.White };
            export.Click += (s,e) => ExportSnapshot();
            bar.Controls.Add(refresh); bar.Controls.Add(export); Controls.Add(bar);
            _overall.Dock = DockStyle.Top; _overall.Height = 42; _overall.Font = new Font("Segoe UI", 12F, FontStyle.Bold); _overall.TextAlign = ContentAlignment.MiddleLeft; Controls.Add(_overall);
            _table.Dock = DockStyle.Fill; _table.ColumnCount = 3; _table.RowCount = 1; _table.Padding = new Padding(0, 10, 0, 0); _table.AutoScroll = true;
            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25)); _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45)); Controls.Add(_table);
        }

        private void RunDiagnostics()
        {
            _table.Controls.Clear(); _table.RowCount = 0;
            var settings = _storage.LoadSettings() ?? new AppSettings();
            var analyzers = _storage.LoadAnalyzers() ?? new List<AnalyzerConfig>();
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AshkanAQMS");
            var checks = new List<Tuple<string,string,string,bool>>();
            checks.Add(Tuple.Create("Application", "Runtime", Environment.Version.ToString(), true));
            checks.Add(Tuple.Create("Application", "OS", Environment.OSVersion.VersionString, true));
            checks.Add(Tuple.Create("Storage", "Application Data", folder, Directory.Exists(folder)));
            checks.Add(Tuple.Create("Storage", "Writable", "Configuration and audit directory", CanWrite(folder)));
            checks.Add(Tuple.Create("Configuration", "Analyzers", analyzers.Count + " configured", analyzers.Count > 0));
            checks.Add(Tuple.Create("Configuration", "Polling", settings.PollingIntervalSeconds + " seconds", settings.PollingIntervalSeconds >= 1));
            checks.Add(Tuple.Create("Features", "AI Insights", settings.EnableAiInsights ? "Enabled" : "Disabled", true));
            checks.Add(Tuple.Create("Features", "Advanced Analytics", settings.EnableAdvancedAnalytics ? "Enabled" : "Disabled", true));
            checks.Add(Tuple.Create("Features", "Anomaly Alarms", settings.EnableAnomalyAlarms ? "Enabled" : "Disabled", true));
            checks.Add(Tuple.Create("Features", "Executive Reports", settings.EnableExecutiveReports ? "Enabled" : "Disabled", true));
            checks.Add(Tuple.Create("Features", "Health Monitoring", settings.EnableHealthMonitoring ? "Enabled" : "Disabled", true));
            checks.Add(Tuple.Create("Audit", "Audit Log", _audit.GetPath(), File.Exists(_audit.GetPath()) || CanWrite(Path.GetDirectoryName(_audit.GetPath()))));
            var failed = 0; foreach (var c in checks) { AddRow(c.Item1,c.Item2,c.Item3,c.Item4); if(!c.Item4) failed++; }
            _overall.Text = failed == 0 ? "SYSTEM STATUS  •  READY" : "SYSTEM STATUS  •  REVIEW REQUIRED (" + failed + " check(s) failed)";
            _overall.ForeColor = failed == 0 ? Color.FromArgb(46,204,113) : Color.FromArgb(241,196,15);
        }

        private void AddRow(string group,string name,string value,bool ok)
        {
            var r=_table.RowCount++; _table.RowStyles.Add(new RowStyle(SizeType.Absolute,38));
            var a=Cell(group); var b=Cell(name); var c=Cell(value + (ok ? "   ✓" : "   ✕")); c.ForeColor=ok?Color.FromArgb(46,204,113):Color.FromArgb(231,76,60);
            _table.Controls.Add(a,0,r); _table.Controls.Add(b,1,r); _table.Controls.Add(c,2,r);
        }
        private Label Cell(string text){return new Label{Dock=DockStyle.Fill,Text=text,Padding=new Padding(8,8,8,4),ForeColor=IndustrialTheme.TextPrimary,BackColor=IndustrialTheme.SurfaceCard};}
        private bool CanWrite(string folder){try{if(string.IsNullOrWhiteSpace(folder))return false;if(!Directory.Exists(folder))Directory.CreateDirectory(folder);var p=Path.Combine(folder,".aqms_write_test");File.WriteAllText(p,"ok");File.Delete(p);return true;}catch{return false;}}
        private void ExportSnapshot(){using(var d=new SaveFileDialog{Filter="Text files (*.txt)|*.txt",FileName="AshkanAQMS_Diagnostics_"+DateTime.Now.ToString("yyyyMMdd_HHmmss")+".txt"}){if(d.ShowDialog(this)!=DialogResult.OK)return;File.WriteAllText(d.FileName,"AshkanAQMS System Diagnostics\r\nGenerated: "+DateTime.Now.ToString("O")+"\r\n\r\n"+_overall.Text+"\r\n");MessageBox.Show(this,"Diagnostics snapshot exported.","Export",MessageBoxButtons.OK,MessageBoxIcon.Information);}}
    }
}
