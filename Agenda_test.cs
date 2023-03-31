using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Agenda_TEST : Form
    {
        ImageList items_icon = new ImageList();
        DataTable infos = new DataTable();
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        public Agenda_TEST()
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
            foreach (Control ctr in flowLayoutPanel1.Controls)
            {
                foreach (Control ctr1 in ctr.Controls)
                {
                    if (ctr1.Name.Contains("Dayy_"))
                    {
                        ((ListView)ctr1).SmallImageList = items_icon;
                        listView1_SizeChanged(((ListView)ctr1), null);
                    }
                }
            }
            //---------------
            intial_Modify_fields();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

            startDate = DateTime.Parse("01/" + e.Start.Month + "/" + e.Start.Year);
            endDate = DateTime.Parse(DateTime.DaysInMonth(e.Start.Year, e.Start.Month) + "/" + e.Start.Month + "/" + e.Start.Year);
            //--------------------------------
            int ddds = (int)startDate.DayOfWeek;
            int next_mnth_frst_day_label_nb = 0;
            foreach (Control ctr in flowLayoutPanel1.Controls)
            {
                foreach (Control ctr1 in ctr.Controls)
                {
                    if (ctr1.Name.Contains("Dayy_"))
                    {
                        ((ListView)ctr1).Items.Clear();
                        ((ListView)ctr1).Columns[0].Text = "";
                        if ((int.Parse(ctr1.Name.Substring(5)) - ddds) <= (endDate.Date).Day && (int.Parse(ctr1.Name.Substring(5)) - ddds) >= (startDate.Date).Day)
                        {
                            ((ListView)ctr1).Columns[0].Text = (int.Parse(ctr1.Name.Substring(5)) - ddds).ToString();
                            ((ListView)ctr1).HeaderStyle = ColumnHeaderStyle.Nonclickable;
                            ((ListView)ctr1).BorderStyle = BorderStyle.Fixed3D;
                        }
                        else
                        {
                            ((ListView)ctr1).HeaderStyle = ColumnHeaderStyle.None;
                            ((ListView)ctr1).BorderStyle = BorderStyle.None;
                        }


                    }
                }
            }
            //------------------------
        }
        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).Columns.Count > 0)
            {
                int totalWidth = ((ListView)sender).Columns.Cast<ColumnHeader>().Sum(c => c.Width); // Get the total width of all columns
                int newFirstColumnWidth = ((ListView)sender).ClientSize.Width - (totalWidth - ((ListView)sender).Columns[0].Width); // Calculate the new width of the first column
                ((ListView)sender).Columns[0].Width = newFirstColumnWidth; // Set the new width of the first column
            }
            
        }


        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            e.Item.ForeColor = e.Item.Checked ? Color.Green : SystemColors.WindowText;
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
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedItems.Count > 0)
            {
                // textBox1.Text = ((ListView)sender).SelectedItems[0].SubItems.Count > 1 ? ((ListView)sender).SelectedItems[0].SubItems[1].Text : ""; //Get the ID
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

            if (((ListView)sender).CheckBoxes)
            {
                ((ListView)sender).SelectedItems.Clear();
                if (((ListView)sender).CheckedItems.Count == 0)
                {
                    ((ListView)sender).CheckBoxes = false;
                }
            }
        }



        private void Load_day_events(ListView lst, string dte)
        {
            lst.Items.Clear();
            //----------------
            DateTime dtt = DateTime.Parse("01/01/1999");
            DateTime.TryParse(dte, out dtt);
            if (infos.Rows.Count > 0 && dtt > DateTime.Parse("01/01/1999"))
            {
                infos.Rows.Cast<DataRow>().Where(EE => dtt >= (DateTime)EE["START_TIME"] && dtt <= (DateTime)EE["END_TIME"]).ToList().ForEach(ZZ =>
                {
                    ListViewItem dd = new ListViewItem(ZZ["OBJECT"].ToString());
                    dd.SubItems.Add(ZZ["ID"].ToString());
                    dd.ImageKey = ZZ["ICON_NME"].ToString();
                    dd.ToolTipText = ZZ["DESCRIPTION"] != DBNull.Value ? (string)ZZ["DESCRIPTION"] : "";
                    lst.Items.Add(dd);

                });
                listView1_SizeChanged(lst, null);
            }
        }


        private void dateTimePicker1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.X < 117)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }

        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            this.SelectNextControl((Control)sender, true, true, true, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddMonths(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddMonths(-1);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            startDate = DateTime.Parse("01/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Year);
            endDate = DateTime.Parse(DateTime.DaysInMonth(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month) + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Year);
            //--------------------------------
            int ddds = (int)startDate.DayOfWeek;
            int next_mnth_frst_day_label_nb = 0;
            foreach (Control ctr in flowLayoutPanel1.Controls)
            {
                foreach (Control ctr1 in ctr.Controls)
                {
                    if (ctr1.Name.Contains("Dayy_"))
                    {
                        ((ListView)ctr1).Items.Clear();
                        ((ListView)ctr1).Columns[0].Text = "";
                        if ((int.Parse(ctr1.Name.Substring(5)) - ddds) <= (endDate.Date).Day && (int.Parse(ctr1.Name.Substring(5)) - ddds) >= (startDate.Date).Day)
                        {
                            ((ListView)ctr1).Columns[0].Text = (int.Parse(ctr1.Name.Substring(5)) - ddds).ToString();
                            ((ListView)ctr1).HeaderStyle = ColumnHeaderStyle.Nonclickable;
                            ((ListView)ctr1).BorderStyle = BorderStyle.Fixed3D;
                            //--------------------
                            // infos.AsEnumerable().Where(row => row.Field<DateTime>("")
                        }
                        else
                        {
                            ((ListView)ctr1).HeaderStyle = ColumnHeaderStyle.None;
                            ((ListView)ctr1).BorderStyle = BorderStyle.None;
                        }


                    }
                }
            }
            //------------------------
        }

        private void Agenda_TEST_SizeChanged(object sender, EventArgs e)
        {
            Dim_Flow.Height = Lun_Flow.Height = Mar_Flow.Height = Mer_Flow.Height = Jeu_Flow.Height = Ven_Flow.Height = Sam_Flow.Height = (flowLayoutPanel1.ClientSize.Height < flowLayoutPanel1.DisplayRectangle.Height) ? 533 : flowLayoutPanel1.ClientSize.Height - 6;
            //foreach (Control vw in flowLayoutPanel1.Controls.OfType<ListView>())
            //{
            //    vw.Height = (((FlowLayoutPanel)sender).Height - 28) / 6;
            //}
        }

        private void Dim_Flow_SizeChanged(object sender, EventArgs e)
        {
            //-----------------
            foreach (Control vw in ((FlowLayoutPanel)sender).Controls.OfType<ListView>())
            {
                vw.Height = (((FlowLayoutPanel)sender).Height - 61) / 6;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Visible = !checkBox3.Checked;
            dateTimePicker4.Visible = !checkBox3.Checked && comboBox1.SelectedIndex > 0;
            dateTimePicker2.Width = dateTimePicker5.Width = checkBox3.Checked ? 217 : 168;
        }

        private void intial_Modify_fields()
        {
            pictureBox1.Visible = false;
            checkBox3.Checked = false;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker5.Value = dateTimePicker2.Value.AddHours(1);
            dateTimePicker4.Value = DateTime.Now.AddHours(1);
            comboBox1.SelectedIndex = 0;
            numericUpDown1.Value = 1;
            numericUpDown2.Value = 31;
            //-------------
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--");
            infos.AsEnumerable().Select(row => row.Field<string>("TYPE")).Distinct().ToList().ForEach(row => {
                if (row != null && row.Trim().Length > 0) { comboBox2.Items.Add(row); }
            });
            comboBox2.SelectedIndex= 0;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            verif_dates();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateTimePicker5.Visible = dateTimePicker4.Visible = comboBox1.SelectedIndex > 0;
            dateTimePicker5.Value = dateTimePicker3.Value.Hour < 23 ? dateTimePicker2.Value : dateTimePicker2.Value.AddDays(1);
            dateTimePicker4.Value = dateTimePicker3.Value.AddHours(1);
            checkBox3_CheckedChanged(null, null);
            panel4.Visible = comboBox1.SelectedIndex == 2;
            panel5.Visible = comboBox1.SelectedIndex == 3;
           // verif_dates();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Minimum= numericUpDown1.Value;
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {  
            verif_dates();
        }

        private void verif_dates()
        {
            if(comboBox1.SelectedIndex > 0)
            {
                DateTime dte_from;
                DateTime dte_to;
                if (!checkBox3.Checked)
                {
                    dte_from = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, dateTimePicker3.Value.Hour, dateTimePicker3.Value.Minute, 00);
                    dte_to = new DateTime(dateTimePicker5.Value.Year, dateTimePicker5.Value.Month, dateTimePicker5.Value.Day, dateTimePicker4.Value.Hour, dateTimePicker4.Value.Minute, 00);                    
                    pictureBox1.Visible = dte_from >= dte_to;
                    
                    
                }
                else
                {
                    dte_from = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);
                    dte_to = new DateTime(dateTimePicker5.Value.Year, dateTimePicker5.Value.Month, dateTimePicker5.Value.Day);
                    dateTimePicker5.CalendarTitleBackColor = dateTimePicker4.CalendarTitleBackColor = dte_from > dte_to ? Color.LightCoral : SystemColors.Window;
                    pictureBox1.Visible = dte_from > dte_to;                    
                }
            }
        }
        private void comboBox2_Validated(object sender, EventArgs e)
        {
            if (comboBox2.Text.Length > 0 && !comboBox2.Items.Contains(comboBox2.Text))
            {
                comboBox2.Text = comboBox2.Text.Trim().Substring(0, 1).ToUpper() + comboBox2.Text.Trim().Substring(1, comboBox2.Text.Trim().Length - 1).ToLower();
            }
            else if (comboBox2.Text.Trim().Length == 0)
            {
                comboBox2.SelectedIndex = 0;
            }
        }
    }
}

