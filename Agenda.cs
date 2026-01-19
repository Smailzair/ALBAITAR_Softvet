using ALBAITAR_Softvet.Dialogs;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace ALBAITAR_Softvet.Resources
{
    public partial class Agenda : Form
    {
        int event_id_to_selectt = -1;
        DateTime Date_to_selectt = DateTime.Now;
        public static List<int> selected_clients { get; set; }
        bool Is_New_To_Insert = true;
        string Current_items_id = string.Empty;
        ImageList items_icon = new ImageList();
        DataTable infos = new DataTable();
        DataTable icons = new DataTable();
        DataTable animals = new DataTable();
        DataTable clients = new DataTable();
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        string selected_ids_to_delete = "";
        bool tmp_pause = false;
        ListView prev = null;

        Image tmmmmp_img = null;
        DateTime tmmp;
        //---------
        bool Ajouter_pour_tout_monde_40001,
            Ajouter_pour_juste_lui_40002,
            Modifier_pour_tous_40003,
            Modifier_pour_juste_lui_40004,
            Supprimer_pour_tous_40005,
            Supprimer_pour_juste_lui_40006
            = false;
        //-----------
        public static ListViewItem[] Clientss;
        public static ListViewItem[] Clientss2;
        public static ListViewItem[] Animm;
        public static ListViewItem[] Animm2;
        public static string Userss;
        int selected_img_idx = -1;
        public Agenda(int? event_id_to_select, DateTime? Date_to_select)
        {

            InitializeComponent();
            event_id_to_selectt = (int)(event_id_to_select != null ? event_id_to_select : -1);
            Date_to_selectt = (DateTime)(Date_to_select != null ? Date_to_select : DateTime.Now);
            //----------------------
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                Ajouter_pour_tout_monde_40001 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40001" && (Int32)QQ[3] == 1).Count() > 0;
                Ajouter_pour_juste_lui_40002 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40002" && (Int32)QQ[3] == 1).Count() > 0;
                Modifier_pour_tous_40003 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40003" && (Int32)QQ[3] == 1).Count() > 0;
                Modifier_pour_juste_lui_40004 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40004" && (Int32)QQ[3] == 1).Count() > 0;
                Supprimer_pour_tous_40005 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40005" && (Int32)QQ[3] == 1).Count() > 0;
                Supprimer_pour_juste_lui_40006 = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40006" && (Int32)QQ[3] == 1).Count() > 0;
                //-----------
                button3.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10000" && (Int32)QQ[3] == 1).Count() > 0; //Clients (Propritaires)                
                button6.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20000" && (Int32)QQ[3] == 1).Count() > 0; //Animaux              
            }
            else
            {
                Ajouter_pour_tout_monde_40001 =
                Ajouter_pour_juste_lui_40002 =
                Modifier_pour_tous_40003 =
                Modifier_pour_juste_lui_40004 =
                Supprimer_pour_tous_40005 =
                Supprimer_pour_juste_lui_40006 = true;
            }
            //---------------
            items_icon.ImageSize = new Size(32, 32);
            listView_Icons.SmallImageList = items_icon;
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
            //--------------------------------
            Load_all_data();
            //------------------
        }
        private void Load_all_data()
        {
            clients = PreConnection.Load_data("SELECT * FROM tb_clients ORDER BY FAMNME;");
            animals = PreConnection.Load_data("SELECT * FROM tb_animaux ORDER BY NME;");
            infos = PreConnection.Load_data("SELECT * FROM tb_agenda WHERE `FOR_THIS_USERS` IN (NULL, '') OR `FOR_THIS_USERS` LIKE '" + Properties.Settings.Default.Last_login_user_idx + "' OR `FOR_THIS_USERS` LIKE '%," + Properties.Settings.Default.Last_login_user_idx + ",%' OR `FOR_THIS_USERS` LIKE '%," + Properties.Settings.Default.Last_login_user_idx + "' OR `FOR_THIS_USERS` LIKE '" + Properties.Settings.Default.Last_login_user_idx + ",%';");

            icons = PreConnection.Load_data("SELECT * FROM tb_images WHERE IMG_DATA IS NOT NULL GROUP BY IMG_DATA ORDER BY MODIF_TIME;");
            //------------------------------
            items_icon.Images.Clear();
            listView_Icons.Items.Clear();
            items_icon.Images.Add("-1",  Properties.Resources.icons8_Checkmark_30px);
            if (icons != null)
            {
                icons.Rows.Cast<DataRow>().ToList().ForEach(row =>
                {
                    Image img = PreConnection.ByteArrayToImage((byte[])row["IMG_DATA"]);
                    bool exist = false;
                    for (int i = 0; i < items_icon.Images.Count; i++)
                    {
                        if (PreConnection.ArePicturesEqual(items_icon.Images[i], img) || img == null)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        try
                        {
                            items_icon.Images.Add(row["ID"].ToString(), img);
                        }
                        catch { }

                    }
                    //------------------------
                    listView_Icons.SmallImageList = items_icon;
                    ListViewItem itm = new ListViewItem("");
                    itm.ImageKey = row["ID"].ToString();
                    itm.SubItems.Add(row["ID"].ToString());
                    listView_Icons.Items.Add(itm);
                });
            }
            //----------------
            radioButton1.Text = Properties.Settings.Default.Last_login_user_full_nme;
            //-------------------------
            if (event_id_to_selectt > -1)
            {
                dateTimePicker1.Value = Date_to_selectt;
                Fill_Event_Fields(event_id_to_selectt.ToString());
                event_id_to_selectt = -1;
            }
            else
            {
                dateTimePicker1_ValueChanged(null, null);
                intial_Modify_fields();
            }

        }
        //private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        //{
        //    startDate = DateTime.Parse("01/" + e.Start.Month + "/" + e.Start.Year);
        //    endDate = DateTime.Parse(DateTime.DaysInMonth(e.Start.Year, e.Start.Month) + "/" + e.Start.Month + "/" + e.Start.Year);
        //    //--------------------------------
        //    int ddds = (int)startDate.DayOfWeek;
        //    foreach (Control ctr in flowLayoutPanel1.Controls)
        //    {
        //        foreach (Control ctr1 in ctr.Controls)
        //        {
        //            if (ctr1.Name.Contains("Dayy_"))
        //            {
        //                ((ListView)ctr1).Items.Clear();
        //                ((ListView)ctr1).Columns[0].Text = "";
        //                if ((int.Parse(ctr1.Name.Substring(5)) - ddds) <= (endDate.Date).Day && (int.Parse(ctr1.Name.Substring(5)) - ddds) >= (startDate.Date).Day)
        //                {
        //                    ((ListView)ctr1).Columns[0].Text = (int.Parse(ctr1.Name.Substring(5)) - ddds).ToString();
        //                    ((ListView)ctr1).HeaderStyle = ColumnHeaderStyle.Nonclickable;
        //                    ((ListView)ctr1).BorderStyle = BorderStyle.Fixed3D;
        //                }
        //                else
        //                {
        //                    ((ListView)ctr1).HeaderStyle = ColumnHeaderStyle.None;
        //                    ((ListView)ctr1).BorderStyle = BorderStyle.None;
        //                }
        //            }
        //        }
        //    }
        //    //------------------------
        //}
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
            // e.Item.ForeColor = e.Item.Checked ? Color.Green : SystemColors.WindowText;
            checked_to_delete_nb();
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
            if (!tmp_pause)
            {
                if (((ListView)sender).SelectedItems.Count > 0)
                {
                    Current_items_id = ((ListView)sender).SelectedItems[0].SubItems[1].Text; //Get the ID
                    Is_New_To_Insert = false;
                    pictureBox6.Image = Properties.Resources.MODIF_002;
                    Fill_Event_Fields(Current_items_id);
                    if (prev != null && prev != ((ListView)sender))
                    {
                        try
                        {
                            tmp_pause = true;
                            prev.SelectedIndices.Clear();
                            prev.SelectedItems.Clear();
                            tmp_pause = false;
                        }
                        catch { }
                    }
                    prev = ((ListView)sender);
                }
                else
                {
                    Current_items_id = string.Empty;
                    Is_New_To_Insert = true;
                    pictureBox6.Image = Properties.Resources.NOUVEAU_002;
                    intial_Modify_fields();
                    // prev = null;
                }
            }



        }

        private void Fill_Event_Fields(string ID)
        {
            intial_Modify_fields();
            DataRow row = infos.Rows.Cast<DataRow>().Where(w => w["ID"].ToString() == ID).FirstOrDefault();
            if (row[0] != null)
            {
                Current_items_id = ID;
                Is_New_To_Insert = false;
                pictureBox6.Image = Properties.Resources.MODIF_002;
                button14.Visible = true;
                //----------Loading Data------------
                textBox1.Text = row["OBJECT"].ToString(); //OBJECT
                if (row["ICON_ID"] != DBNull.Value)
                {
                    pictureBox2.Image = items_icon.Images[row["ICON_ID"].ToString()]; //ICON
                    selected_img_idx = int.Parse(row["ICON_ID"].ToString());
                }
                switch (row["EVERY_TYPE"].ToString())
                {
                    case "ONCE":
                        comboBox1.SelectedIndex = 0;
                        break;
                    case "EVERY_DAY":
                        comboBox1.SelectedIndex = 1;
                        break;
                    case "EVERY_WEEK":
                        comboBox1.SelectedIndex = 2;
                        List<string> wk = row["EVERY_WEEK_DAY"].ToString().Split(',').ToList();
                        checkBox10.Checked = wk.Contains("Sam");
                        checkBox4.Checked = wk.Contains("Dim");
                        checkBox5.Checked = wk.Contains("Lun");
                        checkBox6.Checked = wk.Contains("Mar");
                        checkBox7.Checked = wk.Contains("Mer");
                        checkBox8.Checked = wk.Contains("Jeu");
                        checkBox9.Checked = wk.Contains("Ven");
                        break;
                    case "EVERY_MONTH":
                        comboBox1.SelectedIndex = 3;
                        string[] separator2 = { "_TO_" };
                        List<string> mnth = row["EVERY_MONTH_DAY"].ToString().Split(separator2, StringSplitOptions.RemoveEmptyEntries).ToList();
                        numericUpDown1.Value = int.Parse(mnth[0]);
                        numericUpDown2.Value = int.Parse(mnth[1]);
                        break;
                }
                dateTimePicker2.Value = (DateTime)row["START_TIME"];
                dateTimePicker5.Value = (DateTime)row["END_TIME"];
                if (row["HOURS_ARRANG_OF"] != DBNull.Value)
                {
                    TimeSpan tm = TimeSpan.Parse(row["HOURS_ARRANG_OF"].ToString());
                    dateTimePicker3.Value = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, tm.Hours, tm.Minutes, 0);
                    if (row["HOURS_ARRANG_TO"] != DBNull.Value)
                    {
                        TimeSpan tm2 = TimeSpan.Parse(row["HOURS_ARRANG_TO"].ToString());
                        dateTimePicker4.Value = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, tm2.Hours, tm2.Minutes, 0);
                    }
                }
                else
                {
                    checkBox3.Checked = true;
                }
                if (row["REPPEL_BEFORE_DAYS"] != DBNull.Value)
                {
                    checkBox11.Checked = true;
                    numericUpDown3.Value = int.Parse(row["REPPEL_BEFORE_DAYS"].ToString());
                }
                comboBox2.SelectedItem = row["TYPE"].ToString();
                textBox2.Text = row["DESCRIPTION"].ToString();
                //-------------------
                if (row["RELATED_ANIMALS_IDs"].ToString().Trim().Length > 0)
                {
                    string[] separator3 = { "," };
                    List<string> mnth = row["RELATED_ANIMALS_IDs"].ToString().Split(separator3, StringSplitOptions.RemoveEmptyEntries).ToList();
                    mnth.ForEach(AA =>
                    {
                        DataRow ann = animals.Rows.Cast<DataRow>().Where(h => h["ID"].ToString() == AA).FirstOrDefault();
                        if (ann[0] != null)
                        {
                            ListViewItem dd = new ListViewItem(ann["NME"].ToString());
                            dd.SubItems.Add(ann["ID"].ToString());
                            string sss = "";
                            DataRow clt = clients.Rows.Cast<DataRow>().Where(hhj => hhj["ID"].ToString() == ann["CLIENT_ID"].ToString()).FirstOrDefault();
                            if (clt[0] != null)
                            {
                                sss = string.Concat(clt["FAMNME"].ToString(), " ", clt["NME"].ToString());
                            }
                            dd.SubItems.Add(sss);
                            dd.SubItems.Add(ann["CLIENT_ID"].ToString());
                            listView_Anim.Items.Add(dd);
                        }

                    });
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
                }
                //-----------------
                if (row["RELATED_CLIENTS_IDs"].ToString().Trim().Length > 0)
                {
                    string[] separator4 = { "," };
                    List<string> mnth = row["RELATED_CLIENTS_IDs"].ToString().Split(separator4, StringSplitOptions.RemoveEmptyEntries).ToList();
                    mnth.ForEach(AA =>
                    {
                        DataRow cllt = clients.Rows.Cast<DataRow>().Where(h => h["ID"].ToString() == AA).FirstOrDefault();
                        if (cllt != null)
                        {
                            if (cllt[0] != null)
                            {
                                ListViewItem dd = new ListViewItem(string.Concat(cllt["FAMNME"].ToString(), " ", cllt["NME"].ToString()));
                                dd.SubItems.Add(cllt["ID"].ToString());
                                listView_Clients.Items.Add(dd);
                            }
                        }


                    });
                    label12.Text = listView_Clients.Items.Count > 0 ? string.Concat("Propriétaires (", listView_Clients.Items.Count, "):") : "Propriétaires :";
                }
                //-----------------
                if (row["FOR_THIS_USERS"].ToString().Trim().Length > 0)
                {
                    string[] separator5 = { "," };
                    List<string> usrs = row["FOR_THIS_USERS"].ToString().Split(separator5, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (usrs.Count == 1 && usrs[0] == Properties.Settings.Default.Last_login_user_idx.ToString())
                    {
                        radioButton1.Checked = true;
                    }
                    else
                    {
                        radioButton3.Checked = true;
                        //usrs.ForEach(AA =>
                        //{
                        //    Userss += ',' + AA;
                        //});
                        //Userss = Userss.Substring(1, Userss.Length - 1);
                    }
                }
                else
                {
                    radioButton2.Checked = true; //tout le monde
                }
                //--------------
                Userss = row["FOR_THIS_USERS"].ToString();
                //============= Autorisations --> ===================                
                if (Modifier_pour_tous_40003)
                {
                    radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = true;
                    button7.Visible = true;
                    //---------
                    foreach (Control ctrl in panel2.Controls)
                    {
                        ctrl.Enabled = true;
                    }
                }
                else if (Modifier_pour_juste_lui_40004 && Userss == Properties.Settings.Default.Last_login_user_idx.ToString())
                {
                    //---------
                    foreach (Control ctrl in panel2.Controls)
                    {
                        ctrl.Enabled = true;
                    }
                    //----------
                    button7.Visible = true;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = radioButton3.Enabled = false;
                }
                else
                {
                    button7.Visible = false;
                    foreach (Control ctrl in panel1.Controls)
                    {
                        ctrl.Enabled = false;
                    }
                    foreach (Control ctrl in panel2.Controls)
                    {
                        if (ctrl.Name != "panel1")
                            ctrl.Enabled = false;
                    }
                }
                button14.Visible = Supprimer_pour_tous_40005 || (Supprimer_pour_juste_lui_40006 && Userss == Properties.Settings.Default.Last_login_user_idx.ToString());
                //============= <-- Autorisations ===================
            }
            else
            {
                Current_items_id = string.Empty;
                Is_New_To_Insert = true;
                pictureBox6.Image = Properties.Resources.NOUVEAU_002;
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

            checked_to_delete_nb();
        }

        private void checked_to_delete_nb()
        {
            int ttt = 0;
            foreach (Control ctr in flowLayoutPanel1.Controls)
            {
                foreach (Control ctr1 in ctr.Controls)
                {
                    if (ctr1.Name.Contains("Dayy_"))
                    {
                        if (((ListView)ctr1).CheckBoxes)
                        {
                            ttt += ((ListView)ctr1).CheckedItems.Count;
                        }
                    }
                }
            }
            button10.Text = "Supprimer (" + ttt + ")";
            button10.Visible = button15.Visible = ttt > 0;
            if (ttt < 1)
            {
                foreach (Control ctr in flowLayoutPanel1.Controls)
                {
                    foreach (Control ctr1 in ctr.Controls)
                    {
                        if (ctr1.Name.Contains("Dayy_"))
                        {
                            if (((ListView)ctr1).CheckBoxes)
                            {
                                ((ListView)ctr1).CheckBoxes = false;
                            }
                        }
                    }
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
                int y = 11;
                infos.Rows.Cast<DataRow>().Where(EE => dtt >= (DateTime)EE["START_TIME"] && dtt <= (DateTime)EE["END_TIME"]).ToList().ForEach(ZZ =>
                {
                    y++;
                    bool Write_It = false;
                    switch (ZZ["EVERY_TYPE"])
                    {
                        case "ONCE":
                        case "EVERY_DAY":
                            Write_It = true;
                            break;
                        case "EVERY_WEEK":
                            if (ZZ["EVERY_WEEK_DAY"].ToString().Split(',').ToList().Contains(dtt.DayOfWeek.ToString().Replace("Saturday", "Sam").Replace("Sunday", "Dim").Replace("Monday", "Lun").Replace("Tuesday", "Mar").Replace("Wednesday", "Mer").Replace("Thursday", "Jeu").Replace("Friday", "Ven")))
                            {
                                Write_It = true;
                            }
                            break;
                        case "EVERY_MONTH":
                            string[] separators = { "_TO_" };
                            string[] substrings = ZZ["EVERY_MONTH_DAY"].ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            int val1 = int.Parse(substrings[0]);
                            int val2 = int.Parse(substrings[1]);
                            if (dtt.Day >= val1 && dtt.Day <= val2)
                            {
                                Write_It = true;
                            }
                            break;
                    }
                    //-----------
                    if (Write_It)
                    {
                        ListViewItem dd = new ListViewItem(ZZ["OBJECT"].ToString());
                        dd.SubItems.Add(ZZ["ID"].ToString());

                        if (ZZ["ICON_ID"] != DBNull.Value)
                        {
                            dd.ImageKey = ZZ["ICON_ID"].ToString();
                        }
                        else
                        {
                            dd.ImageKey = "-1";
                        }
                        if (ZZ["DESCRIPTION"] != DBNull.Value && ZZ["DESCRIPTION"].ToString().Length > 0)
                        {
                            dd.ToolTipText = ZZ["OBJECT"].ToString() + "\n" + "----<.>----\n" + (string)ZZ["DESCRIPTION"];
                        }
                        lst.Items.Add(dd);
                    }

                });
                //---------------------------------
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
                            Load_day_events((ListView)ctr1, string.Concat((int.Parse(ctr1.Name.Substring(5)) - ddds), "/", dateTimePicker1.Value.Month, "/", dateTimePicker1.Value.Year));
                        }
                        else
                        {
                            ((ListView)ctr1).HeaderStyle = ColumnHeaderStyle.None;
                            ((ListView)ctr1).BorderStyle = BorderStyle.None;
                        }
                        tmmp = new DateTime(1900, 12, 12);
                        DateTime.TryParse(string.Concat((int.Parse(ctr1.Name.Substring(5)) - ddds), "/", dateTimePicker1.Value.Month, "/", dateTimePicker1.Value.Year), out tmmp);
                        ((ListView)ctr1).BackColor = DateTime.Today == tmmp ? Color.HotPink : SystemColors.Window;

                    }
                }
            }
            //------------------------
            linkLabel1.Visible = dateTimePicker1.Value.Month != DateTime.Now.Month;
        }

        private void Agenda_TEST_SizeChanged(object sender, EventArgs e)
        {
            Sam_Flow.Height = Dim_Flow.Height = Lun_Flow.Height = Mar_Flow.Height = Mer_Flow.Height = Jeu_Flow.Height = Ven_Flow.Height = (flowLayoutPanel1.ClientSize.Height < flowLayoutPanel1.DisplayRectangle.Height) ? 533 : flowLayoutPanel1.ClientSize.Height - 6;
            Sam_Flow.Width = Dim_Flow.Width = Lun_Flow.Width = Mar_Flow.Width = Mer_Flow.Width = Jeu_Flow.Width = Ven_Flow.Width = (flowLayoutPanel1.ClientSize.Width > flowLayoutPanel1.DisplayRectangle.Width) ? 162 : (flowLayoutPanel1.ClientSize.Width - 48) / 7;
        }

        private void Dim_Flow_SizeChanged(object sender, EventArgs e)
        {
            //-----------------
            foreach (Control vw in ((FlowLayoutPanel)sender).Controls.OfType<ListView>())
            {
                //  vw.Width = (((FlowLayoutPanel)sender).Width - 9);
                vw.Height = (((FlowLayoutPanel)sender).Height - 60) / 6;
                vw.Width = (((FlowLayoutPanel)sender).Width - 6);
            }
            label2.Width = label3.Width = label4.Width = label5.Width = label6.Width = label7.Width = label8.Width = (((FlowLayoutPanel)sender).Width - 6);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Visible = !checkBox3.Checked;
            dateTimePicker4.Visible = !checkBox3.Checked && comboBox1.SelectedIndex > 0;
            dateTimePicker2.Width = dateTimePicker5.Width = checkBox3.Checked ? 217 : 168;
        }

        private void intial_Modify_fields()
        {
            radioButton1.Checked = true;
            Userss = Properties.Settings.Default.Last_login_user_idx.ToString();
            pictureBox6.Image = Properties.Resources.NOUVEAU_002;
            button14.Visible = false;
            pictureBox2.Image = Properties.Resources.icons8_camera_30px;
            selected_img_idx = -1;
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
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            checkBox10.Checked = checkBox4.Checked = checkBox5.Checked = checkBox6.Checked = checkBox7.Checked = checkBox8.Checked = checkBox9.Checked = false;
            //-------------
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--");
            infos.AsEnumerable().Select(row => row.Field<string>("TYPE")).Distinct().ToList().ForEach(row =>
            {
                if (row != null && row.Trim().Length > 0) { comboBox2.Items.Add(row); }
            });
            comboBox2.SelectedIndex = 0;
            //-----------------
            listView_Anim.Items.Clear();
            label13.Text = "Animaux :";
            listView_Clients.Items.Clear();
            label12.Text = "Propriétaires :";

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
            new Clients_List().ShowDialog();
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
                //----------------
                label12.Text = listView_Clients.Items.Count > 0 ? string.Concat("Propriétaires (", listView_Clients.Items.Count, "):") : "Propriétaires :";
            }
            else
            {
                label12.Text = "Propriétaires :";
            }


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
            Animm2 = new ListViewItem[listView_Anim.Items.Count];
            for (int i = 0; i < listView_Anim.Items.Count; i++)
            {
                Animm2[i] = listView_Anim.Items[i];
            }
            //--------------
            //Anims_List ann = new Anims_List();
            new Anims_List().ShowDialog();
            //--------------
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
                        //----------------Add clients to clients list
                        if (listView_Clients.Items.Cast<ListViewItem>().Where(itm => itm.SubItems[1].Text == itttm.SubItems[3].Text).ToList().Count == 0)
                        {
                            string[] itm_tmmp = new string[] { itttm.SubItems[2].Text, itttm.SubItems[3].Text }; //Client FULL_NME + Client ID
                            ListViewItem clnt = new ListViewItem(itm_tmmp);
                            listView_Clients.Items.Add(clnt);

                        }
                    }
                    label12.Text = listView_Clients.Items.Count > 0 ? string.Concat("Propriétaires (", listView_Clients.Items.Count, "):") : "Propriétaires :";
                }
                //----------------
                label13.Text = listView_Anim.Items.Count > 0 ? string.Concat("Animaux (", listView_Anim.Items.Count, "):") : "Animaux :";
            }
            else
            {
                label13.Text = "Animaux :";
            }
            //------------
            foreach (ColumnHeader column in listView_Anim.Columns)
            {
                if (column.Width > 0)
                {
                    column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itttm in listView_Anim.CheckedItems)
            {
                //---------Remove it from clients list
                if (listView_Clients.Items.Cast<ListViewItem>().Where(itm => itm.SubItems[1].Text == itttm.SubItems[3].Text).ToList().Count > 0)
                {
                    listView_Clients.Items.Cast<ListViewItem>().Where(itm => itm.SubItems[1].Text == itttm.SubItems[3].Text).First().Remove();
                }
                //-------------------                
                itttm.Remove();
            }
            //----------------
            label13.Text = listView_Anim.Items.Count > 0 ? string.Concat("Animaux (", listView_Anim.Items.Count, "):") : "Animaux :";
            label12.Text = listView_Clients.Items.Count > 0 ? string.Concat("Propriétaires (", listView_Clients.Items.Count, "):") : "Propriétaires :";
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
            msg_err += textBox1.Text.Trim().Length > 0 ? "" : "\n- L'objet d'événement (titre).";
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
            string loop = "";
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    loop = "ONCE";
                    break;
                case 1:
                    loop = "EVERY_DAY";
                    break;
                case 2: //Chaque Semaine
                    loop = "EVERY_WEEK";
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
                    loop = "EVERY_MONTH";
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
                string related_clients = "";
                listView_Clients.Items.Cast<ListViewItem>().ToList().ForEach(ZZZ => { related_clients += "," + ZZZ.SubItems[1].Text; });
                related_clients = related_clients.Length > 0 ? related_clients.Replace("K,", "").Replace("K", "").Substring(1) : related_clients;
                string related_Animaux = "";
                listView_Anim.Items.Cast<ListViewItem>().ToList().ForEach(ZZZ => { related_Animaux += "," + ZZZ.SubItems[1].Text; });
                related_Animaux = related_Animaux.Length > 0 ? related_Animaux.Replace("M,", "").Replace("M", "").Substring(1) : related_Animaux;
                //string cmmd = "";
                if (Is_New_To_Insert) //Insert
                {
                    PreConnection.Excut_Cmd(1, "tb_agenda", new List<string>
                    {
                        "START_TIME",
"END_TIME",
"EVERY_TYPE",
"EVERY_WEEK_DAY",
"EVERY_MONTH_DAY",
"HOURS_ARRANG_OF",
"HOURS_ARRANG_TO",
"TYPE",
"OBJECT",
"DESCRIPTION",
"REPPEL_BEFORE_DAYS",
"RELATED_CLIENTS_IDs",
"RELATED_ANIMALS_IDs",
"ICON_ID",
"FOR_THIS_USERS"
                    }, new List<object>
                    {
                        dateTimePicker2.Value.ToString("yyyy-MM-dd"),
                        dateTimePicker5.Visible ? dateTimePicker5.Value.ToString("yyyy-MM-dd") : dateTimePicker2.Value.ToString("yyyy-MM-dd"),
                        loop,
                        Week_loop.Length > 0 ? (object)Week_loop : DBNull.Value,
                        loop.Equals("EVERY_MONTH") ? (object)(numericUpDown1.Value + "_TO_" + numericUpDown2.Value)  : DBNull.Value,
                        dateTimePicker3.Visible ? (object)dateTimePicker3.Value.ToString("HH:mm:ss") : DBNull.Value,
                        dateTimePicker4.Visible ? (object)dateTimePicker4.Value.ToString("HH:mm:ss") : DBNull.Value,
                        comboBox2.Text,
                        textBox1.Text,
                        textBox2.Text,
                        numericUpDown3.Enabled ? (object)numericUpDown3.Value : DBNull.Value,
                        string.IsNullOrWhiteSpace(related_clients) ? DBNull.Value : (object)related_clients,
                        string.IsNullOrWhiteSpace(related_Animaux) ? DBNull.Value : (object)related_Animaux,
                        selected_img_idx > -1 ? (object)selected_img_idx : DBNull.Value,
                        Userss
                    }, null, null, null);


                    //cmmd = "INSERT INTO tb_agenda "
                    //       + "(`START_TIME`,"
                    //       + "`END_TIME`,"
                    //       + "`EVERY_TYPE`,"
                    //       + "`EVERY_WEEK_DAY`,"
                    //       + "`EVERY_MONTH_DAY`,"
                    //       + "`HOURS_ARRANG_OF`,"
                    //       + "`HOURS_ARRANG_TO`,"
                    //       + "`TYPE`,"
                    //       + "`OBJECT`,"
                    //       + "`DESCRIPTION`,"
                    //       + "`REPPEL_BEFORE_DAYS`,"
                    //       + "`RELATED_CLIENTS_IDs`,"
                    //       + "`RELATED_ANIMALS_IDs`,"
                    //       + "`ICON_ID`,"
                    //       + "`FOR_THIS_USERS`)"
                    //       + "VALUES"
                    //       + "('" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'," //START_TIME
                    //       + "'" + (dateTimePicker5.Visible ? dateTimePicker5.Value.ToString("yyyy-MM-dd") : dateTimePicker2.Value.ToString("yyyy-MM-dd")) + "'," //END_TIME
                    //       + "'" + loop + "'," //EVERY_TYPE
                    //       + (Week_loop.Length > 0 ? "'" + Week_loop + "'" : "NULL") + "," //EVERY_WEEK_DAY
                    //       + (loop.Equals("EVERY_MONTH") ? "'" + numericUpDown1.Value + "_TO_" + numericUpDown2.Value + "'" : "NULL") + "," //EVERY_MONTH_DAY
                    //       + (dateTimePicker3.Visible ? "'" + dateTimePicker3.Value.ToString("HH:mm:ss") + "'" : "NULL") + "," //"<{HOURS_ARRANG_OF: }>,"
                    //       + (dateTimePicker4.Visible ? "'" + dateTimePicker4.Value.ToString("HH:mm:ss") + "'" : "NULL") + ","//"<{HOURS_ARRANG_TO: }>,"
                    //       + "'" + comboBox2.Text.Replace("'","''") + "'," //TYPE
                    //       + "'" + textBox1.Text.Replace("'", "''") + "'," //OBJECT
                    //       + "'" + textBox2.Text.Replace("'", "''") + "'," //DESCRIPTION
                    //       + (numericUpDown3.Enabled ? numericUpDown3.Value.ToString() : "NULL") + "," //REPPEL_BEFORE_DAYS
                    //       + "'" + related_clients + "'," //RELATED_CLIENTS_IDs
                    //       + "'" + related_Animaux + "'," //RELATED_ANIMALS_IDs
                    //       + "@Icon," //ICO
                    //       + "'" + Userss.Replace("'", "''") + "');"; //FOR_THIS_USERS


                }
                else //Update
                {
                    PreConnection.Excut_Cmd(2, "tb_agenda", new List<string>
                    {
                        "START_TIME",
"END_TIME",
"EVERY_TYPE",
"EVERY_WEEK_DAY",
"EVERY_MONTH_DAY",
"HOURS_ARRANG_OF",
"HOURS_ARRANG_TO",
"TYPE",
"OBJECT",
"DESCRIPTION",
"REPPEL_BEFORE_DAYS",
"RELATED_CLIENTS_IDs",
"RELATED_ANIMALS_IDs",
"ICON_ID",
"FOR_THIS_USERS"
                    }, new List<object>
                    {
                        dateTimePicker2.Value.ToString("yyyy-MM-dd"),
                        dateTimePicker5.Visible ? dateTimePicker5.Value.ToString("yyyy-MM-dd") : dateTimePicker2.Value.ToString("yyyy-MM-dd"),
                        loop,
                        Week_loop.Length > 0 ? (object)Week_loop : DBNull.Value,
                        loop.Equals("EVERY_MONTH") ? (object)(numericUpDown1.Value + "_TO_" + numericUpDown2.Value)  : DBNull.Value,
                        dateTimePicker3.Visible ? (object)dateTimePicker3.Value.ToString("HH:mm:ss") : DBNull.Value,
                        dateTimePicker4.Visible ? (object)dateTimePicker4.Value.ToString("HH:mm:ss") : DBNull.Value,
                        comboBox2.Text,
                        textBox1.Text,
                        textBox2.Text,
                        numericUpDown3.Enabled ? (object)numericUpDown3.Value : DBNull.Value,
                        string.IsNullOrWhiteSpace(related_clients) ? DBNull.Value : (object)related_clients,
                        string.IsNullOrWhiteSpace(related_Animaux) ? DBNull.Value : (object)related_Animaux,
                        selected_img_idx > -1 ? (object)selected_img_idx : DBNull.Value,
                        Userss
                    }, "ID = @IDD", new List<string> { "IDD" }, new List<object>
                    {
                        Current_items_id
                    });

                    //cmmd = "UPDATE `tb_agenda` SET "
                    //       + "`START_TIME` = '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "',"
                    //       + "`END_TIME` = '" + (dateTimePicker5.Visible ? dateTimePicker5.Value.ToString("yyyy-MM-dd") : dateTimePicker2.Value.ToString("yyyy-MM-dd")) + "',"
                    //       + "`EVERY_TYPE` = '" + loop + "',"
                    //       + "`EVERY_WEEK_DAY` = " + (Week_loop.Length > 0 ? "'" + Week_loop + "'" : "NULL") + ","
                    //       + "`EVERY_MONTH_DAY` = " + (loop.Equals("EVERY_MONTH") ? "'" + numericUpDown1.Value + "_TO_" + numericUpDown2.Value + "'" : "NULL") + ","
                    //       + "`HOURS_ARRANG_OF` = " + (dateTimePicker3.Visible ? "'" + dateTimePicker3.Value.ToString("HH:mm:ss") + "'" : "NULL") + ","
                    //       + "`HOURS_ARRANG_TO` = " + (dateTimePicker4.Visible ? "'" + dateTimePicker4.Value.ToString("HH:mm:ss") + "'" : "NULL") + ","
                    //       + "`TYPE` = '" + comboBox2.Text.Replace("'", "''") + "',"
                    //       + "`OBJECT` = '" + textBox1.Text.Replace("'", "''") + "',"
                    //       + "`DESCRIPTION` = '" + textBox2.Text.Replace("'", "''") + "',"
                    //       + "`REPPEL_BEFORE_DAYS` = " + (numericUpDown3.Enabled ? numericUpDown3.Value.ToString() : "NULL") + ","
                    //       + "`RELATED_CLIENTS_IDs` = '" + related_clients + "',"
                    //       + "`RELATED_ANIMALS_IDs` = '" + related_Animaux + "',"
                    //       + "`ICON_ID` = @Icon,"
                    //       + "`FOR_THIS_USERS` = '" + Userss.Replace("'", "''") + "'"
                    //       + " WHERE `ID` = " + Current_items_id + ";";
                }
                //PreConnection.open_conn();
                //MySqlCommand mySqlCommand = new MySqlCommand(cmmd, PreConnection.mySqlConnection);
                ////-------------------

                //if (pictureBox2.Image == null || selected_img_idx == -1)
                //{
                //    mySqlCommand.Parameters.AddWithValue("@Icon", DBNull.Value);
                //}
                //else
                //{
                //    mySqlCommand.Parameters.AddWithValue("@Icon", selected_img_idx);
                //}
                //int row_affected = mySqlCommand.ExecuteNonQuery();
                Agenda_Just_Display.make_update = true;
                Load_all_data();
            }
            else
            {
                MessageBox.Show(msg_err, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void listView_Icons_ItemActivate(object sender, EventArgs e)
        {
            if (label16.Visible) //supprimer les icons
            {
                PreConnection.Excut_Cmd(3, "tb_images", null, null, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { listView_Icons.SelectedItems[0].ImageKey });
                listView_Icons.SelectedItems[0].Remove();
            }
            else //selectionner une icon
            {
                selected_img_idx = int.Parse(listView_Icons.SelectedItems[0].ImageKey);
                pictureBox2.Image = items_icon.Images[listView_Icons.SelectedItems[0].Index + 1];


                panel14.Visible = false;
                textBox1.Focus();
            }

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
            selected_img_idx = -1;
            //---------------------------
            label16.Visible = !label16.Visible;
            listView_Icons.Focus();
        }

        private void Agenda_TEST_MouseDown(object sender, MouseEventArgs e)
        {
            if (panel14.Visible && !panel14.Bounds.Contains(e.Location))
            {
                panel14.Visible = false;
                textBox1.Focus();
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {
            panel14.Visible = false;
            textBox1.Focus();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown3.Enabled = checkBox11.Checked;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            openFileDialog_icon_choose.ShowDialog();
        }

        private void panel14_VisibleChanged(object sender, EventArgs e)
        {
            label16.Visible = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.icons8_camera_30px;
            panel14.Visible = false;
            textBox1.Focus();
            selected_img_idx = -1;
        }

        private void listView_Icons_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                label16.Visible = false;
            }
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            panel15.Visible = true;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            panel15.Visible = false;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            selected_ids_to_delete = string.Empty;
            List<string> list_tmp = new List<string>();
            foreach (Control ctr in flowLayoutPanel1.Controls)
            {
                foreach (Control ctr1 in ctr.Controls)
                {
                    if (ctr1.Name.Contains("Dayy_"))
                    {
                        if (((ListView)ctr1).CheckBoxes)
                        {
                            foreach (ListViewItem itm in ((ListView)ctr1).CheckedItems)
                            {
                                if (!list_tmp.Contains(itm.SubItems[1].Text))
                                {
                                    list_tmp.Add(itm.SubItems[1].Text);
                                }
                            }
                        }
                    }
                }
            }

            if (list_tmp.Count == 0)//selected_ids_to_delete == string.Empty)
            {
                selected_ids_to_delete = Current_items_id.ToString();
            }
            else
            {
                list_tmp.ForEach(pp =>
                {
                    selected_ids_to_delete += "," + pp;
                });
                selected_ids_to_delete = selected_ids_to_delete.Substring(1, selected_ids_to_delete.Length - 1);
            }


            if (MessageBox.Show("Sures de faire la suppression ?\n\nRMQ : L'évenement origine sera supprimer (tous les évènements lies seront supprimés)", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Supprimer_pour_tous_40005)
                {
                    PreConnection.Excut_Cmd_personnel("DELETE FROM tb_agenda WHERE ID IN (" + selected_ids_to_delete + ")", null,null);
                    //PreConnection.Excut_Cmd(3, "tb_agenda", null, null, "ID IN (@P_ID)", new List<string> { "P_ID" }, new List<object> { selected_ids_to_delete });
                }
                else if (Supprimer_pour_juste_lui_40006)
                {
                    int tmpppp = PreConnection.Excut_Cmd_personnel("DELETE FROM tb_agenda WHERE ID IN ("+ selected_ids_to_delete + ") AND `FOR_THIS_USERS` LIKE '"+ Properties.Settings.Default.Last_login_user_idx + "' OR `FOR_THIS_USERS` LIKE CONCAT('%,', "+ Properties.Settings.Default.Last_login_user_idx + " , '%,')  OR `FOR_THIS_USERS` LIKE CONCAT('%,', "+ Properties.Settings.Default.Last_login_user_idx + ")  OR `FOR_THIS_USERS` LIKE CONCAT("+ Properties.Settings.Default.Last_login_user_idx + " , '%,')", null,null);
                    //int tmpppp = PreConnection.Excut_Cmd(3, "tb_agenda", null, null, "ID IN (@P_ID) AND `FOR_THIS_USERS` LIKE @P_Last_login_user_idx OR `FOR_THIS_USERS` LIKE CONCAT('%,', @P_Last_login_user_idx , '%,')  OR `FOR_THIS_USERS` LIKE CONCAT('%,', @P_Last_login_user_idx)  OR `FOR_THIS_USERS` LIKE CONCAT(@P_Last_login_user_idx , '%,')", new List<string> { "P_ID", "P_Last_login_user_idx" }, new List<object> { selected_ids_to_delete, Properties.Settings.Default.Last_login_user_idx });
                    if (list_tmp.Count != tmpppp)
                    {
                        if (tmpppp == 0)
                        {
                            new Non_Autorized_Msg("Vous n'êtes pas autorisé à supprimer ces événements.").Show();
                        }
                        else
                        {
                            new Non_Autorized_Msg("Seuls vos événements sont supprimés.").Show();
                        }

                    }
                }
                Load_all_data();
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Current_items_id = string.Empty;
            Is_New_To_Insert = true;
            pictureBox6.Image = Properties.Resources.NOUVEAU_002;
            //--------------------------
            radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = button7.Visible = true;
            if (!Ajouter_pour_tout_monde_40001) //Ajouter Event pour tout le monde
            {
                radioButton2.Enabled = radioButton3.Enabled = button7.Visible = false;
                radioButton1.Enabled = button7.Visible = Ajouter_pour_juste_lui_40002;
            }
            //----------------------------
            intial_Modify_fields();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in flowLayoutPanel1.Controls)
            {
                foreach (Control ctr1 in ctr.Controls)
                {
                    if (ctr1.Name.Contains("Dayy_"))
                    {
                        if (((ListView)ctr1).CheckBoxes)
                        {
                            ((ListView)ctr1).CheckBoxes = false;
                        }
                    }
                }
            }
            button10.Visible = button15.Visible = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sures de faire la suppression ?\n\nL'évenement : '" + textBox1.Text + "'\n\nRMQ : L'évenement origine sera supprimer (tous les répétition liés seront supprimés)", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Supprimer_pour_tous_40005)
                {
                    PreConnection.Excut_Cmd(3, "tb_agenda", null, null, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { Current_items_id });
                }
                else if (Supprimer_pour_juste_lui_40006)
                {
                    //int tmpppp = PreConnection.Excut_Cmd(3, "tb_agenda", null, null, "ID = @P_ID AND `FOR_THIS_USERS` IN ('',CONCAT(',',@P_LOGIN_USR_ID),CONCAT(@P_LOGIN_USR_ID,','),CONCAT(',',@P_LOGIN_USR_ID,','))", new List<string> { "P_ID,@P_LOGIN_USR_ID" }, new List<object> { Current_items_id, Properties.Settings.Default.Last_login_user_idx });
                    int tmpppp = PreConnection.Excut_Cmd_personnel("DELETE FROM tb_agenda WHERE ID = " + Current_items_id + " AND `FOR_THIS_USERS` IN ('',CONCAT(','," + Properties.Settings.Default.Last_login_user_idx + "),CONCAT(" + Properties.Settings.Default.Last_login_user_idx + ",','),CONCAT(','," + Properties.Settings.Default.Last_login_user_idx + ",','))",null,null);
                    if (tmpppp == 0)
                    {
                        new Non_Autorized_Msg("Vous n'êtes pas autorisé à supprimer cet événement.").Show();
                    }
                }
                else
                {
                    new Non_Autorized_Msg("Vous n'êtes pas autorisé à supprimer cet événement.").Show();
                }
                Load_all_data();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        private void openFileDialog_icon_choose_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Path.GetExtension(openFileDialog_icon_choose.FileName).ToLower() == ".png")
            {
                byte[] imageData = File.Exists(openFileDialog_icon_choose.FileName) ? File.ReadAllBytes(openFileDialog_icon_choose.FileName) : null;
                int affected_rows_nb = PreConnection.Excut_Cmd(1, "tb_images", new List<string> { "NME", "IMG_DATA" }, new List<object> { "", imageData }, null, null, null);
                if (affected_rows_nb > 0)
                {
                    DataTable dt = PreConnection.Load_data("SELECT * FROM tb_images WHERE ID = (SELECT MAX(`ID`) AS LAST_ID FROM tb_images LIMIT 1);");
                    if (dt.Rows.Count > 0)
                    {
                        //-----------------------                            
                        tmmmmp_img = Image.FromFile(openFileDialog_icon_choose.FileName);
                        items_icon.Images.Add(dt.Rows[0]["ID"].ToString(), tmmmmp_img);
                        listView_Icons.SmallImageList = items_icon;
                        //------
                        ListViewItem itmm = new ListViewItem("");
                        itmm.ImageKey = dt.Rows[0]["ID"].ToString();
                        itmm.SubItems.Add(dt.Rows[0]["ID"].ToString());
                        listView_Icons.Items.Add(itmm);

                        //---------
                        selected_img_idx = (int)dt.Rows[0]["ID"];
                        pictureBox2.Image = tmmmmp_img;
                    }
                }

                panel14.Visible = false;
                textBox1.Focus();
            }
            else
            {
                MessageBox.Show("Ce type d'image non accepté !", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void radioButton3_MouseClick(object sender, MouseEventArgs e)
        {
            (new Users_List()).ShowDialog();
            if (Userss.Length == 0 || Userss == Properties.Settings.Default.Last_login_user_idx.ToString())
            {
                radioButton1.Checked = true;
                Userss = Properties.Settings.Default.Last_login_user_idx.ToString();
            }
        }

        private void flowLayoutPanel1_Enter(object sender, EventArgs e)
        {
            if (prev != null)
            {
                if (prev.SelectedItems.Count > 0)
                {
                    listView1_SelectedIndexChanged(prev, null);
                }
            }

        }


        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            Userss = Properties.Settings.Default.Last_login_user_idx.ToString();
        }

        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            Userss = string.Empty;
        }


        private void Agenda_Load(object sender, EventArgs e)
        {
            button9.Visible = Ajouter_pour_tout_monde_40001 || Ajouter_pour_juste_lui_40002; //Ajouter
            //button14.Visible = button15.Visible = button10.Visible = Supprimer_pour_tous_40005 || Supprimer_pour_juste_lui_40006; //Supprimer            
            //-----------------------
            radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = button7.Visible = true;
            if (!Ajouter_pour_tout_monde_40001) //Ajouter Event pour tout le monde
            {
                radioButton2.Enabled = radioButton3.Enabled = button7.Visible = false;
                if (!Ajouter_pour_juste_lui_40002)
                {
                    radioButton1.Enabled = false;
                    foreach (Control ctrl in panel1.Controls)
                    {
                        ctrl.Enabled = false;
                    }
                    foreach (Control ctrl in panel2.Controls)
                    {
                        if (ctrl.Name != "panel1")
                            ctrl.Enabled = false;
                    }
                }
                else
                {
                    radioButton1.Enabled = button7.Visible = true;
                }
            }
            //----------------------------
        }
    }
}

