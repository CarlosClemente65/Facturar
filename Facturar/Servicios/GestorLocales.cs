using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Facturar.Entidades;
using Facturar.Interfaces;
using Utiles = Facturar.Utilidades.UtilesGenerales;



namespace Facturar.Servicios
{
    public class GestorLocales : IRepositorioLocales
    {
        //Constructor privado para evitar instanciación externa
        public GestorLocales()
        {
        }


        /// <summary>
        /// Inserta un nuevo local en la base de datos
        /// </summary>
        /// <param name="local"></param>
        /// <returns>Devuelve 'true' si se ha podido grabar en la BBDD</returns>
        /// <exception cref="ApplicationException"></exception>
        public bool Agregar(Local local)
        {
            try
            {
                // Valida que el local no sea nulo y se pase el Id de la empresa
                ValidarLocal(local);

                // Asigna la fecha de alta si no está establecida
                local.FechaAlta = Utiles.ValidarFecha(local.FechaAlta);

                // Inserta el nuevo local en la base de datos
                var parametros = new[]
                {
                    new SQLiteParameter("@EmpresaId", local.EmpresaId),
                    new SQLiteParameter("@Descripcion", local.Descripcion),
                    new SQLiteParameter("@Direccion", local.Direccion),
                    new SQLiteParameter("@CodigoPostal", local.CodigoPostal),
                    new SQLiteParameter("@Poblacion", local.Poblacion),
                    new SQLiteParameter("@Provincia", local.Provincia),
                    new SQLiteParameter("@ImporteAlquiler", local.ImporteAlquiler),
                    new SQLiteParameter("@Observaciones", local.Observaciones),
                    new SQLiteParameter("@FechaAlta", local.FechaAlta.Date),
                    new SQLiteParameter("@FechaBaja", local.FechaBaja != default(DateTime) ? (object)local.FechaBaja: DBNull.Value)
                };

                // Ejecuta el comando y obtiene el numero de filas insertadas
                string sqlInsertarLocal = "INSERT INTO Locales " +
                    "(EmpresaId, Descripcion, Direccion, CodigoPostal, Poblacion, Provincia, ImporteAlquiler, Observaciones, FechaAlta, FechaBaja) " +
                    "VALUES (@EmpresaId, @Descripcion, @Direccion, @CodigoPostal, @Poblacion, @Provincia, @ImporteAlquiler, @Observaciones, @FechaAlta, @FechaBaja)";

                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarLocal, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se pudo insertar el local en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException("Error al agregar el local.", ex);
            }
        }


        /// <summary>
        /// Permite actualizar los datos de un cliente en la BBDD
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns>True si se ha podido actualizar</returns>
        /// <exception cref="InvalidOperationException"></exception>

        public bool Actualizar(Local local)
        {
            try
            {
                // Valida que el local no sea nulo, y se pase el Id de la empresa
                ValidarLocal(local);

                // Nota: no se incluye la fecha de alta porque solo se graba en el alta, no se puede modificar en la actualizacion
                var parametros = new[]
                {
                    new SQLiteParameter("@Id", local.Id),
                    new SQLiteParameter("@EmpresaId", local.EmpresaId),
                    new SQLiteParameter("@Descripcion", local.Descripcion),
                    new SQLiteParameter("@Direccion", local.Direccion),
                    new SQLiteParameter("@CodigoPostal", local.CodigoPostal),
                    new SQLiteParameter("@Poblacion", local.Poblacion),
                    new SQLiteParameter("@Provincia", local.Provincia),
                    new SQLiteParameter("@ImporteAlquiler", local.ImporteAlquiler),
                    new SQLiteParameter("@Observaciones", local.Observaciones),
                    new SQLiteParameter("@FechaBaja", local.FechaBaja != default(DateTime) ? (object)local.FechaBaja: DBNull.Value)
                };

                // Ejecuta la actualizacion y devuelve las filas actualizadas
                string sqlActualizarLocal = "UPDATE Locales SET " +
                    "EmpresaId = @EmpresaId, " +
                    "Descripcion = @Descripcion, " +
                    "Direccion = @Direccion, " +
                    "CodigoPostal = @CodigoPostal, " +
                    "Poblacion = @Poblacion, " +
                    "Provincia = @Provincia, " +
                    "ImporteAlquiler = @ImporteAlquiler, " +
                    "Observaciones = @Observaciones, " +
                    "FechaBaja = @FechaBaja " +
                    "WHERE Id = @Id";

                int filasActualizadas = GestorDatos.EjecutarComando(sqlActualizarLocal, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se encontro el local para actualizar en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido actualizar el local: {ex.Message}", ex);
            }

        }


        /// <summary>
        /// Permite eliminar un local de la base de datos pasando el Id del local
        /// </summary>
        /// <param name="nif"></param>
        /// <returns>True si se ha podido eliminar</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool EliminarLocal(int id)
        {
            // Verifica que exista el local antes de intentar eliminarlo
            var local = ObtenerPorId(id);
            if(local == null)
            {
                throw new InvalidOperationException("El local no existe en la base de datos.");
            }
            try
            {
                // Ejecuta el borrado y devuelve las filas afectadas
                string sqlEliminarLocal = "DELETE FROM Locales WHERE Id = @Id";
                var parametros = new[] { new SQLiteParameter("@Id", local.Id) };

                int filasActualizadas = GestorDatos.EjecutarComando(sqlEliminarLocal, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se encontro el local para borrar en la base de datos");
                }
                return true; // Indica que la eliminacion fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido local el cliente: {ex.Message}", ex);
            }
        }


        // No se implementa este metodo porque la eliminacion se hace por Id
        public bool Eliminar(string nif)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Graba la fecha de baja de un local pasando el Id del local
        /// Si no se pasa fecha, se pone la fecha actual
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool BajaLocal(int id, DateTime? fechaBaja = null)
        {
            // Verifica que exista el local
            var local = ObtenerPorId(id);
            if(local == null)
            {
                throw new InvalidOperationException("El local no existe en la base de datos.");
            }

            try
            {
                // Graba la fecha de baja en la propiedad del local
                local.FechaBaja = Utiles.ValidarFecha(fechaBaja); // Si la fecha es nula se establece la fecha actual
                return Actualizar(local);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido dar de baja el local: {ex.Message}", ex);
            }
        }


        /// No se implementa este metodo porque la baja se hace por Id
        public bool Baja(string nif, DateTime? fechaBaja)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene los locales de una empresa segun el parametro 'activos'
        /// </summary>
        /// <param name="empresaId"></param>
        /// <param name="activos"></param>
        /// <returns>Lista con los locales de la emrpesa</returns>
        public IEnumerable<Local> ListarLocalesPorEmpresa(int empresaId, bool? activos = null)
        {
            // Crea una lista de locales vacia
            var listaLocales = new List<Local>();

            // Asigna el parametro de empresaId a las consultas
            var parametros = new[]
            {
                new SQLiteParameter("@EmpresaId", empresaId)
            };

            string sql = "SELECT * FROM Locales WHERE EmpresaId = @EmpresaId";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value
                    ? " AND FechaBaja IS NULL"
                    : " AND FechaBaja IS NOT NULL";
            }

            // Carga una tabla con todos los locales
            DataTable tabla = GestorDatos.EjecutarConsulta(sql, parametros);

            // Va añadiendo cada local a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var local = Utilidades.MapeadorDatos.MapearFila<Local>(fila);
                listaLocales.Add(local);
            }

            return listaLocales;
        }


        /// <summary>
        /// Obtiene todos los locales segun el parametro 'activos'
        /// </summary>
        /// <param name="activos"></param>
        /// <returns>Lista con los locales</returns>
        public IEnumerable<Local> ListarTodos(bool? activos = null)
        {
            // Crea una lista de locales vacia
            var listaLocales = new List<Local>();

            string sql = "SELECT * FROM Locales ";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sql += activos.Value
                    ? " WHERE FechaBaja IS NULL"
                    : " WHERE FechaBaja IS NOT NULL";
            }

            // Carga una tabla con todos los locales
            DataTable tabla = GestorDatos.EjecutarConsulta(sql);

            // Va añadiendo cada local a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var local = Utilidades.MapeadorDatos.MapearFila<Local>(fila);
                listaLocales.Add(local);
            }

            return listaLocales;
        }


        /// <summary>
        /// Obtiene un local por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto local con sus propiedades</returns>
        public Local ObtenerPorId(int id)
        {
            return GestorDatos.ObtenerDatosPorId<Local>("Locales", id);
        }


        // No se implementa este metodo porque los locales no se buscan por NIF
        public Local ObtenerPorNIF(string nif)
        {
            throw new NotImplementedException();
        }

        private void ValidarLocal(Local local)
        {
            // Evita agregar locales nulos
            if(local == null)
            {
                throw new ArgumentNullException(nameof(local), "El local no puede ser nulo.");
            }

            // Valida que la empresa del exista
            var gestor = new GestorEmpresas();
            var empresa = local.EmpresaId;
            if(gestor.ObtenerPorId(empresa) == null)
            {
                throw new ArgumentException("La empresa asignada al local no existe");
            }

            // Validar campos obligatorios
            if(local.EmpresaId == 0)
            {
                throw new ArgumentException("El codigo de empresa es obligatorio.");
            }

            if(string.IsNullOrEmpty(local.Descripcion))
            {
                throw new ArgumentException("La descripcion del local es obligatoria.");
            }

            // Valida las propiedades del Local
            local.ValidarPropiedadesObjeto();
        }
    }
}
