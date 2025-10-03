using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Entidades
{
    public class Cliente :EntidadBase
    {
        // Lista de empresas asociadas al cliente
        public List<Empresa> Empresas { get; set; } = new List<Empresa>();
        public List<Local> Locales { get; set; } = new List<Local>(); // Locales asociados al cliente

        // Datos de facturación
        public string FormaPago { get; set; } // ej. "Transferencia", "Domiciliación"
        public string IBAN { get; set; } // para domiciliación o transferencia
        public string Observaciones { get; set; } // notas internas sobre el cliente
    }
}
