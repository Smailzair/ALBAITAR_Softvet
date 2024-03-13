using ALBAITAR_Softvet.Dialogs;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using static ServiceStack.Script.Lisp;
using Excc = Microsoft.Office.Interop.Excel;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Produits : Form
    {
        bool Is_New_Product = true;
        bool Is_New_Stock = true;
        DataTable Products;
        DataTable Stock;
        BindingSource Products_Bind;
        BindingSource Stock_Bind;
        decimal prev_sld = 0;
        decimal prev_sld2 = 0;
        decimal numericUpDown2_old_val = 0;
        decimal selected_prod_qnt_min = 0;
        bool selected_prod_alert_on_min = false;

        int ttimee = 0;
        int ttimee2 = 0;

        public Produits()
        {
            InitializeComponent();
            //----------------------
            numericUpDown2.Controls[0].Visible = false;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.SetProperty, null,
            dataGridView1, new object[] { true });
            //------
            Products_Bind = new BindingSource();
            dataGridView1.DataSource = Products_Bind;
            Stock_Bind = new BindingSource();
            dataGridView2.DataSource = Stock_Bind;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            numericUpDown2.Value += 1;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if ((numericUpDown2.Value - 1) >= numericUpDown2.Minimum)
            {
                numericUpDown2.Value -= 1;
            }
            else if ((numericUpDown2.Value - (decimal)0.1) >= numericUpDown2.Minimum)
            {
                numericUpDown2.Value -= (decimal)0.1;
            }
            else if ((numericUpDown2.Value - (decimal)0.01) >= numericUpDown2.Minimum)
            {
                numericUpDown2.Value -= (decimal)0.01;
            }

        }

        private void Produits_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button4.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30002" && (Int32)QQ[3] == 1).Count() > 0; //Supprimer
                button3.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30001" && (Int32)QQ[3] == 1).Count() > 0; //Ajouter                   
                panel2.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30003" && (Int32)QQ[3] == 1).Count() > 0; //Modifier                              
                if (Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30005" && (Int32)QQ[3] == 1).Count() > 0) //Modifier Historique
                {
                    panel1.Enabled = true;
                }
                else if (Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30004" && (Int32)QQ[3] == 1).Count() > 0)//Consulter Historique
                {
                    panel1.Enabled = false;
                }
                else
                {
                    groupBox2.Visible = false;
                }
            }
            //////////////////load_prods(false);
            //////////////////load_stocks(false);
            load_data(false, false);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown5.Enabled = checkBox1.Checked;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool autorisat = Properties.Settings.Default.Last_login_is_admin || (Is_New_Product && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30001" && (Int32)QQ[3] == 1).Count() > 0) || (!Is_New_Product && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30003" && (Int32)QQ[3] == 1).Count() > 0);
            if (autorisat)
            {
                textBox2.BackColor = textBox2.Text.Trim().Length > 0 ? SystemColors.Window : Color.LightCoral;
                textBox3.BackColor = textBox3.Text.Trim().Length > 0 ? SystemColors.Window : Color.LightCoral;
                if (Is_New_Product && textBox2.Text.Trim().Length > 0)
                {
                    int sdf = Products.Rows.Cast<DataRow>().Where(cc => cc["CODE"].ToString() == textBox2.Text).Count();
                    label24.Visible = sdf > 0;
                    textBox2.BackColor = sdf > 0 ? Color.LightCoral : SystemColors.Window;
                }
                //else if (!Is_New_Product)
                //{
                //    DateTime minDate = Stock.AsEnumerable()
                //            .Where(row => row["PROD_ID"].ToString() == dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString() && row["OBSERV"].ToString() != "Achat (Premier Stock)")
                //            .Min(row => row.Field<DateTime?>("OP_DATE")) ?? new DateTime(2999, 01, 01);
                //    if (minDate != null)
                //    {

                //        label13.BackColor = dateTimePicker2.Value > minDate ? Color.LightCoral : SystemColors.Control;
                //        if (label13.BackColor == Color.LightCoral)
                //        {
                //            MessageBox.Show("Dans les opérations de stock de cet produit, il y a une date avec une valeur supérieure à la date sélectionnée.\n\nVous avez deux choix :\n1- Changer la date à une valeur inférieure ou égale à '" + minDate.ToString("dd/MM/yyyy") + "'.\nOu\n2- Changer les dates d'opérations de stock de date '" + minDate.ToString("dd/MM/yyyy") + "' à '" + dateTimePicker2.Value.ToString("dd/MM/yyyy") + "' ou plus superieur.\n", "Probleme de date :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        }
                //    }
                //}
                numericUpDown1.BackColor = numericUpDown1.Value > 0 ? SystemColors.Window : Color.LightCoral;

                //--------------------
                bool ready = true;
                ready &= label13.BackColor != Color.LightCoral;
                ready &= textBox2.BackColor != Color.LightCoral;
                ready &= textBox3.BackColor != Color.LightCoral;
                ready &= label24.Visible == false;
                ready &= numericUpDown1.Value > 0;
                //---------------------------
                if (ready)
                {
                    if (Is_New_Product)
                    {
                        PreConnection.Excut_Cmd(1, "tb_produits", new List<string> {
                        "CODE",
"NME",
"CATEGOR",
//"QNT",
"ALERT_MIN_ON",
"QNT_MIN",
"REVIENT_PRTICE",
"VENTE_PRICE",
"TMP_FIRST_INSERT_DATE"}, new List<object>
{
     textBox2.Text, //CODE
                                              textBox3.Text, //NME
                                              comboBox2.SelectedItem, //CATEGOR
                                              (checkBox1.Checked ? 1 : 0), //ALERT_MIN_ON
                                              Convert.ToDouble(numericUpDown5.Value), //QNT_MIN
                                              Convert.ToDouble(numericUpDown1.Value), //REVIENT_PRTICE
                                              Convert.ToDouble(numericUpDown3.Value), //VENTE_PRICE
                                              dateTimePicker2.Value
}, null, null, null);
                        //PreConnection.Excut_Cmd("INSERT INTO `tb_produits`"
                        //                      + "(`CODE`,"
                        //                      + "`NME`,"
                        //                      + "`CATEGOR`,"
                        //                      + "`QNT`,"
                        //                      + "`ALERT_MIN_ON`,"
                        //                      + "`QNT_MIN`,"
                        //                      + "`REVIENT_PRTICE`,"
                        //                      + "`VENTE_PRICE`,"
                        //                      + "`TMP_FIRST_INSERT_DATE`)"
                        //                      + "VALUES"
                        //                      + "('" + textBox2.Text.Replace("'", "''") + "'," //CODE
                        //                      + "'" + textBox3.Text.Replace("'", "''") + "'," //NME
                        //                      + "'" + ((string)comboBox2.SelectedItem).Replace("'", "''") + "'," //CATEGOR
                        //                      + Convert.ToDouble(numericUpDown4.Value) + "," //QNT
                        //                      + (checkBox1.Checked ? 1 : 0) + "," //ALERT_MIN_ON
                        //                      + Convert.ToDouble(numericUpDown5.Value) + "," //QNT_MIN
                        //                      + Convert.ToDouble(numericUpDown1.Value) + "," //REVIENT_PRTICE
                        //                      + Convert.ToDouble(numericUpDown3.Value) + "," //VENTE_PRICE
                        //                      + "'" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "');" //TMP_FIRST_INSERT_DATE
                        //                      );

                    }
                    else
                    {
                        PreConnection.Excut_Cmd(2, "tb_produits", new List<string> {
                        "CODE",
"NME",
"CATEGOR",
//"QNT",
"ALERT_MIN_ON",
"QNT_MIN",
"REVIENT_PRTICE",
"VENTE_PRICE",
"TMP_FIRST_INSERT_DATE"}, new List<object>
{
     textBox2.Text, //CODE
                                              textBox3.Text, //NME
                                              comboBox2.SelectedItem, //CATEGOR
                                              //Convert.ToDouble(numericUpDown4.Value), //QNT
                                              (checkBox1.Checked ? 1 : 0), //ALERT_MIN_ON
                                              Convert.ToDouble(numericUpDown5.Value), //QNT_MIN
                                              Convert.ToDouble(numericUpDown1.Value), //REVIENT_PRTICE
                                              Convert.ToDouble(numericUpDown3.Value), //VENTE_PRICE
                                              dateTimePicker2.Value
}, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView1.SelectedRows[0].Cells["ID"].Value });
                        //PreConnection.Excut_Cmd("UPDATE `tb_produits` SET "
                        //                      + "`CODE` = '" + textBox2.Text.Replace("'", "''") + "'," //CODE
                        //                      + "`NME` = '" + textBox3.Text.Replace("'", "''") + "'," //NME
                        //                      + "`CATEGOR` = '" + ((string)comboBox2.SelectedItem).Replace("'", "''") + "'," //CATEGOR
                        //                      + "`QNT` = " + Convert.ToDouble(numericUpDown4.Value) + "," //QNT
                        //                      + "`ALERT_MIN_ON` = " + (checkBox1.Checked ? 1 : 0) + "," //ALERT_MIN_ON
                        //                      + "`QNT_MIN` = " + Convert.ToDouble(numericUpDown5.Value) + "," //QNT_MIN
                        //                      + "`REVIENT_PRTICE` = " + Convert.ToDouble(numericUpDown1.Value) + "," //REVIENT_PRTICE
                        //                      + "`VENTE_PRICE` = " + Convert.ToDouble(numericUpDown3.Value) + ","//VENTE_PRICE
                        //                      + "`TMP_FIRST_INSERT_DATE` = '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'"//TMP_FIRST_INSERT_DATE
                        //                      + " WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + ";");
                    }
                    //////////////////load_prods(Is_New_Product);
                    //////////////////load_stocks(false);
                    load_data(Is_New_Product, false);
                }
            }
            else
            {
                new Non_Autorized_Msg("").ShowDialog();
            }



        }

        private void initial_details_fields()
        {
            Is_New_Product = true;
            dateTimePicker2.Value = DateTime.Now;
            textBox2.Clear();
            textBox3.Clear();
            comboBox2.SelectedIndex = 0;
            numericUpDown1.Value = numericUpDown3.Value = numericUpDown5.Value = 0;
            //numericUpDown4.Minimum = 0;
            checkBox1.Checked = false;
            textBox2.BackColor = textBox3.BackColor = SystemColors.Window;
            label13.BackColor = SystemColors.Control;
            pictureBox1.Image = Properties.Resources.NOUVEAU;
            radioButton2.Text = "--";
            radioButton1.Checked = true;
            label24.Visible = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
            if (((TextBox)sender).Name == "textBox2")
            {
                label24.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            initial_details_fields();
        }

        ////////////////////private void load_prods(bool is_insert)
        ////////////////////{
        ////////////////////    ////////////////bool dgv1_SelectionChanged_removed = true;
        ////////////////////    ////////////////dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;

        ////////////////////    ////////////////int dgv1_prev_select = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : -1;
        ////////////////////    ////////////////int dgv1_first_shown_rw_idx = dataGridView1.FirstDisplayedCell.RowIndex;
        ////////////////////    ////////////////Products = PreConnection.Load_data("SELECT tb1.*,tb2.SLD FROM tb_produits tb1 left join (SELECT `PROD_ID`,SUM(`QNT_IN`) - SUM(`QNT_OUT`) AS SLD FROM tb_stock_mouv GROUP BY `PROD_ID`) tb2 on tb2.PROD_ID = tb1.ID;");
        ////////////////////    ////////////////Products_Bind.DataSource = new BindingSource(Products, "");

        ////////////////////    ////////////////if (dataGridView1.Rows[dgv1_prev_select] != null)
        ////////////////////    ////////////////{
        ////////////////////    ////////////////    dataGridView1.ClearSelection();
        ////////////////////    ////////////////    dgv1_SelectionChanged_removed = false;
        ////////////////////    ////////////////    dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        ////////////////////    ////////////////    dataGridView1.Rows[dgv1_prev_select].Selected = true;
        ////////////////////    ////////////////    //---------------

        ////////////////////    ////////////////}

        ////////////////////    ////////////////if (dataGridView1.Rows[dgv1_first_shown_rw_idx] != null)
        ////////////////////    ////////////////{
        ////////////////////    ////////////////    dataGridView1.FirstDisplayedCell = dataGridView1.Rows[dgv1_first_shown_rw_idx].Cells["CODE"];
        ////////////////////    ////////////////}

        ////////////////////    ////////////////if (dgv1_SelectionChanged_removed)
        ////////////////////    ////////////////{
        ////////////////////    ////////////////    dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        ////////////////////    ////////////////}
        ////////////////////    //==============================================
        ////////////////////    int prec_select_id = 0;
        ////////////////////    int dgv1_first_shown_rw_idx = dataGridView1.DisplayedRowCount(true) > 0 ? dataGridView1.FirstDisplayedCell.RowIndex : -1;
        ////////////////////    if (dataGridView1.SelectedRows.Count > 0)
        ////////////////////    {
        ////////////////////        prec_select_id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
        ////////////////////    }
        ////////////////////    Products = PreConnection.Load_data("SELECT tb1.*,tb2.SLD FROM tb_produits tb1 left join (SELECT `PROD_ID`,SUM(`QNT_IN`) - SUM(`QNT_OUT`) AS SLD FROM tb_stock_mouv GROUP BY `PROD_ID`) tb2 on tb2.PROD_ID = tb1.ID;");


        ////////////////////    Products_Bind = new BindingSource(Products, "");


        ////////////////////    string dgv1_sortby_col_nme = dataGridView1.SortedColumn != null ? dataGridView1.SortedColumn.Name : "NME";
        ////////////////////    ////////////////dataGridView1.DataSource = Products;
        ////////////////////    dataGridView1.DataSource = Products_Bind;
        ////////////////////    dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
        ////////////////////    dataGridView1.Sort(dataGridView1.Columns[dgv1_sortby_col_nme], System.ComponentModel.ListSortDirection.Ascending);
        ////////////////////    dataGridView1.ClearSelection();
        ////////////////////    if (is_insert)
        ////////////////////    {
        ////////////////////        dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        ////////////////////        if (dataGridView1.Rows.Count > 0 && Products != null)
        ////////////////////        {
        ////////////////////            var maxIdRow = dataGridView1.Rows.Cast<DataGridViewRow>()
        ////////////////////                            .OrderByDescending(r => Convert.ToInt32(r.Cells["ID"].Value))
        ////////////////////                            .FirstOrDefault();
        ////////////////////            if (maxIdRow != null)
        ////////////////////            {
        ////////////////////                maxIdRow.Selected = true;
        ////////////////////            }
        ////////////////////        }
        ////////////////////    }
        ////////////////////    else
        ////////////////////    {
        ////////////////////        if (dataGridView1.Rows.Cast<DataGridViewRow>().Where(P => (int)P.Cells["ID"].Value == prec_select_id).Count() > 0)
        ////////////////////        {
        ////////////////////            dataGridView1.Rows.Cast<DataGridViewRow>().Where(P => (int)P.Cells["ID"].Value == prec_select_id).First().Selected = true;
        ////////////////////        }
        ////////////////////        else if (dataGridView1.Rows.Count > 0)
        ////////////////////        {
        ////////////////////            dataGridView1.Rows[0].Selected = true;

        ////////////////////        }
        ////////////////////        else
        ////////////////////        {
        ////////////////////            initial_details_fields();
        ////////////////////        }
        ////////////////////        dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        ////////////////////    }

        ////////////////////    if (dgv1_first_shown_rw_idx > -1)
        ////////////////////    {
        ////////////////////        if (dataGridView1.Rows[dgv1_first_shown_rw_idx] != null)
        ////////////////////        {
        ////////////////////            dataGridView1.FirstDisplayedCell = dataGridView1.Rows[dgv1_first_shown_rw_idx].Cells["CODE"];
        ////////////////////        }
        ////////////////////    }

        ////////////////////}

        private void load_data(bool is_prod_insert, bool is_stock_insert)
        {
            panel5.Visible = true;
            //----------
            bool radiobtn1_checked = radioButton1.Checked;
            int dgv1_first_shown_rw_idx = dataGridView1.DisplayedRowCount(true) > 0 ? dataGridView1.FirstDisplayedCell.RowIndex : -1;
            int dgv2_first_shown_rw_idx = dataGridView2.DisplayedRowCount(true) > 0 ? dataGridView2.FirstDisplayedCell.RowIndex : -1;
            int prec_select_id = 0;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                prec_select_id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
            }
            int prec_dgv2_select = 0;
            if (dataGridView2.SelectedRows.Count > 0)
            {
                prec_dgv2_select = (int)dataGridView2.SelectedRows[0].Cells["ID2"].Value;
            }

            //-------- I\- PRODUCTS ------------------------
            Products = PreConnection.Load_data("SELECT tb1.*,CASE WHEN tb2.SLD IS NULL THEN 0.00 ELSE tb2.SLD END AS SLD FROM tb_produits tb1 left join (SELECT `PROD_ID`,SUM(`QNT_IN`) - SUM(`QNT_OUT`) AS SLD FROM tb_stock_mouv GROUP BY `PROD_ID`) tb2 on tb2.PROD_ID = tb1.ID;");


            //Products_Bind = new BindingSource(Products, "");
            Products_Bind.DataSource = Products ?? null;

            //Products_Bind = new BindingSource(Products, "");


            string dgv1_sortby_col_nme = dataGridView1.SortedColumn != null ? dataGridView1.SortedColumn.Name : "NME";
            Products_Bind.Sort = dgv1_sortby_col_nme + " ASC";
            ///////////dataGridView1.DataSource = Products_Bind;
            dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
            //////////////////dataGridView1.Sort(dataGridView1.Columns[dgv1_sortby_col_nme], System.ComponentModel.ListSortDirection.Ascending);

            dataGridView1.ClearSelection();
            if (is_prod_insert)
            {
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                if (dataGridView1.Rows.Count > 0 && Products != null)
                {
                    var maxIdRow = dataGridView1.Rows.Cast<DataGridViewRow>()
                                    .OrderByDescending(r => Convert.ToInt32(r.Cells["ID"].Value))
                                    .FirstOrDefault();
                    if (maxIdRow != null)
                    {
                        maxIdRow.Selected = true;
                    }
                }
            }
            else
            {
                if (dataGridView1.Rows.Cast<DataGridViewRow>().Where(P => (int)P.Cells["ID"].Value == prec_select_id).Count() > 0)
                {
                    dataGridView1.Rows.Cast<DataGridViewRow>().Where(P => (int)P.Cells["ID"].Value == prec_select_id).First().Selected = true;
                }
                else if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Selected = true;

                }
                else
                {
                    initial_details_fields();
                }
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            }


            if (dgv1_first_shown_rw_idx > -1)
            {
                if (dataGridView1.Rows[dgv1_first_shown_rw_idx] != null)
                {
                    dataGridView1.FirstDisplayedCell = dataGridView1.Rows[dgv1_first_shown_rw_idx].Cells["CODE"];
                }
            }

            textBox1_TextChanged(null, null);
            //-------- II\- STOCKS ------------------------

            Stock = PreConnection.Load_data("SELECT tb1.*,CASE WHEN tb2.SLD IS NULL THEN 0.00 ELSE tb2.SLD END AS SLD FROM (SELECT tb1.`ID`,tb1.`OP_DATE`,tb1.`PROD_ID`,tb2.`CODE`,tb2.`NME`,tb1.`OBSERV`,IF(tb1.`QNT_IN` > 0, tb1.`QNT_IN`,tb1.QNT_OUT * -1) AS QNT FROM tb_stock_mouv tb1 LEFT JOIN tb_produits tb2 ON tb2.`ID` = tb1.`PROD_ID`) tb1 LEFT JOIN (SELECT `PROD_ID`,SUM(`QNT_IN`) - SUM(`QNT_OUT`) AS SLD FROM tb_stock_mouv GROUP BY `PROD_ID`) tb2 ON tb1.`PROD_ID` = tb2.`PROD_ID` ORDER BY `OP_DATE` DESC, `ID` DESC;");
            Stock_Bind.DataSource = Stock ?? null;


            radioButton1.Checked = radiobtn1_checked;
            radioButton1_CheckedChanged(null, null);

            dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
            dataGridView2.ClearSelection();
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;


            if (is_stock_insert)
            {
                if (dataGridView2.Rows.Count > 0 && Stock != null)
                {
                    var maxIdRow = dataGridView2.Rows.Cast<DataGridViewRow>()
                                    .OrderByDescending(r => Convert.ToInt32(r.Cells["ID2"].Value))
                                    .FirstOrDefault();

                    if (maxIdRow != null)
                    {
                        maxIdRow.Selected = true;
                    }

                }
            }
            else
            {
                if (dataGridView2.Rows.Cast<DataGridViewRow>().Where(P => (int)P.Cells["ID2"].Value == prec_dgv2_select).Count() > 0)
                {
                    dataGridView2.Rows.Cast<DataGridViewRow>().Where(P => (int)P.Cells["ID2"].Value == prec_dgv2_select).First().Selected = true;
                }
                else if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows[0].Selected = true;

                }
                else
                {
                    initial_stock_fields();
                }



                //if (dataGridView2.Rows.Count > prec_dgv2_select)
                //{
                //    dataGridView2.Rows[prec_dgv2_select].Selected = true;
                //}
                //else if (dataGridView2.Rows.Count > 0)
                //{
                //    dataGridView2.Rows[0].Selected = true;

                //}
                //else
                //{
                //    initial_stock_fields();
                //}
            }
            if (radioButton2.Checked)
            {
                dataGridView1_SelectionChanged(null, null);
            }
            if (dgv2_first_shown_rw_idx > -1)
            {
                if (dataGridView2.Rows[dgv2_first_shown_rw_idx] != null)
                {
                    dataGridView2.FirstDisplayedCell = dataGridView2.Rows[dgv2_first_shown_rw_idx].Cells["OP_DATE"];
                }
            }
            //----------------
            panel5.Visible = false;
        }

        //////////////private void load_stocks(bool is_insert)
        //////////////{
        //////////////    int prec_select = 0;
        //////////////    if (dataGridView2.SelectedRows.Count > 0)
        //////////////    {
        //////////////        prec_select = dataGridView2.SelectedRows[0].Index;
        //////////////    }
        //////////////    Stock = PreConnection.Load_data("SELECT tb1.*,tb2.SLD FROM (SELECT tb1.`ID`,tb1.`OP_DATE`,tb1.`PROD_ID`,tb2.`CODE`,tb2.`NME`,tb1.`OBSERV`,IF(tb1.`QNT_IN` > 0, tb1.`QNT_IN`,tb1.QNT_OUT * -1) AS QNT FROM tb_stock_mouv tb1 LEFT JOIN tb_produits tb2 ON tb2.`ID` = tb1.`PROD_ID`) tb1 LEFT JOIN (SELECT `PROD_ID`,SUM(`QNT_IN`) - SUM(`QNT_OUT`) AS SLD FROM tb_stock_mouv GROUP BY `PROD_ID`) tb2 ON tb1.`PROD_ID` = tb2.`PROD_ID` ORDER BY `OP_DATE` DESC, `ID` DESC;");

        //////////////    if (Stock != null)
        //////////////    {
        //////////////        dataGridView2.DataSource = Stock;
        //////////////    }



        //////////////    radioButton1_CheckedChanged(null, null);
        //////////////    dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
        //////////////    dataGridView2.ClearSelection();
        //////////////    dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;


        //////////////    if (is_insert)
        //////////////    {
        //////////////        if (dataGridView2.Rows.Count > 0 && Stock != null)
        //////////////        {
        //////////////            var maxIdRow = dataGridView2.Rows.Cast<DataGridViewRow>()
        //////////////                            .OrderByDescending(r => Convert.ToInt32(r.Cells["ID2"].Value))
        //////////////                            .FirstOrDefault();

        //////////////            if (maxIdRow != null)
        //////////////            {
        //////////////                maxIdRow.Selected = true;
        //////////////            }

        //////////////        }
        //////////////    }
        //////////////    else
        //////////////    {
        //////////////        if (dataGridView2.Rows.Count > prec_select)
        //////////////        {
        //////////////            dataGridView2.Rows[prec_select].Selected = true;
        //////////////        }
        //////////////        else if (dataGridView2.Rows.Count > 0)
        //////////////        {
        //////////////            dataGridView2.Rows[0].Selected = true;

        //////////////        }
        //////////////        else
        //////////////        {
        //////////////            initial_stock_fields();
        //////////////        }
        //////////////    }
        //////////////    if (radioButton2.Checked)
        //////////////    {
        //////////////        dataGridView1_SelectionChanged(null, null);
        //////////////    }
        //////////////}

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            groupBox2.Visible = dataGridView1.SelectedRows.Count == 1 || radioButton1.Checked;
            panel2.Visible = dataGridView1.SelectedRows.Count == 1 || Products?.Rows.Count == 0 || Products == null;
            panel1.Visible = button8.Enabled = (dataGridView2.SelectedRows.Count == 0 && dataGridView1.SelectedRows.Count == 1) || dataGridView2.SelectedRows.Count == 1;
            prev_sld2 = 0;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //-------------------
                pictureBox1.Image = Properties.Resources.MODIF;
                //-----------------------------------                              
                dateTimePicker2.Value = (DateTime)dataGridView1.SelectedRows[0].Cells["TMP_FIRST_INSERT_DATE"].Value; //TMP_FIRST_INSERT_DATE
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["CODE"].Value.ToString(); //CODE
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["NME"].Value.ToString(); //NME
                comboBox2.SelectedItem = dataGridView1.SelectedRows[0].Cells["CATEGOR"].Value.ToString(); //CATEGOR                
                //numericUpDown4.Value = (decimal)dataGridView1.SelectedRows[0].Cells["QNT"].Value; //QNT                
                checkBox1.Checked = Convert.ToSByte(dataGridView1.SelectedRows[0].Cells["ALERT_MIN_ON"].Value) == 1; //ALERT_MIN_ON
                numericUpDown5.Value = (decimal)dataGridView1.SelectedRows[0].Cells["QNT_MIN"].Value; //QNT_MIN
                numericUpDown1.Value = (decimal)dataGridView1.SelectedRows[0].Cells["REVIENT_PRTICE"].Value; //REVIENT_PRTICE
                numericUpDown3.Value = (decimal)dataGridView1.SelectedRows[0].Cells["VENTE_PRICE"].Value; //VENTE_PRICE
                //----------
                Is_New_Product = false;
                radioButton2.Text = dataGridView1.SelectedRows[0].Cells["NME"].Value.ToString();
                textBox6.TextChanged -= textBox6_TextChanged;
                textBox6.Clear();
                textBox6.TextChanged += textBox6_TextChanged;
                bool tmmp = radioButton2.Checked;
                radioButton2.Checked = true;
                if (tmmp)
                {
                    radioButton1_CheckedChanged(null, null);
                }
                //--------------------
                if (Stock != null)
                {
                    var edd = Stock.Rows.Cast<DataRow>().Where(FF => int.Parse(FF["PROD_ID"].ToString()) == (int)dataGridView1.SelectedRows[0].Cells["ID"].Value).FirstOrDefault();

                    if (edd != null)
                    {

                        prev_sld2 = (decimal)edd["SLD"];

                    }
                }
                //numericUpDown4.Minimum = (numericUpDown4.Value - prev_sld2) > 0 ? (numericUpDown4.Value - prev_sld2) : 0;
            }
            else
            {
                initial_details_fields();
                radioButton2.Text = "--";
                radioButton1.Checked = true;
                //-------------------

            }

        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (dataGridView1.Rows[e.RowIndex].Selected)
                    {
                        dataGridView1_SelectionChanged(null, null);
                    }
                }

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            string ids_to_delete = string.Empty;
            dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row =>
            {
                cnt++;
                ids_to_delete += "," + row.Cells["ID"].Value;
            });
            ids_to_delete = cnt > 0 ? ids_to_delete.Substring(1, ids_to_delete.Length - 1) : "";
            if (cnt > 0)
            {
                if (MessageBox.Show("Êtes-vous sûr de supprimer ces [" + cnt + "] produits ?\n\nAttention :\nTous les dépôts concernés seront supprimés.", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    PreConnection.Excut_Cmd_personnel("DELETE FROM tb_produits WHERE `ID` IN (" + ids_to_delete + ")", null, null);
                    //PreConnection.Excut_Cmd(3, "tb_produits", null, null, "ID IN (@P_ID)", new List<string> { "P_ID" }, new List<object> { ids_to_delete });
                    //////////////////load_prods(false);
                    //////////////////load_stocks(false);
                    load_data(false, false);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            initial_stock_fields();
        }

        [Obsolete]
        private void button9_Click(object sender, EventArgs e)
        {

            DataTable tmp_deleted_qnt = new DataTable();
            tmp_deleted_qnt.Columns.Add("OP_ID", typeof(Int32));
            tmp_deleted_qnt.Columns.Add("PROD_ID", typeof(Int32));
            tmp_deleted_qnt.Columns.Add("DEL_QNT", typeof(Decimal));
            //------------------------------------
            int cntt = 0;
            int prod_cnt = 0;
            List<int> ids_to_del = new List<int>();
            dataGridView2.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row =>
            {

                if (!ids_to_del.Contains((int)row.Cells["ID2"].Value))
                {
                    cntt++;
                    ids_to_del.Add((int)row.Cells["ID2"].Value);
                }

                //////////////}
                DataRow tmp_del_rw = tmp_deleted_qnt.NewRow();
                tmp_del_rw[0] = row.Cells["ID2"].Value;
                tmp_del_rw[1] = row.Cells["PROD_ID"].Value;
                tmp_del_rw[2] = row.Cells["QNT2"].Value;
                tmp_deleted_qnt.Rows.Add(tmp_del_rw);


            });
            if (cntt > 0)
            {
                string alert_min_msg_txt = "";
                string alert_impossible_del = "";
                tmp_deleted_qnt.Rows.Cast<DataRow>().GroupBy(Y => Y["PROD_ID"]).ForEach(P =>
                {
                    prod_cnt++;
                    Products?.Rows.Cast<DataRow>().Where(R => (int)R["ID"] == (int)P.First()["PROD_ID"] && ((decimal)R["SLD"] - P.Sum(H => (decimal)H["DEL_QNT"])) <= (decimal)R["QNT_MIN"]).ForEach(E =>
                        {
                            if (((decimal)E["SLD"] - P.Sum(H => (decimal)H["DEL_QNT"])) < 0)
                            {
                                prod_cnt--;
                                cntt -= P.Count();

                                P.ForEach(F => ids_to_del.Remove((int)F["OP_ID"]));

                                alert_impossible_del += string.IsNullOrEmpty(alert_impossible_del) ? "La suppression des opérations suivantes est annulées car le solde sera négatif :\n\n" : "";
                                alert_impossible_del += "** [" + E["NME"] + "]\n (" + ((decimal)E["SLD"] - P.Sum(H => (decimal)H["DEL_QNT"])).ToString("# ##0.00") + ")\n   ------------------";
                            }
                            else
                            {
                                if ((SByte)E["ALERT_MIN_ON"] == 1 || ((decimal)E["SLD"] - P.Sum(H => (decimal)H["DEL_QNT"])) == 0)
                                {

                                    alert_min_msg_txt += string.IsNullOrEmpty(alert_min_msg_txt) ? "La quantité des produits suivants est minime ou nulle :\n\n" : "";
                                    alert_min_msg_txt += "** [" + E["NME"] + "]\nQnt: " + ((decimal)E["SLD"] - P.Sum(H => (decimal)H["DEL_QNT"])).ToString("# ##0.00") + "\nMin: " + ((decimal)E["QNT_MIN"]).ToString("# ##0.00") + "\n    ------------------";
                                }
                            }

                        });
                });

                if (!string.IsNullOrEmpty(alert_impossible_del))
                {
                    MessageBox.Show(alert_impossible_del, "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                if (cntt > 0)
                {
                    if (MessageBox.Show("Êtes-vous sûr de supprimer ces [" + ids_to_del.Count + "] opérations de [" + prod_cnt + "] produits ?", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string iddss = "";
                        ids_to_del.ForEach(G => iddss += "," + G);
                        iddss = iddss.Substring(1);
                        PreConnection.Excut_Cmd_personnel("DELETE FROM tb_stock_mouv WHERE `ID` IN (" + iddss + ")", null, null);
                        load_data(false, false);
                        //----------------
                        if (!string.IsNullOrEmpty(alert_min_msg_txt))
                        {
                            MessageBox.Show(alert_min_msg_txt, "Attention:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

            }




        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked && radioButton2.Text == "--" && dataGridView1.SelectedRows.Count > 0)
            {
                radioButton2.Text = dataGridView1.SelectedRows[0].Cells["NME"].Value.ToString();
            }
            //-----------------------
            textBox6_TextChanged(null, null);
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            label6.Visible = dataGridView2.SelectedRows.Count > 1;
            panel1.Visible = button8.Enabled = (dataGridView2.SelectedRows.Count == 0 && dataGridView1.SelectedRows.Count == 1) || dataGridView2.SelectedRows.Count == 1;

            if (dataGridView2.SelectedRows.Count == 1)
            {

                DataRow rw = Products.Rows.Cast<DataRow>().Where(XX => (int)XX["ID"] == (int)dataGridView2.SelectedRows[0].Cells["PROD_ID"].Value).FirstOrDefault();

                if (rw != null)
                {
                    //--------------
                    pictureBox2.Image = Properties.Resources.MODIF;
                    //-----------------------------------
                    dateTimePicker1.Value = (DateTime)dataGridView2.SelectedRows[0].Cells["OP_DATE"].Value; //OP_DATE
                                                                                                            //-----------------------
                    comboBox1.SelectedItem = (string)rw["CATEGOR"] == "--" ? "- Autre -" : rw["CATEGOR"];
                    comboBox3.SelectedValue = (int)rw["ID"];

                    textBox5.Text = (string)dataGridView2.SelectedRows[0].Cells["OBSERV"].Value;
                    //-----------------------
                    numericUpDown2.Minimum = -10000000000; //temprarly
                    numericUpDown2.Value = numericUpDown2_old_val = (decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value;
                    // load_sld();
                    //----------
                    Is_New_Stock = false;

                    
                }
                else
                {
                    initial_stock_fields();
                }



            }
            else if (dataGridView2.SelectedRows.Count > 1)
            {
                initial_stock_fields();
                //-----------------
                decimal sum = dataGridView2.SelectedRows.Cast<DataGridViewRow>()
               .Sum(row => Convert.ToDecimal(row.Cells["QNT2"].Value));

                label6.Text = "Sélection : " + sum.ToString("# ##0.00");
            }
            else
            {
                initial_stock_fields();
                //------------
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    DataRow rw = Products.Rows.Cast<DataRow>().Where(XX => (int)XX["ID"] == (int)dataGridView1.SelectedRows[0].Cells["ID"].Value).FirstOrDefault();

                    if (rw != null)
                    {
                        //--------------
                        //pictureBox2.Image = Properties.Resources.MODIF;
                        //-----------------------------------
                        // dateTimePicker1.Value = (DateTime)dataGridView2.SelectedRows[0].Cells["OP_DATE"].Value; //OP_DATE
                        //-----------------------
                        comboBox1.SelectedItem = (string)rw["CATEGOR"] == "--" ? "- Autre -" : rw["CATEGOR"];
                        comboBox3.SelectedValue = (int)rw["ID"];

                        // textBox5.Text = (string)dataGridView2.SelectedRows[0].Cells["OBSERV"].Value;
                        //-----------------------
                        numericUpDown2.Minimum = -10000000000; //temprarly
                        ////////numericUpDown2.Value = numericUpDown2_old_val = (decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value;
                        load_sld();
                        //----------
                        Is_New_Stock = true;
                    }
                    else
                    {
                        initial_stock_fields();
                    }
                }
            }


            //////////////////if (dataGridView2.SelectedRows.Count > 0)
            //////////////////{

            //////////////////    //-------------
            //////////////////    if (dataGridView2.SelectedRows.Count == 1)
            //////////////////    {

            //////////////////        //--------------
            //////////////////        DataRow rw = Products.Rows.Cast<DataRow>().Where(XX => (int)XX["ID"] == (int)dataGridView2.SelectedRows[0].Cells["PROD_ID"].Value).FirstOrDefault();
            //////////////////        if (rw != null)
            //////////////////        {
            //////////////////            //--------------
            //////////////////            pictureBox2.Image = Properties.Resources.MODIF;
            //////////////////            //-----------------------------------
            //////////////////            dateTimePicker1.Value = (DateTime)dataGridView2.SelectedRows[0].Cells["OP_DATE"].Value; //OP_DATE
            //////////////////                                                                                                    //-----------------------
            //////////////////            comboBox1.SelectedItem = (string)rw["CATEGOR"] == "--" ? "- Autre -" : rw["CATEGOR"];
            //////////////////            comboBox3.SelectedValue = (int)rw["ID"];

            //////////////////            textBox5.Text = (string)dataGridView2.SelectedRows[0].Cells["OBSERV"].Value;
            //////////////////            //-----------------------
            //////////////////            numericUpDown2.Minimum = -10000000000; //temprarly
            //////////////////            numericUpDown2.Value = numericUpDown2_old_val = (decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value;
            //////////////////            load_sld();

            //////////////////            //----------
            //////////////////            Is_New_Stock = false;
            //////////////////        }
            //////////////////        else
            //////////////////        {
            //////////////////            initial_stock_fields();
            //////////////////        }


            //////////////////    }
            //////////////////    else
            //////////////////    {
            //////////////////        //-----------------
            //////////////////        decimal sum = dataGridView2.SelectedRows.Cast<DataGridViewRow>()
            //////////////////       .Sum(row => Convert.ToDecimal(row.Cells["QNT2"].Value));

            //////////////////        label6.Text = "Sélection : " + sum.ToString("# ##0.00");
            //////////////////    }

            //////////////////}
            //////////////////else
            //////////////////{
            //////////////////    initial_stock_fields();
            //////////////////}
            //-----------------------------------
        }

        private void initial_stock_fields()
        {
            Is_New_Stock = true;
            pictureBox2.Image = Properties.Resources.NOUVEAU;
            ////////////////////////textBox5.Enabled = comboBox1.Enabled = comboBox3.Enabled = dateTimePicker1.Enabled = true;
            //-------------
            ////////DateTime minDate = DateTime.MinValue;
            ////////if (dataGridView1.SelectedRows.Count > 0)
            ////////{
            ////////    var gg = Stock.AsEnumerable().Where(row => row.Field<int>("PROD_ID") == (int)dataGridView1.SelectedRows[0].Cells["ID"].Value);
            ////////    if (gg.Any()) { minDate = gg.Min(row => row.Field<DateTime>("OP_DATE")); }
            ////////}
            ////////dateTimePicker1.MinDate = minDate;
            ////////dateTimePicker1.Value = DateTime.Now >= minDate ? DateTime.Now : minDate;



            //////dateTimePicker1.Value = DateTime.Now >= dateTimePicker1.MinDate ? DateTime.Now : dateTimePicker1.MinDate;
            //------------------------
            textBox5.Clear();


            //--------------------
            label25.Visible = false;
            label26.Visible = false;
            label10.ForeColor = SystemColors.ControlText;
            //---------------------
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
            comboBox3.TextChanged -= comboBox3_TextChanged;

            comboBox1.SelectedIndex = dataGridView1.SelectedRows.Count == 0 ? 0 : comboBox1.SelectedIndex;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                comboBox3.SelectedValue = dataGridView1.SelectedRows[0].Cells["ID"].Value;
            }
            comboBox3_TextChanged(null, null);

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            comboBox3.TextChanged += comboBox3_TextChanged;
            //-----------
            numericUpDown2.ValueChanged -= numericUpDown2_ValueChanged;
            numericUpDown2_old_val = 0;
            numericUpDown2.BackColor = SystemColors.Window;
            numericUpDown2.Minimum = prev_sld * -1;
            //numericUpDown2.Value = numericUpDown2.Minimum <= 0 ? 0 : numericUpDown2.Minimum;
            numericUpDown2.Value = numericUpDown2.Minimum > 0 ? numericUpDown2.Minimum : 0;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;

            if (!textBox1.Focused && !textBox6.Focused)
            {
                numericUpDown2.Focus();
                numericUpDown2.Select(0, 4);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<DataRow> lst = new List<DataRow>();
            if (comboBox1.SelectedIndex == 0) //Categorie - Tous -
            {
                lst = Products.Rows.Cast<DataRow>().ToList();
            }
            else if (comboBox1.SelectedIndex == 1)//Categorie - Autre -
            {
                lst = Products.Rows.Cast<DataRow>().Where(DD => DD["CATEGOR"].ToString() == "--" || DD["CATEGOR"] == DBNull.Value || DD["CATEGOR"].ToString().Trim().Length == 0).ToList();
            }
            else//Categorie - specifié -
            {
                lst = Products.Rows.Cast<DataRow>().Where(DD => DD["CATEGOR"].ToString() == comboBox1.Text).ToList();
            }
            //--------------
            if (lst.Count > 0)
            {
                comboBox3.DataSource = lst.CopyToDataTable();
                comboBox3.ValueMember = "ID";
                comboBox3.DisplayMember = "NME";
            }
            else
            {
                comboBox3.DataSource = null;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3_TextChanged(null, null);

        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            comboBox3.BackColor = SystemColors.Window;
            //--------------------------------
            if (comboBox3.SelectedValue == null && comboBox3.Text != string.Empty && comboBox3.DataSource != null)
            {
                var dd = ((DataTable)comboBox3.DataSource).Rows.Cast<DataRow>().Where(DD => DD["NME"].ToString() == comboBox3.Text).FirstOrDefault();
                if (dd != null)
                {
                    comboBox3.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
                    comboBox3.SelectedValue = (int)((DataRow)dd)["ID"];
                    comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
                }
            }
            //--------------------------------
            if (comboBox3.DataSource != null)
            {
                if (((DataTable)comboBox3.DataSource).Rows.Cast<DataRow>().Where(DD => DD["NME"].ToString() == comboBox3.Text).ToList().Count == 0)
                {
                    comboBox3.BackColor = Color.LightCoral;
                }
            }
            else
            {
                if (Products.Rows.Cast<DataRow>().Where(DD => DD["NME"].ToString() == comboBox3.Text).ToList().Count == 0)
                {
                    comboBox3.BackColor = Color.LightCoral;
                }
            }

            if (comboBox3.BackColor == SystemColors.Window && comboBox3.SelectedValue != null)
            {
                var edd = ((DataTable)comboBox3.DataSource).Rows.Cast<DataRow>().Where(FF => (int)FF["ID"] == (int)comboBox3.SelectedValue).FirstOrDefault();
                label20.Text = edd != null ? ((DataRow)edd)["CODE"].ToString() : "--";
            }
            else
            {
                label20.Text = "--";
            }
            load_sld();
        }



        private void load_sld()
        {
            label10.Visible = label9.Visible = label26.Visible = false;
            label10.Text = "--";
            prev_sld = 0;
            selected_prod_qnt_min = 0;
            selected_prod_alert_on_min = false;
            if (Products != null && comboBox3.SelectedValue != null && comboBox3.BackColor == SystemColors.Window)
            {
                //var edd = Stock.Rows.Cast<DataRow>().Where(FF => int.Parse(FF["PROD_ID"].ToString()) == (int)comboBox3.SelectedValue).FirstOrDefault();
                var edd = Products.Rows.Cast<DataRow>().Where(FF => int.Parse(FF["ID"].ToString()) == (int)comboBox3.SelectedValue).FirstOrDefault();
                if (edd != null)
                {
                    label10.Visible = label9.Visible = true;
                    prev_sld = (decimal)edd["SLD"];
                    selected_prod_qnt_min = (decimal)edd["QNT_MIN"];
                    selected_prod_alert_on_min = (SByte)edd["ALERT_MIN_ON"] == 1;
                    label10.Text = prev_sld.ToString("# ##0.00");
                    label26.Visible = prev_sld <= selected_prod_qnt_min;
                }
            }
            //numericUpDown2.Minimum = textBox5.Text == "Achat (Premier Stock)" ? ((numericUpDown2.Value - prev_sld) > 0 ? (numericUpDown2.Value - prev_sld) : 0) : numericUpDown2.Value - prev_sld;
            numericUpDown2.Minimum = numericUpDown2.Value - prev_sld;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool autorisat = Properties.Settings.Default.Last_login_is_admin || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30005" && (Int32)QQ[3] == 1).Count() > 0;
            if (autorisat)
            {
                string alert_min_msg_txt = "";
                bool alert_on = Products != null ? Products.Rows.Cast<DataRow>().Where(R => (int)R["ID"] == (int)comboBox3.SelectedValue && (SByte)R["ALERT_MIN_ON"] == 1).Count() > 0 : false;
                if (label26.Visible && alert_on)
                {
                    alert_min_msg_txt = "La quantité de produit\n[" + comboBox1.Text + " - " + comboBox3.Text + "]\n(" + label10.Text + ")\nest inférieure à la quantité minimale\n(" + selected_prod_qnt_min.ToString("# ##0.00") + ")";
                }
                //------------------------------------
                bool ready = true;
                ready &= comboBox3.SelectedValue != null && comboBox3.Text.Trim().Length > 0;
                ready &= numericUpDown2.BackColor != Color.LightCoral;
                ready &= numericUpDown2.Value != 0;
                //double mnnt = 0.00;
                //double.TryParse(numericUpDown2.Text.Trim().Replace(" ", ""), out mnnt);
                //ready &= mnnt != 0;
                //numericUpDown2.BackColor = mnnt != 0 ? numericUpDown2.BackColor : Color.LightCoral;
                if (ready)
                {
                    if (Is_New_Stock)
                    {
                        PreConnection.Excut_Cmd(1, "tb_stock_mouv", new List<string> {
                        "OP_DATE",
"PROD_ID",
"QNT_IN",
"QNT_OUT",
"OBSERV"}, new List<object>
{
    dateTimePicker1.Value,//OP_DATE
                                            comboBox3.SelectedValue,//PROD_ID
                                            (numericUpDown2.Value > 0 ? numericUpDown2.Value : 0), //QNT_IN
                                            (numericUpDown2.Value < 0 ? numericUpDown2.Value * -1 : 0), //QNT_OUT
                                            textBox5.Text //OBSERV
                    }, null, null, null);
                        //PreConnection.Excut_Cmd("INSERT INTO `tb_stock_mouv`"
                        //                    + "(`OP_DATE`,"
                        //                    + "`PROD_ID`,"
                        //                    + "`QNT_IN`,"
                        //                    + "`QNT_OUT`,"
                        //                    + "`OBSERV`)"
                        //                    + "VALUES"
                        //                    + "('" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"//OP_DATE
                        //                    + comboBox3.SelectedValue + ","//PROD_ID
                        //                    + (numericUpDown2.Value > 0 ? numericUpDown2.Value : 0) + ","//QNT_IN
                        //                    + (numericUpDown2.Value < 0 ? numericUpDown2.Value * -1 : 0) + ","//QNT_OUT
                        //                    + "'" + textBox5.Text.Replace("'", "''") + "');");//OBSERV
                    }
                    else
                    {
                        PreConnection.Excut_Cmd(2, "tb_stock_mouv", new List<string> {
                        "OP_DATE",
"PROD_ID",
"QNT_IN",
"QNT_OUT",
"OBSERV"}, new List<object>
{
    dateTimePicker1.Value,//OP_DATE
                                            comboBox3.SelectedValue,//PROD_ID
                                            (numericUpDown2.Value > 0 ? numericUpDown2.Value : 0), //QNT_IN
                                            (numericUpDown2.Value < 0 ? numericUpDown2.Value * -1 : 0), //QNT_OUT
                                            textBox5.Text //OBSERV
                    }, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView2.SelectedRows[0].Cells["ID2"].Value });
                        //PreConnection.Excut_Cmd("UPDATE `tb_stock_mouv` SET "
                        //                    + "`OP_DATE` = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"//OP_DATE
                        //                    + "`PROD_ID` = " + comboBox3.SelectedValue + ","//PROD_ID
                        //                    + "`QNT_IN` = " + (numericUpDown2.Value > 0 ? numericUpDown2.Value : 0) + ","//QNT_IN
                        //                    + "`QNT_OUT` = " + (numericUpDown2.Value < 0 ? numericUpDown2.Value * -1 : 0) + ","//QNT_OUT
                        //                    + "`OBSERV` = '" + textBox5.Text.Replace("'", "''") + "' "//OBSERV
                        //                    + "WHERE `ID` = " + dataGridView2.SelectedRows[0].Cells["ID2"].Value + ";");




                        //if (textBox5.Text == "Achat (Premier Stock)")
                        //{
                        //    PreConnection.Excut_Cmd_personnel("UPDATE tb_produits SET `QNT` = @Param_01 WHERE `ID` = @Param_002;", new List<string> { "Param_01", "Param_002" }, new List<object> { Convert.ToDouble(numericUpDown2.Value), comboBox3.SelectedValue });
                        //    //PreConnection.Excut_Cmd("UPDATE tb_produits SET `QNT` = " + Convert.ToDouble(numericUpDown2.Value) + " WHERE `ID` = " + comboBox3.SelectedValue + ";");
                        //    load_prods(false);
                        //}
                    }
                    //---------------------------
                    DataRow rww = Products.Rows.Cast<DataRow>().Where(G => (string)G["CATEGOR"] == comboBox1.Text && (string)G["NME"] == comboBox3.Text && (string)G["CODE"] == label20.Text).FirstOrDefault();
                    if (rww != null)
                    {
                        rww["SLD"] = decimal.Parse(label10.Text);
                    }
                    dataGridView1.Refresh();
                    //--------------------------
                    ////////////////load_stocks(Is_New_Stock);
                    load_data(false, Is_New_Stock);
                    //----------------------------
                    if (alert_min_msg_txt.Length > 0)
                    {
                        MessageBox.Show(alert_min_msg_txt, "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    //----------------------------

                }
                else if (numericUpDown2.Value == 0)
                {
                    MessageBox.Show("Veuillez entrer une quantité valide.\n(0.00 non accepté)", "Champs requis :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    numericUpDown2.Focus();
                    numericUpDown2.Select(0, 4);
                }
            }
            else
            {
                new Non_Autorized_Msg("").ShowDialog();
            }


        }


        private void dataGridView2_DataSourceChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count < 2)
            {
                decimal sum = dataGridView2.Rows.Cast<DataGridViewRow>()
                   .Sum(row => Convert.ToDecimal(row.Cells["QNT2"].Value));

                label21.Text = "Total : " + sum.ToString("# ##0.00");
            }

        }

        private void dataGridView2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            decimal tmpp = 0;
            decimal.TryParse(dataGridView2.Rows[e.RowIndex].Cells["QNT2"].Value.ToString().Replace(" ", ""), out tmpp);
            if (tmpp == 0)
            {
                dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView2.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Window;
            }
            else if (tmpp > 0)
            {
                dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView2.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(192, 255, 192);
            }
            else
            {
                dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView2.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 192, 192);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView2.DataSource != null)
            {
                if (textBox6.Text.Length > 0)
                {
                    if (radioButton2.Checked && dataGridView1.SelectedRows.Count > 0)
                    {
                        string ddd = "PROD_ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value;
                        ddd += " AND (Convert([OP_DATE], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([CODE], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([NME], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([OBSERV], System.String) LIKE '%{0}%')";
                        //////////////////((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = String.Format(ddd, textBox6.Text.Replace("'", "''"));
                        Stock_Bind.Filter = String.Format(ddd, textBox6.Text.Replace("'", "''"));
                    }
                    else
                    {
                        string ddd = "Convert([OP_DATE], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([CODE], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([NME], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([OBSERV], System.String) LIKE '%{0}%'";
                        //////////////////((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = String.Format(ddd, textBox6.Text.Replace("'", "''"));
                        Stock_Bind.Filter = String.Format(ddd, textBox6.Text.Replace("'", "''"));
                    }
                }
                else
                {
                    if (radioButton2.Checked && dataGridView1.SelectedRows.Count > 0)
                    {
                        /////////////((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "PROD_ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value;
                        Stock_Bind.Filter = "PROD_ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value;
                    }
                    else
                    {
                        /////////////((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "";
                        Stock_Bind.Filter = "";
                    }
                }

            }
            if (dataGridView2.SelectedRows.Count == 1)
            {
                decimal sum = dataGridView2.Rows.Cast<DataGridViewRow>().Sum(row => Convert.ToDecimal(row.Cells["QNT2"].Value));
                label21.Text = "Total : " + sum.ToString("# ##0.00");
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            label13.BackColor = SystemColors.Control;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.BackColor = SystemColors.Window;
        }

        //private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        //{
        //    //label6.Visible = numericUpDown4.Value <= numericUpDown4.Minimum && !Is_New_Product;
        //    label6.Visible = numericUpDown4.Value <= numericUpDown4.Minimum && numericUpDown4.Visible;
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ////////////////////if (dataGridView1.DataSource != null)
            ////////////////////{
            ////////////////////    string fltr = "";
            ////////////////////    if (radioButton4.Checked)
            ////////////////////    {
            ////////////////////        fltr += "[SLD] <= [QNT_MIN] AND (";
            ////////////////////    }
            ////////////////////    else if (radioButton5.Checked)
            ////////////////////    {
            ////////////////////        fltr += "[SLD] > [QNT_MIN] AND (";
            ////////////////////    }
            ////////////////////    fltr += "Convert([CODE], System.String) LIKE '%{0}%'";
            ////////////////////    fltr += " OR Convert([NME], System.String) LIKE '%{0}%'";
            ////////////////////    fltr += " OR Convert([CATEGOR], System.String) LIKE '%{0}%'";
            ////////////////////    fltr += radioButton4.Checked || radioButton5.Checked ? ")" : "";

            ////////////////////    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format(fltr, textBox1.Text.Replace("'", "''"));
            ////////////////////}
            ///

            if (Products_Bind.DataSource != null)
            {
                string fltr = "";
                if (radioButton4.Checked)
                {
                    fltr += "[SLD] <= [QNT_MIN] AND (";
                }
                else if (radioButton5.Checked)
                {
                    fltr += "[SLD] > [QNT_MIN] AND (";
                }
                fltr += "Convert([CODE], System.String) LIKE '%{0}%'";
                fltr += " OR Convert([NME], System.String) LIKE '%{0}%'";
                fltr += " OR Convert([CATEGOR], System.String) LIKE '%{0}%'";
                fltr += radioButton4.Checked || radioButton5.Checked ? ")" : "";

                Products_Bind.Filter = String.Format(fltr, textBox1.Text.Replace("'", "''"));
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            decimal diff = numericUpDown2.Value - numericUpDown2_old_val;
            decimal prev_sld_tmp = prev_sld + diff;
            //---------------------------
            if (label10.Visible)
            {
                label10.Text = (prev_sld_tmp).ToString("# ##0.00");
            }
            numericUpDown2.BackColor = prev_sld_tmp >= 0 ? (prev_sld_tmp == 0 ? Color.Khaki : SystemColors.Window) : Color.LightCoral;
            label10.ForeColor = prev_sld_tmp > 0 ? SystemColors.ControlText : Color.Red;
            //-----------------------
            label25.Visible = prev_sld_tmp == 0;
            //-----------------------
            label26.Visible = prev_sld_tmp <= selected_prod_qnt_min;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if ((dataGridView1.Rows.Count - (dataGridView1.AllowUserToAddRows ? 1 : 0)) > 0)
            {
                Excc.Application xcelApp = new Excc.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                xcelApp.Application.Workbooks[1].Title = "Produits";
                xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Produits";
                List<string> cols = new List<string>();
                //cols.AddRange(new string[] { "CODE", "NME", "CATEGOR", "QNT", "REVIENT_PRTICE", "VENTE_PRICE" });
                cols.AddRange(new string[] { "CODE", "NME", "CATEGOR", "REVIENT_PRTICE", "VENTE_PRICE" });
                dataGridView1.Columns.Cast<DataGridViewColumn>().Where(cc => cols.Contains(cc.Name)).ToList().ForEach(g =>
                {
                    xcelApp.Cells[1, (g.Index > 4 ? g.Index - 2 : g.Index)].Value = g.HeaderText.Replace("Prix de revient", "Prix -unitaire- de revient").Replace("Prix de vente", "Prix -unitaire- de vente"); ;
                    ((Excc.Range)xcelApp.Cells[1, (g.Index > 4 ? g.Index - 2 : g.Index)]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[1, (g.Index > 4 ? g.Index - 2 : g.Index)]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[1, (g.Index > 4 ? g.Index - 2 : g.Index)]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                    try
                    {
                        if (dataGridView1.Columns[g.Index].DefaultCellStyle.Format == "N2")
                        {
                            ((Excc.Range)xcelApp.Columns[(g.Index > 4 ? g.Index - 2 : g.Index)]).NumberFormat = "#,##0.00 [$Da-fr-dz]";
                        }
                        else if (dataGridView1.Columns[g.Index].DefaultCellStyle.Format.Contains("MM/yyyy"))
                        {
                            ((Excc.Range)xcelApp.Columns[(g.Index > 4 ? g.Index - 2 : g.Index)]).NumberFormat = "dd/MM/yyyy" + (dataGridView1.Columns[g.Index].DefaultCellStyle.Format.Contains("HH") ? " HH:mm:ss" : "");
                        }
                        else
                        {
                            ((Excc.Range)xcelApp.Columns[(g.Index > 4 ? g.Index - 2 : g.Index)]).NumberFormat = "@";
                        }
                    }
                    catch { }
                });


                dataGridView1.Rows.Cast<DataGridViewRow>().ToList().ForEach(t =>
                {
                    t.Cells.Cast<DataGridViewCell>().Where(cc => cols.Contains(cc.OwningColumn.Name)).ToList().ForEach(b =>
                    {
                        //xcelApp.Cells[t.Index + 2, (b.ColumnIndex > 4 ? b.ColumnIndex - 2 : b.ColumnIndex)].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : "";
                        if (b.ColumnIndex <= 4)
                        {
                            xcelApp.Cells[t.Index + 2, b.ColumnIndex].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().TrimStart().TrimEnd() : "";
                        }
                        else
                        {
                            xcelApp.Cells[t.Index + 2, b.ColumnIndex - 2].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? Convert.ToDouble(dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value) : 0;
                        }

                    });

                });

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
                //Marshal.ReleaseComObject(xcelApp.Application.Workbooks[1]);
                xcelApp.Quit();
                //-------------------
            }
            else
            {
                MessageBox.Show("Aucun donnés !", ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Selected)
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells.Count > dataGridView1.Columns["SLD_REAL"].Index)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells["QNT_MIN"].Value != DBNull.Value && dataGridView1.Rows[e.RowIndex].Cells["SLD_REAL"].Value != DBNull.Value)
                {
                    if ((decimal)dataGridView1.Rows[e.RowIndex].Cells["QNT_MIN"].Value >= (decimal)dataGridView1.Rows[e.RowIndex].Cells["SLD_REAL"].Value)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>().ForEach(Z => e.CellStyle.BackColor = e.CellStyle.SelectionBackColor = Color.Yellow);
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>().ForEach(Z => e.CellStyle.BackColor = e.CellStyle.SelectionBackColor = Color.White);
                    }
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>().ForEach(Z => e.CellStyle.BackColor = e.CellStyle.SelectionBackColor = Color.White);
                }
            }

            //-------------------
            if (dataGridView1.Rows[e.RowIndex].Selected)
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
            }



        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if ((decimal)dataGridView1.Rows[e.RowIndex].Cells["QNT_MIN"].Value >= (decimal)dataGridView1.Rows[e.RowIndex].Cells["SLD_REAL"].Value)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Yellow;
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.White;
            }

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox1_TextChanged(null, null);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                textBox1_TextChanged(null, null);
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                textBox1_TextChanged(null, null);
            }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Interval = 100;
            ttimee = 0;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ttimee++;
            if (ttimee > 6)
            {
                numericUpDown2.Value += 1;
                if (ttimee > 18 && timer1.Interval > 40)
                {
                    timer1.Interval = 40;
                }

                if (ttimee > 30 && timer1.Interval > 1)
                {
                    timer1.Interval = 1;
                }
            }

        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {

            timer1.Stop();
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            timer2.Interval = 100;
            ttimee2 = 0;
            timer2.Start();

        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            timer2.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ttimee2++;
            if (ttimee2 > 6)
            {
                button2_Click(null, null);
                if (ttimee2 > 18 && timer2.Interval > 40)
                {
                    timer2.Interval = 40;
                }

                if (ttimee2 > 30 && timer2.Interval > 1)
                {
                    timer2.Interval = 1;
                }
            }
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                //--------------------
                decimal sum = dataGridView2.Rows.Cast<DataGridViewRow>()
               .Sum(row => Convert.ToDecimal(row.Cells["QNT2"].Value));
                label21.Text = "Total : " + sum.ToString("# ##0.00");
                //-------------------
            }
            else
            {
                label21.Text = "Total : --";
            }

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            dataGridView1.Refresh();
            panel1.Refresh();
            panel2.Refresh();
        }


    }
}


