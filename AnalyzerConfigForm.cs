using AshkanAQMS.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

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
            cmbGasType.SelectedItem = Configuration.GasType;
            cmbUnit.SelectedItem = Configuration.Unit;
            numChannel.Value = Configuration.Channel;
            numDecimals.Value = Configuration.DecimalDigits;

            if (Configuration.ConnectionType == "IP")
                rdbIP.Checked = true;
            else
                rdbCOM.Checked = true;

            cmbComPort.Text = Configuration.ComPort;
            cmbBaudRate.Text = Configuration.BaudRate.ToString();
            txtIpAddress.Text = Configuration.IpAddress;
            numIpPort.Value = Configuration.IpPort;
        }

        private void ToggleConnectionFields()
        {
            bool isCom = rdbCOM.Checked;
            pnlSerial.Enabled = isCom;
            pnlNetwork.Enabled = !isCom;
        }

        private void rdbConnectionType_CheckedChanged(object sender, EventArgs e)
        {
            ToggleConnectionFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter an Analyzer Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Configuration.Name = txtName.Text.Trim();
            Configuration.Model = txtModel.Text.Trim();
            Configuration.GasType = cmbGasType.SelectedItem?.ToString() ?? "PM2.5";
            Configuration.Unit = cmbUnit.SelectedItem?.ToString() ?? "µg/m³";
            Configuration.Channel = (int)numChannel.Value;
            Configuration.DecimalDigits = (int)numDecimals.Value;
            Configuration.ConnectionType = rdbCOM.Checked ? "COM" : "IP";
            Configuration.ComPort = cmbComPort.Text;
            Configuration.BaudRate = int.TryParse(cmbBaudRate.Text, out int baud) ? baud : 9600;
            Configuration.IpAddress = txtIpAddress.Text.Trim();
            Configuration.IpPort = (int)numIpPort.Value;

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
