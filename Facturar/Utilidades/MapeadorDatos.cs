using System;
using System.Data;

namespace Facturar.Utilidades
{
    public static class MapeadorDatos
    {
        /// <summary>
        /// Metodo para mapear una tabla con los valores de las entidades en el objeto que corresponda, siempre que el nombre del campo coincida con el nombre de la propiedad.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fila"></param>
        /// <returns>El objeto pasado como tipo T</returns>
        public static T MapearFila<T>(DataRow fila) where T : new()
        {
            // El tipo generico 'T' debe ser un constructor publico sin parametros para poder crear una instancia, y recibe una fila con los datos devueltos por la base de datos
            var objeto = new T();
            var tipo = typeof(T);

            foreach(DataColumn columna in fila.Table.Columns)
            {
                var propiedad = tipo.GetProperty(columna.ColumnName); // Asigna a 'propiedad' la propiedad de la clase con el mismo nombre de la columna
                if(propiedad != null && propiedad.CanWrite)
                {
                    // Obtiene el valor de la columna
                    object valor = fila[columna.ColumnName];
                    if(valor == DBNull.Value) // Para bases de datos, los valores nulos se representan como DBNull.Value
                    {
                        valor = null; // Se convierte a null para poder asignarlo a la propiedad
                    }

                    // Conversion automatica si el tipo es compatible
                    if(valor != null && propiedad.PropertyType != valor.GetType()) // Comprueba si el tipo de la propiedad es distinto del valor de la BBDD
                    {
                        valor = Convert.ChangeType(valor, Nullable.GetUnderlyingType(propiedad.PropertyType) ?? propiedad.PropertyType);
                    }

                    propiedad.SetValue(objeto, valor);
                }
            }

            return objeto;
        }
    }
}
