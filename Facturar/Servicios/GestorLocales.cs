using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Facturar.Entidades;
using Facturar.Interfaces;
using static Facturar.Servicios.GestorClientes;

namespace Facturar.Servicios
{
    public class GestorLocales : IRepositorioLocales
    {
        // Propiedades para comandos CRUD
        private string sqlInsertarLocal =
            "INSERT INTO Locales " +
            "(EmpresaId, Descripcion, Direccion, CodigoPostal, Poblacion, Provincia, ImporteAlquiler, Observaciones, FechaAlta, FechaBaja) " +
            "VALUES (@EmpresaId, @Descripcion, @Direccion, @CodigoPostal, @Poblacion, @Provincia, @ImporteAlquiler, @Observaciones, @FechaAlta, @FechaBaja)";

        private string sqlActualizarLocal =
            "UPDATE Locales SET " +
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

        private string sqlEliminarLocal =
            "DELETE " +
            "FROM Locales " +
            "WHERE Id = @Id";
      
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
                if(local.FechaAlta == DateTime.MinValue)
                {
                    //Asigna la fecha de alta
                    local.FechaAlta = DateTime.Now.Date;
                }

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
                    new SQLiteParameter("@FechaBaja", local.FechaBaja != DateTime.MinValue ? (object)local.FechaBaja: DBNull.Value)
                };

                // Ejecuta el comando y obtiene el numero de filas insertadas
                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarLocal, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se pudo insertar el local en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException("Error al agregar el cliente.", ex);
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
                    new SQLiteParameter("@FechaBaja", local.FechaBaja != DateTime.MinValue ? (object)local.FechaBaja: DBNull.Value)
                };

                // Ejecuta la actualizacion y devuelve las filas actualizadas
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
                local.FechaBaja = fechaBaja ?? DateTime.Now.Date;
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


        public IEnumerable<Local> ListarLocalesPorEmpresa(int empresaId, bool? activos = null)
        {
            // Crea una lista de locales vacia
            var listaLocales = new List<Local>();

            // Carga una tabla con todos los locales de la empresa segun el paremetro 'activos'
            DataTable tabla = null;

            // Asigna el parametro de empresaId a las consultas
            var parametros = new[]
            {
                new SQLiteParameter("@EmpresaId", empresaId)
            };

            tabla = ConsultarLocalesEmpresa(activos, parametros);

            // Va añadiendo cada local a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var local = Utilidades.MapeadorDatos.MapearFila<Local>(fila);
                listaLocales.Add(local);
            }

            return listaLocales;
        }

        public IEnumerable<Local> ListarTodos(bool? activos = null)
        {
            // Crea una lista de locales vacia
            var listaLocales = new List<Local>();

            // Carga una tabla con todos los locales
            DataTable tabla = ConsultarTotalLocales(activos);

            // Va añadiendo cada local a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var local = Utilidades.MapeadorDatos.MapearFila<Local>(fila);
                listaLocales.Add(local);
            }

            return listaLocales; ;
        }


        /// <summary>
        /// Devuelve una tabla con todos los locales de una empresa segun el parametro 'activos'
        /// </summary>
        /// <returns></returns>
        public DataTable ConsultarLocalesEmpresa(bool? activos, params SQLiteParameter[] pametros)
        {
            string sqlLocales = "SELECT * FROM Locales WHERE EmpresaId = @EmpresaId";
            string sqlLocalesActivos = sqlLocales + " AND FechaBaja IS NULL ";
            string sqlLocalesInactivos = sqlLocales + " AND FechaBaja IS NOT NULL ";
            if(activos == true)
            {
                return GestorDatos.EjecutarConsulta(sqlLocalesActivos, pametros);
            }
            else if(activos == false)
            {
                return GestorDatos.EjecutarConsulta(sqlLocalesInactivos, pametros);
            }
            else
            {
                return GestorDatos.EjecutarConsulta(sqlLocales, pametros);
            }
        }

        /// <summary>
        /// Devuelve una tabla con todos los locales
        /// </summary>
        /// <returns></returns>
        public DataTable ConsultarTotalLocales(bool? activos)
        {
            string sqlLocales = "SELECT * FROM Locales "; ;
            string sqlocalesActivos = sqlLocales + " WHERE FechaBaja IS NULL ";
            string sqloLocalesInactivos = sqlLocales + " WHERE FechaBaja IS NOT NULL ";
            if(activos == true)
            {
                return GestorDatos.EjecutarConsulta(sqlocalesActivos);
            }
            else if(activos == false)
            {
                return GestorDatos.EjecutarConsulta(sqloLocalesInactivos);
            }
            else
            {
                return GestorDatos.EjecutarConsulta(sqlLocales);
            }
        }


        private void ValidarLocal(Local local)
        {
            // Evita agregar clientes nulos
            if(local == null)
            {
                throw new ArgumentNullException(nameof(local), "El local no puede ser nulo.");
            }

            // Validar campos obligatorios
            if(local.EmpresaId == 0)
            {
                throw new ArgumentException("El codigo de empresa es obligatorio.");
            }

            if(string.IsNullOrEmpty(local.Direccion))
            {
                throw new ArgumentException("La direccion del local es obligatorio.");
            }
        }


        /// <summary>
        /// Obtiene un local por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto local con las propiedades que tenga</returns>
        public Local ObtenerPorId(int id)
        {
            return GestorDatos.ObtenerDatosPorId<Local>("Locales", id);
        }


        // No se implementa este metodo porque los locales no se buscan por NIF
        public Local ObtenerPorNIF(string nif)
        {
            throw new NotImplementedException();
        }

    }
}
