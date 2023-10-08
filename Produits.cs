using ALBAITAR_Softvet.Dialogs;
using MySql.Data.MySqlClient;
using ServiceStack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excc = Microsoft.Office.Interop.Excel;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Produits : Form
    {
        bool Is_New_Product = true;
        bool Is_New_Stock = true;
        DataTable Products;
        DataTable Stock;
        decimal prev_sld = 0;
        decimal prev_sld2 = 0;
        decimal numericUpDown2_old_val = 0;
        public Produits()
        {
            InitializeComponent();
            //----------------------
            numericUpDown2.Controls[0].Visible = false;
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
            load_prods(false);
            load_stocks(false);
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
                else if (!Is_New_Product)
                {
                    DateTime minDate = Stock.AsEnumerable()
                            .Where(row => row["PROD_ID"].ToString() == dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString() && row["OBSERV"].ToString() != "Achat (Premier Stock)")
                            .Min(row => row.Field<DateTime?>("OP_DATE")) ?? new DateTime(2999, 01, 01);
                    if (minDate != null)
                    {

                        label13.BackColor = dateTimePicker2.Value > minDate ? Color.LightCoral : SystemColors.Control;
                        if (label13.BackColor == Color.LightCoral)
                        {
                            MessageBox.Show("Dans les opérations de stock de cet produit, il y a une date avec une valeur supérieure à la date sélectionnée.\n\nVous avez deux choix :\n1- Changer la date à une valeur inférieure ou égale à '" + minDate.ToString("dd/MM/yyyy") + "'.\nOu\n2- Changer les dates d'opérations de stock de date '" + minDate.ToString("dd/MM/yyyy") + "' à '" + dateTimePicker2.Value.ToString("dd/MM/yyyy") + "' ou plus superieur.\n", "Probleme de date :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                numericUpDown1.BackColor = numericUpDown1.Value > 0 ? SystemColors.Window : Color.LightCoral;
                numericUpDown4.BackColor = numericUpDown4.Value > 0 ? numericUpDown4.BackColor : Color.LightCoral;

                //--------------------
                bool ready = true;
                ready &= label13.BackColor != Color.LightCoral;
                ready &= textBox2.BackColor != Color.LightCoral;
                ready &= textBox3.BackColor != Color.LightCoral;
                ready &= label24.Visible == false;
                ready &= numericUpDown4.Value > 0;
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
"QNT",
"ALERT_MIN_ON",
"QNT_MIN",
"REVIENT_PRTICE",
"VENTE_PRICE",
"TMP_FIRST_INSERT_DATE"}, new List<object>
{
     textBox2.Text, //CODE
                                              textBox3.Text, //NME
                                              comboBox2.SelectedItem, //CATEGOR
                                              Convert.ToDouble(numericUpDown4.Value), //QNT
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
"QNT",
"ALERT_MIN_ON",
"QNT_MIN",
"REVIENT_PRTICE",
"VENTE_PRICE",
"TMP_FIRST_INSERT_DATE"}, new List<object>
{
     textBox2.Text, //CODE
                                              textBox3.Text, //NME
                                              comboBox2.SelectedItem, //CATEGOR
                                              Convert.ToDouble(numericUpDown4.Value), //QNT
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
                    load_prods(Is_New_Product);
                    load_stocks(false);
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
            numericUpDown4.Minimum = 0;
            numericUpDown4.Value = 1;
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
            initial_details_fields();
        }

        private void load_prods(bool is_insert)
        {
            int prec_select = 0;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                prec_select = dataGridView1.SelectedRows[0].Index;
            }
            Products = PreConnection.Load_data("SELECT * FROM tb_produits;");
            dataGridView1.DataSource = Products;
            dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
            dataGridView1.ClearSelection();
            if (is_insert)
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
                if (dataGridView1.Rows.Count > prec_select)
                {
                    dataGridView1.Rows[prec_select].Selected = true;
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


        }

        private void load_stocks(bool is_insert)
        {
            int prec_select = 0;
            if (dataGridView2.SelectedRows.Count > 0)
            {
                prec_select = dataGridView2.SelectedRows[0].Index;
            }
            Stock = PreConnection.Load_data("SELECT tb1.*,tb2.SLD FROM (SELECT tb1.`ID`,tb1.`OP_DATE`,tb1.`PROD_ID`,tb2.`CODE`,tb2.`NME`,tb1.`OBSERV`,IF(tb1.`QNT_IN` > 0, tb1.`QNT_IN`,tb1.QNT_OUT * -1) AS QNT FROM tb_stock_mouv tb1 LEFT JOIN tb_produits tb2 ON tb2.`ID` = tb1.`PROD_ID`) tb1 LEFT JOIN (SELECT `PROD_ID`,SUM(`QNT_IN`) - SUM(`QNT_OUT`) AS SLD FROM tb_stock_mouv GROUP BY `PROD_ID`) tb2 ON tb1.`PROD_ID` = tb2.`PROD_ID` ORDER BY `OP_DATE` DESC, `ID` DESC;");
            dataGridView2.DataSource = Stock;
            radioButton1_CheckedChanged(null, null);
            dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
            dataGridView2.ClearSelection();
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
            if (is_insert)
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
                if (dataGridView2.Rows.Count > prec_select)
                {
                    dataGridView2.Rows[prec_select].Selected = true;
                }
                else if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows[0].Selected = true;

                }
                else
                {
                    initial_stock_fields();
                }
            }
            dataGridView1_SelectionChanged(null, null);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            numericUpDown4.Minimum = prev_sld2 = 0;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //-------------------
                panel1.Enabled = button8.Enabled = true;
                //--------------
                pictureBox1.Image = Properties.Resources.MODIF;
                //-----------------------------------                              
                dateTimePicker2.Value = (DateTime)dataGridView1.SelectedRows[0].Cells["TMP_FIRST_INSERT_DATE"].Value; //TMP_FIRST_INSERT_DATE
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["CODE"].Value.ToString(); //CODE
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["NME"].Value.ToString(); //NME
                comboBox2.SelectedItem = dataGridView1.SelectedRows[0].Cells["CATEGOR"].Value.ToString(); //CATEGOR                
                numericUpDown4.Value = (decimal)dataGridView1.SelectedRows[0].Cells["QNT"].Value; //QNT                
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
                numericUpDown4.Minimum = (numericUpDown4.Value - prev_sld2) > 0 ? (numericUpDown4.Value - prev_sld2) : 0;
            }
            else
            {
                initial_details_fields();
                radioButton2.Text = "--";
                radioButton1.Checked = true;
                //-------------------
                panel1.Enabled = button8.Enabled = false;
            }

        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (dataGridView1.Rows[e.RowIndex].Selected)
                {
                    dataGridView1_SelectionChanged(null, null);
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
                    //PreConnection.Excut_Cmd("DELETE FROM tb_produits WHERE `ID` IN (" + ids_to_delete + ");");
                    PreConnection.Excut_Cmd(3, "tb_produits", null, null, "ID IN (@P_ID)", new List<string> { "P_ID" }, new List<object> { ids_to_delete });
                    load_prods(false);
                    load_stocks(false);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            initial_stock_fields();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int cntt = 0;
            string ids_to_delette = string.Empty;
            dataGridView2.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row =>
            {
                if ((string)row.Cells["OBSERV"].Value != "Achat (Premier Stock)")
                {
                    cntt++;
                    ids_to_delette += "," + row.Cells["ID2"].Value;
                }
            });
            ids_to_delette = cntt > 0 ? ids_to_delette.Substring(1, ids_to_delette.Length - 1) : "";
            if (cntt > 0)
            {
                if (MessageBox.Show("Êtes-vous sûr de supprimer ces [" + cntt + "] Mouvements ?", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //PreConnection.Excut_Cmd("DELETE FROM tb_stock_mouv WHERE `ID` IN (" + ids_to_delette + ");");
                    PreConnection.Excut_Cmd(3, "tb_produits", null, null, "ID IN (@P_ID)", new List<string> { "P_ID" }, new List<object> { ids_to_delette });
                    load_stocks(false);
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
            if (dataGridView2.SelectedRows.Count > 0)
            {
                if (dataGridView2.SelectedRows.Count == 1)
                {
                    //--------------------
                    decimal sum = dataGridView2.Rows.Cast<DataGridViewRow>()
                   .Sum(row => Convert.ToDecimal(row.Cells["QNT2"].Value));
                    label21.Text = "Total : " + sum.ToString("# ##0.00");
                    //--------------
                    DataRow rw = Products.Rows.Cast<DataRow>().Where(XX => (int)XX["ID"] == (int)dataGridView2.SelectedRows[0].Cells["PROD_ID"].Value).FirstOrDefault();
                    if (rw != null)
                    {
                        //--------------
                        pictureBox2.Image = Properties.Resources.MODIF;
                        //-----------------------------------
                        dateTimePicker1.Value = (DateTime)dataGridView2.SelectedRows[0].Cells["OP_DATE"].Value; //OP_DATE
                        comboBox1.SelectedItem = (string)rw["CATEGOR"] == "--" ? "- Autre -" : rw["CATEGOR"];
                        bool ttt = (int)comboBox3.SelectedValue == (int)rw["ID"];
                        comboBox3.SelectedValue = (int)rw["ID"];
                        if (ttt)
                        {
                            load_sld();
                        }
                        else
                        {
                            label9.Visible = label10.Visible = false;
                        }
                        textBox5.Text = (string)dataGridView2.SelectedRows[0].Cells["OBSERV"].Value;
                        //textBox4.Text = ((decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value).ToString("# ##0.00");

                        if (sum >= 0)
                        {
                            numericUpDown2.Minimum = textBox5.Text == "Achat (Premier Stock)" ? (((decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value - prev_sld) > 0 ? ((decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value - prev_sld) : 0) : (decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value - prev_sld;
                        }
                        else
                        {
                            numericUpDown2.Minimum = -10000000000;
                        }



                        numericUpDown2.Value = numericUpDown2_old_val = (decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value;
                        //-------------------------
                        textBox5.Enabled = comboBox1.Enabled = comboBox3.Enabled = dateTimePicker1.Enabled = !textBox5.Text.Equals("Achat (Premier Stock)");
                        //----------
                        Is_New_Stock = false;
                    }
                    else
                    {
                        initial_stock_fields();
                    }


                }
                else
                {
                    initial_stock_fields();
                    //-----------------
                    decimal sum = dataGridView2.SelectedRows.Cast<DataGridViewRow>()
                   .Sum(row => Convert.ToDecimal(row.Cells["QNT2"].Value));

                    label21.Text = "Total (Sélect.) : " + sum.ToString("# ##0.00");
                }

            }
            else
            {
                initial_stock_fields();
            }
            //-----------------------------------
            button9.Visible = dataGridView2.SelectedRows.Count != dataGridView2.SelectedRows.Cast<DataGridViewRow>().Count(row => (string)row.Cells["OBSERV"].Value == "Achat (Premier Stock)");
        }

        private void initial_stock_fields()
        {
            Is_New_Stock = true;
            pictureBox2.Image = Properties.Resources.NOUVEAU;
            textBox5.Enabled = comboBox1.Enabled = comboBox3.Enabled = dateTimePicker1.Enabled = true;
            //-------------
            DateTime minDate = new DateTime(1900, 01, 01);
            if (dataGridView1.SelectedRows.Count > 0)
            {
                minDate = Stock.AsEnumerable()
                                 .Where(row => row.Field<int>("PROD_ID") == (int)dataGridView1.SelectedRows[0].Cells["ID"].Value)
                                   .Min(row => row.Field<DateTime>("OP_DATE"));
            }
            dateTimePicker1.MinDate = minDate;
            dateTimePicker1.Value = DateTime.Now >= minDate ? DateTime.Now : minDate;
            //------------------------
            textBox5.Clear();
            comboBox1.SelectedIndex = dataGridView1.SelectedRows.Count == 0 ? 0 : comboBox1.SelectedIndex;

            //--------------------
            label25.Visible = false;
            label10.ForeColor = SystemColors.ControlText;
            //---------------------
            if (dataGridView1.SelectedRows.Count > 0)
            {
                comboBox3.SelectedValue = dataGridView1.SelectedRows[0].Cells["ID"].Value;
            }
            //-----------
            numericUpDown2.ValueChanged -= numericUpDown2_ValueChanged;
            numericUpDown2_old_val = 0;
            numericUpDown2.BackColor = SystemColors.Window;
            numericUpDown2.Minimum = prev_sld * -1;
            numericUpDown2.Value = numericUpDown2.Minimum <= 0 ? 0 : numericUpDown2.Minimum;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
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
            label10.Visible = label9.Visible = false;
            label10.Text = "--";
            prev_sld = 0;
            if (Stock != null && comboBox3.SelectedValue != null && comboBox3.BackColor == SystemColors.Window && dataGridView2.SelectedRows.Count > 0)
            {
                var edd = Stock.Rows.Cast<DataRow>().Where(FF => int.Parse(FF["PROD_ID"].ToString()) == (int)comboBox3.SelectedValue).FirstOrDefault();
                if (edd != null)
                {
                    label10.Visible = label9.Visible = true;
                    //if (Is_New_Stock)
                    //{
                    prev_sld = (decimal)edd["SLD"];
                    //}
                    //else
                    //{
                    //    prev_sld = (decimal)edd["SLD"] - (decimal)dataGridView2.SelectedRows[0].Cells["QNT2"].Value;
                    //}
                    label10.Text = prev_sld.ToString("# ##0.00");
                }
            }
            //textBox4_TextChanged(null, null);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //double des = 0.00;
            //double.TryParse(textBox4.Text.Trim().Replace(" ", ""), out des);
            //if ((des + (double)prev_sld) < 0)
            //{
            //    textBox4.Text = ((double)prev_sld * -1).ToString("# ##0.00");
            //}
            //else
            //{
            //    if (label10.Visible)
            //    {
            //        label10.Text = (des + (double)prev_sld).ToString("# ##0.00");
            //    }
            //    textBox4.BackColor = (des + (double)prev_sld) >= 0 ? ((des + (double)prev_sld) == 0 ? Color.Khaki : SystemColors.Window) : Color.LightCoral;
            //    label10.ForeColor = (des + (double)prev_sld) > 0 ? SystemColors.ControlText : Color.Red;
            //}

        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool autorisat = Properties.Settings.Default.Last_login_is_admin || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30005" && (Int32)QQ[3] == 1).Count() > 0;
            if (autorisat)
            {
                bool ready = true;
                ready &= comboBox3.SelectedValue != null && comboBox3.Text.Trim().Length > 0;
                ready &= numericUpDown2.BackColor != Color.LightCoral;
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
"OBSERV"},new List<object>
{
    dateTimePicker1.Value,//OP_DATE
                                            comboBox3.SelectedValue,//PROD_ID
                                            (numericUpDown2.Value > 0 ? numericUpDown2.Value : 0), //QNT_IN
                                            (numericUpDown2.Value < 0 ? numericUpDown2.Value * -1 : 0), //QNT_OUT
                                            textBox5.Text //OBSERV
                    },null,null,null);
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
                    }, "ID = @P_ID", new List<string> { "P_ID"}, new List<object> { dataGridView2.SelectedRows[0].Cells["ID2"].Value });
                        //PreConnection.Excut_Cmd("UPDATE `tb_stock_mouv` SET "
                        //                    + "`OP_DATE` = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"//OP_DATE
                        //                    + "`PROD_ID` = " + comboBox3.SelectedValue + ","//PROD_ID
                        //                    + "`QNT_IN` = " + (numericUpDown2.Value > 0 ? numericUpDown2.Value : 0) + ","//QNT_IN
                        //                    + "`QNT_OUT` = " + (numericUpDown2.Value < 0 ? numericUpDown2.Value * -1 : 0) + ","//QNT_OUT
                        //                    + "`OBSERV` = '" + textBox5.Text.Replace("'", "''") + "' "//OBSERV
                        //                    + "WHERE `ID` = " + dataGridView2.SelectedRows[0].Cells["ID2"].Value + ";");
                        if (textBox5.Text == "Achat (Premier Stock)")
                        {
                            PreConnection.Excut_Cmd_personnel("UPDATE tb_produits SET `QNT` = @Param_01 WHERE `ID` = @Param_002;",new List<string> { "Param_01","Param_002"},new List<object> { Convert.ToDouble(numericUpDown2.Value) , comboBox3.SelectedValue });
                            //PreConnection.Excut_Cmd("UPDATE tb_produits SET `QNT` = " + Convert.ToDouble(numericUpDown2.Value) + " WHERE `ID` = " + comboBox3.SelectedValue + ";");
                            load_prods(false);
                        }
                    }
                    load_stocks(Is_New_Stock);

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
                dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            else if (tmpp > 0)
            {
                dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);
            }
            else
            {
                dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 192);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView2.DataSource != null)
            {
                if (textBox6.Text.Length > 0)
                {
                    if (radioButton2.Checked)
                    {
                        string ddd = "PROD_ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value;
                        ddd += " AND (Convert([OP_DATE], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([CODE], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([NME], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([OBSERV], System.String) LIKE '%{0}%')";
                        ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = String.Format(ddd, textBox6.Text.Replace("'", "''"));
                    }
                    else
                    {
                        string ddd = "Convert([OP_DATE], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([CODE], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([NME], System.String) LIKE '%{0}%'";
                        ddd += " OR Convert([OBSERV], System.String) LIKE '%{0}%'";
                        ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = String.Format(ddd, textBox6.Text.Replace("'", "''"));
                    }
                }
                else
                {
                    if (radioButton2.Checked)
                    {
                        ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "PROD_ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value;
                    }
                    else
                    {
                        ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "";
                    }
                }

            }
            if (dataGridView2.SelectedRows.Count < 2)
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
            numericUpDown1.BackColor = SystemColors.Control;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            label6.Visible = numericUpDown4.Value <= numericUpDown4.Minimum && !Is_New_Product;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null)
            {

                string ddd = "Convert([CODE], System.String) LIKE '%{0}%'";
                ddd += " OR Convert([NME], System.String) LIKE '%{0}%'";
                ddd += " OR Convert([CATEGOR], System.String) LIKE '%{0}%'";
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format(ddd, textBox1.Text.Replace("'", "''"));
            }
            else
            {
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "";
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
                cols.AddRange(new string[] { "CODE", "NME", "CATEGOR", "QNT", "REVIENT_PRTICE", "VENTE_PRICE" });
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
                        xcelApp.Cells[t.Index + 2, (b.ColumnIndex > 4 ? b.ColumnIndex - 2 : b.ColumnIndex)].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : "";
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
    }
}

