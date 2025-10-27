using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Utilidades;

namespace Facturar.Presentacion.Procesos
{
    public interface IProcesoBoton
    {
        // El parametro estado solo se utiliza (de momento en el boton SeleccionActivos)
        void Ejecutar(Enumeradores.TipoEntidad entidadActiva, bool? estado = true);
    }
}
