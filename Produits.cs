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
            if(textBox4.Text.Trim() == string.Empty) { textBox4.Text = "0"; }
            bool sss = textBox4.Text != string.Empty && !double.TryParse(textBox4.Text.Replace("-",""), out double dd);
            e.Cancel = sss;
            textBox4.BackColor= sss ? Color.LightCoral : SystemColors.Window;
        }

        private void button2_Click(object sender, EventArgs e)
        {
                double des = 0.00;
            bool ssq = textBox4.Text.Contains("-");
                double.TryParse(textBox4.Text.Trim().Replace("-",""), out des);
            des = des * (ssq ? -1 : 1) - 1;
                textBox4.Text = des.ToString("# ##0.00");
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}

