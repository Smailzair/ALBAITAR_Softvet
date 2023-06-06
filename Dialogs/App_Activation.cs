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
                
                string  strt_date = PreConnection.ReadFromRegistry("SoftVet_Start_Date");
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
                label13.Text = (delay >=0 ? delay : 0) + " Jours";
                label18.ForeColor = label13.ForeColor = delay <= 0 ? Color.Red : label18.ForeColor;
            }
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length > 0 && textBox1.BackColor != Color.LightCoral && label8.Text.Length == 29)
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    MimeMessage Mssg = new MimeMessage();
                    Mssg.From.Add(new MailboxAddress("RancoSoft", "rancosoft@gmail.com"));
                    Mssg.To.Add(MailboxAddress.Parse("smailbahmida@yahoo.fr"));
                    Mssg.Subject = "ALBAITAR Softvet - Demande code d'activation";
                    Mssg.Body = new TextPart("plain")
                    {
                        Text = "Une demande d'activation de logiciel 'ALBAITAR Softvet' :\n\n" +
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


                    SmtpClient clnt = new SmtpClient();
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
            if (textBox1.Text.Length > 0)
            {
                bool good = false;
                try
                {
                    var addr = new System.Net.Mail.MailAddress(textBox1.Text);
                    good = addr.Address == textBox1.Text;
                }
                catch
                {
                    good = false;
                }

                if (!good)
                {
                    e.Cancel = true;
                    textBox1.BackColor = Color.LightCoral;
                    textBox1.Focus();
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox3.Text.Trim().Length == 0)
            {
                textBox3.BackColor = Color.LightCoral;
                textBox3.Focus();
            }
            else
            {
                if (label8.Text.Length == 29)
                {
                    label11.Visible = true;
                    label11.Refresh();
                    //--------------------------
                    if(PreConnection.Verif_Activation_SOftVet(textBox3.Text))
                    {
                        MessageBox.Show("Produit bien Activé !\n\n  ** Bienvenue avec AL BAITAR SoftVet **\n\n","",MessageBoxButtons.OK,MessageBoxIcon.Information);

                        label6.Text = textBox3.Text.Substring(0, 3) + "***************" + textBox3.Text.Substring(22, 2);
                        Properties.Settings.Default.Codified_Activate_Code = PreConnection.Codify_txt(textBox3.Text);
                        Properties.Settings.Default.Save();
                        panel4.Visible = true;
                        panel2.Visible = false;
                        pictureBox1.Image = Properties.Resources.icons8_safe_ok_120px;
                        textBox2.Visible = pictureBox3.Visible = panel3.Visible = false;
                        this.Size = new System.Drawing.Size(this.Width, this.Height - panel3.Height);
                    }
                    else
                    {
                        MessageBox.Show("Le code n'est pas valide !", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
