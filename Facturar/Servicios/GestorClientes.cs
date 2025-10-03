using System;
using System.Collections.Generic;
using System.Linq;
using Facturar.Entidades;
using Facturar.Utilidades;

namespace Facturar.Servicios
{
    public class GestorClientes
    {
        private static GestorClientes _instancia; //Propiedad privada para la instancia única
        private static readonly object _lock = new object(); //Objeto para asegurar la sincronización en entornos multihilo

        private List<Cliente> _clientes = new List<Cliente>();

        // Constructor privado para evitar instanciación externa
        private GestorClientes()
        {

        }

        // Propiedad para obtener la instancia única del gestor (Singleton)
        public static GestorClientes Instancia
        {
            get
            {
                if(_instancia == null)
                {
                    lock(_lock)
                    {
                        if(_instancia == null)
                        {
                            _instancia = new GestorClientes();
                        }
                    }
                }
                return _instancia;
            }
        }


        public void AgregarCliente(Cliente cliente, Empresa empresa)
        {
            // Evita agregar clientes nulos
            if(cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");
            }

            // Evita agregar clientes si la empresa no existe
            if(empresa == null)
            {
                throw new ArgumentNullException(nameof(empresa), "La empresa no puede ser nula.");
            }

            // Evita agregar clientes sin NIF
            if(string.IsNullOrWhiteSpace(cliente.NIF))
            {
                throw new ArgumentException("El NIF del cliente es obligatorio.", nameof(cliente));
            }

            // Evita agregar clientes sin nombre
            if(string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new ArgumentException("El nombre del cliente es obligatorio.", nameof(cliente));
            }

            // Evita agregar clientes con NIF duplicados
            if(_clientes.Any(c => c.NIF == cliente.NIF))
            {
                throw new InvalidOperationException("El cliente ya existe.");
            }

            //Asigna automáticamente un Id único
            cliente.Id = _clientes.Any() ? _clientes.Max(c => c.Id) + 1 : 1;

            //Asigna la fecha de alta
            cliente.FechaAlta = DateTime.Now;
            cliente.FechaBaja = null; // Asegura que la fecha de baja es nula al crear un nuevo cliente

            // Añade la empresa a la lista de empresas a la que pertenece el cliente
            cliente.Empresas.Add(empresa);

            // Añade el cliente a la lista de clientes de la empresa
            if(!empresa.Clientes.Contains(cliente))
            {
                empresa.Clientes.Add(cliente);
            }

            // Agrega el cliente a la lista principal de clientes
            _clientes.Add(cliente);

            // Guarda los cambios en el archivo
            GestorDatos.Instancia.GuardarDatos();
        }

        public void AsignaClienteAEmpresa(Cliente cliente, Empresa empresa)
        {
            // Permite modificar la asignación de un cliente a una empresa existente
            // Evita agregar clientes nulos
            if(cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");
            }

            // Evita agregar clientes si la empresa no existe
            if(empresa == null)
            {
                throw new ArgumentNullException(nameof(empresa), "La empresa no puede ser nula.");
            }

            // Verifica que el cliente exista en la lista de clientes
            var clienteExistente = _clientes.FirstOrDefault(c => c.NIF == cliente.NIF);
            if(clienteExistente == null)
            {
                throw new InvalidOperationException("El cliente no existe.");
            }

            // Verifica que el cliente este activo
            if(!clienteExistente.Activo)
            {
                throw new InvalidOperationException("El cliente no esta activo.");
            }

            // Añade la empresa a la lista de empresas a la que pertenece el cliente si no está ya asignada
            if(!clienteExistente.Empresas.Contains(empresa))
            {
                clienteExistente.Empresas.Add(empresa);
            }

            // Añade el cliente a la lista de clientes de la empresa si no está ya asignado
            if(!empresa.Clientes.Contains(clienteExistente))
            {
                empresa.Clientes.Add(clienteExistente);
            }

            // Guarda los cambios en el archivo
            GestorDatos.Instancia.GuardarDatos();
        }

        public void ModificarCliente(Cliente cliente)
        {
            // Filtra la lista de clientes activos y lo busca por NIF
            var clienteExistente = _clientes.FirstOrDefault(c => c.NIF == cliente.NIF && c.Activo);

            // Contorla que el cliente no sea nulo
            if(clienteExistente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no existe o esta dado de baja.");
            }

            // Actualiza los datos del cliente
            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Direccion = cliente.Direccion;
            clienteExistente.CodigoPostal = cliente.CodigoPostal;
            clienteExistente.Poblacion = cliente.Poblacion;
            clienteExistente.Provincia = cliente.Provincia;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Email = cliente.Email;
            clienteExistente.PersonaContacto = cliente.PersonaContacto;
            clienteExistente.FormaPago = cliente.FormaPago;
            clienteExistente.IBAN = cliente.IBAN;
            clienteExistente.Observaciones = cliente.Observaciones;

            // Guarda los cambios en el archivo
            GestorDatos.Instancia.GuardarDatos();
        }

        public void EliminarCliente(string nif, DateTime? fechaBaja = null)
        {
            // Controla que el NIF exista en la lista
            var cliente = _clientes.FirstOrDefault(c => c.NIF == nif && c.Activo);

            // Controla que el cliente exista y no este dado de baja
            if(cliente == null)
            {
                throw new ArgumentNullException(nameof(nif), "El cliente no existe o ya esta dado de baja.");
            }

            //Establece la fecha de baja
            cliente.FechaBaja = fechaBaja ?? DateTime.Now;

            // Buscar la empresa a la que pertenece el cliente y eliminarlo de su lista de clientes
            var empresas = GestorEmpresas.Instancia.ListarEmpresas(false)
                   .Where(e => e.Clientes.Contains(cliente));

            foreach(var empresa in empresas)
            {
                empresa.Clientes.Remove(cliente);
            }

            // Guarda los cambios en el archivo
            GestorDatos.Instancia.GuardarDatos();
        }

        public IReadOnlyList<Cliente> ListarClientes(bool incluirInactivos = false)
        {
            // Filtra la lista de clientes según el parámetro 'incluirActivos'
            var clientesFiltrados = incluirInactivos
                ? _clientes
                : _clientes.Where(c => c.Activo).ToList();

            // Devuelve una lista de clientes de solo lectura para evitar modificaciones externas
            return clientesFiltrados.AsReadOnly();
        }

        public Cliente ObtenerClientePorNIF(string nif, bool incluirInactivos = false)
        {
            //Filtra los clientes según el parámetro 'incluirInactivos'
            var cliente = incluirInactivos
                ? _clientes.FirstOrDefault(c => c.NIF == nif)
                : _clientes.FirstOrDefault(c => c.NIF == nif && c.Activo);

            // Controla que el cliente exista o no este dado de baja
            if(cliente == null)
            {
                throw new ArgumentNullException(nameof(nif), "El cliente no existe o esta dado de baja.");
            }

            // Devuelve el cliente encontrado
            return cliente;
        }

        public IReadOnlyList<Cliente> BuscarClientes(string criterio, bool incluirInactivos = false)
        {
            // Filtra los clientes según el parámetro 'incluirInactivos'
            var clientesFiltrados = incluirInactivos
                ? _clientes
                : _clientes.Where(c => c.Activo);

            // Devuelve la lista de clientes que contengan en el nombre o NIF el criterio pasado (case insensitive)
            return clientesFiltrados
                .Where(c => c.Nombre.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                            c.NIF.Contains(criterio, StringComparison.OrdinalIgnoreCase))
                .ToList().AsReadOnly();
        }

        public void CargarClientes(List<Cliente> listaClientes)
        {
            // Metodo para cargar la lista de clientes desde el gestor de datos
            try
            {
                _clientes = listaClientes ?? new List<Cliente>();

                // Actualiza la lista de clientes en cada empresa
                foreach(var cliente in _clientes)
                {
                    foreach(var empresa in cliente.Empresas)
                    {
                        if(!empresa.Clientes.Contains(cliente))
                        {
                            empresa.Clientes.Add(cliente);
                        }
                    }
                }
            }

            catch(Exception ex)
            {
                throw new InvalidOperationException("Error al cargar los clientes: " + ex.Message);
            }
        }
    }
}
