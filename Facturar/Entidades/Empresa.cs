using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class Empresa : EntidadBase
    {
        //Lista de locales y clientes asociados a la empresa
        public List<Local> Locales { get; set; } = new List<Local>();
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();

        //Configuracion para la emision de facturas
        public string SerieFactura { get; set; }
        public int NumeroFacturaActual { get; set; }

    }
}
