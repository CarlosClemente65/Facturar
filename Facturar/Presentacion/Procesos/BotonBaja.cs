using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using Facturar.Presentacion.Controles;
using Facturar.Servicios;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

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
            // Acceso a los gestores del formulario frmBase
            var gestorEmpresas = formulario.GestorEmpresas;
            var gestorLocales = formulario.GestorLocales;
            var gestorClientes = formulario.GestorClientes;
            var gestorContratos = formulario.GestorContratos;

            // Se debe grabar la entidad seleccionada en el UserControl correspondiente para poder acceder a las propiedades que tenga el objeto y hacer la modificacion en la base de datos

            // Aplica el efecto de bloqueo de edicion
            Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: true);

            // Mensaje de confirmacion de la baja
            DialogResult resultado = MessageBox.Show("Esta seguro de dar de baja el registro", "Baja registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(resultado == DialogResult.Yes)
            {
                switch(entidadActiva)
                {
                    case Enumerador.TipoEntidad.Empresa:
                        // Aplica el efecto de bloqueo de edicion
                        Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: true);

                        // Actualiza la empresa en la base de datos
                        gestorEmpresas.Baja(nif: ucEmpresas.EmpresaActual.NIF);
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
            else
            {
                MessageBox.Show("Proceso de baja cancelado", "Baja registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Aplica el efecto de bloqueo de edicion
            Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: false);

            // Refresca el grid de empresas
            ucEmpresas.CargarEmpresas(); // Refresca el grid
        }
    }
}
