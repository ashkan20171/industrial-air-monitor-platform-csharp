namespace AshkanAQMS.Controls
{
    partial class ReportsControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlFilterHeader;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel pnlStatsSummary;
        private System.Windows.Forms.Label lblAvgAqi;
        private System.Windows.Forms.Label lblMaxAqi;
        private System.Windows.Forms.Label lblMinAqi;
        private System.Windows.Forms.Label lblTotalLogs;

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
            this.pnlFilterHeader = new System.Windows.Forms.Panel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.pnlStatsSummary = new System.Windows.Forms.Panel();
            this.lblTotalLogs = new System.Windows.Forms.Label();
            this.lblAvgAqi = new System.Windows.Forms.Label();
            this.lblMaxAqi = new System.Windows.Forms.Label();
            this.lblMinAqi = new System.Windows.Forms.Label();

            this.pnlFilterHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.pnlStatsSummary.SuspendLayout();
            this.SuspendLayout();

            // pnlFilterHeader
            this.pnlFilterHeader.BackColor = System.Drawing.Color.FromArgb(40, 42, 48);
            this.pnlFilterHeader.Controls.Add(this.btnExportCsv);
            this.pnlFilterHeader.Controls.Add(this.btnGenerateReport);
            this.pnlFilterHeader.Controls.Add(this.dtpTo);
            this.pnlFilterHeader.Controls.Add(this.lblTo);
            this.pnlFilterHeader.Controls.Add(this.dtpFrom);
            this.pnlFilterHeader.Controls.Add(this.lblFrom);
            this.pnlFilterHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlFilterHeader.Name = "pnlFilterHeader";
            this.pnlFilterHeader.Size = new System.Drawing.Size(900, 80);
            this.pnlFilterHeader.TabIndex = 0;

            // lblFrom
            this.lblFrom.AutoSize = true;
            this.lblFrom.ForeColor = System.Drawing.Color.White;
            this.lblFrom.Location = new System.Drawing.Point(20, 31);
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(43, 17);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";

            // dtpFrom
            this.dtpFrom.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(70, 27);
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(160, 24);
            this.dtpFrom.TabIndex = 1;

            // lblTo
            this.lblTo.AutoSize = true;
            this.lblTo.ForeColor = System.Drawing.Color.White;
            this.lblTo.Location = new System.Drawing.Point(250, 31);
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(25, 17);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";

            // dtpTo
            this.dtpTo.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(285, 27);
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(160, 24);
            this.dtpTo.TabIndex = 3;

            // btnGenerateReport
            this.btnGenerateReport.BackColor = System.Drawing.Color.FromArgb(26, 115, 232);
            this.btnGenerateReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateReport.Location = new System.Drawing.Point(470, 24);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(130, 32);
            this.btnGenerateReport.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnGenerateReport.TabIndex = 4;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.UseVisualStyleBackColor = false;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);

            // btnExportCsv
            this.btnExportCsv.BackColor = System.Drawing.Color.FromArgb(52, 168, 83);
            this.btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCsv.ForeColor = System.Drawing.Color.White;
            this.btnExportCsv.Location = new System.Drawing.Point(610, 24);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(120, 32);
            this.btnExportCsv.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportCsv.TabIndex = 5;
            this.btnExportCsv.Text = "Export to CSV";
            this.btnExportCsv.UseVisualStyleBackColor = false;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);

            // dgvReport
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.Location = new System.Drawing.Point(0, 80);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.Size = new System.Drawing.Size(900, 420);
            this.dgvReport.TabIndex = 1;

            // pnlStatsSummary
            this.pnlStatsSummary.BackColor = System.Drawing.Color.FromArgb(40, 42, 48);
            this.pnlStatsSummary.Controls.Add(this.lblTotalLogs);
            this.pnlStatsSummary.Controls.Add(this.lblAvgAqi);
            this.pnlStatsSummary.Controls.Add(this.lblMaxAqi);
            this.pnlStatsSummary.Controls.Add(this.lblMinAqi);
            this.pnlStatsSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatsSummary.Location = new System.Drawing.Point(0, 500);
            this.pnlStatsSummary.Name = "pnlStatsSummary";
            this.pnlStatsSummary.Size = new System.Drawing.Size(900, 60);
            this.pnlStatsSummary.TabIndex = 2;

            // lblTotalLogs
            this.lblTotalLogs.AutoSize = true;
            this.lblTotalLogs.ForeColor = System.Drawing.Color.LightGray;
            this.lblTotalLogs.Location = new System.Drawing.Point(20, 22);
            this.lblTotalLogs.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTotalLogs.Name = "lblTotalLogs";
            this.lblTotalLogs.Size = new System.Drawing.Size(85, 17);
            this.lblTotalLogs.TabIndex = 0;
            this.lblTotalLogs.Text = "Total Logs: 0";

            // lblAvgAqi
            this.lblAvgAqi.AutoSize = true;
            this.lblAvgAqi.ForeColor = System.Drawing.Color.LightGray;
            this.lblAvgAqi.Location = new System.Drawing.Point(220, 22);
            this.lblAvgAqi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAvgAqi.Name = "lblAvgAqi";
            this.lblAvgAqi.Size = new System.Drawing.Size(114, 17);
            this.lblAvgAqi.TabIndex = 1;
            this.lblAvgAqi.Text = "Average AQI: N/A";

            // lblMaxAqi
            this.lblMaxAqi.AutoSize = true;
            this.lblMaxAqi.ForeColor = System.Drawing.Color.FromArgb(234, 67, 53);
            this.lblMaxAqi.Location = new System.Drawing.Point(450, 22);
            this.lblMaxAqi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMaxAqi.Name = "lblMaxAqi";
            this.lblMaxAqi.Size = new System.Drawing.Size(85, 17);
            this.lblMaxAqi.TabIndex = 2;
            this.lblMaxAqi.Text = "Max AQI: N/A";

            // lblMinAqi
            this.lblMinAqi.AutoSize = true;
            this.lblMinAqi.ForeColor = System.Drawing.Color.FromArgb(52, 168, 83);
            this.lblMinAqi.Location = new System.Drawing.Point(680, 22);
            this.lblMinAqi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMinAqi.Name = "lblMinAqi";
            this.lblMinAqi.Size = new System.Drawing.Size(82, 17);
            this.lblMinAqi.TabIndex = 3;
            this.lblMinAqi.Text = "Min AQI: N/A";

            // ReportsControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(32, 33, 36);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnlStatsSummary);
            this.Controls.Add(this.pnlFilterHeader);
            this.Name = "ReportsControl";
            this.Size = new System.Drawing.Size(900, 560);

            this.pnlFilterHeader.ResumeLayout(false);
            this.pnlFilterHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.pnlStatsSummary.ResumeLayout(false);
            this.pnlStatsSummary.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
