namespace C3_Logger
{
    partial class MainForm
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
            this.txbLog = new System.Windows.Forms.TextBox();
            this.datePeack = new System.Windows.Forms.DateTimePicker();
            this.lblAbout = new System.Windows.Forms.Label();
            this.btnGetLog = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txbLog
            // 
            this.txbLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txbLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbLog.Location = new System.Drawing.Point(0, 118);
            this.txbLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbLog.Multiline = true;
            this.txbLog.Name = "txbLog";
            this.txbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbLog.Size = new System.Drawing.Size(481, 182);
            this.txbLog.TabIndex = 5;
            // 
            // datePeack
            // 
            this.datePeack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.datePeack.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePeack.Location = new System.Drawing.Point(106, 10);
            this.datePeack.Name = "datePeack";
            this.datePeack.Size = new System.Drawing.Size(115, 21);
            this.datePeack.TabIndex = 2;
            this.datePeack.Value = new System.DateTime(2020, 3, 18, 12, 30, 48, 0);
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAbout.Location = new System.Drawing.Point(434, 67);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(27, 15);
            this.lblAbout.TabIndex = 4;
            this.lblAbout.Text = "info";
            this.lblAbout.Click += new System.EventHandler(this.lblAbout_Click);
            // 
            // btnGetLog
            // 
            this.btnGetLog.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnGetLog.BackgroundImage = global::C3_Logger.Properties.Resources.calendar;
            this.btnGetLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGetLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetLog.Location = new System.Drawing.Point(12, 12);
            this.btnGetLog.Name = "btnGetLog";
            this.btnGetLog.Size = new System.Drawing.Size(88, 69);
            this.btnGetLog.TabIndex = 1;
            this.btnGetLog.UseVisualStyleBackColor = false;
            this.btnGetLog.Click += new System.EventHandler(this.btnGetLog_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSettings.BackgroundImage = global::C3_Logger.Properties.Resources.settings;
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Location = new System.Drawing.Point(428, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(41, 37);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 300);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnGetLog);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.datePeack);
            this.Controls.Add(this.txbLog);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "C3-200 Get Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txbLog;
        private System.Windows.Forms.DateTimePicker datePeack;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.Button btnGetLog;
        private System.Windows.Forms.Button btnSettings;
    }
}

