using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Servicios;
using Enumerador = Facturar.Utilidades.Enumeradores;
using UtilesUI = Facturar.Utilidades.UtilidadesUI;
using Utiles = Facturar.Utilidades.UtilesGenerales;


namespace Facturar.Presentacion.Controles
{
    public partial class UC_Locales : UC_GridBase
    {
        // Propiedad privada para almacenar el local seleccionado en el grid
        private Local LocalSeleccionado;

        private Empresa EmpresaLocal;

        // Define el tipo de proceso (alta o edicion)
        public Enumerador.TipoProceso tipoProceso;

        // Instancias de los gestores necesarios
        GestorEmpresas gestorEmpresas = new GestorEmpresas();
        GestorLocales gestorLocales = new GestorLocales();
        GestorContratos gestorContratos = new GestorContratos();

        // Almacena la lista de locales para poder ordenar
        private IEnumerable<Local> listaLocales;

        private bool ordenAscendente = true;

        private bool datosCargados = false;

        public UC_Locales()
        {
            InitializeComponent();
        }

        // Propiedad publica para ver el local seleccionado en el grid
        public Local LocalActual
        {
            get => LocalSeleccionado;
            set => LocalSeleccionado = value;
        }

        private void UC_Locales_Load(object sender, EventArgs e)
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
                CargarLocales(activos: true);
                datosCargados = true;
            }
        }

        public void CargarLocales(bool? activos = true)
        {
            listaLocales = gestorLocales.ListarTodos(activos: activos);

            // Carga las empresas relacionadas para mostrar los datos en el grid
            foreach(var local in listaLocales)
            {
                local.CargarRelaciones(gestorEmpresas: gestorEmpresas, gestorContratos: gestorContratos);
            }

            // Carga los datos de los locales en e gridbase
            GridBase.DataSource = null;
            GridBase.DataSource = listaLocales.ToList();

            AplicarFormatoColumnas();

            // Carga la lista de empresas en el combobox
            CargarListaEmpresas(activos);
        }

        // Define las columnas a mostrar en el grid base y el orden que tendran
        public void ActualizarLocalSeleccionado()
        {
            if(dgvBase.CurrentRow?.DataBoundItem is Local local)
            {
                // Carga el objeto local segun la fila seleccionada
                LocalSeleccionado = local;
                EmpresaLocal = local.Empresa ?? gestorEmpresas.ObtenerPorId(local.IdEmpresa); // Carga la empresa del contrato y si no existe lo obtiene del gestor
            }
        }

        // Actualiza las propiedades del local segun el contenido de los textBox
        public void ActualizaPropiedadesLocal(Local local)
        {
            if(local == null)
            {
                throw new ArgumentNullException("No se han pasado datos del local para actualizar");
            }

            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                // Campos a actualizar en el caso del alta (de momento no hay restricciones)
                local.IdEmpresa = EmpresaLocal.Id;
            }

            // Resto de campos comunes
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
            local.FechaAlta = Utiles.ConvertirFecha(txtFechaAlta.Text) ?? DateTime.Today;
        }

        private void InicializaColumnas()
        {
            var columnas = new (string nombrePropiedad, int orden)[]
            {
                ("Id", 0),
                ("Descripcion", 1),
                ("Direccion", 2),
                ("CodigoPostal", 3),
                ("Poblacion", 4),
                ("Provincia", 5),
                ("NIFEmpresa",6),
                ("NombreEmpresa",7),
                ("ImporteAlquiler", 8),
                ("Observaciones", 9),
                ("IdContrato", 10),
                ("FechaAlta", 11),
                ("FechaBaja", 12)
            };

            // Pasa las columnas al grid base para que las configure
            ConfigurarColumnas<Local>(columnas);
        }

        private void AplicarFormatoColumnas()
        {
            if(dgvBase.Columns.Count == 0) return; // Protege contra columnas vacías

            // Lista con los nombres de las propiedades a ajustar
            string[] columnasCentradas = { "Id", "CodigoPostal", "FechaAlta", "FechaBaja", "IdContrato" };
            string[] columnasFecha = { "FechaAlta", "FechaBaja" };
            string[] columnasImportes = { "ImporteAlquiler" };

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

        // Evento que se lanza al seleccionar una fila en el grid base
        public void BloqueoTextBoxAlta()
        {
            // Gestion de los controles que se pueden editar en modo alta
            cbEmpresa.Enabled = true;
            cbEmpresa.SelectedIndex = 0;
            txtImporte.Enabled = false;
            txtFechaBaja.Enabled = false;
            txtDescripcion.Focus();
        }

        public void BloqueoTextBoxEdicion()
        {
            // Deshabilita los TextBox que no se pueden editar
            cbEmpresa.Enabled = false;
            txtImporte.Enabled = false;
            txtFechaAlta.Enabled = false;
            txtFechaBaja.Enabled = false;
        }

        private void GridBase_FilaSeleccionada(object sender, object entidad)
        {
            // Como recibe un objeto genérico, se chequea que sea del tipo Local
            if(entidad is Local local)
            {
                // Actualiza la empresa seleccionada
                LocalSeleccionado = local;

                // Limpia los textBox y muestra los datos de la empresa seleccionada
                UtilesUI.LimpiarTextBoxes(this);

                // Muestra los datos del local seleccionado
                MostrarDatosLocal(local);
            }
        }

        // Evento que se lanza al ordenar una columna en el grid base
        private void GridBase_Columnaseleccionada(object sender, int columnaIndex)
        {
            string nombreColumna = GridBase.Columns[columnaIndex].DataPropertyName;

            if(ordenAscendente)
            {
                GridBase.DataSource = listaLocales.OrderBy(emp => UtilesUI.GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                GridBase.DataSource = listaLocales.OrderByDescending(emp => UtilesUI.GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }

        // Muestra los datos de la empresa en los textBox correspondientes
        private void MostrarDatosLocal(Local local)
        {
            // Carga los valores en los campos
            txtDescripcion.Text = local.Descripcion;
            txtImporte.Text = local.ImporteAlquiler.ToString("F2");
            txtFechaAlta.Text = Utiles.FormatearFecha(local.FechaAlta);
            if(local.FechaBaja.HasValue) // La fecha de baja puede ser nula
            {
                txtFechaBaja.Text = Utiles.FormatearFecha(local.FechaBaja.Value);
            }
            else
            {
                txtFechaBaja.Text = "";
            }
            txtDireccion.Text = local.Direccion;
            txtCodigoPostal.Text = local.CodigoPostal;
            txtPoblacion.Text = local.Poblacion;
            txtProvincia.Text = local.Provincia;
            cbEmpresa.SelectedValue = local.IdEmpresa; // Carga en el combobox la empresa del local
            txtObservaciones.Text = local.Observaciones;
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
            cbEmpresa.SelectedValue = LocalSeleccionado.IdEmpresa; // Muestra en el campo el elemento seleccionado
        }

        private void txtFechaAlta_Enter(object sender, EventArgs e)
        {
            txtFechaAlta.Text = Utiles.FormatearFecha(DateTime.Today);
        }

        private void txtFechaAlta_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de alta
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
            bool esValida = DateTime.TryParseExact(
                txtFechaAlta.Text,                                  // Fecha a validar
                formatosValidos,                                    // Formatos validos
                System.Globalization.CultureInfo.InvariantCulture,  // Cultura
                System.Globalization.DateTimeStyles.None,           // Sin estilos adicionales
                out _                                     // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaAlta.Focus();
            }
        }

        private void txtFechaBaja_Enter(object sender, EventArgs e)
        {
            txtFechaBaja.Text = Utiles.FormatearFecha(DateTime.Today);
        }

        private void txtFechaBaja_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de baja
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
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
                out _                                     // Fecha resultante
                );

            if(!esValida)
            {
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy", "Error de formato de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaBaja.Focus();
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
            UtilesUI.FormatearImporte(sender as TextBox);
        }


        // Evento al seleccionar un elemento y cerrar la lista
        private void cbEmpresa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            EmpresaLocal = cbEmpresa.SelectedItem as Empresa;
            txtObservaciones.Focus();
        }

        private void cbEmpresa_Enter(object sender, EventArgs e)
        {
            cbEmpresa.SelectedIndex = 0;
        }
    }
}
