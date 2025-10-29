using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;
using Facturar.Interfaces;
using Utiles = Facturar.Utilidades.UtilesGenerales;



namespace Facturar.Servicios
{
    public class GestorFacturas : IRepositorioFacturas
    {
        //Constructor privado para evitar instanciación externa
        public GestorFacturas()
        {

        }

        public bool Actualizar(Factura entidad, bool esBaja = false)
        {
            throw new NotImplementedException();
        }

        public bool Agregar(Factura entidad)
        {
            throw new NotImplementedException();
        }

        public bool Baja(string nif, DateTime? fechaBaja)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(string nif)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Factura> ListarFacturasPorCliente(int? clienteId = null, string clienteNif = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contrato> ListarFacturasPorEmpresa(int? empresaId = null, string empresaNif = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contrato> ListarFacturasPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contrato> ListarFacturasPorSerie(string serieFactura)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Factura> ListarTodos(bool? activo = null)
        {
            // Crea una lista de facturas
            var listaFacturas = new List<Factura>();

            string sqlFacturas = "SELECT * " + "FROM Facturas ";

            // Carga una tabla con todas las facturas
            DataTable tabla = GestorDatos.EjecutarConsulta(sqlFacturas);

            // Va añadiendo cada empresa a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var factura = Utilidades.MapeadorDatos.MapearFila<Factura>(fila);

                // Carga la empresa emisora
                factura.Empresa = new GestorEmpresas().ObtenerPorId(factura.IdEmpresa);
                factura.Cliente = new GestorClientes().ObtenerPorId(factura.IdCliente);

                listaFacturas.Add(factura);
            }
            return listaFacturas;
        }

        public Factura ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Factura ObtenerPorNIF(string nif)
        {
            throw new NotImplementedException();
        }
    }
}
