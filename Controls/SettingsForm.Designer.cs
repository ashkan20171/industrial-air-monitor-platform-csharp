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
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.grpAi = new System.Windows.Forms.GroupBox();
            this.grpAlerts = new System.Windows.Forms.GroupBox();
            this.chkEnableSound = new System.Windows.Forms.CheckBox();
            this.chkEnableAutoArchive = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlContainer.SuspendLayout();
            this.grpAlerts.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.btnReset);
            this.pnlContainer.Controls.Add(this.btnCancel);
            this.pnlContainer.Controls.Add(this.btnSave);
            this.pnlContainer.Controls.Add(this.grpAlerts);
            this.pnlContainer.Controls.Add(this.grpAi);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(16);
            this.pnlContainer.Size = new System.Drawing.Size(464, 391);
            this.pnlContainer.TabIndex = 0;
            // 
            // grpAi
            // 
            this.grpAi.Location = new System.Drawing.Point(16, 16);
            this.grpAi.Name = "grpAi";
            this.grpAi.Size = new System.Drawing.Size(430, 180);
            this.grpAi.TabIndex = 0;
            this.grpAi.TabStop = false;
            this.grpAi.Text = " پارامترهای تحلیل و پردازش داده (AI & Data) ";
            // 
            // grpAlerts
            // 
            this.grpAlerts.Controls.Add(this.chkEnableAutoArchive);
            this.grpAlerts.Controls.Add(this.chkEnableSound);
            this.grpAlerts.Location = new System.Drawing.Point(16, 205);
            this.grpAlerts.Name = "grpAlerts";
            this.grpAlerts.Size = new System.Drawing.Size(430, 100);
            this.grpAlerts.TabIndex = 1;
            this.grpAlerts.TabStop = false;
            this.grpAlerts.Text = " اعلان‌ها و بایگانی ";
            // 
            // chkEnableSound
            // 
            this.chkEnableSound.AutoSize = true;
            this.chkEnableSound.Location = new System.Drawing.Point(20, 28);
            this.chkEnableSound.Name = "chkEnableSound";
            this.chkEnableSound.Size = new System.Drawing.Size(273, 21);
            this.chkEnableSound.TabIndex = 0;
            this.chkEnableSound.Text = "پخش هشدار صوتی در زمان شناسایی ناهنجاری آنی";
            this.chkEnableSound.UseVisualStyleBackColor = true;
            // 
            // chkEnableAutoArchive
            // 
            this.chkEnableAutoArchive.AutoSize = true;
            this.chkEnableAutoArchive.Location = new System.Drawing.Point(20, 60);
            this.chkEnableAutoArchive.Name = "chkEnableAutoArchive";
            this.chkEnableAutoArchive.Size = new System.Drawing.Size(251, 21);
            this.chkEnableAutoArchive.TabIndex = 1;
            this.chkEnableAutoArchive.Text = "بایگانی خودکار داده‌های ثبت‌شده در دیتابیس";
            this.chkEnableAutoArchive.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(16, 325);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 35);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "ذخیره و اعمال";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(65)))), ((int)(((byte)(78)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(135, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 35);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(63)))));
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Location = new System.Drawing.Point(360, 325);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(85, 35);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "پیش‌فرض";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(464, 391);
            this.Controls.Add(this.pnlContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "تنظیمات سامانه مانیتورینگ و هوش مصنوعی";
            this.pnlContainer.ResumeLayout(false);
            this.grpAlerts.ResumeLayout(false);
            this.grpAlerts.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.GroupBox grpAi;
        private System.Windows.Forms.GroupBox grpAlerts;
        private System.Windows.Forms.CheckBox chkEnableSound;
        private System.Windows.Forms.CheckBox chkEnableAutoArchive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
    }
}
