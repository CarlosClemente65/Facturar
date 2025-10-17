using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;
using Facturar.Servicios;


namespace Facturar.Utilidades
{
    public static class Pruebas
    {
        public static void LanzaPruebas(Entidades entidad, Procesos proceso)
        {
            switch(entidad)
            {
                case Entidades.Empresas:
                    ProbarEmpresas(proceso);
                    break;

                case Entidades.Locales:
                    break;

                case Entidades.Clientes:
                    break;

                case Entidades.Contratos:

                    break;

                case Entidades.BaseDatos:
                    break;

            }
        }

        private static void ProbarEmpresas(Procesos proceso)
        {
            var repo = GestorDatos.AbrirConexion();
            var empresa = new Empresa();
            var gestor = new GestorEmpresas();
            bool resultado;
            switch(proceso)
            {
                case Procesos.Alta:
                    empresa.Nombre = "CLEMENTE RODRIGUEZ, CARLOS";
                    empresa.NIF = "05196375P";
                    empresa.Direccion = "Calle Mayor, 1";
                    empresa.CodigoPostal = "02002";
                    empresa.Poblacion = "Albacete";
                    empresa.Provincia = "Albacete";
                    empresa.Telefono = "666333222";
                    empresa.Email = "correo@correo.com";
                    empresa.PersonaContacto = "Persona contacto";
                    empresa.SerieFactura = "A";

                    resultado = gestor.Agregar(empresa);

                    break;

                case Procesos.Baja:
                    resultado = gestor.BajaEmpresa("05100001G");
                    break;

                case Procesos.Modificacion:
                    empresa.Nombre = "Empresa de pruebas modificada";
                    empresa.NIF = "05100001G";
                    empresa.Direccion = "Calle Mayor, 1";
                    empresa.CodigoPostal = "02002";
                    empresa.Poblacion = "Albacete";
                    empresa.Provincia = "Albacete";
                    empresa.Telefono = "666333222";
                    empresa.Email = "correo@correo.com";
                    empresa.PersonaContacto = "Persona contacto";
                    empresa.SerieFactura = "A";

                    resultado = gestor.Actualizar(empresa);
                    break;


                case Procesos.Consulta:
                   List<Empresa> ConsultaEmpresa = gestor.ListarTodos().ToList();
                    break;

                case Procesos.ConsultaActivas:
                    List<Empresa> ConsultaActivas = gestor.ListarActivas().ToList();
                    break;

                case Procesos.Eliminacion:
                    resultado = gestor.Eliminar("05100001G");
                    break;
            }
        }

        public enum Entidades
        {
            Empresas = 0,
            Clientes = 1,
            Contratos = 2,
            Locales = 3,
            BaseDatos = 4
        }

        public enum Procesos
        {
            Alta = 0,
            Baja = 1,
            Modificacion = 2,
            Consulta = 3,
            ConsultaActivas= 4,
            Eliminacion = 5
        }
    }
}
