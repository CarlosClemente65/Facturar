using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Facturar.Entidades;
using Facturar.Interfaces;


namespace Facturar.Servicios
{
    public class GestorEmpresas : IRepositorioEmpresas
    {
        // Propiedades para comandos CRUD
        private string sqlInsertarEmpresa =
            "INSERT INTO Empresas " +
            "(NIF, Nombre, Direccion, CodigoPostal, Poblacion, Provincia, Telefono, Email, PersonaContacto, FechaAlta, FechaBaja, SerieFactura, NumeroFacturaActual) " +
            "VALUES (@NIF, @Nombre, @Direccion, @CodigoPostal, @Poblacion, @Provincia, @Telefono, @Email, @PersonaContacto, @FechaAlta, @FechaBaja, @SerieFactura, @NumeroFacturaActual)";

        private string sqlActualizarEmpresa =
            "UPDATE Empresas SET " +
            "NIF = @NIF, " +
            "Nombre = @Nombre, " +
            "Direccion = @Direccion, " +
            "CodigoPostal = @CodigoPostal, " +
            "Poblacion =@Poblacion, " +
            "Provincia = @Provincia, " +
            "Telefono = @Telefono, " +
            "Email = @Email, " +
            "PersonaContacto = @PersonaContacto, " +
            "FechaAlta = @FechaAlta, " +
            "FechaBaja = @FechaBaja, " +
            "SerieFactura = @SerieFactura, " +
            "NumeroFacturaActual = @NumeroFacturaActual" +
            " WHERE NIF = @NIF";

        private string sqlEliminarEmpresa =
            "DELETE " +
            "FROM Empresas " +
            "WHERE Id = @Id";

        private string sqlSeleccionEmpresasActivas =
            "SELECT * " +
            "FROM Empresas " +
            "WHERE FechaBaja IS NULL";

        private string sqlSeleccionEmpresas =
            "SELECT * " + "FROM Empresas ";

        //Constructor privado para evitar instanciación externa
        private GestorEmpresas()
        {

        }

        /// <summary>
        /// Inserta una nueva empresa en la base de datos
        /// </summary>
        /// <param name="_empresa"></param>
        /// <returns>Devuelve 'true' si se ha podido grabar en la BBDD</returns>
        /// <exception cref="ApplicationException"></exception>
        public bool Agregar(Empresa _empresa)
        {
            try
            {
                // Valida que la empresa no sea nula, y el NIF y nombre tengan contenido
                ValidarEmpresa(_empresa);

                // Asigna la fecha de alta si no está establecida
                if(_empresa.FechaAlta == DateTime.MinValue)
                {
                    //Asigna la fecha de alta
                    _empresa.FechaAlta = DateTime.Now;
                }

                _empresa.SerieFactura = _empresa.SerieFactura ?? string.Empty; // Asigna una serie por defecto si no se proporciona

                // Verifica que no exista ya una empresa con el mismo NIF
                if(EmpresaExiste(_empresa.NIF))
                {
                    throw new InvalidOperationException("Ya existe una empresa con ese NIF.");
                }

                // Inserta la nueva empresa en la base de datos
                var parametros = new[]
                {
                    new SQLiteParameter("@NIF", _empresa.NIF),
                    new SQLiteParameter("@Nombre", _empresa.Nombre),
                    new SQLiteParameter("@Direccion", _empresa.Direccion),
                    new SQLiteParameter("@CodigoPostal", _empresa.CodigoPostal),
                    new SQLiteParameter("@Poblacion", _empresa.Poblacion),
                    new SQLiteParameter("@Provincia", _empresa.Provincia),
                    new SQLiteParameter("@Telefono", _empresa.Telefono),
                    new SQLiteParameter("@Email", _empresa.Email),
                    new SQLiteParameter("@PersonaContacto", _empresa.PersonaContacto),
                    new SQLiteParameter("@FechaAlta", _empresa.FechaAlta),
                    new SQLiteParameter("@FechaBaja", _empresa.FechaBaja != DateTime.MinValue ? (object)_empresa.FechaBaja: DBNull.Value),
                    new SQLiteParameter("@SerieFactura", _empresa.SerieFactura),
                    new SQLiteParameter("@NumeroFacturaActual", _empresa.NumeroFacturaActual)
                };

                // Ejecuta el comando y obtiene el Id generado
                _empresa.Id = Convert.ToInt32(GestorDatos.EjecutarEscalar(sqlInsertarEmpresa, parametros));

                if(_empresa.Id <= 0)
                {
                    throw new InvalidOperationException("No se pudo insertar la empresa en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                // Manejo de la excepción (puede ser logging, rethrow, etc.)
                throw new ApplicationException("Error al agregar la empresa.", ex);
            }
        }


        /// <summary>
        /// Permite actualizar los datos de una empresa en la BBDD
        /// </summary>
        /// <param name="_empresa"></param>
        /// <returns>True si se ha podido actualizar</returns>
        /// <exception cref="InvalidOperationException"></exception>

        public bool Actualizar(Empresa _empresa)
        {
            try
            {
                ValidarEmpresa(_empresa);

                // Verifica que exista una empresa con el mismo NIF
                if(!EmpresaExiste(_empresa.NIF))
                {
                    throw new InvalidOperationException("No existe una empresa con ese NIF.");
                }

                var parametros = new[]
                {
                    new SQLiteParameter("@NIF", _empresa.NIF),
                    new SQLiteParameter("@Nombre", _empresa.Nombre),
                    new SQLiteParameter("@Direccion", _empresa.Direccion),
                    new SQLiteParameter("@CodigoPostal", _empresa.CodigoPostal),
                    new SQLiteParameter("@Poblacion", _empresa.Poblacion),
                    new SQLiteParameter("@Provincia", _empresa.Provincia),
                    new SQLiteParameter("@Telefono", _empresa.Telefono),
                    new SQLiteParameter("@Email", _empresa.Email),
                    new SQLiteParameter("@PersonaContacto", _empresa.PersonaContacto),
                    new SQLiteParameter("@FechaAlta", _empresa.FechaAlta),
                    new SQLiteParameter("@FechaBaja", _empresa.FechaBaja != DateTime.MinValue ? (object)_empresa.FechaBaja: DBNull.Value),
                    new SQLiteParameter("@SerieFactura", _empresa.SerieFactura),
                    new SQLiteParameter("@NumeroFacturaActual", _empresa.NumeroFacturaActual)
                };

                // Ejecuta la actualizacion y devuelve las filas actualizadas
                int filasActualizadas = GestorDatos.EjecutarNonQuery(sqlActualizarEmpresa, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se encontro la empresa para actualizar en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido actualizar la empresa: {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Permite eliminar una empresa de la base de datos
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True si se ha podido eliminar</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Eliminar(int id)
        {
            // Verifica que exista la empresa
            var empresa = Obtener(id);
            if(empresa == null)
            {
                throw new InvalidOperationException("La empresa no existe en la base de datos.");
            }
            try
            {
                // Ejecuta el borrado y devuelve las filas afectadas
                var parametros = new[] { new SQLiteParameter("@Id", id) };
                int filasActualizadas = GestorDatos.EjecutarNonQuery(sqlEliminarEmpresa, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se encontro la empresa para borrar en la base de datos");
                }
                return true; // Indica que la eliminacion fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido eliminar la empresa: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Graba la fecha de baja de una empresa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool BajaEmpresa(int id, DateTime? fechaBaja = null)
        {
            // Verifica que exista la empresa
            var empresa = Obtener(id);
            if(empresa == null)
            {
                throw new InvalidOperationException("La empresa no existe en la base de datos.");
            }

            try
            {
                // Graba la fecha de baja en la propiedad de la empresa
                empresa.FechaBaja = fechaBaja ?? DateTime.Now;
                return Actualizar(empresa);

            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido dar de baja la empresa: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Obtiene una empresa por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto empresa con las propiedades que tenga</returns>
        public Empresa Obtener(int _id)
        {
            return GestorDatos.ObtenerDatosPorId<Empresa>("Empresas", _id);
        }

        /// <summary>
        /// Permite obtener una lista con todas las empresas (activas o no)
        /// </summary>
        /// <returns>Lista con las empresas y sus propiedades</returns>
        public IEnumerable<Empresa> ListarTodos()
        {
            // Crea una lista de empresas
            var listaEmpresas = new List<Empresa>();

            // Carga una tabla con todas las empresas
            DataTable tabla = ConsultarEmpresas();

            // Va añadiendo cada empresa a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var empresa = Utilidades.MapeadorDatos.MapearFila<Empresa>(fila);
                listaEmpresas.Add(empresa);
            }
            return listaEmpresas;
        }


        /// <summary>
        /// Permite obtener una lista con todas las empresas activas
        /// </summary>
        /// <returns>Lista con las empresas y sus propiedades</returns>
        public IEnumerable<Empresa> ListarActivas()
        {
            // Crea una lista de empresas
            var listaEmpresas = new List<Empresa>();

            // Carga una tabla con todas las empresas
            DataTable tabla = ConsultarActivas();

            // Va añadiendo cada empresa a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var empresa = Utilidades.MapeadorDatos.MapearFila<Empresa>(fila);
                listaEmpresas.Add(empresa);
            }
            return listaEmpresas;
        }

        public IEnumerable<Empresa> ListarLocalesEmpresa(bool activas = true)
        {
            throw new NotImplementedException();
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
        /// <param name="_empresa"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void ValidarEmpresa(Empresa _empresa)
        {
            // Evita agregar entidades nulas
            if(_empresa == null)
            {
                throw new ArgumentNullException(nameof(_empresa), "La entidad no puede ser nula.");
            }

            // Validar campos obligatorios
            if(string.IsNullOrEmpty(_empresa.Nombre))
            {
                throw new ArgumentException("El nombre de la empresa es obligatorio.");
            }

            if(string.IsNullOrEmpty(_empresa.NIF))
            {
                throw new ArgumentException("El NIF de la empresa es obligatorio.");
            }
        }

        /// <summary>
        /// Devuelve una tabla con todas las empresas activas y sus datos
        /// </summary>
        /// <returns></returns>
        public DataTable ConsultarActivas()
        {
            return GestorDatos.EjecutarConsulta(sqlSeleccionEmpresasActivas);
        }

        /// <summary>
        /// Devuelve una tabla con todas las empresas y sus datos
        /// </summary>
        /// <returns></returns>
        public DataTable ConsultarEmpresas()
        {
            return GestorDatos.EjecutarConsulta(sqlSeleccionEmpresas);
        }
    }
}
