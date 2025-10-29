using System;
using System.Windows.Forms;

namespace Facturar.Presentacion.Paneles
{
    public partial class PanelInferiorFacturas : UserControl
    {
        public event EventHandler AltaClicked;
        public event EventHandler GenerarClicked;
        public event EventHandler EditarClicked;
        public event EventHandler EliminarClicked;
        public PanelInferiorFacturas()
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
            EliminarClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            GenerarClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
