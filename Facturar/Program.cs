using System;
using System.Windows.Forms;
using Facturar.Presentacion;
using Facturar.Servicios;
using Facturar.Utilidades;

namespace Facturar
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GestorDatos.ChequeoBaseDatos();

            // Procesos de pruebas
            // Empresas
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Alta);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Modificacion);
            Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Baja);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Eliminacion);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Consulta);

            // Clientes
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Clientes, Pruebas.Procesos.Alta);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Clientes, Pruebas.Procesos.Modificacion);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Clientes, Pruebas.Procesos.Baja);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Clientes, Pruebas.Procesos.Eliminacion);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Clientes, Pruebas.Procesos.Consulta);

            // Locales
            Pruebas.LanzaPruebas(Pruebas.Entidades.Locales, Pruebas.Procesos.Alta);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Locales, Pruebas.Procesos.Modificacion);
            Pruebas.LanzaPruebas(Pruebas.Entidades.Locales, Pruebas.Procesos.Baja);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Locales, Pruebas.Procesos.Consulta);
            Pruebas.LanzaPruebas(Pruebas.Entidades.Locales, Pruebas.Procesos.ConsultaActivas);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Locales, Pruebas.Procesos.Eliminacion);

            Environment.Exit(0);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmBase());
        }
    }
}
