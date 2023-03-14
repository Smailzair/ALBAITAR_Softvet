using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excc = Microsoft.Office.Interop.Excel;
using Label = System.Windows.Forms.Label;
using Rectangle = System.Drawing.Rectangle;

namespace ALBAITAR_Softvet
{
    internal class PreConnection
    {

        public static MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost;Database=albaitar_db;Uid=albaitar_user;Pwd=AlBaiTar9999;"); //DB Origine
        //public static MySqlConnection mySqlConnection = new MySqlConnection("Server=instances.spawn.cc;Port=31681;Database=TRESOR_LUNAR_TEST;Uid=root;Pwd=kOluo0PgmDVowykt;"); //Pour le test
        static bool Connection_opened = false;
        public static void open_conn()
        {
            try
            {

                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();

                }
                Connection_opened = true;
            }
            catch
            {
                Connection_opened = false; MessageBox.Show("Probleme de connection avec la base donnée, veuillez vérifier l'internet et ...", "--", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }

        }

        public static void close_conn()
        {

            if (mySqlConnection.State == ConnectionState.Open)
            {
                mySqlConnection.Close();

            }

        }


        public static DataTable Load_data_keeping_duplicates(string cmd)
        {
            DataTable tbl = new System.Data.DataTable();

            MySqlCommand cd = new MySqlCommand(cmd, mySqlConnection);
            open_conn();
            try
            {
                MySqlDataReader read = cd.ExecuteReader(); for (int t = 0; t < read.FieldCount; t++)
                {
                    tbl.Columns.Add(read.GetName(t), read.GetFieldType(t));
                    tbl.Columns[t].Unique = false;
                }
                tbl.PrimaryKey = null;

                //----------
                int mm = 0;

                while (read.Read())
                {
                    tbl.Rows.Add(tbl.NewRow());
                    for (int r = 0; r < read.FieldCount; r++)
                    {
                        tbl.Rows[tbl.Rows.Count - 1][r] = read.GetValue(r);
                    }
                    mm++;
                }
            }
            catch { if (!Connection_opened) { MessageBox.Show("Probleme de connection avec la base donnée, veuillez vérifier l'internet et ..."); } }
            ///--------------------


            //-----------
            return tbl;
        }
        public static DataTable Load_data(string cmd)
        {

            DataTable gg = new DataTable();
            MySqlCommand cd = new MySqlCommand(cmd, mySqlConnection);
            open_conn();
            try { MySqlDataReader read = cd.ExecuteReader(); gg.Load(read); } catch { if (!Connection_opened) { MessageBox.Show("Probleme de connection avec la base donnée, veuillez vérifier l'internet et ..."); } }

            return gg;
            //----------------------------------
        }

        public static void Excut_Cmd(string cmd)
        {

            MySqlCommand cmmd = new MySqlCommand(cmd, mySqlConnection);
            open_conn();
            try { cmmd.ExecuteNonQuery(); } catch { if (!Connection_opened) { MessageBox.Show("Probleme de connection avec la base donnée, veuillez vérifier l'internet et ..."); } }

        }


        ///////////////////////  RancoSoft Cammands   /////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////
        public static MySqlConnection client_manag = new MySqlConnection(@"Server=instances.spawn.cc;Port=31681;Database=CLIENTS_MANAG;Uid=root;Pwd=kOluo0PgmDVowykt;");
        ////////////////////////////////////////////        
        static string SERIAL = "zXdsf14s6q35EDdc7xc82vvc6d";
        ////////////////////////////////////////////
        public static void check_app_actiavtion()
        {

            ////--------------------------
            bool good = false;
            bool not_autorized = false;
            try
            {
                int previous_id = Properties.Settings.Default.RANCOSOFT_ACTIVE_CODE_ID;
                int ch = send_infos_to_server(previous_id);
                Properties.Settings.Default.RANCOSOFT_LAST_ACT_VERIF_DATE = DateTime.Now.ToString();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
                if (ch > 0)
                {
                    Properties.Settings.Default.RANCOSOFT_ACTIVE_CODE_ID = ch;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                    good = true;
                    WriteIntoRegistry(Application.ProductName + "_Activ", Codify_txt(Properties.Settings.Default.RANCOSOFT_LAST_ACT_VERIF_DATE + Environment.MachineName + System.Security.Principal.WindowsIdentity.GetCurrent().Name + SERIAL.Substring(SERIAL.Length - 4)));
                }
                else
                {
                    int ch2 = -2;
                    if (previous_id > 0)
                    {
                        try { ch2 = send_infos_to_server(0); } catch{ }
                        if (ch2 > 0)
                        {
                            Properties.Settings.Default.RANCOSOFT_ACTIVE_CODE_ID = ch2;
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();
                            good = true;
                            WriteIntoRegistry(Application.ProductName + "_Activ", Codify_txt(Properties.Settings.Default.RANCOSOFT_LAST_ACT_VERIF_DATE + Environment.MachineName + System.Security.Principal.WindowsIdentity.GetCurrent().Name + SERIAL.Substring(SERIAL.Length - 4)));
                        }
                        else
                        {
                            not_autorized = true;
                            Properties.Settings.Default.RANCOSOFT_ACTIVE_CODE_ID = 0;
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();
                            WriteIntoRegistry(Application.ProductName + "_Activ", "No");
                        }
                    }
                    else
                    {
                        not_autorized = true;
                        Properties.Settings.Default.RANCOSOFT_ACTIVE_CODE_ID = 0;
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.Reload();
                        WriteIntoRegistry(Application.ProductName + "_Activ", "No");
                    }
                    
                }
            }
            catch
            { }
            if (!good)
            {
                if (not_autorized)
                {
                    foreach (Form frm in Application.OpenForms)
                    {

                        frm.Dispose();
                    }
                    MessageBox.Show("Contacter RancoSoft : \n---------------------\nrancosoft@gmail.com\n0779.54.24.75", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
                else
                {
                    DateTime dte = new DateTime(1990, 01, 01);
                    if (Properties.Settings.Default.RANCOSOFT_LAST_ACT_VERIF_DATE.Length > 0)
                    {
                        DateTime.TryParse(Properties.Settings.Default.RANCOSOFT_LAST_ACT_VERIF_DATE, out dte);
                    }
                    else
                    {
                        dte = DateTime.Now;
                    }


                    if ((DateTime.Now - dte).TotalDays > 30 || ReadFromRegistry(Application.ProductName + "_Activ") != Codify_txt(Properties.Settings.Default.RANCOSOFT_LAST_ACT_VERIF_DATE + Environment.MachineName + System.Security.Principal.WindowsIdentity.GetCurrent().Name + SERIAL.Substring(SERIAL.Length - 4)))
                    {
                        foreach (Form frm in Application.OpenForms)
                        {
                            frm.Dispose();
                        }
                        MessageBox.Show("Vous avez obligatoirement besoin d'internet ... !\nVeuillez vérifier la connection puis lancer logiciel.", "Besoin d'une connection Internet", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Application.Exit();
                    }
                }


            }
        }

        public static void Update_activaion_sit()
        {
            Properties.Settings.Default.RANCOSOFT_LAST_USED_DATE = DateTime.Today.ToString();
            Properties.Settings.Default.RANCOSOFT_LAST_DEVICE_NAME = Environment.MachineName;
            Properties.Settings.Default.Save();
        }

        public static int send_infos_to_server(int ID_IN)
        {
            int ID_OUT = ID_IN;
            if (SERIAL.Length > 0)
            {
                MySqlCommand cmmd = new MySqlCommand("Insert_Into_CODES_ACTIVES", client_manag);
                cmmd.CommandType = CommandType.StoredProcedure;
                cmmd.Parameters.AddWithValue("@ACTIV_KEY", SERIAL).Direction = ParameterDirection.Input;
                cmmd.Parameters.AddWithValue("@Device_nme", Environment.MachineName).Direction = ParameterDirection.Input;
                cmmd.Parameters.AddWithValue("@OP_ID_IN", ID_IN).Direction = ParameterDirection.Input;
                cmmd.Parameters.AddWithValue("@OP_ID_OUT", ID_OUT).Direction = ParameterDirection.Output;
                if (client_manag.State != ConnectionState.Open)
                {
                    client_manag.Open();
                }
                if (client_manag.State == ConnectionState.Open)
                {

                    try
                    {
                        cmmd.ExecuteNonQuery();
                        ID_OUT = (int)cmmd.Parameters["@OP_ID_OUT"].Value;
                    }
                    catch { }
                    client_manag.Close();
                }
            }
            return ID_OUT;
        }
        /////////////////////// ///////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////


        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        public static bool Compare_Images(Bitmap b1, Bitmap b2)
        {
            if ((b1 == null) != (b2 == null)) return false;
            if (b1.Size != b2.Size) return false;

            var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr bd1scan0 = bd1.Scan0;
                IntPtr bd2scan0 = bd2.Scan0;

                int stride = bd1.Stride;
                int len = stride * b1.Height;

                return memcmp(bd1scan0, bd2scan0, len) == 0;
            }
            finally
            {
                b1.UnlockBits(bd1);
                b2.UnlockBits(bd2);
            }
        }

        public static void Excport_to_excel(DataGridView dgv, string Workbook_title, string Worksheet_title, DataTable tbl_to_add, bool Also_Hidden_Columns)
        {

            //================================================================
            if ((dgv.Rows.Count - (dgv.AllowUserToAddRows ? 1 : 0)) > 0)
            {
                Excc.Application xcelApp = new Excc.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                xcelApp.Application.Workbooks[1].Title = Workbook_title != null ? Workbook_title : "Classeur";
                xcelApp.Application.Workbooks[1].Worksheets[1].Name = Worksheet_title != null ? Worksheet_title : "Page";
                dgv.Columns.Cast<DataGridViewColumn>().ToList().ForEach(g =>
                {
                    xcelApp.Cells[1, g.Index + 1].Value = g.HeaderText;
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                    try
                    {
                        if (dgv.Columns[g.Index].DefaultCellStyle.Format == "N2")
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "#,##0.00 [$Da-fr-dz]";
                        }
                        else if (dgv.Columns[g.Index].DefaultCellStyle.Format.Contains("MM/yyyy"))
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "dd/MM/yyyy" + (dgv.Columns[g.Index].DefaultCellStyle.Format.Contains("HH") ? " HH:mm:ss" : "");
                        }
                    }
                    catch { }
                });


                dgv.Rows.Cast<DataGridViewRow>().ToList().ForEach(t =>
                {
                    t.Cells.Cast<DataGridViewCell>().ToList().ForEach(b =>
                    {
                        xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dgv.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dgv.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").TrimStart().TrimEnd() : "";
                    });

                });

                if (!Also_Hidden_Columns)
                {
                    int yy = 0;
                    dgv.Columns.Cast<DataGridViewColumn>().Where(z => !z.Visible).ToList().ForEach(g =>
                    {
                        xcelApp.Columns[g.Index + 1 - yy].Delete();
                        yy++;
                    });

                }

                if (tbl_to_add != null)
                {
                    tbl_to_add.Rows.Cast<DataRow>().ToList().ForEach(t =>
                    {
                        tbl_to_add.Columns.Cast<DataColumn>().ToList().ForEach(b =>
                        {
                            xcelApp.Cells[dgv.Rows.Count + tbl_to_add.Rows.IndexOf(t) + 2, tbl_to_add.Columns.IndexOf(b) + 1].Value = t[b] != null ? t[b].ToString().Replace(" 00", "").Replace(":00", "").Replace(",", ".").TrimStart().TrimEnd() : "";
                        });

                    });
                }
                xcelApp.Columns.AutoFit();
                //------------------
                SaveFileDialog svd = new SaveFileDialog();
                svd.Filter = "Excel | *.xlsx";
                svd.DefaultExt = "*.xlsx";
                svd.FileName = xcelApp.Application.Workbooks[1].Title + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                if (svd.ShowDialog() == DialogResult.OK)
                {
                    xcelApp.Workbooks[1].SaveAs(Path.GetFullPath(svd.FileName));
                    Process.Start(Path.GetFullPath(svd.FileName));
                }
                xcelApp.Application.Workbooks[1].Close(false);
                //Marshal.ReleaseComObject(xcelApp.Application.Workbooks[1]);
                xcelApp.Quit();
                //-------------------
            }
            else
            {
                MessageBox.Show("Aucun donnés !", ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        static List<string> ussrctrl = new List<string>();

        public static void search_filter_datagridview(DataGridView dgv, String searchTxt) //add it in search bar textchanged event
        {
            string ddd = "";
            foreach (DataGridViewColumn dd in dgv.Columns)
            {

                ddd += " OR Convert([" + dd.Name + "], System.String) LIKE '%{0}%'";
            }
            ddd = ddd.Substring(4);
            ((DataTable)dgv.DataSource).DefaultView.RowFilter = String.Format(ddd, searchTxt);
        }

        ///////////////////////////////////////////////////////////////////////
        ///
        public static string Codify_txt(string input)
        {
            string output = "";
            input.ToCharArray().ToList().ForEach(x =>
            {
                output += (char)(x + 1);
            });
            return output;
        }

        public static string Traduct_Codified_txt(string input)
        {
            string output = "";
            input.ToCharArray().ToList().ForEach(x =>
            {
                output += (char)(x - 1);
            });
            return output;
        }
        /// ///////////////////////////////////////////
        /// Registry
        /// 
        static string subKey = @"SOFTWARE\" + Application.ProductName;
        //RegistryKey domoCom = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Master", true);
        public static string ReadFromRegistry(string keyName)
        {

            RegistryKey sk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(subKey, true);
            //RegistryKey sk = Registry.LocalMachine.OpenSubKey(subKey);
            if (sk == null)
            {
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).CreateSubKey(subKey);
                sk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(subKey, true);
            }
            ///////////
            ///
            string ret = "";
            try
            {
                ret = sk.GetValue(keyName).ToString();
            }
            catch { }
            return ret;

        }

        public static void WriteIntoRegistry(string keyname, string value)
        {
            RegistryKey sk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(subKey, true);
            //RegistryKey sk = Registry.LocalMachine.OpenSubKey(subKey);
            if (sk == null)
            {
                //Registry.LocalMachine.CreateSubKey(subKey);
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).CreateSubKey(subKey);
                //sk = Registry.LocalMachine.OpenSubKey(subKey);
                sk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(subKey, true);
            }
            sk.SetValue(keyname, value);
            sk.Close();

        }
        public static void DeleteRegistryKey()
        {
            RegistryKey sk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(subKey, true);
            if (sk != null)
            {
                sk.Close();
                Registry.LocalMachine.DeleteSubKeyTree(subKey);
            }

        }
    }
}
