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
        public Vente()
        {
            InitializeComponent();
            //----------------------
            stock_to_modify.Columns.Add("PROD_ID", typeof(int));
            stock_to_modify.Columns.Add("PROD_CODE", typeof(string));
            stock_to_modify.Columns.Add("QNT_DIMIN", typeof(decimal));
            //----------------------------
            selected_item = new DataGridViewRow();
            //--------------------------
            dataGridView3.Rows.Add(new DataGridViewRow());
            dataGridView3.Rows.Add(new DataGridViewRow());
            dataGridView3.Rows.Add(new DataGridViewRow());            
            dataGridView3.Rows[0].Height = dataGridView3.Rows[1].Height = dataGridView3.Rows[2].Height = 30;
            dataGridView3.Rows[0].Cells[0].Value = "Total HT :";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selected_item = new DataGridViewRow();
            //------------------------
            new Add_Vente_Fact_Item().ShowDialog();
            //----------------------
            if(selected_item != null && selected_item.Cells.Count > 0)
            {
                dataGridView2.Rows.Add(selected_item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stock_to_modify.Rows.Clear();
            selected_item = new DataGridViewRow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow rwx in dataGridView2.SelectedRows)
            {
                stock_to_modify.Rows.Cast<DataRow>().Where(x => x["PROD_CODE"].ToString() == rwx.Cells["PRODUCT_CODE"].Value.ToString()).ToList().ForEach(x => x.Delete());
                dataGridView2.Rows.Remove(rwx);
            }
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            calcul_bill_tot();
        }

        private void calcul_bill_tot()
        {
            decimal tot = 0;
            foreach(DataGridViewRow rw in dataGridView2.Rows)
            {
                tot += rw.Cells["SLD"].Value != DBNull.Value ? (decimal)rw.Cells["SLD"].Value : 0;
            }                        
            dataGridView3.Rows[0].Cells[1].Value = tot;
        }
    }
}

