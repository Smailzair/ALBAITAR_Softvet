using ALBAITAR_Softvet.Dialogs;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
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
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excc = Microsoft.Office.Interop.Excel;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Vente : Form
    {
        static public DataGridViewRow selected_item = null;
        static public DataTable stock_to_modify = new DataTable();
        decimal TVA_percent = 9;
        DataTable clients;
        DataTable factures;
        public Vente()
        {
            InitializeComponent();
            //-----------------------
            clients = PreConnection.Load_data("SELECT `ID`,CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS FULL_NME FROM tb_clients;");
            comboBox1.DataSource = clients;
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";            
            //----------------------
            if(stock_to_modify.Columns.Count == 0)
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
            stock_to_modify.Rows.Clear();
            selected_item = new DataGridViewRow();
            TVA_percent = decimal.Parse(Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 5).Select(QQ => QQ["VAL"]).First().ToString());
            dataGridView3.Rows[1].Cells[0].Value = "TVA (" + TVA_percent.ToString("N2") + " %):";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow rwx in dataGridView2.SelectedRows)
            {
                stock_to_modify.Rows.Cast<DataRow>().Where(x => x["PROD_CODE"].ToString() == rwx.Cells["PRODUCT_CODE"].Value.ToString()).ToList().ForEach(x => x.Delete());
                dataGridView2.Rows.Remove(rwx);
            }
            calcul_bill_tot();
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


        }

        private void Vente_Load(object sender, EventArgs e)
        {
            load_factures();
        }

        private void load_factures()
        {
            factures = PreConnection.Load_data("SELECT * FROM tb_factures_vente;");
            dataGridView1.DataSource = factures;
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
            msg_txt += "\n- Aucun élement dans la facture.";
            //------------
            All_Ready &= !label3.Visible;

            //-----------------
            if (All_Ready)
            {

            }
            else if(msg_txt.Length > 0)
            {
                MessageBox.Show(msg_txt,"Vérifiez s'il vous plaît:",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
            }
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = numericUpDown1.Value.ToString("0000");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            int dd = 1;
            if (int.TryParse(textBox2.Text, out dd))
            {
                if(dd <= 9999)
                {
                    numericUpDown1.Value = dd;
                }
                else
                {                    
                    MessageBox.Show("N° de facture ne doit superieur à 9999.", "Ref. de facture :", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    numericUpDown1_ValueChanged(null, null);
                }                
            }
            else
            {
                MessageBox.Show("La Ref. ne doit pas vide.\nEt les 4 derniers chiffres de Ref. de facture n'accepte que les nombres.", "Ref. de facture :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numericUpDown1_ValueChanged(null, null);
            }
        }
    }
}

