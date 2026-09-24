using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    public sealed class OperationsPlannerControl : UserControl
    {
        private readonly StorageService storage = new StorageService();
        private readonly DataGridView grid = new DataGridView();
        private readonly List<WorkItem> items = new List<WorkItem>();
        public OperationsPlannerControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);Build();}
        private void Build(){
            var title=IndustrialTheme.Heading("Operations Planner & Work Orders",18);title.Dock=DockStyle.Top;title.Height=38;
            var sub=new Label{Text="Preventive maintenance, calibration due dates, inspections and accountable work queues.",Dock=DockStyle.Top,Height=34,ForeColor=IndustrialTheme.Muted};
            var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=52,Padding=new Padding(0,6,0,6)};
            var add=IndustrialTheme.Action("+ New work order");var complete=IndustrialTheme.Action("Mark completed");var generate=IndustrialTheme.Action("Generate PM plan");
            add.Click+=(s,e)=>AddManual();complete.Click+=(s,e)=>CompleteSelected();generate.Click+=(s,e)=>GeneratePlan();bar.Controls.AddRange(new Control[]{add,complete,generate});
            grid.Dock=DockStyle.Fill;grid.ReadOnly=true;grid.AllowUserToAddRows=false;grid.SelectionMode=DataGridViewSelectionMode.FullRowSelect;grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;IndustrialTheme.PolishGrid(grid);
            Controls.Add(grid);Controls.Add(bar);Controls.Add(sub);Controls.Add(title);GeneratePlan();
        }
        private void GeneratePlan(){items.Clear();var analyzers=storage.LoadAnalyzers();int i=1;foreach(var a in analyzers){items.Add(new WorkItem{Id="WO-"+(1000+i++),Asset=a.Name,Type="Preventive inspection",Priority=a.Enabled?"Normal":"Low",Due=DateTime.Today.AddDays(i%7+1),Owner="Instrumentation",Status="Open"});if(a.Enabled)items.Add(new WorkItem{Id="WO-"+(1000+i++),Asset=a.Name,Type="Calibration readiness",Priority="High",Due=DateTime.Today.AddDays(i%4+1),Owner="QA / Calibration",Status="Open"});}if(items.Count==0)items.Add(new WorkItem{Id="WO-1001",Asset="Station",Type="Daily station inspection",Priority="Normal",Due=DateTime.Today.AddDays(1),Owner="Operator",Status="Open"});Bind();}
        private void AddManual(){items.Insert(0,new WorkItem{Id="WO-"+DateTime.Now.ToString("HHmmss"),Asset="Station",Type="Operator follow-up",Priority="Normal",Due=DateTime.Today.AddDays(1),Owner="Operator",Status="Open"});Bind();}
        private void CompleteSelected(){if(grid.CurrentRow==null)return;var id=Convert.ToString(grid.CurrentRow.Cells[0].Value);var x=items.FirstOrDefault(z=>z.Id==id);if(x!=null)x.Status="Completed";Bind();}
        private void Bind(){grid.DataSource=null;grid.DataSource=items.Select(x=>new{x.Id,x.Asset,x.Type,x.Priority,Due=x.Due.ToString("yyyy-MM-dd"),x.Owner,x.Status}).ToList();}
        private sealed class WorkItem{public string Id{get;set;}public string Asset{get;set;}public string Type{get;set;}public string Priority{get;set;}public DateTime Due{get;set;}public string Owner{get;set;}public string Status{get;set;}}
    }
}
