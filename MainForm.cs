namespace C3_Logger
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="MainForm" />.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Defines the settingsForm.
        /// </summary>
        private SettingsForm settingsForm;

        /// <summary>
        /// Defines the documents.
        /// </summary>
        internal String documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ESD_LOGS\\";

        /// <summary>
        /// Defines the rowLog.
        /// </summary>
        internal String rowLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ESD_LOGS\\ESD_LOG.ROW";

        /// <summary>
        /// Defines the csvLog.
        /// </summary>
        internal String csvLog = Properties.Settings.Default.LogPath;

        /// <summary>
        /// Defines the mdfFile.
        /// </summary>
        internal String mdfFile = Properties.Settings.Default.dbPath;

        /// <summary>
        /// Defines the strcount.
        /// </summary>
        internal string strcount = "";

        /// <summary>
        /// Defines the h.
        /// </summary>
        internal IntPtr h = IntPtr.Zero;

        /// <summary>
        /// Defines the users.
        /// </summary>
        internal Dictionary<int, String> users = new Dictionary<int, String>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            if (!Directory.Exists(documents))
            {
                Directory.CreateDirectory(documents);
                File.Copy(@"Resources\script.js", documents + "script.js");
                File.Copy(@"Resources\styles.css", documents + "styles.css");
            }
            if (!File.Exists(documents + "script.js"))
                File.Copy(@"Resources\script.js", documents + "script.js");
            if (!File.Exists(documents + "styles.css"))
                File.Copy(@"Resources\styles.css", documents + "styles.css");
            csvLog = Properties.Settings.Default.LogPath;
            if (!File.Exists(csvLog))
                csvLog = documents + "ESD_LOG.csv";
        }

        /// <summary>
        /// The Connect.
        /// </summary>
        /// <param name="Parameters">The Parameters<see cref="string"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        [DllImport(@"lib\plcommpro.dll", EntryPoint = "Connect")]
        public static extern IntPtr Connect(string Parameters);

        /// <summary>
        /// The PullLastError.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        [DllImport(@"lib\plcommpro.dll", EntryPoint = "PullLastError")]
        public static extern int PullLastError();

        /// <summary>
        /// The Disconnect.
        /// </summary>
        /// <param name="h">The h<see cref="IntPtr"/>.</param>
        [DllImport(@"lib\plcommpro.dll", EntryPoint = "Disconnect")]
        public static extern void Disconnect(IntPtr h);

        /// <summary>
        /// The GetDeviceData.
        /// </summary>
        /// <param name="h">The h<see cref="IntPtr"/>.</param>
        /// <param name="buffer">The buffer<see cref="byte"/>.</param>
        /// <param name="buffersize">The buffersize<see cref="int"/>.</param>
        /// <param name="tablename">The tablename<see cref="string"/>.</param>
        /// <param name="filename">The filename<see cref="string"/>.</param>
        /// <param name="filter">The filter<see cref="string"/>.</param>
        /// <param name="options">The options<see cref="string"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        [DllImport(@"lib\plcommpro.dll", EntryPoint = "GetDeviceData")]
        public static extern int GetDeviceData(IntPtr h, ref byte buffer, int buffersize, string tablename, string filename, string filter, string options);

        /// <summary>
        /// The btnConnect_Click.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool connect()
        {
            Cursor = Cursors.WaitCursor;
            string str = "";
            str = "protocol=TCP,ipaddress=" + Properties.Settings.Default.IP +
                ",port=" + Properties.Settings.Default.Port +
                ",timeout=3000,passwd=" + Properties.Settings.Default.Pass;  //protocol=TCP,ipaddress=192.168.2.46,port=4370,timeout=2000,passwd=
            if (IntPtr.Zero == h)
            {
                h = Connect(str);

                if (h != IntPtr.Zero)
                {
                    txbLog.Text += "Connected!" + Environment.NewLine;
                    getLog();
                    Disconnect(h);
                    h = IntPtr.Zero;
                    txbLog.Text += "Disconnected!" + Environment.NewLine;
                    Cursor = Cursors.Default;
                    return true;
                }
                else
                {
                    txbLog.Text += "Connection error! Can't connect to address " + Properties.Settings.Default.IP + ":" + Properties.Settings.Default.Port + Environment.NewLine;
                }
                Cursor = Cursors.Default;

            }
            return false;
        }

        /// <summary>
        /// The getLog.
        /// </summary>
        private void getLog()
        {
            int ret = 0;
            string str = "Cardno\tPin\tEventType\tInOutState\tTime_second";
            int BUFFERSIZE = 4 * 1024 * 1024;
            byte[] buffer = new byte[BUFFERSIZE];
            string options = "";

            if (IntPtr.Zero != h)
            {
                ret = GetDeviceData(h, ref buffer[0], BUFFERSIZE, "transaction", str, "EventType=207", options);
            }
            else
            {
                MessageBox.Show("Connect device failed!");
                return;
            }

            if (ret >= 0)
            {
                strcount = Encoding.Default.GetString(buffer);
                try
                {
                    StreamWriter sw = new StreamWriter(rowLog);
                    sw.WriteLine(strcount);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Get Log error: " + ex.Message);
                    txbLog.Text += ex.Message + Environment.NewLine;
                }
                saveLog();
                txbLog.Text += "Get " + ret + " records" + Environment.NewLine;
            }
            else
            {
                txbLog.Text += "Get data failed.The error is " + ret + Environment.NewLine;
                return;
            }
        }

        /// <summary>
        /// The formatLog.
        /// </summary>
        /// <param name="rowTime">The rowTime<see cref="String"/>.</param>
        /// <returns>The <see cref="String[]"/>.</returns>
        private String[] formatLog(String rowTime)
        {
            int second, minute, hour;
            String strSecond, strMinute, strHour;
            String[] date = new String[4]; //time,day,month,year
            int time = Int32.Parse(rowTime);
            second = time % 60;
            if (second < 10)
                strSecond = "0" + second;
            else
                strSecond = "" + second;
            time = time / 60;
            minute = time % 60;
            if (minute < 10)
                strMinute = "0" + minute;
            else
                strMinute = "" + minute;
            time = time / 60;
            hour = time % 24;
            if (hour < 10)
                strHour = "0" + hour;
            else
                strHour = "" + hour;

            date[0] = String.Format("{0}:{1}:{2}", strHour, strMinute, strSecond);

            time = time / 24;
            date[1] = (time % 31 + 1) + "";

            time = time / 31;
            date[2] = (time % 12 + 1) + "";

            time = time / 12;
            date[3] = (time + 2000) + "";
            return date;
        }

        /// <summary>
        /// The getUserNames.
        /// </summary>
        private void getUserNames()
        {
            mdfFile = Properties.Settings.Default.dbPath;
            if (!File.Exists(mdfFile))
                mdfFile = @"C:\ZKTeco\ZKAccess3.5\access.mdb";
            if (!File.Exists(mdfFile))
            {
                txbLog.Text += "Can't connect to DB: " + mdfFile + Environment.NewLine;
                return;
            }
            txbLog.Text += "Get DB " + mdfFile + Environment.NewLine;
            using (OleDbConnection connection = new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", mdfFile)))
            {
                using (OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM USERINFO", connection))
                {
                    connection.Open();

                    DataTable table = new DataTable();
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    adapter.SelectCommand = selectCommand;
                    adapter.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        object nameValue = row["NAME"];
                        object userNum = row["Badgenumber"];
                        try
                        {
                            users.Add(Int16.Parse(userNum.ToString()), nameValue.ToString());
                        }
                        catch (ArgumentException)
                        {

                        }

                    }

                }
            }
        }

        /// <summary>
        /// The saveLog.
        /// </summary>
        private void saveLog()
        {
            try
            {
                getUserNames();
            }
            catch (Exception ex)
            {
                txbLog.Text += "Save log error! Can't get user names" + ex.Message + Environment.NewLine;
                return;
            }


            StreamReader sr = new StreamReader(rowLog);
            try
            {
                StreamWriter sw = new StreamWriter(csvLog);
                sw.WriteLine("Name," + sr.ReadLine() + ",Day,Month,Year");
                if (users.Count <= 0)
                {
                    txbLog.Text += "User list i empty!" + Environment.NewLine;
                    sw.Close();
                    sr.Close();
                    delRow();
                    return;
                }
                else
                {
                    while (sr.Peek() > 0)
                    {
                        String[] arr = sr.ReadLine().Split(',');
                        String[] date = formatLog(arr[4]);
                        String line = "";
                        line = users[Int16.Parse(arr[1])] + "," + arr[0] + "," + arr[1] + "," + arr[2] + ",PASSED," + date[0] + "," + date[1] + "," + date[2] + "," + date[3];
                        sw.WriteLine(line);
                    }

                    txbLog.Text += "Log created: " + csvLog + Environment.NewLine;
                    sw.Close();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Save Log Exception: " + ex.Message);
                txbLog.Text += ex.Message + Environment.NewLine;
            }
            delRow();
        }

        /// <summary>
        /// The delRow.
        /// </summary>
        private void delRow()
        {
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(rowLog))
                {
                    // If file found, delete it    
                    File.Delete(rowLog);
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
        }

        /// <summary>
        /// The buildHTML.
        /// </summary>
        /// <param name="year">The year<see cref="int"/>.</param>
        /// <param name="month">The month<see cref="int"/>.</param>
        private void buildHTML(int year, int month)
        {
            int nodays = DateTime.DaysInMonth(year, month);
            String reportFile = documents + year + "_" + month + ".html";
            if (users.Count <= 0)
                return;
            if (!File.Exists(reportFile))
            {
                try
                {
                    StreamReader sr = new StreamReader(csvLog);
                    StreamWriter sw = new StreamWriter(reportFile);

                    String page_start = "<!doctype html>\n <html lang = \"en\">\n<head><meta charset = \"utf-8\">\n" +
                        "<title> ESD Table</title>\n" +
                        "<meta name=\"description\"content=\"The HTML5 Herald\">\n" +
                        "<meta name=\"author\"content=\"SitePoint\">\n" +
                        "<link rel=\"stylesheet\" href=\"styles.css?v=1.0\"> </head><body>\n" +
                        "<script src=\"script.js\"></script>\n" +
                        "<h2>ESD tests for month: <span id='date'>" + datePeack.Value.Month + "/" + datePeack.Value.Year + "</span></h2>";

                    sw.WriteLine(page_start);
                    String table = "<table id='tbMonth' border='1'>";
                    String[] headLine = sr.ReadLine().Split(',');
                    table += "<tr><th>ID</th> <th>User Name</th>";
                    for (int i = 1; i <= nodays; i++)
                    {
                        table += "<th>" + i + "</th>";
                    }

                    table += "</tr>\n";

                    var stats = getUsersStats(year, month);
                    for (int i = 1; i < stats.GetLength(0); i++)
                    {
                        table += string.Format("<tr><td class='id'>{0}</td><td class='name'>{1}</td>", i, stats[i, 0]);
                        for (int j = 1; j < stats.GetLength(1); j++)
                        {
                            table += string.Format("<td class='day'>{0}</td>", stats[i, j]);
                        }
                        table += "</tr>\n";
                    }

                    sw.WriteLine(table);

                    String page_end = "</body></html>";
                    sw.WriteLine(page_end);
                    sr.Close();
                    sw.Close();
                    System.Diagnostics.Process.Start(reportFile);
                    txbLog.Text += "Report created: " + documents + year + "_" + month + ".html" + Environment.NewLine;
                }
                catch (FileNotFoundException ex)
                {
                    txbLog.Text += "Build HTML Error: " + ex.Message + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    txbLog.Text += "Build HTML Error: " + ex.Message + Environment.NewLine;
                }
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("delete previous, and create new?", "Report exists!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    System.Diagnostics.Process.Start(reportFile);
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    File.Delete(reportFile);
                    buildHTML(year, month);
                }
            }
            System.Diagnostics.Process.Start(documents);
        }

        /// <summary>
        /// The getUsersStats.
        /// </summary>
        /// <param name="year">The year<see cref="int"/>.</param>
        /// <param name="month">The month<see cref="int"/>.</param>
        /// <returns>The <see cref="String[,]"/>.</returns>
        private String[,] getUsersStats(int year, int month)
        {
            int monthDays = DateTime.DaysInMonth(year, month);
            String[,] monthStats = new string[users.Count + 1, monthDays + 1];

            if (File.Exists(csvLog))
            {
                foreach (var user in users)
                {
                    StreamReader sr = new StreamReader(csvLog);
                    String[] tmp = sr.ReadLine().Split(',');
                    monthStats[user.Key, 0] = user.Value;
                    for (int i = 0; i < monthDays; i++)
                    {
                        while (sr.Peek() > 0)
                        {
                            tmp = sr.ReadLine().Split(',');
                            if (Int16.Parse(tmp[7]) == month && Int16.Parse(tmp[8]) == year && user.Value.Equals(tmp[0]))
                            {
                                monthStats[user.Key, Int16.Parse(tmp[6])] = " V ";
                            }
                        }
                    }
                    sr.Close();
                }

            }
            else
            {
                txbLog.Text += csvLog + " not found" + Environment.NewLine;
            }
            return monthStats;
        }

        /// <summary>
        /// The lblAbout_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void lblAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Chaim Gorbov for Avdor-HELET", "About C3-200 Logger", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// The btnGetLog_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnGetLog_Click(object sender, EventArgs e)
        {
            if (connect())
            {
                buildHTML(datePeack.Value.Year, datePeack.Value.Month);
            }
        }

        /// <summary>
        /// The btnSettings_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            settingsForm = new SettingsForm();
            settingsForm.Show();
        }
    }
}
