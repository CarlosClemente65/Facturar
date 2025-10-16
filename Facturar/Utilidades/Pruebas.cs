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
        public static void GestionBD(string proceso, string tabla)
        {
            switch(tabla.ToLower())
            {
                case "empresas":
                    ProbarEmpresas(proceso);
                    break;

                case "locales":

                    break;

                case "clientes":

                    break;

                case "contratos":

                    break;
            }
        }

        private static void ProbarEmpresas(string proceso)
        {
            var repo = GestorDatos.AbrirConexion();
            switch(proceso.ToLower())
            {
                case "alta":
                    var empresa = new Empresa
                    {
                        Nombre = "Empresa de pruebas",
                        NIF = "05100001G",
                        Direccion = "Calle Mayor, 1",
                        CodigoPostal = "02002",
                        Poblacion = "Albacete",
                        Provincia = "Albacete",
                        Telefono = "666333222",
                        Email = "correo@correo.com",
                        PersonaContacto = "Persona contacto",
                        SerieFactura = "A"
                    };

                    break;

                case "baja":

                    break;

                case "modificacion":

                    break;

                case "consulta":

                    break;
            }
        }
    }
}
