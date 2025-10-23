using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;
using Facturar.Servicios;
using Utiles = Facturar.Utilidades.UtilesGenerales;


namespace Facturar.Utilidades
{
    public static class Pruebas
    {
        public static void LanzaPruebas(Entidades entidad, Procesos proceso)
        {
            switch(entidad)
            {
                case Entidades.Empresas:
                    ProbarEmpresas(proceso);
                    break;

                case Entidades.Locales:
                    ProbarLocales(proceso);
                    break;

                case Entidades.Clientes:
                    ProbarClientes(proceso);
                    break;

                case Entidades.Contratos:
                    ProbarContratos(proceso);
                    break;

                case Entidades.RevisionContratos:
                    ProbarRevisiones(proceso);
                    break;

                case Entidades.BaseDatos:
                    break;

            }
        }


        private static void ProbarEmpresas(Procesos proceso)
        {
            var empresa = new Empresa();
            var gestor = new GestorEmpresas();
            var gestorLocales = new GestorLocales();
            bool resultado;
            try
            {
                switch(proceso)
                {
                    case Procesos.Alta:
                        empresa.Nombre = "EMPRESA PRUEBAS UNO";
                        empresa.NIF = "05100001G";
                        empresa.Direccion = "Calle pruebas uno, 1";
                        empresa.CodigoPostal = "02002";
                        empresa.Poblacion = "Albacete";
                        empresa.Provincia = "Albacete";
                        empresa.Telefono = "666111111";
                        empresa.Email = "correo@correo.com";
                        empresa.PersonaContacto = "Persona contacto uno";
                        empresa.SerieFactura = "A";

                        resultado = gestor.Agregar(empresa);

                        empresa.Nombre = "EMPRESA PRUEBAS DOS";
                        empresa.NIF = "05196375P";
                        empresa.Direccion = "Calle Pruebas dos, 1";
                        empresa.CodigoPostal = "02002";
                        empresa.Poblacion = "Albacete";
                        empresa.Provincia = "Albacete";
                        empresa.Telefono = "666222222";
                        empresa.Email = "correo@correo.com";
                        empresa.PersonaContacto = "Persona contacto dos";
                        empresa.SerieFactura = "B";

                        resultado = gestor.Agregar(empresa);

                        break;

                    case Procesos.Baja:
                        resultado = gestor.Baja("05100001G", Utiles.ConvertirFecha("30/10/2025"));
                        break;

                    case Procesos.Modificacion:
                        empresa.Nombre = "Empresa de pruebas modificada";
                        empresa.NIF = "05100001G";
                        empresa.Direccion = "Calle Mayor, 1";
                        empresa.CodigoPostal = "02002";
                        empresa.Poblacion = "Albacete";
                        empresa.Provincia = "Albacete";
                        empresa.Telefono = "666333222";
                        empresa.Email = "correo@correo.com";
                        empresa.PersonaContacto = "Persona contacto";
                        empresa.SerieFactura = "A";

                        resultado = gestor.Actualizar(empresa);
                        break;


                    case Procesos.Consulta:
                        List<Empresa> ConsultaEmpresa = gestor.ListarTodos().ToList();
                        List<Empresa> ConsultaActivas = gestor.ListarTodos(true).ToList();
                        break;

                    case Procesos.Eliminacion:
                        resultado = gestor.Eliminar("05100001G");
                        break;
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al procesar las empresas: {ex.Message}");
            }
        }

        private static void ProbarClientes(Procesos proceso)
        {
            var cliente = new Cliente();
            var gestor = new GestorClientes();
            bool resultado;
            try
            {
                switch(proceso)
                {
                    case Procesos.Alta:
                        cliente.Nombre = "PRIMER CLIENTE NOMBRE";
                        cliente.NIF = "05100001G";
                        cliente.Direccion = "Calle cliente primero, 1";
                        cliente.CodigoPostal = "02002";
                        cliente.Poblacion = "Albacete";
                        cliente.Provincia = "Albacete";
                        cliente.Telefono = "666111111";
                        cliente.Email = "correo@correo.com";
                        cliente.PersonaContacto = "Persona contacto primero";
                        cliente.FormaPago = Cliente.FormasPago.Domiciliacion.ToString();
                        cliente.Observaciones = "Observaciones del cliente 1";

                        resultado = gestor.Agregar(cliente);

                        cliente.Nombre = "SEGUNDO CLIENTE NOMBRE";
                        cliente.NIF = "05126963X";
                        cliente.Direccion = "Calle cliente segundo, 1";
                        cliente.CodigoPostal = "02002";
                        cliente.Poblacion = "Albacete";
                        cliente.Provincia = "Albacete";
                        cliente.Telefono = "666222222";
                        cliente.Email = "correo@correo.com";
                        cliente.FechaAlta = Utiles.ValidarFecha("15/05/2025");
                        cliente.PersonaContacto = "Persona contacto segundo";
                        cliente.FormaPago = Cliente.FormasPago.Transferencia.ToString();
                        cliente.Observaciones = "Observaciones del cliente 2";

                        resultado = gestor.Agregar(cliente);

                        break;

                    case Procesos.Baja:
                        resultado = gestor.Baja(nif: "05100001G", Utiles.ConvertirFecha("30/11/2025"));
                        break;

                    case Procesos.Modificacion:
                        cliente.Nombre = "CLIENTE DE PRUEBAS MODIFICADO";
                        cliente.NIF = "05100001G";
                        cliente.Direccion = "Calle Mayor, 1";
                        cliente.CodigoPostal = "02002";
                        cliente.Poblacion = "Albacete";
                        cliente.Provincia = "Albacete";
                        cliente.Telefono = "666333222";
                        cliente.Email = "correo@correo.com";
                        cliente.PersonaContacto = "Persona contacto";
                        cliente.FormaPago = Cliente.FormasPago.Transferencia.ToString();
                        cliente.IBAN = "ES6601822032002200231234";
                        cliente.Observaciones = "Cliente de pruebas modificado";

                        resultado = gestor.Actualizar(cliente);
                        break;


                    case Procesos.Consulta:
                        List<Cliente> ClientesInactivos = gestor.ListarTodos(activos: false).ToList();
                        List<Cliente> TodosClientes = gestor.ListarTodos().ToList();
                        List<Cliente> ClientesActivos = gestor.ListarTodos(activos: true).ToList();
                        break;

                    case Procesos.Eliminacion:
                        resultado = gestor.Eliminar(nif: "05100001G");
                        break;
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al procesar las empresas\n {ex.Message}");
            }
        }


        private static void ProbarLocales(Procesos proceso)
        {
            var local = new Local();
            var gestor = new GestorLocales();
            bool resultado;
            try
            {
                switch(proceso)
                {
                    case Procesos.Alta:
                        local.EmpresaId = 2;
                        local.Descripcion = "Local empresa uno";
                        local.Direccion = "POLIGONO CAMPOLLANO C/B, 1";
                        local.CodigoPostal = "02007";
                        local.Poblacion = "ALBACETE";
                        local.Provincia = "ALBACETE";
                        local.ImporteAlquiler = 800m;
                        local.FechaAlta = Utiles.ValidarFecha("01/05/2025");

                        resultado = gestor.Agregar(local);

                        local.EmpresaId = 1;
                        local.Descripcion = "Local empresa dos";
                        local.Direccion = "POLIGONO CAMPOLLANO C/B, 1";
                        local.CodigoPostal = "02007";
                        local.Poblacion = "ALBAETE";
                        local.Provincia = "ALBAETE";
                        local.ImporteAlquiler = 900m;
                        local.Observaciones = "Observaciones del local 2";

                        resultado = gestor.Agregar(local);

                        break;

                    case Procesos.Baja:
                        resultado = gestor.BajaLocal(id: 4);
                        break;

                    case Procesos.Modificacion:
                        var localNuevo = gestor.ObtenerPorId(id: 4);
                        localNuevo.EmpresaId = 2;
                        resultado = gestor.Actualizar(localNuevo);

                        break;


                    case Procesos.Consulta:
                        List<Local> ConsultaLocal = gestor.ListarTodos().ToList();
                        List<Local> ConsultaTodosActivos = gestor.ListarTodos(activos: true).ToList();
                        List<Local> ConsultaLocalesInactivos = gestor.ListarLocalesPorEmpresa(empresaId: 1, false).ToList();
                        break;

                    case Procesos.Eliminacion:
                        resultado = gestor.EliminarLocal(id: 1);
                        break;
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al procesar los locales: {ex.Message}");
            }
        }


        private static void ProbarContratos(Procesos proceso)
        {
            var contrato = new Contrato();
            var gestor = new GestorContratos();
            bool resultado;
            try
            {
                switch(proceso)
                {
                    case Procesos.Alta:
                        contrato.EmpresaId = 1;
                        contrato.ClienteId = 4;
                        contrato.LocalId = 3;
                        contrato.PrecioMensual = 755.22m;
                        contrato.FechaInicio = Utiles.ConvertirFecha("15/05/2025").Value;
                        contrato.Observaciones = "Observaciones contrato 1";

                        resultado = gestor.Agregar(contrato);

                        contrato.EmpresaId = 2;
                        contrato.ClienteId = 3;
                        contrato.LocalId = 4;
                        contrato.PrecioMensual = 800m;
                        contrato.FechaInicio = Utiles.ConvertirFecha("15/05/2025").Value;
                        contrato.Observaciones = "Observaciones contrato 2";

                        resultado = gestor.Agregar(contrato);

                        break;

                    case Procesos.Baja:
                        resultado = gestor.Baja(9, fechaBaja: Utiles.ConvertirFecha("14/10/2025"));
                        break;

                    case Procesos.Modificacion:
                        var contratoNuevo = gestor.ObtenerPorId(8);
                        contratoNuevo.FechaFin = Utiles.ConvertirFecha("30/10/2025");
                        contratoNuevo.Observaciones = "Observaciones modificadas por fecha baja";

                        resultado = gestor.Actualizar(contratoNuevo);
                        break;


                    case Procesos.Consulta:
                        List<Contrato> ConsultaContratos = gestor.ListarTodos().ToList();

                        // Consulta todos los contratos activos
                        List<Contrato> ConsultaContratosActivos = gestor.ListarTodos(true).ToList();

                        // Consulta contratos de un cliente
                        List<Contrato> ConsultaContratosCliente = gestor.ListarContratosPorCliente(clienteNif: "05100001G").ToList();

                        //Consulta los contratos de una empresa
                        List<Contrato> ConsultaContratosEmpresa = gestor.ListarContratosPorEmpresa(empresaNif: "05196375P").ToList();

                        // Consulta los contratos de un local
                        List<Contrato> ConsultaContratosLocal = gestor.ListarContratosPorLocal(4).ToList();

                        //Consulta contratos por fecha
                        List<Contrato> ConsultaContratosFecha = gestor.ListarContratosPorFecha(Utiles.ConvertirFecha("01/05/2025").Value, Utiles.ConvertirFecha("15/05/2025").Value,activos:true).ToList();

                        break;

                    case Procesos.Eliminacion:
                        resultado = gestor.Eliminar(1);
                        break;
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al procesar los contratos: {ex.Message}");
            }
        }


        private static void ProbarRevisiones(Procesos proceso)
        {
            var revision = new RevisionContrato();
            var gestor = new GestorContratos();
            bool resultado;
            try
            {
                switch(proceso)
                {
                    case Procesos.Alta:
                        revision.ContratoId = 8;
                        revision.FechaRevision = Utiles.ConvertirFecha("14/05/2026").Value;
                        revision.PrecioAnterior = 700;
                        revision.PrecioRevisado = 750m;
                        revision.Observaciones = "Revision contrato de pruebas";

                        resultado = gestor.AgregarRevisionContrato(revision);

                        revision.ContratoId = 9;
                        revision.FechaRevision = Utiles.ConvertirFecha("16/05/2025").Value;
                        revision.PrecioAnterior = 100;
                        revision.PrecioRevisado = 780m;
                        revision.Observaciones = "Revision contrato de pruebas";

                        resultado = gestor.AgregarRevisionContrato(revision);

                        break;

                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error al procesar las revisiones de contrato: {ex.Message}");
            }
        }

        public enum Entidades
        {
            Empresas = 0,
            Clientes = 1,
            Contratos = 2,
            Locales = 3,
            BaseDatos = 4,
            RevisionContratos = 5
        }

        public enum Procesos
        {
            Alta = 0,
            Baja = 1,
            Modificacion = 2,
            Consulta = 3,
            Eliminacion = 4
        }
    }
}
