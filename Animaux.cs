using ALBAITAR_Softvet.Dialogs;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xamarin.Forms.Internals;
using Excc = Microsoft.Office.Interop.Excel;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Animaux : Form
    {
        public static int ID_to_selectt = -1;
        public static int visite_idd = -1;
        DataTable clients;
        DataTable animaux;
        DataTable poids_tbl = new DataTable();
        DataTable maladies_tbl = new DataTable();
        List<string> full_nme_clients;
        bool Is_New = true;
        bool Is_New_Visite = true;
        DataTable Races_Espèces = new DataTable();
        DataTable chosen_anim_from_search;
        int prev_rw_idx = -1;
        int prev_col_idx = -1;

        int spliter_panel1_wdth = 0;
        int frm_width = 0;
        int splitter_prev_dist = 0;

        string[] default_maladies;
        public Animaux(int ID_to_select, int visite_id)
        {
            InitializeComponent();
            ID_to_selectt = ID_to_select;
            visite_idd = visite_id;

            frm_width = this.Width;
            spliter_panel1_wdth = splitContainer1.Panel1.Width;
            splitter_prev_dist = splitContainer1.SplitterDistance;
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
            //----------------------
            comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = 0;
            //----------------------
            clients = PreConnection.Load_data_keeping_duplicates("SELECT *,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            comboBox1.DataSource = clients;
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";
            if (clients.Rows.Count > 0) { comboBox1.SelectedIndex = 0; }
            full_nme_clients = new List<string>();
            clients.Rows.Cast<DataRow>().ToList().ForEach(clt =>
            {
                full_nme_clients.Add((string)clt["FULL_NME"]);
            });
            //--------------------
            comboBox5.AutoCompleteCustomSource.AddRange(clients.AsEnumerable().Select(row => row.Field<string>("FULL_NME")).ToArray());
            //---------------------
            default_maladies = new string[] {
                "- Tous -",
                "Anémie infectieuse des équidés",
                "Avortement enzootique des brebis",
                "Bronchite infectieuse aviaire",
                "Brucellose dans les espèces bovine, ovine, caprine",
                "Bursite infectieuse (Gomboro)",
                "Campylobactériose génitale bovine",
                "Charbon Symptomatique",
                "Choléra aviaire",
                "Clavelée et Variole caprine",
                "Cysticercose",
                "Dourine",
                "Echinococcose/Hydatidose",
                "Encéphalopathie spongiforme des bovins",
                "Fièvre Aphteuse",
                "Fièvre catarrhale du mouton",
                "Fièvre charbonneuse chez toutes les espèces mammifères",
                "Fièvre de la vallée du Rift",
                "Fièvre Q",
                "Gale des équidés",
                "Leishmaniose",
                "Leptospirose bovine",
                "Leucose bovine enzootique",
                "Leucoses aviaires",
                "Loque, la Nosémose et l’acariose des abeilles",
                "Marek",
                "New-castle",
                "Maladie hémorragique virale du lapin",
                "Métrite contagieuse équine",
                "Morve",
                "Myxomatose",
                "Ornithose/Psittacoses",
                "Paratuberculose",
                "Péripneumonie contagieuse bovine",
                "Peste aviaire",
                "Peste Bovine",
                "Peste des petits ruminants",
                "peste Équine",
                "Rage dans toutes les espèces",
                "Rhinotrachéite infectieuse bovine",
                "Salmonelloses aviaires à Salmonella : pullorum-gallinarum",
                "Trichomonose bovine",
                "Trypanosomose des camelins à Tevansi (surra)",
                "Tuberculose bovine",
                "Tularémie",
                "Variole aviaire",
                "Variole cameline",
                "Varoise des abeilles"
            };
            comboBox7.SelectedIndexChanged -= comboBox7_SelectedIndexChanged;
            comboBox7.SelectedIndex = 0;
            comboBox7.SelectedIndexChanged += comboBox7_SelectedIndexChanged;
            //---------------------
            Load_anims_from_DB();
            //---------------------


        }
        private void Load_anims_from_DB()
        {
            int fd = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].Index : 99999999;
            animaux = PreConnection.Load_data_keeping_duplicates("SELECT * FROM tb_animaux;");
            dataGridView1.DataSource = animaux;
            if (dataGridView1.Rows.Count > fd)
            { dataGridView1.ClearSelection(); dataGridView1.Rows[fd].Selected = true; }
            else if (dataGridView1.Rows.Count > 0)
            { dataGridView1.ClearSelection(); dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true; }

            Load_malad_1();
            //------------------
            reload_cbx6_data();
            //-----------------
            anim_filter();
        }
        private void Load_selected_anim_fields()
        {
            openFileDialog1.FileName = "";

            label13.Visible = false;
            textBox2.Validated -= textBox2_Validated;
            textBox3.Validated -= textBox2_Validated;
            comboBox1.Validating -= comboBox1_Validating;
            comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
            pictureBox1.Image = Properties.Resources.MODIF;
            pictureBox2.Image = null;
            button7.Visible = false;
            button9.Visible = false;
            button8.Visible = true;
            panel1.Visible = false;
            panel2.Visible = false;


            //----------------------------------------------                
            dateTimePicker3.Value = (DateTime)dataGridView1.SelectedRows[0].Cells["DATE_ADDED"].Value;
            textBox3.Text = (string)dataGridView1.SelectedRows[0].Cells["NME"].Value;
            textBox2.Text = (string)dataGridView1.SelectedRows[0].Cells["NUM_IDENTIF"].Value;
            textBox4.Text = (string)dataGridView1.SelectedRows[0].Cells["NUM_PASSPORT"].Value;
            comboBox1.SelectedValue = (int)dataGridView1.SelectedRows[0].Cells["CLIENT_ID"].Value;
            comboBox2.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["ESPECE"].Value;
            comboBox3.Text = (string)dataGridView1.SelectedRows[0].Cells["RACE"].Value;
            comboBox4.SelectedItem = (string)dataGridView1.SelectedRows[0].Cells["SEXE"].Value;
            checkBox2.Checked = dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value != DBNull.Value;
            dateTimePicker1.Value = dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value != DBNull.Value ? (DateTime)dataGridView1.SelectedRows[0].Cells["NISS_DATE"].Value : (DateTime)dataGridView1.SelectedRows[0].Cells["DATE_ADDED"].Value;// DateTime.Now.Date;
            textBox6.Text = (string)dataGridView1.SelectedRows[0].Cells["ROBE"].Value;
            textBox8.Text = (string)dataGridView1.SelectedRows[0].Cells["OBSERVATIONS"].Value;
            checkBox1.Checked = (SByte)dataGridView1.SelectedRows[0].Cells["IS_RADIATED"].Value != 0;
            dateTimePicker2.Value = dataGridView1.SelectedRows[0].Cells["RADIATION_DATE"].Value != DBNull.Value ? (DateTime)dataGridView1.SelectedRows[0].Cells["RADIATION_DATE"].Value : (DateTime)dataGridView1.SelectedRows[0].Cells["DATE_ADDED"].Value;// DateTime.Now.Date;
            textBox5.Text = (string)dataGridView1.SelectedRows[0].Cells["RADIATION_CAUSES"].Value;
            pictureBox2.Image = dataGridView1.SelectedRows[0].Cells["picture"].Value != DBNull.Value ? PreConnection.ByteArrayToImage((byte[])dataGridView1.SelectedRows[0].Cells["picture"].Value) : (Properties.Settings.Default.Use_animals_logo ? (Image)Properties.Resources.ResourceManager.GetObject(comboBox2.Text) : null);
            button7.Visible = dataGridView1.SelectedRows[0].Cells["picture"].Value != DBNull.Value;
            //----------------------------------------------
            textBox2.Validated += textBox2_Validated;
            textBox3.Validated += textBox2_Validated;
            comboBox1.Validating += comboBox1_Validating;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;


        }

        private void verif_if_déja_exist_animal()
        {

            bool exist = false;

            if (animaux != null && (Is_New || (!Is_New && dataGridView1.SelectedRows.Count > 0)))
            {

                int cntt = animaux.Rows.Cast<DataRow>().Where(zz =>

                ((string)zz["NUM_IDENTIF"] == textBox2.Text.Trim().Replace(" ", "") &&
                zz["NME"].ToString().ToLower() == textBox3.Text.ToLower() &&
                (zz["ESPECE"] != DBNull.Value ? zz["ESPECE"].ToString() : "--").ToLower() == comboBox2.Text.ToLower() &&
                (zz["RACE"] != DBNull.Value ? zz["RACE"].ToString() : "--").ToLower() == comboBox3.Text.ToLower() &&
                (zz["SEXE"] != DBNull.Value ? zz["SEXE"].ToString() : "--").ToLower() == comboBox4.Text.ToLower()) &&
                int.Parse(zz["CLIENT_ID"].ToString()) == (comboBox1.SelectedValue != null ? (int)comboBox1.SelectedValue : -2) &&
                (!Is_New && dataGridView1.SelectedRows.Count > 0 ? (int)zz["ID"] != (int)dataGridView1.SelectedRows[0].Cells["ID"].Value : true)

                ).ToList().Count();


                exist = cntt > 0;


            }

            label13.Visible = exist;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            anim_filter();
            textBox1.Focus();




        }
        DataTable filtred_maladies_tbl;
        private void anim_filter()
        {
            filtred_maladies_tbl = new DataTable();
            string fltr = textBox1.Text != string.Empty ? String.Format("NME LIKE '%{0}%'", textBox1.Text) : "";
            if (checkBox3.Checked)
            {

                fltr += string.IsNullOrWhiteSpace(fltr) ? "" : " AND ";
                string iddx = "";
                if (radioButton1.Checked) //Actually
                {
                    if (comboBox6.Text.Contains("Tous") || string.IsNullOrWhiteSpace(comboBox6.Text))
                    {
                        if (comboBox7.SelectedIndex > 0)
                        {
                            //maladies_tbl.AsEnumerable().Where(V =>
                            //(V["MALAD_LEVEL"] != DBNull.Value ? (string)V["MALAD_LEVEL"] == comboBox7.Text : false) &&
                            //(V["START_DATE"] != DBNull.Value ? (DateTime)V["START_DATE"] <= DateTime.Now : true) &&
                            //(V["ESTIM_END_DATE"] != DBNull.Value ? (DateTime)V["ESTIM_END_DATE"] >= DateTime.Now : true))
                            //    .Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            //{
                            //    iddx += "," + H.ToString();
                            //});


                            var FFF = maladies_tbl.AsEnumerable().Where(V =>
                             (V["MALAD_LEVEL"] != DBNull.Value ? (string)V["MALAD_LEVEL"] == comboBox7.Text : false) &&
                             (V["START_DATE"] != DBNull.Value ? (DateTime)V["START_DATE"] <= DateTime.Now : true) &&
                             (V["ESTIM_END_DATE"] != DBNull.Value ? (DateTime)V["ESTIM_END_DATE"] >= DateTime.Now : true));

                            if (FFF.Any()) { filtred_maladies_tbl = FFF.CopyToDataTable(); }
                        }
                        else
                        {
                            //maladies_tbl.AsEnumerable().Where(V => 
                            //(V["START_DATE"] != DBNull.Value ? (DateTime)V["START_DATE"] <= DateTime.Now : true) && 
                            //(V["ESTIM_END_DATE"] != DBNull.Value ? (DateTime)V["ESTIM_END_DATE"] >= DateTime.Now : true))
                            //    .Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            //{
                            //    iddx += "," + H.ToString();
                            //});



                            var FFF = maladies_tbl.AsEnumerable().Where(V =>
                            (V["START_DATE"] != DBNull.Value ? (DateTime)V["START_DATE"] <= DateTime.Now : true) &&
                            (V["ESTIM_END_DATE"] != DBNull.Value ? (DateTime)V["ESTIM_END_DATE"] >= DateTime.Now : true));

                            if (FFF.Any()) { filtred_maladies_tbl = FFF.CopyToDataTable(); }
                        }
                    }
                    else
                    {
                        if (comboBox7.SelectedIndex > 0)
                        {
                            //maladies_tbl.AsEnumerable().Where(V => 
                            //(V["MALAD_NME"] != DBNull.Value ? (string)V["MALAD_NME"] == comboBox6.Text : false) && 
                            //(V["MALAD_LEVEL"] != DBNull.Value ? (string)V["MALAD_LEVEL"] == comboBox7.Text : false) && 
                            //(V["START_DATE"] != DBNull.Value ? (DateTime)V["START_DATE"] <= DateTime.Now : true) && 
                            //(V["ESTIM_END_DATE"] != DBNull.Value ? (DateTime)V["ESTIM_END_DATE"] >= DateTime.Now : true))
                            //    .Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            //{
                            //    iddx += "," + H.ToString();
                            //});


                            var FFF = maladies_tbl.AsEnumerable().Where(V =>
                            (V["MALAD_NME"] != DBNull.Value ? (string)V["MALAD_NME"] == comboBox6.Text : false) &&
                            (V["MALAD_LEVEL"] != DBNull.Value ? (string)V["MALAD_LEVEL"] == comboBox7.Text : false) &&
                            (V["START_DATE"] != DBNull.Value ? (DateTime)V["START_DATE"] <= DateTime.Now : true) &&
                            (V["ESTIM_END_DATE"] != DBNull.Value ? (DateTime)V["ESTIM_END_DATE"] >= DateTime.Now : true));

                            if (FFF.Any()) { filtred_maladies_tbl = FFF.CopyToDataTable(); }


                        }
                        else
                        {
                            //maladies_tbl.AsEnumerable().Where(V => 
                            //(V["MALAD_NME"] != DBNull.Value ? (string)V["MALAD_NME"] == comboBox6.Text : false) && 
                            //(V["START_DATE"] != DBNull.Value ? (DateTime)V["START_DATE"] <= DateTime.Now : true) && 
                            //(V["ESTIM_END_DATE"] != DBNull.Value ? (DateTime)V["ESTIM_END_DATE"] >= DateTime.Now : true))
                            //    .Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            //{
                            //    iddx += "," + H.ToString();
                            //});



                            var FFF = maladies_tbl.AsEnumerable().Where(V =>
                            (V["MALAD_NME"] != DBNull.Value ? (string)V["MALAD_NME"] == comboBox6.Text : false) &&
                            (V["START_DATE"] != DBNull.Value ? (DateTime)V["START_DATE"] <= DateTime.Now : true) &&
                            (V["ESTIM_END_DATE"] != DBNull.Value ? (DateTime)V["ESTIM_END_DATE"] >= DateTime.Now : true));

                            if (FFF.Any()) { filtred_maladies_tbl = FFF.CopyToDataTable(); }
                        }
                    }
                }
                else
                {
                    if (comboBox6.Text.Contains("Tous") || string.IsNullOrWhiteSpace(comboBox6.Text))
                    {
                        if (comboBox7.SelectedIndex > 0)
                        {
                            //maladies_tbl.AsEnumerable().Where(V => 
                            //V["MALAD_LEVEL"] != DBNull.Value ? (string)V["MALAD_LEVEL"] == comboBox7.Text : false)
                            //    .Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            //{
                            //    iddx += "," + H.ToString();
                            //});



                            var FFF = maladies_tbl.AsEnumerable().Where(V =>
                            V["MALAD_LEVEL"] != DBNull.Value ? (string)V["MALAD_LEVEL"] == comboBox7.Text : false);

                            if (FFF.Any()) { filtred_maladies_tbl = FFF.CopyToDataTable(); }
                        }
                        else
                        {
                            //maladies_tbl.AsEnumerable().Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            //{
                            //    iddx += "," + H.ToString();
                            //});

                            filtred_maladies_tbl = maladies_tbl.Copy();

                        }
                    }
                    else
                    {
                        if (comboBox7.SelectedIndex > 0)
                        {
                            //maladies_tbl.AsEnumerable().Where(V => 
                            //(V["MALAD_NME"] != DBNull.Value ? (string)V["MALAD_NME"] == comboBox6.Text : false) && 
                            //(V["MALAD_LEVEL"] != DBNull.Value ? (string)V["MALAD_LEVEL"] == comboBox7.Text : false))
                            //    .Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            //{
                            //    iddx += "," + H.ToString();
                            //});


                            var FFF = maladies_tbl.AsEnumerable().Where(V =>
                            (V["MALAD_NME"] != DBNull.Value ? (string)V["MALAD_NME"] == comboBox6.Text : false) &&
                            (V["MALAD_LEVEL"] != DBNull.Value ? (string)V["MALAD_LEVEL"] == comboBox7.Text : false));

                            if (FFF.Any()) { filtred_maladies_tbl = FFF.CopyToDataTable(); }
                        }
                        else
                        {
                            //maladies_tbl.AsEnumerable().Where(V => 
                            //V["MALAD_NME"] != DBNull.Value ? (string)V["MALAD_NME"] == comboBox6.Text : false)
                            //    .Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            //{
                            //    iddx += "," + H.ToString();
                            //});



                            var FFF = maladies_tbl.AsEnumerable().Where(V =>
                            V["MALAD_NME"] != DBNull.Value ? (string)V["MALAD_NME"] == comboBox6.Text : false);

                            if (FFF.Any()) { filtred_maladies_tbl = FFF.CopyToDataTable(); }
                        }
                    }
                }
                //------------------------------
                if (filtred_maladies_tbl != null)
                {
                    if (filtred_maladies_tbl.Rows.Count > 0)
                    {
                        filtred_maladies_tbl.AsEnumerable()
                                .Select(F => F["ANIM_ID"]).Distinct().ToList().ForEach(H =>
                            {
                                iddx += "," + H.ToString();
                            });
                    }

                }


                iddx = iddx.Length > 0 ? iddx.Substring(1) : string.Empty;

                if (iddx.Length > 0)
                {
                    fltr += !string.IsNullOrWhiteSpace(iddx) ? "ID IN (" + iddx + ")" : "";
                }
                else if ((comboBox6.Text.Contains("Tous") || string.IsNullOrWhiteSpace(comboBox6.Text)) && comboBox7.SelectedIndex == 0 && radioButton2.Checked)
                {
                    fltr = "";
                }
                else
                {
                    fltr = "ID = -1";
                }

                //-------------
                label25.Text = comboBox6.Text;
                label26.Text = comboBox7.SelectedIndex > 0 ? comboBox7.Text : "- Tous -";
                label27.Text = radioButton1.Checked ? "Oui" : "- Tous -";
            }
            else
            {
                label25.Text = label26.Text = label27.Text = "- Tous -";
                filtred_maladies_tbl = maladies_tbl.Copy();
            }

            Debug.WriteLine(">>>>>>>>>>>>>>> filtred_maladies_tbl >>>>>>>>>>>>>>> " + filtred_maladies_tbl.Rows.Count);
            Debug.WriteLine(">>>>>>>>>>>>>>> maladies_tbl >>>>>>>>>>>>>>> " + maladies_tbl.Rows.Count);

            ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = fltr;
            dataGridView1.Columns["ID"].Visible = false;


            label19.Visible = dataGridView1.Rows.Count == 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool autorisat = Properties.Settings.Default.Last_login_is_admin || (Is_New && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20001" && (Int32)QQ[3] == 1).Count() > 0) || (!Is_New && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20003" && (Int32)QQ[3] == 1).Count() > 0);
            if (autorisat)
            {
                bool all_ready = true;
                textBox3.BackColor = textBox3.Text.TrimStart().TrimEnd() != string.Empty ? SystemColors.Window : Color.LightCoral;
                comboBox1.BackColor = comboBox1.Text.TrimStart().TrimEnd() != string.Empty && comboBox1.SelectedValue != null ? SystemColors.Window : Color.LightCoral;
                panel2.Visible = comboBox2.Text.Length == 0 || comboBox2.Text == "--";//Espèce (Obligé !)
                panel1.Visible = comboBox4.Text.Length == 0 || comboBox4.Text == "--"; //Sexe (Obligé !)
                int mm = 0;
                if (Is_New) { mm = animaux.Rows.Cast<DataRow>().Where(XX => XX["NUM_IDENTIF"].ToString().Length > 0 && (string)XX["NUM_IDENTIF"] == textBox2.Text.Trim().Replace(" ", "")).ToList().Count; }
                else { mm = animaux.Rows.Cast<DataRow>().Where(XX => (int)XX["ID"] != (int)dataGridView1.SelectedRows[0].Cells["ID"].Value && XX["NUM_IDENTIF"].ToString().Length > 0 && (string)XX["NUM_IDENTIF"] == textBox2.Text.Trim().Replace(" ", "")).ToList().Count; }

                if (textBox2.Text.Trim().Replace(" ", "").Length == 0 || mm > 0)
                {
                    textBox2.BackColor = Color.LightCoral;
                    if (textBox2.Text.Trim().Replace(" ", "").Length > 0) { MessageBox.Show("Ce N° d'identification déja existe pour un autre animal,\n\nVeuillez le changer puis réesayer.\n\n", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    else { MessageBox.Show("Le N° d'identification est obligé,\n\nVeuillez le saisir puis réesayer.\n\n", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                }
                else
                {
                    all_ready &= comboBox1.BackColor == SystemColors.Window;
                    all_ready &= textBox3.Text.TrimStart().TrimEnd() != string.Empty;
                    all_ready &= !panel2.Visible;
                    all_ready &= !panel1.Visible;
                    if (all_ready && label13.Visible) { all_ready &= MessageBox.Show("Ce animal déja existe pour ce client,\n\nVoulez vous continuer comme meme ?\n\n", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes; }
                    //-------------
                    label12.Visible = !all_ready;
                    //-------------
                    if (all_ready)
                    {
                        if (Is_New) //INSERT
                        {
                            byte[] imageData = File.Exists(openFileDialog1.FileName) ? File.ReadAllBytes(openFileDialog1.FileName) : null;
                            List<string> Cols = new List<string> {"DATE_ADDED",
"NME",
"NUM_IDENTIF",
"NUM_PASSPORT",
"CLIENT_ID",
"ESPECE",
"RACE",
"SEXE",
"NISS_DATE",
"ROBE",
"OBSERVATIONS",
"IS_RADIATED",
"RADIATION_DATE",
"RADIATION_CAUSES" };
                            if (File.Exists(openFileDialog1.FileName)) { Cols.Add("PICTURE"); }
                            List<object> varrs = new List<object>{
                                dateTimePicker3.Value.Date,
textBox3.Text,
textBox2.Text,
textBox4.Text,
comboBox1.SelectedValue,
comboBox2.Text,
comboBox3.Text,
comboBox4.Text,
(checkBox2.Checked ? dateTimePicker1.Value.Date : (DateTime?)null),
textBox6.Text,
textBox8.Text,
checkBox1.Checked,
(checkBox1.Checked ? dateTimePicker2.Value.Date : (DateTime?)null),
(checkBox1.Checked ? textBox5.Text : "")
};
                            if (File.Exists(openFileDialog1.FileName)) { varrs.Add(imageData); }
                            PreConnection.Excut_Cmd(1, "tb_animaux", Cols, varrs, null, null, null);
                        }
                        else //UPDATE
                        {

                            byte[] imageData = File.Exists(openFileDialog1.FileName) ? File.ReadAllBytes(openFileDialog1.FileName) : null;
                            List<string> Cols = new List<string> {"DATE_ADDED",
"NME",
"NUM_IDENTIF",
"NUM_PASSPORT",
"CLIENT_ID",
"ESPECE",
"RACE",
"SEXE",
"NISS_DATE",
"ROBE",
"OBSERVATIONS",
"IS_RADIATED",
"RADIATION_DATE",
"RADIATION_CAUSES" };
                            if (File.Exists(openFileDialog1.FileName) || !button7.Visible) { Cols.Add("PICTURE"); }
                            List<object> varrs = new List<object>{
                                dateTimePicker3.Value.Date,
textBox3.Text,
textBox2.Text,
textBox4.Text,
comboBox1.SelectedValue,
comboBox2.Text,
comboBox3.Text,
comboBox4.Text,
(checkBox2.Checked ? dateTimePicker1.Value.Date : (DateTime?)null),
textBox6.Text,
textBox8.Text,
checkBox1.Checked,
(checkBox1.Checked ? dateTimePicker2.Value.Date : (DateTime?)null),
(checkBox1.Checked ? textBox5.Text : "")
};
                            if (File.Exists(openFileDialog1.FileName))
                            {
                                varrs.Add(imageData);
                            }
                            else if (!button7.Visible)
                            {
                                varrs.Add(DBNull.Value);
                            }
                            PreConnection.Excut_Cmd(2, "tb_animaux", Cols, varrs, "ID = @ID", new List<string> { "ID" }, new List<object> { dataGridView1.SelectedRows[0].Cells["ID"].Value });

                            //================================== OLD CODE !! ==========================================================================================================
                            //byte[] imageData = File.Exists(openFileDialog1.FileName) ? File.ReadAllBytes(openFileDialog1.FileName) : null;
                            //string insert_cmnd = "UPDATE `tb_animaux` SET "
                            //        + "`DATE_ADDED` = '" + dateTimePicker3.Value.Date.ToString("yyyy-MM-dd") + "',"
                            //        + "`NME` = '" + textBox3.Text.Replace("'", "''") + "',"
                            //        + "`NUM_IDENTIF` = '" + textBox2.Text.Replace("'", "''") + "',"
                            //        + "`NUM_PASSPORT` = '" + textBox4.Text.Replace("'", "''") + "',"
                            //        + "`CLIENT_ID` = " + comboBox1.SelectedValue + ","
                            //        + "`ESPECE` = '" + comboBox2.Text.Replace("'", "''") + "',"
                            //        + "`RACE` = '" + comboBox3.Text.Replace("'", "''") + "',"
                            //        + "`SEXE` = '" + comboBox4.Text.Replace("'", "''") + "',"
                            //        + "`NISS_DATE` = " + (checkBox2.Checked ? ("'" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","
                            //        + "`ROBE` = '" + textBox6.Text.Replace("'", "''") + "',"
                            //        + "`OBSERVATIONS` = '" + textBox8.Text.Replace("'", "''") + "',"
                            //        + "`IS_RADIATED` = " + (checkBox1.Checked ? "TRUE" : "FALSE") + ","
                            //        + "`RADIATION_DATE` = " + (checkBox1.Checked ? ("'" + dateTimePicker2.Value.Date.ToString("yyyy-MM-dd") + "'") : "NULL") + ","
                            //        + "`RADIATION_CAUSES` = '" + (checkBox1.Checked ? textBox5.Text.Replace("'", "''") : "") + "'"
                            //        + (File.Exists(openFileDialog1.FileName) ? ",`PICTURE` = @Pic" : (!button7.Visible ? ",`PICTURE` = NULL" : ""))
                            //        + " WHERE `ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + ";";
                            //MySqlCommand cmd = new MySqlCommand(insert_cmnd, PreConnection.mySqlConnection);
                            //if (File.Exists(openFileDialog1.FileName)) { cmd.Parameters.AddWithValue("@Pic", imageData); }
                            //PreConnection.open_conn();
                            //cmd.ExecuteNonQuery();
                            //============================================================================================================================================
                        }
                        //----------------
                        Load_anims_from_DB();
                    }
                }



            }
            else
            {
                new Non_Autorized_Msg("").ShowDialog();
            }


        }

        private void label12_VisibleChanged(object sender, EventArgs e)
        {
            if (label12.Visible)
            {
                System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
                tmr.Interval = 1000;
                tmr.Tick += Tmr_Tick;
                tmr.Start();
            }
        }

        int timm = 0;
        private void Tmr_Tick(object sender, EventArgs e)
        {
            timm++;
            if (timm >= 3)
            {
                label12.Visible = false;
                timm = 0;
                ((System.Windows.Forms.Timer)sender).Stop();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
            label13.Visible = false;
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(300));
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Poid_Panel.Show();
                Malad_Panel.Show();
                Visit_Panel.Show();
                //-----------------
                Is_New = false;

                if (tabControl1.SelectedTab == tabPage1)
                {
                    Load_selected_anim_fields();
                }
                else if (tabControl1.SelectedTab == tabPage3)
                {
                    load_poids();
                }
                else if (tabControl1.SelectedTab == tabPage4)
                {
                    Load_malad_1();
                }
                else if (tabControl1.SelectedTab == tabPage2)
                {
                    label18.Visible = false;
                    load_visites();
                }
            }
            else
            {
                Poid_Panel.Hide();
                Malad_Panel.Hide();
                Visit_Panel.Hide();
            }







        }

        private void load_poids()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                int prev_select = dataGridView3.SelectedRows.Count > 0 ? (int)dataGridView3.SelectedRows[0].Cells["IDD"].Value : -1;

                dataGridView3.CellValueChanged -= dataGridView3_CellValueChanged;
                poids_tbl.Rows.Clear();
                poids_tbl = PreConnection.Load_data("SELECT * FROM tb_poids WHERE ANIM_ID = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + " ORDER BY DATETIME ASC;");
                dataGridView3.DataSource = poids_tbl;
                dataGridView3.CellValueChanged += dataGridView3_CellValueChanged;

                dataGridView3.ClearSelection();
                if (prev_select > -1)
                {
                    dataGridView3.Rows.Cast<DataGridViewRow>().Where(ZZ => (int)ZZ.Cells["IDD"].Value == prev_select).ToList().ForEach(FF =>
                    {
                        FF.Selected = true;
                        if (FF.Index > dataGridView3.DisplayedRowCount(false)) { dataGridView3.FirstDisplayedScrollingRowIndex = FF.Index; }
                    });

                }
                else
                {
                    if (dataGridView3.DisplayedRowCount(false) < dataGridView3.RowCount) { dataGridView3.FirstDisplayedScrollingRowIndex = dataGridView3.NewRowIndex; }
                }

                dataGridView3_Scroll(null, null);
            }
        }

        private void new_anim(bool btn_clicked)
        {
            dataGridView1.ClearSelection();
            openFileDialog1.FileName = "";
            pictureBox2.Image = null;
            Is_New = true;
            comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged -= comboBox4_SelectedIndexChanged;
            comboBox3.Text = "--";
            foreach (Control ctrl in tabPage1.Controls)
            {
                if (ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(MaskedTextBox))
                {
                    ctrl.Text = string.Empty;
                }
                else if (ctrl.GetType() == typeof(ComboBox) && ((ComboBox)ctrl).DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    ((ComboBox)ctrl).SelectedIndex = 0;
                }
                else if (ctrl.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)ctrl).SelectedValue = -1;
                }
            }
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            button9.Visible = true;
            button9.PerformClick();
            button8.Visible = false;
            checkBox1.Checked = checkBox2.Checked = false;
            label13.Visible = false;
            pictureBox1.Image = Properties.Resources.NOUVEAU;
            poids_tbl.Rows.Clear();
            //------------------------
            Poid_Panel.Hide();
            Malad_Panel.Hide();
            Visit_Panel.Hide();
            tabControl1.SelectedTab = tabPage1;
            //-----------
            if (btn_clicked) { textBox3.Select(); }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            new_anim(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {

                if (MessageBox.Show("Vous étes sures de supprimer [" + dataGridView1.SelectedRows.Count + "] animaux ?\n\nAttention: Tous " + (dataGridView1.SelectedRows.Count == 1 ? "ses" : "leurs") + " infos associées seront supprimés (Laboratires, Visies, Agenda ...).", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string fff = "";
                    dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row => fff += "," + row.Cells["ID"].Value);
                    fff = fff.Substring(1);
                    //PreConnection.Excut_Cmd("DELETE FROM tb_animaux WHERE ID IN (" + fff + ");");
                    PreConnection.Excut_Cmd(3, "tb_animaux", null, null, "ID IN (@ID)", new List<string> { "ID" }, new List<object> { fff });
                    Load_anims_from_DB();
                    tabControl1.SelectedTab = tabPage1;
                }

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                Excc.Application xcelApp = new Excc.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                xcelApp.Application.Workbooks[1].Title = Application.ProductName + " - Animaux";
                xcelApp.Application.Workbooks[1].Worksheets[1].Name = "Animaux";
                dataGridView1.Columns.Cast<DataGridViewColumn>().Where(ss => ss.Name != "ID" && ss.Name != "IS_RADIATED" && ss.Name != "PICTURE").ToList().ForEach(g =>
                {
                    switch (g.HeaderText)
                    {
                        case "DATE_ADDED":
                            xcelApp.Cells[1, g.Index + 1].Value = "Ajouté le";
                            break;
                        case "NME":
                            xcelApp.Cells[1, g.Index + 1].Value = "Nom";
                            break;
                        case "NUM_IDENTIF":
                            xcelApp.Cells[1, g.Index + 1].Value = "N° Ident.";
                            break;
                        case "NUM_PASSPORT":
                            xcelApp.Cells[1, g.Index + 1].Value = "N° Passport";
                            break;
                        case "CLIENT_ID":
                            xcelApp.Cells[1, g.Index + 1].Value = "Propriétaire";
                            break;
                        case "ESPECE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Espèce";
                            break;
                        case "RACE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Race";
                            break;
                        case "SEXE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Sexe";
                            break;
                        case "NISS_DATE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Date Naissance";
                            break;
                        case "ROBE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Robe";
                            break;
                        case "OBSERVATIONS":
                            xcelApp.Cells[1, g.Index + 1].Value = "Observations";
                            break;
                        case "RADIATION_DATE":
                            xcelApp.Cells[1, g.Index + 1].Value = "Date Radiation";
                            break;
                        case "RADIATION_CAUSES":
                            xcelApp.Cells[1, g.Index + 1].Value = "Causes Radiation";
                            break;
                    }
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                    try
                    {
                        if (dataGridView1.Columns[g.Index].DefaultCellStyle.Format == "N2")
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "#,##0.00 [$Da-fr-dz]";
                        }
                        else if (dataGridView1.Columns[g.Index].DefaultCellStyle.Format.Contains("MM/yyyy"))
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "dd/MM/yyyy";
                        }
                    }
                    catch { }
                });

                dataGridView1.Rows.Cast<DataGridViewRow>().ToList().ForEach(t =>
                {
                    t.Cells.Cast<DataGridViewCell>().ToList().ForEach(b =>
                    {
                        if (xcelApp.Cells[1, b.ColumnIndex + 1].Value == "Propriétaire")
                        {
                            xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? (((DataRow)clients.AsEnumerable().FirstOrDefault(row => row.Field<int>("ID") == (int)dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value))["FULL_NME"]) : "";
                        }
                        else
                        {
                            xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").Replace("00:00:00", "").TrimStart().TrimEnd() : "";
                        }
                    });
                });
                xcelApp.Columns[dataGridView1.Columns["ID"].Index + 1].Delete();
                xcelApp.Columns[dataGridView1.Columns["IS_RADIATED"].Index].Delete();
                xcelApp.Columns[dataGridView1.Columns["PICTURE"].Index - 1].Delete();
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
                xcelApp.Quit();
                //-------------------
            }
            else
            {
                MessageBox.Show("Aucun donnés !", ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = checkBox1.Checked;
            //------------------------------
            if (checkBox1.Checked)
            {
                checkBox1.Location = new Point(groupBox1.Location.X + 160, groupBox1.Location.Y - 20);
                checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            }
            else
            {
                checkBox1.Location = new Point(14, pictureBox1.Location.Y - 20);
                checkBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Small_New_Client().ShowDialog();

            comboBox1.Validating -= comboBox1_Validating;
            clients = PreConnection.Load_data_keeping_duplicates("SELECT *,CONCAT(FAMNME,' ',NME) AS FULL_NME FROM tb_clients ORDER BY FULL_NME ASC;");
            comboBox1.DataSource = clients;
            comboBox1.DisplayMember = "FULL_NME";
            comboBox1.ValueMember = "ID";
            if (clients.Rows.Count > 0) { comboBox1.SelectedValue = (int)clients.AsEnumerable().Max(row => row.Field<int>("ID")); }
            full_nme_clients.Clear();
            clients.Rows.Cast<DataRow>().ToList().ForEach(clt =>
            {
                full_nme_clients.Add((string)clt["FULL_NME"]);
            });
            comboBox1.Validating += comboBox1_Validating;
        }

        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {
            if (comboBox1.Text.Length > 0 && !full_nme_clients.Contains(comboBox1.Text))
            {
                comboBox1.BackColor = Color.LightCoral;
            }
            verif_if_déja_exist_animal();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.BackColor = SystemColors.Window;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            panel2.Visible = comboBox2.SelectedIndex == 0;
            if (!button7.Visible)
            {
                if (Properties.Settings.Default.Use_animals_logo)
                {
                    pictureBox2.Image = (Image)Properties.Resources.ResourceManager.GetObject(comboBox2.Text);
                }
                else
                {
                    pictureBox2.Image = null;
                }

            }
            verif_if_déja_exist_animal();
            //------------------------
            string prev_val = comboBox3.Text;
            string[] tmmmp = { "Canine", "Feline", "Oiseaux" };
            ((DataTable)comboBox3.DataSource).DefaultView.RowFilter = tmmmp.Contains(comboBox2.Text) ? "ESPECE LIKE '" + comboBox2.Text + "'" : "";
            comboBox3.Text = prev_val;
            //-----------------------
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
            button7.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            openFileDialog1.FileName = "";
            button7.Visible = false;
            comboBox2_SelectedIndexChanged(null, null);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Visible = comboBox4.SelectedIndex == 0;
            verif_if_déja_exist_animal();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }
        bool visites_not_autorsed = false;
        private void Animaux_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.Last_login_is_admin)
            {
                button4.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20002" && (Int32)QQ[3] == 1).Count() > 0; //Supprimer
                button3.Visible = button1.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20001" && (Int32)QQ[3] == 1).Count() > 0; //Ajouter Animal                                
                foreach (Control ctrl in tabPage1.Controls)
                {
                    if (ctrl.Name != "button8")
                    {
                        ctrl.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "20003" && (Int32)QQ[3] == 1).Count() > 0; //Modifier
                    }
                }
                button1.Visible = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "10001" && (Int32)QQ[3] == 1).Count() > 0; //Ajouter Client
                //---------------------
                if (Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50000" && (Int32)QQ[3] == 1).Count() == 0)
                {
                    visites_not_autorsed = true;
                    tabControl1.TabPages.Remove(tabPage2);
                }
                else
                {
                    button12.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50001" && (Int32)QQ[3] == 1).Count() > 0;
                    button11.Enabled = Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50003" && (Int32)QQ[3] == 1).Count() > 0;
                }
            }
            //-----------
            button9.PerformClick();
            //-----------
            if (ID_to_selectt > 0)
            {
                tabControl1.SelectedTab = tabPage1;
                dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(Q => (int)Q.Cells["ID"].Value == ID_to_selectt).ToList().ForEach(W => W.Selected = true);
                ID_to_selectt = -1;
            }
            else if (ID_to_selectt == -2)
            {
                ID_to_selectt = -1;
                new_anim(true);

            }
            if (visite_idd > 0)
            {
                tabControl1.SelectedTab = tabPage2;
                dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
                dataGridView2.ClearSelection();
                dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
                dataGridView2.Rows.Cast<DataGridViewRow>().Where(xx => (int)xx.Cells["ID_VISITE"].Value == visite_idd).ToList().ForEach(dx => dx.Selected = true);
                visite_idd = -1;
            }
            else if (visite_idd == -2)
            {
                tabControl1.SelectedTab = tabPage2;
                button12.PerformClick();
                visite_idd = -1;
                richTextBox1.Select();
            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = dateTimePicker3.Value.Date.AddDays(1).AddSeconds(-1);
            dateTimePicker2.MinDate = dateTimePicker3.Value.Date;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!label12.Visible && !Is_New)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PARAM_NME", typeof(string));
                dt.Columns.Add("PARAM_VAL", typeof(string));
                //----------------
                dt.Rows.Add(new object[] { "DATE_ADDED", dateTimePicker3.Value.ToString("dd/MM/yyyy") });
                dt.Rows.Add(new object[] { "ANIM_NME", textBox3.Text });
                dt.Rows.Add(new object[] { "ESPECE", comboBox2.Text });
                dt.Rows.Add(new object[] { "RACE", comboBox3.Text });
                dt.Rows.Add(new object[] { "SEX", comboBox4.Text });
                dt.Rows.Add(new object[] { "DATE_NISS", checkBox2.Checked ? dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") : null });
                dt.Rows.Add(new object[] { "REF", dateTimePicker3.Value.ToString("MM") + textBox3.Text.Substring(0, 1).ToUpper() + dateTimePicker3.Value.ToString("dd") + textBox3.Text.Substring(2, 1).ToUpper() + comboBox1.SelectedValue + dataGridView1.SelectedRows[0].Cells["ID"].Value + dateTimePicker3.Value.Month + (dateTimePicker3.Value.Month + dateTimePicker3.Value.Day) + (dateTimePicker3.Value.Month * 2) });
                dt.Rows.Add(new object[] { "NUM_PASSPORT", textBox4.Text });
                dt.Rows.Add(new object[] { "NUM_IDENTIF", textBox2.Text });
                dt.Rows.Add(new object[] { "CABINET", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 1).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_TEL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 2).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_EMAIL", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 3).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "CABINET_ADRESS", Main_Frm.Params.Rows.Cast<DataRow>().Where(QQ => (int)QQ["ID"] == 4).Select(QQ => QQ["VAL"]).FirstOrDefault().ToString() });
                dt.Rows.Add(new object[] { "POIDS", poids_tbl.Rows.Count > 0 ? poids_tbl.AsEnumerable().Last().Field<double>("POIDS").ToString("N2") : "--" });

                DataRow rww = clients.Rows.Cast<DataRow>().Where(FF => (int)FF["ID"] == (int)comboBox1.SelectedValue).FirstOrDefault();
                dt.Rows.Add(new object[] { "CLIENT_SEX", rww["SEX"] != DBNull.Value ? rww["SEX"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_FAMNME", rww["FAMNME"] != DBNull.Value ? rww["FAMNME"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NME", rww["NME"] != DBNull.Value ? rww["NME"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NUM_CNI", rww["NUM_CNI"] != DBNull.Value ? rww["NUM_CNI"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_ADRESS", rww["ADRESS"] != DBNull.Value ? rww["ADRESS"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_CITY", rww["CITY"] != DBNull.Value ? rww["CITY"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_WILAYA", rww["WILAYA"] != DBNull.Value ? rww["WILAYA"].ToString() : null });
                dt.Rows.Add(new object[] { "POSTAL_CODE", rww["POSTAL_CODE"] != DBNull.Value ? rww["POSTAL_CODE"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_NUM_PHONE", rww["NUM_PHONE"] != DBNull.Value ? rww["NUM_PHONE"].ToString() : null });
                dt.Rows.Add(new object[] { "CLIENT_EMAIL", rww["EMAIL"] != DBNull.Value ? rww["EMAIL"].ToString() : null });


                //-------------
                new Print_report("certificat_enreg", dt, null).ShowDialog();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = checkBox2.Checked;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Is_New)
            {
                string vall = string.Concat(DateTime.Now.Millisecond.ToString("D3"), DateTime.Now.ToString("MMMM").Substring(1, 2), int.Parse((DateTime.Now.Minute * DateTime.Now.Second).ToString().Substring(0, (DateTime.Now.Minute * DateTime.Now.Second).ToString().Length > 4 ? 4 : (DateTime.Now.Minute * DateTime.Now.Second).ToString().Length)).ToString("D4"), DateTime.Now.ToString("dddd").Substring(2, 2), DateTime.Now.ToString("yy"), int.Parse((DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Substring(0, (DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Length > 4 ? 4 : (DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Length)).ToString("D4")).ToUpper();
                if (animaux != null)
                {
                    if (animaux.Rows.Count > 0)
                    {
                        while (animaux.Rows.Cast<DataRow>().Where(XX => (string)XX["NUM_IDENTIF"] == vall).ToList().Count > 0)
                        {
                            vall = string.Concat(DateTime.Now.Millisecond.ToString("D3"), DateTime.Now.ToString("MMMM").Substring(1, 2), int.Parse((DateTime.Now.Minute * DateTime.Now.Second).ToString().Substring(0, (DateTime.Now.Minute * DateTime.Now.Second).ToString().Length > 4 ? 4 : (DateTime.Now.Minute * DateTime.Now.Second).ToString().Length)).ToString("D4"), DateTime.Now.ToString("dddd").Substring(2, 2), DateTime.Now.ToString("yy"), int.Parse((DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Substring(0, (DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Length > 4 ? 4 : (DateTime.Now.Month + DateTime.Now.Millisecond).ToString().Length)).ToString("D4")).ToUpper();
                        }
                    }
                }
                textBox2.Text = vall;
            }

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (!Is_New)
            {
                MessageBox.Show("Le N° d'identification est trés sensible, il peut etre utilisé précedemment (Certificat d'identification ...)\n\nDonc confirmer bien avant de faire la modification.", "Attention :", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            label18.Visible = false;
            Is_New_Visite = true;
            dateTimePicker4.Value = DateTime.Now;
            comboBox5.Text = comboBox1.Text;
            richTextBox1.Text = string.Empty;
            richTextBox1.BackColor = SystemColors.Window;
            pictureBox3.Image = Properties.Resources.NOUVEAU_003;
            dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
            dataGridView2.ClearSelection();
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
            richTextBox1.Focus();

        }

        private void button13_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = richTextBox1.Text.Length > 0 ? SystemColors.Window : Color.LightCoral;

            if (richTextBox1.BackColor == SystemColors.Window)
            {
                if (!Properties.Settings.Default.Last_login_is_admin)
                {
                    label18.Visible = (Is_New_Visite && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50001" && (Int32)QQ[3] == 1).Count() == 0) || (!Is_New_Visite && Main_Frm.Autorisations.Rows.Cast<DataRow>().Where(QQ => QQ["CODE"].ToString() == "50002" && (Int32)QQ[3] == 1).Count() == 0);
                }


                if (Is_New_Visite && !label18.Visible)
                {
                    PreConnection.Excut_Cmd(1, "tb_visites", new List<string> {"DATETIME",
"ANIM_ID",
"VISITOR_FULL_NME",
"OBJECT"}, new List<object> { dateTimePicker4.Value,
dataGridView1.SelectedRows[0].Cells["ID"].Value,
comboBox5.Text,
richTextBox1.Text
}, null, null, null);
                }
                else if (!label18.Visible)
                {
                    PreConnection.Excut_Cmd(2, "tb_visites", new List<string> {"DATETIME",
"ANIM_ID",
"VISITOR_FULL_NME",
"OBJECT"}, new List<object> { dateTimePicker4.Value,
dataGridView1.SelectedRows[0].Cells["ID"].Value,
comboBox5.Text,
richTextBox1.Text
}, "ID = @W_ID", new List<string> { "W_ID" }, new List<object> { dataGridView2.SelectedRows[0].Cells["ID_VISITE"].Value });
                }
                bool label18_visible = label18.Visible;
                load_visites();
                label18.Visible = label18_visible;
            }
        }

        private void load_visites()
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int prev_select = dataGridView2.SelectedRows.Count > 0 ? (int)dataGridView2.SelectedRows[0].Cells["ID_VISITE"].Value : -1;
                DataTable visites = PreConnection.Load_data("SELECT tb1.*,tb2.REF AS 'FACTURE_REF' FROM tb_visites tb1 LEFT JOIN ("
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_01` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NOT NULL AND `ITEM_NME_01` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_02` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NOT NULL AND `ITEM_NME_02` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_03` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NOT NULL AND `ITEM_NME_03` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_04` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NOT NULL AND `ITEM_NME_04` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_05` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NOT NULL AND `ITEM_NME_05` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_06` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NOT NULL AND `ITEM_NME_06` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_07` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NOT NULL AND `ITEM_NME_07` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_08` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NOT NULL AND `ITEM_NME_08` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_09` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NOT NULL AND `ITEM_NME_09` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_10` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NOT NULL AND `ITEM_NME_10` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_11` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NOT NULL AND `ITEM_NME_11` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_12` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NOT NULL AND `ITEM_NME_12` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_13` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NOT NULL AND `ITEM_NME_13` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_14` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NOT NULL AND `ITEM_NME_14` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_15` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NOT NULL AND `ITEM_NME_15` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_16` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NOT NULL AND `ITEM_NME_16` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_17` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NOT NULL AND `ITEM_NME_17` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_18` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NOT NULL AND `ITEM_NME_18` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_19` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NOT NULL AND `ITEM_NME_19` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_20` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NOT NULL AND `ITEM_NME_20` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_21` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NOT NULL AND `ITEM_NME_21` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_22` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NOT NULL AND `ITEM_NME_22` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_23` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NOT NULL AND `ITEM_NME_23` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_24` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NOT NULL AND `ITEM_NME_24` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_25` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NOT NULL AND `ITEM_NME_25` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_26` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NOT NULL AND `ITEM_NME_26` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_27` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NOT NULL AND `ITEM_NME_27` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_28` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NOT NULL AND `ITEM_NME_28` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_29` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NOT NULL AND `ITEM_NME_29` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_30` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NOT NULL AND `ITEM_NME_30` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_31` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NOT NULL AND `ITEM_NME_31` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_32` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NOT NULL AND `ITEM_NME_32` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_33` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NOT NULL AND `ITEM_NME_33` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_34` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NOT NULL AND `ITEM_NME_34` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_35` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NOT NULL AND `ITEM_NME_35` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_36` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NOT NULL AND `ITEM_NME_36` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_37` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NOT NULL AND `ITEM_NME_37` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_38` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NOT NULL AND `ITEM_NME_38` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_39` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NOT NULL AND `ITEM_NME_39` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_40` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NOT NULL AND `ITEM_NME_40` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_41` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NOT NULL AND `ITEM_NME_41` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_42` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NOT NULL AND `ITEM_NME_42` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_43` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NOT NULL AND `ITEM_NME_43` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_44` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NOT NULL AND `ITEM_NME_44` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_45` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NOT NULL AND `ITEM_NME_45` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_46` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NOT NULL AND `ITEM_NME_46` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_47` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NOT NULL AND `ITEM_NME_47` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_48` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NOT NULL AND `ITEM_NME_48` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_49` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NOT NULL AND `ITEM_NME_49` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_50` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NOT NULL AND `ITEM_NME_50` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_51` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NOT NULL AND `ITEM_NME_51` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_52` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NOT NULL AND `ITEM_NME_52` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_53` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NOT NULL AND `ITEM_NME_53` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_54` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NOT NULL AND `ITEM_NME_54` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_55` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NOT NULL AND `ITEM_NME_55` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_56` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NOT NULL AND `ITEM_NME_56` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_57` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NOT NULL AND `ITEM_NME_57` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_58` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NOT NULL AND `ITEM_NME_58` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_59` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NOT NULL AND `ITEM_NME_59` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_60` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NOT NULL AND `ITEM_NME_60` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_61` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NOT NULL AND `ITEM_NME_61` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_62` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NOT NULL AND `ITEM_NME_62` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_63` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NOT NULL AND `ITEM_NME_63` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_64` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NOT NULL AND `ITEM_NME_64` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_65` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NOT NULL AND `ITEM_NME_65` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_66` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NOT NULL AND `ITEM_NME_66` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_67` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NOT NULL AND `ITEM_NME_67` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_68` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NOT NULL AND `ITEM_NME_68` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_69` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NOT NULL AND `ITEM_NME_69` IS NOT NULL UNION "
                                                          + "SELECT `REF`,`ITEM_PROD_CODE_70` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NOT NULL AND `ITEM_NME_70` IS NOT NULL "
                                    + ") tb2 ON tb1.`ID` = tb2.`VISIT` WHERE tb1.`ANIM_ID` = " + dataGridView1.SelectedRows[0].Cells["ID"].Value + " ORDER BY DATETIME;");
                dataGridView2.DataSource = visites;
                if (dataGridView2.DisplayedRowCount(false) < dataGridView2.RowCount) { dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows.Count - 1; }
                dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
                dataGridView2.ClearSelection();
                if (prev_select > -1)
                {
                    dataGridView2.Rows.Cast<DataGridViewRow>().Where(ZZ => (int)ZZ.Cells["ID_VISITE"].Value == prev_select).ToList().ForEach(FF => FF.Selected = true);
                }
                else if (dataGridView2.Rows.Count > 0)
                {
                    int idd_max = dataGridView2.Rows.Cast<DataGridViewRow>().Max(row => Convert.ToInt32(row.Cells["ID_VISITE"].Value));
                    dataGridView2.Rows.Cast<DataGridViewRow>().Where(ZZ => (int)ZZ.Cells["ID_VISITE"].Value == idd_max).ToList().ForEach(M => M.Selected = true);
                }
                dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
                dataGridView2_SelectionChanged(null, null);

            }
            else
            {
                ((DataTable)dataGridView2.DataSource).Rows.Clear();
            }

            if (visites_not_autorsed && tabPage2 != null)
            {
                tabControl1.TabPages.Remove(tabPage2);
            }


        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                label18.Visible = false;
                Is_New_Visite = false;
                pictureBox3.Image = Properties.Resources.MODIF_003;
                DataGridViewRow row = dataGridView2.SelectedRows[0];
                dateTimePicker4.Value = (DateTime)row.Cells["DATETIME"].Value;
                comboBox5.Text = (string)row.Cells["VISITOR_FULL_NME"].Value;
                richTextBox1.Text = (string)row.Cells["OBJECT"].Value;
                richTextBox1.BackColor = SystemColors.Window;
            }
            else
            {
                bool label18_visible = label18.Visible;
                button12_Click(null, null);
                label18.Visible = label18_visible;
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count == 1)
            {
                dataGridView2_SelectionChanged(null, null);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.BackColor = SystemColors.Window;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0 && button11.Enabled)
            {
                if (MessageBox.Show("Vous-étes sur de faire la suppression de (" + dataGridView2.SelectedRows.Count + ") visites ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string fact_ref = "";
                    string ids = "";
                    List<int> selected_row_delete_db = new List<int>();
                    dataGridView2.SelectedRows.Cast<DataGridViewRow>().ForEach(rw =>
                    {
                        ids += "," + rw.Cells["ID_VISITE"].Value;
                        if (rw.Cells["FACTURE_REF"].Value != DBNull.Value)
                        {
                            fact_ref += ",'" + rw.Cells["FACTURE_REF"].Value + "'";
                        }
                    });
                    if (ids.Length > 0)
                    {
                        ids = ids.Substring(1);
                        PreConnection.Excut_Cmd(3, "tb_visites", null, null, "ID IN (@W_ID)", new List<string> { "W_ID" }, new List<object> { ids });
                        //-----------
                        if (fact_ref.Length > 0)
                        {
                            PreConnection.Excut_Cmd_personnel("UPDATE tb_factures_vente SET "
                                                     + "`ITEM_PROD_CODE_01` = IF(`ITEM_IS_PROD_01` = FALSE AND `ITEM_PROD_CODE_01` IN (@param_xx), NULL, `ITEM_PROD_CODE_01`),"
                                                    + "`ITEM_PROD_CODE_02` = IF(`ITEM_IS_PROD_02` = FALSE AND `ITEM_PROD_CODE_02` IN (@param_xx), NULL, `ITEM_PROD_CODE_02`),"
                                                    + "`ITEM_PROD_CODE_03` = IF(`ITEM_IS_PROD_03` = FALSE AND `ITEM_PROD_CODE_03` IN (@param_xx), NULL, `ITEM_PROD_CODE_03`),"
                                                    + "`ITEM_PROD_CODE_04` = IF(`ITEM_IS_PROD_04` = FALSE AND `ITEM_PROD_CODE_04` IN (@param_xx), NULL, `ITEM_PROD_CODE_04`),"
                                                    + "`ITEM_PROD_CODE_05` = IF(`ITEM_IS_PROD_05` = FALSE AND `ITEM_PROD_CODE_05` IN (@param_xx), NULL, `ITEM_PROD_CODE_05`),"
                                                    + "`ITEM_PROD_CODE_06` = IF(`ITEM_IS_PROD_06` = FALSE AND `ITEM_PROD_CODE_06` IN (@param_xx), NULL, `ITEM_PROD_CODE_06`),"
                                                    + "`ITEM_PROD_CODE_07` = IF(`ITEM_IS_PROD_07` = FALSE AND `ITEM_PROD_CODE_07` IN (@param_xx), NULL, `ITEM_PROD_CODE_07`),"
                                                    + "`ITEM_PROD_CODE_08` = IF(`ITEM_IS_PROD_08` = FALSE AND `ITEM_PROD_CODE_08` IN (@param_xx), NULL, `ITEM_PROD_CODE_08`),"
                                                    + "`ITEM_PROD_CODE_09` = IF(`ITEM_IS_PROD_09` = FALSE AND `ITEM_PROD_CODE_09` IN (@param_xx), NULL, `ITEM_PROD_CODE_09`),"
                                                    + "`ITEM_PROD_CODE_10` = IF(`ITEM_IS_PROD_10` = FALSE AND `ITEM_PROD_CODE_10` IN (@param_xx), NULL, `ITEM_PROD_CODE_10`),"
                                                    + "`ITEM_PROD_CODE_11` = IF(`ITEM_IS_PROD_11` = FALSE AND `ITEM_PROD_CODE_11` IN (@param_xx), NULL, `ITEM_PROD_CODE_11`),"
                                                    + "`ITEM_PROD_CODE_12` = IF(`ITEM_IS_PROD_12` = FALSE AND `ITEM_PROD_CODE_12` IN (@param_xx), NULL, `ITEM_PROD_CODE_12`),"
                                                    + "`ITEM_PROD_CODE_13` = IF(`ITEM_IS_PROD_13` = FALSE AND `ITEM_PROD_CODE_13` IN (@param_xx), NULL, `ITEM_PROD_CODE_13`),"
                                                    + "`ITEM_PROD_CODE_14` = IF(`ITEM_IS_PROD_14` = FALSE AND `ITEM_PROD_CODE_14` IN (@param_xx), NULL, `ITEM_PROD_CODE_14`),"
                                                    + "`ITEM_PROD_CODE_15` = IF(`ITEM_IS_PROD_15` = FALSE AND `ITEM_PROD_CODE_15` IN (@param_xx), NULL, `ITEM_PROD_CODE_15`),"
                                                    + "`ITEM_PROD_CODE_16` = IF(`ITEM_IS_PROD_16` = FALSE AND `ITEM_PROD_CODE_16` IN (@param_xx), NULL, `ITEM_PROD_CODE_16`),"
                                                    + "`ITEM_PROD_CODE_17` = IF(`ITEM_IS_PROD_17` = FALSE AND `ITEM_PROD_CODE_17` IN (@param_xx), NULL, `ITEM_PROD_CODE_17`),"
                                                    + "`ITEM_PROD_CODE_18` = IF(`ITEM_IS_PROD_18` = FALSE AND `ITEM_PROD_CODE_18` IN (@param_xx), NULL, `ITEM_PROD_CODE_18`),"
                                                    + "`ITEM_PROD_CODE_19` = IF(`ITEM_IS_PROD_19` = FALSE AND `ITEM_PROD_CODE_19` IN (@param_xx), NULL, `ITEM_PROD_CODE_19`),"
                                                    + "`ITEM_PROD_CODE_20` = IF(`ITEM_IS_PROD_20` = FALSE AND `ITEM_PROD_CODE_20` IN (@param_xx), NULL, `ITEM_PROD_CODE_20`),"
                                                    + "`ITEM_PROD_CODE_21` = IF(`ITEM_IS_PROD_21` = FALSE AND `ITEM_PROD_CODE_21` IN (@param_xx), NULL, `ITEM_PROD_CODE_21`),"
                                                    + "`ITEM_PROD_CODE_22` = IF(`ITEM_IS_PROD_22` = FALSE AND `ITEM_PROD_CODE_22` IN (@param_xx), NULL, `ITEM_PROD_CODE_22`),"
                                                    + "`ITEM_PROD_CODE_23` = IF(`ITEM_IS_PROD_23` = FALSE AND `ITEM_PROD_CODE_23` IN (@param_xx), NULL, `ITEM_PROD_CODE_23`),"
                                                    + "`ITEM_PROD_CODE_24` = IF(`ITEM_IS_PROD_24` = FALSE AND `ITEM_PROD_CODE_24` IN (@param_xx), NULL, `ITEM_PROD_CODE_24`),"
                                                    + "`ITEM_PROD_CODE_25` = IF(`ITEM_IS_PROD_25` = FALSE AND `ITEM_PROD_CODE_25` IN (@param_xx), NULL, `ITEM_PROD_CODE_25`),"
                                                    + "`ITEM_PROD_CODE_26` = IF(`ITEM_IS_PROD_26` = FALSE AND `ITEM_PROD_CODE_26` IN (@param_xx), NULL, `ITEM_PROD_CODE_26`),"
                                                    + "`ITEM_PROD_CODE_27` = IF(`ITEM_IS_PROD_27` = FALSE AND `ITEM_PROD_CODE_27` IN (@param_xx), NULL, `ITEM_PROD_CODE_27`),"
                                                    + "`ITEM_PROD_CODE_28` = IF(`ITEM_IS_PROD_28` = FALSE AND `ITEM_PROD_CODE_28` IN (@param_xx), NULL, `ITEM_PROD_CODE_28`),"
                                                    + "`ITEM_PROD_CODE_29` = IF(`ITEM_IS_PROD_29` = FALSE AND `ITEM_PROD_CODE_29` IN (@param_xx), NULL, `ITEM_PROD_CODE_29`),"
                                                    + "`ITEM_PROD_CODE_30` = IF(`ITEM_IS_PROD_30` = FALSE AND `ITEM_PROD_CODE_30` IN (@param_xx), NULL, `ITEM_PROD_CODE_30`),"
                                                    + "`ITEM_PROD_CODE_31` = IF(`ITEM_IS_PROD_31` = FALSE AND `ITEM_PROD_CODE_31` IN (@param_xx), NULL, `ITEM_PROD_CODE_31`),"
                                                    + "`ITEM_PROD_CODE_32` = IF(`ITEM_IS_PROD_32` = FALSE AND `ITEM_PROD_CODE_32` IN (@param_xx), NULL, `ITEM_PROD_CODE_32`),"
                                                    + "`ITEM_PROD_CODE_33` = IF(`ITEM_IS_PROD_33` = FALSE AND `ITEM_PROD_CODE_33` IN (@param_xx), NULL, `ITEM_PROD_CODE_33`),"
                                                    + "`ITEM_PROD_CODE_34` = IF(`ITEM_IS_PROD_34` = FALSE AND `ITEM_PROD_CODE_34` IN (@param_xx), NULL, `ITEM_PROD_CODE_34`),"
                                                    + "`ITEM_PROD_CODE_35` = IF(`ITEM_IS_PROD_35` = FALSE AND `ITEM_PROD_CODE_35` IN (@param_xx), NULL, `ITEM_PROD_CODE_35`),"
                                                    + "`ITEM_PROD_CODE_36` = IF(`ITEM_IS_PROD_36` = FALSE AND `ITEM_PROD_CODE_36` IN (@param_xx), NULL, `ITEM_PROD_CODE_36`),"
                                                    + "`ITEM_PROD_CODE_37` = IF(`ITEM_IS_PROD_37` = FALSE AND `ITEM_PROD_CODE_37` IN (@param_xx), NULL, `ITEM_PROD_CODE_37`),"
                                                    + "`ITEM_PROD_CODE_38` = IF(`ITEM_IS_PROD_38` = FALSE AND `ITEM_PROD_CODE_38` IN (@param_xx), NULL, `ITEM_PROD_CODE_38`),"
                                                    + "`ITEM_PROD_CODE_39` = IF(`ITEM_IS_PROD_39` = FALSE AND `ITEM_PROD_CODE_39` IN (@param_xx), NULL, `ITEM_PROD_CODE_39`),"
                                                    + "`ITEM_PROD_CODE_40` = IF(`ITEM_IS_PROD_40` = FALSE AND `ITEM_PROD_CODE_40` IN (@param_xx), NULL, `ITEM_PROD_CODE_40`),"
                                                    + "`ITEM_PROD_CODE_41` = IF(`ITEM_IS_PROD_41` = FALSE AND `ITEM_PROD_CODE_41` IN (@param_xx), NULL, `ITEM_PROD_CODE_41`),"
                                                    + "`ITEM_PROD_CODE_42` = IF(`ITEM_IS_PROD_42` = FALSE AND `ITEM_PROD_CODE_42` IN (@param_xx), NULL, `ITEM_PROD_CODE_42`),"
                                                    + "`ITEM_PROD_CODE_43` = IF(`ITEM_IS_PROD_43` = FALSE AND `ITEM_PROD_CODE_43` IN (@param_xx), NULL, `ITEM_PROD_CODE_43`),"
                                                    + "`ITEM_PROD_CODE_44` = IF(`ITEM_IS_PROD_44` = FALSE AND `ITEM_PROD_CODE_44` IN (@param_xx), NULL, `ITEM_PROD_CODE_44`),"
                                                    + "`ITEM_PROD_CODE_45` = IF(`ITEM_IS_PROD_45` = FALSE AND `ITEM_PROD_CODE_45` IN (@param_xx), NULL, `ITEM_PROD_CODE_45`),"
                                                    + "`ITEM_PROD_CODE_46` = IF(`ITEM_IS_PROD_46` = FALSE AND `ITEM_PROD_CODE_46` IN (@param_xx), NULL, `ITEM_PROD_CODE_46`),"
                                                    + "`ITEM_PROD_CODE_47` = IF(`ITEM_IS_PROD_47` = FALSE AND `ITEM_PROD_CODE_47` IN (@param_xx), NULL, `ITEM_PROD_CODE_47`),"
                                                    + "`ITEM_PROD_CODE_48` = IF(`ITEM_IS_PROD_48` = FALSE AND `ITEM_PROD_CODE_48` IN (@param_xx), NULL, `ITEM_PROD_CODE_48`),"
                                                    + "`ITEM_PROD_CODE_49` = IF(`ITEM_IS_PROD_49` = FALSE AND `ITEM_PROD_CODE_49` IN (@param_xx), NULL, `ITEM_PROD_CODE_49`),"
                                                    + "`ITEM_PROD_CODE_50` = IF(`ITEM_IS_PROD_50` = FALSE AND `ITEM_PROD_CODE_50` IN (@param_xx), NULL, `ITEM_PROD_CODE_50`),"
                                                    + "`ITEM_PROD_CODE_51` = IF(`ITEM_IS_PROD_51` = FALSE AND `ITEM_PROD_CODE_51` IN (@param_xx), NULL, `ITEM_PROD_CODE_51`),"
                                                    + "`ITEM_PROD_CODE_52` = IF(`ITEM_IS_PROD_52` = FALSE AND `ITEM_PROD_CODE_52` IN (@param_xx), NULL, `ITEM_PROD_CODE_52`),"
                                                    + "`ITEM_PROD_CODE_53` = IF(`ITEM_IS_PROD_53` = FALSE AND `ITEM_PROD_CODE_53` IN (@param_xx), NULL, `ITEM_PROD_CODE_53`),"
                                                    + "`ITEM_PROD_CODE_54` = IF(`ITEM_IS_PROD_54` = FALSE AND `ITEM_PROD_CODE_54` IN (@param_xx), NULL, `ITEM_PROD_CODE_54`),"
                                                    + "`ITEM_PROD_CODE_55` = IF(`ITEM_IS_PROD_55` = FALSE AND `ITEM_PROD_CODE_55` IN (@param_xx), NULL, `ITEM_PROD_CODE_55`),"
                                                    + "`ITEM_PROD_CODE_56` = IF(`ITEM_IS_PROD_56` = FALSE AND `ITEM_PROD_CODE_56` IN (@param_xx), NULL, `ITEM_PROD_CODE_56`),"
                                                    + "`ITEM_PROD_CODE_57` = IF(`ITEM_IS_PROD_57` = FALSE AND `ITEM_PROD_CODE_57` IN (@param_xx), NULL, `ITEM_PROD_CODE_57`),"
                                                    + "`ITEM_PROD_CODE_58` = IF(`ITEM_IS_PROD_58` = FALSE AND `ITEM_PROD_CODE_58` IN (@param_xx), NULL, `ITEM_PROD_CODE_58`),"
                                                    + "`ITEM_PROD_CODE_59` = IF(`ITEM_IS_PROD_59` = FALSE AND `ITEM_PROD_CODE_59` IN (@param_xx), NULL, `ITEM_PROD_CODE_59`),"
                                                    + "`ITEM_PROD_CODE_60` = IF(`ITEM_IS_PROD_60` = FALSE AND `ITEM_PROD_CODE_60` IN (@param_xx), NULL, `ITEM_PROD_CODE_60`),"
                                                    + "`ITEM_PROD_CODE_61` = IF(`ITEM_IS_PROD_61` = FALSE AND `ITEM_PROD_CODE_61` IN (@param_xx), NULL, `ITEM_PROD_CODE_61`),"
                                                    + "`ITEM_PROD_CODE_62` = IF(`ITEM_IS_PROD_62` = FALSE AND `ITEM_PROD_CODE_62` IN (@param_xx), NULL, `ITEM_PROD_CODE_62`),"
                                                    + "`ITEM_PROD_CODE_63` = IF(`ITEM_IS_PROD_63` = FALSE AND `ITEM_PROD_CODE_63` IN (@param_xx), NULL, `ITEM_PROD_CODE_63`),"
                                                    + "`ITEM_PROD_CODE_64` = IF(`ITEM_IS_PROD_64` = FALSE AND `ITEM_PROD_CODE_64` IN (@param_xx), NULL, `ITEM_PROD_CODE_64`),"
                                                    + "`ITEM_PROD_CODE_65` = IF(`ITEM_IS_PROD_65` = FALSE AND `ITEM_PROD_CODE_65` IN (@param_xx), NULL, `ITEM_PROD_CODE_65`),"
                                                    + "`ITEM_PROD_CODE_66` = IF(`ITEM_IS_PROD_66` = FALSE AND `ITEM_PROD_CODE_66` IN (@param_xx), NULL, `ITEM_PROD_CODE_66`),"
                                                    + "`ITEM_PROD_CODE_67` = IF(`ITEM_IS_PROD_67` = FALSE AND `ITEM_PROD_CODE_67` IN (@param_xx), NULL, `ITEM_PROD_CODE_67`),"
                                                    + "`ITEM_PROD_CODE_68` = IF(`ITEM_IS_PROD_68` = FALSE AND `ITEM_PROD_CODE_68` IN (@param_xx), NULL, `ITEM_PROD_CODE_68`),"
                                                    + "`ITEM_PROD_CODE_69` = IF(`ITEM_IS_PROD_69` = FALSE AND `ITEM_PROD_CODE_69` IN (@param_xx), NULL, `ITEM_PROD_CODE_69`),"
                                                    + "`ITEM_PROD_CODE_70` = IF(`ITEM_IS_PROD_70` = FALSE AND `ITEM_PROD_CODE_70` IN (@param_xx), NULL, `ITEM_PROD_CODE_70`)"
                                + ";", new List<string> { "param_xx" }, new List<object> { ids });
                        }
                    }
                    load_visites();
                }
            }
        }

        private void comboBox3_Validated(object sender, EventArgs e)
        {
            verif_if_déja_exist_animal();
        }

        private void Animaux_Activated(object sender, EventArgs e)
        {
            textBox1.Clear();
            if (ID_to_selectt > 0)
            {
                tabControl1.SelectedTab = tabPage1;
                dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(Q => (int)Q.Cells["ID"].Value == ID_to_selectt).ToList().ForEach(W => W.Selected = true);
                ID_to_selectt = -1;
            }
            else if (ID_to_selectt == -2)
            {
                ID_to_selectt = -1;
                new_anim(false);
            }
            //------------
            if (visite_idd > 0)
            {
                tabControl1.SelectedTab = tabPage2;
                dataGridView2.SelectionChanged -= dataGridView2_SelectionChanged;
                dataGridView2.ClearSelection();
                dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
                dataGridView2.Rows.Cast<DataGridViewRow>().Where(xx => (int)xx.Cells["ID_VISITE"].Value == visite_idd).ToList().ForEach(dx => dx.Selected = true);
                visite_idd = -1;
            }
            else if (visite_idd == -2)
            {
                tabControl1.SelectedTab = tabPage2;
                button12.PerformClick();
                visite_idd = -1;
                richTextBox1.Select();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            chosen_anim_from_search = new DataTable();
            Anims_List_Search select = new Anims_List_Search();
            select.DataTableReturned += ChildForm_DataTableReturned;
            select.ShowDialog();
            if (chosen_anim_from_search != null)
            {
                textBox1.Clear();
                comboBox2.SelectedValue = chosen_anim_from_search.Rows[0][1];
                dataGridView1.ClearSelection();
                dataGridView1.Rows.Cast<DataGridViewRow>().Where(c => c.Cells["ID"].Value.ToString() == chosen_anim_from_search.Rows[0]["ID"].ToString()).ForEach(dx => dx.Selected = true);
            }
        }
        private void ChildForm_DataTableReturned(object sender, DataTableEventArgs e)
        {
            chosen_anim_from_search = e.DataTable;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //PreConnection.Excport_to_excel(dataGridView2, "Visites", dataGridView1.SelectedRows[0].Cells["NME"].Value != DBNull.Value ? "Visites" : dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value.ToString(), null, true);
            if (dataGridView2.Rows.Count > 0)
            {
                Excc.Application xcelApp = new Excc.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                xcelApp.Application.Workbooks[1].Title = Application.ProductName + "Visites";
                xcelApp.Application.Workbooks[1].Worksheets[1].Name = dataGridView1.SelectedRows[0].Cells["NME"].Value != DBNull.Value ? "Visites" : dataGridView1.SelectedRows[0].Cells["FULL_NME"].Value.ToString();
                dataGridView2.Columns.Cast<DataGridViewColumn>().Where(ss => ss.Name != "ID_VISITE" && ss.Name != "ANIM_ID").ToList().ForEach(g =>
                {
                    xcelApp.Cells[1, g.Index + 1].Value = g.HeaderText;

                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Interior.Color = ColorTranslator.ToOle(Color.DarkCyan);
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).Font.Bold = true;
                    ((Excc.Range)xcelApp.Cells[1, g.Index + 1]).HorizontalAlignment = Excc.XlHAlign.xlHAlignCenter;
                    try
                    {
                        if (dataGridView2.Columns[g.Index].DefaultCellStyle.Format == "N2")
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "#,##0.00 [$Da-fr-dz]";
                        }
                        else if (dataGridView2.Columns[g.Index].DefaultCellStyle.Format.Contains("MM/yyyy"))
                        {
                            ((Excc.Range)xcelApp.Columns[g.Index + 1]).NumberFormat = "dd/MM/yyyy";
                        }
                    }
                    catch { }
                });

                dataGridView2.Rows.Cast<DataGridViewRow>().ToList().ForEach(t =>
                {
                    t.Cells.Cast<DataGridViewCell>().ToList().ForEach(b =>
                    {
                        //if (xcelApp.Cells[1, b.ColumnIndex + 1].Value == "Propriétaire")
                        //{
                        //    xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? (((DataRow)clients.AsEnumerable().FirstOrDefault(row => row.Field<int>("ID") == (int)dataGridView1.Rows[t.Index].Cells[b.ColumnIndex].Value))["FULL_NME"]) : "";
                        //}
                        //else
                        //{
                        xcelApp.Cells[t.Index + 2, b.ColumnIndex + 1].Value = dataGridView2.Rows[t.Index].Cells[b.ColumnIndex].Value != null ? dataGridView2.Rows[t.Index].Cells[b.ColumnIndex].Value.ToString().Replace(",", ".").Replace("00:00:00", "").TrimStart().TrimEnd() : "";
                        //}
                    });
                });
                xcelApp.Columns[3].Delete();
                xcelApp.Columns[1].Delete();
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
                xcelApp.Quit();
                //-------------------
            }
            else
            {
                MessageBox.Show("Aucun donnés !", ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            prev_rw_idx = dataGridView3.CurrentCell.RowIndex;
            prev_col_idx = dataGridView3.CurrentCell.ColumnIndex;
            if (dataGridView3.CurrentCell.ColumnIndex == dataGridView3.Columns["DATETIMEE"].Index)
            {
                DateTimePicker dateTimePicker = new DateTimePicker();
                dateTimePicker.Format = DateTimePickerFormat.Custom;
                dateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm";
                dateTimePicker.Value = dataGridView3.Rows[prev_rw_idx].Cells["DATETIMEE"].Value != null ? DateTime.Now : DateTime.Parse(dataGridView3.Rows[prev_rw_idx].Cells["DATETIMEE"].Value.ToString());
                dateTimePicker.ValueChanged += (s, args) =>
                {
                    dataGridView3.CellValueChanged -= dataGridView3_CellValueChanged;
                    dataGridView3.Rows[prev_rw_idx].Cells[prev_col_idx].Value = dateTimePicker.Value.ToString("dd/MM/yyyy HH:mm");
                    dataGridView3.CellValueChanged += dataGridView3_CellValueChanged;
                };
                dateTimePicker.Leave += (s, args) =>
                {
                    dataGridView3_CellValueChanged(null, new DataGridViewCellEventArgs(prev_col_idx, prev_rw_idx));
                    dataGridView3_Scroll(null, null);
                };
                dataGridView3.Controls.Add(dateTimePicker);
                dataGridView3.CurrentCell.Style.Padding = new Padding(0);
                dateTimePicker.Visible = true;
                dateTimePicker.Location = dataGridView3.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location;
                dateTimePicker.Size = dataGridView3.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                if (prev_rw_idx == 0) { dataGridView3.BeginEdit(true); }
                dateTimePicker.Focus();
            }
            else if (prev_col_idx == dataGridView3.Columns["POIDS"].Index)
            {

                NumericUpDown numericUpDown = new NumericUpDown();

                numericUpDown.Minimum = 0;
                numericUpDown.Maximum = 100000000000;
                numericUpDown.DecimalPlaces = 2;
                numericUpDown.ThousandsSeparator = true;
                decimal fff = 0;
                decimal.TryParse(dataGridView3.Rows[prev_rw_idx].Cells[prev_col_idx].Value.ToString(), out fff);
                numericUpDown.Value = fff;
                numericUpDown.ValueChanged += (s, args) =>
                {
                    dataGridView3.CellValueChanged -= dataGridView3_CellValueChanged;
                    dataGridView3.Rows[prev_rw_idx].Cells[prev_col_idx].Value = numericUpDown.Value;
                    dataGridView3.CellValueChanged += dataGridView3_CellValueChanged;
                };
                numericUpDown.Leave += (s, args) =>
                {
                    dataGridView3_CellValueChanged(null, new DataGridViewCellEventArgs(prev_col_idx, prev_rw_idx));
                    dataGridView3_Scroll(null, null);
                };
                dataGridView3.Controls.Add(numericUpDown);
                dataGridView3.CurrentCell.Style.Padding = new Padding(0);
                numericUpDown.Visible = true;
                numericUpDown.Location = dataGridView3.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location;
                numericUpDown.Size = dataGridView3.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                numericUpDown.Focus();
                numericUpDown.Select(0, fff.ToString("N2").Length);
            }
            else if (prev_col_idx == dataGridView3.Columns["DELETE"].Index && prev_rw_idx != dataGridView3.NewRowIndex)
            {
                if (MessageBox.Show("Êtes-vous sûrs de faire la suppression ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PreConnection.Excut_Cmd(3, "tb_poids", null, null, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView3.Rows[prev_rw_idx].Cells["IDD"].Value });
                    load_poids();
                }
            }
        }


        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && (dataGridView3.Rows[e.RowIndex].Cells["DATETIMEE"].Value != null || dataGridView3.Rows[e.RowIndex].Cells["POIDS"].Value != null))
            {
                string cmd = "";

                if (dataGridView3.Rows[e.RowIndex].Cells["ANIM_IDD"].Value != DBNull.Value && dataGridView3.Rows[e.RowIndex].Cells["IDD"].Value != null)
                {
                    cmd = "UPDATE `tb_poids` SET "
                                          + "`DATETIME` = '" + ((DateTime)dataGridView3.Rows[e.RowIndex].Cells["DATETIMEE"].Value).ToString("yyyy-MM-dd HH:mm") + "',"
                                          + "`POIDS` = " + dataGridView3.Rows[e.RowIndex].Cells["POIDS"].Value
                                          + " WHERE `ID` = " + dataGridView3.Rows[e.RowIndex].Cells["IDD"].Value + ";";
                }
                else //INSERT
                {
                    if (dataGridView3.Columns[e.ColumnIndex].Name == "DATETIMEE" && dataGridView3.Rows[e.RowIndex].Cells["DATETIMEE"].Value != DBNull.Value)
                    {
                        cmd = "INSERT INTO `tb_poids`"
                                          + "(`ANIM_ID`,"
                                          + "`DATETIME`)"
                                          + " VALUES ("
                                          + dataGridView1.SelectedRows[0].Cells["ID"].Value + ","
                                          + "'" + ((DateTime)dataGridView3.Rows[e.RowIndex].Cells["DATETIMEE"].Value).ToString("yyyy-MM-dd HH:mm") + "')";
                    }
                    else if (dataGridView3.Columns[e.ColumnIndex].Name == "POIDS" && dataGridView3.Rows[e.RowIndex].Cells["POIDS"].Value != DBNull.Value)
                    {
                        cmd = "INSERT INTO `tb_poids`"
                                           + "(`ANIM_ID`,"
                                          + "`POIDS`)"
                                          + " VALUES ("
                                          + dataGridView1.SelectedRows[0].Cells["ID"].Value + ","
                                          + dataGridView3.Rows[e.RowIndex].Cells["POIDS"].Value + ")";
                    }
                }

                if (cmd.Length > 0)
                {
                    PreConnection.Excut_Cmd_personnel(cmd, null, null);
                    load_poids();
                }
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1_SelectionChanged(null, null);
        }

        private void dataGridView3_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Control ctrr in dataGridView3.Controls)
            {
                dataGridView3.Controls.Remove(ctrr);
            }
        }


        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView3.Columns[e.ColumnIndex].Name == "DELETE" && e.RowIndex == dataGridView3.NewRowIndex)
            {
                e.Value = Properties.Resources.icons8_trash_25px_1;

            }
        }

        private void dataGridView3_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = true;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            new Print_visites(1, (int)dataGridView1.SelectedRows[0].Cells["ID"].Value).ShowDialog();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Enabled = checkBox3.Checked;
            if (groupBox3.Enabled) { comboBox6.Focus(); }
            anim_filter();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

            if (splitContainer1.SplitterDistance > splitter_prev_dist)
            {
                int difff = this.Size.Width - frm_width;
                this.Size = new Size(frm_width + (splitContainer1.SplitterDistance - spliter_panel1_wdth) + difff, this.Size.Height);
                splitContainer1.Dock = DockStyle.Right;
                splitContainer1.Dock = DockStyle.Fill;
            }
            splitter_prev_dist = splitContainer1.SplitterDistance;
        }

        private void dataGridView4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (dataGridView4.Rows[e.RowIndex].Cells["ANIM_ID_MALAD"].Value != DBNull.Value && dataGridView4.Rows[e.RowIndex].Cells["ID_MALAD"].Value != DBNull.Value) //UPDATE
                {
                    PreConnection.Excut_Cmd(2, "tb_maladies", new List<string> { "START_DATE", "MALAD_NME", "MALAD_LEVEL", "ESTIM_END_DATE" }, new List<object>
                    {
                        dataGridView4.Rows[e.RowIndex].Cells["START_DATE_MALAD"].Value,
                        dataGridView4.Rows[e.RowIndex].Cells["MALAD_NME"].Value,
                        dataGridView4.Rows[e.RowIndex].Cells["MALAD_LEVEL"].Value,
                        dataGridView4.Rows[e.RowIndex].Cells["ESTIM_END_DATE_MALAD"].Value
                    }, "ID = @PP_IDD", new List<string> { "PP_IDD" }, new List<object> { dataGridView4.Rows[e.RowIndex].Cells["ID_MALAD"].Value });
                }
                else if (dataGridView1.SelectedRows.Count > 0) //INSERT
                {
                    PreConnection.Excut_Cmd(1, "tb_maladies", new List<string> {
                        "ANIM_ID",
                        "START_DATE",
                        "MALAD_NME",
                        "MALAD_LEVEL",
                        "ESTIM_END_DATE" },
                        new List<object>
                    {
                        dataGridView1.SelectedRows[0].Cells["ID"].Value,
                        dataGridView4.Rows[e.RowIndex].Cells["START_DATE_MALAD"].Value != DBNull.Value ? dataGridView4.Rows[e.RowIndex].Cells["START_DATE_MALAD"].Value : DateTime.Now,
                        dataGridView4.Rows[e.RowIndex].Cells["MALAD_NME"].Value != DBNull.Value ? dataGridView4.Rows[e.RowIndex].Cells["MALAD_NME"].Value : DBNull.Value,
                        dataGridView4.Rows[e.RowIndex].Cells["MALAD_LEVEL"].Value != DBNull.Value ? dataGridView4.Rows[e.RowIndex].Cells["MALAD_LEVEL"].Value : DBNull.Value,
                        dataGridView4.Rows[e.RowIndex].Cells["ESTIM_END_DATE_MALAD"].Value
                    }, null, null, null);


                }
                Load_malad_1();
                //------------------
                reload_cbx6_data();
                //-----------------

            }
        }

        private void Load_malad_2()
        {
            if (tabControl1.SelectedTab == tabPage4 && dataGridView1.SelectedRows.Count > 0)
            {
                int prev_select = dataGridView4.SelectedRows.Count > 0 ? (int)dataGridView4.SelectedRows[0].Cells["ID_MALAD"].Value : -1;

                dataGridView4.CellValueChanged -= dataGridView4_CellValueChanged;
                if (dataGridView4.DataSource != null)
                {
                    ((DataTable)dataGridView4.DataSource).Rows.Clear();
                }
                if (filtred_maladies_tbl != null)
                {
                    if (dataGridView4.DataSource != null)
                    {
                        //((DataTable)dataGridView4.DataSource).Rows.Clear();
                        //maladies_tbl.Rows.Cast<DataRow>().Where(F => (int)F["ANIM_ID"] == (int)dataGridView1.SelectedRows[0].Cells["ID"].Value).ToList().ForEach(KK =>
                        //{
                        //    DataRow rww = ((DataTable)dataGridView4.DataSource).NewRow();
                        //    rww.ItemArray = KK.ItemArray;
                        //    ((DataTable)dataGridView4.DataSource).Rows.Add(rww);
                        //});


                        filtred_maladies_tbl.Rows.Cast<DataRow>().Where(F => (int)F["ANIM_ID"] == (int)dataGridView1.SelectedRows[0].Cells["ID"].Value).ToList().ForEach(KK =>
                        {
                            DataRow rww = ((DataTable)dataGridView4.DataSource).NewRow();
                            rww.ItemArray = KK.ItemArray;
                            ((DataTable)dataGridView4.DataSource).Rows.Add(rww);
                        });

                    }
                    else
                    {
                        var ggg = filtred_maladies_tbl.Rows.Cast<DataRow>().Where(F => (int)F["ANIM_ID"] == (int)dataGridView1.SelectedRows[0].Cells["ID"].Value);
                        if (ggg.Any())
                        {
                            dataGridView4.DataSource = ggg.CopyToDataTable();
                        };
                    }

                }



                dataGridView4.CellValueChanged += dataGridView4_CellValueChanged;

                dataGridView4.ClearSelection();
                if (prev_select > -1)
                {
                    dataGridView4.Rows.Cast<DataGridViewRow>().Where(ZZ => (int)ZZ.Cells["ID_MALAD"].Value == prev_select).ToList().ForEach(FF =>
                    {
                        FF.Selected = true;
                        if (FF.Index > dataGridView4.DisplayedRowCount(false)) { dataGridView4.FirstDisplayedScrollingRowIndex = FF.Index; }
                    });
                }
                else
                {
                    if (dataGridView4.DisplayedRowCount(false) < dataGridView4.RowCount) { dataGridView4.FirstDisplayedScrollingRowIndex = dataGridView4.NewRowIndex; }
                }
                dataGridView4_Scroll(null, null);
            }
        }
        private void Load_malad_1()
        {
            maladies_tbl.Rows.Clear();
            maladies_tbl = PreConnection.Load_data("SELECT * FROM tb_maladies ORDER BY START_DATE ASC;");
            //--------------
            anim_filter();
            Load_malad_2();

        }

        private void dataGridView4_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Control ctrr in dataGridView4.Controls)
            {
                dataGridView4.Controls.Remove(ctrr);
            }
        }

        private void dataGridView4_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = true;
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            prev_rw_idx = dataGridView4.CurrentCell.RowIndex;
            prev_col_idx = dataGridView4.CurrentCell.ColumnIndex;
            if (dataGridView4.CurrentCell.ColumnIndex == dataGridView4.Columns["START_DATE_MALAD"].Index || dataGridView4.CurrentCell.ColumnIndex == dataGridView4.Columns["ESTIM_END_DATE_MALAD"].Index)
            {
                DateTimePicker dateTimePicker = new DateTimePicker();
                dateTimePicker.Format = DateTimePickerFormat.Custom;
                dateTimePicker.CustomFormat = "dd/MM/yyyy";
                try { dateTimePicker.Value = dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value != DBNull.Value ? DateTime.Parse(dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value.ToString()) : DateTime.Now; } catch { dateTimePicker.Value = DateTime.Now; }
                dateTimePicker.ValueChanged += (s, args) =>
                {
                    dataGridView4.CellValueChanged -= dataGridView4_CellValueChanged;
                    dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value = dateTimePicker.Value.ToString("dd/MM/yyyy");
                    dataGridView4.CellValueChanged += dataGridView4_CellValueChanged;
                };
                dateTimePicker.Leave += (s, args) =>
                {
                    dataGridView4_CellValueChanged(null, new DataGridViewCellEventArgs(prev_col_idx, prev_rw_idx));
                    dataGridView4_Scroll(null, null);
                };
                dataGridView4.Controls.Add(dateTimePicker);
                dataGridView4.CurrentCell.Style.Padding = new Padding(0);
                dateTimePicker.Visible = true;
                dateTimePicker.Location = dataGridView4.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location;
                dateTimePicker.Size = dataGridView4.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                if (prev_rw_idx == 0) { dataGridView4.BeginEdit(true); }
                dateTimePicker.Focus();
            }
            else if (prev_col_idx == dataGridView4.Columns["MALAD_NME"].Index)
            {
                ComboBox cbx = new ComboBox();
                cbx.Items.AddRange(comboBox6.Items.Cast<object>().Where(G => G.ToString() != "- Tous -").ToArray());
                cbx.DropDownStyle = ComboBoxStyle.DropDown;
                cbx.AutoCompleteSource = AutoCompleteSource.ListItems;
                cbx.AutoCompleteMode = AutoCompleteMode.Suggest;

                try
                {
                    if (dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value != DBNull.Value)
                    {
                        if (cbx.Items.Contains(dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value))
                        {
                            cbx.SelectedItem = dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value;
                        }
                    }
                }
                catch { cbx.SelectedIndex = -1; }
                cbx.Leave += (s, args) =>
                {
                    dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value = cbx.Text;
                    dataGridView4_Scroll(null, null);
                };


                dataGridView4.Controls.Add(cbx);
                dataGridView4.CurrentCell.Style.Padding = new Padding(0);
                cbx.Visible = true;
                cbx.Location = dataGridView4.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location;
                cbx.Size = dataGridView4.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                cbx.Focus();
            }
            else if (prev_col_idx == dataGridView4.Columns["MALAD_LEVEL"].Index)
            {
                ComboBox cbx = new ComboBox();
                cbx.DropDownStyle = ComboBoxStyle.DropDownList;

                cbx.Items.Add("--");
                cbx.Items.Add("Légère");
                cbx.Items.Add("Modéré");
                cbx.Items.Add("Grave");

                try { cbx.SelectedItem = dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value != DBNull.Value ? dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value : "--"; } catch { cbx.SelectedItem = "--"; }

                cbx.SelectedIndexChanged += (s, args) =>
                {
                    dataGridView4.Rows[prev_rw_idx].Cells[prev_col_idx].Value = cbx.SelectedItem.ToString();
                    dataGridView4_Scroll(null, null);
                };


                dataGridView4.Controls.Add(cbx);
                dataGridView4.CurrentCell.Style.Padding = new Padding(0);
                cbx.Visible = true;
                cbx.Location = dataGridView4.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Location;
                cbx.Size = dataGridView4.GetCellDisplayRectangle(prev_col_idx, prev_rw_idx, false).Size;
                cbx.Focus();
            }
            else if (prev_col_idx == dataGridView4.Columns["DEL_MALAD"].Index && prev_rw_idx != dataGridView4.NewRowIndex)
            {
                if (MessageBox.Show("Êtes-vous sûrs de faire la suppression ?", "Confirmer :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PreConnection.Excut_Cmd(3, "tb_maladies", null, null, "ID = @P_ID", new List<string> { "P_ID" }, new List<object> { dataGridView4.Rows[prev_rw_idx].Cells["ID_MALAD"].Value });
                    Load_malad_1();
                    //------------------
                    reload_cbx6_data();
                    //-----------------
                }
            }
        }

        private void reload_cbx6_data()
        {
            //------------------
            string prev_sel_cbx6 = comboBox6.Text;
            var mal_types = maladies_tbl.AsEnumerable().Select(V => V["MALAD_NME"].ToString());
            if (mal_types.Any())
            {
                string[] tmmp = default_maladies.CreateCopy().Concat(mal_types.Distinct()).ToArray();
                Array.Sort(tmmp);
                comboBox6.DataSource = tmmp.Distinct().ToList();
            }
            else
            {
                comboBox6.DataSource = default_maladies;
            }
            comboBox6.Text = comboBox6.Items.Contains(prev_sel_cbx6) ? prev_sel_cbx6 : "- Tous -";
            //-----------------
        }

        private void dataGridView4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView4.Columns[e.ColumnIndex].Name == "DEL_MALAD" && e.RowIndex == dataGridView4.NewRowIndex)
            {
                e.Value = Properties.Resources.icons8_trash_25px_1;

            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == null)
            {
                e.Cancel = true;
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            anim_filter();
        }


        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].HeaderCell.Style.BackColor = dataGridView1.Rows[e.RowIndex].HeaderCell.Style.SelectionBackColor = maladies_tbl.Rows.Cast<DataRow>().Where(F => (int)F["ANIM_ID"] == (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value).ToList().Count > 0 ? panel3.BackColor : Color.White;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                anim_filter();
            }
        }


        private void comboBox6_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox6.Text))
            {
                comboBox6.Text = "- Tous -";
                comboBox6.SelectAll();
            }
            anim_filter();
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.State == DataGridViewElementStates.Selected)
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
            }
        }


    }
}

