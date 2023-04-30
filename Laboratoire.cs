using ALBAITAR_Softvet.Labo;
using Microsoft.ReportingServices.Diagnostics.Internal;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Laboratoire : Form
    {
        public static bool make_historic_refesh = false;
        DataTable animaux;
        public static DataTable labo;
        public static string labo_load_cmd = "SELECT 'Hemogramme' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_hemogramme UNION ALL "
                                         + "SELECT 'Biochimie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_biochimie  UNION ALL "
                                         + "SELECT 'Immunologie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_immunologie;";
        static DataGridViewRow selected_anim = null;
        public Laboratoire()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {
                this.ControlBox = false;
                Hemogramme hemogramme = new Hemogramme(selected_anim);
                hemogramme.Dock = DockStyle.Fill;
                this.Controls.Add(hemogramme);
                hemogramme.BringToFront();
            }
        }

        private void Laboratoire_Load(object sender, EventArgs e)
        {
            
            //--------------------------
            animaux = PreConnection.Load_data("SELECT tb1.`ID`,"
                     + "tb1.`NME`,"
                     + "tb1.`ESPECE`,"
                     + "tb1.`RACE`,"
                     + "tb1.`SEXE`,"
                     + "tb1.`NISS_DATE`,"
                     + "tb1.`OBSERVATIONS`,"
                     + "tb1.`IS_RADIATED`,"
                     + "tb2.*"
                     + "FROM `tb_animaux` tb1 LEFT JOIN (SELECT "
                     + "ID AS CLIENT_ID,"
                     + "CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS CLIENT_FULL_NME,"
                     + "`NUM_CNI` AS CLIENT_NUM_CNI,"
                     + "`ADRESS` AS CLIENT_ADRESS,"
                     + "`CITY` AS CLIENT_CITY,"
                     + "`WILAYA` AS CLIENT_WILAYA,"
                     + "`NUM_PHONE` AS CLIENT_NUM_PHONE,"
                     + "`EMAIL` AS CLIENT_EMAIL "
                     + "FROM tb_clients) tb2 ON tb2.CLIENT_ID = tb1.CLIENT_ID;");

            load_labos_data();
            dataGridView1.DataSource = animaux;
            //---------
            comboBox1.SelectedIndex = 0;
        }

        private void load_labos_data()
        {
            labo = PreConnection.Load_data(labo_load_cmd);

            dataGridView2.DataSource = labo;
            textBox3.TextChanged -= textBox3_TextChanged;
            textBox3.Text = "";
            textBox3.TextChanged += textBox3_TextChanged;
            comboBox1_SelectedIndexChanged(null, null);

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            PreConnection.search_filter_datagridview(dataGridView1, textBox1.Text);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selected_anim = dataGridView1.SelectedRows[0];
                label3.Text = (string)dataGridView1.SelectedRows[0].Cells["NME"].Value;
                label4.Text = (string)dataGridView1.SelectedRows[0].Cells["CLIENT_FULL_NME"].Value;
                label6.Text = (string)dataGridView1.SelectedRows[0].Cells["ESPECE"].Value;
                label8.Text = (string)dataGridView1.SelectedRows[0].Cells["RACE"].Value;
                label13.Text = (string)dataGridView1.SelectedRows[0].Cells["SEXE"].Value;
                label14.Text = dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value != DBNull.Value ? ((DateTime)dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value).ToString("d") : "--";
                textBox2.Text = (string)dataGridView1.SelectedRows[0].Cells["OBSERVATIONS"].Value;
                //------------------------
                button3.Enabled = dataGridView1.SelectedRows[0].Cells["ESPECE"].Value.ToString() == "Canine" || dataGridView1.SelectedRows[0].Cells["ESPECE"].Value.ToString() == "Feline";
                //---------------------------
                textBox3.TextChanged -= textBox3_TextChanged;
                textBox3.Text = "";
                textBox3.TextChanged += textBox3_TextChanged;
            }
            else
            {
                initial_infos_fields();
            }
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void initial_infos_fields()
        {
            selected_anim = null;
            button3.Enabled = false;
            label3.Text = label4.Text = label6.Text = label8.Text = label13.Text = label14.Text = textBox2.Text = "--";
        }

        private void Laboratoire_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (make_historic_refesh)
            {
                load_labos_data();
                make_historic_refesh = false;
            }
        }

        private void Laboratoire_FormClosing(object sender, FormClosingEventArgs e)
        {
            selected_anim = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {
                this.ControlBox = false;
                Biochimie biochimie = new Biochimie(selected_anim);
                biochimie.Dock = DockStyle.Fill;
                this.Controls.Add(biochimie);
                biochimie.BringToFront();
            }
        }
        string histo_filter = "";
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {
                histo_filter = "ANIM_ID = " + selected_anim.Cells["ID"].Value;
                if (comboBox1.SelectedIndex == 1) //Hemogramme
                {
                    histo_filter += " AND LABO_NME LIKE 'Hemogramme'";
                }
                else if (comboBox1.SelectedIndex == 2) //Biochimie
                {
                    histo_filter += " AND LABO_NME LIKE 'Biochimie'";
                }
                else if (comboBox1.SelectedIndex == 3) //Immunologie
                {
                    histo_filter += " AND LABO_NME LIKE 'Immunologie'";
                }
                else if (comboBox1.SelectedIndex == 4) //Protéinogramme
                {
                    histo_filter += " AND LABO_NME LIKE 'Protéinogramme'";
                }
            }
            else
            {
                histo_filter = "";
            }

          //  if(textBox3.Text.Length > 0)
          //  {
                textBox3_TextChanged(null,null);
           // }
           // ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = histo_filter;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            DateTime tmp = DateTime.Now;
            bool tmp2 = DateTime.TryParse(textBox3.Text, out tmp);
            if(dataGridView1.Rows.Count > 0) {
                ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = (histo_filter != "" ? histo_filter + " AND " : "") + "(" +
                (tmp2 ? "DATE_TIME = '" + tmp.ToString("yyyy-MM-dd") + "'" : string.Format("CONVERT(Date_Time, System.String) LIKE '{0}%'", textBox3.Text)) +
                " OR REF LIKE '%" + textBox3.Text + "%')";
            }
            else
            {
                ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "DATE_TIME = '1970-12-12'"; //Just to clean the list
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {
                this.ControlBox = false;
                Immunologie immunologie = new Immunologie(selected_anim);
                immunologie.Dock = DockStyle.Fill;
                this.Controls.Add(immunologie);
                immunologie.BringToFront();
            }
        }
    }
}

