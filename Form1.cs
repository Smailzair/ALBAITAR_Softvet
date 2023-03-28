using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Form1 : Form
    {
        ImageList items_icon = new ImageList();
        DataTable infos = new DataTable();
        public Form1()
        {
            InitializeComponent();
            //----------------------
            items_icon.Images.Add("agenda_001", Properties.Resources.agenda_001);
            items_icon.Images.Add("agenda_002", Properties.Resources.agenda_002);
            items_icon.Images.Add("agenda_003", Properties.Resources.agenda_003);
            items_icon.Images.Add("agenda_004", Properties.Resources.agenda_004);
            items_icon.Images.Add("agenda_005", Properties.Resources.agenda_005);
            items_icon.Images.Add("agenda_006", Properties.Resources.agenda_006);
            items_icon.Images.Add("agenda_007", Properties.Resources.agenda_007);
            items_icon.Images.Add("Feline", Properties.Resources.Feline);
            items_icon.Images.Add("Caprine", Properties.Resources.Caprine);
            items_icon.Images.Add("Equine", Properties.Resources.Equine);
            items_icon.Images.Add("Canine", Properties.Resources.Canine); 
            items_icon.Images.Add("Oiseaux", Properties.Resources.Oiseaux);
            items_icon.Images.Add("Ovine", Properties.Resources.Ovine);
            items_icon.Images.Add("Rongeur", Properties.Resources.Rongeur);
            items_icon.Images.Add("Reptile", Properties.Resources.Reptile);
            //--------------------------------
            infos = PreConnection.Load_data("SELECT * FROM tb_agenda;");
            //-------------------------------
            listView1_SizeChanged(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.SmallImageList = items_icon;
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
           if(sender != null)
            {
                if (((ListView)sender).Columns.Count > 0)
                {
                    int totalWidth = ((ListView)sender).Columns.Cast<ColumnHeader>().Sum(c => c.Width); // Get the total width of all columns
                    int newFirstColumnWidth = ((ListView)sender).ClientSize.Width - (totalWidth - ((ListView)sender).Columns[0].Width); // Calculate the new width of the first column
                    ((ListView)sender).Columns[0].Width = newFirstColumnWidth; // Set the new width of the first column
                }
                    
            }
            

        }


        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            e.Item.ForeColor = e.Item.Checked ? Color.Green : SystemColors.WindowText;
        }
        int ppp = -1;
        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (((ListView)sender).SelectedItems.Count > 0 && e.Button == MouseButtons.Left)
            {
                ((ListView)sender).CheckBoxes = true;
                ListViewItem item = ((ListView)sender).GetItemAt(e.X, e.Y);
                if (item != null && !item.Selected)
                {
                    item.Selected = true;
                }
            }
            //==============================
            ListViewItem itemm = ((ListView)sender).HitTest(e.Location).Item;
            if (itemm != null)//&& ppp != itemm.Index)
            {
                ppp = itemm.Index;
                //toolTip1.ToolTipTitle = "Item Details";
                //toolTip1.ToolTipIcon = ToolTipIcon.Info;

                toolTip1.SetToolTip(((ListView)sender), itemm.ToolTipText);
                toolTip1.Show(itemm.ToolTipText, ((ListView)sender), e.Location);

            }
            else
            {
                //ppp = -1;
                //toolTip1.Hide(((ListView)sender));
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedItems.Count > 0)
            {
                textBox1.Text = ((ListView)sender).SelectedItems[0].SubItems.Count > 1 ? ((ListView)sender).SelectedItems[0].SubItems[1].Text : ""; //Get the ID
            }

        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (((ListView)sender).CheckBoxes && ((ListView)sender).SelectedItems.Count > 0)
            {
                if (((ListView)sender).SelectedItems.Count == 1)
                {
                    ((ListView)sender).SelectedItems[0].Checked = !((ListView)sender).SelectedItems[0].Checked;
                }
                else
                {
                    if (((ListView)sender).SelectedItems.Cast<ListViewItem>().Where(zz => zz.Checked).ToList().Count == ((ListView)sender).SelectedItems.Count || ((ListView)sender).SelectedItems.Cast<ListViewItem>().Where(zz => !zz.Checked).ToList().Count == ((ListView)sender).SelectedItems.Count)
                    {
                        foreach (ListViewItem ittm in ((ListView)sender).SelectedItems)
                        {
                            ittm.Checked = !ittm.Checked;
                        }
                    }
                    else
                    {
                        foreach (ListViewItem ittm in ((ListView)sender).SelectedItems)
                        {
                            ittm.Checked = true;
                        }
                    }

                }
            }

            if (((ListView)sender).CheckBoxes) {
                ((ListView)sender).SelectedItems.Clear();
                if (((ListView)sender).CheckedItems.Count == 0)
                {
                    ((ListView)sender).CheckBoxes = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Load_day_events(listView1, textBox1.Text);
            
        }

        private void Load_day_events(ListView lst, string dte)
        {
            lst.Items.Clear();
            //----------------
            DateTime dtt = DateTime.Parse("01/01/1999");
            DateTime.TryParse(dte, out dtt);
            if(infos.Rows.Count > 0 && dtt > DateTime.Parse("01/01/1999")) {
                infos.Rows.Cast<DataRow>().Where(EE => dtt >= (DateTime)EE["START_TIME"] && dtt <= (DateTime)EE["END_TIME"]).ToList().ForEach(ZZ =>
                {
                    ListViewItem dd = new ListViewItem(ZZ["OBJECT"].ToString());
                    dd.SubItems.Add(ZZ["ID"].ToString());
                    dd.ImageKey = ZZ["ICON_NME"].ToString();
                    dd.ToolTipText = ZZ["DESCRIPTION"].ToString();
                    lst.Items.Add(dd);

                });
               listView1_SizeChanged(lst, null);
            }            
        }
    }
}

