using System.Security;

namespace ALBAITAR_Softvet
{
    partial class Main_Frm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Frm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_cab_nme = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_visites_animal = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabPage_labo_animal = new System.Windows.Forms.TabPage();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage_Calendar = new System.Windows.Forms.TabPage();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.ID_VISITE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATETIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VISITOR_FULL_NME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OBJECT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANIM_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLIENT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANIM_NME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLIENT_FULL_NME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FACTURE_REF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LABO_NME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATE_TIME2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REF2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IDD2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OBSERV2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANIM_IDD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANIM_NMEE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLIENT_IDD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLIENT_FULL_NMEE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NUM_FACT_LAB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.tabPage_infos = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_visites_animal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabPage_labo_animal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button9);
            this.panel1.Controls.Add(this.button11);
            this.panel1.Controls.Add(this.button12);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Location = new System.Drawing.Point(787, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(295, 326);
            this.panel1.TabIndex = 20;
            this.panel1.Visible = false;
            this.panel1.VisibleChanged += new System.EventHandler(this.panel1_VisibleChanged);
            this.panel1.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            // 
            // label_cab_nme
            // 
            this.label_cab_nme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_cab_nme.BackColor = System.Drawing.Color.Transparent;
            this.label_cab_nme.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_cab_nme.ForeColor = System.Drawing.Color.Purple;
            this.label_cab_nme.Location = new System.Drawing.Point(209, 13);
            this.label_cab_nme.Name = "label_cab_nme";
            this.label_cab_nme.Size = new System.Drawing.Size(490, 25);
            this.label_cab_nme.TabIndex = 21;
            this.label_cab_nme.Text = "Cabinet : --";
            this.label_cab_nme.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Propriétaire",
            "Animal"});
            this.comboBox1.Location = new System.Drawing.Point(182, 41);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 27;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 16);
            this.label1.TabIndex = 29;
            this.label1.Text = "Rechercher par :";
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage_infos);
            this.tabControl1.Controls.Add(this.tabPage_visites_animal);
            this.tabControl1.Controls.Add(this.tabPage_labo_animal);
            this.tabControl1.Controls.Add(this.tabPage_Calendar);
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Location = new System.Drawing.Point(1, 69);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(15, 10);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(842, 499);
            this.tabControl1.TabIndex = 30;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage_visites_animal
            // 
            this.tabPage_visites_animal.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_visites_animal.Controls.Add(this.textBox1);
            this.tabPage_visites_animal.Controls.Add(this.radioButton3);
            this.tabPage_visites_animal.Controls.Add(this.radioButton2);
            this.tabPage_visites_animal.Controls.Add(this.radioButton1);
            this.tabPage_visites_animal.Controls.Add(this.dataGridView2);
            this.tabPage_visites_animal.Controls.Add(this.button19);
            this.tabPage_visites_animal.Controls.Add(this.button17);
            this.tabPage_visites_animal.Controls.Add(this.button18);
            this.tabPage_visites_animal.Location = new System.Drawing.Point(37, 4);
            this.tabPage_visites_animal.Name = "tabPage_visites_animal";
            this.tabPage_visites_animal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_visites_animal.Size = new System.Drawing.Size(801, 491);
            this.tabPage_visites_animal.TabIndex = 1;
            this.tabPage_visites_animal.Text = "Visites";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(379, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(355, 21);
            this.textBox1.TabIndex = 68;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.ForeColor = System.Drawing.Color.Red;
            this.radioButton3.Location = new System.Drawing.Point(226, 8);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(99, 20);
            this.radioButton3.TabIndex = 61;
            this.radioButton3.Text = "Non Facturé()";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ForeColor = System.Drawing.Color.DarkGreen;
            this.radioButton2.Location = new System.Drawing.Point(110, 8);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(74, 20);
            this.radioButton2.TabIndex = 60;
            this.radioButton2.Text = "Facturé()";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 8);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(56, 20);
            this.radioButton1.TabIndex = 59;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Tous()";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.ColumnHeadersHeight = 35;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID_VISITE,
            this.DATETIME,
            this.VISITOR_FULL_NME,
            this.OBJECT,
            this.ANIM_ID,
            this.CLIENT_ID,
            this.ANIM_NME,
            this.CLIENT_FULL_NME,
            this.FACTURE_REF});
            this.dataGridView2.EnableHeadersVisualStyles = false;
            this.dataGridView2.Location = new System.Drawing.Point(7, 34);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(791, 413);
            this.dataGridView2.TabIndex = 58;
            this.dataGridView2.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_CellMouseDoubleClick);
            // 
            // tabPage_labo_animal
            // 
            this.tabPage_labo_animal.Controls.Add(this.button15);
            this.tabPage_labo_animal.Controls.Add(this.radioButton4);
            this.tabPage_labo_animal.Controls.Add(this.radioButton5);
            this.tabPage_labo_animal.Controls.Add(this.radioButton6);
            this.tabPage_labo_animal.Controls.Add(this.textBox3);
            this.tabPage_labo_animal.Controls.Add(this.comboBox3);
            this.tabPage_labo_animal.Controls.Add(this.dataGridView1);
            this.tabPage_labo_animal.Controls.Add(this.button14);
            this.tabPage_labo_animal.Controls.Add(this.button13);
            this.tabPage_labo_animal.Controls.Add(this.button10);
            this.tabPage_labo_animal.Location = new System.Drawing.Point(37, 4);
            this.tabPage_labo_animal.Name = "tabPage_labo_animal";
            this.tabPage_labo_animal.Size = new System.Drawing.Size(801, 491);
            this.tabPage_labo_animal.TabIndex = 2;
            this.tabPage_labo_animal.Text = "Laboratoire";
            this.tabPage_labo_animal.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.ForeColor = System.Drawing.Color.Red;
            this.radioButton4.Location = new System.Drawing.Point(220, 39);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(99, 20);
            this.radioButton4.TabIndex = 64;
            this.radioButton4.Text = "Non Facturé()";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.ForeColor = System.Drawing.Color.DarkGreen;
            this.radioButton5.Location = new System.Drawing.Point(104, 39);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(74, 20);
            this.radioButton5.TabIndex = 63;
            this.radioButton5.Text = "Facturé()";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Checked = true;
            this.radioButton6.Location = new System.Drawing.Point(11, 39);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(56, 20);
            this.radioButton6.TabIndex = 62;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "Tous()";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Location = new System.Drawing.Point(276, 11);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(452, 21);
            this.textBox3.TabIndex = 29;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "- Tous -",
            "Hemogramme",
            "Biochimie Sanguine",
            "Immunologie",
            "Protéinogramme",
            "Urologie",
            "- Autres -"});
            this.comboBox3.Location = new System.Drawing.Point(11, 9);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(236, 24);
            this.comboBox3.TabIndex = 28;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeight = 30;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LABO_NME,
            this.DATE_TIME2,
            this.REF2,
            this.IDD2,
            this.OBSERV2,
            this.ANIM_IDD,
            this.ANIM_NMEE,
            this.CLIENT_IDD,
            this.CLIENT_FULL_NMEE,
            this.NUM_FACT_LAB});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(11, 65);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(782, 384);
            this.dataGridView1.TabIndex = 27;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // tabPage_Calendar
            // 
            this.tabPage_Calendar.Location = new System.Drawing.Point(37, 4);
            this.tabPage_Calendar.Name = "tabPage_Calendar";
            this.tabPage_Calendar.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Calendar.Size = new System.Drawing.Size(801, 491);
            this.tabPage_Calendar.TabIndex = 3;
            this.tabPage_Calendar.Text = "Calendrier";
            this.tabPage_Calendar.UseVisualStyleBackColor = true;
            // 
            // comboBox2
            // 
            this.comboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(393, 41);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(242, 24);
            this.comboBox2.TabIndex = 31;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Checked = true;
            this.radioButton7.Location = new System.Drawing.Point(326, 42);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(48, 20);
            this.radioButton7.TabIndex = 33;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "Tous";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(376, 46);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(14, 13);
            this.radioButton8.TabIndex = 34;
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // ID_VISITE
            // 
            this.ID_VISITE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ID_VISITE.DataPropertyName = "ID";
            this.ID_VISITE.HeaderText = "ID";
            this.ID_VISITE.Name = "ID_VISITE";
            this.ID_VISITE.ReadOnly = true;
            this.ID_VISITE.Visible = false;
            this.ID_VISITE.Width = 42;
            // 
            // DATETIME
            // 
            this.DATETIME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DATETIME.DataPropertyName = "DATETIME";
            dataGridViewCellStyle2.Format = "dd/MM/yyyy HH:mm";
            this.DATETIME.DefaultCellStyle = dataGridViewCellStyle2;
            this.DATETIME.HeaderText = "Date";
            this.DATETIME.Name = "DATETIME";
            this.DATETIME.ReadOnly = true;
            this.DATETIME.Width = 58;
            // 
            // VISITOR_FULL_NME
            // 
            this.VISITOR_FULL_NME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.VISITOR_FULL_NME.DataPropertyName = "VISITOR_FULL_NME";
            this.VISITOR_FULL_NME.HeaderText = "Visiteur";
            this.VISITOR_FULL_NME.Name = "VISITOR_FULL_NME";
            this.VISITOR_FULL_NME.ReadOnly = true;
            this.VISITOR_FULL_NME.Width = 67;
            // 
            // OBJECT
            // 
            this.OBJECT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OBJECT.DataPropertyName = "OBJECT";
            this.OBJECT.HeaderText = "Objet";
            this.OBJECT.Name = "OBJECT";
            this.OBJECT.ReadOnly = true;
            // 
            // ANIM_ID
            // 
            this.ANIM_ID.DataPropertyName = "ANIM_ID";
            this.ANIM_ID.HeaderText = "ANIM_ID";
            this.ANIM_ID.Name = "ANIM_ID";
            this.ANIM_ID.ReadOnly = true;
            this.ANIM_ID.Visible = false;
            // 
            // CLIENT_ID
            // 
            this.CLIENT_ID.DataPropertyName = "CLIENT_ID";
            this.CLIENT_ID.HeaderText = "CLIENT_ID";
            this.CLIENT_ID.Name = "CLIENT_ID";
            this.CLIENT_ID.ReadOnly = true;
            this.CLIENT_ID.Visible = false;
            // 
            // ANIM_NME
            // 
            this.ANIM_NME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ANIM_NME.DataPropertyName = "ANIM_NME";
            this.ANIM_NME.HeaderText = "Animal";
            this.ANIM_NME.Name = "ANIM_NME";
            this.ANIM_NME.ReadOnly = true;
            this.ANIM_NME.Width = 67;
            // 
            // CLIENT_FULL_NME
            // 
            this.CLIENT_FULL_NME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CLIENT_FULL_NME.DataPropertyName = "CLIENT_FULL_NME";
            this.CLIENT_FULL_NME.HeaderText = "Propriétaire";
            this.CLIENT_FULL_NME.Name = "CLIENT_FULL_NME";
            this.CLIENT_FULL_NME.ReadOnly = true;
            this.CLIENT_FULL_NME.Width = 92;
            // 
            // FACTURE_REF
            // 
            this.FACTURE_REF.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.FACTURE_REF.DataPropertyName = "FACTURE_REF";
            this.FACTURE_REF.HeaderText = "N° Facture\n(s\'il est facturé)";
            this.FACTURE_REF.Name = "FACTURE_REF";
            this.FACTURE_REF.ReadOnly = true;
            this.FACTURE_REF.Width = 109;
            // 
            // LABO_NME
            // 
            this.LABO_NME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LABO_NME.DataPropertyName = "LABO_NME";
            this.LABO_NME.HeaderText = "Laboratoire";
            this.LABO_NME.Name = "LABO_NME";
            this.LABO_NME.ReadOnly = true;
            this.LABO_NME.Width = 93;
            // 
            // DATE_TIME2
            // 
            this.DATE_TIME2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DATE_TIME2.DataPropertyName = "DATE_TIME";
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.DATE_TIME2.DefaultCellStyle = dataGridViewCellStyle4;
            this.DATE_TIME2.HeaderText = "Date";
            this.DATE_TIME2.Name = "DATE_TIME2";
            this.DATE_TIME2.ReadOnly = true;
            this.DATE_TIME2.Width = 58;
            // 
            // REF2
            // 
            this.REF2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.REF2.DataPropertyName = "REF";
            this.REF2.HeaderText = "Réf.";
            this.REF2.Name = "REF2";
            this.REF2.ReadOnly = true;
            this.REF2.Width = 51;
            // 
            // IDD2
            // 
            this.IDD2.DataPropertyName = "ID";
            this.IDD2.HeaderText = "ID";
            this.IDD2.Name = "IDD2";
            this.IDD2.ReadOnly = true;
            this.IDD2.Visible = false;
            // 
            // OBSERV2
            // 
            this.OBSERV2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OBSERV2.DataPropertyName = "OBSERV";
            this.OBSERV2.HeaderText = "Observ.";
            this.OBSERV2.Name = "OBSERV2";
            this.OBSERV2.ReadOnly = true;
            // 
            // ANIM_IDD
            // 
            this.ANIM_IDD.DataPropertyName = "ANIM_ID";
            this.ANIM_IDD.HeaderText = "ANIM_ID";
            this.ANIM_IDD.Name = "ANIM_IDD";
            this.ANIM_IDD.ReadOnly = true;
            this.ANIM_IDD.Visible = false;
            // 
            // ANIM_NMEE
            // 
            this.ANIM_NMEE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ANIM_NMEE.DataPropertyName = "ANIM_NME";
            this.ANIM_NMEE.HeaderText = "Animal";
            this.ANIM_NMEE.Name = "ANIM_NMEE";
            this.ANIM_NMEE.ReadOnly = true;
            this.ANIM_NMEE.Width = 67;
            // 
            // CLIENT_IDD
            // 
            this.CLIENT_IDD.DataPropertyName = "CLIENT_ID";
            this.CLIENT_IDD.HeaderText = "CLIENT_ID";
            this.CLIENT_IDD.Name = "CLIENT_IDD";
            this.CLIENT_IDD.ReadOnly = true;
            this.CLIENT_IDD.Visible = false;
            // 
            // CLIENT_FULL_NMEE
            // 
            this.CLIENT_FULL_NMEE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CLIENT_FULL_NMEE.DataPropertyName = "CLIENT_FULL_NME";
            this.CLIENT_FULL_NMEE.HeaderText = "Propriétaire";
            this.CLIENT_FULL_NMEE.Name = "CLIENT_FULL_NMEE";
            this.CLIENT_FULL_NMEE.ReadOnly = true;
            this.CLIENT_FULL_NMEE.Width = 92;
            // 
            // NUM_FACT_LAB
            // 
            this.NUM_FACT_LAB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NUM_FACT_LAB.DataPropertyName = "FACTURE_REF";
            this.NUM_FACT_LAB.HeaderText = "N° Facture";
            this.NUM_FACT_LAB.Name = "NUM_FACT_LAB";
            this.NUM_FACT_LAB.ReadOnly = true;
            this.NUM_FACT_LAB.Width = 87;
            // 
            // button7
            // 
            this.button7.BackgroundImage = global::ALBAITAR_Softvet.Properties.Resources.icons8_search_20px;
            this.button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button7.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(639, 41);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(24, 24);
            this.button7.TabIndex = 32;
            this.toolTip1.SetToolTip(this.button7, "Rechercher");
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button6.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_profit_30px;
            this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.Location = new System.Drawing.Point(4, 272);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(282, 46);
            this.button6.TabIndex = 17;
            this.button6.Text = "Vente && Facturation";
            this.toolTip1.SetToolTip(this.button6, "Produits");
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_laboratory_30px;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(4, 164);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(282, 46);
            this.button4.TabIndex = 16;
            this.button4.Text = "Laboratoire";
            this.toolTip1.SetToolTip(this.button4, "Produits");
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            this.button4.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button9.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_people_30px;
            this.button9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.Location = new System.Drawing.Point(4, 4);
            this.button9.Margin = new System.Windows.Forms.Padding(4);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(282, 46);
            this.button9.TabIndex = 6;
            this.button9.Text = "Propriétaires";
            this.toolTip1.SetToolTip(this.button9, "Clients");
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            this.button9.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button11.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_dog_30px;
            this.button11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button11.Location = new System.Drawing.Point(4, 57);
            this.button11.Margin = new System.Windows.Forms.Padding(4);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(282, 46);
            this.button11.TabIndex = 7;
            this.button11.Text = "Animaux";
            this.toolTip1.SetToolTip(this.button11, "Animaux");
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            this.button11.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button12.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button12.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_products_pile_30px_1;
            this.button12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button12.Location = new System.Drawing.Point(4, 110);
            this.button12.Margin = new System.Windows.Forms.Padding(4);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(282, 46);
            this.button12.TabIndex = 8;
            this.button12.Text = "Produits && Stock";
            this.toolTip1.SetToolTip(this.button12, "Produits");
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            this.button12.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_tear_off_calendar_30px;
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.Location = new System.Drawing.Point(4, 218);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(282, 46);
            this.button5.TabIndex = 15;
            this.button5.Text = "Agenda";
            this.toolTip1.SetToolTip(this.button5, "Produits");
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            this.button5.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackgroundImage = global::ALBAITAR_Softvet.Properties.Resources.icons8_lock_30px;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(741, 12);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 46);
            this.button1.TabIndex = 16;
            this.toolTip1.SetToolTip(this.button1, "Verrouiller/Changer l\'utilisateur");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackgroundImage = global::ALBAITAR_Softvet.Properties.Resources.icons8_job_30px_1;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(792, 12);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(43, 46);
            this.button2.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button2, "Parametres");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::ALBAITAR_Softvet.Properties.Resources.icons8_menu_30px;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(13, 13);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(43, 46);
            this.button3.TabIndex = 0;
            this.toolTip1.SetToolTip(this.button3, "Parametres");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button3.MouseEnter += new System.EventHandler(this.button3_MouseEnter);
            this.button3.MouseLeave += new System.EventHandler(this.button3_MouseLeave);
            // 
            // button19
            // 
            this.button19.BackgroundImage = global::ALBAITAR_Softvet.Properties.Resources.icons8_search_20px;
            this.button19.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button19.Enabled = false;
            this.button19.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button19.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button19.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Location = new System.Drawing.Point(356, 8);
            this.button19.Margin = new System.Windows.Forms.Padding(4);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(24, 21);
            this.button19.TabIndex = 69;
            this.toolTip1.SetToolTip(this.button19, "Rechercher");
            this.button19.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button17.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button17.FlatAppearance.BorderColor = System.Drawing.Color.ForestGreen;
            this.button17.FlatAppearance.BorderSize = 2;
            this.button17.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.button17.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.ForeColor = System.Drawing.Color.ForestGreen;
            this.button17.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_add_25px;
            this.button17.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button17.Location = new System.Drawing.Point(688, 450);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(106, 36);
            this.button17.TabIndex = 67;
            this.button17.Text = "Nouveau";
            this.button17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button18.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button18.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button18.FlatAppearance.BorderSize = 2;
            this.button18.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.button18.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button18.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button18.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_pencil_drawing_25px_1;
            this.button18.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button18.Location = new System.Drawing.Point(7, 451);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(106, 36);
            this.button18.TabIndex = 66;
            this.button18.Text = "Modifier";
            this.button18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button15
            // 
            this.button15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button15.FlatAppearance.BorderColor = System.Drawing.Color.ForestGreen;
            this.button15.FlatAppearance.BorderSize = 2;
            this.button15.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.button15.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.ForeColor = System.Drawing.Color.ForestGreen;
            this.button15.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_add_25px;
            this.button15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button15.Location = new System.Drawing.Point(687, 452);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(106, 36);
            this.button15.TabIndex = 65;
            this.button15.Text = "Nouveau";
            this.button15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button14
            // 
            this.button14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button14.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button14.FlatAppearance.BorderSize = 2;
            this.button14.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.button14.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button14.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_pencil_drawing_25px_1;
            this.button14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button14.Location = new System.Drawing.Point(11, 452);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(106, 36);
            this.button14.TabIndex = 50;
            this.button14.Text = "Modifier";
            this.button14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button13
            // 
            this.button13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button13.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.button13.FlatAppearance.BorderSize = 2;
            this.button13.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.button13.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button13.Image = global::ALBAITAR_Softvet.Properties.Resources.icons8_print_25px;
            this.button13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button13.Location = new System.Drawing.Point(123, 452);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(106, 36);
            this.button13.TabIndex = 49;
            this.button13.Text = "Imprimer";
            this.button13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Visible = false;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button10
            // 
            this.button10.BackgroundImage = global::ALBAITAR_Softvet.Properties.Resources.icons8_search_20px;
            this.button10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button10.Enabled = false;
            this.button10.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Location = new System.Drawing.Point(253, 11);
            this.button10.Margin = new System.Windows.Forms.Padding(4);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(24, 21);
            this.button10.TabIndex = 33;
            this.toolTip1.SetToolTip(this.button10, "Rechercher");
            this.button10.UseVisualStyleBackColor = true;
            // 
            // tabPage_infos
            // 
            this.tabPage_infos.AutoScroll = true;
            this.tabPage_infos.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_infos.Location = new System.Drawing.Point(37, 4);
            this.tabPage_infos.Name = "tabPage_infos";
            this.tabPage_infos.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_infos.Size = new System.Drawing.Size(801, 491);
            this.tabPage_infos.TabIndex = 0;
            this.tabPage_infos.Text = "Informations";
            // 
            // Main_Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(843, 569);
            this.Controls.Add(this.radioButton8);
            this.Controls.Add(this.radioButton7);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label_cab_nme);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(765, 608);
            this.Name = "Main_Frm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ALBAITAR Softvet";
            this.Activated += new System.EventHandler(this.Main_Frm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Frm_FormClosing);
            this.Load += new System.EventHandler(this.Main_Frm_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_visites_animal.ResumeLayout(false);
            this.tabPage_visites_animal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabPage_labo_animal.ResumeLayout(false);
            this.tabPage_labo_animal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_visites_animal;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.ComboBox comboBox2;
        public System.Windows.Forms.Label label_cab_nme;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TabPage tabPage_labo_animal;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.TabPage tabPage_Calendar;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_VISITE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATETIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn VISITOR_FULL_NME;
        private System.Windows.Forms.DataGridViewTextBoxColumn OBJECT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANIM_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLIENT_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANIM_NME;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLIENT_FULL_NME;
        private System.Windows.Forms.DataGridViewTextBoxColumn FACTURE_REF;
        private System.Windows.Forms.DataGridViewTextBoxColumn LABO_NME;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATE_TIME2;
        private System.Windows.Forms.DataGridViewTextBoxColumn REF2;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDD2;
        private System.Windows.Forms.DataGridViewTextBoxColumn OBSERV2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANIM_IDD;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANIM_NMEE;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLIENT_IDD;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLIENT_FULL_NMEE;
        private System.Windows.Forms.DataGridViewTextBoxColumn NUM_FACT_LAB;
        private System.Windows.Forms.TabPage tabPage_infos;
    }
}