namespace AshkanAQMS
{
    partial class AnalyzerManagementControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvAnalyzers = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGasType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConnection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpDiagnostics = new System.Windows.Forms.GroupBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.lblDiagnosticStatus = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnalyzers)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.grpDiagnostics.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1100, 80);

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblSubtitle.Location = new System.Drawing.Point(24, 45);
            this.lblSubtitle.Text = "Manage device connection parameters, gas types, channel and scale decimals";

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 42, 54);
            this.lblTitle.Location = new System.Drawing.Point(22, 12);
            this.lblTitle.Text = "Analyzer Config & Management";

            // dgvAnalyzers
            this.dgvAnalyzers.AllowUserToAddRows = false;
            this.dgvAnalyzers.AllowUserToDeleteRows = false;
            this.dgvAnalyzers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAnalyzers.BackgroundColor = System.Drawing.Color.White;
            this.dgvAnalyzers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAnalyzers.ColumnHeadersHeight = 30;
            this.dgvAnalyzers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colId, this.colName, this.colModel, this.colGasType, this.colUnit, this.colConnection
            });
            this.dgvAnalyzers.Location = new System.Drawing.Point(24, 100);
            this.dgvAnalyzers.Name = "dgvAnalyzers";
            this.dgvAnalyzers.ReadOnly = true;
            this.dgvAnalyzers.RowHeadersVisible = false;
            this.dgvAnalyzers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAnalyzers.Size = new System.Drawing.Size(850, 320);
            this.dgvAnalyzers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAnalyzers_CellDoubleClick);

            // DataGridView Column Designs
            this.colId.Visible = false;
            this.colId.Name = "colId";
            this.colName.HeaderText = "Analyzer Name";
            this.colName.Name = "colName";
            this.colModel.HeaderText = "Model";
            this.colModel.Name = "colModel";
            this.colGasType.HeaderText = "Gas/Type";
            this.colGasType.Name = "colGasType";
            this.colUnit.HeaderText = "Unit";
            this.colUnit.Name = "colUnit";
            this.colConnection.HeaderText = "Connection Parameters";
            this.colConnection.Name = "colConnection";

            // pnlActions (دکمه‌های افزودن/ویرایش/حذف)
            this.pnlActions.Controls.Add(this.btnAdd);
            this.pnlActions.Controls.Add(this.btnEdit);
            this.pnlActions.Controls.Add(this.btnDelete);
            this.pnlActions.Location = new System.Drawing.Point(890, 100);
            this.pnlActions.Size = new System.Drawing.Size(180, 200);

            // btnAdd
            this.btnAdd.Text = "Add Analyzer";
            this.btnAdd.Location = new System.Drawing.Point(10, 10);
            this.btnAdd.Size = new System.Drawing.Size(160, 40);
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // btnEdit
            this.btnEdit.Text = "Edit Configuration";
            this.btnEdit.Location = new System.Drawing.Point(10, 60);
            this.btnEdit.Size = new System.Drawing.Size(160, 40);
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Text = "Remove Device";
            this.btnDelete.Location = new System.Drawing.Point(10, 110);
            this.btnDelete.Size = new System.Drawing.Size(160, 40);
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // grpDiagnostics (بخش عیب‌یابی کابل و پورت)
            this.grpDiagnostics.Text = "Hardware Connection Diagnostics";
            this.grpDiagnostics.Location = new System.Drawing.Point(24, 440);
            this.grpDiagnostics.Size = new System.Drawing.Size(1046, 100);
            this.grpDiagnostics.Controls.Add(this.btnTestConnection);
            this.grpDiagnostics.Controls.Add(this.lblDiagnosticStatus);

            // btnTestConnection
            this.btnTestConnection.Text = "Test Port / Connection";
            this.btnTestConnection.Location = new System.Drawing.Point(20, 35);
            this.btnTestConnection.Size = new System.Drawing.Size(180, 40);
            this.btnTestConnection.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnTestConnection.ForeColor = System.Drawing.Color.White;
            this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);

            // lblDiagnosticStatus
            this.lblDiagnosticStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblDiagnosticStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblDiagnosticStatus.Location = new System.Drawing.Point(220, 45);
            this.lblDiagnosticStatus.Size = new System.Drawing.Size(800, 25);
            this.lblDiagnosticStatus.Text = "Select an analyzer above and click \"Test Port / Connection\" to verify physical cable status.";

            // UserControl setup
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Controls.Add(this.grpDiagnostics);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.dgvAnalyzers);
            this.Controls.Add(this.pnlHeader);
            this.Size = new System.Drawing.Size(1100, 560);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnalyzers)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.grpDiagnostics.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvAnalyzers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGasType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConnection;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox grpDiagnostics;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Label lblDiagnosticStatus;
    }
}
