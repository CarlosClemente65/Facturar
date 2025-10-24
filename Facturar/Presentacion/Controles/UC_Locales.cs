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
    public partial class UC_Locales : UserControl
    {
        // Almacena la lista de locales para poder ordenar
        private IEnumerable<Local> listaLocales;

        public UC_Locales()
        {
            InitializeComponent();
        }

        public void CargarLocales(bool? activos)
        {
            var gestorLocales = new Servicios.GestorLocales();
            listaLocales = gestorLocales.ListarTodos(activos: activos);

            // Carga los datos de los locales
            /* Pendiente de desarrollo 
            dgvLocales.DataSource = null;
            dgvLocales.DataSource = listaEmpresas;
            */
        }
    }
}
