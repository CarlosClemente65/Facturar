using System;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Facturar.Presentacion.Controles;

namespace Facturar.Presentacion.Procesos
{
    public class SeleccionActivos : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public SeleccionActivos(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos, UC_Facturas facturas) : base(empresas, locales, clientes, contratos, facturas)
        {

        }

        // Procesos a ejecutar segun el tipo de entidad (el parametro estado no se usa aqui).
        public override void Ejecutar(Enumerador.TipoEntidad entidadActiva, bool? estado)
        {
            // Carga los datos correspondiente en funcion del tipo de entidad 
            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    ucEmpresas?.CargarEmpresas(activas: estado);
                    break;

                case Enumerador.TipoEntidad.Local:
                    ucLocales?.CargarLocales(activos: estado);
                    break;

                case Enumerador.TipoEntidad.Cliente:
                    ucClientes?.CargarClientes(activos: estado);
                    break;

                case Enumerador.TipoEntidad.Contrato:
                    ucContratos?.CargarContratos(activos: estado);
                    break;
            }
        }
    }
}
