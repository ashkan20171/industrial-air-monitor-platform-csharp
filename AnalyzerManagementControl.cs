using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Net.Sockets;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services; // اضافه شد

namespace AshkanAQMS
{
    public partial class AnalyzerManagementControl : UserControl
    {
        private List<AnalyzerConfig> _analyzers = new List<AnalyzerConfig>();

        public AnalyzerManagementControl()
        {
            InitializeComponent();
            ApplyColors();
            LoadData(); // تغییر از LoadMockData به LoadData
            RefreshGrid();
        }

        private void ApplyColors()
        {
            pnlHeader.BackColor = Color.White;
        }

        private void LoadData()
        {
            // تلاش برای بارگذاری از فایل ذخیره شده
            _analyzers = StorageService.LoadAnalyzers();

            // اگر فایلی وجود نداشت، دیتای اولیه را بساز
            if (_analyzers == null || _analyzers.Count == 0)
            {
                _analyzers = new List<AnalyzerConfig>
                {
                    new AnalyzerConfig { Name = "PM2.5 Primary Analyzer", Model = "Thermo 5012", GasType = "PM2.5", ConnectionType = "COM", ComPort = "COM1", BaudRate = 9600 },
                    new AnalyzerConfig { Name = "Stack CO2 Analyzer", Model = "Siemens Ultramat", GasType = "CO2", ConnectionType = "IP", IpAddress = "192.168.1.110", IpPort = 502 }
                };
                StorageService.SaveAnalyzers(_analyzers);
            }
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
                    StorageService.SaveAnalyzers(_analyzers); // ذخیره دائمی
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
            if (e.RowIndex >= 0) EditSelectedAnalyzer();
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
                        StorageService.SaveAnalyzers(_analyzers); // ذخیره تغییرات
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
                    StorageService.SaveAnalyzers(_analyzers); // ذخیره بعد از حذف
                    RefreshGrid();
                }
            }
        }

        // بخش تست اتصال (بدون تغییر اما آماده برای توسعه)
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (dgvAnalyzers.CurrentRow == null) return;
            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            var target = _analyzers.Find(a => a.Id == id);

            if (target != null)
            {
                lblDiagnosticStatus.Text = $"Testing {target.Name}...";
                lblDiagnosticStatus.ForeColor = Color.DarkOrange;
                Application.DoEvents();

                bool isSuccess = target.ConnectionType == "COM"
                    ? TestSerialPort(target.ComPort, target.BaudRate, out string err)
                    : TestNetworkConnection(target.IpAddress, target.IpPort, out err);

                lblDiagnosticStatus.Text = isSuccess ? "Success!" : $"Failed: {err}";
                lblDiagnosticStatus.ForeColor = isSuccess ? Color.ForestGreen : Color.Crimson;
            }
        }

        private bool TestSerialPort(string portName, int baudRate, out string error)
        {
            error = "";
            try
            {
                using (var p = new SerialPort(portName, baudRate))
                {
                    p.Open(); p.Close(); return true;
                }
            }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        private bool TestNetworkConnection(string ip, int port, out string error)
        {
            error = "";
            try
            {
                using (var client = new TcpClient())
                {
                    var res = client.BeginConnect(ip, port, null, null);
                    if (!res.AsyncWaitHandle.WaitOne(2000)) { error = "Timeout"; return false; }
                    client.EndConnect(res); return true;
                }
            }
            catch (Exception ex) { error = ex.Message; return false; }
        }
    }
}
