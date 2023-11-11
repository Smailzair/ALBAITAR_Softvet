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
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using Excc = Microsoft.Office.Interop.Excel;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Vente : Form
    {
        static public int to_select_idx = -1;
        static public DataGridViewRow selected_item = null;
        static public DataTable stock_to_modify = new DataTable();
        static public List<int> visite_to_update_fact_num = new List<int>();
        static public List<string> labos_to_update_fact_num = new List<string>();
        //-------------
        decimal TVA_percent = 9;
        decimal tot_ht = 0;
        //------------
        DataTable clients;
        DataTable factures;
        DataTable facture_to_print;
        DataTable Selected_facture_old_infos;
        DataTable users;
        bool Is_New = true;
        bool Transf_also_caisse = false;
        static public int tmp_current_client_id = -1;
        bool has_asked_for_Transf_also_caisse = false;

        public Vente(int to_select_id)
        {
            InitializeComponent();
            to_select_idx = to_select_id;
            //------------------------
            users = PreConnection.Load_data("SELECT ID,CONCAT(IF(SEX = 'F','Mme. ','Mr. '),`USER_NME`,' ',`USER_FAMNME`) AS USER_FULL_NME FROM tb_login_and_users;");
            comboBox3.DataSource = users;
            comboBox3.DisplayMember = "USER_FULL_NME";
            comboBox3.ValueMember = "ID";
            comboBox3.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
            comboBox3.SelectedValue = users.AsEnumerable().Where(F => (string)F["USER_FULL_NME"] == Properties.Settings.Default.Last_login_user_full_nme).First()["ID"];
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            //-----------------------
            clients = PreConnection.Load_data("SELECT *,CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS FULL_NME FROM tb_clients;");
            comboBox1.DataSource = comboBox2.DataSource = clients;
            comboBox1.DisplayMember = comboBox2.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = comboBox2.ValueMember = "ID";
            //----------------------
            if (stock_to_modify.Columns.Count == 0)
            {
                stock_to_modify.Columns.Add("PROD_ID", typeof(int));
                stock_to_modify.Columns.Add("PROD_CODE", typeof(string));
                stock_to_modify.Columns.Add("QNT_DIMIN", typeof(decimal));
            }
            //----------------------------
            selected_item = new DataGridViewRow();
            //--------------------------
            dataGridView3.Rows.Add(new DataGridViewRow());
            dataGridView3.Rows.Add(new DataGridViewRow());
            dataGridView3.Rows.Add(new DataGridViewRow());
            dataGridView3.Rows.Add(new DataGridViewRow());
            dataGridView3.Rows[3].DefaultCellStyle.Font = new Font(dataGridView3.Font.FontFamily, dataGridView3.Font.Size, FontStyle.Bold);
            dataGridView3.Rows[3].DefaultCellStyle.ForeColor = Color.White;
            dataGridView3.Rows[3].DefaultCellStyle.BackColor = Color.Green;
            dataGridView3.Rows[0].Height = dataGridView3.Rows[1].Height = dataGridView3.Rows[2].Height = dataGridView3.Rows[3].Height = 30;
            dataGridView3.Rows[0].Cells[0].Value = "Total HT :";
            dataGridView3.Rows[1].Cells[0].Value = "TVA :";
            dataGridView3.Rows[2].Cells[0].Value = "--";
            dataGridView3.Rows[3].Cells[0].Value = "Total TTC :";
            //------------------------
            foreach (Control c in groupBox2.Controls)
            {
                if (c is CheckBox)
                {
                    ((CheckBox)c).CheckedChanged += c_ControlChanged;
                }
                else
                {
                    c.TextChanged += new EventHandler(c_ControlChanged);
                }
            }
            numericUpDown2.ValueChanged += new EventHandler(c_ControlChanged);
            //----------------------
            facture_to_print = new DataTable();
            facture_to_print.Columns.Add("Type", typeof(string));
            facture_to_print.Columns.Add("Item", typeof(string));
            facture_to_print.Columns.Add("Qnt", typeof(decimal));
            facture_to_print.Columns.Add("Unit", typeof(decimal));
            facture_to_print.Columns.Add("Tot", typeof(decimal));
        }

        private void button1_Click(object sender, EventArgs e)
        {

            tmp_current_client_id = comboBox1.SelectedValue != null ? (comboBox1.SelectedValue != DBNull.Value ? (int)comboBox1.SelectedValue : -1) : -1;
            selected_item = new DataGridViewRow();
            //------------------------
            new Add_Vente_Fact_Item(comboBox1.Text).ShowDialog();
            //----------------------
            if (selected_item != null && selected_item.Cells.Count > 0)
            {
                dataGridView2.Rows.Add(selected_item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Is_New = true;
            Transf_also_caisse = false;
            has_asked_for_Transf_also_caisse = false;
            pictureBox2.Image = Properties.Resources.NOUVEAU_003;
            stock_to_modify.Rows.Clear();
            selected_item = new DataGridViewRow();
            TVA_percent = decimal.Parse(Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 5).Select(QQ => QQ["VAL"]).First().ToString());
            //----------
            dataGridView3.Rows[1].Cells[0].Value = "TVA (" + TVA_percent.ToString("N2") + " %):";
            dataGridView3.Rows[2].Cells[0].Value = "--";
            dataGridView3.Rows[0].Cells[1].Value = 0.00;
            dataGridView3.Rows[1].Cells[1].Value = 0.00;
            dataGridView3.Rows[2].Cells[1].Value = 0.00;
            dataGridView3.Rows[3].Cells[1].Value = 0.00;
            //----------
            label3.Visible = false;
            comboBox1.BackColor = SystemColors.Window;
            comboBox1.Text = "";
            comboBox1.SelectedValue = DBNull.Value;
            dateTimePicker1.Value = dateTimePicker2.Value = DateTime.Now;
            visite_to_update_fact_num = new List<int>();
            labos_to_update_fact_num = new List<string>();
            int dds = 0;
            if (factures != null)
            {
                if (factures.Rows.Count > 0)
                {
                    dds = factures.AsEnumerable()
                                  .Where(row => row["REF"].ToString().Substring(3, 4) == DateTime.Now.Year.ToString())
                                  .Max(row => int.Parse(row["REF"].ToString().Substring(8)));
                }
            }
            numericUpDown1.Value = dds + 1;
            dataGridView2.Rows.Clear();
            checkBox1.Checked = Properties.Settings.Default.Faire_reg_espece_facture_vente;
            numericUpDown2.Value = 0;
            button5.Visible = false;
            //---------
            if (comboBox1.SelectedValue != null)
            {
                groupBox3.Enabled = int.TryParse(comboBox1.SelectedValue.ToString(), out int yy) ? (clients.Rows.Cast<DataRow>().Where(ww => (int)ww["ID"] == (int)comboBox1.SelectedValue && ww["FULL_NME"].ToString() == comboBox1.Text).ToList().Count > 0 ? true : false) : false;
            }
            else
            {
                groupBox3.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sur de faire la suppression ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool fff = true;
                int yy = dataGridView2.SelectedRows.Cast<DataGridViewRow>().Where(zz => (string)zz.Cells["TYPE"].Value == "Produit" && zz.Cells["PRODUCT_CODE"].Value != DBNull.Value).ToList().Count();
                if (!Is_New && yy > 0) { fff = MessageBox.Show("Retourner la quantité au stock ?\n\n(Excepte les produits connus dans la base donné)", "Stock :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes; }
                foreach (DataGridViewRow rwx in dataGridView2.SelectedRows)
                {
                    if (fff && (string)rwx.Cells["TYPE"].Value == "Produit" && rwx.Cells["PRODUCT_CODE"].Value != DBNull.Value)
                    {
                        if (rwx.Cells["PRODUCT_CODE"].Value.ToString().Trim().Length > 0)
                        {
                            DataRow rw = Vente.stock_to_modify.NewRow();
                            rw["PROD_ID"] = DBNull.Value;
                            rw["PROD_CODE"] = rwx.Cells["PRODUCT_CODE"].Value;
                            rw["QNT_DIMIN"] = (decimal)rwx.Cells["QNT2"].Value * -1;
                            Vente.stock_to_modify.Rows.Add(rw);
                            //stock_to_modify.Rows.Cast<DataRow>().Where(x => x["PROD_CODE"].ToString() == rwx.Cells["PRODUCT_CODE"].Value.ToString()).ToList().ForEach(x => x.Delete());
                        }
                    }
                    if ((string)rwx.Cells["TYPE"].Value == "Service" && rwx.Cells["PRODUCT_CODE"].Value != DBNull.Value)
                    {
                        int dds = -1;
                        if (int.TryParse(rwx.Cells["PRODUCT_CODE"].Value.ToString(), out dds)) //VISIT
                        {
                            visite_to_update_fact_num.Remove(dds);
                        }
                        else //LABO (ANALYSE)
                        {
                            labos_to_update_fact_num.Remove((string)rwx.Cells["PRODUCT_CODE"].Value);
                        }
                    }
                    dataGridView2.Rows.Remove(rwx);
                }
                calcul_bill_tot();
            }

        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            calcul_bill_tot();
        }


        private void calcul_bill_tot()
        {
            tot_ht = 0;
            foreach (DataGridViewRow rw in dataGridView2.Rows)
            {
                tot_ht += rw.Cells["SLD"].Value != DBNull.Value ? (decimal)rw.Cells["SLD"].Value : 0;
            }
            dataGridView3.Rows[0].Cells[1].Value = tot_ht; //HT
            //-------------------------------
            dataGridView3.Rows[1].Cells[1].Value = tot_ht * TVA_percent / 100; //TVA
            //------------------------------
            if (checkBox1.Checked)
            {
                decimal ttc01 = (decimal)dataGridView3.Rows[0].Cells[1].Value + (decimal)dataGridView3.Rows[1].Cells[1].Value;
                if (ttc01 >= 20)
                {
                    decimal mnt_ttmp = ttc01 * 1 / 100;
                    dataGridView3.Rows[2].Cells[1].Value = mnt_ttmp > 2500 ? 2500 : (mnt_ttmp < 5 ? 5 : mnt_ttmp); //D.Timbre
                }
                else
                {
                    dataGridView3.Rows[2].Cells[1].Value = 0;
                }

            }
            else
            {
                dataGridView3.Rows[2].Cells[1].Value = 0;
            }
            //-----------------------------            
            dataGridView3.Rows[3].Cells[1].Value = (decimal)dataGridView3.Rows[0].Cells[1].Value + (decimal)dataGridView3.Rows[1].Cells[1].Value + (checkBox1.Checked ? decimal.Parse(dataGridView3.Rows[2].Cells[1].Value.ToString()) : 0); //TTC
            //---------
            button5.Visible = true;

        }

        private void Vente_Load(object sender, EventArgs e)
        {
            dateTimePicker4.MinDate = DateTime.Now;
            //----------
            load_factures();
        }

        private void load_factures()
        {
            int prev_idx = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : -1;
            //factures = PreConnection.Load_data("SELECT `ID`,`DATE`,`CLIENT_ID`,`CLIENT_FULL_NME`,`REF`,`TVA_PERC`,`DROIT_TIMBRE`,`TOTAL_HT`,`TOTAL_TTC`,`LAST_MODIF_BY` FROM tb_factures_vente;");
            factures = PreConnection.Load_data("SELECT tb1.`ID`,tb1.`DATE`,tb1.`CLIENT_ID`,tb1.`CLIENT_FULL_NME`,tb1.`REF`,tb1.`TVA_PERC`,tb1.`DROIT_TIMBRE`,tb1.`TOTAL_HT`,tb1.`TOTAL_TTC`,tb1.`LAST_MODIF_BY`,tb2.`SLD` FROM tb_factures_vente tb1 LEFT JOIN (SELECT `FACT_NUM`,SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)) AS SLD,CLIENT_ID FROM tb_clients_finance GROUP BY FACT_NUM,CLIENT_ID) tb2 ON tb1.REF = tb2.FACT_NUM AND tb1.CLIENT_ID = tb2.CLIENT_ID;");
            dataGridView1.DataSource = factures;
            factures_filtr();
            if (to_select_idx > -1)
            {
                dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(Q => (int)Q.Cells["ID"].Value == to_select_idx).ToList().ForEach(W =>
                {
                    W.Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = W.Index;
                });
                to_select_idx = -1;
            }
            else if (to_select_idx == 2)
            {
                button3.PerformClick();
                to_select_idx = -1;
            }
            else if (prev_idx > -1 && dataGridView1.Rows.Count > prev_idx)
            {

                dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                dataGridView1.Rows[prev_idx].Selected = true;


            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView3.Rows[2].Cells[0].Value = checkBox1.Checked ? "Droit de timbre (1 %) :" : "--";
            if (dataGridView3.Rows[0].Cells[1].Value != null)
            {
                if (decimal.TryParse(dataGridView3.Rows[0].Cells[1].Value.ToString(), out decimal dd))
                {
                    calcul_bill_tot();
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string msg_txt = "";
            bool All_Ready = true;
            //--------
            All_Ready &= dataGridView2.Rows.Count > 0;
            msg_txt += dataGridView2.Rows.Count == 0 ? "\n- Aucun élement dans la facture." : "";
            //------------
            All_Ready &= dataGridView2.Rows.Count < 71;
            msg_txt += dataGridView2.Rows.Count > 70 ? "\n- Le nombre d'élements de la facture ne doit dépasse 70 élemnt." : "";
            //------------
            All_Ready &= !label3.Visible;
            msg_txt += label3.Visible ? "\n- Référence déja utilisée précedemment." : "";
            //---------------
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                All_Ready = false;
                comboBox1.BackColor = Color.LightCoral;
                msg_txt += "\n- Veuillez séléctionner (ou saisir) le client concerné.";
            }
            else
            {
                var tt = clients.AsEnumerable().Where(x => (x["FULL_NME"] != DBNull.Value ? (string)x["FULL_NME"] : "") == comboBox1.Text && (x["ID"] != DBNull.Value ? (int)x["ID"] : -999) == (int)comboBox1.SelectedValue);
                if (!tt.Any())
                {
                    All_Ready = false;
                    comboBox1.BackColor = Color.LightCoral;
                    msg_txt += "\n- Le client saisi n'est pas encore enregistré (ou a été supprimé), veuillez l'ajouter (via le bouton à côté) et réessayer.";
                }
            }
            //-----------------
            if (All_Ready)
            {
                if (Is_New) //INSERT
                {
                    List<string> Columns = new List<string> {
                        "DATE",
                        "CLIENT_ID",
                        "CLIENT_FULL_NME",
                        "REF",
                        "TVA_PERC",
                        "DROIT_TIMBRE",
                        "TOTAL_HT",
                        "TOTAL_TTC",
                        "LAST_MODIF_BY"
                    };
                    List<object> Columns_vals = new List<object> {
                        dateTimePicker1.Value, //DATE
                        comboBox1.SelectedValue != null ? (clients.Rows.Cast<DataRow>().Where(ww => (int)ww["ID"] == (int)comboBox1.SelectedValue && ww["FULL_NME"].ToString() == comboBox1.Text).ToList().Count > 0 ? comboBox1.SelectedValue : DBNull.Value) : DBNull.Value, //CLIENT_ID
                        comboBox1.Text, //CLIENT_FULL_NME
                        "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text.Replace("'", "''"), //REF
                        dataGridView3.Rows[1].Cells[1].Value, //TVA_PERC
                        dataGridView3.Rows[2].Cells[1].Value, //DROIT_TIMBRE
                        dataGridView3.Rows[0].Cells[1].Value, //TOTAL_HT
                        dataGridView3.Rows[3].Cells[1].Value, //TOTAL_TTC
                        Properties.Settings.Default.Last_login_user_full_nme //LAST_MODIF_BY
                    };

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {

                        Columns.Add("ITEM_NME_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));
                        Columns.Add("ITEM_QNT_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));
                        Columns.Add("ITEM_IS_PROD_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));
                        Columns.Add("ITEM_PROD_CODE_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));
                        Columns.Add("ITEM_PRIX_UNIT_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));

                        Columns_vals.Add(row.Cells["ITEM_NME"].Value);
                        Columns_vals.Add(row.Cells["QNT2"].Value);
                        Columns_vals.Add(row.Cells["TYPE"].Value.ToString() == "Produit" ? 1 : 0);
                        Columns_vals.Add(row.Cells["PRODUCT_CODE"].Value != DBNull.Value ? (row.Cells["PRODUCT_CODE"].Value.ToString().Trim().Length > 0 ? row.Cells["PRODUCT_CODE"].Value : DBNull.Value) : DBNull.Value);
                        Columns_vals.Add(row.Cells["PRIX_UNIT"].Value);
                    }

                    PreConnection.Excut_Cmd(1, "tb_factures_vente", Columns, Columns_vals, null, null, null);

                    //-------Caisse (INSERT)----------                    
                    if (groupBox3.Enabled)
                    {
                        PreConnection.Excut_Cmd(1, "tb_clients_finance", new List<string> { "CLIENT_ID", "OP_DATE", "OBJECT", "DEBIT", "CREDIT", "FACT_NUM" }, new List<object>
                        {
                            comboBox1.SelectedValue,
                                          dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                                          "Droits de facture [FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text,
                                          dataGridView3.Rows[3].Cells[1].Value,
                                          Convert.ToDouble(numericUpDown2.Value),
                                          "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text
                        }, null, null, null);
                    }
                    //---------------------------
                }
                else //UPDATE
                {

                    //if (stock_to_modify != null)
                    // {

                    string inti = "";
                    for (int f = 1; f < 71; f++)
                    {
                        inti += ",`ITEM_NME_" + (f < 10 ? "0" : "") + f + "` = @NULL";
                        inti += ",`ITEM_QNT_" + (f < 10 ? "0" : "") + f + "` = @NULL";
                        inti += ",`ITEM_IS_PROD_" + (f < 10 ? "0" : "") + f + "` = @NULL";
                        inti += ",`ITEM_PROD_CODE_" + (f < 10 ? "0" : "") + f + "` = @NULL";
                        inti += ",`ITEM_PRIX_UNIT_" + (f < 10 ? "0" : "") + f + "` = @NULL";
                    }

                    PreConnection.Excut_Cmd_personnel("UPDATE `tb_factures_vente` SET " + inti.Substring(1) + " WHERE `ID` = @ID", new List<string> { "@NULL", "@ID" }, new List<object> { DBNull.Value, dataGridView1.SelectedRows[0].Cells["ID"].Value });
                    // }
                    //------------
                    List<string> Columns = new List<string> {
                        "DATE",
                        "CLIENT_ID",
                        "CLIENT_FULL_NME",
                        "REF",
                        "TVA_PERC",
                        "DROIT_TIMBRE",
                        "TOTAL_HT",
                        "TOTAL_TTC",
                        "LAST_MODIF_BY"
                    };
                    List<object> Columns_vals = new List<object> {
                        dateTimePicker1.Value, //DATE
                        comboBox1.SelectedValue != null ? (clients.Rows.Cast<DataRow>().Where(ww => (int)ww["ID"] == (int)comboBox1.SelectedValue && ww["FULL_NME"].ToString() == comboBox1.Text).ToList().Count > 0 ? comboBox1.SelectedValue : DBNull.Value) : DBNull.Value, //CLIENT_ID
                        comboBox1.Text, //CLIENT_FULL_NME
                        "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text.Replace("'", "''"), //REF
                        dataGridView3.Rows[1].Cells[1].Value, //TVA_PERC
                        dataGridView3.Rows[2].Cells[1].Value, //DROIT_TIMBRE
                        dataGridView3.Rows[0].Cells[1].Value, //TOTAL_HT
                        dataGridView3.Rows[3].Cells[1].Value, //TOTAL_TTC
                        Properties.Settings.Default.Last_login_user_full_nme //LAST_MODIF_BY
                    };

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {

                        Columns.Add("ITEM_NME_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));
                        Columns.Add("ITEM_QNT_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));
                        Columns.Add("ITEM_IS_PROD_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));
                        Columns.Add("ITEM_PROD_CODE_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));
                        Columns.Add("ITEM_PRIX_UNIT_" + (row.Index < 9 ? "0" : "") + (row.Index + 1));

                        Columns_vals.Add(row.Cells["ITEM_NME"].Value);
                        Columns_vals.Add(row.Cells["QNT2"].Value);
                        Columns_vals.Add(row.Cells["TYPE"].Value.ToString() == "Produit" ? 1 : 0);
                        Columns_vals.Add(row.Cells["PRODUCT_CODE"].Value != DBNull.Value ? (row.Cells["PRODUCT_CODE"].Value.ToString().Trim().Length > 0 ? row.Cells["PRODUCT_CODE"].Value : DBNull.Value) : DBNull.Value);
                        Columns_vals.Add(row.Cells["PRIX_UNIT"].Value);
                    }

                    PreConnection.Excut_Cmd(2, "tb_factures_vente", Columns, Columns_vals, "ID = @P_ID", new List<string> { "@P_ID" }, new List<object> { dataGridView1.SelectedRows[0].Cells["ID"].Value });
                    //-------Caisse (UPDATE)----------
                    //RMQ : il y a un trigger MySQL pour modifier quelques infos.
                    bool ttm = comboBox1.SelectedValue == dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value;
                    bool ttm2 = !ttm ? comboBox1.Text == (dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value != DBNull.Value ? (string)dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value : "") : ttm;
                    int cb1_selected_value = comboBox1.SelectedValue == null ? -1 : (comboBox1.SelectedValue == DBNull.Value ? -1 : (((comboBox1.SelectedValue == dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value && comboBox1.Text == dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value.ToString()) || (clients.AsEnumerable().Where(row => row.Field<int>("ID") == (int)comboBox1.SelectedValue && row.Field<string>("FULL_NME") == comboBox1.Text).Count() > 0)) ? (int)comboBox1.SelectedValue : -1));
                    int old_clt_id = dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value == DBNull.Value ? -1 : (int)dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value;
                    if (!ttm2) //Client était changé
                    {
                        if (Transf_also_caisse)
                        {
                            if (old_clt_id > -1)
                            {
                                if (cb1_selected_value > -1)
                                {
                                    PreConnection.Excut_Cmd(2, "tb_clients_finance", new List<string>
                                    {
                                        "OP_DATE",
                                                    "CLIENT_ID",
                                                    "DEBIT",
                                                    "CREDIT"
                                    }, new List<object>
                                    {
                                        dateTimePicker1.Value,
                                                    cb1_selected_value,
                                                    dataGridView3.Rows[3].Cells[1].Value,
                                                    Convert.ToDouble(numericUpDown2.Value)
                                    }, "FACT_NUM LIKE @FCTT", new List<string> { "@FCTT" }, new List<object> { "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text });
                                }
                                else if (cb1_selected_value == -1)
                                {
                                    PreConnection.Excut_Cmd(3, "tb_clients_finance", null, null, "FACT_NUM LIKE @FACTTT", new List<string> { "@FACTTT" }, new List<object> { "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text });
                                }
                            }
                            else
                            {
                                if (cb1_selected_value > -1)
                                {
                                    PreConnection.Excut_Cmd(1, "tb_clients_finance", new List<string>
                                    {
                                        "CLIENT_ID",
                                        "OP_DATE",
                                        "OBJECT",
                                        "DEBIT",
                                        "CREDIT",
                                        "FACT_NUM"
                                    }, new List<object>
                                    {
                                        cb1_selected_value,
                                        dateTimePicker1.Value,
                                        "Droits de facture [FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "]",
                                        dataGridView3.Rows[3].Cells[1].Value,
                                        Convert.ToDouble(numericUpDown2.Value),
                                        "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text
                                    }, null, null, null);
                                }
                            }
                        }
                        else
                        {
                            if (old_clt_id > -1)
                            {
                                PreConnection.Excut_Cmd_personnel("UPDATE tb_clients_finance SET " +
                                                    "`OBJECT`=CONCAT(`OBJECT`,'(Annulé)')," +
                                                    "`DEBIT`=0," +
                                                    "`FACT_NUM`=NULL" +
                                                    " WHERE `FACT_NUM` LIKE 'FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text.Replace("'", "''") + "';", null, null);
                            }

                            if (cb1_selected_value > -1)
                            {
                                PreConnection.Excut_Cmd(1, "tb_clients_finance", new List<string>
                                    {
                                        "CLIENT_ID",
                                        "OP_DATE",
                                        "OBJECT",
                                        "DEBIT",
                                        "CREDIT",
                                        "FACT_NUM"
                                    }, new List<object>
                                    {
                                        cb1_selected_value,
                                        dateTimePicker1.Value,
                                        "Droits de facture [FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "]",
                                        dataGridView3.Rows[3].Cells[1].Value,
                                        Convert.ToDouble(numericUpDown2.Value),
                                        "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text
                                    }, null, null, null);
                            }
                        }
                    }
                    else
                    {
                        PreConnection.Excut_Cmd(3, "tb_clients_finance", null, null, "FACT_NUM LIKE @FFCT", new List<string> { "@FFCT" }, new List<object> { "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text });
                        if (cb1_selected_value > -1)
                        {
                            PreConnection.Excut_Cmd(1, "tb_clients_finance", new List<string>
                                    {
                                        "CLIENT_ID",
                                        "OP_DATE",
                                        "OBJECT",
                                        "DEBIT",
                                        "CREDIT",
                                        "FACT_NUM"
                                    }, new List<object>
                                    {
                                        cb1_selected_value,
                                        dateTimePicker1.Value,
                                        "Droits de facture [FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "]",
                                        dataGridView3.Rows[3].Cells[1].Value,
                                        Convert.ToDouble(numericUpDown2.Value),
                                        "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text
                                    }, null, null, null);
                        }
                    }
                    //---------------------------
                }
                //-------
                if (stock_to_modify != null)
                {
                    var groupedRows = from row in stock_to_modify.AsEnumerable()
                                      group row by row.Field<string>("PROD_CODE") into grp
                                      select grp;

                    foreach (var group in groupedRows)
                    {
                        decimal sumSLD = group.Sum(row => row.Field<decimal>("QNT_DIMIN"));
                        if (sumSLD != 0)
                        {
                            PreConnection.Excut_Cmd_personnel("INSERT INTO `tb_stock_mouv`"
                                            + "(`OP_DATE`,"
                                            + "`PROD_ID`,"
                                            + "`QNT_IN`,"
                                            + "`QNT_OUT`,"
                                            + "`OBSERV`)"
                                            + " SELECT "
                                            + "@OP_DATE,"//OP_DATE
                                            + "(SELECT `ID` FROM tb_produits WHERE `CODE` LIKE @PROD_CODE LIMIT 1),"//PROD_ID
                                            + (sumSLD < 0 ? sumSLD * -1 : 0) + ","//QNT_IN
                                            + (sumSLD > 0 ? sumSLD : 0) + "," //QNT_OUT
                                            + "'Vente (Facture [" + ("FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text.Replace("'", "''")) + "]) -" + (Is_New ? "Crée" : "Modifiée") + "-' "  //OBSERV
                                            + "WHERE (SELECT COUNT(`CODE`) FROM tb_produits WHERE `CODE` LIKE @PROD_CODE) > 0;"
                                            , new List<string> { "@OP_DATE", "@PROD_CODE" }, new List<object> { dateTimePicker1.Value, group.First()["PROD_CODE"] });
                        }
                    }
                }
                //-------------
                load_factures();
            }
            else if (msg_txt.Length > 0)
            {
                MessageBox.Show(msg_txt, "Vérifiez s'il vous plaît:", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = numericUpDown1.Value.ToString("0000");
            verif_ref_exist();
        }

        private void verif_ref_exist()
        {
            string reff = "FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text;
            int cnt = Is_New ? factures.AsEnumerable().Count(rw => rw["REF"].ToString() == reff) : factures.AsEnumerable().Count(rw => rw["REF"].ToString() == reff && rw["ID"].ToString() != dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());
            label3.Text = "Existe déja !\n(" + reff + ")";
            label3.Visible = cnt > 0;
        }
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            int dd = 1;
            if (int.TryParse(textBox2.Text, out dd))
            {
                if (dd <= 9999)
                {
                    numericUpDown1.Value = dd;
                    verif_ref_exist();
                }
                else
                {
                    MessageBox.Show("N° de facture ne doit superieur à 9999.", "Ref. de facture :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    numericUpDown1_ValueChanged(null, null);
                }
            }
            else
            {
                MessageBox.Show("La Ref. ne doit pas vide.\nEt les 4 derniers chiffres de Ref. de facture n'accepte que les nombres.", "Ref. de facture :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numericUpDown1_ValueChanged(null, null);
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            verif_ref_exist();
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
            if (!Is_New && numericUpDown2.Value != 0 && !has_asked_for_Transf_also_caisse)
            {
                has_asked_for_Transf_also_caisse = true;
                Transf_also_caisse = MessageBox.Show("Voulez-vous rendre le montant " + ((decimal)Selected_facture_old_infos.Rows[0]["SLD_REG_FAC"]).ToString("N2") + " DA au " + dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value + " -qu'il a payé precedement- ?", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            }
            if (comboBox1.SelectedValue != null)
            {
                groupBox3.Enabled = int.TryParse(comboBox1.SelectedValue.ToString(), out int yy) ? (clients.Rows.Cast<DataRow>().Where(ww => (int)ww["ID"] == (int)comboBox1.SelectedValue && ww["FULL_NME"].ToString() == comboBox1.Text).ToList().Count > 0 ? true : false) : false;
            }
            else
            {
                groupBox3.Enabled = false;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            decimal sum = 0;
            if (dataGridView1.SelectedRows.Count > 1)
            {
                sum = dataGridView1.SelectedRows.Cast<DataGridViewRow>()
                  .Sum(row => Convert.ToDecimal(row.Cells["TOTAL_TTC"].Value));
                label21.Text = "Séléctionnée (" + dataGridView1.SelectedRows.Count + ") : ";
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                sum = dataGridView1.Rows.Cast<DataGridViewRow>()
              .Sum(row => Convert.ToDecimal(row.Cells["TOTAL_TTC"].Value));
                label21.Text = "Total : ";
            }
            label21.Text += sum.ToString("# ##0.00");
            //----------------------
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Is_New = false;

                pictureBox2.Image = Properties.Resources.MODIF_003;
                stock_to_modify.Rows.Clear();
                selected_item = new DataGridViewRow();
                label3.Visible = false;
                dataGridView2.Rows.Clear();
                button5.Visible = false;
                numericUpDown2.Value = 0;
                Transf_also_caisse = false;
                has_asked_for_Transf_also_caisse = false;
                visite_to_update_fact_num = new List<int>();
                labos_to_update_fact_num = new List<string>();
                comboBox1.SelectedValue = DBNull.Value;
                //-------------------
                Selected_facture_old_infos = PreConnection.Load_data("SELECT tb1.*,tb2.SLD_REG_FAC FROM tb_factures_vente tb1 LEFT JOIN (SELECT `FACT_NUM`,`CREDIT` AS SLD_REG_FAC FROM tb_clients_finance) tb2 ON tb2.`FACT_NUM` LIKE tb1.`REF` WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + ";");
                if (Selected_facture_old_infos != null)
                {
                    if (Selected_facture_old_infos.Rows.Count > 0)
                    {
                        int ref_year = int.Parse(Selected_facture_old_infos.Rows[0]["REF"].ToString().Substring(3, 4));
                        int ref_num = int.Parse(Selected_facture_old_infos.Rows[0]["REF"].ToString().Substring(8));
                        numericUpDown1.Value = ref_num;
                        dateTimePicker2.Value = new DateTime(ref_year, 1, 1);
                        dateTimePicker1.Value = (DateTime)Selected_facture_old_infos.Rows[0]["DATE"];
                        if (Selected_facture_old_infos.Rows[0]["CLIENT_ID"] != DBNull.Value)
                        {
                            comboBox1.SelectedValue = (int)Selected_facture_old_infos.Rows[0]["CLIENT_ID"];
                        }
                        else
                        {
                            comboBox1.Text = Selected_facture_old_infos.Rows[0]["CLIENT_FULL_NME"].ToString();
                        }
                        checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
                        checkBox1.Checked = Selected_facture_old_infos.Rows[0]["DROIT_TIMBRE"] != DBNull.Value ? ((decimal)Selected_facture_old_infos.Rows[0]["DROIT_TIMBRE"] > 0) : false;
                        checkBox1.CheckedChanged += checkBox1_CheckedChanged;
                        numericUpDown2.Value = Selected_facture_old_infos.Rows[0]["SLD_REG_FAC"] != DBNull.Value ? (decimal)Selected_facture_old_infos.Rows[0]["SLD_REG_FAC"] : 0;
                        //-----------------
                        for (int f = 1; f < 71; f++)
                        {
                            if (Selected_facture_old_infos.Rows[0]["ITEM_NME_" + (f < 10 ? "0" : "") + f] != DBNull.Value)
                            {
                                if (Selected_facture_old_infos.Rows[0]["ITEM_NME_" + (f < 10 ? "0" : "") + f].ToString().Length > 0)
                                {
                                    DataGridViewRow row = new DataGridViewRow();
                                    row.CreateCells(dataGridView2);

                                    row.Cells[0].Value = Selected_facture_old_infos.Rows[0]["ITEM_IS_PROD_" + (f < 10 ? "0" : "") + f] != DBNull.Value ? ((SByte)Selected_facture_old_infos.Rows[0]["ITEM_IS_PROD_" + (f < 10 ? "0" : "") + f] == 1 ? "Produit" : "Service") : "Service";
                                    row.Cells[1].Value = Selected_facture_old_infos.Rows[0]["ITEM_NME_" + (f < 10 ? "0" : "") + f];
                                    row.Cells[2].Value = Selected_facture_old_infos.Rows[0]["ITEM_QNT_" + (f < 10 ? "0" : "") + f];
                                    row.Cells[3].Value = Selected_facture_old_infos.Rows[0]["ITEM_PRIX_UNIT_" + (f < 10 ? "0" : "") + f];
                                    row.Cells[4].Value = (decimal)row.Cells[2].Value * (decimal)row.Cells[3].Value;
                                    row.Cells[5].Value = Selected_facture_old_infos.Rows[0]["ITEM_PROD_CODE_" + (f < 10 ? "0" : "") + f];

                                    dataGridView2.Rows.Add(row);
                                }
                            }
                        }
                        //--------------
                        button5.Visible = false;
                    }
                    else
                    {
                        button3.PerformClick();
                    }
                }
                else
                {
                    button3.PerformClick();
                }
                //---------------------------
                if (comboBox1.SelectedValue != null)
                {
                    groupBox3.Enabled = int.TryParse(comboBox1.SelectedValue.ToString(), out int yy) ? (clients.Rows.Cast<DataRow>().Where(ww => (int)ww["ID"] == (int)comboBox1.SelectedValue && ww["FULL_NME"].ToString() == comboBox1.Text).ToList().Count > 0 ? true : false) : false;
                }
                else
                {
                    groupBox3.Enabled = false;
                }
            }
            else
            {
                button3.PerformClick();
            }
        }

        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Properties.Settings.Default.Faire_reg_espece_facture_vente = checkBox1.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        void c_ControlChanged(object sender, EventArgs e)
        {
            button5.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Êtes-vous sûr de supprimer (" + dataGridView1.SelectedRows.Count + ") factures ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string idx = "";
                    dataGridView1.SelectedRows.Cast<DataGridViewRow>().ForEach(row => idx += "," + row.Cells["ID"].Value);
                    idx = idx.Substring(1);
                    //----------
                    bool ZZZ = MessageBox.Show("Retourner la quantité des produits au stock ?\n\n(Concerne juste les produits connus dans la base donné)", "Stock :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    if (ZZZ)
                    {
                        string cmd_tmp = "";
                        for (int f = 1; f < 71; f++)
                        {

                            //cmd_tmp += "SELECT `ITEM_NME_" + (f < 10 ? "0" : "") + f + "` AS 'ITEM_NME',`ITEM_PROD_CODE_" + (f < 10 ? "0" : "") + f + "` AS 'ITEM_PROD_CODE',SUM(`ITEM_QNT_" + (f < 10 ? "0" : "") + f + "`) AS 'ITEM_QNT' FROM tb_factures_vente WHERE `ID` IN ("+idx+") AND `ITEM_IS_PROD_" + (f < 10 ? "0" : "") + f + "` AND `ITEM_NME_" + (f < 10 ? "0" : "") + f + "` IS NOT NULL UNION ";
                            cmd_tmp += "SELECT `ITEM_NME_" + (f < 10 ? "0" : "") + f + "` AS 'ITEM_NME',`ITEM_PROD_CODE_" + (f < 10 ? "0" : "") + f + "` AS 'ITEM_PROD_CODE',`ITEM_QNT_" + (f < 10 ? "0" : "") + f + "` AS 'ITEM_QNT' FROM tb_factures_vente WHERE `ID` IN (" + idx + ") AND `ITEM_IS_PROD_" + (f < 10 ? "0" : "") + f + "` AND `ITEM_NME_" + (f < 10 ? "0" : "") + f + "` IS NOT NULL UNION ALL ";
                        }
                        cmd_tmp += ";";
                        cmd_tmp = cmd_tmp.Replace(" UNION ALL ;", "");
                        DataTable codes = PreConnection.Load_data("SELECT  tb1.ITEM_NME,SUM(tb1.ITEM_QNT) AS ITEM_QNT, PROD.`ID` AS 'PROD_ID' FROM (" + cmd_tmp + ") AS tb1 LEFT JOIN tb_produits AS PROD ON PROD.CODE = tb1.ITEM_PROD_CODE WHERE tb1.ITEM_NME IS NOT NULL AND PROD.`ID` IS NOT NULL GROUP BY tb1.ITEM_NME,PROD.`ID`;");

                        //RESULT COLUMNS >>> : ITEM_NME / ITEM_QNT / PROD_ID
                        foreach (DataRow row in codes.Rows)
                        {
                            PreConnection.Excut_Cmd(1, "tb_stock_mouv", new List<string> { "OP_DATE", "PROD_ID", "QNT_IN", "QNT_OUT", "OBSERV" }, new List<object> {
                                DateTime.Now,
                                row["PROD_ID"],
                                row["ITEM_QNT"],
                                0,
                                "Vente (Factures Annulées)"
                            }, null, null, null);
                            //PreConnection.Excut_Cmd("INSERT INTO `tb_stock_mouv`(`OP_DATE`,`PROD_ID`,`QNT_IN`,`QNT_OUT`,`OBSERV`) VALUES ("
                            //                               + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',"
                            //                               + "'" + row["PROD_ID"] + "',"
                            //                               + row["ITEM_QNT"] + ","
                            //                               + "0,"
                            //                               + "'Vente (Factures Annulées)');");
                        }
                    }
                    //-------
                    PreConnection.Excut_Cmd(3, "tb_factures_vente", null, null, "ID IN (@IDD)", new List<string> { "@IDD" }, new List<object> { idx });
                    //PreConnection.Excut_Cmd("DELETE FROM tb_factures_vente WHERE ID IN (" + idx + ")");
                    load_factures();
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
            if (!Is_New && numericUpDown2.Value != 0)
            {
                Transf_also_caisse = MessageBox.Show("Voulez-vous rendre le montant " + ((decimal)Selected_facture_old_infos.Rows[0]["SLD_REG_FAC"]).ToString("N2") + " DA au " + dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value + " -qu'il a payé precedement- ?", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            }

            if (comboBox1.SelectedValue != null)
            {
                groupBox3.Enabled = int.TryParse(comboBox1.SelectedValue.ToString(), out int yy) ? (clients.Rows.Cast<DataRow>().Where(ww => (int)ww["ID"] == (int)comboBox1.SelectedValue && ww["FULL_NME"].ToString() == comboBox1.Text).ToList().Count > 0 ? true : false) : false;
            }
            else
            {
                groupBox3.Enabled = false;
            }

        }

        private void button5_VisibleChanged(object sender, EventArgs e)
        {
            button8.Visible = !button5.Visible;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            facture_to_print.Rows.Clear();
            foreach (DataGridViewRow rwwd in dataGridView2.Rows)
            {
                facture_to_print.Rows.Add(rwwd.Cells[0].Value, rwwd.Cells[1].Value, rwwd.Cells[2].Value, rwwd.Cells[3].Value, rwwd.Cells[4].Value);
            }
            //--------------
            DataTable dt = new DataTable();
            dt.Columns.Add("PARAM_NME", typeof(string));
            dt.Columns.Add("PARAM_VAL", typeof(string));
            //----------------
            dt.Rows.Add(new object[] { "CABINET", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
            dt.Rows.Add(new object[] { "CABINET_TEL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
            dt.Rows.Add(new object[] { "CABINET_EMAIL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
            dt.Rows.Add(new object[] { "CABINET_ADRESS", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });

            dt.Rows.Add(new object[] { "REF", ("FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text) });
            dt.Rows.Add(new object[] { "DATE", dateTimePicker1.Value.ToString("dd/MM/yyyy") });

            dt.Rows.Add(new object[] { "TOT_HT", tot_ht });
            dt.Rows.Add(new object[] { "TVA_PERC", TVA_percent });
            dt.Rows.Add(new object[] { "TVA_MNT", tot_ht * TVA_percent / 100 });
            dt.Rows.Add(new object[] { "D_TIMBRE", (checkBox1.Checked ? decimal.Parse(dataGridView3.Rows[2].Cells[1].Value.ToString()) : 0) });
            dt.Rows.Add(new object[] { "TOT_TTC", (decimal)dataGridView3.Rows[0].Cells[1].Value + (decimal)dataGridView3.Rows[1].Cells[1].Value + (checkBox1.Checked ? decimal.Parse(dataGridView3.Rows[2].Cells[1].Value.ToString()) : 0) });

            int iddx = comboBox1.SelectedValue != null ? (comboBox1.SelectedValue != DBNull.Value ? (int)comboBox1.SelectedValue : -1) : -1;
            DataRow rww = clients.Rows.Cast<DataRow>().Where(FF => (int)FF["ID"] == iddx).FirstOrDefault();
            if (rww != null)
            {
                dt.Rows.Add(new object[] { "CLIENT_SEX", rww["SEX"] != DBNull.Value ? rww["SEX"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_FAMNME", rww["FAMNME"] != DBNull.Value ? rww["FAMNME"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NME", rww["NME"] != DBNull.Value ? rww["NME"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NUM_CNI", rww["NUM_CNI"] != DBNull.Value ? rww["NUM_CNI"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_ADRESS", rww["ADRESS"] != DBNull.Value ? rww["ADRESS"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_CITY", rww["CITY"] != DBNull.Value ? rww["CITY"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_WILAYA", rww["WILAYA"] != DBNull.Value ? rww["WILAYA"].ToString() : null });
                dt.Rows.Add(new object[] { "POSTAL_CODE", rww["POSTAL_CODE"] != DBNull.Value ? rww["POSTAL_CODE"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NUM_PHONE", rww["NUM_PHONE"] != DBNull.Value ? rww["NUM_PHONE"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_EMAIL", rww["EMAIL"] != DBNull.Value ? rww["EMAIL"].ToString() : null });
            }
            else
            {
                string full_nme = comboBox1.Text;
                string sexx = full_nme.Length > 4 ? (full_nme.IndexOf(".") > 0 ? full_nme.Substring(0, 5).Substring(0, full_nme.IndexOf(".")) : "Mr.") : "Mr.";
                string nmme = full_nme.Replace(sexx, "");
                dt.Rows.Add(new object[] { "CLIENT_SEX", sexx });
                dt.Rows.Add(new object[] { "CLIENT_FAMNME", nmme });
                dt.Rows.Add(new object[] { "CLIENT_NME", "" });
            }
            //-------------
            new Print_report("facture_vente", dt, facture_to_print).ShowDialog();
        }


        private void groupBox3_EnabledChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(groupBox3, groupBox3.Enabled ? "" : "Juste pour les propriétaires enregistrés !");
            label6.Visible = !groupBox3.Enabled;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //PreConnection.Excport_to_excel(dataGridView1, "ALBAITAR SoftVet Factures", "Vente", null, true);
            if (dataGridView1.Rows.Count > 0)
            {
                Excc.Application xcelApp = new Excc.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Factures";
                xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Factures";
                //----------
                xcelApp.Cells[1, 1].Value = "Date"; //DATE
                xcelApp.Cells[1, 2].Value = "Ref."; //REF
                xcelApp.Cells[1, 3].Value = "Client"; //CLIENT_FULL_NME
                xcelApp.Cells[1, 4].Value = "Total HT"; //TOTAL_HT
                xcelApp.Cells[1, 5].Value = "TVA"; //TVA_PERC
                xcelApp.Cells[1, 6].Value = "D.Timbre"; //DROIT_TIMBRE
                xcelApp.Cells[1, 7].Value = "Total TTC"; //TOTAL_TTC
                xcelApp.Cells[1, 8].Value = "Montant non payé\n(Jusqu'à " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")"; //SLDDD
                xcelApp.Cells[1, 9].Value = "Crée par"; //LAST_MODIF_BY
                //-----------
                for (int i = 1; i < 10; i++)
                {
                    ((Excc.Range)xcelApp.Cells[1, i]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[1, i]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[1, i]).Font.Color = ColorTranslator.ToOle(Color.White);
                    ((Excc.Range)xcelApp.Cells[1, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                }
                ((Excc.Range)xcelApp.Cells[1, 7]).Interior.Color = ColorTranslator.ToOle(Color.Orange); //TTC
                //----------------
                ((Excc.Range)xcelApp.Columns[1]).NumberFormat = "dd/MM/yyyy"; //DATE
                ((Excc.Range)xcelApp.Columns[4]).NumberFormat = //HT
                ((Excc.Range)xcelApp.Columns[5]).NumberFormat = //TVA
                ((Excc.Range)xcelApp.Columns[6]).NumberFormat = //DROIT_TIMBRE
                ((Excc.Range)xcelApp.Columns[7]).NumberFormat = //TOTAL_TTC
                ((Excc.Range)xcelApp.Columns[8]).NumberFormat = "#,##0.00 [$Da-fr-dz]"; //SLDDD
                //--------------
                dataGridView1.Rows.Cast<DataGridViewRow>().ToList().ForEach(t =>
                {
                    xcelApp.Cells[t.Index + 2, 1].Value = dataGridView1.Rows[t.Index].Cells[1].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[1].Value.ToString().Replace("00:00:00", "").TrimStart().TrimEnd() : ""; //DATE
                    xcelApp.Cells[t.Index + 2, 2].Value = dataGridView1.Rows[t.Index].Cells[4].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[4].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : ""; //REF
                    xcelApp.Cells[t.Index + 2, 3].Value = dataGridView1.Rows[t.Index].Cells[3].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[3].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : ""; //CLIENT_FULL_NME
                    xcelApp.Cells[t.Index + 2, 4].Value = dataGridView1.Rows[t.Index].Cells[7].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[7].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : ""; //TOTAL_HT
                    xcelApp.Cells[t.Index + 2, 5].Value = dataGridView1.Rows[t.Index].Cells[5].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[5].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : ""; //TVA_PERC
                    xcelApp.Cells[t.Index + 2, 6].Value = dataGridView1.Rows[t.Index].Cells[6].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[6].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : ""; //DROIT_TIMBRE
                    xcelApp.Cells[t.Index + 2, 7].Value = dataGridView1.Rows[t.Index].Cells[8].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[8].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : ""; //TOTAL_TTC
                    xcelApp.Cells[t.Index + 2, 8].Value = dataGridView1.Rows[t.Index].Cells[10].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[10].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : "(Unconnu)"; //SLDDD
                    xcelApp.Cells[t.Index + 2, 9].Value = dataGridView1.Rows[t.Index].Cells[9].Value != DBNull.Value ? dataGridView1.Rows[t.Index].Cells[9].Value.ToString().TrimStart().TrimEnd() : ""; //LAST_MODIF_BY

                    ((Excc.Range)xcelApp.Cells[t.Index + 2, 7]).Interior.Color = ColorTranslator.ToOle(Color.Moccasin);
                });
                //----------
                xcelApp.Range["D" + (dataGridView1.RowCount + 2)].Formula = "=SUM(D2:D" + (dataGridView1.RowCount + 1) + ")";
                xcelApp.Range["E" + (dataGridView1.RowCount + 2)].Formula = "=SUM(E2:E" + (dataGridView1.RowCount + 1) + ")";
                xcelApp.Range["F" + (dataGridView1.RowCount + 2)].Formula = "=SUM(F2:F" + (dataGridView1.RowCount + 1) + ")";
                xcelApp.Range["G" + (dataGridView1.RowCount + 2)].Formula = "=SUM(G2:G" + (dataGridView1.RowCount + 1) + ")";
                xcelApp.Range["H" + (dataGridView1.RowCount + 2)].Formula = "=SUM(H2:H" + (dataGridView1.RowCount + 1) + ")";
                //-------------
                for (int i = 1; i < 10; i++)
                {
                    ((Excc.Range)xcelApp.Cells[dataGridView1.RowCount + 2, i]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[dataGridView1.RowCount + 2, i]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[dataGridView1.RowCount + 2, i]).Font.Color = ColorTranslator.ToOle(Color.White);
                    ((Excc.Range)xcelApp.Cells[dataGridView1.RowCount + 2, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                }
                ((Excc.Range)xcelApp.Cells[dataGridView1.RowCount + 2, 7]).Interior.Color = ColorTranslator.ToOle(Color.Orange); //TTC
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

        private void Vente_Activated(object sender, EventArgs e)
        {
            if (to_select_idx > -1)
            {
                dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(Q => (int)Q.Cells["ID"].Value == to_select_idx).ToList().ForEach(W =>
                {
                    W.Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = W.Index;
                });
                to_select_idx = -1;
            }
            else if (to_select_idx == -2)
            {
                button3.PerformClick();
                to_select_idx = -1;

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            factures_filtr();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new Small_New_Client().ShowDialog();
            //-----------------------
            clients = PreConnection.Load_data("SELECT *,CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            comboBox1.DataSource = clients;
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";
            if (clients.Rows.Count > 0) { comboBox1.SelectedValue = (int)clients.AsEnumerable().Max(row => row.Field<int>("ID")); }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Enabled = dateTimePicker4.Enabled = checkBox2.Checked;
            factures_filtr();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown3.Enabled = numericUpDown4.Enabled = checkBox4.Checked;
            factures_filtr();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = checkBox3.Checked;
            factures_filtr();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown4.Minimum = numericUpDown3.Value;
            factures_filtr();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            factures_filtr();
        }

        private void factures_filtr()
        {
            if (dataGridView1.DataSource != null)
            {
                string fltr = "";
                if (checkBox2.Checked) //Dates
                {
                    fltr += !string.IsNullOrWhiteSpace(fltr) ? " AND " : "";
                    fltr += string.Format("DATE >= '{0}' AND DATE <= '{1}'", dateTimePicker3.Value, dateTimePicker4.Value);
                }
                if (checkBox3.Checked && !string.IsNullOrWhiteSpace(comboBox2.Text)) //Client
                {
                    fltr += !string.IsNullOrWhiteSpace(fltr) ? " AND " : "";
                    fltr += string.Format("CLIENT_FULL_NME LIKE '%{0}%'", comboBox2.Text.Replace("'", "''"));
                }
                if (checkBox4.Checked) //Montant
                {
                    fltr += !string.IsNullOrWhiteSpace(fltr) ? " AND " : "";
                    fltr += string.Format("TOTAL_TTC >= {0} AND TOTAL_TTC <= {1}", numericUpDown3.Value, numericUpDown4.Value);
                }
                if (checkBox5.Checked) //Created by (User)
                {
                    fltr += !string.IsNullOrWhiteSpace(fltr) ? " AND " : "";
                    fltr += string.Format("LAST_MODIF_BY LIKE '{0}'", comboBox3.Text);
                }
                if (checkBox6.Checked) //Solde non réglé (+/-)
                {
                    fltr += !string.IsNullOrWhiteSpace(fltr) ? " AND " : "";
                    fltr += "(SLD <> 0 OR SLD IS NULL)";
                }
                if (!string.IsNullOrWhiteSpace(textBox1.Text)) //Recherch
                {
                    fltr += !string.IsNullOrWhiteSpace(fltr) ? " AND " : "";
                    fltr += string.Format("(CONVERT(DATE, 'System.String') LIKE '{0}' OR " +
                        "CONVERT(CLIENT_FULL_NME, 'System.String') LIKE '%{0}%' OR " +
                        "CONVERT(REF, 'System.String') LIKE '%{0}%' OR " +
                        "CONVERT(TOTAL_TTC, 'System.String') LIKE '{0}' OR " +
                        "CONVERT(LAST_MODIF_BY, 'System.String') LIKE '%{0}%')", textBox1.Text.Replace("'", "''"));
                }
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = fltr;
            }
            //-----------
            decimal sum = 0;
            if (dataGridView1.SelectedRows.Count > 1)
            {
                sum = dataGridView1.SelectedRows.Cast<DataGridViewRow>()
                  .Sum(row => Convert.ToDecimal(row.Cells["TOTAL_TTC"].Value));
                label21.Text = "Séléctionnée (" + dataGridView1.SelectedRows.Count + ") : ";
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                sum = dataGridView1.Rows.Cast<DataGridViewRow>()
              .Sum(row => Convert.ToDecimal(row.Cells["TOTAL_TTC"].Value));
                label21.Text = "Total : ";
            }
            label21.Text += sum.ToString("# ##0.00");
            //----------------------
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker4.ValueChanged -= dateTimePicker3_ValueChanged;
            dateTimePicker4.MinDate = dateTimePicker3.Value;
            dateTimePicker4.ValueChanged += dateTimePicker3_ValueChanged;
            factures_filtr();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            factures_filtr();
        }

        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
            factures_filtr();
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 2)
            {
                decimal sum = dataGridView1.Rows.Cast<DataGridViewRow>()
                   .Sum(row => Convert.ToDecimal(row.Cells["TOTAL_TTC"].Value));

                label21.Text = "Total : " + sum.ToString("# ##0.00");
            }
            else
            {
                decimal sum = dataGridView1.SelectedRows.Cast<DataGridViewRow>()
                   .Sum(row => Convert.ToDecimal(row.Cells["TOTAL_TTC"].Value));
                label21.Text = "Séléctionnée (" + dataGridView1.SelectedRows.Count + ") : " + sum.ToString("# ##0.00");
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = checkBox5.Checked;
            factures_filtr();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            factures_filtr();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            factures_filtr();
        }
    }
}

