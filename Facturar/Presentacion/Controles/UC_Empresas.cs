using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facturar.Entidades;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Empresas : UserControl
    {

        // Almacena la lista de empresas para poder ordenar
        private IEnumerable<Empresa> listaEmpresas;

        private bool ordenAscendente = true;

        private Empresa EmpresaSeleccionada;
        //Dictionary<string, string> NombresEncabezado;


        public UC_Empresas()
        {
            InitializeComponent();

            // Monta las columnas por orden
            InicializaColumnas();

            // Carga las empresas en el control
            CargarEmpresas(activas: true);

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
    }
}
