using System;
using System.Collections.Generic;
using System.Linq;
using Facturar.Entidades;

namespace Facturar.Servicios
{
    public class GestorLocales
    {
        private static GestorLocales _instancia; //Propiedad privada para la instancia única
        private static readonly object _lock = new object(); //Objeto para asegurar la sincronización en entornos multihilo

        private List<Local> _locales = new List<Local>();

        //Constructor privado para evitar instanciación externa
        private GestorLocales()
        {
        }

        // Propiedad para obtener la instancia única del gestor (Singleton)
        public static GestorLocales Instancia
        {
            get
            {
                if(_instancia == null)
                {
                    lock(_lock)
                    {
                        if(_instancia == null)
                        {
                            _instancia = new GestorLocales();
                        }
                    }
                }
                return _instancia;
            }
        }


        public void AgregarLocal(Local local)
        {
            //Evitar agregar locales nulos o con Descripcion vacía
            if(local == null)
            {
                throw new ArgumentNullException(nameof(local), "El local no puede ser nulo.");
            }

            // Evita agregar locales sin Empresa asociada
            if(local.EmpresaId == null)
            {
                throw new ArgumentException("El local debe estar asociado a una empresa.", nameof(local));
            }

            // Evita agregar sin Descripcion
            if(string.IsNullOrWhiteSpace(local.Descripcion))
            {
                throw new ArgumentException("La descripcon del local no puede estar vacía.", nameof(local));
            }

            // Asignar un Id único al local (simple incremento basado en el conteo actual)
            local.Id = _locales.Any() ? _locales.Max(l => l.Id) + 1 : 1;

            //Asigna la fecha de alta
            local.FechaAlta = DateTime.Now;
            local.FechaBaja = null; // Asegura que la fecha de baja es nula al crear un nuevo local

            _locales.Add(local);

            //// Añade el local a la lista de locales de la empresa
            //if(local.Empresa != null)
            //{
            //    local.Empresa.Locales.Add(local);
            //}

            GestorDatos.Instancia.GuardarDatos();
        }

        public void ModificarLocal(Local local)
        {
            // Filtra la lista de locales activos y la busca por Id
            var localExistente = _locales.FirstOrDefault(l => l.Id == local.Id && l.Activo);

            // Controlar que el local no sea nulo y que exista
            if(localExistente == null)
            {
                throw new ArgumentNullException(nameof(local), "El local no existe o esta dado de baja.");
            }

            // Guardar referencia a la empresa antigua
            var empresaAntigua = localExistente.EmpresaId;

            // Actualiza las propiedades del local existente
            localExistente.Descripcion = local.Descripcion;
            localExistente.Direccion = local.Direccion;
            localExistente.CodigoPostal = local.CodigoPostal;
            localExistente.Poblacion = local.Poblacion;
            localExistente.Provincia = local.Provincia;
            localExistente.EmpresaId = local.EmpresaId;
            localExistente.ImporteAlquiler = local.ImporteAlquiler;
            localExistente.Observaciones = local.Observaciones;

            //// Si la empresa ha cambiado, actualizar listas de locales
            //if(empresaAntigua != null && empresaAntigua != local.EmpresaId)
            //{
            //    empresaAntigua.Locales.Remove(localExistente); // quitar de la antigua
            //    local.EmpresaId.Locales.Add(localExistente);     // añadir a la nueva
            //}

            // Guarda los cambios en el archivo
            GestorDatos.Instancia.GuardarDatos();
        }

        public void EliminarLocal(int id, DateTime? fechaBaja = null)
        {
            // Filtra la lista de locales activos y la busca por Id
            var local = _locales.FirstOrDefault(l => l.Id == id && l.Activo);

            // Controlar que el local exista y esté activo
            if(local == null)
            {
                throw new ArgumentException("El local no existe o ya está dado de baja.", nameof(id));
            }

            // Establece la fecha de baja 
            local.FechaBaja = fechaBaja ?? DateTime.Now;

            //// Elimina el local de la lista de locales de la empresa
            //if(local.Empresa != null)
            //{
            //    local.Empresa.Locales.Remove(local);
            //}

            // Guarda los cambios en el archivo
            GestorDatos.Instancia.GuardarDatos();
        }
        public IReadOnlyList<Local> ListarLocales(bool incluirInactivos = false)
        {
            // Filtra la lista de locales según el parámetro incluirInactivos
            var localesFiltrados = incluirInactivos 
                ? _locales 
                : _locales.Where(l => l.Activo).ToList();

            // Devuelve una lista de solo lectura para evitar modificaciones externas
            return _locales.AsReadOnly();
        }

        public void CargarLocales(List<Local> listaLocales)
        {
            _locales = listaLocales ?? new List<Local>();

            // Se obtiene la lista de empresas para actualizar sus listas de locales
            var empresas = GestorEmpresas.Instancia.ListarEmpresas(true);

            // Asigna cada local a su empresa correspondiente
            foreach(var local in _locales)
            {
                // Solo si local.Empresa no es null
                if(local.EmpresaId != null)
                {
                    // Busca la empresa en la lista de empresas
                    var empresa = empresas.FirstOrDefault(e => e.Id == local.EmpresaId);

                    //// Si la empresa existe y no contiene ya el local, lo añade a su lista de locales
                    //if(empresa != null && !empresa.Locales.Contains(local))
                    //{
                    //    empresa.Locales.Add(local);
                    //}
                }
            }
        }
    }
}
