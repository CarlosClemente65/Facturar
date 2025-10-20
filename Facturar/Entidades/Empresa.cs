using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class Empresa : EntidadBase
    {
        //Configuracion para la emision de facturas
        public string SerieFactura { get; set; } = string.Empty;
        public int NumeroFacturaActual { get; set; } = 0;
        public bool Activo => !FechaBaja.HasValue || FechaBaja.Value.Date > DateTime.Today; // Indica si la entidad está activa (sin fecha de baja o con fecha de baja en el futuro)

    }
}
