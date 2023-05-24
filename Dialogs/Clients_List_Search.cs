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
    
    public partial class Clients_List_Search : Form
    {

        public event EventHandler<DataTableEventArgs_Clients> DataTableReturned;
        private DataTable RESULT;
        public DataTable RETURNED_RESULT
        {
            get { return RESULT; }
            set { RESULT = value; }
        }

        DataTable props;
        int selected_items_count = 0;
        int max_nb_to_select = 999999999;
        List<ListViewItem> items2 = new List<ListViewItem>();
        bool btn2_enabled_first_time = false;
        public Clients_List_Search(int? select_nb)
        {
            if (select_nb != null)
            {
                max_nb_to_select = (int)select_nb;
            }
            InitializeComponent();
            //-------------------------
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filter = textBox1.Text.ToLower();

            foreach (ListViewItem item in items2)
            {
                if (item.SubItems[1].Text.ToLower().Contains(filter) || textBox1.Text.Trim().Length == 0)
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
            button2.Enabled = selected_items_count > 0 ? true : btn2_enabled_first_time;
            button2.Text = "OK " + (button2.Enabled ? "[" + selected_items_count + "]" : "");
            listView1.Enabled = listView1.Visible = (max_nb_to_select - listView2.Items.Count) > 0;
            label2.Visible = (max_nb_to_select - listView2.Items.Count) <= 0;

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
            button2.Enabled = selected_items_count > 0 ? true : btn2_enabled_first_time;
            button2.Text = "OK " + (button2.Enabled ? "[" + selected_items_count + "]" : "");
            listView1.Enabled = listView1.Visible = (max_nb_to_select - listView2.Items.Count) > 0;
            label2.Visible = (max_nb_to_select - listView2.Items.Count) <= 0;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RESULT = new DataTable();
            RESULT.Columns.Add("ID");
            RESULT.Columns.Add("FULL_NME");
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                RESULT.Rows.Add(listView2.Items[i].SubItems[0].Text,
                    listView2.Items[i].SubItems[1].Text
                    );
            }
            Close();
        }

        private void Clients_List_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataTableReturned?.Invoke(this, new DataTableEventArgs_Clients(RESULT));
        }

        private void Anims_List_Load(object sender, EventArgs e)
        {

                props = PreConnection.Load_data("SELECT ID, concat(FAMNME, ' ', NME) AS FULL_NME FROM tb_clients;");
                if (props != null)
                {
                    if (props.Rows.Count > 0)
                    {
                        foreach (DataRow row in props.Rows)
                        {
                        ListViewItem dd = new ListViewItem(row["ID"].ToString());
                        dd.SubItems.Add(row["FULL_NME"].ToString());
                        listView1.Items.Add(dd);
                        items2.Add(dd);
                    }

                        button2.Enabled = selected_items_count > 0;
                        button2.Text = "OK " + (button2.Enabled ? "[" + selected_items_count + "]" : "");



                        btn2_enabled_first_time = button2.Enabled;
                    }
                    else
                    {
                        MessageBox.Show("Aucun animal trouvé !", ".", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Aucun animal trouvé !", ".", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }           
            
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).Columns.Count > 0)
            {
                ((ListView)sender).Columns[1].Width = (((ListView)sender).ClientSize.Width - 1);
            }

        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Peru, e.Bounds);
            if (e.ColumnIndex == 1)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(e.Header.Text, ((ListView)sender).Font, Brushes.White, e.Bounds, stringFormat);
            }else
            {
                e.DrawDefault = true;
            }
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
            if (e.ColumnIndex == 1)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(e.SubItem.Text, ((ListView)sender).Font, Brushes.Black, e.Bounds, stringFormat);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        


    }
    public class DataTableEventArgs_Clients : EventArgs
    {
        public DataTable DataTable { get; }

        public DataTableEventArgs_Clients(DataTable dataTable)
        {
            DataTable = dataTable;
        }
    }
}
