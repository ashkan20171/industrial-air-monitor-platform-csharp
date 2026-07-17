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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.btnExport = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAQI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPM25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCO2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLogTitle = new System.Windows.Forms.Label();
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
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblAppName);
            this.pnlHeader.Controls.Add(this.lblTimestamp);
            this.pnlHeader.Location = new System.Drawing.Point(20, 20);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1160, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblAppName.Location = new System.Drawing.Point(20, 18);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(276, 32);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "Air Quality Monitoring";
            // 
            // lblTimestamp
            // 
            this.lblTimestamp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimestamp.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTimestamp.ForeColor = System.Drawing.Color.Gray;
            this.lblTimestamp.Location = new System.Drawing.Point(724, 24);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new System.Drawing.Size(416, 23);
            this.lblTimestamp.TabIndex = 1;
            this.lblTimestamp.Text = "Last Update: -";
            this.lblTimestamp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlAQICard
            // 
            this.pnlAQICard.BackColor = System.Drawing.Color.White;
            this.pnlAQICard.Controls.Add(this.pnlAQIIndicator);
            this.pnlAQICard.Controls.Add(this.lblStatus);
            this.pnlAQICard.Controls.Add(this.lblAQIValue);
            this.pnlAQICard.Controls.Add(this.lblAQITitle);
            this.pnlAQICard.Location = new System.Drawing.Point(20, 105);
            this.pnlAQICard.Name = "pnlAQICard";
            this.pnlAQICard.Size = new System.Drawing.Size(260, 210);
            this.pnlAQICard.TabIndex = 1;
            // 
            // pnlAQIIndicator
            // 
            this.pnlAQIIndicator.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.pnlAQIIndicator.Location = new System.Drawing.Point(0, 0);
            this.pnlAQIIndicator.Name = "pnlAQIIndicator";
            this.pnlAQIIndicator.Size = new System.Drawing.Size(260, 8);
            this.pnlAQIIndicator.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.lblStatus.Location = new System.Drawing.Point(20, 150);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(220, 28);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Good";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAQIValue
            // 
            this.lblAQIValue.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            this.lblAQIValue.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblAQIValue.Location = new System.Drawing.Point(20, 65);
            this.lblAQIValue.Name = "lblAQIValue";
            this.lblAQIValue.Size = new System.Drawing.Size(220, 72);
            this.lblAQIValue.TabIndex = 2;
            this.lblAQIValue.Text = "0";
            this.lblAQIValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAQITitle
            // 
            this.lblAQITitle.AutoSize = true;
            this.lblAQITitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAQITitle.ForeColor = System.Drawing.Color.Gray;
            this.lblAQITitle.Location = new System.Drawing.Point(90, 25);
            this.lblAQITitle.Name = "lblAQITitle";
            this.lblAQITitle.Size = new System.Drawing.Size(80, 21);
            this.lblAQITitle.TabIndex = 1;
            this.lblAQITitle.Text = "AQI Now";
            // 
            // pnlDetailsCard
            // 
            this.pnlDetailsCard.BackColor = System.Drawing.Color.White;
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
            this.pnlDetailsCard.Location = new System.Drawing.Point(295, 105);
            this.pnlDetailsCard.Name = "pnlDetailsCard";
            this.pnlDetailsCard.Size = new System.Drawing.Size(885, 210);
            this.pnlDetailsCard.TabIndex = 2;
            // 
            // lblHum
            // 
            this.lblHum.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHum.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblHum.Location = new System.Drawing.Point(605, 125);
            this.lblHum.Name = "lblHum";
            this.lblHum.Size = new System.Drawing.Size(230, 35);
            this.lblHum.TabIndex = 11;
            this.lblHum.Text = "0 %";
            // 
            // lblHumHeader
            // 
            this.lblHumHeader.AutoSize = true;
            this.lblHumHeader.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblHumHeader.ForeColor = System.Drawing.Color.Gray;
            this.lblHumHeader.Location = new System.Drawing.Point(605, 100);
            this.lblHumHeader.Name = "lblHumHeader";
            this.lblHumHeader.Size = new System.Drawing.Size(62, 19);
            this.lblHumHeader.TabIndex = 10;
            this.lblHumHeader.Text = "Humidity";
            // 
            // lblTemp
            // 
            this.lblTemp.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTemp.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTemp.Location = new System.Drawing.Point(325, 125);
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Size = new System.Drawing.Size(230, 35);
            this.lblTemp.TabIndex = 9;
            this.lblTemp.Text = "0 °C";
            // 
            // lblTempHeader
            // 
            this.lblTempHeader.AutoSize = true;
            this.lblTempHeader.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTempHeader.ForeColor = System.Drawing.Color.Gray;
            this.lblTempHeader.Location = new System.Drawing.Point(325, 100);
            this.lblTempHeader.Name = "lblTempHeader";
            this.lblTempHeader.Size = new System.Drawing.Size(83, 19);
            this.lblTempHeader.TabIndex = 8;
            this.lblTempHeader.Text = "Temperature";
            // 
            // lblNO2
            // 
            this.lblNO2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblNO2.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblNO2.Location = new System.Drawing.Point(20, 125);
            this.lblNO2.Name = "lblNO2";
            this.lblNO2.Size = new System.Drawing.Size(230, 35);
            this.lblNO2.TabIndex = 7;
            this.lblNO2.Text = "0 ppb";
            // 
            // lblNO2Header
            // 
            this.lblNO2Header.AutoSize = true;
            this.lblNO2Header.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNO2Header.ForeColor = System.Drawing.Color.Gray;
            this.lblNO2Header.Location = new System.Drawing.Point(20, 100);
            this.lblNO2Header.Name = "lblNO2Header";
            this.lblNO2Header.Size = new System.Drawing.Size(37, 19);
            this.lblNO2Header.TabIndex = 6;
            this.lblNO2Header.Text = "NO2";
            // 
            // lblCO2
            // 
            this.lblCO2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCO2.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblCO2.Location = new System.Drawing.Point(605, 45);
            this.lblCO2.Name = "lblCO2";
            this.lblCO2.Size = new System.Drawing.Size(230, 35);
            this.lblCO2.TabIndex = 5;
            this.lblCO2.Text = "0 ppm";
            // 
            // lblCO2Header
            // 
            this.lblCO2Header.AutoSize = true;
            this.lblCO2Header.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCO2Header.ForeColor = System.Drawing.Color.Gray;
            this.lblCO2Header.Location = new System.Drawing.Point(605, 20);
            this.lblCO2Header.Name = "lblCO2Header";
            this.lblCO2Header.Size = new System.Drawing.Size(34, 19);
            this.lblCO2Header.TabIndex = 4;
            this.lblCO2Header.Text = "CO2";
            // 
            // lblPM10
            // 
            this.lblPM10.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPM10.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblPM10.Location = new System.Drawing.Point(325, 45);
            this.lblPM10.Name = "lblPM10";
            this.lblPM10.Size = new System.Drawing.Size(230, 35);
            this.lblPM10.TabIndex = 3;
            this.lblPM10.Text = "0 µg/m³";
            // 
            // lblPM10Header
            // 
            this.lblPM10Header.AutoSize = true;
            this.lblPM10Header.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPM10Header.ForeColor = System.Drawing.Color.Gray;
            this.lblPM10Header.Location = new System.Drawing.Point(325, 20);
            this.lblPM10Header.Name = "lblPM10Header";
            this.lblPM10Header.Size = new System.Drawing.Size(44, 19);
            this.lblPM10Header.TabIndex = 2;
            this.lblPM10Header.Text = "PM10";
            // 
            // lblPM25
            // 
            this.lblPM25.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPM25.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblPM25.Location = new System.Drawing.Point(20, 45);
            this.lblPM25.Name = "lblPM25";
            this.lblPM25.Size = new System.Drawing.Size(230, 35);
            this.lblPM25.TabIndex = 1;
            this.lblPM25.Text = "0 µg/m³";
            // 
            // lblPM25Header
            // 
            this.lblPM25Header.AutoSize = true;
            this.lblPM25Header.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPM25Header.ForeColor = System.Drawing.Color.Gray;
            this.lblPM25Header.Location = new System.Drawing.Point(20, 20);
            this.lblPM25Header.Name = "lblPM25Header";
            this.lblPM25Header.Size = new System.Drawing.Size(45, 19);
            this.lblPM25Header.TabIndex = 0;
            this.lblPM25Header.Text = "PM2.5";
            // 
            // pnlChartCard
            // 
            this.pnlChartCard.BackColor = System.Drawing.Color.White;
            this.pnlChartCard.Controls.Add(this.pnlChartContainer);
            this.pnlChartCard.Controls.Add(this.lblChartTitle);
            this.pnlChartCard.Location = new System.Drawing.Point(20, 330);
            this.pnlChartCard.Name = "pnlChartCard";
            this.pnlChartCard.Size = new System.Drawing.Size(760, 320);
            this.pnlChartCard.TabIndex = 3;
            // 
            // pnlChartContainer
            // 
            this.pnlChartContainer.BackColor = System.Drawing.Color.White;
            this.pnlChartContainer.Location = new System.Drawing.Point(15, 45);
            this.pnlChartContainer.Name = "pnlChartContainer";
            this.pnlChartContainer.Size = new System.Drawing.Size(730, 255);
            this.pnlChartContainer.TabIndex = 1;
            this.pnlChartContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlChartContainer_Paint);
            // 
            // lblChartTitle
            // 
            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblChartTitle.Location = new System.Drawing.Point(15, 15);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Size = new System.Drawing.Size(118, 21);
            this.lblChartTitle.TabIndex = 0;
            this.lblChartTitle.Text = "AQI Trend (50)";
            // 
            // pnlGridCard
            // 
            this.pnlGridCard.BackColor = System.Drawing.Color.White;
            this.pnlGridCard.Controls.Add(this.btnExport);
            this.pnlGridCard.Controls.Add(this.btnStartStop);
            this.pnlGridCard.Controls.Add(this.dgvLogs);
            this.pnlGridCard.Controls.Add(this.lblLogTitle);
            this.pnlGridCard.Location = new System.Drawing.Point(795, 330);
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Size = new System.Drawing.Size(385, 320);
            this.pnlGridCard.TabIndex = 4;
            // 
            // btnExport
            // 
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(210, 15);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(155, 36);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnStartStop
            // 
            this.btnStartStop.FlatAppearance.BorderSize = 0;
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStartStop.ForeColor = System.Drawing.Color.White;
            this.btnStartStop.Location = new System.Drawing.Point(20, 15);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(170, 36);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "Start Monitoring";
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.AllowUserToResizeRows = false;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.White;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLogs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLogs.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLogs.ColumnHeadersHeight = 34;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colAQI,
            this.colPM25,
            this.colCO2,
            this.colTemp,
            this.colHum});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogs.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLogs.EnableHeadersVisualStyles = false;
            this.dgvLogs.GridColor = System.Drawing.Color.FromArgb(235, 235, 235);
            this.dgvLogs.Location = new System.Drawing.Point(20, 60);
            this.dgvLogs.MultiSelect = false;
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowTemplate.Height = 28;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.Size = new System.Drawing.Size(345, 240);
            this.dgvLogs.TabIndex = 1;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 58;
            // 
            // colAQI
            // 
            this.colAQI.HeaderText = "AQI";
            this.colAQI.Name = "colAQI";
            this.colAQI.ReadOnly = true;
            this.colAQI.Width = 50;
            // 
            // colPM25
            // 
            this.colPM25.HeaderText = "PM2.5";
            this.colPM25.Name = "colPM25";
            this.colPM25.ReadOnly = true;
            this.colPM25.Width = 60;
            // 
            // colCO2
            // 
            this.colCO2.HeaderText = "CO2";
            this.colCO2.Name = "colCO2";
            this.colCO2.ReadOnly = true;
            this.colCO2.Width = 55;
            // 
            // colTemp
            // 
            this.colTemp.HeaderText = "Temp";
            this.colTemp.Name = "colTemp";
            this.colTemp.ReadOnly = true;
            this.colTemp.Width = 58;
            // 
            // colHum
            // 
            this.colHum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHum.HeaderText = "Humidity";
            this.colHum.Name = "colHum";
            this.colHum.ReadOnly = true;
            // 
            // lblLogTitle
            // 
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblLogTitle.Location = new System.Drawing.Point(16, -40);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Size = new System.Drawing.Size(91, 21);
            this.lblLogTitle.TabIndex = 0;
            this.lblLogTitle.Text = "Live Logs";
            this.lblLogTitle.Visible = false;
            // 
            // CurrentDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.pnlChartCard);
            this.Controls.Add(this.pnlDetailsCard);
            this.Controls.Add(this.pnlAQICard);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "CurrentDataControl";
            this.Size = new System.Drawing.Size(1200, 680);
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

        #endregion

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
        private System.Windows.Forms.Label lblPM10Header;
        private System.Windows.Forms.Label lblPM10;
        private System.Windows.Forms.Label lblCO2Header;
        private System.Windows.Forms.Label lblCO2;
        private System.Windows.Forms.Label lblNO2Header;
        private System.Windows.Forms.Label lblNO2;
        private System.Windows.Forms.Label lblTempHeader;
        private System.Windows.Forms.Label lblTemp;
        private System.Windows.Forms.Label lblHumHeader;
        private System.Windows.Forms.Label lblHum;
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
