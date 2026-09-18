using System;
using System.Drawing;
using System.Windows.Forms;

namespace AshkanAQMS.Controls
{
    public class AboutControl : UserControl
    {
        public AboutControl()
        {
            Dock = DockStyle.Fill;
            BackColor = IndustrialTheme.BackgroundDark;
            Padding = new Padding(40);
            BuildUi();
        }

        private void BuildUi()
        {
            var title = new Label { Dock = DockStyle.Top, Height = 55, Text = "AshkanAQMS", Font = new Font("Segoe UI", 28F, FontStyle.Bold), ForeColor = IndustrialTheme.TextPrimary };
            var subtitle = new Label { Dock = DockStyle.Top, Height = 38, Text = "Air Quality Monitoring & Decision Support Platform", Font = new Font("Segoe UI", 12F), ForeColor = IndustrialTheme.TextSecondary };
            Controls.Add(subtitle); Controls.Add(title);

            var card = new Panel { Dock = DockStyle.Top, Height = 220, Margin = new Padding(0, 25, 0, 0), BackColor = IndustrialTheme.SurfaceCard, Padding = new Padding(28) };
            var developer = new Label { Dock = DockStyle.Top, Height = 48, Text = "Developed by  Ashkan Motaei", Font = new Font("Segoe UI", 19F, FontStyle.Bold), ForeColor = Color.White };
            var role = new Label { Dock = DockStyle.Top, Height = 35, Text = "Software Engineer • C# / .NET • Industrial Software", Font = new Font("Segoe UI", 10.5F), ForeColor = IndustrialTheme.TextSecondary };
            var version = new Label { Dock = DockStyle.Top, Height = 32, Text = "Version 2.7.0 • .NET Framework 4.8 • Windows Forms", Font = new Font("Segoe UI", 9.5F), ForeColor = IndustrialTheme.TextSecondary };
            var desc = new Label { Dock = DockStyle.Fill, Text = "A portfolio-grade industrial monitoring application featuring real-time telemetry, AQI calculation, configurable alarms, local analytics, data-quality monitoring, reporting and an operator-focused dashboard.", Font = new Font("Segoe UI", 10F), ForeColor = IndustrialTheme.TextPrimary, Padding = new Padding(0, 10, 0, 0) };
            card.Controls.Add(desc); card.Controls.Add(version); card.Controls.Add(role); card.Controls.Add(developer); Controls.Add(card);

            var tech = new Label { Dock = DockStyle.Top, Height = 100, Text = "TECHNOLOGY & ENGINEERING\r\nC#  •  .NET Framework 4.8  •  WinForms  •  Layered Architecture  •  AI/Analytics  •  CSV/XML Persistence\r\nIndustrial UX  •  Alarm Engine  •  Audit Trail  •  Health Monitoring  •  Executive HTML Reports", Font = new Font("Segoe UI", 10F), ForeColor = IndustrialTheme.TextSecondary, Padding = new Padding(0, 24, 0, 0) };
            Controls.Add(tech);

            var footer = new Label { Dock = DockStyle.Bottom, Height = 40, Text = "© 2026 Ashkan Motaei — AshkanAQMS", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 9F), ForeColor = IndustrialTheme.TextSecondary };
            Controls.Add(footer);
        }
    }
}
