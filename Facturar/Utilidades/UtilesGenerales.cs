using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Utilidades
{
    public static class UtilesGenerales
    {
        // Clase para añadir utilidades que luego puedan usarse en el resto de la aplicacion

        public static DateTime ConvertirFecha(string fecha)
        {
            DateTime fechaConvertida = DateTime.ParseExact(fecha, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            return fechaConvertida.Date;
        }
    }
}
