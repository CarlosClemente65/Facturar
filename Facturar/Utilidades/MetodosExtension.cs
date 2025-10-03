using System;

namespace Facturar.Utilidades
{
    public static class MetodosExtension
    {
        // Metodo de extensión para verificar si una cadena contiene otra cadena con una comparación específica (similar a Contains pero con StringComparison)
        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        {
            if(source == null || toCheck == null)
                return false;

            return source.IndexOf(toCheck, comparison) >= 0;
        }
    }

}
