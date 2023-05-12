using ALBAITAR_Softvet.Resources;
using CrystalDecisions.Shared.Json;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ALBAITAR_Softvet.Dialogs
{
    public partial class Add_Vente_Fact_Item : Form
    {
        DataTable services;
        DataTable products;
        decimal prix_vente = 0;
        decimal prix_achat = 0;
        public Add_Vente_Fact_Item()
        {
            InitializeComponent();
            //-------------------------        
        }

        private void Add_Vente_Fact_Item_Load(object sender, EventArgs e)
        {
            checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
            checkBox1.Checked = Properties.Settings.Default.Faire_consom_stock_apres_vente;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            //------------------------------------------     
            services = PreConnection.Load_data("SELECT SERVICE,PRIX FROM ("
                                             + "   SELECT RRR.* FROM("
                                             + "SELECT `ITEM_NME_01` AS 'SERVICE',`ITEM_PRIX_UNIT_01` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_NME_01` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_02` AS 'SERVICE',`ITEM_PRIX_UNIT_02` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_NME_02` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_03` AS 'SERVICE',`ITEM_PRIX_UNIT_03` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_NME_03` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_04` AS 'SERVICE',`ITEM_PRIX_UNIT_04` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_NME_04` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_05` AS 'SERVICE',`ITEM_PRIX_UNIT_05` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_NME_05` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_06` AS 'SERVICE',`ITEM_PRIX_UNIT_06` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_NME_06` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_07` AS 'SERVICE',`ITEM_PRIX_UNIT_07` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_NME_07` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_08` AS 'SERVICE',`ITEM_PRIX_UNIT_08` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_NME_08` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_09` AS 'SERVICE',`ITEM_PRIX_UNIT_09` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_NME_09` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_10` AS 'SERVICE',`ITEM_PRIX_UNIT_10` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_NME_10` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_11` AS 'SERVICE',`ITEM_PRIX_UNIT_11` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_NME_11` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_12` AS 'SERVICE',`ITEM_PRIX_UNIT_12` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_NME_12` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_13` AS 'SERVICE',`ITEM_PRIX_UNIT_13` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_NME_13` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_14` AS 'SERVICE',`ITEM_PRIX_UNIT_14` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_NME_14` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_15` AS 'SERVICE',`ITEM_PRIX_UNIT_15` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_NME_15` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_16` AS 'SERVICE',`ITEM_PRIX_UNIT_16` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_NME_16` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_17` AS 'SERVICE',`ITEM_PRIX_UNIT_17` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_NME_17` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_18` AS 'SERVICE',`ITEM_PRIX_UNIT_18` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_NME_18` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_19` AS 'SERVICE',`ITEM_PRIX_UNIT_19` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_NME_19` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_20` AS 'SERVICE',`ITEM_PRIX_UNIT_20` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_NME_20` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_21` AS 'SERVICE',`ITEM_PRIX_UNIT_21` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_NME_21` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_22` AS 'SERVICE',`ITEM_PRIX_UNIT_22` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_NME_22` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_23` AS 'SERVICE',`ITEM_PRIX_UNIT_23` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_NME_23` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_24` AS 'SERVICE',`ITEM_PRIX_UNIT_24` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_NME_24` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_25` AS 'SERVICE',`ITEM_PRIX_UNIT_25` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_NME_25` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_26` AS 'SERVICE',`ITEM_PRIX_UNIT_26` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_NME_26` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_27` AS 'SERVICE',`ITEM_PRIX_UNIT_27` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_NME_27` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_28` AS 'SERVICE',`ITEM_PRIX_UNIT_28` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_NME_28` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_29` AS 'SERVICE',`ITEM_PRIX_UNIT_29` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_NME_29` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_30` AS 'SERVICE',`ITEM_PRIX_UNIT_30` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_NME_30` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_31` AS 'SERVICE',`ITEM_PRIX_UNIT_31` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_NME_31` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_32` AS 'SERVICE',`ITEM_PRIX_UNIT_32` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_NME_32` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_33` AS 'SERVICE',`ITEM_PRIX_UNIT_33` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_NME_33` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_34` AS 'SERVICE',`ITEM_PRIX_UNIT_34` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_NME_34` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_35` AS 'SERVICE',`ITEM_PRIX_UNIT_35` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_NME_35` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_36` AS 'SERVICE',`ITEM_PRIX_UNIT_36` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_NME_36` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_37` AS 'SERVICE',`ITEM_PRIX_UNIT_37` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_NME_37` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_38` AS 'SERVICE',`ITEM_PRIX_UNIT_38` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_NME_38` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_39` AS 'SERVICE',`ITEM_PRIX_UNIT_39` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_NME_39` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_40` AS 'SERVICE',`ITEM_PRIX_UNIT_40` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_NME_40` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_41` AS 'SERVICE',`ITEM_PRIX_UNIT_41` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_NME_41` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_42` AS 'SERVICE',`ITEM_PRIX_UNIT_42` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_NME_42` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_43` AS 'SERVICE',`ITEM_PRIX_UNIT_43` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_NME_43` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_44` AS 'SERVICE',`ITEM_PRIX_UNIT_44` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_NME_44` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_45` AS 'SERVICE',`ITEM_PRIX_UNIT_45` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_NME_45` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_46` AS 'SERVICE',`ITEM_PRIX_UNIT_46` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_NME_46` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_47` AS 'SERVICE',`ITEM_PRIX_UNIT_47` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_NME_47` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_48` AS 'SERVICE',`ITEM_PRIX_UNIT_48` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_NME_48` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_49` AS 'SERVICE',`ITEM_PRIX_UNIT_49` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_NME_49` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_50` AS 'SERVICE',`ITEM_PRIX_UNIT_50` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_NME_50` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_51` AS 'SERVICE',`ITEM_PRIX_UNIT_51` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_NME_51` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_52` AS 'SERVICE',`ITEM_PRIX_UNIT_52` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_NME_52` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_53` AS 'SERVICE',`ITEM_PRIX_UNIT_53` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_NME_53` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_54` AS 'SERVICE',`ITEM_PRIX_UNIT_54` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_NME_54` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_55` AS 'SERVICE',`ITEM_PRIX_UNIT_55` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_NME_55` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_56` AS 'SERVICE',`ITEM_PRIX_UNIT_56` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_NME_56` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_57` AS 'SERVICE',`ITEM_PRIX_UNIT_57` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_NME_57` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_58` AS 'SERVICE',`ITEM_PRIX_UNIT_58` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_NME_58` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_59` AS 'SERVICE',`ITEM_PRIX_UNIT_59` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_NME_59` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_60` AS 'SERVICE',`ITEM_PRIX_UNIT_60` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_NME_60` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_61` AS 'SERVICE',`ITEM_PRIX_UNIT_61` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_NME_61` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_62` AS 'SERVICE',`ITEM_PRIX_UNIT_62` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_NME_62` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_63` AS 'SERVICE',`ITEM_PRIX_UNIT_63` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_NME_63` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_64` AS 'SERVICE',`ITEM_PRIX_UNIT_64` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_NME_64` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_65` AS 'SERVICE',`ITEM_PRIX_UNIT_65` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_NME_65` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_66` AS 'SERVICE',`ITEM_PRIX_UNIT_66` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_NME_66` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_67` AS 'SERVICE',`ITEM_PRIX_UNIT_67` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_NME_67` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_68` AS 'SERVICE',`ITEM_PRIX_UNIT_68` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_NME_68` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_69` AS 'SERVICE',`ITEM_PRIX_UNIT_69` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_NME_69` IS NOT NULL UNION ALL "  
                                             + "SELECT `ITEM_NME_70` AS 'SERVICE',`ITEM_PRIX_UNIT_70` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_NME_70` IS NOT NULL) RRR ORDER BY RRR.`DATE` DESC"
                                             + ") TTB WHERE SERVICE IN (" 
                                                         +"SELECT `ITEM_NME_01` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_NME_01` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_02` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_NME_02` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_03` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_NME_03` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_04` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_NME_04` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_05` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_NME_05` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_06` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_NME_06` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_07` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_NME_07` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_08` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_NME_08` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_09` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_NME_09` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_10` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_NME_10` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_11` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_NME_11` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_12` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_NME_12` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_13` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_NME_13` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_14` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_NME_14` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_15` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_NME_15` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_16` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_NME_16` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_17` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_NME_17` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_18` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_NME_18` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_19` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_NME_19` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_20` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_NME_20` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_21` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_NME_21` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_22` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_NME_22` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_23` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_NME_23` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_24` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_NME_24` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_25` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_NME_25` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_26` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_NME_26` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_27` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_NME_27` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_28` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_NME_28` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_29` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_NME_29` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_30` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_NME_30` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_31` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_NME_31` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_32` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_NME_32` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_33` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_NME_33` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_34` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_NME_34` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_35` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_NME_35` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_36` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_NME_36` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_37` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_NME_37` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_38` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_NME_38` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_39` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_NME_39` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_40` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_NME_40` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_41` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_NME_41` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_42` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_NME_42` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_43` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_NME_43` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_44` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_NME_44` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_45` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_NME_45` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_46` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_NME_46` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_47` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_NME_47` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_48` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_NME_48` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_49` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_NME_49` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_50` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_NME_50` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_51` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_NME_51` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_52` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_NME_52` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_53` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_NME_53` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_54` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_NME_54` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_55` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_NME_55` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_56` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_NME_56` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_57` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_NME_57` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_58` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_NME_58` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_59` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_NME_59` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_60` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_NME_60` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_61` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_NME_61` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_62` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_NME_62` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_63` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_NME_63` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_64` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_NME_64` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_65` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_NME_65` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_66` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_NME_66` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_67` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_NME_67` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_68` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_NME_68` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_69` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_NME_69` IS NOT NULL UNION "
            + "SELECT `ITEM_NME_70` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_NME_70` IS NOT NULL"
                                    +         ");"
                                                );
            DataTable services2 = services.AsEnumerable()
                                        .GroupBy(row => row.Field<string>("SERVICE"))
                                        .Select(group => group.First()).CopyToDataTable();
            dataGridView1.DataSource = services2;
            //------------------------------------------------------------
            products = PreConnection.Load_data("SELECT tb1.`ID`,tb1.`CODE`,tb1.`CATEGOR`,tb1.`NME`,tb1.`REVIENT_PRTICE`,tb1.`VENTE_PRICE`,tb2.SLD FROM tb_produits AS tb1 LEFT JOIN (SELECT `PROD_ID`,SUM(`QNT_IN`) - SUM(`QNT_OUT`) AS SLD FROM tb_stock_mouv GROUP BY `PROD_ID`) AS tb2 ON tb2.`PROD_ID` = tb1.`ID`;");
            foreach (DataRow rww in Vente.stock_to_modify.Rows)
            {
                products.Rows.Cast<DataRow>().Where(x => x["ID"].ToString() == rww["PROD_ID"].ToString()).ToList().ForEach(f =>
                {
                    f.SetField("SLD", ((decimal)f["SLD"] - (decimal)rww["QNT_DIMIN"]));

                    if ((decimal)f["SLD"] <= 0)
                    {
                        f.Delete();
                    }
                });
                
            }
            dataGridView2.DataSource = products;
            //-------------------------------------------------------------


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "SERVICE LIKE '%" + textBox1.Text + "%'";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string fltr = "CODE LIKE '%" + textBox3.Text + "%'";
            fltr += " OR CATEGOR LIKE '%" + textBox3.Text + "%'";
            fltr += " OR NME LIKE '%" + textBox3.Text + "%'";
            ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = fltr;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = radioButton1.Checked;
            textBox1.Enabled = dataGridView1.Enabled = radioButton2.Checked;
            numericUpDown1.BackColor = numericUpDown2.BackColor = textBox2.BackColor = SystemColors.Window;
            if(radioButton1.Checked) { textBox2.Focus(); }
            //----------
            make_select();
        }

        private void make_select()
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                if (radioButton1.Checked)
                {
                    label2.Text = textBox2.Text.Trim().Length > 0 ? textBox2.Text : "--";
                }
                else
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        label2.Text = (string)dataGridView1.SelectedRows[0].Cells["SERVICESS"].Value;
                        numericUpDown2.Value = (decimal)dataGridView1.SelectedRows[0].Cells["PRIX"].Value;
                    }
                    else
                    {
                        label2.Text = "--";
                        numericUpDown2.Value = 0;
                    }
                }
            }
            else
            {
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    label2.Text = string.Concat("[", dataGridView2.SelectedRows[0].Cells["CODE"].Value, "] - ", dataGridView2.SelectedRows[0].Cells["NME"].Value);
                    numericUpDown2.Value = (decimal)(dataGridView2.SelectedRows[0].Cells["VENTE_PRICE"].Value != DBNull.Value ? dataGridView2.SelectedRows[0].Cells["VENTE_PRICE"].Value : (dataGridView2.SelectedRows[0].Cells["REVIENT_PRTICE"].Value != DBNull.Value ? dataGridView2.SelectedRows[0].Cells["REVIENT_PRTICE"].Value : 0));
                }
                else
                {
                    label2.Text = "--";
                    numericUpDown2.Value = 0;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.BackColor = SystemColors.Window;
            label2.Text = textBox2.Text.Trim().Length > 0 ? textBox2.Text : "--";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Enabled && tabControl1.SelectedTab == tabPage1)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    label2.Text = (string)dataGridView1.SelectedRows[0].Cells["SERVICESS"].Value;
                    numericUpDown2.Value = (decimal)dataGridView1.SelectedRows[0].Cells["PRIX"].Value;

                }
                else
                {
                    label2.Text = "--";
                    numericUpDown2.Value = 0;
                }
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.Enabled && tabControl1.SelectedTab == tabPage2)
            {
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    label2.Text = string.Concat("[", dataGridView2.SelectedRows[0].Cells["CODE"].Value, "] - ", dataGridView2.SelectedRows[0].Cells["NME"].Value);
                    numericUpDown2.Value = (decimal)(dataGridView2.SelectedRows[0].Cells["VENTE_PRICE"].Value != DBNull.Value ? dataGridView2.SelectedRows[0].Cells["VENTE_PRICE"].Value : (dataGridView2.SelectedRows[0].Cells["REVIENT_PRTICE"].Value != DBNull.Value ? dataGridView2.SelectedRows[0].Cells["REVIENT_PRTICE"].Value : 0));
                    prix_vente = (decimal)(dataGridView2.SelectedRows[0].Cells["VENTE_PRICE"].Value != DBNull.Value ? dataGridView2.SelectedRows[0].Cells["VENTE_PRICE"].Value : 0);
                    prix_achat = (decimal)(dataGridView2.SelectedRows[0].Cells["REVIENT_PRTICE"].Value != DBNull.Value ? dataGridView2.SelectedRows[0].Cells["REVIENT_PRTICE"].Value : 0);
                }
                else
                {
                    prix_vente = prix_achat = 0;
                    label2.Text = "--";
                    numericUpDown2.Value = 0;
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown2.Value = 0;
            numericUpDown1.BackColor = numericUpDown2.BackColor = textBox2.BackColor = SystemColors.Window;
            checkBox1.Visible = tabControl1.SelectedTab == tabPage2;
            make_select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool all_ready = true;
            all_ready &= label2.Text.Length > 0 && label2.Text != "--";
            all_ready &= numericUpDown1.Value > 0;
            all_ready &= numericUpDown2.Value > 0;
            numericUpDown1.BackColor = numericUpDown1.Value > 0 ? SystemColors.Window : Color.LightCoral;
            numericUpDown2.BackColor = numericUpDown2.Value > 0 ? SystemColors.Window : Color.LightCoral;
            textBox2.BackColor = radioButton1.Checked && textBox2.Text.Trim().Length == 0 ? Color.LightCoral : SystemColors.Window;
            if (tabControl1.SelectedTab == tabPage2 && dataGridView2.SelectedRows.Count > 0)
            {
                if (numericUpDown1.Value > (decimal)dataGridView2.SelectedRows[0].Cells["SLD"].Value && checkBox1.Checked)
                {
                    all_ready = false;
                    numericUpDown1.BackColor = Color.LightCoral;
                    MessageBox.Show("Le stock actuel (avec d'autres élemnents de cette facture) ne permet pas cette consommation.\n\nDeux choix:\n**Réduisez le montant.\n** Décocher l'option 'Faire consommation stock', puis suivre le control manuallement.\n", "Stock insuffisant:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (numericUpDown2.Value > 0)
                    {
                        if (numericUpDown2.Value >= prix_achat)
                        {
                            if (prix_vente > 0 && numericUpDown2.Value < prix_vente)
                            {
                                all_ready &= MessageBox.Show("Le prix unitaire que vous avez choisi est inférieur au prix de vente prévu pour ce produit.\n\nPrix de vente : " + prix_vente.ToString("N2") + " DA.\n\nContinuerez-vous?", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
                            }
                        }
                        else if (prix_achat > 0)
                        {
                            all_ready &= MessageBox.Show("Le prix unitaire que vous avez choisi est inférieur au prix auquel vous avez acheté ce produit.\n\nPrix de revient : " + prix_achat.ToString("N2") + " DA.\n\nContinuerez-vous?", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
                        }
                    }
                }

            }
            if (all_ready)
            {
                if (tabControl1.SelectedTab == tabPage1) //SERVICE
                {
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[0].Value = "Service";
                    //----------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[1].Value = label2.Text;
                    //----------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[2].Value = numericUpDown1.Value;
                    //----------------------    
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[3].Value = numericUpDown2.Value;
                    //----------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[4].Value = numericUpDown1.Value * numericUpDown2.Value;
                    //----------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[5].Value = DBNull.Value;
                    //----------------------
                }
                else //PRODUIT
                {
                    if (checkBox1.Checked)
                    {
                        //-----------------Stock ---
                        if (Vente.stock_to_modify.Rows.Cast<DataRow>().Where(Z => Z["PROD_CODE"].ToString() == dataGridView2.SelectedRows[0].Cells["CODE"].Value.ToString()).ToList().Count > 0)
                        {
                            Vente.stock_to_modify.Rows.Cast<DataRow>().Where(Z => Z["PROD_CODE"].ToString() == dataGridView2.SelectedRows[0].Cells["CODE"].Value.ToString()).ToList().ForEach(RR => { RR["QNT_DIMIN"] = ((decimal)RR["QNT_DIMIN"] + numericUpDown1.Value); });
                        }
                        else
                        {
                            DataRow rw = Vente.stock_to_modify.NewRow();
                            rw["PROD_ID"] = (int)dataGridView2.SelectedRows[0].Cells["ID"].Value;
                            rw["PROD_CODE"] = dataGridView2.SelectedRows[0].Cells["CODE"].Value.ToString();
                            rw["QNT_DIMIN"] = numericUpDown1.Value;
                            Vente.stock_to_modify.Rows.Add(rw);
                        }

                    }
                    //----------------------------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[0].Value = "Produit";
                    //----------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[1].Value = label2.Text;
                    //----------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[2].Value = numericUpDown1.Value;
                    //----------------------    
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[3].Value = numericUpDown2.Value;
                    //----------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[4].Value = numericUpDown1.Value * numericUpDown2.Value;
                    //----------------------
                    Vente.selected_item.Cells.Add(new DataGridViewTextBoxCell());
                    Vente.selected_item.Cells[5].Value = dataGridView2.SelectedRows[0].Cells["CODE"].Value;
                    //----------------------

                }
                Close();

            }


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.BackColor = SystemColors.Window;
            //-----------------
            decimal mnt = numericUpDown1.Value * numericUpDown2.Value;
            label7.Text = mnt.ToString("N2") + " DA";
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.BackColor = SystemColors.Window;
            //-----------------
            decimal mnt  = numericUpDown1.Value * numericUpDown2.Value;
            label7.Text = mnt.ToString("N2") + " DA";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Visible)
            {
                if (!checkBox1.Checked)
                {
                    if (MessageBox.Show("Avec cette sélection, le contrôle des stocks ne sera pas automatique.\n\nVous devez l'ajuster manuellement après la vente.\n\nVoulez-vous continuer?", "Attention :", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {


                    }
                    else
                    {
                        checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
                        checkBox1.Checked = true;
                        checkBox1.CheckedChanged += checkBox1_CheckedChanged;
                    }
                }
            }
            Properties.Settings.Default.Faire_consom_stock_apres_vente = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void tabPage1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!dataGridView1.Enabled)
            {
                if (dataGridView1.Bounds.Contains(e.Location))
                {
                    radioButton2.Checked = true;
                    //----------------

                }
            }
            else if (textBox2.Bounds.Contains(e.Location))
            {
                radioButton1.Checked = true;
                
            }

        }


    }
}
