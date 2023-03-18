//using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            //---------------------------
            listBox1.SetSelected(0, true);            
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            Label ll = new Label();
            //-----------------
            if (listBox1.SelectedIndex == 0) // Setting Global
            {
                splitContainer1.Panel2.Controls.Add(new SettingGlobal());
            }else if (Properties.Settings.Default.Last_login_is_admin)
            {
                if (listBox1.SelectedIndex == 1) // Trsor items
                {
                    //splitContainer1.Panel2.Controls.Add(new Ttesor_items());
                }
                else if (listBox1.SelectedIndex == 2) // Autorisations
                {
                    //splitContainer1.Panel2.Controls.Add(new Autorizations());
                }
            }
            else
            {
                splitContainer1.Panel2.Controls.Add(ll);
                ll.Text = "Non autorisé.";
                ll.Font = new Font("Microsoft Sans Serif", 12);
                ll.ForeColor = Color.Red;
                ll.AutoSize= true;
                ll.Location = new Point(splitContainer1.Panel2.Width / 2 - ll.Width/2, splitContainer1.Panel2.Height / 2 - ll.Height/2);


            }

            //-----------------
            
            if (splitContainer1.Panel2.Controls[0] != ll)
            {              
               
                splitContainer1.Panel2.Controls[0].Dock = DockStyle.Fill;
            }
            
            
        }

        private void splitContainer1_Panel2_ControlAdded(object sender, ControlEventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized && ((Width < e.Control.Width + (Size.Width - splitContainer1.Panel2.Width)) || Height < e.Control.Height + 40))
            {
                
                Width = e.Control.Width + (Size.Width - splitContainer1.Panel2.Width);
                Height = e.Control.Height + 40;

                this.CenterToScreen();
            }
            

        }
    }
}
