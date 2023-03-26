using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Agenda_test : Form
    {
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        int lineHeight;
        public Agenda_test()
        {
            InitializeComponent();
            //----------------------
            lineHeight = TextRenderer.MeasureText("Sample Text", textBox2.Font).Height;
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
                        ctr1.Enabled = true;
                        ctr1.BackColor = Color.White;
                        foreach (Control ctr2 in ctr1.Controls)
                        {
                            if (!ctr2.Name.Contains("label_nb_day_"))
                            {
                                ctr1.Controls.Remove(ctr2);
                            }
                            else
                            {
                                if ((int.Parse(ctr2.Name.Substring(13)) - ddds) <= (endDate.Date).Day)
                                {
                                    ctr2.Text = (int.Parse(ctr2.Name.Substring(13)) - ddds).ToString();
                                }
                                else
                                {


                                    next_mnth_frst_day_label_nb = next_mnth_frst_day_label_nb > int.Parse(ctr2.Name.Substring(13)) || next_mnth_frst_day_label_nb == 0 ? int.Parse(ctr2.Name.Substring(13)) : next_mnth_frst_day_label_nb;
                                    ctr2.Text = (int.Parse(ctr2.Name.Substring(13)) - ddds - (endDate.Date).Day + 1).ToString();
                                }

                            }
                        }
                    }
                }
            }
            //------------------------
            int ss = ((int)startDate.DayOfWeek);
            if (ss > 0)
            {
                for (int pp = 0; pp < ss; pp++)
                {
                    Controls.Find("Dayy_0" + (pp + 1), true).FirstOrDefault().Controls[0].Text = (startDate.AddDays((ss * -1) + pp)).Day.ToString();
                    Controls.Find("Dayy_0" + (pp + 1), true).FirstOrDefault().BackColor = Color.Gainsboro;
                    Controls.Find("Dayy_0" + (pp + 1), true).FirstOrDefault().Enabled = false;
                }
            }
            //--------------------------
            int zsq = 1;
            while (next_mnth_frst_day_label_nb < 43)
            {
                Controls.Find(string.Concat("label_nb_day_", next_mnth_frst_day_label_nb.ToString()), true).FirstOrDefault().Text = zsq.ToString();
                Controls.Find("Dayy_" + next_mnth_frst_day_label_nb, true).FirstOrDefault().BackColor = Color.Gainsboro;
                Controls.Find("Dayy_" + next_mnth_frst_day_label_nb, true).FirstOrDefault().Enabled = false;
                zsq++;
                next_mnth_frst_day_label_nb++;
            }
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
           
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox2.Text += "\r\ndsfsdfsdfsdf sdfdsf dfdsfdf dfsg dsfg dsds gdsf df fd df df";
            textBox2.Size = new Size(textBox2.Width ,lineHeight * textBox2.Lines.Length + 4);
        }
    }
}

