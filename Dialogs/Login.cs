using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Login : Form
    {
        static bool accept = false;
        static bool change_infos_there = false;
        bool just_return_answer1 = false;
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

        public Login(bool? just_return_answer)
        {
            InitializeComponent();
            just_return_answer1  = just_return_answer ?? false;
        }
        DataTable datat;
        private void Login_Load(object sender, EventArgs e)
        {
            datat = PreConnection.Load_data("SELECT * ,CONCAT(IF(SEX = 'F','Mme. ','Mr. '),`USER_NME`,' ',`USER_FAMNME`) AS USER_FULL_NME FROM tb_login_and_users;");
            //-----------------------
            comboBox1.DataSource = datat;
            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "USER_FULL_NME";
            if(datat.Rows.Cast<DataRow>().Where(er => er["ID"].ToString() == Properties.Settings.Default.Last_login_user_idx.ToString()).Count() > 0)
            {
                comboBox1.SelectedValue = Properties.Settings.Default.Last_login_user_idx;
            }
            
            //-----------------------            
            maskedTextBox1.Select();
        }

        private void maskedTextBox1_Validating(object sender, CancelEventArgs e)
        {
            button1.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (datat.Rows.Cast<DataRow>().Where(rr => int.Parse(rr["ID"].ToString()) == int.Parse(comboBox1.SelectedValue.ToString()) && (rr["PASSWORD"].Equals(maskedTextBox1.Text) || (rr.IsNull("PASSWORD") && maskedTextBox1.Text.Length == 0))).Count() > 0)
            //if (datat.Rows.Cast<DataRow>().AsEnumerable().Select(rr => int.Parse(rr["ID"].ToString()) == int.Parse(comboBox1.SelectedValue.ToString()) && (rr["PASS"].Equals(maskedTextBox1.Text) || (rr.IsNull("PASS") && maskedTextBox1.Text.Length == 0))).First())
            {
                accept = true;
                //-----------------------------
                change_infos_there = int.Parse(comboBox1.SelectedValue.ToString()) != Properties.Settings.Default.Last_login_user_idx;
                Properties.Settings.Default.Last_login_user_idx = int.Parse(comboBox1.SelectedValue.ToString());
                Properties.Settings.Default.Last_login_user_full_nme = comboBox1.Text;
                Properties.Settings.Default.Last_login_is_admin = datat.Rows.Cast<DataRow>().Where(rr => int.Parse(rr["ID"].ToString()) == int.Parse(comboBox1.SelectedValue.ToString()) && (SByte)rr["IS_ADMIN"] == 1).Count() > 0;
                Properties.Settings.Default.Save();
                //------------------                
                Close();
            }
            else
            {
                maskedTextBox1.BackColor = Color.LightCoral;
                maskedTextBox1.SelectAll();
                linkLabel1.Visible = true;
                //-----------
                accept = false;
            }
            //--------------
            if (just_return_answer1) { Close(); }

        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            
            maskedTextBox1.BackColor = SystemColors.Window;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new Login_Pass_Forgot(comboBox1.SelectedValue.ToString())).ShowDialog();
        }


    }
}
