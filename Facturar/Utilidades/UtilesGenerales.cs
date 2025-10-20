using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Utilidades
{
    public static class UtilesGenerales
    {
        // Clase para añadir utilidades que luego puedan usarse en el resto de la aplicacion

        /// <summary>
        /// Convierte una fecha en formato texto 'dd/MM/yyyy' a un DateTime valido o un null
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>Fecha formateada a DateTime o null</returns>
        public static DateTime? ConvertirFecha(string fecha)
        {
            if(string.IsNullOrWhiteSpace(fecha))
            {
                return null;
            }
            if(DateTime.TryParseExact(fecha, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime fechaConvertida))
            {
                return fechaConvertida.Date;
            }
            return null;
        }

        /// <summary>
        /// Convierte una fecha en formato DateTime a una cadena con formato 'dd/MM/yyyy'
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>Cadena con formato 'dd/MM/yyyy'</returns>
        public static string FormatearFecha(DateTime? fecha)
        {
            return fecha?.ToString("dd/MM/yyyy") ?? string.Empty;
        }


        /// <summary>
        /// Asignacion de fechas validas
        /// Si la fecha es nula o el valor por defecto, se devuelve la fehca actual
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>La fecha pasada o la fecha actual</returns>
        public static DateTime ValidarFecha (DateTime? fecha)
        {
            return (fecha.HasValue && fecha.Value != default(DateTime)) ? fecha.Value : DateTime.Today;
        }
        
    }
}
