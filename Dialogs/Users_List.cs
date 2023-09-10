using ALBAITAR_Softvet.Resources;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Users_List : Form
    {

        DataTable users;
        int selected_items_count = 0;
        
        List<ListViewItem> items2 = new List<ListViewItem>();
        bool btn2_enabled_first_time = false;
        bool thers_modif = false;
        public Users_List()
        {
            InitializeComponent();
            //-------------------------
            //props = PreConnection.Load_data("SELECT ID, concat(FAMNME, ' ', NME) AS FULL_NME FROM tb_clients;");
            users = PreConnection.Load_data("SELECT ID ,CONCAT(IF(SEX = 'F','Mme. ','Mr. '),`USER_NME`,' ',`USER_FAMNME`) AS FULL_NME FROM tb_login_and_users;");

            foreach (DataRow row in users.Rows)
            {
                ListViewItem dd = new ListViewItem(row["FULL_NME"].ToString());
                dd.SubItems.Add(row["ID"].ToString());
                listView1.Items.Add(dd);
                items2.Add(dd);
            }

            if(Agenda.Userss.Length > 0)
            {

                string[] separator3 = { "," };
                List<string> mnth = Agenda.Userss.Split(separator3, StringSplitOptions.RemoveEmptyEntries).ToList();
                mnth.ForEach(AA =>
                {
                    ListViewItem item = listView1.Items.Cast<ListViewItem>().Where(XX => XX.SubItems[1].Text == AA).FirstOrDefault();
                    listView1.Items.Remove(item);
                    items2.Remove(item);
                    listView2.ItemSelectionChanged -= listView2_ItemSelectionChanged;
                    listView2.Items.Add(item);
                    listView2.SelectedIndices.Clear();
                    listView2.ItemSelectionChanged += listView2_ItemSelectionChanged;
                    selected_items_count++;
                });
            }


            button2.Text = "OK [" + (selected_items_count > 0 ? selected_items_count.ToString() : "Tout le monde") + "]";
            btn2_enabled_first_time = button2.Enabled;

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filter = textBox1.Text.ToLower().Replace("'", "''");

            foreach (ListViewItem item in items2)
            {
                if (item.SubItems[0].Text.ToLower().Contains(filter) || textBox1.Text.Trim().Length == 0)
                {

                    if (!listView1.Items.Contains(item) && !listView2.Items.Contains(item))
                    {
                        listView1.Items.Add(item);
                    }
                }
                else
                {
                    if (listView1.Items.Contains(item))
                    {
                        listView1.Items.Remove(item);
                    }
                }
            }

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                listView1.Items.Remove(item);
                items2.Remove(item);
                listView2.ItemSelectionChanged -= listView2_ItemSelectionChanged;
                listView2.Items.Add(item);
                listView2.SelectedIndices.Clear();
                listView2.ItemSelectionChanged += listView2_ItemSelectionChanged;
                selected_items_count++;
            }
            
            button2.Text = "OK [" + (selected_items_count > 0 ? selected_items_count.ToString() : "Tout le monde") + "]";
            thers_modif = true;
        }

        private void listView2_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ListViewItem item = listView2.SelectedItems[0];
                listView2.Items.Remove(item);
                items2.Add(item);
                listView1.ItemSelectionChanged -= listView1_ItemSelectionChanged;
                listView1.Items.Add(item);
                listView1.SelectedIndices.Clear();
                listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
                selected_items_count--;

            }
            button2.Text = "OK [" + (selected_items_count > 0 ? selected_items_count.ToString() : "Tout le monde") + "]";
            thers_modif = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Agenda.Userss = string.Empty;
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                Agenda.Userss += ',' + listView2.Items[i].SubItems[1].Text;
            }
            if(Agenda.Userss.Length > 0) { Agenda.Userss = Agenda.Userss.Substring(1, Agenda.Userss.Length - 1); }
            thers_modif = false; //to prevent "Clients_List_FormClosing";
            Close();
        }

        private void Clients_List_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thers_modif)
            {
                e.Cancel = MessageBox.Show("Vous n'avez pas enregistrer les modifications ! \nSuivez-vous comme méme ?", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No;
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
