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
    public partial class PanelInferiorEdicion : UserControl
    {
        public event EventHandler CancelarClicked;
        public event EventHandler ValidarClicked;
        public PanelInferiorEdicion()
        {
            InitializeComponent();
            btnCancelar.Click += btnCancelar_Click;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CancelarClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            ValidarClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
