using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Concesionaria.Clases;

namespace Concesionaria
{
    public partial class FrmVistaPreviaPreVenta : FrmBase
    {
        public FrmVistaPreviaPreVenta()
        {
            InitializeComponent();
        }

        private void FrmVistaPreviaPreVenta_Load(object sender, EventArgs e)
        {
            cFunciones fun = new cFunciones();
            Double Senia = 0;
            Double Total = 0;
            Double Saldo = 0;
            Double Credito = 0;

            if (Principal.CodigoSenia != null)
            {
                Int32 CodPreVenta = Convert.ToInt32(Principal.CodigoSenia);
                cPreVenta pre = new Clases.cPreVenta();
                DataTable trdo = pre.GetPreVentaxCodigo(CodPreVenta);
                if (trdo.Rows.Count > 0)
                {
                    Int32 CodCliente = Convert.ToInt32(trdo.Rows[0]["CodCliente"].ToString());
                    CargarCliente(CodCliente);
                    Int32 CodAuto = Convert.ToInt32(trdo.Rows[0]["CodAutoVendido"].ToString());
                    CargarAuto(CodAuto);
                    if (trdo.Rows[0]["PrecioSenia"].ToString() != "")
                    {
                        Senia = Convert.ToDouble(trdo.Rows[0]["PrecioSenia"].ToString());
                        txtImporteEfectivo.Text = fun.SepararDecimales(Senia.ToString());
                    }

                    if (trdo.Rows[0]["ImporteVenta"].ToString() != "")
                    {  
                        Total = Convert.ToDouble(trdo.Rows[0]["ImporteVenta"].ToString());
                        txtTotalVenta.Text = fun.SepararDecimales(Total.ToString());
                    }

                    if (trdo.Rows[0]["ImporteCreditoBanco"].ToString() != "")
                    {   
                        Credito = Convert.ToDouble(trdo.Rows[0]["ImporteCreditoBanco"].ToString());
                        txtImporteCredito.Text = fun.SepararDecimales(Credito.ToString());
                    }

                    Saldo = Total - Senia;
                    txtSaldo.Text = fun.SepararDecimales(Saldo.ToString());
                }
            }
        }

        private void CargarCliente(Int32 CodCliente)
        {
            cCliente cliente = new cCliente();
            DataTable tcli = cliente.GetClientesxCodigo(CodCliente);
            if (tcli.Rows.Count > 0)
            {
                string nombre = tcli.Rows[0]["Nombre"].ToString();
                nombre = nombre + " " + tcli.Rows[0]["Apellido"].ToString();
                string Direccion = tcli.Rows[0]["Calle"].ToString();
                Direccion = Direccion + " " + tcli.Rows[0]["Numero"].ToString();
                txtDireccion.Text = Direccion;
                txtTelefono.Text = tcli.Rows[0]["Telefono"].ToString();
                txtTelefono.Text = tcli.Rows[0]["Celular"].ToString();
                txtNombre.Text = nombre;
                txtDni.Text = tcli.Rows[0]["NroDocumento"].ToString();
                int b = 0;
                if (tcli.Rows[0]["CodBarrio"].ToString() != "")
                {
                    b = 1;
                    GetCiudadxCodBarrio(Convert.ToInt32(tcli.Rows[0]["CodBarrio"].ToString()));
                }

                if (b == 0)
                {
                    if (tcli.Rows[0]["CodCiudad"].ToString() != "")
                    {
                        //tiene cargada solo la ciudad y no el barrio  
                        Int32 CodCiudad = Convert.ToInt32(tcli.Rows[0]["CodCiudad"].ToString());
                        GetCiudadxCodigo(CodCiudad);
                    }
                }
            }
        }

        private void GetCiudadxCodBarrio(Int32 CodBarrio)
        {
            cCiudad ciudad = new cCiudad();
            DataTable trdo = ciudad.GetCiudadxCodBarrio(CodBarrio);
            if (trdo.Rows.Count > 0)
            {
                string Nombre = trdo.Rows[0]["Nombre"].ToString();
                txtDireccion.Text = txtDireccion.Text + " " + Nombre;
                //BSUCO LA PROVINCIA
                Int32 CodProvincia = Convert.ToInt32(trdo.Rows[0]["CodProvincia"].ToString());
                cProvincia Prov = new Clases.cProvincia();
                string nombreProvincia = Prov.GetNombreProvincia(CodProvincia);
                txtDireccion.Text = txtDireccion.Text + " " + nombreProvincia;
            }
        }

        private void GetCiudadxCodigo(Int32 CodCiudad)
        {
            cCiudad ciudad = new Clases.cCiudad();
            DataTable trdo = ciudad.GetCiudadxId(CodCiudad);
            if (trdo.Rows.Count > 0)
            {
                string Ciudad = trdo.Rows[0]["Nombre"].ToString();
                txtDireccion.Text = txtDireccion.Text + " " + Ciudad;
                Int32 CodProvincia = Convert.ToInt32(trdo.Rows[0]["CodProvincia"].ToString());
                cProvincia prov = new cProvincia();
                string Provincia = prov.GetNombreProvincia(CodProvincia);
                txtDireccion.Text = txtDireccion.Text + " " + Provincia;
            }
        }

        private void CargarAuto(Int32 CodAuto)
        {
            Clases.cAuto auto = new Clases.cAuto();
            DataTable tauto = auto.GetAutoxCodigo(CodAuto);
            {
                if (tauto.Rows.Count > 0)
                {
                    txtMarca.Text = tauto.Rows[0]["Marca"].ToString();
                    txtModelo.Text = tauto.Rows[0]["Descripcion"].ToString();
                    txtChasis.Text = tauto.Rows[0]["Chasis"].ToString();
                    txtMotor.Text = tauto.Rows[0]["Motor"].ToString();
                    txtPatente.Text = tauto.Rows[0]["Patente"].ToString();
                    txtAnio.Text = tauto.Rows[0]["Anio"].ToString();
                    if (tauto.Rows[0]["CodCiudad"].ToString()!="")
                    {
                        Int32 CodCiudad = Convert.ToInt32(tauto.Rows[0]["CodCiudad"].ToString());
                        cCiudad ciudad = new cCiudad();
                        DataTable tb = ciudad.GetCiudadProvincia(CodCiudad);
                        if (tb.Rows.Count >0)
                        {
                            txtLocalidad.Text = tb.Rows[0]["Ciudad"].ToString();
                            txtProvincia.Text = tb.Rows[0]["Provincia"].ToString();
                        }
                    }
                }
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Imprimir();
        }

        private void Imprimir()
        {
            DateTime Fecha = dpFecha.Value;
            int Orden = 0;
            cReporte reporte = new cReporte();
            string Mes = GetMes(Fecha.Month);
            string Parte1 = "", Parte2 = "", Parte3 = "", Parte4 = "";
            string Parte5 = "", Parte6 = "", Parte7 = "";
            string Parte8 = "", Parte9 = "", Parte10 = "";
            Parte1 = "En la Ciudad de San Isidro ";
            Parte1 = Parte1 + " a los " + Fecha.Day.ToString() + " del mes de " + Mes;
            Parte1 = Parte1 + " del año " + Fecha.Year.ToString();
            Parte2 = "Recibí del Sr/Sr " + txtNombre.Text;
            Parte3 = "DNI Nº " + txtDni.Text + " con domicilio en " + txtDireccion.Text;
            Parte4 = "La suma de " + txtImporteEfectivo.Text + "(" + txtTextoImporeEfectivo.Text + ")";
            Parte5 = "como seña por la compra de un / una "  + " Dominio Nº " + txtPatente.Text;
            Parte6 = "Marca " + txtMarca.Text + " Modelo " + txtModelo.Text;
            Parte7 = "Chasis " + txtChasis.Text + " Motor Nº " + txtMotor.Text;
            Parte8 = "del año " + txtAnio.Text + ", ratificado en la localidad de " + txtLocalidad.Text + ", Provincia " + txtProvincia.Text;
            Parte9 = "La venta se realiza por la suma total de " + txtTotalVenta.Text + "(" + txtTextoVenta.Text + ")";
            Parte9 = Parte9 + " siendo el saldo pagado de la siguiente fomra ";
            Parte10 = GetTextoFormaPago();
            reporte.Borrar();
            reporte.Insertar(Orden, Parte1, Parte2, Parte3, Parte4, Parte5,
                Parte6, Parte7, Parte8, Parte9, Parte10);

            FrmReporteSenia frm = new FrmReporteSenia();
            frm.Show();


        }

        private string GetMes(int Mes)
        {
            string Nombre = "";
            switch (Mes)
            {
                case 1:
                    Nombre = "Enero";
                    break;
                case 2:
                    Nombre = "Febrero";
                    break;
                case 3:
                    Nombre = "Marzo";
                    break;
                case 4:
                    Nombre = "Abril";
                    break;
                case 5:
                    Nombre = "Mayo";
                    break;
                case 6:
                    Nombre = "Junio";
                    break;
                case 7:
                    Nombre = "Julio";
                    break;
                case 8:
                    Nombre = "Agosto";
                    break;
                case 9:
                    Nombre = "Septiembre";
                    break;
                case 10:
                    Nombre = "Octubre";
                    break;
                case 11:
                    Nombre = "Noviembre";
                    break;
                case 12:
                    Nombre = "Diciembre";
                    break;
            }
            return Nombre;
        }

        private void txtEfectivoaEntregar_Leave(object sender, EventArgs e)
        {
            cFunciones fun = new cFunciones();
            if (txtEfectivoaEntregar.Text !="")
            {
                txtEfectivoaEntregar.Text = fun.SepararDecimales(txtEfectivoaEntregar.Text);
            }
        }

        private string GetTextoFormaPago()
        {
            string Texto = "";
            string a = "0", b = "0";
            string Rdo = "";
            if (txtEfectivoaEntregar.Text != "")
            {
                a = "1";
            }

            if (txtImporteCredito.Text != "")
            {
                b = "1";
            }

            Rdo = a + b;

            switch(Rdo)
            {
                case "10":
                    Texto = "en efectivo " + txtEfectivoaEntregar.Text;
                    Texto = Texto + "(" + txtTextoEfectivoaEntregar.Text + ")";
                    break;
                case "01":
                    Texto = " Crédito " + txtImporteCredito.Text;
                    Texto = Texto + "(" + txtTextoCredito.Text + ")";
                    break;
                case "11":
                    Texto = "en efectivo " + txtEfectivoaEntregar.Text;
                    Texto = Texto + "(" + txtTextoEfectivoaEntregar.Text + ")";
                    Texto = Texto + " Crédito " + txtImporteCredito.Text;
                    Texto = Texto + "(" + txtTextoCredito.Text + ")";
                    break;
            }

            

            return Texto;
        }
    }
}
