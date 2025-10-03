using System;
using System.IO;

namespace Facturar.Utilidades
{
    public static class Configuracion
    {
        public static string CarpetaDatos
        {
            get
            {
                string carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FacturarApp");

                if(!System.IO.Directory.Exists(carpeta))
                {
                    System.IO.Directory.CreateDirectory(carpeta);
                }
                return carpeta;
            }
        }
        public static string CarpetaCopiasSeguridad
        {
            get
            {
                string carpeta = System.IO.Path.Combine(CarpetaDatos, "Backups");
                if(!System.IO.Directory.Exists(carpeta))
                {
                    System.IO.Directory.CreateDirectory(carpeta);
                }
                return carpeta;
            }
        }

        public static string FicheroDatos
        {
            get
            {
                return System.IO.Path.Combine(CarpetaDatos, "datos.json");
            }
        }

        public static string FicheroFacturas
        {
            get
            {
                return System.IO.Path.Combine(CarpetaDatos, "facturas.json");
            }
        }

        public static string FicheroCopiasSeguridad(string fichero)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(CarpetaCopiasSeguridad, $"{Path.GetFileNameWithoutExtension(fichero)}_{timestamp}.json");
        }
        public static int MaximoCopiasSeguridad { get; set; } = 7; //Numero maximo de copias de seguridad a mantener
    }
}
