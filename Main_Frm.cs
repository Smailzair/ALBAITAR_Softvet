using ALBAITAR_Softvet.Dialogs;
using ALBAITAR_Softvet.Resources;
using MailKit.Net.Smtp;
using MimeKit;
using MySql.Data.MySqlClient;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using DataTable = System.Data.DataTable;
using MethodInvoker = System.Windows.Forms.MethodInvoker;

namespace ALBAITAR_Softvet
{
    public partial class Main_Frm : Form
    {
        int time_delay = 0;
        static DateTime last_update_time = new DateTime(1900, 12, 31);
        Thread th;
        Thread Activ_Ver;
        Thread Send_Vaccin_Alerts;
        bool close_app_because_act = false;
        public static DataTable ADRESSES_SITES;
        bool sites_table_ready = false;
        public static DataTable Autorisations;
        public static DataTable Params;
        int selected_client_id = -1;
        int selected_animal_id = -1;
        public static DataTable Main_Frm_clients_tbl;
        public static DataTable Main_Frm_animals_tbl;
        public static DataTable Main_Frm_vaccination;
        ImageList tabcontrol_img_lst;
        Font simple_font = new Font("Century Gothic", 9, FontStyle.Regular);
        Font bold_font = new Font("Century Gothic", 10, FontStyle.Bold);
        int prev_sel_rw = -1;
        int frst_scrll = -1;
        int prev_sel_rw_facture = -1;
        int frst_scrll_facture = -1;
        string text_to_add_to_title = "";
        //----------
        DataTable chosen_anim_from_search;
        DataTable chosen_client_from_search;
        //-------------
        static DataTable main_visites_tab;
        bool loading_visites_tab = false;
        static bool ended_loading_visites_tab = false;
        //-------------------
        public static DataTable main_poids_tab;
        //-------------------
        static DataTable main_lab_tab;
        bool loading_lab_tab = false;
        static bool ended_loading_lab_tab = false;
        //-------------------
        DataTable main_financ_tab;
        bool loading_finn_tab = false;
        static bool ended_loading_finn_tab = false;
        //-------------------        
        DataTable main_factures_tbl;
        bool loading_fact_tab = false;
        static bool ended_loading_fact_tab = false;
        //-------------------
        public Main_Frm()
        {
            new Splash().Show();
            //------------------
            InitializeComponent();
            //-------------            
            timer1.Enabled = true;
            //-----------
            tabControl1.Alignment = Properties.Settings.Default.Main_Frm_Tabs_Horientation_Is_Verticatl ? TabAlignment.Left : TabAlignment.Top;
            //-----------------
            dataGridView4.Columns["FINN_DEBIT"].HeaderCell.Style.ForeColor = dataGridView6.Columns[1].DefaultCellStyle.SelectionForeColor = Color.LightCoral;
            dataGridView4.Columns["FINN_CREDIT"].HeaderCell.Style.ForeColor = dataGridView6.Columns[2].DefaultCellStyle.SelectionForeColor = Color.LimeGreen;
            //------------------------
            radioButton1.Font = radioButton6.Font = bold_font;
            //----------------------
            tabcontrol_img_lst = new ImageList();
            tabcontrol_img_lst.ColorDepth = ColorDepth.Depth32Bit;
            tabcontrol_img_lst.Images.AddRange(new Image[]
            {
                Properties.Resources.agenda_001,//Visite analyse
                Properties.Resources.agenda_003,//Labo
                Properties.Resources.icons8_info_30px,//Infos
                Properties.Resources.icons8_tear_off_calendar_30px,//calendar
                Properties.Resources.icons8_info_15px_1,//Red Notification
                Properties.Resources.icons8_dog_30px, //Animaux
                Properties.Resources.icons8_profit_30px, //Monetique
                Properties.Resources.agenda_004 //Vaccination
            });
            tabControl1.ImageList = tabcontrol_img_lst;

            tabPage_visites.ImageIndex = 0;
            tabPage_labo.ImageIndex = 1;
            tabPage_infos.ImageIndex = 2;
            tabPage_Calendar.ImageIndex = 3;
            tabPage_animaux.ImageIndex = 5;
            tabPage_monetique.ImageIndex = 6;
            tabPage_vaccin.ImageIndex = 7;
            //---------------------------
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                Autorisations = PreConnection.Load_data("SELECT `ID`,`CODE`,`AUTOR_TEXT`,Usr_" + Properties.Settings.Default.Last_login_user_idx + " FROM tb_autoriz;");
            }
            //----------------------------
            Params = PreConnection.Load_data("SELECT * FROM tb_params;");
            //------------------------------
            th = new Thread(new ThreadStart(Load_sites_table)); //I use it because of starting performance of "Clients" form
            th.Start();
            th.Join();
            //--------------
            Activ_Ver = new Thread(new ThreadStart(Activ_Verif)); //I use it to verify activation situation (not of RancoSoft)
            Activ_Ver.Start();
            Activ_Ver.Join();
            //--------------


        }

        public void Send_Email_Vaccin_Alerts()
        {
            if (PreConnection.IsInternetAvailable() && Params != null) //If the internet available
            {
                DataTable vaccin_alerts = PreConnection.Load_data("SELECT * FROM ( " +
                                                                  "SELECT * FROM (" +
                                                                  "SELECT " +
                                                                  "concat(tb5.SEX,tb5.FAMNME,' ',tb5.NME) AS CLIENT_FULL_NME," +
                                                                  "tb4.ID AS ANIM_ID,tb4.NME AS ANIM_NME,tb4.NUM_IDENTIF AS ANIM_NUM_IDENTIF,tb4.ESPECE AS ANIM_ESPECE,coalesce(ROUND(DATEDIFF(CURRENT_DATE, tb4.NISS_DATE)/365),'--') AS ANIM_AGE," +
                                                                  "tb3.NEXT_DATE AS DATE,tb3.ID AS VACCIN_ID,tb3.VACCIN_NME,tb3.CABINET_EMAIL_ALREADY_SENT,tb3.CLIENT_EMAIL_ALREADY_SENT,tb3.IS_FOR_ALL," +
                                                                  "tb5.EMAIL AS CLIENT_EMAIL FROM (SELECT * FROM tb_animaux WHERE coalesce(IS_RADIATED, 0) = 0) tb4 RIGHT JOIN (" +
                                                                  "SELECT tb2.*," +
                                                                  "IF(SEND_ALERT_TO_CABINE_EMAIL = 1, IF(LAST_ALERT_EMAIL_CABINET_SENT_DATE <= NEXT_DATE AND LAST_ALERT_EMAIL_CABINET_SENT_DATE >= NEXT_ALARM,'Oui','Non'),'(Désactivé)') AS CABINET_EMAIL_ALREADY_SENT," +
                                                                  "IF(SEND_ALERT_TO_CLIENT_EMAIL = 1, IF(LAST_ALERT_EMAIL_CLIENT_SENT_DATE <= NEXT_DATE AND LAST_ALERT_EMAIL_CLIENT_SENT_DATE >= NEXT_ALARM,'Oui','Non'),'(Désactivé)') AS CLIENT_EMAIL_ALREADY_SENT " +
                                                                  "FROM (" +
                                                                  "SELECT tb1.*,DATE_SUB(tb1.NEXT_DATE, INTERVAL tb1.ALERT_BEFORE_DAYS DAY) as NEXT_ALARM FROM (" +
                                                                  "SELECT *," +
                                                                  "    IF(" +
                                                                  "        FIXED_DATE >= CURRENT_DATE, " +
                                                                  "        FIXED_DATE," +
                                                                  "        IF(" +
                                                                  "            CURRENT_DATE BETWEEN START_DATE AND END_DATE," +
                                                                  "            IF(" +
                                                                  "                STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d') >= CURRENT_DATE," +
                                                                  "                STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d')," +
                                                                  "                IF(" +
                                                                  "                    (YEAR(CURRENT_DATE) + 1) <= END_YEAR," +
                                                                  "                    STR_TO_DATE(CONCAT(CAST((YEAR(CURRENT_DATE) + 1) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d')," +
                                                                  "                    NULL" +
                                                                  "                )" +
                                                                  "            )," +
                                                                  "            IF(CURRENT_DATE < STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d')," +
                                                                  "                STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d')," +
                                                                  "                NULL" +
                                                                  "                )" +
                                                                  "        )" +
                                                                  "    ) AS NEXT_DATE " +
                                                                  "FROM tb_vaccin ORDER BY NEXT_DATE DESC) tb1) tb2  WHERE current_date <= NEXT_DATE AND current_date >= NEXT_ALARM AND IS_FOR_ALL = 0) tb3 ON " +
                                                                  "(length(COALESCE(tb3.ANIM_NUM_IDENs, '')) > 0 AND tb3.ANIM_NUM_IDENs LIKE CONCAT('%',tb4.NUM_IDENTIF,'%')) OR " +
                                                                  "(length(COALESCE(tb3.RELATED_CLIENTS_IDS, '')) > 0 AND (tb3.RELATED_CLIENTS_IDS LIKE CONCAT('%,',tb4.CLIENT_ID,',%') OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT(tb4.CLIENT_ID,',%') OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT('%,',tb4.CLIENT_ID) OR tb3.RELATED_CLIENTS_IDS LIKE tb4.CLIENT_ID)) OR " +
                                                                  "(length(COALESCE(tb3.ANIM_NUM_IDENs, '')) = 0 AND length(COALESCE(tb3.RELATED_CLIENTS_IDS, '')) = 0 AND " +
                                                                  "(length(COALESCE(tb3.ANIM_ESPECE, '')) = 0 OR (length(COALESCE(tb3.ANIM_ESPECE, '')) > 0 AND tb3.ANIM_ESPECE LIKE tb4.ESPECE)) AND " +
                                                                  "(length(COALESCE(tb3.ANIM_RACE, '')) = 0 OR (length(COALESCE(tb3.ANIM_RACE, '')) > 0 AND tb3.ANIM_RACE LIKE tb4.RACE)) AND " +
                                                                  "(length(COALESCE(tb3.ANIM_SEXE, '')) = 0 OR (length(COALESCE(tb3.ANIM_SEXE, '')) > 0 AND tb3.ANIM_SEXE LIKE tb4.SEXE)) AND " +
                                                                  "((COALESCE(tb3.DATE_NISS_MIN, '1900-01-01') = '1900-01-01' AND COALESCE(tb3.DATE_NISS_MAX, '1900-01-01') = '1900-01-01') OR (YEAR(COALESCE(tb3.DATE_NISS_MIN, '1900-01-01')) > 2000  AND YEAR(COALESCE(tb3.DATE_NISS_MAX, '1900-01-01')) > 2000 AND COALESCE(tb4.NISS_DATE, '1800-01-01') BETWEEN COALESCE(tb3.DATE_NISS_MIN, '1900-01-01') AND COALESCE(tb3.DATE_NISS_MAX, '1900-01-01'))) AND " +
                                                                  "(COALESCE(tb3.POIDS_MAX, 0) = 0 OR (COALESCE(tb3.POIDS_MAX, 0) > 0 AND coalesce((SELECT POIDS FROM tb_poids  WHERE ANIM_ID = tb4.ID ORDER BY DATETIME DESC LIMIT 1), -1) BETWEEN 0 AND COALESCE(tb3.POIDS_MAX, 0))) AND" +
                                                                  "(length(COALESCE(tb3.RELATED_CLIENTS_IDS, '')) = 0 OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT('%,',tb4.CLIENT_ID,',%') OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT(tb4.CLIENT_ID,',%') OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT('%,',tb4.CLIENT_ID) OR tb3.RELATED_CLIENTS_IDS LIKE tb4.CLIENT_ID))" +
                                                                  "LEFT JOIN tb_clients tb5 ON tb5.ID = tb4.CLIENT_ID) tb5" +
                                                                  " UNION " +
                                                                  "SELECT DISTINCT(CLIENT_FULL_NME),NULL AS ANIM_ID, '-Tous-' AS ANIM_NME,'--' AS ANIM_NUM_IDENTIF,'-Tous-' AS ANIM_ESPECE,'--' AS ANIM_AGE," +
                                                                  "DATE,VACCIN_ID,VACCIN_NME,CABINET_EMAIL_ALREADY_SENT,CLIENT_EMAIL_ALREADY_SENT,IS_FOR_ALL," +
                                                                  "CLIENT_EMAIL " +
                                                                  "FROM (" +
                                                                  "SELECT tb4.ID AS ANIM_ID,tb4.NME AS ANIM_NME,tb4.NUM_IDENTIF AS ANIM_NUM_IDENTIF,tb4.ESPECE AS ANIM_ESPECE,coalesce(ROUND(DATEDIFF(CURRENT_DATE, tb4.NISS_DATE)/365),NULL) AS ANIM_AGE," +
                                                                  "tb3.NEXT_DATE AS DATE,tb3.ID AS VACCIN_ID,tb3.VACCIN_NME,tb3.CABINET_EMAIL_ALREADY_SENT,tb3.CLIENT_EMAIL_ALREADY_SENT,tb3.IS_FOR_ALL," +
                                                                  "concat(tb6.SEX,tb6.FAMNME,' ',tb6.NME) AS CLIENT_FULL_NME,tb6.EMAIL AS CLIENT_EMAIL FROM (SELECT * FROM tb_animaux WHERE coalesce(IS_RADIATED, 0) = 0) tb4 RIGHT JOIN (" +
                                                                  "SELECT tb2.*," +
                                                                  "IF(SEND_ALERT_TO_CABINE_EMAIL = 1, IF(LAST_ALERT_EMAIL_CABINET_SENT_DATE <= NEXT_DATE AND LAST_ALERT_EMAIL_CABINET_SENT_DATE >= NEXT_ALARM,'Oui','Non'),'(Désactivé)') AS CABINET_EMAIL_ALREADY_SENT," +
                                                                  "IF(SEND_ALERT_TO_CLIENT_EMAIL = 1, IF(LAST_ALERT_EMAIL_CLIENT_SENT_DATE <= NEXT_DATE AND LAST_ALERT_EMAIL_CLIENT_SENT_DATE >= NEXT_ALARM,'Oui','Non'),'(Désactivé)') AS CLIENT_EMAIL_ALREADY_SENT " +
                                                                  "FROM (" +
                                                                  "SELECT tb1.*,DATE_SUB(tb1.NEXT_DATE, INTERVAL tb1.ALERT_BEFORE_DAYS DAY) as NEXT_ALARM FROM (" +
                                                                  "SELECT *," +
                                                                  "    IF(" +
                                                                  "        FIXED_DATE >= CURRENT_DATE, " +
                                                                  "        FIXED_DATE," +
                                                                  "        IF(" +
                                                                  "            CURRENT_DATE BETWEEN START_DATE AND END_DATE," +
                                                                  "            IF(" +
                                                                  "                STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d') >= CURRENT_DATE," +
                                                                  "                STR_TO_DATE(CONCAT(CAST(YEAR(CURRENT_DATE) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d')," +
                                                                  "                IF(" +
                                                                  "                    (YEAR(CURRENT_DATE) + 1) <= END_YEAR," +
                                                                  "                    STR_TO_DATE(CONCAT(CAST((YEAR(CURRENT_DATE) + 1) AS CHAR),'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d')," +
                                                                  "                    NULL" +
                                                                  "                )" +
                                                                  "            )," +
                                                                  "            IF(CURRENT_DATE < STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d')," +
                                                                  "                STR_TO_DATE(CONCAT(START_YEAR,'-',EVERY_MOUNTH_NB,'-',EVERY_DAY_NB), '%Y-%m-%d')," +
                                                                  "                NULL" +
                                                                  "                )" +
                                                                  "        )" +
                                                                  "    ) AS NEXT_DATE " +
                                                                  "FROM tb_vaccin ORDER BY NEXT_DATE DESC) tb1) tb2  WHERE current_date <= NEXT_DATE AND current_date >= NEXT_ALARM AND IS_FOR_ALL = 1) tb3 ON " +
                                                                  "(length(COALESCE(tb3.ANIM_NUM_IDENs, '')) > 0 AND tb3.ANIM_NUM_IDENs LIKE CONCAT('%',tb4.NUM_IDENTIF,'%')) OR " +
                                                                  "(length(COALESCE(tb3.RELATED_CLIENTS_IDS, '')) > 0 AND (tb3.RELATED_CLIENTS_IDS LIKE CONCAT('%,',tb4.CLIENT_ID,',%') OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT(tb4.CLIENT_ID,',%') OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT('%,',tb4.CLIENT_ID) OR tb3.RELATED_CLIENTS_IDS LIKE tb4.CLIENT_ID)) OR " +
                                                                  "(length(COALESCE(tb3.ANIM_NUM_IDENs, '')) = 0 AND length(COALESCE(tb3.RELATED_CLIENTS_IDS, '')) = 0 AND " +
                                                                  "(length(COALESCE(tb3.ANIM_ESPECE, '')) = 0 OR (length(COALESCE(tb3.ANIM_ESPECE, '')) > 0 AND tb3.ANIM_ESPECE LIKE tb4.ESPECE)) AND " +
                                                                  "(length(COALESCE(tb3.ANIM_RACE, '')) = 0 OR (length(COALESCE(tb3.ANIM_RACE, '')) > 0 AND tb3.ANIM_RACE LIKE tb4.RACE)) AND " +
                                                                  "(length(COALESCE(tb3.ANIM_SEXE, '')) = 0 OR (length(COALESCE(tb3.ANIM_SEXE, '')) > 0 AND tb3.ANIM_SEXE LIKE tb4.SEXE)) AND " +
                                                                  "((COALESCE(tb3.DATE_NISS_MIN, '1900-01-01') = '1900-01-01' AND COALESCE(tb3.DATE_NISS_MAX, '1900-01-01') = '1900-01-01') OR (YEAR(COALESCE(tb3.DATE_NISS_MIN, '1900-01-01')) > 2000  AND YEAR(COALESCE(tb3.DATE_NISS_MAX, '1900-01-01')) > 2000 AND COALESCE(tb4.NISS_DATE, '1800-01-01') BETWEEN COALESCE(tb3.DATE_NISS_MIN, '1900-01-01') AND COALESCE(tb3.DATE_NISS_MAX, '1900-01-01'))) AND " +
                                                                  "(COALESCE(tb3.POIDS_MAX, 0) = 0 OR (COALESCE(tb3.POIDS_MAX, 0) > 0 AND coalesce((SELECT POIDS FROM tb_poids  WHERE ANIM_ID = tb4.ID ORDER BY DATETIME DESC LIMIT 1), -1) BETWEEN 0 AND COALESCE(tb3.POIDS_MAX, 0))) AND" +
                                                                  "(length(COALESCE(tb3.RELATED_CLIENTS_IDS, '')) = 0 OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT('%,',tb4.CLIENT_ID,',%') OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT(tb4.CLIENT_ID,',%') OR tb3.RELATED_CLIENTS_IDS LIKE CONCAT('%,',tb4.CLIENT_ID) OR tb3.RELATED_CLIENTS_IDS LIKE tb4.CLIENT_ID))" +
                                                                  "LEFT JOIN tb_clients tb6 ON tb6.ID = tb4.CLIENT_ID) tb6) tb7 ORDER BY CLIENT_FULL_NME,DATE;");//WHERE tb6.CABINET_EMAIL_ALREADY_SENT LIKE 'Non' OR tb6.CLIENT_EMAIL_ALREADY_SENT LIKE 'Non'

                if (vaccin_alerts != null)
                {
                    if (vaccin_alerts.Rows.Count > 0)
                    {
                        button33.Invoke((MethodInvoker)delegate
                        {
                            button33.Visible = true;
                        });
                        //================= I - For te clients ===============================================
                        //                        vaccin_alerts.AsEnumerable().Where(G => (string)G["CLIENT_EMAIL_ALREADY_SENT"] == "Non" && !string.IsNullOrWhiteSpace(G["CLIENT_EMAIL"] != DBNull.Value ? (string)G["CLIENT_EMAIL"] : "")).Select(H => H["CLIENT_FULL_NME"]).Distinct().ForEach(D =>
                        //                        {
                        //                            string client_full_nme = (string)D;

                        //                            var builder = new BodyBuilder();
                        //                            builder.HtmlBody = @"<body>
                        //    <style>
                        //        table {
                        //            border-collapse: collapse;
                        //            font-family: Arial, sans-serif;
                        //        }

                        //        th,
                        //        td {
                        //            border: 1px solid rgb(214, 214, 214);
                        //            padding: 5px;
                        //        }

                        //        p {
                        //            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        //            color: black;
                        //        }
                        //    </style>
                        //    <p>Cher propriétaire d'animal, " + client_full_nme + @":</p>
                        //    <p>Nous espérons que vous et votre animal vous portez bien. Nous tenons à vous rappeler que la date de vaccination
                        //        de votre animal de compagnie approche à grands pas. La santé et le bien-être de votre animal de compagnie sont
                        //        d'une importance capitale pour nous, c'est pourquoi nous souhaitons vous assurer que votre animal reçoit les
                        //        soins appropriés et les vaccins nécessaires pour rester en bonne santé.</p>";

                        //                            var specif = vaccin_alerts.AsEnumerable().Where(K => (string)K["CLIENT_FULL_NME"] == client_full_nme && (int)K["IS_FOR_ALL"] == 0);

                        //                            if (specif.Any())
                        //                            {
                        //                                builder.HtmlBody += @"<p>Veuillez noter que le rendez-vous pour la vaccination de votre animal est prévu comme de suite:</p>
                        //    <p style=""color: chocolate; text-decoration: underline;"">&#8226; Vaccinations Spécifiées :</p>
                        //    <table>
                        //        <tr
                        //            style=""background-color: rgb(78, 83, 160); color: rgb(255, 255, 255); font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif"">
                        //            <th>Date</th>
                        //            <th>Vaccination</th>
                        //            <th>Animal (Nom)</th>
                        //            <th>N° Ident.</th>
                        //            <th>Espéce</th>
                        //            <th>Age</th>
                        //        </tr>";
                        //                                specif.ForEach(X =>
                        //                                {
                        //                                    builder.HtmlBody += @"<tr>
                        //            <td>" + ((DateTime)X["DATE"]).ToString("dd/MM/yyyy") + @"</td>
                        //            <td>" + X["VACCIN_NME"] + @"</td>
                        //            <td>" + X["ANIM_NME"] + @"</td>
                        //            <td>" + X["ANIM_NUM_IDENTIF"] + @"</td>
                        //            <td>" + X["ANIM_ESPECE"] + @"</td>
                        //            <td>" + X["ANIM_AGE"] + @"</td>
                        //        </tr>";
                        //                                });
                        //                                builder.HtmlBody += "</table>";
                        //                            }

                        //                            var for_all = vaccin_alerts.AsEnumerable().Where(K => (string)K["CLIENT_FULL_NME"] == client_full_nme && (int)K["IS_FOR_ALL"] == 1);
                        //                            if (for_all.Any())
                        //                            {
                        //                                builder.HtmlBody += @"<p style=""color: rgb(0, 92, 145); text-decoration: underline;"">&#8226; Vaccinations Globales :</p>
                        //    <table>
                        //        <tr
                        //            style=""background-color: rgb(78, 83, 160); color: rgb(255, 255, 255); font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif"">
                        //            <th>Date</th>
                        //            <th>Vaccination</th>
                        //            <th>Animal</th>
                        //        </tr>";
                        //                                for_all.ForEach(X =>
                        //                                {
                        //                                    builder.HtmlBody += @"<tr>
                        //            <td>" + ((DateTime)X["DATE"]).ToString("dd/MM/yyyy") + @"</td>
                        //            <td>" + X["VACCIN_NME"] + @"</td>
                        //            <td>" + X["ANIM_NME"] + @"</td>
                        //        </tr>";
                        //                                });
                        //                                builder.HtmlBody += "</table>";
                        //                            }

                        //                            builder.HtmlBody += @"<p>Nous vous prions de bien vouloir prendre les dispositions nécessaires pour être présent à l'heure convenue. Si
                        //        vous rencontrez des problèmes ou si vous avez des questions, n'hésitez pas à nous contacter immédiatement afin
                        //        que nous puissions trouver une solution adaptée à vos besoins.<br><br>
                        //        Nous nous engageons à fournir les meilleurs soins possibles à votre animal de compagnie, et nous vous remercions
                        //        pour votre confiance continue en nos services vétérinaires. Si vous avez besoin de plus d'informations ou si
                        //        vous souhaitez discuter de tout aspect spécifique de la vaccination, n'hésitez pas à nous contacter.<br><br>
                        //        Nous sommes impatients de vous accueillir avec votre animal de compagnie à la date convenue pour assurer sa
                        //        santé et son bien-être à long terme.<br><br>
                        //        Cordialement,</p>

                        //<p style=""color: rgb(146, 100, 0);"">--------------------------------------------<br>
                        //                            <span style=""font-weight: bold; color: rgb(95, 182, 95);"">" + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + @"</span><br>";

                        //                            if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                        //                            {
                        //                                builder.HtmlBody += @"Tél: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                        //                            }
                        //                            if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                        //                            {
                        //                                builder.HtmlBody += @"Email: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                        //                            }
                        //                            if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                        //                            {
                        //                                builder.HtmlBody += @"Adresse: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                        //                            }
                        //                            builder.HtmlBody += "--------------------------------------------</p>" +
                        //                            "<p style=\"color: rgb(184, 223, 235);\">RancoSoft&copy;</p>" +
                        //                            "</body>";


                        //                            //---------------------------------------

                        //                            MimeMessage Mssg = new MimeMessage();
                        //                            Mssg.From.Add(new MailboxAddress("RancoSoft", "rancosoft@gmail.com"));
                        //                            string clt_email = vaccin_alerts.AsEnumerable().Where(G => (G["CLIENT_FULL_NME"] != DBNull.Value ? (string)G["CLIENT_FULL_NME"] : "") == client_full_nme).FirstOrDefault()["CLIENT_EMAIL"].ToString();
                        //                            Mssg.To.Add(MailboxAddress.Parse(clt_email));
                        //                            Mssg.Subject = "ALBAITAR Softvet - Rappel de rendez-vous pour la vaccination de votre animal";
                        //                            Mssg.Body = builder.ToMessageBody();
                        //                            if (PreConnection.IsInternetAvailable()) //If the internet available
                        //                            {
                        //                                SmtpClient clnt = new SmtpClient();
                        //                                try
                        //                                {
                        //                                    clnt.Connect("smtp.gmail.com", 465, true);
                        //                                    clnt.Authenticate("rancosoft@gmail.com", PreConnection.Traduct_Codified_txt(Properties.Settings.Default.RANCOSOFT_GMAIL_AUTHENT));
                        //                                    clnt.Send(Mssg);
                        //                                    //------------------                                    
                        //                                }
                        //                                catch
                        //                                {
                        //                                    return;
                        //                                }
                        //                                finally
                        //                                {
                        //                                    clnt.Disconnect(true);
                        //                                    clnt.Dispose();
                        //                                }
                        //                                //-----------

                        //                            }
                        //                        });
                        //                        string vaccin_ids = "";
                        //                        vaccin_alerts.AsEnumerable().Select(E => E["VACCIN_ID"]).Distinct().ForEach(Z => vaccin_ids += "," + Z);
                        //                        vaccin_ids = vaccin_ids.Length > 0 ? vaccin_ids.Substring(1) : vaccin_ids;
                        //                        if (vaccin_ids.Length > 0)
                        //                        {
                        //                            PreConnection.Excut_Cmd_personnel("UPDATE tb_vaccin SET LAST_ALERT_EMAIL_CLIENT_SENT_DATE = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE ID IN (" + vaccin_ids + ");", null, null);
                        //                        }
                        vaccin_alerts.AsEnumerable().Where(G => (string)G["CLIENT_EMAIL_ALREADY_SENT"] == "Non" && !string.IsNullOrWhiteSpace(G["CLIENT_EMAIL"] != DBNull.Value ? (string)G["CLIENT_EMAIL"] : "")).Select(H => H["CLIENT_FULL_NME"]).Distinct().ForEach(D =>
                        {
                            string client_full_nme = (string)D;

                            var builder = new BodyBuilder();
                            builder.HtmlBody = @"<body>
    <style>
        table {
            border-collapse: collapse;
            font-family: Arial, sans-serif;
        }

        th,
        td {
            border: 1px solid rgb(214, 214, 214);
            padding: 5px;
        }

        p {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: black;
        }
    </style>
    <p>Cher Client(e), " + client_full_nme + @":</p>
    <p>Dans notre souci de préserver la santé de votre animal, nous tenons à vous rappeler que sa date de vaccination
        approche à grands pas. Sa santé et son bien-être sont d'une importance capitale pour nous, c'est pour cette
        raison que nous souhaitons lui assurer les soins appropriés et les vaccins nécessaires pour sa bonne
        santé.</p>";

                            var specif = vaccin_alerts.AsEnumerable().Where(K => (string)K["CLIENT_FULL_NME"] == client_full_nme && (int)K["IS_FOR_ALL"] == 0);

                            if (specif.Any())
                            {
                                builder.HtmlBody += @"<p>Veuillez noter que le rendez-vous pour la vaccination de votre compagnon est prévu comme suite:</p>
    <p style=""color: chocolate; text-decoration: underline;"">&#8226; Vaccinations Spécifiées :</p>
    <table>
        <tr
            style=""background-color: rgb(78, 83, 160); color: rgb(255, 255, 255); font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif"">
            <th>Date</th>
            <th>Vaccination</th>
            <th style="" background-color: rgb(1, 172, 92);"">Animal (Nom)</th>
            <th style="" background-color: rgb(1, 172, 92);"">N° Ident.</th>
            <th style="" background-color: rgb(1, 172, 92);"">Espéce</th>
            <th style="" background-color: rgb(1, 172, 92);"">Age</th>
        </tr>";
                                specif.ForEach(X =>
                                {
                                    builder.HtmlBody += @"<tr>
            <td>" + ((DateTime)X["DATE"]).ToString("dd/MM/yyyy") + @"</td>
            <td>" + X["VACCIN_NME"] + @"</td>
            <td>" + X["ANIM_NME"] + @"</td>
            <td>" + X["ANIM_NUM_IDENTIF"] + @"</td>
            <td>" + X["ANIM_ESPECE"] + @"</td>
            <td>" + X["ANIM_AGE"] + @"</td>
        </tr>";
                                });
                                builder.HtmlBody += "</table>";
                            }

                            var for_all = vaccin_alerts.AsEnumerable().Where(K => (string)K["CLIENT_FULL_NME"] == client_full_nme && (int)K["IS_FOR_ALL"] == 1);
                            if (for_all.Any())
                            {
                                builder.HtmlBody += @"<p style=""color: rgb(0, 92, 145); text-decoration: underline;"">&#8226; Vaccinations Globales :</p>
    <table>
        <tr
            style=""background-color: rgb(78, 83, 160); color: rgb(255, 255, 255); font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif"">
            <th>Date</th>
            <th>Vaccination</th>
            <th>Animal</th>
        </tr>";
                                for_all.ForEach(X =>
                                {
                                    builder.HtmlBody += @"<tr>
            <td>" + ((DateTime)X["DATE"]).ToString("dd/MM/yyyy") + @"</td>
            <td>" + X["VACCIN_NME"] + @"</td>
            <td>" + X["ANIM_NME"] + @"</td>
        </tr>";
                                });
                                builder.HtmlBody += "</table>";
                            }

                            builder.HtmlBody += @"<p>Et vous prions de bien vouloir nous contacter pour confirmer le jour et l’heure qui vous conviennent le plus.<br>
        En cas de doute ou d’information, n'hésitez pas à nous contacter afin de trouver une solution adaptée à vos
        besoins.<br><br>
        Nous nous engageons à fournir les meilleurs soins possibles à votre animal de compagnie, et nous vous remercions
        pour votre confiance inconditionnelle en nos services.<br>
        Nous sommes impatients de vous accueillir dans notre centre à la date convenue et espérons être à la hauteur de
        vos attentes.
        <br><br>
        Cordialement.</p>
    
<p style=""color: rgb(146, 100, 0);"">--------------------------------------------<br>
                            <span style=""font-weight: bold; color: rgb(95, 182, 95);"">" + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + @"</span><br>";

                            if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                            {
                                builder.HtmlBody += @"Tél: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                            }
                            if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                            {
                                builder.HtmlBody += @"Email: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                            }
                            if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                            {
                                builder.HtmlBody += @"Adresse: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                            }
                            builder.HtmlBody += "--------------------------------------------</p>" +
                            "<p style=\"color: rgb(184, 223, 235);\">RancoSoft&copy;</p>" +
                            "</body>";


                            //---------------------------------------

                            MimeMessage Mssg = new MimeMessage();
                            Mssg.From.Add(new MailboxAddress("RancoSoft", "rancosoft@gmail.com"));
                            string clt_email = vaccin_alerts.AsEnumerable().Where(G => (G["CLIENT_FULL_NME"] != DBNull.Value ? (string)G["CLIENT_FULL_NME"] : "") == client_full_nme).FirstOrDefault()["CLIENT_EMAIL"].ToString();
                            Mssg.To.Add(MailboxAddress.Parse(clt_email));
                            Mssg.Subject = "ALBAITAR Softvet - Rappel de rendez-vous pour la vaccination de votre animal";
                            Mssg.Body = builder.ToMessageBody();
                            if (PreConnection.IsInternetAvailable()) //If the internet available
                            {
                                SmtpClient clnt = new SmtpClient();
                                try
                                {
                                    clnt.Connect("smtp.gmail.com", 465, true);
                                    clnt.Authenticate("rancosoft@gmail.com", PreConnection.Traduct_Codified_txt(Properties.Settings.Default.RANCOSOFT_GMAIL_AUTHENT));
                                    clnt.Send(Mssg);
                                    //------------------                                    
                                }
                                catch
                                {
                                    return;
                                }
                                finally
                                {
                                    clnt.Disconnect(true);
                                    clnt.Dispose();
                                }
                                //-----------

                            }
                        });
                        string vaccin_ids = "";
                        vaccin_alerts.AsEnumerable().Select(E => E["VACCIN_ID"]).Distinct().ForEach(Z => vaccin_ids += "," + Z);
                        vaccin_ids = vaccin_ids.Length > 0 ? vaccin_ids.Substring(1) : vaccin_ids;
                        if (vaccin_ids.Length > 0)
                        {
                            PreConnection.Excut_Cmd_personnel("UPDATE tb_vaccin SET LAST_ALERT_EMAIL_CLIENT_SENT_DATE = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE ID IN (" + vaccin_ids + ");", null, null);
                        }
                        //================= II - For the Cabinet ===============================================

                        //                        string cabinet_nme = Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1 && !string.IsNullOrWhiteSpace(QQ["VAL"] != DBNull.Value ? (string)QQ["VAL"] : "")).Any() ? Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() : "";
                        //                        string cabinet_email = Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3 && !string.IsNullOrWhiteSpace(QQ["VAL"] != DBNull.Value ? (string)QQ["VAL"] : "")).Any() ? Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() : "";

                        //                        if (!string.IsNullOrWhiteSpace(cabinet_nme) && !string.IsNullOrWhiteSpace(cabinet_email))
                        //                        {
                        //                            var vaccin_alertss = vaccin_alerts.AsEnumerable().Where(G => (string)G["CABINET_EMAIL_ALREADY_SENT"] == "Non");
                        //                            if (vaccin_alertss.Any())
                        //                            {
                        //                                var builder = new BodyBuilder();
                        //                                builder.HtmlBody = @"<body>
                        //                                                        <style>
                        //                                                            table {
                        //                                                                border-collapse: collapse;
                        //                                                                font-family: Arial, sans-serif;
                        //                                                            }
                        //                                                             th,
                        //                                                             td {
                        //                                                                 border: 1px solid rgb(214, 214, 214);
                        //                                                                 padding: 5px;
                        //                                                             }

                        //                                                             p {
                        //                                                                 font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        //                                                                 color: black;
                        //                                                             }
                        //                                                         </style>
                        //                                                         <p>Cher personnel du cabinet '" + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + @"',<br>
                        //                                                             Nous souhaitons attirer votre attention sur les rendez-vous de vaccination proches qui nécessitent une préparation adéquate. Il est important de vous assurer que vous disposez des ressources nécessaires et que votre équipe est prête à fournir des services de qualité supérieure à nos patients à fourrure bien-aimés.<br>
                        //                                                             Veuillez assurer que les fournitures médicales pertinentes sont en stock.</p>
                        //                                                             </p>";
                        //                                var specif = vaccin_alertss.AsEnumerable().Where(K => (int)K["IS_FOR_ALL"] == 0);
                        //                                if (specif.Any())
                        //                                {
                        //                                    builder.HtmlBody += @"<p style=""color: chocolate; text-decoration: underline;"">&#8226; Vaccinations Spécifiées :</p>
                        //    <table>
                        //        <tr
                        //            style=""background-color: rgb(78, 83, 160); color: rgb(255, 255, 255); font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif"">
                        //            <th>Date</th>
                        //            <th>Vaccination</th>
                        //            <th style=""background-color: rgb(1, 172, 92);"">Animal (Nom)</th>
                        //            <th style=""background-color: rgb(1, 172, 92);"">N° Ident.</th>
                        //            <th style=""background-color: rgb(1, 172, 92);"">Espéce</th>
                        //            <th style=""background-color: rgb(1, 172, 92);"">Age</th>
                        //            <th style=""background-color: rgb(172, 35, 1);"">Propriétaire</th>
                        //            <th style=""background-color: rgb(172, 35, 1);"">Propriétaire Email</th>
                        //            <th style=""background-color: rgb(172, 35, 1);"">Rappel email envoyé au propriétaire?</th>
                        //        </tr>";
                        //                                    specif.ForEach(X =>
                        //                                    {
                        //                                        builder.HtmlBody += @"<tr>
                        //            <td>" + ((DateTime)X["DATE"]).ToString("dd/MM/yyyy") + @"</td>
                        //            <td>" + X["VACCIN_NME"] + @"</td>
                        //            <td>" + X["ANIM_NME"] + @"</td>
                        //            <td>" + X["ANIM_NUM_IDENTIF"] + @"</td>
                        //            <td>" + X["ANIM_ESPECE"] + @"</td>
                        //            <td>" + X["ANIM_AGE"] + @"</td>
                        //            <td>" + X["CLIENT_FULL_NME"] + @"</td>
                        //            <td>" + X["CLIENT_EMAIL"] + @"</td>
                        //            <td>" + X["CLIENT_EMAIL_ALREADY_SENT"] + @"</td>
                        //        </tr>";
                        //                                    });
                        //                                    builder.HtmlBody += "</table>";
                        //                                }

                        //                                var for_all = vaccin_alertss.AsEnumerable().Where(K => (int)K["IS_FOR_ALL"] == 1);
                        //                                if (for_all.Any())
                        //                                {
                        //                                    builder.HtmlBody += @"<p style=""color: rgb(0, 92, 145); text-decoration: underline;"">&#8226; Vaccinations Globales :</p>
                        //    <table>
                        //        <tr
                        //            style=""background-color: rgb(78, 83, 160); color: rgb(255, 255, 255); font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif"">
                        //            <th>Date</th>
                        //            <th>Vaccination</th>
                        //            <th>Animal</th>
                        //        </tr>";

                        //                                    List<int> alredy_wrote = new List<int>();
                        //                                    for_all.ForEach(X =>
                        //                                    {
                        //                                        if (!alredy_wrote.Contains((int)X["VACCIN_ID"]))
                        //                                        {
                        //                                            alredy_wrote.Add((int)X["VACCIN_ID"]);

                        //                                            builder.HtmlBody += @"<tr>
                        //            <td>" + ((DateTime)X["DATE"]).ToString("dd/MM/yyyy") + @"</td>
                        //            <td>" + X["VACCIN_NME"] + @"</td>
                        //            <td>" + X["ANIM_NME"] + @"</td>
                        //        </tr>";
                        //                                        }
                        //                                    });
                        //                                    builder.HtmlBody += "</table>";
                        //                                }

                        //                                builder.HtmlBody += @"<p>Nous encourageons également une communication proactive avec les propriétaires d'animaux pour confirmer leurs rendez-vous et leur fournir toute information supplémentaire dont ils pourraient avoir besoin.<br>
                        //        N'hésitez pas à contacter notre équipe de gestion si vous avez des questions ou des préoccupations concernant les rendez-vous à venir. Votre diligence et votre engagement envers le bien-être des animaux de compagnie sont grandement appréciés.<br>
                        //        Avec nos remerciements anticipés pour votre attention et votre soin continus.</p>

                        //<p style=""color: rgb(146, 100, 0);"">--------------------------------------------<br>
                        //                            <span style=""font-weight: bold; color: rgb(95, 182, 95);"">" + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + @"</span><br>";

                        //                                if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                        //                                {
                        //                                    builder.HtmlBody += @"Tél: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                        //                                }
                        //                                builder.HtmlBody += @"Email: " + cabinet_email + "<br>";
                        //                                if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                        //                                {
                        //                                    builder.HtmlBody += @"Adresse: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                        //                                }
                        //                                builder.HtmlBody += "--------------------------------------------</p>" +
                        //                                "<p style=\"color: rgb(184, 223, 235);\">RancoSoft&copy;</p>" +
                        //                                "</body>";
                        string cabinet_nme = Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1 && !string.IsNullOrWhiteSpace(QQ["VAL"] != DBNull.Value ? (string)QQ["VAL"] : "")).Any() ? Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() : "";
                        string cabinet_email = Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3 && !string.IsNullOrWhiteSpace(QQ["VAL"] != DBNull.Value ? (string)QQ["VAL"] : "")).Any() ? Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() : "";

                        if (!string.IsNullOrWhiteSpace(cabinet_nme) && !string.IsNullOrWhiteSpace(cabinet_email))
                        {
                            var vaccin_alertss = vaccin_alerts.AsEnumerable().Where(G => (string)G["CABINET_EMAIL_ALREADY_SENT"] == "Non");
                            if (vaccin_alertss.Any())
                            {
                                var builder = new BodyBuilder();
                                builder.HtmlBody = @"<body>
                                                        <style>
                                                            table {
                                                                border-collapse: collapse;
                                                                font-family: Arial, sans-serif;
                                                            }
                                                             th,
                                                             td {
                                                                 border: 1px solid rgb(214, 214, 214);
                                                                 padding: 5px;
                                                             }

                                                             p {
                                                                 font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                                                 color: black;
                                                             }
                                                         </style>
                                                         <p>Cher personnel du cabinet '" + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + @"',<br>
                                                             Nous souhaitons attirer votre attention sur les rendez-vous des prochaines vaccinations qui nécessitent une
        préparation adéquate. Il est important de vous assurer que vous disposez dans vos stocks les vaccins nécessaires
        et que votre équipe est prête à fournir des services de qualité à nos patients à 4 pattes bien-aimés.<br>
                                                             </p>";
                                var specif = vaccin_alertss.AsEnumerable().Where(K => (int)K["IS_FOR_ALL"] == 0);
                                if (specif.Any())
                                {
                                    builder.HtmlBody += @"<p style=""color: chocolate; text-decoration: underline;"">&#8226; Vaccinations Spécifiées :</p>
    <table>
        <tr
            style=""background-color: rgb(78, 83, 160); color: rgb(255, 255, 255); font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif"">
            <th>Date</th>
            <th>Vaccination</th>
            <th style=""background-color: rgb(1, 172, 92);"">Animal (Nom)</th>
            <th style=""background-color: rgb(1, 172, 92);"">N° Ident.</th>
            <th style=""background-color: rgb(1, 172, 92);"">Espéce</th>
            <th style=""background-color: rgb(1, 172, 92);"">Age</th>
            <th style=""background-color: rgb(172, 35, 1);"">Propriétaire</th>
            <th style=""background-color: rgb(172, 35, 1);"">Propriétaire Email</th>
            <th style=""background-color: rgb(172, 35, 1);"">Rappel email envoyé au propriétaire?</th>
        </tr>";
                                    specif.ForEach(X =>
                                    {
                                        builder.HtmlBody += @"<tr>
            <td>" + ((DateTime)X["DATE"]).ToString("dd/MM/yyyy") + @"</td>
            <td>" + X["VACCIN_NME"] + @"</td>
            <td>" + X["ANIM_NME"] + @"</td>
            <td>" + X["ANIM_NUM_IDENTIF"] + @"</td>
            <td>" + X["ANIM_ESPECE"] + @"</td>
            <td>" + X["ANIM_AGE"] + @"</td>
            <td>" + X["CLIENT_FULL_NME"] + @"</td>
            <td>" + X["CLIENT_EMAIL"] + @"</td>
            <td>" + X["CLIENT_EMAIL_ALREADY_SENT"] + @"</td>
        </tr>";
                                    });
                                    builder.HtmlBody += "</table>";
                                }

                                var for_all = vaccin_alertss.AsEnumerable().Where(K => (int)K["IS_FOR_ALL"] == 1);
                                if (for_all.Any())
                                {
                                    builder.HtmlBody += @"<p style=""color: rgb(0, 92, 145); text-decoration: underline;"">&#8226; Vaccinations Globales :</p>
    <table>
        <tr
            style=""background-color: rgb(78, 83, 160); color: rgb(255, 255, 255); font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif"">
            <th>Date</th>
            <th>Vaccination</th>
            <th>Animal</th>
        </tr>";

                                    List<int> alredy_wrote = new List<int>();
                                    for_all.ForEach(X =>
                                    {
                                        if (!alredy_wrote.Contains((int)X["VACCIN_ID"]))
                                        {
                                            alredy_wrote.Add((int)X["VACCIN_ID"]);

                                            builder.HtmlBody += @"<tr>
            <td>" + ((DateTime)X["DATE"]).ToString("dd/MM/yyyy") + @"</td>
            <td>" + X["VACCIN_NME"] + @"</td>
            <td>" + X["ANIM_NME"] + @"</td>
        </tr>";
                                        }
                                    });
                                    builder.HtmlBody += "</table>";
                                }

                                builder.HtmlBody += @"<p>Nous encourageons également une communication proactive avec les propriétaires d'animaux pour confirmer leurs rendez-vous et leur fournir toute information supplémentaire dont ils pourraient avoir besoin.<br>
        N'hésitez pas à contacter notre équipe de gestion si vous avez des questions ou des préoccupations concernant les rendez-vous à venir. Votre diligence et votre engagement envers le bien-être des animaux de compagnie sont grandement appréciés.<br>
        Avec nos remerciements anticipés pour votre attention et votre soin continus.</p>
    
<p style=""color: rgb(146, 100, 0);"">--------------------------------------------<br>
                            <span style=""font-weight: bold; color: rgb(95, 182, 95);"">" + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + @"</span><br>";

                                if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                                {
                                    builder.HtmlBody += @"Tél: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                                }
                                builder.HtmlBody += @"Email: " + cabinet_email + "<br>";
                                if (Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString().Length > 0)
                                {
                                    builder.HtmlBody += @"Adresse: " + Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() + "<br>";
                                }
                                builder.HtmlBody += "--------------------------------------------</p>" +
                                "<p style=\"color: rgb(184, 223, 235);\">RancoSoft&copy;</p>" +
                                "</body>";
                                //---------------------------------------
                                MimeMessage Mssg = new MimeMessage();
                                Mssg.From.Add(new MailboxAddress("RancoSoft", "rancosoft@gmail.com"));
                                Mssg.To.Add(MailboxAddress.Parse(cabinet_email));
                                Mssg.Subject = "ALBAITAR Softvet - Rappel de rendez-vous pour les prochaines vaccinations";
                                Mssg.Body = builder.ToMessageBody();
                                if (PreConnection.IsInternetAvailable()) //If the internet available
                                {
                                    SmtpClient clnt = new SmtpClient();
                                    try
                                    {
                                        clnt.Connect("smtp.gmail.com", 465, true);
                                        clnt.Authenticate("rancosoft@gmail.com", PreConnection.Traduct_Codified_txt(Properties.Settings.Default.RANCOSOFT_GMAIL_AUTHENT));
                                        clnt.Send(Mssg);
                                        //------------------                                    
                                    }
                                    catch
                                    {
                                        return;
                                    }
                                    finally
                                    {
                                        clnt.Disconnect(true);
                                        clnt.Dispose();
                                    }
                                    //-----------
                                    string vaccin_idsz = "";
                                    vaccin_alertss.AsEnumerable().Select(E => E["VACCIN_ID"]).Distinct().ForEach(Z => vaccin_idsz += "," + Z);
                                    vaccin_idsz = vaccin_idsz.Length > 0 ? vaccin_idsz.Substring(1) : vaccin_idsz;
                                    if (vaccin_idsz.Length > 0)
                                    {
                                        PreConnection.Excut_Cmd_personnel("UPDATE tb_vaccin SET LAST_ALERT_EMAIL_CABINET_SENT_DATE = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE ID IN (" + vaccin_ids + ");", null, null);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //---------------------

        }
        public void Activ_Verif()
        {
            if (Properties.Settings.Default.Connection_String_IP_Or_LocalHost == "localhost")
            {
                bool server_activated = PreConnection.Verif_real_server_activ();
                if (!server_activated)
                {
                    if (PreConnection.ReadFromRegistry("Déja_try_version") != "OUI")
                    {
                        string filePath = "C:\\ProgramData\\Al_Baitar_Activation.txt";

                        if (File.Exists(filePath))
                        {
                            string fileContents = File.ReadAllText(filePath);
                            double old_val = 0;
                            double.TryParse(fileContents, out old_val);
                            if (old_val >= 10800) //30Jrs x 6Hr x 60Min (Pour éviter le jouer par date)
                            {
                                PreConnection.WriteIntoRegistry("SoftVet_Start_Date", "01/01/1900");
                            }
                        }
                        else
                        {
                            DateTime dtt = DateTime.UtcNow;
                            bool rr = DateTime.TryParse(PreConnection.ReadFromRegistry("SoftVet_Start_Date"), out dtt);

                            if (!rr || dtt == DateTime.Now)
                            {
                                Application.Run(new App_Activation());
                                return;
                            }
                        }
                        //----------------
                        string codd = PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Activate_Code);
                        if (!PreConnection.Verif_Activation_SOftVet(codd))
                        {
                            string strt_date = PreConnection.ReadFromRegistry("SoftVet_Start_Date");
                            DateTime dt = DateTime.UtcNow;
                            if (strt_date == "")
                            {
                                PreConnection.WriteIntoRegistry("SoftVet_Start_Date", dt.ToString("dd/MM/yyyy"));
                            }

                            DateTime.TryParse(strt_date, out dt);
                            int delay = 30 - (DateTime.UtcNow.Date - dt.Date).Days;

                            text_to_add_to_title = " (Produit non activé - réste [" + delay + "] jours)";
                            if (delay >= 30)
                            {
                                new App_Activation().ShowDialog();
                            }
                            else if (delay <= 0)
                            {
                                PreConnection.WriteIntoRegistry("Déja_try_version", "OUI");
                                Properties.Settings.Default.Codified_Activate_Code = "";
                                Properties.Settings.Default.Save();
                                close_app_because_act = true;
                                Application.Run(new App_Activation());
                            }



                        }
                    }
                    else
                    {
                        Properties.Settings.Default.Codified_Activate_Code = "";
                        Properties.Settings.Default.Save();
                        close_app_because_act = true;
                        Application.Run(new App_Activation());
                    }

                }


            }
            else
            {
                bool activated = false;
                DataTable acct = PreConnection.Load_data("SELECT * FROM tb_params WHERE `ID` = 7;");
                if (acct != null)
                {
                    int dd = int.Parse(acct.Rows[0][2].ToString());
                    activated = dd == 1;
                }
                if (!activated)
                {
                    MessageBox.Show("malheureusement le produit de PC serveur n'est pas activé\navec une tentative expirée!", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    if (MessageBox.Show("Voulez vous de changer la connection à un autre serveur ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        new Connection_Str().ShowDialog();
                        Activ_Verif();
                    }
                    else
                    {
                        Application.Exit();
                    }

                }
            }

        }

        public void Load_sites_table()
        {
            ADRESSES_SITES = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_adresses;");
            sites_table_ready = true;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!sites_table_ready)
            {
                th.Join();
            }
            if (Application.OpenForms["Clients"] == null)
            {
                new Clients(-1, 1, -1).Show();
            }
            else
            {
                Application.OpenForms["Clients"].WindowState = Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Clients"].WindowState;
                Application.OpenForms["Clients"].BringToFront();
            }
            Cursor = Cursors.Default;
            panel1.Visible = false;



        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new Settings()).ShowDialog();
            if (tabControl1.SelectedTab.Name == "tabPage_infos")
            {
                Refresh_current_tab();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms["Animaux"] == null)
            {
                new Animaux(-1, -1).Show();
                // new Animaux_Copy(-1, -1).Show();
            }
            else
            {
                Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                Application.OpenForms["Animaux"].BringToFront();
            }
            panel1.Visible = false;


        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms["Produits"] == null)
            {
                new Produits().Show();
            }
            else
            {
                Application.OpenForms["Produits"].WindowState = Application.OpenForms["Produits"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Produits"].WindowState;
                Application.OpenForms["Produits"].BringToFront();
            }
            panel1.Visible = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Agenda"] == null)
            {
                new Agenda(null, null).Show();
            }
            else
            {
                Application.OpenForms["Agenda"].WindowState = Application.OpenForms["Agenda"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Agenda"].WindowState;
                Application.OpenForms["Agenda"].BringToFront();
            }
            panel1.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Login_Auto_Enter = false;
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        private void Main_Frm_Load(object sender, EventArgs e)
        {
            if (close_app_because_act)
            {
                Close();
            }
            else
            {
                WindowState = Properties.Settings.Default.Maximize_Main_Frm ? FormWindowState.Maximized : FormWindowState.Normal;
                Text = "ALBAITAR Softvet - " + Properties.Settings.Default.Last_login_user_full_nme;
                string cab_doct = Params != null ? Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Any() ? Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() : "" : "";
                if (cab_doct == null || cab_doct.Trim() == string.Empty)
                {
                    if (Properties.Settings.Default.Last_login_is_admin || Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "92004" && (Int32)QQ[3] == 1).Count() > 0)
                    {
                        string dd = "";
                        int p = 3;
                        while (dd == string.Empty && p > 0)
                        {
                            PreConnection.InputBox("Veuillez saisir votre identification : ", "Exp : Dr.xxx , Cabinet xxx, ...", ref dd);
                            p--;
                        }
                        dd = string.IsNullOrEmpty(dd) ? "Cabinet" : dd;
                        if (dd != string.Empty)
                        {
                            PreConnection.Excut_Cmd(2, "tb_params", new List<string> { "VAL" }, new List<object> { dd }, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { 1 });
                            Params = PreConnection.Load_data("SELECT * FROM tb_params;");
                            label_cab_nme.Text = dd;
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    label_cab_nme.Text = cab_doct;
                }
                ///--------------------            
                if (!Properties.Settings.Default.Last_login_is_admin)
                {
                    button9.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10000" && (Int32)QQ[3] == 1).Count() > 0;
                    button11.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20000" && (Int32)QQ[3] == 1).Count() > 0;
                    button12.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30000" && (Int32)QQ[3] == 1).Count() > 0;
                    button4.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "31000" && (Int32)QQ[3] == 1).Count() > 0;
                    button5.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "40000" && (Int32)QQ[3] == 1).Count() > 0;
                    button6.Enabled = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "70000" && (Int32)QQ[3] == 1).Count() > 0;
                    //---------------tabPage_animaux
                    if (Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20000" && (Int32)QQ[3] == 1).Count() == 0)//Consulter nn autoriz
                    {
                        tabPage_animaux.Controls.Add(new Nn_Autorized());
                        tabPage_animaux.Controls["Nn_Autorized"].Dock = DockStyle.Fill;
                        tabPage_animaux.Controls["Nn_Autorized"].BringToFront();
                    }
                    else
                    {
                        button8.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20001" && (Int32)QQ[3] == 1).Count() > 0; //Nouveau
                        button10.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20003" && (Int32)QQ[3] == 1).Count() > 0; //Modifier
                    }
                    //---------------tabPage_visites
                    if (Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50000" && (Int32)QQ[3] == 1).Count() == 0)//Consulter nn autoriz
                    {
                        tabPage_visites.Controls.Add(new Nn_Autorized());
                        tabPage_visites.Controls["Nn_Autorized"].Dock = DockStyle.Fill;
                        tabPage_visites.Controls["Nn_Autorized"].BringToFront();
                    }
                    else
                    {
                        button17.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50001" && (Int32)QQ[3] == 1).Count() > 0; //Nouveau
                        button18.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50003" && (Int32)QQ[3] == 1).Count() > 0; //Modifier
                    }
                    //---------------tabPage_labo
                    if (Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30000" && (Int32)QQ[3] == 1).Count() == 0)//Consulter nn autoriz
                    {
                        tabPage_labo.Controls.Add(new Nn_Autorized());
                        tabPage_labo.Controls["Nn_Autorized"].Dock = DockStyle.Fill;
                        tabPage_labo.Controls["Nn_Autorized"].BringToFront();
                    }
                    else
                    {
                        button15.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30001" && (Int32)QQ[3] == 1).Count() > 0; //Nouveau
                        button14.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30003" && (Int32)QQ[3] == 1).Count() > 0; //Modifier
                        button13.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "30004" && (Int32)QQ[3] == 1).Count() > 0; //Imprimer
                    }
                    //---------------tabPage_monetique
                    bool alll = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60000" && (Int32)QQ[3] == 1).Count() == 0 && Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "70000" && (Int32)QQ[3] == 1).Count() == 0;
                    if (alll) //Monetic + Factures
                    {
                        tabPage_monetique.Controls.Add(new Nn_Autorized());
                        tabPage_monetique.Controls["Nn_Autorized"].Dock = DockStyle.Fill;
                        tabPage_monetique.Controls["Nn_Autorized"].BringToFront();
                    }
                    else
                    {
                        //---->> Monetic
                        label4.Visible = textBox4.Visible = dataGridView4.Visible = dataGridView6.Visible = label9.Visible = button27.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60000" && (Int32)QQ[3] == 1).Count() > 0;
                        button16.Visible = dataGridView4.Visible && Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60001" && (Int32)QQ[3] == 1).Count() > 0; //Nouveau
                        button19.Visible = dataGridView4.Visible && Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "60003" && (Int32)QQ[3] == 1).Count() > 0; //Modifier                    
                                                                                                                                                                                       //---->> Factures
                        label5.Visible = textBox5.Visible = dataGridView5.Visible = dataGridView7.Visible = panel3.Visible = panel4.Visible = label7.Visible = label8.Visible = label6.Visible = button26.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "70000" && (Int32)QQ[3] == 1).Count() > 0;
                        button20.Visible = dataGridView5.Visible && Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "70001" && (Int32)QQ[3] == 1).Count() > 0; //Nouveau
                        button21.Visible = dataGridView5.Visible && Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "70003" && (Int32)QQ[3] == 1).Count() > 0; //Modifier                    
                    }

                }

                //--------------------
                comboBox3.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
                comboBox3.SelectedIndex = 0;
                comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
                //----------------
                Main_Frm_clients_tbl = PreConnection.Load_data("SELECT *,CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS FULL_NME FROM tb_clients;");
                Main_Frm_animals_tbl = PreConnection.Load_data("SELECT  " +
        "    tb1.*,  " +
        "    tb2.CLIENT_FULL_NME,  " +
        "    tb4.MALAD_NME,  " +
        "    tb4.LAST_MALAD_DATE  " +
        "FROM  " +
        "    tb_animaux tb1  " +
        "LEFT JOIN  " +
        "    (SELECT  " +
        "        ID,  " +
        "        CONCAT(FAMNME,' ',NME) AS CLIENT_FULL_NME  " +
        "     FROM  " +
        "        tb_clients) tb2  " +
        "ON  " +
        "    tb1.CLIENT_ID = tb2.ID  " +
        "LEFT JOIN  " +
        "    (SELECT  " +
        "        tb_maladies.ANIM_ID,  " +
        "        tb_maladies.START_DATE AS LAST_MALAD_DATE,  " +
        "        tb_maladies.MALAD_NME  " +
        "     FROM  " +
        "        tb_maladies  " +
        "     JOIN  " +
        "        (SELECT  " +
        "            ANIM_ID,  " +
        "            MAX(START_DATE) AS max_start_date  " +
        "         FROM  " +
        "            tb_maladies  " +
        "         WHERE  " +
        "            (START_DATE <= current_timestamp() OR START_DATE IS NULL)  " +
        "            AND (ESTIM_END_DATE >= current_timestamp() OR ESTIM_END_DATE IS NULL)  " +
        "         GROUP BY  " +
        "            ANIM_ID) tb3  " +
        "     ON  " +
        "        tb_maladies.ANIM_ID = tb3.ANIM_ID  " +
        "        AND tb_maladies.START_DATE = tb3.max_start_date) tb4  " +
        "ON  " +
        "    tb4.ANIM_ID = tb1.ID; ");
                main_poids_tab = PreConnection.Load_data("SELECT * FROM tb_poids;");
                comboBox1.SelectedIndex = 1;
                //-----
                Send_Vaccin_Alerts = new Thread(new ThreadStart(Send_Email_Vaccin_Alerts));
                Send_Vaccin_Alerts.Start();
                // Send_Vaccin_Alerts.Join();
                //--------------
                Application.OpenForms["Splash"]?.Close();
            }

        }

        public void refresh_main_tables()
        {
            last_update_time = DateTime.Now;
            //------------
            int cb1_idx = comboBox1.SelectedIndex > -1 ? comboBox1.SelectedIndex : 0;
            Main_Frm_clients_tbl = PreConnection.Load_data("SELECT *,CONCAT(`SEX`,' ',`FAMNME`,' ',`NME`) AS FULL_NME FROM tb_clients;");
            //Main_Frm_animals_tbl = PreConnection.Load_data("SELECT tb1.*,tb2.`CLIENT_FULL_NME`,tb3.MALAD_NME,tb3.LAST_MALAD_DATE FROM tb_animaux tb1 "
            //                                             + "LEFT JOIN (SELECT `ID`,CONCAT(`FAMNME`,' ',`NME`) AS CLIENT_FULL_NME FROM tb_clients) tb2 ON tb1.`CLIENT_ID` = tb2.`ID` "
            //                                             + "LEFT JOIN (SELECT ANIM_ID, MAX(START_DATE) AS LAST_MALAD_DATE,MALAD_NME "
            //                                             + "FROM tb_maladies WHERE (START_DATE <= current_timestamp() OR START_DATE IS NULL) AND (ESTIM_END_DATE >= current_timestamp() OR ESTIM_END_DATE IS NULL) "
            //                                             + "GROUP BY ANIM_ID) tb3 ON tb3.ANIM_ID = tb1.ID;");
            Main_Frm_animals_tbl = PreConnection.Load_data("SELECT  " +
    "    tb1.*,  " +
    "    tb2.CLIENT_FULL_NME,  " +
    "    tb4.MALAD_NME,  " +
    "    tb4.LAST_MALAD_DATE  " +
    "FROM  " +
    "    tb_animaux tb1  " +
    "LEFT JOIN  " +
    "    (SELECT  " +
    "        ID,  " +
    "        CONCAT(FAMNME,' ',NME) AS CLIENT_FULL_NME  " +
    "     FROM  " +
    "        tb_clients) tb2  " +
    "ON  " +
    "    tb1.CLIENT_ID = tb2.ID  " +
    "LEFT JOIN  " +
    "    (SELECT  " +
    "        tb_maladies.ANIM_ID,  " +
    "        tb_maladies.START_DATE AS LAST_MALAD_DATE,  " +
    "        tb_maladies.MALAD_NME  " +
    "     FROM  " +
    "        tb_maladies  " +
    "     JOIN  " +
    "        (SELECT  " +
    "            ANIM_ID,  " +
    "            MAX(START_DATE) AS max_start_date  " +
    "         FROM  " +
    "            tb_maladies  " +
    "         WHERE  " +
    "            (START_DATE <= current_timestamp() OR START_DATE IS NULL)  " +
    "            AND (ESTIM_END_DATE >= current_timestamp() OR ESTIM_END_DATE IS NULL)  " +
    "         GROUP BY  " +
    "            ANIM_ID) tb3  " +
    "     ON  " +
    "        tb_maladies.ANIM_ID = tb3.ANIM_ID  " +
    "        AND tb_maladies.START_DATE = tb3.max_start_date) tb4  " +
    "ON  " +
    "    tb4.ANIM_ID = tb1.ID; ");
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox1.SelectedIndex = cb1_idx;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1_SelectedIndexChanged(null, null);
            Refresh_current_tab();
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            System.Drawing.Rectangle panelBounds = panel1.RectangleToScreen(panel1.ClientRectangle);
            if (!panelBounds.Contains(MousePosition))
            {
                panel1.Visible = false;
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            System.Drawing.Rectangle panelBounds = panel1.RectangleToScreen(panel1.ClientRectangle);
            if (!panelBounds.Contains(MousePosition))
            {
                panel1.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Laboratoire"] == null)
            {
                new Laboratoire(-1, "", false, "").Show();
            }
            else
            {
                Application.OpenForms["Laboratoire"].WindowState = Application.OpenForms["Laboratoire"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Laboratoire"].WindowState;
                Application.OpenForms["Laboratoire"].BringToFront();
            }
            panel1.Visible = false;
        }

        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                button9.Focus();
            }
            else
            {
                button3.Select();
            }
        }

        private void Main_Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["Splash"]?.Close();
            //-----------
            string filePath = "C:\\ProgramData\\Al_Baitar_Activation.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }

            string fileContents = File.ReadAllText(filePath);
            decimal old_val = 0;
            decimal.TryParse(fileContents, out old_val);

            if (old_val >= 10800) //30Jrs x 6Hr x 60Min (Pour éviter le jouer par date)
            {
                PreConnection.WriteIntoRegistry("SoftVet_Start_Date", "01/01/1900");
                //PreConnection.Excut_Cmd("UPDATE tb_params SET `VAL` = 0 WHERE `ID` = 7;");
                PreConnection.Excut_Cmd(2, "tb_params", new List<string> { "VAL" }, new List<object> { 0 }, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { 7 });
            }

            File.WriteAllText(filePath, ((double)old_val + (double)time_delay / 60).ToString("N2"));
            //----------------------------------
            Properties.Settings.Default.Maximize_Main_Frm = WindowState == FormWindowState.Maximized;
            Properties.Settings.Default.Save();
            //------------------------------------
            string backup_fold = Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 8).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();
            string backup_freq = Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 9).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString();
            if (!string.IsNullOrEmpty(backup_fold) && !string.IsNullOrEmpty(backup_freq))
            {
                if (Properties.Settings.Default.Tmp_Dont_auto_save)
                {
                    Properties.Settings.Default.Tmp_Dont_auto_save = false;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    if (backup_freq.EndsWith("Quit"))
                    {
                        string file_nme = string.Concat("al_baitar_backup_", DateTime.Now.ToString("dd_MM_yyyy_HH'h'_mm'm'_ss's'_"), DateTime.Now.Millisecond, ".sql");
                        PreConnection.DB_Backup(backup_fold + @"\" + file_nme);
                    }
                }
            }
            //------------------------
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Vente"] == null)
            {
                new Vente(-1).Show();
            }
            else
            {
                Application.OpenForms["Vente"].WindowState = Application.OpenForms["Vente"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Vente"].WindowState;
                Application.OpenForms["Vente"].BringToFront();
            }
            panel1.Visible = false;
        }

        static void visites_tab()
        {
            main_visites_tab = PreConnection.Load_data("SELECT tb1.*,tb3.`NME` AS ANIM_NME,tb3.`CLIENT_ID`,tb3.CLIENT_FULL_NME,tb2.REF AS 'FACTURE_REF' FROM tb_visites tb1 LEFT JOIN ("
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_01` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NOT NULL AND `ITEM_NME_01` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_02` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NOT NULL AND `ITEM_NME_02` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_03` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NOT NULL AND `ITEM_NME_03` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_04` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NOT NULL AND `ITEM_NME_04` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_05` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NOT NULL AND `ITEM_NME_05` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_06` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NOT NULL AND `ITEM_NME_06` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_07` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NOT NULL AND `ITEM_NME_07` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_08` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NOT NULL AND `ITEM_NME_08` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_09` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NOT NULL AND `ITEM_NME_09` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_10` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NOT NULL AND `ITEM_NME_10` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_11` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NOT NULL AND `ITEM_NME_11` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_12` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NOT NULL AND `ITEM_NME_12` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_13` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NOT NULL AND `ITEM_NME_13` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_14` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NOT NULL AND `ITEM_NME_14` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_15` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NOT NULL AND `ITEM_NME_15` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_16` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NOT NULL AND `ITEM_NME_16` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_17` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NOT NULL AND `ITEM_NME_17` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_18` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NOT NULL AND `ITEM_NME_18` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_19` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NOT NULL AND `ITEM_NME_19` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_20` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NOT NULL AND `ITEM_NME_20` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_21` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NOT NULL AND `ITEM_NME_21` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_22` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NOT NULL AND `ITEM_NME_22` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_23` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NOT NULL AND `ITEM_NME_23` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_24` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NOT NULL AND `ITEM_NME_24` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_25` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NOT NULL AND `ITEM_NME_25` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_26` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NOT NULL AND `ITEM_NME_26` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_27` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NOT NULL AND `ITEM_NME_27` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_28` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NOT NULL AND `ITEM_NME_28` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_29` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NOT NULL AND `ITEM_NME_29` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_30` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NOT NULL AND `ITEM_NME_30` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_31` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NOT NULL AND `ITEM_NME_31` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_32` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NOT NULL AND `ITEM_NME_32` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_33` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NOT NULL AND `ITEM_NME_33` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_34` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NOT NULL AND `ITEM_NME_34` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_35` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NOT NULL AND `ITEM_NME_35` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_36` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NOT NULL AND `ITEM_NME_36` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_37` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NOT NULL AND `ITEM_NME_37` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_38` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NOT NULL AND `ITEM_NME_38` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_39` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NOT NULL AND `ITEM_NME_39` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_40` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NOT NULL AND `ITEM_NME_40` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_41` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NOT NULL AND `ITEM_NME_41` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_42` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NOT NULL AND `ITEM_NME_42` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_43` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NOT NULL AND `ITEM_NME_43` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_44` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NOT NULL AND `ITEM_NME_44` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_45` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NOT NULL AND `ITEM_NME_45` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_46` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NOT NULL AND `ITEM_NME_46` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_47` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NOT NULL AND `ITEM_NME_47` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_48` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NOT NULL AND `ITEM_NME_48` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_49` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NOT NULL AND `ITEM_NME_49` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_50` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NOT NULL AND `ITEM_NME_50` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_51` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NOT NULL AND `ITEM_NME_51` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_52` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NOT NULL AND `ITEM_NME_52` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_53` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NOT NULL AND `ITEM_NME_53` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_54` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NOT NULL AND `ITEM_NME_54` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_55` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NOT NULL AND `ITEM_NME_55` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_56` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NOT NULL AND `ITEM_NME_56` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_57` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NOT NULL AND `ITEM_NME_57` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_58` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NOT NULL AND `ITEM_NME_58` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_59` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NOT NULL AND `ITEM_NME_59` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_60` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NOT NULL AND `ITEM_NME_60` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_61` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NOT NULL AND `ITEM_NME_61` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_62` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NOT NULL AND `ITEM_NME_62` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_63` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NOT NULL AND `ITEM_NME_63` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_64` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NOT NULL AND `ITEM_NME_64` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_65` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NOT NULL AND `ITEM_NME_65` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_66` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NOT NULL AND `ITEM_NME_66` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_67` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NOT NULL AND `ITEM_NME_67` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_68` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NOT NULL AND `ITEM_NME_68` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_69` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NOT NULL AND `ITEM_NME_69` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_70` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NOT NULL AND `ITEM_NME_70` IS NOT NULL "
                                    + ") tb2 ON tb1.`ID` = tb2.`VISIT` LEFT JOIN (SELECT tbb1.`ID`,tbb1.`NME`,tbb2.`ID` AS CLIENT_ID,CONCAT(tbb2.`FAMNME`,' ',tbb2.`NME`) AS CLIENT_FULL_NME FROM tb_animaux tbb1 LEFT JOIN tb_clients tbb2 ON tbb1.`CLIENT_ID` = tbb2.`ID`) tb3 ON tb1.`ANIM_ID` = tb3.`ID` ORDER BY DATETIME;");
            ended_loading_visites_tab = true;
        }

        private void Main_Frm_Activated(object sender, EventArgs e)
        {
            Params = PreConnection.Load_data("SELECT * FROM tb_params;");
            //-----------
            try
            {
                DateTime tt = DateTime.Now;
                if (Params != null)
                {
                    label_cab_nme.Text = Params.Rows.Cast<DataRow>().Where(RR => (int)RR["ID"] == 1).FirstOrDefault()["VAL"].ToString();
                    //----------
                    DateTime.TryParse(Params.Rows.Cast<DataRow>().Where(RR => (int)RR["ID"] == 6).FirstOrDefault()["VAL"].ToString(), out tt);
                }
                if ((tt - last_update_time).Seconds > 0)
                {
                    refresh_main_tables();
                }
                else
                {
                    Refresh_current_tab();
                }
                //-----------
            }
            catch
            {
            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) //CLIENT
            {
                bool rb8 = radioButton8.Checked;
                //---------------
                if (Main_Frm_clients_tbl != null)
                {
                    comboBox2.DataSource = Main_Frm_clients_tbl;
                    comboBox2.ValueMember = "ID";
                    comboBox2.DisplayMember = "FULL_NME";
                }
                //--------------------
                //--------
                if (rb8)
                {
                    if (!radioButton8.Checked) { radioButton8.CheckedChanged -= radioButton8_CheckedChanged; radioButton8.Checked = true; radioButton8.CheckedChanged += radioButton8_CheckedChanged; }
                }
                else
                {
                    if (!radioButton7.Checked) { radioButton7.CheckedChanged -= radioButton8_CheckedChanged; radioButton7.Checked = true; radioButton7.CheckedChanged += radioButton8_CheckedChanged; }
                    //----------
                    selected_client_id = selected_animal_id = -1;
                    if (comboBox2.SelectedValue != null)
                    {
                        if (int.TryParse(comboBox2.SelectedValue.ToString(), out int yy))
                        {
                            if (comboBox1.SelectedIndex == 0) //CLIENT
                            {
                                selected_client_id = (int)comboBox2.SelectedValue;
                            }
                            else //ANIMAL
                            {
                                selected_animal_id = (int)comboBox2.SelectedValue;
                            }

                        }
                    }

                }
                //-----------
                if (radioButton8.Checked && tabControl1.TabPages["tabPage_animaux"] == null)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.IndexOf(tabPage_visites), tabPage_animaux);
                }
                //-------
                if (tabControl1.TabPages["tabPage_monetique"] == null)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.IndexOf(tabPage_Calendar), tabPage_monetique);
                }

            }
            else //ANIMAL
            {
                if (Main_Frm_animals_tbl != null && Main_Frm_animals_tbl.Columns.Count > 0)
                {
                    comboBox2.DataSource = Main_Frm_animals_tbl;
                    comboBox2.ValueMember = "ID";
                    comboBox2.DisplayMember = "NME";

                }
                if (tabControl1.TabPages["tabPage_animaux"] != null)
                {
                    tabControl1.TabPages.Remove(tabPage_animaux);
                }
                if (tabControl1.TabPages["tabPage_monetique"] != null)
                {
                    tabControl1.TabPages.Remove(tabPage_monetique);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected_animal_id = selected_client_id = -1;
            if (comboBox2.SelectedValue != null)
            {
                radioButton8.CheckedChanged -= radioButton8_CheckedChanged;
                radioButton8.Checked = true;
                radioButton8.CheckedChanged += radioButton8_CheckedChanged;
                if (tabControl1.TabPages["tabPage_infos"] == null)
                {
                    tabControl1.TabPages.Insert(0, tabPage_infos);
                }
                //-----------
                if (comboBox2.SelectedValue != null)
                {
                    int yy = -1;
                    if (int.TryParse(comboBox2.SelectedValue.ToString(), out yy))
                    {
                        if (comboBox1.SelectedIndex == 0) //CLIENT
                        {
                            selected_client_id = yy;
                        }
                        else //ANIMAL
                        {
                            selected_animal_id = yy;
                        }

                    }
                }

            }
            if (tabControl1.SelectedTab.Name == "tabPage_Calendar")
            {
                Agenda_Just_Display.make_filter_refresh = true;
            }
            Vaccination.selected_anim_id = selected_animal_id;
            Vaccination.selected_client_id = selected_client_id;
            Vaccination.make_refresh = true;
            Refresh_current_tab();
        }


        private void button7_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                chosen_client_from_search = new DataTable();
                Clients_List_Search select = new Clients_List_Search();
                select.DataTableReturned += ChildForm_DataTableReturned2;
                select.ShowDialog();
                if (chosen_client_from_search != null)
                {
                    radioButton8.CheckedChanged -= radioButton8_CheckedChanged;
                    radioButton8.Checked = true;
                    radioButton8.CheckedChanged += radioButton8_CheckedChanged;
                    comboBox2.SelectedValue = chosen_client_from_search.Rows[0][0];
                }
            }
            else
            {
                chosen_anim_from_search = new DataTable();
                Anims_List_Search select = new Anims_List_Search();
                select.DataTableReturned += ChildForm_DataTableReturned;
                select.ShowDialog();
                if (chosen_anim_from_search != null)
                {
                    radioButton8.CheckedChanged -= radioButton8_CheckedChanged;
                    radioButton8.Checked = true;
                    radioButton8.CheckedChanged += radioButton8_CheckedChanged;
                    comboBox2.SelectedValue = chosen_anim_from_search.Rows[0][1];
                }
            }
            Refresh_current_tab();
        }

        private void ChildForm_DataTableReturned2(object sender, DataTableEventArgs_Clients e)
        {
            chosen_client_from_search = e.DataTable;
        }

        private void ChildForm_DataTableReturned(object sender, DataTableEventArgs e)
        {
            chosen_anim_from_search = e.DataTable;
        }

        private void Refresh_current_tab()
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tabPage_visites":
                    if (!loading_visites_tab)
                    {
                        visites_tab();
                        loading_visites_tab = true;
                        //----------------------
                        while (loading_visites_tab)
                        {
                            if (ended_loading_visites_tab)
                            {
                                loading_visites_tab = false;
                                dataGridView2.DataSource = main_visites_tab;
                                dataGridView2.Refresh();
                                int fct = main_visites_tab.AsEnumerable().Count(t => t["FACTURE_REF"] != DBNull.Value && ((string)t["FACTURE_REF"]).Trim().Length > 0);
                                radioButton1.Text = "Tous (" + main_visites_tab.Rows.Count + ")";
                                radioButton2.Text = "Facturé (" + fct + ")";
                                radioButton3.Text = "Non Facturé (" + (main_visites_tab.Rows.Count - fct) + ")";
                                DGV_Visit_Filter(true);
                            }
                        }
                    }
                    break;
                case "tabPage_infos":
                    button3.Focus();
                    main_poids_tab = PreConnection.Load_data("SELECT * FROM tb_poids;");
                    if (comboBox1.SelectedIndex == 0) //Clients
                    {
                        if (tabPage_infos.Controls["Client_Infos"] == null)
                        {
                            int zz = -1;
                            bool ff = int.TryParse((comboBox2.SelectedValue != null ? (comboBox2.SelectedValue != DBNull.Value ? comboBox2.SelectedValue : -1) : -1).ToString(), out zz);
                            if (ff)
                            {
                                tabPage_infos.Controls.Add(new Client_Infos(zz));
                                tabPage_infos.Controls["Client_Infos"].Dock = DockStyle.Fill;
                                tabPage_infos.Controls["Client_Infos"].BringToFront();
                            }
                        }
                        else
                        {
                            Client_Infos.selected_client_id = comboBox2.SelectedValue != null ? (comboBox2.SelectedValue != DBNull.Value ? (int)comboBox2.SelectedValue : -1) : -1;
                            Client_Infos.make_refresh = true;
                            tabPage_infos.Controls["Client_Infos"].Focus();
                            tabPage_infos.Controls["Client_Infos"].BringToFront();
                        }
                    }
                    else //Animaux
                    {
                        if (tabPage_infos.Controls["Anim_Infos"] == null)
                        {
                            int zz = -1;
                            bool ff = int.TryParse((comboBox2.SelectedValue != null ? (comboBox2.SelectedValue != DBNull.Value ? comboBox2.SelectedValue : -1) : -1).ToString(), out zz);
                            if (ff)
                            {
                                tabPage_infos.Controls.Add(new Anim_Infos(zz));
                                tabPage_infos.Controls["Anim_Infos"].Dock = DockStyle.Fill;
                                tabPage_infos.Controls["Anim_Infos"].BringToFront();
                            }
                        }
                        else
                        {
                            Anim_Infos.selected_anim_id = comboBox2.SelectedValue != null ? (comboBox2.SelectedValue != DBNull.Value ? (int.TryParse(comboBox2.SelectedValue.ToString(), out _) ? (int)comboBox2.SelectedValue : -1) : -1) : -1;
                            Anim_Infos.make_refresh = true;
                            tabPage_infos.Controls["Anim_Infos"].Focus();
                            tabPage_infos.Controls["Anim_Infos"].BringToFront();
                        }
                    }
                    break;
                case "tabPage_labo":
                    if (!loading_lab_tab)
                    {
                        animal_lab_tab();
                        loading_lab_tab = true;
                        //----------------------
                        while (loading_lab_tab)
                        {
                            if (ended_loading_lab_tab)
                            {
                                loading_lab_tab = false;
                                dataGridView1.DataSource = main_lab_tab;
                                dataGridView1.Refresh();
                            }
                        }
                        //---------
                        DGV_Lab_Filter(true);
                    }
                    break;
                case "tabPage_Calendar":
                    if (tabPage_Calendar.Controls.Count == 0)
                    {
                        int zz = (int)(comboBox2.SelectedValue != null && radioButton8.Checked ? comboBox2.SelectedValue : -1);
                        tabPage_Calendar.Controls.Add(new Agenda_Just_Display(comboBox1.SelectedIndex + 1, zz));
                        tabPage_Calendar.Controls[0].Dock = DockStyle.Fill;
                    }
                    else
                    {
                        Agenda_Just_Display.Selected_idss = (int)(radioButton8.Checked ? comboBox2.SelectedValue : -1);
                        Agenda_Just_Display.for_animal = comboBox1.SelectedIndex == 1;
                        Agenda_Just_Display.make_update = true;
                        tabPage_Calendar.Controls[0].Focus();
                    }
                    break;
                case "tabPage_animaux":
                    DGV_Anim_Filter();
                    break;
                case "tabPage_monetique":
                    if (!loading_finn_tab)
                    {
                        finance_lab_tab();
                        loading_finn_tab = true;
                        //----------------------
                        while (loading_finn_tab)
                        {
                            if (ended_loading_finn_tab)
                            {
                                loading_finn_tab = false;
                                dataGridView4.DataSource = main_financ_tab;
                                dataGridView4.Refresh();
                            }
                        }
                        //---------
                        textBox4_TextChanged(null, null);
                    }
                    if (!loading_fact_tab)
                    {
                        facture_lab_tab();
                        loading_fact_tab = true;
                        //----------------------
                        while (loading_fact_tab)
                        {
                            if (ended_loading_fact_tab)
                            {
                                loading_fact_tab = false;
                                dataGridView5.DataSource = main_factures_tbl;
                                dataGridView5.Refresh();
                            }
                        }
                        //---------
                        textBox5_TextChanged(null, null);
                    }
                    break;
                case "tabPage_vaccin":
                    if (tabPage_vaccin.Controls["Vaccination"] == null)
                    {
                        if (radioButton8.Checked)
                        {
                            if (comboBox2.SelectedValue != null)
                            {
                                if (comboBox1.SelectedIndex == 0)
                                {
                                    Vaccination.selected_client_id = (int)comboBox2.SelectedValue;
                                }
                                else
                                {
                                    Vaccination.selected_anim_id = (int)comboBox2.SelectedValue;
                                }
                            }

                        }
                        tabPage_vaccin.Controls.Add(new Vaccination());
                        tabPage_vaccin.Controls["Vaccination"].Dock = DockStyle.Fill;
                        tabPage_vaccin.Controls["Vaccination"].BringToFront();

                    }
                    else
                    {
                        //Anim_Infos.selected_anim_id = comboBox2.SelectedValue != null ? (comboBox2.SelectedValue != DBNull.Value ? (int)comboBox2.SelectedValue : -1) : -1;
                        //Anim_Infos.make_refresh = true;
                        tabPage_vaccin.Controls["Vaccination"].Focus();
                        tabPage_vaccin.Controls["Vaccination"].BringToFront();
                    }
                    break;
                    //==================================================================
            }
            save_and_restore_select_sit(2);
        }
        private void finance_lab_tab()
        {
            main_financ_tab = PreConnection.Load_data("SELECT tb1.*,tb2.CLIENT_FULL_NME FROM tb_clients_finance tb1 LEFT JOIN (SELECT `ID`,CONCAT(`FAMNME`,' ',`NME`) AS CLIENT_FULL_NME FROM tb_clients) tb2 ON tb1.`CLIENT_ID` = tb2.`ID`;");
            ended_loading_finn_tab = true;


        }
        private void facture_lab_tab()
        {
            main_factures_tbl = PreConnection.Load_data("SELECT tb1.`ID`,tb1.`DATE`,tb1.`CLIENT_ID`,tb1.`CLIENT_FULL_NME`,tb1.`REF`,tb1.`TOTAL_HT`,tb1.`TVA_PERC`,tb1.`DROIT_TIMBRE`,tb1.`TOTAL_TTC`,tb1.`LAST_MODIF_BY`,tb2.`SLD` AS FACT_PAID_MNT,tb3.SLD AS SLD_OF_CLIENT FROM tb_factures_vente tb1 LEFT JOIN (SELECT SUM(`DEBIT` - `CREDIT`) AS SLD, `FACT_NUM` FROM tb_clients_finance WHERE `FACT_NUM` IS NOT NULL GROUP BY `FACT_NUM`) tb2 ON tb1.`REF` = tb2.`FACT_NUM` LEFT JOIN (SELECT `CLIENT_ID`,SUM(`DEBIT` - `CREDIT`) AS SLD FROM tb_clients_finance WHERE `CLIENT_ID` IS NOT NULL GROUP BY `CLIENT_ID`) tb3 ON tb1.`CLIENT_ID` = tb3.`CLIENT_ID`;");
            ended_loading_fact_tab = true;
        }
        private void animal_lab_tab()
        {
            main_lab_tab = PreConnection.Load_data("SELECT tb1.*,tb2.REF AS 'FACTURE_REF' FROM "
                                                          + "(SELECT 'Hemogramme' AS LABO_NME ,Hem_1.`ID`,Hem_1.`REF`,Hem_1.`DATE_TIME`,Hem_1.`OBSERV`,Hem_2.* FROM tb_labo_hemogramme Hem_1 LEFT JOIN (SELECT tbb1.`ID` AS ANIM_ID,tbb1.`NME` AS ANIM_NME,tbb2.`ID` AS CLIENT_ID,CONCAT(tbb2.`FAMNME`,' ',tbb2.`NME`) AS CLIENT_FULL_NME FROM tb_animaux tbb1 LEFT JOIN tb_clients tbb2 ON tbb1.`CLIENT_ID` = tbb2.`ID`) Hem_2 ON Hem_1.`ANIM_ID` = Hem_2.`ANIM_ID` UNION ALL "
                                                          + "SELECT 'Biochimie' AS LABO_NME ,Bio_1.`ID`,Bio_1.`REF`,Bio_1.`DATE_TIME`,Bio_1.`OBSERV`,Bio_2.* FROM tb_labo_biochimie Bio_1 LEFT JOIN (SELECT tbb1.`ID` AS ANIM_ID,tbb1.`NME` AS ANIM_NME,tbb2.`ID` AS CLIENT_ID,CONCAT(tbb2.`FAMNME`,' ',tbb2.`NME`) AS CLIENT_FULL_NME FROM tb_animaux tbb1 LEFT JOIN tb_clients tbb2 ON tbb1.`CLIENT_ID` = tbb2.`ID`) Bio_2 ON Bio_1.`ANIM_ID` = Bio_2.`ANIM_ID` UNION ALL "
                                                          + "SELECT 'Immunologie' AS LABO_NME ,Imm_1.`ID`,Imm_1.`REF`,Imm_1.`DATE_TIME`,Imm_1.`OBSERV`,Imm_2.* FROM tb_labo_immunologie Imm_1 LEFT JOIN (SELECT tbb1.`ID` AS ANIM_ID,tbb1.`NME` AS ANIM_NME,tbb2.`ID` AS CLIENT_ID,CONCAT(tbb2.`FAMNME`,' ',tbb2.`NME`) AS CLIENT_FULL_NME FROM tb_animaux tbb1 LEFT JOIN tb_clients tbb2 ON tbb1.`CLIENT_ID` = tbb2.`ID`) Imm_2 ON Imm_1.`ANIM_ID` = Imm_2.`ANIM_ID` UNION ALL "
                                                          + "SELECT 'Protéinogramme' AS LABO_NME ,Pro_1.`ID`,Pro_1.`REF`,Pro_1.`DATE_TIME`,Pro_1.`OBSERV`,Pro_2.* FROM tb_labo_proteinogramme Pro_1 LEFT JOIN (SELECT tbb1.`ID` AS ANIM_ID,tbb1.`NME` AS ANIM_NME,tbb2.`ID` AS CLIENT_ID,CONCAT(tbb2.`FAMNME`,' ',tbb2.`NME`) AS CLIENT_FULL_NME FROM tb_animaux tbb1 LEFT JOIN tb_clients tbb2 ON tbb1.`CLIENT_ID` = tbb2.`ID`) Pro_2 ON Pro_1.`ANIM_ID` = Pro_2.`ANIM_ID` UNION ALL "
                                                          + "SELECT 'Urologie' AS LABO_NME ,Uro_1.`ID`,Uro_1.`REF`,Uro_1.`DATE_TIME`,Uro_1.`OBSERV`,Uro_2.* FROM tb_labo_urologie Uro_1 LEFT JOIN (SELECT tbb1.`ID` AS ANIM_ID,tbb1.`NME` AS ANIM_NME,tbb2.`ID` AS CLIENT_ID,CONCAT(tbb2.`FAMNME`,' ',tbb2.`NME`) AS CLIENT_FULL_NME FROM tb_animaux tbb1 LEFT JOIN tb_clients tbb2 ON tbb1.`CLIENT_ID` = tbb2.`ID`) Uro_2 ON Uro_1.`ANIM_ID` = Uro_2.`ANIM_ID` UNION ALL "
                                                          + "SELECT TYPE_ANAL AS LABO_NME ,Atr_1.`ID`,Atr_1.`REF`,Atr_1.`DATE_TIME`,Atr_1.`OBSERV`,Atr_2.* FROM tb_labo_autre Atr_1 LEFT JOIN (SELECT tbb1.`ID` AS ANIM_ID,tbb1.`NME` AS ANIM_NME,tbb2.`ID` AS CLIENT_ID,CONCAT(tbb2.`FAMNME`,' ',tbb2.`NME`) AS CLIENT_FULL_NME FROM tb_animaux tbb1 LEFT JOIN tb_clients tbb2 ON tbb1.`CLIENT_ID` = tbb2.`ID`) Atr_2 ON Atr_1.`ANIM_ID` = Atr_2.`ANIM_ID`) tb1 "
                                                          + "LEFT JOIN ("
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_01` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NOT NULL AND `ITEM_NME_01` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_02` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NOT NULL AND `ITEM_NME_02` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_03` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NOT NULL AND `ITEM_NME_03` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_04` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NOT NULL AND `ITEM_NME_04` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_05` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NOT NULL AND `ITEM_NME_05` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_06` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NOT NULL AND `ITEM_NME_06` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_07` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NOT NULL AND `ITEM_NME_07` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_08` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NOT NULL AND `ITEM_NME_08` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_09` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NOT NULL AND `ITEM_NME_09` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_10` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NOT NULL AND `ITEM_NME_10` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_11` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NOT NULL AND `ITEM_NME_11` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_12` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NOT NULL AND `ITEM_NME_12` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_13` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NOT NULL AND `ITEM_NME_13` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_14` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NOT NULL AND `ITEM_NME_14` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_15` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NOT NULL AND `ITEM_NME_15` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_16` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NOT NULL AND `ITEM_NME_16` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_17` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NOT NULL AND `ITEM_NME_17` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_18` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NOT NULL AND `ITEM_NME_18` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_19` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NOT NULL AND `ITEM_NME_19` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_20` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NOT NULL AND `ITEM_NME_20` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_21` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NOT NULL AND `ITEM_NME_21` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_22` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NOT NULL AND `ITEM_NME_22` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_23` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NOT NULL AND `ITEM_NME_23` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_24` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NOT NULL AND `ITEM_NME_24` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_25` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NOT NULL AND `ITEM_NME_25` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_26` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NOT NULL AND `ITEM_NME_26` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_27` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NOT NULL AND `ITEM_NME_27` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_28` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NOT NULL AND `ITEM_NME_28` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_29` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NOT NULL AND `ITEM_NME_29` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_30` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NOT NULL AND `ITEM_NME_30` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_31` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NOT NULL AND `ITEM_NME_31` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_32` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NOT NULL AND `ITEM_NME_32` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_33` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NOT NULL AND `ITEM_NME_33` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_34` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NOT NULL AND `ITEM_NME_34` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_35` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NOT NULL AND `ITEM_NME_35` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_36` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NOT NULL AND `ITEM_NME_36` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_37` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NOT NULL AND `ITEM_NME_37` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_38` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NOT NULL AND `ITEM_NME_38` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_39` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NOT NULL AND `ITEM_NME_39` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_40` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NOT NULL AND `ITEM_NME_40` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_41` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NOT NULL AND `ITEM_NME_41` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_42` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NOT NULL AND `ITEM_NME_42` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_43` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NOT NULL AND `ITEM_NME_43` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_44` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NOT NULL AND `ITEM_NME_44` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_45` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NOT NULL AND `ITEM_NME_45` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_46` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NOT NULL AND `ITEM_NME_46` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_47` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NOT NULL AND `ITEM_NME_47` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_48` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NOT NULL AND `ITEM_NME_48` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_49` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NOT NULL AND `ITEM_NME_49` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_50` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NOT NULL AND `ITEM_NME_50` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_51` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NOT NULL AND `ITEM_NME_51` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_52` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NOT NULL AND `ITEM_NME_52` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_53` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NOT NULL AND `ITEM_NME_53` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_54` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NOT NULL AND `ITEM_NME_54` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_55` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NOT NULL AND `ITEM_NME_55` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_56` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NOT NULL AND `ITEM_NME_56` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_57` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NOT NULL AND `ITEM_NME_57` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_58` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NOT NULL AND `ITEM_NME_58` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_59` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NOT NULL AND `ITEM_NME_59` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_60` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NOT NULL AND `ITEM_NME_60` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_61` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NOT NULL AND `ITEM_NME_61` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_62` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NOT NULL AND `ITEM_NME_62` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_63` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NOT NULL AND `ITEM_NME_63` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_64` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NOT NULL AND `ITEM_NME_64` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_65` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NOT NULL AND `ITEM_NME_65` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_66` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NOT NULL AND `ITEM_NME_66` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_67` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NOT NULL AND `ITEM_NME_67` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_68` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NOT NULL AND `ITEM_NME_68` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_69` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NOT NULL AND `ITEM_NME_69` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_70` AS 'LABO' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NOT NULL AND `ITEM_NME_70` IS NOT NULL "
                                                          + ") tb2 "
                                                          + "ON tb1.`REF` = tb2.`LABO`;");
            ended_loading_lab_tab = true;
        }
        private void DGV_Anim_Filter()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                //-----------------
                DataTable tmp_animm;
                if (radioButton8.Checked)
                {
                    var rrr = Main_Frm_animals_tbl.AsEnumerable().Where(DD => (int)DD["CLIENT_ID"] == selected_client_id);

                    if (rrr.Any())
                    {
                        tmp_animm = (Main_Frm_animals_tbl.AsEnumerable().Where(DD => (int)DD["CLIENT_ID"] == selected_client_id).CopyToDataTable()).DefaultView.ToTable(false, "ID", "NUM_IDENTIF", "CLIENT_FULL_NME", "NME", "ESPECE", "RACE", "SEXE", "NISS_DATE", "DATE_ADDED", "IS_RADIATED", "OBSERVATIONS");
                    }
                    else if (Main_Frm_animals_tbl != null)
                    {
                        string[] list = { "ID", "NUM_IDENTIF", "CLIENT_FULL_NME", "NME", "ESPECE", "RACE", "SEXE", "NISS_DATE", "DATE_ADDED", "IS_RADIATED", "OBSERVATIONS" };

                        tmp_animm = Main_Frm_animals_tbl.Clone();
                        tmp_animm.Columns.Cast<DataColumn>().Where(Z => !list.Contains(Z.ColumnName)).ToList().ForEach(T => tmp_animm.Columns.Remove(T));
                    }
                    else
                    {
                        tmp_animm = new DataTable();
                    }
                }
                else
                {
                    tmp_animm = Main_Frm_animals_tbl.DefaultView.ToTable(false, "ID", "NUM_IDENTIF", "CLIENT_FULL_NME", "NME", "ESPECE", "RACE", "SEXE", "NISS_DATE", "DATE_ADDED", "IS_RADIATED", "OBSERVATIONS");
                }

                dataGridView3.DataSource = tmp_animm;
                textBox2_TextChanged(null, null);



                //-------------------
            }

        }
        private void DGV_Visit_Filter(bool update_tot)
        {
            string fltr = radioButton8.Checked && comboBox2.Items.Count > 0 ? (comboBox1.SelectedIndex == 0 ? "CLIENT_ID" : "ANIM_ID") + " = " + (comboBox2.SelectedValue != null ? comboBox2.SelectedValue : -1) : "";
            //-----------------------------------
            fltr += string.Format(textBox1.Text.Trim().Length > 0 ? ((fltr.Length > 0 ? " AND " : "") + "("
                + "VISITOR_FULL_NME LIKE '%{0}%'"
                + " OR CONVERT(DATETIME, 'System.String') LIKE '%{0}%'"
                + " OR OBJECT LIKE '%{0}%'"
                + " OR FACTURE_REF LIKE '%{0}%'"
                + " OR ANIM_NME LIKE '%{0}%'"
                + " OR CLIENT_FULL_NME LIKE '%{0}%'"
                + ")") : "", textBox1.Text.Replace("'", "''"));
            bool dd = fltr.Length > 0;
            //------------------------------------

            //===================================================
            int tss = 0;
            int fct1 = 0;
            if (update_tot && main_visites_tab != null)
            {
                DataTable tmppp = main_visites_tab.Copy();
                tmppp.DefaultView.RowFilter = fltr;
                tss = tmppp.DefaultView.Cast<DataRowView>().Count();
                //----
                tmppp.DefaultView.RowFilter = fltr + (dd ? " AND " : "") + "LEN(FACTURE_REF) > 0";
                fct1 = tmppp.DefaultView.Cast<DataRowView>().Count();
            }
            //===================================================
            if (radioButton2.Checked) //Facturé
            {
                fltr += (dd ? " AND " : "") + "LEN(FACTURE_REF) > 0";
            }
            else if (radioButton3.Checked) //Non Facturé
            {

                fltr += (dd ? " AND " : "") + "FACTURE_REF IS NULL OR LEN(FACTURE_REF) = 0";
            }

            ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = fltr;
            //--------------------------------
            if (update_tot)
            {
                radioButton1.Text = "Tous (" + tss + ")";
                radioButton2.Text = "Facturé (" + fct1 + ")";
                radioButton3.Text = "Non Facturé (" + (tss - fct1) + ")";
            }

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            System.Drawing.Font fnt = new System.Drawing.Font(radioButton1.Font.FontFamily, (float)8.25, FontStyle.Regular);
            System.Drawing.Font fnt2 = new System.Drawing.Font(radioButton1.Font.FontFamily, (float)9.25, FontStyle.Bold);
            radioButton1.Font = radioButton1.Checked ? fnt2 : fnt;
            radioButton2.Font = radioButton2.Checked ? fnt2 : fnt;
            radioButton3.Font = radioButton3.Checked ? fnt2 : fnt;
            //--------------------
            DGV_Visit_Filter(false);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh_current_tab();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGV_Lab_Filter(true);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            DGV_Lab_Filter(true);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            button13.Visible = dataGridView1.SelectedRows.Count > 0;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows[0].Cells["REF2"].Value != DBNull.Value)
            {

                (new Laboratoire(selected_animal_id, dataGridView1.SelectedRows[0].Cells["REF2"].Value.ToString(), true, "")).ShowDialog();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].Cells["REF2"].Value != DBNull.Value)
                {

                    (new Laboratoire(selected_animal_id, dataGridView1.SelectedRows[0].Cells["REF2"].Value.ToString(), false, "")).ShowDialog();
                }
                Refresh_current_tab();
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (button14.Visible) { button14_Click(null, null); }

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            System.Drawing.Font fnt = new System.Drawing.Font(radioButton4.Font.FontFamily, (float)8.25, FontStyle.Regular);
            System.Drawing.Font fnt2 = new System.Drawing.Font(radioButton4.Font.FontFamily, (float)9.25, FontStyle.Bold);

            radioButton6.Font = radioButton6.Checked ? fnt2 : fnt;
            radioButton5.Font = radioButton5.Checked ? fnt2 : fnt;
            radioButton4.Font = radioButton4.Checked ? fnt2 : fnt;

            DGV_Lab_Filter(false);
        }

        private void DGV_Lab_Filter(bool update_tot)
        {
            string fltr = "";
            //------------------------------------
            switch (comboBox3.Text)
            {
                case "- Tous -":
                    fltr = "";
                    break;
                case "Hemogramme":
                    fltr = "LABO_NME LIKE 'Hemogramme'";
                    break;
                case "Biochimie Sanguine":
                    fltr = "LABO_NME LIKE 'Biochimie'";
                    break;
                case "Immunologie":
                    fltr = "LABO_NME LIKE 'Immunologie'";
                    break;
                case "Protéinogramme":
                    fltr = "LABO_NME LIKE 'Protéinogramme'";
                    break;
                case "Urologie":
                    fltr = "LABO_NME LIKE 'Urologie'";
                    break;
                case "- Autres -":
                    fltr = "LABO_NME NOT IN ('Hemogramme','Biochimie','Immunologie','Protéinogramme','Urologie')";
                    break;
            }
            fltr += radioButton8.Checked && comboBox2.Items.Count > 0 ? (fltr.Length > 0 ? " AND " : "") + (comboBox1.SelectedIndex == 0 ? "CLIENT_ID" : "ANIM_ID") + " = " + comboBox2.SelectedValue : "";
            fltr += string.Format(textBox3.Text.Trim().Length > 0 ? ((fltr.Length > 0 ? " AND " : "") + "("
                + "LABO_NME LIKE '%{0}%'"
                + " OR CONVERT(DATE_TIME, 'System.String') LIKE '%{0}%'"
                + " OR REF LIKE '%{0}%'"
                + " OR OBSERV LIKE '%{0}%'"
                + " OR ANIM_NME LIKE '%{0}%'"
                + " OR CLIENT_FULL_NME LIKE '%{0}%'"
                + ")") : "", textBox3.Text.Replace("'", "''"));
            bool dd = fltr.Length > 0;
            //===================================================
            int tss = 0;
            int fct1 = 0;
            if (update_tot)
            {
                DataTable tmppp = main_lab_tab.Copy();
                tmppp.DefaultView.RowFilter = fltr;
                tss = tmppp.DefaultView.Cast<DataRowView>().Count();
                //----
                tmppp.DefaultView.RowFilter = fltr + (dd ? " AND " : "") + "LEN(FACTURE_REF) > 0";
                fct1 = tmppp.DefaultView.Cast<DataRowView>().Count();
            }
            //===================================================
            if (radioButton5.Checked) //Facturé
            {
                fltr += (dd ? " AND " : "") + "LEN(FACTURE_REF) > 0";

            }
            else if (radioButton4.Checked) //Non Facturé
            {
                fltr += (dd ? " AND (" : "") + "FACTURE_REF IS NULL OR LEN(FACTURE_REF) = 0" + (dd ? ")" : "");
            }
            if (dataGridView1.DataSource != null)
            {
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = fltr;
            }
            //--------------------
            if (update_tot)
            {
                radioButton6.Text = "Tous (" + tss + ")";
                radioButton5.Text = "Facturé (" + fct1 + ")";
                radioButton4.Text = "Non Facturé (" + (tss - fct1) + ")";
            }
        }

        private void save_and_restore_select_sit(int save_1_restore_2)
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tabPage_visites":
                    if (save_1_restore_2 == 1)
                    {
                        prev_sel_rw = dataGridView2.SelectedRows.Count > 0 ? dataGridView2.SelectedRows[0].Index : -1;
                        frst_scrll = dataGridView2.Rows.Count > 0 ? dataGridView2.FirstDisplayedScrollingRowIndex : -1;
                    }
                    else
                    {
                        //-------------------
                        if (dataGridView2.Rows.Count > prev_sel_rw && prev_sel_rw >= 0)
                        {
                            dataGridView2.ClearSelection();
                            dataGridView2.Rows[prev_sel_rw].Selected = true;
                        }
                        if (dataGridView2.Rows.Count > frst_scrll && frst_scrll >= 0) { dataGridView2.FirstDisplayedScrollingRowIndex = frst_scrll; }
                    }
                    break;
                case "tabPage_labo":
                    if (save_1_restore_2 == 1)
                    {
                        prev_sel_rw = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : -1;
                        frst_scrll = dataGridView1.Rows.Count > 0 ? dataGridView1.FirstDisplayedScrollingRowIndex : -1;
                    }
                    else
                    {
                        //-------------------
                        if (dataGridView1.Rows.Count > prev_sel_rw && prev_sel_rw >= 0)
                        {
                            dataGridView1.ClearSelection();
                            dataGridView1.Rows[prev_sel_rw].Selected = true;

                        }
                        //-------------------
                        if (dataGridView1.Rows.Count > frst_scrll && frst_scrll >= 0)
                        {
                            dataGridView1.FirstDisplayedScrollingRowIndex = frst_scrll;
                        }
                    }
                    break;
                case "tabPage_animaux":
                    if (save_1_restore_2 == 1)
                    {
                        prev_sel_rw = dataGridView3.SelectedRows.Count > 0 ? dataGridView3.SelectedRows[0].Index : -1;
                        frst_scrll = dataGridView3.Rows.Count > 0 ? dataGridView3.FirstDisplayedScrollingRowIndex : -1;
                    }
                    else
                    {
                        //-------------------
                        if (dataGridView3.Rows.Count > prev_sel_rw && prev_sel_rw >= 0)
                        {
                            dataGridView3.ClearSelection();
                            dataGridView3.Rows[prev_sel_rw].Selected = true;
                        }
                        if (dataGridView3.Rows.Count > frst_scrll && frst_scrll >= 0) { dataGridView3.FirstDisplayedScrollingRowIndex = frst_scrll; }
                    }
                    break;
                case "tabPage_monetique":
                    if (save_1_restore_2 == 1)
                    {
                        prev_sel_rw = dataGridView4.SelectedRows.Count > 0 ? dataGridView4.SelectedRows[0].Index : -1;
                        frst_scrll = dataGridView4.Rows.Count > 0 ? dataGridView4.FirstDisplayedScrollingRowIndex : -1;
                        prev_sel_rw_facture = dataGridView5.SelectedRows.Count > 0 ? dataGridView5.SelectedRows[0].Index : -1;
                        frst_scrll_facture = dataGridView5.Rows.Count > 0 ? dataGridView5.FirstDisplayedScrollingRowIndex : -1;
                    }
                    else
                    {
                        if (dataGridView4.Rows.Count > prev_sel_rw && prev_sel_rw >= 0)
                        {
                            dataGridView4.ClearSelection();
                            dataGridView4.Rows[prev_sel_rw].Selected = true;
                        }
                        if (dataGridView4.Rows.Count > frst_scrll && frst_scrll >= 0) { dataGridView4.FirstDisplayedScrollingRowIndex = frst_scrll; }
                        //-------------------
                        if (dataGridView5.Rows.Count > prev_sel_rw_facture && prev_sel_rw_facture >= 0)
                        {
                            dataGridView5.ClearSelection();
                            dataGridView5.Rows[prev_sel_rw_facture].Selected = true;
                        }
                        if (dataGridView5.Rows.Count > frst_scrll_facture && frst_scrll_facture >= 0) { dataGridView5.FirstDisplayedScrollingRowIndex = frst_scrll_facture; }
                    }
                    break;
            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                (new Laboratoire(selected_animal_id, "", false, (string)dataGridView1.SelectedRows[0].Cells["LABO_NME"].Value)).ShowDialog();
            }
            else
            {
                (new Laboratoire(selected_animal_id, "", false, comboBox3.Text)).ShowDialog();
            }

            Refresh_current_tab();
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            button3.Select();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (selected_animal_id > -1)
            {
                if (Application.OpenForms["Animaux"] == null)
                {
                    new Animaux(selected_animal_id, -1).Show();
                }
                else
                {
                    Animaux.ID_to_selectt = selected_animal_id;
                    Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                    Application.OpenForms["Animaux"].BringToFront();
                }
                panel1.Visible = false;
            }

        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (button18.Visible)
            {

                if (e.RowIndex > -1)
                {

                    if (Application.OpenForms["Animaux"] == null)
                    {
                        new Animaux((int)dataGridView2.Rows[e.RowIndex].Cells["ANIM_ID"].Value, (int)dataGridView2.Rows[e.RowIndex].Cells["ID_VISITE"].Value).Show();
                    }
                    else
                    {
                        Animaux.ID_to_selectt = (int)dataGridView2.Rows[e.RowIndex].Cells["ANIM_ID"].Value;
                        Animaux.visite_idd = (int)dataGridView2.Rows[e.RowIndex].Cells["ID_VISITE"].Value;
                        Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                        Application.OpenForms["Animaux"].BringToFront();
                    }
                    panel1.Visible = false;
                }
            }

        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewCellMouseEventArgs rr = new DataGridViewCellMouseEventArgs(1, dataGridView2.SelectedRows[0].Index, 1, 1, new MouseEventArgs(MouseButtons.Left, 2, 1, 1, 0));
                dataGridView2_CellMouseDoubleClick(dataGridView2, rr);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (selected_animal_id > -1)
            {
                if (Application.OpenForms["Animaux"] == null)
                {
                    new Animaux(selected_animal_id, -2).Show();
                }
                else
                {
                    Animaux.ID_to_selectt = selected_animal_id;
                    Animaux.visite_idd = -2;
                    Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                    Application.OpenForms["Animaux"].BringToFront();
                }
                panel1.Visible = false;
            }
            else
            {
                MessageBox.Show("Veuillez séléctionner -d'abord- un animal.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < tabControl1.TabPages.Count)
            {
                if (Properties.Settings.Default.Main_Frm_Tabs_Horientation_Is_Verticatl)
                {
                    //---------
                    var tabPage = tabControl1.TabPages[e.Index];

                    var headerBounds = tabControl1.GetTabRect(e.Index);



                    System.Drawing.Font fntt = tabControl1.Font;
                    Brush color_txt = Brushes.Black;

                    if (e.Index == tabControl1.SelectedIndex)
                    {
                        fntt = bold_font;
                        using (var brush = new SolidBrush(Color.DarkGreen))
                        {
                            e.Graphics.FillRectangle(brush, headerBounds);
                            headerBounds.X -= 3;
                        }
                        color_txt = Brushes.White;
                        // Draw the bottom rectangle with the specified color and height
                        System.Drawing.Rectangle bottomRect = new System.Drawing.Rectangle(e.Bounds.Left, e.Bounds.Bottom - 25, e.Bounds.Width - 2, 25);
                        using (SolidBrush brush = new SolidBrush(Color.DarkSeaGreen))
                        {
                            e.Graphics.FillRectangle(brush, bottomRect);
                        }
                    }
                    else
                    {
                        fntt = simple_font;
                        using (var brush = new SolidBrush(Color.White))
                        {
                            e.Graphics.FillRectangle(brush, headerBounds);
                        }
                    }


                    if (tabcontrol_img_lst != null && tabPage.ImageIndex >= 0 && tabPage.ImageIndex < tabcontrol_img_lst.Images.Count)
                    {
                        var icon = tabcontrol_img_lst.Images[tabPage.ImageIndex];
                        var iconBounds = new System.Drawing.Rectangle(e.Bounds.Left + 10, e.Bounds.Bottom - 20, icon.Width, icon.Height);

                        e.Graphics.DrawImage(icon, iconBounds);
                        headerBounds.X += iconBounds.Width + 4; // Adjust the X position for the text
                    }
                    //---------------Notifiaction (If needed) -----------------
                    //if (tabcontrol_img_lst != null && tabPage.ImageIndex >= 0 && tabPage.ImageIndex < tabcontrol_img_lst.Images.Count)
                    //{
                    //    var icon = tabcontrol_img_lst.Images[4];
                    //    var iconBounds = new System.Drawing.Rectangle(e.Bounds.Left + 10, e.Bounds.Top + 3, icon.Width, icon.Height);
                    //    e.Graphics.DrawImage(icon, iconBounds);
                    //}
                    //--------------------------------
                    e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    e.Graphics.TranslateTransform(headerBounds.Left + headerBounds.Width / 2, headerBounds.Top + headerBounds.Height / 2);
                    e.Graphics.RotateTransform(-90);
                    e.Graphics.DrawString(tabPage.Text, fntt, color_txt, -(headerBounds.Height / 2) + 25, -(headerBounds.Width / 2) - 10, StringFormat.GenericDefault);

                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                }
                else
                {
                    //--------------
                    var tabPage = tabControl1.TabPages[e.Index];
                    var headerBounds = tabControl1.GetTabRect(e.Index);
                    System.Drawing.Font fntt = tabControl1.Font;
                    Brush color_txt = Brushes.Black;

                    if (e.Index == tabControl1.SelectedIndex)
                    {
                        fntt = bold_font;
                        using (var brush = new SolidBrush(Color.DarkGreen))
                        {
                            e.Graphics.FillRectangle(brush, headerBounds);
                            //headerBounds.X -= 3;
                        }
                        color_txt = Brushes.White;
                        // Draw the bottom rectangle with the specified color and height
                        System.Drawing.Rectangle bottomRect = new System.Drawing.Rectangle(e.Bounds.Left, e.Bounds.Top, 30, e.Bounds.Height);
                        using (SolidBrush brush = new SolidBrush(Color.DarkSeaGreen))
                        {
                            e.Graphics.FillRectangle(brush, bottomRect);
                        }
                    }
                    else
                    {
                        fntt = simple_font;
                        using (var brush = new SolidBrush(Color.White))
                        {
                            e.Graphics.FillRectangle(brush, headerBounds);
                        }
                    }
                    if (tabcontrol_img_lst != null && tabPage.ImageIndex >= 0 && tabPage.ImageIndex < tabcontrol_img_lst.Images.Count)
                    {
                        var icon = tabcontrol_img_lst.Images[tabPage.ImageIndex];
                        var iconBounds = new System.Drawing.Rectangle(e.Bounds.Left + 10, e.Bounds.Height / 2 - 3, icon.Width, icon.Height);

                        e.Graphics.DrawImage(icon, iconBounds);
                        headerBounds.X += iconBounds.Width + 4; // Adjust the X position for the text
                    }
                    //--------------------------------
                    e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    e.Graphics.DrawString(tabPage.Text, fntt, color_txt, headerBounds.Left + 15, (headerBounds.Height / 2 - 3), StringFormat.GenericDefault);
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                }

            }














            //if (e.Index < tabControl1.TabPages.Count)
            //{
            //    var tabPage = tabControl1.TabPages[e.Index];
            //    var headerBounds = tabControl1.GetTabRect(e.Index);

            //    e.Graphics.FillRectangle(Brushes.White, headerBounds);

            //    if (e.Index == tabControl1.SelectedIndex)
            //    {
            //        using (var brush = new SolidBrush(Color.DarkGreen))
            //        {
            //            e.Graphics.FillRectangle(brush, headerBounds);
            //        }
            //    }

            //    if (tabcontrol_img_lst != null && tabPage.ImageIndex >= 0 && tabPage.ImageIndex < tabcontrol_img_lst.Images.Count)
            //    {
            //        var icon = tabcontrol_img_lst.Images[tabPage.ImageIndex];
            //        var iconBounds = new Rectangle(headerBounds.Left + 10, headerBounds.Top + 10, icon.Width, icon.Height);
            //        e.Graphics.DrawImage(icon, iconBounds);
            //        headerBounds.X += iconBounds.Width + 4;
            //    }

            //    using (var sf = new StringFormat())
            //    {
            //        sf.Alignment = StringAlignment.Center;
            //        sf.LineAlignment = StringAlignment.Center;
            //        e.Graphics.DrawString(tabPage.Text, tabControl1.Font, Brushes.Black, headerBounds, sf);
            //    }
            //}
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DGV_Visit_Filter(true);

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked && tabControl1.TabPages["tabPage_infos"] == null)
            {
                tabControl1.TabPages.Insert(0, tabPage_infos);
            }
            else if (tabControl1.TabPages["tabPage_infos"] != null)
            {
                tabControl1.TabPages.Remove(tabPage_infos);
            }

            switch (tabControl1.SelectedTab.Name)
            {
                case "tabPage_visites":
                    DGV_Visit_Filter(true);
                    break;
                case "tabPage_labo":
                    DGV_Lab_Filter(true);
                    break;
                case "tabPage_Calendar":
                    Agenda_Just_Display.make_filter_refresh = true;
                    Refresh_current_tab();
                    break;
                case "tabPage_animaux":
                    DGV_Anim_Filter();
                    break;
                case "tabPage_monetique":
                    textBox4_TextChanged(null, null);
                    textBox5_TextChanged(null, null);
                    break;
                case "tabPage_vaccin":
                    Refresh_current_tab();
                    break;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string ddd = "NUM_IDENTIF LIKE '%{0}%' OR CLIENT_FULL_NME LIKE '%{0}%' OR NME LIKE '%{0}%' OR ESPECE LIKE '%{0}%' OR RACE LIKE '%{0}%' OR SEXE LIKE '%{0}%' OR Convert([NISS_DATE], System.String) LIKE '%{0}%' OR Convert([DATE_ADDED], System.String) LIKE '%{0}%'";
            ((DataTable)dataGridView3.DataSource).DefaultView.RowFilter = String.Format(ddd, textBox2.Text.Replace("'", "''"));
            label3.Text = "Total: " + dataGridView3.Rows.Count;
        }

        private void dataGridView3_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cellValue = dataGridView3.Rows[e.RowIndex].Cells["ANIMM_IS_RADIATED"].Value;

                if (cellValue != null && (sbyte)cellValue == 1)
                {
                    dataGridView3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else
                {
                    dataGridView3.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView3.DefaultCellStyle.BackColor;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms["Animaux"] == null)
            {
                new Animaux(-2, -1).Show();
            }
            else
            {
                Animaux.ID_to_selectt = -2;
                Animaux.visite_idd = -1;
                Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                Application.OpenForms["Animaux"].BringToFront();
            }

        }

        private void textBox8_Enter_1(object sender, EventArgs e)
        {
            button10.Focus();
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            textBox8.Text = dataGridView3.SelectedRows.Count > 0 ? (dataGridView3.SelectedRows[0].Cells["ANIMM_OBSERVATIONS"].Value != DBNull.Value ? (string)dataGridView3.SelectedRows[0].Cells["ANIMM_OBSERVATIONS"].Value : "") : "";
        }

        private void button10_Click(object sender, EventArgs e)
        {

            if (dataGridView3.SelectedRows.Count > 0)
            {
                if (Application.OpenForms["Animaux"] == null)
                {
                    new Animaux((int)dataGridView3.SelectedRows[0].Cells["ANIMM_ID"].Value, -1).Show();
                }
                else
                {
                    Animaux.ID_to_selectt = (int)dataGridView3.SelectedRows[0].Cells["ANIMM_ID"].Value;
                    Animaux.visite_idd = -1;
                    Application.OpenForms["Animaux"].WindowState = Application.OpenForms["Animaux"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Animaux"].WindowState;
                    Application.OpenForms["Animaux"].BringToFront();
                }
            }
        }

        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (button10.Visible) { button10_Click(null, null); }

        }

        private void Main_Frm_Deactivate(object sender, EventArgs e)
        {
            save_and_restore_select_sit(1);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string fltr = radioButton8.Checked && comboBox2.SelectedValue != null ? (comboBox2.SelectedValue != DBNull.Value ? "CLIENT_ID = " + comboBox2.SelectedValue.ToString() : "") : "";
            fltr += string.Format(textBox4.Text.Trim().Length > 0 ? (fltr.Length > 0 ? " AND " : "") + "("
                + "OBJECT LIKE '%{0}%'"
                + " OR CONVERT(OP_DATE, 'System.String') LIKE '%{0}%'"
                + " OR CLIENT_FULL_NME LIKE '%{0}%'"
                + " OR FACT_NUM LIKE '%{0}%'"
                + " OR CONVERT([DEBIT], 'System.String') LIKE '%{0}%'"
                + " OR CONVERT([CREDIT], 'System.String') LIKE '%{0}%'"
                + ")" : "", textBox4.Text.Replace("'", "''"));
            ((DataTable)dataGridView4.DataSource).DefaultView.RowFilter = fltr;
            //-------------
            if (dataGridView6.Rows.Count == 0) { dataGridView6.Rows.Add(); }
            dataGridView6.Rows[0].Cells[0].Value = "Total (" + ((DataTable)dataGridView4.DataSource).DefaultView.Count + ") :";
            dataGridView6.Rows[0].Cells[1].Value = ((DataTable)dataGridView4.DataSource).DefaultView.Cast<DataRowView>().Sum(rowView => (decimal)rowView["DEBIT"]);
            dataGridView6.Rows[0].Cells[2].Value = ((DataTable)dataGridView4.DataSource).DefaultView.Cast<DataRowView>().Sum(rowView => (decimal)rowView["CREDIT"]);
            //-----------------
            dataGridView4.Columns["FINN_DEBIT"].Width = dataGridView6.Columns[1].Width;
            dataGridView4.Columns["FINN_CREDIT"].Width = dataGridView6.Columns[2].Width;
            dataGridView6.Width = dataGridView4.Width - (dataGridView4.Controls.OfType<VScrollBar>().FirstOrDefault().Visible ? 17 : 0);
            //--------------------
            decimal sld = 0;
            sld = ((DataTable)dataGridView4.DataSource).DefaultView.Cast<DataRowView>().Sum(row => row["DEBIT"] != DBNull.Value ? Convert.ToDecimal(row["DEBIT"]) : 0) - ((DataTable)dataGridView4.DataSource).DefaultView.Cast<DataRowView>().Sum(row => row["CREDIT"] != DBNull.Value ? Convert.ToDecimal(row["CREDIT"]) : 0);
            if (sld == 0)
            {
                label9.Text = (textBox4.Text.Length == 0 && radioButton8.Checked ? "Rien " : "") + "(0.00 DA).";
            }
            else if (sld >= 0)
            {
                label9.Text = (textBox4.Text.Length == 0 && radioButton8.Checked ? "Il est endetté de " : "Ils sont endettés de ") + "(" + sld.ToString("N2") + " DA).";
            }
            else
            {
                label9.Text = (textBox4.Text.Length == 0 && radioButton8.Checked ? "On lui doit " : "On leur doit ") + "(" + (sld * -1).ToString("N2") + " DA).";
            }

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string fltr = radioButton8.Checked && comboBox2.SelectedIndex >= 0 /*!= DBNull.Value*/ ? "CLIENT_ID = " + comboBox2.SelectedValue.ToString() : "";
            fltr += string.Format(textBox5.Text.Trim().Length > 0 ? (fltr.Length > 0 ? " AND " : "") + "("
                + "CLIENT_FULL_NME LIKE '%{0}%'"
                + " OR CONVERT(DATE, 'System.String') LIKE '%{0}%'"
                + " OR REF LIKE '%{0}%'"
                + " OR CONVERT([TOTAL_HT], 'System.String') LIKE '%{0}%'"
                + " OR CONVERT([TVA_PERC], 'System.String') LIKE '%{0}%'"
                + " OR CONVERT([DROIT_TIMBRE], 'System.String') LIKE '%{0}%'"
                + " OR CONVERT([TOTAL_TTC], 'System.String') LIKE '%{0}%'"
                + " OR CONVERT([LAST_MODIF_BY], 'System.String') LIKE '%{0}%'"
                + " OR CONVERT([FACT_PAID_MNT], 'System.String') LIKE '%{0}%'"
                + " OR CONVERT([SLD_OF_CLIENT], 'System.String') LIKE '%{0}%'"
                + ")" : "", textBox5.Text.Replace("'", "''"));
            ((DataTable)dataGridView5.DataSource).DefaultView.RowFilter = fltr;
            //-------------
            if (dataGridView7.Rows.Count == 0) { dataGridView7.Rows.Add(); }
            dataGridView7.Rows[0].Cells[0].Value = "Total (" + ((DataTable)dataGridView5.DataSource).DefaultView.Count + ") :";

            dataGridView7.Rows[0].Cells[1].Value = ((DataTable)dataGridView5.DataSource).DefaultView.Cast<DataRowView>().Where(XX => XX["TOTAL_HT"] != DBNull.Value).Sum(rowView => (decimal)rowView["TOTAL_HT"]);
            dataGridView7.Rows[0].Cells[2].Value = ((DataTable)dataGridView5.DataSource).DefaultView.Cast<DataRowView>().Where(XX => XX["TVA_PERC"] != DBNull.Value).Sum(rowView => (decimal)rowView["TVA_PERC"]);
            dataGridView7.Rows[0].Cells[3].Value = ((DataTable)dataGridView5.DataSource).DefaultView.Cast<DataRowView>().Where(XX => XX["DROIT_TIMBRE"] != DBNull.Value).Sum(rowView => (decimal)rowView["DROIT_TIMBRE"]);
            dataGridView7.Rows[0].Cells[4].Value = ((DataTable)dataGridView5.DataSource).DefaultView.Cast<DataRowView>().Where(XX => XX["TOTAL_TTC"] != DBNull.Value).Sum(rowView => (decimal)rowView["TOTAL_TTC"]);
            dataGridView7.Rows[0].Cells[5].Value = ((DataTable)dataGridView5.DataSource).DefaultView.Cast<DataRowView>().Where(XX => XX["FACT_PAID_MNT"] != DBNull.Value).Sum(rowView => (decimal)rowView["FACT_PAID_MNT"]);
            //-----------------
            dataGridView5.Columns[5].Width = dataGridView7.Columns[1].Width;
            dataGridView5.Columns[6].Width = dataGridView7.Columns[2].Width;
            dataGridView5.Columns[7].Width = dataGridView7.Columns[3].Width;
            dataGridView5.Columns[8].Width = dataGridView7.Columns[4].Width;
            dataGridView5.Columns[9].Width = dataGridView7.Columns[5].Width;
            //---------------
            dataGridView7.Width = dataGridView5.Width - dataGridView5.Columns[10].Width - (dataGridView5.Controls.OfType<VScrollBar>().FirstOrDefault().Visible ? 17 : 0);
            //-------            
            label8.Text = "Réglé (" + ((DataTable)dataGridView5.DataSource).DefaultView.Cast<DataRowView>().Where(XX => XX["CLIENT_ID"] != DBNull.Value && (XX["FACT_PAID_MNT"] != DBNull.Value ? (decimal)XX["FACT_PAID_MNT"] : 0) <= 0).Count() + ")";
            label7.Text = "Non réglé (" + ((DataTable)dataGridView5.DataSource).DefaultView.Cast<DataRowView>().Where(XX => XX["CLIENT_ID"] != DBNull.Value && (XX["FACT_PAID_MNT"] != DBNull.Value ? (decimal)XX["FACT_PAID_MNT"] : 0) > 0).Count() + ")";
        }

        private void dataGridView4_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (button19.Visible && dataGridView4.Rows[e.RowIndex].Cells["FINN_CLIENT_ID"].Value != DBNull.Value)
            {
                if (Application.OpenForms["Clients"] == null)
                {
                    new Clients((int)dataGridView4.Rows[e.RowIndex].Cells["FINN_CLIENT_ID"].Value, 2, (int)dataGridView4.Rows[e.RowIndex].Cells["FINN_ID"].Value).Show();
                }
                else
                {
                    Clients.ID_to_selectt = (int)dataGridView4.Rows[e.RowIndex].Cells["FINN_CLIENT_ID"].Value;
                    Clients.Infoss_1_Caiss_2 = 2;
                    Clients.Caisse_Idx = (int)dataGridView4.Rows[e.RowIndex].Cells["FINN_ID"].Value;
                    Application.OpenForms["Clients"].WindowState = Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Clients"].WindowState;
                    Application.OpenForms["Clients"].BringToFront();
                }

            }
        }

        private void dataGridView5_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (button21.Visible && dataGridView5.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn1"].Value != DBNull.Value)
            {
                if (Application.OpenForms["Vente"] == null)
                {
                    new Vente((int)dataGridView5.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn1"].Value).Show();
                }
                else
                {
                    Vente.to_select_idx = (int)dataGridView5.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn1"].Value;
                    Application.OpenForms["Vente"].WindowState = Application.OpenForms["Vente"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Vente"].WindowState;
                    Application.OpenForms["Vente"].BringToFront();
                }

            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                DataGridViewCellMouseEventArgs rr = new DataGridViewCellMouseEventArgs(1, dataGridView4.SelectedRows[0].Index, 1, 1, new MouseEventArgs(MouseButtons.Left, 2, 1, 1, 0));
                dataGridView4_CellMouseDoubleClick(null, rr);
            }
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            if (Application.OpenForms["Clients"] == null)
            {
                new Clients(selected_client_id, 2, -2).Show();
            }
            else
            {
                Clients.ID_to_selectt = selected_client_id;
                Clients.Infoss_1_Caiss_2 = 2;
                Clients.Caisse_Idx = -2;
                Application.OpenForms["Clients"].WindowState = Application.OpenForms["Clients"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Clients"].WindowState;
                Application.OpenForms["Clients"].BringToFront();
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Vente"] == null)
            {
                new Vente(-2).Show();
            }
            else
            {
                Vente.to_select_idx = -2;
                Application.OpenForms["Vente"].WindowState = Application.OpenForms["Vente"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Vente"].WindowState;
                Application.OpenForms["Vente"].BringToFront();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (dataGridView5.SelectedRows.Count > 0)
            {
                DataGridViewCellMouseEventArgs rr1 = new DataGridViewCellMouseEventArgs(1, dataGridView5.SelectedRows[0].Index, 1, 1, new MouseEventArgs(MouseButtons.Left, 2, 1, 1, 0));
                dataGridView5_CellMouseDoubleClick(null, rr1);
            }

        }

        private void dataGridView5_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                decimal cellValue = dataGridView5.Rows[e.RowIndex].Cells["FACT_PAID_MNT"].Value != DBNull.Value ? (decimal)dataGridView5.Rows[e.RowIndex].Cells["FACT_PAID_MNT"].Value : 0;
                if (dataGridView5.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn2"].Value != DBNull.Value) //CLIENT_ID
                {
                    dataGridView5.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView5.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = cellValue > 0 ? Color.FromArgb(255, 192, 192) : Color.FromArgb(128, 255, 128);
                }
                else
                {
                    dataGridView5.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridView5.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = dataGridView5.DefaultCellStyle.BackColor;
                }
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            new App_Activation().Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time_delay++;
            if (text_to_add_to_title.Length > 0)
            {
                this.Text += text_to_add_to_title;
                text_to_add_to_title = "";
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            PreConnection.Excport_to_excel(dataGridView3, "Animaux", radioButton7.Checked ? "Tous" : comboBox2.Text, null, false);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            PreConnection.Excport_to_excel(dataGridView2, "Visites" + (comboBox1.SelectedIndex == 0 ? " Propriétaires" : " Animaux"), (radioButton7.Checked ? "Tous" : comboBox2.Text), null, false);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            PreConnection.Excport_to_excel(dataGridView1, "Laboratoires", (radioButton7.Checked ? "Tous" : comboBox2.Text), null, false);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            PreConnection.Excport_to_excel(dataGridView5, "Factures", (radioButton7.Checked ? "Tous" : comboBox2.Text), null, false);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            PreConnection.Excport_to_excel(dataGridView4, "Fonds", (radioButton7.Checked ? "Tous" : comboBox2.Text), null, false);
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            new Print_visites(comboBox1.SelectedIndex == 0 ? 2 : 1, comboBox2.SelectedValue != null ? (int)comboBox2.SelectedValue : -1).ShowDialog();
        }

        private void button28_Click(object sender, EventArgs e)
        {

            int anim_idd = dataGridView2.SelectedRows.Count > 0 ? dataGridView2.SelectedRows[0].Cells["ANIM_ID"].Value != DBNull.Value ? (int)dataGridView2.SelectedRows[0].Cells["ANIM_ID"].Value : -1 : -1;
            if (anim_idd < 0)
            {
                if (selected_animal_id > 0) { anim_idd = selected_animal_id; }
                else if (selected_client_id > 0 && Main_Frm_animals_tbl != null)
                {
                    var yy = Main_Frm_animals_tbl.AsEnumerable().Where(F => (int)F["CLIENT_ID"] == selected_client_id);
                    if (yy.Any()) { anim_idd = (int)yy.First()["ID"]; }
                }
            }
            if (anim_idd > 0) { new New_Ordonnance(anim_idd).ShowDialog(); }
            else
            {
                MessageBox.Show("Aucun animal trouvé", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button29_Click(object sender, EventArgs e)
        {
            int anim_idd = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Cells["ANIM_IDD"].Value != DBNull.Value ? (int)dataGridView1.SelectedRows[0].Cells["ANIM_IDD"].Value : -1 : -1;
            if (anim_idd < 0)
            {
                if (selected_animal_id > 0) { anim_idd = selected_animal_id; }
                else if (selected_client_id > 0 && Main_Frm_animals_tbl != null)
                {
                    var yy = Main_Frm_animals_tbl.AsEnumerable().Where(F => (int)F["CLIENT_ID"] == selected_client_id);
                    if (yy.Any()) { anim_idd = (int)yy.First()["ID"]; }
                }
            }
            if (anim_idd > 0) { new New_Ordonnance(anim_idd).ShowDialog(); }
            else
            {
                MessageBox.Show("Aucun animal trouvé", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = richTextBox1.Text.Length > 0 ? SystemColors.Window : Color.LightCoral;

            if (richTextBox1.BackColor == SystemColors.Window)
            {
                if (!Properties.Settings.Default.Last_login_is_admin)
                {
                    label18.Visible = Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50002" && (Int32)QQ[3] == 1).Count() == 0;
                }
                if (!label18.Visible)
                {
                    //PreConnection.Excut_Cmd("UPDATE `tb_visites` SET "
                    //                      + "`DATETIME` = '" + dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',"
                    //                      + "`VISITOR_FULL_NME` = '" + comboBox5.Text.Replace("'","''") + "',"
                    //                      + "`OBJECT` = '" + richTextBox1.Text.Replace("'", "''") + "'"
                    //                      + " WHERE `ID` = " + dataGridView2.SelectedRows[0].Cells["ID_VISITE"].Value + ";");
                    PreConnection.Excut_Cmd(2, "tb_visites", new List<string> { "DATETIME", "VISITOR_FULL_NME", "OBJECT" }, new List<object> { dateTimePicker4.Value, comboBox5.Text, richTextBox1.Text }, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView2.SelectedRows[0].Cells["ID_VISITE"].Value });
                }
                bool label18_visible = label18.Visible;
                int curr_idx = dataGridView2.SelectedRows[0].Index;
                int curr_scroll = dataGridView2.FirstDisplayedScrollingRowIndex;
                Refresh_current_tab();
                label18.Visible = label18_visible;
                dataGridView2.Rows[curr_idx].Selected = true;
                if (dataGridView2.Rows.Count > 1) { dataGridView2.Rows[0].Selected = false; }
                dataGridView2.FirstDisplayedScrollingRowIndex = curr_scroll;
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                groupBox1.Visible = true;
                //-----
                label18.Visible = false;
                DataGridViewRow row = dataGridView2.SelectedRows[0];
                dateTimePicker4.Value = (DateTime)row.Cells["DATETIME"].Value;
                comboBox5.Text = (string)row.Cells["VISITOR_FULL_NME"].Value;
                richTextBox1.Text = (string)row.Cells["OBJECT"].Value;
                richTextBox1.BackColor = SystemColors.Window;
            }
            else
            {
                groupBox1.Visible = false;
                //-----
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            int anim_idd = dataGridView2.SelectedRows.Count > 0 ? dataGridView2.SelectedRows[0].Cells["ANIM_ID"].Value != DBNull.Value ? (int)dataGridView2.SelectedRows[0].Cells["ANIM_ID"].Value : -1 : -1;
            if (anim_idd < 0)
            {
                if (selected_animal_id > 0) { anim_idd = selected_animal_id; }
                else if (selected_client_id > 0 && Main_Frm_animals_tbl != null)
                {
                    var yy = Main_Frm_animals_tbl.AsEnumerable().Where(F => (int)F["CLIENT_ID"] == selected_client_id);
                    if (yy.Any()) { anim_idd = (int)yy.First()["ID"]; }
                }
            }

            if (anim_idd > 0) { new New_Pers_Cpt_Rnd_Visit(anim_idd).ShowDialog(); }
            else
            {
                MessageBox.Show("Aucun animal trouvé", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button32_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Vaccinations"] == null)
            {
                new Vaccinations().Show();
            }
            else
            {
                Application.OpenForms["Vaccinations"].WindowState = Application.OpenForms["Vaccinations"].WindowState == FormWindowState.Minimized ? FormWindowState.Normal : Application.OpenForms["Vaccinations"].WindowState;
                Application.OpenForms["Vaccinations"].BringToFront();
            }
            panel1.Visible = false;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {
            if (!panel5.Visible)
            {
                if (panel5.Controls["Vaccin_Alerts"] == null)
                {
                    panel5.Controls.Add(new Vaccin_Alerts());
                    panel5.Controls["Vaccin_Alerts"].Dock = DockStyle.Fill;
                }
                panel5.Visible = true;
                panel5.Focus();
                ((Vaccin_Alerts)panel5.Controls[0]).load_data();
            }
            else
            {
                panel5.Visible = false;
            }

        }

        private void panel5_Leave(object sender, EventArgs e)
        {
            panel5.Visible = false;
        }

        private void dataGridView5_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView5.Rows[e.RowIndex].Selected)
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
            }
        }
    }
}

