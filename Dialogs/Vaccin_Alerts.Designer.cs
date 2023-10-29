namespace ALBAITAR_Softvet.Dialogs
{
    partial class Vaccin_Alerts
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NEXT_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VACCIN_NME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IS_PERIODIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FIXED_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EVERY_TXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EVERY_DAY_NB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.START_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.END_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EVERY_MOUNTH_NB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.START_YEAR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.END_YEAR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IS_CONCERN_WHO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IS_IMPORTANT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANIM_NUM_IDENs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANIM_ESPECE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANIM_RACE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANIM_SEXE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POIDS_MAX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATE_NISS_MIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATE_NISS_MAX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DESCRIPTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RELATED_CLIENTS_IDS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ALERT_BEFORE_DAYS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SEND_ALERT_TO_CABINE_EMAIL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SEND_ALERT_TO_CLIENT_EMAIL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAST_ALERT_EMAIL_CABINET_SENT_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAST_ALERT_EMAIL_CLIENT_SENT_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IS_FOR_ALL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CABINET_EMAIL_ALREADY_SENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLIENT_EMAIL_ALREADY_SENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NEXT_ALARM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 38;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.NEXT_DATE,
            this.VACCIN_NME,
            this.IS_PERIODIC,
            this.FIXED_DATE,
            this.EVERY_TXT,
            this.EVERY_DAY_NB,
            this.START_DATE,
            this.END_DATE,
            this.EVERY_MOUNTH_NB,
            this.START_YEAR,
            this.END_YEAR,
            this.IS_CONCERN_WHO,
            this.IS_IMPORTANT,
            this.ANIM_NUM_IDENs,
            this.ANIM_ESPECE,
            this.ANIM_RACE,
            this.ANIM_SEXE,
            this.POIDS_MAX,
            this.DATE_NISS_MIN,
            this.DATE_NISS_MAX,
            this.DESCRIPTION,
            this.RELATED_CLIENTS_IDS,
            this.ALERT_BEFORE_DAYS,
            this.SEND_ALERT_TO_CABINE_EMAIL,
            this.SEND_ALERT_TO_CLIENT_EMAIL,
            this.LAST_ALERT_EMAIL_CABINET_SENT_DATE,
            this.LAST_ALERT_EMAIL_CLIENT_SENT_DATE,
            this.IS_FOR_ALL,
            this.CABINET_EMAIL_ALREADY_SENT,
            this.CLIENT_EMAIL_ALREADY_SENT,
            this.NEXT_ALARM});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(610, 167);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Teal;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Salmon;
            this.label1.Location = new System.Drawing.Point(597, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "X";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // NEXT_DATE
            // 
            this.NEXT_DATE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NEXT_DATE.DataPropertyName = "NEXT_DATE";
            this.NEXT_DATE.HeaderText = "Date";
            this.NEXT_DATE.Name = "NEXT_DATE";
            this.NEXT_DATE.ReadOnly = true;
            this.NEXT_DATE.Width = 58;
            // 
            // VACCIN_NME
            // 
            this.VACCIN_NME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VACCIN_NME.DataPropertyName = "VACCIN_NME";
            this.VACCIN_NME.HeaderText = "Vaccination";
            this.VACCIN_NME.Name = "VACCIN_NME";
            this.VACCIN_NME.ReadOnly = true;
            // 
            // IS_PERIODIC
            // 
            this.IS_PERIODIC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.IS_PERIODIC.DataPropertyName = "IS_PERIODIC";
            this.IS_PERIODIC.HeaderText = "Periodique?";
            this.IS_PERIODIC.Name = "IS_PERIODIC";
            this.IS_PERIODIC.ReadOnly = true;
            this.IS_PERIODIC.Visible = false;
            this.IS_PERIODIC.Width = 96;
            // 
            // FIXED_DATE
            // 
            this.FIXED_DATE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.FIXED_DATE.DataPropertyName = "FIXED_DATE";
            dataGridViewCellStyle2.Format = "d";
            dataGridViewCellStyle2.NullValue = null;
            this.FIXED_DATE.DefaultCellStyle = dataGridViewCellStyle2;
            this.FIXED_DATE.HeaderText = "Date fixé";
            this.FIXED_DATE.Name = "FIXED_DATE";
            this.FIXED_DATE.ReadOnly = true;
            this.FIXED_DATE.Visible = false;
            this.FIXED_DATE.Width = 73;
            // 
            // EVERY_TXT
            // 
            this.EVERY_TXT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.EVERY_TXT.DataPropertyName = "EVERY_TXT";
            this.EVERY_TXT.HeaderText = "Périodique - Chaque";
            this.EVERY_TXT.Name = "EVERY_TXT";
            this.EVERY_TXT.ReadOnly = true;
            this.EVERY_TXT.Visible = false;
            this.EVERY_TXT.Width = 91;
            // 
            // EVERY_DAY_NB
            // 
            this.EVERY_DAY_NB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.EVERY_DAY_NB.DataPropertyName = "EVERY_DAY_NB";
            this.EVERY_DAY_NB.HeaderText = "EVERY_DAY_NB";
            this.EVERY_DAY_NB.Name = "EVERY_DAY_NB";
            this.EVERY_DAY_NB.ReadOnly = true;
            this.EVERY_DAY_NB.Visible = false;
            this.EVERY_DAY_NB.Width = 112;
            // 
            // START_DATE
            // 
            this.START_DATE.DataPropertyName = "START_DATE";
            this.START_DATE.HeaderText = "START_DATE";
            this.START_DATE.Name = "START_DATE";
            this.START_DATE.ReadOnly = true;
            this.START_DATE.Visible = false;
            // 
            // END_DATE
            // 
            this.END_DATE.DataPropertyName = "END_DATE";
            this.END_DATE.HeaderText = "END_DATE";
            this.END_DATE.Name = "END_DATE";
            this.END_DATE.ReadOnly = true;
            this.END_DATE.Visible = false;
            // 
            // EVERY_MOUNTH_NB
            // 
            this.EVERY_MOUNTH_NB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.EVERY_MOUNTH_NB.DataPropertyName = "EVERY_MOUNTH_NB";
            this.EVERY_MOUNTH_NB.HeaderText = "EVERY_MOUNTH_NB";
            this.EVERY_MOUNTH_NB.Name = "EVERY_MOUNTH_NB";
            this.EVERY_MOUNTH_NB.ReadOnly = true;
            this.EVERY_MOUNTH_NB.Visible = false;
            this.EVERY_MOUNTH_NB.Width = 140;
            // 
            // START_YEAR
            // 
            this.START_YEAR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.START_YEAR.DataPropertyName = "START_YEAR";
            this.START_YEAR.HeaderText = "Périodique - De";
            this.START_YEAR.Name = "START_YEAR";
            this.START_YEAR.ReadOnly = true;
            this.START_YEAR.Visible = false;
            this.START_YEAR.Width = 91;
            // 
            // END_YEAR
            // 
            this.END_YEAR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.END_YEAR.DataPropertyName = "END_YEAR";
            this.END_YEAR.HeaderText = "Périodique - Jusqu\'à";
            this.END_YEAR.Name = "END_YEAR";
            this.END_YEAR.ReadOnly = true;
            this.END_YEAR.Visible = false;
            this.END_YEAR.Width = 91;
            // 
            // IS_CONCERN_WHO
            // 
            this.IS_CONCERN_WHO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.IS_CONCERN_WHO.DataPropertyName = "IS_CONCERN_WHO";
            this.IS_CONCERN_WHO.HeaderText = "Concernant";
            this.IS_CONCERN_WHO.Name = "IS_CONCERN_WHO";
            this.IS_CONCERN_WHO.ReadOnly = true;
            this.IS_CONCERN_WHO.Width = 97;
            // 
            // IS_IMPORTANT
            // 
            this.IS_IMPORTANT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.IS_IMPORTANT.DataPropertyName = "IS_IMPORTANT";
            this.IS_IMPORTANT.HeaderText = "Important?";
            this.IS_IMPORTANT.Name = "IS_IMPORTANT";
            this.IS_IMPORTANT.ReadOnly = true;
            this.IS_IMPORTANT.Width = 92;
            // 
            // ANIM_NUM_IDENs
            // 
            this.ANIM_NUM_IDENs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ANIM_NUM_IDENs.DataPropertyName = "ANIM_NUM_IDENs";
            this.ANIM_NUM_IDENs.HeaderText = "ANIM_NUM_IDENs";
            this.ANIM_NUM_IDENs.Name = "ANIM_NUM_IDENs";
            this.ANIM_NUM_IDENs.ReadOnly = true;
            this.ANIM_NUM_IDENs.Visible = false;
            this.ANIM_NUM_IDENs.Width = 128;
            // 
            // ANIM_ESPECE
            // 
            this.ANIM_ESPECE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ANIM_ESPECE.DataPropertyName = "ANIM_ESPECE";
            this.ANIM_ESPECE.HeaderText = "ANIM_ESPECE";
            this.ANIM_ESPECE.Name = "ANIM_ESPECE";
            this.ANIM_ESPECE.ReadOnly = true;
            this.ANIM_ESPECE.Visible = false;
            this.ANIM_ESPECE.Width = 106;
            // 
            // ANIM_RACE
            // 
            this.ANIM_RACE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ANIM_RACE.DataPropertyName = "ANIM_RACE";
            this.ANIM_RACE.HeaderText = "ANIM_RACE";
            this.ANIM_RACE.Name = "ANIM_RACE";
            this.ANIM_RACE.ReadOnly = true;
            this.ANIM_RACE.Visible = false;
            this.ANIM_RACE.Width = 95;
            // 
            // ANIM_SEXE
            // 
            this.ANIM_SEXE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ANIM_SEXE.DataPropertyName = "ANIM_SEXE";
            this.ANIM_SEXE.HeaderText = "ANIM_SEXE";
            this.ANIM_SEXE.Name = "ANIM_SEXE";
            this.ANIM_SEXE.ReadOnly = true;
            this.ANIM_SEXE.Visible = false;
            this.ANIM_SEXE.Width = 91;
            // 
            // POIDS_MAX
            // 
            this.POIDS_MAX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.POIDS_MAX.DataPropertyName = "POIDS_MAX";
            this.POIDS_MAX.HeaderText = "POIDS_MAX";
            this.POIDS_MAX.Name = "POIDS_MAX";
            this.POIDS_MAX.ReadOnly = true;
            this.POIDS_MAX.Visible = false;
            this.POIDS_MAX.Width = 96;
            // 
            // DATE_NISS_MIN
            // 
            this.DATE_NISS_MIN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DATE_NISS_MIN.DataPropertyName = "DATE_NISS_MIN";
            this.DATE_NISS_MIN.HeaderText = "DATE_NISS_MIN";
            this.DATE_NISS_MIN.Name = "DATE_NISS_MIN";
            this.DATE_NISS_MIN.ReadOnly = true;
            this.DATE_NISS_MIN.Visible = false;
            this.DATE_NISS_MIN.Width = 114;
            // 
            // DATE_NISS_MAX
            // 
            this.DATE_NISS_MAX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DATE_NISS_MAX.DataPropertyName = "DATE_NISS_MAX";
            this.DATE_NISS_MAX.HeaderText = "DATE_NISS_MAX";
            this.DATE_NISS_MAX.Name = "DATE_NISS_MAX";
            this.DATE_NISS_MAX.ReadOnly = true;
            this.DATE_NISS_MAX.Visible = false;
            this.DATE_NISS_MAX.Width = 117;
            // 
            // DESCRIPTION
            // 
            this.DESCRIPTION.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DESCRIPTION.DataPropertyName = "DESCRIPTION";
            this.DESCRIPTION.HeaderText = "Déscription";
            this.DESCRIPTION.Name = "DESCRIPTION";
            this.DESCRIPTION.ReadOnly = true;
            this.DESCRIPTION.Visible = false;
            // 
            // RELATED_CLIENTS_IDS
            // 
            this.RELATED_CLIENTS_IDS.DataPropertyName = "RELATED_CLIENTS_IDS";
            this.RELATED_CLIENTS_IDS.HeaderText = "RELATED_CLIENTS_IDS";
            this.RELATED_CLIENTS_IDS.Name = "RELATED_CLIENTS_IDS";
            this.RELATED_CLIENTS_IDS.ReadOnly = true;
            this.RELATED_CLIENTS_IDS.Visible = false;
            // 
            // ALERT_BEFORE_DAYS
            // 
            this.ALERT_BEFORE_DAYS.DataPropertyName = "ALERT_BEFORE_DAYS";
            this.ALERT_BEFORE_DAYS.HeaderText = "ALERT_BEFORE_DAYS";
            this.ALERT_BEFORE_DAYS.Name = "ALERT_BEFORE_DAYS";
            this.ALERT_BEFORE_DAYS.ReadOnly = true;
            this.ALERT_BEFORE_DAYS.Visible = false;
            // 
            // SEND_ALERT_TO_CABINE_EMAIL
            // 
            this.SEND_ALERT_TO_CABINE_EMAIL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.SEND_ALERT_TO_CABINE_EMAIL.DataPropertyName = "SEND_ALERT_TO_CABINE_EMAIL";
            this.SEND_ALERT_TO_CABINE_EMAIL.HeaderText = "SEND_ALERT_TO_CABINE_EMAIL";
            this.SEND_ALERT_TO_CABINE_EMAIL.Name = "SEND_ALERT_TO_CABINE_EMAIL";
            this.SEND_ALERT_TO_CABINE_EMAIL.ReadOnly = true;
            this.SEND_ALERT_TO_CABINE_EMAIL.Visible = false;
            this.SEND_ALERT_TO_CABINE_EMAIL.Width = 199;
            // 
            // SEND_ALERT_TO_CLIENT_EMAIL
            // 
            this.SEND_ALERT_TO_CLIENT_EMAIL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.SEND_ALERT_TO_CLIENT_EMAIL.DataPropertyName = "SEND_ALERT_TO_CLIENT_EMAIL";
            this.SEND_ALERT_TO_CLIENT_EMAIL.HeaderText = "SEND_ALERT_TO_CLIENT_EMAIL";
            this.SEND_ALERT_TO_CLIENT_EMAIL.Name = "SEND_ALERT_TO_CLIENT_EMAIL";
            this.SEND_ALERT_TO_CLIENT_EMAIL.ReadOnly = true;
            this.SEND_ALERT_TO_CLIENT_EMAIL.Visible = false;
            this.SEND_ALERT_TO_CLIENT_EMAIL.Width = 196;
            // 
            // LAST_ALERT_EMAIL_CABINET_SENT_DATE
            // 
            this.LAST_ALERT_EMAIL_CABINET_SENT_DATE.DataPropertyName = "LAST_ALERT_EMAIL_CABINET_SENT_DATE";
            this.LAST_ALERT_EMAIL_CABINET_SENT_DATE.HeaderText = "LAST_ALERT_EMAIL_CABINET_SENT_DATE";
            this.LAST_ALERT_EMAIL_CABINET_SENT_DATE.Name = "LAST_ALERT_EMAIL_CABINET_SENT_DATE";
            this.LAST_ALERT_EMAIL_CABINET_SENT_DATE.ReadOnly = true;
            this.LAST_ALERT_EMAIL_CABINET_SENT_DATE.Visible = false;
            // 
            // LAST_ALERT_EMAIL_CLIENT_SENT_DATE
            // 
            this.LAST_ALERT_EMAIL_CLIENT_SENT_DATE.DataPropertyName = "LAST_ALERT_EMAIL_CLIENT_SENT_DATE";
            this.LAST_ALERT_EMAIL_CLIENT_SENT_DATE.HeaderText = "LAST_ALERT_EMAIL_CLIENT_SENT_DATE";
            this.LAST_ALERT_EMAIL_CLIENT_SENT_DATE.Name = "LAST_ALERT_EMAIL_CLIENT_SENT_DATE";
            this.LAST_ALERT_EMAIL_CLIENT_SENT_DATE.ReadOnly = true;
            this.LAST_ALERT_EMAIL_CLIENT_SENT_DATE.Visible = false;
            // 
            // IS_FOR_ALL
            // 
            this.IS_FOR_ALL.DataPropertyName = "IS_FOR_ALL";
            this.IS_FOR_ALL.HeaderText = "IS_FOR_ALL";
            this.IS_FOR_ALL.Name = "IS_FOR_ALL";
            this.IS_FOR_ALL.ReadOnly = true;
            this.IS_FOR_ALL.Visible = false;
            // 
            // CABINET_EMAIL_ALREADY_SENT
            // 
            this.CABINET_EMAIL_ALREADY_SENT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CABINET_EMAIL_ALREADY_SENT.DataPropertyName = "CABINET_EMAIL_ALREADY_SENT";
            this.CABINET_EMAIL_ALREADY_SENT.HeaderText = "Cabinet - Email Envoyé";
            this.CABINET_EMAIL_ALREADY_SENT.MinimumWidth = 100;
            this.CABINET_EMAIL_ALREADY_SENT.Name = "CABINET_EMAIL_ALREADY_SENT";
            this.CABINET_EMAIL_ALREADY_SENT.ReadOnly = true;
            // 
            // CLIENT_EMAIL_ALREADY_SENT
            // 
            this.CLIENT_EMAIL_ALREADY_SENT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CLIENT_EMAIL_ALREADY_SENT.DataPropertyName = "CLIENT_EMAIL_ALREADY_SENT";
            this.CLIENT_EMAIL_ALREADY_SENT.HeaderText = "Propriétaire - Email Envoyé";
            this.CLIENT_EMAIL_ALREADY_SENT.MinimumWidth = 100;
            this.CLIENT_EMAIL_ALREADY_SENT.Name = "CLIENT_EMAIL_ALREADY_SENT";
            this.CLIENT_EMAIL_ALREADY_SENT.ReadOnly = true;
            // 
            // NEXT_ALARM
            // 
            this.NEXT_ALARM.DataPropertyName = "NEXT_ALARM";
            this.NEXT_ALARM.HeaderText = "NEXT_ALARM";
            this.NEXT_ALARM.Name = "NEXT_ALARM";
            this.NEXT_ALARM.ReadOnly = true;
            this.NEXT_ALARM.Visible = false;
            // 
            // Vaccin_Alerts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Vaccin_Alerts";
            this.Size = new System.Drawing.Size(616, 173);
            this.Enter += new System.EventHandler(this.Vaccin_Alerts_Enter);
            this.Leave += new System.EventHandler(this.Vaccin_Alerts_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NEXT_DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn VACCIN_NME;
        private System.Windows.Forms.DataGridViewTextBoxColumn IS_PERIODIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn FIXED_DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn EVERY_TXT;
        private System.Windows.Forms.DataGridViewTextBoxColumn EVERY_DAY_NB;
        private System.Windows.Forms.DataGridViewTextBoxColumn START_DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn END_DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn EVERY_MOUNTH_NB;
        private System.Windows.Forms.DataGridViewTextBoxColumn START_YEAR;
        private System.Windows.Forms.DataGridViewTextBoxColumn END_YEAR;
        private System.Windows.Forms.DataGridViewTextBoxColumn IS_CONCERN_WHO;
        private System.Windows.Forms.DataGridViewTextBoxColumn IS_IMPORTANT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANIM_NUM_IDENs;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANIM_ESPECE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANIM_RACE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANIM_SEXE;
        private System.Windows.Forms.DataGridViewTextBoxColumn POIDS_MAX;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATE_NISS_MIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATE_NISS_MAX;
        private System.Windows.Forms.DataGridViewTextBoxColumn DESCRIPTION;
        private System.Windows.Forms.DataGridViewTextBoxColumn RELATED_CLIENTS_IDS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ALERT_BEFORE_DAYS;
        private System.Windows.Forms.DataGridViewTextBoxColumn SEND_ALERT_TO_CABINE_EMAIL;
        private System.Windows.Forms.DataGridViewTextBoxColumn SEND_ALERT_TO_CLIENT_EMAIL;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAST_ALERT_EMAIL_CABINET_SENT_DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAST_ALERT_EMAIL_CLIENT_SENT_DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn IS_FOR_ALL;
        private System.Windows.Forms.DataGridViewTextBoxColumn CABINET_EMAIL_ALREADY_SENT;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLIENT_EMAIL_ALREADY_SENT;
        private System.Windows.Forms.DataGridViewTextBoxColumn NEXT_ALARM;
    }
}
