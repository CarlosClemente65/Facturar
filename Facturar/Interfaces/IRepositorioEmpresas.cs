using System.Collections.Generic;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioEmpresas : IRepositorioBase<Empresa>
    {
        IEnumerable<Empresa> ListarLocalesEmpresa(bool activas = true);

    }
}
