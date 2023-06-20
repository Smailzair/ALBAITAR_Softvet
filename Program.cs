using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); //ShowWindow needs an IntPtr

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Properties.Settings.Default.Reload();
            //------------------------------------
            //CultureInfo c = new System.Globalization.CultureInfo("fr-FR");
            //NumberFormatInfo nfi = new NumberFormatInfo();
            //nfi.NumberDecimalSeparator = ".";
            //nfi.NumberGroupSeparator = " ";
            //c.NumberFormat = nfi;
            //c.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //c.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            //Application.CurrentCulture = c;
            //---------------------------------------
            if(args.Length > 0 && args[0] == "Open_Connection_Str")
            {
                Application.Run(new Dialogs.Connection_Str());
            }
            else
            {
                //Thread RancoVerif = new Thread(PreConnection.check_app_actiavtion);
                //RancoVerif.Start();
                //---------------------------------------
                string procName = Process.GetCurrentProcess().ProcessName;
                // get the list of all processes by that name

                Process[] processes = Process.GetProcessesByName(procName);

                if (processes.Length <= 1)
                {

                    if (Properties.Settings.Default.Login_Auto_Enter && (DateTime.Now - Properties.Settings.Default.Last_entred_date_by_Auto_Enter).Days < 7)
                    {
                        Properties.Settings.Default.Last_entred_date_by_Auto_Enter = DateTime.Now;
                        Properties.Settings.Default.Save();
                        Application.Run(new Main_Frm());
                    }
                    else
                    {
                        Properties.Settings.Default.Login_Auto_Enter = false;
                        Properties.Settings.Default.Save();
                        Login log = new Login(false, null);
                        Application.Run(log);
                        log.BringToFront();
                        //--------------
                        if (Login.enter_allow)
                        {
                            Application.Run(new Main_Frm());
                        }
                    }
                }
                else
                {
                    IntPtr hWnd;
                    hWnd = processes[0].MainWindowHandle; //use it as IntPtr not int
                    ShowWindow(hWnd, 3);
                    SetForegroundWindow(hWnd); //set to topmost
                }
            }

            
        }
    }
}
