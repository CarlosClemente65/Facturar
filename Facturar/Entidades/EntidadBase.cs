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
        public bool Activo => !FechaBaja.HasValue || FechaBaja.Value.Date > DateTime.Today; // Indica si la entidad está activa (sin fecha de baja o con fecha de baja en el futuro)


        // Método seguro para establecer la fecha de fin (aplica validación)
        public void EstablecerFechaBaja(DateTime? fechaBaja)
        {
            if(fechaBaja.HasValue && fechaBaja.Value.Date < FechaAlta.Date)
            {
                throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");
            }

            FechaBaja = fechaBaja?.Date;
        }

    }
}
