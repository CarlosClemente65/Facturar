using System;
using System.Collections.Generic;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioContratos : IRepositorioBase<Contrato>
    {
        IEnumerable<Contrato> ListarContratosPorCliente(string nif, bool? activos = null);
        IEnumerable<Contrato> ListarContratosPorEmpresa(string nif, bool? activos = null);
        IEnumerable<Contrato> ListarContratosPorLocal(int localId, bool? activos = null);
        IEnumerable<Contrato> ListarContratosPorFecha(DateTime fechaInicio, DateTime fechaFin, bool? activos = null);
    }
}
