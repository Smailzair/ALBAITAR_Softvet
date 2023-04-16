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
        }
    }
}
