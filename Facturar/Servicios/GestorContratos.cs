using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Facturar.Entidades;
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
                    new SQLiteParameter("@IdEmpresa", contrato.IdEmpresa),
                    new SQLiteParameter("@IdCliente", contrato.IdCliente),
                    new SQLiteParameter("@IdLocal", contrato.IdLocal),
                    new SQLiteParameter("@PrecioMensual", contrato.PrecioMensual),
                    new SQLiteParameter("@FechaInicio", contrato.FechaInicio.Date),
                    new SQLiteParameter("@FechaFin", contrato.FechaFin.HasValue ? (object) contrato.FechaFin.Value.Date: DBNull.Value),
                    new SQLiteParameter("@Observaciones", contrato.Observaciones)
                };

            // Ejecuta el comando y obtiene el numero de filas insertadas
            string sqlInsertarContrato =
                "INSERT INTO Contratos " +
                "(IdEmpresa, IdCliente, IdLocal, PrecioMensual, FechaInicio, FechaFin, Observaciones) " +
               "VALUES " +
               "(@IdEmpresa, @IdCliente, @IdLocal, @PrecioMensual, @FechaInicio, @FechaFin, @Observaciones)";

            var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarContrato, parametros));

            if(filasInsertadas <= 0)
            {
                throw new InvalidOperationException("No se ha podido insertar el contrato en la base de datos");
            }

            return true; // Indica que la inserción fue exitosa
        }


        /// <summary>
        /// Permite actualizar un contrato existente en la base de datos
        /// </summary>
        /// <param name="contrato"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public bool Actualizar(Contrato contrato, bool esBaja = false)
        {
            try
            {
                // Valida que el contrato tenga los campos obligatorios
                if(contrato.Id <= 0)
                {
                    throw new ArgumentException("El ID del contrato es obligatorio para la actualización.");
                }

                // Chequea si el contrato esta activo
                if(!esBaja && !contrato.Activo)
                {
                    throw new InvalidOperationException("El contrato no esta activo. No se puede modificar");
                }

                // Validacion de la fecha de baja para que no sea anterior a la de inicio
                if(contrato.FechaFin.HasValue && contrato.FechaFin.Value.Date < contrato.FechaInicio.Date)
                {
                    throw new ArgumentException("La fecha fin del contrato no puede ser anterior a la fecha de inicio.");
                }

                // Inserta el nuevo contrato en la base de datos
                // Nota: Solo se permite añadir el importe del precio mensual o las observaciones
                var parametros = new[]
                {
                    new SQLiteParameter("@Id", contrato.Id),
                    new SQLiteParameter("@PrecioMensual", contrato.PrecioMensual),
                    new SQLiteParameter("@Observaciones", contrato.Observaciones)
                };

                // Ejecuta el comando y obtiene el numero de filas actualizadas
                string sqlActualizarContrato = "UPDATE Contratos SET PrecioMensual = @PrecioMensual, Observaciones = @Observaciones WHERE Id = @Id";

                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlActualizarContrato, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se ha podido actualizar el contrato en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException("Error al actualizar el contrato en la base de datos.", ex);
            }
        }

        /// <summary>
        /// Metodo no implementado; usar Baja(int contratoId, DateTime? fechaBaja) en su lugar
        /// </summary>
        /// <param name="nif"></param>
        /// <param name="fechaBaja"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Baja(string nif, DateTime? fechaBaja = null)
        {
            // Este metodo no se implementa porque los contratos se identifican por Id y no por NIF
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite dar de baja un contrato estableciendo su fecha de fin
        /// </summary>
        /// <param name="idCcontrato"></param>
        /// <param name="fechaBaja"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Baja(int idCcontrato, DateTime? fechaBaja = null)
        {
            // Verifica que exista el contrato
            var contrato = ObtenerPorId(idCcontrato);
            if(contrato == null)
            {
                throw new InvalidOperationException("El contrato no existe en la base de datos.");
            }

            //Evitar dar de baja un contrato ya dado de baja
            if(!contrato.Activo)
            {
                throw new InvalidOperationException("El contrato ya está dado de baja.");
            }

            try
            {
                // Graba la fecha fin en el contrato 
                contrato.EstablecerFechaFin(fechaBaja ?? DateTime.Today);
                return Actualizar(contrato,esBaja: true);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al dar de baja el contrato: {ex.Message}", ex);
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
        /// <param name="idContrato"></param>
        /// <returns>True si se ha podido eliminar el contrato</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Eliminar(int idContrato)
        {
            // Verifica que exista el contrato
            var contrato = ObtenerPorId(idContrato);
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
                    throw new InvalidOperationException("No se ha podido eliminar el contrato en la base de datos");
                }
                return true; // Indica que la eliminacion fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar el contrato de la base de datos: {ex.Message}", ex);
            }
        }


        public bool AgregarRevisionContrato(RevisionContrato nuevaRevision)
        {
            var contrato = ObtenerPorId(nuevaRevision.IdContrato);
            if(contrato == null)
            {
                throw new InvalidOperationException("El contrato no existe en la base de datos");
            }

            if(!contrato.Activo)
            {
                throw new InvalidOperationException("El contrato no esta activo. No se puede agregar una revision");
            }

            // Valida los campos de la clase
            nuevaRevision.ValidarPropiedadesRevision();

            // Valida que la fecha de revision no sea anterior a la fecha del contrato
            if(nuevaRevision.FechaRevision <= contrato.FechaInicio)
            {
                throw new InvalidOperationException("La fecha de revision es anterior a la fecha del contrato");
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

            // Validar que la empresa exista en la base de datos y este activa
            var gestorEmpresas = new GestorEmpresas();
            var empresaExistente = gestorEmpresas.ObtenerPorId(contrato.IdEmpresa);
            if(empresaExistente == null || !empresaExistente.Activo)
            {
                throw new InvalidOperationException("La empresa no existe en la base de datos o esta inactiva.");
            }


            // Validar que el cliente exista en la base de datos y este activo
            var gestorClientes = new GestorClientes();
            var clienteExistente = gestorClientes.ObtenerPorId(contrato.IdCliente);
            if(clienteExistente == null || !clienteExistente.Activo)
            {
                throw new ArgumentException("El cliente del contrato no existe en la base de datos o esta inactivo.");
            }

            // Validar que el local exista en la base de datos y este activo
            var gestorLocales = new GestorLocales();
            var localExistente = gestorLocales.ObtenerPorId(contrato.IdLocal);
            if(localExistente == null || !localExistente.Activo)
            {
                throw new InvalidOperationException("El local del contrato no existe en la base de datos o esta inactivo.");
            }

            // Valida que el local pertenezca a la empresa
            if(localExistente.IdEmpresa != contrato.IdEmpresa)
            {
                throw new InvalidOperationException("La empresa asignada no tiene ese local");
            }

            // Validar fechas contrato e importe
            contrato.ValidarPropiedadesContrato();

            // Si hay algun contrato, se comprueba que no haya uno activo (solo puede haber un contrato activo)
            var contratosLocal = ListarContratosPorLocal(idLocal: contrato.IdLocal, activos: true);
            if(contratosLocal.Any()) // Si hay algun contrato
            {
                var contratoActivo = ObtenerContratoActivoPorLocal(IdLocal: contrato.Id);
                if(contratoActivo != null)
                {
                    throw new InvalidOperationException(
                        $"El local ya tiene un contrato activo. Debe dar de baja el contrato anterior antes de crear uno nuevo");
                }
            }

        }


        /// <summary>
        /// Obtiene todos los contratos, con opcion de filtrar por activos o inactivos
        /// </summary>
        /// <param name="activos"></param>
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
        /// <param name="nifCliente"></param>
        /// <param name="idCliente"></param>
        /// <param name="activos"></param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Contrato> ListarContratosPorCliente(int? idCliente = null, string nifCliente = null, bool? activos = null)
        {
            // Obtiene el Id del cliente a partir del NIF
            if(idCliente == null && !string.IsNullOrWhiteSpace(nifCliente))
            {
                var gestorClientes = new GestorClientes();
                var cliente = gestorClientes.ObtenerPorNIF(nifCliente);
                if(cliente == null)
                {
                    return Enumerable.Empty<Contrato>();
                }
                idCliente = cliente.Id;
            }

            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM Contratos WHERE IdCliente = @IdCliente";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value
                    ? " AND FechaFin IS NULL"
                    : " AND FechaFin IS NOT NULL";
            }

            var parametros = new[]
            {
                new SQLiteParameter("@IdCliente", idCliente)
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
        /// <param name="idEmpresa"></param>
        /// <param name="nifEmpresa"></param>
        /// <param name="activos"></param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Contrato> ListarContratosPorEmpresa(int? idEmpresa = null, string nifEmpresa = null, bool? activos = null)
        {
            // Obtiene el Id de la empresa a partir del NIF
            if(idEmpresa == null && !string.IsNullOrWhiteSpace(nifEmpresa))
            {
                var gestorEmpresa = new GestorEmpresas();
                var empresa = gestorEmpresa.ObtenerPorNIF(nifEmpresa);
                if(empresa == null)
                {
                    return Enumerable.Empty<Contrato>();
                }
                idEmpresa = empresa.Id;
            }

            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM Contratos WHERE IdEmpresa = @IdEmpresa";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value
                    ? " AND FechaFin IS NULL"
                    : " AND FechaFin IS NOT NULL";
            }

            var parametros = new[]
            {
                new SQLiteParameter("@IdEmpresa", idEmpresa)
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
        /// <param name="idLocal"></param>
        /// <param name="activos"></param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Contrato> ListarContratosPorLocal(int idLocal, bool? activos = null)
        {
            // Controla que el local existe
            var gestorLocales = new GestorLocales();
            var local = gestorLocales.ObtenerPorId(idLocal);
            if(local == null)
            {
                return Enumerable.Empty<Contrato>();
            }

            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM Contratos WHERE IdLocal = @IdLocal";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value
                    ? " AND FechaFin IS NULL"
                    : " AND FechaFin IS NOT NULL";
            }

            var parametros = new[]
            {
                new SQLiteParameter("@IdLocal", local.Id)
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
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
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
        /// Obtiene una relacion de las revisiones de un contrato
        /// </summary>
        /// <param name="IdContrato"></param>
        /// <returns></returns>
        public IEnumerable<RevisionContrato> ListarRevisionesContrato(int IdContrato)
        {
            // Valida que el contrato exista
            var contrato = ObtenerPorId(IdContrato);
            if(contrato == null)
            {
                throw new InvalidOperationException("El contrato no existe en la base de datos");
            }
            // Sql de consulta a la base de datos
            string sql = "SELECT * FROM RevisionesContrato WHERE IdContrato = @IdContrato";

            // Ordenar los contratros por fecha
            sql += " ORDER BY FechaRevision DESC";

            var parametros = new[]
            {
                new SQLiteParameter("@IdContrato", contrato.Id)
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
        /// Obtiene el contrato activo asociado a un local identificado por su Id
        /// </summary>
        /// <param name="IdLocal"></param>
        /// <returns>Objeto con el contrato activo</returns>
        public Contrato ObtenerContratoActivoPorLocal(int IdLocal)
        {
            // Valida que el local exista
            var contratoActivo = ObtenerPorId(IdLocal);
            if(contratoActivo == null)
            {
                return null;
            }

            // Consulta a la base de datos los contratos activos del local
            string sql = "SELECT * FROM Contratos WHERE IdLocal = @IdLocal AND FechaFin IS NULL LIMIT 1";
            var parametros = new[]
            {
               new SQLiteParameter("@IdLocal", IdLocal)
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
