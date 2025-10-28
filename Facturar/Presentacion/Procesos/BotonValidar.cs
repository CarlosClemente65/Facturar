using System;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Presentacion.Controles;
using static Facturar.Utilidades.Enumeradores;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Procesos
{
    public class BotonValidar : ProcesoBotonBase
    {
        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonValidar
        (UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos, frmBase formulario)
            : base(empresas, locales, clientes, contratos, formulario)
        {

        }

        // Procesos a ejecutar segun el tipo de entidad (el parametro estado no se usa aqui).
        public override void Ejecutar(TipoEntidad entidadActiva, bool? estado = true)
        {
            // Acceso a los gestores del formulario frmBase
            var gestorEmpresas = formulario.GestorEmpresas;
            var gestorLocales = formulario.GestorLocales;
            var gestorClientes = formulario.GestorClientes;
            var gestorContratos = formulario.GestorContratos;

            // Carga el tipo de proceso que se ha iniciado (alta, baja, edicion, eliminar)
            var tipoProceso = formulario.TipoProceso;

            // Procesos segun tipo de entidad
            switch(entidadActiva)
            {
                case TipoEntidad.Empresa:
                    Empresa copiaEmpresa = null; // Copia de la empresa por si hay error en la edicion
                    Empresa empresa = null; // Empresa que se va a crear
                    try
                    {
                        // Mensaje para mostrar en el aviso de correcto o incorrecto.
                        string mensajeCorrecto = string.Empty;
                        if(tipoProceso == TipoProceso.Edicion)
                        {
                            // Se obtiene la empresa seleccioanda
                            empresa = ucEmpresas.EmpresaActual;

                            // Hacemos una copia de la empresa actual por si la edicion falla
                            copiaEmpresa = new Empresa(empresa);

                            // Se actualizan las propiedades segun los campos de la pantalla
                            ucEmpresas.ActualizaPropiedadesEmpresa(empresa, TipoProceso.Edicion);

                            // Graba los cambios en la base de datos
                            gestorEmpresas.Actualizar(empresa);

                            // Mensaje de proceso correcto
                            mensajeCorrecto = "Empresa actualizada correctamente.";
                        }
                        else if(tipoProceso == TipoProceso.Alta)
                        {
                            // Crea una nueva empresa
                            empresa = new Empresa();

                            // Se graban las propiedades segun los campos de la pantalla
                            ucEmpresas.ActualizaPropiedadesEmpresa(empresa, TipoProceso.Alta);

                            // Agrega la nueva empresa a la base de datos
                            gestorEmpresas.Agregar(empresa);

                            // Mensaje de proceso correcto
                            mensajeCorrecto = "Empresa creada correctamente.";
                        }

                        // Muestra mensaje de proceso correcto
                        MessageBox.Show(mensajeCorrecto, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Marca que no ha habido error en el proceso
                        formulario.errorProceso = false;

                        // Habilita el grid de empresas
                        ucEmpresas.GridBase.Enabled = true;

                        // Quita el efecto de bloqueo de edicion
                        Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: false);

                        // Refresca el grid de empresas
                        ucEmpresas.CargarEmpresas(); // Refresca el grid

                    }
                    catch(Exception ex)
                    {
                        // Muestra mensaje de error
                        MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        if(tipoProceso == TipoProceso.Edicion) 
                        {
                            // Solo en la edicion se restaura la empresa original 
                            ucEmpresas.EmpresaActual = copiaEmpresa;
                        }

                        formulario.errorProceso = true;
                    }
                    break;

                case Enumerador.TipoEntidad.Local:
                    // Habilita el grid de locales
                    //ucLocales.dgvLocales.Enabled = true; // Pendiente desarrollo

                    //Actualiza la base de datos
                    //gestorLocales.Agregar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Cliente:
                    // Habilita el grid de clientes
                    //ucClientes.dgvClientes.Enabled = true; // Pendiente desarrollo

                    //Actualiza la base de datos
                    //gestorClientes.Agregar(); // Pendiente de desarrollo
                    break;

                case Enumerador.TipoEntidad.Contrato:
                    // Habilita el grid de contratos
                    //ucContratos.dgvContratos.Enabled = true; // Pendiente desarrollo

                    //Actualiza la base de datos
                    //gestorContratos.Agregar(); // Pendiente de desarrollo
                    break;
            }
        }
    }
}
