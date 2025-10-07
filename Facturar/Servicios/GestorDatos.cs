using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Facturar.Entidades;

namespace Facturar.Servicios
{
    public class GestorDatos
    {
        private static GestorDatos _instancia; // Instancia unica del gestor de datos
        private static readonly object _lock = new object(); // Objeto para asegurar la utilizacion en multihilo

        private GestorClientes _gestorClientes;
        private GestorEmpresas _gestorEmpresas;
        private GestorLocales _gestorLocales;

        //Propiedades publicas de solo lectura para acceder a los gestores
        public GestorClientes GestorClientes => _gestorClientes;
        public GestorEmpresas GestorEmpresas => _gestorEmpresas;
        public GestorLocales GestorLocales => _gestorLocales;


        private GestorDatos()
        {
            _gestorClientes = GestorClientes.Instancia;
            // _gestorEmpresas = GestorEmpresas.Instancia; // Descomentar cuando se implemente el singleton en GestorEmpresas
            // _gestorLocales = GestorLocales.Instancia; // Descomentar cuando se implemente el singleton en GestorLocales

            CargarDatos();

        }

        public static GestorDatos Instancia
        {
            get
            {
                if(_instancia == null)
                {
                    lock(_lock)
                    {
                        if(_instancia == null)
                        {
                            _instancia = new GestorDatos();
                        }
                    }
                }
                return _instancia;
            }
        }

        public void CargarDatos()
        {
            try
            {
                // Carga los datos desde el archivo JSON
                var json = File.ReadAllText(Utilidades.Configuracion.FicheroDatos);
                var datos = JsonSerializer.Deserialize<Infraestructura.RepositorioDatos>(json);

                if(datos != null)
                {
                    //_gestorClientes.CargarClientes(datos.Clientes);
                    _gestorEmpresas.CargarEmpresas(datos.Empresas); 
                    _gestorLocales.CargarLocales(datos.Locales); // Descomentar cuando se implemente el singleton en GestorLocales
                }
            }

            catch(Exception ex)
            {
                throw new InvalidOperationException("Error al cargar los clientes: " + ex.Message);
            }
        }
        public void GuardarDatos()
        {
            // Obtener los datos actuales de los gestores
            var datos = new Infraestructura.RepositorioDatos
            {
                Clientes = _gestorClientes.ListarClientes().ToList(),
                Empresas = _gestorEmpresas.ListarEmpresas().ToList(),
                Locales = _gestorLocales.ListarLocales().ToList()
            };

            // Crear copia de seguridad si existe
            if(File.Exists(Utilidades.Configuracion.FicheroDatos))
            {
                string rutaBackup = Utilidades.Configuracion.FicheroCopiasSeguridad(Utilidades.Configuracion.FicheroDatos);
                File.Copy(Utilidades.Configuracion.FicheroDatos, rutaBackup);
            }

            // Serializar y guardar
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };
            var json = JsonSerializer.Serialize(datos, opciones);
            File.WriteAllText(Utilidades.Configuracion.FicheroDatos, json);
        }
    }
}
