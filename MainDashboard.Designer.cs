using System.Drawing;
using System.Windows.Forms;

namespace AshkanAQMS
{
    partial class MainDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnFeatures = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnAnalyzers = new System.Windows.Forms.Button();
            this.btnCurrentData = new System.Windows.Forms.Button();
            this.btnDiagnostics = new System.Windows.Forms.Button();
            this.btnAlarms = new System.Windows.Forms.Button();
            this.btnRemote = new System.Windows.Forms.Button();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            this.pnlTopBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(31, 42, 54);
            this.pnlSidebar.Controls.Add(this.btnAbout);
            this.pnlSidebar.Controls.Add(this.btnDiagnostics);
            this.pnlSidebar.Controls.Add(this.btnAlarms);
            this.pnlSidebar.Controls.Add(this.btnRemote);
            this.pnlSidebar.Controls.Add(this.btnSettings);
            this.pnlSidebar.Controls.Add(this.btnFeatures);
            this.pnlSidebar.Controls.Add(this.btnReports);
            this.pnlSidebar.Controls.Add(this.btnAnalyzers);
            this.pnlSidebar.Controls.Add(this.btnCurrentData);
            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(230, 760);
            this.pnlSidebar.TabIndex = 0;
            // 
            // btnRemote
            this.btnRemote.FlatAppearance.BorderSize = 0;
            this.btnRemote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemote.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRemote.ForeColor = System.Drawing.Color.White;
            this.btnRemote.Location = new System.Drawing.Point(12, 370);
            this.btnRemote.Name = "btnRemote";
            this.btnRemote.Size = new System.Drawing.Size(206, 48);
            this.btnRemote.TabIndex = 10;
            this.btnRemote.Text = "Remote Operations";
            this.btnRemote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemote.UseVisualStyleBackColor = true;
            this.btnRemote.Click += new System.EventHandler(this.btnRemote_Click);

            // btnAlarms
            this.btnAlarms.FlatAppearance.BorderSize = 0;
            this.btnAlarms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlarms.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnAlarms.ForeColor = System.Drawing.Color.White;
            this.btnAlarms.Location = new System.Drawing.Point(12, 424);
            this.btnAlarms.Name = "btnAlarms";
            this.btnAlarms.Size = new System.Drawing.Size(206, 48);
            this.btnAlarms.TabIndex = 6;
            this.btnAlarms.Text = "Alarm History";
            this.btnAlarms.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAlarms.UseVisualStyleBackColor = true;
            this.btnAlarms.Click += new System.EventHandler(this.btnAlarms_Click);
            //
            // btnDiagnostics
            //
            this.btnDiagnostics.FlatAppearance.BorderSize = 0;
            this.btnDiagnostics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiagnostics.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDiagnostics.ForeColor = System.Drawing.Color.White;
            this.btnDiagnostics.Location = new System.Drawing.Point(12, 478);
            this.btnDiagnostics.Name = "btnDiagnostics";
            this.btnDiagnostics.Size = new System.Drawing.Size(206, 48);
            this.btnDiagnostics.TabIndex = 8;
            this.btnDiagnostics.Text = "System Diagnostics";
            this.btnDiagnostics.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDiagnostics.UseVisualStyleBackColor = true;
            this.btnDiagnostics.Click += new System.EventHandler(this.btnDiagnostics_Click);
            //
            // btnFeatures
            // 
            this.btnFeatures.FlatAppearance.BorderSize = 0;
            this.btnFeatures.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFeatures.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnFeatures.ForeColor = System.Drawing.Color.White;
            this.btnFeatures.Location = new System.Drawing.Point(12, 316);
            this.btnFeatures.Name = "btnFeatures";
            this.btnFeatures.Size = new System.Drawing.Size(206, 48);
            this.btnFeatures.TabIndex = 5;
            this.btnFeatures.Text = "Feature Center";
            this.btnFeatures.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFeatures.UseVisualStyleBackColor = true;
            this.btnFeatures.Click += new System.EventHandler(this.btnFeatures_Click);
            // 
            // btnAbout
            this.btnAbout.FlatAppearance.BorderSize = 0;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAbout.ForeColor = System.Drawing.Color.White;
            this.btnAbout.Location = new System.Drawing.Point(12, 532);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(206, 48);
            this.btnAbout.TabIndex = 9;
            this.btnAbout.Text = "About";
            this.btnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);

            // btnSettings
            // 
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(12, 262);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(206, 48);
            this.btnSettings.TabIndex = 4;
            this.btnSettings.Text = "Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnReports
            // 
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.Location = new System.Drawing.Point(12, 208);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(206, 48);
            this.btnReports.TabIndex = 3;
            this.btnReports.Text = "Reports";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnAnalyzers
            // 
            this.btnAnalyzers.FlatAppearance.BorderSize = 0;
            this.btnAnalyzers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyzers.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAnalyzers.ForeColor = System.Drawing.Color.White;
            this.btnAnalyzers.Location = new System.Drawing.Point(12, 154);
            this.btnAnalyzers.Name = "btnAnalyzers";
            this.btnAnalyzers.Size = new System.Drawing.Size(206, 48);
            this.btnAnalyzers.TabIndex = 2;
            this.btnAnalyzers.Text = "Analyzers";
            this.btnAnalyzers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAnalyzers.UseVisualStyleBackColor = true;
            this.btnAnalyzers.Click += new System.EventHandler(this.btnAnalyzers_Click);
            // 
            // btnCurrentData
            // 
            this.btnCurrentData.FlatAppearance.BorderSize = 0;
            this.btnCurrentData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCurrentData.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCurrentData.ForeColor = System.Drawing.Color.White;
            this.btnCurrentData.Location = new System.Drawing.Point(12, 100);
            this.btnCurrentData.Name = "btnCurrentData";
            this.btnCurrentData.Size = new System.Drawing.Size(206, 48);
            this.btnCurrentData.TabIndex = 1;
            this.btnCurrentData.Text = "Current Data";
            this.btnCurrentData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCurrentData.UseVisualStyleBackColor = true;
            this.btnCurrentData.Click += new System.EventHandler(this.btnCurrentData_Click);
            // 
            // lblLogo
            // 
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(20, 20);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(180, 40);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "Ashkan AQMS";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.White;
            this.pnlTopBar.Controls.Add(this.lblSubtitle);
            this.pnlTopBar.Controls.Add(this.lblTitle);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(230, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(1170, 80);
            this.pnlTopBar.TabIndex = 1;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(90, 98, 108);
            this.lblSubtitle.Location = new System.Drawing.Point(26, 48);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(323, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Real-time monitoring and analyzer management";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 42, 54);
            this.lblTitle.Location = new System.Drawing.Point(24, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(379, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Air Quality Monitoring Dashboard";
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(230, 80);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1170, 680);
            this.pnlContent.TabIndex = 2;
            // 
            // MainDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            this.ClientSize = new System.Drawing.Size(1400, 760);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.pnlSidebar);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "MainDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ashkan AQMS Pro";
            this.Load += new System.EventHandler(this.MainDashboard_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Button btnCurrentData;
        private System.Windows.Forms.Button btnAnalyzers;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnFeatures;
        private System.Windows.Forms.Button btnAlarms;
        private System.Windows.Forms.Button btnRemote;
        private System.Windows.Forms.Button btnDiagnostics;
        private System.Windows.Forms.Label lblLogo;
    }
}
