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

        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.lblGasType = new System.Windows.Forms.Label();
            this.cmbGasType = new System.Windows.Forms.ComboBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.lblChannel = new System.Windows.Forms.Label();
            this.numChannel = new System.Windows.Forms.NumericUpDown();
            this.lblDecimals = new System.Windows.Forms.Label();
            this.numDecimals = new System.Windows.Forms.NumericUpDown();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.rdbCOM = new System.Windows.Forms.RadioButton();
            this.rdbIP = new System.Windows.Forms.RadioButton();
            this.pnlSerial = new System.Windows.Forms.Panel();
            this.lblComPort = new System.Windows.Forms.Label();
            this.cmbComPort = new System.Windows.Forms.ComboBox();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.pnlNetwork = new System.Windows.Forms.Panel();
            this.lblIpAddress = new System.Windows.Forms.Label();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.lblIpPort = new System.Windows.Forms.Label();
            this.numIpPort = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.numChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDecimals)).BeginInit();
            this.grpConnection.SuspendLayout();
            this.pnlSerial.SuspendLayout();
            this.pnlNetwork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIpPort)).BeginInit();
            this.SuspendLayout();

            // Form properties
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.ClientSize = new System.Drawing.Size(420, 520);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnalyzerConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Analyzer Detailed Configuration";

            // lblName & txtName
            this.lblName.Location = new System.Drawing.Point(20, 20);
            this.lblName.Size = new System.Drawing.Size(120, 20);
            this.lblName.Text = "Analyzer Name:";
            this.txtName.Location = new System.Drawing.Point(150, 18);
            this.txtName.Size = new System.Drawing.Size(240, 23);

            // lblModel & txtModel
            this.lblModel.Location = new System.Drawing.Point(20, 55);
            this.lblModel.Size = new System.Drawing.Size(120, 20);
            this.lblModel.Text = "Model:";
            this.txtModel.Location = new System.Drawing.Point(150, 53);
            this.txtModel.Size = new System.Drawing.Size(240, 23);

            // lblGasType & cmbGasType
            this.lblGasType.Location = new System.Drawing.Point(20, 90);
            this.lblGasType.Size = new System.Drawing.Size(120, 20);
            this.lblGasType.Text = "Gas Type:";
            this.cmbGasType.Location = new System.Drawing.Point(150, 88);
            this.cmbGasType.Size = new System.Drawing.Size(240, 23);
            this.cmbGasType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGasType.Items.AddRange(new object[] { "PM2.5", "PM10", "CO2", "NO2", "O3", "SO2", "Temperature", "Humidity" });

            // lblUnit & cmbUnit
            this.lblUnit.Location = new System.Drawing.Point(20, 125);
            this.lblUnit.Size = new System.Drawing.Size(120, 20);
            this.lblUnit.Text = "Unit:";
            this.cmbUnit.Location = new System.Drawing.Point(150, 123);
            this.cmbUnit.Size = new System.Drawing.Size(240, 23);
            this.cmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnit.Items.AddRange(new object[] { "µg/m³", "ppm", "%", "°C" });

            // lblChannel & numChannel
            this.lblChannel.Location = new System.Drawing.Point(20, 160);
            this.lblChannel.Size = new System.Drawing.Size(120, 20);
            this.lblChannel.Text = "Modbus Channel:";
            this.numChannel.Location = new System.Drawing.Point(150, 158);
            this.numChannel.Size = new System.Drawing.Size(80, 23);
            this.numChannel.Minimum = 1;
            this.numChannel.Maximum = 255;

            // Decimal digits input (فرمول رقم اعشار)
            this.lblDecimals.Location = new System.Drawing.Point(20, 195);
            this.lblDecimals.Size = new System.Drawing.Size(120, 20);
            this.lblDecimals.Text = "Decimal Digits:";
            this.numDecimals.Location = new System.Drawing.Point(150, 193);
            this.numDecimals.Size = new System.Drawing.Size(80, 23);
            this.numDecimals.Minimum = 0;
            this.numDecimals.Maximum = 4;

            // GroupBox Connection
            this.grpConnection.Text = "Connection parameters";
            this.grpConnection.Location = new System.Drawing.Point(20, 235);
            this.grpConnection.Size = new System.Drawing.Size(370, 210);

            // Connection Radio Buttons
            this.rdbCOM.Text = "Serial Port (COM)";
            this.rdbCOM.Location = new System.Drawing.Point(20, 25);
            this.rdbCOM.Size = new System.Drawing.Size(130, 20);
            this.rdbCOM.Checked = true;
            this.rdbCOM.CheckedChanged += new System.EventHandler(this.rdbConnectionType_CheckedChanged);

            this.rdbIP.Text = "Ethernet (TCP/IP)";
            this.rdbIP.Location = new System.Drawing.Point(170, 25);
            this.rdbIP.Size = new System.Drawing.Size(130, 20);

            // Serial Panel
            this.pnlSerial.Location = new System.Drawing.Point(15, 55);
            this.pnlSerial.Size = new System.Drawing.Size(340, 70);

            this.lblComPort.Text = "Port:";
            this.lblComPort.Location = new System.Drawing.Point(5, 10);
            this.lblComPort.Size = new System.Drawing.Size(60, 20);
            this.cmbComPort.Location = new System.Drawing.Point(80, 8);
            this.cmbComPort.Size = new System.Drawing.Size(240, 23);
            this.cmbComPort.Items.AddRange(new object[] { "COM1", "COM2", "COM3", "COM4", "COM5" });

            this.lblBaudRate.Text = "Baud Rate:";
            this.lblBaudRate.Location = new System.Drawing.Point(5, 40);
            this.lblBaudRate.Size = new System.Drawing.Size(70, 20);
            this.cmbBaudRate.Location = new System.Drawing.Point(80, 38);
            this.cmbBaudRate.Size = new System.Drawing.Size(240, 23);
            this.cmbBaudRate.Items.AddRange(new object[] { "4800", "9600", "19200", "115200" });

            // Network Panel
            this.pnlNetwork.Location = new System.Drawing.Point(15, 130);
            this.pnlNetwork.Size = new System.Drawing.Size(340, 70);

            this.lblIpAddress.Text = "IP Address:";
            this.lblIpAddress.Location = new System.Drawing.Point(5, 10);
            this.lblIpAddress.Size = new System.Drawing.Size(70, 20);
            this.txtIpAddress.Location = new System.Drawing.Point(80, 8);
            this.txtIpAddress.Size = new System.Drawing.Size(240, 23);

            this.lblIpPort.Text = "Port:";
            this.lblIpPort.Location = new System.Drawing.Point(5, 40);
            this.lblIpPort.Size = new System.Drawing.Size(60, 20);
            this.numIpPort.Location = new System.Drawing.Point(80, 38);
            this.numIpPort.Size = new System.Drawing.Size(100, 23);
            this.numIpPort.Maximum = 65535;

            // Buttons
            this.btnSave.Text = "Save Configuration";
            this.btnSave.Location = new System.Drawing.Point(100, 465);
            this.btnSave.Size = new System.Drawing.Size(140, 35);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(250, 465);
            this.btnCancel.Size = new System.Drawing.Size(140, 35);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(189, 195, 199);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // Add controls to groups/panels
            this.pnlSerial.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblComPort, this.cmbComPort, this.lblBaudRate, this.cmbBaudRate });
            this.pnlNetwork.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblIpAddress, this.txtIpAddress, this.lblIpPort, this.numIpPort });
            this.grpConnection.Controls.AddRange(new System.Windows.Forms.Control[] { this.rdbCOM, this.rdbIP, this.pnlSerial, this.pnlNetwork });
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblName, this.txtName, this.lblModel, this.txtModel,
                this.lblGasType, this.cmbGasType, this.lblUnit, this.cmbUnit,
                this.lblChannel, this.numChannel, this.lblDecimals, this.numDecimals,
                this.grpConnection, this.btnSave, this.btnCancel
            });

            this.pnlSerial.ResumeLayout(false);
            this.pnlNetwork.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDecimals)).EndInit();
            this.grpConnection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numIpPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label lblGasType;
        private System.Windows.Forms.ComboBox cmbGasType;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.NumericUpDown numChannel;
        private System.Windows.Forms.Label lblDecimals;
        private System.Windows.Forms.NumericUpDown numDecimals;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.RadioButton rdbCOM;
        private System.Windows.Forms.RadioButton rdbIP;
        private System.Windows.Forms.Panel pnlSerial;
        private System.Windows.Forms.Label lblComPort;
        private System.Windows.Forms.ComboBox cmbComPort;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Panel pnlNetwork;
        private System.Windows.Forms.Label lblIpAddress;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label lblIpPort;
        private System.Windows.Forms.NumericUpDown numIpPort;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
