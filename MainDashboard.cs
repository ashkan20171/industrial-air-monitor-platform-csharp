using System;
using System.Drawing;
using System.Windows.Forms;

namespace AshkanAQMS
{
    public partial class MainDashboard : Form
    {
        private Button _activeButton;

        public MainDashboard()
        {
            InitializeComponent();
            ApplyShellTheme();
        }

        private void MainDashboard_Load(object sender, EventArgs e)
        {
            ShowCurrentDataPage();
        }

        private void LoadPage(UserControl page)
        {
            pnlContent.SuspendLayout();
            try
            {
                pnlContent.Controls.Clear();
                page.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(page);
            }
            finally
            {
                pnlContent.ResumeLayout();
            }
        }

        private void SetActiveButton(Button button)
        {
            if (_activeButton != null)
            {
                // رنگ پس‌زمینه دکمه غیر فعال (رنگ اصلی سایدبار)
                _activeButton.BackColor = Color.FromArgb(31, 42, 54);
                _activeButton.ForeColor = Color.White;
            }

            _activeButton = button;

            if (_activeButton != null)
            {
                // رنگ پس‌زمینه دکمه فعال (رنگ مشخص‌کننده تب فعال)
                _activeButton.BackColor = Color.FromArgb(58, 80, 107);
                _activeButton.ForeColor = Color.White;
            }
        }

        private void ApplyShellTheme()
        {
            BackColor = Color.FromArgb(247, 249, 252);

            pnlSidebar.BackColor = Color.FromArgb(31, 42, 54);
            pnlTopBar.BackColor = Color.White;
            pnlContent.BackColor = Color.FromArgb(247, 249, 252);

            lblTitle.ForeColor = Color.FromArgb(31, 42, 54);
            lblSubtitle.ForeColor = Color.FromArgb(90, 98, 108);

            StyleNavButton(btnCurrentData);
            StyleNavButton(btnAnalyzers);
            StyleNavButton(btnReports);
            StyleNavButton(btnSettings);
        }

        private void StyleNavButton(Button button)
        {
            if (button == null) return;

            button.BackColor = Color.FromArgb(31, 42, 54);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
        }

        private void ShowCurrentDataPage()
        {
            SetActiveButton(btnCurrentData);
            LoadPage(new CurrentDataControl());
        }

        private void ShowAnalyzersPage()
        {
            SetActiveButton(btnAnalyzers);
            LoadPage(new AnalyzerManagementControl());
        }

        private void ShowReportsPage()
        {
            SetActiveButton(btnReports);
            LoadPage(CreatePlaceholderPage("Reports"));
        }

        private void ShowSettingsPage()
        {
            SetActiveButton(btnSettings);
            LoadPage(CreatePlaceholderPage("Settings"));
        }

        private UserControl CreatePlaceholderPage(string title)
        {
            var page = new UserControl
            {
                BackColor = Color.FromArgb(247, 249, 252),
                Dock = DockStyle.Fill
            };

            var label = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 42, 54),
                Location = new Point(40, 40),
                Text = title + " - Coming Soon"
            };

            page.Controls.Add(label);
            return page;
        }

        private void btnCurrentData_Click(object sender, EventArgs e)
        {
            ShowCurrentDataPage();
        }

        private void btnAnalyzers_Click(object sender, EventArgs e)
        {
            ShowAnalyzersPage();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ShowReportsPage();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowSettingsPage();
        }
    }
}
