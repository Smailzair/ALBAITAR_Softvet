using ALBAITAR_Softvet.Dialogs;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Login : Form
    {
        static bool accept = false;
        static bool change_infos_there = false;
        bool just_return_answer1 = false;
        int specified_usr_id = -1;
        
        public bool main_frm_locked = false;
        public static bool enter_allow
        {
            get
            {
                return accept;
            }
            set
            {
                enter_allow = value;
            }
        }

        public static bool change_infos_other_usr
        {
            get
            {
                return change_infos_there;
            }
            set
            {
                change_infos_other_usr = value;
            }
        }

        public Login(bool? just_return_answer, int? specified_user_id)
        {
            InitializeComponent();
            just_return_answer1 = just_return_answer ?? false;
            specified_usr_id = specified_user_id ?? -1;
            maskedTextBox1.Focus();
        }
        DataTable datat;
        private void Login_Load(object sender, EventArgs e)
        {
            accept = false;
            //------------
            datat = PreConnection.Load_data("SELECT * ,CONCAT(IF(SEX = 'F','Mme. ','Mr. '),`USER_NME`,' ',`USER_FAMNME`) AS USER_FULL_NME FROM tb_login_and_users;");
            //-----------------------
            comboBox1.DataSource = datat;
            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "USER_FULL_NME";
            if (specified_usr_id > 0)
            {
                if (datat.Rows.Cast<DataRow>().Where(er => er["ID"].ToString() == specified_usr_id.ToString()).Count() > 0)
                {
                    comboBox1.SelectedValue = specified_usr_id;
                }
                comboBox1.Enabled = false;
            }
            else if (datat.Rows.Cast<DataRow>().Where(er => er["ID"].ToString() == Properties.Settings.Default.Last_login_user_idx.ToString()).Count() > 0)
            {
                comboBox1.SelectedValue = Properties.Settings.Default.Last_login_user_idx;
            }
            checkBox1.Checked = Properties.Settings.Default.Login_Auto_Enter;
            checkBox1.Visible = !just_return_answer1 && comboBox1.Enabled;
            //-----------------------
            comboBox1.Enabled = !main_frm_locked;
            maskedTextBox1.Select();
            //-----
            BringToFront();
            Activate();

        }

        private void maskedTextBox1_Validating(object sender, CancelEventArgs e)
        {
            button1.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (datat.Rows.Cast<DataRow>().Where(rr => int.Parse(rr["ID"].ToString()) == int.Parse(comboBox1.SelectedValue.ToString()) && (rr["PASSWORD"].Equals(maskedTextBox1.Text) || (rr.IsNull("PASSWORD") && maskedTextBox1.Text.Length == 0))).Count() > 0)
            {
                accept = true;
                //-----------------------------
                change_infos_there = int.Parse(comboBox1.SelectedValue.ToString()) != Properties.Settings.Default.Last_login_user_idx;
                Properties.Settings.Default.Last_login_user_idx = int.Parse(comboBox1.SelectedValue.ToString());
                Properties.Settings.Default.Last_login_user_full_nme = comboBox1.Text;
                Properties.Settings.Default.Last_login_is_admin = datat.Rows.Cast<DataRow>().Where(rr => int.Parse(rr["ID"].ToString()) == int.Parse(comboBox1.SelectedValue.ToString()) && (SByte)rr["IS_ADMIN"] == 1).Count() > 0;
                if (checkBox1.Visible)
                {
                    Properties.Settings.Default.Login_Auto_Enter = checkBox1.Checked;
                    if (checkBox1.Checked)
                    {
                        Properties.Settings.Default.Last_entred_date_by_Auto_Enter = DateTime.Now;
                    }
                }
                Properties.Settings.Default.Save();
                Close();
            }
            else
            {
                maskedTextBox1.BackColor = Color.LightCoral;
                maskedTextBox1.Focus();
                maskedTextBox1.SelectAll();
                linkLabel1.Visible = true;
                //-----------
                accept = false;
            }
            //--------------
            if (just_return_answer1)
            {
                Close();
            }
            //----------
            if (accept)
            {
                Application.OpenForms["Main_Frm"]?.Show();
            }
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            maskedTextBox1.BackColor = SystemColors.Window;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new Login_Pass_Forgot(comboBox1.SelectedValue.ToString())).ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            maskedTextBox1.Focus();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(main_frm_locked && !accept)
            {
                Application.OpenForms["Main_Frm"]?.Close();
                Application.Exit();
            }
        }
    }
}
