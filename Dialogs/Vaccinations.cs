using System;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Vaccinations : Form
    {
        public Vaccinations()
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
        private void Vaccinations_Load(object sender, EventArgs e)
        {
            Load_Data();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                new Add_Modif_Vaccin((int)dataGridView1.SelectedRows[0].Cells["ID"].Value).ShowDialog();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            button1.Visible = button3.Visible = dataGridView1.SelectedRows.Count > 0;
        }
    }
}
