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
            stock_to_modify.Columns.Add("QNT_DIMIN", typeof(decimal));
            //----------------------------
            selected_item = new DataGridViewRow();
            //foreach(DataGridViewColumn col in dataGridView2.Columns)
            //{
            //    selected_item.Cells.Add(new DataGridViewTextBoxCell());
            //}
            //--------------------------
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
    }
}

