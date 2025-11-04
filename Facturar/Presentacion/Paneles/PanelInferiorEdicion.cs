using System;
using System.Diagnostics;
using System.Windows.Forms;
using UtilesUI = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Paneles
{
    public partial class PanelInferior_Edicion : UserControl
    {
        public event EventHandler CancelarClicked;
        public event EventHandler ValidarClicked;
        public event EventHandler CancelarMouseDown;


        public PanelInferior_Edicion()
        {
            InitializeComponent();
            btnCancelar.Click += btnCancelar_Click;
        }
        
        private void btnValidar_Click(object sender, EventArgs e)
        {
            ValidarClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CancelarClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
