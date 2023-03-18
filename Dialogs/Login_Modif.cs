using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
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
            button1.Enabled = radioButton1.Enabled = radioButton2.Enabled = comboBox1.Enabled = button4.Enabled = Properties.Settings.Default.Last_login_is_admin;
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
            ready_to_save = textBox1.TextLength > 0; //NOM
            ready_to_save &= textBox5.TextLength > 0; //PRENOM
            textBox1.BackColor = textBox1.TextLength == 0 ? Color.LightCoral : SystemColors.Window;
            textBox5.BackColor = textBox5.TextLength == 0 ? Color.LightCoral : SystemColors.Window;
            ready_to_save &= maskedTextBox1.Text.TrimEnd().TrimStart() == maskedTextBox2.Text.TrimEnd().TrimStart(); //PASSWORD            
            if (maskedTextBox1.Text.TrimEnd().TrimStart().Length > 0)
            {
                ready_to_save &= (textBox2.TextLength > 0 && textBox3.TextLength > 0) || ((textBox2.TextLength + textBox3.TextLength) == 0); //QUESTION & ANSWER
                ready_to_save &= IsValidEmail(textBox4.Text); //EMAIL
                textBox4.BackColor = !IsValidEmail(textBox4.Text) ? Color.LightCoral : SystemColors.Window;
            }
            else
            {
                textBox4.BackColor = SystemColors.Window;
            }



            if (In_Modif_Mod) //UPDATE
            {
                ready_to_save &= (dataGridView1.Rows
                 .Cast<DataGridViewRow>()
                 .Where(r => string.Concat(r.Cells["USER_NME"].Value.ToString(), r.Cells["USER_FAMNME"].Value.ToString()) == string.Concat(textBox1.Text, textBox5.Text))
                 .Count() <= 1);
                //----------------
                if (ready_to_save)
                {
                    PreConnection.Excut_Cmd("UPDATE `albaitar_db`.`tb_login_and_users` SET "
                        + "`USER_NME` = '" + textBox1.Text + "',"
                        + "`USER_FAMNME` = '" + textBox5.Text + "',"
                        + "`SEX` = '" + (radioButton4.Checked ? "M" : "F") + "',"
                        + "`PASSWORD` = '" + maskedTextBox1.Text + "',"
                        + "`FUNCTION` = '" + comboBox1.SelectedItem + "',"
                        + "`IS_ADMIN` = " + (radioButton1.Checked ? "True" : "False") + ","
                        + "`QUESTION` = '" + textBox2.Text + "',"
                        + "`ANSWER` = '" + textBox3.Text + "',"
                        + "`EMAIL` = '" + textBox4.Text + "',"
                        + "`CNI_NUM` = '" + textBox6.Text + "',"
                        + "`ANV_NUM` = '" + textBox7.Text + "'"
                        + "WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString() + ";");
                }
            }
            else //INSERT
            {
                ready_to_save &= (dataGridView1.Rows
                  .Cast<DataGridViewRow>()
                  .Where(r => string.Concat(r.Cells["USER_NME"].Value.ToString(), r.Cells["USER_FAMNME"].Value.ToString()) == string.Concat(textBox1.Text, textBox5.Text))
                  .Count() == 0);
                //----------------
                if (ready_to_save)
                {
                    PreConnection.Excut_Cmd("INSERT INTO `tb_login_and_users` ("
                        + "`USER_NME`,"
                        + "`USER_FAMNME`,"
                        + "`SEX`,"
                        + "`PASSWORD`,"
                        + "`FUNCTION`,"
                        + "`IS_ADMIN`,"
                        + "`QUESTION`,"
                        + "`ANSWER`,"
                        + "`EMAIL`,"
                        + "`CNI_NUM`,"
                        + "`ANV_NUM`)"
                        + "VALUES"
                        + "('" + textBox1.Text + "',"
                        + "'" + textBox5.Text + "',"
                        + "'" + (radioButton4.Checked ? "M" : "F") + "',"
                        + "'" + maskedTextBox1.Text + "',"
                        + "'" + comboBox1.SelectedItem + "',"
                        + (radioButton1.Checked ? "True" : "False") + ","
                        + "'" + textBox2.Text + "',"
                        + "'" + textBox3.Text + "',"
                        + "'" + textBox4.Text + "',"
                        + "'" + textBox6.Text + "',"
                        + "'" + textBox7.Text + "');");

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
                login_data = PreConnection.Load_data("SELECT *, concat(`USER_NME`,' ',`USER_FAMNME`) AS FULL_NME FROM tb_login_and_users;");
                dataGridView1.DataSource = login_data;
                if (previ_idx > -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[previ_idx].Selected = true;
                }
            }




        }

        private void init_fields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox1.UseSystemPasswordChar = maskedTextBox2.UseSystemPasswordChar = true;
            radioButton2.Checked = radioButton4.Checked = true;
            radioButton4_CheckedChanged(null, null);
            textBox1.BackColor = textBox3.BackColor = textBox4.BackColor = SystemColors.Window;
            //-------------------
            foreach (Control ctr in this.Controls)
            {
                if (ctr != label14)
                {
                    ctr.Visible = true;
                }
                else
                {
                    ctr.Visible = false;
                }

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            init_fields();
            //---------------------------
            if (dataGridView1.SelectedRows.Count > 0)
            {
                button4.Enabled = true;
                //--------------
                if (Properties.Settings.Default.Last_login_is_admin)
                {
                    textBox1.Text = dataGridView1.SelectedRows[0].Cells["USER_NME"].Value.ToString();
                    textBox5.Text = dataGridView1.SelectedRows[0].Cells["USER_FAMNME"].Value.ToString();
                    maskedTextBox1.Text = maskedTextBox2.Text = (dataGridView1.SelectedRows[0].Cells["PASSWORD"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["PASSWORD"].Value.ToString() : "");
                    textBox2.Text = dataGridView1.SelectedRows[0].Cells["QUESTION"].Value.ToString();
                    textBox3.Text = dataGridView1.SelectedRows[0].Cells["ANSWER"].Value.ToString();
                    textBox4.Text = dataGridView1.SelectedRows[0].Cells["EMAIL"].Value.ToString();
                    textBox6.Text = dataGridView1.SelectedRows[0].Cells["CNI_NUM"].Value.ToString();
                    textBox7.Text = dataGridView1.SelectedRows[0].Cells["ANV_NUM"].Value.ToString();
                    radioButton1.Checked = (SByte)dataGridView1.SelectedRows[0].Cells["IS_ADMIN"].Value == 1;
                    radioButton3.Checked = dataGridView1.SelectedRows[0].Cells["SEX"].Value.ToString() == "F";
                    comboBox1.SelectedItem = dataGridView1.SelectedRows[0].Cells["FUNCTION"].Value.ToString();
                    button4.Enabled = radioButton1.Enabled = radioButton2.Enabled = dataGridView1.Rows.Count > 1;
                    //--------------
                    In_Modif_Mod = true;
                }
                else if (Properties.Settings.Default.Last_login_user_idx == int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString()))
                {
                    textBox1.Text = dataGridView1.SelectedRows[0].Cells["USER_NME"].Value.ToString();
                    textBox5.Text = dataGridView1.SelectedRows[0].Cells["USER_FAMNME"].Value.ToString();
                    maskedTextBox1.Text = maskedTextBox2.Text = (dataGridView1.SelectedRows[0].Cells["PASSWORD"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["PASSWORD"].Value.ToString() : "");
                    textBox2.Text = dataGridView1.SelectedRows[0].Cells["QUESTION"].Value.ToString();
                    textBox3.Text = dataGridView1.SelectedRows[0].Cells["ANSWER"].Value.ToString();
                    textBox4.Text = dataGridView1.SelectedRows[0].Cells["EMAIL"].Value.ToString();
                    textBox6.Text = dataGridView1.SelectedRows[0].Cells["CNI_NUM"].Value.ToString();
                    textBox7.Text = dataGridView1.SelectedRows[0].Cells["ANV_NUM"].Value.ToString();
                    radioButton1.Checked = (SByte)dataGridView1.SelectedRows[0].Cells["IS_ADMIN"].Value == 1;
                    radioButton3.Checked = dataGridView1.SelectedRows[0].Cells["SEX"].Value.ToString() == "F";
                    comboBox1.SelectedItem = dataGridView1.SelectedRows[0].Cells["FUNCTION"].Value.ToString();
                    button4.Enabled = radioButton1.Enabled = radioButton2.Enabled = false;
                    //--------------
                    In_Modif_Mod = true;
                }
                else
                {
                    foreach (Control ctr in this.Controls)
                    {
                        if (ctr != dataGridView1 && ctr != label13 && ctr != label14)
                        {
                            ctr.Visible = false;
                        }
                        else if (ctr == label14)
                        {
                            ctr.Visible = true;
                        }

                    }
                }
            }
            //---------------------

        }

        private void Login_Modif_Load(object sender, EventArgs e)
        {
            login_data = PreConnection.Load_data("SELECT *, concat(`USER_NME`,' ',`USER_FAMNME`) AS FULL_NME FROM tb_login_and_users;");
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
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.ClearSelection();
            }
            else
            {
                dataGridView1_SelectionChanged(null, null);
            }

            //-------------------------------
            radioButton1.Enabled = radioButton2.Enabled = (dataGridView1.Rows.Count >= 1 && Properties.Settings.Default.Last_login_is_admin);
            button4.Enabled = false;
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
            if (dataGridView1.Rows.Count >= 2)
            {
                string command = "";
                int eee = int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());
                //----------------------------------
                int Admin_Rest = dataGridView1.Rows
                     .Cast<DataGridViewRow>()
                     .Count(row => int.Parse(row.Cells["ID"].Value.ToString()) != eee && (SByte)row.Cells["IS_ADMIN"].Value == 1);
                bool warning = dataGridView1.Rows.Count == 2 && Admin_Rest == 0;
                //-------------------------------------
                if(dataGridView1.Rows.Count > 2 && Admin_Rest == 0)
                {
                    MessageBox.Show("A cause que ce compte est le seul qui a les droits 'Admin',\nVous devez -d'abord- changer le type d'un autre compte 'Standard' à 'Admin'.","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Etes-vous sures de faire la suppression de (" + dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value.ToString() + ") ?\n\n" + (warning ? ("(L'utilisateur '" + dataGridView1.Rows
                                .Cast<DataGridViewRow>()
                                .FirstOrDefault(row => int.Parse(row.Cells["ID"].Value.ToString()) != eee).Cells["FULL_NME"].Value + "' acquerra les droits 'Admin')") : ""), "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command += warning ? "UPDATE tb_login_and_users SET IS_ADMIN = True; " : "";
                        command += "DELETE FROM tb_login_and_users WHERE ID = " + eee + ";";
                        PreConnection.Excut_Cmd(command);
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
                            login_data = PreConnection.Load_data("SELECT *, concat(`USER_NME`,' ',`USER_FAMNME`) AS FULL_NME FROM tb_login_and_users;");
                            dataGridView1.DataSource = login_data;                            
                            if (previ_idx > -1)
                            {
                                dataGridView1.ClearSelection();
                                dataGridView1.Rows[0].Selected = true;
                            }
                        }
                    }
                }                
                
                
            }
            else
            {
                MessageBox.Show("Vous devez d'abord créer un autre compte.", "Impossible :", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
            ((TextBox)sender).BackColor = SystemColors.Window;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            int oos = comboBox1.SelectedIndex > -1 ? comboBox1.SelectedIndex : 0;
            comboBox1.Items.Clear();
            if (radioButton4.Checked)
            {
                comboBox1.Items.AddRange(new object[] { "Vétérinaire", "Téchnicien Vétérinaire", "Assistant Vétérinaire", "Administratif" });
            }
            else
            {
                comboBox1.Items.AddRange(new object[] { "Vétérinaire", "Téchnicienne Vétérinaire", "Assistante Vétérinaire", "Administrative" });
            }
            comboBox1.SelectedIndex = oos;
        }
    }
}
