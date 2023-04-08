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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //------------------------------------
            CultureInfo c = new System.Globalization.CultureInfo("en-EN");
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSeparator = " ";
            c.NumberFormat = nfi;
            c.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            c.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Application.CurrentCulture = c;
            //---------------------------------------
            Thread RancoVerif = new Thread(PreConnection.check_app_actiavtion);
            RancoVerif.Start();
            //---------------------------------------
            string procName = Process.GetCurrentProcess().ProcessName;
            // get the list of all processes by that name

            Process[] processes = Process.GetProcessesByName(procName);

            if (processes.Length <= 1)
            {
                Login log = new Login(false);
                Application.Run(log);
                log.BringToFront();
                //--------------
                if (Login.enter_allow)
                {
                    Application.Run(new Main_Frm());
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
