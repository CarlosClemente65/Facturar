using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facturar.Entidades;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Clientes : UserControl
    {
        // Almacena la lista de locales para poder ordenar
        private IEnumerable<Cliente> listaClientes;

        public UC_Clientes()
        {
            InitializeComponent();
        }

        public void CargarClientes(bool? activos)
        {
            var gestorClientes = new Servicios.GestorClientes();
            listaClientes = gestorClientes.ListarTodos(activos: activos);

            // Carga los datos de los clientes
            /* Pendiente de desarrollo 
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = listaEmpresas;
            */
        }
    }
}
