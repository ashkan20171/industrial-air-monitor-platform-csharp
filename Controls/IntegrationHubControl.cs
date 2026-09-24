using System;
using System.Drawing;
using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
 public sealed class IntegrationHubControl:UserControl
 {
  public IntegrationHubControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);Build();}
  private void Build(){var h=IndustrialTheme.Heading("Integration Hub & Edge Gateway",18);h.Dock=DockStyle.Top;h.Height=38;var s=new Label{Text="Protocol inventory and controlled integration points for plant, cloud and enterprise systems.",Dock=DockStyle.Top,Height=34,ForeColor=IndustrialTheme.Muted};var flow=new FlowLayoutPanel{Dock=DockStyle.Fill,AutoScroll=true,WrapContents=true,Padding=new Padding(0,12,0,0)};string[] cards={"OPC UA|Industrial interoperability|Adapter boundary prepared","MQTT|Telemetry publish / subscribe|Adapter boundary prepared","REST API|External data access|Architecture-ready","SQL Historian|Long-term station history|Local storage active","SignalR|Live operations feed|Legacy capability mapped","CSV / Excel|Portable reporting|CSV export active","Serial / RS-232|Analyzer transport|Real hardware supported","TCP / UDP|Network analyzers|Real hardware supported","Modbus RTU/TCP|Registers and field devices|Protocol catalogued","HTTP / CGI|LAN analyzer endpoints|Legacy drivers retained","DAQ / Analog I/O|4-20mA / voltage / digital|Legacy drivers retained","Backup / Restore|Configuration portability|Analyzer backup supported"};foreach(var x in cards){var p=x.Split('|');flow.Controls.Add(Card(p[0],p[1],p[2]));}Controls.Add(flow);Controls.Add(s);Controls.Add(h);}
  private Control Card(string name,string detail,string status){var p=new AccentPanel{Width=315,Height=120,BackColor=Color.White,Padding=new Padding(17),Margin=new Padding(0,0,14,14),Accent=status.Contains("active")||status.Contains("supported")?IndustrialTheme.Green:IndustrialTheme.Cyan};p.Controls.Add(new Label{Text=status,Dock=DockStyle.Bottom,Height=24,ForeColor=IndustrialTheme.Blue,Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold)});p.Controls.Add(new Label{Text=detail,Dock=DockStyle.Fill,ForeColor=IndustrialTheme.Muted});p.Controls.Add(new Label{Text=name,Dock=DockStyle.Top,Height=30,ForeColor=IndustrialTheme.Ink,Font=new Font("Segoe UI Semibold",11,FontStyle.Bold)});return p;}
 }
}
