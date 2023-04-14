//using Microsoft.Office.Interop.Excel;
using ALBAITAR_Softvet.Resources;
using System;
using System.Drawing;
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
            (new Login_Modif(Properties.Settings.Default.Last_login_user_idx)).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Autorizations().ShowDialog();
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
        }
    }
}
