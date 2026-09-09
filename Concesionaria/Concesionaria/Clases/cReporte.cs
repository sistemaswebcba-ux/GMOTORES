using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Concesionaria.Clases
{
    public class cReporte
    {
        public void Insertar (int Orden,string Parte1, string Parte2, string Parte3,
            string Parte4,string Parte5,string Parte6,string Parte7, 
            string Parte8,string Parte9, string Parte10 )
        {
            string sql = "insert into Reporte (Orden,";
            sql = sql + "Parte1,Parte2,Parte3,Parte4,Parte5,";
            sql = sql + "Parte6,Parte7,Parte8,Parte9,Parte10) ";
            sql = sql + " values (" + Orden.ToString();
            sql = sql + "," + "'" + Parte1 + "'";
            sql = sql + "," + "'" + Parte2 + "'";
            sql = sql + "," + "'" + Parte3 + "'";
            sql = sql + "," + "'" + Parte4 + "'";
            sql = sql + "," + "'" + Parte5 + "'";
            sql = sql + "," + "'" + Parte6 + "'";
            sql = sql + "," + "'" + Parte7 + "'";
            sql = sql + "," + "'" + Parte8 + "'";
            sql = sql + "," + "'" + Parte9 + "'";
            sql = sql + "," + "'" + Parte10 + "'";
            sql = sql + ")";
            cDb.ExecutarNonQuery(sql);
        }

        public void Borrar()
        {
            string sql = " delete from Reporte ";
            cDb.ExecutarNonQuery(sql);
        }
    }
}
