using ALBAITAR_Softvet.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Xamarin.Forms.Internals;

namespace ALBAITAR_Softvet
{
    public partial class Vaccination : UserControl
    {
        public static int selected_anim_id = -1;
        public static int selected_client_id = -1;
        public static bool make_refresh = false;

        int prev_selected_rbx = 0; //0 -> Tous  1 -> Animal  2 -> Propr
        //----------
        DataTable chosen_anim_from_search;
        DataTable chosen_client_from_search;


        int selected_anim_id_for_filter = -1;
        string selected_anim_ident_for_filter = "";
        int selected_client_id_for_filter = -1;

        public Vaccination()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        private void Vaccination_Load(object sender, EventArgs e)
        {
            dateTimePicker1.ValueChanged -= dateTimePicker1_ValueChanged;
            dateTimePicker1.Value = DateTime.Now.AddDays(-15);
            dateTimePicker2.Value = DateTime.Now.AddDays(15);
            dateTimePicker2.MinDate = dateTimePicker1.Value;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            dateTimePicker2.ValueChanged += dateTimePicker1_ValueChanged;
            //-----------
            if (selected_anim_id > 0)
            {
                selected_anim_id_for_filter = selected_anim_id;
                selected_anim_id = -1;

                var ff = Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(D => (int)D["ID"] == selected_anim_id_for_filter);
                if (ff.Any())
                {
                    label4.Text = (string)ff.First()["NME"];
                    selected_anim_ident_for_filter = (string)ff.First()["NUM_IDENTIF"];
                    radioButton9.CheckedChanged -= radioButton9_CheckedChanged;
                    radioButton9.Checked = true;
                    radioButton9.CheckedChanged += radioButton9_CheckedChanged;
                    prev_selected_rbx = 1; //0 -> Tous  1 -> Animal  2 -> Propr
                }
                else
                {
                    label4.Text = "--";
                    selected_anim_ident_for_filter = "";
                    radioButton11.CheckedChanged -= radioButton6_CheckedChanged;
                    radioButton11.Checked = true;
                    radioButton11.CheckedChanged += radioButton6_CheckedChanged;
                    prev_selected_rbx = 0; //0 -> Tous  1 -> Animal  2 -> Propr
                }

            }
            else if (selected_client_id > 0)
            {
                selected_client_id_for_filter = selected_client_id;
                selected_client_id = -1;

                var ff = Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Where(D => (int)D["ID"] == selected_client_id_for_filter);
                if (ff.Any())
                {
                    label5.Text = (string)ff.First()["FULL_NME"];
                    radioButton10.CheckedChanged -= radioButton10_CheckedChanged;
                    radioButton10.Checked = true;
                    radioButton10.CheckedChanged += radioButton10_CheckedChanged;
                    prev_selected_rbx = 2; //0 -> Tous  1 -> Animal  2 -> Propr
                }
                else
                {
                    label5.Text = "--";
                    radioButton11.CheckedChanged -= radioButton6_CheckedChanged;
                    radioButton11.Checked = true;
                    radioButton11.CheckedChanged += radioButton6_CheckedChanged;
                    prev_selected_rbx = 0; //0 -> Tous  1 -> Animal  2 -> Propr
                }
            }
            else
            {
                radioButton11.CheckedChanged -= radioButton6_CheckedChanged;
                radioButton11.Checked = true;
                radioButton11.CheckedChanged += radioButton6_CheckedChanged;
                prev_selected_rbx = 0; //0 -> Tous  1 -> Animal  2 -> Propr
            }
            //----------
            Load_Data();
        }
        private void Load_Data()
        {
            Main_Frm.Main_Frm_vaccination = PreConnection.Load_data("SELECT *," +
                "(concat(EVERY_DAY_NB ,' ',"
+ "CASE "
+ "WHEN EVERY_MOUNTH_NB = 1 THEN 'Janvier'"
+ "WHEN EVERY_MOUNTH_NB = 2 THEN 'Février'"
+ "WHEN EVERY_MOUNTH_NB = 3 THEN 'Mars'"
+ "WHEN EVERY_MOUNTH_NB = 4 THEN 'Avril'"
+ "WHEN EVERY_MOUNTH_NB = 5 THEN 'Mai'"
+ "WHEN EVERY_MOUNTH_NB = 6 THEN 'Juin'"
+ "WHEN EVERY_MOUNTH_NB = 7 THEN 'Juillet'"
+ "WHEN EVERY_MOUNTH_NB = 8 THEN 'Aout'"
+ "WHEN EVERY_MOUNTH_NB = 9 THEN 'Séptembre'"
+ "WHEN EVERY_MOUNTH_NB = 10 THEN 'Octobre'"
+ "WHEN EVERY_MOUNTH_NB = 11 THEN 'Novembre'"
+ "WHEN EVERY_MOUNTH_NB = 12 THEN 'Décembre' "
+ "ELSE ''"
+ "  END)) AS EVERY_TXT,"
+ "     IF("
+ "         FIXED_DATE >= CURRENT_DATE, "
+ "         FIXED_DATE,"
+ "         IF("
+ "             CURRENT_DATE BETWEEN START_DATE AND END_DATE,"
+ "             IF("
+ "                 STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d') >= CURRENT_DATE,"
+ "                 STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
+ "                 IF("
+ "                     (YEAR(CURRENT_DATE) + 1) <= END_YEAR,"
+ "                     STR_TO_DATE(CONCAT(CAST((YEAR(CURRENT_DATE) + 1) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
+ "                     NULL"
+ "                 )"
+ "             ),"
+ "             IF(CURRENT_DATE < STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
+ "                 STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
+ "                 NULL"
+ "                 )"  
+ "         )"
+ "     ) AS NEXT_DATE "
+ " FROM tb_vaccin ORDER BY NEXT_DATE;");

            dataGridView1.DataSource = Main_Frm.Main_Frm_vaccination;
            apply_filter();
        }
        private void Vaccination_Enter(object sender, EventArgs e)
        {
            if (make_refresh)
            {
                make_refresh = false;
                //-----------
                if (selected_anim_id > 0 && selected_anim_id != selected_anim_id_for_filter)
                {
                    selected_anim_id_for_filter = selected_anim_id;
                    selected_anim_id = -1;

                    var ff = Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(D => (int)D["ID"] == selected_anim_id_for_filter);
                    if (ff.Any())
                    {
                        label4.Text = (string)ff.First()["NME"];
                        selected_anim_ident_for_filter = (string)ff.First()["NUM_IDENTIF"];
                        radioButton9.CheckedChanged -= radioButton9_CheckedChanged;
                        radioButton9.Checked = true;
                        radioButton9.CheckedChanged += radioButton9_CheckedChanged;
                    }
                    else
                    {
                        label4.Text = "--";
                        selected_anim_ident_for_filter = "";
                        radioButton11.CheckedChanged -= radioButton6_CheckedChanged;
                        radioButton11.Checked = true;
                        radioButton11.CheckedChanged += radioButton6_CheckedChanged;
                    }

                }
                else if (selected_client_id > 0 && selected_client_id != selected_client_id_for_filter)
                {
                    selected_client_id_for_filter = selected_client_id;
                    selected_client_id = -1;

                    var ff = Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Where(D => (int)D["ID"] == selected_client_id_for_filter);
                    if (ff.Any())
                    {
                        label5.Text = (string)ff.First()["FULL_NME"];
                        radioButton10.CheckedChanged -= radioButton10_CheckedChanged;
                        radioButton10.Checked = true;
                        radioButton10.CheckedChanged += radioButton10_CheckedChanged;
                    }
                    else
                    {
                        label5.Text = "--";
                        radioButton11.CheckedChanged -= radioButton6_CheckedChanged;
                        radioButton11.Checked = true;
                        radioButton11.CheckedChanged += radioButton6_CheckedChanged;
                    }
                }
                else
                {
                    radioButton11.CheckedChanged -= radioButton6_CheckedChanged;
                    radioButton11.Checked = true;
                    radioButton11.CheckedChanged += radioButton6_CheckedChanged;
                }
                //----------
                Load_Data();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            new Add_Modif_Vaccin(-1).ShowDialog();
            Load_Data();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                new Add_Modif_Vaccin((int)dataGridView1.SelectedRows[0].Cells["ID"].Value).ShowDialog();
                Load_Data();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            button1.Visible = button3.Visible = dataGridView1.SelectedRows.Count > 0;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button3.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Sûres de faire la suppression ? \n(" + dataGridView1.SelectedRows.Count + " vaccination)", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string idds = "";
                    dataGridView1.SelectedRows.Cast<DataGridViewRow>().ForEach(row => { idds += "," + row.Cells["ID"].Value.ToString(); });
                    idds = idds.Substring(1);
                    PreConnection.Excut_Cmd(3, "tb_vaccin", null, null, "ID IN (@IDDs)", new List<string> { "IDDs" }, new List<object> { idds });
                    Load_Data();
                    Vaccinations.theres_changes = true;
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!dataGridView1.IsCurrentCellInEditMode)
            {
                button1.PerformClick();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            apply_filter();
        }
        private void apply_filter()
        {
            if (dataGridView1.DataSource != null)
            {
                if(((DataTable)dataGridView1.DataSource).Columns.Count > 0)
                {
                    string fltr = !string.IsNullOrWhiteSpace(textBox1.Text) ? string.Format("VACCIN_NME LIKE '%{0}%'", textBox1.Text) : "";
                    fltr += radioButton9.Checked ? (fltr.Length > 0 ? " AND " : "") + $"(IS_FOR_ALL = 1 OR ANIM_NUM_IDENs LIKE '%{selected_anim_ident_for_filter}%')" : "";
                    if (radioButton10.Checked)
                    {
                        string tmmp_anms = "";
                        Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(Z => (Z["CLIENT_ID"] != DBNull.Value ? (int)Z["CLIENT_ID"] : -1) == selected_client_id_for_filter).ForEach(V => tmmp_anms += ",'" + V["NUM_IDENTIF"] + "'");
                        tmmp_anms = tmmp_anms.Length > 0 ? tmmp_anms.Substring(1) : "";
                        fltr += radioButton10.Checked ? (fltr.Length > 0 ? " AND " : "") + $"(IS_FOR_ALL = 1 OR RELATED_CLIENTS_IDS LIKE '{selected_client_id_for_filter},%' OR RELATED_CLIENTS_IDS LIKE '%,{selected_client_id_for_filter},%' OR RELATED_CLIENTS_IDS LIKE '%,{selected_client_id_for_filter}'" + (!string.IsNullOrEmpty(tmmp_anms) ? $" OR ANIM_NUM_IDENs IN ({tmmp_anms})" : "") + ")" : "";
                    }
                    fltr += !radioButton3.Checked ? (fltr.Length > 0 ? " AND " : "") + "IS_IMPORTANT = '" + (radioButton1.Checked ? "Oui" : "Non") + "'" : "";
                    fltr += !radioButton4.Checked ? (fltr.Length > 0 ? " AND " : "") + "FIXED_DATE IS " + (radioButton5.Checked ? "NOT " : "") + "NULL" : "";
                    if (checkBox1.Checked)
                    {
                        fltr += (fltr.Length > 0 ? " AND " : "");
                        fltr += $"((FIXED_DATE IS NOT NULL AND FIXED_DATE >= #{dateTimePicker1.Value.ToString("MM/dd/yyyy")}# AND FIXED_DATE <= #{dateTimePicker2.Value.ToString("MM/dd/yyyy")}#) OR " +
                            $"(FIXED_DATE IS NULL AND (" +
                            $"(START_DATE >= #{dateTimePicker1.Value.ToString("MM/dd/yyyy")}# AND START_DATE <= #{dateTimePicker2.Value.ToString("MM/dd/yyyy")}#) OR " +
                            $"(END_DATE >= #{dateTimePicker1.Value.ToString("MM/dd/yyyy")}# AND END_DATE <= #{dateTimePicker2.Value.ToString("MM/dd/yyyy")}#)" +
                            $")))";
                    }

                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = fltr;
                }
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                apply_filter();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.ValueChanged -= dateTimePicker1_ValueChanged;
            dateTimePicker2.MinDate = dateTimePicker1.Value;
            dateTimePicker2.ValueChanged += dateTimePicker1_ValueChanged;
            apply_filter();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Enabled = checkBox1.Checked;
            apply_filter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chosen_anim_from_search = new DataTable();
            Anims_List_Search select = new Anims_List_Search();
            select.DataTableReturned += ChildForm_DataTableReturned;
            select.ShowDialog();
            if (chosen_anim_from_search != null)
            {
                selected_anim_id_for_filter = int.Parse(chosen_anim_from_search.Rows[0][1].ToString());
                selected_anim_ident_for_filter = (string)chosen_anim_from_search.Rows[0][4];
                label4.Text = (string)chosen_anim_from_search.Rows[0][0];
                if (!radioButton9.Checked) { radioButton9.Checked = true; }
                prev_selected_rbx = 1; //0 -> Tous  1 -> Animal  2 -> Propr
                apply_filter();
            }
            else
            {
                switch (prev_selected_rbx)
                {
                    case 1:
                        radioButton9.CheckedChanged -= radioButton9_CheckedChanged;
                        radioButton9.Checked = true;
                        radioButton9.CheckedChanged += radioButton9_CheckedChanged;
                        break;
                    case 2:
                        radioButton10.CheckedChanged -= radioButton10_CheckedChanged;
                        radioButton10.Checked = true;
                        radioButton10.CheckedChanged += radioButton10_CheckedChanged;
                        break;
                    case 0:
                        radioButton11.CheckedChanged -= radioButton6_CheckedChanged;
                        radioButton11.Checked = true;
                        radioButton11.CheckedChanged += radioButton6_CheckedChanged;
                        break;
                }
            }
        }

        private void ChildForm_DataTableReturned(object sender, DataTableEventArgs e)
        {
            chosen_anim_from_search = e.DataTable;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chosen_client_from_search = new DataTable();
            Clients_List_Search select = new Clients_List_Search();
            select.DataTableReturned += Select_DataTableReturned;
            select.ShowDialog();
            if (chosen_client_from_search != null)
            {
                selected_client_id_for_filter = int.Parse(chosen_client_from_search.Rows[0][0].ToString());
                label5.Text = (string)chosen_client_from_search.Rows[0][1];
                if (!radioButton10.Checked) { radioButton10.Checked = true; }
                prev_selected_rbx = 2; //0 -> Tous  1 -> Animal  2 -> Propr
                apply_filter();
            }
            else
            {
                switch (prev_selected_rbx)
                {
                    case 1:
                        radioButton9.CheckedChanged -= radioButton9_CheckedChanged;
                        radioButton9.Checked = true;
                        radioButton9.CheckedChanged += radioButton9_CheckedChanged;
                        break;
                    case 2:
                        radioButton10.CheckedChanged -= radioButton10_CheckedChanged;
                        radioButton10.Checked = true;
                        radioButton10.CheckedChanged += radioButton10_CheckedChanged;
                        break;
                    case 0:
                        radioButton11.CheckedChanged -= radioButton6_CheckedChanged;
                        radioButton11.Checked = true;
                        radioButton11.CheckedChanged += radioButton6_CheckedChanged;
                        break;
                }
            }
        }

        private void Select_DataTableReturned(object sender, DataTableEventArgs_Clients e)
        {
            chosen_client_from_search = e.DataTable;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
            {
                if (selected_anim_id_for_filter <= 0)
                {
                    button2.PerformClick();
                }
                else
                {
                    apply_filter();
                }
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
            {
                if (selected_client_id_for_filter <= 0)
                {
                    button4.PerformClick();
                }
                else
                {
                    apply_filter();
                }
            }

        }
    }
}
