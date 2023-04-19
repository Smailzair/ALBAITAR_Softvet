using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Labo
{
    public partial class Hemogramme : UserControl
    {
        DataGridViewRow animm = null;
        
        public Hemogramme(DataGridViewRow selected_anim)
        {
            InitializeComponent();
            animm = selected_anim;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void Hemogramme_Load(object sender, EventArgs e)
        {
            //------------------------------
            label3.Text = (string)animm.Cells["NME"].Value;
            label4.Text = (string)animm.Cells["CLIENT_FULL_NME"].Value;
            label6.Text = (string)animm.Cells["ESPECE"].Value;
            label8.Text = (string)animm.Cells["RACE"].Value;
            label13.Text = (string)animm.Cells["SEXE"].Value;
            label14.Text = animm.Cells["NISS_DATE"].Value != DBNull.Value ? ((DateTime)animm.Cells["NISS_DATE"].Value).ToString("d") : "--";
            textBox2.Text = (string)animm.Cells["OBSERVATIONS"].Value;
            //------------------------
        }
    }
}
