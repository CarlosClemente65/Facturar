using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturar.Presentacion.Paneles
{
    public partial class PanelInferiorGeneral : UserControl
    {
        public event EventHandler AltaClicked;
        public event EventHandler BajaClicked;
        public event EventHandler EditarClicked;
        public PanelInferiorGeneral()
        {
            InitializeComponent();
            btnAlta.Click += btnAlta_Click;
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            AltaClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            EditarClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            BajaClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
