using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class Local
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; } // Empresa a la que pertenece el local
        public string Descripcion { get; set; } // Descripcion a incluir en la factura del local
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }
        public decimal ImporteAlquiler { get; set; } // Importe mensual actual del alquiler
        public string Observaciones { get; set; } // Notas del local
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; } // Nullable para permitir que no tenga fecha de baja
        public bool Activo => !FechaBaja.HasValue || FechaBaja > DateTime.Now.Date; // Indica si la entidad está activa (sin fecha de baja o con fecha de baja en el futuro)
    }

    public class ImporteAlquiler
    {
        public DateTime FechaRevision { get; set; } // Fecha en que se revisa el importe
        public decimal ImporteAnterior { get; set; } // Importe antes de la revisión
        public decimal PorcentajeRevision { get; set; } // Porcentaje de revisión aplicado
        public decimal ImporteRevisado { get; set; } // Importe después de la revisión
    }
}
