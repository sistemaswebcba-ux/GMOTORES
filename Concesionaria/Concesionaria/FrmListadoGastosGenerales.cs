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
    public partial class FrmListadoGastosGenerales : Form
    {
        public FrmListadoGastosGenerales()
        {
            InitializeComponent();
        }

        private void FrmListadoGastosGenerales_Load(object sender, EventArgs e)
        {
            DateTime fechahoy = DateTime.Now;
            txtFechaHasta.Text = fechahoy.ToShortDateString();
            fechahoy = fechahoy.AddMonths(-1);
            txtFechaDesde.Text = fechahoy.ToShortDateString();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            if (fun.ValidarFecha(txtFechaDesde.Text) == false)
            {
                MessageBox.Show("Fecha desde incorrecta", Clases.cMensaje.Mensaje());
                return;
            }

            if (fun.ValidarFecha(txtFechaHasta.Text) == false)
            {
                MessageBox.Show("Fecha hasta incorrecta", Clases.cMensaje.Mensaje());
                return;
            }

            if (Convert.ToDateTime(txtFechaDesde.Text) > Convert.ToDateTime(txtFechaHasta.Text))
            {
                MessageBox.Show("La fecha desde debe ser inferior a la fecha hasta", Clases.cMensaje.Mensaje());
                return;
            }

            Buscar();
            
        }

        public void Buscar()
        {
            cFunciones fun = new cFunciones();
            DateTime FechaDesde = Convert.ToDateTime(txtFechaDesde.Text);
            DateTime FechaHasta = Convert.ToDateTime(txtFechaHasta.Text);
            Clases.cGastosNegocio gasto = new Clases.cGastosNegocio();
            DataTable trdo = gasto.GetGastosNegocioxFecha(FechaDesde, FechaHasta, txtDescripcion.Text);
            trdo = fun.TablaaMiles(trdo, "Importe");
            txtTotal.Text = fun.TotalizarColumna(trdo, "Importe").ToString();
            txtTotal.Text = fun.FormatoEnteroMiles(txtTotal.Text);
            Grilla.DataSource = trdo;
            Grilla.Columns[0].Visible = false;
            Grilla.Columns[2].Width = 523;
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            if (Grilla.CurrentRow == null )
            {
                MessageBox.Show("Debe seleccionar un elemento ");
                return;
            }

            string msj = "Confirma eliminar el registro ";
            var result = MessageBox.Show(msj, "Información",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                return;
            }

            string Descripcion = "Anulación de gasto";
            DateTime Fecha = DateTime.Now;
            Double Importe = 0;
            Int32 CodGasto = Convert.ToInt32(Grilla.CurrentRow.Cells[0].Value.ToString());
            cGastosNegocio gastos = new Clases.cGastosNegocio();
            Importe = gastos.AnulagGasto(CodGasto);
            Clases.cMovimiento mov = new Clases.cMovimiento();
            mov.RegistrarMovimientoDescripcion(-1, Principal.CodUsuarioLogueado,Importe, 0, 0, 0, 0, Fecha, Descripcion);
            MessageBox.Show("Datos grabados correctamente");
            Buscar();
        }
    }
}
