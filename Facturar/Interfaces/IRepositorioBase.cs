using System.Collections.Generic;

namespace Facturar.Interfaces
{
    public interface IRepositorioBase<T>
    {
        bool Agregar(T entidad);
        bool Actualizar(T entidad);
        bool Eliminar(string nif);
        T ObtenerPorId(int id);
        T ObtenerPorNIF(string nif);
        IEnumerable<T> ListarTodos();
    }
}
