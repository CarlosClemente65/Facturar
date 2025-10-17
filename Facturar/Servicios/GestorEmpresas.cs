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
            "NumeroFacturaActual = @NumeroFacturaActual " +
            "WHERE NIF = @NIF";

        private string sqlEliminarEmpresa =
            "DELETE " +
            "FROM Empresas " +
            "WHERE NIF = @NIF";

        private string sqlSeleccionEmpresasActivas =
            "SELECT * " +
            "FROM Empresas " +
            "WHERE FechaBaja IS NULL";

        private string sqlSeleccionEmpresas =
            "SELECT * " + "FROM Empresas ";

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

                // Asigna la fecha de alta si no está establecida
                if(empresa.FechaAlta == DateTime.MinValue)
                {
                    //Asigna la fecha de alta
                    empresa.FechaAlta = DateTime.Now;
                }

                empresa.SerieFactura = empresa.SerieFactura ?? string.Empty; // Asigna una serie por defecto si no se proporciona

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
                    new SQLiteParameter("@FechaAlta", empresa.FechaAlta),
                    new SQLiteParameter("@FechaBaja", empresa.FechaBaja != DateTime.MinValue ? (object)empresa.FechaBaja: DBNull.Value),
                    new SQLiteParameter("@SerieFactura", empresa.SerieFactura),
                    new SQLiteParameter("@NumeroFacturaActual", empresa.NumeroFacturaActual)
                };

                // Ejecuta el comando y obtiene el numero de filas insertadas
                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarEmpresa, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se pudo insertar la empresa en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException("Error al agregar la empresa.", ex);
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
                    new SQLiteParameter("@FechaAlta", empresa.FechaAlta),
                    new SQLiteParameter("@FechaBaja", empresa.FechaBaja != DateTime.MinValue ? (object)empresa.FechaBaja: DBNull.Value),
                    new SQLiteParameter("@SerieFactura", empresa.SerieFactura),
                    new SQLiteParameter("@NumeroFacturaActual", empresa.NumeroFacturaActual)
                };

                // Ejecuta la actualizacion y devuelve las filas actualizadas
                int filasActualizadas = GestorDatos.EjecutarComando(sqlActualizarEmpresa, parametros);

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
                int filasActualizadas = GestorDatos.EjecutarComando(sqlEliminarEmpresa, parametros);

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
        /// Graba la fecha de baja de una empresa pasando el NIF
        /// Si no se pasa fecha, se pone la fecha actual
        /// </summary>
        /// <param name="nif"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool BajaEmpresa(string nif, DateTime? fechaBaja = null)
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
        public Empresa ObtenerPorId(int _id)
        {
            return GestorDatos.ObtenerDatosPorId<Empresa>("Empresas", _id);
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
