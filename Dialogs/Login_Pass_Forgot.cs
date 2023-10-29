using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Login_Pass_Forgot : Form
    {
        string Usr_ID;
        public Login_Pass_Forgot(string User_ID)
        {
            InitializeComponent();
            Usr_ID = User_ID;
        }
        DataTable datatt;
        private void Login_Load(object sender, EventArgs e)
        {
            datatt = PreConnection.Load_data("SELECT * FROM tb_login_and_users WHERE ID = " + Usr_ID + ";");
            //-----------------------
            if ((datatt.Rows[0]["QUESTION"].ToString() ?? "").Length > 0)
            {
                label3.Text = (string)datatt.Rows[0]["QUESTION"]; //Question
            }
            else
            {
                groupBox1.Enabled = false;
            }
            //---------------------------------
            PreConnection.load_rancosoft_gmail_auth();
            if (datatt.Rows[0]["EMAIL"] != DBNull.Value && datatt.Rows[0]["EMAIL"].ToString().Length > 0 && Properties.Settings.Default.RANCOSOFT_GMAIL_AUTHENT.Length > 0)
            {
                groupBox2.Enabled = true;
                label5.Text = (string)datatt.Rows[0]["EMAIL"];
            }
            else
            {
                groupBox2.Enabled = false;
            }
            //--------------------------
            if (!groupBox1.Enabled && !groupBox2.Enabled)
            {
                MessageBox.Show("Vous n'avez aucune mot de passe à récupirer (Vide)");
                Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ((datatt.Rows[0]["ANSWER"].ToString() ?? "") == textBox1.Text)
            {
                MessageBox.Show("Votre mot de passe est :\n\r" + ((datatt.Rows[0]["PASSWORD"].ToString() ?? "").Length > 0 ? (datatt.Rows[0]["PASSWORD"].ToString() ?? "") : "'Vide !'"), "--", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                textBox1.SelectAll();
                textBox1.BackColor = Color.LightCoral;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
        }

        private void Login_Pass_Forgot_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MimeMessage Mssg = new MimeMessage();
                Mssg.From.Add(new MailboxAddress("RancoSoft", "rancosoft@gmail.com"));
                Mssg.To.Add(MailboxAddress.Parse(datatt.Rows[0]["EMAIL"].ToString()));
                Mssg.Subject = "ALBAITAR Softvet - Récuperation de mot de passe";
                Mssg.Body = new TextPart("plain")
                {
                    Text = @"Bonjour " + (datatt.Rows[0]["SEX"].ToString() == "M" ? "Mr." : "Mlle.") + datatt.Rows[0]["USER_NME"].ToString() + " " + datatt.Rows[0]["USER_FAMNME"].ToString() + @",
Voici votre mot de passe de logiciel '" + Application.ProductName.ToString() + "' : " + ((datatt.Rows[0]["PASSWORD"].ToString() ?? "").Length > 0 ? (datatt.Rows[0]["PASSWORD"].ToString() ?? "") : "'Vide !'")
                };


                SmtpClient clnt = new SmtpClient();
                try
                {
                    clnt.Connect("smtp.gmail.com", 465, true);
                    clnt.Authenticate("rancosoft@gmail.com", PreConnection.Traduct_Codified_txt(Properties.Settings.Default.RANCOSOFT_GMAIL_AUTHENT));
                    clnt.Send(Mssg);
                    MessageBox.Show("Veuillez consultez votre Email, pour trouver votre mot de passe.", "Bien envoyé :", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    clnt.Disconnect(true);
                    clnt.Dispose();
                }
                //-----------
                Close();
            }
            else
            {
                MessageBox.Show(" - Veuillez connectez a l'internet puis continuez.\n\r - Vérifiez votre adresse Email.", "Pas de connection internet !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button4.PerformClick();
            }
        }

    }
}
