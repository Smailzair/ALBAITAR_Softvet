using ALBAITAR_Softvet.Resources;
using MySqlX.XDevAPI;
using ServiceStack;
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
    
    public partial class Anims_List_Search : Form
    {

        public event EventHandler<DataTableEventArgs> DataTableReturned;
        private DataTable RESULT;
        public DataTable RETURNED_RESULT
        {
            get { return RESULT; }
            set { RESULT = value; }
        }

        DataTable props;
        List<ListViewItem> items2 = new List<ListViewItem>();
        public Anims_List_Search()
        {
            InitializeComponent();
            //-------------------------
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filter = textBox1.Text.ToLower();

            foreach (ListViewItem item in items2)
            {
                if (item.SubItems[0].Text.ToLower().Contains(filter) || item.SubItems[2].Text.ToLower().Contains(filter) || item.SubItems[4].Text.ToLower().Contains(filter) || textBox1.Text.Trim().Length == 0)
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
            listView1.Visible = listView1.Items.Count > 0;
        }
      

        private void button2_Click(object sender, EventArgs e)
        {
            RESULT = new DataTable();
            RESULT.Columns.Add("NME");
            RESULT.Columns.Add("ID");
            RESULT.Columns.Add("CLIENT_FULL_NME");
            RESULT.Columns.Add("CLIENT_ID");
            RESULT.Columns.Add("NUM_IDENTIF_ANIM");
            if(listView1.SelectedItems.Count > 0 && listView1.Visible)
            {
                RESULT.Rows.Add(listView1.SelectedItems[0].SubItems[0].Text,
                    listView1.SelectedItems[0].SubItems[1].Text,
                    listView1.SelectedItems[0].SubItems[2].Text,
                    listView1.SelectedItems[0].SubItems[3].Text,
                    listView1.SelectedItems[0].SubItems[4].Text
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
            DataTableReturned?.Invoke(this, new DataTableEventArgs(RESULT));
        }

        private void Anims_List_Load(object sender, EventArgs e)
        {

                props = PreConnection.Load_data("SELECT tb1.ID,tb1.NME,tb1.`NUM_IDENTIF` AS 'NUM_IDENTIF_ANIM',tb1.CLIENT_ID,CONCAT(tb2.FAMNME, ' ', tb2.NME) AS CLIENT_FULL_NME FROM tb_animaux AS tb1 LEFT JOIN tb_clients AS tb2 ON tb1.`CLIENT_ID` = tb2.ID;");
                if (props != null)
                {
                    if (props.Rows.Count > 0)
                    {
                        foreach (DataRow row in props.Rows)
                        {
                            ListViewItem dd = new ListViewItem(row["NME"].ToString());
                            dd.SubItems.Add(row["ID"].ToString());
                        dd.SubItems.Add(row["CLIENT_FULL_NME"].ToString());
                        dd.SubItems.Add(row["CLIENT_ID"].ToString());
                        dd.SubItems.Add(row["NUM_IDENTIF_ANIM"].ToString());
                        listView1.Items.Add(dd);
                            items2.Add(dd);
                        }
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
                ((ListView)sender).Columns[0].Width = (((ListView)sender).ClientSize.Width - 1) / 3;
                ((ListView)sender).Columns[2].Width = (((ListView)sender).ClientSize.Width - 1) / 3;
                ((ListView)sender).Columns[4].Width = (((ListView)sender).ClientSize.Width - 1) / 3;
            }

        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle((e.ColumnIndex == 0 ? Brushes.Peru : (e.ColumnIndex == 2 ? Brushes.SaddleBrown : Brushes.SandyBrown)), e.Bounds);
            TextRenderer.DrawText(e.Graphics, e.Header.Text, e.Font, e.Bounds, Color.White, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
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

        private void listView1_ControlAdded(object sender, ControlEventArgs e)
        {
           
        }
    }
    public class DataTableEventArgs : EventArgs
    {
        public DataTable DataTable { get; }

        public DataTableEventArgs(DataTable dataTable)
        {
            DataTable = dataTable;
        }
    }
}
