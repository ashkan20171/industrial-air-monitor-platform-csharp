using System.Drawing;
using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
    public sealed class CommunicationLabControl : UserControl
    {
        public CommunicationLabControl(){Dock=DockStyle.Fill;BackColor=Color.FromArgb(239,243,247);Padding=new Padding(24);var split=new SplitContainer{Dock=DockStyle.Fill,SplitterDistance=360};split.Panel1.BackColor=Color.White;split.Panel2.BackColor=Color.FromArgb(17,28,38);split.Panel1.Controls.Add(new Label{Dock=DockStyle.Fill,Padding=new Padding(18),Text="Communication Diagnostics\r\n\r\nSerial/TCP endpoint\r\nBaud / parity / data bits / stop bits\r\nRequest preset\r\nASCII / HEX mode\r\nTimeout and response time\r\n\r\nRaw write operations should only be used by engineering personnel.",Font=new Font("Segoe UI",11),ForeColor=Color.FromArgb(50,70,85)});split.Panel2.Controls.Add(new Label{Dock=DockStyle.Fill,Padding=new Padding(18),Text="TERMINAL / RAW CAPTURE\r\n\r\nNo device is connected by this screen automatically.\r\nUse Analyzer Management to configure endpoints.",Font=new Font("Consolas",11),ForeColor=Color.FromArgb(135,220,180)});Controls.Add(split);}
    }
}
