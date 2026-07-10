using System.Drawing;
using System.Windows.Forms;

namespace AshkanAQMS
{
    partial class CurrentDataControl
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblTimestamp = new System.Windows.Forms.Label();
            this.pnlAQICard = new System.Windows.Forms.Panel();
            this.pnlAQIIndicator = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblAQIValue = new System.Windows.Forms.Label();
            this.lblAQITitle = new System.Windows.Forms.Label();
            this.pnlDetailsCard = new System.Windows.Forms.Panel();
            this.lblHum = new System.Windows.Forms.Label();
            this.lblHumHeader = new System.Windows.Forms.Label();
            this.lblTemp = new System.Windows.Forms.Label();
            this.lblTempHeader = new System.Windows.Forms.Label();
            this.lblNO2 = new System.Windows.Forms.Label();
            this.lblNO2Header = new System.Windows.Forms.Label();
            this.lblCO2 = new System.Windows.Forms.Label();
            this.lblCO2Header = new System.Windows.Forms.Label();
            this.lblPM10 = new System.Windows.Forms.Label();
            this.lblPM10Header = new System.Windows.Forms.Label();
            this.lblPM25 = new System.Windows.Forms.Label();
            this.lblPM25Header = new System.Windows.Forms.Label();
            this.pnlChartCard = new System.Windows.Forms.Panel();
            this.pnlChartContainer = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.pnlGridCard = new System.Windows.Forms.Panel();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAQI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPM25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCO2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLogTitle = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlAQICard.SuspendLayout();
            this.pnlDetailsCard.SuspendLayout();
            this.pnlChartCard.SuspendLayout();
            this.pnlGridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblAppName);
            this.pnlHeader.Controls.Add(this.lblTimestamp);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1100, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblAppName.Location = new System.Drawing.Point(20, 18);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(205, 32);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "Ashkan AQMS Live";
            // 
            // lblTimestamp
            // 
            this.lblTimestamp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTimestamp.Location = new System.Drawing.Point(900, 26);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new System.Drawing.Size(130, 19);
            this.lblTimestamp.TabIndex = 1;
            this.lblTimestamp.Text = "Status: Disconnected";
            // 
            // pnlAQICard
            // 
            this.pnlAQICard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAQICard.Controls.Add(this.pnlAQIIndicator);
            this.pnlAQICard.Controls.Add(this.lblStatus);
            this.pnlAQICard.Controls.Add(this.lblAQIValue);
            this.pnlAQICard.Controls.Add(this.lblAQITitle);
            this.pnlAQICard.Location = new System.Drawing.Point(20, 90);
            this.pnlAQICard.Name = "pnlAQICard";
            this.pnlAQICard.Size = new System.Drawing.Size(350, 180);
            this.pnlAQICard.TabIndex = 1;
            // 
            // pnlAQIIndicator
            // 
            this.pnlAQIIndicator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAQIIndicator.Location = new System.Drawing.Point(0, 168);
            this.pnlAQIIndicator.Name = "pnlAQIIndicator";
            this.pnlAQIIndicator.Size = new System.Drawing.Size(348, 10);
            this.pnlAQIIndicator.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(140, 90);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(190, 45);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Good";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAQIValue
            // 
            this.lblAQIValue.Font = new System.Drawing.Font("Segoe UI Semibold", 48F, System.Drawing.FontStyle.Bold);
            this.lblAQIValue.Location = new System.Drawing.Point(10, 45);
            this.lblAQIValue.Name = "lblAQIValue";
            this.lblAQIValue.Size = new System.Drawing.Size(130, 80);
            this.lblAQIValue.TabIndex = 1;
            this.lblAQIValue.Text = "0";
            this.lblAQIValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAQITitle
            // 
            this.lblAQITitle.AutoSize = true;
            this.lblAQITitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblAQITitle.Location = new System.Drawing.Point(15, 15);
            this.lblAQITitle.Name = "lblAQITitle";
            this.lblAQITitle.Size = new System.Drawing.Size(205, 21);
            this.lblAQITitle.TabIndex = 0;
            this.lblAQITitle.Text = "AIR QUALITY INDEX (AQI)";
            // 
            // pnlDetailsCard
            // 
            this.pnlDetailsCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetailsCard.Controls.Add(this.lblHum);
            this.pnlDetailsCard.Controls.Add(this.lblHumHeader);
            this.pnlDetailsCard.Controls.Add(this.lblTemp);
            this.pnlDetailsCard.Controls.Add(this.lblTempHeader);
            this.pnlDetailsCard.Controls.Add(this.lblNO2);
            this.pnlDetailsCard.Controls.Add(this.lblNO2Header);
            this.pnlDetailsCard.Controls.Add(this.lblCO2);
            this.pnlDetailsCard.Controls.Add(this.lblCO2Header);
            this.pnlDetailsCard.Controls.Add(this.lblPM10);
            this.pnlDetailsCard.Controls.Add(this.lblPM10Header);
            this.pnlDetailsCard.Controls.Add(this.lblPM25);
            this.pnlDetailsCard.Controls.Add(this.lblPM25Header);
            this.pnlDetailsCard.Location = new System.Drawing.Point(390, 90);
            this.pnlDetailsCard.Name = "pnlDetailsCard";
            this.pnlDetailsCard.Size = new System.Drawing.Size(690, 180);
            this.pnlDetailsCard.TabIndex = 2;
            // 
            // lblHum
            // 
            this.lblHum.AutoSize = true;
            this.lblHum.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblHum.Location = new System.Drawing.Point(480, 130);
            this.lblHum.Name = "lblHum";
            this.lblHum.Size = new System.Drawing.Size(52, 25);
            this.lblHum.TabIndex = 11;
            this.lblHum.Text = "-- %";
            // 
            // lblHumHeader
            // 
            this.lblHumHeader.AutoSize = true;
            this.lblHumHeader.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHumHeader.Location = new System.Drawing.Point(480, 105);
            this.lblHumHeader.Name = "lblHumHeader";
            this.lblHumHeader.Size = new System.Drawing.Size(57, 15);
            this.lblHumHeader.TabIndex = 10;
            this.lblHumHeader.Text = "Humidity";
            // 
            // lblTemp
            // 
            this.lblTemp.AutoSize = true;
            this.lblTemp.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTemp.Location = new System.Drawing.Point(480, 50);
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Size = new System.Drawing.Size(54, 25);
            this.lblTemp.TabIndex = 9;
            this.lblTemp.Text = "-- °C";
            // 
            // lblTempHeader
            // 
            this.lblTempHeader.AutoSize = true;
            this.lblTempHeader.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTempHeader.Location = new System.Drawing.Point(480, 25);
            this.lblTempHeader.Name = "lblTempHeader";
            this.lblTempHeader.Size = new System.Drawing.Size(73, 15);
            this.lblTempHeader.TabIndex = 8;
            this.lblTempHeader.Text = "Temperature";
            // 
            // lblNO2
            // 
            this.lblNO2.AutoSize = true;
            this.lblNO2.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblNO2.Location = new System.Drawing.Point(250, 130);
            this.lblNO2.Name = "lblNO2";
            this.lblNO2.Size = new System.Drawing.Size(67, 25);
            this.lblNO2.TabIndex = 7;
            this.lblNO2.Text = "-- ppb";
            // 
            // lblNO2Header
            // 
            this.lblNO2Header.AutoSize = true;
            this.lblNO2Header.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNO2Header.Location = new System.Drawing.Point(250, 105);
            this.lblNO2Header.Name = "lblNO2Header";
            this.lblNO2Header.Size = new System.Drawing.Size(100, 15);
            this.lblNO2Header.TabIndex = 6;
            this.lblNO2Header.Text = "Nitrogen Dioxide";
            // 
            // lblCO2
            // 
            this.lblCO2.AutoSize = true;
            this.lblCO2.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblCO2.Location = new System.Drawing.Point(250, 50);
            this.lblCO2.Name = "lblCO2";
            this.lblCO2.Size = new System.Drawing.Size(71, 25);
            this.lblCO2.TabIndex = 5;
            this.lblCO2.Text = "-- ppm";
            // 
            // lblCO2Header
            // 
            this.lblCO2Header.AutoSize = true;
            this.lblCO2Header.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCO2Header.Location = new System.Drawing.Point(250, 25);
            this.lblCO2Header.Name = "lblCO2Header";
            this.lblCO2Header.Size = new System.Drawing.Size(89, 15);
            this.lblCO2Header.TabIndex = 4;
            this.lblCO2Header.Text = "Carbon Dioxide";
            // 
            // lblPM10
            // 
            this.lblPM10.AutoSize = true;
            this.lblPM10.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblPM10.Location = new System.Drawing.Point(20, 130);
            this.lblPM10.Name = "lblPM10";
            this.lblPM10.Size = new System.Drawing.Size(95, 25);
            this.lblPM10.TabIndex = 3;
            this.lblPM10.Text = "-- µg/m³";
            // 
            // lblPM10Header
            // 
            this.lblPM10Header.AutoSize = true;
            this.lblPM10Header.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPM10Header.Location = new System.Drawing.Point(20, 105);
            this.lblPM10Header.Name = "lblPM10Header";
            this.lblPM10Header.Size = new System.Drawing.Size(41, 15);
            this.lblPM10Header.TabIndex = 2;
            this.lblPM10Header.Text = "PM 10";
            // 
            // lblPM25
            // 
            this.lblPM25.AutoSize = true;
            this.lblPM25.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblPM25.Location = new System.Drawing.Point(20, 50);
            this.lblPM25.Name = "lblPM25";
            this.lblPM25.Size = new System.Drawing.Size(95, 25);
            this.lblPM25.TabIndex = 1;
            this.lblPM25.Text = "-- µg/m³";
            // 
            // lblPM25Header
            // 
            this.lblPM25Header.AutoSize = true;
            this.lblPM25Header.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPM25Header.Location = new System.Drawing.Point(20, 25);
            this.lblPM25Header.Name = "lblPM25Header";
            this.lblPM25Header.Size = new System.Drawing.Size(47, 15);
            this.lblPM25Header.TabIndex = 0;
            this.lblPM25Header.Text = "PM 2.5";
            // 
            // pnlChartCard
            // 
            this.pnlChartCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChartCard.Controls.Add(this.pnlChartContainer);
            this.pnlChartCard.Controls.Add(this.lblChartTitle);
            this.pnlChartCard.Location = new System.Drawing.Point(20, 290);
            this.pnlChartCard.Name = "pnlChartCard";
            this.pnlChartCard.Size = new System.Drawing.Size(600, 280);
            this.pnlChartCard.TabIndex = 3;
            // 
            // pnlChartContainer
            // 
            this.pnlChartContainer.Location = new System.Drawing.Point(15, 45);
            this.pnlChartContainer.Name = "pnlChartContainer";
            this.pnlChartContainer.Size = new System.Drawing.Size(570, 215);
            this.pnlChartContainer.TabIndex = 1;
            this.pnlChartContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlChartContainer_Paint);
            // 
            // lblChartTitle
            // 
            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.Location = new System.Drawing.Point(15, 12);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Size = new System.Drawing.Size(176, 20);
            this.lblChartTitle.TabIndex = 0;
            this.lblChartTitle.Text = "LIVE TREND MONITORING";
            // 
            // pnlGridCard
            // 
            this.pnlGridCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGridCard.Controls.Add(this.dgvLogs);
            this.pnlGridCard.Controls.Add(this.lblLogTitle);
            this.pnlGridCard.Location = new System.Drawing.Point(640, 290);
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Size = new System.Drawing.Size(440, 280);
            this.pnlGridCard.TabIndex = 4;
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.White;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colAQI,
            this.colPM25,
            this.colCO2,
            this.colTemp,
            this.colHum});
            this.dgvLogs.Location = new System.Drawing.Point(15, 45);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.Size = new System.Drawing.Size(410, 215);
            this.dgvLogs.TabIndex = 1;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            // 
            // colAQI
            // 
            this.colAQI.HeaderText = "AQI";
            this.colAQI.Name = "colAQI";
            this.colAQI.ReadOnly = true;
            // 
            // colPM25
            // 
            this.colPM25.HeaderText = "PM2.5";
            this.colPM25.Name = "colPM25";
            this.colPM25.ReadOnly = true;
            // 
            // colCO2
            // 
            this.colCO2.HeaderText = "CO2";
            this.colCO2.Name = "colCO2";
            this.colCO2.ReadOnly = true;
            // 
            // colTemp
            // 
            this.colTemp.HeaderText = "Temp";
            this.colTemp.Name = "colTemp";
            this.colTemp.ReadOnly = true;
            // 
            // colHum
            // 
            this.colHum.HeaderText = "Hum";
            this.colHum.Name = "colHum";
            this.colHum.ReadOnly = true;
            // 
            // lblLogTitle
            // 
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.Location = new System.Drawing.Point(15, 12);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Size = new System.Drawing.Size(130, 20);
            this.lblLogTitle.TabIndex = 0;
            this.lblLogTitle.Text = "TELEMETRY LOGS";
            // 
            // btnStartStop
            // 
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStop.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnStartStop.ForeColor = System.Drawing.Color.White;
            this.btnStartStop.Location = new System.Drawing.Point(20, 590);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(200, 45);
            this.btnStartStop.TabIndex = 5;
            this.btnStartStop.Text = "Start Live Stream";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnExport
            // 
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(240, 590);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(180, 45);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export to CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // CurrentDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.pnlChartCard);
            this.Controls.Add(this.pnlDetailsCard);
            this.Controls.Add(this.pnlAQICard);
            this.Controls.Add(this.pnlHeader);
            this.Name = "CurrentDataControl";
            this.Size = new System.Drawing.Size(1100, 660);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlAQICard.ResumeLayout(false);
            this.pnlAQICard.PerformLayout();
            this.pnlDetailsCard.ResumeLayout(false);
            this.pnlDetailsCard.PerformLayout();
            this.pnlChartCard.ResumeLayout(false);
            this.pnlChartCard.PerformLayout();
            this.pnlGridCard.ResumeLayout(false);
            this.pnlGridCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblTimestamp;
        private System.Windows.Forms.Panel pnlAQICard;
        private System.Windows.Forms.Label lblAQITitle;
        private System.Windows.Forms.Label lblAQIValue;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel pnlAQIIndicator;
        private System.Windows.Forms.Panel pnlDetailsCard;
        private System.Windows.Forms.Label lblPM25Header;
        private System.Windows.Forms.Label lblPM25;
        private System.Windows.Forms.Label lblPM10;
        private System.Windows.Forms.Label lblPM10Header;
        private System.Windows.Forms.Label lblCO2;
        private System.Windows.Forms.Label lblCO2Header;
        private System.Windows.Forms.Label lblNO2;
        private System.Windows.Forms.Label lblNO2Header;
        private System.Windows.Forms.Label lblTemp;
        private System.Windows.Forms.Label lblTempHeader;
        private System.Windows.Forms.Label lblHum;
        private System.Windows.Forms.Label lblHumHeader;
        private System.Windows.Forms.Panel pnlChartCard;
        private System.Windows.Forms.Panel pnlChartContainer;
        private System.Windows.Forms.Label lblChartTitle;
        private System.Windows.Forms.Panel pnlGridCard;
        private System.Windows.Forms.Label lblLogTitle;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAQI;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPM25;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCO2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTemp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHum;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnExport;
    }
}
