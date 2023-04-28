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
            textBox1.Text = Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.BackColor = textBox1.Text.Trim().Length == 0 ? Color.LightCoral : SystemColors.Window;
            if(textBox1.BackColor == SystemColors.Window)
            {                
                PreConnection.Excut_Cmd("UPDATE tb_params SET `VAL` = '"+textBox1.Text+"' WHERE `ID` = 1;");
                Main_Frm.Params = PreConnection.Load_data("SELECT * FROM tb_params;");
                Main_Frm.label_cab_nme.Text = textBox1.Text;
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
    }
}
