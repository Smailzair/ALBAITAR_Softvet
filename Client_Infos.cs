using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALBAITAR_Softvet
{
    public partial class Client_Infos : UserControl
    {
        public static bool make_refresh = false;
        public static int selected_client_id;
        public Client_Infos(int client_id)
        {
            InitializeComponent();
            //---------------
            selected_client_id = client_id;
            Load_Data();
        }

        private void Load_Data()
        {
            
        }

        private void Client_Infos_Enter(object sender, EventArgs e)
        {
            if (make_refresh)
            {
                make_refresh = false;
                Load_Data();
            }
        }
    }
}
