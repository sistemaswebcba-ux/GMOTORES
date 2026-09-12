using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Concesionaria.Clases
{
    public class cProvincia
    {
        public DataTable GetProvinciaxCodigo(Int32 CodProvincia)
        {
            string sql = "select * from Provincia ";
            sql = sql + " where CodProvincia =" + CodProvincia.ToString();
            return cDb.ExecuteDataTable(sql);
        }

        public string GetNombreProvincia(Int32 CodProvincia)
        {
            string Nombre = "";
            string sql = "select * from Provincia ";
            sql = sql + " where CodProvincia =" + CodProvincia.ToString();
            DataTable tb = cDb.ExecuteDataTable(sql);
            if (tb.Rows.Count >0)
            {
                Nombre = tb.Rows[0]["Nombre"].ToString(); 
            }
            return Nombre;
        }
    }
}
