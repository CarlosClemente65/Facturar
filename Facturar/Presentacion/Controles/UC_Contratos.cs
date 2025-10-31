using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Servicios;
using static System.Net.Mime.MediaTypeNames;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Contratos : UC_GridBase
    {
        // Propiedad privada para almacenar el cliente seleccionado en el grid
        private Contrato ContratoSeleccionado;

        // Almacena la lista de contratos para poder ordenar
        private IEnumerable<Contrato> listaContratos;

        private bool ordenAscendente = true;

        private bool datosCargados = false;
        public UC_Contratos()
        {
            InitializeComponent();
        }

        public Contrato ContratoActual
        {
            get => ContratoSeleccionado;
            set => ContratoSeleccionado = value;
        }

        private void UC_Contratos_Load(object sender, EventArgs e)
        {
            // Suscripcion a los eventos del grid base
            FilaSeleccionada += GridBase_FilaSeleccionada;
            ColumnaOrdenada += GridBase_Columnaseleccionada;

            // Carga el grid base en el panel correspondiente
            GridBase.Location = new Point(0, 0);
            GridBase.Size = panelDgv.Size;

            // Establece el dock y el anclaje para que se ajuste al panel
            GridBase.Dock = DockStyle.None;
            GridBase.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Añade el grid al panel
            panelDgv.Controls.Add(GridBase);

            if(!datosCargados)
            {
                // Monta las columnas por orden
                InicializaColumnas();

                // Carga las empresas en el control
                CargarContratos(activos: true);
                datosCargados = true;
            }
        }

        public void CargarContratos(bool? activos = true)
        {
            var gestorContratos = new Servicios.GestorContratos();
            listaContratos = gestorContratos.ListarTodos(activos: activos);

            // Carga los datos de los contratos en el gridBase
            GridBase.DataSource = null;
            GridBase.DataSource = listaContratos.ToList();

            AplicarFormatoColumnas();

        }

        internal void ActualizarContratoSeleccionado()
        {
            if(dgvBase.CurrentRow != null)
            {
                // Carga el objeto contrato segun la fila seleccionada
                ContratoSeleccionado = dgvBase.CurrentRow.DataBoundItem as Contrato;
            }
        }

        // Actualiza las propiedades del contrato segun el contenido de los textBox
        public void ActualizaPropiedadesContrato(Contrato contrato, Enumerador.TipoProceso tipoProceso)
        {
            if(contrato == null)
            {
                throw new ArgumentNullException("No se han pasado datos del contrato para actualizar");
            }

            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                // En el caso del alta, se localiza el IdEmpresa y el IdCliente a grabar en el local segun el NifEmpresa

                //var empresaAlta = gestorEmpresas.ObtenerPorNIF(txtNifEmpresa.Text);
                contrato.IdEmpresa = ObtenerEmpresaPorNif(txtNifEmpresa.Text).Id;
            }


            // TODO: Revision para actualizar propiedades del contrato
            /* Pendiente de revisar propiedades
             
            // Campos comunes en el alta y edicion
            local.Descripcion = txtDescripcion.Text;
            local.Direccion = txtDireccion.Text;
            local.CodigoPostal = txtCodigoPostal.Text;
            local.Poblacion = txtPoblacion.Text;

            // Solo asigna el importe si es un valor decimal valido
            if(decimal.TryParse(txtImporte.Text, out decimal importe))
            {
                local.ImporteAlquiler = importe;
            }
            local.Observaciones = txtObservaciones.Text;
            local.FechaAlta = DateTime.Parse(txtFechaAlta.Text);

            */
        }


        // Define las columnas a mostrar en el grid base y el orden que tendran
        private void InicializaColumnas()
        {
            var columnas = new (string nombrePropiedad, int orden)[]
            {
                ("Id", 0),
                ("DescripcionLocal", 1),
                ("PrecioMensual", 2),
                ("NIFCliente", 3),
                ("NombreCliente", 4),
                ("NIFEmpresa",5),
                ("NombreEmpresa",6),
                ("Observaciones", 7),
                ("FechaInicio", 8),
                ("FechaFin", 9)
            };

            // Pasa las columnas al grid base para que las configure
            ConfigurarColumnas<Empresa>(columnas);

        }

        private void AplicarFormatoColumnas()
        {
            if(dgvBase.Columns.Count == 0) return; // Protege contra columnas vacías

            // Lista con los nombres de las propiedades a ajustar
            string[] columnasCentradas = { "Id", "NIFCliente", "NIFEmpresa", "PrecioMensual", "FechaInicio", "FechaFin" };
            string[] columnasFecha = { "FechaInicio", "FechaFin" };
            string[] columnasImportes = { "PrecioMensual" };

            // Aplica formatos
            foreach(DataGridViewColumn columna in GridBase.Columns)
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
            GridBase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        public void BloqueoTextBoxAlta()
        {
            // Deshabilita los TextBox que no se pueden editar
            txtFechaFin.Enabled = false;
            txtNombreEmpresa.Enabled = false;
            txtNombreCliente.Enabled = false;
        }

        public void BloqueoTextBoxEdicion()
        {
            // Deshabilita los TextBox que no se pueden editar
            txtFechaInicio.Enabled = false;
            txtNombreEmpresa.Enabled = false;
            txtNombreCliente.Enabled = false;
        }

        // Evento que se lanza al seleccionar una fila en el grid base
        private void GridBase_FilaSeleccionada(object sender, object entidad)
        {
            // Como recibe un objeto genérico, se chequea que sea del tipo Local
            if(entidad is Contrato contrato)
            {
                // Actualiza el contrato seleccionada
                ContratoSeleccionado = contrato;

                // Limpia los textBox y muestra los datos de la empresa seleccionada
                Utiles.LimpiarTextBoxes(this);

                // Muestra los datos del contrato seleccionado
                MostrarDatoscontrato(contrato);
            }
        }


        // Muestra los datos del contrato en los textBox correspondientes
        private void MostrarDatoscontrato(Contrato contrato)
        {
            txtNifEmpresa.Text = contrato.NIFEmpresa;
            txtNombreEmpresa.Text = contrato.NombreEmpresa;
            txtNifCliente.Text = contrato.NIFCliente;
            txtNombreCliente.Text = contrato.NombreCliente;

            // TODO: Cambiar la descripcion del local por un comboBox de locales
            txtDescripcion.Text = contrato.DescripcionLocal;
            txtFechaInicio.Text = contrato.FechaInicio.ToString("dd.MM.yyyy");

            // La fecha de fin puede ser nula
            if(contrato.FechaFin.HasValue)
            {
                txtFechaFin.Text = contrato.FechaFin.Value.ToString("dd.MM.yyyy");
            }
            else
            {
                txtFechaFin.Text = "";
            }

            txtPrecioMensual.Text = contrato.PrecioMensual.ToString("N2");
            txtObservaciones.Text = contrato.Observaciones;
        }

        // Evento que se lanza al ordenar una columna en el grid base
        private void GridBase_Columnaseleccionada(object sender, int columnaIndex)
        {
            string nombreColumna = GridBase.Columns[columnaIndex].DataPropertyName;

            if(ordenAscendente)
            {
                GridBase.DataSource = listaContratos.OrderBy(emp => Utiles.GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                GridBase.DataSource = listaContratos.OrderByDescending(emp => Utiles.GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }

        private void txtFechaInicio_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de inicio
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
            DateTime fechaValida;

            bool esValida = DateTime.TryParseExact(
                txtFechaInicio.Text,                                  // Fecha a validar
                formatosValidos,                                    // Formatos validos
                System.Globalization.CultureInfo.InvariantCulture,  // Cultura
                System.Globalization.DateTimeStyles.None,           // Sin estilos adicionales
                out fechaValida                                     // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaInicio.Focus();
            }
        }

        private void txtFechaFin_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de baja
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
            DateTime fechaValida;

            if(txtFechaFin.Text.Trim() == "")
            {
                // Si el campo está vacío, no se realiza la validación
                return;
            }

            bool esValida = DateTime.TryParseExact(
                txtFechaFin.Text,                                  // Fecha a validar
                formatosValidos,                                    // Formatos validos
                System.Globalization.CultureInfo.InvariantCulture,  // Cultura
                System.Globalization.DateTimeStyles.None,           // Sin estilos adicionales
                out fechaValida                                     // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaFin.Focus();
            }
        }

        private void TextBox_ToUpper(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if(txt != null)
            {
                txt.Text = txt.Text.ToUpper();
            }
        }

        private void txtImporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utiles.ValidarImporte(sender as TextBox, e);
        }

        private void txtImporte_Leave(object sender, EventArgs e)
        {
            Utiles.FormatearImporte(sender as TextBox);
        }

        private void txtNifEmpresa_Leave(object sender, EventArgs e)
        {
            txtNifEmpresa.Text = txtNifEmpresa.Text.ToUpper();
            txtNombreEmpresa.Text = ObtenerEmpresaPorNif(txtNifEmpresa.Text)?.Nombre ?? "";
        }

        private Empresa ObtenerEmpresaPorNif(string nif)
        {
            var gestorEmpresas = new GestorEmpresas();
            return gestorEmpresas.ObtenerPorNIF(nif);
        }

        private void txtNifCliente_Leave(object sender, EventArgs e)
        {
            txtNifCliente.Text = txtNifCliente.Text.ToUpper();
            txtNombreCliente.Text = ObtenerClientePorNif(txtNifEmpresa.Text)?.Nombre ?? "";
        }

        private Cliente ObtenerClientePorNif(string nif)
        {
            var gestorClientes = new GestorClientes();
            return gestorClientes.ObtenerPorNIF(nif);
        }
    }
}
