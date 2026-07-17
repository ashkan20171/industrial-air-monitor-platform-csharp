namespace AshkanAQMS
{
    partial class AnalyzerConfigForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.lblDecimals = new System.Windows.Forms.Label();
            this.numDecimals = new System.Windows.Forms.NumericUpDown();
            this.lblChannel = new System.Windows.Forms.Label();
            this.numChannel = new System.Windows.Forms.NumericUpDown();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.lblGasType = new System.Windows.Forms.Label();
            this.cmbGasType = new System.Windows.Forms.ComboBox();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.pnlNetwork = new System.Windows.Forms.Panel();
            this.lblIpPort = new System.Windows.Forms.Label();
            this.numIpPort = new System.Windows.Forms.NumericUpDown();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.lblIpAddress = new System.Windows.Forms.Label();
            this.lblNetworkNote = new System.Windows.Forms.Label();
            this.pnlSerial = new System.Windows.Forms.Panel();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.lblComPort = new System.Windows.Forms.Label();
            this.cmbComPort = new System.Windows.Forms.ComboBox();
            this.lblSerialNote = new System.Windows.Forms.Label();
            this.rdbIP = new System.Windows.Forms.RadioButton();
            this.rdbCOM = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDecimals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChannel)).BeginInit();
            this.grpConnection.SuspendLayout();
            this.pnlNetwork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIpPort)).BeginInit();
            this.pnlSerial.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpDetails
            // 
            this.grpDetails.Controls.Add(this.lblDecimals);
            this.grpDetails.Controls.Add(this.numDecimals);
            this.grpDetails.Controls.Add(this.lblChannel);
            this.grpDetails.Controls.Add(this.numChannel);
            this.grpDetails.Controls.Add(this.lblUnit);
            this.grpDetails.Controls.Add(this.cmbUnit);
            this.grpDetails.Controls.Add(this.lblGasType);
            this.grpDetails.Controls.Add(this.cmbGasType);
            this.grpDetails.Controls.Add(this.txtModel);
            this.grpDetails.Controls.Add(this.lblModel);
            this.grpDetails.Controls.Add(this.txtName);
            this.grpDetails.Controls.Add(this.lblName);
            this.grpDetails.Location = new System.Drawing.Point(12, 12);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(396, 240);
            this.grpDetails.TabIndex = 0;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Analyzer Details";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(17, 31);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(41, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(110, 28);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(260, 23);
            this.txtName.TabIndex = 1;
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(17, 66);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(45, 15);
            this.lblModel.TabIndex = 2;
            this.lblModel.Text = "Model:";
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(110, 63);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(260, 23);
            this.txtModel.TabIndex = 3;
            // 
            // lblGasType
            // 
            this.lblGasType.AutoSize = true;
            this.lblGasType.Location = new System.Drawing.Point(17, 101);
            this.lblGasType.Name = "lblGasType";
            this.lblGasType.Size = new System.Drawing.Size(56, 15);
            this.lblGasType.TabIndex = 4;
            this.lblGasType.Text = "Gas Type:";
            // 
            // cmbGasType
            // 
            this.cmbGasType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGasType.FormattingEnabled = true;
            this.cmbGasType.Items.AddRange(new object[] {
            "PM2.5",
            "PM10",
            "CO2",
            "NO2",
            "O3",
            "SO2",
            "Temperature",
            "Humidity"});
            this.cmbGasType.Location = new System.Drawing.Point(110, 98);
            this.cmbGasType.Name = "cmbGasType";
            this.cmbGasType.Size = new System.Drawing.Size(160, 23);
            this.cmbGasType.TabIndex = 5;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(17, 136);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(33, 15);
            this.lblUnit.TabIndex = 6;
            this.lblUnit.Text = "Unit:";
            // 
            // cmbUnit
            // 
            this.cmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnit.FormattingEnabled = true;
            this.cmbUnit.Items.AddRange(new object[] {
            "µg/m³",
            "ppm",
            "%",
            "°C"});
            this.cmbUnit.Location = new System.Drawing.Point(110, 133);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Size = new System.Drawing.Size(160, 23);
            this.cmbUnit.TabIndex = 7;
            // 
            // lblChannel
            // 
            this.lblChannel.AutoSize = true;
            this.lblChannel.Location = new System.Drawing.Point(17, 174);
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(53, 15);
            this.lblChannel.TabIndex = 8;
            this.lblChannel.Text = "Channel:";
            // 
            // numChannel
            // 
            this.numChannel.Location = new System.Drawing.Point(110, 172);
            this.numChannel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numChannel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numChannel.Name = "numChannel";
            this.numChannel.Size = new System.Drawing.Size(80, 23);
            this.numChannel.TabIndex = 9;
            this.numChannel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDecimals
            // 
            this.lblDecimals.AutoSize = true;
            this.lblDecimals.Location = new System.Drawing.Point(202, 174);
            this.lblDecimals.Name = "lblDecimals";
            this.lblDecimals.Size = new System.Drawing.Size(86, 15);
            this.lblDecimals.TabIndex = 10;
            this.lblDecimals.Text = "Decimal Digits:";
            // 
            // numDecimals
            // 
            this.numDecimals.Location = new System.Drawing.Point(298, 172);
            this.numDecimals.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numDecimals.Name = "numDecimals";
            this.numDecimals.Size = new System.Drawing.Size(72, 23);
            this.numDecimals.TabIndex = 11;
            this.numDecimals.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.pnlNetwork);
            this.grpConnection.Controls.Add(this.pnlSerial);
            this.grpConnection.Controls.Add(this.rdbIP);
            this.grpConnection.Controls.Add(this.rdbCOM);
            this.grpConnection.Location = new System.Drawing.Point(12, 258);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(396, 205);
            this.grpConnection.TabIndex = 1;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Connection";
            // 
            // rdbCOM
            // 
            this.rdbCOM.AutoSize = true;
            this.rdbCOM.Checked = true;
            this.rdbCOM.Location = new System.Drawing.Point(20, 28);
            this.rdbCOM.Name = "rdbCOM";
            this.rdbCOM.Size = new System.Drawing.Size(144, 19);
            this.rdbCOM.TabIndex = 0;
            this.rdbCOM.TabStop = true;
            this.rdbCOM.Text = "Serial (COM / RS-232)";
            this.rdbCOM.UseVisualStyleBackColor = true;
            this.rdbCOM.CheckedChanged += new System.EventHandler(this.rdbConnectionType_CheckedChanged);
            // 
            // rdbIP
            // 
            this.rdbIP.AutoSize = true;
            this.rdbIP.Location = new System.Drawing.Point(190, 28);
            this.rdbIP.Name = "rdbIP";
            this.rdbIP.Size = new System.Drawing.Size(104, 19);
            this.rdbIP.TabIndex = 1;
            this.rdbIP.Text = "Ethernet / TCP";
            this.rdbIP.UseVisualStyleBackColor = true;
            this.rdbIP.CheckedChanged += new System.EventHandler(this.rdbConnectionType_CheckedChanged);
            // 
            // pnlSerial
            // 
            this.pnlSerial.Controls.Add(this.lblBaudRate);
            this.pnlSerial.Controls.Add(this.cmbBaudRate);
            this.pnlSerial.Controls.Add(this.lblComPort);
            this.pnlSerial.Controls.Add(this.cmbComPort);
            this.pnlSerial.Controls.Add(this.lblSerialNote);
            this.pnlSerial.Location = new System.Drawing.Point(12, 54);
            this.pnlSerial.Name = "pnlSerial";
            this.pnlSerial.Size = new System.Drawing.Size(372, 67);
            this.pnlSerial.TabIndex = 2;
            // 
            // lblComPort
            // 
            this.lblComPort.AutoSize = true;
            this.lblComPort.Location = new System.Drawing.Point(3, 10);
            this.lblComPort.Name = "lblComPort";
            this.lblComPort.Size = new System.Drawing.Size(41, 15);
            this.lblComPort.TabIndex = 0;
            this.lblComPort.Text = "Port:";
            // 
            // cmbComPort
            // 
            this.cmbComPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComPort.FormattingEnabled = true;
            this.cmbComPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5"});
            this.cmbComPort.Location = new System.Drawing.Point(60, 7);
            this.cmbComPort.Name = "cmbComPort";
            this.cmbComPort.Size = new System.Drawing.Size(90, 23);
            this.cmbComPort.TabIndex = 1;
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(168, 10);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(33, 15);
            this.lblBaudRate.TabIndex = 2;
            this.lblBaudRate.Text = "Baud:";
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "115200"});
            this.cmbBaudRate.Location = new System.Drawing.Point(215, 7);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(90, 23);
            this.cmbBaudRate.TabIndex = 3;
            // 
            // lblSerialNote
            // 
            this.lblSerialNote.AutoSize = true;
            this.lblSerialNote.ForeColor = System.Drawing.Color.DimGray;
            this.lblSerialNote.Location = new System.Drawing.Point(3, 40);
            this.lblSerialNote.Name = "lblSerialNote";
            this.lblSerialNote.Size = new System.Drawing.Size(288, 15);
            this.lblSerialNote.TabIndex = 4;
            this.lblSerialNote.Text = "Serial settings are used when COM connection is selected.";
            // 
            // pnlNetwork
            // 
            this.pnlNetwork.Controls.Add(this.lblIpPort);
            this.pnlNetwork.Controls.Add(this.numIpPort);
            this.pnlNetwork.Controls.Add(this.txtIpAddress);
            this.pnlNetwork.Controls.Add(this.lblIpAddress);
            this.pnlNetwork.Controls.Add(this.lblNetworkNote);
            this.pnlNetwork.Location = new System.Drawing.Point(12, 122);
            this.pnlNetwork.Name = "pnlNetwork";
            this.pnlNetwork.Size = new System.Drawing.Size(372, 70);
            this.pnlNetwork.TabIndex = 3;
            // 
            // lblIpAddress
            // 
            this.lblIpAddress.AutoSize = true;
            this.lblIpAddress.Location = new System.Drawing.Point(3, 9);
            this.lblIpAddress.Name = "lblIpAddress";
            this.lblIpAddress.Size = new System.Drawing.Size(65, 15);
            this.lblIpAddress.TabIndex = 0;
            this.lblIpAddress.Text = "IP Address:";
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(74, 6);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(120, 23);
            this.txtIpAddress.TabIndex = 1;
            // 
            // lblIpPort
            // 
            this.lblIpPort.AutoSize = true;
            this.lblIpPort.Location = new System.Drawing.Point(212, 9);
            this.lblIpPort.Name = "lblIpPort";
            this.lblIpPort.Size = new System.Drawing.Size(32, 15);
            this.lblIpPort.TabIndex = 2;
            this.lblIpPort.Text = "Port:";
            // 
            // numIpPort
            // 
            this.numIpPort.Location = new System.Drawing.Point(250, 6);
            this.numIpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numIpPort.Name = "numIpPort";
            this.numIpPort.Size = new System.Drawing.Size(90, 23);
            this.numIpPort.TabIndex = 3;
            this.numIpPort.Value = new decimal(new int[] {
            502,
            0,
            0,
            0});
            // 
            // lblNetworkNote
            // 
            this.lblNetworkNote.AutoSize = true;
            this.lblNetworkNote.ForeColor = System.Drawing.Color.DimGray;
            this.lblNetworkNote.Location = new System.Drawing.Point(3, 40);
            this.lblNetworkNote.Name = "lblNetworkNote";
            this.lblNetworkNote.Size = new System.Drawing.Size(293, 15);
            this.lblNetworkNote.TabIndex = 4;
            this.lblNetworkNote.Text = "Network settings are used when Ethernet / TCP is selected.";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(232, 472);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(328, 472);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AnalyzerConfigForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(420, 515);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpConnection);
            this.Controls.Add(this.grpDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnalyzerConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Analyzer Detailed Configuration";
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDecimals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChannel)).EndInit();
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.pnlNetwork.ResumeLayout(false);
            this.pnlNetwork.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIpPort)).EndInit();
            this.pnlSerial.ResumeLayout(false);
            this.pnlSerial.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblDecimals;
        private System.Windows.Forms.NumericUpDown numDecimals;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.NumericUpDown numChannel;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.Label lblGasType;
        private System.Windows.Forms.ComboBox cmbGasType;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.Panel pnlNetwork;
        private System.Windows.Forms.Label lblIpPort;
        private System.Windows.Forms.NumericUpDown numIpPort;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label lblIpAddress;
        private System.Windows.Forms.Label lblNetworkNote;
        private System.Windows.Forms.Panel pnlSerial;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Label lblComPort;
        private System.Windows.Forms.ComboBox cmbComPort;
        private System.Windows.Forms.Label lblSerialNote;
        private System.Windows.Forms.RadioButton rdbIP;
        private System.Windows.Forms.RadioButton rdbCOM;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}
