using ALBAITAR_Softvet.Dialogs;
using MySql.Data.MySqlClient;
using ServiceStack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excc = Microsoft.Office.Interop.Excel;

namespace ALBAITAR_Softvet.Resources
{
    public partial class Vente : Form
    {
       
        public Vente()
        {
            InitializeComponent();
            //----------------------
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Add_Vente_Fact_Item().ShowDialog();
        }
    }
}

