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

        // Constructor por defecto
        public Empresa()
        {
        }

        // Constructor para crear una copia de una empresa existente
        public Empresa(Empresa copiaEntidad) : base(copiaEntidad)
        {
            SerieFactura = copiaEntidad.SerieFactura;
            NumeroFacturaActual = copiaEntidad.NumeroFacturaActual;

        }



    }
}
