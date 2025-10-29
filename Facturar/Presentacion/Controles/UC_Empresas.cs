using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Utiles = Facturar.Utilidades.UtilidadesUI;
using Enumerador = Facturar.Utilidades.Enumeradores;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Empresas : UC_GridBase
    {
        // Propiedad privada para almacenar la empresa seleccionada en el grid
        private Empresa EmpresaSeleccionada;

        // Almacena la lista de empresas para poder ordenar
        private IEnumerable<Empresa> listaEmpresas;

        private bool ordenAscendente = true;

        private bool datosCargados = false;

        public UC_Empresas()
        {
            InitializeComponent();
        }

        public Empresa EmpresaActual
        {
            get => EmpresaSeleccionada;
            set => EmpresaSeleccionada = value;
        }


        private void UC_Empresas_Load(object sender, EventArgs e)
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
                CargarEmpresas(activas: true);
                datosCargados = true;
            }
        }

        public void CargarEmpresas(bool? activas = true)
        {
            var gestorEmpresas = new Servicios.GestorEmpresas();
            listaEmpresas = gestorEmpresas.ListarTodos(activas: activas);

            // Carga los datos de las empresas en el gridBase
            GridBase.DataSource = null;
            GridBase.DataSource = listaEmpresas.ToList();

            AplicarFormatoColumnas();

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
                ("PersonaContacto", 9),
                ("FechaAlta", 10),
                ("FechaBaja", 11),
                ("SerieFactura", 12),
                ("NumeroFacturaActual", 13)
            };

            // Pasa las columnas al grid base para que las configure
            ConfigurarColumnas<Empresa>(columnas);

        }


        // Evento que se lanza al seleccionar una fila en el grid base
        private void GridBase_FilaSeleccionada(object sender, object entidad)
        {
            // Como recibe un objeto genérico, se chequea que sea del tipo Empresa
            if(entidad is Empresa empresa)
            {
                // Actualiza la empresa seleccionada
                EmpresaSeleccionada = empresa;

                // Limpia los textBox y muestra los datos de la empresa seleccionada
                Utiles.LimpiarTextBoxes(this);

                // Muestra los datos de la empresa seleccionada
                MostrarDatosEmpresa(empresa);
            }
        }

        // Evento que se lanza al ordenar una columna en el grid base
        private void GridBase_Columnaseleccionada(object sender, int columnaIndex)
        {
            string nombreColumna = GridBase.Columns[columnaIndex].DataPropertyName;

            if(ordenAscendente)
            {
                GridBase.DataSource = listaEmpresas.OrderBy(emp => GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                GridBase.DataSource = listaEmpresas.OrderByDescending(emp => GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }


        // Devuelve el valor de una propiedad de un objeto por su nombre
        private object GetPropValue(object obj, string nombreColumna)
        {
            return obj.GetType().GetProperty(nombreColumna).GetValue(obj, null);
        }


        // Muestra los datos de la empresa en los textBox correspondientes
        private void MostrarDatosEmpresa(Empresa empresa)
        {
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
        }


        // Actualiza la empresa seleccionada segun la fila activa del grid
        public void ActualizaEmpresaSeleccionada()
        {
            if(dgvBase.CurrentRow != null)
            {
                EmpresaSeleccionada = dgvBase.CurrentRow.DataBoundItem as Empresa;
            }
        }


        // Actualiza las propiedades de la empresa segun el contenido de los textBox
        public void ActualizaPropiedadesEmpresa(Empresa empresa, Enumerador.TipoProceso tipoProceso)
        {
            if(empresa == null)
            {
                throw new ArgumentNullException("No se han pasado datos de empresa para actualizar");
            }

            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                // En el caso del alta, se asignan las propiedades que no se pueden modificar en la edición
                empresa.NIF = txtNif.Text;  // No se permite modificar el NIF
                empresa.Nombre = txtNombreEmpresa.Text; // No se permite modificar el nombre

                // El campo NumeroFacturaActual es la ultima factura emitida, por lo que en el alta se permite indicar por si empieza por un numero diferente
                int numeroFactura;
                if(!int.TryParse(txtFactura.Text, out numeroFactura))
                {
                    numeroFactura = 0; // Valor por defecto por si el campo esta vacio
                }
                empresa.NumeroFacturaActual = numeroFactura;
            }

            // Campos comunes en el alta y edicion
            empresa.Direccion = txtDireccion.Text;
            empresa.CodigoPostal = txtCodigoPostal.Text;
            empresa.Poblacion = txtPoblacion.Text;
            empresa.Provincia = txtProvincia.Text;
            empresa.Telefono = txtTelefono.Text;
            empresa.Email = txtEmail.Text;
            empresa.PersonaContacto = txtPersonaContacto.Text;
            empresa.SerieFactura = txtSerieFactura.Text;

            /* Los siguientes campos no se permiten modificar
            

            */
        }

        private void AplicarFormatoColumnas()
        {
            if(GridBase.Columns.Count == 0) return; // Protege contra columnas vacías


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

            // Ajuste al contenido
            GridBase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }
}
