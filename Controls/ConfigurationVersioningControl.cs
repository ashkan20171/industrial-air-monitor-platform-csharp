using System;using System.Drawing;using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
 public sealed class ConfigurationVersioningControl:UserControl
 {
  public ConfigurationVersioningControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);var h=new Panel{Dock=DockStyle.Top,Height=84,BackColor=IndustrialTheme.Navy800,Padding=new Padding(20)};h.Controls.Add(new Label{Text="CONFIGURATION VERSIONING & CHANGE CONTROL",Dock=DockStyle.Top,Height=30,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",16,FontStyle.Bold)});h.Controls.Add(new Label{Text="Trace analyzer configuration changes, approvals, rollback points and engineering evidence",Dock=DockStyle.Bottom,Height=22,ForeColor=Color.FromArgb(175,210,224)});
   var split=new SplitContainer{Dock=DockStyle.Fill,SplitterDistance=540,BackColor=IndustrialTheme.Border};var history=Grid();history.DataSource=new[]{new{Version="v16.0",Scope="Station",Change="Reliability & Driver Lab",Author="Engineering",Status="Current"},new{Version="v15.0",Scope="Operations",Change="Mission Control",Author="Engineering",Status="Baseline"},new{Version="v14.0",Scope="Dashboard",Change="Fleet Intelligence",Author="Engineering",Status="Archived"}};split.Panel1.Controls.Add(history);
   var diff=new RichTextBox{Dock=DockStyle.Fill,ReadOnly=true,BorderStyle=BorderStyle.None,BackColor=Color.FromArgb(15,29,40),ForeColor=Color.FromArgb(220,232,238),Font=new Font("Consolas",10),Text="CONFIGURATION DIFF PREVIEW\n\n+ Analyzer engineering range validation\n+ Alarm escalation policy\n+ Driver packet inspection profile\n~ Acquisition governance metadata\n\nRollback requires operator confirmation and an audit entry.\n\nNo production configuration is changed from this preview."};split.Panel2.Controls.Add(diff);Controls.Add(split);Controls.Add(h);}
  static DataGridView Grid(){var g=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill};IndustrialTheme.PolishGrid(g);return g;}
 }
}
