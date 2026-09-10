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
    public partial class FrmVistaPrevia : Form
    {
        public FrmVistaPrevia()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FrmVistaPrevia_Load(object sender, EventArgs e)
        {
            if (Principal.CodigoPrincipalAbm != null)
            {
                CargarDatos(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                BuscarGastosTransferencia(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                CargarMontosVenta(Convert.ToInt32(Principal.CodigoPrincipalAbm));
            }
        }

        private void CargarMontosVenta(Int32 CodVenta)
        {
            cVenta venta = new Clases.cVenta();
            cGastoTransferencia gasto = new cGastoTransferencia();
            Double ImporteVenta = 0;
            Double GastosTransferencia = 0;
            GastosTransferencia = gasto.GetTotalGastosTransferencia(CodVenta);
            cFunciones fun = new cFunciones();
            DataTable trdo = venta.GetVentaxCodigo(CodVenta);
            if (trdo.Rows.Count >0)
            {
                if (trdo.Rows[0]["ImporteVenta"].ToString ()!="")
                {
                    //al importe de la venta le sumo los gastos de transferencia
                    ImporteVenta = Convert.ToDouble(trdo.Rows[0]["ImporteVenta"].ToString());
                    ImporteVenta = ImporteVenta + GastosTransferencia;
                    txtImporteVenta.Text = fun.SepararDecimales(ImporteVenta.ToString());
                }
            }
        }

        private void CargarDatos(Int32 CodVenta)
        {
            string ExTitular = "";
            string Comprador = "";
            GetAutoPartePago(CodVenta);
            Clases.cCliente cliente = new Clases.cCliente();
            Clases.cVenta venta = new Clases.cVenta();
            DataTable trdo = venta.GetVentaxCodigo(CodVenta);
            
            if (trdo.Rows.Count > 0)
            {
                DateTime Fecha = Convert.ToDateTime(trdo.Rows[0]["Fecha"].ToString());
                txtFecha.Text = Fecha.ToShortDateString();
                Int32 CodCliente = Convert.ToInt32(trdo.Rows[0]["CodCliente"].ToString());
                Comprador = GetDatosClientexCod(CodCliente);
             
                Int32 CodStock = Convert.ToInt32(trdo.Rows[0]["CodStock"].ToString());
                ExTitular = GetExTitular(CodStock);
              
                DataTable tcli = cliente.GetClientesxCodigo(CodCliente);
                if (tcli.Rows.Count > 0)
                {
                    string nombre = tcli.Rows[0]["Nombre"].ToString();
                    nombre = nombre + " " + tcli.Rows[0]["Apellido"].ToString();
                    string Direccion = tcli.Rows[0]["Calle"].ToString();
                    Direccion = Direccion + " " + tcli.Rows[0]["Numero"].ToString();
                    txtDireccion.Text = Direccion;
                    txtTelefono.Text = tcli.Rows[0]["Telefono"].ToString();
                    txtNombre.Text = nombre;
                    txtDni.Text = tcli.Rows[0]["NroDocumento"].ToString();
                    if (tcli.Rows[0]["CodBarrio"].ToString ()!="")
                    {
                        GetCiudad(Convert.ToInt32(tcli.Rows[0]["CodBarrio"].ToString()));
                    }
                }
            }
            txtEfectivo.Text = trdo.Rows[0]["ImporteEfectivo"].ToString();
            txtDocumentos.Text = trdo.Rows[0]["ImporteCredito"].ToString(); 
            Int32 CodAuto = Convert.ToInt32(trdo.Rows[0]["CodAutoVendido"].ToString());
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
                }
            }
            Clases.cFunciones fun = new Clases.cFunciones();
            if (txtEfectivo.Text != "0" && txtEfectivo.Text != "")
            {
                txtEfectivo.Text = fun.SepararDecimales(txtEfectivo.Text);
                txtEfectivo.Text = fun.FormatoEnteroMiles(txtEfectivo.Text);
            }

            if (txtDocumentos.Text != "0" && txtDocumentos.Text != "")
            {
                txtDocumentos.Text = fun.SepararDecimales(txtDocumentos.Text);
                txtDocumentos.Text = fun.FormatoEnteroMiles(txtDocumentos.Text);
                cCuota cuo = new cCuota();
                string tex = cuo.GetTextoCuota(Convert.ToInt32(Principal.CodigoPrincipalAbm));
            }
            Clases.cPrenda prenda = new Clases.cPrenda();
            DataTable trdoPrenda = prenda.GetPrendaxCodVenta(Convert.ToInt32(Principal.CodigoPrincipalAbm));
            if (trdoPrenda.Rows.Count > 0)
            {
                string Importe = trdoPrenda.Rows[0]["Importe"].ToString();
                Importe = fun.SepararDecimales(Importe);
                Importe = fun.FormatoEnteroMiles(Importe);
                txtImportePrenda.Text = Importe;
            }
        }

        private void GetCiudad(Int32 CodBarrio)
        {
            cCiudad ciudad = new cCiudad();
            DataTable trdo = ciudad.GetCiudadxCodBarrio(CodBarrio);
            if (trdo.Rows.Count >0)
            {
                string Nombre = trdo.Rows[0]["Nombre"].ToString();
                txtDireccion.Text = txtDireccion.Text + " " + Nombre; 
            }
        }

        private void GrabarDatos()
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            ImprimirBoletoGMotors();
            /*
            GrabarDatos();

            FrmReporte form = new FrmReporte();
            form.Show();
            */
        }

        private string GetTextoFecha()
        {
            DateTime Fecha = Convert.ToDateTime(txtFecha.Text);
            string NombreMes = "";
            int dia = Fecha.Day;
            int Mes = Fecha.Month;
            int anio = Fecha.Year;
            switch(Mes)
            {
                case 1:
                    NombreMes = "Enero";
                    break;
                case 2:
                    NombreMes = "Febrero";
                    break;
                case 3:
                    NombreMes = "Marzo";
                    break;
                case 4:
                    NombreMes = "Abril";
                    break;
                case 5:
                    NombreMes = "Mayo";
                    break;
                case 6:
                    NombreMes = "Junio";
                    break;
                case 7:
                    NombreMes = "Julio";
                    break;
                case 8:
                    NombreMes = "Agosto";
                    break;
                case 9:
                    NombreMes = "Septiembre";
                    break;
                case 10:
                    NombreMes = "Octubre";
                    break;
                case 11:
                    NombreMes = "Noviembre";
                    break;
                case 12:
                    NombreMes = "Diciembre";
                    break;
            }
            string texto = dia.ToString() + " de " + NombreMes;
            texto = texto + " de " + anio.ToString();
            return texto;
        }

        private void BuscarGastosTransferencia(Int32 CodVenta)
        {
            Clases.cGastoTransferencia gasto = new Clases.cGastoTransferencia();
            DataTable trdo = gasto.GetGastoTransferenciaxCodVenta(CodVenta);
            if (trdo.Rows.Count > 0)
            {
                for (int i = 0; i < trdo.Rows.Count; i++)
                {
                    string Codigo = trdo.Rows[i]["CodGastoTranasferencia"].ToString();
                    string Descripcion = trdo.Rows[i]["Descripcion"].ToString();
                    string Importe = trdo.Rows[i]["Importe"].ToString();
                    AgregarGasto(Codigo, Descripcion, Importe, "Transferencia");
                }
            }
            //AgregarGasto(CmbGastosTransferencia.SelectedValue.ToString(), Descripcion, txtImporteGastoTransferencia.Text, "Transferencia");
        }

        private void AgregarGasto(string Codigo, string Descripcion, string Importe, string Tipo)
        {
                     
        }

      
        public void GetAutoPartePago(Int32 CodVenta)
        {
           
        }

        private void txtVerificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtFirmasyForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtRentas_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtMunicipalidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtCancelacionPrenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtOtros_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtMultas_KeyPress(object sender, KeyPressEventArgs e)
        {
             Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtVerificacion_Leave(object sender, EventArgs e)
        {
           
        }

        private void txtFirmasyForm_Leave(object sender, EventArgs e)
        {  
           
        }

        private void txtRentas_Leave(object sender, EventArgs e)
        {   
            
        }

        private void txtMunicipalidad_Leave(object sender, EventArgs e)
        {   
            
        }

        private void txtCancelacionPrenda_Leave(object sender, EventArgs e)
        {   
           
        }

        private void txtMultas_Leave(object sender, EventArgs e)
        {     
            
        }

        private void txtOtros_Leave(object sender, EventArgs e)
        {  
            
        }

        private string  GetExTitular(Int32 CodStock)
        {
            string Texto = "";
            cStockAuto obj = new cStockAuto();
            DataTable trdo = obj.GetStockxCodigo(CodStock);
            if (trdo.Rows[0]["CodCliente"].ToString()!="")
            {
                if (trdo.Rows.Count > 0)
                {
                    Int32 CodCliente = Convert.ToInt32(trdo.Rows[0]["CodCliente"].ToString()); ;
                    Texto = GetDatosClientexCod(CodCliente);
                }
            }
           
            return Texto;
        }

        private string GetDatosClientexCod(Int32 CodCliente)
        {
            string texto = "";
            string Nom = "";
            string Ape = "";
            string NroDoc = "";
            string Calle = "";
            string Numero = "";
            Int32 CodBarrio = 0;
            string Ciudad = "";
            string Provincia = "";
            cCliente cli = new Clases.cCliente();
            DataTable tbcli = cli.GetClientesxCodigo(CodCliente);
            if (tbcli.Rows.Count > 0)
            {
                Nom = tbcli.Rows[0]["Nombre"].ToString();
                Ape = tbcli.Rows[0]["Apellido"].ToString();
                NroDoc = tbcli.Rows[0]["NroDocumento"].ToString();
                Calle = tbcli.Rows[0]["Calle"].ToString();
                Numero = tbcli.Rows[0]["Numero"].ToString();
                if (tbcli.Rows[0]["CodBarrio"].ToString()!="")
                {
                    CodBarrio = Convert.ToInt32(tbcli.Rows[0]["CodBarrio"].ToString());
                    cBarrio bar = new cBarrio();
                    DataTable tbBarrio = bar.GetBarrioCiudadProvincia(CodBarrio);
                    if (tbBarrio.Rows.Count >0)
                    {
                        if (tbBarrio.Rows[0]["Ciudad"].ToString() != "")
                            Ciudad = tbBarrio.Rows[0]["Ciudad"].ToString();
                        if (tbBarrio.Rows[0]["Provincia"].ToString() != "")
                            Provincia = tbBarrio.Rows[0]["Provincia"].ToString();
                    }
                }
            }
            texto = Ape + " " + Nom;
            if (NroDoc != "")
                texto = texto + ", DNI " + NroDoc.ToString();
            if (Calle != "")
                texto = texto + " con domicilio en calle " + Calle;
            if (Numero != "")
                texto = texto + " Numero " + Numero;
            if (Ciudad != "")
                texto = texto + " de la Ciudad de " + Ciudad;
            if (Provincia != "")
                texto = texto + ", Provincia " + Provincia;
            return texto;
        }

        private void ImprimirBoletoGMotors()
        {
            cReporte reporte = new Clases.cReporte();
            int Orden = 1;
            string Cliente = txtNombre.Text;
            string Parte1 = "", Parte2 = "", Parte3 = "", Parte4 = "";
            string Parte5 = "", Parte6 = "", Parte7 = "", Parte8 = "";
            Parte1 = "Isidro, Vende ";
            Parte1 = Parte1 + "y Transfiere al Señor/a xxx ";
            Parte1 = Parte1.Replace("xxx", Cliente);
            Parte2 = "domiciliado es " + txtDireccion.Text;
            Parte3 = "Marca  " + txtMarca.Text + "       Modelo " + txtModelo.Text + "    Año " + txtAnio.Text;
            Parte4 = "Chasis " + txtChasis.Text +"       Motor " + txtMotor.Text + "      Patente " + txtPatente.Text;
            Parte5 = "Expedido por la municipalidad de en el estado en que se encuentra ,tomando en la fecha el compradodr posesión del mismo de conformidad.";
            Parte6 = "El precio de venta se establece en $ " + txtImporteVenta.Text;
            Parte6 = Parte6 + " (" + txtTextoImporteVenta.Text + ")";
            Parte7 = "Pagados de la siguiente forma ";
            if (txtEfectivo.Text !="")
            {
                Parte7 = Parte7 + " Efectivo " + txtEfectivo.Text + "(" + txtTextoEfectivo.Text + ")";
            }
            Parte8 = "en este acto, sirviendo el presente de suficiente recibo; y el saldo de $ ";
            reporte.Borrar();
            reporte.Insertar(Orden, Parte1, Parte2 , Parte3, Parte4, Parte5, Parte6, Parte7, Parte8, "", "");
            FrmBoletoGMotor frm = new FrmBoletoGMotor();
            frm.Show();

        }
    }
}
