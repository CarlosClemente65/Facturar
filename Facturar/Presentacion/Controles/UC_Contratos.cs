using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
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
        private Empresa EmpresaContrato;
        private Cliente ClienteContrato;
        private Local LocalContrato;

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
            var gestorEmpresas = new GestorEmpresas();
            var gestorClientes = new GestorClientes();
            var gestorLocales = new GestorLocales();

            listaContratos = gestorContratos.ListarTodos(activos: activos);

            // Carga las entidades relacionadas para mostrar los datos en el grid
            foreach(var contrato in listaContratos)
            {
                contrato.CargarRelaciones(gestorEmpresas: gestorEmpresas, gestorClientes: gestorClientes, gestorLocales: gestorLocales);
            }

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
                // En el caso del alta, hay que seleccionar un cliente y local para asignarlos al contrato (la empresa esta vinculada al local)
                contrato.IdCliente = ClienteContrato.Id;
                contrato.IdLocal = LocalContrato.Id;
                contrato.IdEmpresa = EmpresaContrato.Id;
            }
            contrato.PrecioMensual = Convert.ToDecimal(txtPrecioMensual.Text);
            contrato.FechaInicio = Utilidades.UtilesGenerales.ConvertirFecha(txtFechaInicio.Text) ?? DateTime.Today;
            contrato.FechaFin = Utilidades.UtilesGenerales.ConvertirFecha(txtFechaFin.Text);
            contrato.Observaciones = txtObservaciones.Text;
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
            ConfigurarColumnas<Contrato>(columnas);

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
            txtNombreCliente.Enabled = false;
            txtDescripcion.Enabled = false;
            txtNifEmpresa.Enabled = false;
            txtNombreEmpresa.Enabled = false;
            txtFechaFin.Enabled = false;
        }

        public void BloqueoTextBoxEdicion()
        {
            // Deshabilita los TextBox que no se pueden editar
            txtNifCliente.Enabled = false;
            txtNombreCliente.Enabled = false;
            txtIdLocal.Enabled = false;
            txtDescripcion.Enabled = false;
            txtNifEmpresa.Enabled = false;
            txtNombreEmpresa.Enabled = false;
            txtFechaInicio.Enabled = false;
        }

        // Evento que se lanza al seleccionar una fila en el grid base
        private void GridBase_FilaSeleccionada(object sender, object entidad)
        {
            // Como recibe un objeto genérico, se chequea que sea del tipo contrato
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
            txtNifCliente.Text = contrato.NIFCliente;
            txtNombreCliente.Text = contrato.NombreCliente;
            txtIdLocal.Text = contrato.IdLocal.ToString();

            // TODO: Cambiar la descripcion del local por un comboBox de locales
            txtDescripcion.Text = contrato.DescripcionLocal;

            txtPrecioMensual.Text = contrato.PrecioMensual.ToString("N2");
            txtNifEmpresa.Text = contrato.NIFEmpresa;
            txtNombreEmpresa.Text = contrato.NombreEmpresa;
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

        private void txtNifCliente_Leave(object sender, EventArgs e)
        {
            txtNifCliente.Text = txtNifCliente.Text.ToUpper();
            ClienteContrato = ObtenerClientePorNif(txtNifCliente.Text);
            txtNombreCliente.Text = ClienteContrato.Nombre;
        }

        private void txtIdLocal_Leave(object sender, EventArgs e)
        {
            ObtenerLocal(Convert.ToInt32(txtIdLocal.Text));
            txtDescripcion.Text = LocalContrato.Descripcion;
            txtNifEmpresa.Text = EmpresaContrato.NIF;
            txtNombreEmpresa.Text = EmpresaContrato.Nombre;
        }

        private void ObtenerLocal(int idLocal)
        {
            var gestorLocales = new GestorLocales();
            LocalContrato = gestorLocales.ObtenerPorId(idLocal);
            var gestorEmpresas = new GestorEmpresas();
            EmpresaContrato = gestorEmpresas.ObtenerPorId(LocalContrato.IdEmpresa);
        }

        private Cliente ObtenerClientePorNif(string nif)
        {
            var gestorClientes = new GestorClientes();
            return gestorClientes.ObtenerPorNIF(nif);
        }
    }
}
