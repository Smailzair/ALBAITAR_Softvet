//using Microsoft.Office.Interop.Excel;
using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Resources;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Settings : Form
    {
        bool save_tva_perc = false;
        public Settings()
        {
            InitializeComponent();
            //---------------------------
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Login_Auto_Enter)
            {
                new Login(false, Properties.Settings.Default.Last_login_user_idx).ShowDialog();
                if (Login.enter_allow)
                {
                    new Login_Modif(Properties.Settings.Default.Last_login_user_idx).ShowDialog();
                }
                else
                {
                    new Non_Autorized_Msg("Mot de passe fausse !").Show();
                }
            }
            else
            {
                new Login_Modif(Properties.Settings.Default.Last_login_user_idx).ShowDialog();
            }          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Login_Auto_Enter)
            {
                new Login(false, Properties.Settings.Default.Last_login_user_idx).ShowDialog();
                if (Login.enter_allow)
                {
                    new Autorizations().ShowDialog();
                }
                else
                {
                    new Non_Autorized_Msg("Mot de passe fausse !").Show();
                }
            }
            else
            {
                new Autorizations().ShowDialog();
            }
                        
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Use_animals_logo = checkBox1.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Properties.Settings.Default.Use_animals_logo;
            button1.Enabled = Properties.Settings.Default.Last_login_is_admin || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "91000" && (Int32)QQ[3] == 1).Count() > 0;
            button2.Enabled = Properties.Settings.Default.Last_login_is_admin || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92000" && (Int32)QQ[3] == 1).Count() > 0 || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92001" && (Int32)QQ[3] == 1).Count() > 0;
            groupBox1.Enabled = Properties.Settings.Default.Last_login_is_admin || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92004" && (Int32)QQ[3] == 1).Count() > 0;
            numericUpDown1.Enabled = Properties.Settings.Default.Last_login_is_admin || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92005" && (Int32)QQ[3] == 1).Count() > 0;
            button5.Enabled = Properties.Settings.Default.Connection_String_IP_Or_LocalHost == "localhost";
            //---------- Identif. -----------
            textBox1.Text = Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();
            textBox2.Text = Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();
            textBox3.Text = Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();
            textBox4.Text = Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();
            //-----------TVA de vente ---------
            numericUpDown1.Value = decimal.Parse(Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 5).Select(QQ => QQ["VAL"]).First().ToString());
            //-----------Tabs Orient ---------------
            radioButton2.CheckedChanged -= radioButton2_CheckedChanged;
            radioButton1.Checked = !Properties.Settings.Default.Main_Frm_Tabs_Horientation_Is_Verticatl;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.BackColor = textBox1.Text.Trim().Length == 0 ? Color.LightCoral : SystemColors.Window;
            if(textBox1.BackColor == SystemColors.Window)
            {
                PreConnection.Excut_Cmd_personnel("UPDATE tb_params SET `VAL` = @Param001 WHERE `ID` = 1;",new System.Collections.Generic.List<string> { "Param001" },new System.Collections.Generic.List<object> { textBox1.Text });
                PreConnection.Excut_Cmd_personnel("UPDATE tb_params SET `VAL` = @Param002 WHERE `ID` = 2;", new System.Collections.Generic.List<string> { "Param002" }, new System.Collections.Generic.List<object> { textBox2.Text });
                PreConnection.Excut_Cmd_personnel("UPDATE tb_params SET `VAL` = @Param003 WHERE `ID` = 3;", new System.Collections.Generic.List<string> { "Param003" }, new System.Collections.Generic.List<object> { textBox3.Text });
                PreConnection.Excut_Cmd_personnel("UPDATE tb_params SET `VAL` = @Param004 WHERE `ID` = 4;", new System.Collections.Generic.List<string> { "Param004" }, new System.Collections.Generic.List<object> { textBox4.Text });

                //string cmmd = "UPDATE tb_params SET `VAL` = '" + textBox1.Text.Replace("'", "''") + "' WHERE `ID` = 1;";
                //cmmd += " UPDATE tb_params SET `VAL` = '" + textBox2.Text.Replace("'", "''") + "' WHERE `ID` = 2;";
                //cmmd += " UPDATE tb_params SET `VAL` = '" + textBox3.Text.Replace("'", "''") + "' WHERE `ID` = 3;";
                //cmmd += " UPDATE tb_params SET `VAL` = '" + textBox4.Text.Replace("'", "''") + "' WHERE `ID` = 4;";                
                //PreConnection.Excut_Cmd(cmmd);
                
                

            }
            else
            {
                textBox1.Focus();           
            }
            
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
            }
        }
        
        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(save_tva_perc)
            {
                PreConnection.Excut_Cmd_personnel("UPDATE tb_params SET `VAL` = @Param005 WHERE `ID` = 5;", new System.Collections.Generic.List<string> { "Param005" }, new System.Collections.Generic.List<object> { numericUpDown1.Value });
                //PreConnection.Excut_Cmd("UPDATE tb_params SET `VAL` = '" + numericUpDown1.Value + "' WHERE `ID` = 5;");
                Main_Frm.Params = PreConnection.Load_data("SELECT * FROM tb_params;");
            }            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            save_tva_perc = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Main_Frm_Tabs_Horientation_Is_Verticatl = radioButton2.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            //---------------
            Application.Restart();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Connection_Str().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new App_Activation().ShowDialog();
        }
    }
}
