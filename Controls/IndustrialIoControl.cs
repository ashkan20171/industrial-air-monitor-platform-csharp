using System.Drawing;
using System.Windows.Forms;
namespace AshkanAQMS.Controls
{
    public sealed class IndustrialIoControl : UserControl
    {
        public IndustrialIoControl()
        {
            Dock=DockStyle.Fill;BackColor=Color.FromArgb(239,243,247);Padding=new Padding(24);
            var tabs=new TabControl{Dock=DockStyle.Fill}; tabs.TabPages.Add(Page("Digital Inputs","8-channel DI matrix • read-only status • last transition • quality")); tabs.TabPages.Add(Page("Digital Outputs","Outputs are read-only by default. Entering control mode should require permission, confirmation and an audit event.")); tabs.TabPages.Add(Page("Analog / Sensors","Analog voltage, engineering value, unit, gain, drift, pin mapping and active state.")); tabs.TabPages.Add(Page("Weather / UPS","Weather station and UPS telemetry are represented as mapped measurement channels with communication health.")); Controls.Add(tabs);
        }
        private TabPage Page(string n,string text){var p=new TabPage(n){BackColor=Color.White};p.Controls.Add(new Label{Dock=DockStyle.Fill,TextAlign=ContentAlignment.MiddleCenter,Text=text,Font=new Font("Segoe UI",12),ForeColor=Color.FromArgb(55,75,90)});return p;}
    }
}
