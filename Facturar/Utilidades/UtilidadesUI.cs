using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Interfaces;
using Facturar.Utilidades;
using Utiles = Facturar.Utilidades.UtilesGenerales;

namespace Facturar.Utilidades
{
    public static class UtilidadesUI
    {
        // Control del color original del DataGridView (BloqueoEdicionDgv)
        private static bool coloresOriginalesGuardados = false;

        private static Color colorOriginalFondo;
        private static Color colorOriginalEncabezado;
        private static Color colorOriginalEncabezadoSeleccionado;
        private static Color colorOriginalTextoEncabezado;
        private static Color colorOriginalFondoCeldas;
        private static Color colorOriginalFuenteCeldas;
        private static Color colorOriginalFondoCeldasSeleccionadas;
        private static Color colorOriginalFuenteCeldasSeleccionadas;


        /// <summary>
        /// Permite limpiar todos los TextBox de un formulario pasado por parametro
        /// </summary>
        /// <param name="contenedor"></param>
        public static void LimpiarTextBoxes(Control contenedor)
        {
            foreach(Control ctrl in contenedor.Controls)
            {
                if(ctrl is TextBox txt)
                {
                    // Limpiar el texto
                    txt.Text = "";
                }

                if(ctrl.HasChildren)
                {
                    LimpiarTextBoxes(ctrl);
                }
            }
        }

        /// <summary>
        /// Permite insertar las columnas en un grid segun el orden indicado
        /// Incluye en el encabezado el nombre del atributo que tenga la propiedad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dgw"></param>
        /// <param name="nombrePropiedad"></param>
        /// <param name="indice"></param>
        public static void InsertaColumnaDGV<T>(DataGridView dgw, string nombrePropiedad, int indice)
        {
            // Obtiene el nombre del atributo DisplayName (si existe)
            var displayName = typeof(T)
                .GetProperty(nombrePropiedad)?
                .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .Cast<DisplayNameAttribute>()
                .FirstOrDefault()?.DisplayName ?? nombrePropiedad;

            // Agrega la columna
            dgw.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nombrePropiedad,
                HeaderText = displayName,
                DisplayIndex = indice
            });
        }

        /// <summary>
        /// Habilita o bloquea todos los TextBox de un contenedor de forma recursiva
        /// </summary>
        /// <param name="contenedor">Control que contiene los TextBox</param>
        /// <param name="habilitar">true para habilitar, false para bloquear</param>
        /// <param name="limpiar">true para que ademas de habilitar se borre el contenido (en alta); defecto = false</param>
        public static void HabilitarTextBoxes(Control contenedor, bool habilitar)
        {
            TextBox primerCampo = null;
            foreach(Control ctrl in contenedor.Controls)
            {
                if(ctrl is TextBox txt)
                {
                    txt.Enabled = habilitar;

                    // Identificar el primer campo por Tag
                    if(txt.Tag?.ToString() == "primerCampo" && primerCampo == null)
                    {
                        primerCampo = txt;
                    }
                }

                else if(ctrl is DateTimePicker dt)
                {
                    dt.Enabled = habilitar;
                }

                // Recursión para controles hijos
                if(ctrl.HasChildren)
                {
                    HabilitarTextBoxes(ctrl, habilitar);
                }
            }


            // Poner foco en el primer campo si se habilita
            if(habilitar && primerCampo != null)
            {
                primerCampo.Focus();
            }
        }

        /// <summary>
        /// Permite aplicar un efecto de bloqueo de edicion en un DataGridView
        /// </summary>
        /// <param name="_grid"></param>
        /// <param name="bloquear"></param>
        public static void BloqueoEdicionDgv(DataGridView _grid, bool bloquear)
        {
            var dgv = _grid;
            Color grisClaro = Color.FromArgb(230, 230, 230);
            Color grisOscuro = Color.FromArgb(200, 200, 200);

            // Guardar los colores originales solo la primera vez
            if(coloresOriginalesGuardados == false)
            {
                // Guardamos los colores originales
                colorOriginalFondo = dgv.BackgroundColor;
                colorOriginalEncabezado = dgv.ColumnHeadersDefaultCellStyle.BackColor;
                colorOriginalEncabezadoSeleccionado = dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor;
                colorOriginalTextoEncabezado = dgv.ColumnHeadersDefaultCellStyle.ForeColor;
                colorOriginalFondoCeldas = dgv.DefaultCellStyle.BackColor;
                colorOriginalFuenteCeldas = dgv.DefaultCellStyle.ForeColor;
                colorOriginalFondoCeldasSeleccionadas = dgv.DefaultCellStyle.SelectionBackColor;
                colorOriginalFuenteCeldasSeleccionadas = dgv.DefaultCellStyle.SelectionForeColor;

                // Controla que no se vuelva a guardar
                coloresOriginalesGuardados = true;
            }

            if(bloquear)
            {
                // Efecto "deshabilitado": tonos grises
                dgv.BackgroundColor = grisClaro;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = grisOscuro;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.DarkSlateGray;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = grisOscuro;
                dgv.DefaultCellStyle.BackColor = grisClaro;
                dgv.DefaultCellStyle.ForeColor = Color.DarkGray;
                dgv.DefaultCellStyle.SelectionBackColor = grisClaro;
                dgv.DefaultCellStyle.SelectionForeColor = Color.DarkGray;
                dgv.EnableHeadersVisualStyles = false; // Necesario para que se apliquen los colores
                dgv.Enabled = false;
            }
            else
            {
                // Restaurar colores originales
                dgv.BackgroundColor = colorOriginalFondo;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = colorOriginalEncabezado;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = colorOriginalTextoEncabezado;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = colorOriginalEncabezadoSeleccionado;
                dgv.DefaultCellStyle.BackColor = colorOriginalFondoCeldas;
                dgv.DefaultCellStyle.ForeColor = colorOriginalFuenteCeldas;
                dgv.DefaultCellStyle.SelectionBackColor = colorOriginalFondoCeldasSeleccionadas;
                dgv.DefaultCellStyle.SelectionForeColor = colorOriginalFuenteCeldasSeleccionadas;
                dgv.Enabled = true;

                coloresOriginalesGuardados = false; // Permite guardar de nuevo si se vuelve a bloquear
            }

            dgv.Refresh();
        }

        public static void RestablecerPaneles<TGestor, TEntidad>(DataGridView grid, bool bloquear, TGestor gestor) where TGestor : IRepositorioBase<TEntidad>
        {
            // Quita el efecto de bloqueo de edicion
            BloqueoEdicionDgv(_grid: grid, bloquear: bloquear);

            // Habilita el grid de empresas
            grid.Enabled = true;
        }

        // Devuelve el valor de una propiedad de un objeto por su nombre
        public static object GetPropValue(object obj, string nombreColumna)
        {
            return obj.GetType().GetProperty(nombreColumna).GetValue(obj, null);
        }

        // Permite validar textoBox con importes
        public static void ValidarImporte(TextBox txt, KeyPressEventArgs e)
        {
            if(char.IsControl(e.KeyChar))
            {
                return;
            }

            if(e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            if(!char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }
            else if(e.KeyChar == ',' && txt.Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        // Permite formatear textBox de importes para que muestren siempre 2 decimales
        public static void FormatearImporte(TextBox txt)
        {
            if(decimal.TryParse(txt.Text, out decimal valor))
            {
                // Formatea con dos decimales y coma como separador decimal
                txt.Text = valor.ToString("N2");
            }
        }
    }
}
