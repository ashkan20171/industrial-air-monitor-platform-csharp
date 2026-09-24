using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Drivers;
namespace AshkanAQMS.Controls
{
    public sealed class DriverCatalogControl : UserControl
    {
        public DriverCatalogControl()
        {
            Dock=DockStyle.Fill; BackColor=Color.FromArgb(239,243,247); Padding=new Padding(24);
            var note=new Label{Dock=DockStyle.Top,Height=58,Text="Vendor Driver Catalog\r\n'Migration reference' means protocol logic was supplied from the legacy application but still requires physical-device validation before production use.",Font=new Font("Segoe UI",10),ForeColor=Color.FromArgb(70,85,98)};
            var grid=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,RowHeadersVisible=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,BackgroundColor=Color.White,BorderStyle=BorderStyle.None};
            grid.DataSource=AnalyzerDriverCatalog.Items.Select(x=>new {x.Manufacturer,Driver=x.DisplayName,x.Transport,Status=x.ValidationStatus,x.Notes}).ToList(); Controls.Add(grid); Controls.Add(note);
        }
    }
}
