using System.Collections.Generic;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioClientes : IRepositorioBase<Cliente>
    {
        IEnumerable<Cliente> ListarClientesActivos();
    }
}
