using ALBAITAR_Softvet.Resources;
using System;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Main_Frm : Form
    {
        
        public Main_Frm()
        {
            InitializeComponent();            
            //----------------------------
            

        }

        private void button9_Click(object sender, EventArgs e)
        {
            new Clients().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new Settings()).ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}

