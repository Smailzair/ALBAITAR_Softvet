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
        Thread th;
        decimal ddd = 0;
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
            // make_scan();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (th != null)
            {
                if (th.ThreadState != System.Threading.ThreadState.Aborted)
                {
                    th.Abort();
                    pictureBox1.Visible = label1.Visible = progressBar1.Visible = false;
                    button1.BackgroundImage = Properties.Resources.icons8_repeat_30px;
                    toolTip1.SetToolTip(button1, "Actualiser");
                }
                else
                {
                    ddd = 0;
                    progressBar1.Value = 0;
                    dataGridView1.Rows.Clear();
                    button1.BackgroundImage = Properties.Resources.icons8_cancel_25px_2;
                    th = new Thread(make_scan);
                    th.Start();
                    pictureBox1.Visible = label1.Visible = progressBar1.Visible = true;
                    toolTip1.SetToolTip(button1, "Arrêter");

                }
            }
            else
            {
                ddd = 0;
                progressBar1.Value = 0;
                dataGridView1.Rows.Clear();
                button1.BackgroundImage = Properties.Resources.icons8_cancel_25px_2;
                th = new Thread(make_scan);
                th.Start();
                pictureBox1.Visible = label1.Visible = progressBar1.Visible = true;
                toolTip1.SetToolTip(button1, "Arrêter");
            }
        }

        private void make_scan()
        {
            Task.Factory.StartNew(new Action(() =>
            {
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
                    if (reply.Status == IPStatus.Success)
                    {
                        progressBar1.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                IPHostEntry host = Dns.GetHostEntry(IPAddress.Parse(ip));
                                dataGridView1.Rows.Add(ip, host.HostName);
                            }
                            catch
                            {
                                //MessageBox.Show($"Couldn't retrieve hostname from {ip}", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            progressBar1.Value += (progressBar1.Value + 1) <= progressBar1.Maximum ? 1 : 0;
                            ddd++;
                        }));
                        label1.BeginInvoke(new Action(() =>
                        {
                            label1.Text = "[" + (ddd / 510 * 100).ToString("N2") + "%]";
                        }));
                    }
                    else
                    {

                        progressBar1.BeginInvoke(new Action(() =>
                        {
                            progressBar1.Value += (progressBar1.Value + 1) <= progressBar1.Maximum ? 1 : 0;
                            ddd++;
                        }));


                        label1.BeginInvoke(new Action(() =>
                        {
                            label1.Text = "[" + (ddd / 510 * 100).ToString("N2") + "%]";
                        }));



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
                    if (reply.Status == IPStatus.Success)
                    {
                        progressBar1.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                IPHostEntry host = Dns.GetHostEntry(IPAddress.Parse(ip));
                                dataGridView1.Rows.Add(ip, host.HostName);
                            }
                            catch
                            {
                                //MessageBox.Show($"Couldn't retrieve hostname from {ip}", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            progressBar1.Value += (progressBar1.Value + 1) <= progressBar1.Maximum ? 1 : 0;
                            ddd++;
                        }));
                        label1.BeginInvoke(new Action(() =>
                        {
                            label1.Text = "[" + (ddd / 510 * 100).ToString("N2") + "%]";
                        }));

                    }
                    else
                    {
                        progressBar1.BeginInvoke(new Action(() =>
                        {
                            progressBar1.Value += (progressBar1.Value + 1) <= progressBar1.Maximum ? 1 : 0;
                            ddd++;
                        }));
                        label1.BeginInvoke(new Action(() =>
                        {
                            label1.Text = "[" + (ddd / 510 * 100).ToString("N2") + "%]";
                        }));

                    }
                }
                //==============================================
                button1_Click(null, null);
            }));
        }
        private void Connection_Str_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (th != null)
            {
                if (th.ThreadState != System.Threading.ThreadState.Aborted)
                {
                    th.Abort();
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Enabled = radioButton2.Checked;
            button1.Visible = radioButton2.Checked;
            button2.Visible = radioButton1.Checked || (dataGridView1.SelectedRows.Count > 0);

            if (th != null)
            {
                if ((radioButton1.Checked && th.ThreadState != System.Threading.ThreadState.Aborted) || (radioButton2.Checked && th.ThreadState != System.Threading.ThreadState.Running))
                {
                    button1_Click(null, null);
                }
            }
            else if (radioButton2.Checked)
            {
                button1_Click(null, null);
            }



        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            button3.Visible = radioButton2.Checked && dataGridView1.SelectedRows.Count > 0;
            button2.Visible = radioButton1.Checked || (dataGridView1.SelectedRows.Count > 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && radioButton2.Checked)
            {
                label6.Visible = true;
                MySqlConnection mySqlConnection = new MySqlConnection("Server=" + dataGridView1.SelectedRows[0].Cells["IP_ADRESS"].Value + ";Port=3306;Database=albaitar_db;Uid=albaitar_user;Pwd=AlBaiTar9999;");


                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

                var connectTask = ConnectToDatabase(cancellationTokenSource.Token, mySqlConnection);

                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(2));

                Task.WhenAny(connectTask, timeoutTask).GetAwaiter().GetResult();

                label6.Visible = false;
                if (mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    CustomMessageBox messageBox = new CustomMessageBox("Connection aprovée !", "Bien connecté :", Properties.Resources.icons8_protect_30px);
                    messageBox.ShowDialog();
                }
                else
                {
                    CustomMessageBox messageBox = new CustomMessageBox("Connection non aprovée\nIl y a un probléme.", "Non connecté :", Properties.Resources.icons8_access_denied_30px);
                    messageBox.ShowDialog();
                }


                if (mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    mySqlConnection.Close();
                }

                
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
    }



}
