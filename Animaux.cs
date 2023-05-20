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
using Xamarin.Forms.Internals;
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
        bool Is_New_Visite = true;
        public Animaux()
        {
            InitializeComponent();
            tabControl1.TabPages.Remove(tabPage2);
            //----------------------
            comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = 0;
            //----------------------
            //clients = PreConnection.Load_data_keeping_duplicates("SELECT ID,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            clients = PreConnection.Load_data_keeping_duplicates("SELECT *,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            comboBox1.DataSource = clients;
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";
            if (clients.Rows.Count > 0) { comboBox1.SelectedIndex = 0; }
            full_nme_clients = new List<string>();
            clients.Rows.Cast<DataRow>().ToList().ForEach(clt =>
            {
                full_nme_clients.Add((string)clt["FULL_NME"]);
            });
            //--------------------
            comboBox5.AutoCompleteCustomSource.AddRange(clients.AsEnumerable().Select(row => row.Field<string>("FULL_NME")).ToArray());
            //---------------------
            Load_anims_from_DB();
            //---------------------


        }
        private void Load_anims_from_DB()
        {
            int fd = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : 99999999;
            animaux = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_animaux;");
            dataGridView1.DataSource = animaux;
            if (dataGridView1.Rows.Count > fd)
            { dataGridView1.ClearSelection(); dataGridView1.Rows[fd].Selected = true; }
            else if (dataGridView1.Rows.Count > 0)
            { dataGridView1.ClearSelection(); dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true; }


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
                comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
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

            if (animaux != null && (Is_New || (!Is_New && dataGridView1.SelectedRows.Count > 0)))
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
                if (Is_New) { mm = animaux.Rows.Cast<DataRow>().Where(XX => XX["NUM_IDENTIF"].ToString().Length > 0 && (string)XX["NUM_IDENTIF"] == textBox2.Text.Trim().Replace(" ", "")).ToList().Count; }
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
            load_visites();
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
                if (ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(MaskedTextBox))
                {
                    ctrl.Text = string.Empty;
                }
                else if (ctrl.GetType() == typeof(ComboBox) && ((ComboBox)ctrl).DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    ((ComboBox)ctrl).SelectedIndex = 0;
                }
                else if (ctrl.GetType() == typeof(ComboBox))
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
            label13.Visible = false;
            pictureBox1.Image = Properties.Resources.NOUVEAU;
            if (tabControl1.TabPages.Count > 1) { tabControl1.TabPages.Remove(tabPage2); }
            if (!textBox1.Focused) { textBox3.Select(); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Vous étes sures de supprimer [" + dataGridView1.SelectedRows.Count + "] animaux ?\n\nAttention: Tous " + (dataGridView1.SelectedRows.Count == 1 ? "ses" : "leurs") + " infos associées seront supprimés (Laboratires, Visies, Agenda ...).", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string fff = "";
                    dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row => fff += "," + row.Cells["ID"].Value );
                    fff = fff.Substring(1);
                    PreConnection.Excut_Cmd("DELETE FROM tb_animaux WHERE ID IN (" + fff + ");");
                    Load_anims_from_DB();
                }

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
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
                        if (xcelApp.Cells[1, b.ColumnIndex + 1].Value == "Propriétaire")
                        {
                            xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? (((DataRow)clients.AsEnumerable().FirstOrDefault(row => row.Field<int>("ID") == (int)dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value))["FULL_NME"]) : "";
                        }
                        else
                        {
                            xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").Replace("00:00:00", "").TrimStart().TrimEnd() : "";
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
            if (checkBox1.Checked)
            {
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
            if (clients.Rows.Count > 0) { comboBox1.SelectedValue = (int)clients.AsEnumerable().Max(row => row.Field<int>("ID")); }
            full_nme_clients.Clear();
            clients.Rows.Cast<DataRow>().ToList().ForEach(clt =>
            {
                full_nme_clients.Add((string)clt["FULL_NME"]);
            });
            comboBox1.Validating += comboBox1_Validating;
        }

        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {
            if (comboBox1.Text.Length > 0 && !full_nme_clients.Contains(comboBox1.Text))
            {
                comboBox1.BackColor = Color.LightCoral;
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
            openFileDialog1.FileName = "";
            button7.Visible = false;
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
                foreach (Control ctrl in tabPage1.Controls)
                {
                    if (ctrl.Name != "button8")
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
                dt.Rows.Add(new object[] { "CLIENT_SEX", rww["SEX"] != DBNull.Value ? rww["SEX"].ToString() : null });
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

        private void button12_Click(object sender, EventArgs e)
        {
            Is_New_Visite = true;
            dateTimePicker4.Value = DateTime.Now;
            comboBox5.Text = comboBox1.Text;
            richTextBox1.Text = string.Empty;
            richTextBox1.BackColor = SystemColors.Window;
            pictureBox3.Image = Properties.Resources.NOUVEAU_003;
            dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
            dataGridView2.ClearSelection();
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = richTextBox1.Text.Length > 0 ? SystemColors.Window : Color.LightCoral;
            if (richTextBox1.BackColor == SystemColors.Window)
            {
                if (Is_New_Visite)
                {
                    PreConnection.Excut_Cmd("INSERT INTO `tb_visites`"
                                          + "(`DATETIME`,"
                                          + "`ANIM_ID`,"
                                          + "`VISITOR_FULL_NME`,"
                                          + "`OBJECT`)"
                                          + "VALUES"
                                          + "('" + dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',"
                                          + dataGridView1.SelectedRows[0].Cells["ID"].Value + ","
                                          + "'" + comboBox5.Text + "',"
                                          + "'" + richTextBox1.Text + "');");
                }
                else
                {
                    PreConnection.Excut_Cmd("UPDATE `tb_visites` SET "
                                          + "`DATETIME` = '" + dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',"
                                          + "`VISITOR_FULL_NME` = '" + comboBox5.Text + "',"
                                          + "`OBJECT` = '" + richTextBox1.Text + "'"
                                          + " WHERE `ID` = " + dataGridView2.SelectedRows[0].Cells["ID_VISITE"].Value + ";");
                }
                load_visites();
            }
        }

        private void load_visites()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int prev_select = dataGridView2.SelectedRows.Count > 0 ? (int)dataGridView2.SelectedRows[0].Cells["ID_VISITE"].Value : -1;
                DataTable visites = PreConnection.Load_data("SELECT tb1.*,tb2.REF AS 'FACTURE_REF' FROM tb_visites tb1 LEFT JOIN ("
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_01` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NOT NULL AND `ITEM_NME_01` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_02` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NOT NULL AND `ITEM_NME_02` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_03` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NOT NULL AND `ITEM_NME_03` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_04` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NOT NULL AND `ITEM_NME_04` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_05` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NOT NULL AND `ITEM_NME_05` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_06` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NOT NULL AND `ITEM_NME_06` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_07` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NOT NULL AND `ITEM_NME_07` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_08` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NOT NULL AND `ITEM_NME_08` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_09` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NOT NULL AND `ITEM_NME_09` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_10` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NOT NULL AND `ITEM_NME_10` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_11` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NOT NULL AND `ITEM_NME_11` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_12` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NOT NULL AND `ITEM_NME_12` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_13` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NOT NULL AND `ITEM_NME_13` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_14` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NOT NULL AND `ITEM_NME_14` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_15` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NOT NULL AND `ITEM_NME_15` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_16` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NOT NULL AND `ITEM_NME_16` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_17` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NOT NULL AND `ITEM_NME_17` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_18` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NOT NULL AND `ITEM_NME_18` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_19` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NOT NULL AND `ITEM_NME_19` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_20` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NOT NULL AND `ITEM_NME_20` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_21` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NOT NULL AND `ITEM_NME_21` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_22` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NOT NULL AND `ITEM_NME_22` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_23` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NOT NULL AND `ITEM_NME_23` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_24` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NOT NULL AND `ITEM_NME_24` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_25` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NOT NULL AND `ITEM_NME_25` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_26` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NOT NULL AND `ITEM_NME_26` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_27` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NOT NULL AND `ITEM_NME_27` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_28` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NOT NULL AND `ITEM_NME_28` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_29` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NOT NULL AND `ITEM_NME_29` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_30` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NOT NULL AND `ITEM_NME_30` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_31` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NOT NULL AND `ITEM_NME_31` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_32` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NOT NULL AND `ITEM_NME_32` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_33` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NOT NULL AND `ITEM_NME_33` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_34` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NOT NULL AND `ITEM_NME_34` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_35` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NOT NULL AND `ITEM_NME_35` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_36` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NOT NULL AND `ITEM_NME_36` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_37` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NOT NULL AND `ITEM_NME_37` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_38` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NOT NULL AND `ITEM_NME_38` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_39` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NOT NULL AND `ITEM_NME_39` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_40` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NOT NULL AND `ITEM_NME_40` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_41` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NOT NULL AND `ITEM_NME_41` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_42` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NOT NULL AND `ITEM_NME_42` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_43` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NOT NULL AND `ITEM_NME_43` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_44` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NOT NULL AND `ITEM_NME_44` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_45` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NOT NULL AND `ITEM_NME_45` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_46` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NOT NULL AND `ITEM_NME_46` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_47` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NOT NULL AND `ITEM_NME_47` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_48` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NOT NULL AND `ITEM_NME_48` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_49` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NOT NULL AND `ITEM_NME_49` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_50` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NOT NULL AND `ITEM_NME_50` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_51` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NOT NULL AND `ITEM_NME_51` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_52` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NOT NULL AND `ITEM_NME_52` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_53` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NOT NULL AND `ITEM_NME_53` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_54` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NOT NULL AND `ITEM_NME_54` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_55` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NOT NULL AND `ITEM_NME_55` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_56` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NOT NULL AND `ITEM_NME_56` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_57` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NOT NULL AND `ITEM_NME_57` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_58` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NOT NULL AND `ITEM_NME_58` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_59` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NOT NULL AND `ITEM_NME_59` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_60` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NOT NULL AND `ITEM_NME_60` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_61` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NOT NULL AND `ITEM_NME_61` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_62` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NOT NULL AND `ITEM_NME_62` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_63` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NOT NULL AND `ITEM_NME_63` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_64` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NOT NULL AND `ITEM_NME_64` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_65` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NOT NULL AND `ITEM_NME_65` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_66` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NOT NULL AND `ITEM_NME_66` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_67` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NOT NULL AND `ITEM_NME_67` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_68` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NOT NULL AND `ITEM_NME_68` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_69` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NOT NULL AND `ITEM_NME_69` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_70` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NOT NULL AND `ITEM_NME_70` IS NOT NULL "
                                    + ") tb2 ON tb1.`ID` = tb2.`VISIT` WHERE tb1.`ANIM_ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + " ORDER BY DATETIME;");
                dataGridView2.DataSource = visites;
                if (dataGridView2.DisplayedRowCount(false) < dataGridView2.RowCount) { dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows.Count - 1; }
                dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
                dataGridView2.ClearSelection();
                if (prev_select > -1)
                {
                    DataGridViewRow rww = dataGridView2.Rows.Cast<DataGridViewRow>().Where(ZZ => (int)ZZ.Cells["ID_VISITE"].Value == prev_select).FirstOrDefault();
                    if (rww != null) { rww.Selected = true; }

                }
                else if (dataGridView2.Rows.Count > 0)
                {
                    int idd_max = dataGridView2.Rows.Cast<DataGridViewRow>().Max(row => Convert.ToInt32(row.Cells["ID_VISITE"].Value));
                    DataGridViewRow rww = dataGridView2.Rows.Cast<DataGridViewRow>().Where(ZZ => (int)ZZ.Cells["ID_VISITE"].Value == idd_max).FirstOrDefault();
                    if (rww != null) { rww.Selected = true; }
                }
                dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
                dataGridView2_SelectionChanged(null, null);

            }
            else
            {
                ((DataTable)dataGridView2.DataSource).Rows.Clear();
            }

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                Is_New_Visite = false;
                pictureBox3.Image = Properties.Resources.MODIF_003;
                DataGridViewRow row = dataGridView2.SelectedRows[0];
                dateTimePicker4.Value = (DateTime)row.Cells["DATETIME"].Value;
                comboBox5.Text = (string)row.Cells["VISITOR_FULL_NME"].Value;
                richTextBox1.Text = (string)row.Cells["OBJECT"].Value;
                richTextBox1.BackColor = SystemColors.Window;
            }
            else
            {
                button12_Click(null, null);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count == 1)
            {
                dataGridView2_SelectionChanged(null, null);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.BackColor = SystemColors.Window;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                if (MessageBox.Show("Vous-étes sur de faire la suppression de (" + dataGridView2.SelectedRows.Count + ") visites ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string fact_ref = "";
                    string ids = "";
                    List<int> selected_row_delete_db = new List<int>();
                    dataGridView2.SelectedRows.Cast<DataGridViewRow>().ForEach(rw => { ids += "," + rw.Cells["ID_VISITE"].Value;
                        if (rw.Cells["FACTURE_REF"].Value != DBNull.Value)
                        {
                            fact_ref += ",'" + rw.Cells["FACTURE_REF"].Value + "'";
                        }
                    });
                    if (ids.Length > 0)
                    {
                        ids = ids.Substring(1);
                        PreConnection.Excut_Cmd("DELETE FROM tb_visites WHERE `ID` IN (" + ids + ");");
                        //-----------
                        if (fact_ref.Length > 0)
                        {
                            PreConnection.Excut_Cmd("UPDATE tb_factures_vente SET "
                                                     + "`ITEM_PROD_CODE_01` = IF(`ITEM_IS_PROD_01` = FALSE AND `ITEM_PROD_CODE_01` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_01`),"
                                                    + "`ITEM_PROD_CODE_02` = IF(`ITEM_IS_PROD_02` = FALSE AND `ITEM_PROD_CODE_02` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_02`),"
                                                    + "`ITEM_PROD_CODE_03` = IF(`ITEM_IS_PROD_03` = FALSE AND `ITEM_PROD_CODE_03` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_03`),"
                                                    + "`ITEM_PROD_CODE_04` = IF(`ITEM_IS_PROD_04` = FALSE AND `ITEM_PROD_CODE_04` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_04`),"
                                                    + "`ITEM_PROD_CODE_05` = IF(`ITEM_IS_PROD_05` = FALSE AND `ITEM_PROD_CODE_05` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_05`),"
                                                    + "`ITEM_PROD_CODE_06` = IF(`ITEM_IS_PROD_06` = FALSE AND `ITEM_PROD_CODE_06` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_06`),"
                                                    + "`ITEM_PROD_CODE_07` = IF(`ITEM_IS_PROD_07` = FALSE AND `ITEM_PROD_CODE_07` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_07`),"
                                                    + "`ITEM_PROD_CODE_08` = IF(`ITEM_IS_PROD_08` = FALSE AND `ITEM_PROD_CODE_08` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_08`),"
                                                    + "`ITEM_PROD_CODE_09` = IF(`ITEM_IS_PROD_09` = FALSE AND `ITEM_PROD_CODE_09` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_09`),"
                                                    + "`ITEM_PROD_CODE_10` = IF(`ITEM_IS_PROD_10` = FALSE AND `ITEM_PROD_CODE_10` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_10`),"
                                                    + "`ITEM_PROD_CODE_11` = IF(`ITEM_IS_PROD_11` = FALSE AND `ITEM_PROD_CODE_11` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_11`),"
                                                    + "`ITEM_PROD_CODE_12` = IF(`ITEM_IS_PROD_12` = FALSE AND `ITEM_PROD_CODE_12` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_12`),"
                                                    + "`ITEM_PROD_CODE_13` = IF(`ITEM_IS_PROD_13` = FALSE AND `ITEM_PROD_CODE_13` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_13`),"
                                                    + "`ITEM_PROD_CODE_14` = IF(`ITEM_IS_PROD_14` = FALSE AND `ITEM_PROD_CODE_14` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_14`),"
                                                    + "`ITEM_PROD_CODE_15` = IF(`ITEM_IS_PROD_15` = FALSE AND `ITEM_PROD_CODE_15` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_15`),"
                                                    + "`ITEM_PROD_CODE_16` = IF(`ITEM_IS_PROD_16` = FALSE AND `ITEM_PROD_CODE_16` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_16`),"
                                                    + "`ITEM_PROD_CODE_17` = IF(`ITEM_IS_PROD_17` = FALSE AND `ITEM_PROD_CODE_17` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_17`),"
                                                    + "`ITEM_PROD_CODE_18` = IF(`ITEM_IS_PROD_18` = FALSE AND `ITEM_PROD_CODE_18` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_18`),"
                                                    + "`ITEM_PROD_CODE_19` = IF(`ITEM_IS_PROD_19` = FALSE AND `ITEM_PROD_CODE_19` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_19`),"
                                                    + "`ITEM_PROD_CODE_20` = IF(`ITEM_IS_PROD_20` = FALSE AND `ITEM_PROD_CODE_20` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_20`),"
                                                    + "`ITEM_PROD_CODE_21` = IF(`ITEM_IS_PROD_21` = FALSE AND `ITEM_PROD_CODE_21` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_21`),"
                                                    + "`ITEM_PROD_CODE_22` = IF(`ITEM_IS_PROD_22` = FALSE AND `ITEM_PROD_CODE_22` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_22`),"
                                                    + "`ITEM_PROD_CODE_23` = IF(`ITEM_IS_PROD_23` = FALSE AND `ITEM_PROD_CODE_23` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_23`),"
                                                    + "`ITEM_PROD_CODE_24` = IF(`ITEM_IS_PROD_24` = FALSE AND `ITEM_PROD_CODE_24` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_24`),"
                                                    + "`ITEM_PROD_CODE_25` = IF(`ITEM_IS_PROD_25` = FALSE AND `ITEM_PROD_CODE_25` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_25`),"
                                                    + "`ITEM_PROD_CODE_26` = IF(`ITEM_IS_PROD_26` = FALSE AND `ITEM_PROD_CODE_26` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_26`),"
                                                    + "`ITEM_PROD_CODE_27` = IF(`ITEM_IS_PROD_27` = FALSE AND `ITEM_PROD_CODE_27` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_27`),"
                                                    + "`ITEM_PROD_CODE_28` = IF(`ITEM_IS_PROD_28` = FALSE AND `ITEM_PROD_CODE_28` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_28`),"
                                                    + "`ITEM_PROD_CODE_29` = IF(`ITEM_IS_PROD_29` = FALSE AND `ITEM_PROD_CODE_29` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_29`),"
                                                    + "`ITEM_PROD_CODE_30` = IF(`ITEM_IS_PROD_30` = FALSE AND `ITEM_PROD_CODE_30` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_30`),"
                                                    + "`ITEM_PROD_CODE_31` = IF(`ITEM_IS_PROD_31` = FALSE AND `ITEM_PROD_CODE_31` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_31`),"
                                                    + "`ITEM_PROD_CODE_32` = IF(`ITEM_IS_PROD_32` = FALSE AND `ITEM_PROD_CODE_32` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_32`),"
                                                    + "`ITEM_PROD_CODE_33` = IF(`ITEM_IS_PROD_33` = FALSE AND `ITEM_PROD_CODE_33` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_33`),"
                                                    + "`ITEM_PROD_CODE_34` = IF(`ITEM_IS_PROD_34` = FALSE AND `ITEM_PROD_CODE_34` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_34`),"
                                                    + "`ITEM_PROD_CODE_35` = IF(`ITEM_IS_PROD_35` = FALSE AND `ITEM_PROD_CODE_35` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_35`),"
                                                    + "`ITEM_PROD_CODE_36` = IF(`ITEM_IS_PROD_36` = FALSE AND `ITEM_PROD_CODE_36` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_36`),"
                                                    + "`ITEM_PROD_CODE_37` = IF(`ITEM_IS_PROD_37` = FALSE AND `ITEM_PROD_CODE_37` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_37`),"
                                                    + "`ITEM_PROD_CODE_38` = IF(`ITEM_IS_PROD_38` = FALSE AND `ITEM_PROD_CODE_38` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_38`),"
                                                    + "`ITEM_PROD_CODE_39` = IF(`ITEM_IS_PROD_39` = FALSE AND `ITEM_PROD_CODE_39` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_39`),"
                                                    + "`ITEM_PROD_CODE_40` = IF(`ITEM_IS_PROD_40` = FALSE AND `ITEM_PROD_CODE_40` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_40`),"
                                                    + "`ITEM_PROD_CODE_41` = IF(`ITEM_IS_PROD_41` = FALSE AND `ITEM_PROD_CODE_41` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_41`),"
                                                    + "`ITEM_PROD_CODE_42` = IF(`ITEM_IS_PROD_42` = FALSE AND `ITEM_PROD_CODE_42` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_42`),"
                                                    + "`ITEM_PROD_CODE_43` = IF(`ITEM_IS_PROD_43` = FALSE AND `ITEM_PROD_CODE_43` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_43`),"
                                                    + "`ITEM_PROD_CODE_44` = IF(`ITEM_IS_PROD_44` = FALSE AND `ITEM_PROD_CODE_44` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_44`),"
                                                    + "`ITEM_PROD_CODE_45` = IF(`ITEM_IS_PROD_45` = FALSE AND `ITEM_PROD_CODE_45` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_45`),"
                                                    + "`ITEM_PROD_CODE_46` = IF(`ITEM_IS_PROD_46` = FALSE AND `ITEM_PROD_CODE_46` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_46`),"
                                                    + "`ITEM_PROD_CODE_47` = IF(`ITEM_IS_PROD_47` = FALSE AND `ITEM_PROD_CODE_47` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_47`),"
                                                    + "`ITEM_PROD_CODE_48` = IF(`ITEM_IS_PROD_48` = FALSE AND `ITEM_PROD_CODE_48` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_48`),"
                                                    + "`ITEM_PROD_CODE_49` = IF(`ITEM_IS_PROD_49` = FALSE AND `ITEM_PROD_CODE_49` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_49`),"
                                                    + "`ITEM_PROD_CODE_50` = IF(`ITEM_IS_PROD_50` = FALSE AND `ITEM_PROD_CODE_50` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_50`),"
                                                    + "`ITEM_PROD_CODE_51` = IF(`ITEM_IS_PROD_51` = FALSE AND `ITEM_PROD_CODE_51` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_51`),"
                                                    + "`ITEM_PROD_CODE_52` = IF(`ITEM_IS_PROD_52` = FALSE AND `ITEM_PROD_CODE_52` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_52`),"
                                                    + "`ITEM_PROD_CODE_53` = IF(`ITEM_IS_PROD_53` = FALSE AND `ITEM_PROD_CODE_53` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_53`),"
                                                    + "`ITEM_PROD_CODE_54` = IF(`ITEM_IS_PROD_54` = FALSE AND `ITEM_PROD_CODE_54` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_54`),"
                                                    + "`ITEM_PROD_CODE_55` = IF(`ITEM_IS_PROD_55` = FALSE AND `ITEM_PROD_CODE_55` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_55`),"
                                                    + "`ITEM_PROD_CODE_56` = IF(`ITEM_IS_PROD_56` = FALSE AND `ITEM_PROD_CODE_56` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_56`),"
                                                    + "`ITEM_PROD_CODE_57` = IF(`ITEM_IS_PROD_57` = FALSE AND `ITEM_PROD_CODE_57` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_57`),"
                                                    + "`ITEM_PROD_CODE_58` = IF(`ITEM_IS_PROD_58` = FALSE AND `ITEM_PROD_CODE_58` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_58`),"
                                                    + "`ITEM_PROD_CODE_59` = IF(`ITEM_IS_PROD_59` = FALSE AND `ITEM_PROD_CODE_59` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_59`),"
                                                    + "`ITEM_PROD_CODE_60` = IF(`ITEM_IS_PROD_60` = FALSE AND `ITEM_PROD_CODE_60` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_60`),"
                                                    + "`ITEM_PROD_CODE_61` = IF(`ITEM_IS_PROD_61` = FALSE AND `ITEM_PROD_CODE_61` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_61`),"
                                                    + "`ITEM_PROD_CODE_62` = IF(`ITEM_IS_PROD_62` = FALSE AND `ITEM_PROD_CODE_62` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_62`),"
                                                    + "`ITEM_PROD_CODE_63` = IF(`ITEM_IS_PROD_63` = FALSE AND `ITEM_PROD_CODE_63` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_63`),"
                                                    + "`ITEM_PROD_CODE_64` = IF(`ITEM_IS_PROD_64` = FALSE AND `ITEM_PROD_CODE_64` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_64`),"
                                                    + "`ITEM_PROD_CODE_65` = IF(`ITEM_IS_PROD_65` = FALSE AND `ITEM_PROD_CODE_65` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_65`),"
                                                    + "`ITEM_PROD_CODE_66` = IF(`ITEM_IS_PROD_66` = FALSE AND `ITEM_PROD_CODE_66` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_66`),"
                                                    + "`ITEM_PROD_CODE_67` = IF(`ITEM_IS_PROD_67` = FALSE AND `ITEM_PROD_CODE_67` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_67`),"
                                                    + "`ITEM_PROD_CODE_68` = IF(`ITEM_IS_PROD_68` = FALSE AND `ITEM_PROD_CODE_68` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_68`),"
                                                    + "`ITEM_PROD_CODE_69` = IF(`ITEM_IS_PROD_69` = FALSE AND `ITEM_PROD_CODE_69` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_69`),"
                                                    + "`ITEM_PROD_CODE_70` = IF(`ITEM_IS_PROD_70` = FALSE AND `ITEM_PROD_CODE_70` IN ("+ ids + "), NULL, `ITEM_PROD_CODE_70`)"
                                + ";");
                        }
                    }
                    
                    load_visites();
                }
            }
        }
    }
}

