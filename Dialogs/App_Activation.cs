using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using System.Net.NetworkInformation;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;
using System.Diagnostics;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace ALBAITAR_Softvet.Dialogs
{

    public partial class App_Activation : Form
    {
        

        public App_Activation()
        {
            InitializeComponent();
            //---------------------------
        }

        private void Connection_Str_Load(object sender, EventArgs e)
        {            
            //--------------------
            label8.Text = PreConnection.generate_ID_of_client();
            label9.Text = Environment.MachineName;
            label7.Text = Environment.UserName;
            //-----------------------
            string codd = PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Activate_Code);
            if (PreConnection.Verif_Activation_SOftVet(codd))
            {
                label6.Text = codd.Substring(0, 3) + "***************" + codd.Substring(23, 2);
                panel4.Visible = true;
                panel2.Visible = false;
                pictureBox1.Image = Properties.Resources.icons8_safe_ok_120px;
                textBox2.Visible = pictureBox3.Visible = panel3.Visible = false;
                this.Size = new System.Drawing.Size(this.Width, this.Height - panel3.Height);
            }
            else
            {
                if(PreConnection.ReadFromRegistry("Déja_try_version") != "OUI")
                {
                    string strt_date = PreConnection.ReadFromRegistry("SoftVet_Start_Date");
                    DateTime dt = DateTime.UtcNow;
                    if (strt_date == "")
                    {
                        PreConnection.WriteIntoRegistry("SoftVet_Start_Date", dt.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        DateTime.TryParse(strt_date, out dt);
                    }

                    int delay = 30 - (DateTime.UtcNow.Date - dt.Date).Days;
                    label13.Text = (delay >= 0 ? delay : 0) + " Jours";
                    if (delay <= 0)
                    {
                        PreConnection.Excut_Cmd("UPDATE tb_params SET `VAL` = 0 WHERE `ID` = 7;");
                        label18.ForeColor = label13.ForeColor = Color.Red;
                    }
                }
                else
                {
                    PreConnection.Excut_Cmd("UPDATE tb_params SET `VAL` = 0 WHERE `ID` = 7;");
                    label13.Text = "0 Jours";
                    label18.ForeColor = label13.ForeColor = Color.Red;
                }
                
                
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
            textBox1_Validating(null, null);
            if (label8.Text.Length == 29 && textBox1.BackColor != Color.LightCoral)
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    MimeMessage Mssg = new MimeMessage();
                    Mssg.From.Add(new MailboxAddress("AlBaitar SoftVet", "rancosoft@gmail.com"));
                    Mssg.To.Add(MailboxAddress.Parse(textBox1.Text));
                    Mssg.Subject = "ALBAITAR Softvet - Demande code d'activation";
                    Mssg.Body = new TextPart("plain")
                    {
                        Text = "======================================================\n" +
                " -<< AlBaitar SoftVet >>-  [ albaitar.technologie@gmail.com ]\n" +
                "======================================================\n\n\n" + 
                "Une demande d'activation de logiciel 'ALBAITAR Softvet' :\n\n" +
                        "1)- Veuillez confirmer les informations. \n" +
                        "2)- Si tous est bien, générer le code d'activation utilisant son ID envoyé. \n" +
                        "3)- Envoyer ce code d'activation (généré) par un message Email déstiné au : " + textBox1.Text + ".\n\n" +
                        "============================================\n\n" +
                        "Détails :\n" +
                        "-----------------\n" +
                        "ID : " + label8.Text + "\n" + 
                        "Ordinateur : " + label9.Text + "\n" +
                        "Utilisateur (de PC) : " + label7.Text + "\n" +
                        "Tentative reste : " +label13.Text+"\n\n" +
                        "====================================="
                    };


                    MailKit.Net.Smtp.SmtpClient clnt = new MailKit.Net.Smtp.SmtpClient();
                    try
                    {
                        clnt.Connect("smtp.gmail.com", 465, true);
                        clnt.Authenticate("rancosoft@gmail.com", PreConnection.Traduct_Codified_txt(Properties.Settings.Default.RANCOSOFT_GMAIL_AUTHENT));
                        clnt.Send(Mssg);
                        MessageBox.Show("Votre demande a bien été envoyée, \nVous recevrez -prochainement- votre numéro d'activation par mail.\n\nMerci.", "Bien envoyé :", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Il y a eu un problème, veuillez réessayer.", "Non éffectué :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        clnt.Disconnect(true);
                        clnt.Dispose();
                    }
                    //-----------
                }
                else
                {
                    MessageBox.Show(" - Veuillez connectez a l'internet puis continuez.\n\r - Vérifiez votre adresse Email.", "Pas de connection internet !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                textBox1.Focus();
                textBox1.SelectAll();
            }


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
            if(textBox3.Text.Trim().Length == 0)
            {
                ready = false;
                textBox3.BackColor = Color.LightCoral;
                textBox3.Focus();
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("L'adresse Email est requis !","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                ready = false;
                textBox1.BackColor = Color.LightCoral;
                textBox1.Focus();
            }
            if(ready)            
            {
                if (label8.Text.Length == 29)
                {
                    label11.Visible = true;
                    label11.Refresh();
                    //--------------------------
                    bool Verifyed_001 = false;
                    MySqlConnection albaitar_online = new MySqlConnection(@"Server=instances.spawn.cc;Port=31681;Database=ALBAITAR_SOFTVET;Uid=root;Pwd=kOluo0PgmDVowykt;");
                    //---------------------
                    MySqlCommand command = new MySqlCommand("INSERT INTO `MOUVEMENTS` (`CLIENT_ID`,`CLIENT_EMAIL`,`ACTIVAT_CODE`) VALUES (" +
                        "'" + PreConnection.generate_ID_of_client() + "'," +
                        "'" + textBox1.Text + "'," +
                        "'" + textBox3.Text + "');", albaitar_online);
                    if (albaitar_online.State != ConnectionState.Open) { albaitar_online.Open(); }
                    try { command.ExecuteNonQuery(); } catch { }
                    albaitar_online.Close();
                    //-------------------------------
                    DataTable dttb = new DataTable();
                    MySqlCommand command2 = new MySqlCommand("SELECT * FROM `MOUVEMENTS` WHERE `CLIENT_EMAIL` = '"+ textBox1.Text + "' AND `ACTIVAT_CODE` = '" + textBox3.Text + "';", albaitar_online);
                    if (albaitar_online.State != ConnectionState.Open) { albaitar_online.Open(); }
                    using (MySqlDataReader reader = command2.ExecuteReader())
                    {
                        dttb.Load(reader);
                    }
                    albaitar_online.Close();
                    if (dttb.Rows.Count > 0) //Good
                    {
                        Verifyed_001 = true;
                        PreConnection.WriteIntoRegistry("Déja_try_version", "OUI");
                    }
                    else //Not activated
                    {

                    }
                    if (Verifyed_001 && PreConnection.Verif_Activation_SOftVet(textBox3.Text))
                    {
                        MessageBox.Show("Produit bien Activé !\n\n  ** Bienvenue avec AL BAITAR SoftVet **\n\n","",MessageBoxButtons.OK,MessageBoxIcon.Information);                        
                        label6.Text = textBox3.Text.Substring(0, 3) + "***************" + textBox3.Text.Substring(22, 2);
                        Properties.Settings.Default.Codifed_Activation_Email = PreConnection.Codify_txt(textBox1.Text);
                        Properties.Settings.Default.Codified_Activate_Code = PreConnection.Codify_txt(textBox3.Text);
                        Properties.Settings.Default.Save();
                        panel4.Visible = true;
                        panel2.Visible = false;
                        pictureBox1.Image = Properties.Resources.icons8_safe_ok_120px;
                        textBox2.Visible = pictureBox3.Visible = panel3.Visible = false;
                        this.Size = new System.Drawing.Size(this.Width, this.Height - panel3.Height);
                        PreConnection.Excut_Cmd("UPDATE tb_params SET `VAL` = 1 WHERE `ID` = 7;");

                    }
                    else
                    {
                        MessageBox.Show("Le code n'est pas valide ou éxpiré !", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox3.Focus();
                        textBox3.SelectAll();
                    }
                    label11.Visible = false;
                }
                else
                {
                    MessageBox.Show("Votre code ID n'est pas valide !", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
    }



}
