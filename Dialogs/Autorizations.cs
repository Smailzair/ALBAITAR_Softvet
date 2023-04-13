using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Autorizations : Form
    {
        DataTable Autoriz_data;
        DataTable Users;
        DataTable tmp_db;
        public Autorizations()
        {
            InitializeComponent();
            //----------------------
            dataGridView1.RowHeadersWidth = 10;
            //---------------
            tmp_db = new DataTable();
            tmp_db.Columns.Add("ID", typeof(int));
            tmp_db.Columns.Add("CODE", typeof(int));
            tmp_db.Columns.Add("AUTOR_TEXT", typeof(string));
            tmp_db.Columns.Add("VALUES", typeof(SByte));

        }

        private void Autorizations_Load(object sender, System.EventArgs e)
        {
            //----------------------
            Autoriz_data = PreConnection.Load_data("SELECT * FROM tb_autoriz;");

            dataGridView1.DataSource = Autoriz_data;
            //-----------------------------
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            Users = PreConnection.Load_data("SELECT 0 AS ID, '- Par défaut -     (Ou de nouveaux utilisateurs)' AS USER_FULL_NME, 0 AS IS_ADMIN UNION ALL SELECT ID ,CONCAT(IF(SEX = 'F','Mme. ','Mr. '),`USER_NME`,' ',`USER_FAMNME`) AS USER_FULL_NME,IS_ADMIN FROM tb_login_and_users WHERE `IS_ADMIN` = 0;");
            comboBox1.DataSource = Users;
            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "USER_FULL_NME";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.SelectedValue = !Properties.Settings.Default.Last_login_is_admin ? Properties.Settings.Default.Last_login_user_idx : 0;
            comboBox1_SelectedIndexChanged(null,null);


        }
        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dataGridView1.Visible = true;
            tmp_db.Rows.Clear();

            if (comboBox1.SelectedIndex > 0)
            {
                foreach (DataRow row in Autoriz_data.Rows)
                {

                    DataRow newRow = tmp_db.NewRow();

                    newRow["ID"] = row["ID"];
                    newRow["CODE"] = row["CODE"];
                    newRow["AUTOR_TEXT"] = row["AUTOR_TEXT"];
                    newRow["VALUES"] = row["Usr_" + comboBox1.SelectedValue.ToString()];

                    tmp_db.Rows.Add(newRow);
                }
            }
            else
            {
                foreach (DataRow row in Autoriz_data.Rows)
                {
                    DataRow newRow = tmp_db.NewRow();

                    newRow["ID"] = row["ID"];
                    newRow["CODE"] = row["CODE"];
                    newRow["AUTOR_TEXT"] = row["AUTOR_TEXT"];
                    newRow["VALUES"] = row["DEFAULT_VALUES"];

                    tmp_db.Rows.Add(newRow);
                }
            }
            dataGridView1.DataSource = tmp_db;
            button2.Visible = comboBox1.SelectedIndex > 0;
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells["VALUES"].Value != null)
            {
                // Get the value of the cell
                var value = dataGridView1.Rows[e.RowIndex].Cells["VALUES"].Value.ToString();

                // Set the background color of the row based on the cell value
                if (value == "0")
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView1.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.FromArgb(255, 192, 192);

                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView1.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.FromArgb(192, 255, 192);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            PreConnection.search_filter_datagridview(dataGridView1, textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rwss = dataGridView1.SelectedRows;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                PreConnection.Excut_Cmd("UPDATE tb_autoriz SET Usr_" + comboBox1.SelectedValue + " = 1 WHERE CODE = " + row.Cells["CODE"].Value);
                row.Cells["VALUES"].Value = 1;
            }
            dataGridView1.ClearSelection();
            dataGridView1.Refresh();
            foreach (DataGridViewRow row in rwss)
            {
                row.Selected = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rwss = dataGridView1.SelectedRows;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                PreConnection.Excut_Cmd("UPDATE tb_autoriz SET Usr_" + comboBox1.SelectedValue + " = 0 WHERE CODE = " + row.Cells["CODE"].Value);
                row.Cells["VALUES"].Value = 0;
            }
            dataGridView1.ClearSelection();
            dataGridView1.Refresh();
            foreach (DataGridViewRow row in rwss)
            {
                row.Selected = true;
            }

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (e.ColumnIndex == dataGridView1.Columns["VALUES"].Index)
                {
                    dataGridView1.Rows[e.RowIndex].HeaderCell.Style.BackColor = dataGridView1.Rows[e.RowIndex].Cells["VALUES"].Value.ToString() == "0" ? Color.FromArgb(255, 192, 192) : Color.FromArgb(192, 255, 192);
                }
            }
            dataGridView1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rwss = dataGridView1.SelectedRows;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int default_val = int.Parse(Autoriz_data.Rows.Cast<DataRow>().FirstOrDefault(x => x["CODE"].ToString() == row.Cells["CODE"].Value.ToString())["DEFAULT_VALUES"].ToString());
                PreConnection.Excut_Cmd("UPDATE tb_autoriz SET Usr_" + comboBox1.SelectedValue + " = "+ default_val + " WHERE CODE = " + row.Cells["CODE"].Value +";");
                row.Cells["VALUES"].Value = default_val;
            }
            dataGridView1.ClearSelection();
            dataGridView1.Refresh();
            foreach (DataGridViewRow row in rwss)
            {
                row.Selected = true;
            }
        }
    }
}
