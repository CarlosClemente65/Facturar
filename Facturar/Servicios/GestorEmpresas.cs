using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Facturar.Entidades;
using Facturar.Interfaces;

namespace Facturar.Servicios
{
    public class GestorEmpresas : IRepositorioEmpresas
    {

        //Constructor privado para evitar instanciación externa
        private GestorEmpresas()
        {

        }

        public bool Agregar(Empresa entidad)
        {
            try
            {
                // Evita agregar entidades nulas
                if(entidad == null)
                {
                    throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");
                }

                // Validar campos obligatorios
                if(string.IsNullOrEmpty(entidad.Nombre))
                {
                    throw new ArgumentException("El nombre de la empresa es obligatorio.");
                }

                if(string.IsNullOrEmpty(entidad.NIF))
                {
                    throw new ArgumentException("El NIF de la empresa es obligatorio.");
                }

                // Asigna la fecha de alta si no está establecida
                if(entidad.FechaAlta == DateTime.MinValue)
                {
                    //Asigna la fecha de alta
                    entidad.FechaAlta = DateTime.Now;
                }

                entidad.SerieFactura = entidad.SerieFactura ?? string.Empty; // Asigna una serie por defecto si no se proporciona

                entidad.NumeroFacturaActual = 0; // Inicializa el número de factura actual a 0

                // Verifica que no exista ya una empresa con el mismo NIF
                if(EmpresaExiste(entidad.NIF))
                {
                    throw new InvalidOperationException("Ya existe una empresa con ese NIF.");
                }

                // Inserta la nueva empresa en la base de datos
                string sqlInsertar = @"
                    INSERT INTO Empresas (NIF, Nombre, Direccion, CodigoPostal, Poblacion, Provincia, Telefono, Email, PersonaContacto, FechaAlta, FechaBaja, SerieFactura, NumeroFacturaActual)
                    VALUES (@NIF, @Nombre, @Direccion, @CodigoPostal, @Poblacion, @Provincia, @Telefono, @Email, @PersonaContacto, @FechaAlta, @FechaBaja, @SerieFactura, @NumeroFacturaActual);
                    SELECT last_insert_rowid();
                    ";
                using(var conexion = Infraestructura.InicializadorBaseDatos.AbrirConexion())
                {
                    using(var comando = new SQLiteCommand(sqlInsertar, conexion))
                    {
                        comando.Parameters.AddWithValue("@NIF", entidad.NIF);
                        comando.Parameters.AddWithValue("@Nombre", entidad.Nombre);
                        comando.Parameters.AddWithValue("@Direccion", entidad.Direccion ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@CodigoPostal", entidad.CodigoPostal ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@Poblacion", entidad.Poblacion ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@Provincia", entidad.Provincia ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@Telefono", entidad.Telefono ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@Email", entidad.Email ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@PersonaContacto", entidad.PersonaContacto ?? (object)DBNull.Value);
                        comando.Parameters.AddWithValue("@FechaAlta", entidad.FechaAlta);
                        comando.Parameters.AddWithValue("@FechaBaja", DBNull.Value); // Siempre nulo al crear
                        comando.Parameters.AddWithValue("@SerieFactura", entidad.SerieFactura);
                        comando.Parameters.AddWithValue("@NumeroFacturaActual", entidad.NumeroFacturaActual);
                        // Ejecuta el comando y obtiene el Id generado
                        entidad.Id = Convert.ToInt32(comando.ExecuteScalar());
                        if(entidad.Id <= 0)
                        {
                            throw new InvalidOperationException("No se pudo insertar la empresa en la base de datos");
                        }
                        return true; // Indica que la inserción fue exitosa
                    }
                }
            }

            catch(Exception ex)
            {
                // Manejo de la excepción (puede ser logging, rethrow, etc.)
                throw new ApplicationException("Error al agregar la empresa.", ex);
            }
        }



        //public void ModificarEmpresa(Empresa empresa)
        //{
        //    // Filtra la lista de empresas activos y la busca por NIF
        //    var empresaExistente = _empresas.FirstOrDefault(c => c.NIF == empresa.NIF && c.Activo);

        //    // Controlar que la empresa no sea nula y que exista
        //    if(empresaExistente == null)
        //    {
        //        throw new ArgumentNullException(nameof(empresa), "La empresa no existe o esta dada de baja.");
        //    }

        //    // Solo se actualizan los datos de la empresa sin modificar las listas de locales o clientes asociados, ya que estos se gestionan desde sus respectivos gestores
        //    empresaExistente.Nombre = empresa.Nombre;
        //    empresaExistente.Direccion = empresa.Direccion;
        //    empresaExistente.CodigoPostal = empresa.CodigoPostal;
        //    empresaExistente.Poblacion = empresa.Poblacion;
        //    empresaExistente.Provincia = empresa.Provincia;
        //    empresaExistente.Telefono = empresa.Telefono;
        //    empresaExistente.Email = empresa.Email;
        //    empresaExistente.PersonaContacto = empresa.PersonaContacto;
        //    empresaExistente.SerieFactura = empresa.SerieFactura;
        //    empresaExistente.NumeroFacturaActual = empresa.NumeroFacturaActual;

        //    // Guarda los cambios en el archivo
        //    GestorDatos.Instancia.GuardarDatos();
        //}

        //public void EliminarEmpresa(string nif, DateTime? fechaBaja = null)
        //{
        //    // Busca la empresa por NIF en la lista de empresas activas
        //    var empresa = _empresas.FirstOrDefault(c => c.NIF == nif && c.Activo);

        //    // Controlar que la empresa exista y no este ya dada de baja
        //    if(empresa == null)
        //    {
        //        throw new ArgumentNullException(nameof(nif), "La empresa no existe o ya esta dada de baja.");
        //    }

        //    // Establece la fecha de baja
        //    empresa.FechaBaja = fechaBaja ?? DateTime.Now;

        //    // Guarda los cambios en el archivo
        //    GestorDatos.Instancia.GuardarDatos();
        //}

        //public IReadOnlyList<Empresa> ListarEmpresas(bool incluirInactivos = false)
        //{
        //    // Filtra la lista de empresas según el parámetro incluirInactivos
        //    var empresasFiltrados = incluirInactivos
        //        ? _empresas.ToList()
        //        : _empresas.Where(c => c.Activo).ToList();

        //    return empresasFiltrados.AsReadOnly();
        //}

        //public Empresa ObtenerEmpresaPorNIF(string nif, bool incluirInactivos = false)
        //{
        //    // Filtra la lista de empresas según el parámetro incluirInactivos
        //    var empresasFiltrados = incluirInactivos
        //        ? _empresas.FirstOrDefault(c => c.NIF == nif)
        //        : _empresas.FirstOrDefault(c => c.NIF == nif && c.Activo);

        //    // Busca la empresa por NIF en la lista de empresas activas
        //    var empresa = _empresas.FirstOrDefault(c => c.NIF == nif && c.Activo);

        //    // Controla que la empresa exista y no este dada de baja
        //    if(empresa == null)
        //    {
        //        throw new ArgumentNullException(nameof(nif), "La empresa no existe o esta dada de baja.");
        //    }

        //    // Devuelve la empresa encontrada
        //    return empresa;
        //}

        //public IReadOnlyList<Empresa> BuscarEmpresas (string criterio, bool incluirInactivos = false)
        //{
        //    // Filtra la lista de empresas según el parámetro incluirInactivos
        //    var empresasFiltradas = incluirInactivos
        //        ? _empresas
        //        : _empresas.Where(e => e.Activo);

        //    // Devuleve la lista de empresas que contengan en el nombre o NIF el criterio pasado (case insensitive)
        //    return empresasFiltradas
        //        .Where(e => e.Nombre.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
        //                    e.NIF.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0)
        //        .ToList().AsReadOnly();
        //}






        public bool Actualizar(Empresa entidad)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Empresa Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Empresa> ListarTodos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Empresa> ListarLocalesEmpresa(bool activas = true)
        {
            throw new NotImplementedException();
        }

        private bool EmpresaExiste(string nif)
        {
            string sqlEmpresas = @"SELECT COUNT(1) FROM Empresas WHERE NIF = @NIF";
            using(var conexion = Infraestructura.InicializadorBaseDatos.AbrirConexion())
            {
                using(var comando = new SQLiteCommand(sqlEmpresas, conexion))
                {
                    comando.Parameters.AddWithValue("@NIF", nif);
                    var resultado = Convert.ToInt32(comando.ExecuteScalar());
                    return resultado > 0;
                }
            }
        }
    }
}
