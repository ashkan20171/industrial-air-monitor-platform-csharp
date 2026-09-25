using System;using System.Drawing;using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
 public sealed class CalibrationEvidenceControl:UserControl
 {
  public CalibrationEvidenceControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);var h=new Panel{Dock=DockStyle.Top,Height=84,BackColor=IndustrialTheme.Navy800,Padding=new Padding(20)};h.Controls.Add(new Label{Text="CALIBRATION EVIDENCE & CERTIFICATE CENTER",Dock=DockStyle.Top,Height=30,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",16,FontStyle.Bold)});h.Controls.Add(new Label{Text="Zero/span evidence, reference standards, operator sign-off and certificate workflow",Dock=DockStyle.Bottom,Height=22,ForeColor=Color.FromArgb(175,210,224)});
   var actions=new FlowLayoutPanel{Dock=DockStyle.Top,Height=58,BackColor=Color.White,Padding=new Padding(10)};actions.Controls.Add(IndustrialTheme.Action("New evidence package"));actions.Controls.Add(IndustrialTheme.Action("Review selected"));actions.Controls.Add(IndustrialTheme.Action("Export certificate draft"));
   var g=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill};IndustrialTheme.PolishGrid(g);g.DataSource=new[]{new{Analyzer="No completed calibration",Pollutant="--",Type="--",Reference="--",Operator="--",Result="Awaiting evidence",Certificate="--"}};
   var n=new Label{Dock=DockStyle.Bottom,Height=44,Padding=new Padding(12),BackColor=Color.White,ForeColor=IndustrialTheme.Muted,Text="TRACEABILITY  •  Certificates are evidence documents only after a completed, reviewed calibration. No certification claim is generated automatically."};Controls.Add(g);Controls.Add(n);Controls.Add(actions);Controls.Add(h);}
 }
}
