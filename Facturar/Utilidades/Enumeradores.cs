using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Utilidades
{
    public static class Enumeradores
    {
        public enum TipoProceso
        {
            Ninguno,
            Alta,
            Baja,
            Edicion,
            Eliminacion
        }

        public enum TipoEntidad
        {
            Ninguno,
            Empresa,
            Cliente,
            Local,
            Contrato,
            Factura,
            Configurar
        }
    }
}
