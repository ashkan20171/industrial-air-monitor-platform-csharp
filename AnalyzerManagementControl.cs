using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Net.Sockets;
using System.Windows.Forms;
using AshkanAQMS.Models;

namespace AshkanAQMS
{
    public partial class AnalyzerManagementControl : UserControl
    {
        private List<AnalyzerConfig> _analyzers = new List<AnalyzerConfig>();

        public AnalyzerManagementControl()
        {
            InitializeComponent();
            ApplyColors();
            LoadMockData();
            RefreshGrid();
        }

        private void ApplyColors()
        {
            pnlHeader.BackColor = Color.White;
        }

        private void LoadMockData()
        {
            // بارگذاری اولیه چند داده تستی
            _analyzers.Add(new AnalyzerConfig { Name = "PM2.5 Primary Analyzer", Model = "Thermo 5012", GasType = "PM2.5", ConnectionType = "COM", ComPort = "COM1", BaudRate = 9600 });
            _analyzers.Add(new AnalyzerConfig { Name = "Stack CO2 Analyzer", Model = "Siemens Ultramat", GasType = "CO2", ConnectionType = "IP", IpAddress = "192.168.1.110", IpPort = 502 });
        }

        private void RefreshGrid()
        {
            dgvAnalyzers.Rows.Clear();
            foreach (var analyzer in _analyzers)
            {
                string connectionDetails = analyzer.ConnectionType == "COM"
                    ? $"{analyzer.ComPort} ({analyzer.BaudRate} bps)"
                    : $"{analyzer.IpAddress}:{analyzer.IpPort}";

                dgvAnalyzers.Rows.Add(
                    analyzer.Id,
                    analyzer.Name,
                    analyzer.Model,
                    analyzer.GasType,
                    analyzer.Unit,
                    connectionDetails
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new AnalyzerConfigForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _analyzers.Add(form.Configuration);
                    RefreshGrid();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditSelectedAnalyzer();
        }

        private void dgvAnalyzers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditSelectedAnalyzer();
            }
        }

        private void EditSelectedAnalyzer()
        {
            if (dgvAnalyzers.CurrentRow == null) return;

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            var target = _analyzers.Find(a => a.Id == id);

            if (target != null)
            {
                using (var form = new AnalyzerConfigForm(target))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshGrid();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAnalyzers.CurrentRow == null) return;

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            var target = _analyzers.Find(a => a.Id == id);

            if (target != null)
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete {target.Name}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _analyzers.Remove(target);
                    RefreshGrid();
                }
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (dgvAnalyzers.CurrentRow == null)
            {
                MessageBox.Show("Please select an analyzer from the grid first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            var target = _analyzers.Find(a => a.Id == id);

            if (target != null)
            {
                lblDiagnosticStatus.Text = $"Testing connection to {target.Name}...";
                lblDiagnosticStatus.ForeColor = Color.DarkOrange;
                Application.DoEvents();

                bool isSuccess = false;
                string errorMessage = string.Empty;

                if (target.ConnectionType == "COM")
                {
                    isSuccess = TestSerialPort(target.ComPort, target.BaudRate, out errorMessage);
                }
                else
                {
                    isSuccess = TestNetworkConnection(target.IpAddress, target.IpPort, out errorMessage);
                }

                if (isSuccess)
                {
                    lblDiagnosticStatus.Text = $"Connection Test Successful! [Type: {target.ConnectionType}]";
                    lblDiagnosticStatus.ForeColor = Color.ForestGreen;
                }
                else
                {
                    lblDiagnosticStatus.Text = $"Connection Failed: {errorMessage}";
                    lblDiagnosticStatus.ForeColor = Color.Crimson;
                }
            }
        }

        private bool TestSerialPort(string portName, int baudRate, out string error)
        {
            error = string.Empty;
            SerialPort port = null;
            try
            {
                port = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
                port.Open();
                port.Close();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            finally
            {
                if (port != null && port.IsOpen)
                    port.Close();
            }
        }

        private bool TestNetworkConnection(string ip, int port, out string error)
        {
            error = string.Empty;
            using (var client = new TcpClient())
            {
                try
                {
                    var result = client.BeginConnect(ip, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2)); // تایم‌اوت ۲ ثانیه‌ای برای تست کابل/پورت شبکه
                    if (!success)
                    {
                        error = "Connection Timeout";
                        return false;
                    }
                    client.EndConnect(result);
                    return true;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
            }
        }
    }
}
