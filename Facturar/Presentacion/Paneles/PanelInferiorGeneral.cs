using System;
using System.Drawing;
using System.Windows.Forms;

namespace Facturar.Presentacion.Paneles
{
    public partial class PanelInferior_general : UserControl
    {
        public event EventHandler AltaClicked;
        public event EventHandler BajaClicked;
        public event EventHandler EditarClicked;
        public event EventHandler SeleccionActivos;
        public event EventHandler EliminarClicked;

        public bool EstadoVisible
        {
            get => panelActivos.Visible;
            set => panelActivos.Visible = value;
        }


        public PanelInferior_general()
        {
            InitializeComponent();
            cbEstado.FlatStyle = FlatStyle.Flat;
            cbEstado.BackColor = Color.Wheat;
            cbEstado.ForeColor = Color.Black;
            cbEstado.SelectedItem = "Activos";
        }


        private void btnAlta_Click(object sender, EventArgs e)
        {
            AltaClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            EditarClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            BajaClicked?.Invoke(this, EventArgs.Empty);
        }

        private void cbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeleccionActivos?.Invoke(this, EventArgs.Empty);
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarClicked?.Invoke(this, EventArgs.Empty);
        }

        public string EstadoSeleccionado
        {
            get { return cbEstado.SelectedItem?.ToString(); }
        }
        

        public void MostrarActivos(bool visible)
        {
            panelActivos.Visible = visible;
            if(visible)
            {
                panelActivos.BringToFront();
            }
            cbEstado.SelectedIndex = 0;
            cbEstado.Enabled = true; // Habilita el comboBox de activos porque se deshabilita en el proceso de deshabilitar los textbox
        }
    }
}
