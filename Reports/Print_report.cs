using Google.Protobuf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WinForms;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xamarin.Forms.Internals;

namespace ALBAITAR_Softvet
{
    public partial class Print_report : Form
    {
        string repprot_nmme = "";
        DataTable parr;
        public Print_report(string report_nme, DataTable paramss)
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            //-------
            PageSettings pg_set = new PageSettings();
            pg_set.PaperSize = new PaperSize("A4", 827, 1170);
            pg_set.Margins.Right = 0;
            pg_set.Margins.Left = 0;
            pg_set.Margins.Top = 0;
            pg_set.Margins.Bottom = 0;            
            this.reportViewer1.SetPageSettings(pg_set);
            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            this.reportViewer1.ZoomPercent = 100;
            this.reportViewer1.ZoomMode = ZoomMode.Percent;
            //------------
            repprot_nmme = report_nme;
            parr = paramss;



        }
        
        private void Print_report_Load(object sender, EventArgs e)
        {
            bool good = false;
            string filee = "ALBAITAR_Softvet.Reports." + repprot_nmme + ".rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection();
            int yy = 0;
            foreach(DataRow rw in parr.Rows)
            {
                reportParameters.Add(new ReportParameter(rw["PARAM_NME"].ToString(), rw["PARAM_VAL"].ToString()));
            }
            
            if (repprot_nmme.Length > 0)
            {
                good = Assembly.GetExecutingAssembly().GetManifestResourceNames().Contains(filee);
            }
            else { good = false; }

            if (good)
            {

                
                reportViewer1.LocalReport.LoadReportDefinition(Assembly.GetExecutingAssembly().GetManifestResourceStream(filee));
                if(parr.Rows.Count > 0) { reportViewer1.LocalReport.SetParameters(reportParameters); }                
                reportViewer1.RefreshReport();
        }
            else
            {
                Dispose();
    }
}

        private void reportViewer1_ReportExport(object sender, ReportExportEventArgs e)
        {
            if (e.Extension.Name == "PDF")
            {
                //e.Cancel = true;
                //----------------
                
            }
        }

        private void reportViewer1_RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {
           
        }
    }
}
