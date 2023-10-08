using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Resources;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Xamarin.Forms.Internals;

namespace ALBAITAR_Softvet.Labo
{
    public partial class Immunologie : UserControl
    {
        DataGridViewRow selected_animm = null;
        DataTable lab_histor;
        DataTable new_initial_tbl;
        bool is_new = true;
        string ref_tmp = string.Empty;
        string IDD_to_select = "";
        double Anim_poids = 0;
        public Immunologie(DataGridViewRow selected_anim, string ID_to_select)
        {
            InitializeComponent();
            selected_animm = selected_anim;
            IDD_to_select = ID_to_select;
            //------------------------------------
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button5.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30004" && (Int32)QQ[3] == 1).Count() > 0; //Imprimer
            }
            //------------------------------------
            new_initial_tbl = new DataTable();
            new_initial_tbl.Columns.Add("MALAD_NME", typeof(string));
            new_initial_tbl.Columns.Add("METHODE", typeof(string));
            new_initial_tbl.Columns.Add("VALUE", typeof(string));
            string[] paramss = new string[15];
            for (int i = 0; i < 15; i++)
            {
                DataRow rw = new_initial_tbl.NewRow();
                new_initial_tbl.Rows.Add(rw);
            }
            dataGridView1.DataSource = new_initial_tbl;
            //------------------------------
            
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            //this.ParentForm.ControlBox = true;
            Dispose();
        }

        private void Hemogramme_Load(object sender, EventArgs e)
        {
            //----------- Autorisations --------------------
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button3.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31001" && (Int32)QQ[3] == 1).Count() > 0; //Ajouter
                button4.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31002" && (Int32)QQ[3] == 1).Count() > 0; //Supprimer
                dataGridView1.ReadOnly = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31003" && (Int32)QQ[3] == 1).Count() == 0 && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31001" && (Int32)QQ[3] == 1).Count() == 0; //Modifier (1)
                textBox3.Enabled = dateTimePicker1.Enabled = textBox1.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31003" && (Int32)QQ[3] == 1).Count() > 0 || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31001" && (Int32)QQ[3] == 1).Count() > 0; //Modifier (2)
            }
            //--------En-tete
            label3.Text = (string)selected_animm.Cells["NME"].Value;
            label4.Text = (string)selected_animm.Cells["CLIENT_FULL_NME"].Value;
            label6.Text = (string)selected_animm.Cells["ESPECE"].Value;
            label8.Text = (string)selected_animm.Cells["RACE"].Value;
            label13.Text = (string)selected_animm.Cells["SEXE"].Value;
            label14.Text = selected_animm.Cells["NISS_DATE"].Value != DBNull.Value ? ((DateTime)selected_animm.Cells["NISS_DATE"].Value).ToString("d") : "--";
            textBox2.Text = (string)selected_animm.Cells["OBSERVATIONS"].Value;
            var CCC = Main_Frm.main_poids_tab.AsEnumerable().Where(F => F.Field<int>("ANIM_ID") == (int)selected_animm.Cells["ID"].Value);
            Anim_poids = CCC.Any() ? CCC.Last().Field<double>("POIDS") : 0;
            label22.Text = Anim_poids.ToString("N2") + " Kg";
            //-------------------------            
            Load_histor();
            //-----------------
            if (IDD_to_select != null)
            {
                if (IDD_to_select.Trim().Length > 0)
                {
                    dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
                    dataGridView2.ClearSelection();
                    dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
                    dataGridView2.Rows.Cast<DataGridViewRow>()
                                 .Where(row => row.Cells["ID"].Value.ToString() == IDD_to_select)
                                 .ToList()
                                 .ForEach(row => row.Selected = true);
                }
                else
                {
                    button3.PerformClick();
                }
            }
            else
            {
                button3.PerformClick();
            }
            //------------------

        }

        private void Load_histor()
        {
            if (Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Immunologie").Count() > 0)
            {
                lab_histor = Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Immunologie").CopyToDataTable();
                dataGridView2.DataSource = lab_histor;
            }
            else
            {
                if (dataGridView2.DataSource != null)
                {
                    DataTable dt = ((DataTable)dataGridView2.DataSource).Clone();
                    dataGridView2.DataSource = dt;
                }

            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            is_new = true;
            pictureBox1.Image = Properties.Resources.NOUVEAU_003;
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox3.Text = ref_tmp = "IMUN_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
            for (int i = 0; i < 15; i++)
            {
                dataGridView1.Rows[i].Cells["VALUE2"].Value = "";
            }
            //----------------
            dataGridView1.Rows[0].Cells["MALAD_NME"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "Adenovirus Canin" : "Calcivirus";
            dataGridView1.Rows[1].Cells["MALAD_NME"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "Circonvirus Canin" : "Coronavirus (PIF)";
            dataGridView1.Rows[2].Cells["MALAD_NME"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "Leishmaniose Canine" : "FIV";
            dataGridView1.Rows[3].Cells["MALAD_NME"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "Maladie de Carre" : "FeLV";
            dataGridView1.Rows[4].Cells["MALAD_NME"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "Parvovirose Canine" : "Herpes Virus";
            dataGridView1.Rows[5].Cells["MALAD_NME"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "" : "Leucemie Féline";
            dataGridView1.Rows[6].Cells["MALAD_NME"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "" : "Panleucopenie";
            dataGridView1.Rows[7].Cells["MALAD_NME"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "" : "Toxoplasmose";
            dataGridView1.Rows[8].Cells["MALAD_NME"].Value = "";
            dataGridView1.Rows[9].Cells["MALAD_NME"].Value = "";
            dataGridView1.Rows[10].Cells["MALAD_NME"].Value = "";
            dataGridView1.Rows[11].Cells["MALAD_NME"].Value = "";
            dataGridView1.Rows[12].Cells["MALAD_NME"].Value = "";
            dataGridView1.Rows[13].Cells["MALAD_NME"].Value = "";
            dataGridView1.Rows[14].Cells["MALAD_NME"].Value = "";
            //----------------
            dataGridView1.Rows[0].Cells["METHODE"].Value = "Acs";
            dataGridView1.Rows[1].Cells["METHODE"].Value = "Acs Elisa / Snap";
            dataGridView1.Rows[2].Cells["METHODE"].Value = "Acs Elisa / Snap";
            dataGridView1.Rows[3].Cells["METHODE"].Value = "Acs";
            dataGridView1.Rows[4].Cells["METHODE"].Value = "Acs";
            dataGridView1.Rows[5].Cells["METHODE"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "" : "Acs Elisa / Snap";
            dataGridView1.Rows[6].Cells["METHODE"].Value = selected_animm.Cells["ESPECE"].Value.ToString() == "Canine" ? "" : "IgG + IgM";
            dataGridView1.Rows[7].Cells["METHODE"].Value = "";
            dataGridView1.Rows[8].Cells["METHODE"].Value = "";
            dataGridView1.Rows[9].Cells["METHODE"].Value = "";
            dataGridView1.Rows[10].Cells["METHODE"].Value = "";
            dataGridView1.Rows[11].Cells["METHODE"].Value = "";
            dataGridView1.Rows[12].Cells["METHODE"].Value = "";
            dataGridView1.Rows[13].Cells["METHODE"].Value = "";
            dataGridView1.Rows[14].Cells["METHODE"].Value = "";
            //----------------
            dataGridView1.Rows[0].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[1].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[2].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[3].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[4].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[5].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[6].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[7].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[8].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[9].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[10].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[11].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[12].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[13].Cells["VALUE2"].Value = "";
            dataGridView1.Rows[14].Cells["VALUE2"].Value = "";
            //-------------------
            button5.Visible = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false; // don't throw an exception
        }


        private void button2_Click(object sender, EventArgs e)
        {
            bool autorisat = Properties.Settings.Default.Last_login_is_admin || (is_new && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31001" && (Int32)QQ[3] == 1).Count() > 0) || (!is_new && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31003" && (Int32)QQ[3] == 1).Count() > 0);
            if (autorisat)
            {
                int current_row_to_select = is_new ? -1 : dataGridView2.SelectedRows[0].Index;
                bool ready = true;
                ready &= label20.Text.Trim().Length == 0;
                //-------------
                bool tttmmmp = true;
                int tt = 0;
                int null_nb = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    tt++;
                    null_nb += dataGridView1.Rows[i].Cells["VALUE2"].Value == DBNull.Value ? 1 : 0;
                }
                if (tt == null_nb)
                {
                    ready = tttmmmp = false;
                    MessageBox.Show("Il n'y a pas des résultats !", "Vide :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //------------------
                if (tttmmmp && !ready)
                {
                    ready = MessageBox.Show("Il y a des erreurs dans votre bilan,\n\nVoulez-vous continuer?\n", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                }
                if (ready)
                {
                    if (is_new)
                    {
                        PreConnection.Excut_Cmd(1, "tb_labo_immunologie",new List<string> {
                        "REF",
"DATE_TIME",
"ANIM_ID",
"OBSERV",
"MALAD_NME_001",
"MALAD_NME_002",
"MALAD_NME_003",
"MALAD_NME_004",
"MALAD_NME_005",
"MALAD_NME_006",
"MALAD_NME_007",
"MALAD_NME_008",
"MALAD_NME_009",
"MALAD_NME_010",
"MALAD_NME_011",
"MALAD_NME_012",
"MALAD_NME_013",
"MALAD_NME_014",
"MALAD_NME_015",
"METHODE_001",
"METHODE_002",
"METHODE_003",
"METHODE_004",
"METHODE_005",
"METHODE_006",
"METHODE_007",
"METHODE_008",
"METHODE_009",
"METHODE_010",
"METHODE_011",
"METHODE_012",
"METHODE_013",
"METHODE_014",
"METHODE_015",
"MALAD_RESULT_001",
"MALAD_RESULT_002",
"MALAD_RESULT_003",
"MALAD_RESULT_004",
"MALAD_RESULT_005",
"MALAD_RESULT_006",
"MALAD_RESULT_007",
"MALAD_RESULT_008",
"MALAD_RESULT_009",
"MALAD_RESULT_010",
"MALAD_RESULT_011",
"MALAD_RESULT_012",
"MALAD_RESULT_013",
"MALAD_RESULT_014",
"MALAD_RESULT_015"},new List<object>
{
    textBox3.Text, //REF
                                              dateTimePicker1.Value, //DATE_TIME
                                             selected_animm.Cells["ID"].Value, //ANIM_ID
                                              textBox1.Text, //OBSERV
                                              dataGridView1.Rows[0].Cells["MALAD_NME"].Value,
dataGridView1.Rows[1].Cells["MALAD_NME"].Value,
dataGridView1.Rows[2].Cells["MALAD_NME"].Value,
dataGridView1.Rows[3].Cells["MALAD_NME"].Value,
dataGridView1.Rows[4].Cells["MALAD_NME"].Value,
dataGridView1.Rows[5].Cells["MALAD_NME"].Value,
dataGridView1.Rows[6].Cells["MALAD_NME"].Value,
dataGridView1.Rows[7].Cells["MALAD_NME"].Value,
dataGridView1.Rows[8].Cells["MALAD_NME"].Value,
dataGridView1.Rows[9].Cells["MALAD_NME"].Value,
dataGridView1.Rows[10].Cells["MALAD_NME"].Value,
dataGridView1.Rows[11].Cells["MALAD_NME"].Value,
dataGridView1.Rows[12].Cells["MALAD_NME"].Value,
dataGridView1.Rows[13].Cells["MALAD_NME"].Value,
dataGridView1.Rows[14].Cells["MALAD_NME"].Value,
dataGridView1.Rows[0].Cells["METHODE"].Value,
dataGridView1.Rows[1].Cells["METHODE"].Value,
dataGridView1.Rows[2].Cells["METHODE"].Value,
dataGridView1.Rows[3].Cells["METHODE"].Value,
dataGridView1.Rows[4].Cells["METHODE"].Value,
dataGridView1.Rows[5].Cells["METHODE"].Value,
dataGridView1.Rows[6].Cells["METHODE"].Value,
dataGridView1.Rows[7].Cells["METHODE"].Value,
dataGridView1.Rows[8].Cells["METHODE"].Value,
dataGridView1.Rows[9].Cells["METHODE"].Value,
dataGridView1.Rows[10].Cells["METHODE"].Value,
dataGridView1.Rows[11].Cells["METHODE"].Value,
dataGridView1.Rows[12].Cells["METHODE"].Value,
dataGridView1.Rows[13].Cells["METHODE"].Value,
dataGridView1.Rows[14].Cells["METHODE"].Value,
dataGridView1.Rows[0].Cells["VALUE2"].Value,
dataGridView1.Rows[1].Cells["VALUE2"].Value,
dataGridView1.Rows[2].Cells["VALUE2"].Value,
dataGridView1.Rows[3].Cells["VALUE2"].Value,
dataGridView1.Rows[4].Cells["VALUE2"].Value,
dataGridView1.Rows[5].Cells["VALUE2"].Value,
dataGridView1.Rows[6].Cells["VALUE2"].Value,
dataGridView1.Rows[7].Cells["VALUE2"].Value,
dataGridView1.Rows[8].Cells["VALUE2"].Value,
dataGridView1.Rows[9].Cells["VALUE2"].Value,
dataGridView1.Rows[10].Cells["VALUE2"].Value,
dataGridView1.Rows[11].Cells["VALUE2"].Value,
dataGridView1.Rows[12].Cells["VALUE2"].Value,
dataGridView1.Rows[13].Cells["VALUE2"].Value,
dataGridView1.Rows[14].Cells["VALUE2"].Value
},null,null,null);
                        //PreConnection.Excut_Cmd("INSERT INTO `tb_labo_immunologie`"
                        //                      + "(`REF`,"
                        //                      + "`DATE_TIME`,"
                        //                      + "`ANIM_ID`,"
                        //                      + "`OBSERV`,"
                        //                      + "`MALAD_NME_001`,"
                        //                      + "`MALAD_NME_002`,"
                        //                      + "`MALAD_NME_003`,"
                        //                      + "`MALAD_NME_004`,"
                        //                      + "`MALAD_NME_005`,"
                        //                      + "`MALAD_NME_006`,"
                        //                      + "`MALAD_NME_007`,"
                        //                      + "`MALAD_NME_008`,"
                        //                      + "`MALAD_NME_009`,"
                        //                      + "`MALAD_NME_010`,"
                        //                      + "`MALAD_NME_011`,"
                        //                      + "`MALAD_NME_012`,"
                        //                      + "`MALAD_NME_013`,"
                        //                      + "`MALAD_NME_014`,"
                        //                      + "`MALAD_NME_015`,"
                        //                      + "`METHODE_001`,"
                        //                      + "`METHODE_002`,"
                        //                      + "`METHODE_003`,"
                        //                      + "`METHODE_004`,"
                        //                      + "`METHODE_005`,"
                        //                      + "`METHODE_006`,"
                        //                      + "`METHODE_007`,"
                        //                      + "`METHODE_008`,"
                        //                      + "`METHODE_009`,"
                        //                      + "`METHODE_010`,"
                        //                      + "`METHODE_011`,"
                        //                      + "`METHODE_012`,"
                        //                      + "`METHODE_013`,"
                        //                      + "`METHODE_014`,"
                        //                      + "`METHODE_015`,"
                        //                      + "`MALAD_RESULT_001`,"
                        //                      + "`MALAD_RESULT_002`,"
                        //                      + "`MALAD_RESULT_003`,"
                        //                      + "`MALAD_RESULT_004`,"
                        //                      + "`MALAD_RESULT_005`,"
                        //                      + "`MALAD_RESULT_006`,"
                        //                      + "`MALAD_RESULT_007`,"
                        //                      + "`MALAD_RESULT_008`,"
                        //                      + "`MALAD_RESULT_009`,"
                        //                      + "`MALAD_RESULT_010`,"
                        //                      + "`MALAD_RESULT_011`,"
                        //                      + "`MALAD_RESULT_012`,"
                        //                      + "`MALAD_RESULT_013`,"
                        //                      + "`MALAD_RESULT_014`,"
                        //                      + "`MALAD_RESULT_015`)"
                        //                      + "VALUES"
                        //                      + "('" + textBox3.Text.Replace("'", "''") + "'," //REF
                        //                      + "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," //DATE_TIME
                        //                      + selected_animm.Cells["ID"].Value + "," //ANIM_ID
                        //                      + "'" + textBox1.Text.Replace("'", "''") + "'," //OBSERV
                        //                      + "'" + (dataGridView1.Rows[0].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[1].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[2].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[3].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[4].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[5].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[6].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[7].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[8].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[9].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[10].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[11].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[12].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[13].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[14].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[0].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[1].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[2].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[3].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[4].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[5].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[6].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[7].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[8].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[9].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[10].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[11].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[12].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[13].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[14].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[0].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[1].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[2].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[3].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[4].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[5].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[6].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[7].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[8].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[9].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[10].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[11].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[12].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[13].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "'" + (dataGridView1.Rows[14].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "');");
                    }
                    else
                    {
                        //PreConnection.Excut_Cmd("UPDATE `tb_labo_immunologie`"
                        //                      + "SET "
                        //                      + "`REF` = '" + textBox3.Text.Replace("'", "''") + "',"
                        //                      + "`DATE_TIME` = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                        //                      + "`ANIM_ID` = "+ selected_animm.Cells["ID"].Value + ","
                        //                      + "`OBSERV` = '" + textBox1.Text.Replace("'", "''") + "',"
                        //                      + "`MALAD_NME_001` = '" + (dataGridView1.Rows[0].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_002` = '" + (dataGridView1.Rows[1].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_003` = '" + (dataGridView1.Rows[2].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_004` = '" + (dataGridView1.Rows[3].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_005` = '" + (dataGridView1.Rows[4].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_006` = '" + (dataGridView1.Rows[5].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_007` = '" + (dataGridView1.Rows[6].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_008` = '" + (dataGridView1.Rows[7].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_009` = '" + (dataGridView1.Rows[8].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_010` = '" + (dataGridView1.Rows[9].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_011` = '" + (dataGridView1.Rows[10].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_012` = '" + (dataGridView1.Rows[11].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_013` = '" + (dataGridView1.Rows[12].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_014` = '" + (dataGridView1.Rows[13].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_NME_015` = '" + (dataGridView1.Rows[14].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["MALAD_NME"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_001` = '" + (dataGridView1.Rows[0].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_002` = '" + (dataGridView1.Rows[1].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_003` = '" + (dataGridView1.Rows[2].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_004` = '" + (dataGridView1.Rows[3].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_005` = '" + (dataGridView1.Rows[4].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_006` = '" + (dataGridView1.Rows[5].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_007` = '" + (dataGridView1.Rows[6].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_008` = '" + (dataGridView1.Rows[7].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_009` = '" + (dataGridView1.Rows[8].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_010` = '" + (dataGridView1.Rows[9].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_011` = '" + (dataGridView1.Rows[10].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_012` = '" + (dataGridView1.Rows[11].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_013` = '" + (dataGridView1.Rows[12].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_014` = '" + (dataGridView1.Rows[13].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`METHODE_015` = '" + (dataGridView1.Rows[14].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["METHODE"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_001` = '" + (dataGridView1.Rows[0].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_002` = '" + (dataGridView1.Rows[1].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_003` = '" + (dataGridView1.Rows[2].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_004` = '" + (dataGridView1.Rows[3].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_005` = '" + (dataGridView1.Rows[4].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_006` = '" + (dataGridView1.Rows[5].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_007` = '" + (dataGridView1.Rows[6].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_008` = '" + (dataGridView1.Rows[7].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_009` = '" + (dataGridView1.Rows[8].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_010` = '" + (dataGridView1.Rows[9].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_011` = '" + (dataGridView1.Rows[10].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_012` = '" + (dataGridView1.Rows[11].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_013` = '" + (dataGridView1.Rows[12].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_014` = '" + (dataGridView1.Rows[13].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "',"
                        //                      + "`MALAD_RESULT_015` = '" + (dataGridView1.Rows[14].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["VALUE2"].Value.ToString().Replace("'","''") : "") + "'"
                        //                      + " WHERE `ID` = " + dataGridView2.SelectedRows[0].Cells["ID"].Value + ";");
                        PreConnection.Excut_Cmd(2, "tb_labo_immunologie", new List<string> {
                        "REF",
"DATE_TIME",
"OBSERV",
"MALAD_NME_001",
"MALAD_NME_002",
"MALAD_NME_003",
"MALAD_NME_004",
"MALAD_NME_005",
"MALAD_NME_006",
"MALAD_NME_007",
"MALAD_NME_008",
"MALAD_NME_009",
"MALAD_NME_010",
"MALAD_NME_011",
"MALAD_NME_012",
"MALAD_NME_013",
"MALAD_NME_014",
"MALAD_NME_015",
"METHODE_001",
"METHODE_002",
"METHODE_003",
"METHODE_004",
"METHODE_005",
"METHODE_006",
"METHODE_007",
"METHODE_008",
"METHODE_009",
"METHODE_010",
"METHODE_011",
"METHODE_012",
"METHODE_013",
"METHODE_014",
"METHODE_015",
"MALAD_RESULT_001",
"MALAD_RESULT_002",
"MALAD_RESULT_003",
"MALAD_RESULT_004",
"MALAD_RESULT_005",
"MALAD_RESULT_006",
"MALAD_RESULT_007",
"MALAD_RESULT_008",
"MALAD_RESULT_009",
"MALAD_RESULT_010",
"MALAD_RESULT_011",
"MALAD_RESULT_012",
"MALAD_RESULT_013",
"MALAD_RESULT_014",
"MALAD_RESULT_015"}, new List<object>
{
    textBox3.Text, //REF
                                              dateTimePicker1.Value, //DATE_TIME
                                              textBox1.Text, //OBSERV
                                              dataGridView1.Rows[0].Cells["MALAD_NME"].Value,
dataGridView1.Rows[1].Cells["MALAD_NME"].Value,
dataGridView1.Rows[2].Cells["MALAD_NME"].Value,
dataGridView1.Rows[3].Cells["MALAD_NME"].Value,
dataGridView1.Rows[4].Cells["MALAD_NME"].Value,
dataGridView1.Rows[5].Cells["MALAD_NME"].Value,
dataGridView1.Rows[6].Cells["MALAD_NME"].Value,
dataGridView1.Rows[7].Cells["MALAD_NME"].Value,
dataGridView1.Rows[8].Cells["MALAD_NME"].Value,
dataGridView1.Rows[9].Cells["MALAD_NME"].Value,
dataGridView1.Rows[10].Cells["MALAD_NME"].Value,
dataGridView1.Rows[11].Cells["MALAD_NME"].Value,
dataGridView1.Rows[12].Cells["MALAD_NME"].Value,
dataGridView1.Rows[13].Cells["MALAD_NME"].Value,
dataGridView1.Rows[14].Cells["MALAD_NME"].Value,
dataGridView1.Rows[0].Cells["METHODE"].Value,
dataGridView1.Rows[1].Cells["METHODE"].Value,
dataGridView1.Rows[2].Cells["METHODE"].Value,
dataGridView1.Rows[3].Cells["METHODE"].Value,
dataGridView1.Rows[4].Cells["METHODE"].Value,
dataGridView1.Rows[5].Cells["METHODE"].Value,
dataGridView1.Rows[6].Cells["METHODE"].Value,
dataGridView1.Rows[7].Cells["METHODE"].Value,
dataGridView1.Rows[8].Cells["METHODE"].Value,
dataGridView1.Rows[9].Cells["METHODE"].Value,
dataGridView1.Rows[10].Cells["METHODE"].Value,
dataGridView1.Rows[11].Cells["METHODE"].Value,
dataGridView1.Rows[12].Cells["METHODE"].Value,
dataGridView1.Rows[13].Cells["METHODE"].Value,
dataGridView1.Rows[14].Cells["METHODE"].Value,
dataGridView1.Rows[0].Cells["VALUE2"].Value,
dataGridView1.Rows[1].Cells["VALUE2"].Value,
dataGridView1.Rows[2].Cells["VALUE2"].Value,
dataGridView1.Rows[3].Cells["VALUE2"].Value,
dataGridView1.Rows[4].Cells["VALUE2"].Value,
dataGridView1.Rows[5].Cells["VALUE2"].Value,
dataGridView1.Rows[6].Cells["VALUE2"].Value,
dataGridView1.Rows[7].Cells["VALUE2"].Value,
dataGridView1.Rows[8].Cells["VALUE2"].Value,
dataGridView1.Rows[9].Cells["VALUE2"].Value,
dataGridView1.Rows[10].Cells["VALUE2"].Value,
dataGridView1.Rows[11].Cells["VALUE2"].Value,
dataGridView1.Rows[12].Cells["VALUE2"].Value,
dataGridView1.Rows[13].Cells["VALUE2"].Value,
dataGridView1.Rows[14].Cells["VALUE2"].Value
}, "ID = @P_ID", new List<string> { "P_ID"}, new List<object> { dataGridView2.SelectedRows[0].Cells["ID"].Value });
                    }
                    //--------
                    Laboratoire.labo = PreConnection.Load_data(Laboratoire.labo_load_cmd);
                    Laboratoire.make_historic_refesh = true;
                    //------------
                    Load_histor();
                    dataGridView2.ClearSelection();
                    if (dataGridView2.Rows.Count > 0)
                    {
                        dataGridView2.Rows[current_row_to_select == -1 ? dataGridView2.Rows.Count - 1 : current_row_to_select].Selected = true;
                    }

                }
            }
            else
            {
                new Non_Autorized_Msg("").ShowDialog();
            }
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label20.Text = "";
            if (lab_histor != null)
            {
                int nmb = 0;
                if (is_new)
                {
                    nmb = lab_histor.AsEnumerable().Where(P => ((string)P["REF"]).Trim().Length > 0 && (string)P["REF"] == textBox3.Text.Trim()).Count();
                }
                else if (dataGridView2.SelectedRows.Count > 0)
                {
                    nmb = lab_histor.AsEnumerable().Where(P => (int)dataGridView2.SelectedRows[0].Cells["ID"].Value != (int)P["ID"] && ((string)P["REF"]).Trim().Length > 0 && (string)P["REF"] == textBox3.Text.Trim()).Count();
                }
                label20.Text = nmb > 0 ? "-Déja existe !\n" : "";
            }
            label20.Text += textBox3.Text.Trim().Length < 10 ? "-Nb Chiffres doit > 9" : "";
            label20.TextAlign = ContentAlignment.BottomLeft;
            button5.Visible = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == ref_tmp)
            {
                textBox3.Text = ref_tmp = "IMUN_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
            }
            button5.Visible = false;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && !dataGridView1.CurrentCell.ReadOnly)
            {
                dataGridView1.CurrentCell.Value = DBNull.Value;
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_labo_immunologie WHERE ID = " + dataGridView2.SelectedRows[0].Cells["ID"].Value + ";");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        is_new = false;
                        pictureBox1.Image = Properties.Resources.MODIF_003;                        
                        ref_tmp = string.Empty;
                        dateTimePicker1.Value = (DateTime)dt.Rows[0]["DATE_TIME"];
                        textBox3.Text = (string)dt.Rows[0]["REF"];
                        textBox1.Text = (string)dt.Rows[0]["OBSERV"];
                        for (int f = 5; f < 20; f++)
                        {
                            dataGridView1.Rows[f - 5].Cells["MALAD_NME"].Value = dt.Rows[0][f] != null ? dt.Rows[0][f] : "";
                        }
                        for (int f = 20; f < 35; f++)
                        {
                            dataGridView1.Rows[f - 20].Cells["METHODE"].Value = dt.Rows[0][f] != null ? dt.Rows[0][f] : "";
                        }
                        string[] sss = {"", "Positif", "Négatif" };
                        for (int f = 35; f < 50; f++)
                        {
                            if(dt.Rows[0][f] != null)
                            {
                                if (sss.Contains(dt.Rows[0][f].ToString()))
                                {
                                    dataGridView1.Rows[f - 35].Cells["VALUE2"].Value = dt.Rows[0][f];
                                }
                                else
                                {
                                    dataGridView1.Rows[f - 35].Cells["VALUE2"].Value = "";
                                }
                            }
                            else
                            {
                                dataGridView1.Rows[f - 35].Cells["VALUE2"].Value = "";
                            }                            
                        }
                        button5.Visible = true;
                    }
                    else
                    {
                        button3.PerformClick();
                    }
                }
                else
                {
                    button3.PerformClick();
                }
            }
            else
            {
                button3.PerformClick();
            }

        }

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Selected)
            {
                dataGridView2_SelectionChanged(null, null);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Sures de supprimer (" + dataGridView2.SelectedRows.Count + ") bilan ?", "Confirmation : ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string dq = "";
                    dataGridView2.SelectedRows.Cast<DataGridViewRow>().ForEach(row => { dq += "," + row.Cells["ID"].Value; });
                    dq = dq.Substring(1, dq.Length - 1);
                    //PreConnection.Excut_Cmd("DELETE FROM tb_labo_immunologie WHERE ID IN (" + dq + ");");
                    PreConnection.Excut_Cmd(3, "tb_labo_immunologie", null, null, "ID IN (@P_ID)", new List<string> { "P_ID" }, new List<object> { dq });
                    //--------
                    Laboratoire.labo = PreConnection.Load_data(Laboratoire.labo_load_cmd);
                    Laboratoire.make_historic_refesh = true;
                    //------------
                    Load_histor();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && label20.Text.Trim().Length == 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PARAM_NME", typeof(string));
                dt.Columns.Add("PARAM_VAL", typeof(string));
                //----------------
                dt.Rows.Add(new object[] { "DATE", dateTimePicker1.Value.ToString("dd/MM/yyyy") });
                dt.Rows.Add(new object[] { "ANIM_NME", label3.Text });
                dt.Rows.Add(new object[] { "PRIOR", label4.Text });
                dt.Rows.Add(new object[] { "ESPECE", label6.Text });
                dt.Rows.Add(new object[] { "RACE", label8.Text });
                dt.Rows.Add(new object[] { "SEX", label13.Text });
                dt.Rows.Add(new object[] { "DATE_NISS", label14.Text });
                dt.Rows.Add(new object[] { "REF", textBox3.Text });
                dt.Rows.Add(new object[] { "OBSERV", textBox1.Text });
                dt.Rows.Add(new object[] { "CABINET", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_TEL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_EMAIL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_ADRESS", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "POIDS", Anim_poids > 0 ? Anim_poids.ToString("N2") : "" });

                dt.Rows.Add(new object[] { "CLIENT_NUM_CNI", (string)selected_animm.Cells["CLIENT_NUM_CNI"].Value });
                dt.Rows.Add(new object[] { "CLIENT_ADRESS", (string)selected_animm.Cells["CLIENT_ADRESS"].Value });
                dt.Rows.Add(new object[] { "CLIENT_CITY", (string)selected_animm.Cells["CLIENT_CITY"].Value });
                dt.Rows.Add(new object[] { "CLIENT_WILAYA", (string)selected_animm.Cells["CLIENT_WILAYA"].Value });
                dt.Rows.Add(new object[] { "CLIENT_NUM_PHONE", (string)selected_animm.Cells["CLIENT_NUM_PHONE"].Value });
                dt.Rows.Add(new object[] { "CLIENT_EMAIL", (string)selected_animm.Cells["CLIENT_EMAIL"].Value });



                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dt.Rows.Add(new object[] { "IMUN_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["MALAD_NME"].Value.ToString() : "" });
                    dt.Rows.Add(new object[] { "IMUN2_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["METHODE"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["METHODE"].Value.ToString() : "" });
                    dt.Rows.Add(new object[] { "IMUN3_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["VALUE2"].Value.ToString() : "" });
                }
                //-------------
                new Print_report("immunologie", dt,null).ShowDialog();
            }

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            button5.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button5.Visible = false;
        }

    }
}
