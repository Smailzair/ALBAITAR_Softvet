using ALBAITAR_Softvet.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xamarin.Forms.Internals;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Add_Modif_Vaccin : Form
    {
        int ID_Modif = -1;
        static public ListViewItem[] Animm2;
        static public ListViewItem[] Animm;
        static public ListViewItem[] Clientss2;
        static public ListViewItem[] Clientss;
        public Add_Modif_Vaccin(int ID)
        {
            InitializeComponent();
            ID_Modif = ID;
        }

        private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                panel1.Visible = radioButton2.Checked;
            }
        }

        private void Add_Modif_Vaccin_Load(object sender, System.EventArgs e)
        {
            dateTimePicker3.MinDate = dateTimePicker2.Value.AddDays(1);
            dateTimePicker3.Value = dateTimePicker2.Value.AddYears(1);
            dateTimePicker6.MinDate = dateTimePicker5.Value;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            if(Main_Frm.Main_Frm_clients_tbl.Rows.Count > 0)
            {
                comboBox6.DataSource = Main_Frm.Main_Frm_clients_tbl;
                comboBox6.DisplayMember = "FULL_NME";
                comboBox6.ValueMember = "ID";
                comboBox6.SelectedIndex = 0;
            }
            else
            {
                checkBox9.Visible = comboBox6.Visible = false;
            }
            //----------------
            if (ID_Modif > 0)
            {
                Text = "Modification de vaccination :";
                button1.Visible = true;
                //------------
                var infos = Main_Frm.Main_Frm_vaccination.AsEnumerable().Where(Z => (int)Z["ID"] == ID_Modif);
                if (infos.Any())
                {
                    DataRow rww = infos.First();
                    textBox1.Text = rww["VACCIN_NME"] != DBNull.Value ? (string)rww["VACCIN_NME"] : "";
                    radioButton2.Checked = (rww["IS_PERIODIC"] != DBNull.Value ? (string)rww["IS_PERIODIC"] : "Non") == "Oui";
                    if (radioButton1.Checked) //FIXED
                    {
                        dateTimePicker4.Value = rww["FIXED_DATE"] != DBNull.Value ? (DateTime)rww["FIXED_DATE"] : DateTime.Now;
                    }
                    else //PERIODIC
                    {
                        int dayy = rww["EVERY_DAY_NB"] != DBNull.Value ? (int)rww["EVERY_DAY_NB"] : 0;
                        int mnth = rww["EVERY_MOUNTH_NB"] != DBNull.Value ? (int)rww["EVERY_MOUNTH_NB"] : 0;
                        dateTimePicker1.Value = dayy > 0 && mnth > 0 ? new DateTime(DateTime.Now.Year, mnth, dayy) : DateTime.Now;
                        dateTimePicker2.Value = rww["START_YEAR"] != DBNull.Value ? new DateTime((int)rww["START_YEAR"], 1, 1) : new DateTime(DateTime.Now.Year, 1, 1);
                        dateTimePicker3.Value = rww["END_YEAR"] != DBNull.Value ? new DateTime((int)rww["END_YEAR"], 12, 31) : new DateTime(DateTime.Now.Year, 12, 31);
                    }
                    comboBox1.SelectedItem = rww["IS_CONCERN_WHO"] != DBNull.Value ? (string)rww["IS_CONCERN_WHO"] : comboBox1.Items[0];
                    if (comboBox1.SelectedIndex == 0)
                    {
                        comboBox2.SelectedIndex = (rww["ANIM_NUM_IDENs"] != DBNull.Value ? (string)rww["ANIM_NUM_IDENs"] : "").Length > 0 ? 0 : 1;
                        if (comboBox2.SelectedIndex == 0)
                        {
                            string[] idents = (rww["ANIM_NUM_IDENs"] != DBNull.Value ? (string)rww["ANIM_NUM_IDENs"] : "").Split(',');
                            var animms = Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(FF => idents.Contains(FF["NUM_IDENTIF"] != DBNull.Value ? (string)FF["NUM_IDENTIF"] : "zzzzzzz"));
                            if (animms.Any())
                            {
                                animms.ForEach(R =>
                                {
                                    int clnt_idd = R["CLIENT_ID"] != DBNull.Value ? (int)R["CLIENT_ID"] : -1;
                                    var clnt_varr = Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Where(EE => (int)EE["ID"] == clnt_idd);
                                    string clnt_nme = clnt_varr.Any() ? (string)clnt_varr.First()["FULL_NME"] : "";
                                    string[] fff = { (string)R["NME"], R["ID"].ToString(), (!string.IsNullOrWhiteSpace(clnt_nme) ? clnt_nme : ""), R["CLIENT_ID"].ToString(), (string)R["NUM_IDENTIF"] };
                                    ListViewItem itm = new ListViewItem(fff);
                                    listView_Anim.Items.Add(itm);
                                });
                                listView_Anim.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                                listView_Anim.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                                listView_Anim.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                            }
                        }
                        else
                        {
                            
                            string espec = rww["ANIM_ESPECE"] != DBNull.Value ? (string)rww["ANIM_ESPECE"] : "";
                            if (espec.Length > 0) { checkBox4.Checked = true; comboBox3.SelectedItem = espec; }
                            string racc = rww["ANIM_RACE"] != DBNull.Value ? (string)rww["ANIM_RACE"] : "";
                            if (racc.Length > 0) { checkBox5.Checked = true; comboBox4.SelectedItem = racc; }
                            string sexx = rww["ANIM_SEXE"] != DBNull.Value ? (string)rww["ANIM_SEXE"] : "";
                            if (sexx.Length > 0) { checkBox6.Checked = true; comboBox5.SelectedItem = sexx; }
                            if(rww["DATE_NISS_MIN"] != DBNull.Value && rww["DATE_NISS_MAX"] != DBNull.Value)
                            {
                                checkBox7.Checked = true;
                                dateTimePicker5.Value = (DateTime)rww["DATE_NISS_MIN"];
                                dateTimePicker6.Value = (DateTime)rww["DATE_NISS_MAX"];
                            }
                            double poidd = rww["POIDS_MAX"] != DBNull.Value ? (double)rww["POIDS_MAX"] : -1;
                            if (poidd > 0) { checkBox8.Checked = true; numericUpDown2.Value = (decimal)poidd; }
                            string[] idents = (rww["RELATED_CLIENTS_IDS"] != DBNull.Value ? (string)rww["RELATED_CLIENTS_IDS"] : "").Split(',');
                            if(idents.Count() > 0)
                            {
                                if (idents[0].Length > 0)
                                {
                                    checkBox9.Checked = true;
                                    comboBox6.SelectedValue = int.Parse(idents[0]);
                                }
                            }
                            
                        }

                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        string[] idents = (rww["RELATED_CLIENTS_IDS"] != DBNull.Value ? (string)rww["RELATED_CLIENTS_IDS"] : "").Split(',');
                        var cltss = Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Where(FF => idents.Contains(FF["ID"] != DBNull.Value ? FF["ID"].ToString() : "-1"));
                        if (cltss.Any())
                        {
                            string[] fff = { (string)cltss.First()["FULL_NME"], cltss.First()["ID"].ToString() };
                            ListViewItem itm = new ListViewItem(fff);
                            listView_Clients.Items.Add(itm);
                        }
                        listView_Clients.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }

                    checkBox10.Checked = (rww["IS_IMPORTANT"] != DBNull.Value ? (string)rww["IS_IMPORTANT"] : "Non") == "Oui";
                    numericUpDown1.Value = rww["ALERT_BEFORE_DAYS"] != DBNull.Value ? (int)rww["ALERT_BEFORE_DAYS"] : 1;
                    checkBox1.Checked = (rww["SEND_ALERT_TO_CABINE_EMAIL"] != DBNull.Value ? (int)rww["SEND_ALERT_TO_CABINE_EMAIL"] : -1) == 1;
                    checkBox2.Checked = (rww["SEND_ALERT_TO_CLIENT_EMAIL"] != DBNull.Value ? (int)rww["SEND_ALERT_TO_CLIENT_EMAIL"] : -1) == 1;
                    richTextBox1.Text = rww["DESCRIPTION"] != DBNull.Value ? (string)rww["DESCRIPTION"] : "";
                }
                else
                {
                    Close();
                }
            }

        }

        private void dateTimePicker2_ValueChanged(object sender, System.EventArgs e)
        {
            dateTimePicker3.MinDate = dateTimePicker2.Value.AddYears(1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            panel4.Visible = comboBox1.SelectedIndex == 0;
            panel8.Visible = comboBox1.SelectedIndex == 1;

        }

        private void dateTimePicker5_ValueChanged(object sender, System.EventArgs e)
        {
            dateTimePicker6.MinDate = dateTimePicker5.Value;
        }

        private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            panel5.Visible = comboBox2.SelectedIndex == 0;
            panel7.Visible = comboBox2.SelectedIndex == 1;
        }

        private void checkBox4_CheckedChanged(object sender, System.EventArgs e)
        {
            comboBox3.Enabled = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, System.EventArgs e)
        {
            comboBox4.Enabled = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, System.EventArgs e)
        {
            comboBox5.Enabled = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, System.EventArgs e)
        {
            dateTimePicker5.Enabled = dateTimePicker6.Enabled = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, System.EventArgs e)
        {
            numericUpDown2.Enabled = checkBox8.Checked;
            numericUpDown2.BackColor = SystemColors.Window;
        }

        private void checkBox9_CheckedChanged(object sender, System.EventArgs e)
        {
            comboBox6.Enabled = checkBox9.Checked;
            checkBox9.ForeColor = Color.Black;
        }

        private void button6_Click(object sender, System.EventArgs e)
        {


            Animm2 = new ListViewItem[listView_Anim.Items.Count];
            for (int i = 0; i < listView_Anim.Items.Count; i++)
            {
                Animm2[i] = listView_Anim.Items[i];
            }
            //--------------
            Anims_List_For_Vaccin ann = new Anims_List_For_Vaccin();
            ann.ShowDialog();
            //--------------
            listView_Anim.Items.Clear();
            if (Animm != null)
            {
                if (Animm.Length > 0)
                {
                    for (int yd = 0; yd < Animm.Length; yd++)
                    {
                        ListViewItem itttm = Animm[yd];
                        Animm[yd].Remove();
                        listView_Anim.Items.Add(itttm);

                    }
                }
            }
            //------------
            foreach (ColumnHeader column in listView_Anim.Columns)
            {
                if (column.Width > 0)
                {
                    column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            //----------------
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            foreach (ListViewItem itttm in listView_Anim.CheckedItems)
            {
                itttm.Remove();
            }
            //----------------
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Clientss2 = new ListViewItem[listView_Clients.Items.Count];
            for (int i = 0; i < listView_Clients.Items.Count; i++)
            {
                Clientss2[i] = listView_Clients.Items[i];
            }
            //--------------
            Clients_List_For_Vaccin lsst = new Clients_List_For_Vaccin();
            lsst.ShowDialog();
            //-------------
            if (Clientss != null)
            {
                listView_Clients.Items.Clear();
                if (Clientss.Length > 0)
                {
                    for (int yd = 0; yd < Clientss.Length; yd++)
                    {
                        ListViewItem itttm = Clientss[yd];
                        Clientss[yd].Remove();
                        listView_Clients.Items.Add(itttm);
                    }
                }
            }

            //----------------
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            foreach (ListViewItem itttm in listView_Clients.CheckedItems)
            {
                itttm.Remove();
            }
            //----------------
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            DateTime FIXED_DATE = new DateTime(1999, 1, 1);
            int START_YEAR = -1;
            int END_YEAR = -1;
            string IS_PERIODIC = "Non";
            int EVERY_DAY_NB = 0;
            int EVERY_MOUNTH_NB = 0;
            string VACCIN_NME;
            string IS_IMPORTANT;
            string IS_CONCERN_WHO;
            string ANIM_NUM_IDENs = "";
            string ANIM_ESPECE = "";
            string ANIM_RACE = "";
            string ANIM_SEXE = "";
            double POIDS_MAX = 0;
            DateTime DATE_NISS_MIN = new DateTime(1999, 1, 1);
            DateTime DATE_NISS_MAX = new DateTime(1999, 1, 1);
            string DESCRIPTION;
            string RELATED_CLIENTS_IDS = "";
            int ALERT_BEFORE_DAYS;
            int SEND_ALERT_TO_CABINE_EMAIL = 0;
            int SEND_ALERT_TO_CLIENT_EMAIL = 0;
            int IS_FOR_ALL = 0;
            //-----------------------------
            bool pret = true;
            string error_msg = string.IsNullOrWhiteSpace(textBox1.Text) ? "\n- Aucun nom de vaccination." : "";
            textBox1.BackColor = string.IsNullOrWhiteSpace(textBox1.Text) ? Color.LightCoral : SystemColors.Window;

            pret &= !string.IsNullOrWhiteSpace(textBox1.Text); //Vaccin Name

            //------------------------
            VACCIN_NME = textBox1.Text; //NME
            IS_IMPORTANT = checkBox10.Checked ? "Oui" : "Non";
            IS_PERIODIC = radioButton1.Checked ? "Non" : "Oui";
            if (radioButton1.Checked) //FIXED DATE
            {
                FIXED_DATE = dateTimePicker4.Value.Date;
                // START_DATE = END_DATE = dateTimePicker4.Value.Date;
                // EVERY_DAY_NB = EVERY_MOUNTH_NB = 0;
            }
            else //PERIODIQUE
            {
                START_YEAR = dateTimePicker2.Value.Year;
                END_YEAR = dateTimePicker3.Value.Year;
                EVERY_DAY_NB = dateTimePicker1.Value.Day;
                EVERY_MOUNTH_NB = dateTimePicker1.Value.Month;
                //if(new DateTime(dateTimePicker2.Value.Year, EVERY_MOUNTH_NB, EVERY_DAY_NB) < START_DATE || new DateTime(dateTimePicker3.Value.Year, EVERY_MOUNTH_NB, EVERY_DAY_NB) > END_DATE)
                //{
                //    pret = false;
                //    error_msg += "\n- Date périodique un correcte.";
                //}
            }
            if (comboBox1.SelectedIndex == 0) //Specific anims
            {
                if (comboBox2.SelectedIndex == 0) //By Nme or N° IDent
                {
                    if (listView_Anim.Items.Count > 0)
                    {
                        listView_Anim.Items.Cast<ListViewItem>().ToList().ForEach(ZZZ => { ANIM_NUM_IDENs += "," + ZZZ.SubItems[4].Text; });
                        ANIM_NUM_IDENs = ANIM_NUM_IDENs.Length > 0 ? ANIM_NUM_IDENs.Substring(1) : ANIM_NUM_IDENs;

                    }
                    else
                    {
                        pret = false;
                        error_msg += "\n- Aucun animal séléctioné.";
                    }
                }
                else //Autre
                {
                    pret &= (checkBox4.Checked || checkBox5.Checked || checkBox6.Checked || checkBox7.Checked || checkBox8.Checked || checkBox9.Checked);
                    error_msg += (checkBox4.Checked || checkBox5.Checked || checkBox6.Checked || checkBox7.Checked || checkBox8.Checked || checkBox9.Checked) ? "" : "\n- Aucun choix coché.";
                    pret = checkBox8.Checked && numericUpDown2.Value == 0 ? false : pret;
                    numericUpDown2.BackColor = checkBox8.Checked && numericUpDown2.Value == 0 ? Color.LightCoral : SystemColors.Window;
                    pret = checkBox8.Checked && numericUpDown2.Value == 0 ? false : pret;
                    checkBox9.ForeColor = checkBox9.Checked && comboBox6.Items.Count == 0 ? Color.LightCoral : Color.Black;
                    pret = checkBox9.Checked && comboBox6.Items.Count == 0 ? false : pret;

                    ANIM_ESPECE = checkBox4.Checked ? comboBox3.Text : "";
                    ANIM_RACE = checkBox5.Checked ? comboBox4.Text : "";
                    ANIM_SEXE = checkBox6.Checked ? comboBox5.Text : "";
                    if (checkBox7.Checked) //NISS LIMIT
                    {
                        DATE_NISS_MIN = dateTimePicker5.Value.Date;
                        DATE_NISS_MAX = dateTimePicker6.Value.Date;
                    }
                    POIDS_MAX = checkBox8.Checked ? double.Parse(numericUpDown2.Value.ToString()) : 0;
                    if(checkBox9.Visible && checkBox9.Checked) { RELATED_CLIENTS_IDS = comboBox6.SelectedValue.ToString(); }
                }
            }
            else if (comboBox1.SelectedIndex == 1) //Specific Clients
            {
                if (listView_Clients.Items.Count > 0)
                {
                    listView_Clients.Items.Cast<ListViewItem>().ToList().ForEach(ZZZ => { RELATED_CLIENTS_IDS += "," + ZZZ.SubItems[1].Text; });
                    RELATED_CLIENTS_IDS = RELATED_CLIENTS_IDS.Length > 0 ? RELATED_CLIENTS_IDS.Substring(1) : RELATED_CLIENTS_IDS;

                }
                else
                {
                    pret = false;
                    error_msg += "\n- Aucun propriétaire séléctioné.";

                }
            }
            else //All Anims
            {
                IS_FOR_ALL = 1;
            }

            IS_CONCERN_WHO = comboBox1.Text;
            DESCRIPTION = richTextBox1.Text;
            ALERT_BEFORE_DAYS = (int)numericUpDown1.Value;
            SEND_ALERT_TO_CABINE_EMAIL = checkBox2.Checked ? 1 : 0;
            SEND_ALERT_TO_CLIENT_EMAIL = checkBox1.Checked ? 1 : 0;
            //-------------------------
            if (pret)
            {
                if (ID_Modif > 0) //UPDATE
                {
                    PreConnection.Excut_Cmd(2, "tb_vaccin", new List<string> {
                        "FIXED_DATE",
                        "START_YEAR",
                        "END_YEAR",
                        "IS_PERIODIC",
                        "EVERY_DAY_NB",
                        "EVERY_MOUNTH_NB",
                        "VACCIN_NME",
                        "IS_IMPORTANT",
                        "IS_CONCERN_WHO",
                        "ANIM_NUM_IDENs",
                        "ANIM_ESPECE",
                        "ANIM_RACE",
                        "ANIM_SEXE",
                        "POIDS_MAX",
                        "DATE_NISS_MIN",
                        "DATE_NISS_MAX",
                        "DESCRIPTION",
                        "RELATED_CLIENTS_IDS",
                        "ALERT_BEFORE_DAYS",
                        "SEND_ALERT_TO_CABINE_EMAIL",
                        "SEND_ALERT_TO_CLIENT_EMAIL",
                        "LAST_ALERT_EMAIL_CABINET_SENT_DATE",
                        "LAST_ALERT_EMAIL_CLIENT_SENT_DATE",
                        "IS_FOR_ALL",
                        "LAST_ALERT_LUE_DATE"
                    }, new List<object>
                    {
                        FIXED_DATE != new DateTime(1999, 1, 1) ? FIXED_DATE : (object)DBNull.Value,
                        START_YEAR > 0 ? START_YEAR : (object)DBNull.Value,
                        END_YEAR > 0 ? END_YEAR : (object)DBNull.Value,
IS_PERIODIC,
EVERY_DAY_NB,
EVERY_MOUNTH_NB,
VACCIN_NME,
IS_IMPORTANT,
IS_CONCERN_WHO,
ANIM_NUM_IDENs,
ANIM_ESPECE,
ANIM_RACE,
ANIM_SEXE,
POIDS_MAX,
DATE_NISS_MIN != new DateTime(1999, 1, 1) ? DATE_NISS_MIN : (object)DBNull.Value,
DATE_NISS_MAX != new DateTime(1999, 1, 1) ? DATE_NISS_MAX : (object)DBNull.Value,
DESCRIPTION,
RELATED_CLIENTS_IDS,
ALERT_BEFORE_DAYS,
SEND_ALERT_TO_CABINE_EMAIL,
SEND_ALERT_TO_CLIENT_EMAIL,
DBNull.Value,
DBNull.Value,
IS_FOR_ALL,
DBNull.Value
                    }, "ID = @ID", new List<string> { "ID" }, new List<object> { ID_Modif });
                }
                else //INSERT
                {
                    PreConnection.Excut_Cmd(1, "tb_vaccin", new List<string> {
                        "FIXED_DATE",
                        "START_YEAR",
                        "END_YEAR",
                        "IS_PERIODIC",
                        "EVERY_DAY_NB",
                        "EVERY_MOUNTH_NB",
                        "VACCIN_NME",
                        "IS_IMPORTANT",
                        "IS_CONCERN_WHO",
                        "ANIM_NUM_IDENs",
                        "ANIM_ESPECE",
                        "ANIM_RACE",
                        "ANIM_SEXE",
                        "POIDS_MAX",
                        "DATE_NISS_MIN",
                        "DATE_NISS_MAX",
                        "DESCRIPTION",
                        "RELATED_CLIENTS_IDS",
                        "ALERT_BEFORE_DAYS",
                        "SEND_ALERT_TO_CABINE_EMAIL",
                        "SEND_ALERT_TO_CLIENT_EMAIL",
                        "IS_FOR_ALL"
                    }, new List<object>
                    {
                        FIXED_DATE != new DateTime(1999, 1, 1) ? FIXED_DATE : (object)DBNull.Value,
                        START_YEAR > 0 ? START_YEAR : (object)DBNull.Value,
                        END_YEAR > 0 ? END_YEAR : (object)DBNull.Value,
IS_PERIODIC,
EVERY_DAY_NB,
EVERY_MOUNTH_NB,
VACCIN_NME,
IS_IMPORTANT,
IS_CONCERN_WHO,
ANIM_NUM_IDENs,
ANIM_ESPECE,
ANIM_RACE,
ANIM_SEXE,
POIDS_MAX,
DATE_NISS_MIN != new DateTime(1999, 1, 1) ? DATE_NISS_MIN : (object)DBNull.Value,
DATE_NISS_MAX != new DateTime(1999, 1, 1) ? DATE_NISS_MAX : (object)DBNull.Value,
DESCRIPTION,
RELATED_CLIENTS_IDS,
ALERT_BEFORE_DAYS,
SEND_ALERT_TO_CABINE_EMAIL,
SEND_ALERT_TO_CLIENT_EMAIL,
IS_FOR_ALL
                    }, null, null, null);
                }
                Vaccinations.theres_changes = true;
                Vaccination.make_refresh = true;
                Close();
            }
            else if (error_msg.Length > 0)
            {
                MessageBox.Show(error_msg, "Veuillez verifier ces points : ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            textBox1.BackColor = SystemColors.Window;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.BackColor = SystemColors.Window;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox9.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sûres de faire la suppression ?", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                PreConnection.Excut_Cmd(3, "tb_vaccin", null, null, "ID = @IDDs", new List<string> { "IDDs" }, new List<object> { ID_Modif });
                Vaccinations.theres_changes = true;
                Vaccination.make_refresh = true;
                Close();
            }
        }
    }
}
