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
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Alta);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Modificacion);
            Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Baja);
            //Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Eliminacion);
            Pruebas.LanzaPruebas(Pruebas.Entidades.Empresas, Pruebas.Procesos.Consulta);

            Environment.Exit(0);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmBase());
        }
    }
}
