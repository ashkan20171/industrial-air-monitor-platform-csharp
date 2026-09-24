using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
 public sealed class IncidentCenterControl:UserControl
 {
  private readonly List<Incident> incidents=new List<Incident>(); private readonly DataGridView grid=new DataGridView();
  public IncidentCenterControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);Build();Seed();}
  private void Build(){var h=IndustrialTheme.Heading("Incident & Shift Handover Center",18);h.Dock=DockStyle.Top;h.Height=38;var s=new Label{Text="Operational incidents, ownership, acknowledgement and shift-to-shift continuity.",Dock=DockStyle.Top,Height=34,ForeColor=IndustrialTheme.Muted};var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=52,Padding=new Padding(0,6,0,6)};var add=IndustrialTheme.Action("+ Log incident");var ack=IndustrialTheme.Action("Acknowledge");var resolve=IndustrialTheme.Action("Resolve");add.Click+=(x,y)=>{incidents.Insert(0,new Incident{Id="INC-"+DateTime.Now.ToString("HHmmss"),Opened=DateTime.Now,Severity="Medium",Area="Station",Summary="Operator-created follow-up",Owner="Current shift",Status="Open"});Bind();};ack.Click+=(x,y)=>Set("Acknowledged");resolve.Click+=(x,y)=>Set("Resolved");bar.Controls.AddRange(new Control[]{add,ack,resolve});grid.Dock=DockStyle.Fill;grid.ReadOnly=true;grid.AllowUserToAddRows=false;grid.SelectionMode=DataGridViewSelectionMode.FullRowSelect;grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;IndustrialTheme.PolishGrid(grid);Controls.Add(grid);Controls.Add(bar);Controls.Add(s);Controls.Add(h);}
  private void Seed(){incidents.Add(new Incident{Id="INC-DEMO-01",Opened=DateTime.Now.AddMinutes(-18),Severity="Low",Area="Communications",Summary="Demonstration handover item",Owner="Instrumentation",Status="Open"});Bind();}
  private void Set(string state){if(grid.CurrentRow==null)return;var id=Convert.ToString(grid.CurrentRow.Cells[0].Value);var x=incidents.FirstOrDefault(i=>i.Id==id);if(x!=null)x.Status=state;Bind();}
  private void Bind(){grid.DataSource=null;grid.DataSource=incidents.Select(x=>new{x.Id,Opened=x.Opened.ToString("yyyy-MM-dd HH:mm"),x.Severity,x.Area,x.Summary,x.Owner,x.Status}).ToList();}
  private sealed class Incident{public string Id{get;set;}public DateTime Opened{get;set;}public string Severity{get;set;}public string Area{get;set;}public string Summary{get;set;}public string Owner{get;set;}public string Status{get;set;}}
 }
}
