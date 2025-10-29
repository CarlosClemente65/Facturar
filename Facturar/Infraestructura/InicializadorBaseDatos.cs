using System.Data.SQLite;
using System.IO;
using Facturar.Servicios;

namespace Facturar.Infraestructura
{
    public static class InicializadorBaseDatos
    {
        // Configuracion para la creacion de las tablas
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
                        NumeroFacturaActual INTEGER
                    );
                ";

        static string sqlLocales = @"
                    CREATE TABLE IF NOT EXISTS Locales (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        IdEmpresa INTEGER NOT NULL,
                        Descripcion TEXT NOT NULL,
                        Direccion TEXT,
                        CodigoPostal TEXT,
                        Poblacion TEXT,
                        Provincia TEXT,
                        ImporteAlquiler DECIMAL,
                        Observaciones TEXT,
                        FechaAlta DATETIME NOT NULL,
                        FechaBaja DATETIME,
                        FOREIGN KEY(EmpresaId) REFERENCES Empresas(Id)
                    );
                ";

        static string sqlClientes = @"
                    CREATE TABLE IF NOT EXISTS Clientes (
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
                        PorcentajeRevision DECIMAL,
                        PrecioRevisado DECIMAL NOT NULL,
                        Observaciones TEXT,
                        FOREIGN KEY(ContratoId) REFERENCES Contratos(Id)
                    );
                ";

        static string sqlFacturas = @"
                    CREATE TABLE IF NOT EXISTS Facturas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        IdEmpresa INTEGER NOT NULL,
                        IdCliente INTEGER NOT NULL,
                        FechaFactura DATETIME NOT NULL,
                        SerieFactura TEXT,
                        NumeroFactura TEXT NOT NULL,
                        TotalBase DECIMAL,
                        TotalIVA DECIMAL,
                        TotalIRPF DECIMAL,
                        TotalFactura DECIMAL,       
                        Observaciones TEXT,
                        FOREIGN KEY(IdEmpresa) REFERENCES Empresas(Id),
                        FOREIGN KEY(IdCliente) REFERENCES Clientes(Id)
                    );
                ";

        static string sqlLineasFactura = @"
                    CREATE TABLE IF NOT EXISTS LineasFactura (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        IdFactura INTEGER NOT NULL,
                        Descripcion TEXT,
                        Cantidad DECIMAL,
                        PrecioUnitario DECIMAL,
                        Subtotal DECIMAL,
                        TipoIVA DECIMAL,
                        CuotaIVA DECIMAL,
                        TipoIRPF DECIMAL,
                        CuotaIRPF DECIMAL,
                        TotalLinea DECIMAL,
                        FOREIGN KEY(IdFactura) REFERENCES Facturas(Id)
                    );
                ";


        // Inicializar la base de datos y crea las tablas
        public static void Inicializar(string rutaBD)
        {
            SQLiteConnection.CreateFile(rutaBD);
            CrearTablas();
        }

        private static void CrearTablas()
        {
            using(var conexion = GestorDatos.AbrirConexion())
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

                using(var comando = new SQLiteCommand(sqlFacturas, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                using(var comando = new SQLiteCommand(sqlLineasFactura, conexion))
                {
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
