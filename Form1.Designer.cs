namespace ALBAITAR_Softvet
{
    partial class Form1
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
            this.Dayy_4 = new System.Windows.Forms.ListView();
            this.label_nb_day_4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // Dayy_4
            // 
            this.Dayy_4.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.Dayy_4.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.Dayy_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Dayy_4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.label_nb_day_4});
            this.Dayy_4.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dayy_4.FullRowSelect = true;
            this.Dayy_4.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.Dayy_4.HideSelection = false;
            this.Dayy_4.Location = new System.Drawing.Point(165, 58);
            this.Dayy_4.Name = "Dayy_4";
            this.Dayy_4.ShowItemToolTips = true;
            this.Dayy_4.Size = new System.Drawing.Size(418, 173);
            this.Dayy_4.TabIndex = 16;
            this.Dayy_4.UseCompatibleStateImageBehavior = false;
            this.Dayy_4.View = System.Windows.Forms.View.Details;
            this.Dayy_4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseMove);
            this.Dayy_4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            // 
            // label_nb_day_4
            // 
            this.label_nb_day_4.Text = "01";
            this.label_nb_day_4.Width = 200;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Dayy_4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Dayy_4;
        private System.Windows.Forms.ColumnHeader label_nb_day_4;
    }
}