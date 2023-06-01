using ALBAITAR_Softvet.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Anim_Infos : UserControl
    {
        public static bool make_refresh = false;
        public static int selected_anim_id;
        public Anim_Infos(int anim_id)
        {
            InitializeComponent();
            //--------------------
            selected_anim_id = anim_id;
            Load_Data();
        }

        private void Load_Data()
        {
            DataRow inf = Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(zz => (int)zz["ID"] == selected_anim_id).FirstOrDefault();
            if (inf != null)
            {
                label16.Text = inf["DATE_ADDED"] != DBNull.Value ? ((DateTime)inf["DATE_ADDED"]).ToString("dddd dd/MM/yyyy", new CultureInfo("fr-FR")) : "--";
                label25.Text = inf["NME"] != DBNull.Value ? (string)inf["NME"] : "--";
                label17.Text = inf["NUM_IDENTIF"] != DBNull.Value ? (string)inf["NUM_IDENTIF"] : "--";
                label18.Text = inf["NUM_PASSPORT"] != DBNull.Value ? (string)inf["NUM_PASSPORT"] : "--";
                DataRow clnt_nme = Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Where(zz => (int)zz["ID"] == (inf["CLIENT_ID"] != DBNull.Value ? (int)inf["CLIENT_ID"] : -1)).FirstOrDefault();
                label21.Text = clnt_nme != null ? (string)clnt_nme["FULL_NME"] : "--";
                label23.Text = inf["ESPECE"] != DBNull.Value ? (string)inf["ESPECE"] : "--";
                label22.Text = inf["RACE"] != DBNull.Value ? (string)inf["RACE"] : "--";
                label20.Text = inf["SEXE"] != DBNull.Value ? (string)inf["SEXE"] : "--";
                label26.Text = inf["NISS_DATE"] != DBNull.Value ? ((DateTime)inf["NISS_DATE"]).ToString("dd/MM/yyyy") : "--";
                label19.Text = inf["ROBE"] != DBNull.Value ? (string)inf["ROBE"] : "--";
                textBox8.Text = inf["OBSERVATIONS"] != DBNull.Value ? (string)inf["OBSERVATIONS"] : "";
                bool ttt = inf["IS_RADIATED"] != DBNull.Value;
                ttt = ttt ? (sbyte)inf["IS_RADIATED"] == 1 : false;
                if (ttt)
                {
                    label24.Text = "Oui";
                    groupBox1.Visible = true;
                    label27.Text = inf["RADIATION_DATE"] != DBNull.Value ? ((DateTime)inf["RADIATION_DATE"]).ToString("dd/MM/yyyy") : "--";
                    textBox5.Text = inf["RADIATION_CAUSES"] != DBNull.Value ? (string)inf["RADIATION_CAUSES"] : "";
                }
                else
                {
                    label24.Text = "Non";
                    groupBox1.Visible = false;
                    label27.Text = "--";
                    textBox5.Text = "";
                }
                pictureBox2.Image = inf["PICTURE"] != DBNull.Value ? PreConnection.ByteArrayToImage((byte[])inf["PICTURE"]) : (Properties.Settings.Default.Use_animals_logo ? (Image)Properties.Resources.ResourceManager.GetObject(label23.Text) : null);
            }
        }

        private void Anim_Infos_Enter(object sender, EventArgs e)
        {
            if (make_refresh)
            {
                make_refresh = false;
                Load_Data();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Animaux"] == null)
            {
                new Animaux(-2, -1).Show();
            }
            else
            {
                Animaux.ID_to_selectt = -2;
                Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                Application.OpenForms["Animaux"].BringToFront();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Animaux"] == null)
            {
                new Animaux(selected_anim_id, -1).Show();
            }
            else
            {
                Animaux.ID_to_selectt = selected_anim_id;
                Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                Application.OpenForms["Animaux"].BringToFront();
            }            
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            button16.Focus();
        }
    }
}
