using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Login_Modif : Form
    {
        DataTable login_data;
        int User_Idd;
        bool In_Modif_Mod = true;
        bool was_admin = false;
        bool Mofifier_son_cpt_91001,
            Supprimer_son_cpt_91002,
            Ajouter_user_standard_91003,
            Modifier_user_standard_91004,
            Supprimer_user_standard_91005 = false;
        public Login_Modif(int? User_ID)
        {
            InitializeComponent();
            //---------------------------
            if (Properties.Settings.Default.Last_login_is_admin)
            {
                Mofifier_son_cpt_91001 =
                Supprimer_son_cpt_91002 =
                Ajouter_user_standard_91003 =
                Modifier_user_standard_91004 =
                Supprimer_user_standard_91005 = true;
            }
            else
            {
                Mofifier_son_cpt_91001 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "91001" && (Int32)QQ[3] == 1).Count() > 0;
                Supprimer_son_cpt_91002 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "91002" && (Int32)QQ[3] == 1).Count() > 0;
                Ajouter_user_standard_91003 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "91003" && (Int32)QQ[3] == 1).Count() > 0;
                Modifier_user_standard_91004 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "91004" && (Int32)QQ[3] == 1).Count() > 0;
                Supprimer_user_standard_91005 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "91005" && (Int32)QQ[3] == 1).Count() > 0;
            }
            button1.Enabled = Ajouter_user_standard_91003;
            //-------------------------
            User_Idd = (User_ID ?? 0);
            //button1.Enabled = radioButton1.Enabled = radioButton2.Enabled = comboBox1.Enabled = button4.Enabled = Properties.Settings.Default.Last_login_is_admin;
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
            bool restart_requir = false;
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

            if (In_Modif_Mod)
            {
                int admin_count = login_data.Rows.Cast<DataRow>().Where(SS => (SByte)SS["IS_ADMIN"] == 1 && (int)SS["ID"] != (int)dataGridView1.SelectedRows[0].Cells["ID"].Value).ToList().Count();
                if (admin_count == 0 && !radioButton1.Checked)
                {
                    ready_to_save = false;
                    MessageBox.Show("A cause que ce compte est le seul qui a les droits 'Admin',\nVous devez -d'abord- changer le type d'un autre compte 'Standard' à 'Admin'.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                    restart_requir = Properties.Settings.Default.Last_login_user_idx == (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
                    //---------
                    PreConnection.Excut_Cmd(2, "tb_login_and_users", new List<string> { "USER_NME", "USER_FAMNME", "SEX", "PASSWORD", "FUNCT", "IS_ADMIN", "QUESTION", "ANSWER", "EMAIL", "CNI_NUM", "ANV_NUM" }, new List<object> {
                    textBox1.Text,
                    textBox5.Text,
                    radioButton4.Checked ? "M" : "F",
                    maskedTextBox1.Text,
                    comboBox1.SelectedItem,
                    radioButton1.Checked,
                    textBox2.Text,
                    textBox3.Text,
                    textBox4.Text,
                    textBox6.Text,
                    textBox7.Text}, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView1.SelectedRows[0].Cells["ID"].Value });

                    if (was_admin && !radioButton1.Checked && MessageBox.Show("Voulez-vous mettre les autorisations 'par defaut' [Oui] \n ou de conserver les autorisations précédentes (avant d'étre 'Admin') ? [Non]", "Autorisations :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        PreConnection.Excut_Cmd_personnel("UPDATE tb_autoriz t1 JOIN tb_autoriz t2 ON t1.id = t2.id SET t2.Usr_" + dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString().Replace("'", "''") + " = t1.`DEFAULT_VALUES`;", null, null);
                    }


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
                    PreConnection.Excut_Cmd(1, "tb_login_and_users", new List<string> {
                        "USER_NME",
                        "USER_FAMNME",
                        "SEX",
                        "PASSWORD",
                        "FUNCT",
                        "IS_ADMIN",
                        "QUESTION",
                        "ANSWER",
                        "EMAIL",
                        "CNI_NUM",
                        "ANV_NUM"
                    }, new List<object>
                    {
                        textBox1.Text,
                        textBox5.Text,
                        radioButton4.Checked ? "M" : "F",
                        maskedTextBox1.Text,
                        comboBox1.SelectedItem,
                        radioButton1.Checked,
                        textBox2.Text,
                        textBox3.Text,
                        textBox4.Text,
                        textBox6.Text,
                        textBox7.Text
                    }, null, null, null);

                    //------Autorisations -------------------
                    DataTable dt = PreConnection.Load_data("SELECT MAX(ID) FROM tb_login_and_users");
                    PreConnection.Excut_Cmd_personnel("ALTER TABLE tb_autoriz ADD COLUMN Usr_" + dt.Rows[0][0].ToString().Replace("'", "''") + " INT;", null, null);
                    PreConnection.Excut_Cmd_personnel("UPDATE tb_autoriz e1 JOIN tb_autoriz e2 ON e1.ID = e2.ID SET e1.Usr_" + dt.Rows[0][0].ToString().Replace("'", "''") + " = e2.DEFAULT_VALUES;", null, null);
                }
            }
            //--------------------------
            if (ready_to_save)
            {
                if (restart_requir)
                {
                    MessageBox.Show("Bien enregistré !\n\nL'application va redémarrer (requis).", "Success : ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //----------
                    Application.OpenForms["Main_Frm"].Close();
                    Application.Restart();
                }
                else
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
                    radioButton1.Checked = was_admin = (SByte)dataGridView1.SelectedRows[0].Cells["IS_ADMIN"].Value == 1;
                    radioButton3.Checked = dataGridView1.SelectedRows[0].Cells["SEX"].Value.ToString() == "F";
                    comboBox1.SelectedItem = dataGridView1.SelectedRows[0].Cells["FUNCT"].Value.ToString();
                    button4.Enabled = radioButton1.Enabled = radioButton2.Enabled = dataGridView1.Rows.Count > 1;
                    //--------------
                    In_Modif_Mod = true;
                }
                else if (Properties.Settings.Default.Last_login_user_idx == int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString()))
                {
                    button4.Enabled = Supprimer_son_cpt_91002;

                    if (Mofifier_son_cpt_91001)
                    {
                        textBox1.Text = dataGridView1.SelectedRows[0].Cells["USER_NME"].Value.ToString();
                        textBox5.Text = dataGridView1.SelectedRows[0].Cells["USER_FAMNME"].Value.ToString();
                        maskedTextBox1.Text = maskedTextBox2.Text = (dataGridView1.SelectedRows[0].Cells["PASSWORD"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["PASSWORD"].Value.ToString() : "");
                        textBox2.Text = dataGridView1.SelectedRows[0].Cells["QUESTION"].Value.ToString();
                        textBox3.Text = dataGridView1.SelectedRows[0].Cells["ANSWER"].Value.ToString();
                        textBox4.Text = dataGridView1.SelectedRows[0].Cells["EMAIL"].Value.ToString();
                        textBox6.Text = dataGridView1.SelectedRows[0].Cells["CNI_NUM"].Value.ToString();
                        textBox7.Text = dataGridView1.SelectedRows[0].Cells["ANV_NUM"].Value.ToString();
                        radioButton1.Checked = was_admin = (SByte)dataGridView1.SelectedRows[0].Cells["IS_ADMIN"].Value == 1;
                        radioButton3.Checked = dataGridView1.SelectedRows[0].Cells["SEX"].Value.ToString() == "F";
                        comboBox1.SelectedItem = dataGridView1.SelectedRows[0].Cells["FUNCT"].Value.ToString();
                        radioButton1.Enabled = radioButton2.Enabled = false;
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
                else
                {
                    button4.Enabled = Supprimer_user_standard_91005 && (SByte)dataGridView1.SelectedRows[0].Cells["IS_ADMIN"].Value == 0;

                    if (Modifier_user_standard_91004)
                    {
                        textBox1.Text = dataGridView1.SelectedRows[0].Cells["USER_NME"].Value.ToString();
                        textBox5.Text = dataGridView1.SelectedRows[0].Cells["USER_FAMNME"].Value.ToString();
                        maskedTextBox1.Text = maskedTextBox2.Text = (dataGridView1.SelectedRows[0].Cells["PASSWORD"].Value != DBNull.Value ? dataGridView1.SelectedRows[0].Cells["PASSWORD"].Value.ToString() : "");
                        textBox2.Text = dataGridView1.SelectedRows[0].Cells["QUESTION"].Value.ToString();
                        textBox3.Text = dataGridView1.SelectedRows[0].Cells["ANSWER"].Value.ToString();
                        textBox4.Text = dataGridView1.SelectedRows[0].Cells["EMAIL"].Value.ToString();
                        textBox6.Text = dataGridView1.SelectedRows[0].Cells["CNI_NUM"].Value.ToString();
                        textBox7.Text = dataGridView1.SelectedRows[0].Cells["ANV_NUM"].Value.ToString();
                        radioButton1.Checked = was_admin = (SByte)dataGridView1.SelectedRows[0].Cells["IS_ADMIN"].Value == 1;
                        radioButton3.Checked = dataGridView1.SelectedRows[0].Cells["SEX"].Value.ToString() == "F";
                        comboBox1.SelectedItem = dataGridView1.SelectedRows[0].Cells["FUNCT"].Value.ToString();
                        radioButton1.Enabled = radioButton2.Enabled = false;
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
            //-------------------------------
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
                int eee = int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());
                //----------------------------------
                int Admin_Rest = dataGridView1.Rows
                     .Cast<DataGridViewRow>()
                     .Count(row => int.Parse(row.Cells["ID"].Value.ToString()) != eee && (SByte)row.Cells["IS_ADMIN"].Value == 1);
                bool warning = dataGridView1.Rows.Count == 2 && Admin_Rest == 0;
                //-------------------------------------
                if (dataGridView1.Rows.Count > 2 && Admin_Rest == 0)
                {
                    MessageBox.Show("A cause que ce compte est le seul qui a les droits 'Admin',\nVous devez -d'abord- changer le type d'un autre compte 'Standard' à 'Admin'.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Etes-vous sures de faire la suppression de (" + dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value.ToString() + ") ?\n\n" + (warning ? ("(L'utilisateur '" + dataGridView1.Rows
                                .Cast<DataGridViewRow>()
                                .FirstOrDefault(row => int.Parse(row.Cells["ID"].Value.ToString()) != eee).Cells["FULL_NME"].Value + "' acquerra les droits 'Admin')") : ""), "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (warning) { PreConnection.Excut_Cmd_personnel("UPDATE tb_login_and_users SET IS_ADMIN = True;", null, null); }
                        PreConnection.Excut_Cmd_personnel("DELETE FROM tb_login_and_users WHERE ID = " + eee + ";", null, null);
                        PreConnection.Excut_Cmd_personnel("ALTER TABLE tb_autoriz DROP COLUMN Usr_" + eee + ";", null, null);

                        if (eee == Properties.Settings.Default.Last_login_user_idx)
                        {
                            Application.OpenForms["Main_Frm"].Close();
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
