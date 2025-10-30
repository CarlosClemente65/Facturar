using System;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Presentacion.Controles;
using Facturar.Servicios;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Procesos
{
    public class BotonEliminar : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonEliminar(UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos, UC_Facturas facturas, frmBase _formulario) : base(empresas, locales, clientes, contratos, facturas, _formulario)
        {

        }

        public override void Ejecutar(Enumerador.TipoEntidad entidadActiva, bool? estado = true)
        {
            // Acceso a los gestores del formulario frmBase
            var gestorEmpresas = formulario.GestorEmpresas;
            var gestorLocales = formulario.GestorLocales;
            var gestorClientes = formulario.GestorClientes;
            var gestorContratos = formulario.GestorContratos;
            var gestorFacturas = formulario.GestorFacturas;

            // Aplica el efecto de bloqueo de edicion
            Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: true);

            // Mensaje de confirmacion de la baja
            DialogResult resultado = MessageBox.Show("Esta seguro de eliminar el registro de la base de datos \n(no se podrá recuperar)", "Eliminar registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if(resultado == DialogResult.Yes)
            {
                switch(entidadActiva)
                {
                    case Enumerador.TipoEntidad.Empresa:
                        try
                        {
                            // Elimina la empresa en la base de datos
                            gestorEmpresas.Eliminar(nif: ucEmpresas.EmpresaActual.NIF);

                            // Muestra mensaje de proceso correcto
                            MessageBox.Show("Empresa eliminada de la base de datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            // Muestra mensaje de error
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Restablece el bloqueo del grid y carga las empresas
                        Utiles.RestablecerPaneles<GestorEmpresas, Empresa>(ucEmpresas.GridBase, false, gestorEmpresas);

                        // Refresca el grid de empresas
                        ucEmpresas.CargarEmpresas(); // Refresca el grid

                        break;

                    case Enumerador.TipoEntidad.Local:
                        try
                        {
                            // Elimina el local en la base de datos
                            gestorLocales.EliminarLocal(id: ucLocales.LocalActual.Id);

                            // Muestra mensaje de proceso correcto
                            MessageBox.Show("Local eliminado de la base de datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            // Muestra mensaje de error
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Restablece el bloqueo y habilita el grid
                        Utiles.RestablecerPaneles<GestorLocales, Local>(ucLocales.GridBase, false, gestorLocales);

                        // Refresca el grid de locales
                        ucLocales.CargarLocales(); // Refresca el grid
                        break;

                    case Enumerador.TipoEntidad.Cliente:
                        try
                        {
                            // Elimina el cliente en la base de datos
                            gestorClientes.Eliminar(nif: ucClientes.ClienteActual.NIF);

                            // Muestra mensaje de proceso correcto
                            MessageBox.Show("Eliminado el cliente en la base de datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            // Muestra mensaje de error
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Restablece el bloqueo y habilita el grid
                        Utiles.RestablecerPaneles<GestorClientes, Cliente>(ucClientes.GridBase, false, gestorClientes);

                        // Refresca el grid de clientes
                        ucClientes.CargarClientes(); // Refresca el grid

                        break;

                    case Enumerador.TipoEntidad.Contrato:
                        try
                        {
                            // Elimina el contrato en la base de datos
                            gestorContratos.Eliminar(contratoId: ucContratos.ContratoActual.Id);

                            // Muestra mensaje de proceso correcto
                            MessageBox.Show("Eliminado el contrato de la base de datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            // Muestra mensaje de error
                            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Restablece el bloqueo y habilita el grid
                        Utiles.RestablecerPaneles<GestorContratos, Contrato>(ucContratos.GridBase, false, gestorContratos);

                        // Refresca el grid de contratos
                        ucContratos.CargarContratos(); // Refresca el grid

                        break;
                }
            }
            else
            {
                MessageBox.Show("Proceso de borrado cancelado", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
