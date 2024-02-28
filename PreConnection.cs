using ALBAITAR_Softvet.Dialogs;
using Microsoft.ReportingServices.Diagnostics.Internal;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
//using Xamarin.Forms.Internals;
using Excc = Microsoft.Office.Interop.Excel;
using Label = System.Windows.Forms.Label;
using Rectangle = System.Drawing.Rectangle;

namespace ALBAITAR_Softvet
{
    internal class PreConnection
    {
        //  public static Loading loading = new Loading();
        static string connectionString_txt = "Server=" + Properties.Settings.Default.Connection_String_IP_Or_LocalHost + ";Port=3306;Database=albaitar_db;Uid=albaitar_user;Pwd=AlBaiTar9999;";
        public static MySqlConnection mySqlConnection = new MySqlConnection(connectionString_txt); //DB Origine                
        // static bool Connection_opened = false;



        public static void open_conn()
        {
            try
            {
                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();

                }
                //  Connection_opened = true;
            }
            catch
            {
                // Connection_opened = false;
                MessageBox.Show("Probleme de connection avec la base donnée, veuillez vérifier ...", "--", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Process myProcess = Process.Start("ALBAITAR_Softvet.exe", "Open_Connection_Str");

                Process.GetCurrentProcess().Kill();
            }

        }

        public static void close_conn()
        {
            try
            {
                if (mySqlConnection.State == ConnectionState.Open)
                {
                    mySqlConnection.Close();

                }
            }
            catch { }
            

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
            catch
            {
                //    if (!Connection_opened) { MessageBox.Show("Probleme de connection avec la base donnée, veuillez vérifier l'internet et ..."); } 
            }
            ///--------------------
            close_conn();

            //-----------
            return tbl;
        }
        public static DataTable Load_data(string cmd)
        {

            DataTable gg = new DataTable();
            MySqlCommand cd = new MySqlCommand(cmd, mySqlConnection);
            open_conn();
            try { MySqlDataReader read = cd.ExecuteReader(); gg.Load(read); }
            catch
            {
                //if (!Connection_opened) { MessageBox.Show("Probleme de connection avec la base donnée, veuillez vérifier l'internet et ..."); }
            }
            close_conn();
            return gg;
            //----------------------------------
        }

        //public static int Excut_Cmd(string cmd)
        //{
        //    int rows_nb = 0;
        //    MySqlCommand cmmd = new MySqlCommand(cmd, mySqlConnection);
        //    open_conn();
        //    //try { cmmd.ExecuteNonQuery(); } catch { if (!Connection_opened) { MessageBox.Show("Probleme de connection avec la base donnée, veuillez vérifier l'internet et ..."); } }
        //    try { rows_nb = cmmd.ExecuteNonQuery(); }
        //    catch
        //    {
        //        //if (!Connection_opened) { MessageBox.Show("Probleme de connection avec la base donnée !"); }
        //    }
        //    close_conn();
        //    return rows_nb;
        //}

        public static int Excut_Cmd(int Insert_1_or_Update_2_Delete_3, string Table_nme, List<string> Columns_names, List<object> col_values, string where_expression, List<string> where_columns, List<object> where_values)
        {
            string pattern = @"[^a-zA-Z0-9_@]";
            string cmmd = "";
            if (Insert_1_or_Update_2_Delete_3 == 1) //INSERT
            {
                cmmd += "INSERT INTO " + Table_nme + " (";
                foreach (var col in Columns_names)
                {
                    cmmd += "`" + col + "`,";
                }
                cmmd = cmmd.TrimEnd(',');
                cmmd += ") VALUES (";
                for (int i = 0; i < Columns_names.Count; i++)
                {
                    string col_nme = Regex.Replace(Columns_names[i], pattern, "");
                    cmmd += "@i" + i.ToString() + "_" + col_nme + ",";
                }
                cmmd = cmmd.TrimEnd(',');
                cmmd += ")";
                if (where_expression != null) { if (where_expression.Length > 0) { cmmd += " WHERE " + where_expression; } }
                cmmd += ";";
            }
            else if (Insert_1_or_Update_2_Delete_3 == 2) //UPDATE
            {
                cmmd += "UPDATE " + Table_nme + " SET ";
                for (int i = 0; i < Columns_names.Count; i++)
                {
                    string col_nme = Regex.Replace(Columns_names[i], pattern, "");
                    cmmd += "`" + Columns_names[i] + "`" + " = @i" + i.ToString() + "_" + col_nme + ",";
                }
                cmmd = cmmd.TrimEnd(',');
                if (where_expression != null) { if (where_expression.Length > 0) { cmmd += " WHERE " + where_expression; } }
                cmmd += ";";
            }
            else //DELETE ------>> BE CAREFULL !! THERE'S PROBLEME OF CASTING WHERE VALUE (EXP: ..IN (12,13) It SEE IT AS DOUBLE NOT TWO INTEGERS) SO BEST TO USE VOID OF 'Excut_Cmd_personnel' WITHOUT PARAMS (ALL IN CMD TEXT), THAT'S ALL GENERALLY WHILE USING 'IN'.
            {
                cmmd += "DELETE FROM " + Table_nme;
                if (where_expression != null) { if (where_expression.Length > 0) { cmmd += " WHERE " + where_expression; } }
                cmmd += ";";
            }

            int rows_nb = 0;
            open_conn();
            try
            {
                using (MySqlCommand command = new MySqlCommand(cmmd, mySqlConnection))
                {
                    if (Columns_names != null && col_values != null)
                    {
                        for (int i = 0; i < Columns_names.Count; i++)
                        {
                            string col_nme = Regex.Replace(Columns_names[i], pattern, "");
                            command.Parameters.AddWithValue("@i" + i.ToString() + "_" + col_nme, col_values[i]);
                        }
                    }

                    if (where_columns != null && where_values != null)
                    {
                        for (int i = 0; i < where_columns.Count; i++)
                        {
                            command.Parameters.AddWithValue(where_columns[i], where_values[i].ToString());
                        }
                    }

                    rows_nb = command.ExecuteNonQuery();
                }

            }
            catch { }
            close_conn();
            return rows_nb;
        }

        public static int Excut_Cmd_personnel(string cmmd, List<string> params_names, List<object> params_values)
        {
            int rows_nb = 0;
            open_conn();
            try
            {
                using (MySqlCommand command = new MySqlCommand(cmmd, mySqlConnection))
                {
                    if (params_names != null)
                    {
                        if (params_names.Count > 0)
                        {
                            for (int i = 0; i < params_names.Count; i++)
                            {
                                command.Parameters.AddWithValue("@" + params_names[i], params_values[i]);
                            }
                        }
                    }
                    rows_nb = command.ExecuteNonQuery();
                }

            }
            catch { }
            close_conn();
            return rows_nb;
        }

        public static bool DB_Backup(string backupPath)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString_txt);
            string host = builder.Server;
            string userId = builder.UserID;
            string password = builder.Password;
            string database = builder.Database;

            string[] tablesToExclude = { "tb_login_and_users", "tb_params"};

            string ignoreTables = string.Join(" ", tablesToExclude);
            string command = $"mysqldump --host={host} --user={userId} --password={password} --databases {database} --routines --triggers --ignore-table={database}.{ignoreTables} > \"{backupPath}\"";
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                using (StreamWriter sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine(command);
                    }
                }
                //--------------------------
                FileInfo flle = new FileInfo(backupPath);
                int max_s = 0;
                int.TryParse(Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 10).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString(), out max_s);
                while (max_s > 0 && GetFolderSizeInMB(flle.DirectoryName) > max_s)
                {
                    File.Delete((new DirectoryInfo(flle.DirectoryName)).GetFiles("*.sql", SearchOption.AllDirectories).OrderBy(TT => TT.CreationTime).First().FullName);
                }
                //------------------------
                process.WaitForExit();
                process.Close();

            }
            catch (Exception)
            {
            }

            //-------------
            bool answer = false;
            FileInfo inff = new FileInfo(backupPath);
            if (File.Exists(backupPath))
            {
                answer = (new FileInfo(backupPath)).Length > 0;
            }
            return answer;
        }
        static double GetFolderSizeInMB(string folderPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            if (!directoryInfo.Exists)
            {
                return 0;
            }

            // Calculate the size of the folder and its subfolders in megabytes
            double sizeInBytes = 0;
            foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.sql", SearchOption.AllDirectories))
            {
                sizeInBytes += fileInfo.Length;
            }

            double sizeInMB = sizeInBytes / (1024.0 * 1024.0); // Convert bytes to megabytes
            return sizeInMB;
        }

        public static bool DB_Restore(string backupPath)
        {
            bool answer = true;

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString_txt);
            string host = builder.Server;
            string userId = builder.UserID;
            string password = builder.Password;
            string database = builder.Database;


            string command = $"mysql --host={host} --user={userId} --password={password} {database} < \"{backupPath}\"";

            try
            {
                // Execute the mysqldump command
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                using (StreamWriter sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine(command);
                    }
                }

                process.WaitForExit();
                process.Close();

            }
            catch (Exception)
            {
                answer = false;
            }

            //-------------
            return answer;
        }
        ///////////////////////  RancoSoft Cammands   /////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////
        public static MySqlConnection client_manag = new MySqlConnection(@"Server=62.72.50.1;Port=3306;Database=u844866977_CLIENTS_MANAG;Uid=u844866977_RANCO_USER;Pwd=v[y:hyiN3W4;");
        ////////////////////////////////////////////        
        static string SERIAL = "zXdsf14s6q35EDdc7xc82vvc6d";
        ////////////////////////////////////////////
        public static void check_app_actiavtion()
        {
            //--------------------------
            int ch = -1;
            try
            {
                ch = Verif_manual_stop_of_RancoSoft();
            }
            catch { }
            //----------
            if (ch <= 0) // NOT STOPED (Or not yet)
            {
                if (ch == 0) //Verifed (Good)
                {
                    WriteIntoRegistry(Application.ProductName + "_Activ", "Yes");
                    WriteIntoRegistry("RANCOSOFT_LAST_ACT_VERIF_DATE", DateTime.Now.ToString());
                }
                else //-1 Not veried
                {
                    WriteIntoRegistry(Application.ProductName + "_Activ", "No");
                    DateTime dte;
                    if (ReadFromRegistry("RANCOSOFT_LAST_ACT_VERIF_DATE").Length > 0)
                    {
                        DateTime.TryParse(ReadFromRegistry("RANCOSOFT_LAST_ACT_VERIF_DATE"), out dte);
                    }
                    else
                    {
                        dte = DateTime.Now;
                    }
                    if ((DateTime.Now - dte).TotalDays > 60)
                    {
                        foreach (Form frm in Application.OpenForms)
                        {
                            frm.Dispose();
                        }
                        MessageBox.Show("Vous avez obligatoirement besoin d'internet ... !\nVeuillez vérifier la connection puis lancer logiciel.", "Besoin d'une connection Internet ", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        Application.Exit();
                    }
                }
                //-----------

            }
            else //STOPED
            {
                WriteIntoRegistry(Application.ProductName + "_Activ", "No");
                WriteIntoRegistry("RANCOSOFT_LAST_ACT_VERIF_DATE", DateTime.Now.ToString());
                //--------------------
                MessageBox.Show("Vous avez un problème ... !\nN'hisiter de nous contacter pour faire le diagnostique.\n\n[ albaitar.technologie@gmail.com ]", "--", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        public static bool IsInternetAvailable()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public static void load_rancosoft_gmail_auth()
        {
            if (IsInternetAvailable())
            {
                if (client_manag.State != ConnectionState.Open)
                {
                    client_manag.Open();
                }
                if (client_manag.State == ConnectionState.Open)
                {
                    try
                    {
                        //Load Gmail Authent Pass (to use it to send forgot login pass of login of clients)
                        MySqlDataAdapter adp = new MySqlDataAdapter("SELECT VALUE_TXT FROM PARAMS_AND_VALUES WHERE NME = 'RancoSoft Gmail Auth';", client_manag);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0) { Properties.Settings.Default.RANCOSOFT_GMAIL_AUTHENT = PreConnection.Codify_txt(dt.Rows[0][0].ToString()); Properties.Settings.Default.Save(); }
                        //==========================================================
                    }
                    catch { }
                    client_manag.Close();
                }
            }
        }
        public static int Verif_manual_stop_of_RancoSoft()
        {
            int ID_OUT = -1;
            if (SERIAL.Length > 0)
            {
                MySqlCommand cmmd = new MySqlCommand("MANUAL_STOP_2nd_VERIF", client_manag);
                cmmd.CommandType = CommandType.StoredProcedure;
                cmmd.Parameters.AddWithValue("ACT_KEY", SERIAL).Direction = ParameterDirection.Input;
                cmmd.Parameters.AddWithValue("MAN_STOP", ID_OUT).Direction = ParameterDirection.Output; //1-> Stopped   0-> Not stopped  (-1)->Nothing
                if (client_manag.State != ConnectionState.Open)
                {
                    client_manag.Open();
                }
                if (client_manag.State == ConnectionState.Open)
                {
                    try
                    {
                        cmmd.ExecuteNonQuery();
                        ID_OUT = (int)cmmd.Parameters["MAN_STOP"].Value;
                    }
                    catch { }
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

        public static bool ArePicturesEqual(Image pictureBox1, Image pictureBox2)
        {
            if (pictureBox1 == null || pictureBox2 == null)
            {
                return false;
            }

            if (pictureBox1.Width != pictureBox2.Width ||
                pictureBox1.Height != pictureBox2.Height)
            {
                // The images have different sizes
                return false;
            }

            Bitmap bmp1 = new Bitmap(pictureBox1);
            Bitmap bmp2 = new Bitmap(pictureBox2);

            for (int y = 0; y < bmp1.Height; y++)
            {
                for (int x = 0; x < bmp1.Width; x++)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
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
                        else
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "@";
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
            ((DataTable)dgv.DataSource).DefaultView.RowFilter = String.Format(ddd, searchTxt.Replace("'", "''"));
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
                ret = sk.GetValue(keyName) != null ? sk.GetValue(keyName).ToString() : "";
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

        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));

        }

        public static Image ByteArrayToImage(byte[] imageBytes)
        {
            try
            {
                //using (var memoryStream = new MemoryStream(imageBytes))
                //{
                //    return Image.FromStream(memoryStream);
                //}


                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms, false, false);
                    return image;
                }
            }
            catch
            {
                return null;
            }

        }
        //================================================
        public static string generate_ID_of_client()
        {
            string tmmp = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string IDd = "";
            //---------
            string PC_Nme = Environment.MachineName;
            string MAC = GetPhysicalEthernetMacAddress();
            //---------
            IDd += MAC.Substring(4, 1) + MAC.Substring(7, 1);
            IDd += PC_Nme.Length > 0 ? PC_Nme.Substring(1, 1) : "K";
            IDd += MAC.Substring(9, 1) + MAC.Substring(6, 1);
            IDd += MAC.Substring(2, 1) + MAC.Substring(5, 1);
            IDd += (PC_Nme.Length * 5).ToString("0").Length > 3 ? (PC_Nme.Length * 5).ToString("0").Substring(0, 3) : "BVT";
            IDd += MAC.Substring(1, 1) + MAC.Substring(3, 1);
            string h = IDd;
            h.ForEach(CC =>
            {
                IDd += tmmp.Substring(tmmp.IndexOf(CC) - 1 > 0 ? tmmp.IndexOf(CC) - 1 : tmmp.IndexOf(CC) + 15, 1);
            });
            IDd += MAC.Substring(10, 1) + MAC.Substring(11, 1);
            if (IDd.Length >= 25)
            {
                IDd = InsertHyphens(IDd.Substring(0, 25));
            }
            return IDd;
        }
        static string GetPhysicalEthernetMacAddress()
        {
            NetworkInterface[] physicalEthernetInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && !nic.Description.ToLower().Contains("virtual"))
                .ToArray();

            foreach (NetworkInterface nic in physicalEthernetInterfaces)
            {
                PhysicalAddress macAddress = nic.GetPhysicalAddress();
                if (macAddress != null && macAddress.ToString() != string.Empty)
                {
                    return macAddress.ToString();
                }
            }

            return "ZD51VQX9L7MO";
        }
        static string InsertHyphens(string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i += 5)
            {
                if (i > 0)
                    sb.Append('-');
                sb.Append(input.Substring(i, Math.Min(5, input.Length - i)));
            }
            return sb.ToString();
        }

        public static bool Verif_Activation_SOftVet(string Code_Act)
        {
            string ID = generate_ID_of_client();

            if (!string.IsNullOrEmpty(ID) && Code_Act.Length > 0)
            {

                string zzz = "Eck47eNK1pqhQ3UvsG6BfOnHmo9iMTlJbDA58dFjgXZxtVz0rSRWYyw2CILuPa";
                int[] dds = { 22, 21, 0, 45, 12, 61, 2, 44, 3, 23, 18, 1, 46, 51, 20, 6, 43, 17, 24, 5, 26, 4, 19, 53, 33, 11, 52, 25, 32, 55, 50, 7, 36, 41, 31, 56, 54, 10, 40, 30, 47, 39, 8, 42, 29, 9, 57, 48, 59, 15, 34, 38, 28, 60, 35, 58, 14, 27, 16, 49, 37, 13 };
                string[] ff = ID.Split('-');
                string v1 = ff[0];
                string v2 = ff[1];
                string v3 = ff[2];
                string v4 = ff[3];
                string v5 = ff[4];
                string w1 = "";
                string w2 = "";
                string w3 = "";
                string w4 = "";
                string w5 = "";
                for (int t = 0; t < 5; t++)
                {

                    w1 += zzz.Substring(dds[(zzz.IndexOf(v1.Substring(t, 1)) + t + 4) < 62 ? (zzz.IndexOf(v1.Substring(t, 1)) + t + 4) : (zzz.IndexOf(v1.Substring(t, 1)) - t - 4)], 1);
                    w2 += zzz.Substring(dds[(zzz.IndexOf(v2.Substring(t, 1)) + t + 2) < 62 ? (zzz.IndexOf(v2.Substring(t, 1)) + t + 2) : (zzz.IndexOf(v2.Substring(t, 1)) - t - 2)], 1);
                    w3 += zzz.Substring(dds[(zzz.IndexOf(v3.Substring(t, 1)) + t + 1) < 62 ? (zzz.IndexOf(v3.Substring(t, 1)) + t + 1) : (zzz.IndexOf(v3.Substring(t, 1)) - t - 1)], 1);
                    w4 += zzz.Substring(dds[(zzz.IndexOf(v4.Substring(t, 1)) + t + 5) < 62 ? (zzz.IndexOf(v4.Substring(t, 1)) + t + 5) : (zzz.IndexOf(v4.Substring(t, 1)) - t - 5)], 1);
                    w5 += zzz.Substring(dds[(zzz.IndexOf(v5.Substring(t, 1)) + t + 3) < 62 ? (zzz.IndexOf(v5.Substring(t, 1)) + t + 3) : (zzz.IndexOf(v5.Substring(t, 1)) - t - 3)], 1);

                }
                string Act_code = string.Concat(w1, w2, w3, w4, w5);
                return Act_code == Code_Act;
            }
            else
            {
                return false;
            }


        }

        public static bool Verif_real_server_activ()
        {
            bool Verifyed_001 = false;
            if (Properties.Settings.Default.Codifed_Activation_Email.Length > 2 && Properties.Settings.Default.Codified_Activate_Code.Length > 2)
            {
                MySqlConnection albaitar_online = new MySqlConnection(@"Server=62.72.50.1;Port=3306;Database=u844866977_BAITAR_CLIENTS;Uid=u844866977_baitar_user;Pwd=Zsd52##dQemN41;");
                //---------------------
                MySqlCommand command = new MySqlCommand("INSERT INTO `MOUVEMENTS` (`CLIENT_ID`,`CLIENT_EMAIL`,`ACTIVAT_CODE`) VALUES (" +
                    "'" + PreConnection.generate_ID_of_client() + "'," +
                    "'" + PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codifed_Activation_Email) + "'," +
                    "'" + PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Activate_Code) + "');", albaitar_online);

                try
                {
                    if (albaitar_online.State != ConnectionState.Open) { albaitar_online.Open(); }
                    command.ExecuteNonQuery();
                }
                catch { }
                albaitar_online.Close();
                //-------------------------------
                DataTable dttb = new DataTable();
                MySqlCommand command2 = new MySqlCommand("SELECT * FROM `MOUVEMENTS` WHERE `CLIENT_EMAIL` = '" + PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codifed_Activation_Email) + "' AND  `ACTIVAT_CODE` = '" + PreConnection.Traduct_Codified_txt(Properties.Settings.Default.Codified_Activate_Code) + "';", albaitar_online);
                try
                {
                    if (albaitar_online.State != ConnectionState.Open) { albaitar_online.Open(); }
                    using (MySqlDataReader reader = command2.ExecuteReader())
                    {
                        dttb.Load(reader);
                    }
                }
                catch { }
                albaitar_online.Close();
                if (dttb.Rows.Count > 0) //Good
                {
                    Verifyed_001 = true;
                }
            }
            return Verifyed_001;
        }

    }
}
