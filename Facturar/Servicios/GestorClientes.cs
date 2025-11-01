using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Facturar.Entidades;
using Facturar.Interfaces;
using Utiles = Facturar.Utilidades.UtilesGenerales;

namespace Facturar.Servicios
{
    public class GestorClientes : IRepositorioClientes
    {
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
                //Asigna la fecha de alta
                cliente.FechaAlta = Utiles.ValidarFecha(cliente.FechaAlta);

                // Valida que el cliente no sea nulo, y el NIF y nombre tengan contenido
                ValidarCliente(cliente);

                // Verifica que no exista ya un cliente con el mismo NIF
                if(ClienteExiste(cliente.NIF))
                {
                    throw new InvalidOperationException("Ya existe un cliente con ese NIF en la base de datos.");
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
                string sqlInsertarCliente = "INSERT INTO Clientes " +
                    "(NIF, Nombre, Direccion, CodigoPostal, Poblacion, Provincia, Telefono, Email, PersonaContacto, FechaAlta, FechaBaja, FormaPago, IBAN, Observaciones) " +
                    "VALUES (@NIF, @Nombre, @Direccion, @CodigoPostal, @Poblacion, @Provincia, @Telefono, @Email, @PersonaContacto, @FechaAlta, @FechaBaja, @FormaPago, @IBAN, @Observaciones)";
                var filasInsertadas = Convert.ToInt32(GestorDatos.EjecutarComando(sqlInsertarCliente, parametros));

                if(filasInsertadas <= 0)
                {
                    throw new InvalidOperationException("No se pudo agregar el cliente en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new ApplicationException($"Error al agregar el cliente a la base de datos. {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Permite actualizar los datos de un cliente en la BBDD
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns>True si se ha podido actualizar</returns>
        /// <exception cref="InvalidOperationException"></exception>

        public bool Actualizar(Cliente cliente, bool esBaja = false)
        {
            try
            {
                // Valida que el cliente no sea nulo, y el NIF y nombre tengan contenido
                ValidarCliente(cliente);

                // Verifica que exista un cliente con el NIF proporcionado
                if(!ClienteExiste(cliente.NIF))
                {
                    throw new InvalidOperationException("No existe un cliente con ese NIF en la base de datos.");
                }

                if(!esBaja && !cliente.Activo)
                {
                    throw new InvalidOperationException("La empresa no esta activa");
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
                string sqlActualizar = "UPDATE Clientes SET " +
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

                int filasActualizadas = GestorDatos.EjecutarComando(sqlActualizar, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No ha podido actualizar el cliente en la base de datos");
                }
                return true; // Indica que la inserción fue exitosa
            }

            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al actualizar el cliente en la base de datos: {ex.Message}", ex);
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
                string sqlEliminarCliente = "DELETE FROM Clientes WHERE NIF = @NIF";
                int filasActualizadas = GestorDatos.EjecutarComando(sqlEliminarCliente, parametros);

                if(filasActualizadas == 0)
                {
                    throw new InvalidOperationException("No se ha podido eliminar el cliente en la base de datos");
                }
                return true; // Indica que la eliminacion fue exitosa
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar el cliente en la base de datos: {ex.Message}", ex);
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

            // Verifica que el cliente esté activo
            if(!cliente.Activo)
            {
                throw new InvalidOperationException("La empresa ya está dada de baja.");
            }

            try
            {
                // Graba la fecha de baja en la propiedad del cliente
                cliente.EstablecerFechaBaja(Utiles.ValidarFecha(fechaBaja));
                return Actualizar(cliente, esBaja: true);

            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al dar de baja el cliente en la base de datos: {ex.Message}", ex);
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

            // Hace la consulta de clientes segun el parametro 'activos'
            string sqlClientes = "SELECT * " + "FROM Clientes ";

            // Ajusta la consulta segun el estado solicitado (activo, inactivo o todos)
            if(activos.HasValue)
            {
                sqlClientes += activos.Value
                    ? " WHERE FechaBaja IS NULL"
                    : " WHERE FechaBaja IS NOT NULL";
            }

            DataTable tabla = GestorDatos.EjecutarConsulta(sqlClientes);

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

    }
}
