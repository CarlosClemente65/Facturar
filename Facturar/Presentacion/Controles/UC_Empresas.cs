using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Empresas : UserControl
    {
        // Propiedad privada para almacenar la empresa seleccionada en el grid
        private Empresa EmpresaSeleccionada;

        // Almacena la lista de empresas para poder ordenar
        private IEnumerable<Empresa> listaEmpresas;

        private bool ordenAscendente = true;

        public UC_Empresas()
        {
            InitializeComponent();

            // Aplica el color de fondo de las filas seleccionadas (necesario para aplicar el efecto de bloqueo)
            dgvEmpresas.DefaultCellStyle.SelectionBackColor = Color.OldLace;
            dgvEmpresas.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Monta las columnas por orden
            InicializaColumnas();

            // Carga las empresas en el control
            CargarEmpresas(activas: true);


        }

        public Empresa EmpresaActual
        {
            get => EmpresaSeleccionada;
        }

        public void CargarEmpresas(bool? activas = true)
        {
            var gestorEmpresas = new Servicios.GestorEmpresas();
            listaEmpresas = gestorEmpresas.ListarTodos(activas: activas);

            // Carga los datos de las empresas
            dgvEmpresas.DataSource = null;
            dgvEmpresas.DataSource = listaEmpresas;
        }

        private void InicializaColumnas()
        {
            dgvEmpresas.AutoGenerateColumns = false; // Se desactiva la autogeneracion de columnas
            dgvEmpresas.Columns.Clear();

            // Inserta las columnas en el grid segun el orden indicado
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "Id", 0);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "NIF", 1);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "Nombre", 2);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "Direccion", 3);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "CodigoPostal", 4);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "Poblacion", 5);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "Provincia", 6);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "Telefono", 7);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "Email", 8);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "PersonaContacto", 9);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "FechaAlta", 10);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "FechaBaja", 11);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "SerieFactura", 12);
            Utiles.InsertaColumnaDGW<Empresa>(dgw: dgvEmpresas, "NumeroFacturaActual", 13);

        }

        private void dgvEmpresas_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nombreColumna = dgvEmpresas.Columns[e.ColumnIndex].DataPropertyName;

            if(ordenAscendente)
            {
                dgvEmpresas.DataSource = listaEmpresas.OrderBy(emp => GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                dgvEmpresas.DataSource = listaEmpresas.OrderByDescending(emp => GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }

        private object GetPropValue(object obj, string nombreColumna)
        {
            return obj.GetType().GetProperty(nombreColumna).GetValue(obj, null);
        }

        private void dgvEmpresas_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvEmpresas.CurrentRow != null && dgvEmpresas.CurrentRow.DataBoundItem is Empresa empresa)
            {
                EmpresaSeleccionada = empresa;
                Utiles.LimpiarTextBoxes(this);
                MostrarDatosEmpresa(empresa);
            }
        }


        private void MostrarDatosEmpresa(Empresa empresa)
        {
            txtNif.Text = empresa.NIF;
            txtNombreEmpresa.Text = empresa.Nombre;
            txtFechaAlta.Text = empresa.FechaAlta.ToShortDateString();
            if(empresa.FechaBaja.HasValue)
            {
                txtFechaBaja.Text = empresa.FechaBaja.Value.ToShortDateString();
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

        public void ActualizaEmpresaSeleccionada()
        {
            if(dgvEmpresas.CurrentRow != null)
            {
                // Carga el objeto empresa segun la fila seleccionada.
                EmpresaSeleccionada = dgvEmpresas.CurrentRow.DataBoundItem as Empresa;
            }
        }

        public void ActualizaPropiedadesEmpresa(Empresa empresa)
        {
            if(empresa == null)
            {
                throw new ArgumentNullException("No se han pasado datos de empresa para actualizar");
            }

            // Actualizacion de la empresa segun el contenido de los textBox
            empresa.NIF = txtNif.Text;
            empresa.Nombre = txtNombreEmpresa.Text;
            empresa.Direccion = txtDireccion.Text;
            empresa.CodigoPostal = txtCodigoPostal.Text;
            empresa.Poblacion = txtPoblacion.Text;
            empresa.Provincia = txtProvincia.Text;
            empresa.Telefono = txtTelefono.Text;
            empresa.Email = txtEmail.Text;
            empresa.PersonaContacto = txtPersonaContacto.Text;
            empresa.SerieFactura = txtSerieFactura.Text;

            int numeroFactura;
            if(!int.TryParse(txtFactura.Text, out numeroFactura))
            {
                numeroFactura = 0; // Valor por defecto por si el campo esta vacio
            }
            empresa.NumeroFacturaActual = numeroFactura;
        }

        
    }
}
