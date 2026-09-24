using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Services;
namespace AshkanAQMS.Controls
{
 public sealed class DigitalTwinControl:UserControl
 {
  private readonly StorageService storage=new StorageService(); private readonly Timer timer=new Timer(); private readonly FlowLayoutPanel assets=new FlowLayoutPanel(); private readonly Label heartbeat=new Label();
  public DigitalTwinControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);Build();timer.Interval=3000;timer.Tick+=(s,e)=>RefreshTwin();timer.Start();RefreshTwin();}
  private void Build(){var h=IndustrialTheme.Heading("Station Digital Twin",18);h.Dock=DockStyle.Top;h.Height=38;var s=new Label{Text="A visual operational model of analyzers, acquisition, weather, DAQ, storage and station services.",Dock=DockStyle.Top,Height=34,ForeColor=IndustrialTheme.Muted};heartbeat.Dock=DockStyle.Top;heartbeat.Height=34;heartbeat.ForeColor=IndustrialTheme.Green;heartbeat.Font=new Font("Segoe UI Semibold",9,FontStyle.Bold);assets.Dock=DockStyle.Fill;assets.AutoScroll=true;assets.WrapContents=true;assets.Padding=new Padding(0,12,0,0);Controls.Add(assets);Controls.Add(heartbeat);Controls.Add(s);Controls.Add(h);}
  private void RefreshTwin(){assets.Controls.Clear();var a=storage.LoadAnalyzers();foreach(var x in a)assets.Controls.Add(Node(x.Name,x.Manufacturer+" • "+x.Model,x.Enabled?"ACQUISITION READY":"DISABLED",x.Enabled?IndustrialTheme.Green:IndustrialTheme.Muted));assets.Controls.Add(Node("Acquisition Gateway","Safety boundary","ONLINE",IndustrialTheme.Cyan));assets.Controls.Add(Node("Historical Store","Local archive / reports","READY",IndustrialTheme.Blue));assets.Controls.Add(Node("Alarm Engine","Rules + event lifecycle","ARMED",IndustrialTheme.Amber));assets.Controls.Add(Node("Weather / DAQ","Auxiliary telemetry","AVAILABLE",IndustrialTheme.Purple));heartbeat.Text="● DIGITAL TWIN SYNCHRONIZED  •  "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"  •  "+a.Count(x=>x.Enabled)+" active assets";}
  private Control Node(string name,string detail,string state,Color accent){var p=new AccentPanel{Width=300,Height=118,BackColor=Color.White,Padding=new Padding(18),Margin=new Padding(0,0,14,14),Accent=accent};p.Controls.Add(new Label{Text=state,Dock=DockStyle.Bottom,Height=24,ForeColor=accent,Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold)});p.Controls.Add(new Label{Text=detail,Dock=DockStyle.Fill,ForeColor=IndustrialTheme.Muted});p.Controls.Add(new Label{Text=name,Dock=DockStyle.Top,Height=30,ForeColor=IndustrialTheme.Ink,Font=new Font("Segoe UI Semibold",11,FontStyle.Bold)});return p;}
  protected override void Dispose(bool disposing){if(disposing)timer.Dispose();base.Dispose(disposing);}
 }
}
