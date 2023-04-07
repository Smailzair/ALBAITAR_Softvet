using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace ALBAITAR_Softvet
{
    public partial class Form1 : Form
    {
        ImageList items_icon = new ImageList();
        public Form1()
        {
            InitializeComponent();
            //--------------------------
            items_icon.ImageSize = new Size(32, 32);
            //--------------------------------            
            items_icon.Images.Add("1", Properties.Resources.icons8_Checkmark_30px);
            Dayy_4.SmallImageList = items_icon;
            for (int j = 1; j < 10; j++)
            {
                ListViewItem itm = new ListViewItem("xxxxx " + j);
                itm.ImageKey = "1";
                //itm.SubItems.Add("xxxxx");
                Dayy_4.Items.Add(itm);
            }
                    Dayy_4.SelectedIndices.Clear();


        }
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

            if (((ListView)sender).CheckBoxes)
            {
                ((ListView)sender).SelectedItems.Clear();
                if (((ListView)sender).CheckedItems.Count == 0)
                {
                    ((ListView)sender).CheckBoxes = false;
                }
            }
        }
    }
}
