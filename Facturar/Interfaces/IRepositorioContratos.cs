using System;
using System.Collections.Generic;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioContratos : IRepositorioBase<Contrato>
    {
        IEnumerable<Contrato> ListarContratos(bool activos = true);
        IEnumerable<Contrato> ListarContratosPorCliente(int clienteId);
        IEnumerable<Contrato> ListarContratosPorEmpresa(int empresaId);
        IEnumerable<Contrato> ListarContratosPorLocal(int localId);
        IEnumerable<Contrato> ListarContratosPorFecha(DateTime fechaInicio, DateTime fechaFin);
        Contrato ObtenerContratoActivoPorLocal(int localId);
    }
}
