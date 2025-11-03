using System;
using System.IO;

namespace Facturar.Utilidades
{
    public static class Configuracion
    {
        public static string CarpetaBase
        {
            get
            {
                string Carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "FacturarApp");
                return ChequeoCarpeta(Carpeta);
            }
        }

        public static string CarpetaBackup
        {
            get
            {
                string carpeta = Path.Combine(CarpetaBase, "Backups");
                return ChequeoCarpeta(carpeta);
            }
        }

        public static string CarpetaDatos
        {
            get
            {
                string carpeta = Path.Combine(CarpetaBase, "Datos");
                return ChequeoCarpeta(carpeta);
            }
        }

        public static string CarpetaLogs
        {
            get
            {
                string carpeta = Path.Combine(CarpetaBase, "Logs");
                return ChequeoCarpeta(carpeta);
            }
        }

        /// <summary>
        /// Comprueba si existe la carpeta pasada y si no la crea
        /// </summary>
        /// <param name="Carpeta"></param>
        /// <returns></returns>
        private static string ChequeoCarpeta(string Carpeta)
        {
            if(!Directory.Exists(Carpeta))
            {
                Directory.CreateDirectory(Carpeta);
            }
            return Carpeta;
        }

        public static string FicheroCopiasSeguridad(string fichero)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(CarpetaBackup, $"{Path.GetFileNameWithoutExtension(fichero)}_{timestamp}.json");
        }
        public static int MaximoCopiasSeguridad { get; set; } = 7; //Numero maximo de copias de seguridad a mantener
    }
}
