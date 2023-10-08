using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Small_New_Client : Form
    {
        DataTable clients;
        public Small_New_Client()
        {
            InitializeComponent();
            //---------------------
            comboBox1.SelectedIndex = 0;
            clients = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_clients;");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool all_ready = true;
            textBox2.BackColor = textBox2.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox3.BackColor = textBox3.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
            all_ready &= textBox2.Text.TrimStart().TrimEnd() != string.Empty;
            all_ready &= textBox3.Text.TrimStart().TrimEnd() != string.Empty;
            //-------------------------
            if (all_ready)
            {
                PreConnection.Excut_Cmd(1, "tb_clients",new List<string>
                {
                    "SEX",
                    "FAMNME",
                    "NME",
                    "NUM_CNI",
                    "ADRESS",
                    "POSTAL_CODE",
                    "CITY",
                    "WILAYA",
                    "NUM_PHONE",
                    "EMAIL",
                    "OBSERVATIONS"
                },new List<object>
                {
                    comboBox1.Text,
                    textBox3.Text,
                    textBox2.Text,
                    textBox4.Text,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "" },null,null,null
                );
                //----------------
                Close();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
            label13.Visible = false;
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                int cnt = clients.Rows.Cast<DataRow>().Where(zz => zz["FAMNME"].ToString().ToLower().Equals(textBox3.Text.ToLower()) && zz["NME"].ToString().ToLower().Equals(textBox2.Text.ToLower()) && zz["NUM_CNI"].ToString().ToLower().Equals(textBox2.Text.ToLower())).ToList().Count();
                label13.Visible = cnt > 0;
            }
            else { label13.Visible = false; }
        }
    }
}
