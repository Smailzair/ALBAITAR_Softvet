using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class CustomMessageBox : Form
    {
        private Label labelMessage;
        private PictureBox pictureBoxIcon;

        public CustomMessageBox(string message, string title, Bitmap icon)
        {
            InitializeComponent();

            this.Text = title;
            this.labelMessage.Text = message;

            if (icon != null)
                this.pictureBoxIcon.Image = icon;
            else
                this.pictureBoxIcon.Visible = false;

            this.labelMessage.Font = new Font("Century Gothic", 8, FontStyle.Regular);
        }

        private void InitializeComponent()
        {
            this.labelMessage = new Label();
            this.pictureBoxIcon = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.SuspendLayout();

            // labelMessage
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new Point(57, 23);
            this.labelMessage.Margin = new Padding(2, 0, 2, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new Size(35, 13);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "label1";

            // pictureBoxIcon
            this.pictureBoxIcon.Location = new Point(20, 20);
            this.pictureBoxIcon.Margin = new Padding(2, 2, 2, 2);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new Size(32, 32);
            this.pictureBoxIcon.TabIndex = 1;
            this.pictureBoxIcon.TabStop = false;

            // CustomMessageBox
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(292, 75);
            this.Controls.Add(this.pictureBoxIcon);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomMessageBox";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    

}
