using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Facturar.Entidades
{
    public class Empresa : EntidadBase
    {
        //Configuracion para la emision de facturas
        [DisplayName("Serie factura")]
        public string SerieFactura { get; set; } = string.Empty;
        [DisplayName("Ultima factura")]
        public int NumeroFacturaActual { get; set; } = 0;


    }
}
