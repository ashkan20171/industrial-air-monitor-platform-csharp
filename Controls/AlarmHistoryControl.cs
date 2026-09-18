using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    /// <summary>Operator-facing audit/alarm history viewer backed by the local audit log.</summary>
    public class AlarmHistoryControl : UserControl
    {
        private readonly AuditLogger _auditLogger = new AuditLogger();
        private readonly DataGridView _grid = new DataGridView();
        private readonly Label _summary = new Label();
        private TextBox _search;
        private List<AuditRow> _rows = new List<AuditRow>();

        private sealed class AuditRow
        {
            public DateTime Timestamp { get; set; }
            public string Action { get; set; }
            public string Details { get; set; }
        }

        public AlarmHistoryControl()
        {
            Dock = DockStyle.Fill;
            BackColor = IndustrialTheme.BackgroundDark;
            Padding = new Padding(24);
            BuildUi();
            LoadHistory();
        }

        private void BuildUi()
        {
            var title = new Label { Dock = DockStyle.Top, Height = 38, Text = "Alarm & Audit History", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = IndustrialTheme.TextPrimary };
            var subtitle = new Label { Dock = DockStyle.Top, Height = 34, Text = "Review operational events, alarm activity and the local audit trail.", Font = new Font("Segoe UI", 9.5F), ForeColor = IndustrialTheme.TextSecondary };
            Controls.Add(subtitle); Controls.Add(title);

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(0, 8, 0, 8) };
            _search = new TextBox { Width = 300, Height = 30, Location = new Point(0, 10), Font = new Font("Segoe UI", 9F) };
            _search.TextChanged += (s, e) => ApplyFilter();
            var refresh = CreateButton("Refresh", 100, 10);
            refresh.Click += (s, e) => LoadHistory();
            var export = CreateButton("Export CSV", 210, 10);
            export.Click += (s, e) => ExportCsv();
            var open = CreateButton("Open Log", 320, 10);
            open.Click += (s, e) => OpenLogFolder();
            toolbar.Controls.Add(_search); toolbar.Controls.Add(refresh); toolbar.Controls.Add(export); toolbar.Controls.Add(open);
            Controls.Add(toolbar);

            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.AllowUserToDeleteRows = false;
            _grid.AutoGenerateColumns = false; _grid.RowHeadersVisible = false; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.BackgroundColor = IndustrialTheme.SurfaceCard; _grid.BorderStyle = BorderStyle.None;
            _grid.EnableHeadersVisualStyles = false;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Timestamp", HeaderText = "Timestamp", DataPropertyName = "Timestamp", Width = 180 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Action", HeaderText = "Action", DataPropertyName = "Action", Width = 130 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Details", HeaderText = "Details", DataPropertyName = "Details", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            Controls.Add(_grid);

            _summary.Dock = DockStyle.Bottom; _summary.Height = 34; _summary.TextAlign = ContentAlignment.MiddleLeft; _summary.ForeColor = IndustrialTheme.TextSecondary;
            Controls.Add(_summary);
        }

        private Button CreateButton(string text, int x, int y)
        {
            return new Button { Text = text, Location = new Point(x, y), Size = new Size(100, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(52, 73, 94), ForeColor = Color.White };
        }

        private void LoadHistory()
        {
            _rows = new List<AuditRow>();
            try
            {
                var path = _auditLogger.GetPath();
                if (File.Exists(path))
                {
                    foreach (var line in File.ReadAllLines(path).Reverse().Take(2000))
                    {
                        var first = line.IndexOf(']');
                        var second = first >= 0 ? line.IndexOf(']', first + 1) : -1;
                        if (first < 0 || second < 0) continue;
                        DateTime timestamp;
                        if (!DateTime.TryParse(line.Substring(1, first - 1), out timestamp)) continue;
                        var action = line.Substring(first + 3, second - first - 3).Trim();
                        var details = line.Substring(second + 1).Trim();
                        _rows.Add(new AuditRow { Timestamp = timestamp, Action = action, Details = details });
                    }
                }
            }
            catch { }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = (_search == null ? string.Empty : _search.Text).Trim();
            var filtered = string.IsNullOrWhiteSpace(query) ? _rows : _rows.Where(x => (x.Action + " " + x.Details).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            _grid.DataSource = new BindingSource { DataSource = filtered };
            _summary.Text = string.Format("{0} events shown • Audit log: {1}", filtered.Count, _auditLogger.GetPath());
        }

        private void ExportCsv()
        {
            using (var dialog = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", FileName = "AQMS_Audit_History_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv" })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                var lines = new List<string> { "Timestamp,Action,Details" };
                foreach (var row in _rows) lines.Add(string.Join(",", Csv(row.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")), Csv(row.Action), Csv(row.Details)));
                File.WriteAllLines(dialog.FileName, lines);
                MessageBox.Show(this, "Audit history exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string Csv(string value) { return "\"" + (value ?? string.Empty).Replace("\"", "\"\"") + "\""; }

        private void OpenLogFolder()
        {
            try { System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + _auditLogger.GetPath() + "\""); }
            catch { MessageBox.Show(this, _auditLogger.GetPath(), "Audit Log Path", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
    }
}
