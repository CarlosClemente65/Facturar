using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class Cliente :EntidadBase
    {
        // Datos de facturación
        public FormasPago FormaPago { get; set; } = FormasPago.Transferencia; // ej. "Transferencia", "Domiciliación"
        public string IBAN { get; set; } // para domiciliación o transferencia
        public string Observaciones { get; set; } // notas internas sobre el cliente

        // Permite rellenar el combobox del cliente seleccionado en los contratos.
        public string DatosCliente => $"{NIF} - {Nombre}";

        // Constructor por defecto
        public Cliente()
        {

        }

        // Constructor para crear una copia de un cliente existente
        public Cliente(Cliente copiaEntidad) : base(copiaEntidad)
        {
            FormaPago = copiaEntidad.FormaPago;
            IBAN = copiaEntidad.IBAN;
            Observaciones = copiaEntidad.Observaciones;
        }

        public enum FormasPago
        {
            Transferencia,
            Domiciliacion,
            Efectivo
        }

    }
}
