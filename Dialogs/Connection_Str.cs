using System;
using System.Management;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.ComponentModel;
using MySqlX.XDevAPI.Common;
using MySql.Data.MySqlClient;
using System.IO;

namespace ALBAITAR_Softvet.Dialogs
{

    public partial class Connection_Str : Form
    {
        bool Is_Running = false;
        decimal ddd = 0;
        private CancellationTokenSource cancellationTokenSource;
        public Connection_Str()
        {
            InitializeComponent();
            //---------------------------

            pictureBox1.Parent = dataGridView1;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Location = new Point(80, 50);

            label1.Parent = dataGridView1;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(170, 50);
            label1.BringToFront();

            progressBar1.Maximum = 510;

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Century Gothic", 8);

        }

        private void Connection_Str_Load(object sender, EventArgs e)
        {   
            string ipp = Properties.Settings.Default.Connection_String_IP_Or_LocalHost;
            switch (ipp)
            {
                case "localhost":
                    checkBox1.Checked = true;
                    checkBox2.Checked = false;
                    break;
                    default:                 
                    checkBox1.Checked = false;
                    checkBox2.Checked = true;
                    dataGridView2.Rows.Add(ipp, Properties.Settings.Default.Connection_String_PC_name);                    
                    break;
            }            
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            label7.Visible = true;
            if (Is_Running)
            {
                Is_Running = false;
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                }

                pictureBox1.Visible = label1.Visible = progressBar1.Visible = false;
                button1.BackgroundImage = Properties.Resources.icons8_repeat_30px;
                toolTip1.SetToolTip(button1, "Actualiser");
            }
            else
            {
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                }

                cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    await Task.Run(() => make_scan(cancellationTokenSource.Token));
                }
                catch (OperationCanceledException)
                {
                }
            }
            label7.Visible = false;
        }

        private void make_scan(CancellationToken cancellationToken)
        {
            Invoke((MethodInvoker)delegate
            {
                Is_Running = true;
                ddd = 0;
                progressBar1.Value = 0;
                dataGridView1.Rows.Clear();
                button1.BackgroundImage = Properties.Resources.icons8_cancel_25px_2;
                pictureBox1.Visible = label1.Visible = progressBar1.Visible = true;
                toolTip1.SetToolTip(button1, "Arrêter");
            });

            //---------------------------
            List<string> localpc_ips = new List<string>();
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress IPp in entry.AddressList)
            {
                if (IPp.AddressFamily == AddressFamily.InterNetwork)
                {
                    localpc_ips.Add(IPp.ToString());
                }
            }
            //======01 : Wifi =======================
            for (int i = 1; i <= 255; i++)
            {
                string ip = $"{"192.168.1"}.{i}";
                if (localpc_ips.Contains(ip))
                    continue;
                Ping ping = new Ping();
                PingReply reply = ping.Send(ip, 100);
                if (reply.Status == IPStatus.Success && Is_Running)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            IPHostEntry host = Dns.GetHostEntry(IPAddress.Parse(ip));
                            dataGridView1.Rows.Add(ip, host.HostName);
                        }
                        catch
                        {
                            
                        }
                        progressBar1.Value += (progressBar1.Value + 1) <= progressBar1.Maximum ? 1 : 0;
                        ddd++;
                        label1.Text = "[" + (ddd / 510 * 100).ToString("N2") + "%]";
                    });
                }
                else if (Is_Running)
                {
                    Invoke((MethodInvoker)delegate
                    {

                        progressBar1.Value += (progressBar1.Value + 1) <= progressBar1.Maximum ? 1 : 0;
                        ddd++;
                        label1.Text = "[" + (ddd / 510 * 100).ToString("N2") + "%]";
                    });
                }
            }
            //======01 : Ethernet =======================
            for (int i = 1; i <= 255; i++)
            {
                string ip = $"{"192.168.0"}.{i}";
                if (localpc_ips.Contains(ip))
                    continue;
                Ping ping = new Ping();
                PingReply reply = ping.Send(ip, 100);
                if (reply.Status == IPStatus.Success && Is_Running)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            IPHostEntry host = Dns.GetHostEntry(IPAddress.Parse(ip));
                            dataGridView1.Rows.Add(ip, host.HostName);
                        }
                        catch
                        {
                           
                        }
                        progressBar1.Value += (progressBar1.Value + 1) <= progressBar1.Maximum ? 1 : 0;
                        ddd++;
                        label1.Text = "[" + (ddd / 510 * 100).ToString("N2") + "%]";
                    });

                }
                else if (Is_Running)
                {
                    Invoke((MethodInvoker)delegate
                    {

                        progressBar1.Value += (progressBar1.Value + 1) <= progressBar1.Maximum ? 1 : 0;
                        ddd++;
                        label1.Text = "[" + (ddd / 510 * 100).ToString("N2") + "%]";
                    });

                }
            }
            //==============================================
            Invoke((MethodInvoker)delegate
            {
                Is_Running = false;
                pictureBox1.Visible = label1.Visible = progressBar1.Visible = false;
                button1.BackgroundImage = Properties.Resources.icons8_repeat_30px;
                toolTip1.SetToolTip(button1, "Actualiser");
            });



        }
        private void Connection_Str_FormClosed(object sender, FormClosedEventArgs e)
        {
            Is_Running = false;
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Enabled = radioButton2.Checked;
            button1.Visible = radioButton2.Checked;
            button2.Visible = radioButton1.Checked || (dataGridView1.SelectedRows.Count > 0);

            if (radioButton2.Checked && !Is_Running)
            {
                button1_Click(null, null);
            }else if(radioButton1.Checked && Is_Running)
            {
                Is_Running = false;
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {            
            button2.Visible = radioButton1.Checked || (dataGridView1.SelectedRows.Count > 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost;Port=3306;Database=albaitar_db;Uid=albaitar_user;Pwd=AlBaiTar9999;");

            if (radioButton2.Checked)
            {
                if(dataGridView1.SelectedRows.Count > 0)
                {
                    mySqlConnection = new MySqlConnection("Server=" + dataGridView1.SelectedRows[0].Cells["IP_ADRESS"].Value + ";Port=3306;Database=albaitar_db;Uid=albaitar_user;Pwd=AlBaiTar9999;");
                }
                else
                {
                    mySqlConnection = null;
                }
                
            }

            if(mySqlConnection != null)
            {
                label6.Visible = true;
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

                var connectTask = ConnectToDatabase(cancellationTokenSource.Token, mySqlConnection);

                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(2));

                Task.WhenAny(connectTask, timeoutTask).GetAwaiter().GetResult();

                label6.Visible = false;
                if (mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    MessageBox.Show("Bien Connecté !", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Non Connecté\nIl y'a un probléme.", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }


                if (mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    mySqlConnection.Close();
                }
            }
            else
            {
                MessageBox.Show("Aucun selection !");
            }
            
        }
        private static async Task<bool> ConnectToDatabase(CancellationToken cancellationToken, MySqlConnection conn_str)
        {

            var completionSource = new TaskCompletionSource<MySqlConnection>();

            cancellationToken.Register(() =>
            {
                completionSource.TrySetCanceled();
            });

            try
            {
                await conn_str.OpenAsync(cancellationToken);
                completionSource.TrySetResult(conn_str);
                return conn_str.State == System.Data.ConnectionState.Open;
            }
            catch (Exception ex)
            {
                completionSource.TrySetException(ex);
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Is_Running = false;
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
            //--------------------------------------------
            if (radioButton1.Checked) //Localhost
            {
                Properties.Settings.Default.Connection_String_IP_Or_LocalHost = "localhost";
                Properties.Settings.Default.Save();
                Process myProcess = Process.Start("ALBAITAR_Softvet.exe");
                Process.GetCurrentProcess().Kill();
            }
            else //Other IP/Pc
            {
                if(dataGridView1.SelectedRows.Count > 0) {
                    Properties.Settings.Default.Connection_String_IP_Or_LocalHost = (string)dataGridView1.SelectedRows[0].Cells["IP_ADRESS"].Value;
                    Properties.Settings.Default.Connection_String_PC_name = (string)dataGridView1.SelectedRows[0].Cells["PC"].Value;
                    Properties.Settings.Default.Save();
                    Process myProcess = Process.Start("ALBAITAR_Softvet.exe");
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    MessageBox.Show("Aucun selection !");
                }
                
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MySqlConnection mySqlConnection = new MySqlConnection("Server="+Properties.Settings.Default.Connection_String_IP_Or_LocalHost+";Port=3306;Database=albaitar_db;Uid=albaitar_user;Pwd=AlBaiTar9999;");
                        
                label8.Visible = true;
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

                var connectTask = ConnectToDatabase(cancellationTokenSource.Token, mySqlConnection);

                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(2));

                Task.WhenAny(connectTask, timeoutTask).GetAwaiter().GetResult();

                label8.Visible = false;
                if (mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    MessageBox.Show("Bien Connecté !", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Non Connecté\nIl y'a un probléme.", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }


                if (mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    mySqlConnection.Close();
                }
            
        }
    }



}
