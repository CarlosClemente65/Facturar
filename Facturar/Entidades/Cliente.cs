using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class Cliente :EntidadBase
    {
        // Datos de facturación
        public string FormaPago { get; set; } = FormasPago.Transferencia.ToString(); // ej. "Transferencia", "Domiciliación"
        public string IBAN { get; set; } // para domiciliación o transferencia
        public string Observaciones { get; set; } // notas internas sobre el cliente

        public enum FormasPago
        {
            Transferencia,
            Domiciliacion,
            Efectivo
        }

    }
}
