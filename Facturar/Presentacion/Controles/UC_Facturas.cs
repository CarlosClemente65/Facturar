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
    public partial class UC_Facturas : UC_GridBase
    {
        // Propiedad privada para almacenar la factura seleccionada en el grid
        private Factura FacturaSeleccionada;

        // Almacena la lista de facturas para poder ordenar
        private IEnumerable<Factura> listaFacturas;

        private bool ordenAscendente = true;

        private bool datosCargados = false; // Evita que se recargue el control innecesariamente
        public UC_Facturas()
        {
            InitializeComponent();
        }

        public Factura FacturaActual
        {
            get => FacturaSeleccionada;
            set => FacturaSeleccionada = value;
        }

        private void UC_Facturas_Load(object sender, EventArgs e)
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
                CargarFacturas();
                datosCargados = true;
            }

            //BloqueoTextBox();
        }

        public void BloqueoTextBoxAlta()
        {
            // Deshabilita los textBox de importes de la factura
            txtBaseFactura.Enabled = false;
            txtCuotaIVA.Enabled = false;
            txtCuotaIRPF.Enabled = false;
            txtTotalFactura.Enabled = false;
        }

        public void BloqueoTextBoxEditar()
        {
            // Deshabilita los TextBox que no se pueden editar
            BloqueoTextBoxAlta();
            txtNombreCliente.Enabled = false;
            txtNombreEmpresa.Enabled = false;
        }


        public void CargarFacturas()
        {
            var gestorFacturas = new Servicios.GestorFacturas();
            listaFacturas = gestorFacturas.ListarTodos();

            // Carga los datos de las empresas en el gridBase
            GridBase.DataSource = null;
            GridBase.DataSource = listaFacturas.ToList();

            AplicarFormatoColumnas();
        }

        // Define las columnas a mostrar en el grid base y el orden que tendran
        private void InicializaColumnas()
        {
            var columnas = new (string nombrePropiedad, int orden)[]
            {
                ("FechaFactura", 0),
                ("SerieFactura", 1),
                ("NumeroFactura", 2),
                ("NIFEmpresa", 3),
                ("NombreEmpresa", 4),
                ("NIFCliente", 5),
                ("NombreCliente", 6),
                ("TotalBase", 7),
                ("TotalIVA", 8),
                ("TotalIRPF", 9),
                ("TotalFactura", 10),
            };

            // Pasa las columnas al grid base para que las configure
            ConfigurarColumnas<Factura>(columnas);

        }

        // Evento que se lanza al seleccionar una fila en el grid base
        private void GridBase_FilaSeleccionada(object sender, object entidad)
        {
            // Como recibe un objeto genérico, se chequea que sea del tipo Factura
            if(entidad is Factura factura)
            {
                // Actualiza la factura seleccionada
                FacturaSeleccionada = factura;

                // Limpia los textBox y muestra los datos de la empresa seleccionada
                Utiles.LimpiarTextBoxes(this);

                // Muestra los datos de la empresa seleccionada
                MostrarDatosFactura(factura);
            }
        }

        // Evento que se lanza al ordenar una columna en el grid base
        private void GridBase_Columnaseleccionada(object sender, int columnaIndex)
        {
            string nombreColumna = GridBase.Columns[columnaIndex].DataPropertyName;

            if(ordenAscendente)
            {
                GridBase.DataSource = listaFacturas.OrderBy(emp => GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                GridBase.DataSource = listaFacturas.OrderByDescending(emp => GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }


        // Devuelve el valor de una propiedad de un objeto por su nombre
        private object GetPropValue(object obj, string nombreColumna)
        {
            return obj.GetType().GetProperty(nombreColumna).GetValue(obj, null);
        }


        // Muestra los datos de la factura en los textBox correspondientes
        private void MostrarDatosFactura(Factura factura)
        {
            txtFechaFactura.Text = factura.FechaFactura.ToString("dd.MM.yyyy");
            txtSerieFactura.Text = factura.SerieFactura;
            txtNumeroFactura.Text = factura.NumeroFactura;
            txtBaseFactura.Text = factura.TotalBase.ToString("N2");
            txtCuotaIVA.Text = factura.TotalIVA.ToString("N2");
            txtCuotaIRPF.Text = factura.TotalIRPF.ToString("N2");
            txtTotalFactura.Text = factura.TotalFactura.ToString("N2");
            txtNifEmpresa.Text = factura.NIFEmpresa;
            txtNombreEmpresa.Text = factura.NombreEmpresa;
            txtNifCliente.Text = factura.NIFCliente;
            txtNombreCliente.Text = factura.NombreCliente;
            txtObservaciones.Text = factura.Observaciones;
        }


        // Actualiza la factura seleccionada segun la fila activa del grid
        public void ActualizaFacturaSeleccionada()
        {
            if(dgvBase.CurrentRow != null)
            {
                FacturaSeleccionada = dgvBase.CurrentRow.DataBoundItem as Factura;
            }
        }

        private void AplicarFormatoColumnas()
        {
            if(GridBase.Columns.Count == 0) return; // Protege contra columnas vacías

            // Lista con los nombres de las propiedades a ajustar
            string[] columnasImportes = { "TotalBase", "TotalIVA", "TotalIRPF", "TotalFactura" };
            string[] columnasCentradas = { "FechaFactura", "SerieFactura", "NumeroFactura" };
            string[] columnasFecha = { "FechaFactura" };

            // Aplica formatos
            foreach(DataGridViewColumn columna in GridBase.Columns)
            {
                // Aplica formato de importe y alineado a la derecha
                if(columnasImportes.Contains(columna.DataPropertyName))
                {
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    columna.DefaultCellStyle.Format = "N2";
                }

                // Centrado del texto
                if(columnasCentradas.Contains(columna.DataPropertyName))
                {
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Aplica formato de fecha
                if(columnasFecha.Contains(columna.DataPropertyName))
                {
                    columna.DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }

            // Ajuste al contenido
            GridBase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        // Actualiza las propiedades de la factura segun el contenido de los textBox
        public void ActualizaPropiedadesFactura(Factura factura, Enumerador.TipoProceso tipoProceso)
        {
            if(factura == null)
            {
                throw new ArgumentNullException("No se han pasado datos de la factura para actualizar");
            }

            /* Metodo comentado hasta decidir si se permite crear facturas desde el control. Tiene el codigo de empresas para tenerlo como guia
            if(tipoProceso == Enumerador.TipoProceso.Alta)
            {
                  
                // En el caso del alta, se asignan las propiedades que no se pueden modificar en la edición
                empresa.NIF = txtNifEmpresa.Text;  // No se permite modificar el NIF
                empresa.Nombre = txtNombreEmpresa.Text; // No se permite modificar el nombre

                // El campo NumeroFacturaActual es la ultima factura emitida, por lo que en el alta se permite indicar por si empieza por un numero diferente
                int numeroFactura;
                if(!int.TryParse(txtNumeroFactura.Text, out numeroFactura))
                {
                    numeroFactura = 0; // Valor por defecto por si el campo esta vacio
                }
                empresa.NumeroFacturaActual = numeroFactura;

            }

            // Campos comunes en el alta y edicion
            empresa.Direccion = txtDireccion.Text;
            empresa.CodigoPostal = txtCodigoPostal.Text;
            empresa.Poblacion = txtBaseFactura.Text;
            empresa.Provincia = txtCuotaIVA.Text;
            empresa.Telefono = txtTelefono.Text;
            empresa.Email = txtEmail.Text;
            empresa.PersonaContacto = txtPersonaContacto.Text;
            empresa.SerieFactura = txtSerieFactura.Text;

            */
        }

        
    }
}
