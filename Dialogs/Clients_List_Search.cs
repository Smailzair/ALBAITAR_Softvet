using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
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
        List<ListViewItem> items2 = new List<ListViewItem>();
        public Clients_List_Search()
        {
            InitializeComponent();
            //-------------------------
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filter = Regex.Replace(textBox1.Text.ToLower(), Main_Frm.client_mr_pattern, "", RegexOptions.IgnoreCase).Replace(".", "").Replace("'", "''").Trim();

            foreach (ListViewItem item in items2)
            {
                if (item.SubItems[1].Text.ToLower().Contains(filter) || item.SubItems[2].Text.ToLower().Contains(filter) || textBox1.Text.Trim().Length == 0)
                {

                    if (!listView1.Items.Contains(item))
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
            label1.Visible = listView1.Items.Count == 0;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            RESULT = new DataTable();
            RESULT.Columns.Add("ID");
            RESULT.Columns.Add("FULL_NME");
            RESULT.Columns.Add("NUM_CNI");
            if (listView1.SelectedItems.Count > 0)
            {
                RESULT.Rows.Add(listView1.SelectedItems[0].SubItems[0].Text,
                    listView1.SelectedItems[0].SubItems[1].Text,
                    listView1.SelectedItems[0].SubItems[2].Text
                    );
            }
            else
            {
                RESULT = null;
            }
            Close();
        }

        private void Clients_List_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataTableReturned?.Invoke(this, new DataTableEventArgs_Clients(RESULT));
        }

        private void load_data()
        {
            items2.Clear();
            listView1.Items.Clear();
            //----------
            props = PreConnection.Load_data("SELECT *,concat(FAMNME,' ',NME) AS FULL_NME FROM tb_clients tb1" + (checkBox1.Checked ? " WHERE ID IN (SELECT CLIENT_ID FROM tb_clients_finance GROUP BY CLIENT_ID HAVING SUM(DEBIT)-SUM(CREDIT) <> 0)" : "") + (checkBox2.Checked ? (checkBox1.Checked ? " AND " : " WHERE ") + "(SELECT COUNT(*) FROM tb_animaux WHERE CLIENT_ID = tb1.ID) = 0" : "") + " ORDER BY FULL_NME;");
            if (props != null)
            {
                if (props.Rows.Count > 0)
                {
                    label1.Visible = false;
                    foreach (DataRow row in props.Rows)
                    {
                        ListViewItem dd = new ListViewItem(row["ID"].ToString());
                        dd.SubItems.Add(row["FULL_NME"].ToString());
                        dd.SubItems.Add(row["NUM_CNI"].ToString());
                        listView1.Items.Add(dd);
                        items2.Add(dd);
                    }

                }
                else
                {
                    label1.Visible = true;
                }
            }
            else
            {
                label1.Visible = true;
            }
        }
        private void Anims_List_Load(object sender, EventArgs e)
        {

            load_data();
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).Columns.Count > 0)
            {
                ((ListView)sender).Columns[1].Width = (((ListView)sender).ClientSize.Width - 1) / 2;
                ((ListView)sender).Columns[2].Width = (((ListView)sender).ClientSize.Width - 1) / 2;
            }

        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(e.Header.Text, ((ListView)sender).Font, Brushes.White, e.Bounds, stringFormat);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(Brushes.LightSeaGreen, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
            }
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, e.SubItem.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            load_data();
            textBox1_TextChanged(null, null);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2.PerformClick();
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
