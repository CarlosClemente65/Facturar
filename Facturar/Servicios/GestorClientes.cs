using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Facturar.Entidades;
using Facturar.Interfaces;

namespace Facturar.Servicios
{
    public class GestorClientes : IRepositorioClientes
    {
        // Propiedades para comandos CRUD
        private string sqlInsertarCliente =
            "INSERT INTO Clientes " +
            "(NIF, Nombre, Direccion, CodigoPostal, Poblacion, Provincia, Telefono, Email, PersonaContacto, FechaAlta, FechaBaja, FormaPago, IBAN, Observaciones) " +
            "VALUES (@NIF, @Nombre, @Direccion, @CodigoPostal, @Poblacion, @Provincia, @Telefono, @Email, @PersonaContacto, @FechaAlta, @FechaBaja, @FormaPago, @IBAN, @Observaciones)";

        private string sqlActualizarCliente =
            "UPDATE Clientes SET " +
            "NIF = @NIF, " +
            "Nombre = @Nombre, " +
            "Direccion = @Direccion, " +
            "CodigoPostal = @CodigoPostal, " +
            "Poblacion = @Poblacion, " +
            "Provincia = @Provincia, " +
            "Telefono = @Telefono, " +
            "Email = @Email, " +
            "PersonaContacto = @PersonaContacto, " +
            "FechaBaja = @FechaBaja, " +
            "FormaPago = @FormaPago, " +
            "IBAN = @IBAN, " +
            "Observaciones = @Observaciones " +
            "WHERE NIF = @NIF";

        private string sqlEliminarCliente =
            "DELETE " +
            "FROM Clientes " +
            "WHERE NIF = @NIF";


        // Constructor privado para evitar instanciación externa
        public GestorClientes()
        {

        }

        /// <summary>
        /// Inserta un nuevo cliente en la base de datos
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns>Devuelve 'true' si se ha podido grabar en la BBDD</returns>
        /// <exception cref="ApplicationException"></exception>
        public bool Agregar(Cliente cliente)
        {
            try
            {
                // Valida que el cliente no sea nulo, y el NIF y nombre tengan contenido
                ValidarCliente(cliente);

                // Asigna la fecha de alta si no está establecida
                if(cliente.FechaAlta == DateTime.MinValue)
                {
                    //Asigna la fecha de alta
                    cliente.FechaAlta = DateTime.Now.Date;
                }

                // Asigna la forma de pago por defecto si no está 
                cliente.FormaPago = cliente.FormaPago ?? FormasPago.Transferencia.ToString();

                // Verifica que no exista ya un cliente con el mismo NIF
                if(ClienteExiste(cliente.NIF))
                {
                    throw new InvalidOperationException("Ya existe un cliente con ese NIF.");
                }

                // Inserta el nuevo cliente en la base de datos
                var parametros = new[]
                {
                    new SQLiteParameter("@NIF", cliente.NIF),
                    new SQLiteParameter("@Nombre", cliente.Nombre),
                    new SQLiteParameter("@Direccion", cliente.Direccion),
                    new SQLiteParameter("@CodigoPostal", cliente.CodigoPostal),
                    new SQLiteParameter("@Poblacion", cliente.Poblacion),
                    new SQLiteParameter("@Provincia", cliente.Provincia),
                    new SQLiteParameter("@Telefono", cliente.Telefono),
                    new SQLiteParameter("@Email", cliente.Email),
                    new SQLiteParameter("@PersonaContacto", cliente.PersonaContacto),
                    new SQLiteParameter("@FechaAlta", cliente.FechaAlta.Date),
                    new SQLiteParameter("@FechaBaja", cliente.FechaBaja != DateTime.MinValue ? (object)cliente.FechaBaja: DBNull.Value),
                    new SQLiteParameter("@FormaPago", cliente.FormaPago),
                    new SQLiteParameter("@IBAN", cliente.IBAN),
                    new SQLiteParameter("@Observaciones", cliente.Observaciones)
                };

                // Ejecuta el comando y obtiene el numero de filas insertadas
                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarCliente, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se pudo insertar el cliente en la base de datos");
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

        public bool Actualizar(Cliente cliente)
        {
            try
            {
                // Valida que el cliente no sea nulo, y el NIF y nombre tengan contenido
                ValidarCliente(cliente);

                // Verifica que exista un cliente con el NIF proporcionado
                if(!ClienteExiste(cliente.NIF))
                {
                    throw new InvalidOperationException("No existe un cliente con ese NIF.");
                }

                // Nota: no se incluye la fecha de alta porque solo se graba en el alta, no se puede modificar en la actualizacion
                var parametros = new[]
                {
                    new SQLiteParameter("@NIF", cliente.NIF),
                    new SQLiteParameter("@Nombre", cliente.Nombre),
                    new SQLiteParameter("@Direccion", cliente.Direccion),
                    new SQLiteParameter("@CodigoPostal", cliente.CodigoPostal),
                    new SQLiteParameter("@Poblacion", cliente.Poblacion),
                    new SQLiteParameter("@Provincia", cliente.Provincia),
                    new SQLiteParameter("@Telefono", cliente.Telefono),
                    new SQLiteParameter("@Email", cliente.Email),
                    new SQLiteParameter("@PersonaContacto", cliente.PersonaContacto),
                    new SQLiteParameter("@FechaBaja", cliente.FechaBaja != DateTime.MinValue ? (object)cliente.FechaBaja: DBNull.Value),
                    new SQLiteParameter("@FormaPago", cliente.FormaPago),
                    new SQLiteParameter("@IBAN", cliente.IBAN),
                    new SQLiteParameter("@Observaciones", cliente.Observaciones)
                };

                // Ejecuta la actualizacion y devuelve las filas actualizadas
                int filasActualizadas = GestorDatos.EjecutarComando(sqlActualizarCliente, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se encontro el cliente para actualizar en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido actualizar el cliente: {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Permite eliminar un cliente de la base de datos pasando el NIF
        /// </summary>
        /// <param name="nif"></param>
        /// <returns>True si se ha podido eliminar</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Eliminar(string nif)
        {
            // Verifica que exista el cliente
            var cliente = ObtenerPorNIF(nif);
            if(cliente == null)
            {
                throw new InvalidOperationException("El cliente no existe en la base de datos.");
            }
            try
            {
                // Ejecuta el borrado y devuelve las filas afectadas
                var parametros = new[] { new SQLiteParameter("@NIF", cliente.NIF) };
                int filasActualizadas = GestorDatos.EjecutarComando(sqlEliminarCliente, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se encontro el cliente para borrar en la base de datos");
                }
                return true; // Indica que la eliminacion fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido eliminar el cliente: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Graba la fecha de baja de un cliente pasando el NIF
        /// Si no se pasa fecha, se pone la fecha actual
        /// </summary>
        /// <param name="nif"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Baja(string nif, DateTime? fechaBaja = null)
        {
            // Verifica que exista el cliente
            var cliente = ObtenerPorNIF(nif);
            if(cliente == null)
            {
                throw new InvalidOperationException("El cliente no existe en la base de datos.");
            }

            try
            {
                // Graba la fecha de baja en la propiedad del cliente
                cliente.FechaBaja = fechaBaja ?? DateTime.Now.Date;
                return Actualizar(cliente);

            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"No se ha podido dar de baja el cliente: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene un cliente por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto clente con las propiedades que tenga</returns>
        public Cliente ObtenerPorId(int id)
        {
            return GestorDatos.ObtenerDatosPorId<Cliente>("Clientes", id);
        }

        /// <summary>
        /// Obtiene un cliente por su NIF
        /// </summary>
        /// <param name="nif"></param>
        /// <returns>Objeto cliente con las propiedades que tenga</returns>
        public Cliente ObtenerPorNIF(string nif)
        {
            return GestorDatos.ObtenerDatosPorNIF<Cliente>("Clientes", nif);
        }

        /// <summary>
        /// Permite obtener una lista con todas los clientes segun el parametro 'activos'
        /// </summary>
        /// <returns>Lista con los clientes y sus propiedades</returns>
        public IEnumerable<Cliente> ListarTodos(bool? activos = null)
        {
            // Crea una lista de empresas
            var listaClientes = new List<Cliente>();

            // Carga una tabla con todas las empresas
            DataTable tabla = ConsultarClientes(activos);

            // Va añadiendo cada cliente a la lista, utilizando el mapeador de filas
            foreach(DataRow fila in tabla.Rows)
            {
                var cliente = Utilidades.MapeadorDatos.MapearFila<Cliente>(fila);
                listaClientes.Add(cliente);
            }

            return listaClientes;
        }

        /// <summary>
        /// Metodo para validar que un cliente no sea nulo y que tenga NIF y nombre
        /// </summary>
        /// <param name="cliente"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void ValidarCliente(Cliente cliente)
        {
            // Evita agregar clientes nulos
            if(cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");
            }

            // Validar campos obligatorios
            if(string.IsNullOrEmpty(cliente.Nombre))
            {
                throw new ArgumentException("El nombre del cliente es obligatorio.");
            }

            if(string.IsNullOrEmpty(cliente.NIF))
            {
                throw new ArgumentException("El NIF del cliente es obligatorio.");
            }
        }

        private bool ClienteExiste(string nif)
        {
            string sqlClientes = @"SELECT COUNT(1) FROM Clientes WHERE NIF = @NIF";
            using(var conexion = GestorDatos.AbrirConexion())
            {
                using(var comando = new SQLiteCommand(sqlClientes, conexion))
                {
                    comando.Parameters.AddWithValue("@NIF", nif);
                    var resultado = Convert.ToInt32(comando.ExecuteScalar());
                    return resultado > 0;
                }
            }
        }

        /// <summary>
        /// Devuelve una tabla con todos los clientes y sus datos
        /// </summary>
        /// <returns></returns>
        public DataTable ConsultarClientes(bool? activos)
        {
            string sqlClientes = "SELECT * " + "FROM Clientes "; ;
            string sqlClientesActivos = sqlClientes + " WHERE FechaBaja IS NULL";
            string sqlClientesInactivos = sqlClientes + " WHERE FechaBaja IS NOT NULL";
            if (activos == true)
            {
                return GestorDatos.EjecutarConsulta(sqlClientesActivos);
            }
            else if (activos == false)
            {
                return GestorDatos.EjecutarConsulta(sqlClientesInactivos);
            }
            else
            {
                return GestorDatos.EjecutarConsulta(sqlClientes);
            }
        }


        public enum FormasPago
        {
            Transferencia,
            Domiciliacion,
            Efectivo
        }
    }
}
