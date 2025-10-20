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
        public int LocalId { get; set; }
        public decimal PrecioMensual { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Activo => !FechaFin.HasValue || FechaFin.Value.Date > DateTime.Today; // Contrato activo si la FechaFin esta no esta rellena o tiene una fecha posterior a hoy
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

        // Método seguro para establecer la fecha de fin (aplica validación)
        public void EstablecerFechaFin(DateTime? fechaFin)
        {
            if(fechaFin.HasValue && fechaFin.Value.Date < FechaInicio.Date)
            {
                throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.", nameof(fechaFin));
            }

            FechaFin = fechaFin?.Date;
        }

        // Validaciones del contrato
        public void ValidarPropiedadesObjeto()
        {
            if (PrecioMensual <= 0m)
            {
                throw new ArgumentException("El precio mensual debe ser mayor que cero.", nameof(PrecioMensual));
            }

            if(FechaInicio == default(DateTime))
            {
                throw new ArgumentException("La fecha de inicio es obligatoria.", nameof(FechaInicio));
            }

            if(FechaFin.HasValue && FechaFin.Value.Date < FechaInicio.Date)
            {
                throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.", nameof(FechaFin));
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
