using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            checkBox1.Checked = Properties.Settings.Default.Show_Login;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Show_Login = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new Login_Modif(Properties.Settings.Default.Last_login_user_idx)).ShowDialog();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.Open_Last_Fold_Automatic = checkBox2.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
