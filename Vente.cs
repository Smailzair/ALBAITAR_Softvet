using ALBAITAR_Softvet.Dialogs;
using Microsoft.Reporting.WinForms;
using ServiceStack;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Vente : Form
    {
        static public DataGridViewRow selected_item = null;
        static public DataTable stock_to_modify = new DataTable();
        decimal TVA_percent = 9;
        DataTable clients;
        DataTable factures;
        DataTable facture_to_print;
        DataTable Selected_facture_old_infos;
        bool Is_New = true;
        bool Transf_also_caisse = false;
        public Vente()
        {
            InitializeComponent();
            //-----------------------
            clients = PreConnection.Load_data("SELECT *,CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS FULL_NME FROM tb_clients;");
            comboBox1.DataSource = clients;
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";
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
            selected_item = new DataGridViewRow();
            //------------------------
            new Add_Vente_Fact_Item().ShowDialog();
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
            pictureBox2.Image = Properties.Resources.NOUVEAU_003;
            stock_to_modify.Rows.Clear();
            selected_item = new DataGridViewRow();
            TVA_percent = decimal.Parse(Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 5).Select(QQ => QQ["VAL"]).First().ToString());
            dataGridView3.Rows[1].Cells[0].Value = "TVA (" + TVA_percent.ToString("N2") + " %):";
            label3.Visible = false;
            comboBox1.BackColor = SystemColors.Window;
            comboBox1.Text = "";
            comboBox1.SelectedValue = DBNull.Value;
            dateTimePicker1.Value = dateTimePicker2.Value = DateTime.Now;
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
            numericUpDown1.Value = dds != null ? (dds + 1) : 1;
            dataGridView2.Rows.Clear();
            checkBox1.Checked = Properties.Settings.Default.Faire_reg_espece_facture_vente;
            numericUpDown2.Value = 0;
            button5.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sur de faire la suppression ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool fff = true;
                if (!Is_New) { fff = MessageBox.Show("Retourner la quantité au stock ?\n\n(Excepte les produits connus dans la base donné)", "Stock :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes; }
                foreach (DataGridViewRow rwx in dataGridView2.SelectedRows)
                {
                    if (fff)
                    {
                        DataRow rw = Vente.stock_to_modify.NewRow();
                        rw["PROD_ID"] = DBNull.Value;
                        rw["PROD_CODE"] = rwx.Cells["PRODUCT_CODE"].Value;
                        rw["QNT_DIMIN"] = (decimal)rwx.Cells["QNT2"].Value * -1;
                        Vente.stock_to_modify.Rows.Add(rw);
                        //stock_to_modify.Rows.Cast<DataRow>().Where(x => x["PROD_CODE"].ToString() == rwx.Cells["PRODUCT_CODE"].Value.ToString()).ToList().ForEach(x => x.Delete());
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
            decimal tot = 0;
            foreach (DataGridViewRow rw in dataGridView2.Rows)
            {
                tot += rw.Cells["SLD"].Value != DBNull.Value ? (decimal)rw.Cells["SLD"].Value : 0;
            }
            dataGridView3.Rows[0].Cells[1].Value = tot; //HT
            //-------------------------------
            dataGridView3.Rows[1].Cells[1].Value = tot * TVA_percent / 100; //TVA
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
            load_factures();
        }

        private void load_factures()
        {
            int prev_idx = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : -1;
            factures = PreConnection.Load_data("SELECT `ID`,`DATE`,`CLIENT_ID`,`CLIENT_FULL_NME`,`REF`,`TVA_PERC`,`DROIT_TIMBRE`,`TOTAL_HT`,`TOTAL_TTC` FROM tb_factures_vente;");
            dataGridView1.DataSource = factures;
            if (prev_idx > -1 && dataGridView1.Rows.Count > prev_idx)
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
            All_Ready &= comboBox1.Text.Trim().Length > 0;
            comboBox1.BackColor = comboBox1.Text.Trim().Length == 0 ? Color.LightCoral : SystemColors.Window;
            //-----------------
            if (All_Ready)
            {
                if (Is_New) //INSERT
                {
                    string cmmd = "INSERT INTO `tb_factures_vente`"
                                + "(`DATE`,"
                                + "`CLIENT_ID`,"
                                + "`CLIENT_FULL_NME`,"
                                + "`REF`,"
                                + "`TVA_PERC`,"
                                + "`DROIT_TIMBRE`,"
                                + "`TOTAL_HT`,"
                                + "`TOTAL_TTC`";
                    string cmmd2 = " VALUES (" +
                        "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," + //DATE
                        (comboBox1.SelectedValue != null ? (clients.Rows.Cast<DataRow>().Where(ww => (int)ww["ID"] == (int)comboBox1.SelectedValue && ww["FULL_NME"].ToString() == comboBox1.Text).ToList().Count > 0 ? comboBox1.SelectedValue : "NULL") : "NULL") + "," + //CLIENT_ID
                        "'" + comboBox1.Text + "'," + //CLIENT_FULL_NME
                        "'" + ("FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text) + "'," + //REF
                        dataGridView3.Rows[1].Cells[1].Value + "," + //TVA_PERC
                        dataGridView3.Rows[2].Cells[1].Value + "," + //DROIT_TIMBRE
                        dataGridView3.Rows[0].Cells[1].Value + "," + //TOTAL_HT
                        dataGridView3.Rows[3].Cells[1].Value; //TOTAL_TTC

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        cmmd += ",`ITEM_NME_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "`";
                        cmmd += ",`ITEM_QNT_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "`";
                        cmmd += ",`ITEM_IS_PROD_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "`";
                        cmmd += ",`ITEM_PROD_CODE_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "`";
                        cmmd += ",`ITEM_PRIX_UNIT_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "`";
                        //----------------
                        cmmd2 += ",'" + row.Cells["ITEM_NME"].Value.ToString() + "'";
                        cmmd2 += "," + row.Cells["QNT2"].Value;
                        cmmd2 += "," + (row.Cells["TYPE"].Value.ToString() == "Produit" ? 1 : 0);
                        cmmd2 += "," + (row.Cells["TYPE"].Value.ToString() == "Produit" ? "'" + row.Cells["PRODUCT_CODE"].Value.ToString() + "'" : "NULL");
                        cmmd2 += "," + row.Cells["PRIX_UNIT"].Value.ToString();
                    }

                    cmmd += ")";
                    cmmd2 += ");";
                    PreConnection.Excut_Cmd(cmmd + cmmd2);
                    //-------Caisse (INSERT)----------                    
                    if (groupBox3.Enabled)
                    {
                        PreConnection.Excut_Cmd("INSERT INTO `tb_clients_finance`"
                                          + "(`CLIENT_ID`,"
                                          + "`OP_DATE`,"
                                          + "`OBJECT`,"
                                          + "`DEBIT`,"
                                          + "`CREDIT`,"
                                          + "`FACT_NUM`)"
                                          + "VALUES"
                                          + "(" + comboBox1.SelectedValue + ","
                                          + "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                                          + "'Droits de facture [FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "]',"
                                          + dataGridView3.Rows[3].Cells[1].Value + ","
                                          + numericUpDown2.Value + ","
                                          + "'FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "');");
                    }
                    //---------------------------
                }
                else //UPDATE
                {
                    if (stock_to_modify != null)
                    {
                        string inti = "";
                        for (int f = 1; f < 71; f++)
                        {
                            inti += ",`ITEM_NME_" + (f < 10 ? "0" : "") + f + "` = NULL";
                            inti += ",`ITEM_QNT_" + (f < 10 ? "0" : "") + f + "` = NULL";
                            inti += ",`ITEM_IS_PROD_" + (f < 10 ? "0" : "") + f + "` = NULL";
                            inti += ",`ITEM_PROD_CODE_" + (f < 10 ? "0" : "") + f + "` = NULL";
                            inti += ",`ITEM_PRIX_UNIT_" + (f < 10 ? "0" : "") + f + "` = NULL";
                        }
                        PreConnection.Excut_Cmd("UPDATE `tb_factures_vente` SET " + inti.Substring(1) + " WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString() + ";");
                    }
                    //------------
                    string cmmd = "UPDATE `tb_factures_vente` SET "
                            + "`DATE` = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                            + "`CLIENT_ID` = " + (comboBox1.SelectedValue != null ? (clients.Rows.Cast<DataRow>().Where(ww => (int)ww["ID"] == (int)comboBox1.SelectedValue && ww["FULL_NME"].ToString() == comboBox1.Text).ToList().Count > 0 ? comboBox1.SelectedValue : "NULL") : "NULL") + ","
                            + "`CLIENT_FULL_NME` = '" + comboBox1.Text + "',"
                            + "`REF` = '" + ("FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text) + "',"
                            + "`TVA_PERC` = " + dataGridView3.Rows[1].Cells[1].Value + ","
                            + "`DROIT_TIMBRE` = " + dataGridView3.Rows[2].Cells[1].Value + ","
                            + "`TOTAL_HT` = " + dataGridView3.Rows[0].Cells[1].Value + ","
                            + "`TOTAL_TTC` = " + dataGridView3.Rows[3].Cells[1].Value;
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        cmmd += ",`ITEM_NME_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "` = '" + row.Cells["ITEM_NME"].Value.ToString() + "'";
                        cmmd += ",`ITEM_QNT_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "` = " + row.Cells["QNT2"].Value;
                        cmmd += ",`ITEM_IS_PROD_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "` = " + (row.Cells["TYPE"].Value.ToString() == "Produit" ? 1 : 0);
                        cmmd += ",`ITEM_PROD_CODE_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "` = " + (row.Cells["TYPE"].Value.ToString() == "Produit" ? "'" + row.Cells["PRODUCT_CODE"].Value.ToString() + "'" : "NULL");
                        cmmd += ",`ITEM_PRIX_UNIT_" + (row.Index < 9 ? "0" : "") + (row.Index + 1) + "` = " + row.Cells["PRIX_UNIT"].Value.ToString();
                        //----------------
                    }

                    cmmd += " WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString() + ";";
                    PreConnection.Excut_Cmd(cmmd);
                    //-------Caisse (UPDATE)----------
                    //RMQ : il y a un trigger MySQL pour modifier quelques infos.
                    bool ttm = comboBox1.SelectedValue == dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value;
                    bool ttm2 = !ttm ? comboBox1.Text == (dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value != DBNull.Value ? (string)dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value : "") : ttm;
                    int cb1_selected_value = comboBox1.SelectedValue == DBNull.Value ? -1 : (((comboBox1.SelectedValue == dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value && comboBox1.Text == dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value.ToString()) || (clients.AsEnumerable().Where(row => row.Field<int>("ID") == (int)comboBox1.SelectedValue && row.Field<string>("FULL_NME") == comboBox1.Text).Count() > 0)) ? (int)comboBox1.SelectedValue : -1);
                    int old_clt_id = dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value == DBNull.Value ? -1 : (int)dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value;
                    if (!ttm2) //Client était changé
                    {
                        if (Transf_also_caisse)
                        {
                            if (old_clt_id > -1)
                            {
                                if (cb1_selected_value > -1)
                                {
                                    PreConnection.Excut_Cmd("UPDATE tb_clients_finance SET " +
                                                    "`OP_DATE`='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," +
                                                    "`CLIENT_ID`=" + cb1_selected_value + "," +
                                                    "`DEBIT`=" + dataGridView3.Rows[3].Cells[1].Value + "," +
                                                    "`CREDIT`=" + numericUpDown2.Value +
                                                    " WHERE `FACT_NUM` LIKE 'FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "';");

                                }
                                else if (cb1_selected_value == -1)
                                {
                                    PreConnection.Excut_Cmd("DELETE FROM tb_clients_finance WHERE `FACT_NUM` LIKE 'FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "';");
                                }
                            }
                            else
                            {
                                if (cb1_selected_value > -1)
                                {
                                    PreConnection.Excut_Cmd("INSERT INTO `tb_clients_finance`"
                                     + "(`CLIENT_ID`,"
                                     + "`OP_DATE`,"
                                     + "`OBJECT`,"
                                     + "`DEBIT`,"
                                     + "`CREDIT`,"
                                     + "`FACT_NUM`)"
                                     + "VALUES"
                                     + "(" + cb1_selected_value + ","
                                     + "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                                     + "'Droits de facture [FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "]',"
                                     + dataGridView3.Rows[3].Cells[1].Value + ","
                                     + numericUpDown2.Value + ","
                                     + "'FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "');");
                                }
                            }
                        }
                        else
                        {
                            if (old_clt_id > -1)
                            {
                                PreConnection.Excut_Cmd("UPDATE tb_clients_finance SET " +
                                                    "`OBJECT`=CONCAT(`OBJECT`,'(Annulé)')," +
                                                    "`DEBIT`=0," +
                                                    "`FACT_NUM`=NULL" +
                                                    " WHERE `FACT_NUM` LIKE 'FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "';");
                                
                            }

                            if (cb1_selected_value > -1)
                            {
                                PreConnection.Excut_Cmd("INSERT INTO `tb_clients_finance`"
                                 + "(`CLIENT_ID`,"
                                 + "`OP_DATE`,"
                                 + "`OBJECT`,"
                                 + "`DEBIT`,"
                                 + "`CREDIT`,"
                                 + "`FACT_NUM`)"
                                 + "VALUES"
                                 + "(" + cb1_selected_value + ","
                                 + "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                                 + "'Droits de facture [FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "]',"
                                 + dataGridView3.Rows[3].Cells[1].Value + ","
                                 + numericUpDown2.Value + ","
                                 + "'FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "');");
                            }
                        }
                    }
                    else
                    {
                        if (cb1_selected_value > -1)
                        {
                            PreConnection.Excut_Cmd("UPDATE tb_clients_finance SET " +
                            "`OP_DATE`='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," +
                            "`DEBIT`=" + dataGridView3.Rows[3].Cells[1].Value + "," +
                            "`CREDIT`=" + numericUpDown2.Value +
                            " WHERE `FACT_NUM` LIKE 'FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text + "';");
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
                            PreConnection.Excut_Cmd("INSERT INTO `tb_stock_mouv`"
                                            + "(`OP_DATE`,"
                                            + "`PROD_ID`,"
                                            + "`QNT_IN`,"
                                            + "`QNT_OUT`,"
                                            + "`OBSERV`)"
                                            + "VALUES"
                                            + "('" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"//OP_DATE
                                            + "(SELECT `ID` FROM tb_produits WHERE `CODE` LIKE '" + group.First()["PROD_CODE"] + "' LIMIT 1),"//PROD_ID
                                            + (sumSLD < 0 ? sumSLD * -1 : 0) + ","//QNT_IN
                                            + (sumSLD > 0 ? sumSLD : 0) + "," //QNT_OUT
                                            + "'Vente (Facture [" + ("FA_" + dateTimePicker2.Value.ToString("yyyy") + "_" + textBox2.Text) + "]) -" + (Is_New ? "Crée" : "Modifiée") + "-') "  //OBSERV
                                            + "WHERE (SELECT COUNT(`CODE`) FROM tb_produits WHERE `CODE` LIKE '" + group.First()["PROD_CODE"] + "') > 0"
                                            + ";");
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
            if (!Is_New && numericUpDown2.Value != 0)
            {
                Transf_also_caisse = MessageBox.Show("Voulez-vous rendre le montant " + ((decimal)Selected_facture_old_infos.Rows[0]["SLD_REG_FAC"]).ToString("N2") + " DA au " + dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value + " -qu'il a payé precedement- ?", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
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
                //-------------------
                //DataTable data = PreConnection.Load_data("SELECT * FROM tb_factures_vente WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + ";");
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
                        comboBox1.SelectedValue = Selected_facture_old_infos.Rows[0]["CLIENT_ID"] != DBNull.Value ? Selected_facture_old_infos.Rows[0]["CLIENT_ID"] : DBNull.Value;
                        if (comboBox1.Text == string.Empty) { comboBox1.Text = Selected_facture_old_infos.Rows[0]["CLIENT_FULL_NME"].ToString(); }
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
                    bool ZZZ = MessageBox.Show("Retourner la quantité des produits au stock ?\n\n(Excepte les produits connus dans la base donné)", "Stock :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
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
                        DataTable codes = PreConnection.Load_data("SELECT  tb1.ITEM_NME,SUM(tb1.ITEM_QNT) AS ITEM_QNT, PROD.`ID` AS 'PROD_ID' FROM (" + cmd_tmp + ") AS tb1 LEFT JOIN tb_produits AS PROD ON PROD.CODE = tb1.ITEM_PROD_CODE WHERE tb1.ITEM_NME IS NOT NULL AND PROD.`ID` IS NOT NULL GROUP BY tb1.ITEM_NME;");

                        //RESULT COLUMNS >>> : ITEM_NME / ITEM_QNT / PROD_ID
                        foreach (DataRow row in codes.Rows)
                        {
                            PreConnection.Excut_Cmd("INSERT INTO `tb_stock_mouv`(`OP_DATE`,`PROD_ID`,`QNT_IN`,`QNT_OUT`,`OBSERV`) VALUES ("
                                                           + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',"
                                                           + "'" + row["PROD_ID"] + "',"
                                                           + row["ITEM_QNT"] + ","
                                                           + "0,"
                                                           + "'Vente (Factures Annulées)');");
                        }
                    }
                    //-------
                    PreConnection.Excut_Cmd("DELETE FROM tb_factures_vente WHERE ID IN (" + idx + ")");
                    load_factures();
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
            if (!Is_New && numericUpDown2.Value != 0)
            {
                Transf_also_caisse = MessageBox.Show("Voulez-vous rendre le montant " + ((decimal)Selected_facture_old_infos.Rows[0]["SLD_REG_FAC"]).ToString("N2") + " DA au " + dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value + " -qu'il a payé precedement- ?", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
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


            DataRow rww = clients.Rows.Cast<DataRow>().Where(FF => (int)FF["ID"] == (int)comboBox1.SelectedValue).FirstOrDefault();
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
            //-------------
            new Print_report("facture_vente", dt, facture_to_print).ShowDialog();
        }


        private void groupBox3_EnabledChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(groupBox3, groupBox3.Enabled ? "" : "Juste pour les propriétaires enregistrés !");
        }

    }
}

