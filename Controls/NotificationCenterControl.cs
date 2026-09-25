using System;
using System.Drawing;
using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
 public sealed class NotificationCenterControl:UserControl
 {
  readonly DataGridView grid=new DataGridView();
  public NotificationCenterControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);
   var h=new Panel{Dock=DockStyle.Top,Height=84,BackColor=IndustrialTheme.Navy800,Padding=new Padding(20)};h.Controls.Add(new Label{Text="NOTIFICATION & ESCALATION CENTER",Dock=DockStyle.Top,Height=30,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",16,FontStyle.Bold)});h.Controls.Add(new Label{Text="Routing policy for alarms, analyzer health, calibration and maintenance events",Dock=DockStyle.Bottom,Height=22,ForeColor=Color.FromArgb(175,210,224)});
   var actions=new FlowLayoutPanel{Dock=DockStyle.Top,Height=58,BackColor=Color.White,Padding=new Padding(10)};actions.Controls.Add(IndustrialTheme.Action("+ Add routing rule"));actions.Controls.Add(IndustrialTheme.Action("Test channel"));actions.Controls.Add(IndustrialTheme.Action("Escalation policy"));
   grid.Dock=DockStyle.Fill;grid.ReadOnly=true;grid.AllowUserToAddRows=false;grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;IndustrialTheme.PolishGrid(grid);grid.DataSource=new[]{new{Event="Critical analyzer offline",Severity="Critical",Channel="Email / Webhook",Delay="Immediate",Escalation="15 min",Enabled="Yes"},new{Event="Calibration due",Severity="Warning",Channel="Email",Delay="24 h",Escalation="Supervisor",Enabled="Yes"},new{Event="Data completeness < 95%",Severity="Warning",Channel="Operations",Delay="5 min",Escalation="30 min",Enabled="Yes"},new{Event="Maintenance overdue",Severity="Advisory",Channel="Operations",Delay="Daily",Escalation="Engineer",Enabled="Yes"}};
   var note=new Label{Dock=DockStyle.Bottom,Height=44,Padding=new Padding(12),BackColor=Color.White,ForeColor=IndustrialTheme.Muted,Text="INTEGRATION NOTE  •  Channel delivery is an integration surface; production SMTP/webhook credentials must be configured securely before deployment."};Controls.Add(grid);Controls.Add(note);Controls.Add(actions);Controls.Add(h);}
 }
}
