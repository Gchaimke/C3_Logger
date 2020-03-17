using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace C3_Logger
{
    public partial class MainForm : Form
    {
        String documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string strcount = "";
        IntPtr h = IntPtr.Zero;
        Dictionary<int, String> users = new Dictionary<int, String>();

        public MainForm()
        {
            InitializeComponent();
        }

        [DllImport(@"lib\plcommpro.dll", EntryPoint = "Connect")]
        public static extern IntPtr Connect(string Parameters);

        [DllImport(@"lib\plcommpro.dll", EntryPoint = "PullLastError")]
        public static extern int PullLastError();

        [DllImport(@"lib\plcommpro.dll", EntryPoint = "Disconnect")]
        public static extern void Disconnect(IntPtr h);

        [DllImport(@"lib\plcommpro.dll", EntryPoint = "GetDeviceData")]
        public static extern int GetDeviceData(IntPtr h, ref byte buffer, int buffersize, string tablename, string filename, string filter, string options);

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string str = "";
            str = "protocol=TCP,ipaddress=" + this.txbIP.Text + ",port=4370,timeout=3000,passwd=";  //protocol=TCP,ipaddress=192.168.2.46,port=4370,timeout=2000,passwd=
            if (IntPtr.Zero == h)
            {
                h = Connect(str);
                
                if (h != IntPtr.Zero)
                {
                    btnConnect.Enabled = false;
                    txbLog.Text += "Connected!"+ Environment.NewLine;
                    getLog();
                    Disconnect(h);
                    h = IntPtr.Zero;
                    txbLog.Text += "Disconnected!"+ Environment.NewLine;
                    btnConnect.Enabled = true;
                    System.Diagnostics.Process.Start(documents);
                }
                else
                {
                    txbLog.Text += "Connection error!"+ Environment.NewLine;
                }
                Cursor = Cursors.Default;
            }
        }

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
                    StreamWriter sw = new StreamWriter(documents + "\\ESD_LOG.ROW");
                    sw.WriteLine(strcount);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    txbLog.Text += ex.Message + Environment.NewLine;
                }
                saveLog();
                txbLog.Text +="Get " + ret + " records" + Environment.NewLine;
            }
            else
            {
                txbLog.Text += "Get data failed.The error is " + ret + Environment.NewLine;
                return;
            }
        }

        private String[] formatLog(String rowTime)
        {
            int second, minute, hour;
            String strSecond, strMinute, strHour;
            String[] date =new String[4]; //time,day,month,year
            int time = Int32.Parse(rowTime);
            second = time % 60;
            if (second < 10)
                strSecond = "0"+second;
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
            date[1] = (time % 31 + 1)+"";

            time = time / 31;
            date[2] = (time % 12 + 1)+"";

            time = time / 12;
            date[3] = (time + 2000)+"";
            return date;
        }


        private void saveLog()
        {
            try
            {
                getUserNames();
            }
            catch(Exception ex)
            {
                txbLog.Text += ex.Message + Environment.NewLine;
            }

            StreamReader sr = new StreamReader(documents + "\\ESD_LOG.ROW");
            try
            {
                
                StreamWriter sw = new StreamWriter(documents + "\\ESD_LOG.csv");
                sw.WriteLine("Name,"+sr.ReadLine()+",Day,Month,Year");
                while (sr.Peek() > 0)
                {
                    String[] arr = sr.ReadLine().Split(',');
                    String[] date = formatLog(arr[4]);
                    String line = "";
                    if (users.Count > 0)
                    {
                        line = users[Int16.Parse(arr[1])] + "," + arr[0] + "," + arr[1] + "," + arr[2] + ",PASSED," + date[0]+","+ date[1] + "," + date[2] + "," + date[3];
                    }
                    else
                    {
                        txbLog.Text += "Fale with names not found, printing user number" + Environment.NewLine;
                        line = arr[0] + "," + arr[1] + "," + arr[2] + ",PASSED," + date[0] + "," + date[1] + "," + date[2] + "," + date[3];
                    }
                    sw.WriteLine(line);
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                txbLog.Text += ex.Message + Environment.NewLine;
            }
            sr.Close();
            delRow();
        }

        private void delRow()
        {
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(documents + "\\ESD_LOG.ROW"))
                {
                    // If file found, delete it    
                    File.Delete(documents + "\\ESD_LOG.ROW");
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
        }

        private void buildHTML(int year, int month)
        {
            DateTimeFormatInfo dinfo = new DateTimeFormatInfo();
            int nodays = DateTime.DaysInMonth(year, month);

            StreamReader sr = new StreamReader(documents + "\\ESD_LOG.csv");
            StreamWriter sw = new StreamWriter(documents + "\\ESD_LOG.html");

            String page_start = "<!doctype html>\n <html lang = \"en\">\n<head><meta charset = \"utf-8\">\n" +
                "<title> ESD Table</title>\n" +
                "<meta name=\"description\"content=\"The HTML5 Herald\">\n" +
                "<meta name=\"author\"content=\"SitePoint\">\n" +
                "<link rel=\"stylesheet\" href=\"styles.css?v=1.0\"> </head><body>\n" +
                "<script src=\"script.js\"></script>";

            sw.WriteLine(page_start);
            String table = "<table id='tbMonth'>";
            String[] headLine = sr.ReadLine().Split(',');
            getUserNames();
            table += "<tr><th>ID<th> <th>User Name<th>";
            for(int i = 0; i <= nodays; i++)
            {
                table += "<th>"+ i + "</th>";
            }
               
            table += "</tr>";

            foreach (KeyValuePair<int, string> user in users)
            {
                table += string.Format("<tr><td>{0}</td><td>{1}</td></tr>", user.Key, user.Value);
            }

            sw.WriteLine(table);

            String page_end="</body></html>";
            sw.WriteLine(page_end);

            sr.Close();
            sw.Close();
            System.Diagnostics.Process.Start(documents);
        }

        private void getUserNames()
        {
            string mdfFile = @"C:\ZKTeco\ZKAccess3.5\access.mdb";

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

                        users.Add(Int16.Parse(userNum.ToString()),nameValue.ToString());
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //dbTest();
            buildHTML(2020,03);
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            pictureBox1.BackColor = Color.Gray;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            pictureBox1.BackColor = Color.Transparent;

        }
    }
}
