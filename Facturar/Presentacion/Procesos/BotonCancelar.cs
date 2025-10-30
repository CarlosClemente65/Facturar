using Facturar.Presentacion.Controles;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Presentacion.Procesos
{
    public class BotonCancelar : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonCancelar(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos, UC_Facturas facturas) : base(empresas, locales, clientes, contratos, facturas)
        {

        }

        // Procesos a ejecutar segun el tipo de entidad (el parametro estado no se usa aqui).
        public override void Ejecutar(Enumerador.TipoEntidad entidadActiva, bool? estado = true)
        {
            // Vuelve a activar el grid de cada entidad
            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    // Habilita el grid de empresas
                    ucEmpresas.GridBase.Enabled = true;

                    // Quita el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: false);

                    // Refresca el grid de empresas
                    ucEmpresas.CargarEmpresas();

                    break;

                case Enumerador.TipoEntidad.Local:
                    // Habilita el grid de locales
                    ucLocales.GridBase.Enabled = true;

                    // Quita el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucLocales.GridBase, bloquear: false);

                    // Refresca el grid de locales
                    ucLocales.CargarLocales();

                    break;

                case Enumerador.TipoEntidad.Cliente:
                    /* Pendiente de desarrollo
                     
                    // Habilita el grid de clientes
                    ucClientes.GridBase.Enabled = true;

                    // Quita el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucClientes.GridBase, bloquear: false);

                    // Refresca el grid de clientes
                    ucClientes.CargarClientes();

                    */
                    break;

                case Enumerador.TipoEntidad.Contrato:
                    /* Pendiente desarrollo
                      
                    // Habilita el grid de contratos
                    //ucContratos.GridBase.Enabled = true; 

                    // Quita el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucContratos.GridBase, bloquear: false);

                    // Refresca el grid de contratos
                    ucContratos.CargarContratos();

                    */
                    break;

                case Enumerador.TipoEntidad.Factura:
                    // Habilita el grid de facturas
                    ucFacturas.GridBase.Enabled = true;

                    // Quita el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucFacturas.GridBase, bloquear: false);

                    // Refresca el grid de empresas
                    ucFacturas.CargarFacturas();
                    break;
            }
        }
    }
}
