using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xamarin.Forms.Internals;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class DB_Backup : Form
    {
        string save_folder = "";
        public DB_Backup()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = radioButton2.Checked;
        }

        private void DB_Backup_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            //-----------

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string old_folder = textBox1.Text.Trim();
                textBox1.Text = save_folder = folderBrowserDialog1.SelectedPath;
                bool deplac = false;
                int cnnt = 0;
                if (Directory.Exists(old_folder))
                {
                    cnnt = Directory.GetFiles(old_folder).AsEnumerable().Where(RR => (new FileInfo(RR)).Extension.ToLower() == ".sql").OrderBy(EE => (new FileInfo(EE)).LastWriteTime).ToList().Count();
                    if (cnnt > 0)
                    {
                        deplac = MessageBox.Show("Voulez vous de déplacer les anciens fichiers à ce dossier ?", "Question :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    }
                }

                if (deplac)
                {
                    string[] files = Directory.GetFiles(old_folder);
                    string[] new_fld_files = Directory.GetFiles(save_folder);
                    files.AsEnumerable().Where(RR => (new FileInfo(RR)).Extension.ToLower() == ".sql").OrderBy(EE => (new FileInfo(EE)).LastWriteTime).ToList().ForEach(M =>
                    {
                        string fileName = Path.GetFileName(M);
                        if(new_fld_files.AsEnumerable().Where(RR => (new FileInfo(RR)).Name == fileName).Count() > 0)
                        {
                            fileName = "OLD_" + fileName;
                        }
                        File.Move(M, Path.Combine(save_folder, fileName));
                    });
                }
                else if (cnnt > 0)
                {
                    MessageBox.Show("Les sauvegardes du dossier précedent ne sera plus affichés dans la liste.\n\nRMQ: Cependant, vous pouvez le restaurer et l'utiliser manuellement.", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                scan_files();

            }
        }

        private void scan_files()
        {

            dataGridView1.Rows.Clear();
            if (!string.IsNullOrWhiteSpace(save_folder))
            {
                try
                {
                    string[] files = Directory.GetFiles(save_folder);
                    files.AsEnumerable().Where(RR => (new FileInfo(RR)).Extension.ToLower() == ".sql").OrderBy(EE => (new FileInfo(EE)).LastWriteTime).ToList().ForEach(M =>
                    {
                        FileInfo fileInfo = new FileInfo(M);
                        dataGridView1.Rows.Add(fileInfo.LastWriteTime, fileInfo.Name, fileInfo.FullName);
                    });

                    if (dataGridView1.Rows.Count >= 1)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
                        dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
                    }
                }
                catch (Exception)
                {
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Voulez vous de supprimer (" + dataGridView1.SelectedRows.Count + ") sauvegardes ?\n\n(méme le fichier stocké sera supprimé)", "Confirmation :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    int prev_sel = dataGridView1.SelectedRows[0].Index;
                    foreach (DataGridViewRow rw in dataGridView1.SelectedRows)
                    {
                        try
                        {
                            if (File.Exists((string)rw.Cells["PATH"].Value))
                            {
                                File.Delete((string)rw.Cells["PATH"].Value);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    scan_files();
                    if (dataGridView1.Rows.Count > prev_sel)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[prev_sel].Selected = true;
                    }
                }

            }
        }
    }
}
