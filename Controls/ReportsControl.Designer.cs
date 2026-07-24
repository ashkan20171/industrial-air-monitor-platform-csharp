namespace AshkanAQMS.Controls
{
    partial class ReportsControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlFilterHeader;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel pnlStatsSummary;
        private System.Windows.Forms.Label lblAvgAqi;
        private System.Windows.Forms.Label lblMaxAqi;
        private System.Windows.Forms.Label lblMinAqi;
        private System.Windows.Forms.Label lblTotalLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAnalyzerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPm25;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCo2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAqi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;

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
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAnalyzerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPm25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCo2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAqi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlStatsSummary = new System.Windows.Forms.Panel();
            this.lblAvgAqi = new System.Windows.Forms.Label();
            this.lblMaxAqi = new System.Windows.Forms.Label();
            this.lblMinAqi = new System.Windows.Forms.Label();
            this.lblTotalLogs = new System.Windows.Forms.Label();
            this.pnlFilterHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.pnlStatsSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilterHeader
            // 
            this.pnlFilterHeader.BackColor = System.Drawing.Color.White;
            this.pnlFilterHeader.Controls.Add(this.lblFrom);
            this.pnlFilterHeader.Controls.Add(this.lblTo);
            this.pnlFilterHeader.Controls.Add(this.dtpFrom);
            this.pnlFilterHeader.Controls.Add(this.dtpTo);
            this.pnlFilterHeader.Controls.Add(this.btnGenerateReport);
            this.pnlFilterHeader.Controls.Add(this.btnExportCsv);
            this.pnlFilterHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlFilterHeader.Name = "pnlFilterHeader";
            this.pnlFilterHeader.Size = new System.Drawing.Size(980, 72);
            this.pnlFilterHeader.TabIndex = 0;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFrom.Location = new System.Drawing.Point(18, 16);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(39, 15);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTo.Location = new System.Drawing.Point(274, 16);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 15);
            this.lblTo.TabIndex = 1;
            this.lblTo.Text = "To";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(21, 34);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowUpDown = true;
            this.dtpFrom.Size = new System.Drawing.Size(230, 23);
            this.dtpFrom.TabIndex = 2;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(277, 34);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowUpDown = true;
            this.dtpTo.Size = new System.Drawing.Size(230, 23);
            this.dtpTo.TabIndex = 3;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerateReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateReport.Location = new System.Drawing.Point(533, 27);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(130, 33);
            this.btnGenerateReport.TabIndex = 4;
            this.btnGenerateReport.Text = "Generate";
            this.btnGenerateReport.UseVisualStyleBackColor = false;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnExportCsv.FlatAppearance.BorderSize = 0;
            this.btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCsv.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportCsv.ForeColor = System.Drawing.Color.White;
            this.btnExportCsv.Location = new System.Drawing.Point(679, 27);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(130, 33);
            this.btnExportCsv.TabIndex = 5;
            this.btnExportCsv.Text = "Export CSV";
            this.btnExportCsv.UseVisualStyleBackColor = false;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AllowUserToResizeRows = false;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTimestamp,
            this.colAnalyzerId,
            this.colPm25,
            this.colCo2,
            this.colAqi,
            this.colStatus});
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.Location = new System.Drawing.Point(0, 72);
            this.dgvReport.MultiSelect = false;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(980, 508);
            this.dgvReport.TabIndex = 1;
            // 
            // colTimestamp
            // 
            this.colTimestamp.DataPropertyName = "Timestamp";
            this.colTimestamp.HeaderText = "Timestamp";
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.ReadOnly = true;
            this.colTimestamp.Width = 160;
            // 
            // colAnalyzerId
            // 
            this.colAnalyzerId.DataPropertyName = "AnalyzerId";
            this.colAnalyzerId.HeaderText = "Analyzer ID";
            this.colAnalyzerId.Name = "colAnalyzerId";
            this.colAnalyzerId.ReadOnly = true;
            this.colAnalyzerId.Width = 110;
            // 
            // colPm25
            // 
            this.colPm25.DataPropertyName = "PM25";
            this.colPm25.HeaderText = "PM2.5";
            this.colPm25.Name = "colPm25";
            this.colPm25.ReadOnly = true;
            this.colPm25.Width = 90;
            // 
            // colCo2
            // 
            this.colCo2.DataPropertyName = "CO2";
            this.colCo2.HeaderText = "CO2";
            this.colCo2.Name = "colCo2";
            this.colCo2.ReadOnly = true;
            this.colCo2.Width = 90;
            // 
            // colAqi
            // 
            this.colAqi.DataPropertyName = "AQI";
            this.colAqi.HeaderText = "AQI";
            this.colAqi.Name = "colAqi";
            this.colAqi.ReadOnly = true;
            this.colAqi.Width = 80;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            // 
            // pnlStatsSummary
            // 
            this.pnlStatsSummary.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlStatsSummary.Controls.Add(this.lblAvgAqi);
            this.pnlStatsSummary.Controls.Add(this.lblMaxAqi);
            this.pnlStatsSummary.Controls.Add(this.lblMinAqi);
            this.pnlStatsSummary.Controls.Add(this.lblTotalLogs);
            this.pnlStatsSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatsSummary.Location = new System.Drawing.Point(0, 500);
            this.pnlStatsSummary.Name = "pnlStatsSummary";
            this.pnlStatsSummary.Size = new System.Drawing.Size(980, 80);
            this.pnlStatsSummary.TabIndex = 2;
            // 
            // lblAvgAqi
            // 
            this.lblAvgAqi.AutoSize = true;
            this.lblAvgAqi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAvgAqi.Location = new System.Drawing.Point(260, 28);
            this.lblAvgAqi.Name = "lblAvgAqi";
            this.lblAvgAqi.Size = new System.Drawing.Size(97, 19);
            this.lblAvgAqi.TabIndex = 1;
            this.lblAvgAqi.Text = "Average AQI: -";
            // 
            // lblMaxAqi
            // 
            this.lblMaxAqi.AutoSize = true;
            this.lblMaxAqi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMaxAqi.Location = new System.Drawing.Point(470, 28);
            this.lblMaxAqi.Name = "lblMaxAqi";
            this.lblMaxAqi.Size = new System.Drawing.Size(73, 19);
            this.lblMaxAqi.TabIndex = 2;
            this.lblMaxAqi.Text = "Max AQI: -";
            // 
            // lblMinAqi
            // 
            this.lblMinAqi.AutoSize = true;
            this.lblMinAqi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMinAqi.Location = new System.Drawing.Point(650, 28);
            this.lblMinAqi.Name = "lblMinAqi";
            this.lblMinAqi.Size = new System.Drawing.Size(72, 19);
            this.lblMinAqi.TabIndex = 3;
            this.lblMinAqi.Text = "Min AQI: -";
            // 
            // lblTotalLogs
            // 
            this.lblTotalLogs.AutoSize = true;
            this.lblTotalLogs.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalLogs.Location = new System.Drawing.Point(21, 28);
            this.lblTotalLogs.Name = "lblTotalLogs";
            this.lblTotalLogs.Size = new System.Drawing.Size(90, 19);
            this.lblTotalLogs.TabIndex = 0;
            this.lblTotalLogs.Text = "Total Logs: 0";
            // 
            // ReportsControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnlStatsSummary);
            this.Controls.Add(this.pnlFilterHeader);
            this.Name = "ReportsControl";
            this.Size = new System.Drawing.Size(980, 580);
            this.pnlFilterHeader.ResumeLayout(false);
            this.pnlFilterHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.pnlStatsSummary.ResumeLayout(false);
            this.pnlStatsSummary.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
