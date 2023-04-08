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
    public partial class Agenda_Filter : Form
    {
        
        DataTable props;
        int selected_items_count = 0;
        List<ListViewItem> items2 = new List<ListViewItem>();
        bool btn2_enabled_first_time = false;
        bool thers_modif = false;
        public Agenda_Filter()
        {
            InitializeComponent();
            //-------------------------
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filter = textBox1.Text.ToLower();

            foreach (ListViewItem item in items2)
            {
                if (item.SubItems[0].Text.ToLower().Contains(filter) || item.SubItems[2].Text.ToLower().Contains(filter) || textBox1.Text.Trim().Length == 0)
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
            button2.Enabled = selected_items_count > 0 ? true : btn2_enabled_first_time;
            button2.Text = "OK " + (button2.Enabled ? "[" + selected_items_count + "]" : "");
            thers_modif = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Agenda.Animm = new ListViewItem[listView2.Items.Count];
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                Agenda.Animm[i] = listView2.Items[i];
            }

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

        private void Anims_List_Load(object sender, EventArgs e)
        {
            //if (CLNTS != null)
            //{
            //    string cmmd = "";
            //    CLNTS.ForEach(x => {
            //        cmmd += "," + x;
            //    });
            //    cmmd = cmmd.Substring(1, cmmd.Length - 1);
                props = PreConnection.Load_data("SELECT tb1.ID,tb1.NME,tb1.CLIENT_ID,CONCAT(tb2.FAMNME, ' ', tb2.NME) AS CLIENT_FULL_NME FROM tb_animaux AS tb1 LEFT JOIN tb_clients AS tb2 ON tb1.`CLIENT_ID` = tb2.ID;");
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
                            listView1.Items.Add(dd);
                            items2.Add(dd);
                        }

                        if (Agenda.Animm2.Length > 0)
                        {
                            for (int dd = 0; dd < Agenda.Animm2.Length; dd++)
                            {
                                ListViewItem item = listView1.Items.Cast<ListViewItem>().Where(XX => XX.SubItems[0].Text == Agenda.Animm2[dd].SubItems[0].Text).FirstOrDefault();
                                listView1.Items.Remove(item);
                                items2.Remove(item);
                                listView2.ItemSelectionChanged -= listView2_ItemSelectionChanged;
                                listView2.Items.Add(item);
                                listView2.SelectedIndices.Clear();
                                listView2.ItemSelectionChanged += listView2_ItemSelectionChanged;
                                selected_items_count++;
                            }
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
            //}
            //else
            //{
            //    MessageBox.Show("Veuillez sélectionner d'abord un propriétaire, puis réessayer.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    Close();
            //}

            
            
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).Columns.Count > 0)
            {
                ((ListView)sender).Columns[0].Width = (((ListView)sender).ClientSize.Width - 1) / 2;
                ((ListView)sender).Columns[2].Width = (((ListView)sender).ClientSize.Width - 1) / 2;
            }

        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle((e.ColumnIndex == 0 ? Brushes.Peru : Brushes.SaddleBrown), e.Bounds);
            if (e.ColumnIndex == 0 || e.ColumnIndex == 2)
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
            if (e.ColumnIndex == 0 || e.ColumnIndex == 2)
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = checkBox1.Checked;
        }
    }
}
