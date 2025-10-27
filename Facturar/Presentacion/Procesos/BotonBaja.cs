using Utiles = Facturar.Utilidades.UtilidadesUI;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Facturar.Presentacion.Controles;
using System;

namespace Facturar.Presentacion.Procesos
{
    // Procesos a ejecutar con el boton Baja
    public class BotonBaja : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonBaja(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos) : base(empresas, locales, clientes, contratos)
        {

        }

        // Procesos a ejecutar segun el tipo de entidad (el parametro estado no se usa aqui).
        public override void Ejecutar(Enumerador.TipoEntidad entidadActiva, bool? estado = true)
        {
            // Se debe grabar la entidad seleccionada en el UserControl correspondiente para poder acceder a las propiedades que tenga el objeto y hacer la modificacion en la base de datos
            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    //Deshabilita el grid de empresas
                    ucEmpresas.GridBase.Enabled = false;

                    // Actualiza la empresa seleccionada en UC_Empresa
                    ucEmpresas.ActualizaEmpresaSeleccionada();
                    break;

                case Enumerador.TipoEntidad.Local:
                    // Actualiza el local seleccionada en UC_Local
                    ucLocales.ActualizarLocalSeleccionado();// Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Cliente:
                    // Actualiza el cliente seleccionada en UC_Clientes
                    ucClientes.ActualizarClienteSeleccionado();// Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Contrato:
                    // Actualiza el contrato seleccionado en UC_Contratos
                    ucContratos.ActualizarContratoSeleccionado();// Pendiente de desarrollo
                    break;

            }
        }
    }
}
