using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
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
        private readonly BindingSource _bindingSource;
        private List<SensorLog> _currentLogs;

        public ReportsControl()
        {
            InitializeComponent();

            _dbService = new DbService();
            _bindingSource = new BindingSource();
            _currentLogs = new List<SensorLog>();

            Load += ReportsControl_Load;
            dgvReport.DataBindingComplete += dgvReport_DataBindingComplete;
        }

        private void ReportsControl_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Now.Date.AddDays(-7);
            dtpTo.Value = DateTime.Now;

            ConfigureGrid();
            RefreshReport();
        }

        private void ConfigureGrid()
        {
            dgvReport.AutoGenerateColumns = false;
            dgvReport.ReadOnly = true;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.MultiSelect = false;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.RowHeadersVisible = false;
            dgvReport.DataSource = _bindingSource;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (_currentLogs == null || _currentLogs.Count == 0)
            {
                RefreshReport();
            }

            if (_currentLogs == null || _currentLogs.Count == 0)
            {
                MessageBox.Show(
                    "There is no report data to export.",
                    "Export",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Export Report to CSV";
                dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dialog.FileName = $"AQMS_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    ExportCurrentReport(dialog.FileName);
                    MessageBox.Show(
                        "CSV export completed successfully.",
                        "Export",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Export failed:\n{ex.Message}",
                        "Export Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshReport()
        {
            var from = dtpFrom.Value;
            var to = dtpTo.Value;

            if (from > to)
            {
                MessageBox.Show(
                    "From date must be less than or equal to To date.",
                    "Invalid Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _currentLogs = _dbService.GetLogs(from, to)
                                     .OrderBy(x => x.Timestamp)
                                     .ToList();

            _bindingSource.DataSource = new BindingList<SensorLog>(_currentLogs);

            UpdateSummary();
            ApplyEmptyStateIfNeeded();
        }

        private void UpdateSummary()
        {
            if (_currentLogs.Count == 0)
            {
                lblTotalLogs.Text = "Total Logs: 0";
                lblAvgAqi.Text = "Average AQI: -";
                lblMaxAqi.Text = "Max AQI: -";
                lblMinAqi.Text = "Min AQI: -";
                return;
            }

            var total = _currentLogs.Count;
            var avg = _currentLogs.Average(x => x.AQI);
            var max = _currentLogs.Max(x => x.AQI);
            var min = _currentLogs.Min(x => x.AQI);

            lblTotalLogs.Text = $"Total Logs: {total}";
            lblAvgAqi.Text = $"Average AQI: {avg:0.0}";
            lblMaxAqi.Text = $"Max AQI: {max:0.0}";
            lblMinAqi.Text = $"Min AQI: {min:0.0}";
        }

        private void ApplyEmptyStateIfNeeded()
        {
            if (_currentLogs.Count == 0)
            {
                dgvReport.Columns["colTimestamp"].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dgvReport_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.DataBoundItem is SensorLog log)
                {
                    ApplyAqiRowStyle(row, log.AQI);
                }
            }
        }

        private void ApplyAqiRowStyle(DataGridViewRow row, double aqi)
        {
            if (aqi <= 50)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            }
            else if (aqi <= 100)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
            }
            else if (aqi <= 150)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 178);
            }
            else if (aqi <= 200)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
            }
            else if (aqi <= 300)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(225, 190, 231);
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(238, 238, 238);
            }

            row.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void ExportCurrentReport(string filePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Timestamp,AnalyzerId,PM25,CO2,AQI,Status");

            foreach (var item in _currentLogs.OrderBy(x => x.Timestamp))
            {
                sb.AppendLine(string.Join(",",
                    Csv(item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)),
                    Csv(item.AnalyzerId),
                    Csv(item.PM25.ToString("0.0", CultureInfo.InvariantCulture)),
                    Csv(item.CO2.ToString("0.0", CultureInfo.InvariantCulture)),
                    Csv(item.AQI.ToString("0.0", CultureInfo.InvariantCulture)),
                    Csv(item.Status)
                ));
            }

            File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        private string Csv(string value)
        {
            if (value == null)
                return "\"\"";

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
