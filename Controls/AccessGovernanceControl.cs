using System.Drawing;
using System.Windows.Forms;

namespace AshkanAQMS.Controls
{
    public sealed class AccessGovernanceControl : UserControl
    {
        public AccessGovernanceControl()
        {
            Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(24);
            var header=new Panel{Dock=DockStyle.Top,Height=96,BackColor=IndustrialTheme.Navy800,Padding=new Padding(22)};
            header.Controls.Add(new Label{Text="ACCESS GOVERNANCE & RBAC BLUEPRINT",Dock=DockStyle.Top,Height=32,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",17,FontStyle.Bold)});
            header.Controls.Add(new Label{Text="Least-privilege role matrix for operations, engineering, calibration and audit workflows",Dock=DockStyle.Bottom,Height=25,ForeColor=Color.FromArgb(175,210,224)});
            var grid=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill};IndustrialTheme.PolishGrid(grid);
            grid.DataSource=new[]{
                new {Role="Administrator",Dashboard="View",Configuration="Manage",Calibration="Approve",Remote="Control",Alarms="Manage",Audit="View"},
                new {Role="Engineer",Dashboard="View",Configuration="Manage",Calibration="Execute",Remote="Control",Alarms="Acknowledge",Audit="View"},
                new {Role="Operator",Dashboard="View",Configuration="Read",Calibration="Read",Remote="Restricted",Alarms="Acknowledge",Audit="Read"},
                new {Role="QA / Auditor",Dashboard="View",Configuration="Read",Calibration="Review",Remote="None",Alarms="Review",Audit="Export"},
                new {Role="Viewer",Dashboard="View",Configuration="None",Calibration="None",Remote="None",Alarms="Read",Audit="None"}};
            var note=new Label{Dock=DockStyle.Bottom,Height=52,Padding=new Padding(12),Text="SECURITY NOTE  •  This screen documents the RBAC model. Authentication/enforcement must be integrated with the deployment identity provider before production use.",ForeColor=Color.FromArgb(115,135,145),BackColor=Color.White};
            Controls.Add(grid);Controls.Add(note);Controls.Add(header);
        }
    }
}
