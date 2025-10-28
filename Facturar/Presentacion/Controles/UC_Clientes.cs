using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Facturar.Entidades;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Clientes : UC_GridBase
    {
        // Propiedad privada para almacenar el cliente seleccionado en el grid
        private Cliente ClienteSeleccionado;

        // Almacena la lista de locales para poder ordenar
        private IEnumerable<Cliente> listaClientes;

        public UC_Clientes()
        {
            InitializeComponent();
        }

        public Cliente ClienteActual
        {
            get => ClienteSeleccionado;
        }

        public void CargarClientes(bool? activos = true)
        {
            var gestorClientes = new Servicios.GestorClientes();
            listaClientes = gestorClientes.ListarTodos(activos: activos);

            // Carga los datos de los clientes
            /* Pendiente de desarrollo 
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = listaEmpresas;
            */
        }

        public void ActualizarClienteSeleccionado()
        {
            // Pendiente de desarrollo
            /*
            if(dgvClientes.CurrentRow != null)
            {
                // Carga el objeto cliente segun la fila seleccionada
                ClienteSeleccionado = dgvClientes.CurrentRow.DataBoundItem as Cliente;
            }
            */
        }
    }
}
