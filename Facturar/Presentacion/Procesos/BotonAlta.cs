using Facturar.Presentacion.Controles;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Procesos
{
    // Procesos a ejecutar con el boton Alta
    public class BotonAlta : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonAlta(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos, UC_Facturas facturas) : base (empresas, locales, clientes, contratos, facturas)
        {

        }

        // Procesos a ejecutar segun el tipo de entidad (el parametro estado no se usa aqui).
        public override void Ejecutar(Enumerador.TipoEntidad entidadActiva, bool? estado = true)
        {
            // Se debe grabar la entidad seleccionada en el UserControl correspondiente para poder acceder a las propiedades que tenga el objeto y hacer la modificacion en la base de datos
            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    // Deshabilita los TextBox que no se pueden editar
                    ucEmpresas.BloqueoTextBoxAlta();

                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: true);

                    break;

                case Enumerador.TipoEntidad.Local:
                    // Deshabilita los TextBox que no se pueden editar
                    ucLocales.BloqueoTextBoxAlta();

                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucLocales.GridBase, bloquear: true);

                    break;

                case Enumerador.TipoEntidad.Cliente:
                    //gestorClientes.Agregar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Contrato:
                    //gestorContratos.Agregar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Factura:
                    // Deshabilita los TextBox que no se pueden editar
                    ucFacturas.BloqueoTextBoxAlta();

                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucFacturas.GridBase, bloquear: true);
                    break;
            }
        }

    }
}
