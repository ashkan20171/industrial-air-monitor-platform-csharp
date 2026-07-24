namespace AshkanAQMS
{
    partial class CurrentDataControl
    {
        private System.ComponentModel.IContainer components = null;

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
        private System.Windows.Forms.Label lblDominantPollutant;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblTimestamp = new System.Windows.Forms.Label();

            this.pnlAQICard = new System.Windows.Forms.Panel();
            this.lblAQITitle = new System.Windows.Forms.Label();
            this.lblAQIValue = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlAQIIndicator = new System.Windows.Forms.Panel();

            this.pnlDetailsCard = new System.Windows.Forms.Panel();
            this.lblPM25Header = new System.Windows.Forms.Label();
            this.lblPM25 = new System.Windows.Forms.Label();
            this.lblPM10Header = new System.Windows.Forms.Label();
            this.lblPM10 = new System.Windows.Forms.Label();
            this.lblCO2Header = new System.Windows.Forms.Label();
            this.lblCO2 = new System.Windows.Forms.Label();
            this.lblNO2Header = new System.Windows.Forms.Label();
            this.lblNO2 = new System.Windows.Forms.Label();
            this.lblTempHeader = new System.Windows.Forms.Label();
            this.lblTemp = new System.Windows.Forms.Label();
            this.lblHumHeader = new System.Windows.Forms.Label();
            this.lblHum = new System.Windows.Forms.Label();
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

            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.Controls.Add(this.lblAppName);
            this.pnlHeader.Controls.Add(this.lblTimestamp);

            // lblAppName
            this.lblAppName.AutoSize = true;
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAppName.Location = new System.Drawing.Point(20, 15);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Text = "Current Air Quality";

            // lblTimestamp
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblTimestamp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblTimestamp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblTimestamp.Location = new System.Drawing.Point(900, 20);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Text = "Last update: --:--:--";

            // pnlAQICard
            this.pnlAQICard.BackColor = System.Drawing.Color.White;
            this.pnlAQICard.Location = new System.Drawing.Point(20, 80);
            this.pnlAQICard.Name = "pnlAQICard";
            this.pnlAQICard.Size = new System.Drawing.Size(280, 160);
            this.pnlAQICard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAQICard.Controls.Add(this.lblAQITitle);
            this.pnlAQICard.Controls.Add(this.lblAQIValue);
            this.pnlAQICard.Controls.Add(this.lblStatus);
            this.pnlAQICard.Controls.Add(this.pnlAQIIndicator);

            // lblAQITitle
            this.lblAQITitle.AutoSize = true;
            this.lblAQITitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAQITitle.Location = new System.Drawing.Point(15, 15);
            this.lblAQITitle.Name = "lblAQITitle";
            this.lblAQITitle.Text = "AQI";

            // lblAQIValue
            this.lblAQIValue.AutoSize = true;
            this.lblAQIValue.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblAQIValue.Location = new System.Drawing.Point(15, 45);
            this.lblAQIValue.Name = "lblAQIValue";
            this.lblAQIValue.Text = "0";

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(18, 105);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "Unknown";

            // pnlAQIIndicator
            this.pnlAQIIndicator.Location = new System.Drawing.Point(220, 15);
            this.pnlAQIIndicator.Name = "pnlAQIIndicator";
            this.pnlAQIIndicator.Size = new System.Drawing.Size(40, 40);
            this.pnlAQIIndicator.BackColor = System.Drawing.Color.Gray;
            this.pnlAQIIndicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // pnlDetailsCard
            this.pnlDetailsCard.BackColor = System.Drawing.Color.White;
            this.pnlDetailsCard.Location = new System.Drawing.Point(320, 80);
            this.pnlDetailsCard.Name = "pnlDetailsCard";
            this.pnlDetailsCard.Size = new System.Drawing.Size(360, 160);
            this.pnlDetailsCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetailsCard.Controls.Add(this.lblPM25Header);
            this.pnlDetailsCard.Controls.Add(this.lblPM25);
            this.pnlDetailsCard.Controls.Add(this.lblPM10Header);
            this.pnlDetailsCard.Controls.Add(this.lblPM10);
            this.pnlDetailsCard.Controls.Add(this.lblCO2Header);
            this.pnlDetailsCard.Controls.Add(this.lblCO2);
            this.pnlDetailsCard.Controls.Add(this.lblNO2Header);
            this.pnlDetailsCard.Controls.Add(this.lblNO2);
            this.pnlDetailsCard.Controls.Add(this.lblTempHeader);
            this.pnlDetailsCard.Controls.Add(this.lblTemp);
            this.pnlDetailsCard.Controls.Add(this.lblHumHeader);
            this.pnlDetailsCard.Controls.Add(this.lblHum);
            this.pnlDetailsCard.Controls.Add(this.lblDominantPollutant);

            // lblPM25Header
            this.lblPM25Header.AutoSize = true;
            this.lblPM25Header.Location = new System.Drawing.Point(15, 15);
            this.lblPM25Header.Name = "lblPM25Header";
            this.lblPM25Header.Text = "PM2.5:";

            // lblPM25
            this.lblPM25.AutoSize = true;
            this.lblPM25.Location = new System.Drawing.Point(110, 15);
            this.lblPM25.Name = "lblPM25";
            this.lblPM25.Text = "0.0";

            // lblPM10Header
            this.lblPM10Header.AutoSize = true;
            this.lblPM10Header.Location = new System.Drawing.Point(15, 40);
            this.lblPM10Header.Name = "lblPM10Header";
            this.lblPM10Header.Text = "PM10:";

            // lblPM10
            this.lblPM10.AutoSize = true;
            this.lblPM10.Location = new System.Drawing.Point(110, 40);
            this.lblPM10.Name = "lblPM10";
            this.lblPM10.Text = "0.0";

            // lblCO2Header
            this.lblCO2Header.AutoSize = true;
            this.lblCO2Header.Location = new System.Drawing.Point(15, 65);
            this.lblCO2Header.Name = "lblCO2Header";
            this.lblCO2Header.Text = "CO2:";

            // lblCO2
            this.lblCO2.AutoSize = true;
            this.lblCO2.Location = new System.Drawing.Point(110, 65);
            this.lblCO2.Name = "lblCO2";
            this.lblCO2.Text = "0";

            // lblNO2Header
            this.lblNO2Header.AutoSize = true;
            this.lblNO2Header.Location = new System.Drawing.Point(15, 90);
            this.lblNO2Header.Name = "lblNO2Header";
            this.lblNO2Header.Text = "NO2:";

            // lblNO2
            this.lblNO2.AutoSize = true;
            this.lblNO2.Location = new System.Drawing.Point(110, 90);
            this.lblNO2.Name = "lblNO2";
            this.lblNO2.Text = "0.0";

            // lblTempHeader
            this.lblTempHeader.AutoSize = true;
            this.lblTempHeader.Location = new System.Drawing.Point(190, 15);
            this.lblTempHeader.Name = "lblTempHeader";
            this.lblTempHeader.Text = "Temp:";

            // lblTemp
            this.lblTemp.AutoSize = true;
            this.lblTemp.Location = new System.Drawing.Point(285, 15);
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Text = "0.0";

            // lblHumHeader
            this.lblHumHeader.AutoSize = true;
            this.lblHumHeader.Location = new System.Drawing.Point(190, 40);
            this.lblHumHeader.Name = "lblHumHeader";
            this.lblHumHeader.Text = "Hum:";

            // lblHum
            this.lblHum.AutoSize = true;
            this.lblHum.Location = new System.Drawing.Point(285, 40);
            this.lblHum.Name = "lblHum";
            this.lblHum.Text = "0.0";

            // lblDominantPollutant
            this.lblDominantPollutant.AutoSize = true;
            this.lblDominantPollutant.Location = new System.Drawing.Point(190, 90);
            this.lblDominantPollutant.Name = "lblDominantPollutant";
            this.lblDominantPollutant.Text = "Dominant: Unknown";

            // pnlChartCard
            this.pnlChartCard.BackColor = System.Drawing.Color.White;
            this.pnlChartCard.Location = new System.Drawing.Point(20, 260);
            this.pnlChartCard.Name = "pnlChartCard";
            this.pnlChartCard.Size = new System.Drawing.Size(660, 240);
            this.pnlChartCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChartCard.Controls.Add(this.lblChartTitle);
            this.pnlChartCard.Controls.Add(this.pnlChartContainer);

            // lblChartTitle
            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.Location = new System.Drawing.Point(15, 12);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Text = "AQI Trend";

            // pnlChartContainer
            this.pnlChartContainer.Location = new System.Drawing.Point(15, 40);
            this.pnlChartContainer.Name = "pnlChartContainer";
            this.pnlChartContainer.Size = new System.Drawing.Size(630, 185);
            this.pnlChartContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlChartContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlChartContainer_Paint);

            // pnlGridCard
            this.pnlGridCard.BackColor = System.Drawing.Color.White;
            this.pnlGridCard.Location = new System.Drawing.Point(700, 80);
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Size = new System.Drawing.Size(540, 420);
            this.pnlGridCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGridCard.Controls.Add(this.lblLogTitle);
            this.pnlGridCard.Controls.Add(this.dgvLogs);

            // lblLogTitle
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.Location = new System.Drawing.Point(15, 12);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Text = "Recent Logs";

            // dgvLogs
            this.dgvLogs.Location = new System.Drawing.Point(15, 40);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.Size = new System.Drawing.Size(510, 360);
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.MultiSelect = false;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colTime,
                this.colAQI,
                this.colPM25,
                this.colCO2,
                this.colTemp,
                this.colHum
            });

            // colTime
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;

            // colAQI
            this.colAQI.HeaderText = "AQI";
            this.colAQI.Name = "colAQI";
            this.colAQI.ReadOnly = true;

            // colPM25
            this.colPM25.HeaderText = "PM2.5";
            this.colPM25.Name = "colPM25";
            this.colPM25.ReadOnly = true;

            // colCO2
            this.colCO2.HeaderText = "CO2";
            this.colCO2.Name = "colCO2";
            this.colCO2.ReadOnly = true;

            // colTemp
            this.colTemp.HeaderText = "Temp";
            this.colTemp.Name = "colTemp";
            this.colTemp.ReadOnly = true;

            // colHum
            this.colHum.HeaderText = "Hum";
            this.colHum.Name = "colHum";
            this.colHum.ReadOnly = true;

            // btnStartStop
            this.btnStartStop.Location = new System.Drawing.Point(700, 20);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(110, 35);
            this.btnStartStop.Text = "Stop";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);

            // btnExport
            this.btnExport.Location = new System.Drawing.Point(820, 20);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(110, 35);
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            // CurrentDataControl
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlAQICard);
            this.Controls.Add(this.pnlDetailsCard);
            this.Controls.Add(this.pnlChartCard);
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.btnExport);
            this.Name = "CurrentDataControl";
            this.Size = new System.Drawing.Size(1260, 540);

            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
