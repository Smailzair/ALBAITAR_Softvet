using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using word = Microsoft.Office.Interop.Word.Application;
using Excel = Microsoft.Office.Interop.Excel.Application;
using Microsoft.ReportingServices.Interfaces;
using System.Diagnostics;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using DataSet = System.Data.DataSet;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;

namespace ALBAITAR_Softvet
{
    public partial class Print_report : Form
    {
        string repprot_nmme = "";
        System.Data.DataTable parr;
        System.Data.DataTable datasrc;
        public Print_report(string report_nme, System.Data.DataTable paramss, System.Data.DataTable src)
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            //-------
            PageSettings pg_set = new PageSettings();
            pg_set.PaperSize = new System.Drawing.Printing.PaperSize("A4", 827, 1170);
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
            //------
            if(src != null)
            {
                datasrc = src;
            }



        }

        private void Print_report_Load(object sender, EventArgs e)
        {
            bool good = false;
            string filee = "ALBAITAR_Softvet.Reports." + repprot_nmme + ".rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection();
            foreach (DataRow rw in parr.Rows)
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


                //reportViewer1.LocalReport.LoadReportDefinition(Assembly.GetExecutingAssembly().GetManifestResourceStream(filee));

                // Set the processing mode to local
                reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;

                // Set the embedded resource name of the RDLC file
                reportViewer1.LocalReport.ReportEmbeddedResource = filee;       

                if (parr.Rows.Count > 0) { reportViewer1.LocalReport.SetParameters(reportParameters); }

                if (datasrc != null)
                {
                    // Define a dataset with the same name as the data source
                    DataSet ds = new DataSet("MyDataSource");

                    // Add a table to the dataset with the same schema as the DataTable
                    System.Data.DataTable table = datasrc.Copy();
                    table.TableName = "MyTable";
                    ds.Tables.Add(table);

                    // Set the LocalReport object's data source to the dataset
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("MyDataSource", ds.Tables[0]));
                }

                reportViewer1.RefreshReport();
            }
            else
            {
                Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = button2.Enabled = button3.Enabled = false;
            //----------------
            SaveFileDialog dlg2 = new SaveFileDialog();
            if (dlg2.ShowDialog() == DialogResult.OK)
            {
                string without_ext2 = dlg2.FileName.Contains(".") ? dlg2.FileName.Substring(0, dlg2.FileName.LastIndexOf(".")) : dlg2.FileName;
                string inputFilePath2 = without_ext2 + ".xlsx";
                byte[] excelBytes = reportViewer1.LocalReport.Render("ExcelOpenXml"); // render the report as an Excel file


                File.WriteAllBytes(inputFilePath2, excelBytes); // save the byte array as an Excel file
                File.SetAttributes(inputFilePath2, FileAttributes.Hidden);

                Excel excelApp = new Excel(); // create an instance of the Excel application
                Workbook workbook = excelApp.Workbooks.Open(inputFilePath2); // open the Excel file as a workbook
                Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1]; // get the first worksheet in the workbook

                // ...
                // Disable alerts and screen updates to improve performance
                excelApp.DisplayAlerts = false;
                excelApp.ScreenUpdating = false;

                try
                {
                    // Get the used range of the worksheet
                    Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;

                    // Replace all occurrences of '.' with ','
                    usedRange.Replace(".", ",");

                    // Save the changes
                    workbook.Save();

                }
                finally
                {

                }



                // ...
                // Enable alerts and screen updates
                excelApp.DisplayAlerts = true;
                excelApp.ScreenUpdating = true;

                // Close the workbook and quit Excel
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects to prevent memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                File.SetAttributes(inputFilePath2, FileAttributes.Normal);
                //---------------------------------------

            }
            //----------------
            button1.Enabled = button2.Enabled = button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = button2.Enabled = button3.Enabled = false;
            //----------------
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string without_ext = dlg.FileName.Contains(".") ? dlg.FileName.Substring(0, dlg.FileName.LastIndexOf(".")) : dlg.FileName;
                string inputFilePath = without_ext + ".docx";


                byte[] renderedBytes = reportViewer1.LocalReport.Render("WordOpenXML", null, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);


                using (FileStream stream = new FileStream(inputFilePath, FileMode.Create))
                {
                    stream.Write(renderedBytes, 0, renderedBytes.Length);
                }
                File.SetAttributes(inputFilePath, FileAttributes.Hidden);
                //===========================
                word wordApplication = new word();
                Document wordDocument = wordApplication.Documents.Open(inputFilePath);

                // Get the first section of the document
                Section firstSection = wordDocument.Sections[1];

                // Get the header of the first section
                Microsoft.Office.Interop.Word.HeaderFooter header = firstSection.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary];

                // Get the first paragraph in the header
                Paragraph firstParagraph = header.Range.Paragraphs[1];

                // Set the top spacing of the first line of the paragraph to 0.5 inches
                float topSpacingInInches = 0.5f;
                float topSpacingInPoints = topSpacingInInches * 72f;
                firstParagraph.SpaceBefore = topSpacingInPoints;


                Microsoft.Office.Interop.Word.Range footerRange = wordDocument.Sections[wordDocument.Sections.Count].Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                footerRange.Text = "01/01";
                footerRange.InsertAfter("\r");
                footerRange.InsertAfter("\r");
                footerRange.Font.Name = "Century Gothic";
                footerRange.Font.Size = 10;
                footerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;


                

                wordDocument.Save();

                wordDocument.Close();
                wordApplication.Quit();
                File.SetAttributes(inputFilePath, FileAttributes.Normal);
                //-----------------------
                
            }
            //----------------
            button1.Enabled = button2.Enabled = button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = button2.Enabled = button3.Enabled = false;
            //----------------
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PDF Files (*.pdf)|*.pdf";
            dlg.Title = "Enregistrer fichier PDF :";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string without_ext_for_pdf = dlg.FileName.Contains(".") ? dlg.FileName.Substring(0, dlg.FileName.LastIndexOf(".")) : dlg.FileName;

              
                    byte[] reportBytes = reportViewer1.LocalReport.Render("PDF");
                        File.WriteAllBytes(without_ext_for_pdf + ".pdf", reportBytes);
                }

                //----------------
            button1.Enabled = button2.Enabled = button3.Enabled = true;
            
        }       
    }
}
