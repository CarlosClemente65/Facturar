using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;

namespace Facturar.Servicios
{
    public class GestorLocales
    {
        private List<Local> _locales = new List<Local>();

        public IReadOnlyList<Local> ListarLocales()
        {
            return _locales.AsReadOnly();
        }
    }
}
