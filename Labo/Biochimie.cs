using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Resources;
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
using System.Windows.Forms;
using Xamarin.Forms.Internals;

namespace ALBAITAR_Softvet.Labo
{
    public partial class Biochimie : UserControl
    {
        DataGridViewRow selected_animm = null;
        DataTable lab_histor;
        DataTable new_initial_tbl;
        bool is_new = true;
        string ref_tmp = string.Empty;
        //bool default_modif_autorized = false;
        string IDD_to_select = "";
        public Biochimie(DataGridViewRow selected_anim, string ID_to_select)
        {
            InitializeComponent();
            selected_animm = selected_anim;
            IDD_to_select = ID_to_select;
            //------------------------------------
            new_initial_tbl = new DataTable();
            new_initial_tbl.Columns.Add("PARAM_NME", typeof(string));
            new_initial_tbl.Columns.Add("VALUE", typeof(decimal));
            new_initial_tbl.Columns.Add("UNIT", typeof(string));
            new_initial_tbl.Columns.Add("DEFAULT_MIN", typeof(decimal));
            new_initial_tbl.Columns.Add("DEFAULT_MAX", typeof(decimal));
            new_initial_tbl.Columns.Add("DEFAULT_FULL", typeof(string));
            string[] paramss = { "Glucose", "Urée (BUN)", "Créatinine", "Acide Urique", "Cholesterol", "Triglycérides", "Proteines Totales", "Albumina", "Globulines", "Indice alb/glb", "Bilirubine Totale", "Bilirubine Conjuguée", "GPT(ALT)", "GOT(AST)", "Phosphatases Alc", "Gamma-GT", "L.D.H", "C.P.K", "Lipase", "Amylase", "Fructosamine", "Calcium", "Phosphore", "Chlore", "Potassium", "Sodium", "Amoniac", "Fer" };
            foreach (string param in paramss)
            {
                DataRow rw = new_initial_tbl.NewRow();
                rw["PARAM_NME"] = param;
                new_initial_tbl.Rows.Add(rw);
            }
            new_initial_tbl.Rows[0]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[1]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[2]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[3]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[4]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[5]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[6]["UNIT"] = "g/dl";
            new_initial_tbl.Rows[7]["UNIT"] = "g/dl";
            new_initial_tbl.Rows[8]["UNIT"] = "g/dl";
            new_initial_tbl.Rows[9]["UNIT"] = "";
            new_initial_tbl.Rows[10]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[11]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[12]["UNIT"] = "U/L";
            new_initial_tbl.Rows[13]["UNIT"] = "U/L";
            new_initial_tbl.Rows[14]["UNIT"] = "U/L";
            new_initial_tbl.Rows[15]["UNIT"] = "U/L";
            new_initial_tbl.Rows[16]["UNIT"] = "U/L";
            new_initial_tbl.Rows[17]["UNIT"] = "U/L";
            new_initial_tbl.Rows[18]["UNIT"] = "U/L";
            new_initial_tbl.Rows[19]["UNIT"] = "U/L";
            new_initial_tbl.Rows[20]["UNIT"] = "umol/l";
            new_initial_tbl.Rows[21]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[22]["UNIT"] = "mg/dl";
            new_initial_tbl.Rows[23]["UNIT"] = "mmol/l";
            new_initial_tbl.Rows[24]["UNIT"] = "mmol/l";
            new_initial_tbl.Rows[25]["UNIT"] = "mmol/l";
            new_initial_tbl.Rows[26]["UNIT"] = "mmol/l";
            new_initial_tbl.Rows[27]["UNIT"] = "ug/dl";
            dataGridView1.DataSource = new_initial_tbl;
            //------------------------------
            

        }
        private void initial_normatifs_defaults()
        {
            bool rr = true;
            string cc = comboBox1.Visible ? (string)comboBox1.SelectedItem : selected_animm.Cells["ESPECE"].Value.ToString();
            switch (cc)
            {
                case "Canine":
                    dataGridView1.Rows[0].Cells["DEFAULT_MIN2"].Value = 70;
                    dataGridView1.Rows[1].Cells["DEFAULT_MIN2"].Value = 8;
                    dataGridView1.Rows[2].Cells["DEFAULT_MIN2"].Value = 0.7;
                    dataGridView1.Rows[3].Cells["DEFAULT_MIN2"].Value = 0;
                    dataGridView1.Rows[4].Cells["DEFAULT_MIN2"].Value = 70;
                    dataGridView1.Rows[5].Cells["DEFAULT_MIN2"].Value = 16;
                    dataGridView1.Rows[6].Cells["DEFAULT_MIN2"].Value = 5;
                    dataGridView1.Rows[7].Cells["DEFAULT_MIN2"].Value = 2;
                    dataGridView1.Rows[8].Cells["DEFAULT_MIN2"].Value = 1.5;
                    dataGridView1.Rows[9].Cells["DEFAULT_MIN2"].Value = 0.62;
                    dataGridView1.Rows[10].Cells["DEFAULT_MIN2"].Value = 0;
                    dataGridView1.Rows[11].Cells["DEFAULT_MIN2"].Value = 0;
                    dataGridView1.Rows[12].Cells["DEFAULT_MIN2"].Value = 28;
                    dataGridView1.Rows[13].Cells["DEFAULT_MIN2"].Value = 18;
                    dataGridView1.Rows[14].Cells["DEFAULT_MIN2"].Value = 12;
                    dataGridView1.Rows[15].Cells["DEFAULT_MIN2"].Value = 4;
                    dataGridView1.Rows[16].Cells["DEFAULT_MIN2"].Value = 50;
                    dataGridView1.Rows[17].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[18].Cells["DEFAULT_MIN2"].Value = 5;
                    dataGridView1.Rows[19].Cells["DEFAULT_MIN2"].Value = 580;
                    dataGridView1.Rows[20].Cells["DEFAULT_MIN2"].Value = 1.9;
                    dataGridView1.Rows[21].Cells["DEFAULT_MIN2"].Value = 6;
                    dataGridView1.Rows[22].Cells["DEFAULT_MIN2"].Value = 2;
                    dataGridView1.Rows[23].Cells["DEFAULT_MIN2"].Value = 107;
                    dataGridView1.Rows[24].Cells["DEFAULT_MIN2"].Value = 3.5;
                    dataGridView1.Rows[25].Cells["DEFAULT_MIN2"].Value = 138;
                    dataGridView1.Rows[26].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[27].Cells["DEFAULT_MIN2"].Value = 90;
                    //--------------
                    dataGridView1.Rows[0].Cells["DEFAULT_MAX2"].Value = 120;
                    dataGridView1.Rows[1].Cells["DEFAULT_MAX2"].Value = 33;
                    dataGridView1.Rows[2].Cells["DEFAULT_MAX2"].Value = 1.6;
                    dataGridView1.Rows[3].Cells["DEFAULT_MAX2"].Value = 10;
                    dataGridView1.Rows[4].Cells["DEFAULT_MAX2"].Value = 250;
                    dataGridView1.Rows[5].Cells["DEFAULT_MAX2"].Value = 120;
                    dataGridView1.Rows[6].Cells["DEFAULT_MAX2"].Value = 7;
                    dataGridView1.Rows[7].Cells["DEFAULT_MAX2"].Value = 4;
                    dataGridView1.Rows[8].Cells["DEFAULT_MAX2"].Value = 5;
                    dataGridView1.Rows[9].Cells["DEFAULT_MAX2"].Value = 1.28;
                    dataGridView1.Rows[10].Cells["DEFAULT_MAX2"].Value = 0.9;
                    dataGridView1.Rows[11].Cells["DEFAULT_MAX2"].Value = 0.3;
                    dataGridView1.Rows[12].Cells["DEFAULT_MAX2"].Value = 78;
                    dataGridView1.Rows[13].Cells["DEFAULT_MAX2"].Value = 70;
                    dataGridView1.Rows[14].Cells["DEFAULT_MAX2"].Value = 121;
                    dataGridView1.Rows[15].Cells["DEFAULT_MAX2"].Value = 23;
                    dataGridView1.Rows[16].Cells["DEFAULT_MAX2"].Value = 450;
                    dataGridView1.Rows[17].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[18].Cells["DEFAULT_MAX2"].Value = 500;
                    dataGridView1.Rows[19].Cells["DEFAULT_MAX2"].Value = 2000;
                    dataGridView1.Rows[20].Cells["DEFAULT_MAX2"].Value = 2.7;
                    dataGridView1.Rows[21].Cells["DEFAULT_MAX2"].Value = 12;
                    dataGridView1.Rows[22].Cells["DEFAULT_MAX2"].Value = 7;
                    dataGridView1.Rows[23].Cells["DEFAULT_MAX2"].Value = 120;
                    dataGridView1.Rows[24].Cells["DEFAULT_MAX2"].Value = 5;
                    dataGridView1.Rows[25].Cells["DEFAULT_MAX2"].Value = 150;
                    dataGridView1.Rows[26].Cells["DEFAULT_MAX2"].Value = 98;
                    dataGridView1.Rows[27].Cells["DEFAULT_MAX2"].Value = 150;
                    break;
                case "Feline":
                    dataGridView1.Rows[0].Cells["DEFAULT_MIN2"].Value = 75;
                    dataGridView1.Rows[1].Cells["DEFAULT_MIN2"].Value = 15;
                    dataGridView1.Rows[2].Cells["DEFAULT_MIN2"].Value = 0.8;
                    dataGridView1.Rows[3].Cells["DEFAULT_MIN2"].Value = 0;
                    dataGridView1.Rows[4].Cells["DEFAULT_MIN2"].Value = 73;
                    dataGridView1.Rows[5].Cells["DEFAULT_MIN2"].Value = 21;
                    dataGridView1.Rows[6].Cells["DEFAULT_MIN2"].Value = 5.5;
                    dataGridView1.Rows[7].Cells["DEFAULT_MIN2"].Value = 2.5;
                    dataGridView1.Rows[8].Cells["DEFAULT_MIN2"].Value = 2.5;
                    dataGridView1.Rows[9].Cells["DEFAULT_MIN2"].Value = 0.4;
                    dataGridView1.Rows[10].Cells["DEFAULT_MIN2"].Value = 0;
                    dataGridView1.Rows[11].Cells["DEFAULT_MIN2"].Value = 0;
                    dataGridView1.Rows[12].Cells["DEFAULT_MIN2"].Value = 10;
                    dataGridView1.Rows[13].Cells["DEFAULT_MIN2"].Value = 10;
                    dataGridView1.Rows[14].Cells["DEFAULT_MIN2"].Value = 10;
                    dataGridView1.Rows[15].Cells["DEFAULT_MIN2"].Value = 1;
                    dataGridView1.Rows[16].Cells["DEFAULT_MIN2"].Value = 75;
                    dataGridView1.Rows[17].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[18].Cells["DEFAULT_MIN2"].Value = 25;
                    dataGridView1.Rows[19].Cells["DEFAULT_MIN2"].Value = 500;
                    dataGridView1.Rows[20].Cells["DEFAULT_MIN2"].Value = 2;
                    dataGridView1.Rows[21].Cells["DEFAULT_MIN2"].Value = 9;
                    dataGridView1.Rows[22].Cells["DEFAULT_MIN2"].Value = 2.5;
                    dataGridView1.Rows[23].Cells["DEFAULT_MIN2"].Value = 115;
                    dataGridView1.Rows[24].Cells["DEFAULT_MIN2"].Value = 3.5;
                    dataGridView1.Rows[25].Cells["DEFAULT_MIN2"].Value = 145;
                    dataGridView1.Rows[26].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[27].Cells["DEFAULT_MIN2"].Value = 68;

                    //--------------
                    dataGridView1.Rows[0].Cells["DEFAULT_MAX2"].Value = 200;
                    dataGridView1.Rows[1].Cells["DEFAULT_MAX2"].Value = 30;
                    dataGridView1.Rows[2].Cells["DEFAULT_MAX2"].Value = 1.8;
                    dataGridView1.Rows[3].Cells["DEFAULT_MAX2"].Value = 0.3;
                    dataGridView1.Rows[4].Cells["DEFAULT_MAX2"].Value = 300;
                    dataGridView1.Rows[5].Cells["DEFAULT_MAX2"].Value = 156;
                    dataGridView1.Rows[6].Cells["DEFAULT_MAX2"].Value = 7.1;
                    dataGridView1.Rows[7].Cells["DEFAULT_MAX2"].Value = 4;
                    dataGridView1.Rows[8].Cells["DEFAULT_MAX2"].Value = 5;
                    dataGridView1.Rows[9].Cells["DEFAULT_MAX2"].Value = 1.4;
                    dataGridView1.Rows[10].Cells["DEFAULT_MAX2"].Value = 0.9;
                    dataGridView1.Rows[11].Cells["DEFAULT_MAX2"].Value = 0.1;
                    dataGridView1.Rows[12].Cells["DEFAULT_MAX2"].Value = 80;
                    dataGridView1.Rows[13].Cells["DEFAULT_MAX2"].Value = 80;
                    dataGridView1.Rows[14].Cells["DEFAULT_MAX2"].Value = 80;
                    dataGridView1.Rows[15].Cells["DEFAULT_MAX2"].Value = 10;
                    dataGridView1.Rows[16].Cells["DEFAULT_MAX2"].Value = 600;
                    dataGridView1.Rows[17].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[18].Cells["DEFAULT_MAX2"].Value = 200;
                    dataGridView1.Rows[19].Cells["DEFAULT_MAX2"].Value = 1800;
                    dataGridView1.Rows[20].Cells["DEFAULT_MAX2"].Value = 3;
                    dataGridView1.Rows[21].Cells["DEFAULT_MAX2"].Value = 12;
                    dataGridView1.Rows[22].Cells["DEFAULT_MAX2"].Value = 9;
                    dataGridView1.Rows[23].Cells["DEFAULT_MAX2"].Value = 130;
                    dataGridView1.Rows[24].Cells["DEFAULT_MAX2"].Value = 5.1;
                    dataGridView1.Rows[25].Cells["DEFAULT_MAX2"].Value = 160;
                    dataGridView1.Rows[26].Cells["DEFAULT_MAX2"].Value = 98;
                    dataGridView1.Rows[27].Cells["DEFAULT_MAX2"].Value = 215;
                    break;
                default:
                    dataGridView1.Rows[0].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[1].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[2].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[3].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[4].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[5].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[6].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[7].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[8].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[9].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[10].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[11].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[12].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[13].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[14].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[15].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[16].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[17].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[18].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[19].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[20].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[21].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[22].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[23].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[24].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[25].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[26].Cells["DEFAULT_MIN2"].Value = DBNull.Value;
                    dataGridView1.Rows[27].Cells["DEFAULT_MIN2"].Value = DBNull.Value;

                    //--------------
                    dataGridView1.Rows[0].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[1].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[2].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[3].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[4].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[5].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[6].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[7].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[8].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[9].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[10].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[11].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[12].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[13].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[14].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[15].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[16].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[17].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[18].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[19].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[20].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[21].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[22].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[23].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[24].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[25].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[26].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    dataGridView1.Rows[27].Cells["DEFAULT_MAX2"].Value = DBNull.Value;
                    //----------
                    rr = false;
                    break;
            }
            if (rr)
            {
                for (int i = 0; i < 28; i++)
                {
                    string val = (dataGridView1.Rows[i].Cells["DEFAULT_MIN2"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["DEFAULT_MIN2"].Value.ToString() + "-" : "inf ") + (dataGridView1.Rows[i].Cells["DEFAULT_MAX2"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["DEFAULT_MAX2"].Value.ToString() : "");
                    dataGridView1.Rows[i].Cells["DEFAULT_FULL"].Value = val.Equals("inf ") ? "" : val;
                }
            }


            //------------
            //if (comboBox1.Visible && comboBox1.SelectedIndex == 0)
            //{
            //    default_modif_autorized = true;
            //    dataGridView1.Columns["DEFAULT_FULL"].ReadOnly = false;
            //    dataGridView1.Columns["DEFAULT_FULL"].DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 192);
            //}
            //else
            //{
            //    default_modif_autorized = false;
            //    dataGridView1.Columns["DEFAULT_FULL"].ReadOnly = true;
            //    dataGridView1.Columns["DEFAULT_FULL"].DefaultCellStyle.BackColor = Color.White;
            //}
            //----------------
            dataGridView1.Refresh();
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
            //-------------------------            
            label21.Visible = comboBox1.Visible = !comboBox1.Items.Contains(selected_animm.Cells["ESPECE"].Value.ToString());
            if (comboBox1.Visible) { comboBox1.SelectedIndex = 0; }
            //---------------------------
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
            if (Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Biochimie").Count() > 0)
            {
                lab_histor = Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Biochimie").CopyToDataTable();
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

        //private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    if (dataGridView1.Columns[e.ColumnIndex].Name == "VALUE2" && e.RowIndex > -1)
        //    {
        //        if (dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Value.ToString().Trim().Length > 0)
        //        {
        //            bool gd = true;
        //            decimal dd = (decimal)-0.01;
        //            gd &= decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Value.ToString(), out dd);
        //            if (dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value != null && dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value.ToString().Trim().Length > 0)
        //            {
        //                gd &= dd >= (decimal)dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value;
        //            }
        //            if (dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value != null && dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value.ToString().Trim().Length > 0)
        //            {
        //                gd &= dd <= (decimal)dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value;
        //            }
        //            dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Style.BackColor = gd ? Color.FromArgb(149, 238, 163) : Color.LightCoral;
        //        }
        //        else
        //        {
        //            dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Style.BackColor = Color.FromArgb(255, 224, 192);
        //        }

        //    }
        //}

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.Columns[dataGridView1.SelectedCells[0].ColumnIndex].ReadOnly)
                {
                    //if (dataGridView1.Columns[dataGridView1.SelectedCells[0].ColumnIndex].Name == "DEFAULT_FULL" && default_modif_autorized)
                    //{

                    //}
                    //else
                    //{
                        dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                        int rww = dataGridView1.SelectedCells[0].RowIndex;
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[rww].Cells["VALUE2"].Selected = true;
                        dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                    //}
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            is_new = true;
            pictureBox1.Image = Properties.Resources.NOUVEAU_003;            
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox3.Text = ref_tmp = "BIO_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
            
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["VALUE2"].Value = DBNull.Value;
            }
            Debug.WriteLine(">>>>>>>>>>>>>>>> xx3 >>>>>>>>>> ENTRDDDD>>>>>>>>>>>>is_new >>> " + is_new);
            initial_normatifs_defaults();
            button5.Visible = false;
            Debug.WriteLine(">>>>>>>>>>>>>>>> xx3 >>>>>>>>>> ENTRDDDD>>>>>>>>>>>>is_new >>> " + is_new);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false; // don't throw an exception
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ((TextBox)e.Control).TextChanged += Hemogramme_TextChanged;
        }

        private void Hemogramme_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.EndsWith(","))
            {
                ((TextBox)sender).Text = ((TextBox)sender).Text.Substring(0, ((TextBox)sender).Text.Length - 1) + ".";
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool autorisat = Properties.Settings.Default.Last_login_is_admin || (is_new && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31001" && (Int32)QQ[3] == 1).Count() > 0) || (!is_new && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31003" && (Int32)QQ[3] == 1).Count() > 0);
            if (autorisat)
            {
                int current_row_to_select = is_new ? -1 : dataGridView2.SelectedRows[0].Index;
                bool ready = true;
                int tt = 0;
                int null_nb = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //--------
                    tt++;
                    null_nb += dataGridView1.Rows[i].Cells["VALUE2"].Value == DBNull.Value ? 1 : 0;
                }
                ready &= label20.Text.Trim().Length == 0;
                if (tt == null_nb)
                {
                    ready = false;
                    MessageBox.Show("Il n'y a pas des résultats !", "Vide :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (ready)
                {
                    if (is_new)
                    {
                        PreConnection.Excut_Cmd("INSERT INTO `tb_labo_biochimie`"
                                              + "(`REF`,"
                                              + "`DATE_TIME`,"
                                              + "`ANIM_ID`,"
                                              + "`OBSERV`,"
                                              + "`Glucose`,"
                                              + "`Urée (BUN)`,"
                                              + "`Créatinine`,"
                                              + "`Acide Urique`,"
                                              + "`Cholesterol`,"
                                              + "`Triglycérides`,"
                                              + "`Proteines Totales`,"
                                              + "`Albumina`,"
                                              + "`Globulines`,"
                                              + "`Indice alb/glb`,"
                                              + "`Bilirubine Totale`,"
                                              + "`Bilirubine Conjuguée`,"
                                              + "`GPT(ALT)`,"
                                              + "`GOT(AST)`,"
                                              + "`Phosphatases Alc`,"
                                              + "`Gamma-GT`,"
                                              + "`L.D.H`,"
                                              + "`C.P.K`,"
                                              + "`Lipase`,"
                                              + "`Amylase`,"
                                              + "`Fructosamine`,"
                                              + "`Calcium`,"
                                              + "`Phosphore`,"
                                              + "`Chlore`,"
                                              + "`Potassium`,"
                                              + "`Sodium`,"
                                              + "`Amoniac`,"
                                              + "`Fer`,"
                                              + "`Glucose_NORMATIF`,"
                                              + "`Urée (BUN)_NORMATIF`,"
                                              + "`Créatinine_NORMATIF`,"
                                              + "`Acide Urique_NORMATIF`,"
                                              + "`Cholesterol_NORMATIF`,"
                                              + "`Triglycérides_NORMATIF`,"
                                              + "`Proteines Totales_NORMATIF`,"
                                              + "`Albumina_NORMATIF`,"
                                              + "`Globulines_NORMATIF`,"
                                              + "`Indice alb/glb_NORMATIF`,"
                                              + "`Bilirubine Totale_NORMATIF`,"
                                              + "`Bilirubine Conjuguée_NORMATIF`,"
                                              + "`GPT(ALT)_NORMATIF`,"
                                              + "`GOT(AST)_NORMATIF`,"
                                              + "`Phosphatases Alc_NORMATIF`,"
                                              + "`Gamma-GT_NORMATIF`,"
                                              + "`L.D.H_NORMATIF`,"
                                              + "`C.P.K_NORMATIF`,"
                                              + "`Lipase_NORMATIF`,"
                                              + "`Amylase_NORMATIF`,"
                                              + "`Fructosamine_NORMATIF`,"
                                              + "`Calcium_NORMATIF`,"
                                              + "`Phosphore_NORMATIF`,"
                                              + "`Chlore_NORMATIF`,"
                                              + "`Potassium_NORMATIF`,"
                                              + "`Sodium_NORMATIF`,"
                                              + "`Amoniac_NORMATIF`,"
                                              + "`Fer_NORMATIF`)"
                                              + " VALUES "
                                              + "('" + textBox3.Text.Replace("'", "''") + "'," //REF
                                              + "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," //DATE_TIME
                                              + selected_animm.Cells["ID"].Value + "," //ANIM_ID
                                              + "'" + textBox1.Text.Replace("'", "''") + "'," //OBSERV
                                              + (dataGridView1.Rows[0].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[1].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[2].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[3].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[4].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[5].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[6].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[7].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[8].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[9].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[10].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[11].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[12].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[13].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[14].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[15].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[15].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[16].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[16].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[17].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[17].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[18].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[18].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[19].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[19].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[20].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[20].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[21].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[21].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[22].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[22].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[23].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[23].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[24].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[24].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[25].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[25].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[26].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[26].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[27].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[27].Cells["VALUE2"].Value : "NULL") + ","
                                              + (dataGridView1.Rows[0].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[1].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[1].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[2].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[2].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[3].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[3].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[4].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[4].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[5].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[5].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[6].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[6].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[7].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[7].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[8].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[8].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[9].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[9].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[10].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[10].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[11].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[11].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[12].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[12].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[13].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[13].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[14].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[14].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[15].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[15].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[16].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[16].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[17].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[17].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[18].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[18].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[19].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[19].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[20].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[20].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[21].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[21].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[22].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[22].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[23].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[23].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[24].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[24].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[25].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[25].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[26].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[26].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[27].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[27].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'" : "NULL") + ");");
                    }
                    else
                    {
                        PreConnection.Excut_Cmd("UPDATE `tb_labo_biochimie` SET "
                                              + "`REF` = '" + textBox3.Text.Replace("'", "''") + "',"
                                              + "`DATE_TIME` = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                                              + "`OBSERV` = '" + textBox1.Text.Replace("'","''") + "',"
                                              + "`Glucose` = " + (dataGridView1.Rows[0].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Urée (BUN)` = " + (dataGridView1.Rows[1].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Créatinine` = " + (dataGridView1.Rows[2].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Acide Urique` = " + (dataGridView1.Rows[3].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Cholesterol` = " + (dataGridView1.Rows[4].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Triglycérides` = " + (dataGridView1.Rows[5].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Proteines Totales` = " + (dataGridView1.Rows[6].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Albumina` = " + (dataGridView1.Rows[7].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Globulines` = " + (dataGridView1.Rows[8].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Indice alb/glb` = " + (dataGridView1.Rows[9].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Bilirubine Totale` = " + (dataGridView1.Rows[10].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Bilirubine Conjuguée` = " + (dataGridView1.Rows[11].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`GPT(ALT)` = " + (dataGridView1.Rows[12].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`GOT(AST)` = " + (dataGridView1.Rows[13].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Phosphatases Alc` = " + (dataGridView1.Rows[14].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Gamma-GT` = " + (dataGridView1.Rows[15].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[15].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`L.D.H` = " + (dataGridView1.Rows[16].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[16].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`C.P.K` = " + (dataGridView1.Rows[17].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[17].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Lipase` = " + (dataGridView1.Rows[18].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[18].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Amylase` = " + (dataGridView1.Rows[19].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[19].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Fructosamine` = " + (dataGridView1.Rows[20].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[20].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Calcium` = " + (dataGridView1.Rows[21].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[21].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Phosphore` = " + (dataGridView1.Rows[22].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[22].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Chlore` = " + (dataGridView1.Rows[23].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[23].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Potassium` = " + (dataGridView1.Rows[24].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[24].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Sodium` = " + (dataGridView1.Rows[25].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[25].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Amoniac` = " + (dataGridView1.Rows[26].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[26].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Fer` = " + (dataGridView1.Rows[27].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[27].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Glucose_NORMATIF` = " + (dataGridView1.Rows[0].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Urée (BUN)_NORMATIF` = " + (dataGridView1.Rows[1].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[1].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Créatinine_NORMATIF` = " + (dataGridView1.Rows[2].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[2].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Acide Urique_NORMATIF` = " + (dataGridView1.Rows[3].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[3].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Cholesterol_NORMATIF` = " + (dataGridView1.Rows[4].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[4].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Triglycérides_NORMATIF` = " + (dataGridView1.Rows[5].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[5].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Proteines Totales_NORMATIF` = " + (dataGridView1.Rows[6].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[6].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Albumina_NORMATIF` = " + (dataGridView1.Rows[7].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[7].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Globulines_NORMATIF` = " + (dataGridView1.Rows[8].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[8].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Indice alb/glb_NORMATIF` = " + (dataGridView1.Rows[9].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[9].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Bilirubine Totale_NORMATIF` = " + (dataGridView1.Rows[10].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[10].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Bilirubine Conjuguée_NORMATIF` = " + (dataGridView1.Rows[11].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[11].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`GPT(ALT)_NORMATIF` = " + (dataGridView1.Rows[12].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[12].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`GOT(AST)_NORMATIF` = " + (dataGridView1.Rows[13].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[13].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Phosphatases Alc_NORMATIF` = " + (dataGridView1.Rows[14].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[14].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Gamma-GT_NORMATIF` = " + (dataGridView1.Rows[15].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[15].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`L.D.H_NORMATIF` = " + (dataGridView1.Rows[16].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[16].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`C.P.K_NORMATIF` = " + (dataGridView1.Rows[17].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[17].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Lipase_NORMATIF` = " + (dataGridView1.Rows[18].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[18].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Amylase_NORMATIF` = " + (dataGridView1.Rows[19].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[19].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Fructosamine_NORMATIF` = " + (dataGridView1.Rows[20].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[20].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Calcium_NORMATIF` = " + (dataGridView1.Rows[21].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[21].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Phosphore_NORMATIF` = " + (dataGridView1.Rows[22].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[22].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Chlore_NORMATIF` = " + (dataGridView1.Rows[23].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[23].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Potassium_NORMATIF` = " + (dataGridView1.Rows[24].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[24].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Sodium_NORMATIF` = " + (dataGridView1.Rows[25].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[25].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Amoniac_NORMATIF` = " + (dataGridView1.Rows[26].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[26].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL") + ","
                                              + "`Fer_NORMATIF` = " + (dataGridView1.Rows[27].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" +  dataGridView1.Rows[27].Cells["DEFAULT_FULL"].Value.ToString().Replace("'","''") + "'"  : "NULL")
                                              + " WHERE `ID` = " + dataGridView2.SelectedRows[0].Cells["ID"].Value + ";");
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
                else
                {
                    MessageBox.Show("Il y a des erreurs dans votre bilan,\n\nVeuillez les corriger.\n", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                new Non_Autorized_Msg("").ShowDialog();
            }
        }


        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            button2.Visible = false;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            button2.Visible = true;
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
                textBox3.Text = ref_tmp = "BIO_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
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
                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_labo_biochimie WHERE ID = " + dataGridView2.SelectedRows[0].Cells["ID"].Value + ";");
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
                        for (int f = 5; f < 33; f++)
                        {
                            dataGridView1.Rows[f - 5].Cells["VALUE2"].Value = dt.Rows[0][f];
                        }
                        for (int f = 33; f < dt.Columns.Count; f++)
                        {
                            dataGridView1.Rows[f - 33].Cells["DEFAULT_FULL"].Value = dt.Rows[0][f];
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
                    PreConnection.Excut_Cmd("DELETE FROM tb_labo_biochimie WHERE ID IN (" + dq + ");");
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


                dt.Rows.Add(new object[] { "CLIENT_NUM_CNI", (string)selected_animm.Cells["CLIENT_NUM_CNI"].Value });
                dt.Rows.Add(new object[] { "CLIENT_ADRESS", (string)selected_animm.Cells["CLIENT_ADRESS"].Value });
                dt.Rows.Add(new object[] { "CLIENT_CITY", (string)selected_animm.Cells["CLIENT_CITY"].Value });
                dt.Rows.Add(new object[] { "CLIENT_WILAYA", (string)selected_animm.Cells["CLIENT_WILAYA"].Value });
                dt.Rows.Add(new object[] { "CLIENT_NUM_PHONE", (string)selected_animm.Cells["CLIENT_NUM_PHONE"].Value });
                dt.Rows.Add(new object[] { "CLIENT_EMAIL", (string)selected_animm.Cells["CLIENT_EMAIL"].Value });



                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dt.Rows.Add(new object[] { "BIO_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["VALUE2"].Value.ToString() : "" });
                    dt.Rows.Add(new object[] { "BIO2_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["DEFAULT_FULL"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["DEFAULT_FULL"].Value.ToString() : "" });
                }
                //-------------
                new Print_report("biochimie", dt, null).ShowDialog();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            initial_normatifs_defaults();
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
