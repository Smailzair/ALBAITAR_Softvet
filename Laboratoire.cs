using ALBAITAR_Softvet.Labo;
using Microsoft.ReportingServices.Diagnostics.Internal;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using Excc = Microsoft.Office.Interop.Excel;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Laboratoire : Form
    {
        public static bool make_historic_refesh = false;
        DataTable animaux;
        public static DataTable labo;
        public static string labo_load_cmd = "SELECT 'Hemogramme' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_hemogramme UNION ALL "
                                           + "SELECT 'Biochimie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_biochimie  UNION ALL "
                                           + "SELECT 'Immunologie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_immunologie  UNION ALL "
                                           + "SELECT 'Protéinogramme' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_proteinogramme  UNION ALL "
                                           + "SELECT TYPE_ANAL AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_autre;";
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
                Hemogramme hemogramme = new Hemogramme(selected_anim,null);
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
               // button3.Enabled = dataGridView1.SelectedRows[0].Cells["ESPECE"].Value.ToString() == "Canine" || dataGridView1.SelectedRows[0].Cells["ESPECE"].Value.ToString() == "Feline";
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
            //button3.Enabled = false;
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
                Biochimie biochimie = new Biochimie(selected_anim, null);
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
                else if (comboBox1.SelectedIndex == 5) //Autres
                {
                    histo_filter += " AND LABO_NME NOT LIKE 'Hemogramme' AND LABO_NME NOT LIKE 'Biochimie' AND LABO_NME NOT LIKE 'Immunologie' AND LABO_NME NOT LIKE 'Protéinogramme'";
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
                Immunologie immunologie = new Immunologie(selected_anim, null);
                immunologie.Dock = DockStyle.Fill;
                this.Controls.Add(immunologie);
                immunologie.BringToFront();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {
                this.ControlBox = false;
                Protéinogramme prot = new Protéinogramme(selected_anim, null);
                prot.Dock = DockStyle.Fill;
                this.Controls.Add(prot);
                prot.BringToFront();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {
                this.ControlBox = false;
                Autre_Lab atr = new Autre_Lab(selected_anim, null);
                atr.Dock = DockStyle.Fill;
                this.Controls.Add(atr);
                atr.BringToFront();
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {            
            if (dataGridView2[e.ColumnIndex, e.RowIndex].Value != null && selected_anim != null)
            {
                               
                switch ((string)dataGridView2.Rows[e.RowIndex].Cells["LABO_NME"].Value)
                {
                    case "Hemogramme":                        
                        this.ControlBox = false;
                        Hemogramme labb = new Hemogramme(selected_anim, dataGridView2.Rows[e.RowIndex].Cells["IDD2"].Value.ToString());
                        labb.Dock = DockStyle.Fill;
                        this.Controls.Add(labb);
                        labb.BringToFront();
                        break;
                    case "Biochimie":
                        this.ControlBox = false;
                        Biochimie biochimie = new Biochimie(selected_anim, dataGridView2.Rows[e.RowIndex].Cells["IDD2"].Value.ToString());
                        biochimie.Dock = DockStyle.Fill;
                        this.Controls.Add(biochimie);
                        biochimie.BringToFront();
                        break;
                    case "Immunologie":
                        this.ControlBox = false;
                        Immunologie immunologie = new Immunologie(selected_anim, dataGridView2.Rows[e.RowIndex].Cells["IDD2"].Value.ToString());
                        immunologie.Dock = DockStyle.Fill;
                        this.Controls.Add(immunologie);
                        immunologie.BringToFront();
                        break;
                    case "Protéinogramme":
                        this.ControlBox = false;
                        Protéinogramme prot = new Protéinogramme(selected_anim, dataGridView2.Rows[e.RowIndex].Cells["IDD2"].Value.ToString());
                        prot.Dock = DockStyle.Fill;
                        this.Controls.Add(prot);
                        prot.BringToFront();
                        break;
                    default:
                        this.ControlBox = false;
                        Autre_Lab atr = new Autre_Lab(selected_anim, dataGridView2.Rows[e.RowIndex].Cells["IDD2"].Value.ToString());
                        atr.Dock = DockStyle.Fill;
                        this.Controls.Add(atr);
                        atr.BringToFront();
                        break;
                }
                
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {                
                if (comboBox1.SelectedIndex == 1) //Hemogramme
                {                    
                    DataTable dt_hemo = PreConnection.Load_data("SELECT `REF`,\r\n`DATE_TIME`,\r\n(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',\r\n(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',\r\n(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',\r\n(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',\r\n(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',\r\n`OBSERV`,\r\n`Hematies`,\r\n`Hemoglobine`,\r\n`Hematocrite`,\r\n`VGM`,\r\n`CCMH`,\r\n`TCMH`,\r\n`Reticulocytes`,\r\n`Plaquettes`,\r\n`Leucocytes`,\r\n`Granulocytes`,\r\n`Neutrophiles`,\r\n`Eosinophiles`,\r\n`Basophiles`,\r\n`Lymphocytes`,\r\n`Monocytes`,\r\n`Hematies_NORMATIF`,\r\n`Hemoglobine_NORMATIF`,\r\n`Hematocrite_NORMATIF`,\r\n`VGM_NORMATIF`,\r\n`CCMH_NORMATIF`,\r\n`TCMH_NORMATIF`,\r\n`Reticulocytes_NORMATIF`,\r\n`Plaquettes_NORMATIF`,\r\n`Leucocytes_NORMATIF`,\r\n`Granulocytes_NORMATIF`,\r\n`Neutrophiles_NORMATIF`,\r\n`Eosinophiles_NORMATIF`,\r\n`Basophiles_NORMATIF`,\r\n`Lymphocytes_NORMATIF`,\r\n`Monocytes_NORMATIF`\r\nFROM `tb_labo_hemogramme` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    Excc.Application xcelApp = new Excc.Application();
                    xcelApp.Application.Workbooks.Add(Type.Missing);
                    xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Hemogramme";
                    xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Hemogramme";
                    //-------------------
                    xcelApp.Cells[1, 1].Value = "Nom :";
                    xcelApp.Cells[1, 2].Value = dt_hemo.Rows[0]["ANIM_NME"];

                    xcelApp.Cells[1, 4].Value = "N° d'ident. :";
                    xcelApp.Cells[1, 5].Value = dt_hemo.Rows[0]["ANIM_IDENT_NUM"];

                    xcelApp.Cells[2, 1].Value = "Analyse de :";
                    xcelApp.Cells[2, 2].Value = "Hemogramme";

                    xcelApp.Cells[1, 8].Value = "Propriétaire :";
                    xcelApp.Cells[1, 9].Value = dt_hemo.Rows[0]["CLIENT_FULL_NME"];

                    xcelApp.Cells[1, 11].Value = "N° CNI :";
                    xcelApp.Cells[1, 12].Value = dt_hemo.Rows[0]["CLIENT_NUM_CNI"];

                    xcelApp.Cells[2, 8].Value = "N° Tél :";
                    xcelApp.Cells[2, 9].Value = dt_hemo.Rows[0]["CLIENT_NUM_PHONE"];

                    int[] ttt = { 1, 4,8,11};
                    ttt.ForEach(x => {
                        ((Excc.Range)xcelApp.Cells[1, x]).Interior.Color = ((Excc.Range)xcelApp.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                        ((Excc.Range)xcelApp.Cells[1, x]).Font.Underline = ((Excc.Range)xcelApp.Cells[2, x]).Font.Underline = true;
                    });                    
                    //--------------------
                    xcelApp.Cells[4, 1].Value = "Date";
                    ((Excc.Range)xcelApp.Columns[1]).NumberFormat = "dd/MM/yyyy";
                    xcelApp.Cells[4, 2].Value = "Ref.";
                    dt_hemo.Columns.Cast<DataColumn>().Where(dd => dt_hemo.Columns.IndexOf(dd) >= 8 && dt_hemo.Columns.IndexOf(dd) < 23).ToList().ForEach(SS => {
                        xcelApp.Cells[4, dt_hemo.Columns.IndexOf(SS) - 5].Value = SS.ColumnName;
                    });
                    xcelApp.Cells[4, 18].Value = "Observ.";
                    for (int i = 1; i < 19; i++)
                    {
                        ((Excc.Range)xcelApp.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                        ((Excc.Range)xcelApp.Cells[4, i]).Font.Bold = true;
                        ((Excc.Range)xcelApp.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                    }
                    //xcelApp.Columns.AutoFit();
                    int y = 4;
                    dt_hemo.Rows.Cast<DataRow>().ForEach(PP => {
                        y++;
                        xcelApp.Cells[y , 1].Value = PP["DATE_TIME"];
                        xcelApp.Cells[y, 2].Value = PP["REF"];
                        for(int t = 1; t < 16; t++)
                        {
                            xcelApp.Cells[y, t + 2].Value = PP[t + 7];
                        }
                    });
                    
                    //-------------


                    //------------------
                    SaveFileDialog svd = new SaveFileDialog();
                    svd.Filter = "Excel | *.xlsx";
                    svd.DefaultExt = "*.xlsx";
                    svd.FileName = xcelApp.Application.Workbooks[1].Title + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                    if (svd.ShowDialog() == DialogResult.OK)
                    {
                        xcelApp.Workbooks[1].SaveAs(Path.GetFullPath(svd.FileName));
                        Process.Start(Path.GetFullPath(svd.FileName));
                    }
                    xcelApp.Application.Workbooks[1].Close(false);
                    xcelApp.Quit();
                    //-------------------
                }
                else if (comboBox1.SelectedIndex == 2) //Biochimie
                {
                   
                }
                else if (comboBox1.SelectedIndex == 3) //Immunologie
                {
                    
                }
                else if (comboBox1.SelectedIndex == 4) //Protéinogramme
                {
                   
                }
                else if (comboBox1.SelectedIndex == 5) //Autres
                {
                   
                }
            }
        }
    }
}

