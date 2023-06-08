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
    public partial class Urologie : UserControl
    {
        DataGridViewRow selected_animm = null;
        DataTable lab_histor;
        DataTable new_initial_tbl;
        bool is_new = true;
        string ref_tmp = string.Empty;
        //bool default_modif_autorized = false;
        string IDD_to_select = "";

        public Urologie(DataGridViewRow selected_anim, string ID_to_select)
        {
            InitializeComponent();
            selected_animm = selected_anim;
            IDD_to_select = ID_to_select;
            //------------------------------------
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button5.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30004" && (Int32)QQ[3] == 1).Count() > 0; //Imprimer
            }
            //--------------------------------------
            new_initial_tbl = new DataTable();
            new_initial_tbl.Columns.Add("PARAM_NME", typeof(string));
            string[] paramss = { "Densité","Leucocites", "Nitrites","pH","Proteines","Couleur"};
            string[] paramss2 = { "Glucose","Corps Cétoniques","Urobilirogenes", "Bilirubine", "Sang", "Hémoglobine","Turbulances"};
            foreach (string param in paramss)
            {
                dataGridView1.Rows.Add(param);
            }
            foreach (string param in paramss2)
            {
                dataGridView3.Rows.Add(param);
            }
            //------------------------------

            
        }
        //private void initial_normatifs_defaults()
        //{
            
        //    //----------------
        //    dataGridView1.Refresh();
        //}
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
                    IDD_to_select = "-9999";
                    //button3.PerformClick();
                }
            }
            else
            {
                IDD_to_select = "-9999";
                //button3.PerformClick();
            }
            //------------------
        }

        private void Load_histor()
        {
            if (Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Urologie").Count() > 0)
            {
                lab_histor = Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Urologie").CopyToDataTable();
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

        
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).SelectedCells.Count > 0)
            {
                if (((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].ReadOnly)
                {
                    ((DataGridView)sender).SelectionChanged -= dataGridView1_SelectionChanged;
                        int rww = ((DataGridView)sender).SelectedCells[0].RowIndex;
                    ((DataGridView)sender).ClearSelection();
                    ((DataGridView)sender).Rows[rww].Cells[1].Selected = true;
                    ((DataGridView)sender).SelectionChanged += dataGridView1_SelectionChanged;                   

                }
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            is_new = true;
            pictureBox1.Image = Properties.Resources.NOUVEAU_003;            
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox3.Text = ref_tmp = "URO_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = DBNull.Value;
            }
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView3.CellValueChanged -= dataGridView1_CellValueChanged;
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                dataGridView3.Rows[i].Cells[1].Value = DBNull.Value;
            }
            dataGridView3.CellValueChanged += dataGridView1_CellValueChanged;
            textBox5.Text = string.Empty;
            button5.Visible = false;
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
                //bool tmpp = true;
                int tt = 0;
                int null_nb = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //--------
                    tt++;
                    null_nb += dataGridView1.Rows[i].Cells["VALUE2"].Value == DBNull.Value ? 1 : 0;
                }
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    //--------
                    tt++;
                    null_nb += dataGridView3.Rows[i].Cells[1].Value == DBNull.Value ? 1 : 0;
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
                        PreConnection.Excut_Cmd("INSERT INTO `tb_labo_urologie`"
                                              + "(`REF`,"
                                              + "`DATE_TIME`,"
                                              + "`ANIM_ID`,"
                                              + "`OBSERV`,"
                                              + "`Densité`,"
                                              + "`Leucocites`,"
                                              + "`Nitrites`,"
                                              + "`pH`,"
                                              + "`Proteines`,"
                                              + "`Couleur`,"
                                              + "`Glucose`,"
                                              + "`Corps Cétoniques`,"
                                              + "`Urobilirogenes`,"
                                              + "`Bilirubine`,"
                                              + "`Sang`,"
                                              + "`Hémoglobine`,"
                                              + "`Turbulances`,"
                                              + "`Sediment Urinaire`)"
                                              + " VALUES "
                                              + "('" + textBox3.Text.Replace("'", "''") + "'," //REF
                                              + "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," //DATE_TIME
                                              + selected_animm.Cells["ID"].Value + "," //ANIM_ID
                                              + "'" + textBox1.Text.ToString().Replace("'", "''") + "'," //OBSERV
                                              + (dataGridView1.Rows[0].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells[1].Value + "'" : "NULL") + "," //Densité
                                              + (dataGridView1.Rows[1].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[1].Cells[1].Value + "'" : "NULL") + "," //Leucocites
                                              + (dataGridView1.Rows[2].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[2].Cells[1].Value + "'" : "NULL") + "," //Nitrites
                                              + (dataGridView1.Rows[3].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[3].Cells[1].Value + "'" : "NULL") + "," //pH
                                              + (dataGridView1.Rows[4].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[4].Cells[1].Value + "'" : "NULL") + "," //Proteines
                                              + (dataGridView1.Rows[5].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[5].Cells[1].Value + "'" : "NULL") + "," //Couleur

                                              + (dataGridView3.Rows[0].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[0].Cells[1].Value + "'" : "NULL") + "," //Glucose
                                              + (dataGridView3.Rows[1].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[1].Cells[1].Value + "'" : "NULL") + "," //Corps Cétoniques
                                              + (dataGridView3.Rows[2].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[2].Cells[1].Value + "'" : "NULL") + "," //Urobilirogenes
                                              + (dataGridView3.Rows[3].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[3].Cells[1].Value + "'" : "NULL") + "," //Bilirubine
                                              + (dataGridView3.Rows[4].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[4].Cells[1].Value + "'" : "NULL") + "," //Sang
                                              + (dataGridView3.Rows[5].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[5].Cells[1].Value + "'" : "NULL") + "," //Hémoglobine
                                              + (dataGridView3.Rows[6].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[6].Cells[1].Value + "'" : "NULL") + "," //Turbulances

                                              + "'" + textBox5.Text + "');"); //Sediment Urinaire

                    }
                    else
                    {
                         PreConnection.Excut_Cmd("UPDATE `tb_labo_urologie` SET "
                                              + "`REF` = '" + textBox3.Text.Replace("'", "''") + "',"
                                              + "`DATE_TIME` = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                                              + "`OBSERV` = '" + textBox1.Text.ToString().Replace("'", "''") + "',"
                                              + "`Densité` = " + (dataGridView1.Rows[0].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells[1].Value + "'" : "NULL") + "," //Densité
                                              + "`Leucocites` = " + (dataGridView1.Rows[1].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[1].Cells[1].Value + "'" : "NULL") + "," //Leucocites
                                              + "`Nitrites` = " + (dataGridView1.Rows[2].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[2].Cells[1].Value + "'" : "NULL") + "," //Nitrites
                                              + "`pH` = " + (dataGridView1.Rows[3].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[3].Cells[1].Value + "'" : "NULL") + "," //pH
                                              + "`Proteines` = " + (dataGridView1.Rows[4].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[4].Cells[1].Value + "'" : "NULL") + "," //Proteines
                                              + "`Couleur` = " + (dataGridView1.Rows[5].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[5].Cells[1].Value + "'" : "NULL") + "," //Couleur
                                              + "`Glucose` = " + (dataGridView3.Rows[0].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[0].Cells[1].Value + "'" : "NULL") + "," //Glucose
                                              + "`Corps Cétoniques` = " + (dataGridView3.Rows[1].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[1].Cells[1].Value + "'" : "NULL") + "," //Corps Cétoniques
                                              + "`Urobilirogenes` = " + (dataGridView3.Rows[2].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[2].Cells[1].Value + "'" : "NULL") + "," //Urobilirogenes
                                              + "`Bilirubine` = " + (dataGridView3.Rows[3].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[3].Cells[1].Value + "'" : "NULL") + "," //Bilirubine
                                              + "`Sang` = " + (dataGridView3.Rows[4].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[4].Cells[1].Value + "'" : "NULL") + "," //Sang
                                              + "`Hémoglobine` = " + (dataGridView3.Rows[5].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[5].Cells[1].Value + "'" : "NULL") + "," //Hémoglobine
                                              + "`Turbulances` = " + (dataGridView3.Rows[6].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[6].Cells[1].Value + "'" : "NULL") + "," //Turbulances
                                              + "`Sediment Urinaire` = '" + textBox5.Text + "'"
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
                textBox3.Text = ref_tmp = "URO_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
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
                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_labo_urologie WHERE ID = " + dataGridView2.SelectedRows[0].Cells["ID"].Value + ";");
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
                        for (int f = 5; f < 11; f++)
                        {
                            dataGridView1.Rows[f - 5].Cells[1].Value = dt.Rows[0][f];
                        }
                        for (int f = 11; f < 18; f++)
                        {
                            dataGridView3.Rows[f - 11].Cells[1].Value = dt.Rows[0][f];
                        }
                        textBox5.Text = dt.Rows[0][18] != DBNull.Value ? (string)dt.Rows[0][18] : "";
                        button5.Visible = true;
                        //--------------------
                        if (IDD_to_select == "-9999")
                        {
                            IDD_to_select = null;
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
                    PreConnection.Excut_Cmd("DELETE FROM tb_labo_urologie WHERE ID IN (" + dq + ");");
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

                dt.Rows.Add(new object[] { "BIO2_001", dataGridView1.Rows[0].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells[1].Value + "'" : "" });      //Densité              
                dt.Rows.Add(new object[] { "BIO2_002", dataGridView1.Rows[1].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[1].Cells[1].Value + "'" : "" });      //Leucocites              
                dt.Rows.Add(new object[] { "BIO2_003", dataGridView1.Rows[2].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[2].Cells[1].Value + "'" : "" });      //Nitrites              
                dt.Rows.Add(new object[] { "BIO2_004", dataGridView1.Rows[3].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[3].Cells[1].Value + "'" : "" });      //pH              
                dt.Rows.Add(new object[] { "BIO2_005", dataGridView1.Rows[4].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[4].Cells[1].Value + "'" : "" });      //Proteines              
                dt.Rows.Add(new object[] { "BIO2_006", dataGridView1.Rows[5].Cells[1].Value != DBNull.Value ? "'" + dataGridView1.Rows[5].Cells[1].Value + "'" : "" });      //Couleur
                                                                                                                                                                                 
                dt.Rows.Add(new object[] { "BIO2_007", dataGridView3.Rows[0].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[0].Cells[1].Value + "'" : "" });      //Glucose              
                dt.Rows.Add(new object[] { "BIO2_008", dataGridView3.Rows[1].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[1].Cells[1].Value + "'" : "" });      //Corps Cétoniques              
                dt.Rows.Add(new object[] { "BIO2_009", dataGridView3.Rows[2].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[2].Cells[1].Value + "'" : "" });      //Urobilirogenes              
                dt.Rows.Add(new object[] { "BIO2_010", dataGridView3.Rows[3].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[3].Cells[1].Value + "'" : "" });      //Bilirubine              
                dt.Rows.Add(new object[] { "BIO2_011", dataGridView3.Rows[4].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[4].Cells[1].Value + "'" : "" });      //Sang              
                dt.Rows.Add(new object[] { "BIO2_012", dataGridView3.Rows[5].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[5].Cells[1].Value + "'" : "" });      //Hémoglobine              
                dt.Rows.Add(new object[] { "BIO2_013", dataGridView3.Rows[6].Cells[1].Value != DBNull.Value ? "'" + dataGridView3.Rows[6].Cells[1].Value + "'" : "" });      //Turbulances
                                                                                                                                                                                 //
                dt.Rows.Add(new object[] { "BIO2_014", textBox5.Text });      //Sediment Urinaire              
                

                //-------------
                new Print_report("urologie", dt, null).ShowDialog();
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
