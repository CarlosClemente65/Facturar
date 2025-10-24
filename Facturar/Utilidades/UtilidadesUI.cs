using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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

        /// <summary>
        /// Habilita o bloquea todos los TextBox de un contenedor de forma recursiva
        /// </summary>
        /// <param name="contenedor">Control que contiene los TextBox</param>
        /// <param name="habilitar">true para habilitar, false para bloquear</param>
        /// <param name="limpiar">true para que ademas de habilitar se borre el contenido (en alta); defecto = false</param>
        public static void HabilitarTextBoxes(Control contenedor, bool habilitar)
        {
            TextBox primerCampo = null;
            foreach (Control ctrl in contenedor.Controls)
            {
                if (ctrl is TextBox txt)
                {
                    txt.Enabled = habilitar;

                    // Identificar el primer campo por Tag
                    if(txt.Tag?.ToString() == "primerCampo" && primerCampo == null)
                    {
                        primerCampo = txt;
                    }
                }

                // Recursión para controles hijos
                if (ctrl.HasChildren)
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
    }
}
