using ALBAITAR_Softvet.Dialogs;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
//using Xamarin.Forms.Internals;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Excc = Microsoft.Office.Interop.Excel;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Clients : Form
    {
        public static int ID_to_selectt = -1;
        public static int Infoss_1_Caiss_2 = 1;
        public static int Caisse_Idx = -1;
        DataTable clients;
        DataTable sites;
        List<string> wilayaa;
        List<string> cities;
        bool Is_New = true;
        //bool monetic_not_autorsed = false;
        bool monetic_del_autorised = true;
        int prev_rw_idx = -1;
        int prev_col_idx = -1;

        //string fact_ref_pattern = @"FA_\d{4}_\d{4}";
        int last_rw_idx_shown_conf_msg = -1;
        public Clients(int ID_to_select, int Infos_1_Caiss_2, int Caisse_Id)
        {
            InitializeComponent();
            //tabControl1.TabPages.Remove(tabPage1);
            ID_to_selectt = ID_to_select;
            Infoss_1_Caiss_2 = Infos_1_Caiss_2;
            Caisse_Idx = Caisse_Id;

            dataGridView1.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            //----------------------
            Load_clients_from_DB();
            //---------------------
            sites = Main_Frm.ADRESSES_SITES;
            wilayaa = new List<string>();
            cities = new List<string>();
            sites.Rows.Cast<DataRow>().ToList().ForEach(row =>
            {
                if (!wilayaa.Contains(row["WILAYA"])) { wilayaa.Add(row["WILAYA"].ToString()); }
                if (!cities.Contains(row["CITY"])) { cities.Add(row["CITY"].ToString()); }
                if (!comboBox2.Items.Contains(row["CITY"])) { comboBox2.Items.Add(row["CITY"].ToString()); }
            });
            comboBox3.DataSource = wilayaa;
            //------------------
            if (dataGridView1.SelectedRows.Count == 0)
            {
                comboBox3.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
                comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
                textBox6.TextChanged -= textBox6_TextChanged;
                textBox6.Validating -= textBox6_Validating;
                textBox6.Validated -= textBox6_Validated;
                //-----------------------------
                comboBox1.SelectedIndex = 0;
                comboBox3.SelectedIndex = comboBox2.SelectedIndex = -1;
                textBox6.Text = string.Empty;
                //--------------
                comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
                comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
                textBox6.TextChanged += textBox6_TextChanged;
                textBox6.Validating += textBox6_Validating;
                textBox6.Validated += textBox6_Validated;
            }
            //------------------------


        }
        private void Load_clients_from_DB()
        {
            int prec_nb = dataGridView1.Rows.Count;
            int fd = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : 0;
            //clients = PreConnection.Load_data_keeping_duplicates("SELECT *,concat(FAMNME,' ',NME) AS FULL_NME FROM tb_clients tb1" + (checkBox1.Checked ? " WHERE ID IN (SELECT CLIENT_ID FROM tb_clients_finance HAVING SUM(DEBIT)-SUM(CREDIT) <> 0)" : "") + (checkBox2.Checked ? (checkBox1.Checked ? " AND " : " WHERE ") + "(SELECT COUNT(*) FROM tb_animaux WHERE CLIENT_ID = tb1.ID) = 0" : "") + ";");
            clients = PreConnection.Load_data_keeping_duplicates("SELECT tb1.*, "
                                                               + "CONCAT(tb1.FAMNME, ' ', tb1.NME) AS FULL_NME, "
                                                               + "    SUM(COALESCE(tb2.DEBIT, 0)) - SUM(COALESCE(tb2.CREDIT, 0)) AS SLD, "
                                                               + "    tb3.ANIM_CNT "
                                                               + "FROM tb_clients tb1 "
                                                               + "LEFT JOIN tb_clients_finance tb2 ON tb1.ID = tb2.CLIENT_ID "
                                                               + "LEFT JOIN ("
                                                               + "    SELECT CLIENT_ID, COUNT(*) AS ANIM_CNT"
                                                               + "    FROM tb_animaux  WHERE IS_RADIATED = 0 OR IS_RADIATED IS NULL"
                                                               + "    GROUP BY CLIENT_ID"
                                                               + ") tb3 ON tb1.ID = tb3.CLIENT_ID "
                                                               + "GROUP BY tb1.ID, tb1.FAMNME, tb1.NME, FULL_NME, tb3.ANIM_CNT ORDER BY FULL_NME; ");
            //dataGridView1.DataSource = clients;
            dgv1_fltr();
            // PreConnection.search_filter_datagridview(dataGridView1, textBox1.Text);
            if (dataGridView1.Rows.Count > prec_nb)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows.Cast<DataGridViewRow>().OrderBy(r => r.Cells["ID"].Value).Last().Selected = true;
            }
            else if (dataGridView1.Rows.Count > fd) { dataGridView1.ClearSelection(); dataGridView1.Rows[fd].Selected = true; }
            else { 
                button3.PerformClick();
            }
        }

        private void dgv1_fltr()
        {
            DataView dv = new DataView(clients);
            string fltr = "";
            if (checkBox1.Checked)
            {
                fltr = "SLD > 0";
            }

            if (checkBox2.Checked)
            {
                fltr += (fltr.Length > 0 ? " AND " : "") + "(ANIM_CNT = 0 OR ANIM_CNT IS NULL)";
            }

            if (textBox1.Text.Trim().Length > 0)
            {
                bool ff = fltr.Length > 0;
                fltr += (ff ? " AND (" : "");
                //-------------
                fltr += "FULL_NME LIKE '%{0}%'";
                fltr += " OR NUM_CNI LIKE '%{0}%'";
                fltr += " OR ADRESS LIKE '%{0}%'";
                fltr += " OR POSTAL_CODE LIKE '%{0}%'";
                fltr += " OR CITY LIKE '%{0}%'";
                fltr += " OR WILAYA LIKE '%{0}%'";
                fltr += " OR NUM_PHONE LIKE '%{0}%'";
                fltr += " OR EMAIL LIKE '%{0}%'";
                fltr += " OR OBSERVATIONS LIKE '%{0}%'";
                //-------------
                fltr += (ff ? ")" : "");

            }
            if (clients.Columns.Count > 0)
            {
                dv.RowFilter = string.Format(fltr, textBox1.Text);
            }

            dataGridView1.DataSource = dv;



        }
        private void Load_selected_client_fields()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                label13.Visible = false;
                label15.Visible = false;
                textBox9.Validated -= textBox9_Validated;
                textBox2.Validated -= textBox2_Validated;
                textBox3.Validated -= textBox2_Validated;
                comboBox3.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
                comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
                textBox6.TextChanged -= textBox6_TextChanged;
                textBox6.Validating -= textBox6_Validating;
                textBox6.Validated -= textBox6_Validated;
                maskedTextBox1.Enter -= maskedTextBox1_Enter;
                maskedTextBox1.Validating -= maskedTextBox1_Validating;
                textBox7.TextChanged -= textBox7_TextChanged;
                textBox7.Validating -= textBox7_Validating;
                pictureBox1.Image = Properties.Resources.MODIF;
                panel3.Visible = true;
                //if (tabControl1.TabPages.Count < 2) { tabControl1.TabPages.Add(tabPage1); }
                //----------------------------------------------
                textBox9.Text = dataGridView1.SelectedRows[0].Cells["REF"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["REF"].Value.ToString() : "";
                comboBox1.SelectedItem = dataGridView1.SelectedRows[0].Cells["SEX"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["SEX"].Value.ToString() : "";
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["FAMNME"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["FAMNME"].Value.ToString() : "";
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["NME"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["NME"].Value.ToString() : "";
                textBox4.Text = dataGridView1.SelectedRows[0].Cells["NUM_CNI"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["NUM_CNI"].Value.ToString() : "";
                textBox5.Text = dataGridView1.SelectedRows[0].Cells["ADRESS"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["ADRESS"].Value.ToString() : "";
                textBox6.Text = dataGridView1.SelectedRows[0].Cells["POSTAL_CODE"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["POSTAL_CODE"].Value.ToString() : "";
                textBox7.Text = dataGridView1.SelectedRows[0].Cells["EMAIL"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["EMAIL"].Value.ToString() : "";
                textBox8.Text = dataGridView1.SelectedRows[0].Cells["OBSERVATIONS"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["OBSERVATIONS"].Value.ToString() : "";
                comboBox3.Text = dataGridView1.SelectedRows[0].Cells["WILAYA"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["WILAYA"].Value.ToString() : "";
                comboBox2.Text = dataGridView1.SelectedRows[0].Cells["CITY"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["CITY"].Value.ToString() : "";
                maskedTextBox1.Text = dataGridView1.SelectedRows[0].Cells["NUM_PHONE"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["NUM_PHONE"].Value.ToString() : "";

                //----------------------------------------------
                textBox9.Validated += textBox9_Validated;
                textBox2.Validated += textBox2_Validated;
                textBox3.Validated += textBox2_Validated;
                comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
                comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
                textBox6.TextChanged += textBox6_TextChanged;
                textBox6.Validating += textBox6_Validating;
                textBox6.Validated += textBox6_Validated;
                maskedTextBox1.Enter += maskedTextBox1_Enter;
                maskedTextBox1.Validating += maskedTextBox1_Validating;
                textBox7.TextChanged += textBox7_TextChanged;
                textBox7.Validating += textBox7_Validating;
            }
            else
            {
                button3.PerformClick();
            }

        }
        private void verif_if_déja_exist_client()
        {
            if (textBox2.Text.Length > 0 && textBox3.Text.Length > 0 && (Is_New || (!Is_New && dataGridView1.SelectedRows.Count > 0)))
            {
                DataTable tmmmmp = PreConnection.Load_data("SELECT * FROM tb_clients WHERE FAMNME LIKE '" + textBox3.Text.Replace("'", "''") + "' AND NME LIKE '" + textBox2.Text.Replace("'", "''") + "' AND NUM_CNI LIKE '" + textBox4.Text.Replace("'", "''") + "'" + (!Is_New ? " AND ID <> " + dataGridView1.SelectedRows[0].Cells["ID"].Value : "") + ";");
                int cnt = tmmmmp != null ? tmmmmp.Rows.Count : 0;
                //int cnt = clients.Rows.Cast<DataRow>().Where(zz =>
                //zz["FAMNME"].ToString().ToLower().Equals(textBox3.Text.ToLower()) &&
                //zz["NME"].ToString().ToLower().Equals(textBox2.Text.ToLower()) &&
                //zz["NUM_CNI"].ToString().Equals(textBox4.Text) &&
                //(!Is_New && dataGridView1.SelectedRows.Count > 0 ? (int)zz["ID"] != (int)dataGridView1.SelectedRows[0].Cells["ID"].Value : true)

                //).ToList().Count();
                label13.Visible = cnt > 0;
            }
            else { label13.Visible = false; }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dgv1_fltr();
        }

        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            if ((!int.TryParse(textBox6.Text.Trim(), out _) && textBox6.Text.Length > 0))
            {
                e.Cancel = true;
                textBox6.BackColor = Color.LightCoral;
            }
            else
            {
                textBox6.BackColor = SystemColors.Window;
            }
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            textBox6.BackColor = SystemColors.Window;
            if (textBox6.Text == string.Empty) { comboBox2_SelectedIndexChanged(null, null); }
        }
        bool textbx6_typed = false;
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox6.BackColor = SystemColors.Window;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3_Validating(null, null);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<DataRow> ze = sites.Rows.Cast<DataRow>().Where(dd => dd["CITY"].Equals(comboBox2.Text)).ToList();
            if ((!textbx6_typed || textBox6.Text == string.Empty) && ze.Count > 0)
            {
                textBox6.Text = ze[0]["ZIP_CODE"].ToString();
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbx6_typed = true;
        }

        private void comboBox3_Validating(object sender, CancelEventArgs e)
        {
            if (comboBox3.Text != string.Empty)
            {
                List<string> tmmp = new List<string>();
                sites.Rows.Cast<DataRow>().Where(dda => dda["WILAYA"].Equals(comboBox3.Text)).ToList().ForEach(row =>
                {
                    if (!tmmp.Contains(row["CITY"])) { tmmp.Add(row["CITY"].ToString()); }
                });
                //------------------
                string dd = comboBox2.Text;
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(tmmp.Count > 0 ? tmmp.ToArray() : cities.ToArray());
                comboBox2.SelectedIndex = comboBox2.Items.Contains(dd) ? comboBox2.Items.IndexOf(dd) : (comboBox2.Items.Count > 0 ? 0 : -1);
            }
            else
            {
                string dd = comboBox2.Text;
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(cities.ToArray());
                comboBox2.SelectedIndex = comboBox2.Items.Contains(dd) ? comboBox2.Items.IndexOf(dd) : (comboBox2.Items.Count > 0 ? 0 : -1);
            }
        }

        private void maskedTextBox1_Validating(object sender, CancelEventArgs e)
        {
            if (maskedTextBox1.Text != string.Empty && int.TryParse(maskedTextBox1.Text, out int jh) && !maskedTextBox1.Text.StartsWith("+"))
            {
                if (maskedTextBox1.Text.Length == 10)
                {
                    maskedTextBox1.Mask = "0000 00 00 00";
                }
                else if (maskedTextBox1.Text.Length == 9)
                {
                    maskedTextBox1.Mask = "000 00 00 00";
                }
                else
                {
                    maskedTextBox1.Mask = "";
                }
            }
            else
            {
                maskedTextBox1.Mask = "";
            }
        }

        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            maskedTextBox1.Mask = "";
        }

        private void textBox7_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBox7.Text, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z") && textBox7.Text.Length > 0)
            {
                e.Cancel = true;
                textBox7.BackColor = Color.LightCoral;
            }
            else
            {
                textBox7.BackColor = SystemColors.Window;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            bool insert_autori = true;
            bool updadate_autori = true;
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                insert_autori = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10001" && (Int32)QQ[3] == 1).Count() > 0;
                updadate_autori = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10003" && (Int32)QQ[3] == 1).Count() > 0;
            }

            bool all_ready = true;
            textBox9.BackColor = textBox9.Text.Trim() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox2.BackColor = textBox2.Text.Trim() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox3.BackColor = textBox3.Text.Trim() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox4.BackColor = textBox4.Text.Trim() != string.Empty ? SystemColors.Window : Color.LightCoral;

            all_ready &= textBox9.Text.Trim() != string.Empty;
            all_ready &= textBox2.Text.Trim() != string.Empty;
            all_ready &= textBox3.Text.Trim() != string.Empty;
            all_ready &= textBox4.Text.Trim() != string.Empty;
            all_ready &= !label13.Visible;
            all_ready &= !label15.Visible;
            //-------------
            label12.Visible = !all_ready;
            //-------------
            if (all_ready)
            {
                if (Is_New) //INSERT
                {
                    if (insert_autori)
                    {
                        PreConnection.Excut_Cmd(1, "tb_clients", new List<string> { "REF","SEX", "FAMNME", "NME", "NUM_CNI", "ADRESS", "POSTAL_CODE", "CITY", "WILAYA", "NUM_PHONE", "EMAIL", "OBSERVATIONS" }, new List<object> { textBox9.Text, comboBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text, comboBox2.Text, comboBox3.Text, maskedTextBox1.Text, textBox7.Text, textBox8.Text }, null, null, null);
                    }
                    else
                    {
                        new Non_Autorized_Msg("").ShowDialog();
                    }

                }
                else //UPDATE
                {
                    if (updadate_autori)
                    {
                        PreConnection.Excut_Cmd(2, "tb_clients", new List<string> { "REF", "SEX", "FAMNME", "NME", "NUM_CNI", "ADRESS", "POSTAL_CODE", "CITY", "WILAYA", "NUM_PHONE", "EMAIL", "OBSERVATIONS" }, new List<object> { textBox9.Text, comboBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text, comboBox2.Text, comboBox3.Text, maskedTextBox1.Text, textBox7.Text, textBox8.Text }, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView1.SelectedRows[0].Cells["ID"].Value });
                    }
                    else
                    {
                        new Non_Autorized_Msg("").ShowDialog();
                    }

                }
                //----------------
                Load_clients_from_DB();
            }



        }

        private void label12_VisibleChanged(object sender, EventArgs e)
        {
            if (label12.Visible)
            {
                Timer tmr = new Timer();
                tmr.Interval = 1000;
                tmr.Tick += Tmr_Tick;
                tmr.Start();
            }
        }

        int timm = 0;
        private void Tmr_Tick(object sender, EventArgs e)
        {
            timm++;
            if (timm >= 3)
            {
                label12.Visible = false;
                timm = 0;
                ((Timer)sender).Stop();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
            label13.Visible = false;
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(".", " ");
            verif_if_déja_exist_client();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Is_New = false;
            Load_selected_client_fields();
            Load_finace_historic();

            //if (monetic_not_autorsed && tabPage1 != null)
            //{
            //    tabControl1.TabPages.Remove(tabPage1);
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if(tabControl1.SelectedTab != tabPage2)
            //{
            //    tabControl1.SelectTab(tabPage2);
            //}
            dataGridView1.ClearSelection();
            Is_New = true;
            //can_start_historic_saving = false;
            foreach (Control ctrl in tabPage2.Controls)//            foreach(Control ctrl in splitContainer1.Panel2.Controls)
            {
                if (ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(MaskedTextBox) || (ctrl.GetType() == typeof(ComboBox) && ((ComboBox)ctrl).DropDownStyle != ComboBoxStyle.DropDownList))
                {
                    ctrl.Text = string.Empty;
                }
                else if (ctrl.GetType() == typeof(ComboBox) && ((ComboBox)ctrl).DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    ((ComboBox)ctrl).SelectedIndex = 0;
                }
            }
            label15.Visible = false;
            label13.Visible = false;
            pictureBox1.Image = Properties.Resources.NOUVEAU;
            panel3.Visible = false;
            //-----------
            textBox9.Text = "00001";
            if (clients.Rows.Count > 0) {
                int yy = (int)clients.Rows.Cast<DataRow>().Max(rr => rr["ID"]) + 1;
                textBox9.Text = yy.ToString("00000"); }
            //----------
            //if (tabControl1.TabPages.Count > 1) { tabControl1.TabPages.Remove(tabPage1); }
            if (!textBox1.Focused) { textBox3.Select(); }
            button5.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string fff = "";
                dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row => fff += "," + row.Cells["ID"].Value);
                fff = fff.Substring(1);
                DataTable dt = PreConnection.Load_data("SELECT SUM(`DEBIT`-`CREDIT`) AS SLLD FROM tb_clients_finance WHERE CLIENT_ID IN (" + fff + ");");
                decimal dd = dt.Rows[0][0] != DBNull.Value ? (decimal)dt.Rows[0][0] : 0;
                string slld = dd != 0 ? "- Il y a des soldes monétiques non réglés !" : "";
                if (MessageBox.Show("Vous étes sures de supprimer " + (dataGridView1.SelectedRows.Count > 1 ? ("ces [" + dataGridView1.SelectedRows.Count + "] clients ?") : "ce client ?") + "\n\n\nAttention :\n\n" + slld + "\n\n-Tous " + (dataGridView1.SelectedRows.Count == 1 ? "ses" : "leurs") + " animaux seront supprimés!\n(Avec tous informations associés (Laboratires, Agenda ...))\n", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    PreConnection.Excut_Cmd_personnel("DELETE FROM tb_clients WHERE ID IN (" + fff + ");", null, null);
                    Load_clients_from_DB();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                Excc.Application xcelApp = new Excc.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Clients";
                xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Clients";
                dataGridView1.Columns.Cast<DataGridViewColumn>().Where(ss => ss.Name != "ID" && ss.Name != "FULL_NME").ToList().ForEach(g =>
                {
                    switch (g.HeaderText)
                    {
                        case "REF":
                            xcelApp.Cells[1, g.Index + 1].Value = "Réf.";
                            break;
                        case "SEX":
                            xcelApp.Cells[1, g.Index + 1].Value = "Sexe";
                            break;
                        case "FAMNME":
                            xcelApp.Cells[1, g.Index + 1].Value = "Prénom";
                            break;
                        case "NME":
                            xcelApp.Cells[1, g.Index + 1].Value = "Nom";
                            break;
                        case "NUM_CNI":
                            xcelApp.Cells[1, g.Index + 1].Value = "N° CNI";
                            break;
                        case "ADRESS":
                            xcelApp.Cells[1, g.Index + 1].Value = "Adresse";
                            break;
                        case "POSTAL_CODE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Code Postal";
                            break;
                        case "CITY":
                            xcelApp.Cells[1, g.Index + 1].Value = "Ville";
                            break;
                        case "WILAYA":
                            xcelApp.Cells[1, g.Index + 1].Value = "Wilaya";
                            break;
                        case "NUM_PHONE":
                            xcelApp.Cells[1, g.Index + 1].Value = "N° Tél";
                            break;
                        case "EMAIL":
                            xcelApp.Cells[1, g.Index + 1].Value = "Email";
                            break;
                        case "OBSERVATIONS":
                            xcelApp.Cells[1, g.Index + 1].Value = "Observations";
                            break;
                        case "SLD":
                            xcelApp.Cells[1, g.Index + 1].Value = "Solde Monétique";
                            break;
                        case "ANIM_CNT":
                            xcelApp.Cells[1, g.Index + 1].Value = "Nombre d'Animaux";
                            break;
                    }
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                    try
                    {
                        if (dataGridView1.Columns[g.Index].DefaultCellStyle.Format == "N2")
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "#,##0.00 [$Da-fr-dz]";
                        }
                        else if (dataGridView1.Columns[g.Index].DefaultCellStyle.Format.Contains("MM/yyyy"))
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "dd/MM/yyyy" + (dataGridView1.Columns[g.Index].DefaultCellStyle.Format.Contains("HH") ? " HH:mm:ss" : "");
                        }
                    }
                    catch { }
                });

                dataGridView1.Rows.Cast<DataGridViewRow>().ToList().ForEach(t =>
                {
                    t.Cells.Cast<DataGridViewCell>().ToList().ForEach(b =>
                    {
                        xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").Replace("00:00:00", "").Trim() : "";
                    });

                });
                xcelApp.Columns[dataGridView1.Columns["ID"].Index + 1].Delete();
                xcelApp.Columns[dataGridView1.Columns["FULL_NME"].Index].Delete();
                xcelApp.Columns.AutoFit();
                //------------------
                SaveFileDialog svd = new SaveFileDialog();
                svd.Filter = "Excel | *.xlsx";
                svd.DefaultExt = "*.xlsx";
                svd.FileName = xcelApp.Application.Workbooks[1].Title + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                if (svd.ShowDialog() == DialogResult.OK)
                {
                    xcelApp.Workbooks[1].SaveAs(Path.GetFullPath(svd.FileName));
                    Process.Start(Path.GetFullPath(svd.FileName));
                }
                xcelApp.Application.Workbooks[1].Close(false);
                xcelApp.Quit();
                //-------------------
            }
            else
            {
                MessageBox.Show("Aucun donnés !", ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void Clients_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button4.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10002" && (Int32)QQ[3] == 1).Count() > 0; //Supprimer
                button3.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10001" && (Int32)QQ[3] == 1).Count() > 0; //Ajouter
                //--------------------
                if (Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50000" && (Int32)QQ[3] == 1).Count() == 0)
                {
                    //monetic_not_autorsed = true;
                    panel3.Visible = false;
                    //tabControl1.TabPages.Remove(tabPage1);
                }
                else
                {
                    dataGridView2.ReadOnly = (Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60001" && (Int32)QQ[3] == 1).Count() +
                        Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60002" && (Int32)QQ[3] == 1).Count()) == 2;
                    monetic_del_autorised = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60003" && (Int32)QQ[3] == 1).Count() > 0;
                }
            }
            //--------------
            if (ID_to_selectt > 0)
            {
                tabControl1.SelectedTab = Infoss_1_Caiss_2 == 2 ? tabPage1 : tabPage2;
                dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(Q => (int)Q.Cells["ID"].Value == ID_to_selectt).ToList().ForEach(W => W.Selected = true);
                ID_to_selectt = -1;
                //-------
                if (Caisse_Idx > -1)
                {
                    dataGridView2.ClearSelection();
                    //var yy = dataGridView2.Rows.Cast<DataGridViewRow>().Where(F => F.Cells["IDD_FINANC"] != null).Where(Q => (Q.Cells["IDD_FINANC"].Value != DBNull.Value ? (int)Q.Cells["IDD_FINANC"].Value : -999) == Caisse_Idx);
                    var yy = dataGridView2.Rows.Cast<DataGridViewRow>()
    .Where(F => F.Cells["IDD_FINANC"] != null && F.Cells["IDD_FINANC"].Value != null)
    .Where(Q => Convert.ToInt32(Q.Cells["IDD_FINANC"].Value) == Caisse_Idx);
                    if (yy.Any())
                    {
                        yy.ForEach(W =>
                        {
                            W.Cells[4].Selected = true;
                            dataGridView2.CurrentCell = dataGridView2.SelectedCells[0];
                            dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.SelectedCells[0].RowIndex;
                        });
                    }

                    Caisse_Idx = -1;
                }
                else if (Caisse_Idx == -2)
                {
                    Caisse_Idx = -1;
                }

            }
            else if (ID_to_selectt == -2) //NEW
            {
                ID_to_selectt = -1;
                button3.PerformClick();
            }

        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && monetic_del_autorised && dataGridView2.SelectedCells.Count > 0)
            {
                if (MessageBox.Show("Êtes-vous sûrs de faire la suppression ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PreConnection.Excut_Cmd(3, "tb_clients_finance", null, null, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells["IDD_FINANC"].Value });
                    Load_finace_historic();
                    //------------------
                }
            }
        }
        private void calc_sold()
        {
            if (dataGridView2.Columns["DEBIT"] != null && dataGridView2.Columns["CREDIT"] != null)
            {
                double tot_debit = dataGridView2.Rows.Cast<DataGridViewRow>().Sum(row =>
                     row.Cells["DEBIT"].Value != null && row.Cells["DEBIT"].Value != DBNull.Value ?
                     Convert.ToDouble(row.Cells["DEBIT"].Value) : 0);

                double tot_credit = dataGridView2.Rows.Cast<DataGridViewRow>().Sum(row =>
                    row.Cells["CREDIT"].Value != null && row.Cells["CREDIT"].Value != DBNull.Value ?
                    Convert.ToDouble(row.Cells["CREDIT"].Value) : 0);
                //---------
                double sld = tot_debit - tot_credit;

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    dataGridView1.SelectedRows[0].Cells["SLD"].Value = sld;
                    dataGridView1.InvalidateRow(dataGridView1.SelectedRows[0].Index);
                }
                //-----------------
                if (dataGridView3.Columns.Count > 0)
                {
                    if (dataGridView3.Rows.Count == 0)
                    {
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[0].Cells[0].Value = "dd";
                        dataGridView3.Rows[0].Cells[1].Value = tot_debit;
                        dataGridView3.Rows[0].Cells[2].Value = tot_credit;
                    }
                    dataGridView3.Rows[0].Cells[0].Value = "Solde Total : " + (sld == 0 ? "Rien (0.00 DA)." : (sld >= 0 ? "Il a une dette de (" + sld.ToString("N2") + " DA)." : "On lui doit (" + (sld * -1).ToString("N2") + " DA)."));
                    dataGridView3.Rows[0].Cells[1].Value = tot_debit;
                    dataGridView3.Rows[0].Cells[2].Value = tot_credit;
                }
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (dataGridView2.Rows[e.RowIndex].IsNewRow) //INSERT
                {

                    PreConnection.Excut_Cmd(1, "tb_clients_finance", new List<string> {
                        "CLIENT_ID",
                        "OP_DATE",
                        "OBJECT",
                        "DEBIT",
                        "CREDIT" },
                    new List<object>
                {
                        dataGridView1.SelectedRows[0].Cells["ID"].Value,
                        (dataGridView2.Rows[e.RowIndex].Cells["OP_DATE"].Value != null && dataGridView2.Rows[e.RowIndex].Cells["OP_DATE"].Value != DBNull.Value) ? dataGridView2.Rows[e.RowIndex].Cells["OP_DATE"].Value : DateTime.Now,
                        (dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value != null && dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value != DBNull.Value)? dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value : DBNull.Value,
                        (dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value != null && dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value != DBNull.Value)? dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value : 0,
                        (dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value != null && dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value != DBNull.Value)? dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value : 0
                }, null, null, null);

                }
                else //UPDATE
                {
                    if (dataGridView2.Columns[e.ColumnIndex].Name == "DEBIT" && (dataGridView2.Rows[e.RowIndex].Cells["FACT_NUM"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["FACT_NUM"].Value.ToString() : "").Length > 0)
                    {
                        MessageBox.Show("N'est pas possible de modifier la cellul '[+] Endetté' à cause qu'il concerne le montant d'une facture, vous devez visiter la forme de facturation pour faire modifer montant de facture (Réf: '" + dataGridView2.Rows[e.RowIndex].Cells["FACT_NUM"].Value + "').", "Non possible :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        string Obj = dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value.ToString() : "";
                        if (e.ColumnIndex == dataGridView2.Columns["OBJECT"].Index)
                        {

                            string fac_num = dataGridView2.Rows[e.RowIndex].Cells["FACT_NUM"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["FACT_NUM"].Value.ToString() : "";
                            if (fac_num.Trim().Length > 0 && !Obj.Contains(fac_num))
                            {
                                Obj += " [" + fac_num.Trim() + "]";
                                MessageBox.Show("Cette opération est relative à une facture,\ndonc ce texte doit inclue la reference de facture qu'il concerne,\na cause qu'il est votre repaire de révision plus tard.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        PreConnection.Excut_Cmd(2, "tb_clients_finance", new List<string> {
                        "OP_DATE",
                        "OBJECT",
                        "DEBIT",
                        "CREDIT" },
                            new List<object>
                        {
                        dataGridView2.Rows[e.RowIndex].Cells["OP_DATE"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["OP_DATE"].Value : DateTime.Now,
                        Obj,
                        dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value : 0,
                        dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value : 0
                        }, "ID = @IDD", new List<string> { "IDD" }, new List<object> { dataGridView2.Rows[e.RowIndex].Cells["IDD_FINANC"].Value });
                    }


                }
            }
            Load_finace_historic();

        }
        private void Load_finace_historic()
        {
            last_rw_idx_shown_conf_msg = -1;
            //------------------------
            int prev_select_rw = dataGridView2.SelectedCells.Count > 0 ? dataGridView2.SelectedCells[0].RowIndex : -1;
            int prev_select_col = dataGridView2.SelectedCells.Count > 0 ? dataGridView2.SelectedCells[0].ColumnIndex : -1;

            dataGridView2.CellValueChanged -= dataGridView2_CellValueChanged;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_clients_finance WHERE CLIENT_ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + " ORDER BY OP_DATE;");
                dataGridView2.DataSource = dt;
                if (dataGridView2.DisplayedRowCount(false) < dataGridView2.RowCount) { dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows.Count - 1; }
            }
            else
            {
                if (dataGridView2.DataSource != null)
                {
                    ((DataTable)dataGridView2.DataSource).Rows.Clear();
                }
            }
            if (dataGridView2.Rows.Count > prev_select_rw && prev_select_rw > -1) { dataGridView2.CurrentCell = dataGridView2.Rows[prev_select_rw].Cells[prev_select_col]; }

            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
            calc_sold();
            //--------------
        }


        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false; // don't throw an exception
        }


        private void Clients_Activated(object sender, EventArgs e)
        {
            if (ID_to_selectt > 0)
            {
                tabControl1.SelectedTab = Infoss_1_Caiss_2 == 2 ? tabPage1 : tabPage2;
                dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(Q => (int)Q.Cells["ID"].Value == ID_to_selectt).ToList().ForEach(W => W.Selected = true);
                ID_to_selectt = -1;
                //-------
                if (Caisse_Idx > -1)
                {
                    dataGridView2.ClearSelection();
                    dataGridView2.Rows.Cast<DataGridViewRow>().Where(Q => (int)Q.Cells["IDD_FINANC"].Value == Caisse_Idx).ToList().ForEach(W =>
                    {
                        W.Cells[4].Selected = true;
                        dataGridView2.CurrentCell = dataGridView2.SelectedCells[0];
                        dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.SelectedCells[0].RowIndex;
                    });
                    Caisse_Idx = -1;
                }
                else if (Caisse_Idx == -2)
                {
                    dataGridView2.Focus();
                    Caisse_Idx = -1;
                }
            }
            else if (ID_to_selectt == -2) //NEW
            {
                ID_to_selectt = -1;
                button3.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PreConnection.Excport_to_excel(dataGridView2, "Historique monétique", dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value != DBNull.Value ? "Monét." : dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value.ToString(), null, false);
            if (dataGridView2.Rows.Count > 0)
            {
                Excc.Application xcelApp = new Excc.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Historique monétique";
                xcelApp.Application.Workbooks[1].Worksheets[1].Name = dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value;
                //----------
                xcelApp.Cells[1, 1].Value = "Date"; //OP_DATE
                xcelApp.Cells[1, 2].Value = "Objet"; //OBJECT
                xcelApp.Cells[1, 3].Value = "[+] Endetté"; //DEBIT
                xcelApp.Cells[1, 4].Value = "[-] Doit"; //CREDIT
                xcelApp.Cells[1, 5].Value = "Solde (Dette)";
                //-----------
                for (int i = 1; i < 5; i++)
                {
                    ((Excc.Range)xcelApp.Cells[1, i]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[1, i]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[1, i]).Font.Color = ColorTranslator.ToOle(Color.White);
                    ((Excc.Range)xcelApp.Cells[1, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                }
                ((Excc.Range)xcelApp.Cells[1, 5]).Interior.Color = ColorTranslator.ToOle(Color.Orange); //TTC
                //----------------
                ((Excc.Range)xcelApp.Columns[1]).NumberFormat = "dd/MM/yyyy"; //OP_DATE
                ((Excc.Range)xcelApp.Columns[3]).NumberFormat = //DEBIT
                ((Excc.Range)xcelApp.Columns[4]).NumberFormat = //CREDIT
                ((Excc.Range)xcelApp.Columns[5]).NumberFormat = "#,##0.00 [$Da-fr-dz]"; //SLDDD
                //--------------
                dataGridView2.Rows.Cast<DataGridViewRow>().Where(R => R.Index < dataGridView2.NewRowIndex).ToList().ForEach(t =>
                {
                    xcelApp.Cells[t.Index + 2, 1].Value = dataGridView2.Rows[t.Index].Cells["OP_DATE"].Value != DBNull.Value ? dataGridView2.Rows[t.Index].Cells["OP_DATE"].Value.ToString().Replace("00:00:00", "").Trim() : ""; //OP_DATE
                    xcelApp.Cells[t.Index + 2, 2].Value = dataGridView2.Rows[t.Index].Cells["OBJECT"].Value != DBNull.Value ? dataGridView2.Rows[t.Index].Cells["OBJECT"].Value.ToString().Trim() : ""; //OBJECT
                    xcelApp.Cells[t.Index + 2, 3].Value = dataGridView2.Rows[t.Index].Cells["DEBIT"].Value != DBNull.Value ? dataGridView2.Rows[t.Index].Cells["DEBIT"].Value : 0; //DEBIT
                    xcelApp.Cells[t.Index + 2, 4].Value = dataGridView2.Rows[t.Index].Cells["CREDIT"].Value != DBNull.Value ? dataGridView2.Rows[t.Index].Cells["CREDIT"].Value : 0; //CREDIT
                    xcelApp.Cells[t.Index + 2, 5].Value = xcelApp.Cells[t.Index + 2, 3].Value - xcelApp.Cells[t.Index + 2, 4].Value; //SLD

                    ((Excc.Range)xcelApp.Cells[t.Index + 2, 5]).Interior.Color = ColorTranslator.ToOle(Color.Moccasin);
                });
                //----------
                xcelApp.Range["C" + (dataGridView2.RowCount + 1)].Formula = "=SUM(C2:C" + (dataGridView2.RowCount) + ")";
                xcelApp.Range["D" + (dataGridView2.RowCount + 1)].Formula = "=SUM(D2:D" + (dataGridView2.RowCount) + ")";
                xcelApp.Range["E" + (dataGridView2.RowCount + 1)].Formula = "=SUM(E2:E" + (dataGridView2.RowCount) + ")";
                //-------------
                for (int i = 1; i < 5; i++)
                {
                    ((Excc.Range)xcelApp.Cells[dataGridView2.RowCount + 1, i]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[dataGridView2.RowCount + 1, i]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[dataGridView2.RowCount + 1, i]).Font.Color = ColorTranslator.ToOle(Color.White);
                    ((Excc.Range)xcelApp.Cells[dataGridView2.RowCount + 1, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                }
                ((Excc.Range)xcelApp.Cells[dataGridView2.RowCount + 1, 5]).Interior.Color = ColorTranslator.ToOle(Color.Orange);
                //-----------
                xcelApp.Columns.AutoFit();
                //------------------
                SaveFileDialog svd = new SaveFileDialog();
                svd.Filter = "Excel | *.xlsx";
                svd.DefaultExt = "*.xlsx";
                svd.FileName = xcelApp.Application.Workbooks[1].Title + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                if (svd.ShowDialog() == DialogResult.OK)
                {
                    xcelApp.Workbooks[1].SaveAs(Path.GetFullPath(svd.FileName));
                    Process.Start(Path.GetFullPath(svd.FileName));
                }
                xcelApp.Application.Workbooks[1].Close(false);
                xcelApp.Quit();
                //-------------------
            }
            else
            {
                MessageBox.Show("Aucun donnés !", ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dgv1_fltr();

        }

        private void dataGridView1_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.Column.Name == "ID")
            {
                if (e.Column.Visible == true)
                {
                    e.Column.Visible = false;
                }
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {


            // Check if the cell value is not null or DBNull and cast it safely
            var cellValue = dataGridView1.Rows[e.RowIndex].Cells["SLD"].Value;

            bool mm = cellValue != null && cellValue != DBNull.Value && Convert.ToDecimal(cellValue) > 0;

            // Set the HeaderCell style based on the condition
            var headerCellStyle = dataGridView1.Rows[e.RowIndex].HeaderCell.Style;
            headerCellStyle.SelectionBackColor = headerCellStyle.BackColor = mm ? panel2.BackColor : SystemColors.Control;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {


            if (dataGridView1.Rows[e.RowIndex].Selected)
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
            }
        }

        private void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            if (prev_col_idx == dataGridView2.Columns["OP_DATE"].Index)
            {
                dateTimePicker1.Location = new Point(dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.X + 2, dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.Y + 3);
                dateTimePicker1.Visible = prev_rw_idx >= dataGridView2.FirstDisplayedScrollingRowIndex && prev_rw_idx < (dataGridView2.FirstDisplayedScrollingRowIndex + dataGridView2.DisplayedRowCount(false));
            }
            else if ((prev_col_idx == dataGridView2.Columns["DEBIT"].Index || prev_col_idx == dataGridView2.Columns["CREDIT"].Index) && dataGridView2.CurrentCell.ColumnIndex == prev_col_idx)
            {
                numericUpDown1.Location = new Point(dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.X + 2, dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.Y + 3);
                numericUpDown1.Visible = prev_rw_idx >= dataGridView2.FirstDisplayedScrollingRowIndex && prev_rw_idx < (dataGridView2.FirstDisplayedScrollingRowIndex + dataGridView2.DisplayedRowCount(false));
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "DEL_FNC")
            {
                e.Value = e.RowIndex == dataGridView2.NewRowIndex || !monetic_del_autorised ? Properties.Resources.icons8_square_full_25px_1 : Properties.Resources.icons8_trash_25px_1;
            }
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {

            if (prev_col_idx > -1 && prev_rw_idx > -1)
            {
                if (!dataGridView2.Rows[prev_rw_idx].IsNewRow || numericUpDown1.Value != 0)
                {
                    dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx].Value = numericUpDown1.Value;
                }
            }
            numericUpDown1.Visible = false;
        }

        private void numericUpDown1_VisibleChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Visible)
            {
                numericUpDown1.Select();
            }
        }


        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            if (prev_col_idx == 3 && prev_rw_idx > -1)
            {
                if (dateTimePicker1.Location.Y == dataGridView2.Location.Y + dataGridView2.ColumnHeadersHeight + 1)
                {
                    dataGridView2.Rows[0].Cells[3].Value = dateTimePicker1.Value;
                }
                else
                {
                    dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx].Value = dateTimePicker1.Value;
                }
            }

            dateTimePicker1.Visible = false;

        }

        private void dateTimePicker1_VisibleChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Visible)
            {

                dateTimePicker1.Select();

            }
        }

        private void dataGridView2_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {

            if (dataGridView3.Columns.Count >= 3)
            {
                if (dataGridView2.Columns["OBJECT"] != null && dataGridView2.Columns["DEL_FNC"] != null)
                {
                    dataGridView3.Location = new Point(dataGridView2.GetColumnDisplayRectangle(dataGridView2.Columns["OBJECT"].Index, true).Location.X, dataGridView3.Location.Y);
                    dataGridView3.Width = dataGridView2.GetColumnDisplayRectangle(dataGridView2.Columns["DEL_FNC"].Index, true).Location.X - dataGridView2.GetColumnDisplayRectangle(dataGridView2.Columns["OBJECT"].Index, true).Location.X;
                }

                if (dataGridView2.Columns["DEBIT"] != null && dataGridView2.Columns["CREDIT"] != null)
                {
                    dataGridView3.Columns[1].Width = dataGridView2.Columns["DEBIT"].Width;
                    dataGridView3.Columns[2].Width = dataGridView2.Columns["CREDIT"].Width;
                }

            }

        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)// && !dataGridView2.Rows[prev_rw_idx].IsNewRow)
            {
                numericUpDown1.Visible = false;
                dataGridView2.Focus();
                dataGridView2.Rows[dataGridView2.CurrentRow.Index + 1].Cells[prev_col_idx].Selected = true;
                dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.CurrentRow.Index + 1].Cells[prev_col_idx];
                dataGridView2_SelectionChanged(null, null);
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)//) && dataGridView2.Rows[prev_rw_idx].IsNewRow)
            {
                numericUpDown1.Visible = false;
                dataGridView2.Focus();
            }





        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            prev_rw_idx = dataGridView2.CurrentCell != null ? dataGridView2.CurrentCell.RowIndex : 0;
            prev_col_idx = dataGridView2.CurrentCell != null ? dataGridView2.CurrentCell.ColumnIndex : 0;
            if (!dataGridView2.ReadOnly)
            {

                if (prev_col_idx == dataGridView2.Columns["OP_DATE"].Index && !dataGridView2.Rows[prev_rw_idx].IsNewRow)
                {
                    try { dateTimePicker1.Value = dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx].Value != DBNull.Value ? DateTime.Parse(dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx].Value.ToString()) : DateTime.Now; } catch { dateTimePicker1.Value = DateTime.Now; }
                    dataGridView2.CurrentCell.Style.Padding = new Padding(0);
                    dateTimePicker1.Visible = true;
                    dateTimePicker1.Location = new Point(dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.X + 2, dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.Y + 3);
                    dateTimePicker1.Size = dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                    //if (prev_rw_idx == 0) { dataGridView2.BeginEdit(true); }
                    try
                    {
                        dateTimePicker1.Focus();
                    }
                    catch
                    {
                        dataGridView2.CurrentCell = dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx];
                        dateTimePicker1.Location = new Point(dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.X + 2, dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.Y + 3);
                        dateTimePicker1.Size = dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                    }


                }
                else if (prev_col_idx == dataGridView2.Columns["DEBIT"].Index || prev_col_idx == dataGridView2.Columns["CREDIT"].Index)
                {
                    decimal fff = 0;
                    if (dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx].Value != null) { decimal.TryParse(dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx].Value.ToString(), out fff); }
                    numericUpDown1.Value = fff;
                    dataGridView2.CurrentCell.Style.Padding = new Padding(0);
                    numericUpDown1.Visible = true;
                    numericUpDown1.Location = new Point(dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.X + 2, dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.Y + 3);
                    numericUpDown1.Size = dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                    numericUpDown1.Focus();
                    numericUpDown1.Select(0, fff.ToString("N2").Length);
                }
            }


        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !dataGridView2.Rows[dataGridView2.CurrentRow.Index + 1].IsNewRow)
            {
                dateTimePicker1.Visible = false;
                dataGridView2.Focus();
                dataGridView2.Rows[dataGridView2.CurrentRow.Index + 1].Cells[prev_col_idx].Selected = true;
                dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.CurrentRow.Index + 1].Cells[prev_col_idx];
                dataGridView2_SelectionChanged(null, null);
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                dateTimePicker1.Visible = false;
                dataGridView2.Focus();
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].IsNewRow)
            {
                if (prev_col_idx == dataGridView2.Columns["OP_DATE"].Index)
                {
                    try { dateTimePicker1.Value = dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx].Value != DBNull.Value ? DateTime.Parse(dataGridView2.Rows[prev_rw_idx].Cells[prev_col_idx].Value.ToString()) : DateTime.Now; } catch { dateTimePicker1.Value = DateTime.Now; }
                    dataGridView2.CurrentCell.Style.Padding = new Padding(0);
                    dateTimePicker1.Visible = true;
                    dateTimePicker1.Location = new Point(dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.X + 2, dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.Y + 3);
                    dateTimePicker1.Size = dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                    dateTimePicker1.Focus();
                }
            }
        }

        private void Clients_FormClosing(object sender, FormClosingEventArgs e)
        {
            //To update the cell value if is not yet.
            if (dateTimePicker1.Visible)
            {
                dateTimePicker1.Visible = false;
            }

            if (numericUpDown1.Visible)
            {
                numericUpDown1.Visible = false;
            }
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (prev_rw_idx == 0 && prev_col_idx == dataGridView2.Columns["OP_DATE"].Index) //Because the first load of data the selection event not fired yet (for the 1st cell)
            {
                dataGridView2_SelectionChanged(null, null);
            }
            else if (prev_col_idx == dataGridView2.Columns["DEL_FNC"].Index && prev_rw_idx != dataGridView2.NewRowIndex && monetic_del_autorised)
            {
                //if ((dataGridView2.Rows[prev_rw_idx].Cells["FACT_NUM"].Value != DBNull.Value ? dataGridView2.Rows[prev_rw_idx].Cells["FACT_NUM"].Value.ToString() : "").Length > 0)
                //{
                //    MessageBox.Show("Cette ligne ne peut pas être supprimée, car elle concerne les droits sur une facture, pour la supprimer, vous devez supprimer la facture correspondante (Réf: '" + dataGridView2.Rows[e.RowIndex].Cells["FACT_NUM"].Value + "').", "Non possible :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                //}
                //else
                //{
                    if (MessageBox.Show("Êtes-vous sûrs de faire la suppression ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        PreConnection.Excut_Cmd(3, "tb_clients_finance", null, null, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView2.Rows[prev_rw_idx].Cells["IDD_FINANC"].Value });
                        Load_finace_historic();
                        dateTimePicker1.Visible = numericUpDown1.Visible = false;
                        //------------------
                    }
                //}

            }
        }

        private void dataGridView2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["OBJECT"].Index)
            {
                DataGridViewCell cell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                object originalValue = cell.Tag;

                string val1 = e.FormattedValue != DBNull.Value ? (e.FormattedValue != null ? e.FormattedValue : "").ToString() : "";
                string val2 = originalValue != DBNull.Value ? (originalValue != null ? originalValue : "").ToString() : "";
                if (!val1.Equals(val2) && !val2.ToString().IsEmpty())
                {
                    string Obj = dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value.ToString() : "";
                    string fac_num = dataGridView2.Rows[e.RowIndex].Cells["FACT_NUM"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["FACT_NUM"].Value.ToString() : "";
                    if (fac_num.Trim().IsEmpty())
                    {
                        bool ref_exist = false;
                        string found_reff = "";
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            found_reff = row.Cells["FACT_NUM"].Value?.ToString() ?? "";

                            if (Obj.Contains(found_reff) && !found_reff.IsEmpty() && e.RowIndex != row.Index)
                            {
                                ref_exist = true;
                                break;
                            }
                        }
                        if (ref_exist && last_rw_idx_shown_conf_msg != e.RowIndex)
                        {
                            last_rw_idx_shown_conf_msg = e.RowIndex;
                            Client_Finance_Duplicate_Msg form = new Client_Finance_Duplicate_Msg(this, found_reff);
                            form.ShowDialog();
                            if (form.UserChoice.Equals("modify"))
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }


        }
        private void dataGridView2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

            DataGridViewCell cell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Tag = cell.Value;

        }

        private void Clients_SizeChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Visible)
            {
                numericUpDown1.Location = new Point(dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.X + 2, dataGridView2.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location.Y + 3);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {            
            ((TextBox)sender).BackColor = SystemColors.Window;
            label15.Visible = false;
            button5.Visible = ((TextBox)sender).Text.Trim() == string.Empty || label15.Visible;
        }

        private void textBox9_Validated(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(".", "").Replace(" ", "");
            verif_if_déja_exist_ref();           
        }

        private void verif_if_déja_exist_ref()
        {
            if (textBox9.Text.Length > 0 && (Is_New || (!Is_New && dataGridView1.SelectedRows.Count > 0)))
            {
                DataTable tmmmmpp = PreConnection.Load_data("SELECT * FROM tb_clients WHERE REF LIKE '" + textBox9.Text.Replace("'", "''") + "'" + (!Is_New ? " AND ID <> " + dataGridView1.SelectedRows[0].Cells["ID"].Value : "") + ";");
                int cntt = tmmmmpp != null ? tmmmmpp.Rows.Count : 0;

                label15.Visible = cntt > 0;
            }
            else { label15.Visible = false; }
            button5.Visible = textBox9.Text.Trim() == string.Empty || label15.Visible;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int tt = clients.Rows.Cast<DataRow>().Max(Q => Q["REF"].ToString().Length > 0 && Q["REF"].ToString().All(c => char.IsDigit(c)) ? int.Parse(Q["REF"].ToString()) : 0);
            textBox9.Text = (tt + 1).ToString("00000");
            verif_if_déja_exist_ref();
            button5.Visible = false;
        }
    }
}
