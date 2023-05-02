using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Resources;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Main_Frm : Form
    {
        Thread th;
        public static DataTable ADRESSES_SITES;
        bool sites_table_ready = false;
        public static DataTable Autorisations;
        public static DataTable Params;
        public Main_Frm()
        {
            InitializeComponent();
            //------------------------
           
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                Autorisations = PreConnection.Load_data("SELECT `ID`,`CODE`,`AUTOR_TEXT`,Usr_" + Properties.Settings.Default.Last_login_user_idx + " FROM tb_autoriz;");
            }
            //----------------------------
            Params = PreConnection.Load_data("SELECT * FROM tb_params;");
            //------------------------------
            th = new Thread(new ThreadStart(Load_sites_table)); //I use it because of starting perfermance of "Clients" from
            th.Start();
            //--------------

        }


        public void Load_sites_table()
        {
            ADRESSES_SITES = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_adresses;");
            sites_table_ready = true;
        }
        private void button9_Click(object sender, EventArgs e)
        {

            Cursor = Cursors.WaitCursor;
            if (!sites_table_ready)
            {
                th.Join();
            }
            if (Application.OpenForms["Clients"] == null)
            {
                new Clients().Show();
            }
            else
            {
                Application.OpenForms["Clients"].WindowState = Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Clients"].WindowState;
                Application.OpenForms["Clients"].BringToFront();
            }
            Cursor = Cursors.Default;
            panel1.Visible = false;
            


        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new Settings()).ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms["Animaux"] == null)
            {
                new Animaux().Show();
            }
            else
            {
                Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                Application.OpenForms["Animaux"].BringToFront();
            }
            panel1.Visible = false;
            

        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms["Produits"] == null)
            {
                new Produits().Show();
            }
            else
            {
                Application.OpenForms["Produits"].WindowState = Application.OpenForms["Produits"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Produits"].WindowState;
                Application.OpenForms["Produits"].BringToFront();
            }
            panel1.Visible = false;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Agenda"] == null)
            {
                new Agenda().Show();
            }
            else
            {
                Application.OpenForms["Agenda"].WindowState = Application.OpenForms["Agenda"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Agenda"].WindowState;
                Application.OpenForms["Agenda"].BringToFront();
            }
            panel1.Visible = false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Login_Auto_Enter = false;
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        private void Main_Frm_Load(object sender, EventArgs e)
        {
            WindowState = Properties.Settings.Default.Maximize_Main_Frm ? FormWindowState.Maximized : FormWindowState.Normal;
            Text = "ALBAITAR Softvet - " + Properties.Settings.Default.Last_login_user_full_nme;
            string cab_doct = Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();                        
            if(cab_doct == null || cab_doct.Trim() == string.Empty)
            {
                if(Properties.Settings.Default.Last_login_is_admin || Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92004" && (Int32)QQ[3] == 1).Count() > 0)
                {
                    string dd = "";
                    int p = 3;
                    while (dd == string.Empty && p > 0)
                    {
                        PreConnection.InputBox("Veuillez saisir votre identification : ", "Exp : Dr.xxx , Cabinet xxx, ...", ref dd);
                        p--;
                    }
                    if (dd != string.Empty)
                    {
                        PreConnection.Excut_Cmd("UPDATE tb_params SET `VAL` = '" + dd + "' WHERE `ID` = 1;");
                        Params = PreConnection.Load_data("SELECT * FROM tb_params;");
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                label_cab_nme.Text = cab_doct;
            }
            
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button9.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10000" && (Int32)QQ[3] == 1).Count() > 0;
                button11.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20000" && (Int32)QQ[3] == 1).Count() > 0;
                button12.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30000" && (Int32)QQ[3] == 1).Count() > 0;
                button4.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31000" && (Int32)QQ[3] == 1).Count() > 0;
                button5.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40000" && (Int32)QQ[3] == 1).Count() > 0;
                button7.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50000" && (Int32)QQ[3] == 1).Count() > 0;
            }

        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            Rectangle panelBounds = panel1.RectangleToScreen(panel1.ClientRectangle);
            if (!panelBounds.Contains(MousePosition))
            {
                panel1.Visible = false;
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            Rectangle panelBounds = panel1.RectangleToScreen(panel1.ClientRectangle);
            if (!panelBounds.Contains(MousePosition))
            {
                panel1.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Laboratoire"] == null)
            {
                new Laboratoire().Show();
            }
            else
            {
                Application.OpenForms["Laboratoire"].WindowState = Application.OpenForms["Laboratoire"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Laboratoire"].WindowState;
                Application.OpenForms["Laboratoire"].BringToFront();
            }
            panel1.Visible = false;
            
        }

        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            if(panel1.Visible)
            {
                button9.Focus();
            }
            else
            {
                button3.Select();
            }
        }

        private void Main_Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Maximize_Main_Frm = WindowState == FormWindowState.Maximized;
            Properties.Settings.Default.Save();
        }
    }
}

