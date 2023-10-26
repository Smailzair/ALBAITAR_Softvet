using ALBAITAR_Softvet.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        private void checkBox3_CheckedChanged(object sender, System.EventArgs e)
        {
            numericUpDown1.Enabled = checkBox1.Visible = checkBox2.Visible = checkBox3.Checked;
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
            //----------------
            if (ID_Modif > 0)
            {
                Text = "Modification de vaccination :";
            }

        }

        private void dateTimePicker2_ValueChanged(object sender, System.EventArgs e)
        {
            dateTimePicker3.MinDate = dateTimePicker2.Value.AddDays(1);
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
            DateTime START_DATE;
            DateTime END_DATE;
            int EVERY_TYPE_0_FIXED_1_PERIDIC;
            int EVERY_DAY_NB;
            int EVERY_MOUNTH_NB;
            string VACCIN_NME;
            int IS_IMPORTANT;
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

            //-----------------------------
            bool pret = true;
            string error_msg = "";
            textBox1.BackColor = string.IsNullOrWhiteSpace(textBox1.Text) ? Color.LightCoral : SystemColors.Window;

            pret &= !string.IsNullOrWhiteSpace(textBox1.Text); //Vaccin Name

            //------------------------
            VACCIN_NME = textBox1.Text; //NME
            IS_IMPORTANT = checkBox10.Checked ? 1 : 0;
            EVERY_TYPE_0_FIXED_1_PERIDIC = radioButton1.Checked ? 0 : 1;
            if (radioButton1.Checked) //FIXED DATE
            {
                START_DATE = END_DATE = dateTimePicker4.Value.Date;
                EVERY_DAY_NB = EVERY_MOUNTH_NB = 0;
            }
            else //PERIODIQUE
            {
                START_DATE = new DateTime(dateTimePicker2.Value.Year, 1, 1);
                END_DATE = new DateTime(dateTimePicker3.Value.Year, 12, 31);
                EVERY_DAY_NB = dateTimePicker1.Value.Day;
                EVERY_MOUNTH_NB = dateTimePicker1.Value.Month;
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
            //else //All Anims
            //{

            //}

            IS_CONCERN_WHO = comboBox1.Text;
            DESCRIPTION = richTextBox1.Text;
            ALERT_BEFORE_DAYS = checkBox3.Checked ? (int)numericUpDown1.Value : 0;
            SEND_ALERT_TO_CABINE_EMAIL = checkBox3.Checked && checkBox2.Checked ? 1 : 0;
            SEND_ALERT_TO_CLIENT_EMAIL = checkBox3.Checked && checkBox1.Checked ? 1 : 0;
            //-------------------------
            if (pret)
            {
                if (ID_Modif > 0) //UPDATE
                {

                }
                else //INSERT
                {
                    PreConnection.Excut_Cmd(1, "tb_vaccin", new List<string> {
                        "START_DATE",
                        "END_DATE",
                        "EVERY_TYPE_0_FIXED_1_PERIDIC",
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
                        "SEND_ALERT_TO_CLIENT_EMAIL"
                    }, new List<object>
                    {
                        START_DATE,
END_DATE,
EVERY_TYPE_0_FIXED_1_PERIDIC,
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
SEND_ALERT_TO_CLIENT_EMAIL
                    },null,null,null);
                }
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
    }
}
