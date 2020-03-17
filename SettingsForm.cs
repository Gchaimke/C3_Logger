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
                Filter = "Text files (*.mdb)|*.mdb",
                Title = "Open Database file"
            };
            txbDBPath.Text = Properties.Settings.Default.dbPath;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.dbPath = txbDBPath.Text;
            Properties.Settings.Default.IP = txbIp.Text;
            Properties.Settings.Default.Port = txbIp.Text;
            Properties.Settings.Default.Pass = txbPass.Text;
            Properties.Settings.Default.Save();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
