using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Services;
namespace AshkanAQMS.Controls
{
 public sealed class FleetHealthControl:UserControl
 {
  public FleetHealthControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(24);var h=IndustrialTheme.Heading("Analyzer Fleet Health",20);h.Dock=DockStyle.Top;h.Height=45;var sub=new Label{Dock=DockStyle.Top,Height=35,Text="Operational view of enablement, transport, endpoint, validation and acquisition health.",ForeColor=IndustrialTheme.Muted,Font=new Font("Segoe UI",9.5f)};var g=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill};IndustrialTheme.PolishGrid(g);var s=new StorageService();g.DataSource=s.LoadAnalyzers().Select(x=>new{x.Name,x.Manufacturer,x.Model,Driver=x.DriverId,State=x.Enabled?"READY":"DISABLED",Transport=x.ConnectionType,Endpoint=x.ConnectionType=="COM"?x.ComPort:x.IpAddress+":"+x.IpPort,x.GasType,x.Unit}).ToList();Controls.Add(g);Controls.Add(sub);Controls.Add(h);}
 }
}
