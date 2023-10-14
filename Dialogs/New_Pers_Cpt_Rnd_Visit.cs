using ALBAITAR_Softvet.Resources;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class New_Pers_Cpt_Rnd_Visit : Form
    {
        DataTable Races_Espèces = new DataTable();
        int anim_id = -1;
        public New_Pers_Cpt_Rnd_Visit(int Anim_ID)
        {
            InitializeComponent();
            //-------------------------
            if (Anim_ID > 0) { anim_id = Anim_ID; }
            //----------------------
            Races_Espèces.Columns.Add("ESPECE", typeof(string));
            Races_Espèces.Columns.Add("RACE", typeof(string));
            Races_Espèces.Rows.Add(new object[] { "Canine", "Husky Siberien" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Jack Russel" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Jagdterrier" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Komodor" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Korthals" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Labrador" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Levrier Afghan" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Levrier Espagnol" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Lhassa Apso" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Malamute de l'Alaska" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Pekinois" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Pinscher" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Pit Bull" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Podenco" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Pointer" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Rhodesian Ridgeback" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Rottweiler" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Saint-Bernard" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Saluki" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Samoyede" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Schnauzer" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Schnauzer Geant" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Schnauzer Moyen" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Schnauzer Nain" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Scottish Terrier" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Setter Anglais" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Setter Gordon" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Setter Irlandais" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Shar-pei" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Shiba Inu" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Shih Tzu" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Sloughi" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Spitz Japonais" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Spitz Nain" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Staffordshire Bull Terrier" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Teckel" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Teckel à poil dur" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Teckel à poil long" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Teckel Nain" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Terre Neuve" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Terrier Tibétain" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Westhiland West terrier" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Whippet" });
            Races_Espèces.Rows.Add(new object[] { "Canine", "Yorkshire Terrier" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "American Bobtail poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "American Curl poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "American Shorthair" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "American Wirehair (variété de l'American SH)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Angora turc" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Asian (variété de Burmese Européen)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Balinais (Siamois PL)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Bengal" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Bobtail Japonais poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Bombay" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "British Shorthair et Longhair" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Burmese Américain" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Burmese Anglais ou Européen" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Californian Rex (Cornish Rex PL)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Californian Spangled" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Ceylan" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Chartreux" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Chausie" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Cornish Rex" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Cymric (Manx PL)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Devon Rex" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Européen" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Exotic Shorthair (Persan PC)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "German Rex" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Havana Brown" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Highland Fold (Scottish PL)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Korat" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Kurilian Bobtail poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Laperm poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Maine Coon" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Mandarin (Oriental PL)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Manx" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Mau Égyptien" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Munchkin poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Nebelung (Russe PL)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Norvégien" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Ocicat" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Ojos Azules poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Oriental (Siamois coloré)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Persan" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Peterbald" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Pixie-Bob poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Ragdoll" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Russe" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Sacré de Birmanie (Birman)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Savannah" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Scottish Fold" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Selkirk Rex poil court et poil long" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Serengeti" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Siamois" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Sibérien" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Singapura" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Snowshoe" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Sokoké" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Somali (Abyssin PL)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Sphynx" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Sphynx du Don" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Thaï" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Tiffany (Burmese Européen ou Asian PL)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Tonkinois poil court et poil long (Tibétain)" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "Turc de Van" });
            Races_Espèces.Rows.Add(new object[] { "Feline", "York Chocolate" });
            Races_Espèces.Rows.Add(new object[] { "Oiseaux", "Perroquet" });
            Races_Espèces.Rows.Add(new object[] { "Oiseaux", "Perruche" });
            //---
            comboBox3.DataSource = Races_Espèces;
            comboBox3.ValueMember = "RACE";
            comboBox3.DisplayMember = "RACE";
            //-----------
            comboBox2.DataSource = Main_Frm.Main_Frm_animals_tbl;
            comboBox2.ValueMember = "ID";
            comboBox2.DisplayMember = "NME";
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox2_SelectedIndexChanged(null, null);
            //----------------------
            comboBox1.DataSource = Main_Frm.Main_Frm_clients_tbl;
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";
            comboBox1.AutoCompleteCustomSource.AddRange(Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Select(row => row.Field<string>("FULL_NME")).ToArray());
            if (Main_Frm.Main_Frm_clients_tbl.Rows.Count > 0) { comboBox1.SelectedIndex = 0; comboBox1_SelectedIndexChanged(null,null); }
            //--------------------

        }

        private void Add_Vente_Fact_Item_Load(object sender, EventArgs e)
        {
            if(anim_id > 0)
            {
                var ttt = Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(E=> (int)E["ID"] == anim_id);
                if (ttt.Any())
                {
                    comboBox1.SelectedValue = ttt.First()["CLIENT_ID"];
                }
                comboBox2.SelectedValue = anim_id;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = checkBox2.Checked;
        }
       

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = checkBox3.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox1.Checked;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //------------------------
            string prev_val = comboBox3.Text;
            string[] tmmmp = { "Canine", "Feline", "Oiseaux" };
            ((DataTable)comboBox3.DataSource).DefaultView.RowFilter = tmmmp.Contains(comboBox5.Text) ? "ESPECE LIKE '" + comboBox5.Text + "'" : "";
            comboBox3.Text = prev_val;
            //-----------------------
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int yy = -1;
            if (comboBox1.SelectedValue != null) { int.TryParse(comboBox1.SelectedValue.ToString(), out yy); }
            if (yy > 0)
            {
                var tt = Main_Frm.Main_Frm_clients_tbl.AsEnumerable().Where(B => B.Field<int>("ID") == yy);
                if (tt.Any())
                {
                    textBox4.Text = tt.First().Field<string>("NUM_PHONE");
                    textBox2.Text = string.Concat(
                        (tt.First().Field<string>("ADRESS").Length > 0 ? tt.First().Field<string>("ADRESS") : ""),
                        (tt.First().Field<string>("CITY").Length > 0 ? (", " + tt.First().Field<string>("CITY")) : ""),
                        (tt.First().Field<string>("WILAYA").Length > 0 ? (", " + tt.First().Field<string>("WILAYA")) : ""),
                        (tt.First().Field<string>("POSTAL_CODE").Length > 0 ? " (" + tt.First().Field<string>("POSTAL_CODE") + ")" : "")
                        );
                }
                //--------
                var SSS = Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(G => G.Field<int?>("CLIENT_ID") == yy);
                comboBox2.DataSource = SSS.Any() ? SSS.CopyToDataTable() : null;
                comboBox2.ValueMember = "ID";
                comboBox2.DisplayMember = "NME";
                comboBox2_SelectedIndexChanged(null, null);
            }
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            comboBox3.Text = "";
            checkBox3.Checked = false;
            //--------------------------------
            int yy2 = -1;
            if (comboBox2.SelectedValue != null) { int.TryParse(comboBox2.SelectedValue.ToString(), out yy2); }
            if (yy2 > 0)
            {
                var tt2 = Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Where(B => B.Field<int>("ID") == yy2);
                if (tt2.Any())
                {
                    textBox3.Text = tt2.First().Field<string>("NUM_IDENTIF");
                    comboBox4.Text = tt2.First().Field<string>("SEXE");
                    comboBox5.Text = tt2.First().Field<string>("ESPECE");
                    comboBox3.Text = tt2.First().Field<string>("RACE");
                    dateTimePicker2.Value = tt2.First().Field<DateTime?>("NISS_DATE") ?? dateTimePicker2.Value;
                    checkBox3.Checked = tt2.First().Field<DateTime?>("NISS_DATE") != null;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("CABINET", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString()),
                new ReportParameter("CABINET_TEL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString()),
                new ReportParameter("CABINET_EMAIL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString()),
                new ReportParameter("CABINET_ADRESS", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString()),

                new ReportParameter("Report_Date", dateTimePicker1.Value.ToString("yyyy-MM-dd")),
                new ReportParameter("REF", checkBox1.Checked ? textBox1.Text : ""),

                new ReportParameter("CLIENT_NME", comboBox1.Text),
                new ReportParameter("CLIENT_NUM_PHONE", textBox4.Text),
                new ReportParameter("CLIENT_ADRESS", textBox2.Text),

                new ReportParameter("REPORT_TXT", textBox5.Text)


            };

            if (checkBox2.Checked)
            {
                reportParameters.Add(new ReportParameter("SHOW_ANIM_INFOS", "YES"));
                reportParameters.Add(new ReportParameter("ANIM_NME", comboBox2.Text));
                reportParameters.Add(new ReportParameter("ANIM_IDENT", textBox3.Text));
                reportParameters.Add(new ReportParameter("ANIM_SEXE", comboBox4.Text));
                reportParameters.Add(new ReportParameter("ANIM_ESPECE", comboBox5.Text));
                reportParameters.Add(new ReportParameter("ANIM_RACE", comboBox3.Text));
                if (checkBox3.Checked) { reportParameters.Add(new ReportParameter("ANIM_NISS", dateTimePicker2.Value.ToString("dd/MM/yyyy"))); }
            }


            new Print_CRVP(reportParameters).ShowDialog();

        }

    }
}
