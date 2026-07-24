using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Net.Sockets;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS
{
    public partial class AnalyzerManagementControl : UserControl
    {
        private List<AnalyzerConfig> _analyzers = new List<AnalyzerConfig>();

        public AnalyzerManagementControl()
        {
            InitializeComponent();
            ApplyColors();
            LoadAnalyzers();
            RefreshGrid();
        }

        private void ApplyColors()
        {
            pnlHeader.BackColor = Color.FromArgb(41, 128, 185);
            lblTitle.ForeColor = Color.White;

            btnAdd.BackColor = Color.FromArgb(39, 174, 96);
            btnAdd.ForeColor = Color.White;

            btnEdit.BackColor = Color.FromArgb(241, 196, 15);
            btnEdit.ForeColor = Color.White;

            btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.ForeColor = Color.White;

            btnTestConnection.BackColor = Color.FromArgb(52, 152, 219);
            btnTestConnection.ForeColor = Color.White;

            lblDiagnosticStatus.ForeColor = Color.DimGray;
        }

        private void LoadAnalyzers()
        {
            try
            {
                _analyzers = StorageService.LoadAnalyzers();

                if (_analyzers == null)
                {
                    _analyzers = new List<AnalyzerConfig>();
                }

                if (_analyzers.Count == 0)
                {
                    LoadDefaultAnalyzers();
                    SaveAnalyzers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load analyzers.\n\n" + ex.Message,
                    "Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                _analyzers = new List<AnalyzerConfig>();
                LoadDefaultAnalyzers();
            }
        }

        private void LoadDefaultAnalyzers()
        {
            _analyzers.Add(new AnalyzerConfig
            {
                Name = "PM2.5 Primary Analyzer",
                Model = "Thermo 5012",
                GasType = "PM2.5",
                ConnectionType = "COM",
                ComPort = "COM1",
                BaudRate = 9600
            });

            _analyzers.Add(new AnalyzerConfig
            {
                Name = "Stack CO2 Analyzer",
                Model = "Siemens Ultramat",
                GasType = "CO2",
                ConnectionType = "IP",
                IpAddress = "192.168.1.110",
                IpPort = 502
            });
        }

        private void SaveAnalyzers()
        {
            try
            {
                StorageService.SaveAnalyzers(_analyzers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to save analyzers.\n\n" + ex.Message,
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RefreshGrid()
        {
            dgvAnalyzers.Rows.Clear();

            foreach (var analyzer in _analyzers)
            {
                string connectionDetails =
                    analyzer.ConnectionType == "COM"
                        ? $"{analyzer.ComPort} ({analyzer.BaudRate} bps)"
                        : $"{analyzer.IpAddress}:{analyzer.IpPort}";

                dgvAnalyzers.Rows.Add(
                    analyzer.Id,
                    analyzer.Name,
                    analyzer.Model,
                    analyzer.GasType,
                    analyzer.Unit,
                    connectionDetails);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new AnalyzerConfigForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _analyzers.Add(form.Configuration);
                    SaveAnalyzers();
                    RefreshGrid();
                    lblDiagnosticStatus.Text = "Analyzer added successfully.";
                    lblDiagnosticStatus.ForeColor = Color.FromArgb(39, 174, 96);
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
            if (dgvAnalyzers.CurrentRow == null)
            {
                return;
            }

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            var target = _analyzers.Find(a => a.Id == id);

            if (target != null)
            {
                using (var form = new AnalyzerConfigForm(target))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        SaveAnalyzers();
                        RefreshGrid();
                        lblDiagnosticStatus.Text = "Analyzer updated successfully.";
                        lblDiagnosticStatus.ForeColor = Color.FromArgb(39, 174, 96);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAnalyzers.CurrentRow == null)
            {
                return;
            }

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            var target = _analyzers.Find(a => a.Id == id);

            if (target != null)
            {
                var confirm = MessageBox.Show(
                    $"Are you sure you want to delete {target.Name}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    _analyzers.Remove(target);
                    SaveAnalyzers();
                    RefreshGrid();
                    lblDiagnosticStatus.Text = "Analyzer deleted successfully.";
                    lblDiagnosticStatus.ForeColor = Color.FromArgb(231, 76, 60);
                }
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (dgvAnalyzers.CurrentRow == null)
            {
                MessageBox.Show(
                    "Please select an analyzer first.",
                    "Test Connection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            var target = _analyzers.Find(a => a.Id == id);

            if (target == null)
            {
                return;
            }

            bool isSuccess;
            string errorMessage;

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
                lblDiagnosticStatus.Text = "Connection successful.";
                lblDiagnosticStatus.ForeColor = Color.FromArgb(39, 174, 96);
                MessageBox.Show(
                    "Connection test was successful.",
                    "Test Connection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                lblDiagnosticStatus.Text = "Connection failed.";
                lblDiagnosticStatus.ForeColor = Color.FromArgb(231, 76, 60);
                MessageBox.Show(
                    errorMessage,
                    "Test Connection Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private bool TestSerialPort(string portName, int baudRate, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (SerialPort port = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One))
                {
                    port.Open();
                    port.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Serial port test failed: " + ex.Message;
                return false;
            }
        }

        private bool TestNetworkConnection(string ipAddress, int port, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    IAsyncResult result = client.BeginConnect(ipAddress, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));

                    if (!success)
                    {
                        errorMessage = "Network connection timed out.";
                        return false;
                    }

                    client.EndConnect(result);
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Network connection failed: " + ex.Message;
                return false;
            }
        }
    }
}
