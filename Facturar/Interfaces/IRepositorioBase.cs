using System;
using System.Collections.Generic;

namespace Facturar.Interfaces
{
    public interface IRepositorioBase<T>
    {
        bool Agregar(T entidad);
        bool Actualizar(T entidad, bool esBaja = false);
        bool Eliminar(string nif);
        bool Baja (string nif, DateTime? fechaBaja);
        T ObtenerPorId(int id);
        T ObtenerPorNIF(string nif);
        IEnumerable<T> ListarTodos(bool? activo = null);
    }
}
