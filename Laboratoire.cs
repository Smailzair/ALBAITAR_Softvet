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
                                           + "SELECT 'Biochimie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_biochimie UNION ALL "
                                           + "SELECT 'Immunologie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_immunologie UNION ALL "
                                           + "SELECT 'Protéinogramme' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_proteinogramme UNION ALL "
                                           + "SELECT 'Urologie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_urologie UNION ALL "
                                           + "SELECT TYPE_ANAL AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME`,`OBSERV` FROM tb_labo_autre;";
        static DataGridViewRow selected_anim = null;
        string Ref_To_Selectt = "";
        bool Just_to_printt = false;
        int anim_id_to_selectt = -1;
        string lab_nmee = "";
        public Laboratoire(int anim_id_to_select, string Ref_To_Select, bool? Just_to_print, string Lab_nme)
        {
            anim_id_to_selectt = anim_id_to_select;
            Ref_To_Selectt = Ref_To_Select;
            Just_to_printt = (bool)(Just_to_print != null ? Just_to_print : false);
            lab_nmee = Lab_nme;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {
                this.ControlBox = false;
                Hemogramme hemogramme = new Hemogramme(selected_anim, null);
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
            //------------------
            if (anim_id_to_selectt > -1)
            {                
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(zz => (int)zz.Cells["ID"].Value == anim_id_to_selectt).ForEach(cc => cc.Selected = true);                
            }

            if (Ref_To_Selectt.Length > 0)
            {
                textBox3.Text = Ref_To_Selectt;
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows[0].Selected = true;
                    DataGridViewCellEventArgs arrg = new DataGridViewCellEventArgs(1, 0);
                    dataGridView2_CellDoubleClick(dataGridView2, arrg);
                }
            }
            else if (lab_nmee.Length > 0)
            {                
                switch (lab_nmee)
                {                    
                    case "Hemogramme":
                        button2.PerformClick();
                        break;
                    case "Biochimie":
                        button1.PerformClick();
                        break;
                    case "Immunologie":
                        button3.PerformClick();
                        break;
                    case "Protéinogramme":
                        button6.PerformClick();
                        break;
                    case "Urologie":
                        button7.PerformClick();
                        break;
                    default:
                        button4.PerformClick();
                        break;
                }

            }
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
            if (textBox3.Text == Ref_To_Selectt)
            {
                textBox3.Text = Ref_To_Selectt = "";
            }
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
                else if (comboBox1.SelectedIndex == 5) //Urologie
                {
                    histo_filter += " AND LABO_NME LIKE 'Urologie'";
                }
                else if (comboBox1.SelectedIndex == 6) //Autres
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
            textBox3_TextChanged(null, null);
            // }
            // ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = histo_filter;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            DateTime tmp = DateTime.Now;
            bool tmp2 = DateTime.TryParse(textBox3.Text, out tmp);
            if (dataGridView1.Rows.Count > 0)
            {
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
                    case "Urologie":
                        this.ControlBox = false;
                        Urologie urol = new Urologie(selected_anim, dataGridView2.Rows[e.RowIndex].Cells["IDD2"].Value.ToString());
                        urol.Dock = DockStyle.Fill;
                        this.Controls.Add(urol);
                        urol.BringToFront();
                        break;
                    default:
                        this.ControlBox = false;
                        Autre_Lab atr = new Autre_Lab(selected_anim, dataGridView2.Rows[e.RowIndex].Cells["IDD2"].Value.ToString());
                        atr.Dock = DockStyle.Fill;
                        this.Controls.Add(atr);
                        atr.BringToFront();
                        this.ActiveControl = atr;
                        break;
                }
                //----------------------------------
                if (Just_to_printt)
                {
                    ((Button)this.Controls[0].Controls["button5"]).PerformClick();
                    Close();
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
                    if (dt_hemo.Rows.Count > 0)
                    {
                        Excc.Application xcelApp = new Excc.Application();
                        xcelApp.Application.Workbooks.Add(Type.Missing);
                        xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Hemogramme";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Hemogramme";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Rows[4].RowHeight = 30;
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

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)xcelApp.Cells[1, x]).Interior.Color = ((Excc.Range)xcelApp.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)xcelApp.Cells[1, x]).Font.Underline = ((Excc.Range)xcelApp.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        xcelApp.Cells[4, 1].Value = "Date";
                        ((Excc.Range)xcelApp.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        xcelApp.Cells[4, 2].Value = "Ref.";
                        dt_hemo.Columns.Cast<DataColumn>().Where(dd => dt_hemo.Columns.IndexOf(dd) >= 8 && dt_hemo.Columns.IndexOf(dd) < 23).ToList().ForEach(SS =>
                        {
                            xcelApp.Cells[4, dt_hemo.Columns.IndexOf(SS) - 5].Value = SS.ColumnName;
                        });
                        xcelApp.Cells[4, 18].Value = "Observ.";
                        xcelApp.Cells[5, 1].Value = "Normatifs :";
                        for (int i = 1; i < 19; i++)
                        {
                            ((Excc.Range)xcelApp.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)xcelApp.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)xcelApp.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)xcelApp.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;
                            if (i < 16)
                            {
                                xcelApp.Cells[5, i + 2].Value2 = "'" + dt_hemo.Rows[0][i + 22].ToString();
                                ((Excc.Range)xcelApp.Cells[5, i + 2]).HorizontalAlignment = Excc.XlHAlign.xlHAlignRight;
                            }
                            ((Excc.Range)xcelApp.Cells[5, i]).Interior.Color = ColorTranslator.ToOle(Color.Pink);
                        }
                        int y = 5;
                        dt_hemo.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            xcelApp.Cells[y, 1].Value = PP["DATE_TIME"];
                            xcelApp.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 16; t++)
                            {
                                xcelApp.Cells[y, t + 2].Value = PP[t + 7];
                            }
                            xcelApp.Cells[y, 18].Value = PP["OBSERV"];
                        });
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_hemo.Rows.Count + 5, 17]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_hemo.Rows.Count + 5, 17]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_hemo.Rows.Count + 5, 17]]).Borders.Color = Color.Gray;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_hemo.Rows.Count + 5, 17]]).Columns.AutoFit();
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
                }
                else if (comboBox1.SelectedIndex == 2) //Biochimie
                {
                    DataTable dt_bioch = PreConnection.Load_data("SELECT `REF`,\r\n`DATE_TIME`,\r\n(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',\r\n(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',\r\n(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',\r\n(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',\r\n(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',\r\n`OBSERV`,\r\n`Glucose`,\r\n`Urée (BUN)`,\r\n`Créatinine`,\r\n`Acide Urique`,\r\n`Cholesterol`,\r\n`Triglycérides`,\r\n`Proteines Totales`,\r\n`Albumina`,\r\n`Globulines`,\r\n`Indice alb/glb`,\r\n`Bilirubine Totale`,\r\n`Bilirubine Conjuguée`,\r\n`GPT(ALT)`,\r\n`GOT(AST)`,\r\n`Phosphatases Alc`,\r\n`Gamma-GT`,\r\n`L.D.H`,\r\n`C.P.K`,\r\n`Lipase`,\r\n`Amylase`,\r\n`Fructosamine`,\r\n`Calcium`,\r\n`Phosphore`,\r\n`Chlore`,\r\n`Potassium`,\r\n`Sodium`,\r\n`Amoniac`,\r\n`Fer`,\r\n`Glucose_NORMATIF`,\r\n`Urée (BUN)_NORMATIF`,\r\n`Créatinine_NORMATIF`,\r\n`Acide Urique_NORMATIF`,\r\n`Cholesterol_NORMATIF`,\r\n`Triglycérides_NORMATIF`,\r\n`Proteines Totales_NORMATIF`,\r\n`Albumina_NORMATIF`,\r\n`Globulines_NORMATIF`,\r\n`Indice alb/glb_NORMATIF`,\r\n`Bilirubine Totale_NORMATIF`,\r\n`Bilirubine Conjuguée_NORMATIF`,\r\n`GPT(ALT)_NORMATIF`,\r\n`GOT(AST)_NORMATIF`,\r\n`Phosphatases Alc_NORMATIF`,\r\n`Gamma-GT_NORMATIF`,\r\n`L.D.H_NORMATIF`,\r\n`C.P.K_NORMATIF`,\r\n`Lipase_NORMATIF`,\r\n`Amylase_NORMATIF`,\r\n`Fructosamine_NORMATIF`,\r\n`Calcium_NORMATIF`,\r\n`Phosphore_NORMATIF`,\r\n`Chlore_NORMATIF`,\r\n`Potassium_NORMATIF`,\r\n`Sodium_NORMATIF`,\r\n`Amoniac_NORMATIF`,\r\n`Fer_NORMATIF`\r\nFROM `tb_labo_biochimie` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_bioch.Rows.Count > 0)
                    {
                        Excc.Application xcelApp = new Excc.Application();
                        xcelApp.Application.Workbooks.Add(Type.Missing);
                        xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Biochimie";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Biochimie";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Rows[4].RowHeight = 30;
                        //-------------------
                        xcelApp.Cells[1, 1].Value = "Nom :";
                        xcelApp.Cells[1, 2].Value = dt_bioch.Rows[0]["ANIM_NME"];

                        xcelApp.Cells[1, 4].Value = "N° d'ident. :";
                        xcelApp.Cells[1, 5].Value = dt_bioch.Rows[0]["ANIM_IDENT_NUM"];

                        xcelApp.Cells[2, 1].Value = "Analyse de :";
                        xcelApp.Cells[2, 2].Value = "Biochimie";

                        xcelApp.Cells[1, 8].Value = "Propriétaire :";
                        xcelApp.Cells[1, 9].Value = dt_bioch.Rows[0]["CLIENT_FULL_NME"];

                        xcelApp.Cells[1, 11].Value = "N° CNI :";
                        xcelApp.Cells[1, 12].Value = dt_bioch.Rows[0]["CLIENT_NUM_CNI"];

                        xcelApp.Cells[2, 8].Value = "N° Tél :";
                        xcelApp.Cells[2, 9].Value = dt_bioch.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)xcelApp.Cells[1, x]).Interior.Color = ((Excc.Range)xcelApp.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)xcelApp.Cells[1, x]).Font.Underline = ((Excc.Range)xcelApp.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        xcelApp.Cells[4, 1].Value = "Date";
                        ((Excc.Range)xcelApp.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        xcelApp.Cells[4, 2].Value = "Ref.";
                        dt_bioch.Columns.Cast<DataColumn>().Where(dd => dt_bioch.Columns.IndexOf(dd) >= 8 && dt_bioch.Columns.IndexOf(dd) < 36).ToList().ForEach(SS =>
                        {
                            xcelApp.Cells[4, dt_bioch.Columns.IndexOf(SS) - 5].Value = SS.ColumnName;
                        });
                        xcelApp.Cells[4, 31].Value = "Observ.";
                        xcelApp.Cells[5, 1].Value = "Normatifs :";
                        for (int i = 1; i < 32; i++)
                        {
                            ((Excc.Range)xcelApp.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)xcelApp.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)xcelApp.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)xcelApp.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;

                            if (i < 29)
                            {
                                xcelApp.Cells[5, i + 2].Value2 = "'" + dt_bioch.Rows[0][i + 35].ToString();
                                ((Excc.Range)xcelApp.Cells[5, i + 2]).HorizontalAlignment = Excc.XlHAlign.xlHAlignRight;
                            }
                            ((Excc.Range)xcelApp.Cells[5, i]).Interior.Color = ColorTranslator.ToOle(Color.Pink);
                        }

                        int y = 5;
                        dt_bioch.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            xcelApp.Cells[y, 1].Value = PP["DATE_TIME"];
                            xcelApp.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 29; t++)
                            {
                                xcelApp.Cells[y, t + 2].Value = PP[t + 7];
                            }
                            xcelApp.Cells[y, 31].Value = PP["OBSERV"];

                        });
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_bioch.Rows.Count + 5, 30]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_bioch.Rows.Count + 5, 30]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_bioch.Rows.Count + 5, 30]]).Borders.Color = Color.Gray;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_bioch.Rows.Count + 5, 30]]).Columns.AutoFit();

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
                }
                else if (comboBox1.SelectedIndex == 3) //Immunologie
                {
                    DataTable dt_immun = PreConnection.Load_data("SELECT `REF`,`DATE_TIME`,(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',`OBSERV`,\r\n`MALAD_NME_001`,\r\n`MALAD_NME_002`,\r\n`MALAD_NME_003`,\r\n`MALAD_NME_004`,\r\n`MALAD_NME_005`,\r\n`MALAD_NME_006`,\r\n`MALAD_NME_007`,\r\n`MALAD_NME_008`,\r\n`MALAD_NME_009`,\r\n`MALAD_NME_010`,\r\n`MALAD_NME_011`,\r\n`MALAD_NME_012`,\r\n`MALAD_NME_013`,\r\n`MALAD_NME_014`,\r\n`MALAD_NME_015`,\r\n`METHODE_001`,\r\n`METHODE_002`,\r\n`METHODE_003`,\r\n`METHODE_004`,\r\n`METHODE_005`,\r\n`METHODE_006`,\r\n`METHODE_007`,\r\n`METHODE_008`,\r\n`METHODE_009`,\r\n`METHODE_010`,\r\n`METHODE_011`,\r\n`METHODE_012`,\r\n`METHODE_013`,\r\n`METHODE_014`,\r\n`METHODE_015`,\r\n`MALAD_RESULT_001`,\r\n`MALAD_RESULT_002`,\r\n`MALAD_RESULT_003`,\r\n`MALAD_RESULT_004`,\r\n`MALAD_RESULT_005`,\r\n`MALAD_RESULT_006`,\r\n`MALAD_RESULT_007`,\r\n`MALAD_RESULT_008`,\r\n`MALAD_RESULT_009`,\r\n`MALAD_RESULT_010`,\r\n`MALAD_RESULT_011`,\r\n`MALAD_RESULT_012`,\r\n`MALAD_RESULT_013`,\r\n`MALAD_RESULT_014`,\r\n`MALAD_RESULT_015`\r\nFROM `tb_labo_immunologie` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_immun.Rows.Count > 0)
                    {
                        Excc.Application xcelApp = new Excc.Application();
                        xcelApp.Application.Workbooks.Add(Type.Missing);
                        xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Immunologie";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Immunologie";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Rows[4].RowHeight = 30;
                        //-------------------
                        xcelApp.Cells[1, 1].Value = "Nom :";
                        xcelApp.Cells[1, 2].Value = dt_immun.Rows[0]["ANIM_NME"];

                        xcelApp.Cells[1, 4].Value = "N° d'ident. :";
                        xcelApp.Cells[1, 5].Value = dt_immun.Rows[0]["ANIM_IDENT_NUM"];

                        xcelApp.Cells[2, 1].Value = "Analyse de :";
                        xcelApp.Cells[2, 2].Value = "Immunologie";

                        xcelApp.Cells[1, 8].Value = "Propriétaire :";
                        xcelApp.Cells[1, 9].Value = dt_immun.Rows[0]["CLIENT_FULL_NME"];

                        xcelApp.Cells[1, 11].Value = "N° CNI :";
                        xcelApp.Cells[1, 12].Value = dt_immun.Rows[0]["CLIENT_NUM_CNI"];

                        xcelApp.Cells[2, 8].Value = "N° Tél :";
                        xcelApp.Cells[2, 9].Value = dt_immun.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)xcelApp.Cells[1, x]).Interior.Color = ((Excc.Range)xcelApp.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)xcelApp.Cells[1, x]).Font.Underline = ((Excc.Range)xcelApp.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        xcelApp.Cells[4, 1].Value = "Date";
                        ((Excc.Range)xcelApp.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        xcelApp.Cells[4, 2].Value = "Ref.";

                        xcelApp.Cells[4, 11].Value = "Observ.";
                        xcelApp.Cells[5, 1].Value = "Unité :";
                        for (int i = 1; i < 12; i++)//19
                        {
                            ((Excc.Range)xcelApp.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)xcelApp.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)xcelApp.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)xcelApp.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;



                            if (i < 9)
                            {
                                xcelApp.Cells[4, i + 2].Value = dt_immun.Rows[0][i + 7].ToString();
                                xcelApp.Cells[5, i + 2].Value2 = "'" + dt_immun.Rows[0][i + 22].ToString();
                                ((Excc.Range)xcelApp.Cells[5, i + 2]).HorizontalAlignment = Excc.XlHAlign.xlHAlignRight;
                            }
                            ((Excc.Range)xcelApp.Cells[5, i]).Interior.Color = ColorTranslator.ToOle(Color.Pink);
                        }

                        int y = 5;
                        dt_immun.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            xcelApp.Cells[y, 1].Value = PP["DATE_TIME"];
                            xcelApp.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 9; t++)
                            {
                                xcelApp.Cells[y, t + 2].Value = PP[t + 37];
                            }
                            xcelApp.Cells[y, 11].Value = PP["OBSERV"];

                        });
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_immun.Rows.Count + 5, 10]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_immun.Rows.Count + 5, 10]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_immun.Rows.Count + 5, 10]]).Borders.Color = Color.Gray;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_immun.Rows.Count + 5, 10]]).Columns.AutoFit();

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
                    }
                }
                else if (comboBox1.SelectedIndex == 4) //Protéinogramme
                {
                    DataTable dt_prot = PreConnection.Load_data("SELECT `REF`,`DATE_TIME`,(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',`OBSERV`,\r\n`Protéines Totales`,\r\n`Albumine`,\r\n`Alpha-1-Globulines`,\r\n`Alpha-2-Globulines`,\r\n`Beta-Globulines`,\r\n`Gamma-Globulines`,\r\n`Globulines Totales`,\r\n`Coefficient A/G`,\r\n`Protéines Totales_UNIT`,\r\n`Albumine_UNIT`,\r\n`Alpha-1-Globulines_UNIT`,\r\n`Alpha-2-Globulines_UNIT`,\r\n`Beta-Globulines_UNIT`,\r\n`Gamma-Globulines_UNIT`,\r\n`Globulines Totales_UNIT`,\r\n`Coefficient A/G_UNIT`,\r\n`Protéines Totales_NORMATIF`,\r\n`Albumine_NORMATIF`,\r\n`Alpha-1-Globulines_NORMATIF`,\r\n`Alpha-2-Globulines_NORMATIF`,\r\n`Beta-Globulines_NORMATIF`,\r\n`Gamma-Globulines_NORMATIF`,\r\n`Globulines Totales_NORMATIF`,\r\n`Coefficient A/G_NORMATIF`\r\nFROM `tb_labo_proteinogramme` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_prot.Rows.Count > 0)
                    {
                        Excc.Application xcelApp = new Excc.Application();
                        xcelApp.Application.Workbooks.Add(Type.Missing);
                        xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Protéinogramme";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Protéinogramme";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Rows[4].RowHeight = 30;
                        //-------------------
                        xcelApp.Cells[1, 1].Value = "Nom :";
                        xcelApp.Cells[1, 2].Value = dt_prot.Rows[0]["ANIM_NME"];

                        xcelApp.Cells[1, 4].Value = "N° d'ident. :";
                        xcelApp.Cells[1, 5].Value = dt_prot.Rows[0]["ANIM_IDENT_NUM"];

                        xcelApp.Cells[2, 1].Value = "Analyse de :";
                        xcelApp.Cells[2, 2].Value = "Protéinogramme";

                        xcelApp.Cells[1, 8].Value = "Propriétaire :";
                        xcelApp.Cells[1, 9].Value = dt_prot.Rows[0]["CLIENT_FULL_NME"];

                        xcelApp.Cells[1, 11].Value = "N° CNI :";
                        xcelApp.Cells[1, 12].Value = dt_prot.Rows[0]["CLIENT_NUM_CNI"];

                        xcelApp.Cells[2, 8].Value = "N° Tél :";
                        xcelApp.Cells[2, 9].Value = dt_prot.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)xcelApp.Cells[1, x]).Interior.Color = ((Excc.Range)xcelApp.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)xcelApp.Cells[1, x]).Font.Underline = ((Excc.Range)xcelApp.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        xcelApp.Cells[4, 1].Value = "Date";
                        ((Excc.Range)xcelApp.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        xcelApp.Cells[4, 2].Value = "Ref.";
                        dt_prot.Columns.Cast<DataColumn>().Where(dd => dt_prot.Columns.IndexOf(dd) >= 8 && dt_prot.Columns.IndexOf(dd) < 17).ToList().ForEach(SS =>
                        {
                            xcelApp.Cells[4, dt_prot.Columns.IndexOf(SS) - 5].Value = SS.ColumnName;
                        });
                        xcelApp.Cells[4, 11].Value = "Observ.";
                        xcelApp.Cells[5, 1].Value = "Unité :";
                        for (int i = 1; i < 12; i++)
                        {
                            ((Excc.Range)xcelApp.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)xcelApp.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)xcelApp.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)xcelApp.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;
                            if (i < 9)
                            {
                                xcelApp.Cells[5, i + 2].Value2 = "'" + "(" + dt_prot.Rows[0][i + 15].ToString() + ")";
                                ((Excc.Range)xcelApp.Cells[5, i + 2]).HorizontalAlignment = Excc.XlHAlign.xlHAlignRight;
                            }
                            ((Excc.Range)xcelApp.Cells[5, i]).Interior.Color = ColorTranslator.ToOle(Color.Pink);
                        }

                        int y = 5;
                        dt_prot.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            xcelApp.Cells[y, 1].Value = PP["DATE_TIME"];
                            xcelApp.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 9; t++)
                            {
                                xcelApp.Cells[y, t + 2].Value = PP[t + 7];
                            }
                            xcelApp.Cells[y, 11].Value = PP["OBSERV"];
                        });
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_prot.Rows.Count + 5, 10]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_prot.Rows.Count + 5, 10]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_prot.Rows.Count + 5, 10]]).Borders.Color = Color.Gray;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_prot.Rows.Count + 5, 10]]).Columns.AutoFit();



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
                }
                else if (comboBox1.SelectedIndex == 5) //Autres
                {
                    DataTable dt_autre = PreConnection.Load_data("SELECT `REF`,`DATE_TIME`,(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',`OBSERV`,\r\n`TYPE_ANAL`,\r\n`METHODE`,\r\n`RESULT`\r\nFROM `tb_labo_autre` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_autre.Rows.Count > 0)
                    {
                        Excc.Application xcelApp = new Excc.Application();
                        xcelApp.Application.Workbooks.Add(Type.Missing);
                        xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Autres";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Autres";
                        xcelApp.Application.Workbooks[1].Worksheets[1].Rows[4].RowHeight = 30;
                        //-------------------
                        xcelApp.Cells[1, 1].Value = "Nom :";
                        xcelApp.Cells[1, 2].Value = dt_autre.Rows[0]["ANIM_NME"];

                        xcelApp.Cells[1, 4].Value = "N° d'ident. :";
                        xcelApp.Cells[1, 5].Value = dt_autre.Rows[0]["ANIM_IDENT_NUM"];

                        xcelApp.Cells[2, 1].Value = "Analyse de :";
                        xcelApp.Cells[2, 2].Value = "Autres Analsyes";

                        xcelApp.Cells[1, 8].Value = "Propriétaire :";
                        xcelApp.Cells[1, 9].Value = dt_autre.Rows[0]["CLIENT_FULL_NME"];

                        xcelApp.Cells[1, 11].Value = "N° CNI :";
                        xcelApp.Cells[1, 12].Value = dt_autre.Rows[0]["CLIENT_NUM_CNI"];

                        xcelApp.Cells[2, 8].Value = "N° Tél :";
                        xcelApp.Cells[2, 9].Value = dt_autre.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)xcelApp.Cells[1, x]).Interior.Color = ((Excc.Range)xcelApp.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)xcelApp.Cells[1, x]).Font.Underline = ((Excc.Range)xcelApp.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        xcelApp.Cells[4, 1].Value = "Date";
                        ((Excc.Range)xcelApp.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        xcelApp.Cells[4, 2].Value = "Ref.";
                        xcelApp.Cells[4, 3].Value = "Type d'analyse";
                        xcelApp.Cells[4, 4].Value = "Méthode";
                        xcelApp.Cells[4, 5].Value = "Résultat";
                        xcelApp.Cells[4, 6].Value = "Observ.";
                        for (int i = 1; i < 7; i++)
                        {
                            ((Excc.Range)xcelApp.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)xcelApp.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)xcelApp.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)xcelApp.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;
                        }
                        int y = 4;
                        dt_autre.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            xcelApp.Cells[y, 1].Value = PP["DATE_TIME"];
                            xcelApp.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 4; t++)
                            {
                                xcelApp.Cells[y, t + 2].Value = PP[t + 7];
                            }
                            xcelApp.Cells[y, 6].Value = PP["OBSERV"];
                        });
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_autre.Rows.Count + 4, 5]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_autre.Rows.Count + 4, 5]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_autre.Rows.Count + 4, 5]]).Borders.Color = Color.Gray;
                        ((Excc.Range)xcelApp.Range[xcelApp.Cells[4, 1], xcelApp.Cells[dt_autre.Rows.Count + 4, 5]]).Columns.AutoFit();
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
                }
                else //Tous
                {
                    Excc.Application xcelApp = new Excc.Application();
                    Excc.Workbook workbook = xcelApp.Workbooks.Add(Type.Missing);
                    workbook.Title = Application.ProductName + "Analyses";
                    //Hemogramme ===========================================

                    DataTable dt_hemo = PreConnection.Load_data("SELECT `REF`,\r\n`DATE_TIME`,\r\n(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',\r\n(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',\r\n(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',\r\n(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',\r\n(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',\r\n`OBSERV`,\r\n`Hematies`,\r\n`Hemoglobine`,\r\n`Hematocrite`,\r\n`VGM`,\r\n`CCMH`,\r\n`TCMH`,\r\n`Reticulocytes`,\r\n`Plaquettes`,\r\n`Leucocytes`,\r\n`Granulocytes`,\r\n`Neutrophiles`,\r\n`Eosinophiles`,\r\n`Basophiles`,\r\n`Lymphocytes`,\r\n`Monocytes`,\r\n`Hematies_NORMATIF`,\r\n`Hemoglobine_NORMATIF`,\r\n`Hematocrite_NORMATIF`,\r\n`VGM_NORMATIF`,\r\n`CCMH_NORMATIF`,\r\n`TCMH_NORMATIF`,\r\n`Reticulocytes_NORMATIF`,\r\n`Plaquettes_NORMATIF`,\r\n`Leucocytes_NORMATIF`,\r\n`Granulocytes_NORMATIF`,\r\n`Neutrophiles_NORMATIF`,\r\n`Eosinophiles_NORMATIF`,\r\n`Basophiles_NORMATIF`,\r\n`Lymphocytes_NORMATIF`,\r\n`Monocytes_NORMATIF`\r\nFROM `tb_labo_hemogramme` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_hemo.Rows.Count > 0)
                    {
                        Excc.Worksheet worksheet_hemo = workbook.Worksheets.Add();
                        worksheet_hemo.Activate();
                        //--------------------
                        worksheet_hemo.Name = "Hemogramme";
                        worksheet_hemo.Rows[4].RowHeight = 30;
                        //-------------------
                        worksheet_hemo.Cells[1, 1].Value = "Nom :";
                        worksheet_hemo.Cells[1, 2].Value = dt_hemo.Rows[0]["ANIM_NME"];

                        worksheet_hemo.Cells[1, 4].Value = "N° d'ident. :";
                        worksheet_hemo.Cells[1, 5].Value = dt_hemo.Rows[0]["ANIM_IDENT_NUM"];

                        worksheet_hemo.Cells[2, 1].Value = "Analyse de :";
                        worksheet_hemo.Cells[2, 2].Value = "Hemogramme";

                        worksheet_hemo.Cells[1, 8].Value = "Propriétaire :";
                        worksheet_hemo.Cells[1, 9].Value = dt_hemo.Rows[0]["CLIENT_FULL_NME"];

                        worksheet_hemo.Cells[1, 11].Value = "N° CNI :";
                        worksheet_hemo.Cells[1, 12].Value = dt_hemo.Rows[0]["CLIENT_NUM_CNI"];

                        worksheet_hemo.Cells[2, 8].Value = "N° Tél :";
                        worksheet_hemo.Cells[2, 9].Value = dt_hemo.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)worksheet_hemo.Cells[1, x]).Interior.Color = ((Excc.Range)worksheet_hemo.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)worksheet_hemo.Cells[1, x]).Font.Underline = ((Excc.Range)worksheet_hemo.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        worksheet_hemo.Cells[4, 1].Value = "Date";
                        ((Excc.Range)worksheet_hemo.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        worksheet_hemo.Cells[4, 2].Value = "Ref.";
                        dt_hemo.Columns.Cast<DataColumn>().Where(dd => dt_hemo.Columns.IndexOf(dd) >= 8 && dt_hemo.Columns.IndexOf(dd) < 23).ToList().ForEach(SS =>
                        {
                            worksheet_hemo.Cells[4, dt_hemo.Columns.IndexOf(SS) - 5].Value = SS.ColumnName;
                        });
                        worksheet_hemo.Cells[4, 18].Value = "Observ.";
                        worksheet_hemo.Cells[5, 1].Value = "Normatifs :";
                        for (int i = 1; i < 19; i++)
                        {
                            ((Excc.Range)worksheet_hemo.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)worksheet_hemo.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)worksheet_hemo.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)worksheet_hemo.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;
                            if (i < 16)
                            {
                                worksheet_hemo.Cells[5, i + 2].Value2 = "'" + dt_hemo.Rows[0][i + 22].ToString();
                                ((Excc.Range)worksheet_hemo.Cells[5, i + 2]).HorizontalAlignment = Excc.XlHAlign.xlHAlignRight;
                            }
                            ((Excc.Range)worksheet_hemo.Cells[5, i]).Interior.Color = ColorTranslator.ToOle(Color.Pink);
                        }
                        int y = 5;
                        dt_hemo.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            worksheet_hemo.Cells[y, 1].Value = PP["DATE_TIME"];
                            worksheet_hemo.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 16; t++)
                            {
                                worksheet_hemo.Cells[y, t + 2].Value = PP[t + 7];
                            }
                            worksheet_hemo.Cells[y, 18].Value = PP["OBSERV"];
                        });
                        ((Excc.Range)worksheet_hemo.Range[worksheet_hemo.Cells[4, 1], worksheet_hemo.Cells[dt_hemo.Rows.Count + 5, 17]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)worksheet_hemo.Range[worksheet_hemo.Cells[4, 1], worksheet_hemo.Cells[dt_hemo.Rows.Count + 5, 17]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)worksheet_hemo.Range[worksheet_hemo.Cells[4, 1], worksheet_hemo.Cells[dt_hemo.Rows.Count + 5, 17]]).Borders.Color = Color.Gray;
                        ((Excc.Range)worksheet_hemo.Range[worksheet_hemo.Cells[4, 1], worksheet_hemo.Cells[dt_hemo.Rows.Count + 5, 17]]).Columns.AutoFit();

                        //-------------------
                    }

                    //Biochimie ===============================================

                    DataTable dt_bioch = PreConnection.Load_data("SELECT `REF`,\r\n`DATE_TIME`,\r\n(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',\r\n(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',\r\n(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',\r\n(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',\r\n(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',\r\n`OBSERV`,\r\n`Glucose`,\r\n`Urée (BUN)`,\r\n`Créatinine`,\r\n`Acide Urique`,\r\n`Cholesterol`,\r\n`Triglycérides`,\r\n`Proteines Totales`,\r\n`Albumina`,\r\n`Globulines`,\r\n`Indice alb/glb`,\r\n`Bilirubine Totale`,\r\n`Bilirubine Conjuguée`,\r\n`GPT(ALT)`,\r\n`GOT(AST)`,\r\n`Phosphatases Alc`,\r\n`Gamma-GT`,\r\n`L.D.H`,\r\n`C.P.K`,\r\n`Lipase`,\r\n`Amylase`,\r\n`Fructosamine`,\r\n`Calcium`,\r\n`Phosphore`,\r\n`Chlore`,\r\n`Potassium`,\r\n`Sodium`,\r\n`Amoniac`,\r\n`Fer`,\r\n`Glucose_NORMATIF`,\r\n`Urée (BUN)_NORMATIF`,\r\n`Créatinine_NORMATIF`,\r\n`Acide Urique_NORMATIF`,\r\n`Cholesterol_NORMATIF`,\r\n`Triglycérides_NORMATIF`,\r\n`Proteines Totales_NORMATIF`,\r\n`Albumina_NORMATIF`,\r\n`Globulines_NORMATIF`,\r\n`Indice alb/glb_NORMATIF`,\r\n`Bilirubine Totale_NORMATIF`,\r\n`Bilirubine Conjuguée_NORMATIF`,\r\n`GPT(ALT)_NORMATIF`,\r\n`GOT(AST)_NORMATIF`,\r\n`Phosphatases Alc_NORMATIF`,\r\n`Gamma-GT_NORMATIF`,\r\n`L.D.H_NORMATIF`,\r\n`C.P.K_NORMATIF`,\r\n`Lipase_NORMATIF`,\r\n`Amylase_NORMATIF`,\r\n`Fructosamine_NORMATIF`,\r\n`Calcium_NORMATIF`,\r\n`Phosphore_NORMATIF`,\r\n`Chlore_NORMATIF`,\r\n`Potassium_NORMATIF`,\r\n`Sodium_NORMATIF`,\r\n`Amoniac_NORMATIF`,\r\n`Fer_NORMATIF`\r\nFROM `tb_labo_biochimie` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_bioch.Rows.Count > 0)
                    {
                        Excc.Worksheet worksheet_bioch = workbook.Worksheets.Add();
                        worksheet_bioch.Activate();
                        //--------------------
                        worksheet_bioch.Name = "Biochimie";
                        worksheet_bioch.Rows[4].RowHeight = 30;
                        //-------------------
                        worksheet_bioch.Cells[1, 1].Value = "Nom :";
                        worksheet_bioch.Cells[1, 2].Value = dt_bioch.Rows[0]["ANIM_NME"];

                        worksheet_bioch.Cells[1, 4].Value = "N° d'ident. :";
                        worksheet_bioch.Cells[1, 5].Value = dt_bioch.Rows[0]["ANIM_IDENT_NUM"];

                        worksheet_bioch.Cells[2, 1].Value = "Analyse de :";
                        worksheet_bioch.Cells[2, 2].Value = "Biochimie";

                        worksheet_bioch.Cells[1, 8].Value = "Propriétaire :";
                        worksheet_bioch.Cells[1, 9].Value = dt_bioch.Rows[0]["CLIENT_FULL_NME"];

                        worksheet_bioch.Cells[1, 11].Value = "N° CNI :";
                        worksheet_bioch.Cells[1, 12].Value = dt_bioch.Rows[0]["CLIENT_NUM_CNI"];

                        worksheet_bioch.Cells[2, 8].Value = "N° Tél :";
                        worksheet_bioch.Cells[2, 9].Value = dt_bioch.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)worksheet_bioch.Cells[1, x]).Interior.Color = ((Excc.Range)worksheet_bioch.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)worksheet_bioch.Cells[1, x]).Font.Underline = ((Excc.Range)worksheet_bioch.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        worksheet_bioch.Cells[4, 1].Value = "Date";
                        ((Excc.Range)worksheet_bioch.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        worksheet_bioch.Cells[4, 2].Value = "Ref.";
                        dt_bioch.Columns.Cast<DataColumn>().Where(dd => dt_bioch.Columns.IndexOf(dd) >= 8 && dt_bioch.Columns.IndexOf(dd) < 36).ToList().ForEach(SS =>
                        {
                            worksheet_bioch.Cells[4, dt_bioch.Columns.IndexOf(SS) - 5].Value = SS.ColumnName;
                        });
                        worksheet_bioch.Cells[4, 31].Value = "Observ.";
                        worksheet_bioch.Cells[5, 1].Value = "Normatifs :";
                        for (int i = 1; i < 32; i++)
                        {
                            ((Excc.Range)worksheet_bioch.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)worksheet_bioch.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)worksheet_bioch.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)worksheet_bioch.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;

                            if (i < 29)
                            {
                                worksheet_bioch.Cells[5, i + 2].Value2 = "'" + dt_bioch.Rows[0][i + 35].ToString();
                                ((Excc.Range)worksheet_bioch.Cells[5, i + 2]).HorizontalAlignment = Excc.XlHAlign.xlHAlignRight;
                            }
                            ((Excc.Range)worksheet_bioch.Cells[5, i]).Interior.Color = ColorTranslator.ToOle(Color.Pink);
                        }

                        int y = 5;
                        dt_bioch.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            worksheet_bioch.Cells[y, 1].Value = PP["DATE_TIME"];
                            worksheet_bioch.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 29; t++)
                            {
                                worksheet_bioch.Cells[y, t + 2].Value = PP[t + 7];
                            }
                            worksheet_bioch.Cells[y, 31].Value = PP["OBSERV"];

                        });
                        ((Excc.Range)worksheet_bioch.Range[worksheet_bioch.Cells[4, 1], worksheet_bioch.Cells[dt_bioch.Rows.Count + 5, 30]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)worksheet_bioch.Range[worksheet_bioch.Cells[4, 1], worksheet_bioch.Cells[dt_bioch.Rows.Count + 5, 30]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)worksheet_bioch.Range[worksheet_bioch.Cells[4, 1], worksheet_bioch.Cells[dt_bioch.Rows.Count + 5, 30]]).Borders.Color = Color.Gray;
                        ((Excc.Range)worksheet_bioch.Range[worksheet_bioch.Cells[4, 1], worksheet_bioch.Cells[dt_bioch.Rows.Count + 5, 30]]).Columns.AutoFit();

                        //------------------
                    }

                    //Immunologie ============================================

                    DataTable dt_immun = PreConnection.Load_data("SELECT `REF`,`DATE_TIME`,(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',`OBSERV`,\r\n`MALAD_NME_001`,\r\n`MALAD_NME_002`,\r\n`MALAD_NME_003`,\r\n`MALAD_NME_004`,\r\n`MALAD_NME_005`,\r\n`MALAD_NME_006`,\r\n`MALAD_NME_007`,\r\n`MALAD_NME_008`,\r\n`MALAD_NME_009`,\r\n`MALAD_NME_010`,\r\n`MALAD_NME_011`,\r\n`MALAD_NME_012`,\r\n`MALAD_NME_013`,\r\n`MALAD_NME_014`,\r\n`MALAD_NME_015`,\r\n`METHODE_001`,\r\n`METHODE_002`,\r\n`METHODE_003`,\r\n`METHODE_004`,\r\n`METHODE_005`,\r\n`METHODE_006`,\r\n`METHODE_007`,\r\n`METHODE_008`,\r\n`METHODE_009`,\r\n`METHODE_010`,\r\n`METHODE_011`,\r\n`METHODE_012`,\r\n`METHODE_013`,\r\n`METHODE_014`,\r\n`METHODE_015`,\r\n`MALAD_RESULT_001`,\r\n`MALAD_RESULT_002`,\r\n`MALAD_RESULT_003`,\r\n`MALAD_RESULT_004`,\r\n`MALAD_RESULT_005`,\r\n`MALAD_RESULT_006`,\r\n`MALAD_RESULT_007`,\r\n`MALAD_RESULT_008`,\r\n`MALAD_RESULT_009`,\r\n`MALAD_RESULT_010`,\r\n`MALAD_RESULT_011`,\r\n`MALAD_RESULT_012`,\r\n`MALAD_RESULT_013`,\r\n`MALAD_RESULT_014`,\r\n`MALAD_RESULT_015`\r\nFROM `tb_labo_immunologie` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_immun.Rows.Count > 0)
                    {
                        Excc.Worksheet worksheet_immun = workbook.Worksheets.Add();
                        worksheet_immun.Activate();
                        //--------------------
                        worksheet_immun.Name = "Immunologie";
                        worksheet_immun.Rows[4].RowHeight = 30;
                        //-------------------
                        worksheet_immun.Cells[1, 1].Value = "Nom :";
                        worksheet_immun.Cells[1, 2].Value = dt_immun.Rows[0]["ANIM_NME"];

                        worksheet_immun.Cells[1, 4].Value = "N° d'ident. :";
                        worksheet_immun.Cells[1, 5].Value = dt_immun.Rows[0]["ANIM_IDENT_NUM"];

                        worksheet_immun.Cells[2, 1].Value = "Analyse de :";
                        worksheet_immun.Cells[2, 2].Value = "Immunologie";

                        worksheet_immun.Cells[1, 8].Value = "Propriétaire :";
                        worksheet_immun.Cells[1, 9].Value = dt_immun.Rows[0]["CLIENT_FULL_NME"];

                        worksheet_immun.Cells[1, 11].Value = "N° CNI :";
                        worksheet_immun.Cells[1, 12].Value = dt_immun.Rows[0]["CLIENT_NUM_CNI"];

                        worksheet_immun.Cells[2, 8].Value = "N° Tél :";
                        worksheet_immun.Cells[2, 9].Value = dt_immun.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)worksheet_immun.Cells[1, x]).Interior.Color = ((Excc.Range)worksheet_immun.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)worksheet_immun.Cells[1, x]).Font.Underline = ((Excc.Range)worksheet_immun.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        worksheet_immun.Cells[4, 1].Value = "Date";
                        ((Excc.Range)worksheet_immun.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        worksheet_immun.Cells[4, 2].Value = "Ref.";

                        worksheet_immun.Cells[4, 11].Value = "Observ.";
                        worksheet_immun.Cells[5, 1].Value = "Unité :";
                        for (int i = 1; i < 12; i++)//19
                        {
                            ((Excc.Range)worksheet_immun.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)worksheet_immun.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)worksheet_immun.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)worksheet_immun.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;



                            if (i < 9)
                            {
                                worksheet_immun.Cells[4, i + 2].Value = dt_immun.Rows[0][i + 7].ToString();
                                worksheet_immun.Cells[5, i + 2].Value2 = "'" + dt_immun.Rows[0][i + 22].ToString();
                                ((Excc.Range)worksheet_immun.Cells[5, i + 2]).HorizontalAlignment = Excc.XlHAlign.xlHAlignRight;
                            }
                            ((Excc.Range)worksheet_immun.Cells[5, i]).Interior.Color = ColorTranslator.ToOle(Color.Pink);
                        }

                        int y = 5;
                        dt_immun.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            worksheet_immun.Cells[y, 1].Value = PP["DATE_TIME"];
                            worksheet_immun.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 9; t++)
                            {
                                worksheet_immun.Cells[y, t + 2].Value = PP[t + 37];
                            }
                            worksheet_immun.Cells[y, 11].Value = PP["OBSERV"];

                        });
                        ((Excc.Range)worksheet_immun.Range[worksheet_immun.Cells[4, 1], worksheet_immun.Cells[dt_immun.Rows.Count + 5, 10]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)worksheet_immun.Range[worksheet_immun.Cells[4, 1], worksheet_immun.Cells[dt_immun.Rows.Count + 5, 10]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)worksheet_immun.Range[worksheet_immun.Cells[4, 1], worksheet_immun.Cells[dt_immun.Rows.Count + 5, 10]]).Borders.Color = Color.Gray;
                        ((Excc.Range)worksheet_immun.Range[worksheet_immun.Cells[4, 1], worksheet_immun.Cells[dt_immun.Rows.Count + 5, 10]]).Columns.AutoFit();


                    }

                    //Protéinogramme ================================================

                    DataTable dt_prot = PreConnection.Load_data("SELECT `REF`,`DATE_TIME`,(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',`OBSERV`,\r\n`Protéines Totales`,\r\n`Albumine`,\r\n`Alpha-1-Globulines`,\r\n`Alpha-2-Globulines`,\r\n`Beta-Globulines`,\r\n`Gamma-Globulines`,\r\n`Globulines Totales`,\r\n`Coefficient A/G`,\r\n`Protéines Totales_UNIT`,\r\n`Albumine_UNIT`,\r\n`Alpha-1-Globulines_UNIT`,\r\n`Alpha-2-Globulines_UNIT`,\r\n`Beta-Globulines_UNIT`,\r\n`Gamma-Globulines_UNIT`,\r\n`Globulines Totales_UNIT`,\r\n`Coefficient A/G_UNIT`,\r\n`Protéines Totales_NORMATIF`,\r\n`Albumine_NORMATIF`,\r\n`Alpha-1-Globulines_NORMATIF`,\r\n`Alpha-2-Globulines_NORMATIF`,\r\n`Beta-Globulines_NORMATIF`,\r\n`Gamma-Globulines_NORMATIF`,\r\n`Globulines Totales_NORMATIF`,\r\n`Coefficient A/G_NORMATIF`\r\nFROM `tb_labo_proteinogramme` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_prot.Rows.Count > 0)
                    {
                        Excc.Worksheet worksheet_prot = workbook.Worksheets.Add();
                        worksheet_prot.Activate();
                        //--------------------
                        worksheet_prot.Name = "Protéinogramme";
                        worksheet_prot.Rows[4].RowHeight = 30;
                        //-------------------                      
                        worksheet_prot.Cells[1, 1].Value = "Nom :";
                        worksheet_prot.Cells[1, 2].Value = dt_prot.Rows[0]["ANIM_NME"];

                        worksheet_prot.Cells[1, 4].Value = "N° d'ident. :";
                        worksheet_prot.Cells[1, 5].Value = dt_prot.Rows[0]["ANIM_IDENT_NUM"];

                        worksheet_prot.Cells[2, 1].Value = "Analyse de :";
                        worksheet_prot.Cells[2, 2].Value = "Protéinogramme";

                        worksheet_prot.Cells[1, 8].Value = "Propriétaire :";
                        worksheet_prot.Cells[1, 9].Value = dt_prot.Rows[0]["CLIENT_FULL_NME"];

                        worksheet_prot.Cells[1, 11].Value = "N° CNI :";
                        worksheet_prot.Cells[1, 12].Value = dt_prot.Rows[0]["CLIENT_NUM_CNI"];

                        worksheet_prot.Cells[2, 8].Value = "N° Tél :";
                        worksheet_prot.Cells[2, 9].Value = dt_prot.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)worksheet_prot.Cells[1, x]).Interior.Color = ((Excc.Range)worksheet_prot.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)worksheet_prot.Cells[1, x]).Font.Underline = ((Excc.Range)worksheet_prot.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        worksheet_prot.Cells[4, 1].Value = "Date";
                        ((Excc.Range)worksheet_prot.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        worksheet_prot.Cells[4, 2].Value = "Ref.";
                        dt_prot.Columns.Cast<DataColumn>().Where(dd => dt_prot.Columns.IndexOf(dd) >= 8 && dt_prot.Columns.IndexOf(dd) < 17).ToList().ForEach(SS =>
                        {
                            worksheet_prot.Cells[4, dt_prot.Columns.IndexOf(SS) - 5].Value = SS.ColumnName;
                        });
                        worksheet_prot.Cells[4, 11].Value = "Observ.";
                        worksheet_prot.Cells[5, 1].Value = "Unité :";
                        for (int i = 1; i < 12; i++)
                        {
                            ((Excc.Range)worksheet_prot.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)worksheet_prot.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)worksheet_prot.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)worksheet_prot.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;
                            if (i < 9)
                            {
                                worksheet_prot.Cells[5, i + 2].Value2 = "'" + "(" + dt_prot.Rows[0][i + 15].ToString() + ")";
                                ((Excc.Range)worksheet_prot.Cells[5, i + 2]).HorizontalAlignment = Excc.XlHAlign.xlHAlignRight;
                            }
                            ((Excc.Range)worksheet_prot.Cells[5, i]).Interior.Color = ColorTranslator.ToOle(Color.Pink);
                        }

                        int y = 5;
                        dt_prot.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            worksheet_prot.Cells[y, 1].Value = PP["DATE_TIME"];
                            worksheet_prot.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 9; t++)
                            {
                                worksheet_prot.Cells[y, t + 2].Value = PP[t + 7];
                            }
                            worksheet_prot.Cells[y, 11].Value = PP["OBSERV"];
                        });
                        ((Excc.Range)worksheet_prot.Range[worksheet_prot.Cells[4, 1], worksheet_prot.Cells[dt_prot.Rows.Count + 5, 10]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)worksheet_prot.Range[worksheet_prot.Cells[4, 1], worksheet_prot.Cells[dt_prot.Rows.Count + 5, 10]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)worksheet_prot.Range[worksheet_prot.Cells[4, 1], worksheet_prot.Cells[dt_prot.Rows.Count + 5, 10]]).Borders.Color = Color.Gray;
                        ((Excc.Range)worksheet_prot.Range[worksheet_prot.Cells[4, 1], worksheet_prot.Cells[dt_prot.Rows.Count + 5, 10]]).Columns.AutoFit();



                        //------------------

                    }

                    //Autres ====================================================

                    DataTable dt_autre = PreConnection.Load_data("SELECT `REF`,`DATE_TIME`,(SELECT `NME` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_NME',(SELECT `NUM_IDENTIF` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`) AS 'ANIM_IDENT_NUM',(SELECT CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_FULL_NME',(SELECT `NUM_CNI` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_CNI',(SELECT `NUM_PHONE` FROM tb_clients tb3 WHERE tb3.`ID` = (SELECT `CLIENT_ID` FROM tb_animaux tb2 WHERE tb2.`ID` = tb1.`ANIM_ID`)) AS 'CLIENT_NUM_PHONE',`OBSERV`,\r\n`TYPE_ANAL`,\r\n`METHODE`,\r\n`RESULT`\r\nFROM `tb_labo_autre` tb1 WHERE `ANIM_ID` = " + selected_anim.Cells["ID"].Value + " ORDER BY `DATE_TIME`;");
                    if (dt_autre.Rows.Count > 0)
                    {
                        Excc.Worksheet worksheet_autre = workbook.Worksheets.Add();
                        worksheet_autre.Activate();
                        //--------------------
                        worksheet_autre.Name = "Autres";
                        worksheet_autre.Rows[4].RowHeight = 30;
                        //-------------------   
                        worksheet_autre.Cells[1, 1].Value = "Nom :";
                        worksheet_autre.Cells[1, 2].Value = dt_autre.Rows[0]["ANIM_NME"];

                        worksheet_autre.Cells[1, 4].Value = "N° d'ident. :";
                        worksheet_autre.Cells[1, 5].Value = dt_autre.Rows[0]["ANIM_IDENT_NUM"];

                        worksheet_autre.Cells[2, 1].Value = "Analyse de :";
                        worksheet_autre.Cells[2, 2].Value = "Autres Analsyes";

                        worksheet_autre.Cells[1, 8].Value = "Propriétaire :";
                        worksheet_autre.Cells[1, 9].Value = dt_autre.Rows[0]["CLIENT_FULL_NME"];

                        worksheet_autre.Cells[1, 11].Value = "N° CNI :";
                        worksheet_autre.Cells[1, 12].Value = dt_autre.Rows[0]["CLIENT_NUM_CNI"];

                        worksheet_autre.Cells[2, 8].Value = "N° Tél :";
                        worksheet_autre.Cells[2, 9].Value = dt_autre.Rows[0]["CLIENT_NUM_PHONE"];

                        int[] ttt = { 1, 4, 8, 11 };
                        ttt.ForEach(x =>
                        {
                            ((Excc.Range)worksheet_autre.Cells[1, x]).Interior.Color = ((Excc.Range)worksheet_autre.Cells[2, x]).Interior.Color = ColorTranslator.ToOle(Color.PaleTurquoise);
                            ((Excc.Range)worksheet_autre.Cells[1, x]).Font.Underline = ((Excc.Range)worksheet_autre.Cells[2, x]).Font.Underline = true;
                        });
                        //--------------------
                        worksheet_autre.Cells[4, 1].Value = "Date";
                        ((Excc.Range)worksheet_autre.Columns[1]).NumberFormat = "dd/MM/yyyy";
                        worksheet_autre.Cells[4, 2].Value = "Ref.";
                        worksheet_autre.Cells[4, 3].Value = "Type d'analyse";
                        worksheet_autre.Cells[4, 4].Value = "Méthode";
                        worksheet_autre.Cells[4, 5].Value = "Résultat";
                        worksheet_autre.Cells[4, 6].Value = "Observ.";
                        for (int i = 1; i < 7; i++)
                        {
                            ((Excc.Range)worksheet_autre.Cells[4, i]).Interior.Color = ColorTranslator.ToOle(Color.BurlyWood);
                            ((Excc.Range)worksheet_autre.Cells[4, i]).Font.Bold = true;
                            ((Excc.Range)worksheet_autre.Cells[4, i]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                            ((Excc.Range)worksheet_autre.Cells[4, i]).VerticalAlignment = Excc.XlVAlign.xlVAlignCenter;
                        }
                        int y = 4;
                        dt_autre.Rows.Cast<DataRow>().ForEach(PP =>
                        {
                            y++;
                            worksheet_autre.Cells[y, 1].Value = PP["DATE_TIME"];
                            worksheet_autre.Cells[y, 2].Value = PP["REF"];
                            for (int t = 1; t < 4; t++)
                            {
                                worksheet_autre.Cells[y, t + 2].Value = PP[t + 7];
                            }
                            worksheet_autre.Cells[y, 6].Value = PP["OBSERV"];
                        });
                        ((Excc.Range)worksheet_autre.Range[worksheet_autre.Cells[4, 1], worksheet_autre.Cells[dt_autre.Rows.Count + 4, 5]]).Borders.LineStyle = Excc.XlLineStyle.xlContinuous;
                        ((Excc.Range)worksheet_autre.Range[worksheet_autre.Cells[4, 1], worksheet_autre.Cells[dt_autre.Rows.Count + 4, 5]]).Borders.Weight = Excc.XlBorderWeight.xlThin;
                        ((Excc.Range)worksheet_autre.Range[worksheet_autre.Cells[4, 1], worksheet_autre.Cells[dt_autre.Rows.Count + 4, 5]]).Borders.Color = Color.Gray;
                        ((Excc.Range)worksheet_autre.Range[worksheet_autre.Cells[4, 1], worksheet_autre.Cells[dt_autre.Rows.Count + 4, 5]]).Columns.AutoFit();





                    }
                    //===============================
                    SaveFileDialog svd = new SaveFileDialog();
                    svd.Filter = "Excel | *.xlsx";
                    svd.DefaultExt = "*.xlsx";
                    svd.FileName = workbook.Title + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                    if (svd.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(Path.GetFullPath(svd.FileName));
                        Process.Start(Path.GetFullPath(svd.FileName));
                    }
                    workbook.Close(false);
                    xcelApp.Quit();
                    //-------------------
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (selected_anim != null)
            {
                this.ControlBox = false;
                Urologie urol = new Urologie(selected_anim, null);
                urol.Dock = DockStyle.Fill;
                this.Controls.Add(urol);
                urol.BringToFront();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

