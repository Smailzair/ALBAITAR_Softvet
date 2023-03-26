using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ImageList lst = new ImageList();
            lst.Images.Add(Properties.Resources.Caprine);
            lst.Images.Add(Properties.Resources.icons8_dog_30px);
            lst.Images.Add(Properties.Resources.icons8_folders_30px);
            lst.Images.Add(Properties.Resources.icons8_down_button_30px1);
            listView1.LargeImageList= lst;
            listView1.SmallImageList = lst;
            listView1.Items[0].ImageIndex = 0;

            listView1.Items[1].BackColor= Color.Coral;


            listView1.Refresh();

        }
    }
}
