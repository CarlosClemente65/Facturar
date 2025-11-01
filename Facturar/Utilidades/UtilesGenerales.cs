using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            string[] formatosValidos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };
            if(string.IsNullOrWhiteSpace(fecha))
            {
                return null;
            }
            if(DateTime.TryParseExact(
                fecha, 
                formatosValidos, 
                System.Globalization.CultureInfo.InvariantCulture, 
                System.Globalization.DateTimeStyles.None, 
                out DateTime fechaConvertida)
                )
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
        /// Asignacion de fechas validas pasadas como DateTime
        /// Si la fecha es nula o el valor por defecto, se devuelve la fecha actual
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>La fecha pasada o la fecha actual</returns>
        public static DateTime ValidarFecha(DateTime? fecha)
        {
            return (fecha.HasValue && fecha.Value != default(DateTime)) ? fecha.Value : DateTime.Today;
        }

        /// <summary>
        /// Asignacion de fechas validas pasadas como string
        /// Si la fecha es nula o el valor por defecto, se devuelve la fecha actual
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>La fecha pasada o la fecha actual</returns>
        public static DateTime ValidarFecha(string fecha)
        {
            if (DateTime.TryParseExact(fecha,"dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }
            return DateTime.Today;
        }


        /// <summary>
        /// Permite registar un log de errores o de actividad 
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        public static void RegistrarLog(string mensaje, tipoLog tipo)
        {
            string carpeta = Path.Combine(Configuracion.CarpetaBase, "logs");

            // Crea la carpeta si no existe
            if(!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            string ruta = string.Empty;

            switch(tipo)
            {
                case tipoLog.Actividad:
                    ruta = Path.Combine(carpeta, "logActividad.txt");
                    break;

                default:
                    ruta = Path.Combine(carpeta, "logErrores.txt");
                    break;
            }

            // Graba el mensaje en el registro de logs
            StringBuilder texto = new StringBuilder();
            int largoLinea = 50; // Largo de linea de separacion entre mensajes
            texto.AppendLine(new string('_', largoLinea));
            texto.AppendLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} - {mensaje}\n");

            // Graba la linea al final del fichero
            File.AppendAllText(ruta, texto.ToString());
        }


        // Tipos de log permitidos
        public enum tipoLog
        {
            Error = 1,
            Actividad = 2
        }

        
    }
}
