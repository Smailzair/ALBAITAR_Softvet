using ALBAITAR_Softvet.Resources;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Add_Modif_Vaccin : Form
    {
        static public ListViewItem[] Animm2;
        static public ListViewItem[] Animm;
        static public ListViewItem[] Clientss2;
        static public ListViewItem[] Clientss;
        public Add_Modif_Vaccin()
        {
            InitializeComponent();
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
        }

        private void checkBox9_CheckedChanged(object sender, System.EventArgs e)
        {
            comboBox6.Enabled = checkBox9.Checked;
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
            Clients_List lsst = new Clients_List();
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
    }
}
