using System.Collections.Generic;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioLocales: IRepositorioBase<Local>
    {
        IEnumerable<Local> ListarLocalesPorEmpresa(int empresaId, bool? activos = null);
    }
}
