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
    public partial class FrmAbmAuto : Form
    {
        public FrmAbmAuto()
        {
            InitializeComponent();
        }

        private void FrmAbmAuto_Load(object sender, EventArgs e)
        {  
            Botonera(1);
            Grupo.Enabled = false;  
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LlenarCombo(cmb_CodMarca, "Marca", "Nombre", "CodMarca");
            fun.LlenarCombo(cmbProvincia , "Provincia", "Nombre", "CodProvincia");
            fun.LlenarCombo(cmb_CodTipoCombustible, "TipoCombustible", "Nombre", "Codigo");
            fun.LlenarCombo(cmb_CodTipoUtilitario, "TipoUtilitario", "Nombre", "CodTipo");
            fun.LlenarCombo(cmb_CodSucursal, "Sucursal", "Nombre", "CodSucursal");
         }

        private void InicializarComponentes()
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LlenarCombo(cmb_CodMarca, "Marca", "Nombre", "CodMarca");
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            if (Validar() == true)
            {
                fun.ModificarGenerico(this, "Cliente", "CodCliente", "1");
                //  if (txtCodCLiente.Text =="")
                //      fun.GuardarNuevoGenerico(this, "Cliente");
            }


        }

        private Boolean Validar()
        {
            if (txt_Patente.Text == "")
            {
                MessageBox.Show("Debe ingresar una patente para continuar", Clases.cMensaje.Mensaje());
                return false;
            }

            if (txt_Kilometros.Text == "")
                txt_Kilometros.Text = "0";

            if (txt_Descripcion.Text == "")
            {
                MessageBox.Show("Debe ingresar una descripción para continuar", Clases.cMensaje.Mensaje());
                return false;
            }
            return true;
        }

        private void FrmAbmCliente_Load(object sender, EventArgs e)
        {
            Botonera(1);
            Grupo.Enabled = true;
        }

        private void Botonera(int Jugada)
        {
            switch (Jugada)
            {
                //estado inicial
                case 1:
                    btnNuevo.Enabled = true;
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnAceptar.Enabled = false;
                    btnCancelar.Enabled = false;

                    break;
                case 2:
                    btnNuevo.Enabled = false;
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = true;
                    btnAceptar.Enabled = true;
                    btnCancelar.Enabled = true;

                    break;
                case 3:
                    //viene del buscador
                    btnNuevo.Enabled = true;
                    btnEditar.Enabled = true;
                    btnEliminar.Enabled = true;
                    btnAceptar.Enabled = false;
                    btnCancelar.Enabled = false;


                    break;
            }


        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Botonera(2);
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LimpiarGenerico(this);
            txtCodAuto.Text = "";
            Grupo.Enabled = true;
            
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            FrmConsultaCLiente form = new FrmConsultaCLiente();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.ShowDialog();
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            //CargarJugador(Convert.ToInt32(PRINCIPAL.CDOGIO_JUGADOR));
            if (Principal.CodigoPrincipalAbm != null)
            {
                if (Principal.CodigoPrincipalAbm != "")
                {
                    Botonera(3);
                    txtCodAuto.Text = Principal.CodigoPrincipalAbm.ToString();
                    CargarImagen(Convert.ToInt32(txtCodAuto.Text));
                    if (Principal.CodigoPrincipalAbm != "")
                        fun.CargarControles(this, "Auto", "CodAuto", txtCodAuto.Text);
                    Grupo.Enabled = false;
                    UbicarProvincia(Convert.ToInt32(txtCodAuto.Text));
                    return;
                }

            }


            //if (Principal.CampoIdSecundarioGenerado != "")
            //{

            //    switch (Principal.NombreTablaSecundario)
            //    {
            //        case "Barrio":
            //            fun.LlenarCombo(cmb_CodBarrio, "Barrio", "Nombre", "CodBarrio");
            //            cmb_CodBarrio.SelectedValue = Principal.CampoIdSecundarioGenerado;
            //            break;
            //    }
            //}
        }

        private void CargarImagen(Int32 CodAuto)
        {
            try
            {
                cAuto auto = new Clases.cAuto();
                DataTable trdo = auto.GetAutoxCodigo(CodAuto);
                if (trdo.Rows.Count > 0)
                {
                    if (trdo.Rows[0]["RutaImagen"].ToString() != "")
                    {
                        string Ruta = trdo.Rows[0]["RutaImagen"].ToString();
                        Imagen.Image = System.Drawing.Image.FromFile(Ruta);
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("No se pudo cargar la imagen");
            }
            
        }

        private void UbicarProvincia(Int32 CodAuto)
        {
            cAuto auto = new Clases.cAuto();
            DataTable trdo = auto.GetAutoxCodigo(CodAuto);
            if (trdo.Rows.Count >0)
            {
                if (trdo.Rows[0]["CodProvincia"].ToString ()!="")
                {
                    Int32  CodProv =Convert.ToInt32 (trdo.Rows[0]["CodProvincia"].ToString());
                    cmbProvincia.SelectedValue = CodProv;
                    cCiudad ciudad = new Clases.cCiudad();
                    DataTable tr = ciudad.GetCiudadxCodProvincia(CodProv);
                    cFunciones fun = new cFunciones();
                    fun.LlenarComboDatatable(cmb_CodCiudad, tr, "Nombre", "CodCiudad");
                    if (trdo.Rows[0]["CodCiudad"].ToString() != "")
                    {
                        string CodCiudad = trdo.Rows[0]["CodCiudad"].ToString();
                        cmb_CodCiudad.SelectedValue = CodCiudad.ToString();
                    }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Botonera(2);
            Grupo.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Botonera(1);
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LimpiarGenerico(this);
            txtCodAuto.Text = "";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            if (Validar() == true)
            {
                //se usa por las dudas ingreso ya exista el deni
                //y no grabe repetido el documento
               // UbicaCliente();
                if (txtCodAuto.Text  == "")
                    fun.GuardarNuevoGenerico(this, "Auto");
                else
                    fun.ModificarGenerico(this, "Cliente", "CodAuto", txtCodAuto.Text);
                MessageBox.Show("Datos grabados Correctamente", Clases.cMensaje.Mensaje());
                Botonera(1);
                fun.LimpiarGenerico(this);
                txtCodAuto.Text = "";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string msj = "Confirma eliminar el Cliente ";
            var result = MessageBox.Show(msj, "Información",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                return;
            }
            Clases.cAuto cAuto = new Clases.cAuto();
            if (cAuto.PuedeBorrar(Convert.ToInt32(txtCodAuto.Text)))
            {
                Clases.cFunciones fun = new Clases.cFunciones();
                fun.EliminarGenerico("Cliente", "CodAuto", txtCodAuto.Text);
                MessageBox.Show("El vehículo se ha eliminado de la base", Clases.cMensaje.Mensaje());
                fun.LimpiarGenerico(this);
                txtCodAuto.Text = "";
                Botonera(1);
            }
            else
            {
                MessageBox.Show("El cliente no se puede eliminar, se perderían datos historicos.", Clases.cMensaje.Mensaje());
            }
        }

        private void txt_NroDocumento_TextChanged(object sender, EventArgs e)
        {
            /*
            Int32 CodTipoDoc = 0;
            if (cmbDocumento.SelectedIndex > 0)
                CodTipoDoc = Convert.ToInt32(cmbDocumento.SelectedValue);
            string nroDocumento = txt_NroDocumento.Text;
            Clases.cCliente cliente = new Clases.cCliente();
            DataTable trdo = cliente.GetClientesxNroDoc(CodTipoDoc, nroDocumento);
            if (trdo.Rows.Count > 0)
            {
                txt_Nombre.Text = trdo.Rows[0]["Nombre"].ToString();
                txt_Apellido.Text = trdo.Rows[0]["Apellido"].ToString();
                txtM_Telefono.Text = trdo.Rows[0]["Telefono"].ToString();
                txtM_Celular.Text = trdo.Rows[0]["Celular"].ToString();
                txt_Calle.Text = trdo.Rows[0]["Calle"].ToString();
                txt_Numero.Text = trdo.Rows[0]["Numero"].ToString();
                if (trdo.Rows[0]["CodBarrio"].ToString() != "")
                    cmb_CodBarrio.SelectedValue = trdo.Rows[0]["CodBarrio"].ToString();
                txtCodCLiente.Text = trdo.Rows[0]["CodCliente"].ToString();
            }
            else
            {
                txt_Nombre.Text = "";
                txt_Apellido.Text = "";
                txtM_Telefono.Text = "";
                txtM_Celular.Text = "";
                txtCodCLiente.Text = "";
                cmb_CodBarrio.SelectedIndex = 0;
                txt_Calle.Text = "";
                txt_Numero.Text = "";
            }
              */


        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNuevoBarrio_Click(object sender, EventArgs e)
        {
            Principal.CampoIdSecundario = "CodBarrio";
            Principal.CampoNombreSecundario = "Nombre";
            Principal.NombreTablaSecundario = "Barrio";
            FrmAltaBasica form = new FrmAltaBasica();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.ShowDialog();
        }

       

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            Botonera(2);
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LimpiarGenerico(this);
            txtCodAuto.Text = "";
            Grupo.Enabled = true;
            
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            Botonera(2);
            Grupo.Enabled = true;
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            string msj = "Confirma eliminar la marca ";
            var result = MessageBox.Show(msj, "Información",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                return;
            }
            Clases.cAuto cauto = new Clases.cAuto();
            try
            {
                if (cauto.PuedeBorrar(Convert.ToInt32(txtCodAuto.Text)))
                {
                    Clases.cFunciones fun = new Clases.cFunciones();
                    fun.EliminarGenerico("Auto", "CodAuto", txtCodAuto.Text);
                    MessageBox.Show("El vehículo se ha eliminado de la base", Clases.cMensaje.Mensaje());
                    fun.LimpiarGenerico(this);
                    txtCodAuto.Text = "";
                    Botonera(1);
                }
                else
                {
                    MessageBox.Show("El vehículo no se puede eliminar, se perderían datos historicos.", Clases.cMensaje.Mensaje());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("El vehículo no se puede eliminar, se perderían datos historicos.", Clases.cMensaje.Mensaje());
                throw;
            }
           
           
        }

        private void btnAceptar_Click_1(object sender, EventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            if (Validar() == true)
            {
                //se usa por las dudas ingreso ya exista el deni
                //y no grabe repetido el documento
                UbicaAuto();

                if (txtCodAuto.Text == "")
                {
                    fun.GuardarNuevoGenerico(this, "Auto");
                    if (ChkAltaStock.Checked == true)
                    {
                        DateTime fecha = DateTime.Now;
                        cAuto auto = new Clases.cAuto();
                        cStockAuto stock = new cStockAuto();

                        Int32 CodAuto = auto.GetMaxCodAuto();
                        stock.InsertarStockAuto(CodAuto, fecha.ToShortDateString(), null, Principal.CodUsuarioLogueado, null);
                    }
                }
                else
                {
                    fun.ModificarGenerico(this, "Auto", "CodAuto", txtCodAuto.Text);
                    if (ChkAltaStock.Checked == true)
                    {
                        Int32 CodAuto = Convert.ToInt32(txtCodAuto.Text);
                        Int32 CodStock = 0;
                        DateTime fecha = DateTime.Now;
                        cAuto auto = new Clases.cAuto();
                        cStockAuto stock = new cStockAuto();
                        CodStock = stock.GetMaxCodStockxAutoVigente(CodAuto);
                        if (CodStock == 0)
                            stock.InsertarStockAuto(CodAuto, fecha.ToShortDateString(), null, Principal.CodUsuarioLogueado, null);
                    }
                }

                MessageBox.Show("Datos grabados Correctamente", Clases.cMensaje.Mensaje());
                Botonera(1);
                fun.LimpiarGenerico(this);
                txtCodAuto.Text = "";
                Imagen.Image = null;
                if (cmbProvincia.Items.Count > 0)
                    cmbProvincia.SelectedIndex = 0;
            }
        }

        private void btnAbrir_Click_1(object sender, EventArgs e)
        {
            Principal.OpcionesdeBusqueda = "Patente";
            Principal.TablaPrincipal = "Auto";
            Principal.OpcionesColumnasGrilla = "CodAuto;Patente;Descripcion";
            Principal.ColumnasVisibles = "0;1;1";
            Principal.ColumnasAncho = "0;190;390";
            FrmBuscadorGenerico form = new FrmBuscadorGenerico();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.ShowDialog();
        }

        private void UbicaAuto()
        {
            string Patente = txt_Patente.Text;
            Clases.cAuto auto = new Clases.cAuto();
            DataTable trdo = auto.GetAutoxPatente(Patente);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodAuto"].ToString() != "")
                    txtCodAuto.Text = trdo.Rows[0]["CodAuto"].ToString();
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            Botonera(1);
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LimpiarGenerico(this);
            txtCodAuto.Text = "";
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProvincia.SelectedIndex < 1)
            {
                return;
            } 
            Int32 CodProvincia = Convert.ToInt32(cmbProvincia.SelectedValue);
            cCiudad ciudad = new Clases.cCiudad();
            DataTable trdo = ciudad.GetCiudadxCodProvincia(CodProvincia);
            cFunciones fun = new cFunciones();
            fun.LlenarComboDatatable(cmb_CodCiudad, trdo, "Nombre", "CodCiudad");
        }

        private void btnSubirImagen_Click(object sender, EventArgs e)
        {  
            cImagen imgAuto = new cImagen();
            string NroImagen = imgAuto.GetProximaImagen().ToString();
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                string ruta = file.FileName;
                txt_RutaImagen.Text = ruta;
                Imagen.Image = System.Drawing.Image.FromFile(ruta);
                string Extension = System.IO.Path.GetExtension(file.FileName.ToString());
                string RutaGrabar = imgAuto.GetRuta() + NroImagen + "." + Extension;
                Imagen.Image.Save(RutaGrabar);
                imgAuto.Grabar(Convert.ToInt32(NroImagen));
                txt_RutaImagen.Text = RutaGrabar;
            }
            else
            {
                txt_RutaImagen.Text = "";
            }
        }

        private void btnAgregarCiudad_Click(object sender, EventArgs e)
        {
            if (cmbProvincia.SelectedIndex < 1)
            {
                MessageBox.Show("Debe seleccionar una provincia para continuar");
                return;
            }
            Principal.CodigoPrincipalAbm = "1";
            Principal.CampoIdSecundario = "CodCiudad";
            Principal.CampoNombreSecundario = "Nombre";
            Principal.NombreTablaSecundario = "Ciudad";
            FrmAltaBasica form = new FrmAltaBasica();
            form.FormClosing += new FormClosingEventHandler(ContinuarCombos);
            form.ShowDialog();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Principal.CampoIdSecundario = "CodMarca";
            Principal.CampoNombreSecundario = "Nombre";
            Principal.NombreTablaSecundario = "Marca";
            Principal.CampoIdSecundarioGenerado = "";
            FrmAltaBasica form = new FrmAltaBasica();
            form.FormClosing += new FormClosingEventHandler(ContinuarCombos);
            form.ShowDialog();
        }

        private void BtnNuevaProvincia_Click(object sender, EventArgs e)
        {

            Principal.CampoIdSecundario = "CodProvincia";
            Principal.CampoNombreSecundario = "Nombre";
            Principal.NombreTablaSecundario = "Provincia";
            Principal.CodigoPrincipalAbm = "1";
            FrmAltaBasica form = new FrmAltaBasica();
            form.FormClosing += new FormClosingEventHandler(ContinuarCombos);
            form.ShowDialog();
        }

        private void ContinuarCombos(object sender, FormClosingEventArgs e)
        {
            if (Principal.CampoIdSecundarioGenerado != "")
            {
                Clases.cFunciones fun = new Clases.cFunciones();
                switch (Principal.NombreTablaSecundario)
                {
                    case "Ciudad":
                        Int32 CodCiudad = 0;
                        if (Principal.CodigoPrincipalAbm == "1")
                        {
                            //fun.LlenarCombo(cmbCiudad, "Ciudad", "Nombre", "CodCiudad");                 
                            CodCiudad = Convert.ToInt32(Principal.CampoIdSecundarioGenerado);
                            Int32 CodProvincia = Convert.ToInt32(cmbProvincia.SelectedValue);
                            cCiudad city = new Clases.cCiudad();
                            city.ActualizarProvincia(CodCiudad, CodProvincia);
                            DataTable tbCiudad = city.GetCiudadxCodProvincia(CodProvincia);
                            fun.LlenarComboDatatable(cmb_CodCiudad, tbCiudad, "Nombre", "CodCiudad");
                            cmb_CodCiudad.SelectedValue = Principal.CampoIdSecundarioGenerado;
                        }

                        break;
                    case "Marca":
                        fun.LlenarCombo(cmb_CodMarca, "Marca", "Nombre", "CodMarca");
                        cmb_CodMarca.SelectedValue = Principal.CampoIdSecundarioGenerado;
                        break;
                    case "Barrio":
                        /*
                        Int32 CodCity = Convert.ToInt32(cmbCiudad2.SelectedValue);
                        Int32 CodBarrio = Convert.ToInt32(Principal.CampoIdSecundarioGenerado);
                        cBarrio obj = new cBarrio();
                        obj.ActualizarCiudad(CodBarrio, CodCity);
                        DataTable tbBarrio = obj.GetBarrioxCiudad(CodCity);
                        fun.LlenarComboDatatable(CmbBarrio, tbBarrio, "Nombre", "CodBarrio");
                        // fun.LlenarCombo(CmbBarrio, "Barrio", "Nombre", "CodBarrio");
                        CmbBarrio.SelectedValue = Principal.CampoIdSecundarioGenerado;
                        */
                        break;                      
                    case "tipoutilitario":
                        fun.LlenarCombo(cmb_CodTipoUtilitario, "TipoUtilitario", "Nombre", "CodTipo");
                        cmb_CodTipoUtilitario.SelectedValue = Principal.CampoIdSecundarioGenerado;
                        break;
                    case "Sucursal":
                    //    fun.LlenarCombo(cmbSucursal, "Sucursal", "Nombre", "CodSucursal");
                    //    cmbSucursal.SelectedValue = Principal.CampoIdSecundarioGenerado;

                        break;
                    case "Provincia":
                        if (Principal.CodigoPrincipalAbm == "1")
                        {
                            fun.LlenarCombo(cmbProvincia, "Provincia", "Nombre", "CodProvincia");
                            cmbProvincia.SelectedValue = Principal.CampoIdSecundarioGenerado;
                        }             
                        break;
                }
            }
        }

        private void btnNuevoTipoUtilitario_Click(object sender, EventArgs e)
        {
            Principal.CampoIdSecundario = "CodTipo";
            Principal.CampoNombreSecundario = "Nombre";
            Principal.NombreTablaSecundario = "tipoutilitario";
            FrmAltaBasica form = new FrmAltaBasica();
            form.FormClosing += new FormClosingEventHandler(ContinuarCombos);
            form.ShowDialog();
        }
    }
}
