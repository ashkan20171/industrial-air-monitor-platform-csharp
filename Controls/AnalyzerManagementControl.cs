using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Services;

namespace AshkanAQMS
{
    public partial class AnalyzerManagementControl : UserControl
    {
        private List<AnalyzerConfig> _analyzers = new List<AnalyzerConfig>();
        private readonly StorageService _storageService;

        public AnalyzerManagementControl()
        {
            InitializeComponent();
            _storageService = new StorageService();
            ApplyColors();
            LoadAnalyzers();
            RefreshGrid();
            pnlActions.Height = 250;
            BuildConfigurationTools();
        }

        private void BuildConfigurationTools()
        {
            var backup = new Button { Text = "Backup Configuration", Width = 150, Height = 34, Left = 10, Top = 160, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(127, 140, 141), ForeColor = Color.White };
            backup.Click += (s, e) =>
            {
                try
                {
                    string path = AnalyzerConfigBackupService.CreateBackup(_analyzers, _storageService.GetAnalyzerBackupFolder());
                    lblDiagnosticStatus.Text = "Backup created: " + Path.GetFileName(path);
                    lblDiagnosticStatus.ForeColor = Color.FromArgb(39, 174, 96);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };
            pnlActions.Controls.Add(backup);

            var validate = new Button { Text = "Validate All", Width = 150, Height = 34, Left = 10, Top = 205, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(52, 73, 94), ForeColor = Color.White };
            validate.Click += (s, e) => ValidateAllConfigurations();
            pnlActions.Controls.Add(validate);

            var toggle = new Button { Text = "Enable / Disable", Width = 150, Height = 34, Left = 10, Top = 250, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(230, 126, 34), ForeColor = Color.White };
            toggle.Click += (s, e) => ToggleSelectedAnalyzer();
            pnlActions.Controls.Add(toggle);
        }

        private void ValidateAllConfigurations()
        {
            var errors = new List<string>(); var warnings = new List<string>();
            foreach (var analyzer in _analyzers)
            {
                var result = AnalyzerConfigValidationService.Validate(analyzer, _analyzers, analyzer.Id);
                errors.AddRange(result.Errors.Select(x => analyzer.Name + ": " + x));
                warnings.AddRange(result.Warnings.Select(x => analyzer.Name + ": " + x));
            }
            if (errors.Count == 0)
            {
                lblDiagnosticStatus.Text = "All analyzer configurations are valid.";
                lblDiagnosticStatus.ForeColor = Color.FromArgb(39, 174, 96);
                MessageBox.Show((warnings.Count == 0 ? "No validation issues found." : "No blocking errors.\n\nWarnings:\n" + string.Join("\n", warnings)), "Configuration Health", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblDiagnosticStatus.Text = errors.Count + " validation issue(s) detected.";
                lblDiagnosticStatus.ForeColor = Color.FromArgb(231, 76, 60);
                MessageBox.Show("Blocking errors:\n" + string.Join("\n", errors) + (warnings.Count > 0 ? "\n\nWarnings:\n" + string.Join("\n", warnings) : ""), "Configuration Health", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                _analyzers = _storageService.LoadAnalyzers();
                if (_analyzers == null) _analyzers = new List<AnalyzerConfig>();
                // An empty configuration is intentional. Do not create COM1/IP demo devices.
                // Real hardware must be explicitly configured by the operator.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load analyzers.\n\n" + ex.Message,
                    "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _analyzers = new List<AnalyzerConfig>();
                LoadDefaultAnalyzers();
            }
        }

        private void LoadDefaultAnalyzers()
        {
            // Intentionally empty. AQMS never invents a hardware endpoint.
        }

        private bool SaveAnalyzers()
        {
            try
            {
                var allErrors = new List<string>();
                foreach (var analyzer in _analyzers)
                {
                    var validation = AnalyzerConfigValidationService.Validate(analyzer, _analyzers, analyzer.Id);
                    allErrors.AddRange(validation.Errors);
                }
                if (allErrors.Count > 0)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, allErrors.Distinct()), "Configuration Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (_analyzers.Count > 0) AnalyzerConfigBackupService.CreateBackup(_analyzers, _storageService.GetAnalyzerBackupFolder());
                _storageService.SaveAnalyzers(_analyzers);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save analyzers.\n\n" + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
                    (analyzer.Enabled ? "[ENABLED] " : "[DISABLED] ") + analyzer.Name,
                    analyzer.Model,
                    analyzer.GasType,
                    analyzer.Unit,
                    connectionDetails
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new AnalyzerConfigForm(_storageService))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _analyzers.Add(form.Configuration);
                    if (!SaveAnalyzers()) return;
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
                EditSelectedAnalyzer();
        }

        private void EditSelectedAnalyzer()
        {
            if (dgvAnalyzers.CurrentRow == null) return;

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(id)) return;

            var target = _analyzers.Find(a => a.Id == id);
            if (target != null)
            {
                using (var form = new AnalyzerConfigForm(_storageService, target))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (!SaveAnalyzers()) return;
                        RefreshGrid();
                        lblDiagnosticStatus.Text = "Analyzer updated successfully.";
                        lblDiagnosticStatus.ForeColor = Color.FromArgb(39, 174, 96);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAnalyzers.CurrentRow == null) return;

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(id)) return;

            var target = _analyzers.Find(a => a.Id == id);
            if (target != null)
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete {target.Name}?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    _analyzers.Remove(target);
                    if (!SaveAnalyzers()) return;
                    RefreshGrid();
                    lblDiagnosticStatus.Text = "Analyzer deleted successfully.";
                    lblDiagnosticStatus.ForeColor = Color.FromArgb(231, 76, 60);
                }
            }
        }

        private void ToggleSelectedAnalyzer()
        {
            if (dgvAnalyzers.CurrentRow == null)
            {
                MessageBox.Show("Select an analyzer first.", "Analyzer State", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            var target = _analyzers.Find(a => a.Id == id);
            if (target == null) return;

            target.Enabled = !target.Enabled;
            if (!SaveAnalyzers())
            {
                target.Enabled = !target.Enabled;
                return;
            }

            RefreshGrid();
            lblDiagnosticStatus.Text = target.Name + (target.Enabled ? " enabled. Acquisition will resume." : " disabled. Acquisition is stopped immediately.");
            lblDiagnosticStatus.ForeColor = target.Enabled ? Color.FromArgb(39,174,96) : Color.FromArgb(230,126,34);
            new AuditLogger().Write("ANALYZER_STATE", target.Name + " | " + (target.Enabled ? "ENABLED" : "DISABLED"));
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (dgvAnalyzers.CurrentRow == null)
            {
                MessageBox.Show("Please select an analyzer first.", "Test Connection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string id = dgvAnalyzers.CurrentRow.Cells["colId"].Value?.ToString();
            var target = _analyzers.Find(a => a.Id == id);
            if (target == null) return;

            try
            {
                if (!target.Enabled)
                {
                    MessageBox.Show("This analyzer is disabled. Enable it before testing live acquisition.", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var reader = new RealAnalyzerReader();
                var reading = reader.ReadAsync(target, System.Threading.CancellationToken.None).GetAwaiter().GetResult();
                if (reading.IsUsable)
                {
                    lblDiagnosticStatus.Text = string.Format("Live sample OK: {0:0.###} {1}", reading.Value, target.Unit);
                    lblDiagnosticStatus.ForeColor = Color.FromArgb(39, 174, 96);
                    MessageBox.Show(string.Format("Live acquisition succeeded.\n\nValue: {0:0.###} {1}\nResponse time: {2} ms\nRaw response: {3}", reading.Value, target.Unit, reading.ResponseTimeMs, reading.RawResponse), "Real Analyzer Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblDiagnosticStatus.Text = "Acquisition failed: " + reading.Quality;
                    lblDiagnosticStatus.ForeColor = Color.FromArgb(231, 76, 60);
                    MessageBox.Show(reading.Message, "Real Analyzer Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                lblDiagnosticStatus.Text = "Acquisition test failed.";
                lblDiagnosticStatus.ForeColor = Color.FromArgb(231, 76, 60);
                MessageBox.Show(ex.Message, "Real Analyzer Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
