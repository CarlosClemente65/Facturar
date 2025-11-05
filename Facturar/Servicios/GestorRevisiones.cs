using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;
using Facturar.Interfaces;

namespace Facturar.Servicios
{
    internal class GestorRevisiones : IRepositorioRevisiones
    {
        public bool Actualizar(RevisionContrato revision, bool esBaja = false)
        {
            try
            {
                // Actualiza la revision del contrato en la base de datos
                var parametros = new[]
                {
                    new SQLiteParameter("@Id", revision.Id),
                    new SQLiteParameter("@FechaRevision", revision.FechaRevision),
                    new SQLiteParameter("@PrecioAnterior", revision.PrecioAnterior),
                    new SQLiteParameter("@PorcentajeRevision", revision.PorcentajeRevision),
                    new SQLiteParameter("@PrecioRevisado", revision.PrecioRevisado),
                    new SQLiteParameter("@Observaciones", revision.Observaciones)
                };

                // Ejecuta el comando y obtiene el numero de filas actualizadas
                string sqlActualizarRevision = "UPDATE RevisionContrato SET FechaRevision = @FechaRevision, PrecioAnterior = @PrecioAnterior, PorcentajeRevision = @PorcentajeRevision, PrecioRevisado = @PrecioRevisado, Observaciones = @Observaciones WHERE Id = @Id";

                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlActualizarRevision, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se ha podido actualizar el contrato en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException("Error al actualizar la revision del contrato en la base de datos.", ex);
            }
        }

        /// <summary>
        /// Inserta una revision del contrato a la base de datos
        /// </summary>
        /// <param name="revision">Objeto con la revision</param>
        /// <returns>Verdadero si se ha podido insertar la revision</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Agregar(RevisionContrato revision)
        {
            // Inserta la revision del contrato en la base de datos
            var parametros = new[]
            {
                    new SQLiteParameter("@IdContrato", revision.IdContrato),
                    new SQLiteParameter("@FechaRevision", revision.FechaRevision),
                    new SQLiteParameter("@PrecioAnterior", revision.PrecioAnterior),
                    new SQLiteParameter("@PorcentajeRevision", revision.PorcentajeRevision),
                    new SQLiteParameter("@PrecioRevisado", revision.PrecioRevisado),
                    new SQLiteParameter("@IdContrato", revision.Observaciones)
                };

            // Ejecuta el comando y obtiene el numero de filas insertadas
            string sqlInsertarRevision =
                "INSERT INTO RevisionesContrato " +
                "(IdContrato, FechaRevision, PrecioAnterior, PorcentajeRevision, PrecioRevisado, Observaciones) " +
               "VALUES " +
               "(@IdContrato, @FechaRevision, @PrecioAnterior, @PorcentajeRevision, @PrecioREvisado, @Observaciones)";

            var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarRevision, parametros));

            if(filasInsertadas <= 0)
            {
                throw new InvalidOperationException("No se ha podido insertar la revision del contrato en la base de datos");
            }

            return true; // Indica que la inserción fue exitosa
        }


        /// <summary>
        /// Metodo no implementado; las revisiones no se pueden dar de baja
        /// </summary>
        /// <param name="nif"></param>
        /// <param name="fechaBaja"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Baja(string nif, DateTime? fechaBaja)
        {
            throw new NotImplementedException();
        }


        public bool Eliminar(string nif)
        {
            // TODO: decidir si se pueden eliminar las revisiones de los contratos
            throw new NotImplementedException();
        }

        public IEnumerable<RevisionContrato> ListarPorContrato(int idContrato)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RevisionContrato> ListarTodos(bool? activo = null)
        {
            throw new NotImplementedException();
        }

        public RevisionContrato ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public RevisionContrato ObtenerPorNIF(string nif)
        {
            throw new NotImplementedException();
        }
    }
}
