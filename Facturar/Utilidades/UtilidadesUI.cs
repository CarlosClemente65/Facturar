using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturar.Utilidades
{
    public static class UtilidadesUI
    {
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
        public static void InsertaColumnaDGW<T>(DataGridView dgw, string nombrePropiedad, int indice)
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
    }
}
