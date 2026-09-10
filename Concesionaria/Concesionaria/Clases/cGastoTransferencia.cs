using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Concesionaria.Clases
{
     public class cGastoTransferencia
    {
         public DataTable GetGastoTransferenciaxCodVenta(Int32 CodVenta)
         {
             string sql = " select *";
             sql = sql + " from GastosTransferencia g,CategoriaGastoTransferencia cg";
             sql = sql + " where g.CodGastoTranasferencia = cg.Codigo";
             sql = sql + " and g.CodVenta =" + CodVenta.ToString();
             return cDb.ExecuteDataTable(sql);
         }

        public Double GetTotalGastosTransferencia(Int32 CodVenta)
        {
            Double Total = 0;
            string sql = " select isnull(sum(Importe),0) as Importe ";
            sql = sql + " from GastosTransferencia ";
            sql = sql + " where CodVenta =" + CodVenta.ToString();
            DataTable trdo = cDb.ExecuteDataTable(sql);
            if (trdo.Rows.Count > 0)
                if (trdo.Rows[0]["Importe"].ToString() != "")
                    Total = Convert.ToDouble(trdo.Rows[0]["Importe"].ToString());
            return Total;
        }
    }
}
