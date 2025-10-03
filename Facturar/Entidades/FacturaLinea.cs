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
        public string Descripcion { get; set; } // ejemplo: "Alquiler Local X", "Gastos comunidad"
        public decimal Cantidad { get; set; }   // normalmente 1 para alquiler mensual, pero puede variar
        public decimal PrecioUnitario { get; set; } // Importe por unidad
        public decimal IVA { get; set; } = 21.00M;        // Porcentaje de IVA
        public decimal Subtotal => Cantidad * PrecioUnitario; // subtotal sin IVA
        public decimal CuotaIVA => Subtotal * IVA / 100; // Importe del IVA de la linea
        public decimal TotalLinea => Subtotal + CuotaIVA;  // total con IVA
    }
}
