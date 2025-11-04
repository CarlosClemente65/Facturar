using System;
using System.Linq;
using System.Windows.Forms;
using Facturar.Entidades;
using Facturar.Presentacion.Controles;
using Facturar.Servicios;
using static Facturar.Utilidades.Enumeradores;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion.Procesos
{
    public class BotonValidar : ProcesoBotonBase
    {
        // Acceso a los gestores del formulario frmBase
        GestorEmpresas gestorEmpresas;
        GestorLocales gestorLocales;
        GestorClientes gestorClientes;
        GestorContratos gestorContratos;
        GestorFacturas gestorFacturas;
        TipoProceso tipoProceso;

        // Mensaje para mostrar en el aviso de correcto o incorrecto.
        string mensajeOk = string.Empty;
        string mensajeKo = string.Empty;

        // Constructor que recibe las instancias de las entidades y las pasa a la clase base para almacenar los valores
        public BotonValidar
        (UC_Empresas empresas, UC_Locales locales, UC_Clientes clientes, UC_Contratos contratos, UC_Facturas facturas, frmBase formulario)
            : base(empresas, locales, clientes, contratos, facturas, formulario)
        {
            // Acceso a los gestores del formulario frmBase
            gestorEmpresas = formulario.GestorEmpresas;
            gestorLocales = formulario.GestorLocales;
            gestorClientes = formulario.GestorClientes;
            gestorContratos = formulario.GestorContratos;
            gestorFacturas = formulario.GestorFacturas;

            // Carga el tipo de proceso que se ha iniciado (alta, baja, edicion, eliminar)
            tipoProceso = formulario.TipoProceso;
        }

        // Procesos a ejecutar segun el tipo de entidad (el parametro estado no se usa aqui).
        public override void Ejecutar(TipoEntidad entidadActiva, bool? estado = true)
        {
            ProcesarEntidad(entidadActiva, tipoProceso);

            // Mostrar mensajes segun corresponda
            if(!string.IsNullOrEmpty(mensajeOk) && !formulario.errorProceso)
            {
                // Muestra mensaje de proceso correcto
                MessageBox.Show(mensajeOk, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(!string.IsNullOrEmpty(mensajeKo))
            {
                // Muestra mensaje de error
                MessageBox.Show(mensajeKo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Procesado de las entidades segun la accion
        private void ProcesarEntidad(TipoEntidad entidadActiva, TipoProceso tipoProceso)
        {
            // Copia de los objetos por si hay error en la edicion
            Empresa copiaEmpresa = null; // Copia de la empresa por si hay error en la edicion
            Empresa empresa = null; // Empresa que se va a crear

            Local copiaLocal = null; // Copia del local por si hay error en la edicion
            Local local = null; // Local que se va a crear

            Cliente copiaCliente = null; // Copia del cliente por si hay error en la edicion
            Cliente cliente = null; // Cliente que se va a crear

            Contrato copiaContrato = null; //Copia del contrato por si hay error en la edicion
            Contrato contrato = null; // Contrato que se va a crear

            Factura copiaFactura = null; // Copia de la factura por si hay error en la edicion
            Factura factura = null; // Factura que se va a crear

            try
            {
                // Procesos segun tipo de entidad
                switch(entidadActiva)
                {
                    case TipoEntidad.Empresa:
                        // Ejecucion del proceso al validar la empresa
                        EjecutarProcesoEmpresa(gestorEmpresas, tipoProceso, ref copiaEmpresa, ref empresa);

                        // Marca que no ha habido error en el proceso
                        formulario.errorProceso = false;

                        // Habilita el grid de empresas
                        ucEmpresas.GridBase.Enabled = true;

                        // Quita el efecto de bloqueo de edicion
                        Utiles.BloqueoEdicionDgv(_grid: ucEmpresas.GridBase, bloquear: false);

                        // Refresca el grid de empresas
                        ucEmpresas.CargarEmpresas(); // Refresca el grid

                        break;

                    case Enumerador.TipoEntidad.Local:
                        // Ejecucion del proceso al validar el local
                        EjecutarProcesoLocal(gestorLocales, tipoProceso, ref copiaLocal, ref local);

                        // Marca que no ha habido error en el proceso
                        formulario.errorProceso = false;

                        // Habilita el grid de locales
                        ucLocales.GridBase.Enabled = true;

                        // Quita el efecto de bloqueo de edicion
                        Utiles.BloqueoEdicionDgv(_grid: ucLocales.GridBase, bloquear: false);

                        // Refresca el grid de locales
                        ucLocales.CargarLocales(); // Refresca el grid

                        break;

                    case Enumerador.TipoEntidad.Cliente:
                        // Ejecucion del proceso al validar el cliente
                        EjecutarProcesoCliente(gestorClientes, tipoProceso, ref copiaCliente, ref cliente);

                        // Marca que no ha habido error en el proceso
                        formulario.errorProceso = false;

                        // Habilita el grid de clientes
                        ucClientes.GridBase.Enabled = true;

                        // Quita el efecto de bloqueo de edicion
                        Utiles.BloqueoEdicionDgv(_grid: ucClientes.GridBase, bloquear: false);

                        // Refresca el grid de clientes
                        ucClientes.CargarClientes(); // Refresca el grid
                        break;

                    case Enumerador.TipoEntidad.Contrato:
                        // Ejecucion del proceso al validar el contrato
                        EjecutarProcesoContrato(gestorContratos, gestorLocales, tipoProceso, ref copiaContrato, ref contrato, ref local);

                        // Marca que no ha habido error en el proceso
                        formulario.errorProceso = false;

                        // Habilita el grid de contratos
                        ucContratos.GridBase.Enabled = true;

                        // Quita el efecto de bloqueo de edicion
                        Utiles.BloqueoEdicionDgv(_grid: ucContratos.GridBase, bloquear: false);

                        // Refresca el grid de contratos
                        ucContratos.CargarContratos(); // Refresca el grid

                        break;

                    case Enumerador.TipoEntidad.Factura:
                        // Ejecucion del proceso al validar la factura
                        EjecutarProcesoFactura(gestorFacturas, tipoProceso, ref copiaFactura, ref factura);

                        // Marca que no ha habido error en el proceso
                        formulario.errorProceso = false;

                        // Habilita el grid de facturas
                        ucFacturas.GridBase.Enabled = true;

                        // Quita el efecto de bloqueo de edicion
                        Utiles.BloqueoEdicionDgv(_grid: ucFacturas.GridBase, bloquear: false);

                        // Refresca el grid de facturas
                        ucFacturas.CargarFacturas(); // Refresca el grid

                        break;
                }
            }
            catch(Exception ex)
            {
                // Muestra mensaje de error
                mensajeKo = $"{ex.Message}";
                formulario.errorProceso = true;

                // Restaura la copia del objeto si hay algun error en el proceso de edicion
                if(tipoProceso == TipoProceso.Edicion)
                {
                    switch(entidadActiva)
                    {
                        case TipoEntidad.Empresa:
                            // Solo en la edicion se restaura la empresa original 
                            ucEmpresas.EmpresaActual = copiaEmpresa;

                            break;

                        case TipoEntidad.Local:
                            // Solo en la edicion se restaura el local original 
                            ucLocales.LocalActual = copiaLocal;

                            break;

                        case TipoEntidad.Cliente:
                            // Solo en la edicion se restaura el cliente original 
                            ucClientes.ClienteActual = copiaCliente;

                            break;

                        case TipoEntidad.Contrato:
                            // Solo en la edicion se restaura el contrato original 
                            ucContratos.ContratoActual = copiaContrato;

                            break;

                        case TipoEntidad.Factura:
                            // Solo en la edicion se restaura la empresa original
                            ucFacturas.FacturaActual = copiaFactura;

                            break;
                    }
                }
            }
        }

        // Metodo para ejecutar los procesos de editar o alta de una empresa
        private void EjecutarProcesoEmpresa(GestorEmpresas gestorEmpresas, TipoProceso tipoProceso, ref Empresa copiaEmpresa, ref Empresa empresa)
        {
            switch(tipoProceso)
            {
                case TipoProceso.Edicion:
                    // Se establece el tipo de proceso en modo edicion
                    ucEmpresas.tipoProceso = TipoProceso.Edicion;

                    // Se obtiene la empresa seleccioanda
                    empresa = ucEmpresas.EmpresaActual;

                    // Hacemos una copia de la empresa actual por si la edicion falla
                    copiaEmpresa = new Empresa(empresa);

                    // Se actualizan las propiedades segun los campos de la pantalla
                    ucEmpresas.ActualizaPropiedadesEmpresa(empresa);

                    // Graba los cambios en la base de datos
                    gestorEmpresas.Actualizar(empresa);

                    // Mensaje de proceso correcto
                    mensajeOk = "Empresa actualizada correctamente.";

                    break;

                case TipoProceso.Alta:
                    // Crea una nueva empresa
                    empresa = new Empresa();

                    // Se establece el tipo de proceso en modo alta
                    ucEmpresas.tipoProceso = TipoProceso.Alta;

                    // Se graban las propiedades segun los campos de la pantalla
                    ucEmpresas.ActualizaPropiedadesEmpresa(empresa);

                    // Agrega la nueva empresa a la base de datos
                    gestorEmpresas.Agregar(empresa);

                    // Mensaje de proceso correcto
                    mensajeOk = "Empresa creada correctamente.";

                    break;
            }
        }

        // Metodo para ejecutar los procesos de editar o alta de un local
        private void EjecutarProcesoLocal(GestorLocales gestorLocales, TipoProceso tipoProceso, ref Local copiaLocal, ref Local local)
        {
            switch(tipoProceso)
            {
                case TipoProceso.Edicion:
                    // Se establece el tipo de proceso en modo edicion
                    ucLocales.tipoProceso = TipoProceso.Edicion;

                    // Se obtiene el local seleccioando
                    local = ucLocales.LocalActual;

                    // Hacemos una copia del local actual por si la edicion falla
                    copiaLocal = new Local(local);

                    // Se actualizan las propiedades segun los campos de la pantalla
                    ucLocales.ActualizaPropiedadesLocal(local);

                    // Graba los cambios en la base de datos
                    gestorLocales.Actualizar(local);

                    // Mensaje de proceso correcto
                    mensajeOk = "Local actualizado correctamente.";

                    break;

                case TipoProceso.Alta:
                    // Crea un nuevo local
                    local = new Local();

                    // Se establece el tipo de proceso en modo alta
                    ucLocales.tipoProceso = TipoProceso.Alta;

                    // Se graban las propiedades segun los campos de la pantalla
                    ucLocales.ActualizaPropiedadesLocal(local);

                    // Agrega el nuev local a la base de datos
                    gestorLocales.Agregar(local);

                    // Mensaje de proceso correcto
                    mensajeOk = "Local creado correctamente.";

                    break;
            }
        }

        // Metodo para ejecutar los procesos de editar o alta de un cliente
        private void EjecutarProcesoCliente(GestorClientes gestorClientes, TipoProceso tipoProceso, ref Cliente copiaCliente, ref Cliente cliente)
        {
            switch(tipoProceso)
            {
                case TipoProceso.Edicion:
                    // Se establece el tipo de proceso en modo edicion
                    ucClientes.tipoProceso = TipoProceso.Edicion;

                    // Se obtiene el cliente seleccioando
                    cliente = ucClientes.ClienteActual;

                    // Hacemos una copia del cliente actual por si la edicion falla
                    copiaCliente = new Cliente(cliente);

                    // Se actualizan las propiedades segun los campos de la pantalla
                    ucClientes.ActualizaPropiedadesCliente(cliente);

                    // Graba los cambios en la base de datos
                    gestorClientes.Actualizar(cliente);

                    // Mensaje de proceso correcto
                    mensajeOk = "Cliente actualizado correctamente.";

                    break;

                case TipoProceso.Alta:
                    // Crea un nuevo cliente
                    cliente = new Cliente();

                    // Se establece el tipo de proceso en modo alta
                    ucClientes.tipoProceso = TipoProceso.Alta;

                    // Se graban las propiedades segun los campos de la pantalla
                    ucClientes.ActualizaPropiedadesCliente(cliente);

                    // Agrega el nuevo cliente a la base de datos
                    gestorClientes.Agregar(cliente);

                    // Mensaje de proceso correcto
                    mensajeOk = "Cliente creado correctamente.";
                    break;
            }
        }

        // Metodo para ejecutar los procesos de editar o alta de un contrato
        private void EjecutarProcesoContrato(GestorContratos gestorContratos,
                                             GestorLocales gestorLocales,
                                             TipoProceso tipoProceso,
                                             ref Contrato copiaContrato,
                                             ref Contrato contrato,
                                             ref Local local)
        {
            switch(tipoProceso)
            {
                case TipoProceso.Edicion:
                    // Se establece el tipo de proceso en modo edicion
                    ucContratos.tipoProceso = TipoProceso.Edicion;

                    // Se obtiene el contrato seleccionado
                    contrato = ucContratos.ContratoActual;

                    // Hacemos una copia del contrato actual por si la edicion falla
                    copiaContrato = new Contrato(contrato);

                    // Se actualizan las propiedades segun los campos de la pantalla
                    ucContratos.ActualizaPropiedadesContrato(contrato);

                    // Graba los cambios en la base de datos
                    gestorContratos.Actualizar(contrato);

                    gestorLocales.Actualizar(contrato.Local);

                    // Mensaje de proceso correcto
                    mensajeOk = "Contrato actualizado correctamente.";
                    break;

                case TipoProceso.Alta:
                    // Crea un nuevo contrato
                    contrato = new Contrato();

                    // Se establece el tipo de proceso en modo alta
                    ucContratos.tipoProceso = TipoProceso.Alta;

                    // Se graban las propiedades segun los campos de la pantalla
                    ucContratos.ActualizaPropiedadesContrato(contrato);

                    // Agrega el nuevo contrato a la base de datos
                    gestorContratos.Agregar(contrato);

                    // Carga el contrato recien añadido segun el Id del local para obtener el Id y asignarlo al local.IdContrato
                    var contratoAgregado = gestorContratos.ListarContratosPorLocal(contrato.Local.Id).FirstOrDefault();

                    int? IdNuevoContrato = null;
                    if(contratoAgregado != null)
                    {
                        // Obtiene el Id del contrato recien generado
                        IdNuevoContrato = contratoAgregado.Id;

                        // Asigna el IdContrato al objeto local del contrato original
                        contrato.Local.IdContrato = IdNuevoContrato;

                        // Se actualiza el contrato en la base de datos
                        gestorLocales.Actualizar(contrato.Local);
                    }

                    // Mensaje de proceso correcto
                    mensajeOk = "Contrato creado correctamente.";
                    break;
            }
        }

        private void EjecutarProcesoFactura(GestorFacturas gestorFacturas, TipoProceso tipoProceso, ref Factura copiaFactura, ref Factura factura)
        {
            switch(tipoProceso)
            {
                case TipoProceso.Edicion:
                    // Se establece el tipo de proceso en modo edicion
                    ucFacturas.tipoProceso = TipoProceso.Edicion;

                    // Se obtiene la factura seleccionada
                    factura = ucFacturas.FacturaActual;

                    // Hacemos una copia de la factura actual por si la edicion falla
                    copiaFactura = new Factura(factura);

                    // Se actualizan las propiedades segun los campos de la pantalla
                    ucFacturas.ActualizaPropiedadesFactura(factura);

                    // Graba los cambios en la base de datos
                    gestorFacturas.Actualizar(factura: factura);

                    // Mensaje de proceso correcto
                    mensajeOk = "Factura actualizada correctamente.";

                    break;

                case TipoProceso.Alta:
                    // Muestra mensaje de proceso inactivo
                    mensajeOk = "Opcion en desarrollo.";

                    /* Pendiente de decidir si se permite el alta de una factura o no 
                    // Crea una nueva factura
                    factura = new Factura();

                    // Se establece el tipo de proceso en modo alta
                    ucFacturas.tipoProceso = TipoProceso.Alta;
                    
                    // Se graban las propiedades segun los campos de la pantalla
                    ucFacturas.ActualizaPropiedadesFactura(factura, TipoProceso.Alta);

                    // Agrega la nueva factura a la base de datos
                    gestorFacturas.Agregar(factura);

                    // Mensaje de proceso correcto
                    mensajeCorrecto = "Factura creada correctamente.";

                    */
                    break;
            }
        }
    }
}
