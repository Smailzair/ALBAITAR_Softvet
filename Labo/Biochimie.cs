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

        public Biochimie(DataGridViewRow selected_anim)
        {
            InitializeComponent();
            selected_animm = selected_anim;
            //------------------------------------
            new_initial_tbl = new DataTable();
            new_initial_tbl.Columns.Add("PARAM_NME", typeof(string));
            new_initial_tbl.Columns.Add("VALUE", typeof(decimal));
            new_initial_tbl.Columns.Add("UNIT", typeof(string));
            new_initial_tbl.Columns.Add("DEFAULT_MIN", typeof(decimal));
            new_initial_tbl.Columns.Add("DEFAULT_MAX", typeof(decimal));
            new_initial_tbl.Columns.Add("DEFAULT_FULL", typeof(string));
            string[] paramss = { "Glucose", "Urée (BUN)", "Créatinine", "Acide Urique", "Cholesterol", "Triglycérides", "Proteines Totales", "Albumina", "Globulines", "Indice alb/glb", "Bilirubine Totale", "Bilirubine Conjuguée", "GPT(ALT)", "GOT(AST)", "Phosphatases Alc", "Gamma-GT", "L.D.H", "C.P.K", "Lipase", "Amylase", "Fructosamine", "Calcium", "Phosphore", "Chlore", "Potassium", "Sodium", "Amoniac", "Fer"};
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

            switch ((string)selected_animm.Cells["ESPECE"].Value)
            {
                case "Canine":
                    new_initial_tbl.Rows[0]["DEFAULT_MIN"] = 70;
                    new_initial_tbl.Rows[1]["DEFAULT_MIN"] = 8;
                    new_initial_tbl.Rows[2]["DEFAULT_MIN"] = 0.7;
                    new_initial_tbl.Rows[3]["DEFAULT_MIN"] = 0;
                    new_initial_tbl.Rows[4]["DEFAULT_MIN"] = 70;
                    new_initial_tbl.Rows[5]["DEFAULT_MIN"] = 16;
                    new_initial_tbl.Rows[6]["DEFAULT_MIN"] = 5;
                    new_initial_tbl.Rows[7]["DEFAULT_MIN"] = 2;
                    new_initial_tbl.Rows[8]["DEFAULT_MIN"] = 1.5;
                    new_initial_tbl.Rows[9]["DEFAULT_MIN"] = 0.62;
                    new_initial_tbl.Rows[10]["DEFAULT_MIN"] = 0;
                    new_initial_tbl.Rows[11]["DEFAULT_MIN"] = 0;
                    new_initial_tbl.Rows[12]["DEFAULT_MIN"] = 28;
                    new_initial_tbl.Rows[13]["DEFAULT_MIN"] = 18;
                    new_initial_tbl.Rows[14]["DEFAULT_MIN"] = 12;
                    new_initial_tbl.Rows[15]["DEFAULT_MIN"] = 4;
                    new_initial_tbl.Rows[16]["DEFAULT_MIN"] = 50;
                    //new_initial_tbl.Rows[17]["DEFAULT_MIN"] = null;
                    new_initial_tbl.Rows[18]["DEFAULT_MIN"] = 5;
                    new_initial_tbl.Rows[19]["DEFAULT_MIN"] = 580;
                    new_initial_tbl.Rows[20]["DEFAULT_MIN"] = 1.9;
                    new_initial_tbl.Rows[21]["DEFAULT_MIN"] = 6;
                    new_initial_tbl.Rows[22]["DEFAULT_MIN"] = 2;
                    new_initial_tbl.Rows[23]["DEFAULT_MIN"] = 107;
                    new_initial_tbl.Rows[24]["DEFAULT_MIN"] = 3.5;
                    new_initial_tbl.Rows[25]["DEFAULT_MIN"] = 138;
                    new_initial_tbl.Rows[26]["DEFAULT_MIN"] = DBNull.Value;
                    new_initial_tbl.Rows[27]["DEFAULT_MIN"] = 90;
                    //--------------
                    new_initial_tbl.Rows[0]["DEFAULT_MAX"] = 120;
                    new_initial_tbl.Rows[1]["DEFAULT_MAX"] = 33;
                    new_initial_tbl.Rows[2]["DEFAULT_MAX"] = 1.6;
                    new_initial_tbl.Rows[3]["DEFAULT_MAX"] = 10;
                    new_initial_tbl.Rows[4]["DEFAULT_MAX"] = 250;
                    new_initial_tbl.Rows[5]["DEFAULT_MAX"] = 120;
                    new_initial_tbl.Rows[6]["DEFAULT_MAX"] = 7;
                    new_initial_tbl.Rows[7]["DEFAULT_MAX"] = 4;
                    new_initial_tbl.Rows[8]["DEFAULT_MAX"] = 5;
                    new_initial_tbl.Rows[9]["DEFAULT_MAX"] = 1.28;
                    new_initial_tbl.Rows[10]["DEFAULT_MAX"] = 0.9;
                    new_initial_tbl.Rows[11]["DEFAULT_MAX"] = 0.3;
                    new_initial_tbl.Rows[12]["DEFAULT_MAX"] = 78;
                    new_initial_tbl.Rows[13]["DEFAULT_MAX"] = 70;
                    new_initial_tbl.Rows[14]["DEFAULT_MAX"] = 121;
                    new_initial_tbl.Rows[15]["DEFAULT_MAX"] = 23;
                    new_initial_tbl.Rows[16]["DEFAULT_MAX"] = 450;
                    new_initial_tbl.Rows[17]["DEFAULT_MAX"] = DBNull.Value;
                    new_initial_tbl.Rows[18]["DEFAULT_MAX"] = 500;
                    new_initial_tbl.Rows[19]["DEFAULT_MAX"] = 2000;
                    new_initial_tbl.Rows[20]["DEFAULT_MAX"] = 2.7;
                    new_initial_tbl.Rows[21]["DEFAULT_MAX"] = 12;
                    new_initial_tbl.Rows[22]["DEFAULT_MAX"] = 7;
                    new_initial_tbl.Rows[23]["DEFAULT_MAX"] = 120;
                    new_initial_tbl.Rows[24]["DEFAULT_MAX"] = 5;
                    new_initial_tbl.Rows[25]["DEFAULT_MAX"] = 150;
                    new_initial_tbl.Rows[26]["DEFAULT_MAX"] = 98;
                    new_initial_tbl.Rows[27]["DEFAULT_MAX"] = 150;
                    break;
                case "Feline":
                    new_initial_tbl.Rows[0]["DEFAULT_MIN"] = 75;
                    new_initial_tbl.Rows[1]["DEFAULT_MIN"] = 15;
                    new_initial_tbl.Rows[2]["DEFAULT_MIN"] = 0.8;
                    new_initial_tbl.Rows[3]["DEFAULT_MIN"] = 0;
                    new_initial_tbl.Rows[4]["DEFAULT_MIN"] = 73;
                    new_initial_tbl.Rows[5]["DEFAULT_MIN"] = 21;
                    new_initial_tbl.Rows[6]["DEFAULT_MIN"] = 5.5;
                    new_initial_tbl.Rows[7]["DEFAULT_MIN"] = 2.5;
                    new_initial_tbl.Rows[8]["DEFAULT_MIN"] = 2.5;
                    new_initial_tbl.Rows[9]["DEFAULT_MIN"] = 0.4;
                    new_initial_tbl.Rows[10]["DEFAULT_MIN"] = 0;
                    new_initial_tbl.Rows[11]["DEFAULT_MIN"] = 0;
                    new_initial_tbl.Rows[12]["DEFAULT_MIN"] = 10;
                    new_initial_tbl.Rows[13]["DEFAULT_MIN"] = 10;
                    new_initial_tbl.Rows[14]["DEFAULT_MIN"] = 10;
                    new_initial_tbl.Rows[15]["DEFAULT_MIN"] = 1;
                    new_initial_tbl.Rows[16]["DEFAULT_MIN"] = 75;
                    //new_initial_tbl.Rows[17]["DEFAULT_MIN"] = null;
                    new_initial_tbl.Rows[18]["DEFAULT_MIN"] = 25;
                    new_initial_tbl.Rows[19]["DEFAULT_MIN"] = 500;
                    new_initial_tbl.Rows[20]["DEFAULT_MIN"] = 2;
                    new_initial_tbl.Rows[21]["DEFAULT_MIN"] = 9;
                    new_initial_tbl.Rows[22]["DEFAULT_MIN"] = 2.5;
                    new_initial_tbl.Rows[23]["DEFAULT_MIN"] = 115;
                    new_initial_tbl.Rows[24]["DEFAULT_MIN"] = 3.5;
                    new_initial_tbl.Rows[25]["DEFAULT_MIN"] = 145;
                    new_initial_tbl.Rows[26]["DEFAULT_MIN"] = DBNull.Value;
                    new_initial_tbl.Rows[27]["DEFAULT_MIN"] = 68;
                    //--------------
                    new_initial_tbl.Rows[0]["DEFAULT_MAX"] = 200;
                    new_initial_tbl.Rows[1]["DEFAULT_MAX"] = 30;
                    new_initial_tbl.Rows[2]["DEFAULT_MAX"] = 1.8;
                    new_initial_tbl.Rows[3]["DEFAULT_MAX"] = 0.3;
                    new_initial_tbl.Rows[4]["DEFAULT_MAX"] = 300;
                    new_initial_tbl.Rows[5]["DEFAULT_MAX"] = 156;
                    new_initial_tbl.Rows[6]["DEFAULT_MAX"] = 7.1;
                    new_initial_tbl.Rows[7]["DEFAULT_MAX"] = 4;
                    new_initial_tbl.Rows[8]["DEFAULT_MAX"] = 5;
                    new_initial_tbl.Rows[9]["DEFAULT_MAX"] = 1.4;
                    new_initial_tbl.Rows[10]["DEFAULT_MAX"] = 0.9;
                    new_initial_tbl.Rows[11]["DEFAULT_MAX"] = 0.1;
                    new_initial_tbl.Rows[12]["DEFAULT_MAX"] = 80;
                    new_initial_tbl.Rows[13]["DEFAULT_MAX"] = 80;
                    new_initial_tbl.Rows[14]["DEFAULT_MAX"] = 80;
                    new_initial_tbl.Rows[15]["DEFAULT_MAX"] = 10;
                    new_initial_tbl.Rows[16]["DEFAULT_MAX"] = 600;
                    new_initial_tbl.Rows[17]["DEFAULT_MAX"] = DBNull.Value;
                    new_initial_tbl.Rows[18]["DEFAULT_MAX"] = 200;
                    new_initial_tbl.Rows[19]["DEFAULT_MAX"] = 1800;
                    new_initial_tbl.Rows[20]["DEFAULT_MAX"] = 3;
                    new_initial_tbl.Rows[21]["DEFAULT_MAX"] = 12;
                    new_initial_tbl.Rows[22]["DEFAULT_MAX"] = 9;
                    new_initial_tbl.Rows[23]["DEFAULT_MAX"] = 130;
                    new_initial_tbl.Rows[24]["DEFAULT_MAX"] = 5.1;
                    new_initial_tbl.Rows[25]["DEFAULT_MAX"] = 160;
                    new_initial_tbl.Rows[26]["DEFAULT_MAX"] = 98;
                    new_initial_tbl.Rows[27]["DEFAULT_MAX"] = 215;
                    break;
            }
                       

            for (int i = 0; i < paramss.Length; i++)
            {
                string val = (new_initial_tbl.Rows[i]["DEFAULT_MIN"] != DBNull.Value ? new_initial_tbl.Rows[i]["DEFAULT_MIN"].ToString() + "-" : "inf ") + (new_initial_tbl.Rows[i]["DEFAULT_MAX"] != null ? new_initial_tbl.Rows[i]["DEFAULT_MAX"].ToString() : "");
                new_initial_tbl.Rows[i]["DEFAULT_FULL"] = val.Equals("inf ") ? "" : val;
            }

            dataGridView1.DataSource = new_initial_tbl;
            //------------------------------
            button3.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ParentForm.ControlBox = true;
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
            Load_histor();


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
                if(dataGridView2.DataSource != null)
                {
                    DataTable dt = ((DataTable)dataGridView2.DataSource).Clone();
                    dataGridView2.DataSource = dt;
                }
                
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "VALUE2" && e.RowIndex > -1)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Value.ToString().Trim().Length > 0)
                {
                    bool gd = true;
                    decimal dd = (decimal)-0.01;
                    gd &= decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Value.ToString(), out dd);
                    if (dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value != null && dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value.ToString().Trim().Length > 0)
                    {
                        gd &= dd >= (decimal)dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value;
                    }
                    if (dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value != null && dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value.ToString().Trim().Length > 0)
                    {
                        gd &= dd <= (decimal)dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value;
                    }
                    dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Style.BackColor = gd ? Color.FromArgb(149, 238, 163) : Color.LightCoral;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Style.BackColor = Color.FromArgb(255, 224, 192);
                }
                
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {            
            if(dataGridView1.SelectedCells.Count > 0) {
                if (dataGridView1.Columns[dataGridView1.SelectedCells[0].ColumnIndex].Name != "VALUE2")
                {
                    dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                    int rww = dataGridView1.SelectedCells[0].RowIndex;
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[rww].Cells["VALUE2"].Selected = true;
                    dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                }
            }
            
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            is_new = true;
            pictureBox1.Image = Properties.Resources.NOUVEAU_003;
            button5.Visible = false;
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox3.Text = ref_tmp = "BIO_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;            
            //label20.Visible = false;
            for(int i = 0;  i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["VALUE2"].Value = DBNull.Value;
            }
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
                bool tmpp = true;
                int tt = 0;
                int null_nb = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    tmpp &= dataGridView1.Rows[i].Cells["VALUE2"].Style.BackColor != Color.LightCoral;
                    //--------
                    tt++;
                    null_nb += dataGridView1.Rows[i].Cells["VALUE2"].Value == DBNull.Value ? 1 : 0;
                }
                ready &= !label20.Visible;
                ready &= textBox3.BackColor != Color.LightCoral;
                if (tt == null_nb)
                {
                    ready = false;
                    MessageBox.Show("Il n'y a pas de données !", "Vide :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!tmpp && ready)
                {
                    ready = MessageBox.Show("Il y a des erreurs dans votre bilan,\n\nVoulez-vous continuer?\n", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
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
                                              + "`Fer`)"
                                              + " VALUES "
                                              + "('" + textBox3.Text + "'," //REF
                                              + "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," //DATE_TIME
                                              + selected_animm.Cells["ID"].Value + "," //ANIM_ID
                                              + "'" + textBox1.Text + "'," //OBSERV
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
                                              + (dataGridView1.Rows[27].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[27].Cells["VALUE2"].Value : "NULL") + ");");
                    }
                    else
                    {
                        PreConnection.Excut_Cmd("UPDATE `tb_labo_biochimie` SET "
                                              + "`REF` = '" + textBox3.Text + "',"
                                              + "`DATE_TIME` = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                                              + "`OBSERV` = '" + textBox1.Text + "',"
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
                                              + "`Fer` = " + (dataGridView1.Rows[27].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[27].Cells["VALUE2"].Value : "NULL")
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
            if(lab_histor != null)
            {
                int nmb = lab_histor.AsEnumerable().Where(P => ((string)P["REF"]).Trim().Length > 0 && (string)P["REF"] == textBox3.Text).Count();
                label20.Visible = is_new ? nmb > 0 : nmb > 1;
                textBox3.BackColor = label20.Visible ? Color.LightCoral : SystemColors.Window;
            }            
            textBox3.BackColor = textBox3.Text.Trim().Length > 0 ? textBox3.BackColor : Color.LightCoral;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if(textBox3.Text == ref_tmp)
            {
                textBox3.Text = ref_tmp = "BIO_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
            }
            
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                dataGridView1.CurrentCell.Value = DBNull.Value;
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView2.SelectedRows.Count > 0)
            {
                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_labo_biochimie WHERE ID = " + dataGridView2.SelectedRows[0].Cells["ID"].Value + ";");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        is_new = false;
                        pictureBox1.Image = Properties.Resources.MODIF_003;
                        button5.Visible = true;
                        ref_tmp = string.Empty;
                        dateTimePicker1.Value = (DateTime)dt.Rows[0]["DATE_TIME"];
                        textBox3.Text = (string)dt.Rows[0]["REF"];
                        textBox1.Text = (string)dt.Rows[0]["OBSERV"];
                        for (int f = 5; f < dt.Columns.Count; f++)
                        {
                            dataGridView1.Rows[f - 5].Cells["VALUE2"].Value = dt.Rows[0][f];
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
            if(dataGridView2.SelectedRows.Count > 0) { 
                if(MessageBox.Show("Sures de supprimer ("+dataGridView2.SelectedRows.Count+") bilan ?","Confirmation : ",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string dq = "";
                    dataGridView2.SelectedRows.Cast<DataGridViewRow>().ForEach(row => { dq += "," + row.Cells["ID"].Value; });
                    dq = dq.Substring(1, dq.Length - 1);
                    PreConnection.Excut_Cmd("DELETE FROM tb_labo_biochimie WHERE ID IN (" + dq+");");
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
            if(dataGridView1.Rows.Count > 0 && !label20.Visible)
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
                dt.Rows.Add(new object[] { "CABINET", Main_Frm.label_cab_nme.Text });



                dt.Rows.Add(new object[] { "CLIENT_NUM_CNI", (string)selected_animm.Cells["CLIENT_NUM_CNI"].Value });
                dt.Rows.Add(new object[] { "CLIENT_ADRESS", (string)selected_animm.Cells["CLIENT_ADRESS"].Value });
                dt.Rows.Add(new object[] { "CLIENT_CITY", (string)selected_animm.Cells["CLIENT_CITY"].Value });
                dt.Rows.Add(new object[] { "CLIENT_WILAYA", (string)selected_animm.Cells["CLIENT_WILAYA"].Value });
                dt.Rows.Add(new object[] { "CLIENT_NUM_PHONE", (string)selected_animm.Cells["CLIENT_NUM_PHONE"].Value });
                dt.Rows.Add(new object[] { "CLIENT_EMAIL", (string)selected_animm.Cells["CLIENT_EMAIL"].Value });



                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {                    
                    dt.Rows.Add(new object[] { "BIO_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["VALUE2"].Value.ToString() : "" });
                }
                //-------------
                new Print_report("biochimie", dt).ShowDialog();
            }
            
        }
    }
}
