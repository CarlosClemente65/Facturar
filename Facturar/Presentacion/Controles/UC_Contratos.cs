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
    public partial class UC_Contratos : UserControl
    {
        private IEnumerable<Cliente> listaContratos;
        public UC_Contratos()
        {
            InitializeComponent();
        }

        public void CargarContratos(bool? activos)
        {
            var gestorContratos = new Servicios.GestorClientes();
            listaContratos = gestorContratos.ListarTodos(activos: activos);

            // Carga los datos de los contratos
            /* Pendiente de desarrollo 
            dgvContratos.DataSource = null;
            dgvContratos.DataSource = listaEmpresas;
            */
        }
    }
}
