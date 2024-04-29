using MimeKit;
using MySql.Data.MySqlClient;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{

    public partial class App_Activation : Form
    {


        int rest_jrs_delay = 30;
        public App_Activation()
        {
            InitializeComponent();
            //---------------------------
            if (Main_Frm.Baitar_Server_Params.Rows.Count > 0)
            {
                string BAITAR_ADRESS_EMAIL = Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().Where(d => d["NME"].Equals("EMAIL_TO_RECIEVE_COMMANDS")).ToList().Count > 0 ? (string)Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().First(d => d["NME"].Equals("EMAIL_TO_RECIEVE_COMMANDS"))["VAL"] : "";
                textBox2.Text += BAITAR_ADRESS_EMAIL;
                label23.Text = BAITAR_ADRESS_EMAIL;
            }
            //----------------------------
            PreConnection.load_rancosoft_gmail_auth();
        }

        private void App_Activation_Load(object sender, EventArgs e)
        {
            try
            {
                Application.OpenForms["Splash"]?.Close();
            }
            catch { }

            //--------------------
            //label8.Text = PreConnection.generate_ID_of_client();
            label8.Text = Properties.Settings.Default.Codified_Act_Client_ID != null ? PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Act_Client_ID) : "";
            label9.Text = Environment.MachineName;
            label7.Text = Environment.UserName;
            textBox1.Validating -= textBox1_Validating;
            textBox1.Text = Properties.Settings.Default.Codifed_Activation_Email != null ? PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codifed_Activation_Email) : "";
            textBox1.Validating += textBox1_Validating;
            textBox5.Text = Properties.Settings.Default.Codified_Act_Clt_Tel != null ? PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Act_Clt_Tel) : "";
            textBox6.Text = Properties.Settings.Default.Codified_Act_Clt_Nme != null ? PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Act_Clt_Nme) : "";
            string cmd_dent_dte = PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Act_Command_dmnd_date);

            bool fff = DateTime.TryParse(cmd_dent_dte, out DateTime dd);
            if (fff)
            {
                label17.Visible = true;
                label17.Text = "Demande envoyée le : " + DateTime.Parse(cmd_dent_dte).ToString("dd/MM/yyyy");
            }
            //-----------------------
            string codd = PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Activate_Code);
            //--------

            //---------
            if (Check_activation(codd, textBox1.Text))
            {
                label6.Text = codd.Substring(0, 3) + "***************" + codd.Substring(23, 2);
                panel4.Visible = true;
                panel2.Visible = false;
                pictureBox1.Image = Properties.Resources.icons8_safe_ok_120px;
                textBox2.Visible = pictureBox3.Visible = panel3.Visible = false;
                this.Size = new System.Drawing.Size(this.Width, this.Height - panel3.Height);
                //---------------
                //-----------------------------//Check Finance
                string finance_stat = PreConnection.verify_baitar_client_finance(textBox1.Text, codd);

                string folderPath = "C:\\ProgramData\\BAITAR_CTRL";
                string filePath = folderPath + "\\Al_Baitar_Activation.txt";

                if (!File.Exists(filePath))
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                        File.SetAttributes(folderPath, FileAttributes.Directory | FileAttributes.Hidden);
                    }
                    //--------
                    File.Create(filePath).Dispose();
                    //---------
                    FileAttributes attributes = File.GetAttributes(filePath);
                    attributes |= FileAttributes.Hidden;
                    File.SetAttributes(filePath, attributes);
                }

                if (File.Exists(filePath))
                {
                    //---------
                    FileAttributes currentAttributes = File.GetAttributes(filePath);
                    FileAttributes updatedAttributes = currentAttributes & ~FileAttributes.Hidden;
                    File.SetAttributes(filePath, updatedAttributes);
                    //---------

                    string[] file_lines = File.ReadAllLines(filePath);
                    string[] file_lines_to_save = new string[4];

                    if (file_lines.Length > 0)
                    {
                        file_lines_to_save[0] = file_lines[0];
                    }

                    if (file_lines.Length > 1)
                    {
                        file_lines_to_save[1] = file_lines[1];
                    }

                    DateTime Server_Verif_Date = Properties.Settings.Default.First_Enter_Date_After_Install == new DateTime(1867, 05, 15) ? DateTime.UtcNow : Properties.Settings.Default.First_Enter_Date_After_Install;
                    if (file_lines.Length > 3)
                    {
                        DateTime.TryParse(PreConnection.Traduct_Codified_txt(file_lines[3]), out Server_Verif_Date);
                        file_lines_to_save[3] = file_lines[3];
                    }

                    if (finance_stat == "good")
                    {
                        file_lines_to_save[2] = PreConnection.Codify_txt("Yes_is_done");
                        file_lines_to_save[3] = PreConnection.Codify_txt(DateTime.UtcNow.ToString());
                        pictureBox5.Visible = true;
                        pictureBox6.Visible = false;
                        label22.Text = "Oui";
                        label22.ForeColor = Color.Green;
                    }
                    else
                    {
                        int Finance_delay = int.Parse(Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().Where(d => d["NME"].Equals("VERIF_DAYS_DELAY_FOR_NN_PAYED")).ToList().Count > 0 ? (string)Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().First(d => d["NME"].Equals("VERIF_DAYS_DELAY_FOR_NN_PAYED"))["VAL"] : "30");
                        label22.ForeColor = Color.Red;
                        if (finance_stat == "not good")
                        {
                            file_lines_to_save[2] = PreConnection.Codify_txt("Not_yet");
                            file_lines_to_save[3] = PreConnection.Codify_txt(DateTime.UtcNow.ToString());
                            label22.Text = "Pas encore";
                        }
                        int rest_finance_delay =
                           (Finance_delay - (DateTime.Now - Properties.Settings.Default.First_Enter_Date_After_Install).TotalDays) > 0 ?
                           Convert.ToInt32(Finance_delay - (DateTime.Now - Properties.Settings.Default.First_Enter_Date_After_Install).TotalDays)
                           : Finance_delay;
                        label22.Text += (label22.Text.Length > 0 ? "\n" : "") + "(Veuillez nous contacter " + (rest_finance_delay == 0 ? "immédiatement" : "avant " + rest_finance_delay + " jrs") + ")";
                        pictureBox5.Visible = false;
                        pictureBox6.Visible = true;

                    }
                    //------------
                    File.WriteAllLines(filePath, file_lines_to_save);
                    //---------
                    FileAttributes attributes = File.GetAttributes(filePath);
                    attributes |= FileAttributes.Hidden;
                    File.SetAttributes(filePath, attributes);
                }
            }
            else
            {
                
                //========= I/ First steps ==============
                string folderPath = "C:\\ProgramData\\BAITAR_CTRL";
                string filePath = folderPath + "\\Al_Baitar_Activation.txt";

                if (File.Exists(filePath))
                {
                    //---------
                    FileAttributes currentAttributes = File.GetAttributes(filePath);
                    FileAttributes updatedAttributes = currentAttributes & ~FileAttributes.Hidden;
                    File.SetAttributes(filePath, updatedAttributes);
                    //---------
                    string[] file_lines = File.ReadAllLines(filePath);
                    string[] file_lines_to_save = new string[4];

                    decimal prev_running_delay = 0; //delay per minute
                    if (file_lines.Length > 0)
                    {
                        decimal.TryParse(file_lines[0].ToString(), out prev_running_delay);
                        file_lines_to_save[0] = file_lines[0];
                    }

                    DateTime prev_saved_running_delay_datetime = new DateTime(1900, 01, 01);
                    if (file_lines.Length > 1)
                    {
                        DateTime.TryParse(PreConnection.Traduct_Codified_txt(file_lines[1]), out prev_saved_running_delay_datetime);
                        file_lines_to_save[1] = file_lines[1];
                    }
                    bool Is_finance_done = false;
                    if (file_lines.Length > 2)
                    {
                        Is_finance_done = PreConnection.Traduct_Codified_txt(file_lines[2]) == "Yes_is_done";
                        file_lines_to_save[2] = file_lines[2];
                    }

                    DateTime Server_Verif_Date = Properties.Settings.Default.First_Enter_Date_After_Install == new DateTime(1867, 05, 15) ? DateTime.UtcNow : Properties.Settings.Default.First_Enter_Date_After_Install;
                    if (file_lines.Length > 3)
                    {
                        DateTime.TryParse(PreConnection.Traduct_Codified_txt(file_lines[3]), out Server_Verif_Date);
                        file_lines_to_save[3] = file_lines[3];
                    }
                    //------------
                    int Finance_delay = int.Parse(Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().Where(d => d["NME"].Equals("VERIF_DAYS_DELAY_FOR_NN_PAYED")).ToList().Count > 0 ? (string)Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().First(d => d["NME"].Equals("VERIF_DAYS_DELAY_FOR_NN_PAYED"))["VAL"] : "30");
                    int Default_delay = int.Parse(Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().Where(d => d["NME"].Equals("VERIF_DAYS_DELAY_DEFAULT")).ToList().Count > 0 ? (string)Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().First(d => d["NME"].Equals("VERIF_DAYS_DELAY_DEFAULT"))["VAL"] : "30");
                    int RESTE_DELAY = Math.Min(Default_delay, Finance_delay);
                    bool reste_delay_ended = (DateTime.Now - Server_Verif_Date).TotalDays >= RESTE_DELAY;

                    if (!reste_delay_ended)
                    {
                        label13.Text = ((RESTE_DELAY - (DateTime.Now - Server_Verif_Date).TotalDays) > 0 ? Convert.ToInt32(RESTE_DELAY - (DateTime.Now - Server_Verif_Date).TotalDays).ToString() : "--") + " jours";
                    }
                    else {
                        label13.Text = Default_delay + " jours";
                    }
                    //------------------
                    //////////////
                    ///
                    //////////////
                    string finance_stat = PreConnection.verify_baitar_client_finance(PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codifed_Activation_Email), PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Activate_Code));

                    if (finance_stat == "good")
                    {
                        file_lines_to_save[2] = PreConnection.Codify_txt("Yes_is_done");
                        file_lines_to_save[3] = PreConnection.Codify_txt(DateTime.UtcNow.ToString());
                        pictureBox5.Visible = true;
                        pictureBox6.Visible = false;
                        label22.Text = "Oui";
                        label22.ForeColor = Color.Green;
                    }
                    else
                    {
                        label22.ForeColor = Color.Red;
                        label22.Text = "";
                        if (finance_stat == "not good")
                        {
                            file_lines_to_save[2] = PreConnection.Codify_txt("Not_yet");
                            file_lines_to_save[3] = PreConnection.Codify_txt(DateTime.UtcNow.ToString());
                            label22.Text = "Pas encore";
                        }
                        //--------------------
                        int rest_finance_delay = 
                            (Finance_delay - (DateTime.Now - Properties.Settings.Default.First_Enter_Date_After_Install).TotalDays) > 0 ? 
                            Convert.ToInt32(Finance_delay - (DateTime.Now - Properties.Settings.Default.First_Enter_Date_After_Install).TotalDays)
                            : Finance_delay;
                        label22.Text += (label22.Text.Length > 0 ? "\n" : "")+ "(Veuillez nous contacter "+ (rest_finance_delay == 0 ? "immédiatement" : "avant " + rest_finance_delay + " jrs") + ")";
                        pictureBox5.Visible = false;
                        pictureBox6.Visible = true;

                    }
                    //------------
                    File.WriteAllLines(filePath, file_lines_to_save);
                    //---------
                    FileAttributes attributes = File.GetAttributes(filePath);
                    attributes |= FileAttributes.Hidden;
                    File.SetAttributes(filePath, attributes);
                   

                   
                    //////////////
                    ///
                    //////////////

                }
                else
                {
                    label13.Text = "30 jours";
                }

                //////////if (PreConnection.ReadFromRegistry("Déja_try_version") != "OUI")
                //////////{
                //////////    string strt_date = PreConnection.ReadFromRegistry("SoftVet_Start_Date");
                //////////    DateTime dt = DateTime.UtcNow;
                //////////    if (strt_date == "")
                //////////    {
                //////////        PreConnection.WriteIntoRegistry("SoftVet_Start_Date", dt.ToString("dd/MM/yyyy"));
                //////////    }
                //////////    else
                //////////    {
                //////////        DateTime.TryParse(strt_date, out dt);
                //////////    }

                //////////    int delay = 30 - (DateTime.UtcNow.Date - dt.Date).Days;
                //////////    rest_jrs_delay = (delay >= 0 ? delay : 0);
                //////////    label13.Text = (delay >= 0 ? delay : 0) + " Jours";
                //////////    if (delay <= 0)
                //////////    {
                //////////        PreConnection.Excut_Cmd(2, "tb_params", new System.Collections.Generic.List<string> { "VAL" }, new System.Collections.Generic.List<object> { 0 }, "ID = @P_ID", new System.Collections.Generic.List<string> { "P_ID" }, new System.Collections.Generic.List<object> { 7 });
                //////////        label18.ForeColor = label13.ForeColor = Color.Red;
                //////////    }
                //////////}
                //////////else
                //////////{
                //////////    PreConnection.Excut_Cmd(2, "tb_params", new System.Collections.Generic.List<string> { "VAL" }, new System.Collections.Generic.List<object> { 0 }, "ID = @P_ID", new System.Collections.Generic.List<string> { "P_ID" }, new System.Collections.Generic.List<object> { 7 });
                //////////    label13.Text = "0 Jours";
                //////////    label18.ForeColor = label13.ForeColor = Color.Red;
                //////////}


            }

        }

        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) { return false; }
            try
            {
                // Attempt to create a MailAddress instance with the email address
                MailAddress mailAddress = new MailAddress(email);

                // Check if the email address has a valid format
                if (mailAddress.Address == email)
                {
                    // Extract the domain from the email address
                    string domain = mailAddress.Host;

                    // Check if the domain has a valid MX record
                    using (var client = new System.Net.Mail.SmtpClient())
                    {
                        var mxRecords = Dns.GetHostEntry(domain).HostName;
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // An exception occurred, indicating the email is not valid
                return false;
            }

            // The email address is not valid
            return false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            label14.Visible = true;
            label14.Refresh();
            textBox1_Validating(null, null);
            textBox5.BackColor = string.IsNullOrWhiteSpace(textBox5.Text) ? Color.LightCoral : Color.White;
            textBox6.BackColor = string.IsNullOrWhiteSpace(textBox6.Text) ? Color.LightCoral : Color.White;
            if (label8.Text.Length == 29 && textBox1.BackColor != Color.LightCoral && textBox5.BackColor != Color.LightCoral && textBox6.BackColor != Color.LightCoral)
            {
                //-------------------
                if (PreConnection.IsInternetAvailable())
                {
                    string Albaitar_Email = "";
                    //---------
                    if (Main_Frm.Baitar_Server_Params.Rows.Count > 0)
                    {
                        Albaitar_Email = Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().Where(d => d["NME"].Equals("EMAIL_TO_RECIEVE_COMMANDS")).ToList().Count > 0 ? (string)Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().First(d => d["NME"].Equals("EMAIL_TO_RECIEVE_COMMANDS"))["VAL"] : "";
                    }
                    else {
                        //------------------------
                        DataTable dt = new DataTable();
                        MySqlCommand cmd = new MySqlCommand("SELECT VAL FROM SETTINGS WHERE NME = 'EMAIL_TO_RECIEVE_COMMANDS';", PreConnection.albaitar_online);
                        try
                        {
                            if (PreConnection.albaitar_online.State != ConnectionState.Open)
                            {
                                PreConnection.albaitar_online.Open();
                            }
                            MySqlDataReader reader = cmd.ExecuteReader();
                            dt.Load(reader);
                            PreConnection.albaitar_online.Close();
                            Albaitar_Email = dt.Rows[0][0].ToString();
                        }
                        catch { }                        
                    }
                    if (!Albaitar_Email.IsNullOrEmpty())
                    {
                        //-----------------------
                        MimeMessage Mssg = new MimeMessage();
                        Mssg.From.Add(new MailboxAddress("AlBaitar SoftVet", textBox1.Text));
                        Mssg.To.Add(MailboxAddress.Parse(Albaitar_Email));
                        Mssg.Subject = "ALBAITAR Softvet - Demande code d'activation";
                        Mssg.Body = new TextPart("plain")
                        {
                            Text = "======================================================\n" +
                            " -<< AlBaitar SoftVet >>-  [ " + Albaitar_Email + " ]\n" +
                            "======================================================\n\n\n" +
                            "Une demande d'activation de logiciel 'ALBAITAR Softvet' :\n\n" +
                            "1)- Veuillez confirmer les informations. \n" +
                            "2)- Si tous est bien, générer le code d'activation utilisant son ID envoyé. \n" +
                            "3)- Envoyer ce code d'activation (généré) par un message Email déstiné au : " + textBox1.Text + ".\n\n" +
                            "============================================\n\n" +
                            "Détails :\n" +
                            "-----------------\n" +
                            //"ID : " + label8.Text + "\n" +
                            "Email : " + textBox1.Text + "\n\n" +
                            "Nom Complet : " + textBox6.Text + "\n" +
                            "N° Tél : " + textBox5.Text + "\n" +
                            "Ordinateur : " + label9.Text + "\n" +
                            "Utilisateur (de PC) : " + label7.Text + "\n" +
                            "Tentative reste : " + label13.Text + "\n\n" +
                            "====================================="
                        };
                        bool good_sent = true;

                        MailKit.Net.Smtp.SmtpClient clnt = new MailKit.Net.Smtp.SmtpClient();
                        try
                        {
                            clnt.Connect("smtp.gmail.com", 465, true);
                            clnt.Authenticate("rancosoft@gmail.com", PreConnection.Traduct_Codified_txt(Properties.Settings.Default.RANCOSOFT_GMAIL_AUTHENT));
                            clnt.Send(Mssg);
                            MessageBox.Show("Votre demande a bien été envoyée, \nVous recevrez -prochainement- votre numéro d'activation par mail.\n\nMerci.", "Bien envoyé :", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        catch
                        {
                            good_sent = false;
                            MessageBox.Show("Il y a eu un problème, veuillez réessayer.", "Non éffectué :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            clnt.Disconnect(true);
                            clnt.Dispose();
                            //---------------------
                            if (good_sent)
                            {
                                MySqlCommand command = new MySqlCommand("INSERT INTO `ACT_COMMANDS`(`CLIENT_ID`,`CLIENT_MAIL`,`CLIENT_TEL`,`CLIENT_NME`,`FIRST_TENTATIVE`) VALUES (" +
                                //"'" + PreConnection.generate_ID_of_client().Replace("'", "''") + "'," + //ID
                                "NULL," + //ID
                                "'" + textBox1.Text.Replace("'", "''") + "'," + //MAIL
                                "'" + textBox5.Text.Replace("'", "''") + "'," + //TEL
                                "'" + textBox6.Text.Replace("'", "''") + "'," + //NME
                                rest_jrs_delay + ");", PreConnection.albaitar_online); //TENTATIVE
                                if (PreConnection.albaitar_online.State != ConnectionState.Open) { PreConnection.albaitar_online.Open(); }
                                try { command.ExecuteNonQuery(); } catch { }
                                PreConnection.albaitar_online.Close();
                                //----------------------------
                                Properties.Settings.Default.Codifed_Activation_Email = PreConnection.Codify_txt(textBox1.Text);
                                Properties.Settings.Default.Codified_Act_Clt_Tel = PreConnection.Codify_txt(textBox5.Text);
                                Properties.Settings.Default.Codified_Act_Clt_Nme = PreConnection.Codify_txt(textBox6.Text);
                                Properties.Settings.Default.Codified_Act_Command_dmnd_date = PreConnection.Codify_txt(DateTime.Now.ToString());
                                Properties.Settings.Default.Save();
                                label17.Visible = true;
                                label17.Text = "Demande envoyée le : " + DateTime.Now.ToString("dd/MM/yyyy");

                            }
                            //-------------------------------
                        }
                        //-----------
                    }
                    else {
                        MessageBox.Show(" - Veuillez connectez a l'internet puis continuez.\n\r - Vérifiez votre adresse Email.", "Pas de connection internet !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                   
                }
                else
                {
                    MessageBox.Show(" - Veuillez connectez a l'internet puis continuez.\n\r - Vérifiez votre adresse Email.", "Pas de connection internet !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Veuillez d'abord saisir tous les champs requis :\n-Email\n-N° Tél\n-Nom Complet", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            label14.Visible = false;

        }


        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            textBox1.BackColor = IsEmailValid(textBox1.Text) ? Color.White : Color.LightCoral;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool ready = true;
            if (textBox3.Text.Trim().Length == 0)
            {
                ready = false;
                textBox3.BackColor = Color.LightCoral;
                textBox3.Focus();
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Veuillez d'abord saisir tous les champs requis :\n-Email\n-N° Tél\n-Nom Complet", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ready = false;
            }
            if (ready)
            {
                //if (label8.Text.Length > 0)
                //{
                label11.Visible = true;
                label11.Refresh();
                //--------------------------

                //---------------------
                MySqlCommand command = new MySqlCommand("INSERT INTO `MOUVEMENTS` (`CLIENT_EMAIL`,`CLIENT_TEL`,`CLIENT_NME`,`ACTIVAT_CODE`) VALUES (" +
                    //"'" + PreConnection.generate_ID_of_client() + "'," + //CLIENT_ID
                    //"'" + label8.Text.Replace("'", "''") + "'," + //CLIENT_ID
                    "'" + textBox1.Text.Replace("'", "''") + "'," + //CLIENT_EMAL
                    "'" + textBox5.Text.Replace("'", "''") + "'," + //CLIENT_TEL
                    "'" + textBox6.Text.Replace("'", "''") + "'," + //CLIENT_NME
                    "'" + textBox3.Text.Replace("'", "''") + "');" + //ACTIVAT_CODE
                    " CALL InsertAndUpdateMOUVEMENTS('" + textBox1.Text.Replace("'", "''") + "', '" + textBox3.Text.Replace("'", "''") + "');", PreConnection.albaitar_online);

                //MySqlCommand command = new MySqlCommand("INSERT INTO `MOUVEMENTS` (`CLIENT_ID`,`CLIENT_EMAIL`,`CLIENT_TEL`,`CLIENT_NME`,`ACTIVAT_CODE`) SELECT " +
                //    "(SELECT CLIENT_EMAIL FROM SERIALS_HISTORIQUE WHERE CLIENT_EMAIL = '" + textBox1.Text.Replace("'", "''") + "' AND ACTIVATION_SERIAL = '" + textBox3.Text.Replace("'", "''") + "' LIMIT 1)," + //CLIENT_ID
                //    "'" + textBox1.Text.Replace("'", "''") + "'," + //CLIENT_EMAL
                //    "'" + textBox5.Text.Replace("'", "''") + "'," + //CLIENT_TEL
                //    "'" + textBox6.Text.Replace("'", "''") + "'," + //CLIENT_NME
                //    "'" + textBox3.Text.Replace("'", "''") + "');" + //ACTIVAT_CODE
                //    " FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM SERIALS_HISTORIQUE WHERE CLIENT_EMAIL = '" + textBox1.Text.Replace("'", "''") + "' AND ACTIVATION_SERIAL = '" + textBox3.Text.Replace("'", "''") + "');" +
                //    " CALL InsertAndUpdateMOUVEMENTS('" + textBox1.Text.Replace("'", "''") + "', '" + textBox3.Text.Replace("'", "''") + "');", albaitar_online);
                if (PreConnection.albaitar_online.State != ConnectionState.Open) { PreConnection.albaitar_online.Open(); }
                try { command.ExecuteNonQuery(); } catch { }
                PreConnection.albaitar_online.Close();
                //-------------------------------
                // string MachineCltID = Properties.Settings.Default.MachineClientID != null ? PreConnection.Traduct_Codified_txt(Properties.Settings.Default.MachineClientID) : "";
                //----------
                if (Check_activation(textBox3.Text.Replace("'", "''"), textBox1.Text.Replace("'", "''"))) //Good
                {

                    try { Main_Frm.text_to_add_to_title = "make_title_activ_state_updat";
                    
                    } catch { }
                    //--------
                    MessageBox.Show("Produit bien Activé !\n\n  ** Bienvenue avec AL BAITAR SoftVet **\n\n", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    label6.Text = textBox3.Text.Substring(0, 3) + "***************" + textBox3.Text.Substring(22, 2);
                    Properties.Settings.Default.Codifed_Activation_Email = PreConnection.Codify_txt(textBox1.Text);
                    Properties.Settings.Default.Codified_Activate_Code = PreConnection.Codify_txt(textBox3.Text);
                    Properties.Settings.Default.Codified_Act_Clt_Tel = PreConnection.Codify_txt(textBox5.Text);
                    Properties.Settings.Default.Codified_Act_Clt_Nme = PreConnection.Codify_txt(textBox6.Text);
                    Properties.Settings.Default.MachineClientID = PreConnection.Codify_txt(PreConnection.generate_ID_of_client());
                    //---------
                    bool tt = Properties.Settings.Default.Codified_Act_Command_dmnd_date == null;
                    tt = (tt ? true : Properties.Settings.Default.Codified_Act_Command_dmnd_date.IsNullOrEmpty());
                    if (tt)
                    {
                        Properties.Settings.Default.Codified_Act_Command_dmnd_date = PreConnection.Codify_txt(DateTime.Now.ToString());
                    }
                    //----------
                    Properties.Settings.Default.Save();
                    panel4.Visible = true;
                    panel2.Visible = false;
                    pictureBox1.Image = Properties.Resources.icons8_safe_ok_120px;
                    textBox2.Visible = pictureBox3.Visible = panel3.Visible = false;
                    this.Size = new System.Drawing.Size(this.Width, this.Height - panel3.Height);
                    PreConnection.Excut_Cmd(2, "tb_params", new System.Collections.Generic.List<string> { "VAL" }, new System.Collections.Generic.List<object> { 1 }, "ID = @P_ID", new System.Collections.Generic.List<string> { "P_ID" }, new System.Collections.Generic.List<object> { 7 });
                }
                else
                {
                    MessageBox.Show("Le code n'est pas valide ou éxpiré !", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox3.Focus();
                    textBox3.SelectAll();
                }
                //-----------------------------//Check Finance
                string finance_stat = PreConnection.verify_baitar_client_finance(textBox1.Text, textBox3.Text);

                string folderPath = "C:\\ProgramData\\BAITAR_CTRL";
                string filePath = folderPath + "\\Al_Baitar_Activation.txt";

                if (!File.Exists(filePath))
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                        File.SetAttributes(folderPath, FileAttributes.Directory | FileAttributes.Hidden);
                    }
                    //--------
                    File.Create(filePath).Dispose();
                    //---------
                    FileAttributes attributes = File.GetAttributes(filePath);
                    attributes |= FileAttributes.Hidden;
                    File.SetAttributes(filePath, attributes);
                }

                if (File.Exists(filePath))
                {
                    //---------
                    FileAttributes currentAttributes = File.GetAttributes(filePath);
                    FileAttributes updatedAttributes = currentAttributes & ~FileAttributes.Hidden;
                    File.SetAttributes(filePath, updatedAttributes);
                    //---------

                    string[] file_lines = File.ReadAllLines(filePath);
                    string[] file_lines_to_save = new string[4];

                    if (file_lines.Length > 0)
                    {
                        file_lines_to_save[0] = file_lines[0];
                    }

                    if (file_lines.Length > 1)
                    {
                        file_lines_to_save[1] = file_lines[1];
                    }

                    
                    if (finance_stat == "good")
                    {
                        file_lines_to_save[2] = PreConnection.Codify_txt("Yes_is_done");
                        file_lines_to_save[3] = PreConnection.Codify_txt(DateTime.UtcNow.ToString());
                        pictureBox5.Visible = true;
                        pictureBox6.Visible = false;
                        label22.Text = "Oui";
                        label22.ForeColor = Color.Green;
                    }
                    else
                    {
                        int Finance_delay = int.Parse(Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().Where(d => d["NME"].Equals("VERIF_DAYS_DELAY_FOR_NN_PAYED")).ToList().Count > 0 ? (string)Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().First(d => d["NME"].Equals("VERIF_DAYS_DELAY_FOR_NN_PAYED"))["VAL"] : "30");
                        label22.ForeColor = Color.Red;
                        if (finance_stat == "not good")
                        {
                            file_lines_to_save[2] = PreConnection.Codify_txt("Not_yet");
                            file_lines_to_save[3] = PreConnection.Codify_txt(DateTime.UtcNow.ToString());
                            label22.Text = "Pas encore";
                        }
                       
                        int rest_finance_delay =
                           (Finance_delay - (DateTime.Now - Properties.Settings.Default.First_Enter_Date_After_Install).TotalDays) > 0 ?
                           Convert.ToInt32(Finance_delay - (DateTime.Now - Properties.Settings.Default.First_Enter_Date_After_Install).TotalDays)
                           : Finance_delay;
                        label22.Text += (label22.Text.Length > 0 ? "\n" : "") + "(Veuillez nous contacter " + (rest_finance_delay == 0 ? "immédiatement" : "avant " + rest_finance_delay + " jrs") + ")";
                        pictureBox5.Visible = false;
                        pictureBox6.Visible = true;
                 
                    }
                    //------------
                    File.WriteAllLines(filePath, file_lines_to_save);
                    //---------
                    FileAttributes attributes = File.GetAttributes(filePath);
                    attributes |= FileAttributes.Hidden;
                    File.SetAttributes(filePath, attributes);
                }






                //DataTable tbbl = new DataTable();
                //MySqlCommand command3 = new MySqlCommand("SELECT * FROM `SERIALS_HISTORIQUE` WHERE `CLIENT_EMAIL` = '" + textBox1.Text.Replace("'", "''") + "' AND `ACTIVATION_SERIAL` = '" + textBox3.Text.Replace("'", "''") + "';", PreConnection.albaitar_online);
                //try
                //{
                //    if (PreConnection.albaitar_online.State != ConnectionState.Open) { PreConnection.albaitar_online.Open(); }
                //    using (MySqlDataReader reader2 = command3.ExecuteReader())
                //    {
                //        tbbl.Load(reader2);
                //    }
                //}
                //catch { Main_Frm.activation_verified_corretly_with_server = false; }
                //PreConnection.albaitar_online.Close();


                //if (tbbl.Rows.Count > 0)
                //{
                //    string folderPath = "C:\\ProgramData\\BAITAR_CTRL";
                //    string filePath = folderPath + "\\Al_Baitar_Activation.txt";

                //    int ee = 0;
                //    int.TryParse(tbbl.Rows[0]["IS_FINANCE_DONE"] != null ? tbbl.Rows[0]["IS_FINANCE_DONE"].ToString() : "0", out ee);

                //    if (!File.Exists(filePath))
                //    {
                //        if (!Directory.Exists(folderPath))
                //        {
                //            Directory.CreateDirectory(folderPath);
                //            File.SetAttributes(folderPath, FileAttributes.Directory | FileAttributes.Hidden);
                //        }
                //        //--------
                //        File.Create(filePath).Dispose();
                //        //---------
                //        FileAttributes attributes = File.GetAttributes(filePath);
                //        attributes |= FileAttributes.Hidden;
                //        File.SetAttributes(filePath, attributes);
                //    }

                //    if (File.Exists(filePath))
                //    {
                //        //---------
                //        FileAttributes currentAttributes = File.GetAttributes(filePath);
                //        FileAttributes updatedAttributes = currentAttributes & ~FileAttributes.Hidden;
                //        File.SetAttributes(filePath, updatedAttributes);
                //        //---------

                //        string[] file_lines = File.ReadAllLines(filePath);
                //        string[] file_lines_to_save = new string[4];

                //        if (file_lines.Length > 0)
                //        {
                //            file_lines_to_save[0] = file_lines[0];
                //        }

                //        if (file_lines.Length > 1)
                //        {
                //            file_lines_to_save[1] = file_lines[1];
                //        }

                //        file_lines_to_save[3] = PreConnection.Codify_txt(DateTime.UtcNow.ToString());
                //        //------------
                //        File.WriteAllLines(filePath, file_lines_to_save);
                //        //---------
                //        FileAttributes attributes = File.GetAttributes(filePath);
                //        attributes |= FileAttributes.Hidden;
                //        File.SetAttributes(filePath, attributes);


                //        if (ee == 1)
                //        {
                //            file_lines_to_save[2] = PreConnection.Codify_txt("Yes_is_done");
                //            pictureBox5.Visible = true;
                //            pictureBox6.Visible = false;
                //            label22.Text = "Oui";
                //            label22.ForeColor = Color.Green;
                //        }
                //        else
                //        {
                //            int Finance_delay = int.Parse(Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().Where(d => d["NME"].Equals("VERIF_DAYS_DELAY_FOR_NN_PAYED")).ToList().Count > 0 ? (string)Main_Frm.Baitar_Server_Params.Rows.Cast<DataRow>().First(d => d["NME"].Equals("VERIF_DAYS_DELAY_FOR_NN_PAYED"))["VAL"] : "3");
                //            label22.ForeColor = Color.Red;
                //            if (panel4.Visible)
                //            {
                //                label22.Text = "(Veuillez nous contacter avant " + Finance_delay + " jrs)";
                //            }
                //            else
                //            {
                //                label22.Text = "Pas encore";
                //            }

                //            file_lines_to_save[2] = PreConnection.Codify_txt("Not_yet");
                //            pictureBox5.Visible = false;
                //            pictureBox6.Visible = true;
                //        }
                //    }
                //}
                //-----------------------------
                label11.Visible = false;
                label11.Refresh();
                //------------------------------

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.White;
        }




        private void App_Activation_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (panel4.Visible)
            {
                Application.Restart();
            }
            else
            {
                Application.Exit();
            }
        }

        private void panel2_VisibleChanged(object sender, EventArgs e)
        {
            button2.Visible = textBox4.Visible = panel2.Visible;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Connection_Str().ShowDialog();
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label8.Text);
            button2.Image = Properties.Resources.icons8_Checkmark_20px;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox5.BackColor = Color.White;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox6.BackColor = Color.White;
        }

        private bool Check_activation(string code, string email)
        {
            MySqlCommand command2 = new MySqlCommand("SELECT * FROM `MOUVEMENTS` WHERE `CLIENT_EMAIL` = '" + email + "' AND `ACTIVAT_CODE` = '" + code + "';", PreConnection.albaitar_online);
            DataTable dttb = new DataTable();
            try
            {
                if (PreConnection.albaitar_online.State != ConnectionState.Open) { PreConnection.albaitar_online.Open(); }
                using (MySqlDataReader reader = command2.ExecuteReader())
                {
                    dttb.Load(reader);
                }
                PreConnection.albaitar_online.Close();
            }
            catch (Exception ex)
            {

            }


            if (dttb.Rows.Count > 0)
            {
                //---------                
                Properties.Settings.Default.Codified_Act_Client_ID = PreConnection.Codify_txt(dttb.Rows[0]["CLIENT_ID"] != DBNull.Value ? (string)dttb.Rows[0]["CLIENT_ID"] : "");
                Properties.Settings.Default.Save();
                //------

            }
            return dttb.Rows.Count > 0;
        }

       
    }



}
