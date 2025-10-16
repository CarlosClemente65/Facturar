using System;

namespace Facturar.Utilidades
{
    public static class MetodosExtension
    {
        // Metodo de extensión de la clase 'string' para verificar si una cadena contiene otra cadena pudiendo indicar que se omitan la comparacion con mayusculas / minusculas (string.Contains no lo permite)
        // Por ejemplo: "Hola Mundo".Contains("hola", StringComparison.OrdinalIgnoreCase) devolveria 'true' porque se ignoran las mayusculas
        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        // Al poner 'this' delante del primero parametro se indica que el metodo se comporta como un metodo de instancia de 'string' y al usarlo con un parametro adicional se comporta como una sobrecarga del metodo original
        {
            if(source == null || toCheck == null)
                return false;

            return source.IndexOf(toCheck, comparison) >= 0;
        }
    }

}
