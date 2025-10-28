using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Facturar.Entidades;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_Contratos : UC_GridBase
    {
        // Propiedad privada para almacenar el cliente seleccionado en el grid
        private Contrato ContratoSeleccionado;

        // Almacena la lista de contratos para poder ordenar
        private IEnumerable<Contrato> listaContratos;
        public UC_Contratos()
        {
            InitializeComponent();
        }

        public Contrato ContratoActual
        {
            get => ContratoSeleccionado;
        }

        public void CargarContratos(bool? activos = true)
        {
            var gestorContratos = new Servicios.GestorContratos();
            listaContratos = gestorContratos.ListarTodos(activos: activos);

            // Carga los datos de los contratos
            /* Pendiente de desarrollo 
            dgvContratos.DataSource = null;
            dgvContratos.DataSource = listaEmpresas;
            */
        }

        internal void ActualizarContratoSeleccionado()
        {
            // Pendiente de desarrollo
            /*
            if(dgvContratos.CurrentRow != null)
            {
                // Carga el objeto contrato segun la fila seleccionada
                ContratoSeleccionado = dgvContratos.CurrentRow.DataBoundItem as Contrato;
            }
            */
        }
    }
}
