using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanAQMS.Models;

namespace AshkanAQMS
{
    public partial class AnalyzerConfigForm : Form
    {
        public AnalyzerConfig Configuration { get; private set; }

        public AnalyzerConfigForm(AnalyzerConfig config = null)
        {
            InitializeComponent();
            Configuration = config ?? new AnalyzerConfig();
            LoadConfiguration();
            ToggleConnectionFields();
        }

        private void LoadConfiguration()
        {
            txtName.Text = Configuration.Name;
            txtModel.Text = Configuration.Model;

            if (!string.IsNullOrEmpty(Configuration.GasType) && cmbGasType.Items.Contains(Configuration.GasType))
                cmbGasType.SelectedItem = Configuration.GasType;
            else
                cmbGasType.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(Configuration.Unit) && cmbUnit.Items.Contains(Configuration.Unit))
                cmbUnit.SelectedItem = Configuration.Unit;
            else
                cmbUnit.SelectedIndex = 0;

            if (Configuration.Channel >= (int)numChannel.Minimum && Configuration.Channel <= (int)numChannel.Maximum)
                numChannel.Value = Configuration.Channel;

            if (Configuration.DecimalDigits >= (int)numDecimals.Minimum && Configuration.DecimalDigits <= (int)numDecimals.Maximum)
                numDecimals.Value = Configuration.DecimalDigits;

            if (Configuration.ConnectionType == "IP")
                rdbIP.Checked = true;
            else
                rdbCOM.Checked = true;

            cmbComPort.Text = Configuration.ComPort;
            cmbBaudRate.Text = Configuration.BaudRate.ToString();
            txtIpAddress.Text = Configuration.IpAddress;

            if (Configuration.IpPort >= (int)numIpPort.Minimum && Configuration.IpPort <= (int)numIpPort.Maximum)
                numIpPort.Value = Configuration.IpPort;
        }

        private void ToggleConnectionFields()
        {
            bool isCom = rdbCOM.Checked;
            pnlSerial.Enabled = isCom;
            pnlNetwork.Enabled = !isCom;

            if (isCom)
            {
                if (string.IsNullOrWhiteSpace(cmbComPort.Text) || !cmbComPort.Items.Contains(cmbComPort.Text))
                    cmbComPort.SelectedIndex = 0;

                if (string.IsNullOrWhiteSpace(cmbBaudRate.Text) || !cmbBaudRate.Items.Contains(cmbBaudRate.Text))
                    cmbBaudRate.SelectedIndex = 1; // 9600
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtIpAddress.Text))
                    txtIpAddress.Text = "192.168.1.100";

                if (numIpPort.Value == 0)
                    numIpPort.Value = 502;
            }
        }

        private void rdbConnectionType_CheckedChanged(object sender, EventArgs e)
        {
            ToggleConnectionFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(
                    "Please enter an Analyzer Name.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Configuration.Name = txtName.Text.Trim();
            Configuration.Model = txtModel.Text.Trim();
            Configuration.GasType = cmbGasType.SelectedItem?.ToString() ?? "PM2.5";
            Configuration.Unit = cmbUnit.SelectedItem?.ToString() ?? "µg/m³";
            Configuration.Channel = (int)numChannel.Value;
            Configuration.DecimalDigits = (int)numDecimals.Value;

            if (rdbCOM.Checked)
            {
                Configuration.ConnectionType = "COM";
                Configuration.ComPort = cmbComPort.Text;
                Configuration.BaudRate = int.TryParse(cmbBaudRate.Text, out int baud) ? baud : 9600;
                Configuration.IpAddress = "";
                Configuration.IpPort = 0;
            }
            else
            {
                Configuration.ConnectionType = "IP";
                Configuration.IpAddress = txtIpAddress.Text.Trim();
                Configuration.IpPort = (int)numIpPort.Value;
                Configuration.ComPort = "";
                Configuration.BaudRate = 0;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
