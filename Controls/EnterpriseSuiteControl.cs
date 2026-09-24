using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Drivers;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    public sealed class EnterpriseSuiteControl : UserControl
    {
        private readonly StorageService storage = new StorageService();
        public EnterpriseSuiteControl()
        {
            Dock=DockStyle.Fill; BackColor=IndustrialTheme.Canvas; Padding=new Padding(24);
            var scroll=new Panel{Dock=DockStyle.Fill,AutoScroll=true,BackColor=IndustrialTheme.Canvas}; Controls.Add(scroll);
            var root=new TableLayoutPanel{Dock=DockStyle.Top,AutoSize=true,ColumnCount=1,BackColor=IndustrialTheme.Canvas}; root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100)); scroll.Controls.Add(root);
            root.Controls.Add(Hero()); root.Controls.Add(Kpis()); root.Controls.Add(Capabilities()); root.Controls.Add(DriverMatrix()); root.Controls.Add(Roadmap());
        }
        private Control Hero()
        {
            var p=new Panel{Height=170,Dock=DockStyle.Top,BackColor=IndustrialTheme.Navy800,Padding=new Padding(28),Margin=new Padding(0,0,0,16)};
            var badge=new Label{Text="INDUSTRIAL AIR QUALITY OPERATIONS PLATFORM",AutoSize=true,ForeColor=Color.FromArgb(119,218,226),Font=new Font("Segoe UI Semibold",9,FontStyle.Bold),Location=new Point(28,25)};
            var h=new Label{Text="Ashkan AQMS Enterprise",AutoSize=true,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",28,FontStyle.Bold),Location=new Point(25,49)};
            var s=new Label{Text="Real-hardware acquisition • analyzer fleet health • calibration • alarms • weather • DAQ • remote operations • analytics",AutoSize=true,ForeColor=Color.FromArgb(194,213,224),Font=new Font("Segoe UI",10.5f),Location=new Point(29,101)};
            var chip=new Label{Text="  Safety-first acquisition boundary  ",AutoSize=true,BackColor=Color.FromArgb(24,74,87),ForeColor=Color.FromArgb(168,238,218),Font=new Font("Segoe UI Semibold",9),Location=new Point(29,130),Padding=new Padding(5)};
            p.Controls.AddRange(new Control[]{badge,h,s,chip}); return p;
        }
        private Control Kpis()
        {
            var a=storage.LoadAnalyzers(); var enabled=a.Count(x=>x.Enabled); var t=new TableLayoutPanel{Height=118,Dock=DockStyle.Top,ColumnCount=4,Margin=new Padding(0,0,0,16)};
            for(int i=0;i<4;i++)t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,25));
            t.Controls.Add(Kpi("ANALYZER FLEET",a.Count.ToString(),"configured devices",IndustrialTheme.Blue),0,0);
            t.Controls.Add(Kpi("ACQUISITION READY",enabled.ToString(),"enabled for polling",IndustrialTheme.Green),1,0);
            t.Controls.Add(Kpi("DRIVER LIBRARY",AnalyzerDriverCatalog.Items.Count.ToString(),"protocol families",IndustrialTheme.Cyan),2,0);
            t.Controls.Add(Kpi("SAFETY BOUNDARY","2×","pre-poll + pre-commit checks",IndustrialTheme.Purple),3,0); return t;
        }
        private Control Kpi(string cap,string value,string sub,Color accent)
        {
            var p=new AccentPanel{Dock=DockStyle.Fill,BackColor=Color.White,Margin=new Padding(0,0,12,0),Padding=new Padding(18),Accent=accent};
            p.Controls.Add(new Label{Text=sub,Dock=DockStyle.Bottom,Height=22,ForeColor=IndustrialTheme.Muted,Font=new Font("Segoe UI",8.8f)});
            p.Controls.Add(new Label{Text=value,Dock=DockStyle.Fill,ForeColor=IndustrialTheme.Ink,Font=new Font("Segoe UI Semibold",24,FontStyle.Bold)});
            p.Controls.Add(new Label{Text=cap,Dock=DockStyle.Top,Height=22,ForeColor=accent,Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold)}); return p;
        }
        private Control Capabilities()
        {
            var wrap=new FlowLayoutPanel{Dock=DockStyle.Top,AutoSize=true,WrapContents=true,BackColor=IndustrialTheme.Canvas,Margin=new Padding(0,0,0,16)};
            string[] items={"Live AQI Command Center|Real-time pollutants, quality flags and alarms","Fleet Health Matrix|Latency, last success, failures and communication state","Calibration Center|Zero/span workflow, interlocks and traceability","Trend Intelligence|Historical charts, aggregates, export and executive reports","Industrial I/O|DAQ digital/analog channels, UPS and auxiliary sensors","Weather Intelligence|Vaisala, Vantage and Delta OHM station support","Remote Operations|Supervised analyzer remote sessions and command audit","Protocol Lab|Serial/TCP diagnostics, raw frames and troubleshooting","Data Quality Engine|Range, freshness, parsing and validity classification","Alarm Lifecycle|Acknowledge-ready event model and operational history","Configuration Security|Protected credentials, validation, backup and restore","Presentation Mode|Portfolio-ready full-screen operations experience","Digital Twin|Visual station topology and synchronized asset state","Operations Planner|Work orders, PM scheduling and calibration readiness","Incident Center|Acknowledgement, ownership and shift handover","Integration Hub|OPC UA, MQTT, REST, Modbus and edge architecture","Data Explorer|Cross-pollutant analytics and quality-aware investigations"};
            foreach(var x in items){var parts=x.Split('|');var c=new AccentPanel{Width=335,Height=105,BackColor=Color.White,Padding=new Padding(17),Margin=new Padding(0,0,12,12),Accent=IndustrialTheme.Cyan};c.Controls.Add(new Label{Text=parts[1],Dock=DockStyle.Fill,ForeColor=IndustrialTheme.Muted,Font=new Font("Segoe UI",9)});c.Controls.Add(new Label{Text=parts[0],Dock=DockStyle.Top,Height=28,ForeColor=IndustrialTheme.Ink,Font=new Font("Segoe UI Semibold",11,FontStyle.Bold)});wrap.Controls.Add(c);} return wrap;
        }
        private Control DriverMatrix()
        {
            var p=new Panel{Dock=DockStyle.Top,Height=310,BackColor=Color.White,Padding=new Padding(18),Margin=new Padding(0,0,0,16)};
            var h=IndustrialTheme.Heading("Analyzer & transport coverage",14);h.Dock=DockStyle.Top;h.Height=32;
            var grid=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill}; IndustrialTheme.PolishGrid(grid);
            grid.DataSource=AnalyzerDriverCatalog.Items.Select(x=>new{x.Manufacturer,Analyzer=x.DisplayName,x.Transport,Status=x.ValidationStatus}).ToList(); p.Controls.Add(grid);p.Controls.Add(h);return p;
        }
        private Control Roadmap()
        {
            var p=new AccentPanel{Dock=DockStyle.Top,Height=115,BackColor=Color.White,Padding=new Padding(18),Margin=new Padding(0,0,0,24),Accent=IndustrialTheme.Amber};
            p.Controls.Add(new Label{Dock=DockStyle.Fill,Text="Hardware validation remains explicit: migrated vendor protocols are retained as reference source and catalogued separately until tested against physical analyzers. This avoids presenting unverified hardware behavior as production-certified.",ForeColor=IndustrialTheme.Muted,Font=new Font("Segoe UI",9.2f)});
            p.Controls.Add(new Label{Dock=DockStyle.Top,Height=30,Text="Engineering integrity",ForeColor=IndustrialTheme.Ink,Font=new Font("Segoe UI Semibold",12,FontStyle.Bold)});return p;
        }
    }
}
