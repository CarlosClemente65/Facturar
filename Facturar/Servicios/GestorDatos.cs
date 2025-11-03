using System.Data;
using System.Data.SQLite;
using System.IO;
using Facturar.Utilidades;
using Facturar.Infraestructura;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Facturar.Servicios
{
    public static class GestorDatos
    {
        // Propiedades para inicializar la base de datos
        public static readonly string rutaBD = Path.Combine(Configuracion.CarpetaDatos, "FacturarApp.db");
        public static readonly string cadenaConexion = $"Data Source={rutaBD};Version=3;";


        public static void ChequeoBaseDatos()
        {
            if(!File.Exists(rutaBD))
            {
                InicializadorBaseDatos.Inicializar(rutaBD);
            }
        }

        /// <summary>
        /// Abre y devuelve una conexión SQLite usando la configuración general.
        /// </summary>
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

        /// <summary>
        /// Ejecuta una consulta SQL sin devolver resultados (INSERT, UPDATE, DELETE).
        /// </summary>
        public static int EjecutarComando(string sql, params SQLiteParameter[] parametros)
        {
            using(var conexion = AbrirConexion())
            {
                using(var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddRange(parametros);
                    return comando.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL que devuelve un solo valor (por ejemplo, COUNT o MAX).
        /// </summary>
        public static object EjecutarComandoValorUnico(string sql, params SQLiteParameter[] parametros)
        {
            // Se puede usar para contar registros o maximos (SELECT COUNT(*) FROM Clientes o SELECT MAX(FechaAlta) FROM Empresas)
            using(var conexion = AbrirConexion())
            {
                using(var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddRange(parametros);
                    return comando.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL que devuelve un conjunto de resultados en una tabla (SELECT).
        /// </summary>
        public static DataTable EjecutarConsulta(string sql, params SQLiteParameter[] parametros)
        {
            using(var conexion = AbrirConexion())
            {
                using(var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddRange(parametros);
                    using(var adaptador = new SQLiteDataAdapter(comando))
                    {
                        var tabla = new DataTable();
                        adaptador.Fill(tabla);
                        return tabla;
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene los datos de la tabla pasada segun el Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nombreTabla"></param>
        /// <param name="id"></param>
        /// <returns>Objeto del tipo que corresponda a T</returns>
        public static T ObtenerDatosPorId<T>(string nombreTabla, int id) where T : new()
        {
            // Hace la consulta a la base de datos de la tabla pasada seleccionado por el Id
            string sqlConsulta = $"SELECT * FROM {nombreTabla} WHERE Id = @Id";
            var parametros = new[] { new SQLiteParameter("@Id", id) };

            // Se almacena el resultado en la tabla que luego se mapea al objeto pasado 'T'
            DataTable tabla = EjecutarConsulta(sqlConsulta, parametros);

            if(tabla.Rows.Count == 0)
            {
                return default(T); // Devuelve null si T es una clase
            }

            // Mapea la fila obtenida en la tabla anterior al tipo de objeto pasado 'T'
            return MapeadorDatos.MapearFila<T>(tabla.Rows[0]);
        }


        /// <summary>
        /// Obtiene los datos de la tabla pasada segun el Nif
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nombreTabla"></param>
        /// <param name="nif"></param>
        /// <returns></returns>
        public static T ObtenerDatosPorNIF<T>(string nombreTabla, string nif) where T : new()
        {
            // Hace la consulta a la base de datos de la tabla pasada seleccionado por el NIF
            string sqlConsulta = $"SELECT * FROM {nombreTabla} WHERE NIF = @NIF";
            var parametros = new[] { new SQLiteParameter("@NIF", nif) };

            // Se almacena el resultado en la tabla que luego se mapea al objeto pasado 'T'
            DataTable tabla = EjecutarConsulta(sqlConsulta, parametros);

            if(tabla.Rows.Count == 0)
            {
                return default(T); // Devuelve null si T es una clase
            }

            // Mapea la fila obtenida en la tabla anterior al tipo de objeto pasado 'T'
            return MapeadorDatos.MapearFila<T>(tabla.Rows[0]);
        }

        public static long ObtenerUltimoId()
        {
            using(var conexion = AbrirConexion())
            {
                conexion.Open();
                using(var comando = new SQLiteCommand("SELECT last_insert_rowid()", conexion))
                {
                    return (long)comando.ExecuteScalar();
                }
            }
        }
    }
}
