using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Services;
namespace AshkanAQMS.Controls
{
 public sealed class QualityAssuranceControl:UserControl
 {
  readonly StorageService storage=new StorageService(); readonly DataGridView grid=new DataGridView(); readonly Label summary=new Label();
  public QualityAssuranceControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);Build();LoadData();}
  void Build(){var title=new Label{Text="DATA QUALITY & QA CENTER",Dock=DockStyle.Top,Height=42,Font=new Font("Segoe UI Semibold",18,FontStyle.Bold),ForeColor=IndustrialTheme.Ink};summary.Dock=DockStyle.Top;summary.Height=58;summary.Font=new Font("Segoe UI",10);summary.ForeColor=IndustrialTheme.Muted;grid.Dock=DockStyle.Fill;grid.BackgroundColor=Color.White;grid.BorderStyle=BorderStyle.None;grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;grid.AllowUserToAddRows=false;grid.ReadOnly=true;grid.RowHeadersVisible=false;grid.Columns.Add("Analyzer","Analyzer");grid.Columns.Add("Driver","Driver");grid.Columns.Add("Measurements","Measurements");grid.Columns.Add("Range","Range QA");grid.Columns.Add("State","Configuration State");Controls.Add(grid);Controls.Add(summary);Controls.Add(title);}
  void LoadData(){var list=storage.LoadAnalyzers()??new System.Collections.Generic.List<Models.AnalyzerConfig>();grid.Rows.Clear();foreach(var a in list){var m=a.Measurements==null?0:a.Measurements.Count(x=>x.Enabled);grid.Rows.Add(a.Name,a.DriverId,m,a.EnableEngineeringRangeCheck?(a.EngineeringMin+" … "+a.EngineeringMax):"Not enforced",a.Enabled?"Enabled":"Disabled");}summary.Text=$"Configured analyzers: {list.Count}   •   Enabled: {list.Count(x=>x.Enabled)}   •   Engineering-range QA: {list.Count(x=>x.EnableEngineeringRangeCheck)}\nUse this center to review configuration quality before commissioning or maintenance handover.";}
 }
}
