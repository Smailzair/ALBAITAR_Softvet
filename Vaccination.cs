using ALBAITAR_Softvet.Dialogs;
using System;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Vaccination : UserControl
    {
        public Vaccination()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            new Add_Modif_Vaccin().ShowDialog();
        }
    }
}
