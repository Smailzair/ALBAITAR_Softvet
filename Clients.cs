using ALBAITAR_Softvet.Dialogs;
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
        bool can_start_historic_saving = false;
        DataTable chosen_client_from_search;
        bool monetic_not_autorsed = false;
        public Clients(int ID_to_select, int Infos_1_Caiss_2, int Caisse_Id)
        {
            InitializeComponent();
            tabControl1.TabPages.Remove(tabPage1);
            ID_to_selectt = ID_to_select;
            Infoss_1_Caiss_2 = Infos_1_Caiss_2;
            Caisse_Idx = Caisse_Id;
            
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
            int fd = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : 0;
            clients = PreConnection.Load_data_keeping_duplicates("SELECT *,concat(FAMNME,' ',NME) AS FULL_NME FROM tb_clients;");
            dataGridView1.DataSource = clients;
            if (dataGridView1.Rows.Count > fd) { dataGridView1.ClearSelection(); dataGridView1.Rows[fd].Selected = true; }

        }
        private void Load_selected_client_fields()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                label13.Visible = false;
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
                if (tabControl1.TabPages.Count < 2) { tabControl1.TabPages.Add(tabPage1); }
                //----------------------------------------------
                comboBox1.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["SEX"].Value;
                textBox3.Text = (string)dataGridView1.SelectedRows[0].Cells["FAMNME"].Value;
                textBox2.Text = (string)dataGridView1.SelectedRows[0].Cells["NME"].Value;
                textBox4.Text = (string)dataGridView1.SelectedRows[0].Cells["NUM_CNI"].Value;
                textBox5.Text = (string)dataGridView1.SelectedRows[0].Cells["ADRESS"].Value;
                textBox6.Text = (string)dataGridView1.SelectedRows[0].Cells["POSTAL_CODE"].Value;
                textBox7.Text = (string)dataGridView1.SelectedRows[0].Cells["EMAIL"].Value;
                textBox8.Text = (string)dataGridView1.SelectedRows[0].Cells["OBSERVATIONS"].Value;
                comboBox3.Text = (string)dataGridView1.SelectedRows[0].Cells["WILAYA"].Value;
                comboBox2.Text = (string)dataGridView1.SelectedRows[0].Cells["CITY"].Value;
                maskedTextBox1.Text = (string)dataGridView1.SelectedRows[0].Cells["NUM_PHONE"].Value;
                //----------------------------------------------
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
                int cnt = clients.Rows.Cast<DataRow>().Where(zz =>

                zz["FAMNME"].ToString().ToLower().Equals(textBox3.Text.ToLower()) &&
                zz["NME"].ToString().ToLower().Equals(textBox2.Text.ToLower()) &&
                zz["NUM_CNI"].ToString().Equals(textBox4.Text) &&
                (!Is_New && dataGridView1.SelectedRows.Count > 0 ? (int)zz["ID"] != (int)dataGridView1.SelectedRows[0].Cells["ID"].Value : true)

                ).ToList().Count();
                label13.Visible = cnt > 0;
            }
            else { label13.Visible = false; }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            PreConnection.search_filter_datagridview(dataGridView1, textBox1.Text);
        }

        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            if ((!int.TryParse(textBox6.Text.TrimStart().TrimEnd(), out int ff) && textBox6.Text.Length > 0))
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
            textBox2.BackColor = textBox2.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox3.BackColor = textBox3.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox4.BackColor = textBox4.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;

            all_ready &= textBox2.Text.TrimStart().TrimEnd() != string.Empty;
            bool all_ready_01 = all_ready;
            all_ready &= textBox3.Text.TrimStart().TrimEnd() != string.Empty;
            bool all_ready_02 = all_ready;
            all_ready &= textBox4.Text.TrimStart().TrimEnd() != string.Empty;
            bool all_ready_03 = all_ready;
            all_ready &= !label13.Visible;
            bool all_ready_04 = all_ready;
            //-------------
            label12.Visible = !all_ready;
            bool all_ready_05 = all_ready;
            //-------------
            if (all_ready)
            {
                if (Is_New) //INSERT
                {
                    if (insert_autori)
                    {
                        PreConnection.Excut_Cmd(1, "tb_clients", new List<string> { "SEX", "FAMNME", "NME", "NUM_CNI", "ADRESS", "POSTAL_CODE", "CITY", "WILAYA", "NUM_PHONE", "EMAIL", "OBSERVATIONS" }, new List<object> { comboBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text, comboBox2.Text, comboBox3.Text, maskedTextBox1.Text, textBox7.Text, textBox8.Text },null,null,null);
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
                        PreConnection.Excut_Cmd(2, "tb_clients", new List<string> { "SEX", "FAMNME", "NME", "NUM_CNI", "ADRESS", "POSTAL_CODE", "CITY", "WILAYA", "NUM_PHONE", "EMAIL", "OBSERVATIONS" }, new List<object> { comboBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text, comboBox2.Text, comboBox3.Text, maskedTextBox1.Text, textBox7.Text, textBox8.Text }, "ID = @P_ID", new List<string> { "P_ID" },new List<object> { dataGridView1.SelectedRows[0].Cells["ID"].Value });
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
            verif_if_déja_exist_client();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Is_New = false;
            Load_selected_client_fields();
            Load_finace_historic();
            if (monetic_not_autorsed && tabPage1 != null)
            {
                tabControl1.TabPages.Remove(tabPage1);
            }
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
            label13.Visible = false;
            pictureBox1.Image = Properties.Resources.NOUVEAU;
            if (tabControl1.TabPages.Count > 1) { tabControl1.TabPages.Remove(tabPage1); }
            if (!textBox1.Focused) { textBox3.Select(); }
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
                    PreConnection.Excut_Cmd(3, "tb_clients", null, null, "ID IN (@P_ID)", new List<string> { "P_ID" }, new List<object> { fff });
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
                        xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").Replace("00:00:00", "").TrimStart().TrimEnd() : "";
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
                    monetic_not_autorsed = true;
                    tabControl1.TabPages.Remove(tabPage1);
                }
                else
                {
                    button7.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60001" && (Int32)QQ[3] == 1).Count() > 0;//Ajouter
                    dataGridView2.ReadOnly = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60002" && (Int32)QQ[3] == 1).Count() > 0;//Modifier
                    button5.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60003" && (Int32)QQ[3] == 1).Count() > 0;//Supprimer
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

                    button7.PerformClick();
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
            if (!monetic_not_autorsed)
            {
                if (e.KeyCode == Keys.Delete && !dataGridView2.CurrentCell.ReadOnly)
                {
                    dataGridView2.CurrentCell.Value = DBNull.Value;
                }
            }
            
        }

        private void calc_sold()
        {
            if (dataGridView2.Columns["DEBIT"] != null && dataGridView2.Columns["CREDIT"] != null)
            {
                decimal sld = 0;
                sld = dataGridView2.Rows.Cast<DataGridViewRow>().Sum(row => row.Cells["DEBIT"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["DEBIT"].Value) : 0) - dataGridView2.Rows.Cast<DataGridViewRow>().Sum(row => row.Cells["CREDIT"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["CREDIT"].Value) : 0);
                if (sld == 0)
                {
                    label14.Text = "Rien (0.00 DA).";
                }
                else if (sld >= 0)
                {
                    label14.Text = "Il est endetté par (" + sld.ToString("N2") + " DA).";
                }
                else
                {
                    label14.Text = "On lui doit (" + (sld * -1).ToString("N2") + " DA).";
                }
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            calc_sold();
            //---------------------
            if (can_start_historic_saving)
            {
                bool is_update = (int)dataGridView2.Rows[e.RowIndex].Cells["IDD_FINANC"].Value > -1;
                if (is_update) //UPDATE
                {
                    PreConnection.Excut_Cmd(2, "tb_clients_finance", new List<string> { "OP_DATE", "OBJECT", "DEBIT", "CREDIT" }, new List<object> { dataGridView2.Rows[e.RowIndex].Cells["OP_DATE"].Value, dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value, dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value : 0, dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value : 0 }, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView2.Rows[e.RowIndex].Cells["IDD_FINANC"].Value });
                }
                else //INSERT
                {
                    PreConnection.Excut_Cmd(1, "tb_clients_finance", new List<string> { "OP_DATE", "OBJECT", "DEBIT", "CREDIT" }, new List<object> { dataGridView2.Rows[e.RowIndex].Cells["OP_DATE"].Value, dataGridView2.Rows[e.RowIndex].Cells["OBJECT"].Value, dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["DEBIT"].Value : 0, dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value != DBNull.Value ? dataGridView2.Rows[e.RowIndex].Cells["CREDIT"].Value : 0 },null,null,null);
                }
                Load_finace_historic();

            }

        }
        private void Load_finace_historic()
        {
            can_start_historic_saving = false;
            int prev_select_rw = dataGridView2.SelectedCells.Count > 0 ? dataGridView2.SelectedCells[0].RowIndex : -1;
            int prev_select_col = dataGridView2.SelectedCells.Count > 0 ? dataGridView2.SelectedCells[0].ColumnIndex : -1;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_clients_finance WHERE CLIENT_ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + ";");
                dataGridView2.DataSource = dt;
                if (dataGridView2.DisplayedRowCount(false) < dataGridView2.RowCount) { dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows.Count - 1; }
            }
            else
            {
                dataGridView2.Rows.Cast<DataGridViewRow>().ForEach(rr => { if (rr.Index < dataGridView2.NewRowIndex) { dataGridView2.Rows.RemoveAt(rr.Index); } });
            }
            if (dataGridView2.Rows.Count > prev_select_rw && prev_select_rw > -1) { dataGridView2.CurrentCell = dataGridView2.Rows[prev_select_rw].Cells[prev_select_col]; }
            can_start_historic_saving = true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                string ids = "";
                List<int> selected_row_delete_db = new List<int>();
                List<int> selected_row_just_from_dgv = new List<int>();
                dataGridView2.SelectedCells.Cast<DataGridViewCell>().ForEach(cl =>
                {
                    if (dataGridView2.Rows[cl.RowIndex].Cells["IDD_FINANC"].Value != DBNull.Value && (int)dataGridView2.Rows[cl.RowIndex].Cells["IDD_FINANC"].Value > -1 && !selected_row_delete_db.Contains(cl.RowIndex)) { selected_row_delete_db.Add(cl.RowIndex); }
                    else if (!selected_row_just_from_dgv.Contains(cl.RowIndex) && (int)dataGridView2.Rows[cl.RowIndex].Cells["IDD_FINANC"].Value != -1) { selected_row_just_from_dgv.Add(cl.RowIndex); }
                });
                selected_row_delete_db.ForEach(rw => ids += "," + dataGridView2.Rows[rw].Cells["IDD_FINANC"].Value);


                if (ids.Length > 0)
                {
                    ids = ids.Substring(1);
                    PreConnection.Excut_Cmd(3, "tb_clients", null, null, "ID IN (@P_ID)", new List<string> { "P_ID" }, new List<object> { ids });
                    //Load_finace_historic();
                }
                dataGridView2.Rows.Cast<DataGridViewRow>().Where(zz => selected_row_delete_db.Contains(zz.Index) || selected_row_just_from_dgv.Contains(zz.Index)).ToList().ForEach(ff => dataGridView2.Rows.Remove(ff)); ;

            }
            calc_sold();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView2.CellValueChanged -= dataGridView2_CellValueChanged;
            //((DataTable)dataGridView2.DataSource).Rows.Add(-1, dataGridView1.SelectedRows[0].Cells["ID"].Value, DateTime.Now);
            ((DataTable)dataGridView2.DataSource).Rows.Add();
            if (dataGridView2.DisplayedRowCount(false) < dataGridView2.RowCount) { dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows.Count - 1; }
            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells["OP_DATE"].Value = DateTime.Now;
            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells["IDD_FINANC"].Value = -1;
            dataGridView2.ClearSelection();
            dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells["OBJECT"];
            dataGridView2.BeginEdit(true);
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false; // don't throw an exception
        }

        private void dataGridView2_DataSourceChanged(object sender, EventArgs e)
        {
            calc_sold();
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
                    button7.PerformClick();
                    Caisse_Idx = -1;
                }
            }
            else if (ID_to_selectt == -2) //NEW
            {
                ID_to_selectt = -1;
                button3.PerformClick();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            chosen_client_from_search = new DataTable();
            Clients_List_Search select = new Clients_List_Search();
            select.DataTableReturned += ChildForm_DataTableReturned2;
            select.ShowDialog();
            if (chosen_client_from_search != null)
            {
                textBox1.Clear();
                comboBox2.SelectedValue = chosen_client_from_search.Rows[0][1];
                dataGridView1.ClearSelection();
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(c => c.Cells["ID"].Value.ToString() == chosen_client_from_search.Rows[0]["ID"].ToString()).ForEach(dx => dx.Selected = true);
            }
        }
        private void ChildForm_DataTableReturned2(object sender, DataTableEventArgs_Clients e)
        {
            chosen_client_from_search = e.DataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PreConnection.Excport_to_excel(dataGridView2, "Historique monétique", dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value != DBNull.Value ? "Monét." : dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value.ToString(), null,false);
        }
    }
}
