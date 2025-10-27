using System.Windows.Forms;
using Facturar.Interfaces;
using Facturar.Presentacion.Controles;
using Facturar.Utilidades;

namespace Facturar.Presentacion.Procesos
{
    // Clase abstracta comun a todos los botones. Implementa en el constructor los valores de las instancias de las entidades
    public abstract class ProcesoBotonBase : IProcesoBoton
    {
        // Instancias de UserControl 
        protected UC_Empresas ucEmpresas;
        protected UC_Locales ucLocales;
        protected UC_Clientes ucClientes;
        protected UC_Contratos ucContratos;

        // Formulario opcional
        protected frmBase formulario;

        // Constructor habitual (sin formulario)
        public ProcesoBotonBase(UC_Empresas _ucEmpresas, UC_Locales _ucLocales, UC_Clientes _ucClientes, UC_Contratos _ucContratos)
        {
            ucEmpresas = _ucEmpresas;
            ucLocales = _ucLocales;
            ucClientes = _ucClientes;
            ucContratos = _ucContratos;
            formulario = null;

        }

        // Constructor alternativo (con formulario)
        public ProcesoBotonBase(UC_Empresas _ucEmpresas, UC_Locales _ucLocales, UC_Clientes _ucClientes, UC_Contratos _ucContratos, frmBase _formulario)
            : this(_ucEmpresas, _ucLocales, _ucClientes, _ucContratos)
        {
            formulario = _formulario;
        }

        // Metodo para ejecutar las acciones de cada boton
        // El parametro estado solo se utiliza (de momento) en el boton SeleccionActivos
        public abstract void Ejecutar(Enumeradores.TipoEntidad entidadActiva, bool? estado = true);
        
    }
}
