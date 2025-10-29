using System;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Presentacion.Controles;
using Facturar.Servicios;
using static Facturar.Utilidades.Enumeradores;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Procesos
{
    // Procesos a ejecutar con el boton Baja
    public class BotonBaja : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores (necesita el formulario para las instancias de los gestores)
        public BotonBaja(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos, UC_Facturas facturas, frmBase _formulario) : base(empresas, locales, clientes, contratos, facturas, _formulario)
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

            // Aplica el efecto de bloqueo de edicion
            Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: true);

            // Mensaje de confirmacion de la baja
            DialogResult resultado = MessageBox.Show("Esta seguro de dar de baja el registro \n(quedara inactivo sin eliminarlo)", "Baja registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if(resultado == DialogResult.Yes)
            {
                switch(entidadActiva)
                {
                    case Enumerador.TipoEntidad.Empresa:
                        try
                        {
                            // Graba la fecha de baja en la empresa en la base de datos
                            gestorEmpresas.Baja(nif: ucEmpresas.EmpresaActual.NIF);

                            // Muestra mensaje de proceso correcto
                            MessageBox.Show("Grabada fecha de baja en la empresa.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            // Muestra mensaje de error
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        break;

                    case Enumerador.TipoEntidad.Local:
                        try
                        {
                            // Graba la fecha de baja en el local en la base de datos
                            gestorLocales.BajaLocal(id: ucLocales.LocalActual.Id);

                            // Muestra mensaje de proceso correcto
                            MessageBox.Show("Grabada fecha de baja en el local.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            // Muestra mensaje de error
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        break;

                    case Enumerador.TipoEntidad.Cliente:
                        try
                        {
                            // Graba la fecha de baja en el cliente en la base de datos
                            gestorClientes.Baja(nif: ucClientes.ClienteActual.NIF);

                            // Muestra mensaje de proceso correcto
                            MessageBox.Show("Grabada fecha de baja en el cliente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            // Muestra mensaje de error
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        
                        break;

                    case Enumerador.TipoEntidad.Contrato:
                        try
                        {
                            // Graba la fecha de baja en el contrato en la base de datos
                            gestorContratos.Baja(contratoId: ucContratos.ContratoActual.Id);

                            // Muestra mensaje de proceso correcto
                            MessageBox.Show("Grabada fecha de baja en el contrato.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            // Muestra mensaje de error
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        
                        break;
                }
            }
            else
            {
                MessageBox.Show("Proceso de baja cancelado", "Baja registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    // Restablece el bloqueo y habilita el grid
                    Utiles.RestablecerPaneles<GestorEmpresas, Empresa>(ucEmpresas.GridBase, false, gestorEmpresas);

                    // Refresca el grid de empresas
                    ucEmpresas.CargarEmpresas(); // Refresca el grid

                    break;

                case Enumerador.TipoEntidad.Local:
                    // Restablece el bloqueo y habilita el grid
                    Utiles.RestablecerPaneles<GestorLocales, Local>(ucLocales.GridBase, false, gestorLocales);

                    // Refresca el grid de locales
                    ucLocales.CargarLocales(); // Refresca el grid

                    break;

                case Enumerador.TipoEntidad.Cliente:
                    // Restablece el bloqueo y habilita el grid
                    Utiles.RestablecerPaneles<GestorClientes, Cliente>(ucClientes.GridBase, false, gestorClientes);

                    // Refresca el grid de clientes
                    ucClientes.CargarClientes(); // Refresca el grid

                    break;

                case Enumerador.TipoEntidad.Contrato:
                    // Restablece el bloqueo y habilita el grid
                    Utiles.RestablecerPaneles<GestorContratos, Contrato>(ucContratos.GridBase, false, gestorContratos);

                    // Refresca el grid de contratos
                    ucContratos.CargarContratos(); // Refresca el grid

                    break;

            }
        }
    }
}
