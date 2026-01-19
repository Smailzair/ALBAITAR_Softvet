
using ALBAITAR_Softvet.Resources;
using System;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Client_Finance_Duplicate_Msg : Form
    {
        private readonly Resources.Clients _clt;

        public string UserChoice { get; set; }

        public Client_Finance_Duplicate_Msg(Clients clnt, string fact_ref)
        {
            InitializeComponent();
            _clt = clnt;
            label1.Text = "Vous avez écrit une référence à une facture existante dans une autre transaction financière,\r\n('"+fact_ref+"')\r\n\r\nContinuer comme méme?\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserChoice = "continue";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserChoice = "modify";
        }
    }
}
