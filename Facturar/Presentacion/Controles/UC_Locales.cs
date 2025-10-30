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
                ("FechaBaja", 11)
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
            // Pendiente de desarrollo y poner los campos que corresponda
            txtDescripcion.Text = local.Descripcion;
            txtImporte.Text = local.ImporteAlquiler.ToString("F2");
            txtDireccion.Text = local.Direccion;
            txtCodigoPostal.Text = local.CodigoPostal;
            txtPoblacion.Text = local.Poblacion;
            txtProvincia.Text = local.Provincia;
            txtNifEmpresa.Text = local.NIFEmpresa;
            txtNombreEmpresa.Text = local.NombreEmpresa;
            txtObservaciones.Text = local.Observaciones;
            txtFechaAlta.Text = local.FechaAlta.ToString("dd.MM.yyyy");
            
            // La fecha de baja puede ser nula
            if(local.FechaBaja.HasValue)
            {
                txtFechaBaja.Text = local.FechaBaja.Value.ToString("dd.MM.yyyy");
            }
            else
            {
                txtFechaBaja.Text = "";
            }
        }

        public void BloqueoTextBoxAlta()
        {
            // Deshabilita los TextBox que no se pueden editar
            txtFechaBaja.Enabled = false;
            txtNombreEmpresa.Enabled = false;
        }

        public void BloqueoTextBoxEdicion()
        {
            // Deshabilita los TextBox que no se pueden editar
            txtFechaAlta.Enabled = false;
            txtNombreEmpresa.Enabled = false;
        }

        public void ActualizarLocalSeleccionado()
        {
            
            if(dgvBase.CurrentRow != null)
            {
                // Carga el objeto local segun la fila seleccionada
                LocalSeleccionado = dgvBase.CurrentRow.DataBoundItem as Local;
            }
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
                // En el caso del alta, se localiza el IdEmpresa a grabar en el local segun el NifEmpresa
                
                //var empresaAlta = gestorEmpresas.ObtenerPorNIF(txtNifEmpresa.Text);
                local.IdEmpresa = ObtenerEmpresaPorNif(txtNifEmpresa.Text).Id;
            }

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
        }

        private void AplicarFormatoColumnas()
        {
            if(dgvBase.Columns.Count == 0) return; // Protege contra columnas vacías
             
            // Lista con los nombres de las propiedades a ajustar
            string[] columnasCentradas = { "Id", "CodigoPostal", "FechaAlta", "FechaBaja" };
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
                MessageBox.Show("Formato de fecha inválido. Usa uno de estos formatos: \ndd/MM/yyyy, dd.MM.yyyy o dd-MM-yyyy","Error de formato de fecha",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtFechaAlta.Focus();
            }
        }

        private void txtFechaBaja_Leave(object sender, EventArgs e)
        {
            // Validacion de la fecha de baja
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
            DateTime fechaValida;

            if (txtFechaBaja.Text.Trim() == "")
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
    }
}
