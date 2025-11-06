using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Presentacion.Controles;
using Facturar.Servicios;
using Utiles = Facturar.Utilidades.UtilesGenerales;
using UtilesUI = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Formularios
{
    public partial class frmRevisionContrato : Form
    {
        private GestorRevisiones gestor = new GestorRevisiones();
        private Contrato contratoSeleccionado;

        private IEnumerable<RevisionContrato> listaRevisiones = new List<RevisionContrato>();

        public frmRevisionContrato(Contrato contrato)
        {
            InitializeComponent();
            
            contratoSeleccionado = contrato;

            panelRevisionContrato_general.AltaClicked += PanelInferior_general_altaClicked;
            panelRevisionContrato_general.BajaClicked += PanelInferior_general_bajaClicked;
            panelRevisionContrato_general.EditarClicked += PanelInferior_general_editarClicked;
            panelRevisionContrato_general.EliminarClicked += PanelInferior_general_eliminarClicked;
            panelRevisionContrato_general.SeleccionActivos += PanelRevisionContrato_general_SeleccionActivos;
            panelRevisionContrato_Edicion.ValidarClicked += PanelRevisionContrato_Edicion_ValidarClicked;
            panelRevisionContrato_Edicion.CancelarClicked += PanelRevisionContrato_Edicion_CancelarClicked;
        }

        private void frmRevisionContrato_Load(object sender, EventArgs e)
        {
            panelRevisionContrato_general.EstadoVisible = true;
            ConfigurarGrid();

            // Monta las columnas por orden
            InicializaColumnas();

            // Carga las revisiones en el grid
            CargarRevisiones();
        }

        private void ConfigurarGrid()
        {
            dgvRevisiones.DefaultCellStyle.SelectionBackColor = Color.Wheat;
            dgvRevisiones.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void CargarRevisiones()
        {
            // Carga una lista con las revisiones del contrato
            listaRevisiones = gestor.ListarPorContrato(contratoSeleccionado.Id);

            // Carga los datos de los contratos en el gridBase
            dgvRevisiones.DataSource = null;
            dgvRevisiones.DataSource = listaRevisiones;

            AplicarFormatoColumnas();
        }

        private void PanelInferior_general_altaClicked(object sender, EventArgs e)
        {
            HabilitarCampos(mostrar: true);
            MostrarPanelGeneral(false);
        }

        private void PanelInferior_general_bajaClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Las revisiones de un contrato no se pueden dar de baja.", "Error baja revision contrato", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void PanelInferior_general_editarClicked(object sender, EventArgs e)
        {
            HabilitarCampos(mostrar: false);
            MostrarPanelGeneral(false);
        }

        private void PanelInferior_general_eliminarClicked(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Esta seguro de eliminar la revision",
                "Confirmar eliminacion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

            if(resultado == DialogResult.Yes)
            {
                gestor.Eliminar(ActualizarPropiedades());
            }
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

        private void HabilitarCampos(bool mostrar)
        {
            txtFechaRevision.Enabled = mostrar;
            txtPrecioAnterior.Enabled = mostrar;
            txtRevision.Enabled = mostrar;
            txtPrecioRevisado.Enabled = mostrar;
            txtObservaciones.Enabled = mostrar;
            dgvRevisiones.Enabled = !mostrar;
        }

        private void btnContratos_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private RevisionContrato ActualizarPropiedades()
        {
            RevisionContrato revisionContrato = new RevisionContrato
            {
                FechaRevision = Utiles.ConvertirFecha(txtFechaRevision.Text) ?? DateTime.Today,
                PrecioAnterior = Convert.ToDecimal(txtPrecioAnterior.Text),
                PorcentajeRevision = Convert.ToDecimal(txtRevision?.Text),
                PrecioRevisado = Convert.ToDecimal(txtPrecioRevisado.Text),
                Observaciones = txtObservaciones.Text
            };

            return revisionContrato;
        }

        private void InicializaColumnas()
        {
            var columnas = new (string nombrePropiedad, int orden)[]
            {
                ("Id", 0),
                ("FechaRevision", 1),
                ("PrecioAnterior", 2),
                ("PorcentajeRevision", 3),
                ("PrecioRevisado",4),
                ("IdContrato", 5),
                ("Observaciones", 6),
            };

            // Pasa las columnas al grid base para que las configure
            ConfigurarColumnas<RevisionContrato>(columnas);

        }

        private void ConfigurarColumnas<T>(IEnumerable<(string nombrePropiedad, int orden)> columnas)
        {
            dgvRevisiones.AutoGenerateColumns = false;
            dgvRevisiones.Columns.Clear();

            foreach(var (nombrePropiedad, orden) in columnas)
            {
                UtilesUI.InsertaColumnaDGV<T>(dgvRevisiones, nombrePropiedad, orden);
            }
        }

        private void AplicarFormatoColumnas()
        {
            if(dgvRevisiones.Columns.Count == 0) return; // Protege contra columnas vacías

            // Lista con los nombres de las propiedades a ajustar
            string[] columnasCentradas = { "Id", "FechaRevision", "IdContrato", "PorcentajeRevision"};
            string[] columnasFecha = { "FechaRevision" };
            string[] columnasImportes = { "PrecioAnterior", "PrecioRevisado" };

            // Aplica formatos
            foreach(DataGridViewColumn columna in dgvRevisiones.Columns)
            {
                // Ajuste al centro
                if(columnasCentradas.Contains(columna.DataPropertyName))
                {
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Ajuste formato fecha
                if(columnasFecha.Contains(columna.DataPropertyName))
                {
                    columna.DefaultCellStyle.Format = "dd.MM.yyyy";
                }

                // Aplica formato de importe y alineado a la derecha
                if(columnasImportes.Contains(columna.DataPropertyName))
                {
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    columna.DefaultCellStyle.Format = "N2";
                }
            }

            // Ajuste al contenido
            dgvRevisiones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }
}
