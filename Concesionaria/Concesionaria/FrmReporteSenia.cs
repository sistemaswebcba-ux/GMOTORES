using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Concesionaria
{
    public partial class FrmReporteSenia : FrmBase
    {
        public FrmReporteSenia()
        {
            InitializeComponent();
        }

        private void FrmReporteSenia_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DsReporte.Reporte' table. You can move, or remove it, as needed.
            this.ReporteTableAdapter.Fill(this.DsReporte.Reporte);

            this.reportViewer1.RefreshReport();
        }
    }
}
