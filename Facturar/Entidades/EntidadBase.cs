using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class EntidadBase
    {
        public int Id { get; set; }
        public string NIF { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string PersonaContacto { get; set; }

        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; } // Nullable para permitir que no tenga fecha de baja
        public bool Activo => !FechaBaja.HasValue || FechaBaja > DateTime.Now; // Indica si la entidad está activa (sin fecha de baja o con fecha de baja en el futuro)


    }
}
