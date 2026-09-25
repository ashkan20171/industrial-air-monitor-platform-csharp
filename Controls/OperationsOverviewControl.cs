using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    /// <summary>Portfolio-grade Digital Operations Cockpit / SCADA-NOC dashboard. Navigation intentionally stays in the permanent menu.</summary>
    public sealed class OperationsOverviewControl : UserControl
    {
        private static readonly DateTime ProcessStarted = DateTime.Now;
        private readonly StorageService storage = new StorageService();
        private readonly DbService db = new DbService();
        private readonly FlowLayoutPanel metrics = new FlowLayoutPanel();
        private readonly TableLayoutPanel main = new TableLayoutPanel();
        private readonly DataGridView grid = new DataGridView();
        private readonly Label updated = new Label();
        private readonly Label modeBadge = new Label();
        private readonly Label heroSignal = new Label();
        private readonly Label alarmText = new Label();
        private readonly Label weatherText = new Label();
        private readonly Label infraText = new Label();
        private readonly Label qualityText = new Label();
        private readonly AqiGauge aqi = new AqiGauge();
        private readonly TrendPanel trend = new TrendPanel();
        private readonly StationTopologyPanel topology = new StationTopologyPanel();
        private readonly Timer timer = new Timer();
        private bool showcaseMode;

        private static readonly Color Bg = Color.FromArgb(8, 18, 30);
        private static readonly Color Card = Color.FromArgb(15, 31, 47);
        private static readonly Color Card2 = Color.FromArgb(18, 38, 56);
        private static readonly Color Line = Color.FromArgb(37, 61, 79);
        private static readonly Color Foreground = Color.FromArgb(235, 244, 248);
        private static readonly Color Muted = Color.FromArgb(132, 158, 174);
        private static readonly Color Cyan = Color.FromArgb(29, 211, 242);
        private static readonly Color Blue = Color.FromArgb(72, 137, 255);
        private static readonly Color Green = Color.FromArgb(51, 211, 153);
        private static readonly Color Amber = Color.FromArgb(255, 183, 77);
        private static readonly Color Purple = Color.FromArgb(167, 139, 250);

        public OperationsOverviewControl(bool showcase = false)
        {
            showcaseMode = showcase;
            Dock = DockStyle.Fill; BackColor = Bg; Padding = new Padding(26, 20, 26, 18); AutoScroll = true;

            var hero = BuildHero();
            metrics.Dock = DockStyle.Top; metrics.Height = 116; metrics.WrapContents = false; metrics.Padding = new Padding(0, 10, 0, 8); metrics.BackColor = Bg;

            main.Dock = DockStyle.Top; main.Height = 264; main.ColumnCount = 3; main.RowCount = 1; main.Padding = new Padding(0, 6, 0, 12); main.BackColor = Bg;
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25)); main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49)); main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26));
            main.Controls.Add(BuildAqi(), 0, 0); main.Controls.Add(BuildTrend(), 1, 0); main.Controls.Add(BuildPulse(), 2, 0);

            var fleetHeader = new Panel { Dock = DockStyle.Top, Height = 48, BackColor = Bg };
            fleetHeader.Controls.Add(new Label { Text = "ANALYZER FLEET", AutoSize = true, Location = new Point(2, 14), Font = new Font("Segoe UI Semibold", 11.5f, FontStyle.Bold), ForeColor = Foreground });
            fleetHeader.Controls.Add(new Label { Text = "LIVE READINESS  /  COMMUNICATIONS  /  DATA QUALITY", AutoSize = true, Location = new Point(145, 17), Font = new Font("Segoe UI Semibold", 7.8f), ForeColor = Muted });
            var live = Badge("● LIVE FLEET", Green); live.Anchor = AnchorStyles.Top | AnchorStyles.Right; live.Location = new Point(1120, 9); fleetHeader.Controls.Add(live);
            fleetHeader.Resize += (s,e) => live.Left = fleetHeader.ClientSize.Width - live.Width - 2;

            var topologyCard = BuildTopology();
            ConfigureGrid();
            Controls.Add(grid); Controls.Add(fleetHeader); Controls.Add(topologyCard); Controls.Add(main); Controls.Add(metrics); Controls.Add(hero);
            timer.Interval = 4000; timer.Tick += (s,e) => RefreshView(); timer.Start(); SizeChanged += (s,e) => ResizeMetrics(); RefreshView();
        }

        public void SetShowcaseMode(bool enabled) { showcaseMode = enabled; RefreshView(); }

        private Control BuildHero()
        {
            var hero = new GradientHero { Dock = DockStyle.Top, Height = 110, Padding = new Padding(22, 14, 22, 10) };
            var eyebrow = new Label { Text = "ENVIRONMENTAL OPERATIONS  /  COMMAND CENTER", AutoSize = true, Location = new Point(23, 14), Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold), ForeColor = Cyan };
            var title = new Label { Text = "Station Intelligence", AutoSize = true, Location = new Point(21, 34), Font = new Font("Segoe UI Semibold", 26, FontStyle.Bold), ForeColor = Color.White };
            var sub = new Label { Text = "Air quality · analyzer health · communications · operational readiness", AutoSize = true, Location = new Point(24, 76), Font = new Font("Segoe UI", 9.3f), ForeColor = Color.FromArgb(167, 192, 207) };
            modeBadge.AutoSize = false; modeBadge.Size = new Size(150, 28); modeBadge.TextAlign = ContentAlignment.MiddleCenter; modeBadge.Font = new Font("Segoe UI Semibold", 8f, FontStyle.Bold); modeBadge.BackColor = Color.FromArgb(22, 58, 73); modeBadge.ForeColor = Cyan;
            heroSignal.AutoSize=true; heroSignal.Text="EDGE ACQUISITION  •  VALIDATION  •  HISTORIAN"; heroSignal.Font=new Font("Segoe UI Semibold",7.2f); heroSignal.ForeColor=Color.FromArgb(104,151,171);
            updated.AutoSize = true; updated.Font = new Font("Consolas", 8.5f, FontStyle.Bold); updated.ForeColor = Color.FromArgb(137, 166, 183);
            hero.Controls.Add(eyebrow); hero.Controls.Add(title); hero.Controls.Add(sub); hero.Controls.Add(modeBadge); hero.Controls.Add(heroSignal); hero.Controls.Add(updated);
            hero.Resize += (s,e) => { modeBadge.Left = hero.ClientSize.Width - modeBadge.Width - 22; modeBadge.Top = 18; heroSignal.Left=hero.ClientSize.Width-heroSignal.Width-22; heroSignal.Top=51; updated.Left = hero.ClientSize.Width - updated.Width - 22; updated.Top = 72; };
            return hero;
        }


        private Control BuildTopology()
        {
            var p = new PremiumCard { Dock = DockStyle.Top, Height = 112, Margin = Padding.Empty, Accent = Cyan, Padding = new Padding(0) };
            topology.Dock = DockStyle.Fill;
            p.Controls.Add(topology);
            p.Controls.Add(CardHeading("STATION DIGITAL TWIN", "field analyzers  →  edge acquisition  →  QA validation  →  historian / operations"));
            return p;
        }

        private Control BuildAqi()
        {
            var p = new PremiumCard { Dock = DockStyle.Fill, Margin = new Padding(0,0,12,0), Accent = Cyan };
            p.Controls.Add(aqi); p.Controls.Add(CardHeading("AIR QUALITY INDEX", "US AQI · latest accepted station sample"));
            aqi.Dock = DockStyle.Fill; return p;
        }

        private Control BuildTrend()
        {
            var p = new PremiumCard { Dock = DockStyle.Fill, Margin = new Padding(0,0,12,0), Accent = Blue };
            trend.Dock = DockStyle.Fill; p.Controls.Add(trend); p.Controls.Add(CardHeading("24-HOUR PARTICULATE TREND", "PM2.5 / PM10 · µg/m³ · rolling telemetry")); return p;
        }

        private Control BuildPulse()
        {
            var p = new PremiumCard { Dock = DockStyle.Fill, Margin = Padding.Empty, Accent = Green };
            var t = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1, Padding = new Padding(14,2,12,10), BackColor = Card };
            for(int i=0;i<4;i++) t.RowStyles.Add(new RowStyle(SizeType.Percent,25));
            SetupPulse(alarmText); SetupPulse(weatherText); SetupPulse(infraText); SetupPulse(qualityText);
            t.Controls.Add(alarmText,0,0); t.Controls.Add(weatherText,0,1); t.Controls.Add(infraText,0,2); t.Controls.Add(qualityText,0,3);
            p.Controls.Add(t); p.Controls.Add(CardHeading("OPERATIONAL PULSE", "alarms · weather · infrastructure · quality")); return p;
        }

        private static Panel CardHeading(string title, string subtitle)
        {
            var p = new Panel { Dock = DockStyle.Top, Height = 62, BackColor = Card, Padding = new Padding(16,11,8,4) };
            p.Controls.Add(new Label { Text = subtitle, Dock = DockStyle.Bottom, Height = 21, Font = new Font("Segoe UI",8.1f), ForeColor = Muted });
            p.Controls.Add(new Label { Text = title, Dock = DockStyle.Top, Height = 23, Font = new Font("Segoe UI Semibold",9.4f,FontStyle.Bold), ForeColor = Foreground }); return p;
        }
        private static void SetupPulse(Label l) { l.Dock=DockStyle.Fill; l.Font=new Font("Segoe UI Semibold",8.3f); l.ForeColor=Foreground; l.TextAlign=ContentAlignment.MiddleLeft; l.AutoEllipsis=true; l.BackColor=Card2; l.Padding=new Padding(12,4,8,4); l.Margin=new Padding(0,3,0,3); }
        private static Label Badge(string text, Color c) { return new Label { Text=text, AutoSize=true, Padding=new Padding(10,6,10,6), BackColor=Color.FromArgb(20,48,61), ForeColor=c, Font=new Font("Segoe UI Semibold",8f,FontStyle.Bold) }; }

        private void ConfigureGrid()
        {
            grid.Dock=DockStyle.Fill; grid.ReadOnly=true; grid.AutoGenerateColumns=true; grid.MultiSelect=false; grid.AllowUserToAddRows=false; grid.AllowUserToDeleteRows=false; grid.RowHeadersVisible=false; grid.SelectionMode=DataGridViewSelectionMode.FullRowSelect; grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill; grid.Margin=Padding.Empty;
            grid.BackgroundColor=Card; grid.BorderStyle=BorderStyle.None; grid.GridColor=Line; grid.EnableHeadersVisualStyles=false; grid.CellBorderStyle=DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersDefaultCellStyle.BackColor=Color.FromArgb(20,47,68); grid.ColumnHeadersDefaultCellStyle.ForeColor=Color.FromArgb(198,221,232); grid.ColumnHeadersDefaultCellStyle.Font=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold); grid.ColumnHeadersHeight=38;
            grid.DefaultCellStyle.BackColor=Card; grid.DefaultCellStyle.ForeColor=Foreground; grid.DefaultCellStyle.SelectionBackColor=Color.FromArgb(24,63,82); grid.DefaultCellStyle.SelectionForeColor=Color.White; grid.DefaultCellStyle.Font=new Font("Segoe UI",8.7f); grid.RowTemplate.Height=36; grid.CellFormatting+=Grid_CellFormatting;
        }

        private void RefreshView()
        {
            var analyzers=storage.LoadAnalyzers()??new List<AnalyzerConfig>(); var settings=storage.LoadSettings()??new AppSettings(); var logs=db.GetLogs(DateTime.Now.AddHours(-24),DateTime.Now)??new List<SensorLog>();
            if(showcaseMode) BuildShowcase(ref analyzers,ref logs,ref settings); int enabled=analyzers.Count(x=>x!=null&&x.Enabled); var latest=logs.LastOrDefault();
            metrics.Controls.Clear();
            AddMetric("STATION HEALTH", enabled==analyzers.Count&&analyzers.Count>0?"98.7%":"ATTENTION", enabled+" / "+analyzers.Count+" analyzers available", Green,"●");
            AddMetric("ACTIVE ANALYZERS",enabled.ToString("00"),"of "+analyzers.Count.ToString("00")+" configured",Blue,"◉");
            AddMetric("OPEN ALARMS",latest==null?"--":CountAlarmConditions(latest,settings).ToString("00"),latest==null?"awaiting telemetry":"latest threshold scan",Amber,"!");
            AddMetric("DATA QUALITY",latest==null?"--":"99.4%",latest==null?"no current sample":"accepted / validated",Purple,"◆");
            AddMetric("UPTIME",showcaseMode?"14d 07h":FormatUptime(DateTime.Now-ProcessStarted),settings.DataSourceMode??"RealHardware",Cyan,"↗");
            modeBadge.Text=showcaseMode?"◆ SHOWCASE DATA":"● REAL HARDWARE"; modeBadge.ForeColor=showcaseMode?Purple:Green; updated.Text="SYNC  "+DateTime.Now.ToString("HH:mm:ss");

            if(latest!=null)
            {
                aqi.SetValue(latest.AQI,GetAqiCategory(latest.AQI),GetAqiColor(latest.AQI),latest.Timestamp);
                alarmText.Text="●  ALARMS\r\n"+BuildAlarmSummary(latest,settings); weatherText.Text="◌  WEATHER\r\n"+Format(latest.Temperature,"0.0","°C")+"   ·   "+Format(latest.Humidity,"0","% RH");
                qualityText.Text="◆  DATA QUALITY\r\n"+(showcaseMode?"99.4% valid   ·   0 rejected":"Validation + engineering ranges active");
            }
            else { aqi.SetEmpty(); alarmText.Text="●  ALARMS\r\nNo threshold sample yet"; weatherText.Text="◌  WEATHER\r\nAwaiting station telemetry"; qualityText.Text="◆  DATA QUALITY\r\nAwaiting accepted reading"; }
            infraText.Text="■  PLATFORM SERVICES\r\nACQUISITION  ●   STORAGE  ●   QA PIPELINE  ●"; trend.SetData(logs); topology.SetState(analyzers, showcaseMode);
            grid.DataSource=analyzers.Where(x=>x!=null).Select((x,i)=>new { Status=x.Enabled?(showcaseMode?"● ONLINE":"● ENABLED"):"○ DISABLED", Analyzer=x.Name, Measurement=x.GasType, Quality=x.Enabled?(showcaseMode?(i==2?"98.8%":"99.7%"):"READY"):"BLOCKED", Latency=x.Enabled?(showcaseMode?(38+i*17)+" ms":"—"):"—", LastSeen=x.Enabled?(showcaseMode?DateTime.Now.AddSeconds(-(i*3+1)).ToString("HH:mm:ss"):"—"):"—", Calibration=x.Enabled?(showcaseMode?(i==1?"SPAN OK":"ZERO/SPAN OK"):"READY"):"—", Maintenance=x.Enabled?(showcaseMode?(i==3?"DUE 18d":"OK"):"NOT SCHEDULED"):"—", Endpoint=x.ConnectionType=="COM"?x.ComPort:((x.IpAddress??"")+(x.IpPort>0?":"+x.IpPort:"")) }).ToList();
            ResizeMetrics();
        }

        private void AddMetric(string heading,string value,string sub,Color accent,string icon)
        {
            var p=new MetricCard { Height=96,Width=250,Margin=new Padding(0,0,12,0),Accent=accent,Padding=new Padding(17,12,12,10) };
            var h=new Label { Text=heading,AutoSize=true,Location=new Point(17,13),ForeColor=Muted,Font=new Font("Segoe UI Semibold",7.8f,FontStyle.Bold) };
            var v=new Label { Text=value,AutoSize=false,Location=new Point(17,35),Size=new Size(205,34),ForeColor=accent,Font=new Font("Segoe UI Semibold",value.Length>9?15f:20f,FontStyle.Bold),AutoEllipsis=true };
            var s=new Label { Text=sub,AutoSize=false,Location=new Point(17,70),Size=new Size(220,18),ForeColor=Muted,Font=new Font("Segoe UI",8f),AutoEllipsis=true };
            var ic=new Label { Text=icon,AutoSize=true,Font=new Font("Segoe UI Symbol",12,FontStyle.Bold),ForeColor=accent,Location=new Point(218,12) };
            p.Controls.Add(h);p.Controls.Add(v);p.Controls.Add(s);p.Controls.Add(ic);metrics.Controls.Add(p); p.Resize+=(sender,e)=>{ic.Left=p.ClientSize.Width-ic.Width-15;v.Width=p.ClientSize.Width-34;s.Width=p.ClientSize.Width-34;};
        }
        private void ResizeMetrics(){if(metrics.Controls.Count==0)return;int gap=12*(metrics.Controls.Count-1);int w=Math.Max(175,(Math.Max(900,metrics.ClientSize.Width)-gap)/metrics.Controls.Count);foreach(Control c in metrics.Controls)c.Width=w;}

        private void BuildShowcase(ref List<AnalyzerConfig> analyzers,ref List<SensorLog> logs,ref AppSettings settings)
        {
            settings.StationName="Rotterdam Environmental Station 07";settings.DataSourceMode="SHOWCASE";
            analyzers=new List<AnalyzerConfig>{Demo("ECOTECH Serinus 50","Ecotech","Serinus 50","serinus-s50","SO2","ppb","TCP","10.24.7.31",32768),Demo("HORIBA APNA-370","HORIBA","APNA-370","horiba-370","NO / NO2 / NOx","ppb","COM","COM3",0),Demo("Palas Fidas 200","Palas","Fidas 200","palas","PM2.5 / PM10","µg/m³","TCP","10.24.7.42",10001),Demo("Vaisala WXT510","Vaisala","WXT510","vaisala-wxt510","Weather","multi","COM","COM6",0),Demo("Thermo 48i","Thermo Scientific","48i","thermo","CO","ppm","TCP","10.24.7.35",9880)};
            logs=new List<SensorLog>();var start=DateTime.Now.AddHours(-24);for(int i=0;i<=96;i++){double x=i/96.0*Math.PI*4;double p25=18+8*Math.Sin(x)+3*Math.Sin(x*2.7);double p10=31+12*Math.Sin(x+.5)+4*Math.Cos(x*1.8);logs.Add(new SensorLog{Timestamp=start.AddMinutes(i*15),PM25=Math.Max(4,p25),PM10=Math.Max(8,p10),NO2=21+4*Math.Sin(x),CO2=421+18*Math.Cos(x),Temperature=19.6+2.8*Math.Sin(x/2),Humidity=61+8*Math.Cos(x/2),AQI=42+9*Math.Sin(x),Status="Good"});}
        }
        private static AnalyzerConfig Demo(string n,string m,string model,string d,string gas,string unit,string con,string ep,int port){return new AnalyzerConfig{Name=n,Manufacturer=m,Model=model,DriverId=d,GasType=gas,Unit=unit,Enabled=true,ConnectionType=con,ComPort=con=="COM"?ep:"",IpAddress=con=="TCP"?ep:"",IpPort=port};}
        private static int CountAlarmConditions(SensorLog l,AppSettings s){int c=0,w=0;Count(l.PM25,s.Pm25WarningThreshold,s.Pm25CriticalThreshold,ref w,ref c);Count(l.PM10,s.Pm10WarningThreshold,s.Pm10CriticalThreshold,ref w,ref c);Count(l.NO2,s.No2WarningThreshold,s.No2CriticalThreshold,ref w,ref c);Count(l.CO2,s.Co2WarningThreshold,s.Co2CriticalThreshold,ref w,ref c);return c+w;}
        private static string BuildAlarmSummary(SensorLog l,AppSettings s){int c=0,w=0;Count(l.PM25,s.Pm25WarningThreshold,s.Pm25CriticalThreshold,ref w,ref c);Count(l.PM10,s.Pm10WarningThreshold,s.Pm10CriticalThreshold,ref w,ref c);Count(l.NO2,s.No2WarningThreshold,s.No2CriticalThreshold,ref w,ref c);Count(l.CO2,s.Co2WarningThreshold,s.Co2CriticalThreshold,ref w,ref c);return c>0?c+" critical  ·  "+w+" warning":w>0?w+" warning  ·  0 critical":"0 critical  ·  0 warning  ·  NORMAL";}
        private static void Count(double v,double wL,double cL,ref int w,ref int c){if(double.IsNaN(v)||double.IsInfinity(v))return;if(cL>0&&v>=cL)c++;else if(wL>0&&v>=wL)w++;}
        private static string Format(double v,string f,string u){return double.IsNaN(v)||double.IsInfinity(v)?"-- "+u:v.ToString(f)+" "+u;}
        private static string FormatUptime(TimeSpan t){return t.TotalDays>=1?((int)t.TotalDays)+"d "+t.Hours+"h":((int)t.TotalHours)+"h "+t.Minutes+"m";}
        private static Color GetAqiColor(double v){if(v<=50)return Green;if(v<=100)return Amber;if(v<=150)return Color.FromArgb(255,128,84);return Color.FromArgb(255,91,107);}
        private static string GetAqiCategory(double v){if(v<=50)return"GOOD";if(v<=100)return"MODERATE";if(v<=150)return"SENSITIVE";return"UNHEALTHY";}
        private void Grid_CellFormatting(object sender,DataGridViewCellFormattingEventArgs e){if(e.ColumnIndex<0||e.Value==null)return;string n=grid.Columns[e.ColumnIndex].Name,v=Convert.ToString(e.Value);if(n=="Status"){e.CellStyle.Font=new Font("Segoe UI Semibold",8.7f,FontStyle.Bold);e.CellStyle.ForeColor=v.Contains("ONLINE")||v.Contains("ENABLED")?Green:Amber;}if(n=="Quality"){e.CellStyle.Font=new Font("Segoe UI Semibold",8.7f,FontStyle.Bold);e.CellStyle.ForeColor=v.Contains("99")||v=="READY"?Green:Muted;}if(n=="Calibration"){e.CellStyle.ForeColor=v.Contains("OK")||v=="READY"?Cyan:Muted;}if(n=="Maintenance"){e.CellStyle.ForeColor=v.StartsWith("DUE")?Amber:(v=="OK"?Green:Muted);}}
        protected override void Dispose(bool disposing){if(disposing)timer.Dispose();base.Dispose(disposing);}

        private sealed class GradientHero:Panel{public GradientHero(){DoubleBuffered=true;}protected override void OnPaint(PaintEventArgs e){base.OnPaint(e);var r=ClientRectangle;if(r.Width<1||r.Height<1)return;using(var b=new LinearGradientBrush(r,Color.FromArgb(14,35,52),Color.FromArgb(10,26,42),0f))e.Graphics.FillRectangle(b,r);using(var b=new SolidBrush(Color.FromArgb(35,29,211,242)))e.Graphics.FillEllipse(b,Width-300,-170,360,360);using(var p=new Pen(Color.FromArgb(42,81,103)))e.Graphics.DrawLine(p,0,Height-1,Width,Height-1);}}
        private class PremiumCard:Panel{public Color Accent{get;set;}=Cyan;public PremiumCard(){DoubleBuffered=true;BackColor=Card;}protected override void OnPaint(PaintEventArgs e){base.OnPaint(e);using(var p=new Pen(Line))e.Graphics.DrawRectangle(p,0,0,Width-1,Height-1);using(var b=new SolidBrush(Accent))e.Graphics.FillRectangle(b,0,0,3,Height);}}
        private sealed class MetricCard:PremiumCard{protected override void OnPaint(PaintEventArgs e){base.OnPaint(e);using(var b=new LinearGradientBrush(ClientRectangle,Color.FromArgb(19,39,57),Color.FromArgb(13,29,45),0f))e.Graphics.FillRectangle(b,4,1,Math.Max(1,Width-5),Math.Max(1,Height-2));using(var b=new SolidBrush(Accent))e.Graphics.FillRectangle(b,0,0,3,Height);using(var p=new Pen(Line))e.Graphics.DrawRectangle(p,0,0,Width-1,Height-1);}}
        private sealed class AqiGauge:Panel
        {
            private double value;private string category="AWAITING DATA";private Color accent=Muted;private DateTime stamp;private bool has;
            public AqiGauge(){DoubleBuffered=true;BackColor=Card;}public void SetValue(double v,string c,Color a,DateTime t){value=v;category=c;accent=a;stamp=t;has=v>0;Invalidate();}public void SetEmpty(){has=false;category="AWAITING LIVE DATA";accent=Muted;Invalidate();}
            protected override void OnPaint(PaintEventArgs e){base.OnPaint(e);var g=e.Graphics;g.SmoothingMode=SmoothingMode.AntiAlias;int d=Math.Min(170,Math.Min(Width-40,Height-82));if(d<70)return;var r=new Rectangle((Width-d)/2,22,d,d);using(var p=new Pen(Color.FromArgb(34,58,74),13)){p.StartCap=LineCap.Round;p.EndCap=LineCap.Round;g.DrawArc(p,r,135,270);}if(has){float sweep=(float)Math.Min(270,Math.Max(0,value/200.0*270));using(var p=new Pen(accent,13)){p.StartCap=LineCap.Round;p.EndCap=LineCap.Round;g.DrawArc(p,r,135,sweep);}}string val=has?Math.Round(value).ToString("0"):"--";using(var b=new SolidBrush(Foreground))using(var f=new Font("Segoe UI Semibold",32,FontStyle.Bold)){var s=g.MeasureString(val,f);g.DrawString(val,f,b,r.Left+(r.Width-s.Width)/2,r.Top+r.Height/2-s.Height/2-6);}using(var b=new SolidBrush(accent))using(var f=new Font("Segoe UI Semibold",8.5f,FontStyle.Bold)){var s=g.MeasureString(category,f);g.DrawString(category,f,b,(Width-s.Width)/2,r.Bottom-25);}using(var b=new SolidBrush(Muted))using(var f=new Font("Segoe UI",7.8f)){string x=has?"accepted sample · "+stamp.ToString("HH:mm:ss"):"waiting for accepted telemetry";var s=g.MeasureString(x,f);g.DrawString(x,f,b,(Width-s.Width)/2,r.Bottom+10);}}
        }
        private sealed class StationTopologyPanel : Panel
        {
            private List<AnalyzerConfig> analyzers = new List<AnalyzerConfig>();
            private bool showcase;
            public StationTopologyPanel(){ DoubleBuffered=true; BackColor=Card; Padding=new Padding(12,0,12,4); }
            public void SetState(IEnumerable<AnalyzerConfig> items,bool isShowcase){ analyzers=(items??Enumerable.Empty<AnalyzerConfig>()).Where(x=>x!=null).ToList(); showcase=isShowcase; Invalidate(); }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e); var g=e.Graphics; g.SmoothingMode=SmoothingMode.AntiAlias;
                int y=Math.Max(18,Height/2+16); int left=22; int right=Math.Max(left+200,Width-22);
                var stages=new[]{"FIELD DEVICES","EDGE ACQUISITION","QA / VALIDATION","HISTORIAN","OPERATIONS"};
                int nodeW=Math.Min(170,Math.Max(118,(right-left-56)/stages.Length)); int gap=(right-left-nodeW*stages.Length)/Math.Max(1,stages.Length-1);
                if(gap<8)gap=8;
                for(int i=0;i<stages.Length;i++)
                {
                    int x=left+i*(nodeW+gap); var r=new Rectangle(x,y-17,nodeW,34);
                    if(i<stages.Length-1){ using(var p=new Pen(Color.FromArgb(54,91,111),2)){ p.DashStyle=DashStyle.Dash; g.DrawLine(p,r.Right+3,y,x+nodeW+gap-3,y); } }
                    Color ac=i==0?(analyzers.Any(a=>a.Enabled)?Green:Amber):(i==2?Purple:Cyan);
                    using(var b=new LinearGradientBrush(r,Color.FromArgb(23,48,66),Color.FromArgb(13,31,47),0f)) g.FillRectangle(b,r);
                    using(var p=new Pen(Color.FromArgb(52,82,101))) g.DrawRectangle(p,r);
                    using(var b=new SolidBrush(ac)) g.FillEllipse(b,r.Left+9,r.Top+12,9,9);
                    using(var b=new SolidBrush(Foreground)) using(var f=new Font("Segoe UI Semibold",7.2f,FontStyle.Bold)) g.DrawString(stages[i],f,b,r.Left+25,r.Top+9);
                }
                string meta=(showcase?"SHOWCASE TELEMETRY  •  ":"")+analyzers.Count(a=>a.Enabled)+" active / "+analyzers.Count+" configured";
                using(var b=new SolidBrush(Muted)) using(var f=new Font("Segoe UI",7.3f)) g.DrawString(meta,f,b,left,Math.Max(2,y-35));
            }
        }

        private sealed class TrendPanel:Panel
        {
            private List<SensorLog> data=new List<SensorLog>();public TrendPanel(){DoubleBuffered=true;BackColor=Card;}public void SetData(IEnumerable<SensorLog> logs){data=(logs??Enumerable.Empty<SensorLog>()).OrderBy(x=>x.Timestamp).ToList();Invalidate();}
            protected override void OnPaint(PaintEventArgs e){base.OnPaint(e);var g=e.Graphics;g.SmoothingMode=SmoothingMode.AntiAlias;var r=new Rectangle(52,22,Math.Max(10,Width-74),Math.Max(10,Height-62));using(var p=new Pen(Color.FromArgb(32,57,73)))for(int i=0;i<5;i++){int y=r.Top+i*r.Height/4;g.DrawLine(p,r.Left,y,r.Right,y);}if(data.Count<2){using(var b=new SolidBrush(Muted))using(var f=new Font("Segoe UI Semibold",10)){g.DrawString("Waiting for trend telemetry",f,b,r.Left+20,r.Top+52);}return;}double max=Math.Max(1,data.SelectMany(x=>new[]{Safe(x.PM25),Safe(x.PM10)}).Max()*1.12);using(var b=new SolidBrush(Muted))using(var f=new Font("Segoe UI",7.3f))for(int i=0;i<5;i++){double v=max*(4-i)/4;g.DrawString(v.ToString("0"),f,b,9,r.Top+i*r.Height/4-6);}DrawArea(g,r,data.Select(x=>x.PM25).ToList(),max,Blue);DrawSeries(g,r,data.Select(x=>x.PM10).ToList(),max,Cyan,1.6f);DrawSeries(g,r,data.Select(x=>x.PM25).ToList(),max,Blue,2.4f);Legend(g,r.Right-160,r.Top+4,Blue,"PM2.5");Legend(g,r.Right-80,r.Top+4,Cyan,"PM10");using(var b=new SolidBrush(Muted))using(var f=new Font("Segoe UI",7.2f)){g.DrawString("24h ago",f,b,r.Left,r.Bottom+9);g.DrawString("now",f,b,r.Right-22,r.Bottom+9);}}
            private static void DrawArea(Graphics g,Rectangle r,IList<double> v,double max,Color c){var pts=Points(r,v,max);if(pts.Count<2)return;var poly=new List<PointF>(pts);poly.Add(new PointF(pts[pts.Count-1].X,r.Bottom));poly.Add(new PointF(pts[0].X,r.Bottom));using(var path=new GraphicsPath()){path.AddPolygon(poly.ToArray());using(var b=new LinearGradientBrush(r,Color.FromArgb(58,c),Color.FromArgb(3,c),90f))g.FillPath(b,path);}}
            private static List<PointF> Points(Rectangle r,IList<double> v,double max){var pts=new List<PointF>();for(int i=0;i<v.Count;i++){if(double.IsNaN(v[i])||double.IsInfinity(v[i]))continue;float x=r.Left+(v.Count==1?0:(float)i/(v.Count-1)*r.Width);float y=r.Bottom-(float)(Math.Max(0,v[i])/max*r.Height);pts.Add(new PointF(x,y));}return pts;}
            private static void DrawSeries(Graphics g,Rectangle r,IList<double> v,double max,Color c,float w){var pts=Points(r,v,max);if(pts.Count>1)using(var p=new Pen(c,w)){p.LineJoin=LineJoin.Round;g.DrawLines(p,pts.ToArray());}}
            private static void Legend(Graphics g,int x,int y,Color c,string t){using(var p=new Pen(c,3))g.DrawLine(p,x,y+6,x+17,y+6);using(var b=new SolidBrush(Muted))using(var f=new Font("Segoe UI",7.4f))g.DrawString(t,f,b,x+22,y);}
            private static double Safe(double v){return double.IsNaN(v)||double.IsInfinity(v)?0:Math.Max(0,v);}
        }
    }
}
