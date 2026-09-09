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
    public partial class FrmBuscadorCliente : FrmBase
    {
        public FrmBuscadorCliente()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void Buscar()
        {
            cFunciones fun = new cFunciones();
            cCliente cli = new Clases.cCliente();
            string Nombre = "";
            string Apellido = "";
            if (txtNombre.Text != "")
                Nombre = txtNombre.Text;
            if (txtApellido.Text != "")
                Apellido = txtApellido.Text;

            DataTable tb = cli.BuscarCliente(Nombre, Apellido);
            Grilla.DataSource = tb;
            fun.AnchoColumnas(Grilla, "0;40;40;20");

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Grilla.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar un regisstro", "Sistema");
                return;
            }
            Principal.CodigoPrincipalAbm = Grilla.CurrentRow.Cells[0].Value.ToString();
            this.Close();
        }
    }
}
