using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Facturar.Entidades
{
    public class Factura
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }   // Id empresa emisora
        public Empresa Empresa { get; set; }   // Objeto empresa emisora

        [DisplayName("NIF empresa")]
        public string NIFEmpresa => Empresa?.NIF ?? string.Empty; // NIF de la empresa emisora

        [DisplayName("Nombre empresa")]
        public string NombreEmpresa => Empresa?.Nombre ?? string.Empty; // Nombre de la empresa emisora

        public int IdCliente { get; set; }    // Id cliente receptor
        public Cliente Cliente { get; set; }   // Objeto cliente receptor

        [DisplayName("NIF cliente")]
        public string NIFCliente => Cliente?.NIF ?? string.Empty; // NIF del cliente receptor

        [DisplayName("Nombre cliente")]
        public string NombreCliente => Cliente?.Nombre ?? string.Empty; // Nombre del cliente receptor

        [DisplayName("Fecha")]
        public DateTime FechaFactura { get; set; }    // fecha de emisión

        [DisplayName("Serie")]
        public string SerieFactura { get; set; } // ej. "FAC-2025"

        [DisplayName("Número")]
        public string NumeroFactura { get; set; } // ej. "00001"

        public List<FacturaLinea> Lineas { get; set; } = new List<FacturaLinea>();

        
        // Totales calculados automáticamente
        [DisplayName("Total base")]
        public decimal TotalBase { get; set; }

        [DisplayName("Total IVA")]
        public decimal TotalIVA { get; set; }

        [DisplayName("Total IRPF")]
        public decimal TotalIRPF { get; set; }

        [DisplayName("Total factura")]
        public decimal TotalFactura { get; set; }

        public string Observaciones { get; set; } // notas internas o para el cliente
    }
}
