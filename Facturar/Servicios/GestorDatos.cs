using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Facturar.Entidades;
using Facturar.Utilidades;

namespace Facturar.Servicios
{
    public static class GestorDatos
    {
        // Propiedades para inicializar la base de datos
        private static readonly string rutaBD = "./datos/facturacion.db";
        private static readonly string cadenaConexion = $"Data Source={rutaBD};Version=3;";


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
        public static int EjecutarNonQuery(string sql, params SQLiteParameter[] parametros)
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
        public static int EjecutarEscalar(string sql, params SQLiteParameter[] parametros)
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
        /// Ejecuta una consulta SQL que devuelve un conjunto de resultados (SELECT).
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
        /// Metodo generico para mapear en un objeto pasado con 'T' segun los datos de la tabla que se obtiene de la BBDD
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

            // Se almacena el resultado en la tabla que luego se mapea al objeto Empresa
            DataTable tabla = EjecutarConsulta(sqlConsulta, parametros);

            if(tabla.Rows.Count == 0)
            {
                return default(T); // Devuelve null si T es una clase
            }

            // Mapea la fila obtenida en la tabla anterior al tipo de objeto pasado 'T'
            return MapeadorDatos.MapearFila<T>(tabla.Rows[0]);
        }
    }
}
