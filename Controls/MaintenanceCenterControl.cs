using System.Drawing;
using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
 public sealed class MaintenanceCenterControl:UserControl
 {
  public MaintenanceCenterControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(24);var h=IndustrialTheme.Heading("Maintenance & Reliability Center",20);h.Dock=DockStyle.Top;h.Height=45;Controls.Add(Build());Controls.Add(h);}
  private Control Build(){var t=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=3,RowCount=2};for(int i=0;i<3;i++)t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.33f));t.RowStyles.Add(new RowStyle(SizeType.Percent,50));t.RowStyles.Add(new RowStyle(SizeType.Percent,50));string[] a={"Preventive maintenance|Service schedules, due dates and ownership","Calibration readiness|Zero/span prerequisites and traceable checks","Consumables|Filters, cylinders and replacement planning","Communication reliability|Timeout and failure-rate review","Configuration governance|Backup, restore and change traceability","Engineering notes|Shift handover and device-specific observations"};for(int i=0;i<a.Length;i++){var x=a[i].Split('|');var p=new AccentPanel{Dock=DockStyle.Fill,BackColor=Color.White,Padding=new Padding(18),Margin=new Padding(8),Accent=i<3?IndustrialTheme.Cyan:IndustrialTheme.Blue};p.Controls.Add(new Label{Text=x[1],Dock=DockStyle.Fill,ForeColor=IndustrialTheme.Muted,Font=new Font("Segoe UI",9.2f)});p.Controls.Add(new Label{Text=x[0],Dock=DockStyle.Top,Height=32,ForeColor=IndustrialTheme.Ink,Font=new Font("Segoe UI Semibold",11,FontStyle.Bold)});t.Controls.Add(p,i%3,i/3);}return t;}
 }
}
