using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Facturar.Entidades;
using Facturar.Interfaces;
using Utiles = Facturar.Utilidades.UtilesGenerales;


namespace Facturar.Servicios
{
    public class GestorEmpresas : IRepositorioEmpresas
    {
        //Constructor privado para evitar instanciación externa
        public GestorEmpresas()
        {

        }

        /// <summary>
        /// Inserta una nueva empresa en la base de datos
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns>Devuelve 'true' si se ha podido grabar en la BBDD</returns>
        /// <exception cref="ApplicationException"></exception>
        public bool Agregar(Empresa empresa)
        {
            try
            {
                // Valida que la empresa no sea nula, y el NIF y nombre tengan contenido
                ValidarEmpresa(empresa);

                //Asigna la fecha de alta
                empresa.FechaAlta = Utiles.ValidarFecha(empresa.FechaAlta);

                // Verifica que no exista ya una empresa con el mismo NIF
                if(EmpresaExiste(empresa.NIF))
                {
                    throw new InvalidOperationException("Ya existe una empresa con ese NIF.");
                }

                // Inserta la nueva empresa en la base de datos
                var parametros = new[]
                {
                    new SQLiteParameter("@NIF", empresa.NIF),
                    new SQLiteParameter("@Nombre", empresa.Nombre),
                    new SQLiteParameter("@Direccion", empresa.Direccion),
                    new SQLiteParameter("@CodigoPostal", empresa.CodigoPostal),
                    new SQLiteParameter("@Poblacion", empresa.Poblacion),
                    new SQLiteParameter("@Provincia", empresa.Provincia),
                    new SQLiteParameter("@Telefono", empresa.Telefono),
                    new SQLiteParameter("@Email", empresa.Email),
                    new SQLiteParameter("@PersonaContacto", empresa.PersonaContacto),
                    new SQLiteParameter("@FechaAlta", empresa.FechaAlta.Date),
                    new SQLiteParameter("@FechaBaja", empresa.FechaBaja.HasValue ? (object)empresa.FechaBaja.Value.Date: DBNull.Value),
                    new SQLiteParameter("@SerieFactura", empresa.SerieFactura),
                    new SQLiteParameter("@NumeroFacturaActual", empresa.NumeroFacturaActual)
                };

                // Ejecuta el comando y obtiene el numero de filas insertadas
                string sqlInsertarEmpresa = "INSERT INTO Empresas " +
                    "(NIF, Nombre, Direccion, CodigoPostal, Poblacion, Provincia, Telefono, Email, PersonaContacto, FechaAlta, FechaBaja, SerieFactura, NumeroFacturaActual) " +
                    "VALUES (@NIF, @Nombre, @Direccion, @CodigoPostal, @Poblacion, @Provincia, @Telefono, @Email, @PersonaContacto, @FechaAlta, @FechaBaja, @SerieFactura, @NumeroFacturaActual)";

                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarEmpresa, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se pudo insertar la empresa en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException($"Error al agregar la empresa: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Permite actualizar los datos de una empresa en la BBDD
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns>True si se ha podido actualizar</returns>
        /// <exception cref="InvalidOperationException"></exception>

        public bool Actualizar(Empresa empresa)
        {
            try
            {
                // Valida que la empresa no sea nula, y el NIF y nombre tengan contenido
                ValidarEmpresa(empresa);

                // Verifica que exista una empresa con el NIF proporcionado
                if(!EmpresaExiste(empresa.NIF))
                {
                    throw new InvalidOperationException("No existe una empresa con ese NIF.");
                }

                if(empresa.Activo)
                {
                    throw new InvalidOperationException("La empresa no esta activa");
                }

                // Nota: no se incluye la fecha de alta porque se graba en el alta y no se debe modificar al actualizar
                var parametros = new[]
                {
                    new SQLiteParameter("@NIF", empresa.NIF),
                    new SQLiteParameter("@Nombre", empresa.Nombre),
                    new SQLiteParameter("@Direccion", empresa.Direccion),
                    new SQLiteParameter("@CodigoPostal", empresa.CodigoPostal),
                    new SQLiteParameter("@Poblacion", empresa.Poblacion),
                    new SQLiteParameter("@Provincia", empresa.Provincia),
                    new SQLiteParameter("@Telefono", empresa.Telefono),
                    new SQLiteParameter("@Email", empresa.Email),
                    new SQLiteParameter("@PersonaContacto", empresa.PersonaContacto),
                    new SQLiteParameter("@FechaBaja", empresa.FechaBaja.HasValue ? (object)empresa.FechaBaja.Value.Date: DBNull.Value),
                    new SQLiteParameter("@SerieFactura", empresa.SerieFactura),
                    new SQLiteParameter("@NumeroFacturaActual", empresa.NumeroFacturaActual)
                };

                // Ejecuta la actualizacion y devuelve las filas actualizadas
                string sqlActualizar = "UPDATE Empresas SET " +
                    "NIF = @NIF, Nombre = @Nombre, Direccion = @Direccion, CodigoPostal = @CodigoPostal, Poblacion =@Poblacion, Provincia = @Provincia, Telefono = @Telefono, Email = @Email, PersonaContacto = @PersonaContacto, FechaBaja = @FechaBaja, SerieFactura = @SerieFactura, NumeroFacturaActual = @NumeroFacturaActual " +
                    "WHERE NIF = @NIF";
                int filasActualizadas = GestorDatos.EjecutarComando(sqlActualizar, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No ha podido actualizar la empresa en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al actualizar la empresa en la base de datos: {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Permite eliminar una empresa de la base de datos pasando el NIF
        /// </summary>
        /// <param name="nif"></param>
        /// <returns>True si se ha podido eliminar</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Eliminar(string nif)
        {
            // Verifica que exista la empresa
            var empresa = ObtenerPorNIF(nif);
            if(empresa == null)
            {
                throw new InvalidOperationException("La empresa no existe en la base de datos.");
            }
            try
            {
                // Ejecuta el borrado y devuelve las filas afectadas
                var parametros = new[] { new SQLiteParameter("@NIF", empresa.NIF) };

                string sqlEliminar = "DELETE FROM Empresas WHERE NIF = @NIF";
                int filasActualizadas = GestorDatos.EjecutarComando(sqlEliminar, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se ha podido eliminar la empresa en la base de datos");
                }
                return true; // Indica que la eliminacion fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar la empresa en la base de datos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Graba la fecha de baja de una empresa pasando el NIF
        /// Si no se pasa fecha, se pone la fecha actual
        /// </summary>
        /// <param name="nif"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Baja(string nif, DateTime? fechaBaja = null)
        {
            // Verifica que exista la empresa
            var empresa = ObtenerPorNIF(nif);
            if(empresa == null)
            {
                throw new InvalidOperationException("La empresa no existe en la base de datos.");
            }

            try
            {
                // Graba la fecha de baja en la propiedad de la empresa
                empresa.FechaBaja = Utiles.ValidarFecha(fechaBaja);
                return Actualizar(empresa);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al dar de baja la empresa en la base de datos: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Obtiene una empresa por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto empresa con las propiedades que tenga</returns>
        public Empresa ObtenerPorId(int id)
        {
            return GestorDatos.ObtenerDatosPorId<Empresa>("Empresas", id);
        }


        /// <summary>
        /// Obtiene una empresa por su NIF
        /// </summary>
        /// <param name="nif"></param>
        /// <returns>Objeto empresa con las propiedades que tenga</returns>
        public Empresa ObtenerPorNIF(string nif)
        {
            return GestorDatos.ObtenerDatosPorNIF<Empresa>("Empresas", nif);
        }

        /// <summary>
        /// Permite obtener una lista con todas las empresas según su estado (activas o no)
        /// </summary>
        /// <returns>Lista con las empresas y sus propiedades</returns>
        public IEnumerable<Empresa> ListarTodos(bool? activas = null)
        {
            // Crea una lista de empresas
            var listaEmpresas = new List<Empresa>();

            string sqlEmpresas = "SELECT * " + "FROM Empresas ";
            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activas.HasValue)
            {
                sqlEmpresas += activas.Value
                    ? " WHERE FechaBaja IS NULL"
                    : " WHERE FechaBaja IS NOT NULL";
            }

            // Carga una tabla con todas las empresas
            DataTable tabla = GestorDatos.EjecutarConsulta(sqlEmpresas);

            // Va añadiendo cada empresa a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var empresa = Utilidades.MapeadorDatos.MapearFila<Empresa>(fila);
                listaEmpresas.Add(empresa);
            }
            return listaEmpresas;
        }

        public DataTable ConsultaLocalesEmpresa(string nif, bool? activas = true)
        {
            // Asigna parametros y consulta SQL según si se quieren locales activos o todos
            string sql = @"SELECT * FROM Empresas";
            var parametros = new[] { new SQLiteParameter("@NIF", nif) };
            if(activas.HasValue)
            {
                sql += activas.Value
                    ? " WHERE FechaBaja IS NULL"
                    : " WHERE FechaBaja IS NOT NULL";
            }

            return GestorDatos.EjecutarConsulta(sql, parametros);
        }


        /// <summary>
        /// Permite comprobar si la empresa existe en la base de datos
        /// </summary>
        /// <param name="nif"></param>
        /// <returns>True si existe</returns>
        private bool EmpresaExiste(string nif)
        {
            string sqlEmpresas = @"SELECT COUNT(1) FROM Empresas WHERE NIF = @NIF";
            using(var conexion = GestorDatos.AbrirConexion())
            {
                using(var comando = new SQLiteCommand(sqlEmpresas, conexion))
                {
                    comando.Parameters.AddWithValue("@NIF", nif);
                    var resultado = Convert.ToInt32(comando.ExecuteScalar());
                    return resultado > 0;
                }
            }
        }

        /// <summary>
        /// Metodo para validar que una empresa no sea nula y que tenga NIF y nombre
        /// </summary>
        /// <param name="empresa"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void ValidarEmpresa(Empresa empresa)
        {
            // Evita agregar entidades nulas
            if(empresa == null)
            {
                throw new ArgumentNullException(nameof(empresa), "La entidad no puede ser nula.");
            }

            // Validar campos obligatorios
            if(string.IsNullOrEmpty(empresa.Nombre))
            {
                throw new ArgumentException("El nombre de la empresa es obligatorio.");
            }

            if(string.IsNullOrEmpty(empresa.NIF))
            {
                throw new ArgumentException("El NIF de la empresa es obligatorio.");
            }
        }
    }
}
