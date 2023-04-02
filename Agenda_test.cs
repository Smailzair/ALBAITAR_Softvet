using ALBAITAR_Softvet.Dialogs;
using MySqlX.XDevAPI;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Agenda_TEST : Form
    {
        public static List<int> selected_clients { get; set; }
        bool Is_New_To_Insert = true;
        ImageList items_icon = new ImageList();
        DataTable infos = new DataTable();
        DataTable all_clients = new DataTable();
        DataTable all_animals = new DataTable();
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        //-----------
        public static ListViewItem[] Clientss;
        public static ListViewItem[] Clientss2;
        public static ListViewItem[] Animm;
        public static ListViewItem[] Animm2;
        public Agenda_TEST()
        {
            InitializeComponent();
            //----------------------
            items_icon.ImageSize = new Size(32, 32);
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
            all_clients = PreConnection.Load_data("SELECT * FROM tb_agenda;");
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
            //----------------------------
            listView_Icons.SmallImageList= items_icon;
            for (int s = 0; s < items_icon.Images.Count; s++)
            {
                ListViewItem itm = new ListViewItem("", s);
                listView_Icons.Items.Add(itm);
            }
            //---------------            
            intial_Modify_fields();
            //------------------
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

            startDate = DateTime.Parse("01/" + e.Start.Month + "/" + e.Start.Year);
            endDate = DateTime.Parse(DateTime.DaysInMonth(e.Start.Year, e.Start.Month) + "/" + e.Start.Month + "/" + e.Start.Year);
            //--------------------------------
            int ddds = (int)startDate.DayOfWeek;
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
            int ddds = ((int)startDate.DayOfWeek + 1) == 7 ? 0 : ((int)startDate.DayOfWeek + 1);
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
            Sam_Flow.Height = Dim_Flow.Height = Lun_Flow.Height = Mar_Flow.Height = Mer_Flow.Height = Jeu_Flow.Height = Ven_Flow.Height = (flowLayoutPanel1.ClientSize.Height < flowLayoutPanel1.DisplayRectangle.Height) ? 533 : flowLayoutPanel1.ClientSize.Height - 6;
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
            numericUpDown2.Value = 7;
            numericUpDown3.Value = 1;
            checkBox11.Checked = false;
            //-------------
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--");
            infos.AsEnumerable().Select(row => row.Field<string>("TYPE")).Distinct().ToList().ForEach(row =>
            {
                if (row != null && row.Trim().Length > 0) { comboBox2.Items.Add(row); }
            });
            comboBox2.SelectedIndex = 0;
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
            numericUpDown2.Minimum = numericUpDown1.Value;
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            verif_dates();
        }

        private void verif_dates()
        {
            if (comboBox1.SelectedIndex > 0)
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
        private void button3_Click(object sender, EventArgs e)
        {
            Clientss2 = new ListViewItem[listView_Clients.Items.Count];
            for (int i = 0; i < listView_Clients.Items.Count; i++)
            {
                Clientss2[i] = listView_Clients.Items[i];
            }
            //--------------
            Clients_List lsst = new Clients_List();
            lsst.ShowDialog();
            //-------------
            listView_Clients.Items.Clear();
            if (Clientss != null)
            {
                if (Clientss.Length > 0)
                {
                    for (int yd = 0; yd < Clientss.Length; yd++)
                    {
                        ListViewItem itttm = Clientss[yd];
                        Clientss[yd].Remove();
                        listView_Clients.Items.Add(itttm);
                    }
                }
            }

            //----------------
            label12.Text = listView_Clients.Items.Count > 0 ? string.Concat("Propriétaires (", listView_Clients.Items.Count, "):") : "Propriétaires :";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itttm in listView_Clients.CheckedItems)
            {
                itttm.Remove();
            }
            //----------------
            label12.Text = listView_Clients.Items.Count > 0 ? string.Concat("Propriétaires (", listView_Clients.Items.Count, "):") : "Propriétaires :";
        }

        private void listView_Clients_ItemActivate(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedItems.Count > 0)
            {
                ListViewItem item = ((ListView)sender).SelectedItems[0];
                item.Checked = !item.Checked;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //if(listView_Clients.Items.Count > 0)
            //{
            Animm2 = new ListViewItem[listView_Anim.Items.Count];
            for (int i = 0; i < listView_Anim.Items.Count; i++)
            {
                Animm2[i] = listView_Anim.Items[i];
            }
            //--------------
            //List<int> idd = new List<int>();
            //listView_Clients.Items.Cast<ListViewItem>().ToList().ForEach(w =>
            //{
            //    idd.Add(int.Parse(w.SubItems[1].Text));
            //});
            //Anims_List ann = new Anims_List(idd);
            Anims_List ann = new Anims_List();
            ann.ShowDialog();
            //--------------

            //-------------
            listView_Anim.Items.Clear();
            if (Animm != null)
            {
                if (Animm.Length > 0)
                {
                    for (int yd = 0; yd < Animm.Length; yd++)
                    {
                        ListViewItem itttm = Animm[yd];
                        Animm[yd].Remove();
                        listView_Anim.Items.Add(itttm);
                    }
                }
            }
            //------------
            foreach (ColumnHeader column in listView_Anim.Columns)
            {
                if (column.Width > 0)
                {
                    column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            //----------------
            label13.Text = listView_Anim.Items.Count > 0 ? string.Concat("Animaux (", listView_Anim.Items.Count, "):") : "Animaux :";
            //}
            //else
            //{
            //    MessageBox.Show("Veuillez sélectionner d'abord un propriétaire, puis réesayer.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itttm in listView_Anim.CheckedItems)
            {
                itttm.Remove();
            }
            //----------------
            label13.Text = listView_Anim.Items.Count > 0 ? string.Concat("Animaux (", listView_Anim.Items.Count, "):") : "Animaux :";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DateTime dte_from;// = DateTime.Now;
            DateTime dte_to;
            string Week_loop = string.Empty;
            bool ready_to_save = true;
            string msg_err = "Veuillez d'abord vérifier ces perturbateurs :\n";
            //-------------- Objet
            ready_to_save = textBox1.Text.Trim().Length > 0;
            msg_err += textBox1.Text.Trim().Length > 0 ? "" : "\n- L'objet d'événement.";
            //-------------- Dates 

            if (!checkBox3.Checked)
            {
                dte_from = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, dateTimePicker3.Value.Hour, dateTimePicker3.Value.Minute, 00);
                dte_to = new DateTime(dateTimePicker5.Value.Year, dateTimePicker5.Value.Month, dateTimePicker5.Value.Day, dateTimePicker4.Value.Hour, dateTimePicker4.Value.Minute, 00);


                if (dte_from >= dte_to && dateTimePicker5.Visible)
                {
                    ready_to_save = false;
                    msg_err += "\n- La première date est (après/égales) la deuxième date.";
                }
            }
            else
            {
                dte_from = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day);
                dte_to = new DateTime(dateTimePicker5.Value.Year, dateTimePicker5.Value.Month, dateTimePicker5.Value.Day);
                if (dte_from > dte_to && dateTimePicker5.Visible)
                {
                    ready_to_save = false;
                    msg_err += "\n- La première date est après la deuxième date.";
                }
            }

            //---------------- Loop
            switch (comboBox1.SelectedIndex)
            {
                case 2: //Chaque Semaine
                    List<string> week_tmp = new List<string>();
                    if (checkBox4.Checked) { week_tmp.Add("Dim"); }
                    if (checkBox5.Checked) { week_tmp.Add("Lun"); }
                    if (checkBox6.Checked) { week_tmp.Add("Mar"); }
                    if (checkBox7.Checked) { week_tmp.Add("Mer"); }
                    if (checkBox8.Checked) { week_tmp.Add("Jeu"); }
                    if (checkBox9.Checked) { week_tmp.Add("Ven"); }
                    if (checkBox10.Checked) { week_tmp.Add("Sam"); }
                    week_tmp.ForEach(rr => Week_loop += "," + rr);
                    Week_loop = week_tmp.Count > 0 ? Week_loop.Substring(1, Week_loop.Length - 1) : "";
                    //--------------
                    bool sem_tmp = false;
                    for (int y = 0; y < 7; y++)
                    {
                        DateTime tmmp_dt = dte_from.AddDays(y);
                        if (tmmp_dt <= dte_to)
                        {
                            sem_tmp = week_tmp.Contains(tmmp_dt.ToString("dddd", new CultureInfo("fr-FR")).ToTitleCase().Substring(0, 3));
                            if (sem_tmp) { break; }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (sem_tmp == false || week_tmp.Count == 0)
                    {
                        ready_to_save = false;
                        msg_err += "\n- Aucun jour de semaine trouvé dans cette période.";
                    }
                    break;
                case 3: //Chaque Mois
                    bool sem_tmp2 = false;
                    for (int y = 0; y < 31; y++)
                    {
                        DateTime tmmp_dt = dte_from.AddDays(y);
                        if (tmmp_dt <= dte_to)
                        {
                            sem_tmp2 = tmmp_dt.Day >= numericUpDown1.Value && tmmp_dt.Day <= numericUpDown2.Value;
                            if (sem_tmp2) { break; }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (sem_tmp2 == false)
                    {
                        ready_to_save = false;
                        msg_err += "\n- Aucun jour de mois trouvé dans cette période.";
                    }
                    break;
            }
            //----------

            if (ready_to_save)
            {
                string loop = "";
                switch (comboBox1.SelectedIndex)
                {
                    case 2: //Chaque semaine

                        break; 
                    case 3: //Chaque mois
                        break;
                }
                if (Is_New_To_Insert) //Insert
                {
                    string cmmd = "INSERT INTO tb_agenda "
                                + "(`START_TIME`,"
                                + "`END_TIME`,"
                                + "`EVERY_TYPE`,"
                                + "`EVERY_WEEK_DAY`,"
                                + "`EVERY_MONTH_DAY`,"
                                + "`HOURS_ARRANG_OF`,"
                                + "`HOURS_ARRANG_TO`,"
                                + "`TYPE`,"
                                + "`OBJECT`,"
                                + "`DESCRIPTION`,"
                                + "`TASK_DONE`,"
                                + "`REPPEL_BEFORE_DAYS`,"
                                + "`RELATED_CLIENTS_IDs`,"
                                + "`RELATED_ANIMALS_IDs`,"
                                + "`ICON_NME`)"
                                + "VALUES"
                                + "('"+dateTimePicker2.Value.ToString("yyyy-MM-dd") + " " + (dateTimePicker3.Visible ? dateTimePicker3.Value.ToString("HH:mm:ss") : "00:00:00") +"'," //START_TIME
                                + (dateTimePicker5.Visible ? ("'" + dateTimePicker5.Value.ToString("yyyy-MM-dd") + " " + (dateTimePicker4.Visible ? dateTimePicker4.Value.ToString("HH:mm:ss") : "00:00:00")) : (dateTimePicker2.Value.ToString("yyyy-MM-dd") + " " + (dateTimePicker3.Visible ? dateTimePicker3.Value.ToString("HH:mm:ss") : "00:00:00"))) + "'," //END_TIME
                                + "'"+ comboBox1.SelectedItem +"'," //EVERY_TYPE
                                + (Week_loop.Length > 0 ? "'"+Week_loop+"'" : "NULL") + "," //EVERY_WEEK_DAY
                                + "<{EVERY_MONTH_DAY: }>," //EVERY_MONTH_DAY
                                + "<{HOURS_ARRANG_OF: }>,"
                                + "<{HOURS_ARRANG_TO: }>,"
                                + "<{TYPE: }>,"
                                + "<{OBJECT: }>,"
                                + "<{DESCRIPTION: }>,"
                                + "<{TASK_DONE: 0}>,"
                                + "<{REPPEL_BEFORE_DAYS: }>,"
                                + "<{RELATED_CLIENTS_IDs: }>,"
                                + "<{RELATED_ANIMALS_IDs: }>,"
                                + "<{ICON_NME: }>);";
                    PreConnection.Excut_Cmd(cmmd);
                }
                else //Update
                {

                }                
            }
            else
            {
                MessageBox.Show(msg_err,"",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            

        }

        private void listView_Anim_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            checkBox11.Checked = true;
        }

        private void listView_Icons_Leave(object sender, EventArgs e)
        {
            panel14.Visible = false;
        }

        private void listView_Icons_ItemActivate(object sender, EventArgs e)
        {            
            pictureBox2.Image = items_icon.Images[listView_Icons.SelectedItems[0].ImageIndex];
            panel14.Visible = false;
            textBox1.Focus();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panel14.Visible = true;
            listView_Icons.SelectedItems.Clear();
            panel14.Focus();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.icons8_camera_30px;
            panel14.Visible = false;
            textBox1.Focus();
        }

        private void Agenda_TEST_MouseDown(object sender, MouseEventArgs e)
        {
            if (panel14.Visible && !panel14.Bounds.Contains(e.Location)) { 
                panel14.Visible = false;
                textBox1.Focus();
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {
            panel14.Visible = false;
            textBox1.Focus();
        }
    }
}

