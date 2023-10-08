using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Resources;
using ServiceStack;
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
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using TextBox = System.Windows.Forms.TextBox;

namespace ALBAITAR_Softvet.Labo
{
    public partial class Autre_Lab : UserControl
    {
        DataGridViewRow selected_animm = null;
        DataTable lab_histor;
        bool is_new = true;
        string ref_tmp = string.Empty;
        string current_analys_type = "Coprologie";
        string IDD_to_select = "";
        double Anim_poids = 0;
        public Autre_Lab(DataGridViewRow selected_anim, string ID_to_select)
        {
            InitializeComponent();
            selected_animm = selected_anim;
            IDD_to_select = ID_to_select;
            //------------------------------
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button5.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30004" && (Int32)QQ[3] == 1).Count() > 0; //Imprimer
            }
            lab_histor = new DataTable();
        }

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
                groupBox3.Enabled = textBox5.Enabled = textBox6.Enabled = textBox3.Enabled = dateTimePicker1.Enabled = textBox1.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31003" && (Int32)QQ[3] == 1).Count() > 0 || Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31001" && (Int32)QQ[3] == 1).Count() > 0; //Modifier
            }
            //--------En-tete
            label3.Text = (string)selected_animm.Cells["NME"].Value;
            label4.Text = (string)selected_animm.Cells["CLIENT_FULL_NME"].Value;
            label6.Text = (string)selected_animm.Cells["ESPECE"].Value;
            label8.Text = (string)selected_animm.Cells["RACE"].Value;
            label13.Text = (string)selected_animm.Cells["SEXE"].Value;
            var CCC = Main_Frm.main_poids_tab.AsEnumerable().Where(F => F.Field<int>("ANIM_ID") == (int)selected_animm.Cells["ID"].Value);
            Anim_poids = CCC.Any() ? CCC.Last().Field<double>("POIDS") : 0;
            label2.Text = Anim_poids.ToString("N2") + " Kg";
            label14.Text = selected_animm.Cells["NISS_DATE"].Value != DBNull.Value ? ((DateTime)selected_animm.Cells["NISS_DATE"].Value).ToString("d") : "--";
            textBox2.Text = (string)selected_animm.Cells["OBSERVATIONS"].Value;
            //-------------------------
            Load_histor();
            //------------------

        }


        private void Load_histor()
        {
            string[] sss = { "Hemogramme", "Biochimie", "Immunologie", "Protéinogramme", "Urologie" };
            if (Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && !sss.Contains((string)P["LABO_NME"])).Count() > 0)
            {
                lab_histor = Laboratoire.labo.AsEnumerable().Where(P => (int)P["ANIM_ID"] == (int)selected_animm.Cells["ID"].Value && !sss.Contains((string)P["LABO_NME"])).CopyToDataTable();
                dataGridView22.DataSource = lab_histor;
                dataGridView22.Refresh();
            }
            else
            {
                if (dataGridView22.DataSource != null)
                {
                    DataTable dt = ((DataTable)dataGridView22.DataSource).Clone();
                    dataGridView22.DataSource = dt;
                }
            }
            //---------------

        }


        private void button3_Click(object sender, EventArgs e)
        {
            is_new = true;
            pictureBox1.Image = Properties.Resources.NOUVEAU_003;
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox4.Clear();
            radioButton1.Checked = true;
            textBox3.Text = ref_tmp = "ATR_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
            //------------
            button5.Visible = false;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.BackColor = textBox4.Enabled && textBox4.Text.Trim().Length == 0 && radioButton10.Checked ? Color.LightCoral : SystemColors.Window;
            textBox5.BackColor = textBox5.Text.Trim().Length > 0 ? SystemColors.Window : Color.LightCoral;
            textBox6.BackColor = textBox6.Text.Trim().Length > 0 ? SystemColors.Window : Color.LightCoral;
            bool autorisat = Properties.Settings.Default.Last_login_is_admin || (is_new && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31001" && (Int32)QQ[3] == 1).Count() > 0) || (!is_new && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31003" && (Int32)QQ[3] == 1).Count() > 0);
            if (autorisat)
            {
                int current_row_to_select = is_new ? -1 : dataGridView22.SelectedRows[0].Index;
                bool ready = true;
                ready &= label20.Text.Length == 0;
                ready &= (!radioButton10.Enabled || !radioButton10.Checked || (radioButton10.Checked && textBox4.BackColor != Color.LightCoral));
                ready &= textBox5.BackColor != Color.LightCoral;
                ready &= textBox6.BackColor != Color.LightCoral;

                if (ready)
                {
                    if (is_new)
                    {
                        PreConnection.Excut_Cmd(1, "tb_labo_autre", new List<string> {
                            "REF",
                            "DATE_TIME",
                            "ANIM_ID",
                            "OBSERV",
                            "TYPE_ANAL",
                            "METHODE",
                            "RESULT"}, new List<object>
                            {
                                textBox3.Text,
                                dateTimePicker1.Value,
                                selected_animm.Cells["ID"].Value,
                                textBox1.Text,
                                current_analys_type,
                                textBox5.Text,
                                textBox6.Text
                            }, null, null, null);
                    }
                    else
                    {
                        PreConnection.Excut_Cmd(2, "tb_labo_autre", new List<string> {
                            "REF",
                            "DATE_TIME",
                            "OBSERV",
                            "TYPE_ANAL",
                            "METHODE",
                            "RESULT"}, new List<object>
                            {
                                textBox3.Text,
                                dateTimePicker1.Value,
                                textBox1.Text,
                                current_analys_type,
                                textBox5.Text,
                                textBox6.Text
                            }, "ID = @P_ID", new List<string> { "P_ID"}, new List<object> { dataGridView22.SelectedRows[0].Cells["ID"].Value });
                    }
                    //--------
                    Laboratoire.labo = PreConnection.Load_data(Laboratoire.labo_load_cmd);
                    Laboratoire.make_historic_refesh = true;
                    //------------
                    Load_histor();
                    dataGridView22.ClearSelection();
                    if (dataGridView22.Rows.Count > 0)
                    {
                        dataGridView22.Rows[current_row_to_select == -1 ? dataGridView22.Rows.Count - 1 : current_row_to_select].Selected = true;
                    }
                    //----------------
                    button5.Visible = true;
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
                else if (dataGridView22.SelectedRows.Count > 0)
                {
                    nmb = lab_histor.AsEnumerable().Where(P => (int)dataGridView22.SelectedRows[0].Cells["ID"].Value != (int)P["ID"] && ((string)P["REF"]).Trim().Length > 0 && (string)P["REF"] == textBox3.Text.Trim()).Count();
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
                textBox3.Text = ref_tmp = "ATR_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHffff") + "_" + selected_animm.Cells["ID"].Value;
            }
            button5.Visible = false;
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {

            if (dataGridView22.SelectedRows.Count > 0)
            {

                DataTable dt = PreConnection.Load_data("SELECT * FROM tb_labo_autre WHERE ID = " + dataGridView22.SelectedRows[0].Cells["ID"].Value + ";");


                //--------------------
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        is_new = false;
                        pictureBox1.Image = Properties.Resources.MODIF_003;
                        ref_tmp = string.Empty;
                        dateTimePicker1.Value = (DateTime)dt.Rows[0]["DATE_TIME"];
                        textBox3.Text = (string)dt.Rows[0]["REF"];
                        textBox5.Text = (string)dt.Rows[0]["METHODE"];
                        textBox6.Text = (string)dt.Rows[0]["RESULT"];
                        textBox1.Text = (string)dt.Rows[0]["OBSERV"];

                        bool rr = false;
                        foreach (RadioButton rd in groupBox3.Controls.OfType<RadioButton>())
                        {
                            if (rd.Text == (string)dt.Rows[0]["TYPE_ANAL"])
                            {
                                rr = true;
                                rd.Checked = true;
                                break;
                            }
                        }
                        if (!rr)
                        {
                            radioButton10.Checked = true;
                            textBox4.Text = (string)dt.Rows[0]["TYPE_ANAL"];
                        }
                        else
                        {
                            textBox4.Clear();
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
            if (dataGridView22.Rows[e.RowIndex].Selected)
            {
                dataGridView2_SelectionChanged(null, null);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView22.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Sures de supprimer (" + dataGridView22.SelectedRows.Count + ") bilan ?", "Confirmation : ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string dq = "";
                    dataGridView22.SelectedRows.Cast<DataGridViewRow>().ForEach(row => { dq += "," + row.Cells["ID"].Value; });
                    dq = dq.Substring(1, dq.Length - 1);
                    PreConnection.Excut_Cmd(3, "tb_labo_autre", null, null, "ID IN (@P_ID)",new List<string> { "P_ID"},new List<object> { dq} );
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
            if (label20.Text.Length == 0)
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
                dt.Rows.Add(new object[] { "POIDS", Anim_poids > 0 ? Anim_poids.ToString("N2") : "" });


                dt.Rows.Add(new object[] { "CLIENT_NUM_CNI", (string)selected_animm.Cells["CLIENT_NUM_CNI"].Value });
                dt.Rows.Add(new object[] { "CLIENT_ADRESS", (string)selected_animm.Cells["CLIENT_ADRESS"].Value });
                dt.Rows.Add(new object[] { "CLIENT_CITY", (string)selected_animm.Cells["CLIENT_CITY"].Value });
                dt.Rows.Add(new object[] { "CLIENT_WILAYA", (string)selected_animm.Cells["CLIENT_WILAYA"].Value });
                dt.Rows.Add(new object[] { "CLIENT_NUM_PHONE", (string)selected_animm.Cells["CLIENT_NUM_PHONE"].Value });
                dt.Rows.Add(new object[] { "CLIENT_EMAIL", (string)selected_animm.Cells["CLIENT_EMAIL"].Value });

                dt.Rows.Add(new object[] { "TYPE_ANAL", current_analys_type });
                dt.Rows.Add(new object[] { "TECHN", textBox5.Text });
                dt.Rows.Add(new object[] { "RESULTT", textBox6.Text });
                //-------------
                new Print_report("autre", dt, null).ShowDialog();
            }

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox5.BackColor = SystemColors.Window;
            button5.Visible = false;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox6.BackColor = SystemColors.Window;
            button5.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ((RadioButton)sender).Font = new Font(((RadioButton)sender).Font, ((RadioButton)sender).Checked ? FontStyle.Bold : FontStyle.Regular);
            ((RadioButton)sender).ForeColor = ((RadioButton)sender).Checked ? Color.Sienna : SystemColors.ControlText;
            if (((RadioButton)sender).Name == "radioButton10")
            {
                textBox4.Enabled = ((RadioButton)sender).Checked;
                current_analys_type = textBox4.Text;
            }
            else
            {
                current_analys_type = ((RadioButton)sender).Text;
            }
            button5.Visible = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
            {
                current_analys_type = textBox4.Text;
            }
            textBox4.BackColor = SystemColors.Window;
            button5.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button5.Visible = false;
        }

        private void Autre_Lab_Enter(object sender, EventArgs e)
        {
            if (IDD_to_select != null)
            {
                if (IDD_to_select.Trim().Length > 0)
                {
                    dataGridView22.SelectionChanged -= dataGridView2_SelectionChanged;
                    dataGridView22.ClearSelection();
                    dataGridView22.SelectionChanged += dataGridView2_SelectionChanged;
                    dataGridView22.Rows.Cast<DataGridViewRow>()
                                 .Where(row => row.Cells["ID"].Value.ToString() == IDD_to_select)
                                 .ToList()
                                 .ForEach(row => row.Selected = true);
                }
                else
                {
                    button3.PerformClick();
                }
                IDD_to_select = null;
            }
            else
            {
                button3.PerformClick();
            }
        }
    }
}
