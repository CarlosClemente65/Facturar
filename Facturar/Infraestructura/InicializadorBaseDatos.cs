using System;
using System.Data.SQLite;
using System.IO;

namespace Facturar.Infraestructura
{
    public static class InicializadorBaseDatos
    {
        private static readonly string rutaBD = "./datos/facturacion.db";
        private static readonly string cadenaConexion = $"Data Source={rutaBD};Version=3;";

        static string sqlEmpresas = @"
                    CREATE TABLE IF NOT EXISTS Empresas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        NIF TEXT NOT NULL UNIQUE,
                        Nombre TEXT NOT NULL,
                        Direccion TEXT,
                        CodigoPostal TEXT,
                        Poblacion TEXT,
                        Provincia TEXT,
                        Telefono TEXT,
                        Email TEXT,
                        PersonaContacto TEXT,
                        FechaAlta DATETIME NOT NULL,
                        FechaBaja DATETIME,
                        SerieFactura TEXT,
                        NumeroFacturaActual INTEGER NOT NULL
                    );
                ";

        static string sqlLocales = @"
                    CREATE TABLE IF NOT EXISTS Locales (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        EmpresaId INTEGER NOT NULL,
                        Descripcion TEXT,
                        Direccion TEXT NOT NULL,
                        CodigoPostal TEXT,
                        Poblacion TEXT,
                        Provincia TEXT,
                        ImporteAlquiler DECIMAL NOT NULL,
                        Observaciones TEXT,
                        FechaAlta DATETIME NOT NULL,
                        FechaBaja DATETIME,
                        FOREIGN KEY(EmpresaId) REFERENCES Empresas(Id)
                    );
                ";

        static string sqlClientes = @"
                    CREATE TABLE IF NOT EXISTS Clientes (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        NIF TEXT NOT NULL,
                        Nombre TEXT NOT NULL,
                        Direccion TEXT,
                        CodigoPostal TEXT,
                        Poblacion TEXT,
                        Provincia TEXT,
                        Telefono TEXT,
                        Email TEXT,
                        PersonaContacto TEXT,
                        FechaAlta DATETIME NOT NULL,
                        FechaBaja DATETIME, 
                        FormaPago TEXT,
                        IBAN TEXT,
                        Observaciones TEXT
                    );
                ";

        static string sqlClientesEmpresas = @"
                    CREATE TABLE IF NOT EXISTS ClientesEmpresas (
                        ClienteId INTEGER NOT NULL,
                        EmpresaId INTEGER NOT NULL,
                        PRIMARY KEY (ClienteId, EmpresaId),
                        FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
                        FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id)
                    );
                ";

        static string sqlContratos = @"
                    CREATE TABLE IF NOT EXISTS Contratos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        EmpresaId INTEGER NOT NULL,
                        ClienteId INTEGER NOT NULL,
                        LocalId INTEGER NOT NULL,
                        PrecioMensual DECIMAL NOT NULL,
                        FechaInicio DATETIME NOT NULL,
                        FechaFin DATETIME,
                        Observaciones TEXT,
                        FOREIGN KEY(EmpresaId) REFERENCES Empresas(Id),
                        FOREIGN KEY(ClienteId) REFERENCES Clientes(Id),
                        FOREIGN KEY(LocalId) REFERENCES Locales(Id)
                    );
                ";

        static string sqlRevisionesContrato = @"
                    CREATE TABLE IF NOT EXISTS RevisionesContrato (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ContratoId INTEGER NOT NULL,
                        FechaRevision DATETIME NOT NULL,
                        PrecioAnterior DECIMAL NOT NULL,
                        PorcentajeRevision DECIMAL NOT NULL,
                        PrecioRevisado DECIMAL NOT NULL,
                        FOREIGN KEY(ContratoId) REFERENCES Contratos(Id)
                    );
                ";

        public static void Inicializar()
        {
            if(!File.Exists(rutaBD))
            {
                // Crear la carpeta si no existe
                var carpeta = Path.GetDirectoryName(rutaBD);
                if(!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                SQLiteConnection.CreateFile(rutaBD);
                CrearTablas();
            }
        }

        private static void CrearTablas()
        {
            using(var conexion = AbrirConexion())
            {
                // Creacion de las tablas
                using(var comando = new SQLiteCommand(sqlEmpresas, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                using(var comando = new SQLiteCommand(sqlLocales, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                using(var comando = new SQLiteCommand(sqlClientes, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                using(var comando = new SQLiteCommand(sqlClientesEmpresas, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                using(var comando = new SQLiteCommand(sqlContratos, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                using(var comando = new SQLiteCommand(sqlRevisionesContrato, conexion))
                {
                    comando.ExecuteNonQuery();
                }
            }
        }

        public static SQLiteConnection AbrirConexion()
        {
            var conexion = new SQLiteConnection(cadenaConexion);
            conexion.Open();

            // Activar foreign keys
            using(var comando = new SQLiteCommand("PRAGMA foreign_keys = ON;", conexion))
            {
                comando.ExecuteNonQuery();
            }

            return conexion;
        }
    }
}
