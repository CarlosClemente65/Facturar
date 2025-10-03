using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class Factura
    {
        public int Id { get; set; }
        public Empresa Empresa { get; set; }   // empresa emisora
        public Cliente Cliente { get; set; }   // cliente receptor
        public DateTime Fecha { get; set; }    // fecha de emisión

        public string SerieFactura { get; set; } // ej. "FAC-2025"
        public string NumeroFactura { get; set; } // ej. "00001"

        public List<FacturaLinea> Lineas { get; set; } = new List<FacturaLinea>();

        // Totales calculados automáticamente
        public decimal TotalBase => Lineas.Sum(l => l.Subtotal);
        public decimal TotalIVA => Lineas.Sum(l => l.CuotaIVA);
        public decimal TotalFactura => Lineas.Sum(l => l.TotalLinea);

        public string Observaciones { get; set; } // notas internas o para el cliente
    }
}
