using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class DB_Backup : Form
    {
        //string save_folder = "";
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
        static string ExtractNumericPart(string input)
        {
            Match match = Regex.Match(input, @"\d+");
            return match.Success ? match.Value : string.Empty;
        }
        private void DB_Backup_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            //-----------
            string Back_Folder = (string)Main_Frm.Params.AsEnumerable().Where(R => (int)R["ID"] == 8).First()["VAL"];
            string Back_frq = (string)Main_Frm.Params.AsEnumerable().Where(R => (int)R["ID"] == 9).First()["VAL"];
            textBox1.Text = Back_Folder;
            if (!string.IsNullOrWhiteSpace(Back_frq))
            {
                if (Back_frq.EndsWith("Quit"))
                {
                    radioButton1.Checked = true;
                }
                else if (Back_frq.EndsWith("Mois") || Back_frq.EndsWith("Jours"))
                {
                    radioButton2.Checked = true;
                    comboBox1.SelectedIndex = Back_frq.EndsWith("Mois") ? 1 : 0;
                    int nb = 1;
                    string nb_str = ExtractNumericPart(Back_frq);
                    if (nb_str.Length > 0)
                    {
                        int.TryParse(nb_str, out nb);
                    }
                    numericUpDown1.Value = nb;
                }
                else
                {
                    radioButton3.Checked = true;
                }
            }
            else
            {
                radioButton3.Checked = true;
            }
            scan_files();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string old_folder = textBox1.Text.Trim();
            if (Directory.Exists(old_folder))
            {
                folderBrowserDialog1.SelectedPath = Path.GetDirectoryName(old_folder);
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(folderBrowserDialog1.SelectedPath))
                {
                    textBox1.Text = folderBrowserDialog1.SelectedPath;
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
                        string[] new_fld_files = Directory.GetFiles(textBox1.Text);
                        files.AsEnumerable().Where(RR => (new FileInfo(RR)).Extension.ToLower() == ".sql").OrderBy(EE => (new FileInfo(EE)).LastWriteTime).ToList().ForEach(M =>
                        {
                            string fileName = Path.GetFileName(M);
                            if (new_fld_files.AsEnumerable().Where(RR => (new FileInfo(RR)).Name == fileName).Count() > 0)
                            {
                                fileName = "OLD_" + fileName;
                            }
                            File.Move(M, Path.Combine(textBox1.Text, fileName));
                        });
                    }
                    else if (cnnt > 0)
                    {
                        MessageBox.Show("Les sauvegardes du dossier précedent ne sera plus affichés dans la liste.\n\nRMQ: Cependant, vous pouvez le restaurer et l'utiliser manuellement.", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    scan_files();
                }
                else
                {
                    MessageBox.Show("Le dossier '" + folderBrowserDialog1.SelectedPath + "' non trouvé\nVeuillez réesayer.");
                }
            }
        }

        private void scan_files()
        {

            dataGridView1.Rows.Clear();
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                try
                {
                    string[] files = Directory.GetFiles(textBox1.Text);
                    files.AsEnumerable().Where(RR => (new FileInfo(RR)).Extension.ToLower() == ".sql" && (new FileInfo(RR)).Length > 0).OrderBy(EE => (new FileInfo(EE)).LastWriteTime).ToList().ForEach(M =>
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

        private void button1_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(textBox1.Text);
            string file_nme = string.Concat("al_baitar_backup_", DateTime.Now.ToString("dd_MM_yyyy_HH'h'_mm'm'_ss's'_"), DateTime.Now.Millisecond, ".sql");
            PreConnection.DB_Backup(textBox1.Text + @"\" + file_nme);
            if (PreConnection.DB_Backup(textBox1.Text + @"\" + file_nme))
            {
                MessageBox.Show("Bien sauvegardé !");
                scan_files();
            }
            else
            {
                MessageBox.Show("Sauvegarde non efféctuée, veuillez réesayer.", "Non effectuée :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void DB_Backup_FormClosing(object sender, FormClosingEventArgs e)
        {
            PreConnection.Excut_Cmd(2, "tb_params", new List<string> { "VAL" }, new List<object> { textBox1.Text }, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { 8 });

            string backk = "None";
            if (radioButton2.Checked)
            {
                backk = string.Concat(numericUpDown1.Value, (comboBox1.SelectedIndex == 0 ? " Jours" : " Mois"));
            }else if (radioButton1.Checked)
            {
                backk = "In_App_Quit";
            }

            PreConnection.Excut_Cmd(2, "tb_params", new List<string> { "VAL" }, new List<object> { backk }, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { 9 });
            Main_Frm.Params = PreConnection.Load_data("SELECT * FROM tb_params;");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0) {
                if(MessageBox.Show("Êtes-vous sûr de restaurer la base de données ?\n\n(Toutes les données existantes seront écrasées !)", "Attention :",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    panel2.Visible = true;
                    panel2.Refresh();
                    if (PreConnection.DB_Restore((string)dataGridView1.SelectedRows[0].Cells["PATH"].Value))
                    {
                        panel2.Visible = false;
                        panel2.Refresh();
                        MessageBox.Show("Restauration terminée avec succès, l'application se quitte, veuillez relancer le programme.");
                        Application.Exit();
                    }
                }
            }
        }
    }
}

