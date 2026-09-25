using System;
using System.Drawing;
using System.Windows.Forms;

namespace AshkanAQMS.Controls
{
    public sealed class MultiStationCommandCenterControl : UserControl
    {
        private readonly DataGridView _grid = new DataGridView();
        public MultiStationCommandCenterControl()
        {
            Dock=DockStyle.Fill; BackColor=IndustrialTheme.Canvas; Padding=new Padding(22);
            var header=Header("MULTI-STATION COMMAND CENTER","Fleet-of-stations overview for distributed AQMS operations • deployment-ready UI surface");
            var kpis=new TableLayoutPanel{Dock=DockStyle.Top,Height=104,ColumnCount=5,Padding=new Padding(0,10,0,10)};
            for(int i=0;i<5;i++)kpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,20));
            kpis.Controls.Add(Kpi("STATIONS","04","Configured sites"),0,0); kpis.Controls.Add(Kpi("ONLINE","03","Connected"),1,0);
            kpis.Controls.Add(Kpi("ANALYZERS","18","Fleet assets"),2,0); kpis.Controls.Add(Kpi("OPEN ALARMS","02","Requires review"),3,0); kpis.Controls.Add(Kpi("DATA COMPLETENESS","98.4%","24-hour window"),4,0);
            _grid.Dock=DockStyle.Fill; _grid.ReadOnly=true; _grid.AllowUserToAddRows=false; _grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill; IndustrialTheme.PolishGrid(_grid);
            _grid.DataSource=new[]{
                new {Station="Station A",Region="Primary",State="ONLINE",Analyzers="6 / 6",AQI="--",Completeness="--",LastSync="Awaiting telemetry"},
                new {Station="Station B",Region="Remote North",State="CONFIGURED",Analyzers="4",AQI="--",Completeness="--",LastSync="Not connected"},
                new {Station="Station C",Region="Remote East",State="CONFIGURED",Analyzers="5",AQI="--",Completeness="--",LastSync="Not connected"},
                new {Station="Station D",Region="Mobile Lab",State="CONFIGURED",Analyzers="3",AQI="--",Completeness="--",LastSync="Not connected"}};
            var note=new Label{Dock=DockStyle.Bottom,Height=44,Padding=new Padding(12),BackColor=Color.White,ForeColor=IndustrialTheme.Muted,Text="REAL-DATA RULE  •  Remote values remain blank until a station transport/API is configured; the command center does not fabricate production telemetry."};
            Controls.Add(_grid);Controls.Add(note);Controls.Add(kpis);Controls.Add(header);
        }
        static Panel Header(string title,string sub){var p=new Panel{Dock=DockStyle.Top,Height=82,BackColor=IndustrialTheme.Navy800,Padding=new Padding(20,14,20,8)};p.Controls.Add(new Label{Text=title,Dock=DockStyle.Top,Height=31,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",16,FontStyle.Bold)});p.Controls.Add(new Label{Text=sub,Dock=DockStyle.Bottom,Height=24,ForeColor=Color.FromArgb(175,210,224)});return p;}
        static Panel Kpi(string title,string value,string sub){var p=new Panel{Dock=DockStyle.Fill,BackColor=Color.White,Margin=new Padding(5),Padding=new Padding(12)};p.Controls.Add(new Label{Text=sub,Dock=DockStyle.Bottom,Height=18,ForeColor=IndustrialTheme.Muted});p.Controls.Add(new Label{Text=value,Dock=DockStyle.Fill,ForeColor=IndustrialTheme.Navy700,Font=new Font("Segoe UI Semibold",19,FontStyle.Bold),TextAlign=ContentAlignment.MiddleLeft});p.Controls.Add(new Label{Text=title,Dock=DockStyle.Top,Height=18,ForeColor=IndustrialTheme.Muted,Font=new Font("Segoe UI",8.5f,FontStyle.Bold)});return p;}
    }
}
