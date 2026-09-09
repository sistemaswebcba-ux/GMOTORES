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
    public partial class FrmVerCliente : Form
    {
        public FrmVerCliente()
        {
            InitializeComponent();
        }

        private void FrmVerCliente_Load(object sender, EventArgs e)
        {
            cFunciones fun = new Clases.cFunciones();
            fun.LlenarCombo(cmbProvincia2, "Provincia", "Nombre", "CodProvincia");
            fun.LlenarCombo(cmb_CodTipoDoc, "TipoDocumento", "Nombre", "CodTipoDoc");
            if (cmb_CodTipoDoc.Items.Count > 0)
                cmb_CodTipoDoc.SelectedIndex = 1;

            if (Principal.CodigoPrincipalAbm != "")
            {
                fun.CargarControles(this, "Cliente", "CodCliente", Principal.CodigoPrincipalAbm);
                cCliente cli = new cCliente();
                DataTable tbCLi = cli.GetClientesxCodigo(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                if (tbCLi.Rows.Count > 0)
                {
                    if (tbCLi.Rows[0]["CodBarrio"].ToString() != "")
                    {
                        Int32 CodBarrio = Convert.ToInt32(tbCLi.Rows[0]["CodBarrio"].ToString());
                        CargarCiudadxBarrio(CodBarrio);
                    }

                }
            }
               

        }

        private void CargarCiudadxBarrio(Int32 CodBarrio)
        {
            if (CodBarrio > 0)
            {
                cBarrio barrio = new cBarrio();
                DataTable tbBarrio = barrio.GetBarrioxId(CodBarrio);
                if (tbBarrio.Rows.Count > 0)
                {
                    if (tbBarrio.Rows[0]["CodCiudad"].ToString() != "")
                    {
                        Int32 CodCiudad = Convert.ToInt32(tbBarrio.Rows[0]["CodCiudad"].ToString());
                        cCiudad objCiudad = new cCiudad();
                        DataTable tbCiudad = objCiudad.GetCiudadxId(CodCiudad);
                        if (tbCiudad.Rows.Count > 0)
                        {
                            if (tbCiudad.Rows[0]["CodProvincia"].ToString() != "")
                            {
                                Int32 CodProvincia = Convert.ToInt32(tbCiudad.Rows[0]["CodProvincia"].ToString());
                                cmbProvincia2.SelectedValue = CodProvincia.ToString();
                                DataTable trCiudad = objCiudad.GetCiudadxCodProvincia(CodProvincia);
                                cFunciones fun = new cFunciones();
                                fun.LlenarComboDatatable(cmbCiudad2, trCiudad, "Nombre", "CodCiudad");
                                cmbCiudad2.SelectedValue = CodCiudad.ToString();
                                cmb_CodBarrio.SelectedValue = CodBarrio.ToString();
                            }
                        }
                    }
                }
            }
        }

        private void cmbCiudad2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCiudad2.SelectedIndex < 1)
            {
                // MessageBox.Show("Seleccione una ciudad");
                return;
            }

            Int32 CodCiudad = Convert.ToInt32(cmbCiudad2.SelectedValue);
            cBarrio barrio = new cBarrio();
            DataTable tbBarrio = barrio.GetBarrioxCiudad(CodCiudad);
            cFunciones fun = new cFunciones();
            fun.LlenarComboDatatable(cmb_CodBarrio, tbBarrio, "Nombre", "CodBarrio");
        }
    }
}
