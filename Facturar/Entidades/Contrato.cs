using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Servicios;
using Utiles = Facturar.Utilidades.UtilesGenerales;

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
        public string Observaciones { get; set; } // Notas del contrato


        // Constructor por defecto
        public Contrato()
        {

        }

        // Constructor para crear una copia de un contrato existente
        public Contrato(Contrato copiaContrato)
        {
            Id = copiaContrato.Id;
            EmpresaId = copiaContrato.EmpresaId;
            ClienteId = copiaContrato.ClienteId;
            LocalId = copiaContrato.LocalId;
            PrecioMensual = copiaContrato.PrecioMensual;
            FechaInicio = copiaContrato.FechaInicio;
            FechaFin = copiaContrato.FechaFin;
            Observaciones = copiaContrato.Observaciones;

        }


        // Método seguro para establecer la fecha de fin (aplica validación)
        public void EstablecerFechaFin(DateTime? fechaFin)
        {
            if(fechaFin.HasValue && fechaFin.Value.Date < FechaInicio.Date)
            {
                throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");
            }

            FechaFin = fechaFin?.Date;
        }

        // Validaciones del contrato
        public void ValidarPropiedadesContrato()
        {
            if(PrecioMensual <= 0m)
            {
                throw new ArgumentException("El precio mensual debe ser mayor que cero.");
            }

            if(FechaInicio == default(DateTime))
            {
                throw new ArgumentException("La fecha de inicio es obligatoria.");
            }

            if(FechaFin.HasValue && FechaFin.Value.Date < FechaInicio.Date)
            {
                throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");
            }
        }
    }
    public class RevisionContrato
    {
        public int ContratoId { get; set; }
        public DateTime FechaRevision { get; set; } // Fecha en que se revisa el contrato
        public decimal? PrecioAnterior { get; set; } // Precio antes de la revisión
        public decimal? PorcentajeRevision { get; set; } // Porcentaje de revisión aplicado
        public decimal PrecioRevisado { get; set; } // Precio después de la revisión
        public string Observaciones { get; set; } // Observaciones de la revision

        public void ValidarPropiedadesRevision()
        {
            // Valida la fecha de revision y si no se ha puesto, le pone la actual
            if(FechaRevision == default(DateTime))
            {
                FechaRevision = Utiles.ValidarFecha(FechaRevision);
            }

            if (PrecioAnterior.HasValue && PrecioAnterior == 0)
            {
                throw new ArgumentException("El importe del precio anterior es obligatorio.");
            }

            if(PrecioRevisado == 0m)
            {
                throw new ArgumentException("El importe del precio revisado es obligatorio");
            }
        }
    }
}
