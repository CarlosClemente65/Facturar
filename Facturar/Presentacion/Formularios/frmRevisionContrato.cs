using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Presentacion.Paneles;

namespace Facturar.Presentacion.Formularios
{
    public partial class frmRevisionContrato : Form
    {
        private Contrato contratoActual;
        public frmRevisionContrato(Contrato contrato)
        {
            InitializeComponent();
            ConfigurarGrid();

            contratoActual = contrato;

            panelRevisionContrato_general.AltaClicked += PanelInferior_general_altaClicked;
            panelRevisionContrato_general.BajaClicked += PanelInferior_general_bajaClicked;
            panelRevisionContrato_general.EditarClicked += PanelInferior_general_editarClicked;
            panelRevisionContrato_general.EliminarClicked += PanelInferior_general_eliminarClicked;
            panelRevisionContrato_general.SeleccionActivos += PanelRevisionContrato_general_SeleccionActivos;
            panelRevisionContrato_Edicion.ValidarClicked += PanelRevisionContrato_Edicion_ValidarClicked;
            panelRevisionContrato_Edicion.CancelarClicked += PanelRevisionContrato_Edicion_CancelarClicked;
        }

        private void ConfigurarGrid()
        {
            dgvRevisiones.DefaultCellStyle.SelectionBackColor = Color.Wheat;
            dgvRevisiones.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void frmRevisionContrato_Load(object sender, EventArgs e)
        {
            panelRevisionContrato_general.EstadoVisible = true;
        }

        private void PanelInferior_general_altaClicked(object sender, EventArgs e)
        {
            MostrarPanelGeneral(false);
        }

        private void PanelInferior_general_bajaClicked(object sender, EventArgs e)
        {
            MostrarPanelGeneral(false);
        }

        private void PanelInferior_general_editarClicked(object sender, EventArgs e)
        {
            MostrarPanelGeneral(false);
        }

        private void PanelInferior_general_eliminarClicked(object sender, EventArgs e)
        {
            MostrarPanelGeneral(false);

        }

        private void PanelRevisionContrato_general_SeleccionActivos(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PanelRevisionContrato_Edicion_CancelarClicked(object sender, EventArgs e)
        {
            MostrarPanelGeneral(true);
        }

        private void PanelRevisionContrato_Edicion_ValidarClicked(object sender, EventArgs e)
        {
            MostrarPanelGeneral(true);
        }

        // Metodo para mostrar u ocultar los paneles general y edicion alternativamente
        private void MostrarPanelGeneral(bool mostrar)
        {
            panelRevisionContrato_general.Visible = mostrar;
            panelRevisionContrato_Edicion.Visible = !mostrar;
            panelRevisionContrato_general.EstadoVisible = mostrar;
            btnContratos.Visible = mostrar;
        }

        private void btnContratos_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
