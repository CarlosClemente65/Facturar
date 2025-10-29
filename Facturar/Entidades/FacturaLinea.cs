using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class FacturaLinea
    {
        public int Id { get; set; }
        public int IdFactura { get; set; } // Id de la factura a la que pertenece esta linea
        public string Descripcion { get; set; } // ejemplo: "Alquiler Local X", "Gastos comunidad"
        public decimal Cantidad { get; set; }   // normalmente 1 para alquiler mensual, pero puede variar
        public decimal PrecioUnitario { get; set; } // Importe por unidad
        public decimal Subtotal => Cantidad * PrecioUnitario; // subtotal sin IVA
        public decimal TipoIVA { get; set; } = 21.00M;        // Porcentaje de IVA
        public decimal CuotaIVA => Subtotal * TipoIVA / 100; // Importe del IVA de la linea
        public decimal TipoIRPF { get; set; } = 19.00M;       // Porcentaje de retención de IRPF, si aplica
        public decimal CuotaIRPF => Subtotal * TipoIRPF / 100; // Importe de la retención de IRPF de la linea
        public decimal TotalLinea => Subtotal + CuotaIVA - CuotaIRPF;  // total con IVA
    }
}
