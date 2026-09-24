using System.Drawing;
using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
    public sealed class CalibrationCenterControl : UserControl
    {
        public CalibrationCenterControl()
        {
            Dock=DockStyle.Fill;BackColor=Color.FromArgb(239,243,247);Padding=new Padding(24);
            var layout=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=2,RowCount=2}; layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));layout.RowStyles.Add(new RowStyle(SizeType.Percent,50));layout.RowStyles.Add(new RowStyle(SizeType.Percent,50));
            layout.Controls.Add(Card("Calibration workflow","Pre-check → Zero → Stabilize → Span → Verify → Complete\r\nCommands must be validated for the selected physical analyzer before use."),0,0);
            layout.Controls.Add(Card("Safety interlocks","• Analyzer must be enabled and online\r\n• Operator confirmation required\r\n• Remote session and calibration are mutually exclusive\r\n• Abort reason is audited"),1,0);
            layout.Controls.Add(Card("Live stabilization","Reserved for live reference/current value chart, elapsed time, deviation and stability indicator."),0,1);
            layout.Controls.Add(Card("Calibration history","Calibration sessions, operator, zero/span references, result, abort/failure reason and audit correlation."),1,1); Controls.Add(layout);
        }
        private Control Card(string h,string t){var p=new Panel{Dock=DockStyle.Fill,BackColor=Color.White,Margin=new Padding(8),Padding=new Padding(18)};p.Controls.Add(new Label{Text=t,Dock=DockStyle.Fill,Font=new Font("Segoe UI",10),ForeColor=Color.FromArgb(70,85,98)});p.Controls.Add(new Label{Text=h,Dock=DockStyle.Top,Height=36,Font=new Font("Segoe UI",14,FontStyle.Bold),ForeColor=Color.FromArgb(20,76,112)});return p;}
    }
}
