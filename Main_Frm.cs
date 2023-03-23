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
            if (Application.OpenForms["Clients"] == null)
            {
                new Clients().Show();
            }
            else
            {
                Application.OpenForms["Clients"].WindowState = Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Clients"].WindowState;
                Application.OpenForms["Clients"].BringToFront();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new Settings()).ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Animaux"] == null)
            {
                new Animaux().Show();
            }
            else
            {
                Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                Application.OpenForms["Animaux"].BringToFront();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Produits"] == null)
            {
                new Produits().Show();
            }
            else
            {
                Application.OpenForms["Produits"].WindowState = Application.OpenForms["Produits"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Produits"].WindowState;
                Application.OpenForms["Produits"].BringToFront();
            }
        }
    }
}

