using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;

namespace Facturar.Interfaces
{
    public interface IRepositorioFacturas : IRepositorioBase<Factura>
    {
        IEnumerable<Factura> ListarFacturasPorCliente(int? clienteId = null, string clienteNif = null);
        IEnumerable<Contrato> ListarFacturasPorEmpresa(int? empresaId = null, string empresaNif = null);
        IEnumerable<Contrato> ListarFacturasPorSerie(string serieFactura);
        IEnumerable<Contrato> ListarFacturasPorFecha(DateTime fechaInicio, DateTime fechaFin);
    }
}
