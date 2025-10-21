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
        public FormasPago FormaPago { get; set; } = FormasPago.Transferencia; // ej. "Transferencia", "Domiciliación"
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
