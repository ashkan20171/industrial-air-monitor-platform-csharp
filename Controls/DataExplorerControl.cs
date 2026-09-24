using System;
using System.Drawing;
using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
 public sealed class DataExplorerControl:UserControl
 {
  public DataExplorerControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(22);Build();}
  private void Build(){var h=IndustrialTheme.Heading("Data Explorer & Analytics Workbench",18);h.Dock=DockStyle.Top;h.Height=38;var s=new Label{Text="A portfolio-grade workspace for comparing pollutants, quality, periods and operational context.",Dock=DockStyle.Top,Height=34,ForeColor=IndustrialTheme.Muted};var body=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=2,RowCount=2,Padding=new Padding(0,10,0,0)};body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,68));body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,32));body.RowStyles.Add(new RowStyle(SizeType.Percent,65));body.RowStyles.Add(new RowStyle(SizeType.Percent,35));body.Controls.Add(Box("Interactive trend canvas","Overlay pollutants • zoom/pan • quality markers • thresholds • anomaly context • raw vs aggregate views",IndustrialTheme.Cyan),0,0);body.Controls.Add(Box("Query builder","Time range\nPollutants\nAnalyzer / station\nQuality state\nAggregation\nAlarm context",IndustrialTheme.Blue),1,0);body.Controls.Add(Box("Analytics","Min / Max / Mean • completeness • freshness • anomaly count • rolling statistics • forecast hooks",IndustrialTheme.Purple),0,1);body.Controls.Add(Box("Export & sharing","CSV • executive report • diagnostic snapshot • saved view design",IndustrialTheme.Green),1,1);Controls.Add(body);Controls.Add(s);Controls.Add(h);}
  private Control Box(string title,string text,Color accent){var p=new AccentPanel{Dock=DockStyle.Fill,BackColor=Color.White,Padding=new Padding(18),Margin=new Padding(0,0,14,14),Accent=accent};p.Controls.Add(new Label{Text=text,Dock=DockStyle.Fill,ForeColor=IndustrialTheme.Muted,Font=new Font("Segoe UI",10)});p.Controls.Add(new Label{Text=title,Dock=DockStyle.Top,Height=32,ForeColor=IndustrialTheme.Ink,Font=new Font("Segoe UI Semibold",12,FontStyle.Bold)});return p;}
 }
}
