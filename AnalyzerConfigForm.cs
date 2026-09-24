using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AshkanAQMS.Models;
using AshkanAQMS.Drivers;
using System.Collections.Generic;
using AshkanAQMS.Services;

namespace AshkanAQMS
{
    public partial class AnalyzerConfigForm : Form
    {
        private readonly StorageService _storageService;
        private readonly bool _isEditMode;
        private Panel _advancedPanel;
        private CheckBox _enabled;
        private TextBox _deviceId, _username, _password;
        private NumericUpDown _interval, _timeout, _dataBits, _gain, _offset, _engineeringMin, _engineeringMax;
        private CheckBox _engineeringRangeCheck;
        private ComboBox _parity, _stopBits;
        private Label _validationStatus;
        private Button _testButton;
        private TextBox _requestCommand, _responseDelimiter, _responseKey, _responseRegex, _encoding;
        private NumericUpDown _responseFieldIndex, _readDelay;
        private ComboBox _driverSelector;
        private CheckedListBox _measurementSelector;
        private Label _driverMeta;
        private Button _selectAllMeasurements, _clearMeasurements;

        public AnalyzerConfig Configuration { get; private set; }

        public AnalyzerConfigForm() : this(new StorageService(), null) { }
        public AnalyzerConfigForm(StorageService storageService) : this(storageService, null) { }
        public AnalyzerConfigForm(AnalyzerConfig configToEdit) : this(new StorageService(), configToEdit) { }

        public AnalyzerConfigForm(StorageService storageService, AnalyzerConfig configToEdit)
        {
            InitializeComponent();
            _storageService = storageService ?? new StorageService();
            BuildAdvancedConfigurationUi();
            Configuration = configToEdit ?? new AnalyzerConfig();
            _isEditMode = configToEdit != null;
            if (_isEditMode) PopulateFields(Configuration); else SetDefaults();
            Text = _isEditMode ? "Analyzer Configuration Center — Edit" : "Analyzer Configuration Center — Add";
        }

        private void BuildAdvancedConfigurationUi()
        {
            ClientSize = new Size(820, 820);
            MinimumSize = new Size(820, 820);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            BackColor = Color.FromArgb(245, 247, 250);

            _advancedPanel = new Panel { Location = new Point(425, 12), Size = new Size(375, 750), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, AutoScroll = true };
            Controls.Add(_advancedPanel);
            AddHeader("Analyzer Driver & Measurements", 12);
            int y = 42;
            _driverSelector = AddCombo("Analyzer / Driver", ref y, AnalyzerDriverCatalog.Items.Select(x => x.ToString()).ToArray());
            _driverSelector.SelectedIndexChanged += DriverSelector_SelectedIndexChanged;
            _driverMeta = new Label { Left = 15, Top = y, Width = 330, Height = 42, ForeColor = Color.FromArgb(76, 90, 110), AutoEllipsis = true }; _advancedPanel.Controls.Add(_driverMeta); y += 46;
            _measurementSelector = new CheckedListBox { Left = 15, Top = y, Width = 330, Height = 105, CheckOnClick = true, BorderStyle = BorderStyle.FixedSingle }; _advancedPanel.Controls.Add(_measurementSelector); y += 112;
            _selectAllMeasurements = new Button { Left=15, Top=y, Width=105, Height=27, Text="Select all", FlatStyle=FlatStyle.Flat }; _clearMeasurements = new Button { Left=128, Top=y, Width=105, Height=27, Text="Clear", FlatStyle=FlatStyle.Flat }; _advancedPanel.Controls.Add(_selectAllMeasurements); _advancedPanel.Controls.Add(_clearMeasurements); _selectAllMeasurements.Click += (s,e)=>{ for(int i=0;i<_measurementSelector.Items.Count;i++) _measurementSelector.SetItemChecked(i,true); SyncPrimaryMeasurement(); }; _clearMeasurements.Click += (s,e)=>{ for(int i=0;i<_measurementSelector.Items.Count;i++) _measurementSelector.SetItemChecked(i,false); }; _measurementSelector.ItemCheck += (s,e)=>{ if (IsHandleCreated) BeginInvoke(new Action(SyncPrimaryMeasurement)); }; y += 38;
            AddHeader("Operational & Communication Settings", y); y += 30;
            _enabled = AddCheck("Enabled / Active", ref y, true);
            _deviceId = AddText("Device ID", ref y);
            _interval = AddNumber("Request interval (ms)", ref y, 100, 600000, 1000);
            _timeout = AddNumber("Timeout (ms)", ref y, 100, 120000, 2000);
            _dataBits = AddNumber("Data bits", ref y, 5, 8, 8);
            _parity = AddCombo("Parity", ref y, new[] { "None", "Even", "Odd", "Mark", "Space" });
            _stopBits = AddCombo("Stop bits", ref y, new[] { "None", "One", "OnePointFive", "Two" });
            _gain = AddNumber("Gain", ref y, -100000, 100000, 1, 2);
            _offset = AddNumber("Offset", ref y, -100000, 100000, 0, 2);
            _engineeringRangeCheck = AddCheck("Reject values outside engineering range", ref y, false);
            _engineeringMin = AddNumber("Engineering min", ref y, -1000000, 1000000, 0, 3);
            _engineeringMax = AddNumber("Engineering max", ref y, -1000000, 1000000, 10000, 3);
            _username = AddText("Username", ref y);
            _password = AddText("Password", ref y, true);
            _password.UseSystemPasswordChar = true;

            AddHeader("Real Analyzer ASCII / Text Protocol", y + 4); y += 34;
            _requestCommand = AddText("Request command", ref y);
            _responseDelimiter = AddText("Response delimiter", ref y);
            _responseFieldIndex = AddNumber("Field index", ref y, 0, 255, 0);
            _responseKey = AddText("Response key", ref y);
            _responseRegex = AddText("Response regex", ref y);
            _readDelay = AddNumber("Read delay (ms)", ref y, 0, 10000, 50);
            _encoding = AddText("Encoding", ref y);

            var hint = new Label { Left = 15, Top = y + 4, Width = 330, Height = 55, Text = "Credentials are protected with Windows DPAPI and are not written to logs.\nGain/offset are applied by the acquisition layer.", ForeColor = Color.DimGray };
            _advancedPanel.Controls.Add(hint); y += 68;
            _testButton = new Button { Left = 15, Top = y, Width = 150, Height = 36, Text = "Test Connection", FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White };
            _testButton.Click += async (s, e) => await TestConnectionAsync();
            _advancedPanel.Controls.Add(_testButton);
            _validationStatus = new Label { Left = 175, Top = y + 2, Width = 175, Height = 60, Text = "Configuration not validated", ForeColor = Color.DimGray, AutoEllipsis = true };
            _advancedPanel.Controls.Add(_validationStatus);

            btnSave.Location = new Point(570, 745); btnSave.Size = new Size(105, 38); btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(690, 745); btnCancel.Size = new Size(105, 38); btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(39, 174, 96); btnSave.ForeColor = Color.White; btnSave.FlatStyle = FlatStyle.Flat;
            btnCancel.BackColor = Color.FromArgb(108, 117, 125); btnCancel.ForeColor = Color.White; btnCancel.FlatStyle = FlatStyle.Flat;
            _advancedPanel.BringToFront();
        }

        private void AddHeader(string text, int y)
        {
            var l = new Label { Left = 15, Top = y, Width = 340, Height = 25, Text = text, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 42, 54) };
            _advancedPanel.Controls.Add(l);
        }

        private CheckBox AddCheck(string text, ref int y, bool value)
        {
            var c = new CheckBox { Left = 15, Top = y, Width = 330, Height = 25, Text = text, Checked = value };
            _advancedPanel.Controls.Add(c); y += 35; return c;
        }
        private TextBox AddText(string label, ref int y, bool password = false)
        {
            AddFieldLabel(label, y); var t = new TextBox { Left = 150, Top = y - 3, Width = 195, Height = 24 }; _advancedPanel.Controls.Add(t); y += 35; return t;
        }
        private NumericUpDown AddNumber(string label, ref int y, decimal min, decimal max, decimal value, int decimals = 0)
        {
            AddFieldLabel(label, y); var n = new NumericUpDown { Left = 150, Top = y - 3, Width = 120, Minimum = min, Maximum = max, Value = Math.Max(min, Math.Min(max, value)), DecimalPlaces = decimals, Increment = decimals == 0 ? 1 : 0.1M }; _advancedPanel.Controls.Add(n); y += 35; return n;
        }
        private ComboBox AddCombo(string label, ref int y, string[] items)
        {
            AddFieldLabel(label, y); var c = new ComboBox { Left = 150, Top = y - 3, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList }; c.Items.AddRange(items); c.SelectedIndex = 0; _advancedPanel.Controls.Add(c); y += 35; return c;
        }
        private void AddFieldLabel(string text, int y) { _advancedPanel.Controls.Add(new Label { Left = 15, Top = y, Width = 130, Height = 22, Text = text + ":", ForeColor = Color.FromArgb(70, 78, 87) }); }

        private void DriverSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_driverSelector.SelectedIndex < 0 || _driverSelector.SelectedIndex >= AnalyzerDriverCatalog.Items.Count) return;
            var d = AnalyzerDriverCatalog.Items[_driverSelector.SelectedIndex];
            _driverMeta.Text = d.Transport + "  •  " + d.ValidationStatus + "\n" + d.Notes;
            _measurementSelector.Items.Clear();
            foreach (var m in d.Measurements) _measurementSelector.Items.Add(m, false);
            txtModel.Text = d.DisplayName;
            if (Configuration != null) AnalyzerDriverCatalog.ApplyDefaults(Configuration, d.Id);
            if (d.Transport.Contains("TCP") || d.Transport.Contains("HTTP")) rdbIP.Checked = true; else rdbCOM.Checked = true;
        }

        private void SyncPrimaryMeasurement()
        {
            var selected = _measurementSelector.CheckedItems.Cast<AnalyzerMeasurementDescriptor>().FirstOrDefault();
            if (selected == null) return;
            cmbGasType.Items.Clear();
            foreach (AnalyzerMeasurementDescriptor m in _measurementSelector.CheckedItems) cmbGasType.Items.Add(m.Name);
            if (cmbGasType.Items.Count > 0) cmbGasType.SelectedIndex = 0;
            cmbUnit.Items.Clear(); cmbUnit.Items.Add(selected.Unit); cmbUnit.SelectedIndex = 0;
        }

        private void SelectDriverAndMeasurements(AnalyzerConfig config)
        {
            int index = AnalyzerDriverCatalog.Items.ToList().FindIndex(x => string.Equals(x.Id, config.DriverId, StringComparison.OrdinalIgnoreCase));
            if (index < 0) index = 0; _driverSelector.SelectedIndex = index;
            var wanted = config.Measurements != null && config.Measurements.Count > 0 ? config.Measurements.Select(x=>x.Name).ToList() : new List<string>{config.GasType};
            for(int i=0;i<_measurementSelector.Items.Count;i++) { var m=(AnalyzerMeasurementDescriptor)_measurementSelector.Items[i]; _measurementSelector.SetItemChecked(i,wanted.Any(x=>string.Equals(x,m.Name,StringComparison.OrdinalIgnoreCase))); }
            SyncPrimaryMeasurement();
            if(!string.IsNullOrWhiteSpace(config.GasType)) cmbGasType.Text=config.GasType;
            if(!string.IsNullOrWhiteSpace(config.Unit)) { if(!cmbUnit.Items.Contains(config.Unit)) cmbUnit.Items.Add(config.Unit); cmbUnit.Text=config.Unit; }
        }

        private void SetDefaults()
        {
            _driverSelector.SelectedIndex = 0; if (_measurementSelector.Items.Count > 0) _measurementSelector.SetItemChecked(0, true); SyncPrimaryMeasurement(); rdbCOM.Checked = true; cmbBaudRate.SelectedIndex = 1;
            _enabled.Checked = true; _deviceId.Text = "1"; _interval.Value = 1000; _timeout.Value = 2000; _dataBits.Value = 8; _parity.SelectedItem = "None"; _stopBits.SelectedItem = "One"; _gain.Value = 1; _offset.Value = 0; _engineeringRangeCheck.Checked = false; _engineeringMin.Value = 0; _engineeringMax.Value = 10000; _requestCommand.Text = string.Empty; _responseDelimiter.Text = "\r\n"; _responseFieldIndex.Value = 0; _responseKey.Text = string.Empty; _responseRegex.Text = string.Empty; _readDelay.Value = 50; _encoding.Text = "ASCII";
            RefreshComPorts(); ToggleConnectionFields();
        }

        private void PopulateFields(AnalyzerConfig config)
        {
            SelectDriverAndMeasurements(config); txtName.Text = config.Name; txtModel.Text = config.Model; cmbGasType.Text = config.GasType; cmbUnit.Text = config.Unit;
            numChannel.Value = Math.Max(numChannel.Minimum, Math.Min(numChannel.Maximum, config.Channel)); numDecimals.Value = Math.Max(numDecimals.Minimum, Math.Min(numDecimals.Maximum, config.DecimalDigits));
            if (string.Equals(config.ConnectionType, "IP", StringComparison.OrdinalIgnoreCase)) { rdbIP.Checked = true; txtIpAddress.Text = config.IpAddress; numIpPort.Value = Math.Max(1, Math.Min(65535, config.IpPort)); }
            else { rdbCOM.Checked = true; RefreshComPorts(); cmbComPort.Text = config.ComPort; cmbBaudRate.Text = config.BaudRate.ToString(); }
            _enabled.Checked = config.Enabled; _deviceId.Text = config.DeviceId; _interval.Value = Clamp(_interval, config.RequestIntervalMs); _timeout.Value = Clamp(_timeout, config.TimeoutMs); _dataBits.Value = Clamp(_dataBits, config.DataBits);
            _parity.SelectedItem = config.Parity.ToString(); _stopBits.SelectedItem = config.StopBits.ToString(); _gain.Value = Clamp(_gain, (decimal)config.Gain); _offset.Value = Clamp(_offset, (decimal)config.Offset); _engineeringRangeCheck.Checked = config.EnableEngineeringRangeCheck; _engineeringMin.Value = Clamp(_engineeringMin, (decimal)config.EngineeringMin); _engineeringMax.Value = Clamp(_engineeringMax, (decimal)config.EngineeringMax); _username.Text = config.Username; _password.Text = AnalyzerConfigSecurityService.Unprotect(config.ProtectedPassword);
            _requestCommand.Text = config.RequestCommand; _responseDelimiter.Text = config.ResponseDelimiter; _responseFieldIndex.Value = Clamp(_responseFieldIndex, config.ResponseFieldIndex); _responseKey.Text = config.ResponseKey; _responseRegex.Text = config.ResponseRegex; _readDelay.Value = Clamp(_readDelay, config.ReadAfterWriteDelayMs); _encoding.Text = string.IsNullOrWhiteSpace(config.EncodingName) ? "ASCII" : config.EncodingName;
            ToggleConnectionFields();
        }

        private decimal Clamp(NumericUpDown n, decimal value) { return Math.Max(n.Minimum, Math.Min(n.Maximum, value)); }
        private void RefreshComPorts() { string current = cmbComPort.Text; cmbComPort.Items.Clear(); cmbComPort.Items.AddRange(SerialPort.GetPortNames().OrderBy(x => x).Cast<object>().ToArray()); if (!string.IsNullOrWhiteSpace(current) && !cmbComPort.Items.Contains(current)) cmbComPort.Items.Add(current); if (cmbComPort.Items.Count > 0 && string.IsNullOrWhiteSpace(cmbComPort.Text)) cmbComPort.SelectedIndex = 0; }
        private void ToggleConnectionFields() { bool serial = rdbCOM.Checked; pnlSerial.Visible = serial; pnlNetwork.Visible = !serial; }
        private void rdbConnectionType_CheckedChanged(object sender, EventArgs e) { ToggleConnectionFields(); }

        private AnalyzerValidationResult BuildAndValidate()
        {
            Configuration.Name = txtName.Text.Trim(); Configuration.Model = txtModel.Text.Trim(); Configuration.GasType = cmbGasType.Text; Configuration.Unit = cmbUnit.Text;
            if (_driverSelector.SelectedIndex >= 0) { var d = AnalyzerDriverCatalog.Items[_driverSelector.SelectedIndex]; Configuration.DriverId = d.Id; Configuration.Manufacturer = d.Manufacturer; }
            Configuration.Measurements = _measurementSelector.CheckedItems.Cast<AnalyzerMeasurementDescriptor>().Select((m,i)=>new AnalyzerMeasurementConfig { Name=m.Name, Unit=m.Unit, MapperId=m.MapperId, Channel=i+1, Enabled=true, Gain=1.0, Offset=0.0 }).ToList();
            if (Configuration.Measurements.Count > 0) { var primary=Configuration.Measurements.FirstOrDefault(x=>string.Equals(x.Name, Configuration.GasType, StringComparison.OrdinalIgnoreCase)) ?? Configuration.Measurements[0]; Configuration.GasType=primary.Name; Configuration.Unit=primary.Unit; Configuration.MapperId=primary.MapperId; }
            Configuration.Channel = (int)numChannel.Value; Configuration.DecimalDigits = (int)numDecimals.Value; Configuration.Enabled = _enabled.Checked; Configuration.DeviceId = _deviceId.Text.Trim(); Configuration.RequestIntervalMs = (int)_interval.Value; Configuration.TimeoutMs = (int)_timeout.Value; Configuration.DataBits = (int)_dataBits.Value;
            Configuration.Gain = (double)_gain.Value; Configuration.Offset = (double)_offset.Value; Configuration.EnableEngineeringRangeCheck = _engineeringRangeCheck.Checked; Configuration.EngineeringMin = (double)_engineeringMin.Value; Configuration.EngineeringMax = (double)_engineeringMax.Value; Configuration.Username = _username.Text.Trim(); Configuration.Password = _password.Text;
            Configuration.RequestCommand = _requestCommand.Text; Configuration.ResponseDelimiter = _responseDelimiter.Text; Configuration.ResponseFieldIndex = (int)_responseFieldIndex.Value; Configuration.ResponseKey = _responseKey.Text.Trim(); Configuration.ResponseRegex = _responseRegex.Text.Trim(); Configuration.ReadAfterWriteDelayMs = (int)_readDelay.Value; Configuration.EncodingName = string.IsNullOrWhiteSpace(_encoding.Text) ? "ASCII" : _encoding.Text.Trim();
            Parity parity; if (!Enum.TryParse(_parity.Text, out parity)) parity = Parity.None; Configuration.Parity = parity;
            StopBits stop; if (!Enum.TryParse(_stopBits.Text, out stop)) stop = StopBits.One; Configuration.StopBits = stop;
            if (rdbIP.Checked) { Configuration.ConnectionType = "IP"; Configuration.IpAddress = txtIpAddress.Text.Trim(); Configuration.IpPort = (int)numIpPort.Value; }
            else { Configuration.ConnectionType = "COM"; Configuration.ComPort = cmbComPort.Text.Trim(); int baud; Configuration.BaudRate = int.TryParse(cmbBaudRate.Text, out baud) ? baud : 0; }
            return AnalyzerConfigValidationService.Validate(Configuration, _storageService.LoadAnalyzers(), _isEditMode ? Configuration.Id : null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AnalyzerValidationResult result = BuildAndValidate();
            _validationStatus.Text = result.IsValid ? "✓ Configuration valid" : "✕ Validation failed";
            _validationStatus.ForeColor = result.IsValid ? Color.FromArgb(39, 174, 96) : Color.FromArgb(192, 57, 43);
            if (!result.IsValid || result.Warnings.Count > 0)
            {
                string message = result.ToMessage();
                if (!result.IsValid) { MessageBox.Show(message, "Configuration Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show(message + "\n\nSave anyway?", "Configuration Warnings", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            }
            Configuration.ProtectedPassword = AnalyzerConfigSecurityService.Protect(Configuration.Password); Configuration.Password = string.Empty;
            DialogResult = DialogResult.OK; Close();
        }

        private async Task TestConnectionAsync()
        {
            var result = BuildAndValidate();
            if (!result.IsValid) { MessageBox.Show(result.ToMessage(), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            _testButton.Enabled = false; _validationStatus.Text = "Testing..."; _validationStatus.ForeColor = Color.DarkOrange;
            try
            {
                var reader = new RealAnalyzerReader();
                var reading = await reader.ReadAsync(Configuration, System.Threading.CancellationToken.None);
                bool ok = reading.IsUsable;
                string detail = ok
                    ? string.Format("Live sample: {0:0.###} {1} ({2} ms)", reading.Value, Configuration.Unit, reading.ResponseTimeMs)
                    : reading.Message;
                _validationStatus.Text = ok ? "✓ " + detail : "✕ " + detail; _validationStatus.ForeColor = ok ? Color.FromArgb(39,174,96) : Color.FromArgb(192,57,43);
            }
            catch (Exception ex) { _validationStatus.Text = "✕ Test failed"; _validationStatus.ForeColor = Color.FromArgb(192,57,43); MessageBox.Show(ex.Message, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            finally { _testButton.Enabled = true; }
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
    }
}
