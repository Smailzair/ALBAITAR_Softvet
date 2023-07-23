using ALBAITAR_Softvet.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class New_Ordonnance : Form
    {
        DataTable Races_Espèces = new DataTable();
        public static string Item_to_add;
        public New_Ordonnance()
        {
            InitializeComponent();
            //-------------------------        
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
            comboBox2.AutoCompleteCustomSource.AddRange(Main_Frm.Main_Frm_animals_tbl.AsEnumerable().Select(row => row.Field<string>("NME")).ToArray());
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

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = checkBox2.Checked;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow rw in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(rw);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Item_to_add = "";
            new Add_Ordonnance_Item().ShowDialog();
            if(Item_to_add.Trim().Length > 0 && Item_to_add != "--")
            {
                dataGridView1.Rows.Add(Item_to_add);
            }
            
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
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
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
    }
}
