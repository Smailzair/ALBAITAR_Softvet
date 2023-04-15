using System;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Non_Autorized_Msg : Form
    {
        public Non_Autorized_Msg(string msg_txt)
        {
            InitializeComponent();
            //--------------
            if(msg_txt == null || msg_txt.Trim().Length == 0) {
                label1.Text = "Vous n'avez pas l'autorisations";
            }
            else
            {
                label1.Text = msg_txt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Non_Autorized_Msg_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Non_Autorized_Msg_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

    }
}
