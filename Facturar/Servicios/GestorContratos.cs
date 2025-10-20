using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Facturar.Entidades;
using Facturar.Infraestructura;
using Facturar.Interfaces;

namespace Facturar.Servicios
{
    public class GestorContratos : IRepositorioContratos
    {

        /// <summary>
        /// Inserta un nuevo contrato en la base de datos
        /// </summary>
        /// <param name="contrato"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Agregar(Contrato contrato)
        {
            // Valida que el contrato tenga los campos obligatorios
            ValidarContrato(contrato);

            // Inserta el nuevo contrato en la base de datos
            var parametros = new[]
            {
                    new SQLiteParameter("@EmpresaId", contrato.EmpresaId),
                    new SQLiteParameter("@ClienteId", contrato.ClienteId),
                    new SQLiteParameter("@LocalId", contrato.LocalId),
                    new SQLiteParameter("@PrecioMensual", contrato.PrecioMensual),
                    new SQLiteParameter("@FechaInicio", contrato.FechaInicio.Date),
                    new SQLiteParameter("@FechaFin", contrato.FechaFin.HasValue ? (object) contrato.FechaFin.Value.Date: DBNull.Value),
                    new SQLiteParameter("@Observaciones", contrato.Observaciones)
                };

            // Ejecuta el comando y obtiene el numero de filas insertadas
            string sqlInsertarContrato =
                "INSERT INTO Contratos " +
                "(EmpresaId, ClienteId, LocalId, PrecioMensual, FechaInicio, FechaFin, Observaciones) " +
               "VALUES " +
               "(@EmpresaId, @ClienteId, @LocalId, @PrecioMensual, @FechaInicio, @FechaFin, @Observaciones)";

            var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarContrato, parametros));

            if(filasInsertadas <= 0)
            {
                throw new InvalidOperationException("No se pudo insertar el contrato en la base de datos");
            }
            return true; // Indica que la inserción fue exitosa
        }


        /// <summary>
        /// Permite actualizar un contrato existente en la base de datos
        /// </summary>
        /// <param name="contrato"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public bool Actualizar(Contrato contrato)
        {
            try
            {
                // Valida que el contrato tenga los campos obligatorios
                if(contrato.Id <= 0)
                {
                    throw new ArgumentException("El ID del contrato es obligatorio para la actualización.");
                }

                // Valida que la fecha de fin no sea anterior a la de inicio
                if(contrato.FechaFin.HasValue && contrato.FechaFin.Value.Date < contrato.FechaInicio.Date)
                {
                    throw new ArgumentException("La fecha final no puede ser anterior a la de inicio.");
                }

                // Inserta el nuevo cliente en la base de datos
                // Nota: Solo se permite añadir la fecha de fin o modificar los comentarios
                var parametros = new[]
                {
                    new SQLiteParameter("@Id", contrato.Id),
                    new SQLiteParameter("@FechaFin", contrato.FechaFin.HasValue ? (object) contrato.FechaFin.Value.Date: DBNull.Value),
                    new SQLiteParameter("@Observaciones", contrato.Observaciones)
                };

                // Ejecuta el comando y obtiene el numero de filas actualizadas
                string sqlActualizarContrato = "UPDATE Contratos SET FechaFin= @FechaFin, Observaciones = @Observaciones WHERE Id = @Id";

                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlActualizarContrato, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se pudo actualizar el contrato en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException("Error al actualizar el contrato.", ex);
            }
        }

        /// <summary>
        /// Metodo no implementado; usar Baja(int contratoId, DateTime? fechaBaja) en su lugar
        /// </summary>
        /// <param name="nif"></param>
        /// <param name="fechaBaja"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Baja(string nif, DateTime? fechaBaja)
        {
            // Este metodo no se implementa porque los contratos se identifican por Id y no por NIF
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite dar de baja un contrato estableciendo su fecha de fin
        /// </summary>
        /// <param name="contratoId"></param>
        /// <param name="fechaBaja"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Baja(int contratoId, DateTime? fechaBaja = null)
        {
            // Verifica que exista el contrato
            var contrato = ObtenerPorId(contratoId);
            if(contrato == null)
            {
                throw new InvalidOperationException("El contrato no existe en la base de datos.");
            }

            //Evitar dar de baja un contrato ya dado de baja
            if(contrato.FechaFin.HasValue && contrato.FechaFin.Value.Date <= DateTime.Today)
            {
                throw new InvalidOperationException("El contrato ya está dado de baja.");
            }

            try
            {
                // Graba la fecha fin en el contrato 
                contrato.EstablecerFechaFin(fechaBaja ?? DateTime.Today);
                return Actualizar(contrato);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido dar de baja el contrato: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo no implementado; usar Eliminar(int contratoId) en su lugar
        /// </summary>
        /// <param name="nif"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Eliminar(string nif)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Permite eliminar un contrato de la base de datos
        /// </summary>
        /// <param name="contratoId"></param>
        /// <returns>True si se ha podido eliminar el contrato</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Eliminar(int contratoId)
        {
            // Verifica que exista el contrato
            var contrato = ObtenerPorId(contratoId);
            if(contrato == null)
            {
                throw new InvalidOperationException("El contrato no existe en la base de datos.");
            }

            try
            {
                // Ejecuta el borrado y devuelve las filas afectadas
                var parametros = new[] { new SQLiteParameter("@Id", contrato.Id) };
                string sqlEliminarContrato = "DELETE FROM Contratos WHERE Id = @Id";
                int filasActualizadas = GestorDatos.EjecutarComando(sqlEliminarContrato, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se encontro el contrato para borrar en la base de datos");
                }
                return true; // Indica que la eliminacion fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido eliminar el contrato: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todos los contratos, con opcion de filtrar por activos o inactivos
        /// </summary>
        /// <param name="activas"></param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Contrato> ListarTodos(bool? activos = null)
        {
            // Sql de la consulta
            string sql = "SELECT * FROM Contratos ";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value 
                    ? " WHERE FechaFin IS NULL" 
                    : " WHERE FechaFin IS NOT NULL";
            }

            DataTable tabla = GestorDatos.EjecutarConsulta(sql);

            // Crea una lista de contratos mapeando las propiedades en columnas
            var listaContratos = new List<Contrato>();
            foreach(DataRow fila in tabla.Rows)
            {
                listaContratos.Add(Utilidades.MapeadorDatos.MapearFila<Contrato>(fila));
            }

            return listaContratos;
        }

        /// <summary>
        /// Obtiene los contratos asociados a un cliente
        /// </summary>
        /// <param name="nif"></param>
        /// <param name="activos"></param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Contrato> ListarContratosPorCliente(int? clienteId = null, string clienteNif = null, bool? activos = null)
        {
            // Obtiene el Id del cliente a partir del NIF
            if(clienteId == null && !string.IsNullOrWhiteSpace(clienteNif))
            {
                var gestorClientes = new GestorClientes();
                var cliente = gestorClientes.ObtenerPorNIF(clienteNif);
                if(cliente == null)
                {
                    return Enumerable.Empty<Contrato>();
                }
                clienteId = cliente.Id;
            }

            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM Contratos WHERE ClienteId = @ClienteId";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value 
                    ? " AND FechaFin IS NULL" 
                    : " AND FechaFin IS NOT NULL";
            }

            var parametros = new[]
            {
                new SQLiteParameter("@ClienteId", clienteId)
            };

            DataTable tabla = GestorDatos.EjecutarConsulta(sql, parametros);

            var listaContratos = new List<Contrato>();
            foreach(DataRow fila in tabla.Rows)
            {
                listaContratos.Add(Utilidades.MapeadorDatos.MapearFila<Contrato>(fila));
            }

            return listaContratos;

        }


        /// <summary>
        /// Obtiene los contratos asociados a una empresa
        /// </summary>
        /// <param name="nif"></param>
        /// <param name="activos"></param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Contrato> ListarContratosPorEmpresa(int? empresaId = null, string empresaNif = null, bool? activos = null)
        {
            // Obtiene el Id de la empresa a partir del NIF
            var gestorEmpresas = new GestorEmpresas();
            var empresa = gestorEmpresas.ObtenerPorNIF(empresaNif);
            if(empresa == null)
            {
                return Enumerable.Empty<Contrato>();
            }

            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM Contratos WHERE EmpresaId = @EmpresaId";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value 
                    ? " AND FechaFin IS NULL" 
                    : " AND FechaFin IS NOT NULL";
            }

            var parametros = new[]
            {
                new SQLiteParameter("@EmpresaId", empresa.Id)
            };

            DataTable tabla = GestorDatos.EjecutarConsulta(sql, parametros);

            var listaContratos = new List<Contrato>();
            foreach(DataRow fila in tabla.Rows)
            {
                listaContratos.Add(Utilidades.MapeadorDatos.MapearFila<Contrato>(fila));
            }
            return listaContratos;
        }


        /// <summary>
        /// Obtiene los contratos asociados a un local
        /// </summary>
        /// <param name="localId"></param>
        /// <param name="activos"></param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Contrato> ListarContratosPorLocal(int localId, bool? activos = null)
        {
            // Controla que el local existe
            var gestorLocales = new GestorLocales();
            var local = gestorLocales.ObtenerPorId(localId);
            if(local == null)
            {
                return Enumerable.Empty<Contrato>();
            }

            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM Contratos WHERE LocalId = @LocalId";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value 
                    ? " AND FechaFin IS NULL" 
                    : " AND FechaFin IS NOT NULL";
            }

            var parametros = new[]
            {
                new SQLiteParameter("@LocalId", local.Id)
            };

            DataTable tabla = GestorDatos.EjecutarConsulta(sql, parametros);

            var listaContratos = new List<Contrato>();
            foreach(DataRow fila in tabla.Rows)
            {
                listaContratos.Add(Utilidades.MapeadorDatos.MapearFila<Contrato>(fila));
            }
            return listaContratos;
        }

        /// <summary>
        /// Obtiene los contratos dentro de un rango de fechas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="activos"></param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Contrato> ListarContratosPorFecha(DateTime fechaDesde, DateTime fechaHasta, bool? activos = null)
        {
            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM Contratos WHERE FechaInicio >= @fechaDesde AND FechaInicio <= @fechaHasta";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value 
                    ? " AND FechaFin IS NULL" 
                    : " AND FechaFin IS NOT NULL";
            }

            // Ordenar los contratros por fecha
            sql += " ORDER BY FechaInicio DESC";

            var parametros = new[]
            {
                new SQLiteParameter("@fechaDesde", fechaDesde),
                new SQLiteParameter("@fechaHasta", fechaHasta)
            };

            DataTable tabla = GestorDatos.EjecutarConsulta(sql, parametros);

            var listaContratos = new List<Contrato>();
            foreach(DataRow fila in tabla.Rows)
            {
                listaContratos.Add(Utilidades.MapeadorDatos.MapearFila<Contrato>(fila));
            }
            return listaContratos;
        }


        /// <summary>
        /// Obtiene el contrato activo asociado a un local identificado por su Id
        /// </summary>
        /// <param name="localId"></param>
        /// <returns>Objeto con el contrato activo</returns>
        public Contrato ObtenerContratoActivoPorLocal(int localId)
        {
            // Consulta a la base de datos
            string sql = "SELECT * FROM Contratos WHERE LocalId = @LocalId AND FechaFin IS NULL LIMIT 1";
            var parametros = new[]
            {
               new SQLiteParameter("@LocalId", localId)
            };

            // Almacena el resultado en una tabla
            DataTable tabla = GestorDatos.EjecutarConsulta(sql, parametros);
            if(tabla.Rows.Count == 0)
            {
                return null;
            }

            // Devuelve el resultado mapeado en la tabla.
            return Utilidades.MapeadorDatos.MapearFila<Contrato>(tabla.Rows[0]);
        }

        
        /// <summary>
        /// Obtiene el contrato identificado por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto con el contrato</returns>
        public Contrato ObtenerPorId(int id)
        {
            return GestorDatos.ObtenerDatosPorId<Contrato>("Contratos", id);
        }

        // Metodo no implementado; los contratos no se buscan por NIF
        public Contrato ObtenerPorNIF(string nif)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite validar que al agregar un contrato no sea nulo y que tenga empresa y local
        /// </summary>
        /// <param name="cliente"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void ValidarContrato(Contrato contrato)
        {
            // Evita agregar contratos nulos
            if(contrato == null)
            {
                throw new ArgumentNullException(nameof(contrato), "El contrato no puede ser nulo.");
            }

            // Validar que la empresa, el cliente y el local existan en la base de datos
            var gestorEmpresas = new GestorEmpresas();
            var empresaExistente = gestorEmpresas.ObtenerPorId(contrato.EmpresaId);
            if(empresaExistente == null)
            {
                throw new ArgumentException("La empresa especificada no existe.");
            }

            var gestorClientes = new GestorClientes();
            var clienteExistente = gestorClientes.ObtenerPorId(contrato.ClienteId);
            if(clienteExistente == null)
            {
                throw new ArgumentException("El cliente especificado no existe.");
            }

            var gestorLocales = new GestorLocales();
            var localExistente = gestorLocales.ObtenerPorId(contrato.LocalId);
            if(localExistente == null)
            {
                throw new ArgumentException("El local especificado no existe.");
            }

            // Validar fechas contrato e importe
            contrato.ValidarPropiedadesObjeto();

            // Comprueba que no hay un contrato activo (solo puede haber un contrato activo)
            var contratoActivo = ObtenerContratoActivoPorLocal(contrato.LocalId);
            if(contratoActivo != null)
            {
                throw new InvalidOperationException(
                    $"El local {contrato.LocalId} ya tiene un contrato activo (Id: {contratoActivo.Id}). Debe dar de baja el contrato anterior antes de crear uno nuevo");
            }

        }
    }
}
