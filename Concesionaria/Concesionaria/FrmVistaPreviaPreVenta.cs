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
            if (Principal.CodigoSenia !=null)
            {
                Int32 CodPreVenta = Convert.ToInt32(Principal.CodigoSenia);
                cPreVenta pre = new Clases.cPreVenta();
                DataTable trdo = pre.GetPreVentaxCodigo(CodPreVenta);
                if (trdo.Rows.Count >0)
                {
                    Int32 CodCliente = Convert.ToInt32(trdo.Rows[0]["CodCliente"].ToString());
                    CargarCliente(CodCliente);
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
    }
}
