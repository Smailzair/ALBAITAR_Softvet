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
    public partial class Add_Ordonnance_Item : Form
    {
        public Add_Ordonnance_Item()
        {
            InitializeComponent();
            //---------
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void formatting()
        {
            string output = "";
            output += textBox1.Text + "\n";
            //-----------
            output += (numericUpDown1.Value % 1 == 0 ? (int)numericUpDown1.Value : numericUpDown1.Value) + " " + comboBox1.Text + " ";
            //---------------
            output += "(";
            bool use_tiret = false;
            if (radioButton1.Checked)
            {
                output += "Toujours ";
                use_tiret = true;
            }
            else if (radioButton2.Checked)
            {
                output += "Chaque " + numericUpDown2.Value + " jours ";
                use_tiret = true;
            }
            else
            {
                output += textBox2.Text;
                use_tiret = !string.IsNullOrWhiteSpace(textBox2.Text);
            }
            //--------------
            if (radioButton6.Checked)
            {
                output += use_tiret ? " - " : "";
                output += dateTimePicker1.Value.ToString("HH:mm");
            }
            else if (radioButton5.Checked)
            {
                output += use_tiret ? " - " : "";
                output += "Matin";
            }
            else if (radioButton10.Checked)
            {
                output += use_tiret ? " - " : "";
                output += "Soir";
            }
            else if (radioButton11.Checked)
            {
                output += use_tiret ? " - " : "";
                output += "Matin et Soir";
            }
            else
            {
                output += use_tiret && !string.IsNullOrWhiteSpace(textBox3.Text)? " - " : "";
                output += textBox3.Text;
            }
            output += ")\n";
            //------------
            if (radioButton8.Checked)
            {
                output += numericUpDown4.Value + " " + comboBox2.Text;
            }
            else
            {
                output += textBox4.Text;
            }
            //------------
            output = output.Replace("()", "");
            label4.Text = output;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
            formatting();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            formatting();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            formatting();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(((RadioButton)sender).Checked)
            {
                formatting();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim().Length > 0 && comboBox1.Text.Trim().Length > 0) {
                New_Ordonnance.Item_to_add = label4.Text;
                Close();
            }
            else
            {
                textBox1.BackColor = textBox1.Text.Trim().Length == 0 ? Color.LightCoral : Color.White;
                comboBox1.BackColor = comboBox1.Text.Trim().Length == 0 ? Color.LightCoral : Color.White;
                textBox1.Focus();
            }
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            comboBox1.BackColor = Color.White;
        }
    }
}
