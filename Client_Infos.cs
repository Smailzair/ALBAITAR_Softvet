using ALBAITAR_Softvet.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALBAITAR_Softvet
{
    public partial class Client_Infos : UserControl
    {
        public static bool make_refresh = false;
        public static int selected_client_id;
        public Client_Infos(int client_id)
        {
            InitializeComponent();
            //---------------
            selected_client_id = client_id;
            Load_Data();
        }

        private void Load_Data()
        {
            Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Where(ZZ => (int)ZZ["ID"] == selected_client_id).ForEach(rww => {
                label14.Text = rww["SEX"].ToString();
                label12.Text = rww["FAMNME"].ToString();
                label13.Text = rww["NUM_CNI"].ToString();
                label15.Text = rww["ADRESS"].ToString();
                label20.Text = rww["WILAYA"].ToString();
                label19.Text = rww["CITY"].ToString();
                label16.Text = rww["POSTAL_CODE"].ToString();
                label17.Text = rww["NUM_PHONE"].ToString();
                label18.Text = rww["EMAIL"].ToString();
                textBox8.Text = rww["OBSERVATIONS"].ToString();
            });            
            //----------------------------------------------
        }

        private void Client_Infos_Enter(object sender, EventArgs e)
        {
            if (make_refresh)
            {
                make_refresh = false;
                Load_Data();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Clients"] == null)
            {
                new Clients(selected_client_id, 1).Show();
            }
            else
            {
                Clients.ID_to_selectt = -2;
                Application.OpenForms["Clients"].WindowState = Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Clients"].WindowState;
                Application.OpenForms["Clients"].BringToFront();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Clients"] == null)
            {
                new Clients(-2 ,1).Show();
            }
            else
            {
                Clients.ID_to_selectt = -2;
                Application.OpenForms["Clients"].WindowState = Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Clients"].WindowState;
                Application.OpenForms["Clients"].BringToFront();
            }
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            button16.Focus();
        }
    }
}
