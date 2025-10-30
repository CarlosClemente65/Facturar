using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Servicios;
using static Facturar.Entidades.Cliente;


namespace Facturar.Entidades
{
    public class Local
    {
        [DisplayName("Nº Reg.")]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }   // Id de la empresa a la que pertenece el local
        public Empresa Empresa { get; set; }   // Objeto empresa emisora

        [DisplayName("NIF empresa")]
        public string NIFEmpresa => Empresa?.NIF ?? string.Empty; // NIF de la empresa emisora

        [DisplayName("Nombre empresa")]
        public string NombreEmpresa => Empresa?.Nombre ?? string.Empty; // Nombre de la empresa emisora
        public string Descripcion { get; set; } // Descripcion a incluir en la factura del local
        public string Direccion { get; set; }

        [DisplayName("Codigo postal")]
        public string CodigoPostal { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }

        [DisplayName("Importe mensual alquiler")]
        public decimal ImporteAlquiler { get; set; } = 0m;// Importe mensual actual del alquiler (se establece a cero inicialmente y se atualizara con los contratos
        public string Observaciones { get; set; } // Notas del local

        [DisplayName("Fecha alta")]
        public DateTime FechaAlta { get; set; }

        [DisplayName("Fecha baja")]
        public DateTime? FechaBaja { get; set; } // Nullable para permitir que no tenga fecha de baja

        [Browsable(false)] // Evita mostrarlo en el grid
        public bool Activo => !FechaBaja.HasValue || FechaBaja.Value.Date > DateTime.Today; // Indica si la entidad está activa (sin fecha de baja o con fecha de baja en el futuro)


        // Constructor por defecto
        public Local()
        {

        }

        // Constructor para crear una copia de un cliente existente
        public Local(Local copiaLocal)
        {
            Id = copiaLocal.Id;
            IdEmpresa = copiaLocal.IdEmpresa;
            Empresa = copiaLocal.Empresa;
            Descripcion = copiaLocal.Descripcion;
            Direccion = copiaLocal.Direccion;
            CodigoPostal = copiaLocal.CodigoPostal;
            Poblacion = copiaLocal.Poblacion;
            Provincia = copiaLocal.Provincia;
            ImporteAlquiler = copiaLocal.ImporteAlquiler;
            Observaciones = copiaLocal.Observaciones;
            FechaAlta = copiaLocal.FechaAlta;
            FechaBaja = copiaLocal.FechaBaja;
        }


        public void EstablecerFechaBaja(DateTime? fechaBaja)
        {
            if(fechaBaja.HasValue && fechaBaja.Value.Date < FechaAlta.Date)
            {
                throw new ArgumentException("La fecha de baja no puede ser anterior a la fecha de alta.");
            }

            FechaBaja = fechaBaja?.Date;
        }

        public void ValidarPropiedadesObjeto()
        {
            // Al agregar un local, si no se pasa una fecha de alta se le pone la actual, pero dejo el metodo por coherencia con el resto
            if(FechaAlta == default(DateTime))
            {
                throw new ArgumentException("La fecha de alta es obligatoria.");
            }

            if(FechaBaja.HasValue && FechaBaja.Value.Date < FechaAlta.Date)
            {
                throw new ArgumentException("La fecha de baja no puede ser anterior a la fecha de alta.");
            }

        }
    }

    public class ImporteAlquiler
    {
        public DateTime FechaRevision { get; set; } // Fecha en que se revisa el importe
        public decimal ImporteAnterior { get; set; } // Importe antes de la revisión
        public decimal PorcentajeRevision { get; set; } // Porcentaje de revisión aplicado
        public decimal ImporteRevisado { get; set; } // Importe después de la revisión
    }
}
