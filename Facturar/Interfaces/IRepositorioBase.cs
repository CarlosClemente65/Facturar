using System.Collections.Generic;

namespace Facturar.Interfaces
{
    public interface IRepositorioBase<T>
    {
        bool Agregar(T entidad);
        bool Actualizar(T entidad);
        bool Eliminar(int id);
        T Obtener(int id);
        IEnumerable<T> ListarTodos();
    }
}
