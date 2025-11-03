using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Servicios;
using static Facturar.Utilidades.Enumeradores;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Clientes : UC_GridBase
    {
        // Propiedad privada para almacenar el cliente seleccionado en el grid
        private Cliente ClienteSeleccionado;

        // Instancias de los gestores necesarios
        GestorClientes gestorClientes = new GestorClientes();

        // Define el tipo de proceso (alta o edicion)
        public Enumerador.TipoProceso tipoProceso;

        // Almacena la lista de locales para poder ordenar
        private IEnumerable<Cliente> listaClientes;

        private bool ordenAscendente = true;

        private bool datosCargados = false;

        public UC_Clientes()
        {
            InitializeComponent();
        }

        public Cliente ClienteActual
        {
            get => ClienteSeleccionado;
            set => ClienteSeleccionado = value;
        }

        private void UC_Clientes_Load(object sender, EventArgs e)
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
                CargarClientes(activos: true);
                datosCargados = true;

                // Asigna al comboBox las formas de pago
                cbFormaPago.DataSource = Enum.GetValues(typeof(Cliente.FormasPago));

            }
        }

        public void CargarClientes(bool? activos = true)
        {
            listaClientes = gestorClientes.ListarTodos(activos: activos);

            // Carga los datos de las empresas en el gridBase
            GridBase.DataSource = null;
            GridBase.DataSource = listaClientes.ToList();

            AplicarFormatoColumnas();
        }

        public void ActualizarClienteSeleccionado()
        {
            if(dgvBase.CurrentRow != null)
            {
                // Carga el objeto cliente segun la fila seleccionada
                ClienteSeleccionado = dgvBase.CurrentRow.DataBoundItem as Cliente;
            }
        }

        // Evento que se lanza al seleccionar una fila en el grid base
        private void GridBase_FilaSeleccionada(object sender, object entidad)
        {
            // Como recibe un objeto genérico, se chequea que sea del tipo Cliente
            if(entidad is Cliente cliente)
            {
                // Actualiza el cliente seleccionado
                ClienteSeleccionado = cliente;

                // Limpia los textBox y muestra los datos del cliente seleccionado
                Utiles.LimpiarTextBoxes(this);

                // Muestra los datos del cliente seleccionado
                MostrarDatosCliente(cliente);
            }
        }

        // Evento que se lanza al ordenar una columna en el grid base
        private void GridBase_Columnaseleccionada(object sender, int columnaIndex)
        {
            string nombreColumna = GridBase.Columns[columnaIndex].DataPropertyName;

            if(ordenAscendente)
            {
                GridBase.DataSource = listaClientes.OrderBy(emp => Utiles.GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                GridBase.DataSource = listaClientes.OrderByDescending(emp => Utiles.GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }

        // Muestra los datos de la empresa en los textBox correspondientes
        private void MostrarDatosCliente(Cliente cliente)
        {
            txtNifCliente.Text = cliente.NIF;
            txtNombreCliente.Text = cliente.Nombre;
            txtDireccion.Text = cliente.Direccion;
            txtCodigoPostal.Text = cliente.CodigoPostal;
            txtPoblacion.Text = cliente.Poblacion;
            txtProvincia.Text = cliente.Provincia;
            txtTelefono.Text = cliente.Telefono;
            txtEmail.Text = cliente.Email;
            txtPersonaContacto.Text = cliente.PersonaContacto;
            txtFechaAlta.Text = cliente.FechaAlta.ToString("dd.MM.yyyy");

            // La fecha de baja puede ser nula
            if(cliente.FechaBaja.HasValue)
            {
                txtFechaBaja.Text = cliente.FechaBaja.Value.ToString("dd.MM.yyyy");
            }
            else
            {
                txtFechaBaja.Text = "";
            }

            cbFormaPago.SelectedItem = ClienteSeleccionado.FormaPago;
            txtIban.Text = cliente.IBAN;
            txtObservaciones.Text = cliente.Observaciones;

        }

        public void BloqueoTextBoxAlta()
        {
            // Deshabilita los textBox en el alta
            txtFechaBaja.Enabled = false;
        }

        public void BloqueoTextBoxEdicion()
        {
            // Deshabilita los TextBox que no se pueden editar
            txtNifCliente.Enabled = false;
            txtNombreCliente.Enabled = false;
            txtFechaAlta.Enabled = false;
            txtFechaBaja.Enabled = false;
        }

        // Define las columnas a mostrar en el grid base y el orden que tendran
        private void InicializaColumnas()
        {
            var columnas = new (string nombrePropiedad, int orden)[]
            {
                ("Id", 0),
                ("NIF", 1),
                ("Nombre", 2),
                ("Direccion", 3),
                ("CodigoPostal", 4),
                ("Poblacion", 5),
                ("Provincia", 6),
                ("Telefono", 7),
                ("Email", 8),
                ("FormaPago", 9),
                ("IBAN", 10),
                ("PersonaContacto", 11),
                ("FechaAlta", 12),
                ("FechaBaja", 13),
                ("Observaciones", 14)
            };

            // Pasa las columnas al grid base para que las configure
            ConfigurarColumnas<Cliente>(columnas);

        }

        private void AplicarFormatoColumnas()
        {
            if(GridBase.Columns.Count == 0) return; // Protege contra columnas vacías

            // Lista con los nombres de las propiedades a ajustar
            string[] columnasCentradas = { "Id", "CodigoPostal", "FechaAlta", "FechaBaja", "Telefono" };
            string[] columnasFecha = { "FechaAlta", "FechaBaja" };

            // Aplica formato de fecha
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
            }

            // Ajuste al contenido
            GridBase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        // Actualiza las propiedades de la empresa segun el contenido de los textBox
        public void ActualizaPropiedadesCliente(Cliente cliente)
        {
            if(cliente == null)
            {
                throw new ArgumentNullException("No se han pasado datos del cliente para actualizar");
            }

            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                // En el caso del alta, se asignan las propiedades que no se pueden modificar en la edición
                cliente.NIF = txtNifCliente.Text;  // No se permite modificar el NIF
                cliente.Nombre = txtNombreCliente.Text; // No se permite modificar el nombre

            }

            // Campos comunes en el alta y edicion
            cliente.Direccion = txtDireccion.Text;
            cliente.CodigoPostal = txtCodigoPostal.Text;
            cliente.Poblacion = txtPoblacion.Text;
            cliente.Provincia = txtProvincia.Text;
            cliente.Telefono = txtTelefono.Text;
            cliente.Email = txtEmail.Text;
            cliente.PersonaContacto = txtPersonaContacto.Text;
            cliente.FormaPago = (Cliente.FormasPago)cbFormaPago.SelectedItem;
            cliente.IBAN = txtIban.Text;
            cliente.Observaciones = txtObservaciones.Text;
        }

        private void txtFechaAlta_Enter(object sender, EventArgs e)
        {
            txtFechaAlta.Text = Utilidades.UtilesGenerales.FormatearFecha(DateTime.Today).ToString();
        }

        private void txtFechaAlta_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de alta
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
            DateTime fechaValida;

            bool esValida = DateTime.TryParseExact(
                txtFechaAlta.Text,                                  // Fecha a validar
                formatosValidos,                                    // Formatos validos
                System.Globalization.CultureInfo.InvariantCulture,  // Cultura
                System.Globalization.DateTimeStyles.None,           // Sin estilos adicionales
                out fechaValida                                     // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaAlta.Focus();
            }
        }

        private void txtFechaBaja_Enter(object sender, EventArgs e)
        {
            txtFechaBaja.Text = Utilidades.UtilesGenerales.FormatearFecha(DateTime.Today).ToString();
        }

        private void txtFechaBaja_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de baja
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
            DateTime fechaValida;

            if(txtFechaBaja.Text.Trim() == "")
            {
                // Si el campo está vacío, no se realiza la validación
                return;
            }

            bool esValida = DateTime.TryParseExact(
                txtFechaBaja.Text,                                  // Fecha a validar
                formatosValidos,                                    // Formatos validos
                System.Globalization.CultureInfo.InvariantCulture,  // Cultura
                System.Globalization.DateTimeStyles.None,           // Sin estilos adicionales
                out fechaValida                                     // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaBaja.Focus();
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

        private void txtNifCliente_Leave(object sender, EventArgs e)
        {
            // Solo se permite acceder al NIF en el alta
            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                txtNifCliente.Text = txtNifCliente.Text.ToUpper();

                // Busca el cliente por su NIF en la base de datos
                ClienteSeleccionado = ObtenerClientePorNif(txtNifCliente.Text);
                if(ClienteSeleccionado != null)
                {
                    MessageBox.Show("El cliente indicado ya existe en la base de datos.",
                                    "Empresa duplicada",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                    txtNombreCliente.Text = string.Empty;
                    txtNifCliente.Focus();
                }
                else
                {
                    txtNombreCliente.Text = ClienteSeleccionado?.Nombre ?? string.Empty;
                }
            }
        }

        private Cliente ObtenerClientePorNif(string nif)
        {
            return gestorClientes.ObtenerPorNIF(nif);
        }

    }
}
