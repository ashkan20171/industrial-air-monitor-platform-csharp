using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    public sealed class OperationsLogbookControl : UserControl
    {
        private readonly DataGridView grid = new DataGridView();
        private readonly List<Entry> entries = new List<Entry>();
        public OperationsLogbookControl()
        {
            Dock=DockStyle.Fill; BackColor=IndustrialTheme.Canvas; Padding=new Padding(24);
            var header=new Panel{Dock=DockStyle.Top,Height=92,BackColor=IndustrialTheme.Navy800,Padding=new Padding(22)};
            header.Controls.Add(new Label{Text="DIGITAL OPERATIONS LOGBOOK",Dock=DockStyle.Top,Height=30,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",16,FontStyle.Bold)});
            header.Controls.Add(new Label{Text="Shift notes • operator observations • incidents • calibration and maintenance context",Dock=DockStyle.Bottom,Height=24,ForeColor=Color.FromArgb(170,205,220),Font=new Font("Segoe UI",9.5f)});
            var actions=new FlowLayoutPanel{Dock=DockStyle.Top,Height=54,Padding=new Padding(0,10,0,4),BackColor=IndustrialTheme.Canvas};
            var add=ButtonOf("+ NEW ENTRY"); var acknowledge=ButtonOf("ACKNOWLEDGE"); var export=ButtonOf("EXPORT CSV");
            add.Click+=(s,e)=>AddEntry(); acknowledge.Click+=(s,e)=>Acknowledge(); export.Click+=(s,e)=>Export(); actions.Controls.AddRange(new Control[]{add,acknowledge,export});
            grid.Dock=DockStyle.Fill; grid.ReadOnly=true; grid.AllowUserToAddRows=false; grid.SelectionMode=DataGridViewSelectionMode.FullRowSelect; grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill; IndustrialTheme.PolishGrid(grid);
            entries.Add(new Entry{Time=DateTime.Now,Category="SYSTEM",Severity="INFO",Owner=Environment.UserName,Message="Operations logbook ready",Status="OPEN"}); RefreshGrid();
            Controls.Add(grid); Controls.Add(actions); Controls.Add(header);
        }
        private Button ButtonOf(string text){return new Button{Text=text,Width=145,Height=34,FlatStyle=FlatStyle.Flat,BackColor=IndustrialTheme.Navy800,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold),Margin=new Padding(0,0,10,0)};}
        private void AddEntry(){using(var f=new EntryForm()){if(f.ShowDialog(this)==DialogResult.OK){entries.Insert(0,new Entry{Time=DateTime.Now,Category=f.Category,Severity=f.Severity,Owner=Environment.UserName,Message=f.EntryText,Status="OPEN"});RefreshGrid();}}}
        private void Acknowledge(){if(grid.CurrentRow==null)return;var t=grid.CurrentRow.Cells[0].Value?.ToString();var msg=grid.CurrentRow.Cells[4].Value?.ToString();var x=entries.FirstOrDefault(z=>z.Message==msg && z.Time.ToString("yyyy-MM-dd HH:mm:ss")==t);if(x!=null)x.Status="ACKNOWLEDGED";RefreshGrid();}
        private void Export(){using(var d=new SaveFileDialog{Filter="CSV files|*.csv",FileName="operations-logbook.csv"}){if(d.ShowDialog(this)!=DialogResult.OK)return;var s=new StorageService();var rows=entries.Select(x=>new List<string>{x.Time.ToString("s"),x.Category,x.Severity,x.Owner,x.Message,x.Status}).ToList();s.ExportToCsv(new List<string>{"Time","Category","Severity","Owner","Message","Status"},rows,d.FileName);}}
        private void RefreshGrid(){grid.DataSource=null;grid.DataSource=entries.Select(x=>new{Time=x.Time.ToString("yyyy-MM-dd HH:mm:ss"),x.Category,x.Severity,x.Owner,x.Message,x.Status}).ToList();}
        private sealed class Entry{public DateTime Time;public string Category;public string Severity;public string Owner;public string Message;public string Status;}
        private sealed class EntryForm:Form
        {
            private readonly ComboBox category=new ComboBox(),severity=new ComboBox(); private readonly TextBox text=new TextBox(); public string Category=>category.Text;public string Severity=>severity.Text;public string EntryText=>text.Text.Trim();
            public EntryForm(){Text="New operations entry";StartPosition=FormStartPosition.CenterParent;ClientSize=new Size(560,260);BackColor=Color.White;Font=new Font("Segoe UI",9);var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(18),ColumnCount=2,RowCount=4};root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,100));root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));category.DropDownStyle=ComboBoxStyle.DropDownList;category.Items.AddRange(new object[]{"OPERATIONS","ALARM","CALIBRATION","MAINTENANCE","COMMUNICATION","QA"});category.SelectedIndex=0;severity.DropDownStyle=ComboBoxStyle.DropDownList;severity.Items.AddRange(new object[]{"INFO","NOTICE","WARNING","CRITICAL"});severity.SelectedIndex=0;text.Multiline=true;text.Height=100;var ok=new Button{Text="SAVE ENTRY",DialogResult=DialogResult.OK,Width=120,Height=32};root.Controls.Add(new Label{Text="Category",AutoSize=true},0,0);root.Controls.Add(category,1,0);root.Controls.Add(new Label{Text="Severity",AutoSize=true},0,1);root.Controls.Add(severity,1,1);root.Controls.Add(new Label{Text="Entry",AutoSize=true},0,2);root.Controls.Add(text,1,2);root.Controls.Add(ok,1,3);Controls.Add(root);AcceptButton=ok;}
        }
    }
}
