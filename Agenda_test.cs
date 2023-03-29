using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Agenda_TEST : Form
    {
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        public Agenda_TEST()
        {
            InitializeComponent();
            //----------------------
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
                            ((ListView)ctr1).BorderStyle= BorderStyle.None;
                        }


                    }
                }
            }
            //------------------------
            //int ss = ((int)startDate.DayOfWeek);
            //if (ss > 0)
            //{
            //    for (int pp = 0; pp < ss; pp++)
            //    {
            //        ((ListView)Controls.Find("Dayy_" + (pp + 1), true).FirstOrDefault()).Columns[0].Text = (startDate.AddDays((ss * -1) + pp)).Day.ToString();
            //        Controls.Find("Dayy_" + (pp + 1), true).FirstOrDefault().BackColor = Color.Gainsboro;
            //        Controls.Find("Dayy_" + (pp + 1), true).FirstOrDefault().Enabled = false;
            //    }
            //}
            ////--------------------------
            //int zsq = 1;
            //while (next_mnth_frst_day_label_nb < 43)
            //{
            //    ((ListView)Controls.Find("Dayy_" + next_mnth_frst_day_label_nb.ToString(), true).FirstOrDefault()).Columns[0].Text = zsq.ToString();                
            //    Controls.Find("Dayy_" + next_mnth_frst_day_label_nb, true).FirstOrDefault().BackColor = Color.Gainsboro;
            //    Controls.Find("Dayy_" + next_mnth_frst_day_label_nb, true).FirstOrDefault().Enabled = false;
            //    zsq++;
            //    next_mnth_frst_day_label_nb++;
            //}
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Label ddsq= new Label();
            ddsq.Text = "fsfsd sdfsdf sdfsd sdfsdff fsd sdfsdfsdf dfdsfdfsfds gdfsgdsfgdsfg sdfgdsfgdsf";
            Dayy_3.Controls.Add(ddsq);
            Dayy_3.AutoSize= true;
            //Dayy_03.AutoSize = false;
        }

    }
}

