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
    public partial class Print_visites : Form
    {
        System.Data.DataTable vistes_infos;
        int IDDx = -1;
        bool is_anim = true;
        public Print_visites(int Anim_1_Or_Prop_2, int ID)
        {
            InitializeComponent();
            //-------------------
            IDDx = ID;
            is_anim = Anim_1_Or_Prop_2 == 1;
            //---------------------
            dateTimePicker1.ValueChanged -= dateTimePicker1_ValueChanged;
            dateTimePicker2.ValueChanged -= dateTimePicker2_ValueChanged;
            dateTimePicker3.ValueChanged -= dateTimePicker3_ValueChanged;
            radioButton1.CheckedChanged -= radioButton1_CheckedChanged;
            radioButton2.CheckedChanged -= radioButton2_CheckedChanged;
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
            //-----------------------
            dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
            dateTimePicker2.MinDate = dateTimePicker1.Value;
            dateTimePicker3.MinDate = dateTimePicker2.Value;
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
        }

        private void Print_report_Load(object sender, EventArgs e)
        {
            Main_Frm.Main_Frm_animals_tbl = PreConnection.Load_data("SELECT  " +
"    tb1.*,  " +
"    tb2.CLIENT_FULL_NME,  " +
"    tb4.MALAD_NME,  " +
"    tb4.LAST_MALAD_DATE  " +
"FROM  " +
"    tb_animaux tb1  " +
"LEFT JOIN  " +
"    (SELECT  " +
"        ID,  " +
"        CONCAT(FAMNME,' ',NME) AS CLIENT_FULL_NME  " +
"     FROM  " +
"        tb_clients) tb2  " +
"ON  " +
"    tb1.CLIENT_ID = tb2.ID  " +
"LEFT JOIN  " +
"    (SELECT  " +
"        tb_maladies.ANIM_ID,  " +
"        tb_maladies.START_DATE AS LAST_MALAD_DATE,  " +
"        tb_maladies.MALAD_NME  " +
"     FROM  " +
"        tb_maladies  " +
"     JOIN  " +
"        (SELECT  " +
"            ANIM_ID,  " +
"            MAX(START_DATE) AS max_start_date  " +
"         FROM  " +
"            tb_maladies  " +
"         WHERE  " +
"            (START_DATE <= current_timestamp() OR START_DATE IS NULL)  " +
"            AND (ESTIM_END_DATE >= current_timestamp() OR ESTIM_END_DATE IS NULL)  " +
"         GROUP BY  " +
"            ANIM_ID) tb3  " +
"     ON  " +
"        tb_maladies.ANIM_ID = tb3.ANIM_ID  " +
"        AND tb_maladies.START_DATE = tb3.max_start_date) tb4  " +
"ON  " +
"    tb4.ANIM_ID = tb1.ID ORDER BY NME; ");
            //----------------
            comboBox1.DataSource = Main_Frm.Main_Frm_animals_tbl;
            comboBox1.DisplayMember = "NME";
            comboBox1.ValueMember = "ID";
            //---------------
            comboBox2.DataSource = Main_Frm.Main_Frm_clients_tbl;
            comboBox2.DisplayMember = "FULL_NME";
            comboBox2.ValueMember = "ID";
            //-----------
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            //----------
            if (IDDx > 0)
            {
                if (is_anim)
                {
                    comboBox1.SelectedValue = IDDx;
                }
                else
                {
                    radioButton2.Checked = true;
                    comboBox2.SelectedValue = IDDx;
                }
            }

            load_report();
            //----------------
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged;
            dateTimePicker3.ValueChanged += dateTimePicker3_ValueChanged;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;

        }
        private void load_report()
        {
            vistes_infos = PreConnection.Load_data("SELECT tb1.*,tb2.NME AS ANIM_NME,tb2.CLIENT_ID,tb2.NUM_IDENTIF,tb2.SEXE,tb2.ESPECE,tb2.RACE,(SELECT AI.POIDS FROM tb_poids AI WHERE AI.ANIM_ID = tb1.ANIM_ID AND AI.DATETIME < tb1.DATETIME AND AI.POIDS > 0 ORDER BY AI.DATETIME DESC LIMIT 1) AS POIDS FROM tb_visites tb1 LEFT JOIN tb_animaux tb2 ON tb1.ANIM_ID = tb2.ID" + (groupBox1.Enabled ? (" WHERE DATETIME >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00:00' AND DATETIME <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59'") : "") + " ORDER BY DATETIME ASC;");
            ReportParameterCollection reportParameters = new ReportParameterCollection();
            DataTable filtred_data = new DataTable();
            //-----------
            reportParameters.Add(new ReportParameter("CABINET", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString()));
            reportParameters.Add(new ReportParameter("CABINET_TEL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString()));
            reportParameters.Add(new ReportParameter("CABINET_EMAIL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString()));
            reportParameters.Add(new ReportParameter("CABINET_ADRESS", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString()));
            reportParameters.Add(new ReportParameter("DATE_OF", groupBox1.Enabled ? dateTimePicker1.Value.ToString("dd/MM/yyyy") : "1900-01-01"));
            reportParameters.Add(new ReportParameter("DATE_TO", groupBox1.Enabled ? dateTimePicker2.Value.ToString("dd/MM/yyyy") : "1900-01-01"));

            reportParameters.Add(new ReportParameter("Report_Date", checkBox1.Checked ? dateTimePicker3.Value.ToString("dd/MM/yyyy") : "1900-01-01"));

            //----------

            if (vistes_infos != null)
            {
                if (vistes_infos.Rows.Count > 0)
                {



                    if (radioButton1.Checked) //par animal
                    {
                        var ggg = vistes_infos.AsEnumerable().Where(W => W.Field<int?>("ANIM_ID") == (comboBox1.SelectedValue != null && comboBox1.SelectedValue != DBNull.Value ? (int)comboBox1.SelectedValue : -1));
                        if (ggg.Any())
                        {
                            filtred_data = ggg.CopyToDataTable();
                        }
                    }
                    else //par propr.
                    {
                        var ggg = vistes_infos.AsEnumerable().Where(W => W.Field<int?>("CLIENT_ID") == (comboBox2.SelectedValue != null && comboBox2.SelectedValue != DBNull.Value ? (int)comboBox2.SelectedValue : -1));
                        if (ggg.Any())
                        {
                            filtred_data = ggg.CopyToDataTable();
                        }
                    }
                }
            }
            //------
            //--------------
            int clt_int = -1;
            clt_int = filtred_data.Rows.Count > 0 ? (int)filtred_data.Rows[0]["CLIENT_ID"] : -1;
            if (clt_int == -1)
            {
                if (radioButton1.Checked) //par animal
                {
                    var ggg = Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(W => W.Field<int?>("ID") == (comboBox1.SelectedValue != null && comboBox1.SelectedValue != DBNull.Value ? (int)comboBox1.SelectedValue : -1));
                    if (ggg.Any())
                    {
                        clt_int = ggg.First().Field<int?>("CLIENT_ID") ?? -1;
                    }
                }
                else //par propr.
                {
                    clt_int = comboBox2.SelectedValue != null && comboBox2.SelectedValue != DBNull.Value ? (int)comboBox2.SelectedValue : -1;
                }
            }
            var ff = Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Where(G => G.Field<int>("ID") == clt_int);
            if (ff.Any())
            {
                reportParameters.Add(new ReportParameter("CLIENT_NME", ff.First().Field<string>("FULL_NME")));
                reportParameters.Add(new ReportParameter("CLIENT_NUM_CNI", ff.First().Field<string>("NUM_CNI")));
                reportParameters.Add(new ReportParameter("CLIENT_ADRESS", ff.First().Field<string>("ADRESS")));
                reportParameters.Add(new ReportParameter("CLIENT_CITY", ff.First().Field<string>("CITY")));
                reportParameters.Add(new ReportParameter("CLIENT_WILAYA", ff.First().Field<string>("WILAYA")));
                reportParameters.Add(new ReportParameter("CLIENT_NUM_PHONE", ff.First().Field<string>("NUM_PHONE")));
                reportParameters.Add(new ReportParameter("CLIENT_EMAIL", ff.First().Field<string>("EMAIL")));
            }
            else
            {
                reportParameters.Add(new ReportParameter("CLIENT_NME", ""));
                reportParameters.Add(new ReportParameter("CLIENT_NUM_CNI", ""));
                reportParameters.Add(new ReportParameter("CLIENT_ADRESS", ""));
                reportParameters.Add(new ReportParameter("CLIENT_CITY", ""));
                reportParameters.Add(new ReportParameter("CLIENT_WILAYA", ""));
                reportParameters.Add(new ReportParameter("CLIENT_NUM_PHONE", ""));
                reportParameters.Add(new ReportParameter("CLIENT_EMAIL", ""));
            }
            //------------
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = null;
            reportViewer1.LocalReport.ReportEmbeddedResource = "ALBAITAR_Softvet.Reports.visite_report.rdlc";
            reportViewer1.LocalReport.SetParameters(reportParameters);
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", filtred_data));
            reportViewer1.RefreshReport();
            //---------------------------------------
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
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1]; // get the first worksheet in the workbook

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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.ValueChanged -= dateTimePicker2_ValueChanged;
            dateTimePicker3.ValueChanged -= dateTimePicker3_ValueChanged;
            dateTimePicker2.MinDate = dateTimePicker1.Value;
            dateTimePicker3.MinDate = dateTimePicker2.Value;
            dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged;
            dateTimePicker3.ValueChanged += dateTimePicker3_ValueChanged;
            load_report();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.ValueChanged -= dateTimePicker1_ValueChanged;
            dateTimePicker3.ValueChanged -= dateTimePicker3_ValueChanged;
            dateTimePicker1.MaxDate = dateTimePicker2.Value;
            dateTimePicker3.MinDate = dateTimePicker2.Value;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            dateTimePicker3.ValueChanged += dateTimePicker3_ValueChanged;
            load_report();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioButton1.CheckedChanged -= radioButton1_CheckedChanged;
            radioButton1.Checked = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            load_report();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                load_report();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioButton2.CheckedChanged -= radioButton2_CheckedChanged;
            radioButton2.Checked = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            load_report();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                load_report();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Enabled = checkBox1.Checked;
            load_report();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            load_report();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox2.Checked;
            load_report();
        }
    }
}
