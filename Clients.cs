using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Clients : Form
    {
        DataTable clients;
        DataTable sites;
        List<string> wilayaa;
        List<string> cities;
        bool Is_New = true;
        public Clients()
        {
            InitializeComponent();
            //----------------------
            Load_clients_from_DB();
            //---------------------
            sites = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_adresses;");
            wilayaa = new List<string>();
            cities = new List<string>();
            sites.Rows.Cast<DataRow>().ToList().ForEach(row =>
            {
                if (!wilayaa.Contains(row["WILAYA"])) { wilayaa.Add(row["WILAYA"].ToString()); }
                if (!cities.Contains(row["CITY"])) { cities.Add(row["CITY"].ToString()); }
                if (!comboBox2.Items.Contains(row["CITY"])) { comboBox2.Items.Add(row["CITY"].ToString()); }
            });
            comboBox3.DataSource = wilayaa;
            //------------------
            comboBox1.SelectedIndex = 0;
        }
        private void Load_clients_from_DB()
        {
            clients = PreConnection.Load_data_keeping_duplicates("SELECT *,concat(FAMNME,' ',NME) AS FULL_NME FROM tb_clients;");
            dataGridView1.DataSource = clients;

        }
        private void Load_selected_client_fields()
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                label13.Visible = false;
                textBox2.Validated -= textBox2_Validated;
                textBox3.Validated -= textBox2_Validated;
                comboBox3.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
                comboBox3.TextUpdate -= comboBox3_TextUpdate;
                comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
                textBox6.TextChanged -= textBox6_TextChanged;
                textBox6.Validating -= textBox6_Validating;
                textBox6.Validated -= textBox6_Validated;
                maskedTextBox1.Enter -= maskedTextBox1_Enter;
                maskedTextBox1.Validating -= maskedTextBox1_Validating;
                textBox7.TextChanged -= textBox7_TextChanged;
                textBox7.Validating -= textBox7_Validating;
                //----------------------------------------------
                comboBox1.SelectedItem = dataGridView1.SelectedRows[0].Cells["SEX"].Value.ToString();
                //----------------------------------------------
                textBox2.Validated += textBox2_Validated;
                textBox3.Validated += textBox2_Validated;
                comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
                comboBox3.TextUpdate += comboBox3_TextUpdate;
                comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
                textBox6.TextChanged += textBox6_TextChanged;
                textBox6.Validating += textBox6_Validating;
                textBox6.Validated += textBox6_Validated;
                maskedTextBox1.Enter += maskedTextBox1_Enter;
                maskedTextBox1.Validating += maskedTextBox1_Validating;
                textBox7.TextChanged += textBox7_TextChanged;
                textBox7.Validating += textBox7_Validating;
            }
            
        }
        private void verif_if_déja_exist_client()
        {
            if (textBox2.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                int cnt = clients.Rows.Cast<DataRow>().Where(zz => zz["FAMNME"].ToString().ToLower().Equals(textBox2.Text.ToLower()) && zz["NME"].ToString().ToLower().Equals(textBox3.Text.ToLower()) && (textBox4.Text.Length > 0 ? zz["NME"].ToString().Equals(textBox4.Text) : true)).ToList().Count();
                label13.Visible = Is_New ? (cnt > 0) : (cnt > 1);
            }
            else { label13.Visible = false; }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            PreConnection.search_filter_datagridview(dataGridView1, textBox1.Text);
        }

        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            if ((!int.TryParse(textBox6.Text.TrimStart().TrimEnd(), out int ff) && textBox6.Text.Length > 0))
            {
                e.Cancel = true;
                textBox6.BackColor = Color.LightCoral;
            }
            else
            {
                textBox6.BackColor = SystemColors.Window;
            }

        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            textBox6.BackColor = SystemColors.Window;
            if (textBox6.Text == string.Empty) { comboBox2_SelectedIndexChanged(null, null); }
        }
        bool textbx6_typed = false;
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox6.BackColor = SystemColors.Window;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3_Validating(null, null);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<DataRow> ze = sites.Rows.Cast<DataRow>().Where(dd => dd["CITY"].Equals(comboBox2.Text)).ToList();
            if ((!textbx6_typed || textBox6.Text == string.Empty) && ze.Count > 0)
            {
                textBox6.Text = ze[0]["ZIP_CODE"].ToString();
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbx6_typed = true;
        }

        private void comboBox3_Validating(object sender, CancelEventArgs e)
        {
            List<string> tmmp = new List<string>();
            sites.Rows.Cast<DataRow>().Where(dda => dda["WILAYA"].Equals(comboBox3.Text)).ToList().ForEach(row =>
            {
                if (!tmmp.Contains(row["CITY"])) { tmmp.Add(row["CITY"].ToString()); }
            });
            //------------------
            string dd = comboBox2.Text;
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(tmmp.Count > 0 ? tmmp.ToArray() : cities.ToArray());
            comboBox2.SelectedIndex = comboBox2.Items.Contains(dd) ? comboBox2.Items.IndexOf(dd) : (comboBox2.Items.Count > 0 ? 0 : -1);
        }

        private void comboBox3_TextUpdate(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_Validating(object sender, CancelEventArgs e)
        {
            if (maskedTextBox1.Text != string.Empty && int.TryParse(maskedTextBox1.Text, out int jh) && !maskedTextBox1.Text.StartsWith("+"))
            {
                if (maskedTextBox1.Text.Length == 10)
                {
                    maskedTextBox1.Mask = "0000 00 00 00";
                }
                else if (maskedTextBox1.Text.Length == 9)
                {
                    maskedTextBox1.Mask = "000 00 00 00";
                }
                else
                {
                    maskedTextBox1.Mask = "";
                }
            }
            else
            {
                maskedTextBox1.Mask = "";
            }
        }

        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            maskedTextBox1.Mask = "";
        }

        private void textBox7_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBox7.Text, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z") && textBox7.Text.Length > 0)
            {
                e.Cancel = true;
                textBox7.BackColor = Color.LightCoral;
            }
            else
            {
                textBox7.BackColor = SystemColors.Window;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            bool all_ready = true;
            textBox2.BackColor = textBox2.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
            textBox3.BackColor = textBox3.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
            all_ready &= textBox2.Text.TrimStart().TrimEnd() != string.Empty;
            all_ready &= textBox3.Text.TrimStart().TrimEnd() != string.Empty;
            //-------------
            label12.Visible = !all_ready;
            //-------------
            if (all_ready)
            {
                if (Is_New) //INSERT
                {
                    //Debug.WriteLine("INSERT INTO `tb_clients` "
                    //        + "(`SEX`,"
                    //        + "`FAMNME`,"
                    //        + "`NME`,"
                    //        + "`NUM_CNI`,"
                    //        + "`ADRESS`,"
                    //        + "`POSTAL_CODE`,"
                    //        + "`CITY`,"
                    //        + "`WILAYA`,"
                    //        + "`NUM_PHONE`,"
                    //        + "`EMAIL`,"
                    //        + "`OBSERVATIONS`)"
                    //        + "VALUES"
                    //        + "('" + comboBox1.Text + "',"
                    //        + "'" + textBox2.Text + "',"
                    //        + "'" + textBox3.Text + "',"
                    //        + "'" + textBox4.Text + "',"
                    //        + "'" + textBox5.Text + "',"
                    //        + "'" + textBox6.Text + "',"
                    //        + "'" + comboBox2.Text + "',"
                    //        + "'" + comboBox3.Text + "',"
                    //        + "'" + maskedTextBox1.Text + "',"
                    //        + "'" + textBox7.Text + "',"
                    //        + "'" + textBox8.Text + "');");
                    PreConnection.Excut_Cmd("INSERT INTO `tb_clients` "
                            + "(`SEX`,"
                            + "`FAMNME`,"
                            + "`NME`,"
                            + "`NUM_CNI`,"
                            + "`ADRESS`,"
                            + "`POSTAL_CODE`,"
                            + "`CITY`,"
                            + "`WILAYA`,"
                            + "`NUM_PHONE`,"
                            + "`EMAIL`,"
                            + "`OBSERVATIONS`)"
                            + "VALUES"
                            + "('" + comboBox1.Text + "',"
                            + "'" + textBox2.Text + "',"
                            + "'" + textBox3.Text + "',"
                            + "'" + textBox4.Text + "',"
                            + "'" + textBox5.Text + "',"
                            + "'" + textBox6.Text + "',"
                            + "'" + comboBox2.Text + "',"
                            + "'" + comboBox3.Text + "',"
                            + "'" + maskedTextBox1.Text + "',"
                            + "'" + textBox7.Text + "',"
                            + "'" + textBox8.Text + "');");
                }
                else //UPDATE
                {

                }
                //----------------
                Load_clients_from_DB();
            }

        }

        private void label12_VisibleChanged(object sender, EventArgs e)
        {
            if (label12.Visible)
            {
                Timer tmr = new Timer();
                tmr.Interval = 1000;
                tmr.Tick += Tmr_Tick;
                tmr.Start();
            }
        }

        int timm = 0;
        private void Tmr_Tick(object sender, EventArgs e)
        {
            timm++;
            if (timm >= 3)
            {
                label12.Visible = false;
                timm = 0;
                ((Timer)sender).Stop();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
            label13.Visible = false;
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            verif_if_déja_exist_client();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Is_New = false;
            Load_selected_client_fields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            Is_New = true;
            textBox2.Select();
        }
    }
}
