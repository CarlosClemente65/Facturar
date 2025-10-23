using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Empresas : UserControl
    {
        public UC_Empresas()
        {
            InitializeComponent();
            var gestorEmpresas = new Servicios.GestorEmpresas();
            var dtEmpresas = gestorEmpresas.ListarTodos(activas: true);
            dgvEmpresas.DataSource = dtEmpresas;

            //dgvEmpresas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dgvEmpresas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgvEmpresas.ReadOnly = true;
            dgvEmpresas.Columns["Id"].DisplayIndex = 0;
            dgvEmpresas.Columns["NIF"].DisplayIndex = 1;
            dgvEmpresas.Columns["Nombre"].DisplayIndex = 2;
            dgvEmpresas.Columns["Direccion"].DisplayIndex = 3;
            dgvEmpresas.Columns["CodigoPostal"].DisplayIndex = 4;
            dgvEmpresas.Columns["CodigoPostal"].HeaderText = "Codigo postal";
            dgvEmpresas.Columns["Poblacion"].DisplayIndex = 5;
            dgvEmpresas.Columns["Provincia"].DisplayIndex = 6;
            dgvEmpresas.Columns["Telefono"].DisplayIndex = 7;
            dgvEmpresas.Columns["Email"].DisplayIndex = 8;
            dgvEmpresas.Columns["PersonaContacto"].DisplayIndex = 9;
            dgvEmpresas.Columns["PersonaContacto"].HeaderText = "Persona contacto";
            dgvEmpresas.Columns["FechaAlta"].DisplayIndex = 10;
            dgvEmpresas.Columns["FechaAlta"].HeaderText = "Fecha alta";
            dgvEmpresas.Columns["FechaBaja"].DisplayIndex = 11;
            dgvEmpresas.Columns["FechaBaja"].HeaderText = "Fecha baja";
            dgvEmpresas.Columns["SerieFactura"].DisplayIndex = 12;
            dgvEmpresas.Columns["SerieFactura"].HeaderText = "Serie factura";
            dgvEmpresas.Columns["NumeroFacturaActual"].DisplayIndex = 13;
            dgvEmpresas.Columns["NumeroFacturaActual"].HeaderText = "Ultima factura";
            dgvEmpresas.Columns["Activo"].Visible = false;
        }
    }
}
