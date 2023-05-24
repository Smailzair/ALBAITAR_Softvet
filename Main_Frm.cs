using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Resources;
using Microsoft.Office.Interop.Word;
using Npgsql.Logging;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace ALBAITAR_Softvet
{
    public partial class Main_Frm : Form
    {
        DateTime last_update_time = new DateTime(1900, 12, 31);
        Thread th;
        public static DataTable ADRESSES_SITES;
        bool sites_table_ready = false;
        public static DataTable Autorisations;
        public static DataTable Params;
        int selected_client_id = -1;
        int selected_animal_id = -1;
        DataTable clients;
        DataTable animals;
        ImageList tabcontrol_img_lst;
        //----------
        DataTable chosen_anim_from_search;
        DataTable chosen_client_from_search;
        //-------------
        static DataTable main_visites_tab;
        bool loading_visites_tab = false;
        static bool ended_loading_visites_tab = false;
        //-------------------
        
        public Main_Frm()
        {
            InitializeComponent();
            //------------------------
            tabcontrol_img_lst = new ImageList();
            tabcontrol_img_lst.Images.AddRange(new Image[]
            {
                Properties.Resources.agenda_001,//Visite analyse
                Properties.Resources.agenda_003,//Labo
                Properties.Resources.icons8_info_30px,//Infos
                Properties.Resources.icons8_tear_off_calendar_30px,//calendar
            });
            tabControl1.ImageList = tabcontrol_img_lst;
            tabPage_infos_animal.ImageIndex = 2;
            tabPage_visites_animal.ImageIndex = 0;
            //-------------------------
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                Autorisations = PreConnection.Load_data("SELECT `ID`,`CODE`,`AUTOR_TEXT`,Usr_" + Properties.Settings.Default.Last_login_user_idx + " FROM tb_autoriz;");
            }
            //----------------------------
            Params = PreConnection.Load_data("SELECT * FROM tb_params;");
            //------------------------------
            th = new Thread(new ThreadStart(Load_sites_table)); //I use it because of starting perfermance of "Clients" from
            th.Start();
            th.Join();
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
            if (System.Windows.Forms.Application.OpenForms["Clients"] == null)
            {
                new Clients().Show();
            }
            else
            {
                System.Windows.Forms.Application.OpenForms["Clients"].WindowState = System.Windows.Forms.Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : System.Windows.Forms.Application.OpenForms["Clients"].WindowState;
                System.Windows.Forms.Application.OpenForms["Clients"].BringToFront();
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

            if (System.Windows.Forms.Application.OpenForms["Animaux"] == null)
            {
                new Animaux().Show();
            }
            else
            {
                System.Windows.Forms.Application.OpenForms["Animaux"].WindowState = System.Windows.Forms.Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : System.Windows.Forms.Application.OpenForms["Animaux"].WindowState;
                System.Windows.Forms.Application.OpenForms["Animaux"].BringToFront();
            }
            panel1.Visible = false;


        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (System.Windows.Forms.Application.OpenForms["Produits"] == null)
            {
                new Produits().Show();
            }
            else
            {
                System.Windows.Forms.Application.OpenForms["Produits"].WindowState = System.Windows.Forms.Application.OpenForms["Produits"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : System.Windows.Forms.Application.OpenForms["Produits"].WindowState;
                System.Windows.Forms.Application.OpenForms["Produits"].BringToFront();
            }
            panel1.Visible = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["Agenda"] == null)
            {
                new Agenda().Show();
            }
            else
            {
                System.Windows.Forms.Application.OpenForms["Agenda"].WindowState = System.Windows.Forms.Application.OpenForms["Agenda"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : System.Windows.Forms.Application.OpenForms["Agenda"].WindowState;
                System.Windows.Forms.Application.OpenForms["Agenda"].BringToFront();
            }
            panel1.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Login_Auto_Enter = false;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Restart();
        }

        private void Main_Frm_Load(object sender, EventArgs e)
        {
            WindowState = Properties.Settings.Default.Maximize_Main_Frm ? FormWindowState.Maximized : FormWindowState.Normal;
            Text = "ALBAITAR Softvet - " + Properties.Settings.Default.Last_login_user_full_nme;
            string cab_doct = Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();
            if (cab_doct == null || cab_doct.Trim() == string.Empty)
            {
                if (Properties.Settings.Default.Last_login_is_admin || Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92004" && (Int32)QQ[3] == 1).Count() > 0)
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
                        PreConnection.Excut_Cmd("UPDATE tb_params SET `VAL` = '" + dd.Replace("'", "''") + "' WHERE `ID` = 1;");
                        Params = PreConnection.Load_data("SELECT * FROM tb_params;");
                        label_cab_nme.Text = dd;
                    }
                    else
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                }
                else
                {
                    System.Windows.Forms.Application.Exit();
                }
            }
            else
            {
                label_cab_nme.Text = cab_doct;
            }
            ///--------------------
            foreach (Control ctrr in this.Controls)
            {
                if (ctrr.Name != "button8" && ctrr.Name != "listView1")
                {
                    EventHandlerList events = (EventHandlerList)typeof(Control)
                                     .GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance)
                                      .GetValue(ctrr);

                    object mouseClickEventKey = typeof(Control)
                        .GetField("EventMouseClick", BindingFlags.NonPublic | BindingFlags.Static)
                        .GetValue(ctrr);

                    Delegate mouseClickDelegate = events[mouseClickEventKey] as Delegate;
                    if (mouseClickDelegate == null)
                    {
                        ctrr.MouseClick += tmp_MouseClick;
                    }
                }


            }
            ///---------------------
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button9.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10000" && (Int32)QQ[3] == 1).Count() > 0;
                button11.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20000" && (Int32)QQ[3] == 1).Count() > 0;
                button12.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30000" && (Int32)QQ[3] == 1).Count() > 0;
                button4.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31000" && (Int32)QQ[3] == 1).Count() > 0;
                button5.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40000" && (Int32)QQ[3] == 1).Count() > 0;
            }
            //--------------------
            clients = PreConnection.Load_data("SELECT *,CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS FULL_NME FROM tb_clients;");
            animals = PreConnection.Load_data("SELECT * FROM tb_animaux;");
            comboBox1.SelectedIndex = 0;


        }

        private void refresh_main_tables()
        {
            last_update_time = DateTime.Now;
            //------------
            int cb1_idx = comboBox1.SelectedIndex > -1 ? comboBox1.SelectedIndex : 0;
            int cb2_idx = comboBox2.SelectedValue != null ? (comboBox2.SelectedValue != DBNull.Value ? (int)comboBox2.SelectedValue : 0) : 0;
            clients = PreConnection.Load_data("SELECT *,CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS FULL_NME FROM tb_clients;");
            animals = PreConnection.Load_data("SELECT * FROM tb_animaux;");
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
            comboBox1.SelectedIndex = cb1_idx;
            try { comboBox2.SelectedValue = cb2_idx; }catch (Exception ex) { comboBox2.SelectedIndex = 0; }
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            comboBox1_SelectedIndexChanged(null,null);
            Refresh_current_tab();
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            System.Drawing.Rectangle panelBounds = panel1.RectangleToScreen(panel1.ClientRectangle);
            if (!panelBounds.Contains(MousePosition))
            {
                panel1.Visible = false;
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            System.Drawing.Rectangle panelBounds = panel1.RectangleToScreen(panel1.ClientRectangle);
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
            if (System.Windows.Forms.Application.OpenForms["Laboratoire"] == null)
            {
                new Laboratoire().Show();
            }
            else
            {
                System.Windows.Forms.Application.OpenForms["Laboratoire"].WindowState = System.Windows.Forms.Application.OpenForms["Laboratoire"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : System.Windows.Forms.Application.OpenForms["Laboratoire"].WindowState;
                System.Windows.Forms.Application.OpenForms["Laboratoire"].BringToFront();
            }
            panel1.Visible = false;
        }

        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            if (panel1.Visible)
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["Vente"] == null)
            {
                new Vente().Show();
            }
            else
            {
                System.Windows.Forms.Application.OpenForms["Vente"].WindowState = System.Windows.Forms.Application.OpenForms["Vente"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : System.Windows.Forms.Application.OpenForms["Vente"].WindowState;
                System.Windows.Forms.Application.OpenForms["Vente"].BringToFront();
            }
            panel1.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (!listView1.Visible)
            {
                listView1.Visible = true;
                button8.Location = new System.Drawing.Point(button8.Location.X - listView1.Width, button8.Location.Y);
                button1.Location = new System.Drawing.Point(button1.Location.X - listView1.Width, button1.Location.Y);
                button2.Location = new System.Drawing.Point(button2.Location.X - listView1.Width, button2.Location.Y);
            }
            else
            {
                listView1.Visible = false;
                button8.Location = new System.Drawing.Point(button8.Location.X + listView1.Width, button8.Location.Y);
                button1.Location = new System.Drawing.Point(button1.Location.X + listView1.Width, button1.Location.Y);
                button2.Location = new System.Drawing.Point(button2.Location.X + listView1.Width, button2.Location.Y);
            }

        }


        private void Main_Frm_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.Visible)
            {
                listView1.Visible = false;
                button8.Location = new System.Drawing.Point(button8.Location.X + listView1.Width, button8.Location.Y);
                button1.Location = new System.Drawing.Point(button1.Location.X + listView1.Width, button1.Location.Y);
                button2.Location = new System.Drawing.Point(button2.Location.X + listView1.Width, button2.Location.Y);
            }
        }

        private void tmp_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.Visible && !listView1.Bounds.Contains(e.Location))
            {
                listView1.Visible = false;
                button8.Location = new System.Drawing.Point(button8.Location.X + listView1.Width, button8.Location.Y);
                button1.Location = new System.Drawing.Point(button1.Location.X + listView1.Width, button1.Location.Y);
                button2.Location = new System.Drawing.Point(button2.Location.X + listView1.Width, button2.Location.Y);
            }
        }

        private void Main_Frm_Deactivate(object sender, EventArgs e)
        {
            if (listView1.Visible)
            {
                listView1.Visible = false;
                button8.Location = new System.Drawing.Point(button8.Location.X + listView1.Width, button8.Location.Y);
                button1.Location = new System.Drawing.Point(button1.Location.X + listView1.Width, button1.Location.Y);
                button2.Location = new System.Drawing.Point(button2.Location.X + listView1.Width, button2.Location.Y);
            }
            
        }


        static void visites_tab(object anim_id)
        {
            main_visites_tab = PreConnection.Load_data("SELECT tb1.*,tb2.REF AS 'FACTURE_REF' FROM tb_visites tb1 LEFT JOIN ("
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_01` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NOT NULL AND `ITEM_NME_01` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_02` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NOT NULL AND `ITEM_NME_02` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_03` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NOT NULL AND `ITEM_NME_03` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_04` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NOT NULL AND `ITEM_NME_04` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_05` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NOT NULL AND `ITEM_NME_05` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_06` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NOT NULL AND `ITEM_NME_06` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_07` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NOT NULL AND `ITEM_NME_07` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_08` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NOT NULL AND `ITEM_NME_08` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_09` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NOT NULL AND `ITEM_NME_09` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_10` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NOT NULL AND `ITEM_NME_10` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_11` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NOT NULL AND `ITEM_NME_11` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_12` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NOT NULL AND `ITEM_NME_12` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_13` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NOT NULL AND `ITEM_NME_13` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_14` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NOT NULL AND `ITEM_NME_14` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_15` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NOT NULL AND `ITEM_NME_15` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_16` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NOT NULL AND `ITEM_NME_16` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_17` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NOT NULL AND `ITEM_NME_17` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_18` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NOT NULL AND `ITEM_NME_18` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_19` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NOT NULL AND `ITEM_NME_19` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_20` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NOT NULL AND `ITEM_NME_20` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_21` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NOT NULL AND `ITEM_NME_21` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_22` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NOT NULL AND `ITEM_NME_22` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_23` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NOT NULL AND `ITEM_NME_23` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_24` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NOT NULL AND `ITEM_NME_24` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_25` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NOT NULL AND `ITEM_NME_25` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_26` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NOT NULL AND `ITEM_NME_26` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_27` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NOT NULL AND `ITEM_NME_27` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_28` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NOT NULL AND `ITEM_NME_28` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_29` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NOT NULL AND `ITEM_NME_29` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_30` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NOT NULL AND `ITEM_NME_30` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_31` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NOT NULL AND `ITEM_NME_31` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_32` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NOT NULL AND `ITEM_NME_32` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_33` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NOT NULL AND `ITEM_NME_33` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_34` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NOT NULL AND `ITEM_NME_34` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_35` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NOT NULL AND `ITEM_NME_35` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_36` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NOT NULL AND `ITEM_NME_36` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_37` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NOT NULL AND `ITEM_NME_37` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_38` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NOT NULL AND `ITEM_NME_38` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_39` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NOT NULL AND `ITEM_NME_39` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_40` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NOT NULL AND `ITEM_NME_40` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_41` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NOT NULL AND `ITEM_NME_41` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_42` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NOT NULL AND `ITEM_NME_42` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_43` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NOT NULL AND `ITEM_NME_43` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_44` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NOT NULL AND `ITEM_NME_44` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_45` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NOT NULL AND `ITEM_NME_45` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_46` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NOT NULL AND `ITEM_NME_46` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_47` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NOT NULL AND `ITEM_NME_47` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_48` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NOT NULL AND `ITEM_NME_48` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_49` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NOT NULL AND `ITEM_NME_49` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_50` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NOT NULL AND `ITEM_NME_50` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_51` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NOT NULL AND `ITEM_NME_51` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_52` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NOT NULL AND `ITEM_NME_52` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_53` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NOT NULL AND `ITEM_NME_53` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_54` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NOT NULL AND `ITEM_NME_54` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_55` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NOT NULL AND `ITEM_NME_55` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_56` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NOT NULL AND `ITEM_NME_56` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_57` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NOT NULL AND `ITEM_NME_57` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_58` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NOT NULL AND `ITEM_NME_58` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_59` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NOT NULL AND `ITEM_NME_59` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_60` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NOT NULL AND `ITEM_NME_60` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_61` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NOT NULL AND `ITEM_NME_61` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_62` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NOT NULL AND `ITEM_NME_62` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_63` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NOT NULL AND `ITEM_NME_63` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_64` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NOT NULL AND `ITEM_NME_64` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_65` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NOT NULL AND `ITEM_NME_65` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_66` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NOT NULL AND `ITEM_NME_66` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_67` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NOT NULL AND `ITEM_NME_67` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_68` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NOT NULL AND `ITEM_NME_68` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_69` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NOT NULL AND `ITEM_NME_69` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_70` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NOT NULL AND `ITEM_NME_70` IS NOT NULL "
                                    + ") tb2 ON tb1.`ID` = tb2.`VISIT` WHERE tb1.`ANIM_ID` = " + anim_id + " ORDER BY DATETIME;");
            ended_loading_visites_tab = true;
        }

        private void Main_Frm_Activated(object sender, EventArgs e)
        {            
            Params = PreConnection.Load_data("SELECT * FROM tb_params;");
            //-----------
            label_cab_nme.Text = Params.Rows.Cast<DataRow>().Where(RR => (int)RR["ID"] == 1).First()["VAL"].ToString();
            //----------
            DateTime tt = DateTime.Parse(Params.Rows.Cast<DataRow>().Where(RR => (int)RR["ID"] == 6).First()["VAL"].ToString());            
            if((tt - last_update_time).Seconds > 0)
            {
                refresh_main_tables();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) //CLIENT
            {
                comboBox2.DataSource = clients;
                comboBox2.ValueMember = "ID";
                comboBox2.DisplayMember = "FULL_NME";

            }
            else //ANIMAL
            {
                comboBox2.DataSource = animals;
                comboBox2.ValueMember = "ID";
                comboBox2.DisplayMember = "NME";

            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected_animal_id = selected_client_id = -1;
            if (comboBox2.SelectedValue != null)
            {
                if (int.TryParse(comboBox2.SelectedValue.ToString(), out int yy))
                {
                    if (comboBox1.SelectedIndex == 0) //CLIENT
                    {
                        selected_client_id = (int)comboBox2.SelectedValue;
                    }
                    else //ANIMAL
                    {
                        selected_animal_id = (int)comboBox2.SelectedValue;
                    }

                }
            }
            Refresh_current_tab();
        }

        private string get_blnk_null(object src)
        {
            if (src is DataGridViewCell)
            {
                if (((DataGridViewCell)src) != null)
                {
                    if (((DataGridViewCell)src).Value != DBNull.Value)
                    {
                        return ((DataGridViewCell)src).Value.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if (src != null)
                {
                    if (src != DBNull.Value)
                    {
                        return src.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        
        private void button7_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                chosen_client_from_search = new DataTable();
                Clients_List_Search select = new Clients_List_Search(1);
                select.DataTableReturned += ChildForm_DataTableReturned2;
                select.ShowDialog();
                if (chosen_client_from_search != null)
                {
                    comboBox2.SelectedValue = chosen_client_from_search.Rows[0][0];
                }
            }
            else
            {
                chosen_anim_from_search = new DataTable();
                Anims_List_Search select = new Anims_List_Search(1);
                select.DataTableReturned += ChildForm_DataTableReturned;
                select.ShowDialog();
                if (chosen_anim_from_search != null)
                {
                    comboBox2.SelectedValue = chosen_anim_from_search.Rows[0][1];
                }
            }
            Refresh_current_tab();
        }

        private void ChildForm_DataTableReturned2(object sender, DataTableEventArgs_Clients e)
        {
            chosen_client_from_search = e.DataTable;
        }

        private void ChildForm_DataTableReturned(object sender, DataTableEventArgs e)
        {
            chosen_anim_from_search = e.DataTable;
        }

        private void Refresh_current_tab()
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tabPage_visites_animal":
                    if (!loading_visites_tab)
                    {
                        visites_tab(selected_animal_id);
                        loading_visites_tab = true;
                        //----------------------
                        while (loading_visites_tab)
                        {
                            if (ended_loading_visites_tab)
                            {
                                loading_visites_tab = false;
                                dataGridView2.DataSource = main_visites_tab;
                                dataGridView2.Refresh();
                                int fct = main_visites_tab.AsEnumerable().Count(t => t["FACTURE_REF"] != DBNull.Value && ((string)t["FACTURE_REF"]).Trim().Length > 0);
                                radioButton1.Text = "Tous (" + main_visites_tab.Rows.Count + ")";
                                radioButton2.Text = "Facturé (" + fct + ")";
                                radioButton3.Text = "Non Facturé (" + (main_visites_tab.Rows.Count - fct) + ")";
                                radioButton1_CheckedChanged(null, null);
                            }
                        }
                    }
                break;
                case "tabPage_infos_animal":
                    DataRow inf = animals.AsEnumerable().Where(zz => (int)zz["ID"] == selected_animal_id).FirstOrDefault();                    
                    if (inf != null)
                    {                        
                        label16.Text = inf["DATE_ADDED"] != DBNull.Value ? ((DateTime)inf["DATE_ADDED"]).ToString("dddd dd/MM/yyyy", new CultureInfo("fr-FR")) : "--";
                        label25.Text = inf["NME"] != DBNull.Value ? (string)inf["NME"] : "--";
                        label17.Text = inf["NUM_IDENTIF"] != DBNull.Value ? (string)inf["NUM_IDENTIF"] : "--";
                        label18.Text = inf["NUM_PASSPORT"] != DBNull.Value ? (string)inf["NUM_PASSPORT"] : "--";
                        DataRow clnt_nme = clients.AsEnumerable().Where(zz => (int)zz["ID"] == (inf["CLIENT_ID"] != DBNull.Value ? (int)inf["CLIENT_ID"] : -1)).FirstOrDefault();
                        label21.Text = clnt_nme != null ? (string)clnt_nme["FULL_NME"] : "--";                        
                        label23.Text = inf["ESPECE"] != DBNull.Value ? (string)inf["ESPECE"] : "--";
                        label22.Text = inf["RACE"] != DBNull.Value ? (string)inf["RACE"] : "--";
                        label20.Text = inf["SEXE"] != DBNull.Value ? (string)inf["SEXE"] : "--";
                        label26.Text = inf["NISS_DATE"] != DBNull.Value ? ((DateTime)inf["NISS_DATE"]).ToString("dd/MM/yyyy"): "--";
                        label19.Text = inf["ROBE"] != DBNull.Value ? (string)inf["ROBE"] : "--";
                        textBox8.Text = inf["OBSERVATIONS"] != DBNull.Value ? (string)inf["OBSERVATIONS"] : "";
                        bool ttt = inf["IS_RADIATED"] != DBNull.Value;
                        ttt = ttt ? (sbyte)inf["IS_RADIATED"] == 1 : false;
                        if (ttt)
                        {
                            label24.Text = "Oui";
                            groupBox1.Visible = true;
                            label27.Text = inf["RADIATION_DATE"] != DBNull.Value ? ((DateTime)inf["RADIATION_DATE"]).ToString("dd/MM/yyyy") : "--";
                            textBox5.Text = inf["RADIATION_CAUSES"] != DBNull.Value ? (string)inf["RADIATION_CAUSES"] : "";
                        }
                        else
                        {
                            label24.Text = "Non";
                            groupBox1.Visible = false;
                            label27.Text = "--";
                            textBox5.Text = "";
                        }
                        pictureBox2.Image = inf["PICTURE"] != DBNull.Value ? PreConnection.ByteArrayToImage((byte[])inf["PICTURE"]) : (Properties.Settings.Default.Use_animals_logo ? (Image)Properties.Resources.ResourceManager.GetObject(label23.Text) : null);
                    }                    
                    break;
                    //==================================================================
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            System.Drawing.Font fnt = new System.Drawing.Font(radioButton1.Font.FontFamily, (float)8.25, FontStyle.Regular);
            System.Drawing.Font fnt2 = new System.Drawing.Font(radioButton1.Font.FontFamily, (float)9.25, FontStyle.Bold);

            radioButton1.Font = radioButton1.Checked ? fnt2 : fnt;
            radioButton2.Font = radioButton2.Checked ? fnt2 : fnt;
            radioButton3.Font = radioButton3.Checked ? fnt2 : fnt;
            if (radioButton2.Checked) //Facturé
            {
                ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "LEN(FACTURE_REF) > 0";
            }
            else if (radioButton3.Checked) //Non Facturé
            {

                ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "FACTURE_REF IS NULL OR LEN(FACTURE_REF) = 0";
            }
            else //Tous
            {
                ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "";

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh_current_tab();
        }
    }
}

