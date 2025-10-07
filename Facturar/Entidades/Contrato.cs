using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class Contrato
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public int ClienteId { get; set; }
        public int Locald { get; set; }
        public decimal PrecioMensual { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }
        public List<RevisionContrato> HistorialRevisiones { get; set; } = new List<RevisionContrato>(); // Historial de revisiones del contrato
        public string Observaciones { get; set; } // Notas del contrato

        public void ActualizarPrecio(decimal nuevoPrecio, decimal porcentajeRevision, DateTime fechaRevision)
        {
            if(nuevoPrecio != PrecioMensual)
            {
                // Guardar el precio anterior en el historial
                HistorialRevisiones.Add(new RevisionContrato
                {
                    FechaRevision = fechaRevision,
                    PrecioAnterior = PrecioMensual,
                    PorcentajeRevision = porcentajeRevision,
                    PrecioRevisado = nuevoPrecio
                });

                // Actualizar el precio actual
                PrecioMensual = nuevoPrecio;
            }
        }
    }

    public class RevisionContrato
    {
        public DateTime FechaRevision { get; set; } // Fecha en que se revisa el contrato
        public decimal PrecioAnterior { get; set; } // Precio antes de la revisión
        public decimal PorcentajeRevision { get; set; } // Porcentaje de revisión aplicado
        public decimal PrecioRevisado { get; set; } // Precio después de la revisión
    }
}
