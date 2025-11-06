using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Presentacion.Formularios;
using Facturar.Servicios;
using Enumerador = Facturar.Utilidades.Enumeradores;
using UtilesUI = Facturar.Utilidades.UtilidadesUI;
using Utiles = Facturar.Utilidades.UtilesGenerales;

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
        private IEnumerable<Cliente> listaClientes;

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
            // Carga una lista con los contratos activos
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

            // Metodos para cargar las listas de los combobox
            CargarListaClientes(activos);
            CargarListaLocales(activos);
            CargarListaEmpresas(activos);
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

            // Resto de campos comunes
            contrato.PrecioMensual = Convert.ToDecimal(txtPrecioMensual.Text);
            contrato.FechaInicio = Utilidades.UtilesGenerales.ConvertirFecha(txtFechaInicio.Text) ?? DateTime.Today;
            contrato.FechaFin = Utilidades.UtilesGenerales.ConvertirFecha(txtFechaFin.Text);
            contrato.Observaciones = txtObservaciones.Text;
            contrato.Local = LocalContrato; // Asigna el objeto 'Local' al contrato
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
            // Gestion de los controles que se pueden editar en modo alta
            cbCliente.Enabled = true;
            cbLocal.Enabled = true;
            cbEmpresa.Enabled = true;
            cbLocal.Focus();
            cbCliente.Focus();
            txtFechaFin.Enabled = false;
            btnRevisionContrato.Enabled = false;
        }

        public void BloqueoTextBoxEdicion()
        {
            // Gestion de los controles que se pueden editar en modo edicion
            cbCliente.Enabled = false;
            cbLocal.Enabled = false;
            cbEmpresa.Enabled = false;

            txtFechaInicio.Enabled = false;
            txtFechaFin.Enabled = false; // No se permite poner la ficha fin en edicion
            txtPrecioMensual.Focus();
        }

        // Evento que se lanza al seleccionar una fila en el grid base
        private void GridBase_FilaSeleccionada(object sender, object entidad)
        {
            // Como recibe un objeto genérico, se chequea que sea del tipo contrato
            if(entidad is Contrato contrato)
            {
                // Actualiza el contrato seleccionada
                ContratoSeleccionado = contrato;

                // Limpia los textBox y muestra los datos del contrato seleccionado
                UtilesUI.LimpiarTextBoxes(this);

                // Muestra los datos del contrato seleccionado
                MostrarDatoscontrato(contrato);
            }
        }

        // Evento que se lanza al ordenar una columna en el grid base
        private void GridBase_Columnaseleccionada(object sender, int columnaIndex)
        {
            string nombreColumna = GridBase.Columns[columnaIndex].DataPropertyName;

            if(ordenAscendente)
            {
                GridBase.DataSource = listaContratos.OrderBy(emp => UtilesUI.GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                GridBase.DataSource = listaContratos.OrderByDescending(emp => UtilesUI.GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }

        // Muestra los datos del contrato en los textBox correspondientes
        private void MostrarDatoscontrato(Contrato contrato)
        {
            // Carga en los campos del cliente, local y empresa los datos que tiene el contrato
            cbCliente.SelectedValue = contrato.IdCliente;
            cbLocal.SelectedValue = contrato.IdLocal;
            cbEmpresa.SelectedValue = contrato.IdEmpresa;

            // Carga el resto de valores
            txtPrecioMensual.Text = contrato.PrecioMensual.ToString("N2");
            txtFechaInicio.Text = Utiles.FormatearFecha(contrato.FechaInicio);

            // La fecha de fin puede ser nula
            if(contrato.FechaFin.HasValue)
            {
                txtFechaFin.Text = Utiles.FormatearFecha(contrato.FechaFin.Value);
            }
            else
            {
                txtFechaFin.Text = "";
            }

            txtObservaciones.Text = contrato.Observaciones;
        }

        // Rellena la lista de empresas en el campo de empresas
        private void CargarListaEmpresas(bool? activos)
        {
            // Carga los valores en el campo de seleccion de la empresa
            var listaEmpresas = gestorEmpresas.ListarTodos(activas: activos);

            // Ordenar la lista alfabeticamente
            listaEmpresas = listaEmpresas.OrderBy(e => e.Nombre);

            // Crea una nueva lista para mostrar en el combobox y añade el elemento inicial
            var datosEmpresas = new List<Empresa>
            {
                // Añade a la lista el elemento inicial
                new Empresa { Id = 0, NIF = "", Nombre = "" }
            };

            // Añade la lista de empresas a continuacion
            datosEmpresas.AddRange(listaEmpresas);

            // Carga en el combobox la lista de empresas.
            cbEmpresa.DataSource = datosEmpresas.ToList(); // Origen de datos
            cbEmpresa.DisplayMember = "DatosEmpresa"; // Campo de la clase que se mostrara (campo calculado)
            cbEmpresa.ValueMember = "Id"; // Campo que se utiliza como indice de los elementos
            cbEmpresa.SelectedValue = ContratoSeleccionado.IdEmpresa; // Muestra en el campo el elemento seleccionado
        }

        // Rellena la lista de locales en el campo de locales
        private void CargarListaLocales(bool? activos)
        {
            // Carga los valores en el campo de seleccion del local
            var listaLocales = gestorLocales.ListarTodos(activos: activos);

            // Ordenar la lista alfabeticamente
            listaLocales = listaLocales.OrderBy(l => l.Descripcion);

            // Crea una nueva lista para mostrar en el combobox y añade el elemento inicial
            var datosLocales = new List<Local>
            {
                // Añade a la lista el elemento inicial
                new Local { Id = 0, Descripcion = "Seleccione un local" }
            };

            // Añade la lista de locales a continuacion
            datosLocales.AddRange(listaLocales);

            // Carga en el combobox la lista de clientes.
            cbLocal.DataSource = datosLocales; // Origen de datos
            cbLocal.DisplayMember = "DatosLocal"; //Campo de la clase que se mostrara (campo calculado)
            cbLocal.ValueMember = "Id"; // Campo que se utiliza como indice de los elementos
            cbLocal.SelectedValue = ContratoSeleccionado.IdLocal; // Muestra en el campo el elemento seleccionado
        }

        // Rellena la lista de clientes en el campo de clientes
        private void CargarListaClientes(bool? activos)
        {
            // Carga los valores en el campo de seleccion del cliente
            listaClientes = gestorClientes.ListarTodos(activos: activos);

            // Ordenar la lista alfabeticamente
            listaClientes = listaClientes.OrderBy(c => c.Nombre);

            // Crea una nueva lista para mostrar en el combobox y añade el elemento inicial
            var datosClientes = new List<Cliente>
            {
                // Añade a la lista el elemento inicial
                new Cliente { Id = 0, NIF = "", Nombre = "Seleccione un cliente" }
            };

            // Añade la lista de clientes a continuacion
            datosClientes.AddRange(listaClientes);

            // Carga en el combobox la lista de clientes.
            cbCliente.DataSource = datosClientes; // Origen de datos
            cbCliente.DisplayMember = "DatosCliente"; // Campo de la clase que se mostrara (campo calculado)
            cbCliente.ValueMember = "Id"; // Campo que se utiliza como indice de los elementos
            cbCliente.SelectedValue = ContratoSeleccionado.IdCliente; // Muestra en el campo el elemento seleccionado
        }

        private void txtFechaInicio_Enter(object sender, EventArgs e)
        {
            txtFechaInicio.Text = Utiles.FormatearFecha(DateTime.Today);
        }

        private void txtFechaInicio_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de inicio
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
            bool esValida = DateTime.TryParseExact(
                txtFechaInicio.Text,                                  // Fecha a validar
                formatosValidos,                                    // Formatos validos
                System.Globalization.CultureInfo.InvariantCulture,  // Cultura
                System.Globalization.DateTimeStyles.None,           // Sin estilos adicionales
                out _                                     // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaInicio.Focus();
            }
        }

        private void txtFechaFin_Enter(object sender, EventArgs e)
        {
            txtFechaFin.Text = Utiles.FormatearFecha(DateTime.Today);
        }

        private void txtFechaFin_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de baja
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };

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
                out _                                     // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaFin.Focus();
            }
        }

        private void TextBox_ToUpper(object sender, EventArgs e)
        {
            if(sender is TextBox txt)
            {
                txt.Text = txt.Text.ToUpper();
            }
        }

        private void txtImporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            UtilesUI.ValidarImporte(sender as TextBox, e);
        }

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

        private Empresa ObtenerEmpresaPorIdLocal(int id)
        {
            var gestorEmpresas = new GestorEmpresas();
            return gestorEmpresas.ObtenerPorId(id);
        }

        private void btnRevisionContrato_Click(object sender, EventArgs e)
        {
            var frmRevisiones = new frmRevisionContrato(ContratoActual);
            frmRevisiones.ShowDialog();
            CargarContratos();
        }

        // Evento al seleccionar un elemento y cerrar la lista
        private void cbCliente_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Asigna el cliente seleccionado al contrato
            ClienteContrato = cbCliente.SelectedItem as Cliente;
            cbLocal.Focus(); // Pone el foco en el local para forzar a seleccionar uno

        }

        // Evento al seleccionar un elemento y cerrar la lista
        private void cbLocal_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Carga el local seleccionado en el combobox
            var local = cbLocal.SelectedItem as Local;

            // Chequeo de que el local no tiene un contrato activo
            if(tipoProceso == Enumerador.TipoProceso.Alta && local.ContratoActivo)
            {
                MessageBox.Show("El local ya tiene un contrato activo.",
                                 "Local con contrato activo",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning);
                cbLocal.SelectedIndex = 0;
            }
            else
            {
                // Asigna el local seleccionado al contrato
                LocalContrato = cbLocal.SelectedItem as Local;

                // Obtiene el objeto Empresa segun el IdEmpresa del local
                EmpresaContrato = ObtenerEmpresaPorIdLocal(local.IdEmpresa);

                // Asigna los datos de la empresa al campo
                cbEmpresa.SelectedValue = local.IdEmpresa;
                txtPrecioMensual.Focus();
            }
        }

        private void cbCliente_Enter(object sender, EventArgs e)
        {
            // Al entrar al campo del cliente, se selecciona el texto de ayuda (solo en el alta se puede acceder)
            cbCliente.SelectedIndex = 0;
        }

        private void cbLocal_Enter(object sender, EventArgs e)
        {
            // Al entrar al campo del local , se selecciona el texto de ayuda (solo en el alta se puede acceder)
            cbLocal.SelectedIndex = 0;
            cbEmpresa.SelectedIndex = 0; // Como la empresa esta vinculada al local, se selecciona el texto de ayuda.
        }

        public void RestauraControles(bool activar)
        {
            switch(tipoProceso)
            {
                case Enumerador.TipoProceso.Alta:
                    BloqueoTextBoxAlta();
                    break;

                case Enumerador.TipoProceso.Edicion:
                    BloqueoTextBoxEdicion();
                    break;
            }
            // Restablece el bloqueo y habilita el grid
            UtilesUI.RestablecerPaneles<GestorContratos, Contrato>(GridBase, !activar, gestorContratos);

            // Refresca el grid de contratos
            CargarContratos(); // Refresca el grid
        }
    }
}
