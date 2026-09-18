namespace AshkanAQMS.Controls
{
    partial class CurrentDataControl
    {
        private System.ComponentModel.IContainer components = null;

        // Header controls
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblTimestamp;

        // AQI Card controls
        private System.Windows.Forms.Panel pnlAQICard;
        private System.Windows.Forms.Label lblAQITitle;
        private System.Windows.Forms.Label lblAQIValue;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel pnlAQIIndicator;

        // Details Card controls
        private System.Windows.Forms.Panel pnlDetailsCard;
        private System.Windows.Forms.Label lblPM25Header;
        private System.Windows.Forms.Label lblPM25Value;
        private System.Windows.Forms.Label lblPM10Header;
        private System.Windows.Forms.Label lblPM10Value;
        private System.Windows.Forms.Label lblCO2Header;
        private System.Windows.Forms.Label lblCO2Value;
        private System.Windows.Forms.Label lblNO2Header;
        private System.Windows.Forms.Label lblNO2Value;
        private System.Windows.Forms.Label lblTempHeader;
        private System.Windows.Forms.Label lblTempValue;
        private System.Windows.Forms.Label lblHumHeader;
        private System.Windows.Forms.Label lblHumValue;
        private System.Windows.Forms.Label lblDominantPollutant;

        // Chart Card controls
        private System.Windows.Forms.Panel pnlChartCard;
        private System.Windows.Forms.Panel pnlChartContainer;
        private System.Windows.Forms.Label lblChartTitle;

        // Logs Grid Card controls
        private System.Windows.Forms.Panel pnlGridCard;
        private System.Windows.Forms.Label lblLogTitle;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAQI;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPM25;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCO2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTemp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHum;

        // Actions
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnExport;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblTimestamp = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();

            this.pnlAQICard = new System.Windows.Forms.Panel();
            this.lblAQITitle = new System.Windows.Forms.Label();
            this.lblAQIValue = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlAQIIndicator = new System.Windows.Forms.Panel();

            this.pnlDetailsCard = new System.Windows.Forms.Panel();
            this.lblPM25Header = new System.Windows.Forms.Label();
            this.lblPM25Value = new System.Windows.Forms.Label();
            this.lblPM10Header = new System.Windows.Forms.Label();
            this.lblPM10Value = new System.Windows.Forms.Label();
            this.lblCO2Header = new System.Windows.Forms.Label();
            this.lblCO2Value = new System.Windows.Forms.Label();
            this.lblNO2Header = new System.Windows.Forms.Label();
            this.lblNO2Value = new System.Windows.Forms.Label();
            this.lblTempHeader = new System.Windows.Forms.Label();
            this.lblTempValue = new System.Windows.Forms.Label();
            this.lblHumHeader = new System.Windows.Forms.Label();
            this.lblHumValue = new System.Windows.Forms.Label();
            this.lblDominantPollutant = new System.Windows.Forms.Label();

            this.pnlChartCard = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.pnlChartContainer = new System.Windows.Forms.Panel();

            this.pnlGridCard = new System.Windows.Forms.Panel();
            this.lblLogTitle = new System.Windows.Forms.Label();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAQI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPM25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCO2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHum = new System.Windows.Forms.DataGridViewTextBoxColumn();

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
            this.pnlHeader.Controls.Add(this.btnExport);
            this.pnlHeader.Controls.Add(this.btnStartStop);
            this.pnlHeader.Controls.Add(this.lblTimestamp);
            this.pnlHeader.Controls.Add(this.lblAppName);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1260, 60);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.lblAppName.Location = new System.Drawing.Point(20, 16);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(225, 25);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "Live Monitoring Station";

            // 
            // lblTimestamp
            // 
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTimestamp.ForeColor = System.Drawing.Color.Gray;
            this.lblTimestamp.Location = new System.Drawing.Point(260, 22);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new System.Drawing.Size(100, 17);
            this.lblTimestamp.TabIndex = 1;
            this.lblTimestamp.Text = "Last Update: --";

            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartStop.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartStop.ForeColor = System.Drawing.Color.White;
            this.btnStartStop.Location = new System.Drawing.Point(1030, 14);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(100, 32);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "Stop";
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);

            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(13, 110, 253);
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(1140, 14);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 32);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            // 
            // pnlAQICard
            // 
            this.pnlAQICard.BackColor = System.Drawing.Color.White;
            this.pnlAQICard.Controls.Add(this.pnlAQIIndicator);
            this.pnlAQICard.Controls.Add(this.lblStatus);
            this.pnlAQICard.Controls.Add(this.lblAQIValue);
            this.pnlAQICard.Controls.Add(this.lblAQITitle);
            this.pnlAQICard.Location = new System.Drawing.Point(20, 75);
            this.pnlAQICard.Name = "pnlAQICard";
            this.pnlAQICard.Size = new System.Drawing.Size(280, 170);
            this.pnlAQICard.TabIndex = 1;

            // 
            // lblAQITitle
            // 
            this.lblAQITitle.AutoSize = true;
            this.lblAQITitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAQITitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblAQITitle.Location = new System.Drawing.Point(15, 12);
            this.lblAQITitle.Name = "lblAQITitle";
            this.lblAQITitle.Size = new System.Drawing.Size(130, 19);
            this.lblAQITitle.TabIndex = 0;
            this.lblAQITitle.Text = "AIR QUALITY INDEX";

            // 
            // lblAQIValue
            // 
            this.lblAQIValue.AutoSize = true;
            this.lblAQIValue.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            this.lblAQIValue.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.lblAQIValue.Location = new System.Drawing.Point(10, 35);
            this.lblAQIValue.Name = "lblAQIValue";
            this.lblAQIValue.Size = new System.Drawing.Size(83, 65);
            this.lblAQIValue.TabIndex = 1;
            this.lblAQIValue.Text = "--";

            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.lblStatus.Location = new System.Drawing.Point(15, 105);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 20);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Good";

            // 
            // pnlAQIIndicator
            // 
            this.pnlAQIIndicator.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.pnlAQIIndicator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAQIIndicator.Location = new System.Drawing.Point(0, 164);
            this.pnlAQIIndicator.Name = "pnlAQIIndicator";
            this.pnlAQIIndicator.Size = new System.Drawing.Size(280, 6);
            this.pnlAQIIndicator.TabIndex = 3;

            // 
            // pnlDetailsCard
            // 
            this.pnlDetailsCard.BackColor = System.Drawing.Color.White;
            this.pnlDetailsCard.Controls.Add(this.lblDominantPollutant);
            this.pnlDetailsCard.Controls.Add(this.lblHumValue);
            this.pnlDetailsCard.Controls.Add(this.lblHumHeader);
            this.pnlDetailsCard.Controls.Add(this.lblTempValue);
            this.pnlDetailsCard.Controls.Add(this.lblTempHeader);
            this.pnlDetailsCard.Controls.Add(this.lblNO2Value);
            this.pnlDetailsCard.Controls.Add(this.lblNO2Header);
            this.pnlDetailsCard.Controls.Add(this.lblCO2Value);
            this.pnlDetailsCard.Controls.Add(this.lblCO2Header);
            this.pnlDetailsCard.Controls.Add(this.lblPM10Value);
            this.pnlDetailsCard.Controls.Add(this.lblPM10Header);
            this.pnlDetailsCard.Controls.Add(this.lblPM25Value);
            this.pnlDetailsCard.Controls.Add(this.lblPM25Header);
            this.pnlDetailsCard.Location = new System.Drawing.Point(315, 75);
            this.pnlDetailsCard.Name = "pnlDetailsCard";
            this.pnlDetailsCard.Size = new System.Drawing.Size(370, 170);
            this.pnlDetailsCard.TabIndex = 2;

            // 
            // lblPM25Header
            // 
            this.lblPM25Header.AutoSize = true;
            this.lblPM25Header.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPM25Header.ForeColor = System.Drawing.Color.DimGray;
            this.lblPM25Header.Location = new System.Drawing.Point(15, 15);
            this.lblPM25Header.Name = "lblPM25Header";
            this.lblPM25Header.Size = new System.Drawing.Size(43, 15);
            this.lblPM25Header.TabIndex = 0;
            this.lblPM25Header.Text = "PM2.5:";

            // 
            // lblPM25Value
            // 
            this.lblPM25Value.AutoSize = true;
            this.lblPM25Value.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPM25Value.Location = new System.Drawing.Point(70, 14);
            this.lblPM25Value.Name = "lblPM25Value";
            this.lblPM25Value.Size = new System.Drawing.Size(20, 17);
            this.lblPM25Value.TabIndex = 1;
            this.lblPM25Value.Text = "--";

            // 
            // lblPM10Header
            // 
            this.lblPM10Header.AutoSize = true;
            this.lblPM10Header.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPM10Header.ForeColor = System.Drawing.Color.DimGray;
            this.lblPM10Header.Location = new System.Drawing.Point(190, 15);
            this.lblPM10Header.Name = "lblPM10Header";
            this.lblPM10Header.Size = new System.Drawing.Size(38, 15);
            this.lblPM10Header.TabIndex = 2;
            this.lblPM10Header.Text = "PM10:";

            // 
            // lblPM10Value
            // 
            this.lblPM10Value.AutoSize = true;
            this.lblPM10Value.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPM10Value.Location = new System.Drawing.Point(245, 14);
            this.lblPM10Value.Name = "lblPM10Value";
            this.lblPM10Value.Size = new System.Drawing.Size(20, 17);
            this.lblPM10Value.TabIndex = 3;
            this.lblPM10Value.Text = "--";

            // 
            // lblCO2Header
            // 
            this.lblCO2Header.AutoSize = true;
            this.lblCO2Header.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCO2Header.ForeColor = System.Drawing.Color.DimGray;
            this.lblCO2Header.Location = new System.Drawing.Point(15, 52);
            this.lblCO2Header.Name = "lblCO2Header";
            this.lblCO2Header.Size = new System.Drawing.Size(33, 15);
            this.lblCO2Header.TabIndex = 4;
            this.lblCO2Header.Text = "CO2:";

            // 
            // lblCO2Value
            // 
            this.lblCO2Value.AutoSize = true;
            this.lblCO2Value.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCO2Value.Location = new System.Drawing.Point(70, 51);
            this.lblCO2Value.Name = "lblCO2Value";
            this.lblCO2Value.Size = new System.Drawing.Size(20, 17);
            this.lblCO2Value.TabIndex = 5;
            this.lblCO2Value.Text = "--";

            // 
            // lblNO2Header
            // 
            this.lblNO2Header.AutoSize = true;
            this.lblNO2Header.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblNO2Header.ForeColor = System.Drawing.Color.DimGray;
            this.lblNO2Header.Location = new System.Drawing.Point(190, 52);
            this.lblNO2Header.Name = "lblNO2Header";
            this.lblNO2Header.Size = new System.Drawing.Size(34, 15);
            this.lblNO2Header.TabIndex = 6;
            this.lblNO2Header.Text = "NO2:";

            // 
            // lblNO2Value
            // 
            this.lblNO2Value.AutoSize = true;
            this.lblNO2Value.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNO2Value.Location = new System.Drawing.Point(245, 51);
            this.lblNO2Value.Name = "lblNO2Value";
            this.lblNO2Value.Size = new System.Drawing.Size(20, 17);
            this.lblNO2Value.TabIndex = 7;
            this.lblNO2Value.Text = "--";

            // 
            // lblTempHeader
            // 
            this.lblTempHeader.AutoSize = true;
            this.lblTempHeader.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblTempHeader.ForeColor = System.Drawing.Color.DimGray;
            this.lblTempHeader.Location = new System.Drawing.Point(15, 89);
            this.lblTempHeader.Name = "lblTempHeader";
            this.lblTempHeader.Size = new System.Drawing.Size(39, 15);
            this.lblTempHeader.TabIndex = 8;
            this.lblTempHeader.Text = "Temp:";

            // 
            // lblTempValue
            // 
            this.lblTempValue.AutoSize = true;
            this.lblTempValue.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTempValue.Location = new System.Drawing.Point(70, 88);
            this.lblTempValue.Name = "lblTempValue";
            this.lblTempValue.Size = new System.Drawing.Size(20, 17);
            this.lblTempValue.TabIndex = 9;
            this.lblTempValue.Text = "--";

            // 
            // lblHumHeader
            // 
            this.lblHumHeader.AutoSize = true;
            this.lblHumHeader.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblHumHeader.ForeColor = System.Drawing.Color.DimGray;
            this.lblHumHeader.Location = new System.Drawing.Point(190, 89);
            this.lblHumHeader.Name = "lblHumHeader";
            this.lblHumHeader.Size = new System.Drawing.Size(59, 15);
            this.lblHumHeader.TabIndex = 10;
            this.lblHumHeader.Text = "Humidity:";

            // 
            // lblHumValue
            // 
            this.lblHumValue.AutoSize = true;
            this.lblHumValue.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblHumValue.Location = new System.Drawing.Point(255, 88);
            this.lblHumValue.Name = "lblHumValue";
            this.lblHumValue.Size = new System.Drawing.Size(20, 17);
            this.lblHumValue.TabIndex = 11;
            this.lblHumValue.Text = "--";

            // 
            // lblDominantPollutant
            // 
            this.lblDominantPollutant.AutoSize = true;
            this.lblDominantPollutant.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblDominantPollutant.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.lblDominantPollutant.Location = new System.Drawing.Point(15, 130);
            this.lblDominantPollutant.Name = "lblDominantPollutant";
            this.lblDominantPollutant.Size = new System.Drawing.Size(127, 15);
            this.lblDominantPollutant.TabIndex = 12;
            this.lblDominantPollutant.Text = "Dominant Pollutant: --";

            // 
            // pnlChartCard
            // 
            this.pnlChartCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlChartCard.BackColor = System.Drawing.Color.White;
            this.pnlChartCard.Controls.Add(this.pnlChartContainer);
            this.pnlChartCard.Controls.Add(this.lblChartTitle);
            this.pnlChartCard.Location = new System.Drawing.Point(20, 260);
            this.pnlChartCard.Name = "pnlChartCard";
            this.pnlChartCard.Size = new System.Drawing.Size(665, 260);
            this.pnlChartCard.TabIndex = 3;

            // 
            // lblChartTitle
            // 
            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.lblChartTitle.Location = new System.Drawing.Point(15, 12);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Size = new System.Drawing.Size(117, 19);
            this.lblChartTitle.TabIndex = 0;
            this.lblChartTitle.Text = "AQI Trend Line";

            // 
            // pnlChartContainer
            // 
            this.pnlChartContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChartContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlChartContainer.Location = new System.Drawing.Point(15, 40);
            this.pnlChartContainer.Name = "pnlChartContainer";
            this.pnlChartContainer.Size = new System.Drawing.Size(635, 205);
            this.pnlChartContainer.TabIndex = 1;
            this.pnlChartContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlChartContainer_Paint);

            // 
            // pnlGridCard
            // 
            this.pnlGridCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGridCard.BackColor = System.Drawing.Color.White;
            this.pnlGridCard.Controls.Add(this.dgvLogs);
            this.pnlGridCard.Controls.Add(this.lblLogTitle);
            this.pnlGridCard.Location = new System.Drawing.Point(700, 75);
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Size = new System.Drawing.Size(540, 445);
            this.pnlGridCard.TabIndex = 4;

            // 
            // lblLogTitle
            // 
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.lblLogTitle.Location = new System.Drawing.Point(15, 12);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Size = new System.Drawing.Size(107, 19);
            this.lblLogTitle.TabIndex = 0;
            this.lblLogTitle.Text = "Telemetry Logs";

            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.AllowUserToResizeRows = false;
            this.dgvLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.dgvLogs.Location = new System.Drawing.Point(15, 40);
            this.dgvLogs.MultiSelect = false;
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.Size = new System.Drawing.Size(510, 390);
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
            this.colTemp.HeaderText = "Temp (°C)";
            this.colTemp.Name = "colTemp";
            this.colTemp.ReadOnly = true;

            // 
            // colHum
            // 
            this.colHum.HeaderText = "Humidity (%)";
            this.colHum.Name = "colHum";
            this.colHum.ReadOnly = true;

            // 
            // CurrentDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.pnlChartCard);
            this.Controls.Add(this.pnlDetailsCard);
            this.Controls.Add(this.pnlAQICard);
            this.Controls.Add(this.pnlHeader);
            this.Name = "CurrentDataControl";
            this.Size = new System.Drawing.Size(1260, 540);
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
    }
}
