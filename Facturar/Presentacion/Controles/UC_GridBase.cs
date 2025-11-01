using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_GridBase : UserControl
    {
        // Exposición pública del grid base para si fuera necesario usar alguna propiedad o método específico
        public DataGridView GridBase => dgvBase;

        // Expone eventos protegidos para que las clases derivadas puedan suscribirse
        protected event EventHandler<object> FilaSeleccionada;
        protected event EventHandler<int> ColumnaOrdenada;

        protected UC_GridBase()
        {
            InitializeComponent();
            ConfigurarGrid();
        }


        // Aplica el color de fondo de las filas seleccionadas (necesario para aplicar el efecto de bloqueo)
        private void ConfigurarGrid()
        {
            GridBase.DefaultCellStyle.SelectionBackColor = Color.Wheat;
            GridBase.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        // Evento que se dispara al hacer clic en el encabezado de una columna para ordenar
        private void dgvBase_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColumnaOrdenada?.Invoke(this, e.ColumnIndex);
        }

        // Evento que se dispara al cambiar la selección de fila en el grid
        private void dgvBase_SelectionChanged(object sender, System.EventArgs e)
        {
            if(dgvBase.CurrentRow != null)
            {
                var entidadSeleccionada = GridBase.CurrentRow.DataBoundItem;
                FilaSeleccionada?.Invoke(this, entidadSeleccionada);
            }
        }


        // Configura las columnas del grid según las propiedades y el orden indicados
        protected void ConfigurarColumnas<T>(IEnumerable<(string nombrePropiedad, int orden)> columnas)
        {
            GridBase.AutoGenerateColumns = false;
            GridBase.Columns.Clear();

            foreach(var (nombrePropiedad, orden) in columnas)
            {
                Utiles.InsertaColumnaDGV<T>(GridBase, nombrePropiedad, orden);
            }
        }
    }
}
