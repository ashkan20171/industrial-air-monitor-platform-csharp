using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AshkanAQMS.Controls
{
    public sealed class AlarmConsoleControl : UserControl
    {
        private readonly DataGridView grid = new DataGridView();
        private readonly List<AlarmRow> alarms = new List<AlarmRow>();
        private readonly Label summary = new Label();
        public AlarmConsoleControl()
        {
            Dock=DockStyle.Fill; BackColor=IndustrialTheme.Canvas; Padding=new Padding(24);
            var header=new Panel{Dock=DockStyle.Top,Height=96,BackColor=IndustrialTheme.Navy800,Padding=new Padding(22)};
            header.Controls.Add(new Label{Text="ALARM COMMAND CONSOLE",Dock=DockStyle.Top,Height=32,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",17,FontStyle.Bold)});
            summary.Text="No active operational alarms • acknowledgement and shelving workspace"; summary.Dock=DockStyle.Bottom; summary.Height=25; summary.ForeColor=Color.FromArgb(175,210,224); summary.Font=new Font("Segoe UI",9.5f); header.Controls.Add(summary);
            var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=54,Padding=new Padding(0,10,0,4),BackColor=IndustrialTheme.Canvas};
            var ack=Btn("ACKNOWLEDGE"); var shelve=Btn("SHELVE 15 MIN"); var close=Btn("CLOSE / CLEAR"); var inject=Btn("ADD TEST ALARM");
            ack.Click+=(s,e)=>SetState("ACKNOWLEDGED",null); shelve.Click+=(s,e)=>SetState("SHELVED",DateTime.Now.AddMinutes(15)); close.Click+=(s,e)=>SetState("CLOSED",null); inject.Click+=(s,e)=>AddTest();
            bar.Controls.AddRange(new Control[]{ack,shelve,close,inject});
            grid.Dock=DockStyle.Fill; grid.ReadOnly=true; grid.AllowUserToAddRows=false; grid.SelectionMode=DataGridViewSelectionMode.FullRowSelect; grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill; IndustrialTheme.PolishGrid(grid);
            RefreshGrid(); Controls.Add(grid); Controls.Add(bar); Controls.Add(header);
        }
        private Button Btn(string t){var b=new Button{Text=t,Width=145,Height=34,FlatStyle=FlatStyle.Flat,BackColor=IndustrialTheme.Navy800,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold),Margin=new Padding(0,0,10,0)};b.FlatAppearance.BorderColor=Color.FromArgb(48,77,94);return b;}
        private void AddTest(){alarms.Insert(0,new AlarmRow{Id="ALM-"+(alarms.Count+1).ToString("000"),Time=DateTime.Now,Severity=alarms.Count%2==0?"HIGH":"WARNING",Source="Analyzer Fleet",Message="Demonstration alarm for operator workflow",State="ACTIVE",Owner="—"});RefreshGrid();}
        private void SetState(string state,DateTime? until){if(grid.CurrentRow==null)return;var id=Convert.ToString(grid.CurrentRow.Cells[0].Value);var a=alarms.FirstOrDefault(x=>x.Id==id);if(a==null)return;a.State=state+(until.HasValue?" until "+until.Value.ToString("HH:mm"):"");a.Owner=Environment.UserName;RefreshGrid();}
        private void RefreshGrid(){grid.DataSource=null;grid.DataSource=alarms.Select(a=>new{a.Id,Time=a.Time.ToString("yyyy-MM-dd HH:mm:ss"),a.Severity,a.Source,a.Message,a.State,a.Owner}).ToList();summary.Text=alarms.Count(x=>x.State.StartsWith("ACTIVE"))+" active • "+alarms.Count(x=>x.State.StartsWith("ACK"))+" acknowledged • "+alarms.Count(x=>x.State.StartsWith("SHELVED"))+" shelved";}
        private sealed class AlarmRow{public string Id;public DateTime Time;public string Severity;public string Source;public string Message;public string State;public string Owner;}
    }
}
