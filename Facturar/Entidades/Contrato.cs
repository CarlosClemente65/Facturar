using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Nº reg.")]
        public int Id { get; set; }
        public int IdEmpresa { get; set; }

        public Empresa Empresa { get; set; }   // Objeto empresa emisora

        [DisplayName("NIF empresa")]
        public string NIFEmpresa => Empresa?.NIF ?? string.Empty; // NIF de la empresa emisora

        [DisplayName("Nombre empresa")]
        public string NombreEmpresa => Empresa?.Nombre ?? string.Empty; // Nombre de la empresa emisora

        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }   // Objeto cliente receptor

        [DisplayName("NIF cliente")]
        public string NIFCliente => Cliente?.NIF ?? string.Empty; // NIF del cliente receptor

        [DisplayName("Nombre cliente")]
        public string NombreCliente => Cliente?.Nombre ?? string.Empty; // Nombre del cliente receptor

        public int IdLocal { get; set; }
        public Local Local { get; set; } // Objeto local contrato

        [DisplayName("Descripcion local")]
        public string DescripcionLocal => Local.Descripcion ?? string.Empty;


        [DisplayName("Precio mensual")]
        public decimal PrecioMensual { get; set; }

        [DisplayName("Fecha inicio")]
        public DateTime FechaInicio { get; set; }

        [DisplayName("Fecha fin")]
        public DateTime? FechaFin { get; set; }
        
        public string Observaciones { get; set; } // Notas del contrato

        [Browsable(false)] // Evita mostrarlo en el grid
        public bool Activo => !FechaFin.HasValue || FechaFin.Value.Date > DateTime.Today; // Contrato activo si la FechaFin esta no esta rellena o tiene una fecha posterior a hoy


        // Constructor por defecto
        public Contrato()
        {

        }

        // Constructor para crear una copia de un contrato existente
        public Contrato(Contrato copiaContrato)
        {
            Id = copiaContrato.Id;
            IdEmpresa = copiaContrato.IdEmpresa;
            IdCliente = copiaContrato.IdCliente;
            IdLocal = copiaContrato.IdLocal;
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
        public int IdContrato { get; set; }
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
