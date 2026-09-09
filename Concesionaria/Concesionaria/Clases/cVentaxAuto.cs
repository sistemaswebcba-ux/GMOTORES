using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using Concesionaria.Clases;
namespace Concesionaria.Clases
{
    public class cVentaxAuto
    {
        public DataTable GetAutosxCodVenta(Int32 CodVenta)
        {
            string sql = "select va.CodAuto,a.Patente,a.Descripcion";
            sql = sql + ",m.Nombre,va.Importe,sa.CodStock";
            sql = sql + " from VentasxAuto va, Auto a, StockAuto sa";
            sql = sql + " ,Marca m";
            sql = sql + " where va.CodAuto = a.CodAuto ";
            sql = sql + " and va.CodAuto =sa.CodAuto    ";
            sql = sql + " and m.CodMarca = a.CodMarca ";
            sql = sql + " and va.CodVenta=" + CodVenta.ToString();
            return cDb.ExecuteDataTable(sql);
        }
    }
}
