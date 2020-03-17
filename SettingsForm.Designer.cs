namespace C3_Logger
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txbDBPath = new System.Windows.Forms.TextBox();
            this.btnSelectDB = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txbIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSelectLog = new System.Windows.Forms.Button();
            this.txbLog = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Working Database";
            // 
            // txbDBPath
            // 
            this.txbDBPath.Location = new System.Drawing.Point(12, 48);
            this.txbDBPath.Name = "txbDBPath";
            this.txbDBPath.Size = new System.Drawing.Size(264, 20);
            this.txbDBPath.TabIndex = 1;
            // 
            // btnSelectDB
            // 
            this.btnSelectDB.Location = new System.Drawing.Point(282, 46);
            this.btnSelectDB.Name = "btnSelectDB";
            this.btnSelectDB.Size = new System.Drawing.Size(75, 23);
            this.btnSelectDB.TabIndex = 2;
            this.btnSelectDB.Text = "select";
            this.btnSelectDB.UseVisualStyleBackColor = true;
            this.btnSelectDB.Click += new System.EventHandler(this.btnSelectDB_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(282, 285);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txbIp
            // 
            this.txbIp.Location = new System.Drawing.Point(62, 184);
            this.txbIp.Name = "txbIp";
            this.txbIp.Size = new System.Drawing.Size(122, 20);
            this.txbIp.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 285);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Port";
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(62, 210);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(122, 20);
            this.txbPort.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Pass";
            // 
            // txbPass
            // 
            this.txbPass.Location = new System.Drawing.Point(62, 235);
            this.txbPass.Name = "txbPass";
            this.txbPass.Size = new System.Drawing.Size(122, 20);
            this.txbPass.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "C3-200 TCP Settings";
            // 
            // btnSelectLog
            // 
            this.btnSelectLog.Location = new System.Drawing.Point(282, 105);
            this.btnSelectLog.Name = "btnSelectLog";
            this.btnSelectLog.Size = new System.Drawing.Size(75, 23);
            this.btnSelectLog.TabIndex = 8;
            this.btnSelectLog.Text = "select";
            this.btnSelectLog.UseVisualStyleBackColor = true;
            this.btnSelectLog.Click += new System.EventHandler(this.btnSelectLog_Click);
            // 
            // txbLog
            // 
            this.txbLog.Location = new System.Drawing.Point(12, 107);
            this.txbLog.Name = "txbLog";
            this.txbLog.Size = new System.Drawing.Size(264, 20);
            this.txbLog.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Select Last Log CSV";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 320);
            this.Controls.Add(this.btnSelectLog);
            this.Controls.Add(this.txbLog);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txbPass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txbIp);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectDB);
            this.Controls.Add(this.txbDBPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsForm";
            this.Text = "Logger Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbDBPath;
        private System.Windows.Forms.Button btnSelectDB;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txbIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSelectLog;
        private System.Windows.Forms.TextBox txbLog;
        private System.Windows.Forms.Label label6;
    }
}