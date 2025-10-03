using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;

namespace Facturar.Infraestructura
{
    public class RepositorioDatos
    {
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
        public List<Empresa> Empresas { get; set; } = new List<Empresa>();
        public List<Local> Locales { get; set; } = new List<Local>();
    }
}
