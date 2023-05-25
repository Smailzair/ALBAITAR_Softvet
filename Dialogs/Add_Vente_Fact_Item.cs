using ALBAITAR_Softvet.Resources;
//using CrystalDecisions.Shared.Json;
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
        DataTable visites_non_facturé;
        DataTable labos_non_facturé;
        decimal prix_vente = 0;
        decimal prix_achat = 0;
        string current_client_nme = "--";
        public Add_Vente_Fact_Item(string client_nme)
        {
            InitializeComponent();
            //-------------------------        
            current_client_nme = client_nme;
        }

        private void Add_Vente_Fact_Item_Load(object sender, EventArgs e)
        {
            checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
            checkBox1.Checked = Properties.Settings.Default.Faire_consom_stock_apres_vente;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            //------------------------------------------     
            services = PreConnection.Load_data("SELECT SERVICE,PRIX FROM ("
                                             + "   SELECT RRR.* FROM("
                                             + "SELECT `ITEM_NME_01` AS 'SERVICE',`ITEM_PRIX_UNIT_01` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NULL AND `ITEM_NME_01` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_02` AS 'SERVICE',`ITEM_PRIX_UNIT_02` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NULL AND `ITEM_NME_02` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_03` AS 'SERVICE',`ITEM_PRIX_UNIT_03` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NULL AND `ITEM_NME_03` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_04` AS 'SERVICE',`ITEM_PRIX_UNIT_04` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NULL AND `ITEM_NME_04` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_05` AS 'SERVICE',`ITEM_PRIX_UNIT_05` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NULL AND `ITEM_NME_05` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_06` AS 'SERVICE',`ITEM_PRIX_UNIT_06` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NULL AND `ITEM_NME_06` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_07` AS 'SERVICE',`ITEM_PRIX_UNIT_07` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NULL AND `ITEM_NME_07` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_08` AS 'SERVICE',`ITEM_PRIX_UNIT_08` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NULL AND `ITEM_NME_08` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_09` AS 'SERVICE',`ITEM_PRIX_UNIT_09` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NULL AND `ITEM_NME_09` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_10` AS 'SERVICE',`ITEM_PRIX_UNIT_10` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NULL AND `ITEM_NME_10` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_11` AS 'SERVICE',`ITEM_PRIX_UNIT_11` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NULL AND `ITEM_NME_11` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_12` AS 'SERVICE',`ITEM_PRIX_UNIT_12` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NULL AND `ITEM_NME_12` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_13` AS 'SERVICE',`ITEM_PRIX_UNIT_13` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NULL AND `ITEM_NME_13` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_14` AS 'SERVICE',`ITEM_PRIX_UNIT_14` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NULL AND `ITEM_NME_14` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_15` AS 'SERVICE',`ITEM_PRIX_UNIT_15` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NULL AND `ITEM_NME_15` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_16` AS 'SERVICE',`ITEM_PRIX_UNIT_16` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NULL AND `ITEM_NME_16` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_17` AS 'SERVICE',`ITEM_PRIX_UNIT_17` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NULL AND `ITEM_NME_17` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_18` AS 'SERVICE',`ITEM_PRIX_UNIT_18` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NULL AND `ITEM_NME_18` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_19` AS 'SERVICE',`ITEM_PRIX_UNIT_19` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NULL AND `ITEM_NME_19` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_20` AS 'SERVICE',`ITEM_PRIX_UNIT_20` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NULL AND `ITEM_NME_20` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_21` AS 'SERVICE',`ITEM_PRIX_UNIT_21` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NULL AND `ITEM_NME_21` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_22` AS 'SERVICE',`ITEM_PRIX_UNIT_22` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NULL AND `ITEM_NME_22` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_23` AS 'SERVICE',`ITEM_PRIX_UNIT_23` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NULL AND `ITEM_NME_23` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_24` AS 'SERVICE',`ITEM_PRIX_UNIT_24` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NULL AND `ITEM_NME_24` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_25` AS 'SERVICE',`ITEM_PRIX_UNIT_25` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NULL AND `ITEM_NME_25` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_26` AS 'SERVICE',`ITEM_PRIX_UNIT_26` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NULL AND `ITEM_NME_26` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_27` AS 'SERVICE',`ITEM_PRIX_UNIT_27` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NULL AND `ITEM_NME_27` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_28` AS 'SERVICE',`ITEM_PRIX_UNIT_28` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NULL AND `ITEM_NME_28` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_29` AS 'SERVICE',`ITEM_PRIX_UNIT_29` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NULL AND `ITEM_NME_29` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_30` AS 'SERVICE',`ITEM_PRIX_UNIT_30` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NULL AND `ITEM_NME_30` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_31` AS 'SERVICE',`ITEM_PRIX_UNIT_31` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NULL AND `ITEM_NME_31` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_32` AS 'SERVICE',`ITEM_PRIX_UNIT_32` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NULL AND `ITEM_NME_32` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_33` AS 'SERVICE',`ITEM_PRIX_UNIT_33` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NULL AND `ITEM_NME_33` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_34` AS 'SERVICE',`ITEM_PRIX_UNIT_34` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NULL AND `ITEM_NME_34` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_35` AS 'SERVICE',`ITEM_PRIX_UNIT_35` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NULL AND `ITEM_NME_35` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_36` AS 'SERVICE',`ITEM_PRIX_UNIT_36` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NULL AND `ITEM_NME_36` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_37` AS 'SERVICE',`ITEM_PRIX_UNIT_37` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NULL AND `ITEM_NME_37` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_38` AS 'SERVICE',`ITEM_PRIX_UNIT_38` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NULL AND `ITEM_NME_38` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_39` AS 'SERVICE',`ITEM_PRIX_UNIT_39` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NULL AND `ITEM_NME_39` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_40` AS 'SERVICE',`ITEM_PRIX_UNIT_40` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NULL AND `ITEM_NME_40` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_41` AS 'SERVICE',`ITEM_PRIX_UNIT_41` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NULL AND `ITEM_NME_41` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_42` AS 'SERVICE',`ITEM_PRIX_UNIT_42` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NULL AND `ITEM_NME_42` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_43` AS 'SERVICE',`ITEM_PRIX_UNIT_43` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NULL AND `ITEM_NME_43` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_44` AS 'SERVICE',`ITEM_PRIX_UNIT_44` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NULL AND `ITEM_NME_44` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_45` AS 'SERVICE',`ITEM_PRIX_UNIT_45` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NULL AND `ITEM_NME_45` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_46` AS 'SERVICE',`ITEM_PRIX_UNIT_46` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NULL AND `ITEM_NME_46` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_47` AS 'SERVICE',`ITEM_PRIX_UNIT_47` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NULL AND `ITEM_NME_47` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_48` AS 'SERVICE',`ITEM_PRIX_UNIT_48` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NULL AND `ITEM_NME_48` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_49` AS 'SERVICE',`ITEM_PRIX_UNIT_49` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NULL AND `ITEM_NME_49` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_50` AS 'SERVICE',`ITEM_PRIX_UNIT_50` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NULL AND `ITEM_NME_50` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_51` AS 'SERVICE',`ITEM_PRIX_UNIT_51` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NULL AND `ITEM_NME_51` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_52` AS 'SERVICE',`ITEM_PRIX_UNIT_52` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NULL AND `ITEM_NME_52` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_53` AS 'SERVICE',`ITEM_PRIX_UNIT_53` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NULL AND `ITEM_NME_53` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_54` AS 'SERVICE',`ITEM_PRIX_UNIT_54` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NULL AND `ITEM_NME_54` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_55` AS 'SERVICE',`ITEM_PRIX_UNIT_55` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NULL AND `ITEM_NME_55` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_56` AS 'SERVICE',`ITEM_PRIX_UNIT_56` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NULL AND `ITEM_NME_56` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_57` AS 'SERVICE',`ITEM_PRIX_UNIT_57` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NULL AND `ITEM_NME_57` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_58` AS 'SERVICE',`ITEM_PRIX_UNIT_58` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NULL AND `ITEM_NME_58` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_59` AS 'SERVICE',`ITEM_PRIX_UNIT_59` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NULL AND `ITEM_NME_59` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_60` AS 'SERVICE',`ITEM_PRIX_UNIT_60` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NULL AND `ITEM_NME_60` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_61` AS 'SERVICE',`ITEM_PRIX_UNIT_61` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NULL AND `ITEM_NME_61` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_62` AS 'SERVICE',`ITEM_PRIX_UNIT_62` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NULL AND `ITEM_NME_62` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_63` AS 'SERVICE',`ITEM_PRIX_UNIT_63` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NULL AND `ITEM_NME_63` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_64` AS 'SERVICE',`ITEM_PRIX_UNIT_64` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NULL AND `ITEM_NME_64` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_65` AS 'SERVICE',`ITEM_PRIX_UNIT_65` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NULL AND `ITEM_NME_65` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_66` AS 'SERVICE',`ITEM_PRIX_UNIT_66` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NULL AND `ITEM_NME_66` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_67` AS 'SERVICE',`ITEM_PRIX_UNIT_67` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NULL AND `ITEM_NME_67` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_68` AS 'SERVICE',`ITEM_PRIX_UNIT_68` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NULL AND `ITEM_NME_68` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_69` AS 'SERVICE',`ITEM_PRIX_UNIT_69` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NULL AND `ITEM_NME_69` IS NOT NULL UNION ALL "
                                             + "SELECT `ITEM_NME_70` AS 'SERVICE',`ITEM_PRIX_UNIT_70` AS 'PRIX',`DATE` FROM tb_factures_vente tbb1 WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NULL AND `ITEM_NME_70` IS NOT NULL UNION ALL ) RRR ORDER BY RRR.`DATE` DESC"
                                             + ") TTB WHERE SERVICE IN ("
                                             + "SELECT `ITEM_NME_01` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NULL AND `ITEM_NME_01` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_02` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NULL AND `ITEM_NME_02` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_03` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NULL AND `ITEM_NME_03` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_04` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NULL AND `ITEM_NME_04` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_05` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NULL AND `ITEM_NME_05` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_06` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NULL AND `ITEM_NME_06` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_07` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NULL AND `ITEM_NME_07` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_08` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NULL AND `ITEM_NME_08` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_09` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NULL AND `ITEM_NME_09` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_10` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NULL AND `ITEM_NME_10` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_11` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NULL AND `ITEM_NME_11` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_12` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NULL AND `ITEM_NME_12` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_13` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NULL AND `ITEM_NME_13` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_14` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NULL AND `ITEM_NME_14` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_15` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NULL AND `ITEM_NME_15` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_16` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NULL AND `ITEM_NME_16` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_17` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NULL AND `ITEM_NME_17` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_18` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NULL AND `ITEM_NME_18` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_19` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NULL AND `ITEM_NME_19` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_20` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NULL AND `ITEM_NME_20` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_21` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NULL AND `ITEM_NME_21` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_22` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NULL AND `ITEM_NME_22` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_23` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NULL AND `ITEM_NME_23` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_24` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NULL AND `ITEM_NME_24` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_25` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NULL AND `ITEM_NME_25` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_26` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NULL AND `ITEM_NME_26` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_27` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NULL AND `ITEM_NME_27` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_28` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NULL AND `ITEM_NME_28` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_29` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NULL AND `ITEM_NME_29` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_30` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NULL AND `ITEM_NME_30` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_31` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NULL AND `ITEM_NME_31` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_32` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NULL AND `ITEM_NME_32` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_33` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NULL AND `ITEM_NME_33` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_34` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NULL AND `ITEM_NME_34` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_35` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NULL AND `ITEM_NME_35` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_36` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NULL AND `ITEM_NME_36` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_37` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NULL AND `ITEM_NME_37` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_38` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NULL AND `ITEM_NME_38` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_39` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NULL AND `ITEM_NME_39` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_40` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NULL AND `ITEM_NME_40` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_41` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NULL AND `ITEM_NME_41` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_42` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NULL AND `ITEM_NME_42` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_43` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NULL AND `ITEM_NME_43` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_44` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NULL AND `ITEM_NME_44` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_45` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NULL AND `ITEM_NME_45` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_46` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NULL AND `ITEM_NME_46` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_47` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NULL AND `ITEM_NME_47` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_48` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NULL AND `ITEM_NME_48` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_49` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NULL AND `ITEM_NME_49` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_50` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NULL AND `ITEM_NME_50` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_51` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NULL AND `ITEM_NME_51` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_52` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NULL AND `ITEM_NME_52` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_53` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NULL AND `ITEM_NME_53` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_54` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NULL AND `ITEM_NME_54` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_55` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NULL AND `ITEM_NME_55` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_56` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NULL AND `ITEM_NME_56` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_57` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NULL AND `ITEM_NME_57` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_58` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NULL AND `ITEM_NME_58` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_59` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NULL AND `ITEM_NME_59` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_60` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NULL AND `ITEM_NME_60` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_61` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NULL AND `ITEM_NME_61` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_62` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NULL AND `ITEM_NME_62` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_63` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NULL AND `ITEM_NME_63` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_64` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NULL AND `ITEM_NME_64` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_65` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NULL AND `ITEM_NME_65` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_66` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NULL AND `ITEM_NME_66` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_67` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NULL AND `ITEM_NME_67` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_68` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NULL AND `ITEM_NME_68` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_69` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NULL AND `ITEM_NME_69` IS NOT NULL UNION "
                                             + "SELECT `ITEM_NME_70` AS 'SERVICE' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NULL AND `ITEM_NME_70` IS NOT NULL "
                                    + ");"
                                                );
            if (services.Rows.Count > 0)
            {
                DataTable services2 = services.AsEnumerable()
                                        .GroupBy(row => row.Field<string>("SERVICE"))
                                        .Select(group => group.First()).CopyToDataTable();
                dataGridView1.DataSource = services2;
            }
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
            if (Vente.tmp_current_client_id > 0)
            {
                string fffd = "";
                Vente.visite_to_update_fact_num.ForEach(ss => fffd += "," + ss);
                fffd = fffd.Length > 0 ? fffd.Substring(1) : fffd;
                visites_non_facturé = PreConnection.Load_data("SELECT tb1.`ID`,tb1.`DATETIME`,tb2.`NME` AS ANIM_NME,tb1.`VISITOR_FULL_NME` FROM tb_visites tb1 LEFT JOIN (SELECT `ID`,`NME` FROM tb_animaux) tb2 ON tb1.`ANIM_ID` = tb2.`ID` WHERE tb1.`ANIM_ID` IN (SELECT `ID` FROM tb_animaux WHERE `CLIENT_ID` = " + Vente.tmp_current_client_id + ")" + (fffd.Length > 0 ? " AND tb1.`ID` NOT IN (" + fffd + ")" : "") +
                    " AND tb1.`ID` NOT IN ("
                                             + "SELECT `ITEM_PROD_CODE_01` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_01`) <= 10 AND `ITEM_NME_01` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_02` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_02`) <= 10 AND `ITEM_NME_02` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_03` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_03`) <= 10 AND `ITEM_NME_03` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_04` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_04`) <= 10 AND `ITEM_NME_04` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_05` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_05`) <= 10 AND `ITEM_NME_05` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_06` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_06`) <= 10 AND `ITEM_NME_06` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_07` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_07`) <= 10 AND `ITEM_NME_07` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_08` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_08`) <= 10 AND `ITEM_NME_08` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_09` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_09`) <= 10 AND `ITEM_NME_09` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_10` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_10`) <= 10 AND `ITEM_NME_10` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_11` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_11`) <= 10 AND `ITEM_NME_11` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_12` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_12`) <= 10 AND `ITEM_NME_12` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_13` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_13`) <= 10 AND `ITEM_NME_13` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_14` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_14`) <= 10 AND `ITEM_NME_14` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_15` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_15`) <= 10 AND `ITEM_NME_15` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_16` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_16`) <= 10 AND `ITEM_NME_16` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_17` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_17`) <= 10 AND `ITEM_NME_17` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_18` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_18`) <= 10 AND `ITEM_NME_18` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_19` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_19`) <= 10 AND `ITEM_NME_19` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_20` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_20`) <= 10 AND `ITEM_NME_20` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_21` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_21`) <= 10 AND `ITEM_NME_21` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_22` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_22`) <= 10 AND `ITEM_NME_22` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_23` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_23`) <= 10 AND `ITEM_NME_23` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_24` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_24`) <= 10 AND `ITEM_NME_24` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_25` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_25`) <= 10 AND `ITEM_NME_25` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_26` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_26`) <= 10 AND `ITEM_NME_26` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_27` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_27`) <= 10 AND `ITEM_NME_27` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_28` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_28`) <= 10 AND `ITEM_NME_28` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_29` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_29`) <= 10 AND `ITEM_NME_29` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_30` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_30`) <= 10 AND `ITEM_NME_30` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_31` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_31`) <= 10 AND `ITEM_NME_31` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_32` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_32`) <= 10 AND `ITEM_NME_32` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_33` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_33`) <= 10 AND `ITEM_NME_33` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_34` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_34`) <= 10 AND `ITEM_NME_34` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_35` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_35`) <= 10 AND `ITEM_NME_35` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_36` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_36`) <= 10 AND `ITEM_NME_36` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_37` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_37`) <= 10 AND `ITEM_NME_37` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_38` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_38`) <= 10 AND `ITEM_NME_38` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_39` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_39`) <= 10 AND `ITEM_NME_39` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_40` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_40`) <= 10 AND `ITEM_NME_40` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_41` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_41`) <= 10 AND `ITEM_NME_41` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_42` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_42`) <= 10 AND `ITEM_NME_42` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_43` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_43`) <= 10 AND `ITEM_NME_43` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_44` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_44`) <= 10 AND `ITEM_NME_44` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_45` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_45`) <= 10 AND `ITEM_NME_45` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_46` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_46`) <= 10 AND `ITEM_NME_46` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_47` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_47`) <= 10 AND `ITEM_NME_47` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_48` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_48`) <= 10 AND `ITEM_NME_48` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_49` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_49`) <= 10 AND `ITEM_NME_49` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_50` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_50`) <= 10 AND `ITEM_NME_50` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_51` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_51`) <= 10 AND `ITEM_NME_51` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_52` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_52`) <= 10 AND `ITEM_NME_52` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_53` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_53`) <= 10 AND `ITEM_NME_53` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_54` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_54`) <= 10 AND `ITEM_NME_54` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_55` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_55`) <= 10 AND `ITEM_NME_55` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_56` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_56`) <= 10 AND `ITEM_NME_56` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_57` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_57`) <= 10 AND `ITEM_NME_57` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_58` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_58`) <= 10 AND `ITEM_NME_58` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_59` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_59`) <= 10 AND `ITEM_NME_59` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_60` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_60`) <= 10 AND `ITEM_NME_60` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_61` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_61`) <= 10 AND `ITEM_NME_61` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_62` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_62`) <= 10 AND `ITEM_NME_62` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_63` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_63`) <= 10 AND `ITEM_NME_63` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_64` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_64`) <= 10 AND `ITEM_NME_64` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_65` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_65`) <= 10 AND `ITEM_NME_65` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_66` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_66`) <= 10 AND `ITEM_NME_66` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_67` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_67`) <= 10 AND `ITEM_NME_67` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_68` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_68`) <= 10 AND `ITEM_NME_68` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_69` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_69`) <= 10 AND `ITEM_NME_69` IS NOT NULL UNION "
                                             + "SELECT `ITEM_PROD_CODE_70` AS 'VISIT' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_70`) <= 10 AND `ITEM_NME_70` IS NOT NULL"
                    + ") ORDER BY `DATETIME`;");
                dataGridView3.DataSource = visites_non_facturé;
                label9.Text = current_client_nme;
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage3);
            }
            //-------------------------------------------------------------
            if (Vente.tmp_current_client_id > 0)
            {
                string kkkd = "";
                Vente.labos_to_update_fact_num.ForEach(ss => kkkd += ",'" + ss + "'");
                kkkd = kkkd.Length > 0 ? kkkd.Substring(1) : kkkd;
                labos_non_facturé = PreConnection.Load_data("SELECT tb88.*,tb77.`ANIM_NME`,tb77.NUM_ANIM_IDENT FROM (SELECT 'Hemogramme' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME` FROM tb_labo_hemogramme UNION ALL "
                                    + "SELECT 'Biochimie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME` FROM tb_labo_biochimie  UNION ALL "
                                    + "SELECT 'Immunologie' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME` FROM tb_labo_immunologie  UNION ALL "
                                    + "SELECT 'Protéinogramme' AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME` FROM tb_labo_proteinogramme  UNION ALL "
                                    + "SELECT TYPE_ANAL AS LABO_NME ,`ID`,`REF`,`ANIM_ID`,`DATE_TIME` FROM tb_labo_autre) tb88 LEFT JOIN (SELECT txx1.`ID` AS `ANIM_ID`,`NUM_IDENTIF` AS NUM_ANIM_IDENT,txx1.`NME` AS ANIM_NME,txx2.`ID` AS `CLIENT_ID` FROM tb_animaux txx1 LEFT JOIN tb_clients txx2 ON txx1.`CLIENT_ID` = txx2.`ID`) tb77 ON tb88.`ANIM_ID` = tb77.`ANIM_ID` "
                                    + "WHERE tb77.`CLIENT_ID` = " + Vente.tmp_current_client_id + " AND tb88.`REF` NOT IN ("
                                             + "SELECT `ITEM_PROD_CODE_01` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_01` IS FALSE AND `ITEM_PROD_CODE_01` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_01`) > 10 AND `ITEM_NME_01` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_02` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_02` IS FALSE AND `ITEM_PROD_CODE_02` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_02`) > 10 AND `ITEM_NME_02` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_03` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_03` IS FALSE AND `ITEM_PROD_CODE_03` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_03`) > 10 AND `ITEM_NME_03` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_04` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_04` IS FALSE AND `ITEM_PROD_CODE_04` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_04`) > 10 AND `ITEM_NME_04` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_05` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_05` IS FALSE AND `ITEM_PROD_CODE_05` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_05`) > 10 AND `ITEM_NME_05` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_06` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_06` IS FALSE AND `ITEM_PROD_CODE_06` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_06`) > 10 AND `ITEM_NME_06` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_07` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_07` IS FALSE AND `ITEM_PROD_CODE_07` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_07`) > 10 AND `ITEM_NME_07` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_08` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_08` IS FALSE AND `ITEM_PROD_CODE_08` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_08`) > 10 AND `ITEM_NME_08` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_09` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_09` IS FALSE AND `ITEM_PROD_CODE_09` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_09`) > 10 AND `ITEM_NME_09` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_10` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_10` IS FALSE AND `ITEM_PROD_CODE_10` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_10`) > 10 AND `ITEM_NME_10` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_11` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_11` IS FALSE AND `ITEM_PROD_CODE_11` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_11`) > 10 AND `ITEM_NME_11` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_12` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_12` IS FALSE AND `ITEM_PROD_CODE_12` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_12`) > 10 AND `ITEM_NME_12` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_13` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_13` IS FALSE AND `ITEM_PROD_CODE_13` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_13`) > 10 AND `ITEM_NME_13` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_14` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_14` IS FALSE AND `ITEM_PROD_CODE_14` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_14`) > 10 AND `ITEM_NME_14` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_15` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_15` IS FALSE AND `ITEM_PROD_CODE_15` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_15`) > 10 AND `ITEM_NME_15` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_16` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_16` IS FALSE AND `ITEM_PROD_CODE_16` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_16`) > 10 AND `ITEM_NME_16` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_17` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_17` IS FALSE AND `ITEM_PROD_CODE_17` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_17`) > 10 AND `ITEM_NME_17` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_18` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_18` IS FALSE AND `ITEM_PROD_CODE_18` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_18`) > 10 AND `ITEM_NME_18` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_19` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_19` IS FALSE AND `ITEM_PROD_CODE_19` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_19`) > 10 AND `ITEM_NME_19` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_20` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_20` IS FALSE AND `ITEM_PROD_CODE_20` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_20`) > 10 AND `ITEM_NME_20` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_21` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_21` IS FALSE AND `ITEM_PROD_CODE_21` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_21`) > 10 AND `ITEM_NME_21` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_22` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_22` IS FALSE AND `ITEM_PROD_CODE_22` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_22`) > 10 AND `ITEM_NME_22` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_23` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_23` IS FALSE AND `ITEM_PROD_CODE_23` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_23`) > 10 AND `ITEM_NME_23` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_24` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_24` IS FALSE AND `ITEM_PROD_CODE_24` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_24`) > 10 AND `ITEM_NME_24` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_25` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_25` IS FALSE AND `ITEM_PROD_CODE_25` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_25`) > 10 AND `ITEM_NME_25` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_26` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_26` IS FALSE AND `ITEM_PROD_CODE_26` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_26`) > 10 AND `ITEM_NME_26` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_27` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_27` IS FALSE AND `ITEM_PROD_CODE_27` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_27`) > 10 AND `ITEM_NME_27` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_28` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_28` IS FALSE AND `ITEM_PROD_CODE_28` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_28`) > 10 AND `ITEM_NME_28` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_29` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_29` IS FALSE AND `ITEM_PROD_CODE_29` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_29`) > 10 AND `ITEM_NME_29` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_30` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_30` IS FALSE AND `ITEM_PROD_CODE_30` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_30`) > 10 AND `ITEM_NME_30` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_31` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_31` IS FALSE AND `ITEM_PROD_CODE_31` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_31`) > 10 AND `ITEM_NME_31` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_32` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_32` IS FALSE AND `ITEM_PROD_CODE_32` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_32`) > 10 AND `ITEM_NME_32` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_33` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_33` IS FALSE AND `ITEM_PROD_CODE_33` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_33`) > 10 AND `ITEM_NME_33` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_34` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_34` IS FALSE AND `ITEM_PROD_CODE_34` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_34`) > 10 AND `ITEM_NME_34` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_35` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_35` IS FALSE AND `ITEM_PROD_CODE_35` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_35`) > 10 AND `ITEM_NME_35` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_36` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_36` IS FALSE AND `ITEM_PROD_CODE_36` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_36`) > 10 AND `ITEM_NME_36` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_37` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_37` IS FALSE AND `ITEM_PROD_CODE_37` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_37`) > 10 AND `ITEM_NME_37` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_38` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_38` IS FALSE AND `ITEM_PROD_CODE_38` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_38`) > 10 AND `ITEM_NME_38` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_39` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_39` IS FALSE AND `ITEM_PROD_CODE_39` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_39`) > 10 AND `ITEM_NME_39` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_40` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_40` IS FALSE AND `ITEM_PROD_CODE_40` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_40`) > 10 AND `ITEM_NME_40` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_41` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_41` IS FALSE AND `ITEM_PROD_CODE_41` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_41`) > 10 AND `ITEM_NME_41` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_42` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_42` IS FALSE AND `ITEM_PROD_CODE_42` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_42`) > 10 AND `ITEM_NME_42` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_43` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_43` IS FALSE AND `ITEM_PROD_CODE_43` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_43`) > 10 AND `ITEM_NME_43` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_44` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_44` IS FALSE AND `ITEM_PROD_CODE_44` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_44`) > 10 AND `ITEM_NME_44` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_45` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_45` IS FALSE AND `ITEM_PROD_CODE_45` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_45`) > 10 AND `ITEM_NME_45` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_46` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_46` IS FALSE AND `ITEM_PROD_CODE_46` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_46`) > 10 AND `ITEM_NME_46` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_47` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_47` IS FALSE AND `ITEM_PROD_CODE_47` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_47`) > 10 AND `ITEM_NME_47` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_48` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_48` IS FALSE AND `ITEM_PROD_CODE_48` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_48`) > 10 AND `ITEM_NME_48` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_49` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_49` IS FALSE AND `ITEM_PROD_CODE_49` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_49`) > 10 AND `ITEM_NME_49` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_50` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_50` IS FALSE AND `ITEM_PROD_CODE_50` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_50`) > 10 AND `ITEM_NME_50` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_51` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_51` IS FALSE AND `ITEM_PROD_CODE_51` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_51`) > 10 AND `ITEM_NME_51` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_52` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_52` IS FALSE AND `ITEM_PROD_CODE_52` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_52`) > 10 AND `ITEM_NME_52` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_53` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_53` IS FALSE AND `ITEM_PROD_CODE_53` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_53`) > 10 AND `ITEM_NME_53` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_54` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_54` IS FALSE AND `ITEM_PROD_CODE_54` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_54`) > 10 AND `ITEM_NME_54` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_55` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_55` IS FALSE AND `ITEM_PROD_CODE_55` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_55`) > 10 AND `ITEM_NME_55` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_56` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_56` IS FALSE AND `ITEM_PROD_CODE_56` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_56`) > 10 AND `ITEM_NME_56` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_57` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_57` IS FALSE AND `ITEM_PROD_CODE_57` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_57`) > 10 AND `ITEM_NME_57` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_58` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_58` IS FALSE AND `ITEM_PROD_CODE_58` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_58`) > 10 AND `ITEM_NME_58` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_59` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_59` IS FALSE AND `ITEM_PROD_CODE_59` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_59`) > 10 AND `ITEM_NME_59` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_60` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_60` IS FALSE AND `ITEM_PROD_CODE_60` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_60`) > 10 AND `ITEM_NME_60` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_61` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_61` IS FALSE AND `ITEM_PROD_CODE_61` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_61`) > 10 AND `ITEM_NME_61` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_62` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_62` IS FALSE AND `ITEM_PROD_CODE_62` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_62`) > 10 AND `ITEM_NME_62` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_63` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_63` IS FALSE AND `ITEM_PROD_CODE_63` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_63`) > 10 AND `ITEM_NME_63` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_64` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_64` IS FALSE AND `ITEM_PROD_CODE_64` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_64`) > 10 AND `ITEM_NME_64` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_65` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_65` IS FALSE AND `ITEM_PROD_CODE_65` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_65`) > 10 AND `ITEM_NME_65` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_66` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_66` IS FALSE AND `ITEM_PROD_CODE_66` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_66`) > 10 AND `ITEM_NME_66` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_67` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_67` IS FALSE AND `ITEM_PROD_CODE_67` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_67`) > 10 AND `ITEM_NME_67` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_68` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_68` IS FALSE AND `ITEM_PROD_CODE_68` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_68`) > 10 AND `ITEM_NME_68` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_69` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_69` IS FALSE AND `ITEM_PROD_CODE_69` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_69`) > 10 AND `ITEM_NME_69` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + " UNION "
                                             + "SELECT `ITEM_PROD_CODE_70` AS 'LAB' FROM tb_factures_vente WHERE `ITEM_IS_PROD_70` IS FALSE AND `ITEM_PROD_CODE_70` IS NOT NULL AND LENGTH(`ITEM_PROD_CODE_70`) > 10 AND `ITEM_NME_70` IS NOT NULL AND `CLIENT_ID` = " + Vente.tmp_current_client_id + ")"
                                             + (kkkd.Length > 0 ? " AND tb88.`REF` NOT IN (" + kkkd + ")" : "") +
                                             " ORDER BY `DATE_TIME`;");
                dataGridView4.DataSource = labos_non_facturé;
                label11.Text = current_client_nme;
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage4);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "SERVICE LIKE '%" + textBox1.Text + "%'";
            }

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
            if (radioButton1.Checked) { textBox2.Focus(); }
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
            else if (tabControl1.SelectedTab == tabPage2)
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
            else if (tabControl1.SelectedTab == tabPage3)
            {
                if (dataGridView3.SelectedRows.Count > 0)
                {
                    label2.Text = dataGridView3.SelectedRows.Count > 0 ? string.Concat("Visite le [", dataGridView3.SelectedRows[0].Cells["DATETIME_VISIT"].Value, "] - d'animal : '", dataGridView3.SelectedRows[0].Cells["ANIM_NME"].Value, "'") : "--";
                    numericUpDown2.Value = 0;
                }
                else
                {
                    label2.Text = "--";
                }
            }
            else if (tabControl1.SelectedTab == tabPage4)
            {
                if (dataGridView4.SelectedRows.Count > 0)
                {
                    label2.Text = dataGridView4.SelectedRows.Count > 0 ? string.Concat("Analyse de (" + dataGridView4.SelectedRows[0].Cells["LABO_LABO_NME"].Value + ") le [", dataGridView4.SelectedRows[0].Cells["LABO_DATE_TIME"].Value, "] - d'animal : '", dataGridView4.SelectedRows[0].Cells["LABO_ANIM_NME"].Value, "'") : "--";
                    numericUpDown2.Value = 0;
                }
                else
                {
                    label2.Text = "--";
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
                else if (tabControl1.SelectedTab == tabPage2) //PRODUIT
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
                else if (tabControl1.SelectedTab == tabPage3) //VISITE
                {
                    //----------------------------------------
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
                    Vente.selected_item.Cells[5].Value = dataGridView3.SelectedRows[0].Cells["IDDD_VISIT"].Value;
                    //----------------------
                    Vente.visite_to_update_fact_num.Add((int)dataGridView3.SelectedRows[0].Cells["IDDD_VISIT"].Value);
                }
                else if (tabControl1.SelectedTab == tabPage4) //ANALYSES
                {
                    //----------------------------------------
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
                    Vente.selected_item.Cells[5].Value = dataGridView4.SelectedRows[0].Cells["LABO_REF"].Value;
                    //----------------------
                    Vente.labos_to_update_fact_num.Add((string)dataGridView4.SelectedRows[0].Cells["LABO_REF"].Value);
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
            decimal mnt = numericUpDown1.Value * numericUpDown2.Value;
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

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                label2.Text = dataGridView3.SelectedRows.Count > 0 ? string.Concat("Visite le [", dataGridView3.SelectedRows[0].Cells["DATETIME_VISIT"].Value, "] - d'animal : '", dataGridView3.SelectedRows[0].Cells["ANIM_NME"].Value, "'") : "--";
            }
            else
            {
                label2.Text = "--";
            }
        }

        private void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                label2.Text = dataGridView4.SelectedRows.Count > 0 ? string.Concat("Analyse de (" + dataGridView4.SelectedRows[0].Cells["LABO_LABO_NME"].Value + ") le [", dataGridView4.SelectedRows[0].Cells["LABO_DATE_TIME"].Value, "] - d'animal : '", dataGridView4.SelectedRows[0].Cells["LABO_ANIM_NME"].Value, "'") : "--";
            }
            else
            {
                label2.Text = "--";
            }

        }
    }
}
