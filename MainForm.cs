namespace C3_Logger
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Deployment.Application;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        private SettingsForm settingsForm;
        internal String documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ESD_LOGS\\";
        internal String rowLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ESD_LOGS\\ESD_LOG.ROW";
        internal String csvLog = Properties.Settings.Default.LogPath;
        internal String mdfFile = Properties.Settings.Default.dbPath;
        internal string strcount = "";
        internal IntPtr h = IntPtr.Zero;
        internal SortedDictionary<int, String> users = new SortedDictionary<int, String>();
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
            datePeack.Value = DateTime.Now;
        }

        [DllImport(@"lib\plcommpro.dll", EntryPoint = "Connect")]
        public static extern IntPtr Connect_to_Device(string Parameters);

        [DllImport(@"lib\plcommpro.dll", EntryPoint = "PullLastError")]
        public static extern int PullLastError();

        [DllImport(@"lib\plcommpro.dll", EntryPoint = "Disconnect")]
        public static extern void Disconnect(IntPtr h);

        [DllImport(@"lib\plcommpro.dll", EntryPoint = "GetDeviceData")]
        public static extern int GetDeviceData(IntPtr h, ref byte buffer, int buffersize, string tablename, string filename, string filter, string options);

        private bool Connect()
        {
            Cursor = Cursors.WaitCursor;
            string str = "";
            str = "protocol=TCP,ipaddress=" + Properties.Settings.Default.IP +
                ",port=" + Properties.Settings.Default.Port +
                ",timeout=3000,passwd=" + Properties.Settings.Default.Pass;  //protocol=TCP,ipaddress=192.168.2.46,port=4370,timeout=2000,passwd=
            if (IntPtr.Zero == h)
            {
                h = Connect_to_Device(str);

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
                txbLog.Text += "Start Save log " + Environment.NewLine;
                SaveLog();
                txbLog.Text += "Get " + ret + " records" + Environment.NewLine;
            }
            else
            {
                txbLog.Text += "Get data failed.The error is " + ret + Environment.NewLine;
                return;
            }
        }

        private string[] FormatLog(String rowTime)
        {
            int second, minute, hour;
            string strSecond, strMinute, strHour;
            string[] date = new string[4]; //time,day,month,year
            int time = Int32.Parse(rowTime);
            second = time % 60;
            if (second < 10)
                strSecond = "0" + second;
            else
                strSecond = "" + second;
            time /= 60;
            minute = time % 60;
            if (minute < 10)
                strMinute = "0" + minute;
            else
                strMinute = "" + minute;
            time /= 60;
            hour = time % 24;
            if (hour < 10)
                strHour = "0" + hour;
            else
                strHour = "" + hour;

            date[0] = string.Format("{0}:{1}:{2}", strHour, strMinute, strSecond);

            time /= 24;
            date[1] = (time % 31 + 1) + "";

            time /= 31;
            date[2] = (time % 12 + 1) + "";

            time /= 12;
            date[3] = (time + 2000) + "";
            return date;
        }

        private void GetUserNames()
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
                        users.Add(Int16.Parse(userNum.ToString()), nameValue.ToString());
                    }
                }
            }
        }

        private void SaveLog()
        {
            try
            {
                GetUserNames();
            }
            catch (Exception ex)
            {
                txbLog.Text += "Save log error! Can't get user names" + ex.Message + Environment.NewLine;
                return;
            }
            txbLog.Text += "End Get Users." + Environment.NewLine;

            StreamReader sr = new StreamReader(rowLog);
            StreamWriter sw = new StreamWriter(csvLog);
            try
            {
                sw.WriteLine("Name," + sr.ReadLine() + ",Day,Month,Year");
                if (users.Count <= 0)
                {
                    txbLog.Text += "User list is empty!" + Environment.NewLine;
                    sw.Close();
                    sr.Close();
                    Delete_row_file();
                    return;
                }
                else
                {
                    while (sr.Peek() > 0)
                    {
                        String[] arr = sr.ReadLine().Split(',');
                        String[] date = FormatLog(arr[4]);
                        String line = "";
                        if (users.ContainsKey(Int16.Parse(arr[1])))
                        {
                            line = users[Int16.Parse(arr[1])] + "," + arr[0] + "," + arr[1] + "," + arr[2] + ",PASSED," + date[0] + "," + date[1] + "," + date[2] + "," + date[3];
                        }
                        else
                        {
                            line = "deleted user," + arr[0] + "," + arr[1] + "," + arr[2] + ",PASSED," + date[0] + "," + date[1] + "," + date[2] + "," + date[3];

                        }
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
            sw.Close();
            sr.Close();
            Delete_row_file();
        }

        private void Delete_row_file()
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

        private void BuildHTML(int year, int month)
        {
            int nodays = DateTime.DaysInMonth(year, month);
            String reportFile = documents + year + "_" + month + ".html";
            if (users.Count <= 0)
                return;
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
                string header = "<table id='tbMonth' border='1'><tr><th>User Name</th>";
                for (int i = 1; i <= nodays; i++)
                {
                    header += "<th>" + i + "</th>";
                }

                header += "</tr>\n";
                sw.WriteLine(header);
                string table = "";
                sr.ReadLine().Split(','); //read first line of file
                var stats = GetUsersStats(year, month);
                for (int i = 1; i < stats.GetLength(0); i++)
                {
                    table += string.Format("<tr><td class='name'>{0}</td>", stats[i, 0]);
                    for (int j = 1; j < stats.GetLength(1); j++)
                    {
                        table += string.Format("<td class='day'>{0}</td>", stats[i, j]);
                    }
                    table += "</tr>\n";
                }
                sw.WriteLine(table);
                sw.WriteLine("</table></body></html>");
                sr.Close();
                sw.Close();
                System.Diagnostics.Process.Start(documents);
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

        private string[,] GetUsersStats(int year, int month)
        {
            int monthDays = DateTime.DaysInMonth(year, month);
            string[,] monthStats = new string[users.Count + 1, monthDays + 1];

            if (File.Exists(csvLog))
            {
                int users_count = 0;
                txbLog.Text += "Geting users log from csv" + Environment.NewLine;
                foreach (var user in users)
                {
                    users_count++;
                    StreamReader sr = new StreamReader(csvLog);
                    string[] tmp = sr.ReadLine().Split(',');

                    if (user.Value != "")
                    {
                        monthStats[users_count, 0] = user.Value;

                        for (int i = 0; i < monthDays; i++)
                        {
                            while (sr.Peek() > 0)
                            {
                                tmp = sr.ReadLine().Split(',');
                                if (Int16.Parse(tmp[7]) == month && Int16.Parse(tmp[8]) == year && user.Value.Equals(tmp[0]))
                                {
                                    monthStats[users_count, Int16.Parse(tmp[6])] = " V ";
                                }
                            }
                        }
                    }

                    sr.Close();
                }
                txbLog.Text += "End Geting users log from csv" + Environment.NewLine;
            }
            else
            {
                txbLog.Text += csvLog + " not found" + Environment.NewLine;
            }
            return monthStats;
        }

        private void lblAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Chaim Gorbov for Avdor-HELET \nProgram Version: " + CurrentVersion, "About C3-200 Logger", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string CurrentVersion
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                       ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                       : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }


        private void btnGetLog_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(documents))
            {
                Directory.CreateDirectory(documents);
                File.Copy(@"Resources\script.js", documents + "script.js");
                File.Copy(@"Resources\styles.css", documents + "styles.css");
            }

            if (Connect())
            {
                BuildHTML(datePeack.Value.Year, datePeack.Value.Month);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            settingsForm = new SettingsForm();
            settingsForm.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Delete_row_file();
        }
    }
}
