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
        public string SerieFactura { get; set; }
        public int NumeroFacturaActual { get; set; }

    }
}
