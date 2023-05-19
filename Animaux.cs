using ALBAITAR_Softvet.Dialogs;
using MySql.Data.MySqlClient;
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
            tabControl1.TabPages.Remove(tabPage2);
            //----------------------
            comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = 0;
            //----------------------
            //clients = PreConnection.Load_data_keeping_duplicates("SELECT ID,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            clients = PreConnection.Load_data_keeping_duplicates("SELECT *,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
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
            int fd = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : 99999999;
            animaux = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_animaux;");
            dataGridView1.DataSource = animaux;
            if(dataGridView1.Rows.Count > fd) 
            { dataGridView1.ClearSelection(); dataGridView1.Rows[fd].Selected = true; }
            else if (dataGridView1.Rows.Count > 0)
            {          dataGridView1.ClearSelection();  dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true; }
            

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
                button9.Visible = false;
                button8.Visible = true;
                panel1.Visible = false;
                panel2.Visible = false;
                if (tabControl1.TabPages.Count < 2) { tabControl1.TabPages.Add(tabPage2); }
                //----------------------------------------------                
                dateTimePicker3.Value = (DateTime)dataGridView1.SelectedRows[0].Cells["DATE_ADDED"].Value;
                textBox3.Text = (string)dataGridView1.SelectedRows[0].Cells["NME"].Value;
                textBox2.Text = (string)dataGridView1.SelectedRows[0].Cells["NUM_IDENTIF"].Value;
                textBox4.Text = (string)dataGridView1.SelectedRows[0].Cells["NUM_PASSPORT"].Value;
                comboBox1.SelectedValue = (int)dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value;
                comboBox2.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["ESPECE"].Value;
                comboBox3.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["RACE"].Value;
                comboBox4.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["SEXE"].Value;
                checkBox2.Checked = dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value != DBNull.Value;
                dateTimePicker1.Value = dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value != DBNull.Value ? (DateTime)dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value : (DateTime)dataGridView1.SelectedRows[0].Cells["DATE_ADDED"].Value;// DateTime.Now.Date;
                textBox6.Text = (string)dataGridView1.SelectedRows[0].Cells["ROBE"].Value;
                textBox8.Text = (string)dataGridView1.SelectedRows[0].Cells["OBSERVATIONS"].Value;
                checkBox1.Checked = (SByte)dataGridView1.SelectedRows[0].Cells["IS_RADIATED"].Value != 0;
                dateTimePicker2.Value = dataGridView1.SelectedRows[0].Cells["RADIATION_DATE"].Value != DBNull.Value ? (DateTime)dataGridView1.SelectedRows[0].Cells["RADIATION_DATE"].Value : (DateTime)dataGridView1.SelectedRows[0].Cells["DATE_ADDED"].Value;// DateTime.Now.Date;
                textBox5.Text = (string)dataGridView1.SelectedRows[0].Cells["RADIATION_CAUSES"].Value;
                pictureBox2.Image = dataGridView1.SelectedRows[0].Cells["picture"].Value != DBNull.Value ? PreConnection.ByteArrayToImage((byte[])dataGridView1.SelectedRows[0].Cells["picture"].Value) : (Properties.Settings.Default.Use_animals_logo ? (Image)Properties.Resources.ResourceManager.GetObject(comboBox2.Text) : null);
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
            
            bool exist = false;
            
            if(animaux != null && (Is_New || (!Is_New && dataGridView1.SelectedRows.Count > 0)))
            {
               
                int cntt = animaux.Rows.Cast<DataRow>().Where(zz =>
                
                ((string)zz["NUM_IDENTIF"] == textBox2.Text.Trim().Replace(" ", "") &&
                zz["NME"].ToString().ToLower() == textBox3.Text.ToLower() &&
                (zz["ESPECE"] != DBNull.Value ? zz["ESPECE"].ToString() : "--").ToLower() == comboBox2.Text.ToLower() &&
                (zz["RACE"] != DBNull.Value ? zz["RACE"].ToString() : "--").ToLower() == comboBox3.Text.ToLower() &&
                (zz["SEXE"] != DBNull.Value ? zz["SEXE"].ToString() : "--").ToLower() == comboBox4.Text.ToLower()) &&
                int.Parse(zz["CLIENT_ID"].ToString()) == (comboBox1.SelectedValue != null ? (int)comboBox1.SelectedValue : -2) &&
                (!Is_New && dataGridView1.SelectedRows.Count > 0 ? (int)zz["ID"] != (int)dataGridView1.SelectedRows[0].Cells["ID"].Value : true)
                
                ).ToList().Count();

               
                exist = cntt > 0;

                
            }
            
            label13.Visible = exist;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = String.Format("NME LIKE '%{0}%'", textBox1.Text);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool autorisat = Properties.Settings.Default.Last_login_is_admin || (Is_New && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20001" && (Int32)QQ[3] == 1).Count() > 0) || (!Is_New && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20003" && (Int32)QQ[3] == 1).Count() > 0);
            if (autorisat)
            {
                bool all_ready = true;
                textBox3.BackColor = textBox3.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
                comboBox1.BackColor = comboBox1.Text.TrimStart().TrimEnd() != string.Empty && comboBox1.SelectedValue != null ? SystemColors.Window : Color.LightCoral;
                panel2.Visible = comboBox2.Text.Length == 0 || comboBox2.Text == "--";//Espece (Obligé !)
                panel1.Visible = comboBox4.Text.Length == 0 || comboBox4.Text == "--"; //Sexe (Obligé !)
                int mm = 0;
                if (Is_New) {mm= animaux.Rows.Cast<DataRow>().Where(XX => XX["NUM_IDENTIF"].ToString().Length > 0 && (string)XX["NUM_IDENTIF"] == textBox2.Text.Trim().Replace(" ", "")).ToList().Count; }
                else { mm = animaux.Rows.Cast<DataRow>().Where(XX => (int)XX["ID"] != (int)dataGridView1.SelectedRows[0].Cells["ID"].Value && XX["NUM_IDENTIF"].ToString().Length > 0 && (string)XX["NUM_IDENTIF"] == textBox2.Text.Trim().Replace(" ", "")).ToList().Count; }
             
                if (textBox2.Text.Trim().Replace(" ", "").Length == 0 || mm > 0)
                {
                    textBox2.BackColor = Color.LightCoral;
                    if (textBox2.Text.Trim().Replace(" ", "").Length > 0) { MessageBox.Show("Ce N° d'identification déja existe pour un autre animal,\n\nVeuillez le changer puis réesayer.\n\n", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    else { MessageBox.Show("Le N° d'identification est obligé,\n\nVeuillez le saisir puis réesayer.\n\n", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                }
                else
                {
                    all_ready &= comboBox1.BackColor == SystemColors.Window;
                    all_ready &= textBox3.Text.TrimStart().TrimEnd() != string.Empty;                    
                    all_ready &= !panel2.Visible;                     
                    all_ready &= !panel1.Visible;
                    if (all_ready && label13.Visible) { all_ready &= MessageBox.Show("Ce animal déja existe pour ce client,\n\nVoulez vous continuer comme meme ?\n\n", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes; }
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
                                    + "(`DATE_ADDED`,"
                                    + "`NME`,"
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
                                    + "('" + dateTimePicker3.Value.Date.ToString("yyyy-MM-dd") + "',"//<{ DATE_ADDED: }>,
                                    + "'" + textBox3.Text + "',"//<{ NME: }>,
                                    + "'" + textBox2.Text + "',"//{ NUM_IDENTIF: }>,
                                    + "'" + textBox4.Text + "',"//{ NUM_PASSPORT: }>,
                                    + comboBox1.SelectedValue + ","//{ CLIENT_ID: }>,
                                    + "'" + comboBox2.Text + "',"//{ ESPECE: }>,
                                    + "'" + comboBox3.Text + "',"//{ RACE: }>,
                                    + "'" + comboBox4.Text + "',"//{ SEXE: }>,
                                    + (checkBox2.Checked ? ("'" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","//{ NISS_DATE: }>,
                                    + "'" + textBox6.Text + "',"//{ ROBE: }>,
                                    + "'" + textBox8.Text + "',"//{ OBSERVATIONS: }>,
                                    + (checkBox1.Checked ? "TRUE" : "FALSE") + ","//{ IS_RADIATED: 0}>,
                                    + (checkBox1.Checked ? ("'" + dateTimePicker2.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","//{ RADIATION_DATE: }>,
                                    + "'" + (checkBox1.Checked ? textBox5.Text : "") + "'" //{ RADIATION_CAUSES 
                                    + (File.Exists(openFileDialog1.FileName) ? ",@Pic" : "")
                                    + ");";
                            MySqlCommand cmd = new MySqlCommand(insert_cmnd, PreConnection.mySqlConnection);
                            if (File.Exists(openFileDialog1.FileName)) { cmd.Parameters.AddWithValue("@Pic", imageData); }
                            PreConnection.open_conn();
                            Debug.WriteLine(">>>>>>>>>>>>>>>>>>>>>> " + insert_cmnd);
                            cmd.ExecuteNonQuery();
                        }
                        else //UPDATE
                        {
                            byte[] imageData = File.Exists(openFileDialog1.FileName) ? File.ReadAllBytes(openFileDialog1.FileName) : null;
                            string insert_cmnd = "UPDATE `tb_animaux` SET "
                                    + "`DATE_ADDED` = '" + dateTimePicker3.Value.Date.ToString("yyyy-MM-dd") + "',"
                                    + "`NME` = '" + textBox3.Text + "',"
                                    + "`NUM_IDENTIF` = '" + textBox2.Text + "',"
                                    + "`NUM_PASSPORT` = '" + textBox4.Text + "',"
                                    + "`CLIENT_ID` = " + comboBox1.SelectedValue + ","
                                    + "`ESPECE` = '" + comboBox2.Text + "',"
                                    + "`RACE` = '" + comboBox3.Text + "',"
                                    + "`SEXE` = '" + comboBox4.Text + "',"
                                    + "`NISS_DATE` = " + (checkBox2.Checked ? ("'" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","
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

                
                
            }
            else
            {
                new Non_Autorized_Msg("").ShowDialog();
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
            comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged -= comboBox4_SelectedIndexChanged;

            foreach (Control ctrl in tabPage1.Controls)
            {                
                if(ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(MaskedTextBox))
                {
                    ctrl.Text = string.Empty;
                }else if (ctrl.GetType() == typeof(ComboBox) && ((ComboBox)ctrl).DropDownStyle == ComboBoxStyle.DropDownList){
                    ((ComboBox)ctrl).SelectedIndex= 0;
                }
                else if(ctrl.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)ctrl).SelectedValue = -1;
                }
                //else if (ctrl.GetType() == typeof(DateTimePicker))
                //{
                //    ((DateTimePicker)ctrl).Value = DateTime.Now;
                //}
            }
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            button9.Visible = true;
            button9.PerformClick();
            button8.Visible = false;
            checkBox1.Checked = checkBox2.Checked = false;
            label13.Visible=false;
            pictureBox1.Image = Properties.Resources.NOUVEAU;
            if (tabControl1.TabPages.Count > 1) { tabControl1.TabPages.Remove(tabPage2); }
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
                if (MessageBox.Show("Vous étes sures de supprimer [" + dataGridView1.SelectedRows.Count + "] animaux ?\n\nAttention: Tous "+ (dataGridView1.SelectedRows.Count == 1 ? "ses" : "leurs") + " infos associées seront supprimés (Laboratires, Visies, Agenda ...).", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                        case "DATE_ADDED":
                            xcelApp.Cells[1, g.Index + 1].Value = "Ajouté le";
                            break;
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
            //clients = PreConnection.Load_data_keeping_duplicates("SELECT ID,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            clients = PreConnection.Load_data_keeping_duplicates("SELECT *,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
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
            verif_if_déja_exist_animal();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
            //----------------
            //if((int)comboBox1.SelectedValue > 0 && comboBox1.Text == "")
            //{
            //    comboBox1.SelectedValue = comboBox1.SelectedValue;
            //}
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {          
            panel2.Visible = comboBox2.SelectedIndex == 0;
            if (!button7.Visible)
            {
                if (Properties.Settings.Default.Use_animals_logo)
                {
                    pictureBox2.Image = (Image)Properties.Resources.ResourceManager.GetObject(comboBox2.Text);
                }
                else
                {
                    pictureBox2.Image = null;
                }
                
            }
            verif_if_déja_exist_animal();
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Visible = comboBox4.SelectedIndex == 0;
            verif_if_déja_exist_animal();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }

        private void Animaux_Load(object sender, EventArgs e)
        {         
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button4.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20002" && (Int32)QQ[3] == 1).Count() > 0; //Supprimer
                button3.Visible = button1.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20001" && (Int32)QQ[3] == 1).Count() > 0; //Ajouter Animal                                
                foreach(Control ctrl in tabPage1.Controls)
                {
                    if(ctrl.Name != "button8")
                    {
                        ctrl.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20003" && (Int32)QQ[3] == 1).Count() > 0; //Modifier
                    }
                }
                button1.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10001" && (Int32)QQ[3] == 1).Count() > 0; //Ajouter Client
            }
            //-------------------
            //-----------
            button9.PerformClick();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {            
            dateTimePicker1.MaxDate = dateTimePicker3.Value.Date.AddDays(1).AddSeconds(-1);
            dateTimePicker2.MinDate = dateTimePicker3.Value.Date;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!label12.Visible && !Is_New)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PARAM_NME", typeof(string));
                dt.Columns.Add("PARAM_VAL", typeof(string));
                //----------------
                dt.Rows.Add(new object[] { "DATE_ADDED", dateTimePicker3.Value.ToString("dd/MM/yyyy") });
                dt.Rows.Add(new object[] { "ANIM_NME", textBox3.Text });                
                dt.Rows.Add(new object[] { "ESPECE", comboBox2.Text });
                dt.Rows.Add(new object[] { "RACE", comboBox3.Text });
                dt.Rows.Add(new object[] { "SEX", comboBox4.Text });
                dt.Rows.Add(new object[] { "DATE_NISS", checkBox2.Checked ? dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") : null });
                dt.Rows.Add(new object[] { "REF", dateTimePicker3.Value.ToString("MM") + textBox3.Text.Substring(0, 1).ToUpper() + dateTimePicker3.Value.ToString("dd") + textBox3.Text.Substring(2, 1).ToUpper() + comboBox1.SelectedValue + dataGridView1.SelectedRows[0].Cells["ID"].Value + dateTimePicker3.Value.Month + (dateTimePicker3.Value.Month + dateTimePicker3.Value.Day) + (dateTimePicker3.Value.Month * 2) });
                dt.Rows.Add(new object[] { "NUM_PASSPORT", textBox4.Text });
                dt.Rows.Add(new object[] { "NUM_IDENTIF", textBox2.Text });
                dt.Rows.Add(new object[] { "CABINET", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_TEL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_EMAIL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_ADRESS", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });

                DataRow rww = clients.Rows.Cast<DataRow>().Where(FF => (int)FF["ID"] == (int)comboBox1.SelectedValue).FirstOrDefault();
                dt.Rows.Add(new object[] { "CLIENT_SEX", rww["SEX"] != DBNull.Value ? rww["SEX"].ToString() : null});
                dt.Rows.Add(new object[] { "CLIENT_FAMNME", rww["FAMNME"] != DBNull.Value ? rww["FAMNME"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NME", rww["NME"] != DBNull.Value ? rww["NME"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NUM_CNI", rww["NUM_CNI"] != DBNull.Value ? rww["NUM_CNI"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_ADRESS", rww["ADRESS"] != DBNull.Value ? rww["ADRESS"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_CITY", rww["CITY"] != DBNull.Value ? rww["CITY"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_WILAYA", rww["WILAYA"] != DBNull.Value ? rww["WILAYA"].ToString() : null });
                dt.Rows.Add(new object[] { "POSTAL_CODE", rww["POSTAL_CODE"] != DBNull.Value ? rww["POSTAL_CODE"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NUM_PHONE", rww["NUM_PHONE"] != DBNull.Value ? rww["NUM_PHONE"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_EMAIL", rww["EMAIL"] != DBNull.Value ? rww["EMAIL"].ToString() : null });

                
                //-------------
                new Print_report("certificat_enreg", dt, null).ShowDialog();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = checkBox2.Checked;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Is_New)
            {
                string vall = string.Concat(DateTime.Now.Millisecond.ToString("D3"), DateTime.Now.ToString("MMMM").Substring(1, 2), int.Parse((DateTime.Now.Minute * DateTime.Now.Second).ToString().Substring(0, (DateTime.Now.Minute * DateTime.Now.Second).ToString().Length > 4 ? 4 : (DateTime.Now.Minute * DateTime.Now.Second).ToString().Length)).ToString("D4"), DateTime.Now.ToString("dddd").Substring(2, 2), DateTime.Now.ToString("yy"), int.Parse((DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Substring(0, (DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Length > 4 ? 4 : (DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Length)).ToString("D4")).ToUpper();
                if (animaux != null)
                {
                    if (animaux.Rows.Count > 0)
                    {
                        while (animaux.Rows.Cast<DataRow>().Where(XX => (string)XX["NUM_IDENTIF"] == vall).ToList().Count > 0)
                        {
                            vall = string.Concat(DateTime.Now.Millisecond.ToString("D3"), DateTime.Now.ToString("MMMM").Substring(1, 2), int.Parse((DateTime.Now.Minute * DateTime.Now.Second).ToString().Substring(0, (DateTime.Now.Minute * DateTime.Now.Second).ToString().Length > 4 ? 4 : (DateTime.Now.Minute * DateTime.Now.Second).ToString().Length)).ToString("D4"), DateTime.Now.ToString("dddd").Substring(2, 2), DateTime.Now.ToString("yy"), int.Parse((DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Substring(0, (DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Length > 4 ? 4 : (DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Length)).ToString("D4")).ToUpper();
                        }
                    }
                }                
                textBox2.Text = vall;
            }
            
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (!Is_New)
            {
                MessageBox.Show("Le N° d'identification est trés sensible, il peut etre utilisé précedemment (Certificat d'identification ...)\n\nDonc confirmer bien avant de faire la modification.", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }
    }
}

