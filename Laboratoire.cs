using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Labo;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Fpe;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xamarin.Forms;
using Excc = Microsoft.Office.Interop.Excel;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Laboratoire : Form
    {
        DataTable animaux;
        DataGridViewRow selected_anim = null;
        public Laboratoire()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(selected_anim != null)
            {
                Hemogramme hemogramme = new Hemogramme(selected_anim);
                hemogramme.Dock = DockStyle.Fill;
                this.Controls.Add(hemogramme);
                hemogramme.BringToFront();
            }
            

        }

        private void Laboratoire_Load(object sender, EventArgs e)
        {
            animaux = PreConnection.Load_data("SELECT tb1.`ID`,"
                                                + "tb1.`NME`,"
                                                + "tb1.`CLIENT_ID`,"
                                                + "tb2.`CLIENT_FULL_NME`,"
                                                + "tb1.`ESPECE`,"
                                                + "tb1.`RACE`,"
                                                + "tb1.`SEXE`,"
                                                + "tb1.`NISS_DATE`,"
                                                + "tb1.`OBSERVATIONS`,"
                                                + "tb1.`IS_RADIATED`"
                                                + "FROM `tb_animaux` tb1 LEFT JOIN(SELECT ID, CONCAT(`SEX`, ' ',`FAMNME`, ' ',`NME`) AS CLIENT_FULL_NME FROM tb_clients) tb2 ON tb2.ID = tb1.CLIENT_ID; ");
            dataGridView1.DataSource = animaux;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            PreConnection.search_filter_datagridview(dataGridView1, textBox1.Text);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                selected_anim = dataGridView1.SelectedRows[0];
                label3.Text = (string)dataGridView1.SelectedRows[0].Cells["NME"].Value;
                label4.Text = (string)dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value;
                label6.Text = (string)dataGridView1.SelectedRows[0].Cells["ESPECE"].Value;
                label8.Text = (string)dataGridView1.SelectedRows[0].Cells["RACE"].Value;
                label13.Text = (string)dataGridView1.SelectedRows[0].Cells["SEXE"].Value;
                label14.Text = dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value != DBNull.Value ? ((DateTime)dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value).ToString("d") : "--";
                textBox2.Text = (string)dataGridView1.SelectedRows[0].Cells["OBSERVATIONS"].Value;
            }
            else
            {
                initial_infos_fields();
            }
        }

        private void initial_infos_fields()
        {
            selected_anim = null;
            label3.Text = label4.Text = label6.Text = label8.Text = label13.Text = label14.Text = textBox2.Text = "--";
        }
    }
}

