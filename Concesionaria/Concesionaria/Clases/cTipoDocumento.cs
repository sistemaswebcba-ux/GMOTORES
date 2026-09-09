using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
namespace Concesionaria.Clases
{
    public  class cTipoDocumento
    {
        public void UbicaCombo(ComboBox Combo)
        {
            cFunciones fun = new cFunciones();
            string sql = "select * from TipoDocumento ";
            DataTable trdo = cDb.ExecuteDataTable(sql);
            fun.LlenarCombo(Combo, "TipoDocumento", "Nombre", "CodTipoDoc");
            int Defecto = 0;
            int Codigo = 0;
            for (int i = 0; i < trdo.Rows.Count; i++)
            {
                if (trdo.Rows[i]["Defecto"].ToString() != "")
                {
                    Defecto = Convert.ToInt32(trdo.Rows[i]["Defecto"].ToString());
                    Codigo = Convert.ToInt32(trdo.Rows[i]["CodTipoDoc"].ToString());
                }

            }

            if (Defecto == 1)
            {
                Combo.SelectedValue = Codigo;
            }
        }
    }
}
