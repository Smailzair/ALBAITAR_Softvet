using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Vaccin_Alerts : UserControl
    {
        DataTable data;
        public Vaccin_Alerts()
        {
            InitializeComponent();
            //------------
            if(Main_Frm.Main_Frm_vaccination == null)
            {
                refresh_main_vaccin_tbl();
            }            
            load_data();
        }


        private void refresh_main_vaccin_tbl()
        {
            Main_Frm.Main_Frm_vaccination = PreConnection.Load_data("SELECT *,"
+ "     IF("
+ "         FIXED_DATE >= CURRENT_DATE, "
+ "         FIXED_DATE,"
+ "         IF("
+ "             CURRENT_DATE BETWEEN START_DATE AND END_DATE,"
+ "             IF("
+ "                 STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d') >= CURRENT_DATE,"
+ "                 STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
+ "                 IF("
+ "                     (YEAR(CURRENT_DATE) + 1) <= END_YEAR,"
+ "                     STR_TO_DATE(CONCAT(CAST((YEAR(CURRENT_DATE) + 1) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
+ "                     NULL"
+ "                 )"
+ "             ),"
+ "             IF(CURRENT_DATE < STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
+ "                 STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
+ "                 NULL"
+ "                 )"
+ "         )"
+ "     ) AS NEXT_DATE "
+ " FROM tb_vaccin ORDER BY NEXT_DATE;");
        }
        public void load_data()
        {
            data = PreConnection.Load_data("SELECT tb2.*,"
                                         + "IF(SEND_ALERT_TO_CABINE_EMAIL = 1, IF(LAST_ALERT_EMAIL_CABINET_SENT_DATE <= NEXT_DATE AND LAST_ALERT_EMAIL_CABINET_SENT_DATE >= NEXT_ALARM,'Oui','Non'),'(Désactivé)') AS CABINET_EMAIL_ALREADY_SENT,"
                                         + "IF(SEND_ALERT_TO_CLIENT_EMAIL = 1, IF(LAST_ALERT_EMAIL_CLIENT_SENT_DATE <= NEXT_DATE AND LAST_ALERT_EMAIL_CLIENT_SENT_DATE >= NEXT_ALARM,'Oui','Non'),'(Désactivé)') AS CLIENT_EMAIL_ALREADY_SENT "
                                         + "FROM ("
                                         + "SELECT tb1.*,DATE_SUB(tb1.NEXT_DATE, INTERVAL tb1.ALERT_BEFORE_DAYS DAY) as NEXT_ALARM FROM ("
                                         + "SELECT *,"
                                         + "    IF("
                                         + "        FIXED_DATE >= CURRENT_DATE, "
                                         + "        FIXED_DATE,"
                                         + "        IF("
                                         + "            CURRENT_DATE BETWEEN START_DATE AND END_DATE,"
                                         + "            IF("
                                         + "                STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d') >= CURRENT_DATE,"
                                         + "                STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
                                         + "                IF("
                                         + "                    (YEAR(CURRENT_DATE) + 1) <= END_YEAR,"
                                         + "                    STR_TO_DATE(CONCAT(CAST((YEAR(CURRENT_DATE) + 1) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
                                         + "                    NULL"
                                         + "                )"
                                         + "            ),"
                                         + "            IF(CURRENT_DATE < STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
                                         + "                STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d'),"
                                         + "                NULL"
                                         + "                )"
                                         + "        )"
                                         + "    ) AS NEXT_DATE "
                                         + "FROM tb_vaccin ORDER BY NEXT_DATE DESC) tb1) tb2  WHERE current_date <= NEXT_DATE AND current_date >= NEXT_ALARM;");
            dataGridView1.DataSource = data;
        }

        private void Vaccin_Alerts_Enter(object sender, EventArgs e)
        {
           // load_data();
        }

        private void Vaccin_Alerts_Leave(object sender, EventArgs e)
        {
            ((Panel)Parent).Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ((Panel)Parent).Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                PreConnection.Excut_Cmd(2, "tb_vaccin", new List<string> { "LAST_ALERT_LUE_DATE" }, new List<object> { DateTime.Now.Date }, "ID = @ID", new List<string> { "ID" }, new List<object> { dataGridView1.SelectedRows[0].Cells["ID"].Value });
                //---------
                new Add_Modif_Vaccin((int)dataGridView1.SelectedRows[0].Cells["ID"].Value).ShowDialog();
                refresh_main_vaccin_tbl();
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < dataGridView1.Rows.Count)
            {
                if ((dataGridView1.Rows[e.RowIndex].Cells["LAST_ALERT_LUE_DATE"].Value != DBNull.Value ? (DateTime)dataGridView1.Rows[e.RowIndex].Cells["LAST_ALERT_LUE_DATE"].Value : new DateTime(1999, 12, 12)) < (DateTime)dataGridView1.Rows[e.RowIndex].Cells["NEXT_ALARM"].Value)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            PreConnection.Excut_Cmd(2, "tb_vaccin", new List<string> {"LAST_ALERT_LUE_DATE"}, new List<object>{ DateTime.Now.Date}, "ID = @ID", new List<string> { "ID" }, new List<object> { dataGridView1.Rows[e.RowIndex].Cells["ID"].Value });
            int prev_scrol = dataGridView1.FirstDisplayedScrollingRowIndex;
            load_data();
            dataGridView1.FirstDisplayedScrollingRowIndex = prev_scrol;

        }
    }
}
