using Utiles = Facturar.Utilidades.UtilidadesUI;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Facturar.Presentacion.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturar.Presentacion.Procesos
{
    public class BotonEliminar : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonEliminar(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos) : base(empresas, locales, clientes, contratos)
        {

        }

        public override void Ejecutar(Enumerador.TipoEntidad entidadActiva, bool? estado = true)
        {
            // Se debe grabar la entidad seleccionada en el UserControl correspondiente para poder acceder a las propiedades que tenga el objeto y hacer la modificacion en la base de datos
            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    // Actualiza la empresa seleccionada en UC_Empresa
                    ucEmpresas.ActualizaEmpresaSeleccionada();

                    //gestorEmpresas.Eliminar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Local:
                    //gestorLocales.Eliminar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Cliente:
                    //gestorClientes.Eliminar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Contrato:
                    //gestorContratos.Eliminar(); // Pendiente de desarrollo
                    break;
            }
        }
    }
}
