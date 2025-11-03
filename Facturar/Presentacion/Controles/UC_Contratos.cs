using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

        public Enumerador.TipoProceso tipoProceso;

        // Instancias de los gestores necesarios
        GestorContratos gestorContratos = new Servicios.GestorContratos();
        GestorEmpresas gestorEmpresas = new GestorEmpresas();
        GestorClientes gestorClientes = new GestorClientes();
        GestorLocales gestorLocales = new GestorLocales();

        // Almacena la lista de contratos con todas sus propiedades
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

        // Carga los contratos en el grid base y sus relaciones
        public void CargarContratos(bool? activos = true)
        {
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
            // Carga el objeto contrato segun la fila seleccionada
            if(dgvBase.CurrentRow?.DataBoundItem is Contrato contrato)
            {
                ContratoSeleccionado = contrato;
                LocalContrato = contrato.Local ?? gestorLocales.ObtenerPorId(contrato.IdLocal); // Carga el local del contrato y si no existe lo obtiene del gestor
                ClienteContrato = contrato.Cliente ?? gestorClientes.ObtenerPorId(contrato.IdCliente); // Carga el cliente del contrato y si no existe lo obtiene del gestor
                EmpresaContrato = contrato.Empresa ?? gestorEmpresas.ObtenerPorId(contrato.IdEmpresa); // Carga la empresa del contrato y si no existe lo obtiene del gestor
            }
        }

        // Actualiza las propiedades del contrato segun el contenido de los textBox
        public void ActualizaPropiedadesContrato(Contrato contrato)
        {
            if(contrato == null)
            {
                throw new ArgumentNullException("No se han pasado datos del contrato para actualizar");
            }

            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                // En el caso del alta, la empresa, cliente y local no se modifican
                contrato.IdCliente = ClienteContrato.Id;
                contrato.IdLocal = LocalContrato.Id;
                contrato.IdEmpresa = EmpresaContrato.Id;

            }

            // Establece las propiedades del contrato
            contrato.PrecioMensual = Convert.ToDecimal(txtPrecioMensual.Text);
            contrato.FechaInicio = Utilidades.UtilesGenerales.ConvertirFecha(txtFechaInicio.Text) ?? DateTime.Today;
            contrato.FechaFin = Utilidades.UtilesGenerales.ConvertirFecha(txtFechaFin.Text);
            contrato.Observaciones = txtObservaciones.Text;
            contrato.Local = LocalContrato;
            contrato.Local.ImporteAlquiler = contrato.PrecioMensual;
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
            txtFechaFin.Enabled = false; // No se permite poner la ficha fin en edicion
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
            // TODO: Cambiar el NifCliente por un comboBox de clientes
            txtNifCliente.Text = contrato.NIFCliente;
            txtNombreCliente.Text = contrato.NombreCliente;

            // TODO: Cambiar la descripcion del local por un comboBox de locales
            txtIdLocal.Text = contrato.IdLocal.ToString();
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

        private void txtFechaInicio_Enter(object sender, EventArgs e)
        {
            txtFechaInicio.Text = Utilidades.UtilesGenerales.FormatearFecha(DateTime.Today).ToString();
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

        private void txtFechaFin_Enter(object sender, EventArgs e)
        {
            txtFechaFin.Text = Utilidades.UtilesGenerales.FormatearFecha(DateTime.Today).ToString();
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
            TextBox txt = sender as TextBox;
            decimal importe;

            // Valida que no se introduzca algo que no sean numeros
            if(!decimal.TryParse(txt.Text, out importe))
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
            Utiles.FormatearImporte(sender as TextBox);
        }

        private void txtNifCliente_Leave(object sender, EventArgs e)
        {
            // Solo en el alta se permite acceder al cliente
            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                txtNifCliente.Text = txtNifCliente.Text.ToUpper();

                // En el alta se chequea que el cliente exista
                // Busca el cliente por su NIF en la base de datos
                ClienteContrato = ObtenerClientePorNif(txtNifCliente.Text);
                if(ClienteContrato == null)
                {
                    MessageBox.Show("El cliente indicado no existe",
                                    "Cliente no encontrado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    txtNombreCliente.Text = string.Empty;
                    txtNifCliente.Focus();
                }
                else
                {
                    txtNombreCliente.Text = ClienteContrato?.Nombre ?? string.Empty;
                }
            }

        }

        private void txtIdLocal_Leave(object sender, EventArgs e)
        {
            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                // Solo en el alta se permite acceder al local
                LocalContrato = ObtenerLocalPorId(Convert.ToInt32(txtIdLocal.Text));
                if(LocalContrato == null)
                {
                    MessageBox.Show("El local indicado no existe",
                                    "Local no encontrado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    txtIdLocal.Text = string.Empty;
                    txtIdLocal.Focus();
                }


                // Chequeo de que el local no tiene un contrato activo
                else if(LocalContrato.ContratoActivo)
                {
                    MessageBox.Show("El local ya tiene un contrato activo.",
                                     "Local con contrato activo",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Warning);
                    txtIdLocal.Text = string.Empty;
                    txtIdLocal.Focus();
                }
                else
                {
                    // Se obtiene la empresa vinculada al local
                    EmpresaContrato = ObtenerEmpresaPorIdLocal(LocalContrato.IdEmpresa);

                    //Carga los datos del local y la empresa en los textBox correspondientes
                    txtDescripcion.Text = LocalContrato?.Descripcion ?? string.Empty;
                    txtNifEmpresa.Text = EmpresaContrato?.NIF ?? string.Empty;
                    txtNombreEmpresa.Text = EmpresaContrato?.Nombre ?? string.Empty;
                }
            }
        }

        private Cliente ObtenerClientePorNif(string nif)
        {
            var gestorClientes = new GestorClientes();
            return gestorClientes.ObtenerPorNIF(nif);
        }

        private Local ObtenerLocalPorId(int id)
        {
            var gestorLocales = new GestorLocales();
            return gestorLocales.ObtenerPorId(id);
        }

        private Empresa ObtenerEmpresaPorIdLocal(int id)
        {
            var gestorEmpresas = new GestorEmpresas();
            return gestorEmpresas.ObtenerPorId(id);
        }
    }
}
