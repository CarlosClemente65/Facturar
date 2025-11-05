using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioRevisiones : IRepositorioBase<RevisionContrato>
    {
        IEnumerable<RevisionContrato> ListarPorContrato(int idContrato);
    }
}
