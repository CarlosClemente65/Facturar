using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Controles
{
    public partial class UC_GridBase : UserControl
    {
        public DataGridView GridBase => dgvBase; // Exposición pública del grid

        // Propiedades para gestionar y ordenar los datos
        private IEnumerable<Empresa> listaDatos;
        private bool ordenAscendente = true;

        public UC_GridBase()
        {
            InitializeComponent();
            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            // Aplica el color de fondo de las filas seleccionadas (necesario para aplicar el efecto de bloqueo)
            GridBase.DefaultCellStyle.SelectionBackColor = Color.OldLace;
            GridBase.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        public void EstablecerDatos<T>(IEnumerable<T> datos)
        {
            GridBase.DataSource = null;
            GridBase.DataSource = datos.ToList();
        }
        public void Mostrar()
        {
            this.Visible = true;
        }

        private void dgvBase_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string nombreColumna = GridBase.Columns[e.ColumnIndex].DataPropertyName;

            if(ordenAscendente)
            {
                GridBase.DataSource = listaDatos.OrderBy(emp => GetPropValue(emp, nombreColumna)).ToList();
            }
            else
            {
                GridBase.DataSource = listaDatos.OrderByDescending(emp => GetPropValue(emp, nombreColumna)).ToList();
            }

            ordenAscendente = !ordenAscendente;
        }


        private object GetPropValue(object obj, string nombreColumna)
        {
            return obj.GetType().GetProperty(nombreColumna).GetValue(obj, null);
        }

        public void ConfigurarColumnas<T>(IEnumerable<(string nombrePropiedad, int orden)> columnas)
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
