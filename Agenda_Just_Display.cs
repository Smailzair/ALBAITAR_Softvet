
using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Resources;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
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
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALBAITAR_Softvet
{
    public partial class Agenda_Just_Display : UserControl
    {
        public static int Selected_idss;
        public static bool make_update = false;
        public static bool for_animal = false;
        public static bool make_filter_refresh = false;
        DataTable infos = new DataTable();
        DataTable icons = new DataTable();
        DataTable users = new DataTable();
        ImageList items_icon = new ImageList();
        string Current_items_id = string.Empty;
        ListView prev_selected = null;
        string Userss;
        bool tmp_stop = false;
        bool Modif_autorized = false;

        DateTime tmmp;
        public Agenda_Just_Display(int client_1_animal_2, int? Selected_ids)
        {
            InitializeComponent();
            //---------------------
            Selected_idss = Selected_ids != null ? Selected_ids.Value : 0;
            for_animal = client_1_animal_2 == 2;
            //--------------------
            items_icon.ImageSize = new Size(32, 32);
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
            //infos = PreConnection.Load_data("SELECT * FROM tb_agenda WHERE (`FOR_THIS_USERS` IN (NULL, '') OR `FOR_THIS_USERS` LIKE '" + Properties.Settings.Default.Last_login_user_idx + "' OR `FOR_THIS_USERS` LIKE '%," + Properties.Settings.Default.Last_login_user_idx + ",%' OR `FOR_THIS_USERS` LIKE '%," + Properties.Settings.Default.Last_login_user_idx + "' OR `FOR_THIS_USERS` LIKE '" + Properties.Settings.Default.Last_login_user_idx + ",%')"
            //+ (textBox3.Text.Length > 0 || comboBox1.SelectedIndex > 0 ? " AND (" + (comboBox1.SelectedIndex > 0 ? "`TYPE` LIKE '"+comboBox1.Text+"'" : "") +  (textBox3.Text.Length > 0 ? (comboBox1.SelectedIndex > 0 ? " AND " : "") + "(`OBJECT` LIKE '%" +textBox3.Text+"%' OR `DESCRIPTION` LIKE '%"+textBox3.Text+"%')" : "")  + ")": "") + ";"
            //    );

            infos = PreConnection.Load_data("SELECT * FROM tb_agenda WHERE `FOR_THIS_USERS` IN (NULL, '') OR FIND_IN_SET(" + Properties.Settings.Default.Last_login_user_idx + ",`FOR_THIS_USERS`);");

            // infos = PreConnection.Load_data("SELECT tb1.*,tb2.* FROM tb_agenda tb1 LEFT JOIN (SELECT tbb1.`ID` AS ANIM_ID,tbb1.`NME` AS ANIM_NME,tbb2.`ID` AS CLIENT_ID,CONCAT(tbb2.`FAMNME`,' ',tbb2.`NME`) AS CLIENT_FULL_NME FROM tb_animaux tbb1 LEFT JOIN tb_clients tbb2 ON tbb1.`CLIENT_ID` = tbb2.`ID`) tb2 ON FIND_IN_SET(tb2.`ANIM_ID`, tb1.`RELATED_ANIMALS_IDs`) WHERE (tb1.`FOR_THIS_USERS` IN (NULL, '') OR FIND_IN_SET(" + Properties.Settings.Default.Last_login_user_idx + ",tb1.`FOR_THIS_USERS`));");
            icons = PreConnection.Load_data("SELECT MIN(tb1.`ID`) AS ID,tb2.`MODIF_TIME`,tb2.`NME`,tb1.`IMG_DATA` FROM tb_images tb1 LEFT JOIN tb_images tb2 ON tb1.ID = tb2.ID WHERE tb1.`IMG_DATA` IS NOT NULL  GROUP BY tb1.`IMG_DATA`;");
            users = PreConnection.Load_data("SELECT `ID`,CONCAT(`USER_FAMNME`,' ',`USER_NME`) AS FULL_NME FROM tb_login_and_users;");
            //--------------------------------------
            items_icon.Images.Add("-1", Properties.Resources.icons8_Checkmark_30px);
            if (icons != null)
            {
                icons.Rows.Cast<DataRow>().Where(xx => xx["IMG_DATA"] != DBNull.Value).ToList().ForEach(row =>
                {
                    Image img = PreConnection.ByteArrayToImage((byte[])row["IMG_DATA"]);
                    bool exist = false;
                    for (int i = 0; i < items_icon.Images.Count; i++)
                    {
                        if (PreConnection.ArePicturesEqual(items_icon.Images[i], img))
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        items_icon.Images.Add(row["ID"].ToString(), img);
                    }
                    //------------------------
                    ListViewItem itm = new ListViewItem("");
                    itm.ImageKey = row["ID"].ToString();
                    itm.SubItems.Add(row["ID"].ToString());
                });
            }
            //-------------------------
            int prev_sele = comboBox1.SelectedIndex > -1 ? comboBox1.SelectedIndex : 0;
            int ssss = comboBox1.Items.Count;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("-Tous-");
            infos.AsEnumerable().Select(row => row.Field<string>("TYPE")).Distinct().ForEach(typ => comboBox1.Items.Add(typ));

            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox1.SelectedIndex = ssss == comboBox1.Items.Count ? prev_sele : 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            //--------------------------
            dateTimePicker1_ValueChanged(null, null);
            intial_Modify_fields();
        }
        private void intial_Modify_fields()
        {
            listView1.Items.Clear();
            pictureBox2.Image = Properties.Resources.icons8_camera_30px;
            Userss = Properties.Settings.Default.Last_login_user_idx.ToString();
            label22.Text = DateTime.Now.ToString("dddd dd/MM/yyyy HH:mm");
            label23.Text = DateTime.Now.AddHours(1).ToString("dddd dd/MM/yyyy HH:mm");
            label24.Text = "Une Fois";
            label19.Text = "[1]";
            label17.Text = "[7]";
            label11.Text = "--";
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            checkBox10.Checked = checkBox4.Checked = checkBox5.Checked = checkBox6.Checked = checkBox7.Checked = checkBox8.Checked = checkBox9.Checked = false;
            //-------------            
            label15.Text = "--";
            //-----------------
            listView_Anim.Items.Clear();
            label13.Text = "Animaux :";
            listView_Clients.Items.Clear();
            label12.Text = "Propriétaires :";

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!tmp_stop)
            {
                tmp_stop = true;
                if (((ListView)sender).SelectedItems.Count > 0)
                {
                    button3.Visible = label16.Visible = panel1.Visible = true;
                    //-----------
                    Current_items_id = ((ListView)sender).SelectedItems[0].SubItems[1].Text; //Get the ID
                    Fill_Event_Fields(Current_items_id);


                    if (prev_selected != null && prev_selected != ((ListView)sender))
                    {
                        try
                        {
                            prev_selected.SelectedIndices.Clear();
                            prev_selected.SelectedItems.Clear();
                        }
                        catch { }
                    }
                    prev_selected = ((ListView)sender);


                }
                else
                {
                    button3.Visible = label16.Visible = panel1.Visible = false;
                    //-------------
                    Current_items_id = string.Empty;
                    intial_Modify_fields();
                }
                tmp_stop = false;
            }
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
        private void Fill_Event_Fields(string ID)
        {            
            intial_Modify_fields();
            DataRow row = infos.Rows.Cast<DataRow>().Where(w => w["ID"].ToString() == ID).FirstOrDefault();
            if (row[0] != null)
            {                
                Current_items_id = ID;
                //----------Loading Data------------
                textBox1.Text = row["OBJECT"].ToString(); //OBJECT
                if (row["ICON_ID"] != DBNull.Value)
                {
                    pictureBox2.Image = items_icon.Images[row["ICON_ID"].ToString()]; //ICON
                }
                switch (row["EVERY_TYPE"].ToString())
                {
                    case "ONCE":
                        label24.Text = "Une Fois";
                        break;
                    case "EVERY_DAY":
                        label24.Text = "Chaque Jour";
                        break;
                    case "EVERY_WEEK":
                        label24.Text = "Chaque Semaine";
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
                        label24.Text = "Chaque Mois";
                        string[] separator2 = { "_TO_" };
                        List<string> mnth = row["EVERY_MONTH_DAY"].ToString().Split(separator2, StringSplitOptions.RemoveEmptyEntries).ToList();
                        label19.Text = "[" + int.Parse(mnth[0]).ToString() + "]";
                        label17.Text = "[" + int.Parse(mnth[1]).ToString() + "]";
                        break;
                }
                string strt = ((DateTime)row["START_TIME"]).ToString("dddd dd/MM/yyyy ");
                label22.Text = strt.Substring(0, 1).ToUpper() + strt.Substring(1, strt.Length - 1);
                string endd = ((DateTime)row["END_TIME"]).ToString("dddd dd/MM/yyyy ");
                label23.Text = endd.Substring(0, 1).ToUpper() + endd.Substring(1, endd.Length - 1);
                if (row["HOURS_ARRANG_OF"] != DBNull.Value)
                {
                    label22.Text += TimeSpan.Parse(row["HOURS_ARRANG_OF"].ToString()).Hours + ":" + TimeSpan.Parse(row["HOURS_ARRANG_OF"].ToString()).Minutes;
                    if (row["HOURS_ARRANG_TO"] != DBNull.Value)
                    {
                        label23.Text += TimeSpan.Parse(row["HOURS_ARRANG_TO"].ToString()).Hours + ":" + TimeSpan.Parse(row["HOURS_ARRANG_TO"].ToString()).Minutes;
                    }
                }
                if (row["REPPEL_BEFORE_DAYS"] != DBNull.Value)
                {
                    label11.Text = "Avant : [" + row["REPPEL_BEFORE_DAYS"] + "] Jours";
                }
                label15.Text = row["TYPE"].ToString();
                textBox2.Text = row["DESCRIPTION"].ToString();
                //-------------------
                if (row["RELATED_ANIMALS_IDs"].ToString().Trim().Length > 0)
                {
                    string[] separator3 = { "," };
                    List<string> mnth = row["RELATED_ANIMALS_IDs"].ToString().Split(separator3, StringSplitOptions.RemoveEmptyEntries).ToList();
                    mnth.ForEach(AA =>
                    {
                        DataRow ann = Main_Frm.Main_Frm_animals_tbl.Rows.Cast<DataRow>().Where(h => h["ID"].ToString() == AA).FirstOrDefault();
                        if (ann[0] != null)
                        {
                            ListViewItem dd = new ListViewItem(ann["NME"].ToString());
                            dd.SubItems.Add(ann["ID"].ToString());
                            string sss = "";
                            DataRow clt = Main_Frm.Main_Frm_clients_tbl.Rows.Cast<DataRow>().Where(hhj => hhj["ID"].ToString() == ann["CLIENT_ID"].ToString()).FirstOrDefault();
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
                        DataRow cllt = Main_Frm.Main_Frm_clients_tbl.Rows.Cast<DataRow>().Where(h => h["ID"].ToString() == AA).FirstOrDefault();
                        if (cllt[0] != null)
                        {
                            ListViewItem dd = new ListViewItem(string.Concat(cllt["FAMNME"].ToString(), " ", cllt["NME"].ToString()));
                            dd.SubItems.Add(cllt["ID"].ToString());
                            listView_Clients.Items.Add(dd);
                        }

                    });
                    label12.Text = listView_Clients.Items.Count > 0 ? string.Concat("Propriétaires (", listView_Clients.Items.Count, "):") : "Propriétaires :";
                }
                //-----------------
                if (row["FOR_THIS_USERS"].ToString().Trim().Length > 0)
                {
                    string[] separator5 = { "," };
                    List<string> usrs = row["FOR_THIS_USERS"].ToString().Split(separator5, StringSplitOptions.RemoveEmptyEntries).ToList();
                    users.AsEnumerable().Where(ZZ => usrs.Contains(ZZ["ID"].ToString())).ForEach(RR => listView1.Items.Add(RR["FULL_NME"].ToString()));

                }
                else
                {
                    listView1.Items.Add("- Tout le monde -");
                }
                //--------------
                Userss = row["FOR_THIS_USERS"].ToString();
            }
            else
            {
                Current_items_id = string.Empty;

            }
        }
        private void flowLayoutPanel1_Enter(object sender, EventArgs e)
        {
            if (prev_selected != null)
            {
                if (prev_selected.SelectedItems.Count > 0)
                {
                    listView1_SelectedIndexChanged(prev_selected, null);
                }
            }

        }
        
        private void Load_day_events(ListView lst, string dte)
        {
            lst.Items.Clear();
            //----------------
            DateTime dtt = DateTime.Parse("01/01/1999");
            DateTime.TryParse(dte, out dtt);
            //--------------------------------------------
            DataTable tmp_infos = infos.Copy();
            string fltr = "";
            if (Selected_idss > -1)
            {
                if (for_animal)
                {
                    fltr += "(RELATED_ANIMALS_IDs NOT IN (NULL, '') AND ',' + RELATED_ANIMALS_IDs + ',' LIKE '%," + Selected_idss + ",%')";
                }
                else
                {
                    fltr += "(RELATED_CLIENTS_IDs NOT IN (NULL, '') AND ',' + RELATED_CLIENTS_IDs + ',' LIKE '%," + Selected_idss + ",%')";
                }                
            }
            else  //Tous
            { }
            fltr += (textBox3.Text.Length > 0 || comboBox1.SelectedIndex > 0 ? (fltr.Length > 0 ? " AND " : "") + "(" + (comboBox1.SelectedIndex > 0 ? "`TYPE` LIKE '" + comboBox1.Text + "'" : "")
                + (textBox3.Text.Length > 0 ? (comboBox1.SelectedIndex > 0 ? " AND " : "") +
                "(`OBJECT` LIKE '%" + textBox3.Text + "%'" +
                " OR `DESCRIPTION` LIKE '%" + textBox3.Text + "%'" +
                ")" : "")
                + ")" : "");
            tmp_infos.DefaultView.RowFilter = fltr;
            //-------------------------------------------
            if (tmp_infos.DefaultView.Cast<DataRowView>().Count() > 0 && dtt > DateTime.Parse("01/01/1999"))
            {
                int y = 11;
                tmp_infos.DefaultView.Cast<DataRowView>().Where(EE => dtt >= (DateTime)EE["START_TIME"] && dtt <= (DateTime)EE["END_TIME"]).ToList().ForEach(ZZ =>
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            button3.Visible = panel1.Visible = label16.Visible = false;
            DateTime startDate = DateTime.Parse("01/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Year);
            DateTime endDate = DateTime.Parse(DateTime.DaysInMonth(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month) + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Year);
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
                        tmmp = new DateTime(1900,12,12);
                        DateTime.TryParse(string.Concat((int.Parse(ctr1.Name.Substring(5)) - ddds), "/", dateTimePicker1.Value.Month, "/", dateTimePicker1.Value.Year), out tmmp);
                        ((ListView)ctr1).BackColor = DateTime.Today == tmmp ? Color.HotPink : SystemColors.Window;

                    }
                }
            }
            //------------------------
            linkLabel1.Visible = dateTimePicker1.Value.Month != DateTime.Now.Month;


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

        private void listView_Anim_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                using (var brush = new SolidBrush(Color.BlanchedAlmond))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, e.SubItem.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

        }

        private void listView_Anim_DrawColumnHeader_1(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            Font fnt = new Font(e.Font, FontStyle.Bold);

            if (e.ColumnIndex == 2)
            {
                using (var brush = new SolidBrush(Color.NavajoWhite))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
            TextRenderer.DrawText(e.Graphics, e.Header.Text, fnt, e.Bounds, e.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        private void label24_TextChanged(object sender, EventArgs e)
        {
            panel1.VerticalScroll.Value = panel1.VerticalScroll.Minimum;
            panel4.Visible = label24.Text == "Chaque Semaine";
            panel5.Visible = label24.Text == "Chaque Mois";
            label21.Visible = label23.Visible = label24.Text != "Une Fois";
            label20.Text = label21.Visible ? "De :" : "Au :";
        }

        private void Agenda_Just_Display_Enter(object sender, EventArgs e)
        {
            
            if (make_filter_refresh)
            {
                make_filter_refresh = false;                
                dateTimePicker1_ValueChanged(null, null);

            }else if (make_update)
            {
                make_update = false;
                Load_all_data();
            }
        }


        private void Dayy_1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (button3.Visible && ((ListView)sender).SelectedItems.Count > 0)
            {
                if (Application.OpenForms["Agenda"] != null)
                {
                    Application.OpenForms["Agenda"].Close();
                }
                this.Parent.Focus();
                new Agenda(int.Parse(((ListView)sender).SelectedItems[0].SubItems[1].Text), dateTimePicker1.Value).Show();
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateTimePicker1_ValueChanged(null, null);
        }


        private void button15_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms["Agenda"] != null)
            {
                Application.OpenForms["Agenda"].Close();
            }
            this.Parent.Focus();
            new Agenda(null, null).Show();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            dateTimePicker1_ValueChanged(null, null);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        private void Sam_Flow_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control vw in ((FlowLayoutPanel)sender).Controls.OfType<ListView>())
            {                
                vw.Height = (((FlowLayoutPanel)sender).Height - 60) / 5;
                vw.Width = (((FlowLayoutPanel)sender).Width - 6);
            }
            label2.Width = label3.Width = label4.Width = label5.Width = label6.Width = label7.Width = label8.Width = (((FlowLayoutPanel)sender).Width - 6);         
        }

        private void Agenda_Just_Display_SizeChanged(object sender, EventArgs e)
        {
            Sam_Flow.Height = Dim_Flow.Height = Lun_Flow.Height = Mar_Flow.Height = Mer_Flow.Height = Jeu_Flow.Height = Ven_Flow.Height = (flowLayoutPanel1.ClientSize.Height < flowLayoutPanel1.DisplayRectangle.Height) ? 533 : this.Size.Height - 58;           
            Sam_Flow.Width = Dim_Flow.Width = Lun_Flow.Width = Mar_Flow.Width = Mer_Flow.Width = Jeu_Flow.Width = Ven_Flow.Width = (flowLayoutPanel1.ClientSize.Width > flowLayoutPanel1.DisplayRectangle.Width) ? 162 : (this.Width - 325) / 7;           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Current_items_id.Length > 0)
            {
                if (Application.OpenForms["Agenda"] != null)
                {
                    Application.OpenForms["Agenda"].Close();
                }
                this.Parent.Focus();
                new Agenda(int.Parse(Current_items_id), dateTimePicker1.Value).Show();
            }
        }

        private void Agenda_Just_Display_Load(object sender, EventArgs e)
        {            
            if (Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40000" && (Int32)QQ[3] == 1).Count() == 0)//Consulter nn autoriz
            {
                this.Controls.Add(new Nn_Autorized());
                this.Controls["Nn_Autorized"].Dock = DockStyle.Fill;
                this.Controls["Nn_Autorized"].BringToFront();
            }
            else
            {
                button15.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40001" && (Int32)QQ[3] == 1).Count() > 0 && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40002" && (Int32)QQ[3] == 1).Count() > 0; //Nouveau
                Modif_autorized = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40003" && (Int32)QQ[3] == 1).Count() > 0 && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40004" && (Int32)QQ[3] == 1).Count() > 0; //Modifier
                if (!Modif_autorized)
                {
                    foreach(Control ctrl in panel1.Controls)
                    {
                        if (ctrl is TextBox)
                        {
                            ((TextBox)ctrl).ReadOnly = true;
                        }
                        else
                        {
                            ctrl.Enabled = false;
                        }
                    }
                }
            }
        }

        private void button3_VisibleChanged(object sender, EventArgs e)
        {
            if (!Modif_autorized) { button3.Visible = false; }
        }
    }
}
