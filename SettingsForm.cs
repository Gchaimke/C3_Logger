using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C3_Logger
{
    public partial class SettingsForm : Form
    {
        private OpenFileDialog openFileDialog1;
        public SettingsForm()
        {
            InitializeComponent();
            openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Select a Database file",
                Filter = "Database file (*.mdb)|*.mdb",
                Title = "Open Database file"
            };
            txbDBPath.Text = Properties.Settings.Default.dbPath;
            txbLog.Text = Properties.Settings.Default.LogPath;
            txbIp.Text = Properties.Settings.Default.IP;
            txbPort.Text = Properties.Settings.Default.Port;
            txbPass.Text = Properties.Settings.Default.Pass;

        }

        private void btnSelectDB_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txbDBPath.Text = openFileDialog1.FileName;
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }
        private void btnSelectLog_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select a Log file";
            openFileDialog1.Filter = "Log CSV file (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FileName = "Select last Log file";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txbLog.Text = openFileDialog1.FileName;
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.dbPath = txbDBPath.Text;
            Properties.Settings.Default.IP = txbIp.Text;
            Properties.Settings.Default.Port = txbIp.Text;
            Properties.Settings.Default.Pass = txbPass.Text;
            Properties.Settings.Default.LogPath = txbLog.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
