namespace AshkanAQMS
{
    partial class SettingsForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.chkEnableAutoArchive = new System.Windows.Forms.CheckBox();
            this.chkEnableSound = new System.Windows.Forms.CheckBox();
            this.grpEngine = new System.Windows.Forms.GroupBox();
            this.lblEmaAlpha = new System.Windows.Forms.Label();
            this.numEmaAlpha = new System.Windows.Forms.NumericUpDown();
            this.lblZScore = new System.Windows.Forms.Label();
            this.numZScoreThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblHistorySize = new System.Windows.Forms.Label();
            this.numHistoryWindowSize = new System.Windows.Forms.NumericUpDown();
            this.lblPollingInterval = new System.Windows.Forms.Label();
            this.numPollingInterval = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpGeneral.SuspendLayout();
            this.grpEngine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEmaAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZScoreThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHistoryWindowSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPollingInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // grpGeneral
            // 
            this.grpGeneral.Controls.Add(this.chkEnableAutoArchive);
            this.grpGeneral.Controls.Add(this.chkEnableSound);
            this.grpGeneral.ForeColor = System.Drawing.Color.White;
            this.grpGeneral.Location = new System.Drawing.Point(18, 16);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(370, 95);
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "تنظیمات عمومی و هشدارها";
            // 
            // chkEnableAutoArchive
            // 
            this.chkEnableAutoArchive.AutoSize = true;
            this.chkEnableAutoArchive.Location = new System.Drawing.Point(20, 58);
            this.chkEnableAutoArchive.Name = "chkEnableAutoArchive";
            this.chkEnableAutoArchive.Size = new System.Drawing.Size(185, 20);
            this.chkEnableAutoArchive.TabIndex = 1;
            this.chkEnableAutoArchive.Text = "فعال‌سازی آرشیو خودکار داده‌ها";
            this.chkEnableAutoArchive.UseVisualStyleBackColor = true;
            // 
            // chkEnableSound
            // 
            this.chkEnableSound.AutoSize = true;
            this.chkEnableSound.Location = new System.Drawing.Point(20, 28);
            this.chkEnableSound.Name = "chkEnableSound";
            this.chkEnableSound.Size = new System.Drawing.Size(188, 20);
            this.chkEnableSound.TabIndex = 0;
            this.chkEnableSound.Text = "فعال‌سازی هشدارهای صوتی";
            this.chkEnableSound.UseVisualStyleBackColor = true;
            // 
            // grpEngine
            // 
            this.grpEngine.Controls.Add(this.lblEmaAlpha);
            this.grpEngine.Controls.Add(this.numEmaAlpha);
            this.grpEngine.Controls.Add(this.lblZScore);
            this.grpEngine.Controls.Add(this.numZScoreThreshold);
            this.grpEngine.Controls.Add(this.lblHistorySize);
            this.grpEngine.Controls.Add(this.numHistoryWindowSize);
            this.grpEngine.Controls.Add(this.lblPollingInterval);
            this.grpEngine.Controls.Add(this.numPollingInterval);
            this.grpEngine.ForeColor = System.Drawing.Color.White;
            this.grpEngine.Location = new System.Drawing.Point(18, 122);
            this.grpEngine.Name = "grpEngine";
            this.grpEngine.Size = new System.Drawing.Size(370, 160);
            this.grpEngine.TabIndex = 1;
            this.grpEngine.TabStop = false;
            this.grpEngine.Text = "تنظیمات موتور پایش و هوش مصنوعی";
            // 
            // lblEmaAlpha
            // 
            this.lblEmaAlpha.AutoSize = true;
            this.lblEmaAlpha.Location = new System.Drawing.Point(20, 125);
            this.lblEmaAlpha.Name = "lblEmaAlpha";
            this.lblEmaAlpha.Size = new System.Drawing.Size(117, 16);
            this.lblEmaAlpha.TabIndex = 7;
            this.lblEmaAlpha.Text = "ضریب هموارسازی (Alpha):";
            // 
            // numEmaAlpha
            // 
            this.numEmaAlpha.DecimalPlaces = 2;
            this.numEmaAlpha.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            this.numEmaAlpha.Location = new System.Drawing.Point(240, 122);
            this.numEmaAlpha.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numEmaAlpha.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            this.numEmaAlpha.Name = "numEmaAlpha";
            this.numEmaAlpha.Size = new System.Drawing.Size(110, 22);
            this.numEmaAlpha.TabIndex = 6;
            this.numEmaAlpha.Value = new decimal(new int[] { 3, 0, 0, 65536 });
            // 
            // lblZScore
            // 
            this.lblZScore.AutoSize = true;
            this.lblZScore.Location = new System.Drawing.Point(20, 93);
            this.lblZScore.Name = "lblZScore";
            this.lblZScore.Size = new System.Drawing.Size(114, 16);
            this.lblZScore.TabIndex = 5;
            this.lblZScore.Text = "آستانه Z-Score ناهنجاری:";
            // 
            // numZScoreThreshold
            // 
            this.numZScoreThreshold.DecimalPlaces = 2;
            this.numZScoreThreshold.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            this.numZScoreThreshold.Location = new System.Drawing.Point(240, 90);
            this.numZScoreThreshold.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            this.numZScoreThreshold.Minimum = new decimal(new int[] { 5, 0, 0, 65536 });
            this.numZScoreThreshold.Name = "numZScoreThreshold";
            this.numZScoreThreshold.Size = new System.Drawing.Size(110, 22);
            this.numZScoreThreshold.TabIndex = 4;
            this.numZScoreThreshold.Value = new decimal(new int[] { 25, 0, 0, 65536 });
            // 
            // lblHistorySize
            // 
            this.lblHistorySize.AutoSize = true;
            this.lblHistorySize.Location = new System.Drawing.Point(20, 61);
            this.lblHistorySize.Name = "lblHistorySize";
            this.lblHistorySize.Size = new System.Drawing.Size(119, 16);
            this.lblHistorySize.TabIndex = 3;
            this.lblHistorySize.Text = "اندازه پنجره تاریخچه (AI):";
            // 
            // numHistoryWindowSize
            // 
            this.numHistoryWindowSize.Location = new System.Drawing.Point(240, 58);
            this.numHistoryWindowSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            this.numHistoryWindowSize.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numHistoryWindowSize.Name = "numHistoryWindowSize";
            this.numHistoryWindowSize.Size = new System.Drawing.Size(110, 22);
            this.numHistoryWindowSize.TabIndex = 2;
            this.numHistoryWindowSize.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // lblPollingInterval
            // 
            this.lblPollingInterval.AutoSize = true;
            this.lblPollingInterval.Location = new System.Drawing.Point(20, 29);
            this.lblPollingInterval.Name = "lblPollingInterval";
            this.lblPollingInterval.Size = new System.Drawing.Size(130, 16);
            this.lblPollingInterval.TabIndex = 1;
            this.lblPollingInterval.Text = "بازه پایش داده‌ها (ثانیه):";
            // 
            // numPollingInterval
            // 
            this.numPollingInterval.Location = new System.Drawing.Point(240, 26);
            this.numPollingInterval.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            this.numPollingInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPollingInterval.Name = "numPollingInterval";
            this.numPollingInterval.Size = new System.Drawing.Size(110, 22);
            this.numPollingInterval.TabIndex = 0;
            this.numPollingInterval.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(293, 298);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 32);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "ذخیره";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(18, 298);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(110, 32);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "تنظیمات کارخانه";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(192, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(406, 345);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpEngine);
            this.Controls.Add(this.grpGeneral);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "تنظیمات سیستم AQMS";
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.grpEngine.ResumeLayout(false);
            this.grpEngine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEmaAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZScoreThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHistoryWindowSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPollingInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.CheckBox chkEnableAutoArchive;
        private System.Windows.Forms.CheckBox chkEnableSound;
        private System.Windows.Forms.GroupBox grpEngine;
        private System.Windows.Forms.Label lblPollingInterval;
        private System.Windows.Forms.NumericUpDown numPollingInterval;
        private System.Windows.Forms.Label lblHistorySize;
        private System.Windows.Forms.NumericUpDown numHistoryWindowSize;
        private System.Windows.Forms.Label lblZScore;
        private System.Windows.Forms.NumericUpDown numZScoreThreshold;
        private System.Windows.Forms.Label lblEmaAlpha;
        private System.Windows.Forms.NumericUpDown numEmaAlpha;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCancel;
    }
}
