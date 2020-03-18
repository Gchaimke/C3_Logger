﻿namespace C3_Logger
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.txbLog = new System.Windows.Forms.TextBox();
            this.pbCalendar = new System.Windows.Forms.PictureBox();
            this.pbSettings = new System.Windows.Forms.PictureBox();
            this.datePeack = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.pbCalendar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnConnect.Location = new System.Drawing.Point(13, 12);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(87, 72);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Get Log";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txbLog
            // 
            this.txbLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txbLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbLog.Location = new System.Drawing.Point(0, 104);
            this.txbLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txbLog.Multiline = true;
            this.txbLog.Name = "txbLog";
            this.txbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbLog.Size = new System.Drawing.Size(571, 196);
            this.txbLog.TabIndex = 2;
            // 
            // pbCalendar
            // 
            this.pbCalendar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCalendar.Image = global::C3_Logger.Properties.Resources.calendar;
            this.pbCalendar.Location = new System.Drawing.Point(397, 12);
            this.pbCalendar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbCalendar.Name = "pbCalendar";
            this.pbCalendar.Size = new System.Drawing.Size(86, 72);
            this.pbCalendar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCalendar.TabIndex = 5;
            this.pbCalendar.TabStop = false;
            this.pbCalendar.Click += new System.EventHandler(this.pbCalendar_Click);
            this.pbCalendar.MouseLeave += new System.EventHandler(this.pbCalendar_MouseLeave);
            this.pbCalendar.MouseHover += new System.EventHandler(this.pbCalendar_MouseHover);
            // 
            // pbSettings
            // 
            this.pbSettings.BackColor = System.Drawing.Color.Transparent;
            this.pbSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSettings.Image = global::C3_Logger.Properties.Resources.settings;
            this.pbSettings.Location = new System.Drawing.Point(525, 12);
            this.pbSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbSettings.Name = "pbSettings";
            this.pbSettings.Size = new System.Drawing.Size(33, 34);
            this.pbSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSettings.TabIndex = 4;
            this.pbSettings.TabStop = false;
            this.pbSettings.Click += new System.EventHandler(this.pbSettings_Click);
            this.pbSettings.MouseLeave += new System.EventHandler(this.pbSettings_MouseLeave);
            this.pbSettings.MouseHover += new System.EventHandler(this.pbSettings_MouseHover);
            // 
            // datePeack
            // 
            this.datePeack.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePeack.Location = new System.Drawing.Point(226, 12);
            this.datePeack.Name = "datePeack";
            this.datePeack.Size = new System.Drawing.Size(115, 21);
            this.datePeack.TabIndex = 6;
            this.datePeack.Value = new System.DateTime(2020, 3, 18, 12, 30, 48, 0);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 300);
            this.Controls.Add(this.datePeack);
            this.Controls.Add(this.pbCalendar);
            this.Controls.Add(this.pbSettings);
            this.Controls.Add(this.txbLog);
            this.Controls.Add(this.btnConnect);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "C3-200 Get Log";
            ((System.ComponentModel.ISupportInitialize)(this.pbCalendar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txbLog;
        private System.Windows.Forms.PictureBox pbSettings;
        private System.Windows.Forms.PictureBox pbCalendar;
        private System.Windows.Forms.DateTimePicker datePeack;
    }
}

