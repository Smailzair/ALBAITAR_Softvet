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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using TextBox = System.Windows.Forms.TextBox;

namespace ALBAITAR_Softvet.Labo
{
    public partial class Protéinogramme : UserControl
    {
        DataGridViewRow selected_animm = null;
        DataTable lab_histor;
        DataTable new_initial_tbl;
        bool is_new = true;
        string ref_tmp = string.Empty;
        //bool default_modif_autorized = false;
        bool cbx_tmp = true;
        string IDD_to_select = "";
        public Protéinogramme(DataGridViewRow selected_anim, string ID_to_select)
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
            string[] paramss = { "Protéines Totales", "Albumine", "Alpha-1-Globulines", "Alpha-2-Globulines", "Beta-Globulines", "Gamma-Globulines", "Globulines Totales", "Coefficient A/G"};
            foreach (string param in paramss)
            {
                DataRow rw = new_initial_tbl.NewRow();
                rw["PARAM_NME"] = param;
                new_initial_tbl.Rows.Add(rw);                
                
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
            label21.Visible = comboBox1.Visible = selected_animm.Cells["ESPECE"].Value.ToString() != "Canine";
            if (comboBox1.Visible)
            {
                comboBox1.SelectedIndex = 0;
            }            
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
            }
            //------------------

        }

        private void Load_histor()
        {
            if (Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Protéinogramme").Count() > 0)
            {
                lab_histor = Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && (string)P["LABO_NME"] == "Protéinogramme").CopyToDataTable();
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
        //        if (dataGridView1.Rows[e.RowIndex].Cells["VALUE2"].Value.ToString().Trim().Length > 0 && (!comboBox1.Visible || (comboBox1.Visible && comboBox1.SelectedIndex == 1)))
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
                    //if(dataGridView1.Columns[dataGridView1.SelectedCells[0].ColumnIndex].Name == "DEFAULT_FULL" && default_modif_autorized)
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
            textBox3.Text = ref_tmp = "PROT_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
            //------------
            cbx_tmp = false;
            comboBox1.SelectedIndex = 0;
            cbx_tmp = true;
            //-----------            
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["VALUE2"].Value = DBNull.Value;
            }
            //initial_normatifs_defaults();

            dataGridView1.Rows[0].Cells["UNIT2"].Value = "g/dl";
           // dataGridView1_CellValueChanged(null, new DataGridViewCellEventArgs(dataGridView1.Columns["UNIT2"].Index, 0));
            dataGridView1.Rows[1].Cells["UNIT2"].Value = "%";
            dataGridView1.Rows[2].Cells["UNIT2"].Value = "%";
            dataGridView1.Rows[3].Cells["UNIT2"].Value = "%";
            dataGridView1.Rows[4].Cells["UNIT2"].Value = "%";
            dataGridView1.Rows[5].Cells["UNIT2"].Value = "%";
            dataGridView1.Rows[6].Cells["UNIT2"].Value = "%";
            dataGridView1.Rows[7].Cells["UNIT2"].Value = "%";

            
            //----------------
            dataGridView1.Refresh();
            //---------------------
            button5.Visible = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false; // don't throw an exception
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if(e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;
                cel_tmp = dataGridView1.CurrentCell;                
                comboBox.SelectedIndexChanged += Protéinogramme_SelectedIndexChanged; // Add the event handler
            }
            else
            {
                cel_tmp = null;
                ((TextBox)e.Control).TextChanged += Hemogramme_TextChanged;
            }
            
        }
        DataGridViewCell cel_tmp;
        private void Protéinogramme_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dataGridView1_CellValueChanged(null, new DataGridViewCellEventArgs(dataGridView1.Columns["UNIT2"].Index, cel_tmp.RowIndex));
            if (cel_tmp != null)
            {
                ((ComboBox)sender).SelectedIndexChanged -= Protéinogramme_SelectedIndexChanged;
                dataGridView1.Rows[cel_tmp.RowIndex].Cells[cel_tmp.ColumnIndex].Value = ((ComboBox)sender).SelectedItem.ToString();
            }
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
                    //tmpp &= dataGridView1.Rows[i].Cells["VALUE2"].Style.BackColor != Color.LightCoral;
                    //--------
                    tt++;
                    null_nb += dataGridView1.Rows[i].Cells["VALUE2"].Value == DBNull.Value ? 1 : 0;
                }
                ready &= label20.Text.Trim().Length == 0;
                ready &= textBox3.BackColor != Color.LightCoral;
                if (tt == null_nb)
                {
                    ready = false;
                    MessageBox.Show("Il n'y a pas des resultats !", "Vide :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //if (/*!tmpp && */ready)
                //{
                //    ready = MessageBox.Show("Il y a des erreurs dans votre bilan,\n\nVoulez-vous continuer?\n", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                //}
                if (ready)
                {
                    if (is_new)
                    {
                        PreConnection.Excut_Cmd("INSERT INTO `tb_labo_proteinogramme` "
                                              + "(`REF`,"
                                              + "`DATE_TIME`,"
                                              + "`ANIM_ID`,"
                                              + "`OBSERV`,"
                                              + "`Index_Normatif_De`,"
                                              + "`Protéines Totales`,"
                                              + "`Albumine`,"
                                              + "`Alpha-1-Globulines`,"
                                              + "`Alpha-2-Globulines`,"
                                              + "`Beta-Globulines`,"
                                              + "`Gamma-Globulines`,"
                                              + "`Globulines Totales`,"
                                              + "`Coefficient A/G`,"
                                              + "`Protéines Totales_UNIT`,"
                                              + "`Albumine_UNIT`,"
                                              + "`Alpha-1-Globulines_UNIT`,"
                                              + "`Alpha-2-Globulines_UNIT`,"
                                              + "`Beta-Globulines_UNIT`,"
                                              + "`Gamma-Globulines_UNIT`,"
                                              + "`Globulines Totales_UNIT`,"
                                              + "`Coefficient A/G_UNIT`,"
                                              + "`Protéines Totales_NORMATIF`,"
                                              + "`Albumine_NORMATIF`,"
                                              + "`Alpha-1-Globulines_NORMATIF`,"
                                              + "`Alpha-2-Globulines_NORMATIF`,"
                                              + "`Beta-Globulines_NORMATIF`,"
                                              + "`Gamma-Globulines_NORMATIF`,"
                                              + "`Globulines Totales_NORMATIF`,"
                                              + "`Coefficient A/G_NORMATIF`)"
                                              + " VALUES "
                                              + "('" + textBox3.Text.Replace("'", "''") + "'," //REF
                                              + "'" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," //DATE_TIME
                                              + selected_animm.Cells["ID"].Value + "," //ANIM_ID
                                              + "'" + textBox1.Text.Replace("'", "''") + "'," //OBSERV
                                              + comboBox1.SelectedIndex + "," //Index_Normatif_De

                                              + (dataGridView1.Rows[0].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["VALUE2"].Value : "NULL") + "," 
                                              + (dataGridView1.Rows[1].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["VALUE2"].Value : "NULL") + "," 
                                              + (dataGridView1.Rows[2].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["VALUE2"].Value : "NULL") + "," 
                                              + (dataGridView1.Rows[3].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["VALUE2"].Value : "NULL") + "," 
                                              + (dataGridView1.Rows[4].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["VALUE2"].Value : "NULL") + "," 
                                              + (dataGridView1.Rows[5].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["VALUE2"].Value : "NULL") + "," 
                                              + (dataGridView1.Rows[6].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["VALUE2"].Value : "NULL") + "," 
                                              + (dataGridView1.Rows[7].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["VALUE2"].Value : "NULL") + ","

                                              + (dataGridView1.Rows[0].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[1].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[1].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[2].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[2].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[3].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[3].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[4].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[4].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[5].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[5].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[6].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[6].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[7].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[7].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","

                                              + (dataGridView1.Rows[0].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[1].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[1].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[2].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[2].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[3].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[3].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[4].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[4].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[5].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[5].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ", "
                                              + (dataGridView1.Rows[6].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[6].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + (dataGridView1.Rows[7].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[7].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ");"); 
                    }
                    else
                    {                        
                        PreConnection.Excut_Cmd("UPDATE `tb_labo_proteinogramme` SET "
                                              + "`REF` = '" + textBox3.Text.Replace("'", "''") + "',"
                                              + "`DATE_TIME` = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',"
                                              + "`OBSERV` = '" + textBox1.Text.Replace("'", "''") + "',"
                                              + "`Index_Normatif_De` = " + comboBox1.SelectedIndex + ","
                                              + "`Protéines Totales` = " + (dataGridView1.Rows[0].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[0].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Albumine` = " + (dataGridView1.Rows[1].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[1].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Alpha-1-Globulines` = " + (dataGridView1.Rows[2].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[2].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Alpha-2-Globulines` = " + (dataGridView1.Rows[3].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[3].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Beta-Globulines` = " + (dataGridView1.Rows[4].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[4].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Gamma-Globulines` = " + (dataGridView1.Rows[5].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[5].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Globulines Totales` = " + (dataGridView1.Rows[6].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[6].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Coefficient A/G` = " + (dataGridView1.Rows[7].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[7].Cells["VALUE2"].Value : "NULL") + ","
                                              + "`Protéines Totales_UNIT` = " + (dataGridView1.Rows[0].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Albumine_UNIT` = " + (dataGridView1.Rows[1].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[1].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Alpha-1-Globulines_UNIT` = " + (dataGridView1.Rows[2].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[2].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Alpha-2-Globulines_UNIT` = " + (dataGridView1.Rows[3].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[3].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Beta-Globulines_UNIT` = " + (dataGridView1.Rows[4].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[4].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Gamma-Globulines_UNIT` = " + (dataGridView1.Rows[5].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[5].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Globulines Totales_UNIT` = " + (dataGridView1.Rows[6].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[6].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Coefficient A/G_UNIT` = " + (dataGridView1.Rows[7].Cells["UNIT2"].Value != DBNull.Value ? "'" + dataGridView1.Rows[7].Cells["UNIT2"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Protéines Totales_NORMATIF` = " + (dataGridView1.Rows[0].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[0].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Albumine_NORMATIF` = " + (dataGridView1.Rows[1].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[1].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Alpha-1-Globulines_NORMATIF` = " + (dataGridView1.Rows[2].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[2].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Alpha-2-Globulines_NORMATIF` = " + (dataGridView1.Rows[3].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[3].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Beta-Globulines_NORMATIF` = " + (dataGridView1.Rows[4].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[4].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Gamma-Globulines_NORMATIF` = " + (dataGridView1.Rows[5].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[5].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Globulines Totales_NORMATIF` = " + (dataGridView1.Rows[6].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[6].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL") + ","
                                              + "`Coefficient A/G_NORMATIF` = " + (dataGridView1.Rows[7].Cells["DEFAULT_FULL"].Value != DBNull.Value ? "'" + dataGridView1.Rows[7].Cells["DEFAULT_FULL"].Value.ToString().Replace("'", "''") + "'" : "NULL")
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
                textBox3.Text = ref_tmp = "PROT_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
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
                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_labo_proteinogramme WHERE ID = " + dataGridView2.SelectedRows[0].Cells["ID"].Value + ";");
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
                        
                        dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                        for (int f = 14; f < 22; f++)
                        {
                            dataGridView1.Rows[f - 14].Cells["UNIT2"].Value = dt.Rows[0][f];
                        }
                        dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
                        for (int f = 6; f < 14; f++)
                        {
                            dataGridView1.Rows[f - 6].Cells["VALUE2"].Value = dt.Rows[0][f];
                        }
                        cbx_tmp = false;
                        comboBox1.SelectedIndex = (int)dt.Rows[0]["Index_Normatif_De"];
                        cbx_tmp = true;
                        for (int f = 22; f < 30; f++)
                        {
                            dataGridView1.Rows[f - 22].Cells["DEFAULT_FULL"].Value = dt.Rows[0][f];
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
                    PreConnection.Excut_Cmd("DELETE FROM tb_labo_proteinogramme WHERE ID IN (" + dq + ");");
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
                    dt.Rows.Add(new object[] { "HEM_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["VALUE2"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["VALUE2"].Value.ToString() : "" });
                    dt.Rows.Add(new object[] { "HEM2_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["DEFAULT_FULL"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["DEFAULT_FULL"].Value.ToString() : "" });
                    dt.Rows.Add(new object[] { "HEM3_0" + (i + 1).ToString("D2"), dataGridView1.Rows[i].Cells["UNIT2"].Value != DBNull.Value ? dataGridView1.Rows[i].Cells["UNIT2"].Value.ToString() : "" });
                }
                //-------------
                new Print_report("proteinogramme", dt, null).ShowDialog();
            }

        }

        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (cbx_tmp)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1_CellValueChanged(null, new DataGridViewCellEventArgs(dataGridView1.Columns["UNIT2"].Index, i));
                }
            }          
            //----------------
            dataGridView1.Refresh();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {                
                if ((!comboBox1.Visible || (comboBox1.Visible && comboBox1.SelectedIndex == 1)) && e.ColumnIndex == dataGridView1.Columns["UNIT2"].Index)
                {                   
                    dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                    switch (e.RowIndex)
                    {
                        
                        case 0:
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 5.2 : 5.2;
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 7.6 : 7.6;
                            break;
                        case 1:
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 60 : 2.87;
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 66 : 4.76;
                            break;
                        case 2:
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 5.9 : 0.15;
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 9.8 : 0.42;
                            break;
                        case 3:
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 9 : 0.44;
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 15.2 : 1.21;
                            break;
                        case 4:
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 6.8 : 0.72;
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 7.4 : 1.80;
                            break;
                        case 5:
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 7 : 0.28;
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 13.3 : 1.57;
                            break;
                        case 6:
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 0 : 2.06;
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 0 : 5.06;
                            break;
                        case 7:
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 0.62 : 0.74;
                            dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value = dataGridView1.Rows[e.RowIndex].Cells["UNIT2"].Value.ToString().Equals("g/dl") ? 1.28 : 1.92;
                            break;
                    }
                    //--------------
                    string val = (dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value != DBNull.Value ? dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MIN2"].Value.ToString() + "-" : "inf ") + (dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value != DBNull.Value ? dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_MAX2"].Value.ToString() : "");
                    dataGridView1.Rows[e.RowIndex].Cells["DEFAULT_FULL"].Value = val.Equals("inf ") ? "" : val;

                    dataGridView1.Refresh();
                    dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
                    
                }
            }
            button5.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button5.Visible = false;
        }
    }
}
