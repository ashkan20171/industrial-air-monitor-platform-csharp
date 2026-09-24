using System;using System.Drawing;using System.Linq;using System.Windows.Forms;using AshkanAQMS.Services;
namespace AshkanAQMS.Controls
{
 public sealed class CommissioningControl:UserControl
 {
  readonly StorageService storage=new StorageService(); readonly CheckedListBox checks=new CheckedListBox(); readonly Label score=new Label();
  public CommissioningControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(24);Build();RefreshChecklist();}
  void Build(){var h=new Label{Text="COMMISSIONING & READINESS CHECKLIST",Dock=DockStyle.Top,Height=44,Font=new Font("Segoe UI Semibold",18,FontStyle.Bold),ForeColor=IndustrialTheme.Ink};score.Dock=DockStyle.Top;score.Height=48;score.Font=new Font("Segoe UI Semibold",11,FontStyle.Bold);checks.Dock=DockStyle.Fill;checks.Font=new Font("Segoe UI",10);checks.CheckOnClick=true;checks.ItemCheck+=(s,e)=>BeginInvoke((Action)UpdateScore);var refresh=new Button{Text="Refresh from configuration",Dock=DockStyle.Bottom,Height=40,FlatStyle=FlatStyle.Flat};refresh.Click+=(s,e)=>RefreshChecklist();Controls.Add(checks);Controls.Add(score);Controls.Add(h);Controls.Add(refresh);}
  void RefreshChecklist(){checks.Items.Clear();var a=storage.LoadAnalyzers()??new System.Collections.Generic.List<Models.AnalyzerConfig>();Add("Station settings reviewed",true);Add("At least one analyzer configured",a.Count>0);Add("All enabled analyzers have a driver",a.Where(x=>x.Enabled).All(x=>!string.IsNullOrWhiteSpace(x.DriverId)));Add("All enabled analyzers have an endpoint",a.Where(x=>x.Enabled).All(x=>x.ConnectionType=="COM"?!string.IsNullOrWhiteSpace(x.ComPort):!string.IsNullOrWhiteSpace(x.IpAddress)));Add("Measurement mapping configured",a.Where(x=>x.Enabled).All(x=>x.Measurements!=null&&x.Measurements.Any(m=>m.Enabled)));Add("Engineering ranges reviewed",a.Where(x=>x.Enabled).All(x=>x.EnableEngineeringRangeCheck));Add("Configuration backup created",false);Add("Physical communication test completed",false);Add("Zero/span verification completed",false);Add("Alarm and watchdog test completed",false);UpdateScore();}
  void Add(string text,bool value){checks.Items.Add(text,value);}
  void UpdateScore(){int total=checks.Items.Count,ok=Enumerable.Range(0,total).Count(i=>checks.GetItemChecked(i));score.Text=$"Readiness: {ok}/{total} checks complete   •   Operator sign-off remains required for physical tests.";score.ForeColor=ok==total?IndustrialTheme.Green:IndustrialTheme.Ink;}
 }
}
