using ALBAITAR_Softvet.Dialogs;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Fpe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excc = Microsoft.Office.Interop.Excel;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Produits : Form
    {
        bool Is_New_Product = true;
        bool Is_New_Stock = true;
        DataTable Products;
        DataTable Stock;
        public Produits()
        {
            InitializeComponent();
            //----------------------
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double des = 0.00;
            bool ssq = textBox4.Text.Contains("-");
            double.TryParse(textBox4.Text.Trim().Replace("-", ""), out des);
            des = des * (ssq ? -1 : 1) + 1;
            textBox4.Text = des.ToString("# ##0.00");
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (textBox4.Text.Trim() == string.Empty) { textBox4.Text = "0"; }
            textBox4.Text = textBox4.Text.Replace(",", ".");
            double dd = 0.00;
            bool sss = textBox4.Text != string.Empty && !double.TryParse(textBox4.Text.Replace("-", ""), out dd);
            if (sss)
            {
                e.Cancel = true;
                textBox4.BackColor = Color.LightCoral;                
                textBox4.SelectAll();
            }
            else
            {
                textBox4.Text = dd.ToString("# ##0.00");
                textBox4.BackColor = SystemColors.Window;
            }
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double des = 0.00;
            bool ssq = textBox4.Text.Contains("-");
            double.TryParse(textBox4.Text.Trim().Replace("-", ""), out des);
            des = des * (ssq ? -1 : 1) - 1;
            textBox4.Text = des.ToString("# ##0.00");

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
            load_stocks();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown5.Enabled = checkBox1.Checked;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.BackColor = textBox2.Text.Trim().Length > 0 ? SystemColors.Window : Color.LightCoral;
            textBox3.BackColor = textBox3.Text.Trim().Length > 0 ? SystemColors.Window : Color.LightCoral;
            numericUpDown1.BackColor = numericUpDown1.Value > 0 ? SystemColors.Window : Color.LightCoral;
            numericUpDown4.BackColor = numericUpDown4.Value > 0 ? SystemColors.Window : Color.LightCoral;
            //--------------------
            bool ready = true;
            ready &= textBox2.Text.Trim().Length > 0;
            ready &= textBox3.Text.Trim().Length > 0;
            ready &= numericUpDown4.Value > 0;
            ready &= numericUpDown1.Value > 0;
            if (ready)
            {
                if (Is_New_Product)
                {
                    PreConnection.Excut_Cmd("INSERT INTO `tb_produits`"
                                          + "(`CODE`,"
                                          + "`NME`,"
                                          + "`CATEGOR`,"
                                          + "`QNT`,"
                                          + "`ALERT_MIN_ON`,"
                                          + "`QNT_MIN`,"
                                          + "`REVIENT_PRTICE`,"
                                          + "`TAXES`,"
                                          + "`VENTE_PRICE`,"
                                          + "`TMP_FIRST_INSERT_DATE`)"
                                          + "VALUES"
                                          + "('"+textBox2.Text+"'," //CODE
                                          + "'"+textBox3.Text+"'," //NME
                                          + "'"+comboBox2.SelectedItem+"'," //CATEGOR
                                          + numericUpDown4.Value + "," //QNT
                                          + (checkBox1.Checked ? 1 : 0) + "," //ALERT_MIN_ON
                                          + numericUpDown5.Value + "," //QNT_MIN
                                          + numericUpDown1.Value + "," //REVIENT_PRTICE
                                          + numericUpDown2.Value + "," //TAXES
                                          + numericUpDown3.Value + "," //VENTE_PRICE
                                          + "'" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "');" //TMP_FIRST_INSERT_DATE
                                          );

                }
                else
                {
                    PreConnection.Excut_Cmd("UPDATE `tb_produits` SET "
                                          + "`CODE` = '"+textBox2.Text+"'," //CODE
                                          + "`NME` = '"+textBox3.Text+"'," //NME
                                          + "`CATEGOR` = '"+comboBox2.SelectedItem+"'," //CATEGOR
                                          + "`QNT` = "+ numericUpDown4.Value + "," //QNT
                                          + "`ALERT_MIN_ON` = "+ (checkBox1.Checked ? 1 : 0) + "," //ALERT_MIN_ON
                                          + "`QNT_MIN` = "+ numericUpDown5.Value + "," //QNT_MIN
                                          + "`REVIENT_PRTICE` = "+ numericUpDown1.Value + "," //REVIENT_PRTICE
                                          + "`TAXES` = "+ numericUpDown2.Value + "," //TAXES
                                          + "`VENTE_PRICE` = "+ numericUpDown3.Value //VENTE_PRICE
                                          + " WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value +";");
                }
                load_prods(Is_New_Product);
                load_stocks();
            }


        }

        private void initial_details_fields()
        {
            Is_New_Product = true;
            dateTimePicker2.Value = DateTime.Now;
            textBox2.Clear();
            textBox3.Clear();
            comboBox2.SelectedIndex = 0;
            numericUpDown1.Value = numericUpDown2.Value = numericUpDown3.Value = numericUpDown5.Value = 0;
            numericUpDown4.Value = 1;
            checkBox1.Checked = false;
            textBox2.BackColor = textBox3.BackColor = SystemColors.Window;
            pictureBox1.Image = Properties.Resources.NOUVEAU;
            radioButton2.Text = "--";
            radioButton1.Checked = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            initial_details_fields();
        }

        private void load_prods(bool is_insert)
        {
            int prec_select = 0;
            if(dataGridView1.SelectedRows.Count > 0)
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
                    dataGridView1.Rows[Products.Rows.Count - 1].Selected = true;

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

        private void load_stocks()
        {
            int prec_select = 0;
            if (dataGridView2.SelectedRows.Count > 0)
            {
                prec_select = dataGridView2.SelectedRows[0].Index;
            }
            Stock = PreConnection.Load_data("SELECT tb1.`ID`,tb1.`OP_DATE`,tb1.`PROD_ID`,tb2.`CODE`,tb2.`NME`,tb1.`OBSERV`,IF(tb1.`QNT_IN` > 0, tb1.`QNT_IN`,tb1.QNT_OUT) AS QNT,IF(tb1.QNT_IN > 0, 2,IF(tb1.QNT_OUT > 0, 1,0)) AS MOUV FROM tb_stock_mouv tb1 LEFT JOIN tb_produits tb2 ON tb2.`ID` = tb1.`PROD_ID`;");
            dataGridView2.DataSource = Stock;
            if (dataGridView2.Rows.Count > prec_select)
            {
                dataGridView2.Rows[prec_select].Selected = true;
            }
            radioButton1_CheckedChanged(null,null);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                
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
                numericUpDown2.Value = (decimal)dataGridView1.SelectedRows[0].Cells["TAXES"].Value; //TAXES
                numericUpDown3.Value = (decimal)dataGridView1.SelectedRows[0].Cells["VENTE_PRICE"].Value; //VENTE_PRICE
                //----------
                Is_New_Product = false;
                radioButton2.Text = dataGridView1.SelectedRows[0].Cells["NME"].Value.ToString();
                bool tmmp = radioButton2.Checked;
                radioButton2.Checked = true;
                if (tmmp)
                {
                    radioButton1_CheckedChanged(null,null);
                }
            }
            else
            {
                radioButton2.Text = "--";
                radioButton1.Checked = true;
            }
            
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Selected)
            {
                dataGridView1_SelectionChanged(null, null);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            string ids_to_delete = string.Empty;
            dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row => { 
                cnt++;
                ids_to_delete += "," + row.Cells["ID"].Value;
            });
            ids_to_delete = cnt > 0 ? ids_to_delete.Substring(1, ids_to_delete.Length - 1) : "";
            if(cnt > 0)
            {
                if(MessageBox.Show("Êtes-vous sûr de supprimer ces ["+cnt+ "] produits ?\n\nAttention : tous les dépôts concernés seront supprimés.","Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {                    
                    PreConnection.Excut_Cmd("DELETE FROM tb_produits WHERE `ID` IN ("+ids_to_delete+");");
                    load_prods(false);
                    load_stocks();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
            pictureBox2.Image = Properties.Resources.NOUVEAU;
            Is_New_Stock = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int cntt = 0;
            string ids_to_delette = string.Empty;
            dataGridView2.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row => {
                cntt++;
                ids_to_delette += "," + row.Cells["ID2"].Value;
            });
            ids_to_delette = cntt > 0 ? ids_to_delette.Substring(1, ids_to_delette.Length - 1) : "";
            if (cntt > 0)
            {
                if (MessageBox.Show("Êtes-vous sûr de supprimer ces [" + cntt + "] Mouvements ?", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PreConnection.Excut_Cmd("DELETE FROM tb_stock_mouv WHERE `ID` IN (" + ids_to_delette + ");");
                    load_stocks();
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(dataGridView2.DataSource != null)
            {
                if (radioButton2.Checked)
                {
                    ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "PROD_ID LIKE '" + dataGridView1.SelectedRows[0].Cells["ID"].Value + "'";
                }
                else
                {
                    ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "";
                }
            }
            
            
        }
    }
}

