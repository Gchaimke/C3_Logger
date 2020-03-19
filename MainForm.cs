namespace C3_Logger
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="MainForm" />
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Defines the settingsForm
        /// </summary>
        private SettingsForm settingsForm;

        /// <summary>
        /// Defines the documents
        /// </summary>
        internal String documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ESD_LOGS\\";
        internal String rowLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ESD_LOGS\\ESD_LOG.ROW";
        internal String csvLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ESD_LOGS\\ESD_LOG.csv";

        /// <summary>
        /// Defines the strcount
        /// </summary>
        internal string strcount = "";

        /// <summary>
        /// Defines the h
        /// </summary>
        internal IntPtr h = IntPtr.Zero;

        /// <summary>
        /// Defines the users
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
            if(!File.Exists(documents + "script.js"))
                File.Copy(@"Resources\script.js", documents + "script.js");
            if (!File.Exists(documents + "styles.css"))
                File.Copy(@"Resources\styles.css", documents + "styles.css");
        }

        /// <summary>
        /// The Connect
        /// </summary>
        /// <param name="Parameters">The Parameters<see cref="string"/></param>
        /// <returns>The <see cref="IntPtr"/></returns>
        [DllImport(@"lib\plcommpro.dll", EntryPoint = "Connect")]
        public static extern IntPtr Connect(string Parameters);

        /// <summary>
        /// The PullLastError
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        [DllImport(@"lib\plcommpro.dll", EntryPoint = "PullLastError")]
        public static extern int PullLastError();

        /// <summary>
        /// The Disconnect
        /// </summary>
        /// <param name="h">The h<see cref="IntPtr"/></param>
        [DllImport(@"lib\plcommpro.dll", EntryPoint = "Disconnect")]
        public static extern void Disconnect(IntPtr h);

        /// <summary>
        /// The GetDeviceData
        /// </summary>
        /// <param name="h">The h<see cref="IntPtr"/></param>
        /// <param name="buffer">The buffer<see cref="byte"/></param>
        /// <param name="buffersize">The buffersize<see cref="int"/></param>
        /// <param name="tablename">The tablename<see cref="string"/></param>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <param name="filter">The filter<see cref="string"/></param>
        /// <param name="options">The options<see cref="string"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllImport(@"lib\plcommpro.dll", EntryPoint = "GetDeviceData")]
        public static extern int GetDeviceData(IntPtr h, ref byte buffer, int buffersize, string tablename, string filename, string filter, string options);

        /// <summary>
        /// The btnConnect_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnConnect_Click(object sender, EventArgs e)
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
                    btnConnect.Enabled = false;
                    txbLog.Text += "Connected!" + Environment.NewLine;
                    getLog();
                    Disconnect(h);
                    h = IntPtr.Zero;
                    txbLog.Text += "Disconnected!" + Environment.NewLine;
                    btnConnect.Enabled = true;
                    System.Diagnostics.Process.Start(documents);
                }
                else
                {
                    txbLog.Text += "Connection error! Cant conncet to address " + Properties.Settings.Default.IP + ":" + Properties.Settings.Default.Port + Environment.NewLine;
                }
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// The getLog
        /// </summary>
        private void getLog()
        {
            int ret = 0;
            string str = "Cardno\tPin\tEventType\tInOutState\tTime_second";
            int BUFFERSIZE = 1 * 1024 * 1024;
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
                    Console.WriteLine("Exception: " + ex.Message);
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
        /// The formatLog
        /// </summary>
        /// <param name="rowTime">The rowTime<see cref="String"/></param>
        /// <returns>The <see cref="String[]"/></returns>
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
        /// The saveLog
        /// </summary>
        private void saveLog()
        {
            try
            {
                getUserNames();
            }
            catch (Exception ex)
            {
                txbLog.Text += ex.Message + Environment.NewLine;
            }


            StreamReader sr = new StreamReader(rowLog);
            try
            {

                StreamWriter sw = new StreamWriter(csvLog);
                sw.WriteLine("Name," + sr.ReadLine() + ",Day,Month,Year");
                while (sr.Peek() > 0)
                {
                    String[] arr = sr.ReadLine().Split(',');
                    String[] date = formatLog(arr[4]);
                    String line = "";
                    if (users.Count > 0)
                    {
                        line = users[Int16.Parse(arr[1])] + "," + arr[0] + "," + arr[1] + "," + arr[2] + ",PASSED," + date[0] + "," + date[1] + "," + date[2] + "," + date[3];
                    }
                    else
                    {
                        txbLog.Text += "Fale with names not found, printing user number" + Environment.NewLine;
                        line = arr[0] + "," + arr[1] + "," + arr[2] + ",PASSED," + date[0] + "," + date[1] + "," + date[2] + "," + date[3];
                    }
                    sw.WriteLine(line);
                }
                sw.Close();
                txbLog.Text += "Log created: " + csvLog + Environment.NewLine;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                txbLog.Text += ex.Message + Environment.NewLine;
            }
            sr.Close();
            delRow();
        }

        /// <summary>
        /// The delRow
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
        /// The buildHTML
        /// </summary>
        /// <param name="year">The year<see cref="int"/></param>
        /// <param name="month">The month<see cref="int"/></param>
        private void buildHTML(int year, int month)
        {
            int nodays = DateTime.DaysInMonth(year, month);
            getUserNames();
            try
            {
                StreamReader sr = new StreamReader(Properties.Settings.Default.LogPath);
                StreamWriter sw = new StreamWriter(documents + year + "_" + month + ".html");

                String page_start = "<!doctype html>\n <html lang = \"en\">\n<head><meta charset = \"utf-8\">\n" +
                    "<title> ESD Table</title>\n" +
                    "<meta name=\"description\"content=\"The HTML5 Herald\">\n" +
                    "<meta name=\"author\"content=\"SitePoint\">\n" +
                    "<link rel=\"stylesheet\" href=\"styles.css?v=1.0\"> </head><body>\n" +
                    "<script src=\"script.js\"></script>\n"+
                    "<h2>ESD tests for month: "+datePeack.Value.Month+"/"+datePeack.Value.Year+"</h2>";

                sw.WriteLine(page_start);
                String table = "<table id='tbMonth' border='1'>";
                String[] headLine = sr.ReadLine().Split(',');
                table += "<tr><th>ID</th> <th>User Name</th>";
                for (int i = 1; i <= nodays; i++)
                {
                    table += "<th>" + i + "</th>";
                }

                table += "</tr>";

                var test = getUsersStats(year, month);
                for (int i = 1; i < test.GetLength(0); i++)
                {
                    table += string.Format("<tr><td>{0}</td>", i);
                    for (int j = 0; j < test.GetLength(1); j++)
                    {
                        table += string.Format("<td>{0}</td>", test[i, j]);
                    }
                    table += "</tr>";
                }

                sw.WriteLine(table);

                String page_end = "</body></html>";
                sw.WriteLine(page_end);
                sr.Close();
                sw.Close();
                System.Diagnostics.Process.Start(documents);
                txbLog.Text += "Report created: " + documents + year + "_" + month + ".html" + Environment.NewLine;
            }
            catch (FileNotFoundException ex)
            {
                txbLog.Text += ex.Message + Environment.NewLine;
            }
            catch (Exception ex)
            {
                txbLog.Text += ex.Message + Environment.NewLine;
            }
        }

        /// <summary>
        /// The getUserNames
        /// </summary>
        private void getUserNames()
        {
            string mdfFile = Properties.Settings.Default.dbPath;
            if (!File.Exists(mdfFile))
            {
                mdfFile = @"C:\ZKTeco\ZKAccess3.5\access.mdb";
            }
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

        private String[,] getUsersStats(int year, int month)
        {
            int monthDays = DateTime.DaysInMonth(year, month);
            String[,] monthStats = new string[users.Count+1, monthDays+1];

            

            if (File.Exists(csvLog))
            {
                foreach (var user in users)
                {
                    StreamReader sr = new StreamReader(csvLog);
                    String[] tmp = sr.ReadLine().Split(',');
                    monthStats[user.Key, 0] = user.Value;
                    for(int i = 0; i < monthDays; i++)
                    {
                        while(sr.Peek() > 0)
                        {
                            tmp = sr.ReadLine().Split(',');
                            if(Int16.Parse(tmp[7]) == month && Int16.Parse(tmp[8]) == year && user.Value.Equals(tmp[0]))
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
                txbLog.Text += csvLog+" not found" + Environment.NewLine;
            }
            return monthStats;
        }

        /// <summary>
        /// The pbSettings_MouseHover
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pbSettings_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            pbSettings.BackColor = Color.Gray;
        }

        /// <summary>
        /// The pbSettings_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pbSettings_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            pbSettings.BackColor = Color.Transparent;
        }

        /// <summary>
        /// The pbSettings_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pbSettings_Click(object sender, EventArgs e)
        {
            settingsForm = new SettingsForm();
            settingsForm.Show();
        }

        /// <summary>
        /// The pbCalendar_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pbCalendar_Click(object sender, EventArgs e)
        {

            buildHTML(datePeack.Value.Year, datePeack.Value.Month);
        }

        /// <summary>
        /// The pbCalendar_MouseHover
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pbCalendar_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            pbCalendar.BackColor = Color.Gray;
        }

        /// <summary>
        /// The pbCalendar_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pbCalendar_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            pbCalendar.BackColor = Color.Transparent;
        }
    }
}
