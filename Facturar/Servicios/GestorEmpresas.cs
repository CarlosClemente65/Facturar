using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;
using Facturar.Utilidades;

namespace Facturar.Servicios
{
    public class GestorEmpresas
    {
        private static GestorEmpresas _instancia; //Propiedad privada para la instancia única
        private static readonly object _lock = new object(); //Objeto para asegurar la sincronización en entornos multihilo
        private List<Empresa> _empresas = new List<Empresa>();

        //Constructor privado para evitar instanciación externa
        private GestorEmpresas()
        {

        }

        // Propiedad para obtener la instancia única del gestor (Singleton)
        public static GestorEmpresas Instancia
        {
            get
            {
                if(_instancia == null)
                {
                    lock(_lock)
                    {
                        if(_instancia == null)
                        {
                            _instancia = new GestorEmpresas();
                        }
                    }
                }
                return _instancia;
            }
        }
        public void AgregarEmpresa(Empresa empresa)
        {
            //Evitar agregar empresas nulas o con NIF o Nombre vacíos
            if(empresa == null)
            {
                throw new ArgumentNullException(nameof(empresa), "La empresa no puede ser nula.");
            }

            // Evita agregar empresas sin NIF
            if(string.IsNullOrWhiteSpace(empresa.NIF))
            {
                throw new ArgumentException("El NIF de la empresa es obligatorio.", nameof(empresa));
            }

            // Evita agregar empresas sin nombre 
            if(string.IsNullOrWhiteSpace(empresa.Nombre))
            {
                throw new ArgumentException("El nombre de la empresa es obligatorio.", nameof(empresa));
            }

            // Evita agregar empresas con NIF duplicado
            if(_empresas.Any(e => e.NIF == empresa.NIF))
            {
                throw new InvalidOperationException("La empresa ya existe.");
            }

            //Asigna automáticamente un Id único
            empresa.Id = _empresas.Any() ? _empresas.Max(e => e.Id) + 1 : 1;

            //Asigna la fecha de alta
            empresa.FechaAlta = DateTime.Now;
            empresa.FechaBaja = null; // Asegura que la fecha de baja es nula al crear un nuevo cliente

            // Añade la empresa a la lista de empresas
            _empresas.Add(empresa);

            GestorDatos.Instancia.GuardarDatos();
        }

        public void ModificarEmpresa(Empresa empresa)
        {
            // Filtra la lista de empresas activos y la busca por NIF
            var empresaExistente = _empresas.FirstOrDefault(c => c.NIF == empresa.NIF && c.Activo);

            // Controlar que la empresa no sea nula y que exista
            if(empresaExistente == null)
            {
                throw new ArgumentNullException(nameof(empresa), "La empresa no existe o esta dada de baja.");
            }

            // Solo se actualizan los datos de la empresa sin modificar las listas de locales o clientes asociados, ya que estos se gestionan desde sus respectivos gestores
            empresaExistente.Nombre = empresa.Nombre;
            empresaExistente.Direccion = empresa.Direccion;
            empresaExistente.CodigoPostal = empresa.CodigoPostal;
            empresaExistente.Poblacion = empresa.Poblacion;
            empresaExistente.Provincia = empresa.Provincia;
            empresaExistente.Telefono = empresa.Telefono;
            empresaExistente.Email = empresa.Email;
            empresaExistente.PersonaContacto = empresa.PersonaContacto;
            empresaExistente.SerieFactura = empresa.SerieFactura;
            empresaExistente.NumeroFacturaActual = empresa.NumeroFacturaActual;

            // Guarda los cambios en el archivo
            GestorDatos.Instancia.GuardarDatos();
        }

        public void EliminarEmpresa(string nif, DateTime? fechaBaja = null)
        {
            // Busca la empresa por NIF en la lista de empresas activas
            var empresa = _empresas.FirstOrDefault(c => c.NIF == nif && c.Activo);

            // Controlar que la empresa exista y no este ya dada de baja
            if(empresa == null)
            {
                throw new ArgumentNullException(nameof(nif), "La empresa no existe o ya esta dada de baja.");
            }

            // Establece la fecha de baja
            empresa.FechaBaja = fechaBaja ?? DateTime.Now;

            // Guarda los cambios en el archivo
            GestorDatos.Instancia.GuardarDatos();
        }

        public IReadOnlyList<Empresa> ListarEmpresas(bool incluirInactivos = false)
        {
            // Filtra la lista de empresas según el parámetro incluirInactivos
            var empresasFiltrados = incluirInactivos
                ? _empresas.ToList()
                : _empresas.Where(c => c.Activo).ToList();

            return empresasFiltrados.AsReadOnly();
        }

        public Empresa ObtenerEmpresaPorNIF(string nif, bool incluirInactivos = false)
        {
            // Filtra la lista de empresas según el parámetro incluirInactivos
            var empresasFiltrados = incluirInactivos
                ? _empresas.FirstOrDefault(c => c.NIF == nif)
                : _empresas.FirstOrDefault(c => c.NIF == nif && c.Activo);

            // Busca la empresa por NIF en la lista de empresas activas
            var empresa = _empresas.FirstOrDefault(c => c.NIF == nif && c.Activo);

            // Controla que la empresa exista y no este dada de baja
            if(empresa == null)
            {
                throw new ArgumentNullException(nameof(nif), "La empresa no existe o esta dada de baja.");
            }

            // Devuelve la empresa encontrada
            return empresa;
        }

        public IReadOnlyList<Empresa> BuscarEmpresas (string criterio, bool incluirInactivos = false)
        {
            // Filtra la lista de empresas según el parámetro incluirInactivos
            var empresasFiltradas = incluirInactivos
                ? _empresas
                : _empresas.Where(e => e.Activo);

            // Devuleve la lista de empresas que contengan en el nombre o NIF el criterio pasado (case insensitive)
            return empresasFiltradas
                .Where(e => e.Nombre.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                            e.NIF.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList().AsReadOnly();
        }

        public void CargarEmpresas(List<Empresa> listaEmpresas)
        {
            // Metodo para cargar la lista de empresas desde el gestor de datos
            try
            {
                _empresas = listaEmpresas ?? new List<Empresa>();
            }
            catch(Exception ex)
            {
                // Manejo de errores al cargar empresas
                throw new ApplicationException("Error al cargar las empresas.", ex);
            }
        }
    }
}
