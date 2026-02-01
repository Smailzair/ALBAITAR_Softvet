using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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
            clients = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_clients ORDER BY FAMNME;");
            if (clients.Rows.Count > 0)
            {
                int yy = (int)clients.Rows.Cast<DataRow>().Max(rr => rr["ID"]) + 1;
                textBox9.Text = yy.ToString("00000");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool all_ready = true;
            textBox2.BackColor = textBox2.Text.Trim() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox3.BackColor = textBox3.Text.Trim() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox9.BackColor = textBox9.Text.Trim() != string.Empty ? SystemColors.Window : Color.LightCoral;
            all_ready &= textBox2.Text.Trim() != string.Empty;
            all_ready &= textBox3.Text.Trim() != string.Empty;
            all_ready &= textBox9.Text.Trim() != string.Empty;
            all_ready &= !label13.Visible;
            all_ready &= !label15.Visible;
            //-------------------------
            if (all_ready)
            {
                PreConnection.Excut_Cmd(1, "tb_clients", new List<string>
                {
                    "REF",
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
                }, new List<object>
                {
                    textBox9.Text,
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
                    "" }, null, null, null
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
                int cnt = clients.Rows.Cast<DataRow>().Where(zz => zz["FAMNME"].ToString().ToLower().Equals(textBox3.Text.ToLower()) && zz["NME"].ToString().ToLower().Equals(textBox2.Text.ToLower()) && (string.IsNullOrWhiteSpace(textBox4.Text) || zz["NUM_CNI"].ToString().ToLower().Equals(textBox4.Text.ToLower()))).ToList().Count();
                label13.Visible = cnt > 0;
            }
            else { label13.Visible = false; }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
            label15.Visible = false;
        }

        private void textBox9_Validated(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(".", "").Replace(" ", "");
            verif_if_déja_exist_ref();
        }
        private void verif_if_déja_exist_ref()
        {
            if (textBox9.Text.Length > 0)
            {
                int cntt = clients.Rows.Cast<DataRow>().Where(zz => zz["REF"].ToString().ToLower().Equals(textBox9.Text.ToLower())).ToList().Count();

                label15.Visible = cntt > 0;
            }
            else { label15.Visible = false; }
            textBox9.BackColor = label15.Visible ? Color.LightCoral : SystemColors.Window;
        }
    }
}
