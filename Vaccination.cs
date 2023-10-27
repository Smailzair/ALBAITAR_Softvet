using ALBAITAR_Softvet.Dialogs;
using System;
using System.Data;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Vaccination : UserControl
    {
        public static bool make_refresh = false;
        public Vaccination()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            new Add_Modif_Vaccin(-1).ShowDialog();
        }

        private void Load_Data()
        {
            Main_Frm.Main_Frm_vaccination = PreConnection.Load_data("SELECT * FROM tb_vaccin;");
            dataGridView1.DataSource = Main_Frm.Main_Frm_vaccination;
        }
        private void Vaccination_Load(object sender, EventArgs e)
        {
            Load_Data();
        }

        private void Vaccination_Enter(object sender, EventArgs e)
        {
            if (make_refresh)
            {
                make_refresh = false;
                Load_Data();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
