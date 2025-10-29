using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Utiles = Facturar.Utilidades.UtilidadesUI;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Facturar.Servicios;


namespace Facturar.Presentacion.Controles
{
    public partial class UC_Locales : UC_GridBase
    {
        // Propiedad privada para almacenar el local seleccionado en el grid
        private Local LocalSeleccionado;

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
            var gestorLocales = new Servicios.GestorLocales();
            listaLocales = gestorLocales.ListarTodos(activos: activos);

            // Carga los datos de los locales
            // Carga los datos de las empresas en el gridBase
            GridBase.DataSource = null;
            GridBase.DataSource = listaLocales.ToList();

            AplicarFormatoColumnas();
        }

        // Define las columnas a mostrar en el grid base y el orden que tendran
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
                ("FechaAlta", 10),
                ("FechaBaja", 11),
                ("SerieFactura", 12)
            };

            // Pasa las columnas al grid base para que las configure
            ConfigurarColumnas<Empresa>(columnas);

        }

        // Evento que se lanza al seleccionar una fila en el grid base
        private void GridBase_FilaSeleccionada(object sender, object entidad)
        {
            // Como recibe un objeto genérico, se chequea que sea del tipo Local
            if(entidad is Local local)
            {
                // Actualiza la empresa seleccionada
                LocalSeleccionado = local;

                // Limpia los textBox y muestra los datos de la empresa seleccionada
                Utiles.LimpiarTextBoxes(this);

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
                GridBase.DataSource = listaLocales.OrderBy(emp => Utiles.GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                GridBase.DataSource = listaLocales.OrderByDescending(emp => Utiles.GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }


        // Muestra los datos de la empresa en los textBox correspondientes
        private void MostrarDatosLocal(Local local)
        {
            /* Pendiente de desarrollo y poner los campos que corresponda
            txtNif.Text = empresa.NIF;
            txtNombreEmpresa.Text = empresa.Nombre;
            txtFechaAlta.Text = empresa.FechaAlta.ToString("dd.MM.yyyy");

            // La fecha de baja puede ser nula
            if(empresa.FechaBaja.HasValue)
            {
                txtFechaBaja.Text = empresa.FechaBaja.Value.ToString("dd.MM.yyyy");
            }
            else
            {
                txtFechaBaja.Text = "";
            }

            txtDireccion.Text = empresa.Direccion;
            txtCodigoPostal.Text = empresa.CodigoPostal;
            txtPoblacion.Text = empresa.Poblacion;
            txtProvincia.Text = empresa.Provincia;
            txtTelefono.Text = empresa.Telefono;
            txtEmail.Text = empresa.Email;
            txtPersonaContacto.Text = empresa.PersonaContacto;
            txtSerieFactura.Text = empresa.SerieFactura;
            txtFactura.Text = empresa.NumeroFacturaActual.ToString();

            */
        }

        public void ActualizarLocalSeleccionado()
        {
            /* Pendiente de desarrollo
            
            if(dgvLocales.CurrentRow != null)
            {
                // Carga el objeto local segun la fila seleccionada
                LocalSeleccionado = dgvLocales.CurrentRow.DataBoundItem as Local;
            }

            */
        }

        // Actualiza las propiedades del local segun el contenido de los textBox
        public void ActualizaPropiedadesLocal(Local local, Enumerador.TipoProceso tipoProceso)
        {
            if(local == null)
            {
                throw new ArgumentNullException("No se han pasado datos del local para actualizar");
            }

            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                // En el caso del alta, se asignan las propiedades que no se pueden modificar en la edición
                var gestorEmpresas = new GestorEmpresas();
                var empresaAlta = gestorEmpresas.ObtenerPorNIF(local.NIFEmpresa);
                local.IdEmpresa = empresaAlta.Id;
            }

            // Campos comunes en el alta y edicion
            local.Descripcion = txtDescripcion.Text;
            local.Direccion = txtDireccion.Text;
            local.CodigoPostal = txtCodigoPostal.Text;
            local.Poblacion = txtPoblacion.Text;
            local.ImporteAlquiler = decimal.Parse(txtImporte.Text);
            local.Observaciones = txtObservaciones.Text;

            /* Los siguientes campos no se permiten modificar
            

            */
        }

        private void AplicarFormatoColumnas()
        {
            if(GridBase.Columns.Count == 0) return; // Protege contra columnas vacías

            /* Revisar este metodo para ver el formato de las columas segun corresponda
             
            // Lista con los nombres de las propiedades a ajustar
            string[] columnasCentradas = { "Id", "CodigoPostal", "FechaAlta", "FechaBaja", "SerieFactura", "NumeroFacturaActual" };
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

            */

            // Ajuste al contenido
            GridBase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }
}
