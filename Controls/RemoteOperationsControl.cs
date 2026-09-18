using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanAQMS.Services;

namespace AshkanAQMS.Controls
{
    /// <summary>
    /// Modern, dependency-safe adaptation of the legacy RemoteForm concept.
    /// It provides a dashboard-ready remote operations surface while keeping
    /// hardware-specific protocol calls behind future transport adapters.
    /// </summary>
    public class RemoteOperationsControl : UserControl
    {
        private ComboBox _analyzer;
        private Button _connect;
        private Label _connection;
        private RichTextBox _screen;
        private RichTextBox _log;
        private Label _interlock;
        private CheckBox _simulation;
        private CheckBox _maintenance;
        private CheckBox _remoteEnabled;
        private Timer _heartbeat;
        private int _ticks;

        public RemoteOperationsControl()
        {
            BuildUi();
            UpdateInterlock();
        }

        private void BuildUi()
        {
            Dock = DockStyle.Fill;
            BackColor = IndustrialTheme.BackgroundDark;
            Padding = new Padding(20);

            var title = new Label { Dock = DockStyle.Top, Height = 36, Text = "Remote Operations & Calibration", Font = new Font("Segoe UI", 19F, FontStyle.Bold), ForeColor = IndustrialTheme.TextPrimary };
            var subtitle = new Label { Dock = DockStyle.Top, Height = 42, Text = "Analyzer remote control, calibration supervision and safety interlocks — ready for hardware transport adapters.", Font = new Font("Segoe UI", 9.5F), ForeColor = IndustrialTheme.TextSecondary };
            Controls.Add(subtitle); Controls.Add(title);

            var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, Padding = new Padding(0, 8, 0, 0) };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
            body.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
            body.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
            Controls.Add(body);

            var screenCard = Card("Analyzer Remote Screen");
            _screen = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.FromArgb(10, 15, 20), ForeColor = Color.LightGreen, Font = new Font("Consolas", 10F), BorderStyle = BorderStyle.None, Text = "AQMS REMOTE TERMINAL\n---------------------\nDevice: Demo Analyzer\nStatus: STANDBY\nMode: Local simulation\n\nUse the command buttons to exercise the remote workflow." };
            screenCard.Controls.Add(_screen); body.Controls.Add(screenCard, 0, 0);

            var controls = Card("Connection & Safety");
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(12) };
            _analyzer = new ComboBox { Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
            _analyzer.Items.AddRange(new object[] { "Demo Analyzer A", "Demo Analyzer B", "Ecotech-compatible adapter", "ESA-compatible adapter" });
            _analyzer.SelectedIndex = 0;
            _simulation = new CheckBox { Text = "Simulation / Demo mode", AutoSize = true, ForeColor = Color.White, Checked = true };
            _maintenance = new CheckBox { Text = "Maintenance authorization", AutoSize = true, ForeColor = Color.White };
            _remoteEnabled = new CheckBox { Text = "Remote control enabled", AutoSize = true, ForeColor = Color.White, Checked = true };
            _connect = ActionButton("Connect", 110);
            _connect.Click += Connect_Click;
            _connection = new Label { AutoSize = true, Text = "● Disconnected", ForeColor = Color.Silver, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            _interlock = new Label { AutoSize = false, Width = 260, Height = 48, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            _simulation.CheckedChanged += (s, e) => { Log("Simulation mode: " + (_simulation.Checked ? "ON" : "OFF")); UpdateInterlock(); };
            _maintenance.CheckedChanged += (s, e) => UpdateInterlock();
            _remoteEnabled.CheckedChanged += (s, e) => UpdateInterlock();
            flow.Controls.Add(new Label { Text = "Analyzer", AutoSize = true, ForeColor = IndustrialTheme.TextSecondary });
            flow.Controls.Add(_analyzer); flow.Controls.Add(_simulation); flow.Controls.Add(_maintenance); flow.Controls.Add(_remoteEnabled); flow.Controls.Add(_connect); flow.Controls.Add(_connection); flow.Controls.Add(_interlock);
            controls.Controls.Add(flow); body.Controls.Add(controls, 1, 0);

            var commands = Card("Remote Commands");
            var cmdFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), WrapContents = true, AutoScroll = true };
            string[] commandsList = { "Refresh Screen", "Up", "Down", "Page Up", "Select", "Enter", "Exit", "Stop" };
            foreach (var command in commandsList)
            {
                var b = ActionButton(command, 105); b.Tag = command; b.Click += Command_Click; cmdFlow.Controls.Add(b);
            }
            commands.Controls.Add(cmdFlow); body.Controls.Add(commands, 0, 1);

            var logCard = Card("Operation Log");
            _log = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BackColor = IndustrialTheme.SurfaceCard, ForeColor = IndustrialTheme.TextSecondary, BorderStyle = BorderStyle.None, Font = new Font("Consolas", 8.5F) };
            logCard.Controls.Add(_log); body.Controls.Add(logCard, 1, 1);

            _heartbeat = new Timer { Interval = 1000 }; _heartbeat.Tick += Heartbeat_Tick;
        }

        private Panel Card(string header)
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = IndustrialTheme.SurfaceCard, Padding = new Padding(12), Margin = new Padding(6) };
            var label = new Label { Dock = DockStyle.Top, Height = 28, Text = header, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = IndustrialTheme.TextPrimary };
            panel.Controls.Add(label); return panel;
        }

        private Button ActionButton(string text, int width)
        {
            return new Button { Text = text, Width = width, Height = 34, Margin = new Padding(4), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(41, 128, 185), ForeColor = Color.White };
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (_connection.Text.Contains("Connected"))
            {
                _heartbeat.Stop(); _connection.Text = "● Disconnected"; _connection.ForeColor = Color.Silver; _connect.Text = "Connect"; Log("Remote session closed safely."); return;
            }
            if (!CanOperate()) { MessageBox.Show("Remote control is blocked by the safety interlock. Enable remote control and maintenance authorization, or use Simulation/Demo mode.", "Safety Interlock", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            _connection.Text = "● Connected"; _connection.ForeColor = Color.LightGreen; _connect.Text = "Disconnect"; _heartbeat.Start(); Log("Connected to " + _analyzer.Text + " (" + (_simulation.Checked ? "simulation" : "adapter") + ")."); RenderScreen("CONNECTED", "Remote session active");
        }

        private void Command_Click(object sender, EventArgs e)
        {
            var command = ((Button)sender).Tag.ToString();
            if (!CanOperate() || !_connection.Text.Contains("Connected")) { MessageBox.Show("Command blocked: connect first and satisfy the safety interlock.", "Remote Control", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            Log("Command sent: " + command);
            RenderScreen(command.ToUpperInvariant(), "Command acknowledged by demo transport");
        }

        private bool CanOperate() { return _remoteEnabled.Checked && (_simulation.Checked || _maintenance.Checked); }

        private void UpdateInterlock()
        {
            bool ok = CanOperate();
            _interlock.Text = ok ? "● SAFETY INTERLOCK: READY\nRemote commands permitted." : "● SAFETY INTERLOCK: BLOCKED\nAuthorization required.";
            _interlock.ForeColor = ok ? Color.LightGreen : Color.Orange;
        }

        private void RenderScreen(string status, string detail)
        {
            _screen.Text = "AQMS REMOTE TERMINAL\n---------------------\nDevice: " + _analyzer.Text + "\nStatus: " + status + "\nMode: " + (_simulation.Checked ? "SIMULATION" : "HARDWARE ADAPTER") + "\nTime: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n\n" + detail;
        }

        private void Heartbeat_Tick(object sender, EventArgs e)
        {
            _ticks++; if (_ticks % 5 == 0) Log("Heartbeat OK • remote session alive.");
        }

        private void Log(string message)
        {
            if (_log == null) return;
            _log.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + message + Environment.NewLine);
            _log.SelectionStart = _log.TextLength; _log.ScrollToCaret();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _heartbeat != null) { _heartbeat.Stop(); _heartbeat.Dispose(); }
            base.Dispose(disposing);
        }
    }
}
