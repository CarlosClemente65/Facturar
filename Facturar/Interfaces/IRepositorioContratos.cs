using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioContratos
    {
        void AgregarContrato(Contrato contrato);
        void ActualizarContrato(Contrato contrato);
        void EliminarContrato(int contratoId);
        Contrato ObtenerContrato(int contratoId);

        IEnumerable<Contrato> ListarContratos(bool activos = true);
        IEnumerable<Contrato> ListarContratosPorCliente(int clienteId);
        IEnumerable<Contrato> ListarContratosPorEmpresa(int empresaId);
        IEnumerable<Contrato> ListarContratosPorLocal(int localId);
    }
}
