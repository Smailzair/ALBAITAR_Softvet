using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Xamarin.Forms.Internals;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Vaccinations : Form
    {
        public static bool theres_changes = false;
        public Vaccinations()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            new Add_Modif_Vaccin(-1).ShowDialog();
        }
        private void Load_Data()
        {
            Main_Frm.Main_Frm_vaccination = PreConnection.Load_data("SELECT * FROM tb_vaccin;");
            dataGridView1.DataSource = Main_Frm.Main_Frm_vaccination;
            apply_filter();
        }
        private void Vaccinations_Load(object sender, EventArgs e)
        {
            Load_Data();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                new Add_Modif_Vaccin((int)dataGridView1.SelectedRows[0].Cells["ID"].Value).ShowDialog();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            button1.Visible = button3.Visible = dataGridView1.SelectedRows.Count > 0;
        }

        private void Vaccinations_Activated(object sender, EventArgs e)
        {
            if (theres_changes)
            {
                theres_changes = false;
                Load_Data();
            }
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
                    Vaccination.make_refresh = true;
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
            string fltr = !string.IsNullOrWhiteSpace(textBox1.Text) ? string.Format("VACCIN_NME LIKE '%{0}%'", textBox1.Text) : "";
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
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


    }
}
