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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new Login_Modif(Properties.Settings.Default.Last_login_user_idx)).ShowDialog();
        }


    }
}
