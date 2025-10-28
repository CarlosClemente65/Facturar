using Facturar.Presentacion.Controles;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Procesos
{
    // Procesos a ejecutar con el boton Alta
    public class BotonAlta : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonAlta(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos) : base (empresas, locales, clientes, contratos)
        {

        }

        // Procesos a ejecutar segun el tipo de entidad (el parametro estado no se usa aqui).
        public override void Ejecutar(Enumerador.TipoEntidad entidadActiva, bool? estado = true)
        {
            // Se debe grabar la entidad seleccionada en el UserControl correspondiente para poder acceder a las propiedades que tenga el objeto y hacer la modificacion en la base de datos
            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    // Aplica el efecto de bloqueo de edicion
                    Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: true);

                    // Actualiza la empresa seleccionada en UC_Empresa
                    ucEmpresas.ActualizaEmpresaSeleccionada();

                    break;

                case Enumerador.TipoEntidad.Local:
                    //gestorLocales.Agregar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Cliente:
                    //gestorClientes.Agregar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Contrato:
                    //gestorContratos.Agregar(); // Pendiente de desarrollo
                    break;
            }
        }

    }
}
