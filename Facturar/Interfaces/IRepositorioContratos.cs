using System;
using System.Collections.Generic;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioContratos : IRepositorioBase<Contrato>
    {
        IEnumerable<Contrato> ListarContratosPorCliente(int? clienteId = null, string clienteNif = null, bool? activos = null);
        IEnumerable<Contrato> ListarContratosPorEmpresa(int? empresaId = null, string empresaNif = null, bool? activos = null);
        IEnumerable<Contrato> ListarContratosPorLocal(int localId, bool? activos = null);
        IEnumerable<Contrato> ListarContratosPorFecha(DateTime fechaInicio, DateTime fechaFin, bool? activos = null);
    }
}
