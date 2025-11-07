using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Servicios;
using Facturar.Utilidades;
using Utiles = Facturar.Utilidades.UtilesGenerales;
using UtilesUI = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Formularios
{
    public partial class frmRevisionContrato : Form
    {
        private GestorRevisiones gestor = new GestorRevisiones();
        private Contrato contratoSeleccionado;
        private RevisionContrato revisionSeleccionada;
        private RevisionContrato ultimaRevision;
        private Enumeradores.TipoProceso tipoProceso = Enumeradores.TipoProceso.Ninguno;

        private IEnumerable<RevisionContrato> listaRevisiones = new List<RevisionContrato>();

        public frmRevisionContrato(Contrato contrato)
        {
            InitializeComponent();

            contratoSeleccionado = contrato;

            // Suscripcion a los eventos de los botones del panel inferior general
            panelRevisionContrato_general.AltaClicked += PanelInferior_general_altaClicked;
            panelRevisionContrato_general.BajaClicked += PanelInferior_general_bajaClicked;
            panelRevisionContrato_general.EditarClicked += PanelInferior_general_editarClicked;
            panelRevisionContrato_general.EliminarClicked += PanelInferior_general_eliminarClicked;
            panelRevisionContrato_Edicion.ValidarClicked += PanelRevisionContrato_Edicion_ValidarClicked;
            panelRevisionContrato_Edicion.CancelarClicked += PanelRevisionContrato_Edicion_CancelarClicked;
        }

        private void frmRevisionContrato_Load(object sender, EventArgs e)
        {
            panelRevisionContrato_general.EstadoVisible = false; // Se quita el panel de los estados porque aqui no es necesario

            // Configuracion del grid de revisiones
            ConfigurarGrid();

            // Monta las columnas por orden
            InicializaColumnas();

            // Carga las revisiones en el grid
            CargarRevisiones();
        }


        // Aplica estilo de colores al grid
        private void ConfigurarGrid()
        {
            dgvRevisiones.DefaultCellStyle.SelectionBackColor = Color.Wheat;
            dgvRevisiones.DefaultCellStyle.SelectionForeColor = Color.Black;
        }


        // Carga una lista con las revisiones del contrato ordenada por fecha de revision
        private void CargarRevisiones()
        {
            listaRevisiones = gestor.ListarPorContrato(contratoSeleccionado.Id).OrderByDescending(r => r.FechaRevision).ToList();

            // Carga los datos de los contratos en el gridBase
            dgvRevisiones.DataSource = null;
            dgvRevisiones.DataSource = listaRevisiones;

            AplicarFormatoColumnas();
        }


        // Boton alta
        private void PanelInferior_general_altaClicked(object sender, EventArgs e)
        {
            tipoProceso = Enumeradores.TipoProceso.Alta;
            ModoEdicion(modoEdicion: true);
        }


        // Boton baja (no tiene funcionalidad)
        private void PanelInferior_general_bajaClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Las revisiones de un contrato no se pueden dar de baja.", "Error baja revision contrato", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        // Boton editar
        private void PanelInferior_general_editarClicked(object sender, EventArgs e)
        {
            tipoProceso = Enumeradores.TipoProceso.Edicion;

            // Se valida que no se modifique una revision si hay alguna posterior
            var ultimaRevision = gestor.FechaUltimaRevision(contratoSeleccionado.Id);

            if(revisionSeleccionada.FechaRevision < ultimaRevision)
            {
                MessageBox.Show("No se puede modificar esta revision. Existe una posterior", "Error edicion revision", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ModoEdicion(modoEdicion: true);
        }


        // Boton eliminar
        private void PanelInferior_general_eliminarClicked(object sender, EventArgs e)
        {
            tipoProceso = Enumeradores.TipoProceso.Eliminacion;

            DialogResult resultado = MessageBox.Show(
                "Esta seguro de eliminar la revision",
                "Confirmar eliminacion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

            if(resultado == DialogResult.Yes)
            {
                gestor.Eliminar(revisionSeleccionada);
            }

            CargarRevisiones();
        }


        // Boton cancelar
        private void PanelRevisionContrato_Edicion_CancelarClicked(object sender, EventArgs e)
        {
            tipoProceso = Enumeradores.TipoProceso.Ninguno;
            ModoEdicion(modoEdicion: false);
            CargarRevisiones();
        }

        private void PanelRevisionContrato_Edicion_ValidarClicked(object sender, EventArgs e)
        {
            RevisionContrato copiaRevision; // Copia de la revision selecciona
            RevisionContrato nuevaRevision; // Nueva revision a dar de alta
            string mensajeOk = string.Empty;
            string mensajeKo = string.Empty;
            try
            {
                switch(tipoProceso)
                {
                    case Enumeradores.TipoProceso.Alta:
                        // Se crea una revision con los valores de los campos
                        nuevaRevision = CrearNuevaRevision(contratoSeleccionado);

                        // Validacion de la revision
                        //nuevaRevision.ValidarPropiedadesRevision();

                        gestor.AgregarRevision(nuevaRevision, contratoSeleccionado);

                        mensajeOk = "Revision dada de alta en la base de datos";
                        break;

                    case Enumeradores.TipoProceso.Edicion:
                        // Se hace una copia por si hay errores poder restaurarla
                        if(revisionSeleccionada != null)
                        {
                            copiaRevision = new RevisionContrato(revisionSeleccionada);
                        }

                        // Se crea una nueva revision
                        nuevaRevision = CrearNuevaRevision(contratoSeleccionado);

                        // Se le asigna el Id porque en la creacion no se sabe
                        nuevaRevision.Id = revisionSeleccionada.Id;

                        // Se graba en la base de datos
                        gestor.Actualizar(nuevaRevision);

                        mensajeOk = "Revision actualizada en la base de datos";
                        break;
                }
            }

            catch(Exception ex)
            {
                mensajeKo = $"No se ha podido actualizar la revision en la base de datos\n{ex.Message}";
            }

            if(mensajeOk != string.Empty)
            {
                MessageBox.Show(mensajeOk, "Actualizacion base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(mensajeKo != string.Empty)
            {
                MessageBox.Show(mensajeKo, "Actualizacion base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaRevision.Focus();
                return;
            }

            tipoProceso = Enumeradores.TipoProceso.Ninguno;

            // Cancela el modo edicion
            ModoEdicion(modoEdicion: false);

            // Carga los datos en el grid
            CargarRevisiones();
        }


        // Activa o desactiva campos y grid en el modo edicion
        private void ModoEdicion(bool modoEdicion)
        {
            // Alterna paneles edicion y general
            panelRevisionContrato_Edicion.Visible = modoEdicion;
            panelRevisionContrato_general.Visible = !modoEdicion;

            // Alterna campos de datos
            txtFechaRevision.Enabled = modoEdicion;
            txtPrecioAnterior.Enabled = modoEdicion;
            txtRevision.Enabled = modoEdicion;
            txtPrecioRevisado.Enabled = modoEdicion;
            txtObservaciones.Enabled = modoEdicion;

            // Alterna botones
            btnVolver.Visible = !modoEdicion;

            // Alterna el grid
            dgvRevisiones.Enabled = !modoEdicion;
        }


        // Cierra el formulario de revisiones
        private void btnContratos_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Crea un objeto con una nueva revision cargando los campos del formulario
        private RevisionContrato CrearNuevaRevision(Contrato contratoSeleccionado)
        {
            // Limpieza y conversion de campos numericos
            decimal.TryParse(txtPrecioAnterior.Text.Replace('.', ','), out decimal precioAnterior);
            decimal.TryParse(txtRevision.Text.Replace('.', ','), out decimal revision);
            decimal.TryParse(txtPrecioRevisado.Text.Replace('.', ','), out decimal precioRevisado);


            RevisionContrato nuevaRevision = new RevisionContrato()
            {
                IdContrato = contratoSeleccionado.Id,
                FechaRevision = Utiles.ConvertirFecha(txtFechaRevision.Text) ?? DateTime.Today,
                PrecioAnterior = precioAnterior,
                PorcentajeRevision = revision,
                PrecioRevisado = precioRevisado,
                Observaciones = txtObservaciones.Text
            };

            return nuevaRevision;
        }


        // Organiza las columnas del grid
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


        // Inserta la columnas configuradas en el grid
        private void ConfigurarColumnas<T>(IEnumerable<(string nombrePropiedad, int orden)> columnas)
        {
            dgvRevisiones.AutoGenerateColumns = false;
            dgvRevisiones.Columns.Clear();

            foreach(var (nombrePropiedad, orden) in columnas)
            {
                UtilesUI.InsertaColumnaDGV<T>(dgvRevisiones, nombrePropiedad, orden);
            }
        }


        // Formatea las columnas del grid segun el dato que contenga
        private void AplicarFormatoColumnas()
        {
            if(dgvRevisiones.Columns.Count == 0) return; // Protege contra columnas vacías

            // Lista con los nombres de las propiedades a ajustar
            string[] columnasCentradas = { "Id", "FechaRevision", "IdContrato", "PorcentajeRevision" };
            string[] columnasFecha = { "FechaRevision" };
            string[] columnasImportes = { "PrecioAnterior", "PrecioRevisado", "PorcentajeRevision" };

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


        // Actualiza la revision seleccionada al cambiar la seleccion del grid
        private void dgvRevisiones_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvRevisiones.CurrentRow != null)
            {
                revisionSeleccionada = dgvRevisiones.CurrentRow.DataBoundItem as RevisionContrato;
            }

            // Limpia los textBox y muestra los datos de la revision seleccionada
            UtilesUI.LimpiarTextBoxes(this);
            MostrarDatosRevision(revisionSeleccionada);

        }

        // Rellena los campos con los valores de la revision seleccionada
        private void MostrarDatosRevision(RevisionContrato revisionSeleccionada)
        {
            // Carga los valores de la revision en los campos
            txtFechaRevision.Text = Utiles.FormatearFecha(revisionSeleccionada.FechaRevision);
            txtPrecioAnterior.Text = (revisionSeleccionada.PrecioAnterior ?? 0).ToString("N2"); // Como pueder ser nula se pone a cero

            txtRevision.Text = $"{revisionSeleccionada.PorcentajeRevision:N2}%"; // Formatea a dos decimales y añade el simbolo de porcentaje
            txtPrecioRevisado.Text = $"{revisionSeleccionada.PrecioRevisado:N2}"; // Formatea a dos decimales
            txtObservaciones.Text = revisionSeleccionada.Observaciones;
        }

        // Asigna la fecha de revision a la fecha actual
        private void txtFechaRevision_Enter(object sender, EventArgs e)
        {
            if(txtFechaRevision.Text == "")
            {
                txtFechaRevision.Text = Utiles.FormatearFecha(DateTime.Today);
            }
        }

        // Validacion de la fecha de revision
        private void txtFechaRevision_Leave(object sender, EventArgs e)
        {
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy", "dd.MM.yy" };
            bool esValida = DateTime.TryParseExact(
                txtFechaRevision.Text,                                  // Fecha a validar
                formatosValidos,                                    // Formatos validos
                System.Globalization.CultureInfo.InvariantCulture,  // Cultura
                System.Globalization.DateTimeStyles.None,           // Sin estilos adicionales
                out DateTime fechaRevision                          // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy, dd-MM-yyyy o dd.MM.yy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaRevision.Clear();
                txtFechaRevision.Focus();
            }

            txtFechaRevision.Text = Utiles.FormatearFecha(fechaRevision);

            // Se valida que la fecha de revision no sea anterior a la ultima
            var ultimaRevision = gestor.FechaUltimaRevision(contratoSeleccionado.Id);
            if(fechaRevision < ultimaRevision && tipoProceso == Enumeradores.TipoProceso.Alta)
            {
                MessageBox.Show("La fecha de revision no puede ser anterior a la ultima", "Error en fecha revision", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFechaRevision.Clear();
                txtFechaRevision.Focus();
                return;
            }
        }

        // Pone a mayuscaulas las observaciones
        private void txtObservaciones_Leave(object sender, EventArgs e)
        {
            txtObservaciones.Text = txtObservaciones.Text.ToUpper();
        }


        // Validacion de que no se introduzcan valores erroneos en campos de importe
        private void txtImporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            UtilesUI.ValidarImporte(sender as TextBox, e);
        }


        // Validacion de campos de importe
        private void txtImporte_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Valida que no se introduzca algo que no sean numeros
            if(!decimal.TryParse(txt.Text, out decimal importe))
            {
                MessageBox.Show("Debe introducir un importe numerico valido.", "Importe incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return;
            }

            // Valida que sea un importe positivo
            if(importe <= 0)
            {
                MessageBox.Show("El importe debe ser mayor que cero", "Importe erroneo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return;
            }

            // Si no hay errores, formatea el importe
            UtilesUI.FormatearImporte(sender as TextBox);
        }


        // Calculo del precio revisado si se introduce un porcentaje
        private void txtRevision_Leave(object sender, EventArgs e)
        {

            if(decimal.TryParse(txtPrecioAnterior.Text, out decimal precioAnterior) && decimal.TryParse(txtRevision.Text, out decimal revision))
            {
                decimal incremento = 1 + (revision / 100);
                decimal calculoRevisado = Math.Round(precioAnterior * incremento, 2);

                txtPrecioRevisado.Text = calculoRevisado.ToString("N2");
            }
        }

        private void txtPrecioAnterior_Enter(object sender, EventArgs e)
        {
            ultimaRevision = gestor.ObtenerUltimaRevision(contratoSeleccionado.Id);
            if(tipoProceso == Enumeradores.TipoProceso.Alta && ultimaRevision != null)
            {
                txtPrecioAnterior.Text = ultimaRevision.PrecioRevisado.ToString("N2");
                txtRevision.Focus();
            }
        }
    }
}
