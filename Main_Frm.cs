using ALBAITAR_Softvet.Resources;
using System;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Main_Frm : Form
    {
        Thread th;
        public static DataTable ADRESSES_SITES;
        bool sites_table_ready = false;
        public Main_Frm()
        {
            InitializeComponent();
            //----------------------------
            th = new Thread(new ThreadStart(Load_sites_table)); //I use it because of starting perfermance of "Clients" from
            th.Start();
            //--------------
        }
         

        public void Load_sites_table()
        {
            ADRESSES_SITES = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_adresses;");
            sites_table_ready = true;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Cursor= Cursors.WaitCursor;
            if (!sites_table_ready)
            {
                th.Join();
            }
            if (Application.OpenForms["Clients"] == null)
            {
                new Clients().Show();
            }
            else
            {
                Application.OpenForms["Clients"].WindowState = Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Clients"].WindowState;
                Application.OpenForms["Clients"].BringToFront();
            }
            Cursor = Cursors.Default;
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Agenda"] == null)
            {
                new Agenda().Show();
            }
            else
            {
                Application.OpenForms["Agenda"].WindowState = Application.OpenForms["Agenda"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Agenda"].WindowState;
                Application.OpenForms["Agenda"].BringToFront();
            }
        }

    }
}

