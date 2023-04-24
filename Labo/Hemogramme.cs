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
    public partial class Hemogramme : UserControl
    {
        DataGridViewRow selected_animm = null;
        DataTable lab_histor;
        DataTable new_initial_tbl;
        bool is_new = true;
        string ref_tmp = string.Empty;

        public Hemogramme(DataGridViewRow selected_anim)
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
            string[] paramss = { "Hematies", "Hemoglobine", "Hematocrite", "VGM", "CCMH", "TCMH", "Reticulocytes", "Plaquettes", "Leucocytes", "Granulocytes", "Neutrophiles", "Eosinophiles", "Basophiles", "Lymphocytes", "Monocytes" };
            foreach (string param in paramss)
            {
                DataRow rw = new_initial_tbl.NewRow();
                rw["PARAM_NME"] = param;
                new_initial_tbl.Rows.Add(rw);
            }
            new_initial_tbl.Rows[0]["UNIT"] = "mill/ul";
            new_initial_tbl.Rows[1]["UNIT"] = "g/dl";
            new_initial_tbl.Rows[2]["UNIT"] = "%";
            new_initial_tbl.Rows[3]["UNIT"] = "fl";
            new_initial_tbl.Rows[4]["UNIT"] = "g/dl (%)";
            new_initial_tbl.Rows[5]["UNIT"] = "pg";
            new_initial_tbl.Rows[6]["UNIT"] = "%";
            new_initial_tbl.Rows[7]["UNIT"] = "mil";
            new_initial_tbl.Rows[8]["UNIT"] = "mil";
            new_initial_tbl.Rows[9]["UNIT"] = "%";
            new_initial_tbl.Rows[10]["UNIT"] = "%";
            new_initial_tbl.Rows[11]["UNIT"] = "%";
            new_initial_tbl.Rows[12]["UNIT"] = "%";
            new_initial_tbl.Rows[13]["UNIT"] = "%";
            new_initial_tbl.Rows[14]["UNIT"] = "%";
            new_initial_tbl.Rows[0]["DEFAULT_MIN"] = 5.5;
            new_initial_tbl.Rows[1]["DEFAULT_MIN"] = 12;
            new_initial_tbl.Rows[2]["DEFAULT_MIN"] = 37;
            new_initial_tbl.Rows[3]["DEFAULT_MIN"] = 60;
            new_initial_tbl.Rows[4]["DEFAULT_MIN"] = 32;
            new_initial_tbl.Rows[5]["DEFAULT_MIN"] = 19.5;
            new_initial_tbl.Rows[6]["DEFAULT_MIN"] = 0;
            new_initial_tbl.Rows[7]["DEFAULT_MIN"] = 200;
            new_initial_tbl.Rows[8]["DEFAULT_MIN"] = 6.0;
            new_initial_tbl.Rows[9]["DEFAULT_MIN"] = 3;
            new_initial_tbl.Rows[10]["DEFAULT_MIN"] = 0;
            new_initial_tbl.Rows[11]["DEFAULT_MIN"] = 0.1;
            new_initial_tbl.Rows[13]["DEFAULT_MIN"] = 1;
            new_initial_tbl.Rows[14]["DEFAULT_MIN"] = 0.15;
            new_initial_tbl.Rows[0]["DEFAULT_MAX"] = 8.5;
            new_initial_tbl.Rows[1]["DEFAULT_MAX"] = 18;
            new_initial_tbl.Rows[2]["DEFAULT_MAX"] = 55;
            new_initial_tbl.Rows[3]["DEFAULT_MAX"] = 77;
            new_initial_tbl.Rows[4]["DEFAULT_MAX"] = 36;
            new_initial_tbl.Rows[5]["DEFAULT_MAX"] = 24.5;
            new_initial_tbl.Rows[6]["DEFAULT_MAX"] = 127;
            new_initial_tbl.Rows[7]["DEFAULT_MAX"] = 500;
            new_initial_tbl.Rows[8]["DEFAULT_MAX"] = 17;
            new_initial_tbl.Rows[9]["DEFAULT_MAX"] = 11.5;
            new_initial_tbl.Rows[10]["DEFAULT_MAX"] = 0.3;
            new_initial_tbl.Rows[11]["DEFAULT_MAX"] = 1.25;
            new_initial_tbl.Rows[13]["DEFAULT_MAX"] = 4.8;
            new_initial_tbl.Rows[14]["DEFAULT_MAX"] = 1.35;
            new_initial_tbl.Rows[0]["DEFAULT_FULL"] = "5.5-8.5";
            new_initial_tbl.Rows[1]["DEFAULT_FULL"] = "12-18";
            new_initial_tbl.Rows[2]["DEFAULT_FULL"] = "37-55";
            new_initial_tbl.Rows[3]["DEFAULT_FULL"] = "60-77";
            new_initial_tbl.Rows[4]["DEFAULT_FULL"] = "32-36";
            new_initial_tbl.Rows[5]["DEFAULT_FULL"] = "19.5-24.5";
            new_initial_tbl.Rows[6]["DEFAULT_FULL"] = "0-127";
            new_initial_tbl.Rows[7]["DEFAULT_FULL"] = "200-500";
            new_initial_tbl.Rows[8]["DEFAULT_FULL"] = "6.0-17";
            new_initial_tbl.Rows[9]["DEFAULT_FULL"] = "3-11.5";
            new_initial_tbl.Rows[10]["DEFAULT_FULL"] = "0-0.3";
            new_initial_tbl.Rows[11]["DEFAULT_FULL"] = "0.1-1.25";
            new_initial_tbl.Rows[13]["DEFAULT_FULL"] = "1-4.8";
            new_initial_tbl.Rows[14]["DEFAULT_FULL"] = "0.15-1.35";
            dataGridView1.DataSource = new_initial_tbl;
            //------------------------------
            button3.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void Hemogramme_Load(object sender, EventArgs e)
        {            
            
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
            if (Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Hemogramme").Count() > 0)
            {
                lab_histor = Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Hemogramme").CopyToDataTable();                
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
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox3.Text = ref_tmp = "HEM_" + DateTime.Now.ToString("ddMMyyyy") + "_" + selected_animm.Cells["ID"].Value;            
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
            if(tt == null_nb)
            {
                ready = false;
                MessageBox.Show("Il n'y a pas de données !","Vide :",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            if (!tmpp && ready)
            {
                ready = MessageBox.Show("Il y a des erreurs dans votre bilan,\n\nVoulez-vous continuer?\n","Attention :",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes;
            }
            if (ready)
            {
                if (is_new)
                {   
                    PreConnection.Excut_Cmd("INSERT INTO `tb_labo_hemogramme` "
                                          + "(`REF`,"
                                          + "`DATE_TIME`,"
                                          + "`ANIM_ID`,"
                                          + "`OBSERV`,"
                                          + "`Hematies`,"
                                          + "`Hemoglobine`,"
                                          + "`Hematocrite`,"
                                          + "`VGM`,"
                                          + "`CCMH`,"
                                          + "`TCMH`,"
                                          + "`Reticulocytes`,"
                                          + "`Plaquettes`,"
                                          + "`Leucocytes`,"
                                          + "`Granulocytes`,"
                                          + "`Neutrophiles`,"
                                          + "`Eosinophiles`,"
                                          + "`Basophiles`,"
                                          + "`Lymphocytes`,"
                                          + "`Monocytes`)"
                                          + " VALUES "
                                          + "('" + textBox3.Text + "'," //REF
                                          + "'"+dateTimePicker1.Value.ToString("yyyy-MM-dd")+"'," //DATE_TIME
                                          + selected_animm.Cells["ID"].Value + "," //ANIM_ID
                                          + "'"+textBox1.Text+"'," //OBSERV
                                          + (dataGridView1.Rows[0].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["VALUE2"].Value : "NULL") + "," //Hematies
                                          + (dataGridView1.Rows[1].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["VALUE2"].Value : "NULL") + "," //Hemoglobine
                                          + (dataGridView1.Rows[2].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["VALUE2"].Value : "NULL") + "," //Hematocrite
                                          + (dataGridView1.Rows[3].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["VALUE2"].Value : "NULL") + "," //VGM
                                          + (dataGridView1.Rows[4].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["VALUE2"].Value : "NULL") + "," //CCMH
                                          + (dataGridView1.Rows[5].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["VALUE2"].Value : "NULL") + "," //TCMH
                                          + (dataGridView1.Rows[6].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["VALUE2"].Value : "NULL") + "," //Reticulocytes
                                          + (dataGridView1.Rows[7].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["VALUE2"].Value : "NULL") + "," //Plaquettes
                                          + (dataGridView1.Rows[8].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["VALUE2"].Value : "NULL") + "," //Leucocytes
                                          + (dataGridView1.Rows[9].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["VALUE2"].Value : "NULL") + "," // Granulocytes
                                          + (dataGridView1.Rows[10].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["VALUE2"].Value : "NULL") + "," //Neutrophiles
                                          + (dataGridView1.Rows[11].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["VALUE2"].Value : "NULL") + "," //Eosinophiles
                                          + (dataGridView1.Rows[12].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["VALUE2"].Value : "NULL") + "," //Basophiles
                                          + (dataGridView1.Rows[13].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["VALUE2"].Value : "NULL") + "," //Lymphocytes
                                          + (dataGridView1.Rows[14].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["VALUE2"].Value : "NULL") + ");"); //Monocytes
                }
                else
                {
                    PreConnection.Excut_Cmd("UPDATE `tb_labo_hemogramme` SET "
                                          + "`REF` = '"+ textBox3.Text + "',"
                                          + "`DATE_TIME` = '"+dateTimePicker1.Value.ToString("yyyy-MM-dd")+"',"
                                          + "`OBSERV` = '"+textBox1.Text+"',"
                                          + "`Hematies` = "+ (dataGridView1.Rows[0].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Hemoglobine` = " + (dataGridView1.Rows[1].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Hematocrite` = " + (dataGridView1.Rows[2].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`VGM` = " + (dataGridView1.Rows[3].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`CCMH` = " + (dataGridView1.Rows[4].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`TCMH` = " + (dataGridView1.Rows[5].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Reticulocytes` = " + (dataGridView1.Rows[6].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Plaquettes` = " + (dataGridView1.Rows[7].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Leucocytes` = " + (dataGridView1.Rows[8].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[8].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Granulocytes` = " + (dataGridView1.Rows[9].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[9].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Neutrophiles` = " + (dataGridView1.Rows[10].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[10].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Eosinophiles` = " + (dataGridView1.Rows[11].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[11].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Basophiles` = " + (dataGridView1.Rows[12].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[12].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Lymphocytes` = " + (dataGridView1.Rows[13].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[13].Cells["VALUE2"].Value : "NULL") + ","
                                          + "`Monocytes` = " + (dataGridView1.Rows[14].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[14].Cells["VALUE2"].Value : "NULL")
                                          + " WHERE `ID` = " + dataGridView2.SelectedRows[0].Cells["ID"].Value +";");
                }
                Laboratoire.load_labos_data();
                Load_histor();
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
                textBox3.Text = ref_tmp = "HEM_" + dateTimePicker1.Value.ToString("ddMMyyyy") + "_" + selected_animm.Cells["ID"].Value;
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
                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_labo_hemogramme WHERE ID = " + dataGridView2.SelectedRows[0].Cells["ID"].Value + ";");
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
                    PreConnection.Excut_Cmd("DELETE FROM tb_labo_hemogramme WHERE ID IN ("+dq+");");
                    Laboratoire.load_labos_data();
                    Load_histor();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PARAM_NME", typeof(string));
            dt.Columns.Add("PARAM_VAL", typeof(string));
            //----------------
            dt.Rows.Add(new object[] { "DATE" , "25/05/2025"});
            //-------------
            new Print_report("hemogramme", dt).ShowDialog();
        }
    }
}
