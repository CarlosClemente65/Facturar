using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Contracts;
using System.Linq;
using Facturar.Entidades;
using Facturar.Interfaces;
using Facturar.Utilidades;

namespace Facturar.Servicios
{
    internal class GestorRevisiones : IRepositorioRevisiones
    {
        /// <summary>
        /// Metodo no implementado; usar AgregarRevision
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Agregar(RevisionContrato entidad)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Agrega una revision de un contrato
        /// </summary>
        /// <param name="revision">Objeto con las propiedades de la revision del contrato</param>
        /// <returns>True si se ha podido insertar</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool AgregarRevision(RevisionContrato nuevaRevision, Contrato contrato)
        {
            // Valida los campos de la clase
            nuevaRevision.ValidarPropiedadesRevision();

            // Valida que la fecha de revision no sea anterior a la fecha del contrato
            if(nuevaRevision.FechaRevision <= contrato.FechaInicio)
            {
                throw new InvalidOperationException("La fecha de revision no puede ser anterior a la fecha del contrato");
            }

            // Valida que la fecha de revision no sea anterior a la ultima revision del contrato
            DateTime? ultimaRevision = ObtenerUltimaRevision(nuevaRevision.IdContrato);

            if(ultimaRevision.HasValue && nuevaRevision.FechaRevision <= ultimaRevision.Value)
            {
                throw new InvalidOperationException($"La fecha de revision del contrato({nuevaRevision.FechaRevision:dd/MM/yyyy}) no puede ser anterior o igual a la ultima revision ({ultimaRevision.Value:dd/MM/yyyy})");
            }

            try
            {
                // Asignacion de valores a parametros
                var parametros = new[] {
                    new SQLiteParameter("@IdContrato", nuevaRevision.IdContrato),
                    new SQLiteParameter("@FechaRevision", nuevaRevision.FechaRevision),
                    new SQLiteParameter("@PrecioAnterior", nuevaRevision.PrecioAnterior),
                    new SQLiteParameter("@PorcentajeRevision", nuevaRevision.PorcentajeRevision),
                    new SQLiteParameter("@PrecioRevisado", nuevaRevision.PrecioRevisado),
                    new SQLiteParameter("@Observaciones", nuevaRevision.Observaciones)
                };

                // Ejecuta el comando y obtiene el numero de filas insertadas
                string sql = "INSERT INTO RevisionesContrato " +
                    "(IdContrato, FechaRevision, PrecioAnterior, PorcentajeRevision, PrecioRevisado, Observaciones) " +
                   "VALUES (@IdContrato, @FechaRevision, @PrecioAnterior, @PorcentajeRevision, @PrecioRevisado, @Observaciones)";

                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sql, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se ha podido insertar la revision del contrato en la base de datos");
                }

                // Una vez insertada la revision, se actualiza el precio mensual en el contrato
                string sqlContrato = "UPDATE Contratos SET PrecioMensual = @NuevoPrecio WHERE Id = @IdContrato";
                var parametrosContrato = new[]
                {
                    new SQLiteParameter("@NuevoPrecio", nuevaRevision.PrecioRevisado),
                    new SQLiteParameter("@IdContrato", nuevaRevision.IdContrato)
                };

                var filasActualizadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlContrato, parametrosContrato));

                if(filasActualizadas <= 0)
                {
                    throw new InvalidOperationException("No se ha podido actualizar el precio mensual del contrato en la base de datos");
                }

                return true; // Indica que la inserción fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido insertar la revision del contrato: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Actualiza una revision del contrato
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="esBaja">Sin uso ya que las revisiones no tienen fecha de baja</param>
        /// <returns>True si se ha podido actualizar</returns>
        /// <exception cref="ApplicationException"></exception>
        public bool Actualizar(RevisionContrato revision, bool esBaja = false)
        {
            try
            {
                // Actualiza la revision del contrato en la base de datos
                var parametros = new[]
                {
                    // No se incluye el IdContrato porque no se puede cambiar y se pasa como parametro
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
                    throw new InvalidOperationException("No se ha podido actualizar la revision del contrato en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException("Error al actualizar la revision del contrato en la base de datos.", ex);
            }
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


        /// <summary>
        /// Metodo no implementado; usar Eliminar (int idRevision) en su lugar
        /// </summary>
        /// <param name="nif"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Eliminar(string nif)
        {
            // Solo se podra eliminar la ultima revision
            throw new NotImplementedException();
        }


        /// <summary>
        /// Elimina una revision de un contrato. Solo se permite eliminar la ultima
        /// </summary>
        /// <param name="revision">Objeto con la revision a eliminar</param>
        /// <returns>True si se ha podido eliminar</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Eliminar(RevisionContrato revision)
        {
            // Obtiene todas las revisiones del contrato de la revision a eliminar
            var revisiones = ListarPorContrato(revision.IdContrato);

            // Comprueba si existe una revision posterior
            bool hayRevisionPosterior = revisiones.Any(r =>
                r.FechaRevision > revision.FechaRevision);

            if (hayRevisionPosterior)
            {
                throw new InvalidOperationException("Solo se puede eliminar la ultima revision del contrato");
            }

            try
            {
                // Ejecuta el borrado y devuelve las filas afectadas
                var parametros = new[] { new SQLiteParameter("@IdRevision", revision.Id) };
                string sqlEliminarRevision = "DELETE FROM RevisionesContrato WHERE Id = @IdRevision";
                int filasActualizadas = GestorDatos.EjecutarComando(sqlEliminarRevision, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se ha podido eliminar la revision del contrato de la base de datos");
                }
                return true; // Indica que la eliminacion fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar la revision del contrato de la base de datos.\n{ex.Message}", ex);
            }
        }


        /// <summary>
        /// Metodo no implementado
        /// </summary>
        /// <param name="idContrato"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<RevisionContrato> ListarPorContrato(int idContrato)
        {
            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM RevisionesContrato WHERE IdContrato = @IdContrato";

            var parametros = new[]
            {
                new SQLiteParameter("@IdContrato", idContrato)
            };

            DataTable tabla = GestorDatos.EjecutarConsulta(sql, parametros);

            var listaRevisiones = new List<RevisionContrato>();
            foreach(DataRow fila in tabla.Rows)
            {
                listaRevisiones.Add(Utilidades.MapeadorDatos.MapearFila<RevisionContrato>(fila));
            }

            return listaRevisiones;
        }


        /// <summary>
        /// Obtiene todas las revisiones de los contratos
        /// </summary>
        /// <param name="activo"></param>
        /// <returns></returns>
        public IEnumerable<RevisionContrato> ListarTodos(bool? activo = null)
        {
            // Sql de la consulta
            string sql = "SELECT * FROM RevisionesContrato ";

            DataTable tabla = GestorDatos.EjecutarConsulta(sql);

            // Crea una lista de revisiones mapeando las propiedades en columnas
            var listaRevisiones = new List<RevisionContrato>();
            foreach(DataRow fila in tabla.Rows)
            {
                listaRevisiones.Add(Utilidades.MapeadorDatos.MapearFila<RevisionContrato>(fila));
            }

            return listaRevisiones;
        }


        /// <summary>
        /// Obtiene la revision del contrato identificado por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RevisionContrato ObtenerPorId(int id)
        {
            return GestorDatos.ObtenerDatosPorId<RevisionContrato>("RevisionContratos", id);
        }


        /// <summary>
        /// Metodo no implementado; las revisiones no se identifican por nif
        /// </summary>
        /// <param name="nif"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public RevisionContrato ObtenerPorNIF(string nif)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Obtiene la ultima revision de un contrato
        /// </summary>
        /// <param name="IdContrato"></param>
        /// <returns>Fecha de la ultima revision del contrato</returns>
        private DateTime? ObtenerUltimaRevision(int IdContrato)
        {
            // Valida que exista el contrato
            var gestor = ObtenerPorId(IdContrato);
            if(gestor == null)
            {
                throw new InvalidOperationException("El contrato no existe en la base de datos.");
            }

            // Prepara consulta a la base de datos
            string sql = "SELECT MAX(FechaRevision) FROM RevisionesContrato WHERE IdContrato = @IdContrato";
            var parametros = new[] {
                new SQLiteParameter("@IdContrato", IdContrato)
                };
            object resultado = GestorDatos.EjecutarComandoValorUnico(sql, parametros);

            if(resultado == null || resultado == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(resultado);
        }

        
    }
}
