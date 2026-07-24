using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    public partial class ReportsControl : UserControl
    {
        private readonly DbService _dbService;
        private List<SensorLog> _currentLogs;

        public ReportsControl()
        {
            InitializeComponent();

            _dbService = new DbService();
            _currentLogs = new List<SensorLog>();

            Load += ReportsControl_Load;
        }

        private void ReportsControl_Load(object sender, EventArgs e)
        {
            ConfigureGrid();

            dtpTo.Value = DateTime.Now;
            dtpFrom.Value = DateTime.Now.AddDays(-1);

            LoadReport();
        }

        private void ConfigureGrid()
        {
            dgvReport.AutoGenerateColumns = false;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.ReadOnly = true;
            dgvReport.MultiSelect = false;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.RowHeadersVisible = false;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.BackgroundColor = Color.White;
            dgvReport.BorderStyle = BorderStyle.None;

            dgvReport.Columns.Clear();

            var colTimestamp = new DataGridViewTextBoxColumn
            {
                Name = "colTimestamp",
                HeaderText = "Timestamp",
                DataPropertyName = "Timestamp",
                FillWeight = 170,
                DefaultCellStyle = { Format = "yyyy-MM-dd HH:mm:ss" }
            };

            var colAnalyzerId = new DataGridViewTextBoxColumn
            {
                Name = "colAnalyzerId",
                HeaderText = "Analyzer ID",
                DataPropertyName = "AnalyzerId",
                FillWeight = 110
            };

            var colPm25 = new DataGridViewTextBoxColumn
            {
                Name = "colPm25",
                HeaderText = "PM2.5",
                DataPropertyName = "PM25",
                FillWeight = 80,
                DefaultCellStyle = { Format = "N1" }
            };

            var colCo2 = new DataGridViewTextBoxColumn
            {
                Name = "colCo2",
                HeaderText = "CO2",
                DataPropertyName = "CO2",
                FillWeight = 80,
                DefaultCellStyle = { Format = "N0" }
            };

            var colAqi = new DataGridViewTextBoxColumn
            {
                Name = "colAqi",
                HeaderText = "AQI",
                DataPropertyName = "AQI",
                FillWeight = 70,
                DefaultCellStyle = { Format = "N0" }
            };

            var colStatus = new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                DataPropertyName = "Status",
                FillWeight = 140
            };

            dgvReport.Columns.AddRange(
                colTimestamp,
                colAnalyzerId,
                colPm25,
                colCo2,
                colAqi,
                colStatus
            );
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                DateTime from = dtpFrom.Value;
                DateTime to = dtpTo.Value;

                if (from > to)
                {
                    MessageBox.Show("The 'From' date cannot be greater than the 'To' date.",
                        "Invalid Date Range",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                _currentLogs = _dbService.GetLogs(from, to);

                dgvReport.DataSource = null;
                dgvReport.DataSource = _currentLogs;

                UpdateSummary(_currentLogs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load report.\n{ex.Message}",
                    "Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary(List<SensorLog> logs)
        {
            int total = logs.Count;

            lblTotalLogs.Text = $"Total Logs: {total}";

            if (total == 0)
            {
                lblAvgAqi.Text = "Average AQI: N/A";
                lblMaxAqi.Text = "Max AQI: N/A";
                lblMinAqi.Text = "Min AQI: N/A";
                return;
            }

            double avgAqi = logs.Average(x => x.AQI);
            double maxAqi = logs.Max(x => x.AQI);
            double minAqi = logs.Min(x => x.AQI);

            lblAvgAqi.Text = $"Average AQI: {avgAqi:N1}";
            lblMaxAqi.Text = $"Max AQI: {maxAqi:N0}";
            lblMinAqi.Text = $"Min AQI: {minAqi:N0}";
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (_currentLogs == null || _currentLogs.Count == 0)
            {
                MessageBox.Show("There is no filtered report data to export.",
                    "Export",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Export Report";
                dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dialog.FileName = $"report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportReportToCsv(dialog.FileName);
                        MessageBox.Show("Report exported successfully.",
                            "Export",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to export report.\n{ex.Message}",
                            "Export Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportReportToCsv(string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Timestamp,AnalyzerId,PM25,CO2,AQI,Status");

            foreach (var log in _currentLogs.OrderBy(x => x.Timestamp))
            {
                sb.AppendLine(log.ToCsvLine());
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
}
