namespace C3_Logger
{
    using System;
    using System.Security;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="SettingsForm" />
    /// </summary>
    public partial class SettingsForm : Form
    {
        /// <summary>
        /// Defines the openFileDialog1
        /// </summary>
        private OpenFileDialog openFileDialog1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsForm"/> class.
        /// </summary>
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

        /// <summary>
        /// The btnSelectDB_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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

        /// <summary>
        /// The btnSelectLog_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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

        /// <summary>
        /// The btnSave_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.dbPath = txbDBPath.Text;
            Properties.Settings.Default.IP = txbIp.Text;
            Properties.Settings.Default.Port = txbPort.Text;
            Properties.Settings.Default.Pass = txbPass.Text;
            Properties.Settings.Default.LogPath = txbLog.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        /// <summary>
        /// The btnCancel_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
