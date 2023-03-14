using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Login_Modif : Form
    {
        DataTable login_data;
        int User_Idd;
        bool In_Modif_Mod = true;
        //bool Current_Is_Admin = false;
        public Login_Modif(int? User_ID)
        {
            InitializeComponent();
            User_Idd = (User_ID ?? 0);
            button1.Enabled = radioButton1.Enabled = radioButton2.Enabled = textBox1.Enabled = button4.Enabled = Properties.Settings.Default.Last_login_is_admin;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            maskedTextBox1.UseSystemPasswordChar = maskedTextBox2.UseSystemPasswordChar = !maskedTextBox1.UseSystemPasswordChar;
        }
        private static bool IsValidEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            bool ready_to_save = false;
            ready_to_save = textBox1.TextLength > 0;
            textBox1.BackColor = textBox1.TextLength == 0 ? Color.LightCoral : SystemColors.Window;
            ready_to_save &= maskedTextBox1.Text.TrimEnd().TrimStart() == maskedTextBox2.Text.TrimEnd().TrimStart();
            ready_to_save &= (textBox2.TextLength > 0 && textBox3.TextLength > 0) || ((textBox2.TextLength + textBox3.TextLength) == 0);
            ready_to_save &= IsValidEmail(textBox4.Text);
            textBox4.BackColor = !IsValidEmail(textBox4.Text) ? Color.LightCoral : SystemColors.Window;
            
            if (In_Modif_Mod) //UPDATE
            {
                ready_to_save &= (dataGridView1.Rows
            .Cast<DataGridViewRow>()
             .Where(r => r.Cells["USER"].Value.ToString().Equals(textBox1.Text))
             .Count() <= 1);
                //----------------
                if (ready_to_save)
                {
                    PreConnection.Excut_Cmd("UPDATE LOGIN " +
                     "SET USER = '" + textBox1.Text + "'," +
                    "PASS = " + (maskedTextBox1.Text.TrimEnd().TrimStart().Length > 0 ? "'" + maskedTextBox1.Text.TrimEnd().TrimStart() + "'" : "NULL") + "," +
                     "QUEST = " + (textBox2.TextLength > 0 ? "'" + textBox2.Text + "'" : "NULL") + "," +
                    "ANSWER = " + (textBox3.TextLength > 0 ? "'" + textBox3.Text + "'" : "NULL") + "," +
                    "TYPE = '" + (radioButton1.Checked ? "ADMIN" : "STANDARD") + "'," +
                    "EMAIL = " + (textBox4.TextLength > 0 ? "'" + textBox4.Text + "'" : "NULL") +
                    " WHERE ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString() + ";");
                }
            }
            else //INSERT
            {
                ready_to_save &= (dataGridView1.Rows
            .Cast<DataGridViewRow>()
             .Where(r => r.Cells["USER"].Value.ToString().Equals(textBox1.Text))
             .Count() == 0);
                //----------------
                if (ready_to_save)
                {
                    PreConnection.Excut_Cmd("INSERT INTO LOGIN (USER,PASS,TYPE,EMAIL" + (textBox2.TextLength > 0 ? ",QUEST,ANSWER" : "") + ") VALUES ('" + textBox1.Text + "', " + (maskedTextBox1.Text.TrimEnd().TrimStart().Length > 0 ? "'" + maskedTextBox1.Text.TrimEnd().TrimStart() + "'" : "NULL") + ",'" + (radioButton1.Checked ? "ADMIN" : "STANDARD") + "','" + textBox4.Text + "'" + (textBox2.TextLength > 0 ? ",'" + textBox2.Text + "','" + textBox3.Text + "'" : "") + ")");
                }
            }
            //--------------------------
            if (ready_to_save)
            {
                MessageBox.Show("Bien enregistré !", "Success : ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                
                int previ_idx = -1;
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    previ_idx = dataGridView1.SelectedRows[0].Index;
                }
                login_data = PreConnection.Load_data("SELECT * FROM LOGIN;");
                dataGridView1.DataSource = login_data;
                if (previ_idx > -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[previ_idx].Selected = true;
                }
                //--------
                if(Properties.Settings.Default.Last_login_user_idx == User_Idd)
                {
                    Properties.Settings.Default.Last_login_user_nme = login_data.Rows.Cast<DataRow>().First(er => (int)er["ID"] == User_Idd).Field<string>("USER");
                    Properties.Settings.Default.Save();
                }
            }




        }

        private void init_fields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox1.UseSystemPasswordChar = maskedTextBox2.UseSystemPasswordChar = true;
            radioButton2.Checked = true;
            textBox1.BackColor = textBox3.BackColor = textBox4.BackColor = SystemColors.Window;
            //-------------------
            foreach (Control ctr in this.Controls)
            {
                    ctr.Visible = true;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            init_fields();
            //---------------------------
            if(dataGridView1.SelectedRows.Count > 0)
            {
                if (Properties.Settings.Default.Last_login_is_admin)
                {
                    textBox1.Text = dataGridView1.SelectedRows[0].Cells["USER"].Value.ToString();
                    maskedTextBox1.Text = maskedTextBox2.Text = (dataGridView1.SelectedRows[0].Cells["PASS"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["PASS"].Value.ToString() : "");
                    textBox2.Text = dataGridView1.SelectedRows[0].Cells["QUEST"].Value.ToString();
                    textBox3.Text = dataGridView1.SelectedRows[0].Cells["ANSWER"].Value.ToString();
                    textBox4.Text = dataGridView1.SelectedRows[0].Cells["EMAIL"].Value.ToString();
                    radioButton1.Checked = dataGridView1.SelectedRows[0].Cells["TYPE"].Value.ToString() == "ADMIN";
                    button4.Enabled = radioButton1.Enabled = radioButton2.Enabled = dataGridView1.Rows.Count > 1;
                    //--------------
                    In_Modif_Mod = true;
                }
                else if (Properties.Settings.Default.Last_login_user_idx == int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString()))
                {
                    textBox1.Text = dataGridView1.SelectedRows[0].Cells["USER"].Value.ToString();
                    maskedTextBox1.Text = maskedTextBox2.Text = (dataGridView1.SelectedRows[0].Cells["PASS"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["PASS"].Value.ToString() : "");
                    textBox2.Text = dataGridView1.SelectedRows[0].Cells["QUEST"].Value.ToString();
                    textBox3.Text = dataGridView1.SelectedRows[0].Cells["ANSWER"].Value.ToString();
                    textBox4.Text = dataGridView1.SelectedRows[0].Cells["EMAIL"].Value.ToString();
                    radioButton1.Checked = dataGridView1.SelectedRows[0].Cells["TYPE"].Value.ToString() == "ADMIN";
                    button4.Enabled = radioButton1.Enabled = radioButton2.Enabled = false;
                    //--------------
                    In_Modif_Mod = true;
                }
                else
                {
                    foreach (Control ctr in this.Controls)
                    {
                        if(ctr != dataGridView1)
                        {
                            ctr.Visible = false;
                        }
                    }
                }
            }

            //---------------------

        }

        private void Login_Modif_Load(object sender, EventArgs e)
        {
            login_data = PreConnection.Load_data("SELECT * FROM LOGIN;");
            dataGridView1.DataSource = login_data;
            //--------------------------
            if (User_Idd > 0)
            {
                DataGridViewRow row = dataGridView1.Rows
            .Cast<DataGridViewRow>()
             .Where(r => r.Cells["ID"].Value.ToString().Equals(User_Idd.ToString()))
             .First();
                //----------------
                dataGridView1.Rows[row.Index].Selected = true;

            }
            //------------------------------
            dataGridView1_SelectionChanged(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            In_Modif_Mod = false;
            if(dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.ClearSelection();
            }
            else
            {
                dataGridView1_SelectionChanged(null, null);
            }
            
            //-------------------------------
            //button4.Enabled = dataGridView1.Rows.Count > 1;
            radioButton1.Enabled = radioButton2.Enabled = (dataGridView1.Rows.Count >= 1 && Properties.Settings.Default.Last_login_is_admin);
            //-------------------------------
            textBox1.Select();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.BackColor = textBox2.TextLength > 0 && textBox3.TextLength == 0 ? Color.LightCoral : SystemColors.Window;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox3.BackColor = textBox2.TextLength > 0 && textBox3.TextLength == 0 ? Color.LightCoral : SystemColors.Window;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Etes-vous sures de faire la suppression ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int eee = int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());
                PreConnection.Excut_Cmd("DELETE FROM LOGIN WHERE ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + ";");

                if (eee == Properties.Settings.Default.Last_login_user_idx)
                {
                    Application.Restart();
                }
                else
                {
                    int previ_idx = -1;
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        previ_idx = dataGridView1.SelectedRows[0].Index;
                    }
                    login_data = PreConnection.Load_data("SELECT * FROM LOGIN;");
                    dataGridView1.DataSource = login_data;
                    if (previ_idx > -1)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[0].Selected = true;
                    }
                }
            }

        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (!IsValidEmail(textBox4.Text))
            {

                e.Cancel = true;
                textBox4.BackColor = Color.LightCoral;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.BackColor = SystemColors.Window;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.BackColor = SystemColors.Window;
        }
    }
}
