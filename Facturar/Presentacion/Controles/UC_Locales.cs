using System.Collections.Generic;
using System.Windows.Forms;
using Facturar.Entidades;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Locales : UserControl
    {
        // Propiedad privada para almacenar el local seleccionado en el grid
        private Local LocalSeleccionado;

        // Almacena la lista de locales para poder ordenar
        private IEnumerable<Local> listaLocales;

        public UC_Locales()
        {
            InitializeComponent();
        }

        public Local LocalActual
        {
            get => LocalSeleccionado;
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

        public void ActualizarLocalSeleccionado()
        {
            // Pendiente de desarrollo
            /*
            if(dgvLocales.CurrentRow != null)
            {
                // Carga el objeto local segun la fila seleccionada
                LocalSeleccionado = dgvLocales.CurrentRow.DataBoundItem as Local;
            }
            */
        }
    }
}
