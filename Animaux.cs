using ALBAITAR_Softvet.Dialogs;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Fpe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excc = Microsoft.Office.Interop.Excel;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Animaux : Form
    {
        DataTable clients;
        DataTable animaux;
        List<string> full_nme_clients;
        bool Is_New = true;
        public Animaux()
        {
            InitializeComponent();
            //----------------------
            comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = 0;
            //----------------------
            clients = PreConnection.Load_data_keeping_duplicates("SELECT ID,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            comboBox1.DataSource= clients;            
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";
            if (clients.Rows.Count> 0 ) { comboBox1.SelectedIndex = 0; }
            full_nme_clients= new List<string>();
            clients.Rows.Cast<DataRow>().ToList().ForEach(clt => {
                full_nme_clients.Add((string)clt["FULL_NME"]);
            });
            //---------------------
            Load_anims_from_DB();
            //---------------------
           
        }
        private void Load_anims_from_DB()
        {
            int fd = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : 0;
            animaux = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_animaux;");
            dataGridView1.DataSource = animaux;
            if(dataGridView1.Rows.Count > fd) { dataGridView1.ClearSelection(); dataGridView1.Rows[fd].Selected = true; }

        }
        private void Load_selected_anim_fields()
        {
            openFileDialog1.FileName = "";
            if (dataGridView1.SelectedRows.Count > 0)
            {
                label13.Visible = false;
                textBox2.Validated -= textBox2_Validated;
                textBox3.Validated -= textBox2_Validated;
                comboBox1.Validating -= comboBox1_Validating;
                comboBox2.SelectedIndexChanged-= comboBox2_SelectedIndexChanged;
                pictureBox1.Image = Properties.Resources.MODIF;
                pictureBox2.Image = null;    
                button7.Visible = false;
                //----------------------------------------------
                textBox3.Text = (string)dataGridView1.SelectedRows[0].Cells["NME"].Value;
                textBox2.Text = (string)dataGridView1.SelectedRows[0].Cells["NUM_IDENTIF"].Value;
                textBox4.Text = (string)dataGridView1.SelectedRows[0].Cells["NUM_PASSPORT"].Value;
                comboBox1.SelectedValue = (int)dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value;
                comboBox2.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["ESPECE"].Value;
                comboBox3.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["RACE"].Value;
                comboBox4.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["SEXE"].Value;
                dateTimePicker1.Value = dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value != DBNull.Value ? (DateTime)dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value : DateTime.Now.Date;
                textBox6.Text = (string)dataGridView1.SelectedRows[0].Cells["ROBE"].Value;
                textBox8.Text = (string)dataGridView1.SelectedRows[0].Cells["OBSERVATIONS"].Value;
                checkBox1.Checked = (SByte)dataGridView1.SelectedRows[0].Cells["IS_RADIATED"].Value != 0;
                dateTimePicker2.Value = dataGridView1.SelectedRows[0].Cells["RADIATION_DATE"].Value != DBNull.Value ? (DateTime)dataGridView1.SelectedRows[0].Cells["RADIATION_DATE"].Value : DateTime.Now.Date;
                textBox5.Text = (string)dataGridView1.SelectedRows[0].Cells["RADIATION_CAUSES"].Value;
                pictureBox2.Image = dataGridView1.SelectedRows[0].Cells["picture"].Value != DBNull.Value ? PreConnection.ByteArrayToImage((byte[])dataGridView1.SelectedRows[0].Cells["picture"].Value) : (Image)Properties.Resources.ResourceManager.GetObject(comboBox2.Text);
                button7.Visible = dataGridView1.SelectedRows[0].Cells["picture"].Value != DBNull.Value;
                //----------------------------------------------
                textBox2.Validated += textBox2_Validated;
                textBox3.Validated += textBox2_Validated;
                comboBox1.Validating += comboBox1_Validating;
                comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            }
            else
            {
                button3.PerformClick();
            }

        }
        private void verif_if_déja_exist_animal()
        {
            if (textBox2.Text.Length > 0 && textBox3.Text.Length > 0 && (Is_New || (!Is_New && dataGridView1.SelectedRows.Count > 0)))
            {
                int cnt = animaux.Rows.Cast<DataRow>().Where(zz => zz["NME"].ToString().ToLower().Equals(textBox3.Text.ToLower()) && zz["CLIENT_ID"] == comboBox1.SelectedValue).ToList().Count();
                label13.Visible = cnt > 0;
            }
            else { label13.Visible = false; }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            PreConnection.search_filter_datagridview(dataGridView1, textBox1.Text);
        }




        private void button2_Click(object sender, EventArgs e)
        {
            bool all_ready = true;            
            textBox3.BackColor = textBox3.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
            comboBox1.BackColor = comboBox1.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
            all_ready &= comboBox1.BackColor == SystemColors.Window;
            all_ready &= textBox3.Text.TrimStart().TrimEnd() != string.Empty;
            //-------------
            label12.Visible = !all_ready;
            //-------------
            if (all_ready)
            {
                if (Is_New) //INSERT
                {
                    //byte[] imageData = File.Exists(openFileDialog1.FileName) ? File.ReadAllBytes(openFileDialog1.FileName) : (pictureBox2.Image != null ? PreConnection.ImageToByteArray(pictureBox2.Image) : null);
                    byte[] imageData = File.Exists(openFileDialog1.FileName) ? File.ReadAllBytes(openFileDialog1.FileName) : null;
                    string insert_cmnd = "INSERT INTO `tb_animaux`"
                            + "(`NME`,"
                            + "`NUM_IDENTIF`,"
                            + "`NUM_PASSPORT`,"
                            + "`CLIENT_ID`,"
                            + "`ESPECE`,"
                            + "`RACE`,"
                            + "`SEXE`,"
                            + "`NISS_DATE`,"
                            + "`ROBE`,"
                            + "`OBSERVATIONS`,"
                            + "`IS_RADIATED`,"
                            + "`RADIATION_DATE`,"
                            + "`RADIATION_CAUSES`"
                            + (File.Exists(openFileDialog1.FileName) ? ",`PICTURE`" : "")
                            + ") VALUES "
                            + "('" + textBox3.Text + "',"//<{ NME: }>,
                            + "'" + textBox2.Text + "',"//{ NUM_IDENTIF: }>,
                            + "'" + textBox4.Text + "',"//{ NUM_PASSPORT: }>,
                            + comboBox1.SelectedValue + ","//{ CLIENT_ID: }>,
                            + "'" + comboBox2.Text + "',"//{ ESPECE: }>,
                            + "'" + comboBox3.Text + "',"//{ RACE: }>,
                            + "'" + comboBox4.Text + "',"//{ SEXE: }>,
                            + (dateTimePicker1.Value.Date != DateTime.Now.Date ? ("'" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","//{ NISS_DATE: }>,
                            + "'" + textBox6.Text + "',"//{ ROBE: }>,
                            + "'" + textBox8.Text + "',"//{ OBSERVATIONS: }>,
                            + (checkBox1.Checked ? "TRUE" : "FALSE") + ","//{ IS_RADIATED: 0}>,
                            + (checkBox1.Checked ? ("'" + dateTimePicker2.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","//{ RADIATION_DATE: }>,
                            + "'" + (checkBox1.Checked ? textBox5.Text : "") + "'" //{ RADIATION_CAUSES 
                            + (File.Exists(openFileDialog1.FileName) ? ",@Pic" : "") 
                            + ");";
                    MySqlCommand cmd= new MySqlCommand(insert_cmnd, PreConnection.mySqlConnection);
                    if(File.Exists(openFileDialog1.FileName)) { cmd.Parameters.AddWithValue("@Pic", imageData); }
                    PreConnection.open_conn();
                    cmd.ExecuteNonQuery();
                }
                else //UPDATE
                {
                    byte[] imageData = File.Exists(openFileDialog1.FileName) ? File.ReadAllBytes(openFileDialog1.FileName) : null;
                    string insert_cmnd = "UPDATE `albaitar_db`.`tb_animaux` SET "
                            + "`NME` = '" + textBox3.Text + "',"
                            + "`NUM_IDENTIF` = '" + textBox2.Text + "',"
                            + "`NUM_PASSPORT` = '" + textBox4.Text + "',"
                            + "`CLIENT_ID` = " + comboBox1.SelectedValue + ","
                            + "`ESPECE` = '" + comboBox2.Text + "',"
                            + "`RACE` = '" + comboBox3.Text + "',"
                            + "`SEXE` = '" + comboBox4.Text + "',"
                            + "`NISS_DATE` = " + (dateTimePicker1.Value.Date != DateTime.Now.Date ? ("'" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","
                            + "`ROBE` = '" + textBox6.Text + "',"
                            + "`OBSERVATIONS` = '" + textBox8.Text + "',"
                            + "`IS_RADIATED` = " + (checkBox1.Checked ? "TRUE" : "FALSE") + ","
                            + "`RADIATION_DATE` = " + (checkBox1.Checked ? ("'" + dateTimePicker2.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","
                            + "`RADIATION_CAUSES` = '" + (checkBox1.Checked ? textBox5.Text : "") + "'"
                            + (File.Exists(openFileDialog1.FileName) ? ",`PICTURE` = @Pic" : (!button7.Visible ? ",`PICTURE` = NULL" : ""))
                            + " WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + ";";
                    MySqlCommand cmd = new MySqlCommand(insert_cmnd, PreConnection.mySqlConnection);
                    if (File.Exists(openFileDialog1.FileName)) { cmd.Parameters.AddWithValue("@Pic", imageData); }
                    PreConnection.open_conn();
                    cmd.ExecuteNonQuery();
                }
                //----------------
                Load_anims_from_DB();
            }

        }

        private void label12_VisibleChanged(object sender, EventArgs e)
        {
            if (label12.Visible)
            {
                Timer tmr = new Timer();
                tmr.Interval = 1000;
                tmr.Tick += Tmr_Tick;
                tmr.Start();
            }
        }

        int timm = 0;
        private void Tmr_Tick(object sender, EventArgs e)
        {
            timm++;
            if (timm >= 3)
            {
                label12.Visible = false;
                timm = 0;
                ((Timer)sender).Stop();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
            label13.Visible = false;
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
                Is_New = false;
                Load_selected_anim_fields();            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            openFileDialog1.FileName = "";
            pictureBox2.Image = null;
            Is_New = true;
            foreach(Control ctrl in splitContainer1.Panel2.Controls)
            {                
                if(ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(MaskedTextBox) || (ctrl.GetType() == typeof(ComboBox) && ((ComboBox)ctrl).DropDownStyle != ComboBoxStyle.DropDownList))
                {
                    ctrl.Text = string.Empty;
                }else if (ctrl.GetType() == typeof(ComboBox) && ((ComboBox)ctrl).DropDownStyle == ComboBoxStyle.DropDownList){
                    ((ComboBox)ctrl).SelectedIndex= 0;
                }else if (ctrl.GetType() == typeof(DateTimePicker))
                {
                    ((DateTimePicker)ctrl).Value = DateTime.Now;
                }
            }
            label13.Visible=false;
            pictureBox1.Image = Properties.Resources.NOUVEAU;
            if (!textBox1.Focused) { textBox3.Select(); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string fff = "";
            if (dataGridView1.SelectedRows.Count > 0)
            {

                dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row =>
                {
                    fff += "," + row.Cells["ID"].Value;
                });

                fff = fff.Substring(1);
                if (MessageBox.Show("Vous étes sures de supprimer [" + dataGridView1.SelectedRows.Count + "] animaux ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PreConnection.Excut_Cmd("DELETE FROM tb_animaux WHERE ID IN (" + fff + ");");
                    Load_anims_from_DB();
                }

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count > 0)
            {
                Excc.Application xcelApp = new Excc.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Animaux";
                xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Animaux";
                dataGridView1.Columns.Cast<DataGridViewColumn>().Where(ss => ss.Name != "ID" && ss.Name != "IS_RADIATED" && ss.Name != "PICTURE").ToList().ForEach(g =>
                {
                    switch (g.HeaderText)
                    {
                        case "NME":
                            xcelApp.Cells[1, g.Index + 1].Value = "Nom";
                            break;
                        case "NUM_IDENTIF":
                            xcelApp.Cells[1, g.Index + 1].Value = "N° Ident.";
                            break;
                        case "NUM_PASSPORT":
                            xcelApp.Cells[1, g.Index + 1].Value = "N° Passport";
                            break;
                        case "CLIENT_ID":
                            xcelApp.Cells[1, g.Index + 1].Value = "Propriétaire";
                            break;
                        case "ESPECE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Espéce";
                            break;
                        case "RACE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Race";
                            break;
                        case "SEXE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Sexe";
                            break;
                        case "NISS_DATE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Date Nissance";
                            break;
                        case "ROBE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Robe";
                            break;
                        case "OBSERVATIONS":
                            xcelApp.Cells[1, g.Index + 1].Value = "Observations";
                            break;
                        case "RADIATION_DATE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Date Radiation";
                            break;
                        case "RADIATION_CAUSES":
                            xcelApp.Cells[1, g.Index + 1].Value = "Causes Radiation";
                            break;
                    }
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                    try
                    {
                        if (dataGridView1.Columns[g.Index].DefaultCellStyle.Format == "N2")
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "#,##0.00 [$Da-fr-dz]";
                        }
                        else if (dataGridView1.Columns[g.Index].DefaultCellStyle.Format.Contains("MM/yyyy"))
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "dd/MM/yyyy";
                        }
                    }
                    catch { }
                });

                dataGridView1.Rows.Cast<DataGridViewRow>().ToList().ForEach(t =>
                {
                    t.Cells.Cast<DataGridViewCell>().ToList().ForEach(b =>
                    {
                        if(xcelApp.Cells[1, b.ColumnIndex + 1].Value == "Propriétaire")
                        {
                            xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? (((DataRow)clients.AsEnumerable().FirstOrDefault(row => row.Field<int>("ID") == (int)dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value))["FULL_NME"]) : "";
                        }
                        else
                        {
                            xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").Replace("00:00:00","").TrimStart().TrimEnd() : "";
                        }
                        
                    });

                });
                xcelApp.Columns[dataGridView1.Columns["ID"].Index + 1].Delete();
                xcelApp.Columns[dataGridView1.Columns["IS_RADIATED"].Index].Delete();
                xcelApp.Columns[dataGridView1.Columns["PICTURE"].Index - 1].Delete();
                xcelApp.Columns.AutoFit();
                //------------------
                SaveFileDialog svd = new SaveFileDialog();
                svd.Filter = "Excel | *.xlsx";
                svd.DefaultExt = "*.xlsx";
                svd.FileName = xcelApp.Application.Workbooks[1].Title + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                if (svd.ShowDialog() == DialogResult.OK)
                {
                    xcelApp.Workbooks[1].SaveAs(Path.GetFullPath(svd.FileName));
                    Process.Start(Path.GetFullPath(svd.FileName));
                }
                xcelApp.Application.Workbooks[1].Close(false);
                xcelApp.Quit();
                //-------------------
            }
            else
            {
                MessageBox.Show("Aucun donnés !", ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = checkBox1.Checked;
            //------------------------------
            if (checkBox1.Checked) {
                checkBox1.Location = new Point(groupBox1.Location.X + 160, groupBox1.Location.Y - 20);
                checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            }
            else
            {                
                checkBox1.Location = new Point(14, pictureBox1.Location.Y - 20);
                checkBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Small_New_Client().ShowDialog();

            comboBox1.Validating -= comboBox1_Validating;
            clients = PreConnection.Load_data_keeping_duplicates("SELECT ID,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            comboBox1.DataSource = clients;            
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";
            if(clients.Rows.Count > 0) { comboBox1.SelectedValue = (int)clients.AsEnumerable().Max(row => row.Field<int>("ID")); }
            full_nme_clients.Clear();
            clients.Rows.Cast<DataRow>().ToList().ForEach(clt => {
                full_nme_clients.Add((string)clt["FULL_NME"]);
            });
            comboBox1.Validating += comboBox1_Validating;
        }

        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {            
            if (comboBox1.Text.Length  > 0 && !full_nme_clients.Contains(comboBox1.Text)) { 
                comboBox1.BackColor= Color.LightCoral;
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (!button7.Visible)
            {
                pictureBox2.Image = (Image)Properties.Resources.ResourceManager.GetObject(comboBox2.Text);
            }        
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
            button7.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            openFileDialog1.FileName= "";          
            button7.Visible= false;
            comboBox2_SelectedIndexChanged(null, null);
        }
    }
}
