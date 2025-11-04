using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Facturar.Entidades
{
    public class EntidadBase
    {
        // Constructor por defecto
        public EntidadBase()
        {

        }

        // Constructor para crear una copia de una entidad existente
        public EntidadBase(EntidadBase copiaEntidad)
        {
            Id = copiaEntidad.Id;
            NIF = copiaEntidad.NIF;
            Nombre = copiaEntidad.Nombre;
            Direccion = copiaEntidad.Direccion;
            CodigoPostal = copiaEntidad.CodigoPostal;
            Poblacion = copiaEntidad.Poblacion;
            Provincia = copiaEntidad.Provincia;
            Telefono = copiaEntidad.Telefono;
            Email = copiaEntidad.Email;
            PersonaContacto = copiaEntidad.PersonaContacto;
            FechaAlta = copiaEntidad.FechaAlta;
            FechaBaja = copiaEntidad.FechaBaja;
        }

        [DisplayName("Nº Reg.")]
        public int Id { get; set; }
        public string NIF { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        [DisplayName("Codigo postal")]
        public string CodigoPostal { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }
        public string Telefono { get; set; }

        [DisplayName("Correo electrónico")]
        public string Email { get; set; }

        [DisplayName("Persona contacto")]
        public string PersonaContacto { get; set; }

        [DisplayName("Fecha alta")]
        public DateTime FechaAlta { get; set; }

        [DisplayName("Fecha baja")]
        public DateTime? FechaBaja { get; set; } // Nullable para permitir que no tenga fecha de baja

        [Browsable(false)] // Evita mostrarlo en el grid
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
