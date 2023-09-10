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

        bool Theres_changes = false;

        bool Consulter_autors_d_autres_92001, Modifier_ses_autor_92002, Modifier_autor_d_autres_92003 = false;
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
            tmp_db.Columns.Add("FORM_NME", typeof(string));
            tmp_db.Columns.Add("VALUES", typeof(SByte));
            //--------------------
            if(Properties.Settings.Default.Last_login_is_admin)
            {
                Consulter_autors_d_autres_92001 =Modifier_ses_autor_92002 = Modifier_autor_d_autres_92003 = true;
            }
            else
            {
                Consulter_autors_d_autres_92001 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92001" && (Int32)QQ[3] == 1).Count() > 0;
                Modifier_ses_autor_92002 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92002" && (Int32)QQ[3] == 1).Count() > 0;
                Modifier_autor_d_autres_92003 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92003" && (Int32)QQ[3] == 1).Count() > 0;                
            }
            button2.Visible = comboBox1.SelectedIndex > 0;
        }

        private void Autorizations_Load(object sender, System.EventArgs e)
        {
            //----------------------
            Autoriz_data = PreConnection.Load_data("SELECT * FROM tb_autoriz WHERE LENGTH(TRIM(`AUTOR_TEXT`)) > 0 ORDER BY `CODE`;");

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
            Autoriz_data = PreConnection.Load_data("SELECT * FROM tb_autoriz WHERE LENGTH(TRIM(`AUTOR_TEXT`)) > 0 ORDER BY `CODE`;");
            //----------
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
                    newRow["FORM_NME"] = row["FORM_NME"];
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
                    newRow["FORM_NME"] = row["FORM_NME"];
                    newRow["VALUES"] = row["DEFAULT_VALUES"];

                    tmp_db.Rows.Add(newRow);
                }
            }
            //---------
            dataGridView1.DataSource = tmp_db;            
            //------------
            if(comboBox1.SelectedValue.ToString() == Properties.Settings.Default.Last_login_user_idx.ToString())
            {
                dataGridView1.Visible = true;
                button1.Visible = button2.Visible = button3.Visible = Modifier_ses_autor_92002;
            }
            else
            {
                dataGridView1.Visible = Consulter_autors_d_autres_92001 || Modifier_autor_d_autres_92003;
                button1.Visible = button2.Visible = button3.Visible = Modifier_autor_d_autres_92003;
            }
            button2.Visible = comboBox1.SelectedIndex > 0;
            //---------
            textBox1.Clear();


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
            PreConnection.search_filter_datagridview(dataGridView1, textBox1.Text.Replace("'", "''"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Theres_changes = true;
            DataGridViewSelectedRowCollection rwss = dataGridView1.SelectedRows;
            bool tmmmmp = false;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (Properties.Settings.Default.Last_login_is_admin)
                {
                    PreConnection.Excut_Cmd("UPDATE tb_autoriz SET " + (comboBox1.SelectedValue.ToString() != "0" ? "Usr_" + comboBox1.SelectedValue.ToString().Replace("'", "''") : "DEFAULT_VALUES") + " = 1 WHERE CODE = " + row.Cells["CODE"].Value);
                    row.Cells["VALUES"].Value = 1;
                }
                else
                {
                    if ((int)row.Cells["CODE"].Value < 92000)
                    {
                        PreConnection.Excut_Cmd("UPDATE tb_autoriz SET " + (comboBox1.SelectedValue.ToString() != "0" ? "Usr_" + comboBox1.SelectedValue.ToString().Replace("'", "''") : "DEFAULT_VALUES") + " = 1 WHERE CODE = " + row.Cells["CODE"].Value);
                        row.Cells["VALUES"].Value = 1;
                    }
                    else
                    {
                        tmmmmp = true;
                    }
                }    
            }
            if (tmmmmp)
            {
                MessageBox.Show("Vous ne pouvez pas de changer la situation des élements de fenétre 'Autorisations & priviléges',\n\nDemandez ça d'un 'Admin'.\n ","Information :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            Theres_changes = true; 
            DataGridViewSelectedRowCollection rwss = dataGridView1.SelectedRows;
            bool tmmmmp = false;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (Properties.Settings.Default.Last_login_is_admin)
                {
                    PreConnection.Excut_Cmd("UPDATE tb_autoriz SET " + (comboBox1.SelectedValue.ToString() != "0" ? "Usr_" + comboBox1.SelectedValue.ToString().Replace("'", "''") : "DEFAULT_VALUES") + " = 0 WHERE CODE = " + row.Cells["CODE"].Value);
                    row.Cells["VALUES"].Value = 0;
                }
                else
                {
                    if ((int)row.Cells["CODE"].Value < 92000)
                    {
                        PreConnection.Excut_Cmd("UPDATE tb_autoriz SET " + (comboBox1.SelectedValue.ToString() != "0" ? "Usr_" + comboBox1.SelectedValue.ToString().Replace("'", "''") : "DEFAULT_VALUES") + " = 0 WHERE CODE = " + row.Cells["CODE"].Value);
                        row.Cells["VALUES"].Value = 0;
                    }
                    else
                    {
                        tmmmmp = true;
                    }
                }
            }
            if (tmmmmp)
            {
                MessageBox.Show("Vous ne pouvez pas de changer la situation des élements de fenétre 'Autorisations & priviléges',\n\nDemandez ça d'un 'Admin'.\n ", "Information :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            Theres_changes = true;
            DataGridViewSelectedRowCollection rwss = dataGridView1.SelectedRows;
            bool tmmmmp = false;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (Properties.Settings.Default.Last_login_is_admin)
                {
                    int default_val = int.Parse(Autoriz_data.Rows.Cast<DataRow>().FirstOrDefault(x => x["CODE"].ToString() == row.Cells["CODE"].Value.ToString())["DEFAULT_VALUES"].ToString());
                    PreConnection.Excut_Cmd("UPDATE tb_autoriz SET Usr_" + comboBox1.SelectedValue.ToString().Replace("'", "''") + " = " + default_val + " WHERE CODE = " + row.Cells["CODE"].Value + ";");
                    row.Cells["VALUES"].Value = default_val;
                }
                else
                {
                    if ((int)row.Cells["CODE"].Value < 92000)
                    {
                        int default_val = int.Parse(Autoriz_data.Rows.Cast<DataRow>().FirstOrDefault(x => x["CODE"].ToString() == row.Cells["CODE"].Value.ToString())["DEFAULT_VALUES"].ToString());
                        PreConnection.Excut_Cmd("UPDATE tb_autoriz SET Usr_" + comboBox1.SelectedValue.ToString().Replace("'", "''") + " = " + default_val + " WHERE CODE = " + row.Cells["CODE"].Value + ";");
                        row.Cells["VALUES"].Value = default_val;
                    }
                    else
                    {
                        tmmmmp = true;
                    }
                }
            }
            if (tmmmmp)
            {
                MessageBox.Show("Vous ne pouvez pas de changer la situation des élements de fenétre 'Autorisations & priviléges',\n\nDemandez ça d'un 'Admin'.\n ", "Information :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dataGridView1.ClearSelection();
            dataGridView1.Refresh();
            foreach (DataGridViewRow row in rwss)
            {
                row.Selected = true;
            }
        }

        private void Autorizations_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(Theres_changes && !Properties.Settings.Default.Last_login_is_admin)
            {
                Application.Restart();
            }
        }

    }
}
