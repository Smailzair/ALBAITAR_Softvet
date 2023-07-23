using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel.Application;
using System.Diagnostics;

namespace ALBAITAR_Softvet
{
    public partial class Print_ordonn : Form
    {
        ReportParameterCollection parrams;
        public Print_ordonn(ReportParameterCollection paramss)
        {
            InitializeComponent();
            //-------------------
            parrams = paramss;
            //---------------------

            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            //-------

            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            this.reportViewer1.ZoomPercent = 100;
            this.reportViewer1.ZoomMode = ZoomMode.Percent;
            //------------
        }

        private void Print_report_Load(object sender, EventArgs e)
        {
            //-----------
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            //----------

            load_report();
            //----------------           

        }
        private void load_report()
        {
            //if(parrams != null)
            //{
            //------------
            PageSettings pg_set = new PageSettings();
            if (radioButton3.Checked) // 1 ->>> A5
            {
                pg_set.PaperSize = new System.Drawing.Printing.PaperSize("A5", 580, 830);
                pg_set.Landscape = false;
                reportViewer1.LocalReport.ReportEmbeddedResource = "ALBAITAR_Softvet.Reports.Ordonnance_1_in_A5.rdlc";
            }
            else if (radioButton2.Checked)// 2 ->>> A4
            {
                pg_set.PaperSize = new System.Drawing.Printing.PaperSize("A4", 827, 1170);
                pg_set.Landscape = true;
                reportViewer1.LocalReport.ReportEmbeddedResource = "ALBAITAR_Softvet.Reports.Ordonnance_2_in_A4.rdlc";
            }
            else // 1 ->>> A4
            {
                pg_set.PaperSize = new System.Drawing.Printing.PaperSize("A4", 827, 1170);
                pg_set.Landscape = true;
                reportViewer1.LocalReport.ReportEmbeddedResource = "ALBAITAR_Softvet.Reports.Ordonnance_1_in_A4.rdlc";
            }
            pg_set.Margins.Right = 0;
            pg_set.Margins.Left = 0;
            pg_set.Margins.Top = 0;
            pg_set.Margins.Bottom = 0;
            this.reportViewer1.SetPageSettings(pg_set);
            reportViewer1.LocalReport.SetParameters(parrams);
            reportViewer1.RefreshReport();
            //---------------------------------------
            // }

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
                Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Open(inputFilePath2); // open the Excel file as a workbook
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1]; // get the first worksheet in the workbook

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
                word.Application wordApplication = new word.Application();
                word.Document wordDocument = wordApplication.Documents.Open(inputFilePath);

                // Get the first section of the document
                word.Section firstSection = wordDocument.Sections[1];

                // Get the header of the first section
                Microsoft.Office.Interop.Word.HeaderFooter header = firstSection.Headers[word.WdHeaderFooterIndex.wdHeaderFooterPrimary];

                // Get the first paragraph in the header
                word.Paragraph firstParagraph = header.Range.Paragraphs[1];

                // Set the top spacing of the first line of the paragraph to 0.5 inches
                float topSpacingInInches = 0.5f;
                float topSpacingInPoints = topSpacingInInches * 72f;
                firstParagraph.SpaceBefore = topSpacingInPoints;


                Microsoft.Office.Interop.Word.Range footerRange = wordDocument.Sections[wordDocument.Sections.Count].Footers[word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                footerRange.Text = "01/01";
                footerRange.InsertAfter("\r");
                footerRange.InsertAfter("\r");
                footerRange.Font.Name = "Century Gothic";
                footerRange.Font.Size = 10;
                footerRange.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;




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

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                load_report();
            }
        }
    }
}
