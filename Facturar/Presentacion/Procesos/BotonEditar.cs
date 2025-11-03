using Facturar.Presentacion.Controles;
using Facturar.Utilidades;
using Utiles = Facturar.Utilidades.UtilidadesUI;
using Enumerador = Facturar.Utilidades.Enumeradores;

namespace Facturar.Presentacion.Procesos
{
    public class BotonEditar : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonEditar(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos, UC_Facturas facturas) : base(empresas, locales, clientes, contratos, facturas)
        {

        }

        // Procesos a ejecutar segun el tipo de entidad (el parametro estado no se usa aqui).
        public override void Ejecutar(Enumeradores.TipoEntidad entidadActiva, bool? estado = true)
        {
            // Se debe grabar la entidad seleccionada en el UserControl correspondiente para poder acceder a las propiedades que tenga el objeto y hacer la modificacion en la base de datos
            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    // Establece el tipo de proceso en modo edicion
                    ucEmpresas.tipoProceso = Enumerador.TipoProceso.Edicion;

                    // Deshabilita los TextBox que no se pueden editar
                    ucEmpresas.BloqueoTextBoxEdicion();

                    // Actualiza la empresa seleccionada en UC_Empresa
                    ucEmpresas.ActualizaEmpresaSeleccionada();

                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: true);

                    break;

                case Enumerador.TipoEntidad.Local:
                    // Establece el tipo de proceso en modo edicion
                    ucLocales.tipoProceso = Enumerador.TipoProceso.Edicion;

                    // Deshabilita los TextBox que no se pueden editar
                    ucLocales.BloqueoTextBoxEdicion();

                    // Actualiza el local seleccionado en UC_Local
                    ucLocales.ActualizarLocalSeleccionado();

                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucLocales.GridBase, bloquear: true);

                    break;

                case Enumerador.TipoEntidad.Cliente:
                    // Establece el tipo de proceso en modo edicion
                    ucClientes.tipoProceso = Enumerador.TipoProceso.Edicion;

                    // Deshabilita los TextBox que no se pueden editar
                    ucClientes.BloqueoTextBoxEdicion();

                    // Actualiza el cliente seleccionada en UC_Clientes
                    ucClientes.ActualizarClienteSeleccionado();

                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucClientes.GridBase, bloquear: true);

                    break;

                case Enumerador.TipoEntidad.Contrato:
                    // Establece el tipo de proceso en modo edicion
                    ucContratos.tipoProceso = Enumerador.TipoProceso.Edicion;

                    // Deshabilita los TextBox que no se pueden editar
                    ucContratos.BloqueoTextBoxEdicion();

                    // Actualiza el contrato seleccionado en UC_Contratos
                    ucContratos.ActualizarContratoSeleccionado();

                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucContratos.GridBase, bloquear: true);

                    break;

                case Enumerador.TipoEntidad.Factura:
                    // Establece el tipo de proceso en modo edicion
                    ucFacturas.tipoProceso = Enumerador.TipoProceso.Edicion;

                    // Deshabilita los TextBox que no se pueden editar
                    ucFacturas.BloqueoTextBoxEditar();

                    // Actualiza el local seleccionado en UC_Local
                    ucFacturas.ActualizaFacturaSeleccionada();

                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucFacturas.GridBase, bloquear: true);
                    break;
            }
        }
    }
}
