using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class SettingGlobal : UserControl
    {
        public SettingGlobal()
        {
            InitializeComponent();
            //-------------------------------
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new Login_Modif(Properties.Settings.Default.Last_login_user_idx)).ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Use_animals_logo = checkBox1.Checked;
            Properties.Settings.Default.Save(); 
            Properties.Settings.Default.Reload();
        }

        private void SettingGlobal_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Properties.Settings.Default.Use_animals_logo;
        }
    }
}
